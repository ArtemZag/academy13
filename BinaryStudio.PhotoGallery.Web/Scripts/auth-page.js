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

    liveSubmitButton($("#signin-button"), '../Api/Account/Signin');
    liveSubmitButton($("#signup-button"), '../Api/Account/Signup');

    function liveSubmitButton(submitButton, address) {
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

    function move(obj, options, callback, delayForCallback) {
        var cssCurrent = {};
        var cssForHide = {};

        switch (options.direction) {
        case 'left':
            cssCurrent = { left: obj.css('left'), opacity: obj.css('opacity') };
            cssForHide = { left: '-10%', opacity: 0 };
            break;
        case 'right':
            cssCurrent = { right: obj.css('right'), opacity: obj.css('opacity') };
            cssForHide = { right: '-10%', opacity: 0 };
            break;
        case 'top':
            cssCurrent = { top: obj.css('top'), opacity: obj.css('opacity') };
            cssForHide = { top: '-10%', opacity: 0 };
            break;
        case 'bottom':
            cssCurrent = { bottom: obj.css('bottom'), opacity: obj.css('opacity') };
            cssForHide = { bottom: '-10%', opacity: 0 };
            break;
        }

        var cssFrom = null;
        var cssTo = null;

        switch (options.method) {
        case 'show':
            cssFrom = cssForHide;
            cssTo = cssCurrent;
            break;
        case 'hide':
            cssFrom = cssCurrent;
            cssTo = cssForHide;
            break;
        }

        var animTime = options.animTime | 500;

        obj.css(cssFrom);
        obj.animate(cssTo, animTime, function () {
            if (callback == null) return;

            if (delayForCallback === 'undefined') {
                callback();
            } else {
                setTimeout(function() { callback(); }, delayForCallback);
            }
        });
    }
});