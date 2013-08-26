using System;

namespace BinaryStudio.PhotoGallery.Web.ViewModels
{
    public class UserViewModel : BaseViewModel
	{
        public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Email { get; set; }
		public bool IsAdmin { get; set; }
		public int AlbumsCount { get; set; }
		public bool IsOnline { get; set; }
	    public string PhotoUrl { get; set; }
        public DateTime Birthday { get; set; }
	}
}