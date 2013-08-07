$(document).ready(function() {

    var photoArray = new Array();
    var photoIndex = 0;


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
            photoIndex < (photoArray.length - 1) ? photoIndex++ : null;
            /*GetPhotos(++photoIndex);*/
            SetPhoto(photoArray[photoIndex]);
        };

        self.ShowPrevPhoto = function() {
            photoIndex > 0 ? photoIndex-- : null;
            /*GetPhotos(photoIndex);*/
            SetPhoto(photoArray[photoIndex]);

        };

        self.AddComment = function() {
            $.post("/PhotoComment/AddPhotoComment", { NewComment: self.newComment(), PhotoID: self.PhotoID() }, function(data) {
                SetComments(data);
            });
        };

        self.fbSync = function() {
            $.post("/Photo/FbSync", { photoID: "2" });
        };

        self.ShowLeftSideMenu = function () {
            $("body").css("backgound-color", "black");
            $("#photoSegment").css({ "-webkit-transform-origin": "0% 50%", "-webkit-transition": "all 500ms cubic-bezier(0.77, 0, 0.175, 1)", "transition": "all 500ms cubic-bezier(0.77, 0, 0.175, 1)", "-webkit-transform": "translate(300px) rotateY(-20deg)" })
                          .animate({ "-webkit-transform-origin": "0% 50%", "-webkit-transition": "all 500ms cubic-bezier(0.77, 0, 0.175, 1)", "transition": "all 500ms cubic-bezier(0.77, 0, 0.175, 1)", "-webkit-transform": "translate(300px) rotateY(-20deg)" }, 500);
            
            $("#actionSegment").css({ "-webkit-transform-origin": "0% 50%", "-webkit-transition": "all 500ms cubic-bezier(0.77, 0, 0.175, 1)", "transition": "all 500ms cubic-bezier(0.77, 0, 0.175, 1)", "-webkit-transform": "translate(300px) rotateY(-20deg)" })
                           .animate({ "-webkit-transform-origin": "0% 50%", "-webkit-transition": "all 500ms cubic-bezier(0.77, 0, 0.175, 1)", "transition": "all 500ms cubic-bezier(0.77, 0, 0.175, 1)", "-webkit-transform": "translate(300px) rotateY(-20deg)" }, 500);
            $("#leftSideMenu").css("-webkit-transform", "translateX(300px)").animate("-webkit-transform", "translateX(0px)", 500);
        };
        
        self.HideLeftSideMenu = function () {
            $("#photoSegment").css({ "-webkit-transform-origin": "0px 50%", "-webkit-transition": "all 500ms cubic-bezier(0.77, 0, 0.175, 1)", "transition": "all 500ms cubic-bezier(0.77, 0, 0.175, 1)", "-webkit-transform": "translate(0px) rotateY(0deg)" })
                          .animate({ "-webkit-transform-origin": "0px 50%", "-webkit-transition": "all 500ms cubic-bezier(0.77, 0, 0.175, 1)", "transition": "all 500ms cubic-bezier(0.77, 0, 0.175, 1)", "-webkit-transform": "translate(0px) rotateY(0deg)" }, 500);
            
            $("#actionSegment").css({ "-webkit-transform-origin": "0px 50%", "-webkit-transition": "all 500ms cubic-bezier(0.77, 0, 0.175, 1)", "transition": "all 500ms cubic-bezier(0.77, 0, 0.175, 1)", "-webkit-transform": "translate(0px) rotateY(0deg)" })
                           .animate({ "-webkit-transform-origin": "0px 50%", "-webkit-transition": "all 500ms cubic-bezier(0.77, 0, 0.175, 1)", "transition": "all 500ms cubic-bezier(0.77, 0, 0.175, 1)", "-webkit-transform": "translate(0px) rotateY(0deg)" }, 500);
            $("#leftSideMenu").css("-webkit-transform", "translateX(-300px)").animate("-webkit-transform", "translateX(0px)", 500);
        };

    }

    var model = new PhotoViewModel();
    ko.applyBindings(model);


    var id = document.getElementById("hiddenPhotoID").value;
    model.PhotoID(id);



    function GetFirstPhoto(index) {
        $.post("/Photo/GetPhoto", { photoID: model.PhotoID(), offset: index }, GetAllPhotosFromAlbum);
    }

    function GetAllPhotosFromAlbum(photo) {

        $.post("/Photo/GetPhotosIDFromAlbum", { albumID: photo.AlbumId, begin: 0, end: 1000 }, SetPhotoArray);
    }

    function SetPhotoArray(photos) {
        $.each(photos, function(index, value) {
            photoArray[index] = value;
        });

        photoIndex = photoArray.indexOf(photoArray[model.PhotoID() - 1]);
        SetPhoto(photoArray[photoIndex]);
    }

    function SetPhoto(photo) {
        model.PhotoID(photo.PhotoId);
        model.AlbumID(photo.AlbumID);
        var img = new Image();
        img.onload = function() {
            SetPhotoSize(this.width, this.height);

        };
        img.src = photo.PhotoThumbSource;
        model.src(photo.PhotoThumbSource);

        $("#mainPhoto").attr("src", img.src);
        /*window.history.pushState({ }, "", urlPath);*/
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
    }

    ;

    $('#prevPhotoButton').hover(function() {
        $('#prevPhotoButtonArrow').css({ opacity: 0.0, visibility: "visible" }).animate({ opacity: 1.0 }, 400);
    }, function() {
        $('#prevPhotoButtonArrow').css({ opacity: 0.0, visibility: "visible" }).animate({ opacity: 0.0 }, 500);
    });

    GetFirstPhoto(0);
});