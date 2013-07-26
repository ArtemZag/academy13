$(function () {

    var emailTip = new Opentip("#Email", '', { showOn: null, extends: "alert", target: true });
    var passwordTip = new Opentip($("#Password"), '', { showOn: null, extends: "alert", target: true });
    var confirmPasswordTip = null;
    
    if ($("#ConfirmPassword").length != 0) {
        confirmPasswordTip = new Opentip("#ConfirmPassword", '', { showOn: null, extends: "alert", target: true });
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
        },
        errorPlacement: function (error, element) {
            var tip = null;

            switch (element.attr("name")) {
                case "Email":
                    tip = emailTip;
                    break;
                case "Password":
                    tip = passwordTip;
                    break;
                case "ConfirmPassword":
                    tip = confirmPasswordTip;
                    break;
            }
            
            tip.setContent(error.text());
            tip.show();
        },
        success: function (label, element) {
            switch ($(element).attr("name")) {
                case "Email":
                    emailTip.hide();
                    break;
                case "Password":
                    passwordTip.hide();
                    break;
                case "ConfirmPassword":
                    confirmPasswordTip.hide();
                    break;
            }
        }
    });
});