var verticalResizer_Module = function ($) {
    $.fn.rtg = function (userOptions) {
        rtg.options = $.extend(true, {}, rtg.defaults, userOptions);
        rtg.el = $(this);
        rtg.el.find('.rtg-images').css({ 'height': rtg.options.initialHeight });
        rtg.loading.start();
        rtg.init();
    };
    var rtg = {};
    rtg.defaults = {imageWidth: 300, spacing: 10, center: true, initialHeight: 0 };
    rtg.init = function () {
        rtg.images.resize();
        rtg.images.show();
        rtg.utils.addTransition(rtg.el.find('.rtg-images > div'));
        rtg.images.sort();
        rtg.images.center();
        var resize = function () {
            rtg.images.sort();
            rtg.images.center();
        };
        var resizeTimer;
        $(window).resize(function () {
            clearTimeout(resizeTimer);
            resizeTimer = setTimeout(resize, 200);
        });
        if (navigator.appVersion.indexOf("MSIE 7.") != -1) {
            rtg.el.find('.rtg-categories > li').css('display', 'inline').find('a').css({ 'display': 'block', 'padding': '3px 7px' });
        }
    };
    rtg.loading = {
        image: $("<img src='/Content/images/loading_green.gif'/>"), start: function () {
            this.image.css({ 'position': 'absolute', 'top': 150, 'left': (rtg.el.width() - this.image.width()) / 2 });
            rtg.el.prepend(this.image);
        }, stop: function () {
            this.image.remove();
        }
    };
    rtg.images = {};
    rtg.images.resize = function () {
        var units = rtg.el.find('.rtg-images > div'), opts = rtg.options;
        units.each(function () {
            var unit = $(this);
            image = unit.find('img'), oldWidth = image.width(), oldHeight = image.height(), ratio = opts.imageWidth / oldWidth, newWidth = opts.imageWidth, newHeight = oldHeight * ratio;
            $.merge(unit, unit.find('*')).css({ 'width': newWidth, 'height': newHeight });
        });
    };
    rtg.images.show = function () {
        rtg.el.find('.rtg-images > div').css('opacity', '0').css('visibility', 'visible').each(function () {
            $(this).animate({ 'opacity': '1' }, {
                duration: 100 + Math.floor(Math.random() * 900), complete: function () {
                    rtg.loading.stop();
                }
            });
        });
    };
    rtg.images.sort = function () {
        var units = rtg.el.find('.rtg-images > div'), opts = rtg.options;
        var numberOfColumns = Math.ceil((rtg.el.width()) / (opts.imageWidth + opts.spacing));
        var koef = rtg.el.width() / ((opts.imageWidth + opts.spacing) * numberOfColumns);
        //		opts.imageWidth*=koef;
        numberOfColumns = (numberOfColumns === 0) ? 1 : numberOfColumns;
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
            if (rtg.utils.transitions) {
                $(this).css({ 'top': columnHeights[column], 'left': column * (opts.imageWidth + opts.spacing) });
            } else {
                $(this).animate({ 'top': columnHeights[column], 'left': column * (opts.imageWidth + opts.spacing) }, 500);
            }
            columnHeights[column] = columnHeights[column] + $(this).height() + opts.spacing;
            if (columnHeights[column] > tallest) {
                tallest = columnHeights[column];
            }
        });
        if (rtg.options.center) {
            numberOfColumns = (actualColumns < numberOfColumns) ? actualColumns : numberOfColumns;
        }
        rtg.el.find('.rtg-images').css({ 'height': tallest, 'width': (numberOfColumns * (opts.imageWidth + opts.spacing)) - opts.spacing }, 400);
    };
    rtg.images.center = function () {
        if (!rtg.options.center) {
            return;
        }
        ;
        var images = rtg.el.find('.rtg-images');
        var left = (rtg.el.width() - images.width()) / 2;
        left = (left <= 0) ? 0 : left;
        images.animate({ 'left': left });
        rtg.el.find('.rtg-categories').animate({ 'margin-left': left });
    };
    rtg.utils = {};
    rtg.utils.addTransition = function (el) {
        if (rtg.utils.transitions) {
            el.each(function () {
                $(this).css({ '-webkit-transition': 'all 0.7s ease', '-moz-transition': 'all 0.7s ease', '-o-transition': 'all 0.7s ease', 'transition': 'all 0.7s ease' });
            });
        }
    };
    rtg.utils.removeTransition = function (el) {
        if (rtg.utils.transitions) {
            el.each(function () {
                $(this).css({ '-webkit-transition': 'none 0.7s ease', '-moz-transition': 'none 0.7s ease', '-o-transition': 'none 0.7s ease', 'transition': 'none 0.7s ease' });
            });
        }
    };
    rtg.utils.transitions = (function () {
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