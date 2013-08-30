$(document).ready(function () {

    var getTagsUrl = $("#getTagsUrl").data("url");
    var getAlbumInfoUrl = $("#getAlbumInfoUrl").data("url");
    var getPhotosUrl = $("#getPhotosUrl").data("url");
    var getGroupsUrl = $("#getGroupsUrl").data("url");
    var postAlbumInfoUrl = $("#postAlbumInfoUrl").data("url");
    var postGroupsUrl = $("#postGroupsUrl").data("url");

    function albumViewModel() {

        var self = this;

        self.albumId = 0;

        self.ownerId = 0;

        self.collagePath = ko.observable();

        self.photosCount = ko.observable();

        self.name = ko.observable("Loading...");

        self.description = ko.observable();

        self.photosCount = ko.observable();

        self.dateOfCreation = ko.observable();

        self.groups = ko.observableArray();

        self.selectedGroup = ko.observable();

        self.photos = ko.observableArray();

        self.tags = ko.observableArray();

        // tags array to string
        self.tagsString = ko.computed(function () {

            return self.tags().join(", ");
        }, self);

        self.sendInfo = function () {

            // post name and description
            postAlbumInfo();

            // post new rights for groups
            postRights();

            // notification
            showNotification();
        };

        self.gotoPhotoPage = function (data) {

            window.location = data.PhotoViewPageUrl;
        };
    }

    function groupViewModel(id, name, canSeePhotos, canSeeComments) {

        var self = this;

        self.groupId = id;

        self.name = ko.observable(name);

        self.canSeePhotos = ko.observable();

        self.canSeeComments = ko.observable();

        self.canSeePhotos(canSeePhotos);
        self.canSeeComments(canSeeComments);
    }

    // for contenteditable binding 
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

    function showNotification() {

        $("#notification").show('slow', function () {
            setTimeout(function() {
                $("#notification").hide('slow');
            }, 400);
        });
    }

    function setAlbumInfo(info) {

        album.ownerId = info.OwnerId;

        album.name(info.AlbumName);
        album.photosCount(info.PhotosCount);
        album.description(info.Description);
        album.collagePath(info.CollageSource);
        album.dateOfCreation(formatDate(info.DateOfCreation));

        getGroups();

        setModifyMode();
    }

    function setModifyMode() {

        if (!isCanModify()) {

            $("#saveButton").hide();
            $("#rightsOptions").hide();
            $("#albumName").removeAttr("contenteditable");
            $("#description").removeAttr("contenteditable");
        }
    }

    function isCanModify() {

        var currentUserId = document.getElementById("currentUserId").value;
        var isAdmin = document.getElementById("isAdmin").value;

        return album.ownerId == currentUserId || isAdmin;
    }

    function setAlbumTags(tags) {

        $.each(tags, function (index, val) {

            album.tags.push(val);
        });
    }

    function setGroups(groups) {

        $.each(groups, function (index, val) {

            album.groups.push(new groupViewModel(val.GroupId, val.Name, val.CanSeePhotos, val.CanSeeComments));
        });

        if (album.groups().length == 0) {

            $("#rightsOptions").hide();
        }
    }

    // post name and description

    function postAlbumInfo() {

        $.post(postAlbumInfoUrl, {
            Id: album.albumId,
            AlbumName: album.name(),
            Description: album.description()
        });
    }

    function postRights() {

        $.ajax({
            url: postGroupsUrl,
            type: 'POST',
            data: {
                viewModels: album.groups(),
                albumId: album.albumId
            }
        });

    }

    function getAlbumTags() {

        $.get(getTagsUrl, album.albumId, setAlbumTags);
    }

    function getAlbumInfo() {

        $.get(getAlbumInfoUrl, album.albumId, setAlbumInfo);
    }

    function getGroups() {

        $.ajax({
            url: getGroupsUrl,
            type: 'GET',
            data: {
                albumId: album.albumId,
                userId: album.ownerId
            }
        })
            .done(setGroups);
    }

    function initPhotosDownloader() {

        PhotoPlacer_Module(getPhotosUrl, album.photos, album.albumId);
    }

    function formatDate(dateTime) {

        var dateEndIndex = dateTime.indexOf("T");

        return dateTime.substring(0, dateEndIndex);
    }

    var album = new albumViewModel();
    album.albumId = document.getElementById("albumId").value;

    ko.applyBindings(album);

    getAlbumInfo();
    getAlbumTags();

    initPhotosDownloader();
});