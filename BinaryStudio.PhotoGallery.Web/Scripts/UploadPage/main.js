$(function () {
    var mediator = new Mediator;

    var vm = new UploadViewModel({ element: '.dropzone', mediator: mediator });
    
    $.get('Api/Album')
        .done(function (response) {
            vm.albums(response);
        })
        .fail(function (response) {
            alert(response);
        });
    
    ko.applyBindings(vm);
});