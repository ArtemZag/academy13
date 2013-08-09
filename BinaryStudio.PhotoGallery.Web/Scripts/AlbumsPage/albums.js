function onMouseOverEventHandler() {
   
}
$(document).ready(function () {
    var container = $("#container");
    var info = $("#userInformation");
    var windowObject = $(window);
    var documentObject = $(document);
    var numberOfAlbums = 10;
    var startIndex = 0;
    var endIndex = numberOfAlbums - 1;
    
    windowObject.scroll(scrolling);
    downloadNextPartionOfAlbums();
    numberOfAlbums = 5;
    downloadUserInfo();
   
    function downloadUserInfo() {
        $.post("/Albums/GetUserInfo", getInfo);
    }

    function getInfo(inf) {
        info.html(
            $("#userTmpl").render(inf));
    }

    function downloadNextPartionOfAlbums() {
        $.post("/Albums/GetAlbums", { start: startIndex, end: endIndex }, getAlbums);
        startIndex += numberOfAlbums;
        endIndex += numberOfAlbums;
    }
    function getAlbums(albums) {
        var length = albums.length;
        if (length > 0) {
            container.html(
                $("#collageTmpl").render(albums));
        } else {
            windowObject.unbind("scroll");
        }
    }
    function scrolling() {
        if (windowObject.scrollTop() == (documentObject.height() - windowObject.height())) {
            //Пользователь долистал до низа страницы
            downloadNextPartionOfAlbums();
        }
    }
    
});