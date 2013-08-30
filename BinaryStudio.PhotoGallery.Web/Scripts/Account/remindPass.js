var RemindPass_Init = function(controller) {
    $(document).ready(function() {
        var $loginPanel = $("#loginPanel");
        var $recoverypassPanel = $("#passRecoveryPanel");
        var $okPanel = $("#okPanel");

        $(".backToLogin").click(function() {
            errors.clearErrorMessages();
            Bingally.animation($recoverypassPanel, "move", {direction: 'top',method: 'hide',animTime: 500});
            Bingally.animation($okPanel, "move", { direction: 'top', method: 'hide', animTime: 500 });
            $loginPanel.removeAttr("style");
            Bingally.animation($loginPanel, "move", {direction: 'top', method: 'show',animTime: 600 });
        });

        var $submitButton = $("#sendRecovery");
        $submitButton.click(function(event) {
            errors.clearErrorMessages();

            if (!$('form').valid()) {
                errors.showErrorMessage("Correctly fill in all the fields");
                return false;
            }

            $submitButton.addClass('disabled');
            $submitButton.attr('data-loading', true);

            $.post(controller, $submitButton.parent().serialize())
                .done(function() {
                    Bingally.animation($recoverypassPanel, "move", { direction: 'top', method: 'hide', animTime: 500 });
                    $okPanel.removeAttr("style");
                    Bingally.animation($okPanel, "move", { direction: 'top', method: 'show', animTime: 600 });
                })
                .fail(function(jqXHR) {
                    var errorMsg;

                    switch (jqXHR.status) {
                    case 500:
                    case 400:
                        errorMsg = jqXHR.responseJSON.Message;
                        break;
                    default:
                        errorMsg = "Server is not available";
                        break;
                    }

                    errors.showErrorMessage(errorMsg);
                })
                .always(function() {
                    $submitButton.removeClass('disabled');
                    $submitButton.removeAttr('data-loading', true);
                });

            event.preventDefault();
            return true;
        });

    });
}

