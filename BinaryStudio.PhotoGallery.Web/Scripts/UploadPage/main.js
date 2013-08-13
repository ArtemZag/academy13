$(function () {
    var previews = [];

    var newPreview = new PhotoPreview({
        name: "test1.jpg",
        size: 1123123123
    });
    newPreview.isSelected(true);
    previews.push(newPreview);
    
    newPreview = new PhotoPreview({
        name: "test2.jpg",
        size: 1123123123
    });
    newPreview.isSaved(true);
    previews.push(newPreview);
    
    newPreview = new PhotoPreview({
        name: "test3.jpg",
        size: 1123123123
    });
    previews.push(newPreview);

    newPreview = new PhotoPreview({
        name: "test4.jpg",
        size: 1123123123
    });
    newPreview.isSelected(true);
    newPreview.isSaved(true);
    previews.push(newPreview);

    var vm = new UploadViewModel({ element: '.dropzone' });
    
    $.get('Api/Album')
        .done(function (data) {
            console.log(data);
            vm.albums([{ Id: 1, Name: "Akademie" }, { Id: 2, Name: "Summer" }]);
        });

    vm.previews(previews);
    
    ko.applyBindings(vm);
});