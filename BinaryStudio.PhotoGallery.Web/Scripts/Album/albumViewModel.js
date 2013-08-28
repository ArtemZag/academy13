$(document).ready(function () {

    var getTagsUrl = $("#getTagsUrl").data("url");
    var getAlbumInfoUrl = $("#getAlbumInfoUrl").data("url");

    function albumViewModel() {

        var self = this;

        self.albumId = ko.observable();

        self.name = ko.observableArray();

        self.description = ko.observable();

        self.photosCount = ko.observable();

        self.photos = ko.observableArray();

        self.tags = ko.observableArray();

        // tags array to string
        self.tagsString = ko.computed(function () {

            return self.tags().join(", ");
        }, self);
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

    var albumId = document.getElementById("albumId").value;
    album.albumId(albumId);

    function setAlbumInfo(info) {

        album.name(info.AlbumName);
        album.description(info.Description);
    }

    function setAlbumTags(tags) {

        $.each(tags, function (index, val) {

            album.tags.push(val);
        });
    }

    function getAlbumTags() {

        $.get(getTagsUrl, album.albumId(), setAlbumTags);
    }

    function getAlbumInfo() {

        $.get(getAlbumInfoUrl, album.albumId(), setAlbumInfo);
    }

    getAlbumInfo();
    getAlbumTags();

    ko.applyBindings(album);
});