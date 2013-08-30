$(function () {
    var inputPassword = $('#Password');
    var passwordTip = new Opentip(inputPassword, '', { showOn: null, extends: 'alert', target: true });
    inputPassword.data('opentip', passwordTip);

    var $confirmPassword = $("#ConfirmPassword");
    var confirmPasswordTip = new Opentip($confirmPassword, '', { showOn: null, extends: 'alert', target: true });
    $confirmPassword.data('opentip', confirmPasswordTip);

    $('#changePass').validate({
        rules: {
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
            Password: {
                required: "Please enter your password.",
                minlength: "Password should be 6-20 characters."
            },
            ConfirmPassword: {
                required: "Please repeat your password.",
                equalTo: "Passwords are not equal."
            }
        },
        errorPlacement: function (error, element) {
            var tip = element.data('opentip');

            if (tip != null) {
                tip.setContent(error.text());
                tip.show();
            }
        },
        success: function (label, element) {
            var tip = $(element).data('opentip');

            if (tip != null) {
                tip.hide();
            }
        }
    });
});