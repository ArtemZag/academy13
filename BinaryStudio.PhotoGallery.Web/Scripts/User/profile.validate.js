$(function () {
    var $firstName = $('#FirstName');
    var $lastName = $('#LastName');
    var $Email = $('#Email');

    var nameTip = new Opentip($firstName, '', { showOn: null, extends: 'alert', target: true });
    $firstName.data('opentip', nameTip);

    var lastnameTip = new Opentip($lastName, '', { showOn: null, extends: 'alert', target: true });
    $lastName.data('opentip', lastnameTip);
    
    var emailTip = new Opentip($Email, '', { showOn: null, extends: 'alert', target: true });
    $Email.data('opentip', emailTip);

    $('#EditForm').validate({
        rules: {
            FirstName: {
                required: true,
            },
            LastName: {
                required: true,
            },
            Email: {
                required: true,
                email: true
            }
        },
        messages: {
            FirstName: {
                required: "You name, please",
            },
            LastName: {
                required: "Your lastname, please",
            },
            Email: {
                required: "Please enter your e-mail address.",
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