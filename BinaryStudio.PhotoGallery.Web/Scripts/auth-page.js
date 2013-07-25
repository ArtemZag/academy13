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

//    loginPanel.find('input[type=email] ,input[type=password]').focus();

    function addClickEventTo(submitButton, address) {
        submitButton.click(function (event) {         
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
                    var errorMsg;
                    
                    switch (jqXHR.status) {
                        case 400:
                            errorMsg = "Email or password is incorrect";
                            break;
                        case 500:
                            errorMsg = "Server is not available";
                            break;
                        default:
                            errorMsg = "Uknown server error";
                            break;
                    }

                    showErrorMessage(loginPanel, errorMsg);
                })
                .always(function() {
                    submitButton.removeClass('disabled');
                    submitButton.removeAttr('data-loading', true);
                });
            
            event.preventDefault();
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