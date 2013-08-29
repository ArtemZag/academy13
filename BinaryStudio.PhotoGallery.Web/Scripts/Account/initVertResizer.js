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
        .done(function (photos) {
            var goodPhotos = new Array();
            var length = photos.length;
            var numLoad = 0;
            jQuery.each(photos, function(ind) {
                var img = new Image();
                img.src = this.PhotoThumbSource;
                $(img).load(function() {
                    numLoad++;
                    goodPhotos.push(photos[ind]);
                    if (numLoad == length) { //todo How to check by another way that all of photos have been loaded? 
                        loaded(goodPhotos);
                    }
                })
                    .error(function() {
                        length--;
                        if (numLoad == length) { //todo How to check by another way that all of photos have been loaded? 
                            loaded(goodPhotos);
                        }
                    });
            });
        });
    
    function loaded(goodPhotos) {
        viewModel.addPhotos(goodPhotos);
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
