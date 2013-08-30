$(function () {
    var inputEmail = $('#Email');
    var emailTip = new Opentip(inputEmail, '', { showOn: null, extends: 'alert', target: true });
    inputEmail.data('opentip', emailTip);

    var inputPassword = $('#Password');    
    var passwordTip = new Opentip(inputPassword, '', { showOn: null, extends: 'alert', target: true });
    inputPassword.data('opentip', passwordTip);
    
    $('#loginPanel').validate({
        rules: {
            Email: {
                required: true,
                email: true
            },
            Password: {
                required: true,
                minlength: 6
            }
        },
        messages: {
            Email: {
                required: "Please enter your e-mail address.",
            },
            Password: {
                required: "Please enter your password.",
                minlength: "Password should be 6-20 characters."
            }
        },
        errorPlacement: function (error, element) {
            var tip = element.data('opentip');

            if (tip != null)
            {
                tip.setContent(error.text());
                tip.show();
            }
        },
        success: function (label, element) {
            var tip = $(element).data('opentip');

            if (tip != null)
            {
                tip.hide();
            }
        }
    });
});