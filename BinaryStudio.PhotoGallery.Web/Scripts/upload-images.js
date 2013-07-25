$(function () {
    $('#dropzone').dropzone({
        aramName: 'photos',
        url: "../Home/Upload",
        dictDefaultMessage: "Drag your images here",
        clickable: true,
        enqueueForUpload: true,
        selectedfiles: function() {
//            $('#newAlbum').show();
        }
    });
    

});