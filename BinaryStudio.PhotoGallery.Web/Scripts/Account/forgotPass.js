$(document).ready(function () {
    $("#forgotPass").click(function () {
        Bingally.animation($("#loginPanel"), "move",
            {
                direction: 'top',
                method: 'hide',
                animTime: 500
            });
        $("#passRecoveryPanel").removeClass("invisible");
        Bingally.animation($("#passRecoveryPanel"), "move",
            {
                direction: 'top',
                method: 'show',
                animTime: 600
            });
    });

    $("#backToLogin").click(function() {
        Bingally.animation($("#passRecoveryPanel"), "move",
            {
                direction: 'top',
                method: 'hide',
                animTime: 500
            });
        Bingally.animation($("#loginPanel"), "move",
            {
                direction: 'top',
                method: 'show',
                animTime: 600
            });
    });
})
