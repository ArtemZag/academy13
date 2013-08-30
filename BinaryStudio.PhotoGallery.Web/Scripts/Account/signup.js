$(function() {
    var loginPanel = $('.login-panel');
    var changePanel = $('.change-panel');
    var shadow = $('#full-screen-shadow');

    changePanel.hide();

    shadow.fadeIn();
    
    Bingally.animation(loginPanel, "move",
        {
            direction: 'top',
            method: 'show',
            animTime: 600
        },
        function() {
            changePanel.show();
            Bingally.animation(changePanel, "move", { direction: 'top', method: 'show', animTime: 510 });
        });

    var baseURL = "http://" + window.location.host;

    addClickEventTo($("#signup-button"), baseURL + '/api/registration');

    loginPanel.find('input[type=email], input[type=password]')
        .on('focus', function() {
            errors.clearErrorMessages();
        })
        .on('keypress', function() {
            errors.clearErrorMessages();
        });

    function addClickEventTo(submitButton, address) {
        submitButton.click(function (event) {
            errors.clearErrorMessages();
            
            if (!$('form').valid()) {
                errors.showErrorMessage("Correctly fill in all the fields");
                return false;
            }

            submitButton.addClass('disabled');
            submitButton.attr('data-loading', true);
            
            $.post(address, submitButton.parent().serialize())
                .done(function () {
                    Bingally.animation(loginPanel, "move", { direction: 'top', method: 'hide', animTime: 500 });
                    Bingally.animation(changePanel, "move",
                        {
                            direction: 'top',
                            method: 'hide',
                            animTime: 500
                        },
                        function() {
                            shadow.fadeOut(500, function() {
                                window.location = baseURL + '/login';
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

                    errors.showErrorMessage(errorMsg);
                })
                .always(function() {
                    submitButton.removeClass('disabled');
                    submitButton.removeAttr('data-loading', true);
                });
            
            event.preventDefault();
        });
    }
    
});