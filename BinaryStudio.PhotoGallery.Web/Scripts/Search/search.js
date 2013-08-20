$(document).ready(function () {

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

        searchViewModel.prototype.toJSON = function () {

            var copy = ko.toJS(self);

            delete copy.foundItems;

            return copy;
        };

        self.searchQuery.subscribe(function () {

            isModelChanged = true;
        });

        self.isSearchPhotosByTags.subscribe(function () {

            isModelChanged = true;
        });

        self.isSearchPhotosByDescription.subscribe(function () {

            isModelChanged = true;
        });

        self.isSearchAlbumsByName.subscribe(function () {

            isModelChanged = true;
        });

        self.isSearchAlbumsByTags.subscribe(function () {

            isModelChanged = true;
        });

        self.isSearchAlbumsByDescription.subscribe(function () {

            isModelChanged = true;
        });

        self.isSearchUsersByName.subscribe(function () {

            isModelChanged = true;
        });

        self.isSearchUserByDepartment.subscribe(function () {

            isModelChanged = true;
        });

        self.isSearchByComments.subscribe(function () {

            isModelChanged = true;
        });

        self.searchQuery.subscribe(function () {

            isModelChanged = true;
        });

        self.checkModelChange = function () {

            if (isModelChanged) {

                self.resetSearchResult();
            }

            isModelChanged = false;
        };

        self.resetSearchResult = function () {

            self.foundItems.removeAll();
            self.searchCacheToken = "no token";
        };

        self.search = function () {

            self.checkModelChange();

            self.searchQuery($.trim(self.searchQuery()));

            if (self.searchQuery() && self.foundItems().length == 0) {

                sendSearchRequest();
            }
        };
    }

    var viewModel = new searchViewModel();
    ko.applyBindings(viewModel);

    function sendSearchRequest() {

        $.get("api/search", JSON.parse(ko.toJSON(viewModel)), function (searchResult) {

            viewModel.searchCacheToken = searchResult.SearchCacheToken;

            addResultItems(searchResult.Items);
            resizeImages();
        });
    }

    function addResultItems(items) {

        $.each(items, function (index, value) {

            formatFields(value);

            viewModel.foundItems.push(value);
        });
    }

    // todo: delete

    function resizeImages() {

        setTimeout(function () {
            setImageSize();
        }, 900);
    }

    // transforms some fileds for result item

    function formatFields(value) {


        if (value.Type == "photo" || value.Type == "album" || value.Type == "comment") {

            value.DateOfCreation = formatDate(value.DateOfCreation);

            if (value.Type == "comment") {

                if (value.Text.length > 140) {
                    value.Text = value.Text.substring(0, 140) + "..";
                }
            }
        }
    }

    function formatDate(dateTime) {

        var dateEndIndex = dateTime.indexOf("T");
        var timeEndIndex = dateTime.lastIndexOf(":");

        var date = dateTime.substring(0, dateEndIndex);
        var time = dateTime.substring(dateEndIndex + 1, timeEndIndex);

        return date + " " + time;
    }

    // todo: delete

    function setImageSize() {

        $(".result-image").each(function () {

            var maxWidth = 180;
            var maxHeight = 180;
            var width = $(this).width();
            var height = $(this).height();

            var ratio;

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
            }
        });
    }

    $(window).scroll(function () {

        var totalHeight, currentScroll, visibleHeight;

        currentScroll = $(document).scrollTop();

        totalHeight = document.body.offsetHeight;

        visibleHeight = document.documentElement.clientHeight;

        // scroll to bottom event
        if (visibleHeight + currentScroll >= totalHeight) {
            sendSearchRequest();
        }
    });
});