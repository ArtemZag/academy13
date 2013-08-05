using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AttributeRouting;
using AttributeRouting.Web.Mvc;
using BinaryStudio.PhotoGallery.Database;
using BinaryStudio.PhotoGallery.Domain.Services;
using BinaryStudio.PhotoGallery.Web.ViewModels;

namespace BinaryStudio.PhotoGallery.Web.Controllers
{
	[RoutePrefix("AdminPanel")]
	public class AdminPanelController : Controller
	{
		private readonly IUserService userService;
		private readonly IUsersMonitorTask monitorService;

		public AdminPanelController(IUserService userService, IUsersMonitorTask monitorTask)
		{
			this.userService = userService;
			this.monitorService = monitorTask;
		}

		/// <summary>
		/// Administration page
		/// </summary>
		[GET("")]
		public ActionResult Index()
		{
			var users = this.userService.GetAllUsers().ToList();
			var extendedUserList = users.Select(user => new UserViewModel
				{
					IsOnline = this.monitorService.IsOnline(user.Email), 
					FirstName = user.FirstName, 
					LastName = user.LastName,
					Email = user.Email,
					AlbumsCount = user.Albums.Count
				}).ToList();
			return View(new UsersListViewModel { UserViewModels = extendedUserList});
		}

	}
}
