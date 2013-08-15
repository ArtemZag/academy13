$(function () {
    var inputEmail = $('#Email');
    var emailTip = new Opentip(inputEmail, '', { showOn: null, extends: 'alert', target: true });
    inputEmail.data('opentip', emailTip);

    var inputPassword = $('#Password');    
    var passwordTip = new Opentip(inputPassword, '', { showOn: null, extends: 'alert', target: true });
    inputPassword.data('opentip', passwordTip);
    
    if ($('#ConfirmPassword').length != 0) {
        var confirmPasswordTip = new Opentip(this, '', { showOn: null, extends: 'alert', target: true });
        $(this).data('opentip', confirmPasswordTip);
    }
    
    $('form').validate({
        rules: {
            Email: {
                required: true,
                email: true
            },
            Password: {
                required: true,
                minlength: 6
            },
            ConfirmPassword: {
                required: true,
                equalTo: "#Password"
            }
        },
        messages: {
            Email: {
                required: "Please enter your e-mail address.",
            },
            Password: {
                required: "Please enter your password.",
                minlength: "Password should be 6-20 characters."
            },
            ConfirmPassword: {
                required: "Please repeat your password.",
                equalTo: "Passwords are not equal."
            }
        }
    });
});