$(function () {
    
    var mediator = new Mediator;

    var vm = new UploadViewModel({ element: '.dropzone', chosen: '.chosen-select', mediator: mediator });
    
    $('.chosen-search > input').attr('placeholder', 'Type album name to find or create');

    $.get('Api/Album')
        .done(function (response) {
            response.splice(0, 0, '');
            vm.albums(response);
            vm.reloadChosen();
        })
        .fail(function (response) {
            alert(response);
        });

    $('.chosen-results').on("click", function () {
        var selectedAlbum = $(this).find('.result-selected').text(); 
        
        if (selectedAlbum != '') {
            $(this).val(selectedAlbum);
        }
    });

    $('.chosen-results').on("click", '.no-results .create-album', function () {
        var albumName = $('.chosen-search > input').val();
        vm.createNewAlbum(albumName);
    });
    
    ko.applyBindings(vm);
});