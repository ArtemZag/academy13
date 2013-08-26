using System.Web.Mvc;
using AttributeRouting;
using AttributeRouting.Web.Mvc;
using BinaryStudio.PhotoGallery.Domain.Services;
using BinaryStudio.PhotoGallery.Models;
using BinaryStudio.PhotoGallery.Web.Extensions;
using BinaryStudio.PhotoGallery.Web.Extensions.ViewModels;
using BinaryStudio.PhotoGallery.Web.ViewModels;

namespace BinaryStudio.PhotoGallery.Web.Controllers
{
    [Authorize]
    [RoutePrefix("profile")]
    public class ProfileController : BaseController
    {
        private readonly IUserService _userService;

        public ProfileController(IUserService userService)
        {
            _userService = userService;
        }

        [GET("")]
        public ActionResult Index()
        {
            UserModel user = _userService.GetUser(User.Id);
            return View(user.ToUserViewModel());
        }

        [GET("edit")]
        public ActionResult Edit()
        {
            UserModel user = _userService.GetUser(User.Id);
            return View(user.ToUserViewModel());
        }

        [POST("edit")]
        public ActionResult Edit(UserViewModel userViewModel)
        {
            UserModel user = _userService.GetUser(User.Id);

            if (user == null)
            {
                this.AddCriticalError(string.Format("User with email {0} was not found", User.Email));
                return View(userViewModel);
            }

            user.FirstName = userViewModel.FirstName;
            user.LastName = userViewModel.LastName;
            user.Email = userViewModel.Email;

            _userService.Update(user);

            return View("Index", user.ToUserViewModel());
        }
    }
}