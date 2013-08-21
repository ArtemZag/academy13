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

    function Like(data) {
        var lik = this;

        lik.firstName = ko.observable(data.FirstName);
        lik.lastName = ko.observable(data.LastName);

        lik.fullName = ko.computed(function() {
            return lik.firstName() + " " + lik.lastName();
        }, this);

        /*lik.avaSRC = ko.observable(data.src);*/
    }

    function PhotoVieModel() {
        var self = this;

        self.PhotoId = ko.observable();
        self.AlbumId = ko.observable();
        self.OwnerID = ko.observable();
        self.Description = ko.observable();
        self.src = ko.observable();
        self.IsVisible = ko.observable();
        self.PhotoLikes = ko.observableArray();
        self.PhotoLikeIcon = ko.observable();

        self.comms = ko.observableArray();
        self.newComment = ko.observable();

        self.ShowNextPhoto = function() {
            photoIndex < (photoArray.length - 1) ? photoIndex++ : photoIndex = 0;
            setPhoto(photoArray[photoIndex]);
        };

        self.ShowPrevPhoto = function() {
            photoIndex > 0 ? photoIndex-- : photoIndex = (photoArray.length - 1);
            setPhoto(photoArray[photoIndex]);
        };

        self.AddComment = function() {
            $.post("/api/photo/comment", { NewComment: self.newComment(), PhotoId: self.PhotoId() }, function(data) {
                setComments(data);
            });
        };

        self.fbSync = function() {
            $.get("/photo/facebook", { photoId: "2" });
        };

        self.ShowLeftSideMenu = function() {
            $(".navbar").css({ "-webkit-transform-origin": "30% 50%", "-webkit-transition": "all 500ms cubic-bezier(0.77, 0, 0.175, 1)", "transition": "all 500ms cubic-bezier(0.77, 0, 0.175, 1)", "-webkit-transform": "translate(300px) rotateY(-30deg)" })
                .animate({ "-webkit-transform-origin": "30% 50%", "-webkit-transition": "all 500ms cubic-bezier(0.77, 0, 0.175, 1)", "transition": "all 500ms cubic-bezier(0.77, 0, 0.175, 1)", "-webkit-transform": "translate(300px) rotateY(-30deg)" }, 450);

            $("#photoSegment").css({ "-webkit-transform-origin": "30% 50%", "-webkit-transition": "all 500ms cubic-bezier(0.77, 0, 0.175, 1)", "transition": "all 500ms cubic-bezier(0.77, 0, 0.175, 1)", "-webkit-transform": "translate(300px) rotateY(-30deg)" })
                .animate({ "-webkit-transform-origin": "30% 50%", "-webkit-transition": "all 500ms cubic-bezier(0.77, 0, 0.175, 1)", "transition": "all 500ms cubic-bezier(0.77, 0, 0.175, 1)", "-webkit-transform": "translate(300px) rotateY(-30deg)" }, 450);

            $("#actionSegment").css({ "-webkit-transform-origin": "30% 50%", "-webkit-transition": "all 500ms cubic-bezier(0.77, 0, 0.175, 1)", "transition": "all 500ms cubic-bezier(0.77, 0, 0.175, 1)", "-webkit-transform": "translate(300px) rotateY(-30deg)" })
                .animate({ "-webkit-transform-origin": "30% 50%", "-webkit-transition": "all 500ms cubic-bezier(0.77, 0, 0.175, 1)", "transition": "all 500ms cubic-bezier(0.77, 0, 0.175, 1)", "-webkit-transform": "translate(300px) rotateY(-30deg)" }, 450);
            $("#leftSideMenu").css("-webkit-transform", "translateX(300px)").animate("-webkit-transform", "translateX(0px)", 500);
            $("#leftSideMenuButton").css("background-color", "transparent");
        };

        self.HideLeftSideMenu = function() {
            $(".navbar").css({ "-webkit-transform-origin": "30px 50%", "-webkit-transition": "all 500ms cubic-bezier(0.77, 0, 0.175, 1)", "transition": "all 500ms cubic-bezier(0.77, 0, 0.175, 1)", "-webkit-transform": "translate(0px) rotateY(0deg)" })
                .animate({ "-webkit-transform-origin": "30px 50%", "-webkit-transition": "all 500ms cubic-bezier(0.77, 0, 0.175, 1)", "transition": "all 500ms cubic-bezier(0.77, 0, 0.175, 1)", "-webkit-transform": "translate(0px) rotateY(0deg)" }, 500);

            $("#photoSegment").css({ "-webkit-transform-origin": "30px 50%", "-webkit-transition": "all 500ms cubic-bezier(0.77, 0, 0.175, 1)", "transition": "all 500ms cubic-bezier(0.77, 0, 0.175, 1)", "-webkit-transform": "translate(0px) rotateY(0deg)" })
                .animate({ "-webkit-transform-origin": "30px 50%", "-webkit-transition": "all 500ms cubic-bezier(0.77, 0, 0.175, 1)", "transition": "all 500ms cubic-bezier(0.77, 0, 0.175, 1)", "-webkit-transform": "translate(0px) rotateY(0deg)" }, 500);

            $("#actionSegment").css({ "-webkit-transform-origin": "30px 50%", "-webkit-transition": "all 500ms cubic-bezier(0.77, 0, 0.175, 1)", "transition": "all 500ms cubic-bezier(0.77, 0, 0.175, 1)", "-webkit-transform": "translate(0px) rotateY(0deg)" })
                .animate({ "-webkit-transform-origin": "30px 50%", "-webkit-transition": "all 500ms cubic-bezier(0.77, 0, 0.175, 1)", "transition": "all 500ms cubic-bezier(0.77, 0, 0.175, 1)", "-webkit-transform": "translate(0px) rotateY(0deg)" }, 500);
            $("#leftSideMenu").css("-webkit-transform", "translateX(-300px)").animate("-webkit-transform", "translateX(0px)", 500);
            $("#leftSideMenuButton").css("background-color", "#e7e7e7");
        };

        self.IncrementPhotoLike = function() {
            addLike(self.PhotoId);
            self.PhotoLikeIcon("/Content/images/photo-page/like-icon.png");
        };
    }

    var model = new PhotoVieModel();
    ko.applyBindings(model);

    var id = document.getElementById("hiddenPhotoID").value;
    model.PhotoId(id);

    function getFirstPhoto() {
        $.get("/api/photo/" + model.PhotoId(), getAllPhotosFromAlbum);
    }

    function getAllPhotosFromAlbum(photo) {
        $.get("/api/photo?albumId=" + photo.AlbumId + "&skip=" + 0 + "&take=" + 100, setPhotoArray);
    }

    function setPhotoArray(photos) {
        $.each(photos, function(index, value) {
            photoArray[index] = value;
            if (photoArray[index].PhotoId == model.PhotoId()) {
                photoIndex = index;
            }
        });

        setPhoto(photoArray[photoIndex]);
    }

    function setPhoto(photo) {
        model.PhotoId(photo.PhotoId);
        model.AlbumId(photo.AlbumId);

        var img = new Image();
        img.onload = function() { setPhotoSize(this.width, this.height); };
        img.src = photo.PhotoThumbSource;
        model.src(img.src);

        // todo: needs fixing
        window.history.pushState("", "", "/Photo/" + model.PhotoId());

        $.get("/api/photo/" + photo.PhotoId + "/comments", { skip: 0, take: 50 }, setComments);
        $.get("/api/photo/" + model.PhotoId() + "/likes", setLikes);
    }

    function setComments(comm) {
        model.comms.removeAll();
        $.each(comm, function(k, item) {
            model.comms.push(new Comment(item));
        });
    }

    function setLikes(likes) {
        model.PhotoLikes.removeAll();
        $.each(likes, function(k, item) {
            model.PhotoLikes.push(new Like(item));
        });
    }

    function addLike(photoId) {
        // TODO Must be replaced with PUT method
        $.post("/api/photo/" + photoId() + "/like", setLikes);
    }

    function setPhotoSize(w, h) {
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

    $('#prevPhotoButton').hover(function() {
        $('#prevPhotoButtonArrow').css({ opacity: 0.0, visibility: "visible" }).animate({ opacity: 1.0 }, 400);
    }, function() {
        $('#prevPhotoButtonArrow').css({ opacity: 0.0, visibility: "visible" }).animate({ opacity: 0.0 }, 500);
    });

    getFirstPhoto();
});