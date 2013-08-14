function PhotoPreview(options) {
    var self = this;

    var mediator = options.mediator;
    
    self.name = ko.observable(options.name);
    
    self.size = ko.observable(options.size);
    
    self.isSelected = ko.observable(false);
    
    self.isSelected.subscribe(function (isChecked) {
        mediator.publish("upload:preview", { name: self.name(), isSelected: isChecked });
    });

    self.isSaved = ko.observable(false);
}