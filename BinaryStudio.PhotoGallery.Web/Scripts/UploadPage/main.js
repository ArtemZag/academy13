$(function () {
    var mediator = new Mediator;

    var vm = new UploadViewModel({ element: '.dropzone', mediator: mediator });
    
    $.get('Api/Album')
        .done(function (data) {
//            console.log(data);
            //            vm.albums([{ Id: 1, Name: "Akademie" }, { Id: 2, Name: "Summer" }]);
            vm.albums(data);
        });
    
    ko.applyBindings(vm);
});