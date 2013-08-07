$(document).ready(function() {

    function searchViewModel() {

        var self = this;

        self.searchType = ko.observable("Photos");
        self.searchQuery = ko.observable();


        self.interval = 10;
        self.begin = 0;
        self.end = self.begin + self.interval;

        self.search = function() {

            switch (self.SearchType) {
            case "Photos":
                // clear div data
                    // build string
                break;
            default:
            }
        };

        self.appendSearch = function() {

        };
    }

    ko.applyBindings(searchViewModel);
});