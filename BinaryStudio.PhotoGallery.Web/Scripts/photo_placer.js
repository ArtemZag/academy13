
//$(document).ready(function () {
//    $("#photopreloader").hide();
//    $(window).load(function() {
//        prepareToShow();
//    });
//    $(window).resize(calcPhotoSizes);
//    $(window).scroll(scrolled);
//    //the start index of photo to get
    
//    var startIndex = 30;
//    var scrHeight = $(window).height();
    
//    var busy = false;

//    function scrolled() {
//        if (!busy) {
//            var underScroll = $(this).scrollTop();
//            var phWrapHeight = $("div#photoWrapper").height();
//            var scrollPos = scrHeight + underScroll;

//            if (phWrapHeight - scrollPos < 300) {
//                busy = true;
//                ajaxPhotoLoad();
//            }
//        }
//    }

//    // todo: Maby needs to refactor, external variable. 
//    var ajaxContainer = false;

//    function calcPhotoSizes() {
//        var width = 0;
//        var firstElemInRow = 0;
//        var margins = 0;
//        console.log("calcphotosizes");
//        var wrapperWidth = $('div#photoWrapper').width();
//        var marginPhotoCont = parseInt($('.photoContainer').css('margin-left'))
//                            + parseInt($('.photoContainer').css('margin-right'));
//        var photos;
//        if (!ajaxContainer) {
//            photos = $('div.photoContainer > img');
//        } else {
//            ajaxContainer = false;
//            photos = $('div.photoContainer.marked > img');
//            $(photos).closest('div').removeClass("marked");
//        }
            
//        jQuery.each(photos, function (indPh) {
//            width += this.width;
//            margins += marginPhotoCont;
//            if (width > wrapperWidth - margins) {
//                var coef = (wrapperWidth - margins) / width;
//                for (var indSub = firstElemInRow; indSub <= indPh; indSub++) {
//                    $(photos[indSub]).closest(".photoContainer").css('width', (photos[indSub].width * coef) - 0.2);
//                }
//                firstElemInRow = indPh + 1;
//                width = 0;
//                margins = 0;
//            }
//            else if (indPh == photos.length - 1) {
//                for (indSub = firstElemInRow; indSub <= indPh; indSub++) {
//                    $(photos[indSub]).closest(".photoContainer")
//                        .css('width', photos[indSub].width)
//                        .addClass("marked");
//                }
//            }
//        });
//    }
    
//    function prepareToShow() {
//        calcPhotoSizes();
//        $('div#photoWrapper > div.invisible').removeClass("invisible");
//    }

//    function ajaxPhotoLoad() {
//        $("#photopreloader").show();
//        $.post("/Home/Index/GetPhotosByAjax", { startIndex: startIndex }, getPhotos);
//    }

//    function getPhotos(photos) {
//        console.log("herein");
//        if (photos.length > 0) {
//            //$.each(photos, function() {
//            //    var elem = $("#photoWrapper");
//            //    elem.append('<div class="photoContainer invisible marked" onclick="location.href = \'../Home/ToPhoto/'
//            //                                                            + this.AlbumId + "/" + this.PhotoId + '\'">' +
//            //                    '<img src="' + this.PhotoThumbSource + '"/>' +
//            //                '</div>');
//            //});
//            ajaxContainer = true;
//            prepareToShow();
//            busy = false;
//            startIndex += 30;
//        } else {
//            $(window).unbind("scroll");
//        }
//        $("#photopreloader").hide();
//    }

//});

