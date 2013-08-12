function PhotoPreview(options) {
    var self = this;
    self.name = ko.observable(options.name);
    self.size = ko.observable(options.size);
    self.isSelected = ko.observable(false);
    self.isSaved = ko.observable(false);
}