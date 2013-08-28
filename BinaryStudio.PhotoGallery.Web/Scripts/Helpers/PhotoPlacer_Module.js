var PhotoPlacer_Module = (function (controllerUrl, koPhotos, albumId) {

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

    var photoPortion = 35;
    var startIndex = 0;

    function ajaxPhotoLoad() {
        $("#loader").show();
        $.get(controllerUrl, { albumId: albumId , skip: startIndex, take: photoPortion }, getPhotos)
            .fail(function() {
                $("#loader").hide();
            });
    }
    
    function photosLoaded(goodPhotos) {
        ko.utils.arrayPushAll(koPhotos, goodPhotos);
        var $photos = $('#photoWrapper > div.invisible > img');
        calcPhotoSizes($('#photoWrapper'), $photos, marginsOfPhotoCont);
        $('#photoWrapper > div.invisible').removeClass("invisible");
        $("#loader").hide();
    }

    function getPhotos(photos) {
        if (photos.length < photoPortion) {
            allLoaded = true;
            $(window).unbind("scroll");
        }
        if (photos.length > 0) {
            var goodPhotos = new Array();
            var lenght = photos.length;
            var numLoad = 0;
            jQuery.each(photos, function (ind) {
                var img = new Image();
                img.src = this.PhotoThumbSource;
                $(img).load(function () {
                    numLoad++;
                    goodPhotos.push(photos[ind]);
                    if (numLoad == lenght) {
                        photosLoaded(goodPhotos);
                    }
                })
                    .error(function () {
                        lenght--;
                        if (numLoad == lenght) {
                            photosLoaded(goodPhotos);
                        }
                    });
            });
            
            startIndex += photoPortion;
            busy = false;
        }
        else $("#loader").hide();
    }
    
});