﻿$(document).ready(function () {

    var getTagsUrl = $("#getTagsUrl").data("url");
    var getAlbumInfoUrl = $("#getAlbumInfoUrl").data("url");
    var getPhotosUrl = $("#getPhotosUrl").data("url");

    function albumViewModel() {

        var self = this;

        self.albumId = ko.observable();

        self.collagePath = ko.observable();

        self.name = ko.observableArray();

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

    var albumId = document.getElementById("albumId").value;
    album.albumId(albumId);

    function setAlbumInfo(info) {

        album.name(info.AlbumName);
        album.description(info.Description);
        album.collagePath(info.CollageSource);
        album.dateOfCreation(formatDate(info.DateOfCreation));
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
    
    function initPhotosDownloader() {
        PhotoPlacer_Module(getPhotos, album.photos, albumId);
    }

    function getAlbumPhotos() {

        // todo: photo-placer module call
    }

    function formatDate(dateTime) {

        var dateEndIndex = dateTime.indexOf("T");
        var timeEndIndex = dateTime.lastIndexOf(":");

        var date = dateTime.substring(0, dateEndIndex);
        var time = dateTime.substring(dateEndIndex + 1, timeEndIndex);

        return date + " " + time;
    }

    getAlbumInfo();
    getAlbumTags();
    initPhotosDownloader();

    ko.applyBindings(album);
});