$(function () {
    var validField = $(".validation-summary-errors");

    if (validField.length == 0) {
        showLoginPanel();
    } else {
        correctValidationStyle(validField);
    }

    animateQuery();
});

function animateQuery() {
    var submitButton = $("#signin-button");
    
    submitButton.click(function () {
        submitButton.addClass("disabled");
        submitButton.attr("data-loading", true);
        
        // ajax query
        $.post('../Account/Signin',
            {
                Email: $("#Email").val(),
                Password: $("#Password").val(),
                RememberMe: $("#RememberMe").val()
            })
            .done(function(data) {
                if (data != null) {
                    submitButton.removeClass("disabled");
                    submitButton.removeAttr("data-loading");
                    
                    hideLoginPanel(function () {
                        setTimeout(function () { window.location = "../Home/Index"; }, 500);
                    });
                }
            })
            .fail(function() {
                submitButton.removeClass("disabled");
                submitButton.removeAttr("data-loading");
            });

    });
}

function correctValidationStyle(validField) {
    validField.addClass("alert alert-error");
    validField.prepend("<strong>Errors</strong><br/>");
    validField.prepend("<button type='button' class='close' data-dismiss='alert'>&times;</button>");
}

function showLoginPanel() {
    // Search DOM elements
    var changePanel = $("#change-panel");
    var shadow = $("#full-screen-shadow");
    var loginPanel = $("#login-panel");

    // Save setted values
    var loginPanelCss = { top: loginPanel.css("top"), opacity: loginPanel.css("opacity") };
    var changePanelCss = { top: changePanel.css("top"), opacity: changePanel.css("opacity") };

    // Init new values
    loginPanel.css({ top: "-20%", opacity: 0 });
    changePanel.css({ top: -60, opacity: 0 });

    shadow.hide();

    // Animate all elements
    loginPanel.animate(loginPanelCss, 800, function () {
        changePanel.animate(changePanelCss, 600);
    });

    shadow.fadeIn(500);
}

function hideLoginPanel(callback) {
    // Search DOM elements
    var changePanel = $("#change-panel");
    var shadow = $("#full-screen-shadow");
    var loginPanel = $("#login-panel");
    
    // Animate all elements
    changePanel.animate({ top: -60, opacity: 0 }, 500, function () {
        shadow.fadeOut(500);
    });
    
    loginPanel.animate({ top: "-20%", opacity: 0 }, 800, function() {
        callback();
    });
}