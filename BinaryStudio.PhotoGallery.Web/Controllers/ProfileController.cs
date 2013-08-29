using System;
using System.Web;
using System.Web.Mvc;
using AttributeRouting;
using AttributeRouting.Web.Mvc;
using BinaryStudio.PhotoGallery.Core.PathUtils;
using BinaryStudio.PhotoGallery.Core.PhotoUtils;
using BinaryStudio.PhotoGallery.Domain.Services;
using BinaryStudio.PhotoGallery.Models;
using BinaryStudio.PhotoGallery.Web.Extensions;
using BinaryStudio.PhotoGallery.Web.Extensions.ViewModels;
using BinaryStudio.PhotoGallery.Web.ViewModels.User;

namespace BinaryStudio.PhotoGallery.Web.Controllers
{
    [Authorize]
    [RoutePrefix("profile")]
    public class ProfileController : BaseController
    {
        private readonly IUserService _userService;
        private readonly IPhotoProcessor _photoProcessor;
        private readonly IPathUtil _pathUtil;

        public ProfileController(IUserService userService, IPhotoProcessor photoProcessor, IPathUtil pathUtil)
        {
            _userService = userService;
            _photoProcessor = photoProcessor;
            _pathUtil = pathUtil;
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

        [POST("updatePhoto")]
        public ActionResult UpdatePhoto(HttpPostedFileBase photoFile)
        {
            try
            {
                if ((photoFile != null) && (photoFile.ContentLength > 0))
                {
                    var filePath = _pathUtil.BuildAbsoluteAvatarPath(User.Id, ImageSize.Original);
                    photoFile.SaveAs(filePath);
                    _photoProcessor.CreateAvatarThumbnails(User.Id);
                    return RedirectToAction("Edit");
                }
            }
            catch (Exception e)
            {
                this.AddCriticalError(e.Message);
                return RedirectToAction("Edit");
            }

            this.AddWarningError("Something wrong with uploaded file. Please try another file.");
            return RedirectToAction("Edit");
        }
    }
}