$(document).ready(function () {
    $(window).resize(calcPhotoSizes);
    calcPhotoSizes(); // todo why is it bad works on webkit? Need fix.

    function calcPhotoSizes() {
        var width = 0;
        var firstElemInRow = 0;
        var margins = 0;
        var wrapperWidth = $('div#photoWrapper').width();
        var marginPhotoCont = parseInt($('.photoContainer').css('margin-left'))
                            + parseInt($('.photoContainer').css('margin-right'));
        var photos = $('div#photoWrapper > div > img');
        jQuery.each(photos, function (i) {
            width += this.width;
            margins += marginPhotoCont;
            if (width > wrapperWidth - margins) {
                var coef = (wrapperWidth - margins) / width;
                for (var j = firstElemInRow; j <= i; j++) {
                    $(photos[j]).closest(".photoContainer").css('width', (photos[j].width * coef) - 0.2);
                }
                firstElemInRow = i + 1;
                width = 0;
                margins = 0;
            }
            else if (i == photos.length - 1) {
                for (j = firstElemInRow; j <= i; j++) {
                    $(photos[j]).closest(".photoContainer").css('width', photos[j].width);
                }
            }
        });
    }

    console.log("here");
    $("#photopreloader").hide();
    $("#tester").click(ajaxPhotoLoad);

    var numPh = 5; //чтобы знать с какой записи вытаскивать данные

    function ajaxPhotoLoad() { //Выполняем если по кнопке кликнули
        $("#photopreloader").show(); //Показываем прелоадер
       
        $.post('../Home/Index',
            {
                num: numPh,
            })
            .done(function (data) {

                if (data == "ok") {
                    hideLoginPanel(function () {
                        setTimeout(function () { window.location = "../Home/Index"; }, 500);
                    });
                } else {
                    // show error fields
                    console.log(data);
                }
            })
            .fail(function () {
                
            });
        
        //$.ajax({
        //    url: "../Home/Index/5",
        //    type: "GET",
        //    data: { "num": num },
        //    cache: false,
        //    success: function (response) {
        //        if (response == 0) { // смотрим ответ от сервера и выполняем соответствующее действие
        //            alert("Больше нет записей");
        //        } else {
        //            $("#testdiv").text = "@Model.Num";
        //        }
        //        $("#photopreloader").hide();
        //    }
        //});
    }
});

