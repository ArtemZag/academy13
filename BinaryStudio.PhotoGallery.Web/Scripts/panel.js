$(document).ready(function () {
    $(".to_top").each(function () {
        $(this).mouseover(function () {
            $(".to_top_opacity").stop().animate({ opacity: "1" }, 300);
            $(".to_top_button").stop().animate({ opacity: "1" }, 300);
        });
        $(this).mouseout(function () {
            $(".to_top_opacity").stop().animate({ opacity: "0" }, 300);
            $(".to_top_button").stop().animate({ opacity: "0.2" }, 300);
        });
    });

    $(window).scroll(function () {
        var g = $(window).scrollTop();
        if (g == 0) $(".to_top").removeClass("no-count");

        if (!$(".to_top").hasClass("no-count")) {
            if (g > 200 && $(".to_top").is(":hidden")) {
                $(".to_top").addClass("visible");
                $(".to_top").css("cursor", "pointer");
                $(".to_top").fadeIn(500);
                $(".to_top").click(function () {
                    $("body, html").animate({ scrollTop: 0 }, 800);
                    $(".to_top").fadeOut(300);
                    $(".to_top").addClass("no-count");
                });
            }
            if (g < 200 && $(".to_top").hasClass("visible")) {
                $(".to_top").removeClass("visible");
                $(".to_top").fadeOut(300);
            }
        } else {
            $(".to_top").unbind("click");
        }
    });
});