$(document).ready(function() {

    function searchViewModel() {

        var self = this;

        var isModelChanged = false;

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

            var copy = ko.toJS(self);

            delete copy.foundItems;

            return copy;
        };

        self.searchQuery.subscribe(function() {

            isModelChanged = true;
        });

        self.isSearchPhotosByName.subscribe(function () {
            
            isModelChanged = true;
        });

        self.isSearchPhotosByTags.subscribe(function() {

            isModelChanged = true;
        });

        self.isSearchPhotosByDescription.subscribe(function() {

            isModelChanged = true;
        });

        self.isSearchAlbumsByName.subscribe(function() {

            isModelChanged = true;
        });

        self.isSearchAlbumsByTags.subscribe(function() {

            isModelChanged = true;
        });

        self.isSearchAlbumsByDescription.subscribe(function() {

            isModelChanged = true;
        });

        self.isSearchUsersByName.subscribe(function() {

            isModelChanged = true;
        });

        self.isSearchUserByDepartment.subscribe(function() {

            isModelChanged = true;
        });

        self.isSearchByComments.subscribe(function() {

            isModelChanged = true;
        });

        self.searchQuery.subscribe(function() {

            isModelChanged = true;
        });

        self.resetSearchResult = function() {

            self.foundItems.removeAll();
            self.searchCacheToken = "no token";
        };

        self.checkModelChange = function() {

            if (isModelChanged) {

                self.resetSearchResult();
            }

            isModelChanged = false;
        };

        self.search = function() {

            self.checkModelChange();

            self.searchQuery($.trim(self.searchQuery()));

            if (self.searchQuery()) {

                $.get("api/search", JSON.parse(ko.toJSON(self)), function(searchResult) {

                    self.searchCacheToken = searchResult.SearchCacheToken;

                    // adding search result items to observable array
                    $.each(searchResult.Items, function(index, value) {

                        formatFields(value);
                        
                        self.foundItems.push(value);
                    });

                    // change images size
                    setImageSize();
                });
            }
        };
    }

    ko.applyBindings(new searchViewModel());

    function formatFields(value) {
        
        // date getting 
        if (value.Type == "photo") {
            var dateEndIndex = value.DateOfCreation.indexOf("T");
            value.DateOfCreation = value.DateOfCreation.substr(0, dateEndIndex);
        }
    }

    function setImageSize() {

        $(".result-image").each(function() {

            var maxWidth = 180;
            var maxHeight = 180;
            var ratio = 0;
            var width = $(this).width();
            var height = $(this).height();

            if (width > maxWidth) {

                ratio = maxWidth / width;
                $(this).css("width", maxWidth);
                $(this).css("height", height * ratio);
                height = height * ratio;
                width = width * ratio;
            }

            if (height > maxHeight) {

                ratio = maxHeight / height;
                $(this).css("height", maxHeight);
                $(this).css("width", width * ratio);
                width = width * ratio;
            }
        });
    }
});