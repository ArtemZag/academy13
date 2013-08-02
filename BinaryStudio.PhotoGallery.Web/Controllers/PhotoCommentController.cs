using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AttributeRouting;
using AttributeRouting.Web.Mvc;
using BinaryStudio.PhotoGallery.Domain.Services;
using BinaryStudio.PhotoGallery.Web.Utils;
using BinaryStudio.PhotoGallery.Web.ViewModels.PhotoPage;

namespace BinaryStudio.PhotoGallery.Web.Controllers
{
    [Authorize]
    [RoutePrefix("PhotoComment")]
    public class PhotoCommentController : Controller
    {
        private IPhotoCommentService _photoCommentService;
        private IUserService _userService;

        public PhotoCommentController(IUserService userService, IPhotoCommentService photoCommentService)
        {
            _userService = userService;
            _photoCommentService = photoCommentService;
        }


        [HttpPost]
        public ActionResult GetPhotoComments(int photoID, int begin, int last)
        {
            var photoCommentViewModel = new List<PhotoCommentViewModel>();
            var photoCommentModels = _photoCommentService.GetPhotoComments(photoID, begin, last);

            foreach (var photoCommentModel in photoCommentModels)
            {
                var userModel = _userService.GetUser(photoCommentModel.UserModelID);
                photoCommentViewModel.Add(ModelConverter.GetViewModel(photoCommentModel,userModel));
            }

            return Json(photoCommentViewModel);
        }
    }
}
