$(document).ready(function () {
    $(window).resize(CalcPhotoSizes);    CalcPhotoSizes(); // todo why is it bad works on webkit? Need fix.
});function CalcPhotoSizes() {
    var width = 0;    var firstElemInRow = 0;    var margins = 0;    var wrapperWidth = $('div#photoWrapper').width();    console.debug(wrapperWidth);    var marginPhotoCont = parseInt($('.photoContainer').css('margin-left'))						+ parseInt($('.photoContainer').css('margin-right'));    var photos = $('div#photoWrapper > div > img');
    jQuery.each(photos, function(i) {
        width += this.width;
        margins += marginPhotoCont;
        if (width > wrapperWidth - margins) {
            var coef = (wrapperWidth - margins) / width;
            for (var j = firstElemInRow; j <= i; j++) {
                $(photos[j]).closest(".photoContainer").css('width', (photos[j].width * coef) - 0.2);
            }
            firstElemInRow = i + 1;
            width = 0;
            margins = 0;
        }
    });
}