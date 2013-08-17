function UploadViewModel(options) {
    Dropzone.autoDiscover = false;
    
    var previewsContainer = options.element;

    var dropzoneOptions = {
        maxFiles: 100,
        maxFilesize: 10,
        uploadMultiple: true,
        addRemoveLinks: true,
        clickable: true,
        enqueueForUpload: true,
        acceptedFiles: 'image/*',
        dictRemoveFile: "Remove image"
    };

    var self = this;

    var mediator = options.mediator;

    mediator.subscribe("upload:preview", function (data) {
        var selectedCounter = 0;

        var previewsCount = self.previews().length;

        for (var index = 0; index < previewsCount; index++) {
            var preview = self.previews()[index];
            
            if (preview.name() === data.name) {
                preview.isSelected(data.isSelected);
            }

            if (preview.isSelected()) {
                selectedCounter++;
            }
        }

        var isAllPhotoSelected = self.previews().length === selectedCounter;

        self.chekedAllPhotos(isAllPhotoSelected);
    });

    var dropzone = new Dropzone(previewsContainer, dropzoneOptions);
    
    dropzone.on("success", function (file, response) {
        var $preview = $(file.previewTemplate);

        if (response.length > 0) {
            for (var index = response.length-1; index >= 0; --index) {
                if (response[index].Name === file.name) {
                    $preview.removeClass('dz-success');
                    $preview.addClass('dz-error');
                    $preview.find('.dz-error-message > span').html(response.Error);
                    return;
                }
            }
        }
        
        $preview.prepend('<input type="checkbox" class="photo-checker" data-bind="checked: isSelected, visible: !isSaved()" />');
        
        $preview.find('.dz-filename > span').attr('data-bind', 'text: name');

        $preview.find('.dz-details > img').attr('data-bind', 'click: selectPhoto');
        
        $preview.find('.dz-error-message > span').attr('data-bind', 'text: errorMessage');

        var preview = new PhotoPreview({
            name: file.name,
            size: file.size,
            isSelected: true,
            mediator: mediator,
            element: $preview
        });

        self.previews.push(preview);
        
        ko.applyBindings(preview, file.previewTemplate);
    });

    dropzone.on("removedfile", function (file) {
        for (var index = 0; index < self.previews().length; ++index) {
            var preview = self.previews()[index];
            console.log(preview);
            if (preview.name === file.name) {
                self.previews().remove(preview);
                return;
            }
        }
    });
    
    self.albums = ko.observableArray(typeof (options.albums) !== 'undefined' ? options.albums : []);
    
    var chosenAlbums = $(options.chosen);
    chosenAlbums.chosen({ no_results_text: '<a class="create-album">Create</a> album ' });

    self.reloadChosen = function () {
        chosenAlbums.trigger('chosen:updated');
    };

    self.previews = ko.observableArray(typeof(options.previews) !== 'undefined' ? options.previews : []);

    self.createNewAlbum = function(albumName) {
        self.albums.push(albumName);
        self.selectedAlbum(albumName);
        self.reloadChosen();
        
        // added '=' in request before albumName (otherwise it send null into controller)
        $.post('Api/Album', { '': albumName });
    };

    self.selectedAlbum = ko.observable('');

    self.chekedAllPhotos = ko.observable(false);
    
    self.selectAllPhotos = function () {
        var index = 0;
        var previewsCount = self.previews().length;
        if (self.chekedAllPhotos()) {
            for (index = 0; index < previewsCount; index++) {
                self.previews()[index].isSelected(true);
            }
        } else {
            for (index = 0; index < previewsCount; index++) {
                self.previews()[index].isSelected(false);
            }
        }
        return true; // allow to get check event
    };

    self.canSelectAllPhotos = ko.computed(function () {
        var previewsCount = self.previews().length;
        for (var index = 0; index < previewsCount; index++) {
            var preview = self.previews()[index];
            if (preview.isSaved() != true) {
                return true;
            }
        }
        return false;
    });

    self.canClearUploadedPhotos = ko.computed(function() {
        var previewsCount = self.previews().length;
        for (var index = 0; index < previewsCount; index++) {
            var preview = self.previews()[index];
            if (preview.isSaved() == true) {
                return true;
            }
        }
        return false;
    });

    self.clearUploadedPhotos = function() {
        for (var index = 0; index < self.previews().length; index++) {
            var preview = self.previews()[index];
            if (preview.isSaved() == true) {
                preview.element.remove();
                self.previews.remove(preview);
                index--;
            }
        }
    };

    self.canMovePhotos = ko.computed(function () {
        if (self.albums().length <= 0) {
            return false;
        }

        if (self.selectedAlbum() == null || self.selectedAlbum().length <= 0) {
            return false;
        }

        for (var index = 0; index < self.previews().length; index++) {
            var preview = self.previews()[index];

            if (preview.isSelected() == true) {
                return true;
            }
        }
        return false;
    });

    self.startMoving = function () {
        var album = self.selectedAlbum();

        // Get all names of the selected photos
        var selectedPhotos = $.map(self.previews(), function(preview) {
            if (preview.isSelected() === true) {
                return preview.name();
            }
        });

        $.post('Api/File/MovePhotos', { AlbumName: album, PhotoNames: selectedPhotos })
            .done(function(notAcceptedFiles) {
                $.map(self.previews(), function(preview) {
                    var fileNotSaved = $.map(notAcceptedFiles, function(fileName) {
                        console.log(fileName);
                        if (fileName === preview.name()) {
                            return true;
                        }
                    });

                    preview.isSaved(fileNotSaved[0] === true ? false : true);
                    preview.isSelected(false);
                });
            })
            .fail(function(data) {
                alert(data); // TODO show error as notification
            });
    };
}