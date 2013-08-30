using System;

namespace BinaryStudio.PhotoGallery.Web.ViewModels.User
{
    public class UserViewModel : BaseViewModel
	{
        public int Id { get; set; }
        public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Email { get; set; }
        public string Department { get; set; }
		public bool IsAdmin { get; set; }
		public int AlbumsCount { get; set; }
        public int PhotoCount { get; set; }
		public bool IsOnline { get; set; }
        public bool IsActivated { get; set; }
        public bool IsBlocked { get; set; }

	    public string AvatarUrl { get; set; }
        public string ProfileUrl { get; set; }
        public DateTime Birthday { get; set; }
	}
}