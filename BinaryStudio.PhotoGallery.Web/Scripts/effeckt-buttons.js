$(function() {
var buttons = {
    init: function() {
        this.bindUIActions();
    },
    bindUIActions: function() {
        $('.effeckt-button').on('click', function() {
            buttons.showLoader(this);
        });
    },
    showLoader: function(el) {
        var button = $(el),
            resetTimeout;
        if (button.attr('data-loading')) {
            button.removeAttr('data-loading');
        } else {
            button.attr('data-loading', true);
        }
        clearTimeout(resetTimeout);
        resetTimeout = setTimeout(function() {
            button.removeAttr('data-loading');
        }, 2000);
    }
};
buttons.init();
});