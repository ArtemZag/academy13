var ChangePass_Init = function(controller) {
    $(function() {
        $('#full-screen-shadow').fadeIn();
        var panel = $('#chPassPanel');

        Bingally.animation(panel, "move",
        {
            direction: 'top',
            method: 'show',
            animTime: 600
        });

        panel.find('input[type=password]')
             .on('focus', function () {
                 clearErrorMessages();
             })
             .on('keypress', function () {
                 clearErrorMessages();
             });
        


        function clearErrorMessages() {
            $('.error-field').html('');
        }

    });
}