$(document).ready(function () {

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

        searchViewModel.prototype.toJSON = function () {

            var copy = ko.toJS(self);

            delete copy.foundItems;

            return copy;
        };

        self.searchQuery.subscribe(function () {

            self.resetToken();
        });

        self.isSearchPhotosByName.subscribe(function () {

            self.resetToken();
        });

        self.isSearchPhotosByTags.subscribe(function () {

            self.resetToken();
        });

        self.isSearchPhotosByDescription.subscribe(function () {

            self.resetToken();
        });

        self.isSearchAlbumsByName.subscribe(function () {

            self.resetToken();
        });

        self.isSearchAlbumsByTags.subscribe(function () {

            self.resetToken();
        });

        self.isSearchAlbumsByDescription.subscribe(function () {

            self.resetToken();
        });

        self.isSearchUsersByName.subscribe(function () {

            self.resetToken();
        });

        self.isSearchUserByDepartment.subscribe(function () {

            self.resetToken();
        });

        self.isSearchByComments.subscribe(function () {

            self.resetToken();
        });

        self.searchQuery.subscribe(function () {

            self.foundItems.removeAll();
            self.resetToken();
        });

        self.resetToken = function () {

            self.searchCacheToken = "no token";
        };

        self.search = function () {

            self.searchQuery($.trim(self.searchQuery()));

            if (self.searchQuery()) {

                $.get("api/search", JSON.parse(ko.toJSON(self)), function (searchResult) {

                    self.searchCacheToken = searchResult.SearchCacheToken;

                    // adding search result items to observable array
                    $.each(searchResult.Items, function (index, value) {

                        // date getting 
                        var dateEndIndex = value.DateOfCreation.indexOf("T");
                        value.DateOfCreation = value.DateOfCreation.substr(0, dateEndIndex);

                        self.foundItems.push(value);
                    });

                    // change images size
                    setImageSize();
                });
            }
        };
    }

    ko.applyBindings(new searchViewModel());

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
});