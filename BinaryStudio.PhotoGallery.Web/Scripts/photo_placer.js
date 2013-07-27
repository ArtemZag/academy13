$(document).ready(function () {
    $(window).resize(calcPhotoSizes);
    //the start index of photo to get
    var startIndex = 0;
    ajaxPhotoLoad();
    $("#tester").click(ajaxPhotoLoad);
    
    var scrHeight = $(window).height();

    $(window).scroll(scrolled);
    var busy = false;

    function scrolled() {
        if (!busy) {
            var underScroll = $(this).scrollTop();
            var phWrapHeight = $("div#photoWrapper").height();
            var scrollPos = scrHeight + underScroll;

            if (phWrapHeight - scrollPos < 300) {
                busy = true;
                ajaxPhotoLoad();
            }
        }
    }

    function calcPhotoSizes() {
        var width = 0;
        var firstElemInRow = 0;
        var margins = 0;
        console.log("calcphotosizes");
        var wrapperWidth = $('div#photoWrapper').width();
        var marginPhotoCont = parseInt($('.photoContainer').css('margin-left'))
                            + parseInt($('.photoContainer').css('margin-right'));
        var photos = $('div#photoWrapper > div > img');
        jQuery.each(photos, function (indPh) {
            width += this.width;
            margins += marginPhotoCont;
            if (width > wrapperWidth - margins) {
                var coef = (wrapperWidth - margins) / width;
                for (var indSub = firstElemInRow; indSub <= indPh; indSub++) {
                    $(photos[indSub]).closest(".photoContainer").css('width', (photos[indSub].width * coef) - 0.2);
                }
                firstElemInRow = indPh + 1;
                width = 0;
                margins = 0;
            }
            else if (indPh == photos.length - 1) {
                for (indSub = firstElemInRow; indSub <= indPh; indSub++) {
                    $(photos[indSub]).closest(".photoContainer").css('width', photos[indSub].width);
                }
            }
        });
    }

    function ajaxPhotoLoad() {
        $("#photopreloader").show();
        $.post("/Home/GetPhotos", { startIndex: startIndex }, getPhotos);
    }

    function getPhotos(photos) {
        if (photos.length > 0) {
            $.each(photos, function() {
                var elem = $("#photoWrapper");
                elem.append('<div class="photoContainer invisible"><img src="' + this.PhotoThumbSource + '"/></div>');
            });
            calcPhotoSizes();
            $('div#photoWrapper > div.invisible').removeClass("invisible");
            busy = false;
            startIndex += 20;
        } else {
            console.log("End");
            $(window).unbind("scroll");
        }
        $("#photopreloader").hide();
    }

});

