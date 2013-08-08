$(function () {
    function photoUploaderViewModel(options) {
        var self = this;

        self.dropzone = options.dropzone;
        
        self.previews = ko.observable(options.previews);

        self.canUpload = ko.computed(function () {
        	return self.previews().length > 0;
        });

        self.beginUpload = function() {
            self.dropzone.
        };
    }

//    ko.applyBindings(new photoUploaderViewModel);
});