$(function() {
    $.pnotify.defaults.history = false;
    var hubNoty = $.connection.NotificationsHub;

    hubNoty.client.SendNotification = function(title, message, url) {
        var uniqId = $.generateId();
        showNotify(title, message, uniqId);
        if (url != "") {
            var $selected = $("#" + uniqId + " .ui-pnotify-title");
            $selected.click(function () {
                location.href = url;
            });
            $selected.addClass("handPointer");
        }
    };

    $.generateId = function() {
        return arguments.callee.prefix + arguments.callee.count++;
    };
    $.generateId.prefix = 'noty';
    $.generateId.count = 0;
    

    $.connection.hub.start().done(function () {});
    
    function showNotify(gTitle, message, id) {
        $.pnotify({
            id: id,
            title: gTitle,
            text: message,
            addclass: 'custom',
            icon: 'photo_icon',
            shadow: true,
            opacity: .8,
            delay: 66000,
            closer_hover: true,
            width: "400px",
            //nonblock: true,
            //nonblock_opacity: .2
        });
    }
});




