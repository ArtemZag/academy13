$(document).ready(function () {
    var container = $(".albums");
    var info = $("#userInformation");
    var tool = $("#toolbar");
    var table = $("#content");
    var windowObject = $(window);
    var documentObject = $(document);
    var takeAlbumsCount = 10;
    var skipCount = 0;
    /*var canvas = document.getElementById("canv");
    var ctx = canvas.getContext('2d');
    var colors = ["yellow", "orange", "red", "blue", "indigo"];
    var colorLen = colors.length;
    ctx.lineWidth = 1;*/
    var starColorOffset = 0;
    var width;
    var height;

    //$(window).on('resize', reinitializeCanvas);
    //reinitializeCanvas();

   /* var starSegmentWidth = 8;
    var starSegmentHeight = 8;
    var starWidth = starSegmentWidth * 3;
    var starHeight = starSegmentHeight * 3;
    var xMinIndent = 5;
    var yMinIndent = 5;
    var xIndent;
    var yIndent;*/
    
    windowObject.scroll(scrolling);
    windowObject.resize(resizeTable);
    resizeTable();
    downloadUserInfo();
    downloadNextPartionOfAlbums();

    function downloadUserInfo() {
        $.get("/albums/user", getInfo);
    }

    function resizeTable() {
        table.height(windowObject.height() - tool.height());
    }

    function getInfo(inf) {
        info.html(
            $("#userTmpl").render(inf));
    }

    function downloadNextPartionOfAlbums() {
        $.get("/albums/" + skipCount + "/" + takeAlbumsCount, getAlbums);
        skipCount += takeAlbumsCount;
    }
    function getAlbums(albums) {
        var length = albums.length;
        if (length > 0) {
            container.html(
                $("#collageTmpl").render(albums));
        } else {
            if (skipCount - takeAlbumsCount == 0) {
                container.html(
                $("#uploadTmpl").render());
            }
            windowObject.unbind("scroll");
        }
    }
    function scrolling() {
        if (windowObject.scrollTop() == (documentObject.height() - windowObject.height())) {
            //Пользователь долистал до низа страницы
            downloadNextPartionOfAlbums();
        }
    }

    /*var id2 = setInterval(function () {
        var i = starColorOffset;

        var xOffset = 0;
        var yOffset = 0;
        ctx.clearRect(0, 0, width, height);

        xIndent = checkIndent(starWidth, width, xMinIndent, 0);
        while (xOffset < width) {
            drawStar(ctx, "green", starSegmentWidth, colors[(i++) % colorLen], starSegmentHeight, xOffset, yOffset);
            xOffset += starWidth + xIndent;
        }

        yIndent = checkIndent(starHeight, height - starHeight, yMinIndent, 1);
        xOffset = width - starWidth;
        yOffset = starHeight + yIndent;

        while (yOffset < height) {
            drawStar(ctx, "green", starSegmentWidth, colors[(i++) % colorLen], starSegmentHeight, xOffset, yOffset);
            yOffset += starHeight + yIndent;
        }

        xIndent = checkIndent(starWidth, width - starWidth, xMinIndent, 1);
        xOffset = width - 2 * starWidth - xIndent;
        yOffset = height - starHeight;

        while (xOffset >= -1) {
            drawStar(ctx, "green", starSegmentWidth, colors[(i++) % colorLen], starSegmentHeight, xOffset, yOffset);
            xOffset -= starWidth + xIndent;
        }
        yIndent = checkIndent(starHeight, height - 2 * starHeight, yMinIndent, 2);
        xOffset = 0;
        yOffset = height - 2 * starHeight - yIndent;
        while (yOffset >= starHeight) {
            drawStar(ctx, "green", starSegmentWidth, colors[(i++) % colorLen], starSegmentHeight, xOffset, yOffset);
            yOffset -= starHeight + yIndent;
        }

        starColorOffset--;
        if (starColorOffset <= 0)
            starColorOffset = colorLen - 1;
    }, 100);

    function loadWindow() {
        reinitializeCanvas();
    }

    function checkIndent(bound, length, minIndent, plusIndents) {
        var stars = 1;
        var balance;
        while ((balance = length - bound * stars - (stars - 1.0 + plusIndents) * minIndent) > 0) {
            ++stars;
        }
        --stars;
        balance = length - bound * stars - (stars - 1.0 + plusIndents) * minIndent;
        return minIndent + balance / (stars - 1.0 + plusIndents);
    }

    function reinitializeCanvas() {
        var toolBar = $("#toolbar");
        var toolBarWidth = toolBar.width();
        var toolBarHeight = toolBar.height();

        canvas.width = (width = toolBarWidth);
        canvas.height = (height = $(document).height() - toolBarHeight);

        starColorOffset = 0;
    }
    function scrollExist() {
        return document.scrollHeight == document.offsetHeight;
    }
    /*function scrollWidth() {
        // создадим элемент с прокруткой
        var div = document.createElement('div');
        div.style.overflowY = 'scroll';
        div.style.width =  '50px';
        div.style.height = '50px';
        div.style.visibility = 'hidden';
        document.body.appendChild(div);
        var scrWidth = div.offsetWidth - div.clientWidth;
        document.body.removeChild(div);
        return scrWidth;
    }*/
    /*function drawStar(ctx, strokeColor, width, fillColor, height, dx, dy) {
        ctx.beginPath();
        ctx.moveTo(dx + width, dy + height);
        ctx.lineTo(dx + 3 * width / 2, dy);
        ctx.lineTo(dx + 2 * width, dy + height);
        ctx.lineTo(dx + 3 * width, dy + 3 * height / 2);
        ctx.lineTo(dx + 2 * width, dy + 2 * height);
        ctx.lineTo(dx + 3 * width / 2, dy + 3 * height);
        ctx.lineTo(dx + width, dy + 2 * height);
        ctx.lineTo(dx, dy + 3 * height / 2);
        ctx.closePath();
        ctx.strokeStyle = strokeColor;
        ctx.stroke();
        ctx.fillStyle = fillColor;
        ctx.fill();
    }*/
});