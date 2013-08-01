$(function () {
    Dropzone.autoDiscover = false;

    var dropzoneOptions = {
        maxFilesize: 10,
        uploadMultiple: true,
        addRemoveLinks: true,
        clickable: true,
        enqueueForUpload: true,
        acceptedFiles: 'image/*',

    };
    
    var dropzone = new Dropzone('.dropzone', dropzoneOptions);
    
/*    dropzone.on("success", function(file, response) {
//        console.log(file);
//        console.log(response);
//        file.serverId = response.id;
    });

    dropzone.on("addedfile", function (file) {
//        console.log("new file added");
//        if (!file.serverId) {
//            return;
//        }
    });

    dropzone.on("removedfile", function(file) {
//        console.log("File removed");
//        if (!file.serverId) {
//            return;
//        }
//        $.get("../file/delete/" + file.serverId);
    });*/
});