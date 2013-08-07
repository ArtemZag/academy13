﻿$(document).ready(function () {

    function searchViewModel() {

        var self = this;

        self.interval = 10;
        self.begin = 0;
        self.end = self.begin + self.interval;

        self.searchQuery = ko.observable();

        self.isSearchUsersByName = ko.observable(false);
        self.isSearchUserByDepartment = ko.observable(false);
        self.isSearchUserByNickname = ko.observable(false);

        self.isSearchPhotosByName = ko.observable(false);
        self.isSearchPhotosByTags = ko.observable(false);
        self.isSearchPhotosByDescription = ko.observable(false);

        self.isSearchAlbumsByName = ko.observable(false);
        self.isSearchAlbumsByTags = ko.observable(false);
        self.isSearchAlbumsByDescription = ko.observable(false);

        self.isSearchByComments = ko.observable(false);

        self.search = function () {

        };

        self.appendSearch = function () {

        };
    }

    ko.applyBindings(searchViewModel);
});