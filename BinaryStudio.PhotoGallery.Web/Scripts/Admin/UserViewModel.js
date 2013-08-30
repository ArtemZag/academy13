function UserViewModel(options) {
    var self = this;

    var mediator = options.mediator;

    var userId = options.id;
    
    self.firstName = ko.observable(options.firstName);
    self.lastName = ko.observable(options.lastName);
    self.email = ko.observable(options.email);
    self.department = ko.observable(options.department);

    self.isOnline = ko.observable(options.isOnline || false);
    self.isActivated = ko.observable(options.isActivated || false);
    self.isBlocked = ko.observable(options.isBlocked || false);

    self.avatarUrl = ko.observable(options.avatarUrl);

    self.profileUrl = ko.observable(options.profileUrl);

    self.editAction = function(element) {
        
    };

    self.blockAction = function() {

    };

    self.deleteAction = function (element) {
        
        $.ajax({
            type: 'DELETE',
            url: '/api/user/' + userId
        });
        mediator.publish("admin:deleteUser", element);
    };

    self.resetPassword = function() {

    };

    self.resetInvitation = function() {

    };

    self.fullName = ko.computed({
        read: function() {
            return self.firstName() + " " + self.lastName();
        }
    });

    self.statusCss = ko.computed({
        read: function() {
            if (self.isBlocked()) {
                return "status-blocked";
            } else if (!self.isActivated()) {
                return "status-unactivated";
            } else {
                if (self.isOnline()) {
                    return "status-online";
                } else {
                    return "status-offline";
                }
            }
        }
    });

    self.statusText = ko.computed({
        read: function() {
            if (self.isBlocked()) {
                return "Blocked";
            } else if (!self.isActivated()) {
                return "Unactivated";
            } else {
                if (self.isOnline()) {
                    return "Online";
                } else {
                    return "Offline";
                }
            }
        }
    });

    self.invitationAndPasswordText = ko.computed({
        read: function () {
            if (self.isBlocked()) {
                return "Send invitation";
            } else {
                return "Reset password";
            }
        }
    });
}