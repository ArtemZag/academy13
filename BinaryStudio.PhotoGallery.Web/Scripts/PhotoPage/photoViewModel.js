﻿$(document).ready(function() {

    var photoArray = new Array();
    var i = 0;


    function User(data) {
        var u = this;
        u.firstName = ko.observable(data.OwnerFirstName);
        u.lastName = ko.observable(data.OwnerLastName);
        u.photoSource = ko.observable(data.OwnerPhotoSource);
    }

    function Comment(data) {
        var com = this;
        
        com.text = ko.observable(data.Text);

        var date = new Date(parseInt(data.DateOfCreating.substr(6)));
        com.dateOfCreating = ko.observable(date.toLocaleString());
        
        com.rating = ko.observable(data.Rating);
        com.userInfo = ko.observable(new User(data.UserInfo));

        com.GetUserName = ko.computed(function() {
            return com.userInfo().firstName() + " " + com.userInfo().lastName();
        },
            this);
    }
    
    
    function PhotoViewModel() {
        var self = this;
        self.PhotoID = ko.observable();
        self.AlbumID = ko.observable();
        self.OwnerID = ko.observable();
        self.Description = ko.observable();
        self.src = ko.observable();
        self.IsVisible = ko.observable();

        self.comms = ko.observableArray();
        self.newComment = ko.observable();

        self.ShowNextPhoto = function() {
            GetPhotos();
        };

        self.ShowPrevPhoto = function() {
            i > 1 ? i -= 2 : i--;
            GetPhotos();

        };

        self.AddComment = function() {
            var dataToSend = { userData: ko.toJSON(self.newComment) };
            $.post("/PhotoComment/AddPhotoComment", {NewComment: self.newComment(), PhotoID: self.PhotoID()}, function(data) {
                SetComments(data);
            });
        };

        self.fbSync = function () {
            $.post("/Photo/FbSync", { photoID: "2" });
        };
        
        self.ShowLeftSideMenu = function() {
            $("#leftSideMenu").css("-webkit-transform", "translateX(300px)").animate("-webkit-transform", "translateX(300px)", 300);
            /*$("#photoSegment").css("-webkit-transform", "rotateY(-30deg)").animate("-webkit-transform", "rotateY(-30px)", 300);*/
           /* $("#photoSegment").css("-webkit-transform", "translateX(300px)").animate("-webkit-transform", "translateX(300px)", 300);*/
        };
        self.HideLeftSideMenu = function () {
            $("#leftSideMenu").css("-webkit-transform", "translateX(-300px)").animate("-webkit-transform", "translateX(-300px)", 300);
            /*$("#photoSegment").css("-webkit-transform", "rotateY(30deg)").animate("-webkit-transform", "rotateY(30px)", 300);*/
            /*$("#photoSegment").css("-webkit-transform", "translateX(-300px)").animate("-webkit-transform", "translateX(-300px)", 300);*/
        };
        
    }

    var model = new PhotoViewModel();
    ko.applyBindings(model);


    var id = document.getElementById("hiddenPhotoID").value;
    model.PhotoID(id);




    function GetPhotos(photoID) {
        $.post("/Photo/GetPhoto", { photoID: photoID}, SetPhoto);
        i++;
    }

    function SetPhoto(photo) {
        model.PhotoID(photo.PhotoId);
        var img = new Image();
        img.onload = function() {
            SetPhotoSize(this.width, this.height);
        };
        img.src = photo.PhotoThumbSource;

        $("#mainPhoto").attr("src", img.src);
        $.post("/PhotoComment/GetPhotoComments", { photoID: photo.PhotoId, begin: 0, last: 50 }, SetComments);
    }

    function SetComments(comm) {
        model.comms.destroyAll();
        $.each(comm, function(k, item) {
            model.comms.push(new Comment(item));
        });
    }


    function SetPhotoSize(w, h) {
        var width = $(window).width();
        var height = $(window).height() - 50;

        if (w > h) {
            width = width * 0.81;
            var kw = width / w;
            height = h * kw;
        } else {
            height = height * 0.81;
            var kh = height / h;
            width = w * kh;
        }

        $("#photo").css("width", width);
        $("#photo").css("height", height);
    };

    $('#prevPhotoButton').hover(function() {
        $('#prevPhotoButtonArrow').css({opacity: 0.0, visibility: "visible"}).animate({opacity: 1.0}, 400);
    }, function() {
        $('#prevPhotoButtonArrow').css({ opacity: 0.0, visibility: "visible" }).animate({ opacity: 0.0 }, 500);
    });

    GetPhotos(model.PhotoID());
});