$(document).ready(function () {
    $("#wrapper").css("height", $(window).height());
    verticalResizer_Module('#vFlow-images');

    $(window).on('resize', function () {
        $("#wrapper").css("height", $(window).height());
    });
    
    function initResizer() {
        $('#wrapper').vr({
            imageWidth: 215,
            spacing: 10,
        });
    }
    
    var photoPortion = 40;

    $.get("api/publicphoto/" + photoPortion)
        .done(function(photos) {
            viewModel.addPhotos(photos);
            var $photos = $("div.invisible > img");
            var length = $photos.length;
            var numLoad = 0;
            $photos.load(function() {
                numLoad++;
                if (numLoad == length) { //todo How to check by another way that all of photos have been loaded? 
                    loaded();
                }
            })
            .error(function() {
                length--;
                $(this).closest("div").remove();
                if (numLoad == length) { //todo How to check by another way that all of photos have been loaded? 
                    loaded();
                }
            });
        });
    
    function loaded() {
        initResizer();
    }
    
    ko.applyBindings(viewModel);
});


function LoginPhotoFlowViewModel() {
    var self = this;
    self.RandomPhotos = ko.observableArray();
    
    self.addPhotos = function (photos) {
        ko.utils.arrayPushAll(self.RandomPhotos, photos);
    }.bind(self);
    
}

var viewModel = new LoginPhotoFlowViewModel();
