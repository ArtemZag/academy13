$(document).ready(function () {
    var windowObject = $(window);
    var documentObject = $(document);
    var albumApiUrl = $("#albumApiUrl").data("url");
    var userApiUrl = $("#userApiUrl").data("url");
    var userId = parseInt($("#userId").data("url"), 10);
    var takeAlbumsCount = 10;
    var skipCount = 0;
    
    windowObject.resize(resizeTable);
    resizeTable();
    
    downloadUserInfo();

    function downloadUserInfo() {
        $.get(userApiUrl+'/'+userId, getInfo);
    }
    
    function moveNoAlbumsContainer() {
        var alb = $('.albums');
        var noAlbContainer = $(".noAlbumsTextContainer");
        noAlbContainer.offset({ top: (alb.height() - noAlbContainer.height()) / 2, left: (alb.width() - noAlbContainer.width()) / 2 });
    }

    function resizeTable() {
        $("#content").height(windowObject.height() - $("#toolbar").height());
    }

    function getInfo(inf) {
        $("#userInformation").html($("#userTmpl").render(inf));

        if (inf.FirstName == "None" || inf.LastName == "None") {
            $(".albums").html(
                $("#noneUserTmpl").render());
            windowObject.resize(moveNoAlbumsContainer);
            moveNoAlbumsContainer();
        } else {
            windowObject.scroll(scrolling);
            downloadNextPartionOfAlbums(userId);
        }
    }

    function downloadNextPartionOfAlbums(id) {
        $.get(albumApiUrl, { userId: id, skip: skipCount, take: takeAlbumsCount }, getAlbums);
        skipCount += takeAlbumsCount;
    }
    function getAlbums(model) {
        var length = model.Albums.length;
        if (length > 0) {
            $(".albums").html(
                $("#collageTmpl").render(model.Albums));
        } else {
            if (skipCount - takeAlbumsCount == 0) {
                windowObject.resize(moveNoAlbumsContainer);
                if (model.NoAlbumsToView) {
                    $(".albums").html(
                        $("#dontHaveRightsTmpl").render(model));
                    moveNoAlbumsContainer();
                } else {
                    $(".albums").html(
                        $("#uploadTmpl").render());
                    moveNoAlbumsContainer();
                }
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