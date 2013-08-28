$(document).ready(function () {
    
    $(window).resize(setPhotoSize);
    


    var photoArray = new Array();


    var navbarClass = $(".navbar");
    var photoSegmentId = $("#photoSegment");
    var actionSegmentId = $("#actionSegment");
    var leftSideMenuId = $("#leftSideMenu");
    var leftSideMenuButtonId = $("#leftSideMenuButton");
    var photoId = $("#photo");

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

    function PhotoByTag(data) {
        var pbt = this;

        pbt.src = ko.observable(data.PhotoThumbSource);
        pbt.linkAway = ko.observable(data.PhotoViewPageUrl);
    }

    function PhotoVieModel() {
        var self = this;

        self.PhotoId = ko.observable();
        self.PhotoIndex = ko.observable(0);
        self.AlbumId = ko.observable();
        self.OwnerID = ko.observable();
        self.Description = ko.observable();
        self.src = ko.observable();
        self.IsVisible = ko.observable();
        self.PhotoLikes = ko.observableArray();
        self.PhotoLikeIcon = ko.observable();
        self.tags = ko.observableArray();

        self.comms = ko.observableArray();
        self.newComment = ko.observable();

        self.PhotosByTags = ko.observableArray();

        self.NumberOfPhotos = ko.observable();

        self.ShowNextPhoto = function() {
            self.PhotoIndex() < (photoArray.length - 1) ? self.PhotoIndex(self.PhotoIndex() + 1) : self.PhotoIndex(0);
            setPhoto(photoArray[self.PhotoIndex()]);
        };

        self.ShowPrevPhoto = function() {
            self.PhotoIndex() > 0 ? self.PhotoIndex(self.PhotoIndex() - 1) : self.PhotoIndex(photoArray.length - 1);
            setPhoto(photoArray[self.PhotoIndex()]);
        };

        self.AddComment = function() {
            $.post("/api/photo/comment", { NewComment: self.newComment(), PhotoId: self.PhotoId() }, function(data) {
                setComments(data);
            });
        };

        // Needs refactoring
        self.DeletePhoto = function() {
            $.ajax({
                url: '/api/photo/' + model.PhotoId(),
                type: 'DELETE',
                success: function(){},
                error: function(){}
            }).done(function (msg) {
                alert("Data Saved: " + msg);
            });
        };
        
        self.MovePhoto = function () {
            $.post("/api/photo/movephoto?photoId=" + self.PhotoId() + "&albumId=" + (self.AlbumId()+2), {}, function (data) {
                
            });
        };

        self.fbSync = function() {
            $.get("/photo/facebook", { photoId: "2" });
        };

        self.ShowLeftSideMenu = function() {
            navbarClass.css({ "-webkit-transform-origin": "30% 50%", "-webkit-transition": "all 500ms cubic-bezier(0.77, 0, 0.175, 1)", "transition": "all 500ms cubic-bezier(0.77, 0, 0.175, 1)", "-webkit-transform": "translate(300px) rotateY(-30deg)" })
                .animate({ "-webkit-transform-origin": "30% 50%", "-webkit-transition": "all 500ms cubic-bezier(0.77, 0, 0.175, 1)", "transition": "all 500ms cubic-bezier(0.77, 0, 0.175, 1)", "-webkit-transform": "translate(300px) rotateY(-30deg)" }, 450);

            photoSegmentId.css({ "-webkit-transform-origin": "30% 50%", "-webkit-transition": "all 500ms cubic-bezier(0.77, 0, 0.175, 1)", "transition": "all 500ms cubic-bezier(0.77, 0, 0.175, 1)", "-webkit-transform": "translate(300px) rotateY(-30deg)" })
                .animate({ "-webkit-transform-origin": "30% 50%", "-webkit-transition": "all 500ms cubic-bezier(0.77, 0, 0.175, 1)", "transition": "all 500ms cubic-bezier(0.77, 0, 0.175, 1)", "-webkit-transform": "translate(300px) rotateY(-30deg)" }, 450);

            actionSegmentId.css({ "-webkit-transform-origin": "30% 50%", "-webkit-transition": "all 500ms cubic-bezier(0.77, 0, 0.175, 1)", "transition": "all 500ms cubic-bezier(0.77, 0, 0.175, 1)", "-webkit-transform": "translate(300px) rotateY(-30deg)" })
                .animate({ "-webkit-transform-origin": "30% 50%", "-webkit-transition": "all 500ms cubic-bezier(0.77, 0, 0.175, 1)", "transition": "all 500ms cubic-bezier(0.77, 0, 0.175, 1)", "-webkit-transform": "translate(300px) rotateY(-30deg)" }, 450);
            leftSideMenuId.css("-webkit-transform", "translateX(300px)").animate("-webkit-transform", "translateX(0px)", 500);
            leftSideMenuButtonId.css("background-color", "transparent");
        };

        self.HideLeftSideMenu = function() {
            navbarClass.css({ "-webkit-transform-origin": "30px 50%", "-webkit-transition": "all 500ms cubic-bezier(0.77, 0, 0.175, 1)", "transition": "all 500ms cubic-bezier(0.77, 0, 0.175, 1)", "-webkit-transform": "translate(0px) rotateY(0deg)" })
                .animate({ "-webkit-transform-origin": "30px 50%", "-webkit-transition": "all 500ms cubic-bezier(0.77, 0, 0.175, 1)", "transition": "all 500ms cubic-bezier(0.77, 0, 0.175, 1)", "-webkit-transform": "translate(0px) rotateY(0deg)" }, 500);

            photoSegmentId.css({ "-webkit-transform-origin": "30px 50%", "-webkit-transition": "all 500ms cubic-bezier(0.77, 0, 0.175, 1)", "transition": "all 500ms cubic-bezier(0.77, 0, 0.175, 1)", "-webkit-transform": "translate(0px) rotateY(0deg)" })
                .animate({ "-webkit-transform-origin": "30px 50%", "-webkit-transition": "all 500ms cubic-bezier(0.77, 0, 0.175, 1)", "transition": "all 500ms cubic-bezier(0.77, 0, 0.175, 1)", "-webkit-transform": "translate(0px) rotateY(0deg)" }, 500);

            actionSegmentId.css({ "-webkit-transform-origin": "30px 50%", "-webkit-transition": "all 500ms cubic-bezier(0.77, 0, 0.175, 1)", "transition": "all 500ms cubic-bezier(0.77, 0, 0.175, 1)", "-webkit-transform": "translate(0px) rotateY(0deg)" })
                .animate({ "-webkit-transform-origin": "30px 50%", "-webkit-transition": "all 500ms cubic-bezier(0.77, 0, 0.175, 1)", "transition": "all 500ms cubic-bezier(0.77, 0, 0.175, 1)", "-webkit-transform": "translate(0px) rotateY(0deg)" }, 500);
            leftSideMenuId.css("-webkit-transform", "translateX(-300px)").animate("-webkit-transform", "translateX(0px)", 500);
            leftSideMenuButtonId.css("background-color", "#e7e7e7");
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
                model.PhotoIndex(index);
            }
        });

        model.NumberOfPhotos(photoArray.length);
        setPhoto(photoArray[model.PhotoIndex()]);
    }

    function setPhoto(photo) {
        model.PhotoId(photo.PhotoId);
        model.AlbumId(photo.AlbumId);

        var img = new Image();
        img.onload = function() { setPhotoSize(this.width, this.height); };
        img.src = photo.PhotoThumbSource;
        model.src(img.src);

        // todo: needs fixing
        window.history.pushState("", "", "/photo/" + model.PhotoId());

        $.get("/api/photo/" + photo.PhotoId + "/comments", { skip: 0, take: 50 }, setComments);
        $.get("/api/photo/" + model.PhotoId() + "/likes", setLikes);
        $.get("/api/photo/" + model.PhotoId() + "/photosbytags", setPhotosByTags);

    }

    function setPhotosByTags(photos) {
        model.PhotosByTags.removeAll();
        $.each(photos, function(k, item) {
            model.PhotosByTags.push(new PhotoByTag(item));
        });
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
        $.post('/api/photo/like', { '': photoId() }, setLikes);
    }

    function setPhotoSize(w, h) {
        var viewPortWidth = $(window).width();
        var viewPortHeight = $(window).height() - 50;

        var height = viewPortHeight * 0.81;

        if (w > h) {
            viewPortWidth = viewPortWidth * 0.81;
            var kw = viewPortWidth / w;
            viewPortHeight = h * kw;
            
            if (viewPortHeight > height) {
                viewPortHeight = height / viewPortHeight;
                viewPortWidth = viewPortWidth * viewPortHeight;
                viewPortHeight = height;
            }
            
        } else {
            viewPortHeight = viewPortHeight * 0.81;
            var kh = viewPortHeight / h;
            viewPortWidth = w * kh;
        }

        photoId.css("width", viewPortWidth);
        photoId.css("height", viewPortHeight);
    }

    $('#prevPhotoButton').hover(function() {
        $('#prevPhotoButtonArrow').css({ opacity: 0.0, visibility: "visible" }).animate({ opacity: 1.0 }, 400);
    }, function() {
        $('#prevPhotoButtonArrow').css({ opacity: 0.0, visibility: "visible" }).animate({ opacity: 0.0 }, 500);
    });

    getFirstPhoto();
});