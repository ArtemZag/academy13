var errors = (function() {
    var self = {};
    self.clearErrorMessages = function() {
        $('.error-field').html('');
    };
    self.showErrorMessage = function(message) {
        var errorField = $('.error-field');

        errorField.append('<div class="alert alert-error">'
            + '<button type="button" class="close" data-dismiss="alert">×</button>' + message + '</div>');
        
        $(".alert > .close").click(function () {
            self.clearErrorMessages();
        });
    };

    return self;
}());