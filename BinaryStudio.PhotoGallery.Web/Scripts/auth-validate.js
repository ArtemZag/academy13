$(function () {
    $('#signin-button').parent().validate({
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
                minlength: 6,
                equalTo: "#Password"
            }
        },
        messages: {
            Email: {
                required: "You must enter Email address",
            },
            Password: {
                required: "You must enter Password",
                minlength: "Password should be between 6 and 20 characters"
            }
        }
    });
});