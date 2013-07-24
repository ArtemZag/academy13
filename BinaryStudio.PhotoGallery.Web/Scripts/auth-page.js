﻿$(function() {
    var validField = $(".validation-summary-errors");

    if (validField.length == 0) {
        showLoginPanel();
    } else {
        correctValidationStyle(validField);
    }

    animateQuery();

    function clearErrorMessages() {
        
    }

    function showErrorMessage(message) {
        
    }

    function animateQuery() {
        var submitButton = $("#signin-button");

        submitButton.click(function () {
            clearErrorMessages();

            submitButton.addClass("disabled");
            submitButton.attr("data-loading", true);

            console.log($("#form-signin").serialize());

            // ajax query
            $.post('../Api/Account/Signin',
                $("#form-signin").serialize())
                .done(function() {
                    hideLoginPanel(function() {
                        setTimeout(function() { window.location = "../Home/Index"; }, 500);
                    });
                })
                .fail(function() {
                    
                })
                .always(function() {
                    submitButton.removeClass("disabled");
                    submitButton.removeAttr("data-loading", true);
                });

        });
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
        loginPanel.animate(loginPanelCss, 800, function() {
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
        changePanel.animate({ top: -60, opacity: 0 }, 500, function() {
            shadow.fadeOut(500);
        });

        loginPanel.animate({ top: "-20%", opacity: 0 }, 900, function() {
            callback();
        });
    }
});