$(document).ready(function() {

    function appendSearch() {

    }

    function searchViewModel() {

        var self = this;

        self.SearchType = ko.observable("Photos");
        self.SearchQuery = ko.observable();


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
})