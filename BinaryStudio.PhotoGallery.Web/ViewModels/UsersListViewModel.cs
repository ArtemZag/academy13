using System.Collections.Generic;

namespace BinaryStudio.PhotoGallery.Web.ViewModels
{
	public class UsersListViewModel
	{
		public IList<UserViewModel> UserViewModels { get; set; }
		public UserViewModel SelectedUser { get; set; }
	}
}