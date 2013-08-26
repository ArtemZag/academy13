var verticalResizer_Module = function (wrapper) {
    var $ = jQuery;
    $.fn.vr = function (userOptions) {
        vr.options = $.extend(true, {}, vr.defaults, userOptions);
        vr.el = $(this);
        vr.el.find(wrapper).css({ 'height': vr.options.initialHeight });
        vr.loading.start();
        vr.init();
    };
    var vr = {};
    vr.defaults = {imageWidth: 300, spacing: 10, initialHeight: 0 };
    vr.init = function () {
        vr.images.resize(true);
        vr.images.show();
        vr.utils.addTransition(vr.el.find(wrapper + '> div'));
        vr.images.sort();
        var resize = function () {
            vr.images.resize(false);
            vr.images.sort();
        };
        var resizeTimer;
        $(window).resize(function () {
            clearTimeout(resizeTimer);
            resizeTimer = setTimeout(resize, 300);
        });
    };
    vr.loading = {
        start: function () {
            $("#loader").show();
        }, stop: function () {
            $("#loader").hide();
        }
    };
    vr.images = {};


    var numberOfColumns;
    var koef;

    vr.images.resize = function (init) {
        var $units = vr.el.find(wrapper + ' > div'), opts = vr.options;
        numberOfColumns = Math.ceil((vr.el.width()) / (opts.imageWidth + opts.spacing));
        numberOfColumns = (numberOfColumns === 0) ? 1 : numberOfColumns;
        koef = vr.el.width() / ((opts.imageWidth + opts.spacing) * numberOfColumns + opts.spacing/2);
        $units.each(function () {
            var $unit = $(this);
            var $image = $unit.find('img');
            var oldWidth = $image.width();
            var newWidth = opts.imageWidth * koef;
            var newHeight = $image.height();
            if (init == true) {
                var ratio = opts.imageWidth / oldWidth;
                newHeight *= ratio;
                $unit.find('*').css({ 'width': opts.imageWidth, 'height': newHeight });
            }
            
            $unit.css({ 'width': newWidth, 'height': newHeight });
        });
    };
    vr.images.show = function () {
        vr.el.find(wrapper + ' > div').css('opacity', '0').css('visibility', 'visible').each(function () {
            $(this).animate({ 'opacity': '1' }, {
                duration: 100 + Math.floor(Math.random() * 900), complete: function () {
                    vr.loading.stop();
                }
            });
        });
    };
    
    vr.images.sort = function () {
        var units = vr.el.find(wrapper + ' > div'), opts = vr.options;
        var columnHeights = [], i = 0;
        for (i; i < numberOfColumns; i = i + 1) {
            columnHeights[i] = 0;
        }
        var column, tallest = 0, actualColumns = 0;
        units.each(function () {
            if ($(this).css('display') == 'none') {
                return;
            }
            actualColumns++;
            column = columnHeights.min();
            if (vr.utils.transitions) {
                $(this).css({ 'top': columnHeights[column], 'left': opts.spacing / 2 + column * (opts.imageWidth * koef + opts.spacing) });
            } else {
                $(this).animate({ 'top': columnHeights[column], 'left': opts.spacing / 2 + column * (opts.imageWidth * koef + opts.spacing) }, 500);
            }
            columnHeights[column] = columnHeights[column] + $(this).height() + opts.spacing;
            if (columnHeights[column] > tallest) {
                tallest = columnHeights[column];
            }
        });
        vr.el.find(wrapper).css({ 'height': tallest, 'width': (numberOfColumns * (opts.imageWidth + opts.spacing)) - opts.spacing }, 400);
    };
    vr.utils = {};
    vr.utils.addTransition = function (el) {
        if (vr.utils.transitions) {
            el.each(function () {
                $(this).css({ '-webkit-transition': 'all 0.7s ease', '-moz-transition': 'all 0.7s ease', '-o-transition': 'all 0.7s ease', 'transition': 'all 0.7s ease' });
            });
        }
    };
    vr.utils.removeTransition = function (el) {
        if (vr.utils.transitions) {
            el.each(function () {
                $(this).css({ '-webkit-transition': 'none 0.7s ease', '-moz-transition': 'none 0.7s ease', '-o-transition': 'none 0.7s ease', 'transition': 'none 0.7s ease' });
            });
        }
    };
    vr.utils.transitions = (function () {
        function cssTransitions() {
            var div = document.createElement("div");
            var p, ext, pre = ["ms", "O", "Webkit", "Moz"];
            for (p in pre) {
                if (div.style[pre[p] + "Transition"] !== undefined) {
                    ext = pre[p];
                    break;
                }
            }
            delete div;
            return ext;
        }
        ;
        return cssTransitions();
    }());
    Array.prototype.min = function () {
        var min = 0, i = 0;
        for (i; i < this.length; i = i + 1) {
            if (this[i] < this[min]) {
                min = i;
            }
        }
        return min;
    };
};