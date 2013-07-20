$(function () {
    
    // Search DOM elements
    var changePanel = $("#change-panel");
    var shadow = $("#full-screen-shadow");
    var loginPanel = $("#login-panel");

    // Init start values
    shadow.hide();
    loginPanel.css({ top: "-20%", opacity: 0.0 });
    changePanel.css({ top: -60, opacity: 0.0 });

    // Animate all elements
    loginPanel.animate({ top: "25%", opacity: 1.0 }, 800, function() {
        changePanel.animate({ top: 0, opacity: 1.0 }, 600);
    });

    shadow.fadeIn(500);
});