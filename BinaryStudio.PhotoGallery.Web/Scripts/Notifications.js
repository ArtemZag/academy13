$(function () {
    $.pnotify.defaults.history = false;
    var noty = $.connection.notificationsHub;
    
    noty.client.broadcastNotification = function (title, username, photoname) {
        var message = "Пользователь " + username + " добавил фотографию " + photoname;
        ShowNotify(title, message);
    };

    $.connection.hub.start().done(function () {
        $('#SendButton').click(function () {
            // Call the Send method on the hub. 
            noty.server.photoAdded("Имя_фотографии_для_теста.jpg");
        });
    });
});

function ShowNotify(gTitle, message) {
    $.pnotify({
        title: gTitle,
        text: message,
        addclass: 'custom',
        icon: 'photo_icon',
        shadow: true,
        opacity: .8,
        delay: 6000,
        closer_hover: true,
        //nonblock: true,
        //nonblock_opacity: .2
    });
}
