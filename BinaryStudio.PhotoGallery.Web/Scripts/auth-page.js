$(function () {
    var validField = $(".validation-summary-errors");

    console.log(validField);

    if (validField.length == 0) {
        pageAnimation();
    } else {
        correctValidationStyle(validField);
    }
});

function correctValidationStyle(validField) {
    validField.addClass("alert alert-error");
    validField.prepend("<strong>Errors</strong><br/>");
    validField.prepend("<button type='button' class='close' data-dismiss='alert'>&times;</button>");
}

function pageAnimation() {
    // Search DOM elements
    var changePanel = $("#change-panel");
    var shadow = $("#full-screen-shadow");
    var loginPanel = $("#login-panel");

    // Save setted values
    var loginPanelCss = { top: loginPanel.css("top"), opacity: loginPanel.css("opacity") };
    var changePanelCss = { top: changePanel.css("top"), opacity: changePanel.css("opacity") };

    // Init new values
    loginPanel.css({ top: "-20%", opacity: 0.0 });
    changePanel.css({ top: -60, opacity: 0.0 });

    shadow.hide();

    // Animate all elements
    loginPanel.animate(loginPanelCss, 800, function () {
        changePanel.animate(changePanelCss, 600);
    });

    shadow.fadeIn(500);
}