var Bingally = Bingally || {};

Bingally.AdminPage = Bingally.AdminPage || {};

function AdminPageViewModel() {
	var self = this;

	self.Users = ko.observableArray();

	self.SelectedUser = ko.observable();

	self.SetCurrentUser = function(user) {
		self.SelectedUser(user);
	};

}

Bingally.AdminPage.initializeViewModel = function (initData) {
	var instanceViewModel = new AdminPageViewModel();
	$.each(initData.UserViewModels, function (index, user) {
		instanceViewModel.Users.push(user);
	});
	ko.applyBindings(instanceViewModel);
};



