var Bingally = Bingally === undefined ? {} : Bingally;

Bingally.animation = function (__object, __method, __options, __callback, __delayForCallback) {
    // Select method
    switch (__method) {
    case "move":
        move(__object, __options, __callback, __delayForCallback);
        break;
    }

    function move(obj, options, callback, delayForCallback) {
        var cssCurrent = {};
        var cssForHide = {};

        switch (options.direction) {
        case 'left':
            cssCurrent = { left: obj.css('left'), opacity: obj.css('opacity') };
            cssForHide = { left: '-10%', opacity: 0 };
            break;
        case 'right':
            cssCurrent = { right: obj.css('right'), opacity: obj.css('opacity') };
            cssForHide = { right: '-10%', opacity: 0 };
            break;
        case 'top':
            cssCurrent = { top: obj.css('top'), opacity: obj.css('opacity') };
            cssForHide = { top: '-10%', opacity: 0 };
            break;
        case 'bottom':
            cssCurrent = { bottom: obj.css('bottom'), opacity: obj.css('opacity') };
            cssForHide = { bottom: '-10%', opacity: 0 };
            break;
        }

        var cssFrom = null;
        var cssTo = null;

        switch (options.method) {
        case 'show':
            cssFrom = cssForHide;
            cssTo = cssCurrent;
            break;
        case 'hide':
            cssFrom = cssCurrent;
            cssTo = cssForHide;
            break;
        }

        var animTime = options.animTime | 500;

        obj.css(cssFrom);
        obj.animate(cssTo, animTime, function() {
            if (callback == null) return;

            if (delayForCallback === 'undefined') {
                callback();
            } else {
                setTimeout(function() { callback(); }, delayForCallback);
            }
        });
    }
};
