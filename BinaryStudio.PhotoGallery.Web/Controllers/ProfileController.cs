using System.Web.Mvc;
using AttributeRouting;
using AttributeRouting.Web.Mvc;
using BinaryStudio.PhotoGallery.Domain.Services;
using BinaryStudio.PhotoGallery.Web.ViewModels;
using BinaryStudio.PhotoGallery.Web.Infrastructure.Extensions;

namespace BinaryStudio.PhotoGallery.Web.Controllers
{
    [Authorize]
    [RoutePrefix("profile")]
    public class ProfileController : BaseController
    {
        private IUserService UserService { get; set; }

        public ProfileController(IUserService userService)
        {
            UserService = userService;
        }

        [GET("")]
        public ActionResult Index()
        {
            var user = UserService.GetUser(User.Id);
            return View(UserViewModel.ToViewModel(user));
        }
        
        [GET("edit")]
        public ActionResult Edit()
        {
            var user = UserService.GetUser(User.Id);
            return View(UserViewModel.ToViewModel(user));
        }

        [POST("edit")]
        public ActionResult Edit(UserViewModel userViewModel)
        {
            var user = UserService.GetUser(User.Id);

            if(user == null)
            {
                this.AddCriticalError(string.Format("User with email {0} was not found", User.Identity.Name));
                return View(userViewModel);
            }

            user.FirstName = userViewModel.FirstName;
            user.LastName = userViewModel.LastName;

            UserService.Update(user);

            return View("Index", userViewModel);
        }
    }
}
