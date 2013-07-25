$(function() {
    var loginPanel = $('.login-panel');
    var changePanel = $('.change-panel');
    var shadow = $('#full-screen-shadow');

    changePanel.hide();

    shadow.fadeIn();
    
    move(loginPanel,
        {
            direction: 'top',
            method: 'show',
            animTime: 600
        },
        function() {
            changePanel.show();
            move(changePanel, { direction: 'top', method: 'show', animTime: 510 });
        });

    addClickEventTo($("#signin-button"), '../Api/Account/Signin');
    addClickEventTo($("#signup-button"), '../Api/Account/Signup');

    function addClickEventTo(submitButton, address) {
        submitButton.click(function() {
            clearErrorMessages(loginPanel);

            submitButton.addClass('disabled');
            submitButton.attr('data-loading', true);
            
            $.post(address, submitButton.parent().serialize())
                .done(function () {
                    move(loginPanel, { direction: 'top', method: 'hide', animTime: 500 });
                    move(changePanel,
                        {
                            direction: 'top',
                            method: 'hide',
                            animTime: 500
                        },
                        function() {
                            shadow.fadeOut(500, function() {
                                window.location = '../Home/Index';
                            });
                        });
                })
                .fail(function (jqXHR) {
                    var errorMsg = "Uknown server error";
                    
                    if (jqXHR.status == 400) {
                        errorMsg = "Email or password is incorrect";
                    }

                    showErrorMessage(loginPanel, errorMsg);
                })
                .always(function() {
                    submitButton.removeClass('disabled');
                    submitButton.removeAttr('data-loading', true);
                });
        });
    }
    
    function clearErrorMessages(obj) {
        obj.find('.error-field').html('');
    }

    function showErrorMessage(obj, message) {
        var errorField = obj.find('.error-field');
        
        errorField.append('<div class="alert alert-error">'
                + '<button type="button" class="close" data-dismiss="alert">×</button>' + message + '</div>');
    }
});