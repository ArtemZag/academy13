function AdminViewModel(options) {
    var self = this;

    self.userList = ko.observableArray([]);

    self.takeCount = ko.observable(options.takeCount);

    self.getMoreUsers = function () {
        
    };
    
    var mediator = options.mediator;

    mediator.subscribe("admin:deleteUser", function(element) {
        self.userList.remove(element);
    });

    self.inviteUser = function (data) {
        console.log(data);
//        $.post('api/admin/invite', {});
    };
}