$(document).ready(function () {
    var windowObject = $(window);
    var documentObject = $(document);
    var albumApiUrl = $("#albumApiUrl").data("url");
    var userApiUrl = $("#userApiUrl").data("url");
    var takeAlbumsCount = 10;
    var skipCount = 0;
    var userId = 0;
    
    windowObject.scroll(scrolling);
    windowObject.resize(resizeTable);
    resizeTable();
    
    downloadUserInfo();

    function downloadUserInfo() {
        $.get(userApiUrl, getInfo);
    }
    
    function moveNoAlbumsContainer() {
        var alb = $('.albums');
        var noAlbContainer = $('.noAlbumsTextContainer');
        noAlbContainer.offset({ top: (alb.height() - noAlbContainer.height()) / 2, left: (alb.width() - noAlbContainer.width()) / 2 });
    }

    function resizeTable() {
        $("#content").height(windowObject.height() - $("#toolbar").height());
    }

    function getInfo(inf) {
        $("#userInformation").html($("#userTmpl").render(inf));
        userId = inf.Id;
        downloadNextPartionOfAlbums(userId);
    }

    function downloadNextPartionOfAlbums(id) {
        $.get(albumApiUrl, { userId: id, skip: skipCount, take: takeAlbumsCount }, getAlbums);
        skipCount += takeAlbumsCount;
    }
    function getAlbums(albums) {
        var length = albums.length;
        if (length > 0) {
            $(".albums").html(
                $("#collageTmpl").render(albums));
        } else {
            if (skipCount - takeAlbumsCount == 0) {
                $(".albums").html(
                    $("#uploadTmpl").render());
                windowObject.resize(moveNoAlbumsContainer);
                moveNoAlbumsContainer();
            }
            windowObject.unbind("scroll");
        }
    }
    function scrolling() {
        if (windowObject.scrollTop() == (documentObject.height() - windowObject.height())) {
            //Пользователь долистал до низа страницы
            downloadNextPartionOfAlbums(userId);
        }
    }
});