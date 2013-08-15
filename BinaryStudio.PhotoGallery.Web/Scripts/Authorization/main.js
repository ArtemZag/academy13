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
    var hash = window.location.hash;
    hash = hash.substr(1);

	addClickEventTo($("#signin-button"), baseURL + '/Api/Authorization/Signin');

	$(document).on('click', '#signup-button', function (e) {
		clearErrorMessages();
		submitButton = $("#signup-button");

		if (!$('#SingUpForm').valid()) {
			showErrorMessage("Correctly fill in all the fields");
			return false;
		}
		
		submitButton.addClass('disabled');
		submitButton.attr('data-loading', true);
		
		var SingupViewModel = {
			Email: $('#Email').val(),
			Password: $('#Password').val(),
			Invite: hash,
			ConfirmPassword: $('#ConfirmPassword').val()
		};
		
		$.post(baseURL + '/Api/Authorization/ChangePassword', SingupViewModel)
		.done(function () {
			Bingally.animation(loginPanel, "move", { direction: 'top', method: 'hide', animTime: 500 });
			Bingally.animation(changePanel, "move",
				{
					direction: 'top',
					method: 'hide',
					animTime: 500
				},
				function () {
					shadow.fadeOut(500, function () {
						window.location = baseURL;
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
                			errorMsg = "Uknown server error";

                			break;
                		default:
                			errorMsg = "Server is not available";
                			break;
                	}

                	showErrorMessage(errorMsg);
                })
                .always(function () {
                	submitButton.removeClass('disabled');
                	submitButton.removeAttr('data-loading', true);
                });

		e.preventDefault();
	});

    loginPanel.find('input[type=email], input[type=password]')
        .on('focus', function() {
            clearErrorMessages();
        })
        .on('keypress', function() {
            clearErrorMessages();
        });

    function addClickEventTo(submitButton, address) {
        submitButton.click(function (event) {
            clearErrorMessages();
            
            if (!$('#SingInForm').valid()) {
                showErrorMessage("Correctly fill in all the fields");
                return false;
            }

            submitButton.addClass('disabled');
            submitButton.attr('data-loading', true);

        	$.post(address, $('#SingInForm').serialize())
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
                                window.location = baseURL + '/Home/Index';
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
                            errorMsg = "Uknown server error";
                            
                            break;
                        default:
                            errorMsg = "Server is not available";
                            break;
                    }

                    showErrorMessage(errorMsg);
                })
                .always(function() {
                    submitButton.removeClass('disabled');
                    submitButton.removeAttr('data-loading', true);
                });
            
            event.preventDefault();
        });
    }
    
    function clearErrorMessages() {
        $('.error-field').html('');
    }

    function showErrorMessage(message) {
        var errorField = $('.error-field');
        
        errorField.append('<div class="alert alert-error">'
                + '<button type="button" class="close" data-dismiss="alert">×</button>' + message + '</div>');
    }

	$.ajax({
		type: "GET",
    	url: baseURL + '/Api/Authorization/GetEmail',
    	data: { hash: hash }
	}).done(function (email) {
	    $('#Email').val(email);
    });
});