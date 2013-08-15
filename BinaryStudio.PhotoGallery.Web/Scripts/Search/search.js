$(document).ready(function() {

    var searchInterval = 10;

    function searchViewModel() {

        var self = this;

        self.searchCacheToken = "no token";

        self.begin = 0;
        self.end = searchInterval;

        self.foundItems = ko.observableArray();

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

            self.searchQuery($.trim(this.searchQuery()));

            if (this.searchQuery()) {

                $.get("api/search", JSON.parse(ko.toJSON(self)), function (searchResult) {
                    
                    self.searchCacheToken = searchResult.SearchCacheToken;

                    // adding search result items to observable array
                    $.each(searchResult.Items, function(index, value) {

                        self.foundItems.push(value);
                    });
                });
            }
        };
    }

    ko.applyBindings(new searchViewModel());
});