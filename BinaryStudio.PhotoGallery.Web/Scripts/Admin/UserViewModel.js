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
        // TODO modal editor
    };

    self.blockAction = function () {
        if (self.isBlocked()) {
            $.post('/api/admin/unblock', { '': userId })
                .done(function () {
                    self.isBlocked(false);
                });
        } else {
            $.post('/api/admin/block', { '': userId })
                .done(function() {
                    self.isBlocked(true);
                });
        }
    };

    self.deleteAction = function (element) {
        // TODO modal dialog
        $.ajax({
            type: 'DELETE',
            url: '/api/admin/delete/' + userId
        }).done(function() {
                mediator.publish('admin:deleteUser', element);
            });
    };

    self.resetPassword = function () {
        
    };

    self.resetInvitation = function() {
        $.post('/api/invite', { Email: self.email(), FirstName: self.firstName(), Second: self.lastName() })
            .done(function() {
                console.log("Invite successfully sended");
                // TODO modal popup
            });
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

    self.blockAndUnblockText = ko.computed({
        read: function() {
            if (self.isBlocked()) {
                return "Unblock";
            } else {
                return "Block";
            }
        }
    });

    self.invitationAndPasswordText = ko.computed({
        read: function () {
            if (self.isActivated() && !self.isBlocked()) {
                return "Reset password";
            } else {
                return "Send invitation";
            }
        }
    });
}