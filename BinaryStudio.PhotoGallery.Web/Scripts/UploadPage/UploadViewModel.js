function UploadViewModel(options) {
    Dropzone.autoDiscover = false;
    
    var previewContainer = options.element;

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

    var dropzone = new Dropzone(previewContainer, dropzoneOptions);
    
    dropzone.on("success", function (file, response) {
        var preview = $(file.previewTemplate);
        
        if (response.length > 0) {
            preview.removeClass('dz-success');
            preview.addClass('dz-error');
            
        } else {
            preview.prepend('<input type="checkbox" class="photo-checker" />');
            self.previews().push();
        }
    });

    self.albums = ko.observableArray(typeof(options.albums) !== 'undefined' ? options.albums : []);

    self.previews = ko.observableArray(typeof(options.previews) !== 'undefined' ? options.previews : []);

    self.canSave = ko.computed(function () {
        if (self.albums().length <= 0) {
            return false;
        }

        for (var index = 0; index < self.previews().length; index++) {
            if (self.previews()[index].isSelected() == true) {
                return true;
            }
        }
        return false;
    });

    self.selectedAlbumId = ko.observable();

    self.chekedAllPhotos = ko.observable(false);
    
    self.chekedAllPhotos.subscribe(function (isChecked) {
        var index = 0;
        if (isChecked) {
            for (index = 0; index < self.previews().length; index++) {
                self.previews()[index].isSelected(true);
            }
        } else {
            for (index = 0; index < self.previews().length; index++) {
                self.previews()[index].isSelected(false);
            }
        }
    });

    self.canSelectAllPhotos = ko.computed(function () {
        for (var index = 0; index < self.previews().length; index++) {
            if (self.previews()[index].isSaved() != true) {
                return true;
            }
        }
        return false;
    });

    self.startUpload = function () {
        var albumId = self.selectedAlbumId();

        if (albumId != undefined) {
            var selectedPhotos = $.map(self.previews(), function (value) {
                if (value.isSelected()) {
                    return value.name();
                }
            });

            $.post('Api/File/SavePhotos', { AlbumId: albumId, PhotoNames: selectedPhotos })
                .done(function (data) {
                    console.log(data);
                    // TODO display returned data (not accepted photos)
                    //                    console.log(data);
                    /*for (var index = 0; index < self.previews().length; index++) {
                        self.previews()[index].isSaved(true);
                    }
                    self.chekedAllPhotos(false);*/
                })
                .fail(function (data) {
                    // TODO
                });
        } else {
            alert("Please select an album to save in");
        }
    };
}