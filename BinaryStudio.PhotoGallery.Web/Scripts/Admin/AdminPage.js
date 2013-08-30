var Bingally = Bingally === undefined ? {} : Bingally;

Bingally.AdminPage.initializeViewModel = function (initData) {
	var instanceViewModel = new adminPageViewModel();
	$.each(initData.UserViewModels, function (index, user) {
		instanceViewModel.Users.push(user);
	});
	instanceViewModel.SetCurrentUser(instanceViewModel.Users()[0]);
	
	
	function adminPageViewModel() {
		var self = this;

		self.Users = ko.observableArray();

		self.SelectedUser = ko.observable();

		self.SetCurrentUser = function (user) {
			self.SelectedUser(user);
			
			// TODO: Fix this function.
			$('#deleteButton').on('click', function () {
				var eMail = self.SelectedUser().Email;
				$.ajax({
					type: "DELETE",
					url: "/user/5",
					data: { eMail: eMail}
				}).done(function() {
					alert("User has been deleted!");
				}).fail(function() {
					alert("Server Error!");
				});
			});
		};
	}
	
	$('#invite-form').on('submit', function (e) {
		e.preventDefault();

		if (!$('form').valid()) {
			return false;
		}

		var invitedUser = {
			Email: $('#inviteEmail').val(),
			FirstName: $('#inviteFirstname').val(),
			LastName: $('#inviteLastname').val()
		};

		$.post("/AdminPanel/SendInvite", invitedUser)
			.done(function() {
				alert("Invite was sent");
			});
	});
	
	ko.applyBindings(instanceViewModel);
	
	
};

$(function () {

	$('#invite-form').validate({
		rules: {
			inviteEmail: {
				required: true,
				email: true
			},
			inviteFirstname: {
				required: true
			},
			inviteLastname: {
				required: true
			}
		},
		messages: {
			inviteEmail: {
				required: "Please enter e-mail address."
			},
			inviteFirstname: {
				required: "Please enter the first name."
			},
			inviteLastname: {
				required: "Please enter the last name."
			}
		}
	});

	
});

