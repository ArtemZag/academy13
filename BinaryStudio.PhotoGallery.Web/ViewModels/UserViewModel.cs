using System;

namespace BinaryStudio.PhotoGallery.Web.ViewModels
{
    public class UserViewModel : BaseViewModel
	{
        public int Id { get; set; }
        public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Email { get; set; }
		public bool IsAdmin { get; set; }
		public int AlbumsCount { get; set; }
        public int PhotoCount { get; set; }
		public bool IsOnline { get; set; }
	    public string PhotoUrl { get; set; }
        public DateTime Birthday { get; set; }
	}
}