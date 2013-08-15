$(document).ready(function() {

    function searchViewModel() {

        var self = this;

        self.searchCacheToken = "no token";

        self.interval = 10;

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

        searchViewModel.prototype.toJSON = function() {

            var copy = ko.toJS(this);

            delete copy.foundItems;

            return copy;
        };

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