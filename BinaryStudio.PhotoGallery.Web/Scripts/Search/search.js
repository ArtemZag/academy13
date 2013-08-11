$(document).ready(function() {

    function searchViewModel() {

        var interval = 10;

        var self = this;

        self.cacheToken = "no token";

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

        self.search = function() {

            this.searchQuery($.trim(this.searchQuery()));
            
            if (this.searchQuery()) {

                $.get("api/search", JSON.parse(ko.toJSON(self)), function(data) {
                    console.log(data);
                });
            }
        };
    }

    ko.applyBindings(new searchViewModel());
});