using BinaryStudio.PhotoGallery.Models;

namespace BinaryStudio.PhotoGallery.Web.ViewModels
{
	public class UserViewModel
	{
		public string FirstName { get; set; }

		public string LastName { get; set; }

		public string Email { get; set; }

		public bool IsAdmin { get; set; }

		public int AlbumsCount { get; set; }

		public bool IsOnline { get; set; }
	}
}