function UploadViewModel(options) {
    Dropzone.autoDiscover = false;
    
    var previewsContainer = options.element;

    var dropzoneOptions = {
        maxFiles: 100,
        maxFilesize: 30,
        uploadMultiple: true,
        addRemoveLinks: true,
        clickable: true,
        acceptedFiles: 'image/*',
        dictRemoveFile: "Remove photo"
    };

    var self = this;

    var mediator = options.mediator;

    mediator.subscribe("upload:preview", function (data) {
        var previewsCount = self.previews().length;

        for (var index = 0; index < previewsCount; index++) {
            var preview = self.previews()[index];
            
            if (preview.uploadHash() === data.hash) {
                preview.isSelected(data.isSelected);
            }
        }
    });

    var responseData = null;

    var dropzone = new Dropzone(previewsContainer, dropzoneOptions);

    dropzone.on('addedfile', function (file) {
        var md5Hash = b64_md5(file.name + file.size);      
        var $preview = $(file.previewTemplate);
        $preview.append('<input type="hidden" value="' + md5Hash + '"/>');
    });

    dropzone.on('sending', function (file) {
        var $preview = $(file.previewTemplate);
        $preview.addClass('dz-processing');
    });

    dropzone.on('success', function (file, response) {
        responseData = response;

        var $preview = $(file.previewTemplate);
        
        $preview.removeClass('dz-success');

        $preview.prepend('<input type="checkbox" class="photo-checker" data-bind="checked: isSelected, visible: !isSaved()" />');

        $preview.find('.dz-details > img').attr('data-bind', 'click: selectPhoto');

        $preview.find('.dz-error-message > span').attr('data-bind', 'text: errorMessage');

        var hiddenInput = $preview.find('input[type=hidden]');

        var hash = hiddenInput.attr('value');

        hiddenInput.attr('data-bind', 'value: uploadHash');

        var preview = new PhotoPreviewViewModel({
            uploadHash: hash,
            isSelected: true,
            mediator: mediator,
            element: $preview
        });

        self.previews.push(preview);

        ko.applyBindings(preview, file.previewTemplate);
    });

    dropzone.on("complete", function () {
        if (responseData == null) return;

        $.map(responseData, function (fileInfo) {
            var count = self.previews().length;

            if (count <= 0) return;

            var $preview = null;
            var preview;

            for (var index = 0; index < count; index++) {
                preview = self.previews()[index];
                
                if (preview.isSaved() == false &&
                    preview.isInTempFolder() == false &&
                    preview.uploadHash() == fileInfo.FileHash) {
                    $preview = preview.element;
                    break;
                }
            }

            if ($preview == null) return;

            if (fileInfo.IsAccepted) {
                $preview.addClass('dz-success');
                preview.isInTempFolder(true);
            } else {
                $preview.find('.photo-checker').remove();
                $preview.addClass('dz-error');
                $preview.find('.dz-error-message > span').html(fileInfo.Error);
                ko.cleanNode($preview[0]);
                self.previews.remove(preview);
            }
        });

        responseData = null;
    });

    dropzone.on("removedfile", function (file) {
        for (var index = 0; index < self.previews().length; ++index) {
            var preview = self.previews()[index];
            console.log(preview);
            if (preview.name === file.name) {
                self.previews.remove(preview);
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
    
    self.chekedAllPhotos = ko.computed({
        read: function() {
            var selectedCounter = 0;

            var previewsCount = self.previews().length;

            for (var index = 0; index < previewsCount; index++) {
                var preview = self.previews()[index];
                if (preview.isSelected()) {
                    selectedCounter++;
                }
            }
            
            return self.previews().length === selectedCounter;
        },
        write: function (value) {
            var previewsCount = self.previews().length;
            for (var index = 0; index < previewsCount; index++) {
                self.previews()[index].isSelected(value);
            }
        },
        owner: this
    });

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

    self.clearUploadedPhotos = function() {
        for (var index = 0; index < self.previews().length; index++) {
            var preview = self.previews()[index];
            if (preview.isSaved() == true) {
                preview.element.remove();
                self.previews.remove(preview);
                index--;
            }
        }
        
        // Remove all previews with errors
        $('.dz-error').remove();
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
        var albumName = self.selectedAlbum();

        // Get all names of the selected photos
        var selectedPhotoHashes = $.map(self.previews(), function(preview) {
            if (preview.isSelected() === true) {
                preview.element.removeClass('dz-success');
                return preview.uploadHash();
            }
        });

        $.post('Api/File/MovePhotos', { AlbumName: albumName, PhotoHashes: selectedPhotoHashes })
            .done(function (response) {
                $.map(response, function (fileInfo) {
                    var count = self.previews().length;

                    if (count <= 0) return;

                    var $preview = null;
                    var preview;

                    for (var index = 0; index < count; index++) {
                        preview = self.previews()[index];

                        if (preview.isSaved() == false &&
                            preview.isInTempFolder() == true &&
                            preview.uploadHash() == fileInfo.FileHash) {
                            $preview = preview.element;
                            break;
                        }
                    }

                    if ($preview == null) return;

                    if (fileInfo.IsAccepted) {
                        $preview.addClass('dz-success');
                        preview.isSelected(false);
                        preview.isSaved(true);
                        preview.isInTempFolder(false);
                    } else {
                        $preview.find('.photo-checker').remove();
                        $preview.addClass('dz-error');
                        $preview.find('.dz-error-message > span').html(fileInfo.Error);
                        self.previews.remove(preview);
                        ko.cleanNode($preview[0]);
                    }
                });
            })
            .fail(function () {
                alert("Something happens while photos have uploaded to server"); // TODO show this error as notification
            });
    };
}