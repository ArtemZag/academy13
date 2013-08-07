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
        info.append('<p class="textStyle">Number of albums: ' + inf.albumCount + '</p><hr>');
        info.append('<p class="textStyle">First name: ' + inf.firstName + '</p><hr>');
        info.append('<p class="textStyle">Last name: ' + inf.lastName + '</p><hr>');
        info.append('<p class="textStyle">Nick name: ' + inf.nickName + '</p><hr>');
        info.append('<p class="textStyle">User state: ' + inf.isAdmin + '</p><hr>');
        info.append('<p class="textStyle">Department: ' + inf.department + '</p>');
        $(".avatar").attr('src', inf.userAvatar);
    }

    function downloadNextPartionOfAlbums() {
        $.post("/Albums/GetAlbums", { start: startIndex, end: endIndex }, getAlbums);
        startIndex += numberOfAlbums;
        endIndex += numberOfAlbums;
    }
    function getAlbums(albums) {
        var length = albums.length;
        if (length > 0) {
            for (var index = 0; index < length; index++) {
                container.append('<div class="album">' +
                                '<img class="collage" src="' + albums[index].collageSource + '"' +
                            '</div>'); 
            }
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