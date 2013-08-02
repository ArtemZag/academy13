$(document).ready(function () {
    var main = $(".layer1");
    var sideBar = $(".layer2");
    var body = $("body");
    var windowObject = $(window);
    var numberOfAlbums = 2;
    var mainPercentage = 75;
    var sideBarPercentage = 25;

    var timerId = setInterval(scrollHideCallBack, 40);
    
    function scrollHideCallBack() {
        if (scrollExist()) {
            calcWidthOfMainScreenAndSideBar();
            clearInterval(timerId);
        }
    }

    calcWidthOfMainScreenAndSideBar();
    windowObject.resize(calcWidthOfMainScreenAndSideBar);
    windowObject.scroll(scrolling);
    
    function calcWidthOfMainScreenAndSideBar() {
        var mainOuterSpace = main.outerWidth(true) - main.width();
        var sideBarOuterSpace = sideBar.outerWidth(true) - sideBar.width();

        var bodyWidth = body.innerWidth();
        var mainWidth = Math.floor((bodyWidth * mainPercentage) / 100) - mainOuterSpace;
        var sideBarWidth = Math.ceil((bodyWidth * sideBarPercentage) / 100) - sideBarOuterSpace;

        main.width(mainWidth);
        sideBar.width(sideBarWidth);
    }
    
    function scrollExist() {
        return document.scrollHeight == document.offsetHeight;
    }
    function startDownloading(startIndex,endIndex) {
        $.post("/Albums/AjaxResponse", { start: startIndex,end:endIndex }, getPhotos);
        
    }
    function getPhotos(photos) {
        /*if (photos.length > 0) {
            $.each(photos, function () {
                var elem = $(".layer1");
                elem.append('<div class="photoContainer invisible marked" onclick="location.href = \'../Home/ToPhoto/'
                                                                        + this.AlbumId + "/" + this.PhotoId + '\'">' +
                                '<img src="' + this.PhotoThumbSource + '"/>' +
                            '</div>');
            });
            ajaxContainer = true;
            prepareToShow();
            busy = false;
            startIndex += 30;
        } else {
            $(window).unbind("scroll");
        }
        $("#photopreloader").hide();*/
    }
    function scrolling() {
        
    }
});