$(document).ready(function () {
    $("#wrapper").css("height", $(window).height() - 15);
    verticalResizer_Module(jQuery);

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
              setTimeout(initResizer(), 15000);
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
