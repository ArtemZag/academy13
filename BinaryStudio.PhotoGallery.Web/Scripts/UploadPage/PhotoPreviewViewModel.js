function PhotoPreview(options) {
    var self = this;

    var mediator = options.mediator;

    self.element = options.element;
    
    self.hash = ko.observable(options.hash);
    
//    self.size = ko.observable(options.size);
    
    self.isSelected = ko.observable(false);

    self.errorMessage = ko.observable("");
    
    self.isSelected.subscribe(function (isChecked) {
        mediator.publish("upload:preview", { name: self.name(), isSelected: isChecked });
    });

    self.isSaved = ko.observable(false);

    self.selectPhoto = function () {
        if (!self.isSaved()) {
            self.isSelected(!self.isSelected());
        }
    };
}