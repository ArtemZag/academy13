$(function() {
    var loginPanel = $('.login-panel');
    var shadow = $('#full-screen-shadow');   

    shadow.fadeIn();
    
    Bingally.animation(loginPanel, "move",
        {
            direction: 'top',
            method: 'show',
            animTime: 600
        });

    var baseURL = "http://" + window.location.host;
    
    addClickEventTo($("#signin-button"), baseURL + '/api/login');

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
                    Bingally.animation(loginPanel, "move",
                        {
                            direction: 'top',
                            method: 'hide',
                            animTime: 500
                        },
                        function() {
                            shadow.fadeOut(500, function() {
                                window.location = baseURL + '/';
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
    
    $("#forgotPass").click(function () {
        errors.clearErrorMessages();
        $("#signin-button").parent().attr("disabled", true);
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

});