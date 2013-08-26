var PhotoPlacer_Module = (function (controllerUrl, albumId) {

    var marginsOfPhotoCont;
    $(document).ready(function () {
        $("#photoWrapper").append('<div class="photoContainer" style="display:none;"></div>');
        marginsOfPhotoCont = parseInt($('.photoContainer').css('margin-left'))
            + parseInt($('.photoContainer').css('margin-right'))
            + parseInt($('.photoContainer').css("border-left-width"))
            + parseInt($('.photoContainer').css("border-right-width"));
        ajaxPhotoLoad();
    });
    $(window).on('resize', function() {
        calcPhotoSizes($('#photoWrapper'), $("div.photoContainer > img"), marginsOfPhotoCont);
    });
    $(window).scroll(scrolled);

    var busy = false;
    var scrHeight = $(window).height();

    function scrolled() {
        if (!busy) {
            busy = true;
            var underScroll = $(this).scrollTop();
            var phWrapHeight = $("#photoWrapper").height();
            var scrollPos = scrHeight + underScroll;

            if (phWrapHeight - scrollPos < 300) {
                ajaxPhotoLoad();
            } else {
                busy = false;
            }

        }
    }

    var allLoaded = false;

    function calcPhotoSizes($container, $photos, marginPhotoCont) {
        var width = 0;
        var firstElemInRow = 0;
        var margins = 0;
        var wrapperWidth = $container.width();
        var $lastRow = $();

        jQuery.each($photos, function (indPh) {
            width += this.width;
            margins += marginPhotoCont;
            if (width > wrapperWidth - margins) {
                var koef = (wrapperWidth - margins) / width;
                for (var indSub = firstElemInRow; indSub <= indPh; indSub++) {
                    $($photos[indSub]).closest("div").css('width', ($photos[indSub].width * koef) - 0.2);
                    $($photos[indSub]).closest("div").addClass("resized");
                }
                firstElemInRow = indPh + 1;
                width = 0;
                margins = 0;
            } else if (indPh == $photos.length - 1) {
                for (indSub = firstElemInRow; indSub <= indPh; indSub++) {
                    if (allLoaded) {
                        $($photos[indSub]).closest("div").css('width', $photos[indSub].width);
                    } else {
                        $($photos[indSub]).closest("div").remove();
                        startIndex--;
                    } 
                }
            }

        });
        return ($lastRow);
    }

    var photoPortion = 25;
    var startIndex = 0;

    function ajaxPhotoLoad() {
        $("#loader").show();
        $.get(controllerUrl, { albumId: albumId , skip: startIndex, take: photoPortion }, getPhotos)
            .fail(function() {
                $("#loader").hide();
            });
    }

    function getPhotos(photos) {
        if (photos.length < photoPortion) {
            allLoaded = true;
            $(window).unbind("scroll");
        }
        if (photos.length > 0) {
            ko.utils.arrayPushAll(window.viewModel.Photos, photos);
            var $newPhotoContainers = $('#photoWrapper > div.invisible');
            var $photos = $newPhotoContainers.find("img:first");
            var lenght = $photos.length;
            var numLoad = 0;
            $photos.load(function () {
                numLoad++;
                if (numLoad == lenght) { //todo How to check by another way that all of photos have been loaded? 
                    calcPhotoSizes($('#photoWrapper'), $photos, marginsOfPhotoCont);
                    $newPhotoContainers.removeClass("invisible");
                }
            })
            .error(function() {
                lenght--;
                $(this).closest("div").remove();
            });
            startIndex += photoPortion;
            busy = false;
        } 
        $("#loader").hide();
    }
    
});