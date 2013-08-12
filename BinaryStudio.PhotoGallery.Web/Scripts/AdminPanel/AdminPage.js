var Bingally = Bingally || {};

Bingally.AdminPage = Bingally.AdminPage || {};


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
		};

	}
	ko.applyBindings(instanceViewModel);
};



