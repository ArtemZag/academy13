$(function () {
    var inputEmail = $('#RemindEmail');
    var emailTip = new Opentip(inputEmail, '', { showOn: null, extends: 'alert', target: true });
    inputEmail.data('opentip', emailTip);

    $('#recoveryForm').validate({
        rules: {
            Email: {
                required: true,
                email: true
            }
        },
        messages: {
            Email: {
                required: "Enter your e-mail address.",
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