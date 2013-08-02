$(function () {
    function photoPreviewViewModel(options) {
        var self = this;
        
        self.previews = ko.observable(options.previews);

        self.canUpload = ko.computed(function () {
        	return self.previews().length > 0;
        });
    }
});