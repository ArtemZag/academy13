$(document).ready(function() {

    var photoArray = new Array();
    var i =0 ;

    function Comment(data) {
        var com = this;
        com.text = ko.observable(data.Text);
        com.dateOfCreating = ko.observable(data.DateOfCreating);
        com.rating = ko.observable(data.Rating);
        com.userInfo = ko.observableArray();
    }

    function PhotoViewModel() {
        var self = this;
        self.PhotoID = ko.observable();
        self.AlbumID = ko.observable();
        self.OwnerID = ko.observable();
        self.Description = ko.observable();
        self.src = ko.observable();
        self.IsVisible = ko.observable();
        
        self.comms = ko.observableArray([]);

        self.ShowNextPhoto = function() {
            GetPhotos();
        };

        self.ShowPrevPhoto = function() {
            i--;
            i--;
            GetPhotos();
            
        };
    }

    ko.applyBindings(new PhotoViewModel);







    function GetPhotos() {
        var photoList;
        $.post("/Photo/GetPhotos", { albumName: "Test", begin: i, last: i+1 }, SetPhoto);
        i++;
        return photoList;
    }

    function SetPhoto(photo) {
        
        var img = new Image();
        img.onload = function() {
            SetPhotoSize(this.width, this.height);
        };
        img.src = photo[0].PhotoThumbSource;

        $("#mainPhoto").attr("src", img.src);
        $.post("/PhotoComment/GetPhotoComments", { photoID: photo[0].PhotoId, begin: 0, last: 50 }, SetComments);
    }

    function SetComments(comm) {

        comms = $.map(comm, function (item) { return new Comment(item); });
        PhotoViewModel.comms = comms;
    }


    function SetPhotoSize(w, h) {
        var width = $(window).width();
        var height = $(window).height();

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
});