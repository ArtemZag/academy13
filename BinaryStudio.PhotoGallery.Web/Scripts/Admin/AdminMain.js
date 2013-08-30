$(function() {
    var userTakeCount = 12;
    
    var mediator = new Mediator;
    
    var avm = new AdminViewModel({takeCount: userTakeCount, mediator: mediator});

    $.get('api/user/all', { skip: 0, take: userTakeCount })
        .done(function (data) {
//            console.log(data);
            $.map(data, function (user) {
                avm.userList.push(new UserViewModel({
                    id: user.Id,
                    firstName: user.FirstName,
                    lastName: user.LastName,
                    email: user.Email,
                    department: user.Department,
                    isOnline: user.IsOnline,
                    isActivated: user.IsActivated,
                    isBlocked: user.IsBlocked,
                    avatartUrl: user.AvatartUrl,
                    profileUrl: user.ProfileUrl,
                    mediator: mediator
                }));
            });
        });

    ko.applyBindings(avm);
});