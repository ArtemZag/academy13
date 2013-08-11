$(document).ready(function() {

    function searchViewModel() {

        var self = this;

        self.begin = 0;
        self.end = self.begin;

        self.searchQuery = ko.observable();

        self.isSearchPhotosByName = ko.observable(false);
        self.isSearchPhotosByTags = ko.observable(false);
        self.isSearchPhotosByDescription = ko.observable(false);

        self.isSearchAlbumsByName = ko.observable(false);
        self.isSearchAlbumsByTags = ko.observable(false);
        self.isSearchAlbumsByDescription = ko.observable(false);

        self.isSearchUsersByName = ko.observable(false);
        self.isSearchUserByDepartment = ko.observable(false);

        self.isSearchByComments = ko.observable(false);

        self.search = function () {
            
            $.get("api/search", JSON.parse(ko.toJSON(self)));
        };
    }

    ko.applyBindings(new searchViewModel());
});