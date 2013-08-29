$(document).ready(function () {
    $("#forgotPass").click(function () {
        $(".error-field .alert").remove();
        Bingally.animation($("#loginPanel"), "move",
            {
                direction: 'top',
                method: 'hide',
                animTime: 500
            });
        $("#passRecoveryPanel").removeAttr("style");
        Bingally.animation($("#passRecoveryPanel"), "move",
            {
                direction: 'top',
                method: 'show',
                animTime: 600
            });
    });

    $("#backToLogin").click(function () {
        $(".error-field .alert").remove();
        Bingally.animation($("#passRecoveryPanel"), "move",
            {
                direction: 'top',
                method: 'hide',
                animTime: 500
            });
        $("#loginPanel").removeAttr("style");
        Bingally.animation($("#loginPanel"), "move",
            {
                direction: 'top',
                method: 'show',
                animTime: 600
            });
    });
})
