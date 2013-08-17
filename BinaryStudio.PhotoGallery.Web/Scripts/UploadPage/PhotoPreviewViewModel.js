function PhotoPreviewViewModel(options) {
    var self = this;

    var mediator = options.mediator;

    self.element = options.element;
    
    self.uploadHash = ko.observable(options.uploadHash);

    self.isInTempFolder = ko.observable(false);
    
    self.isSelected = ko.observable(options.isSelected);

    self.errorMessage = ko.observable("");
    
    self.isSelected.subscribe(function (isChecked) {
        mediator.publish("upload:preview", { hash: self.uploadHash(), isSelected: isChecked });
    });

    self.isSaved = ko.observable(false);

    self.selectPhoto = function () {
        if (!self.isSaved()) {
            self.isSelected(!self.isSelected());
        }
    };
}