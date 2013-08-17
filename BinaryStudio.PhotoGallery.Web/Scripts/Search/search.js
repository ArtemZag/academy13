$(document).ready(function() {

    function searchViewModel() {

        var self = this;

        var isModelChanged = false;

        self.searchCacheToken = "no token";

        self.interval = 10;

        self.foundItems = ko.observableArray();

        self.searchQuery = ko.observable();

        self.isSearchPhotosByTags = ko.observable(true);
        self.isSearchPhotosByDescription = ko.observable(true);

        self.isSearchAlbumsByName = ko.observable(true);
        self.isSearchAlbumsByTags = ko.observable(true);
        self.isSearchAlbumsByDescription = ko.observable(true);

        self.isSearchUsersByName = ko.observable(true);
        self.isSearchUserByDepartment = ko.observable(true);

        self.isSearchByComments = ko.observable(true);

        searchViewModel.prototype.toJSON = function() {

            var copy = ko.toJS(self);

            delete copy.foundItems;

            return copy;
        };

        self.searchQuery.subscribe(function() {

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

                    setTimeout(function() {
                        setImageSize();
                    }, 900);
                });
                
                function setImageSize() {

                    $(".result-image").each(function () {

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
            }
        };
        
        function formatFields(value) {

            // date getting 
            if (value.Type == "photo") {
                var dateEndIndex = value.DateOfCreation.indexOf("T");
                value.DateOfCreation = value.DateOfCreation.substr(0, dateEndIndex);
            }
        }
    }

    ko.applyBindings(new searchViewModel());
    
});