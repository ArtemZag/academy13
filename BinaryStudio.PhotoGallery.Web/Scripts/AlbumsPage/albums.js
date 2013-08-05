$(document).ready(function () {
    var container = $("#container");
    var info = $("#inf");
    var windowObject = $(window);
    var numberOfAlbums = 2;
    var startIndex = 0;
    var endIndex = numberOfAlbums-1;

    windowObject.scroll(scrolling);
    downloadNextPartion();

    //var id = setInterval(loadWhileScrollingNotExist, 100);
    
    function loadWhileScrollingNotExist() {
        while (scrollExist()) {
            downloadNextPartion();
        }
        alert(scrollExist());
    }
    function scrollExist() {
        return document.scrollHeight == document.offsetHeight;
    }
    function downloadNextPartion() {
        $.post("/Albums/GetAlbums", { start: startIndex, end: endIndex }, getAlbums);
        startIndex += numberOfAlbums;
        endIndex += numberOfAlbums;
    }
    function getAlbums(albums) {
        var length = albums.length;
        if (length > 0) {
            for (var index = 0; index < length; index++) {
                container.append('<div class="album">' +
                                '<img src="' + albums[index].collageSource + '"' +
                            '</div>');
            }
        } else {
            windowObject.unbind("scroll");
        }
    }
    function scrolling() {
        
    }
});