var ChangePass_Init = function(controller, loginController) {
    $(function () {
        var shadow = $('#full-screen-shadow');
        shadow.fadeIn();
        var $chPasspanel = $('#chPassPanel');
        var $okPanel = $("#okPanel");

        Bingally.animation($chPasspanel, "move",
        {
            direction: 'top',
            method: 'show',
            animTime: 600
        });

        $chPasspanel.find('input[type=password]')
             .on('focus', function () {
                 clearErrorMessages();
             })
             .on('keypress', function () {
                 clearErrorMessages();
             });
        
        var $submitButton = $("#changePass-button");

        $submitButton.click(function (event) {
            clearErrorMessages();

            if (!$('form').valid()) {
                showErrorMessage("Correctly fill in all the fields");
                return false;
            }

            $submitButton.addClass('disabled');
            $submitButton.attr('data-loading', true);

            $.post(controller, $submitButton.parent().serialize())
                .done(function () {
                    Bingally.animation($chPasspanel, "move", { direction: 'top', method: 'hide', animTime: 500 });
                    $okPanel.removeAttr("style");
                    Bingally.animation($okPanel, "move", { direction: 'top', method: 'show', animTime: 600 });
                    logIn();
                })
                .fail(function (jqXHR) {
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

                    showErrorMessage(errorMsg);
                })
                .always(function () {
                    $submitButton.removeClass('disabled');
                    $submitButton.removeAttr('data-loading', true);
                });

            event.preventDefault();
            return true;
        });

        function logIn() {
            setTimeout(function () {
                $.post(loginController, $submitButton.parent().serialize())
                .done(function () {
                    Bingally.animation($okPanel, "move",
                        {
                            direction: 'top',
                            method: 'hide',
                            animTime: 500
                        },
                        function () {
                            shadow.fadeOut(500, function () {
                                window.location = '/';
                            });
                        });
                })
                .fail(function (jqXHR) {
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

                    showErrorMessage(errorMsg);
                });
            }, 3000);  
        }

        function clearErrorMessages() {
            $('.error-field').html('');
        }

        function showErrorMessage(message) {
            var errorField = $('.error-field');

            errorField.append('<div class="alert alert-error">'
                + '<button type="button" class="close" data-dismiss="alert">×</button>' + message + '</div>');
        }
        
    });
}