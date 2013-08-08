var noty = $.connection.notificationsHub;
$(function () {
   
    
    noty.client.broadcastNotification = function (title, username, photoname) {
        var message = "Пользователь " + username + " добавил фотографию " + photoname;
        ShowNotify(title, message);
    };

    $.connection.hub.start().done(function () {
        $('#SendButton').click(function () {
            // Call the Send method on the hub. 
            noty.server.photoAdded();
        });
    });
});

function ShowNotify(gTitle, message) {
    $.pnotify({
        title: gTitle,
        text: message,
        addclass: 'custom',
        icon: 'photo_icon',
        opacity: .8,
        delay: 6000,
        nonblock: true,
        nonblock_opacity: .2
    });
}
