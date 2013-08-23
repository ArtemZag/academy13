$(document).ready(function () {
    verticalResizer_Module(jQuery);
//    verticalResizer_Module()
    $("#wrapper").css("height", $(window).height() - 15);

    $(window).on('resize', function () {
        $("#wrapper").css("height", $(window).height() - 15);
    });


    var photoPortion = 20;
    
    function initResizer() {
        
        $('#myGallery').rtg({
            imageWidth: 315,
            spacing: 10,
            categories: false,
            lightbox: false,
            center: false,
        });
        var didit = false;
        var done = false;
        setTimeout(function () {
            var bWidth = $('.rtg-images').width();
            var resizeStep = function () {
                bWidth = $('.rtg-images').width();
            };
            var stepTimer;
            $(window).resize(function () {
                if (didit === false) {
                    clearTimeout(stepTimer);
                    stepTimer = setTimeout(resizeStep, 200);
                }
            });
        }, 1000);
    }

    $.get("api/publicphoto/"+ photoPortion )
          .done(function (photos) {
              viewModel.addPhotos(photos);
              setTimeout(initResizer(), 10000)
          })
          .fail(function () {

          });
    
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
