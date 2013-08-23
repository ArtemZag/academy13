$(document).ready(function () {
//    verticalResizer_Module()
    $("#wrapper").css("height", $(window).height() - 15);

    $(window).on('resize', function () {
        $("#wrapper").css("height", $(window).height() - 15);
    });

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

    var photoPortion = 20;
    
    $.get("api/publicphoto/"+ photoPortion )
          .done(function (photos) {
              ko.utils.arrayPushAll(viewModel.RandomPhotos, photos);
              ko.applyBindings(viewModel);
          })
          .fail(function () {

          });
    
});


function LoginPhotoFlowViewModel() {
    this.RandomPhotos = ko.observableArray();
}

var viewModel = new LoginPhotoFlowViewModel();
