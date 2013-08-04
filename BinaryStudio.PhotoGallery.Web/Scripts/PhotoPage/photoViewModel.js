$(document).ready(function() {

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
        /*self.comms = typeof(self.comms) !== 'undefined' ? self.comms : [];*/
        self.comms = ko.observableArray();

        self.ShowNextPhoto = function() {
            GetPhotos();
        };

        self.ShowPrevPhoto = function() {
            i--;
            i--;
            GetPhotos();
            
        };

        self.SaveUserData = function() {
            var data_to_send = { userData: ko.toJSON(self) };
            $.post("/PhotoComment/AddPhotoComment", data_to_send, function(data) {
                alert("Your data has been posted to the server!");
            });
        };
    }

    var model = new PhotoViewModel();
    ko.applyBindings(model);







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
});