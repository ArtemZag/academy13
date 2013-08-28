$(document).ready(function() {

    $("#loader").hide();

    function searchViewModel() {

        var self = this;

        var isModelChanged = false;

        self.searchCacheToken = "no token";

        self.skip = 0;
        self.take = 10;

        self.foundItems = ko.observableArray();

        self.searchQuery = ko.observable();

        self.isSearchPhotosByTags = ko.observable(true);
        self.isSearchPhotosByDescription = ko.observable(true);

        self.isSearchAlbumsByName = ko.observable(true);
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

        self.checkModelChange = function() {

            if (isModelChanged) {

                self.resetSearchResult();
            }

            isModelChanged = false;
        };

        self.resetSearchResult = function() {

            self.foundItems.removeAll();
            self.searchCacheToken = "no token";
            self.skip = 0;
        };

        self.search = function() {

            self.checkModelChange();

            self.searchQuery($.trim(self.searchQuery()));

            if (self.searchQuery() && self.foundItems().length == 0) {

                sendSearchRequest();
            }
        };

        self.incrementInterval = function() {

            self.skip += self.take;
        };
    }

    var viewModel = new searchViewModel();
    ko.applyBindings(viewModel);

    // is request
    var busy = false;

    function sendSearchRequest() {

        $("#loader").show();

        $.get("api/search", JSON.parse(ko.toJSON(viewModel)))
            .done(function(searchResult) {

                viewModel.searchCacheToken = searchResult.SearchCacheToken;

                addResultItems(searchResult.Items);

                $("#loader").hide();
                busy = false;

                viewModel.incrementInterval();
            })
            .fail(function() {

                $("#loader").hide();
                busy = false;
            });
    }

    function addResultItems(items) {

        $.each(items, function(index, value) {

            formatFields(value);

            viewModel.foundItems.push(value);
        });
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

    $(window).scroll(function() {

        if (!busy) {
            busy = true;
            var scrHeight = $(window).height();
            var underScroll = $(this).scrollTop();
            var divHeight = $("#items").height();
            
            var scrollPosition = scrHeight + underScroll;

            if (divHeight - scrollPosition < 200 && viewModel.searchQuery()) {
                sendSearchRequest();
            } else {
                busy = false;
            }
        }
    });
});