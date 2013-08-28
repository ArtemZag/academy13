$(document).ready(function () {

//    var getTagsUrl = $("#getPhotoTagsUrl").data("url");
    var getAlbumInfoUrl = $("#getAlbumInfoUrl").data("url");
    var getPhotosUrl = $("#getPhotosUrl").data("url");
    var postAlbumInfoUrl = $("#postAlbumInfoUrl").data("url");

    function albumViewModel() {

        var self = this;

        self.albumId = 0;

        self.collagePath = ko.observable();

        self.photosCount = ko.observable();

        self.name = ko.observable();

        self.description = ko.observable("no description");

        self.photosCount = ko.observable();

        self.dateOfCreation = ko.observable();

        self.photos = ko.observableArray();

        self.tags = ko.observableArray();

        // tags array to string
        self.tagsString = ko.computed(function () {

            return self.tags().join(", ");
        }, self);

        self.sendInfo = function () {

            // post name and description
            postAlbumInfo();

            // tags
            // postAlbumTags();
        };
        
        self.gotoPhotoPage = function (data) {
            
            window.location = data.PhotoViewPageUrl;
        };
    }

    ko.bindingHandlers.editableText = {
        init: function (element, valueAccessor) {
            $(element).on('blur', function () {
                var observable = valueAccessor();
                observable($(this).text());
            });
        },
        update: function (element, valueAccessor) {
            var value = ko.utils.unwrapObservable(valueAccessor());
            $(element).text(value);
        }
    };

    var album = new albumViewModel();

    album.albumId = document.getElementById("albumId").value;

    function setAlbumInfo(info) {

        album.name(info.AlbumName);
        album.photosCount(info.PhotosCount);
        album.description(info.Description);
        album.collagePath(info.CollageSource);
        album.dateOfCreation(formatDate(info.DateOfCreation));
    }

    function setAlbumTags(tags) {

        $.each(tags, function (index, val) {

            album.tags.push(val);
        });
    }

    // post name and description
    function postAlbumInfo() {

        $.post(postAlbumInfoUrl, { Id: album.albumId, AlbumName: album.name(), Description: album.description() });
    }

    function getAlbumTags() {

//        $.get(getTagsUrl, album.albumId, setAlbumTags);
    }

    function getAlbumInfo() {

        $.get(getAlbumInfoUrl, album.albumId, setAlbumInfo);
    }

    function initPhotosDownloader() {
        PhotoPlacer_Module(getPhotosUrl, album.photos, album.albumId);
    }

    function formatDate(dateTime) {

        var dateEndIndex = dateTime.indexOf("T");

        return dateTime.substring(0, dateEndIndex);
    }

    getAlbumInfo();
//    getAlbumTags();
    initPhotosDownloader();

    ko.applyBindings(album);
});