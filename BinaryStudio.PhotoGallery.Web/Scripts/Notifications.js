$(function () {
    // Declare a proxy to reference the hub. 
    var noty = $.connection.notificationsHub;
    // Create a function that the hub can call to broadcast messages.
    noty.client.broadcastNotification = function (title, message) {
        ShowNotify(title, message);
        // Html encode display name and message. 
        //var encodedName = $('<div />').text(name).html();
        //var encodedMsg = $('<div />').text(message).html();
        //// Add the message to the page. 
        //$('#discussion').append('<li><strong>' + encodedName
        //    + '</strong>:&nbsp;&nbsp;' + encodedMsg + '</li>');
    };

    $.connection.hub.start().done(function () {
        $('#SendButton').click(function () {
            // Call the Send method on the hub. 
            noty.server.send('Новая фотография', 'Пользователь useremail добавил фотографию photoname');
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

//title: 'Новая фотография',
//    text: 'Пользователь useremail добавил фотографию photoname',