$(document).ready(function() {

    function searchViewModel() {

        var interval = 10;

        var self = this;

        self.hashToken = "";

        self.begin = 0;
        self.end = interval;

        self.searchQuery = ko.observable();

        self.isSearchPhotosByName = ko.observable();
        self.isSearchPhotosByTags = ko.observable();
        self.isSearchPhotosByDescription = ko.observable();

        self.isSearchAlbumsByName = ko.observable();
        self.isSearchAlbumsByTags = ko.observable();
        self.isSearchAlbumsByDescription = ko.observable();

        self.isSearchUsersByName = ko.observable();
        self.isSearchUserByDepartment = ko.observable();

        self.isSearchByComments = ko.observable();

        self.search = function () {

            $.get("api/search", JSON.parse(ko.toJSON(self)));
        };
    }

    ko.applyBindings(new searchViewModel());
});