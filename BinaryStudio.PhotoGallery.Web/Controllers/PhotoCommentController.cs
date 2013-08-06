using System.Collections.Generic;
using System.Web.Mvc;
using AttributeRouting;
using AttributeRouting.Web.Mvc;
using BinaryStudio.PhotoGallery.Domain.Services;
using BinaryStudio.PhotoGallery.Models;
using BinaryStudio.PhotoGallery.Web.Utils;
using BinaryStudio.PhotoGallery.Web.ViewModels.PhotoPage;

namespace BinaryStudio.PhotoGallery.Web.Controllers
{
    [Authorize]
    [RoutePrefix("PhotoComment")]
    public class PhotoCommentController : Controller
    {
        private readonly IPhotoCommentService _photoCommentService;
        private readonly IUserService _userService;
        private readonly IModelConverter _modelConverter;

        public PhotoCommentController(IUserService userService, IPhotoCommentService photoCommentService, IModelConverter modelConverter)
        {
            _userService = userService;
            _photoCommentService = photoCommentService;
            _modelConverter = modelConverter;
        }


        [POST]
        public ActionResult GetPhotoComments(int photoID, int begin, int last)
        {
            var photoCommentViewModel = new List<PhotoCommentViewModel>();
            var photoCommentModels = _photoCommentService.GetPhotoComments(photoID, begin, last);

            foreach (var photoCommentModel in photoCommentModels)
            {
                var userModel = _userService.GetUser(photoCommentModel.UserModelID);
                photoCommentViewModel.Add(_modelConverter.GetViewModel(photoCommentModel, userModel));
            }

            return Json(photoCommentViewModel);
        }
         
        [POST]
        public ActionResult AddPhotoComment(NewCommentViewModel newCommentViewModel)
        {
            var newPhotoCommentModel = new PhotoCommentModel(_userService.GetUserId(User.Identity.Name), newCommentViewModel.PhotoID,
                                                          newCommentViewModel.NewComment, newCommentViewModel.Reply);

            _photoCommentService.AddPhotoComment(newPhotoCommentModel);


            // Needs refactoring
            //bgein = 0 last = 100
            var photoCommentViewModel = new List<PhotoCommentViewModel>();
            var photoCommentModels = _photoCommentService.GetPhotoComments(newCommentViewModel.PhotoID, 0, 100);

            foreach (var photoCommentModel in photoCommentModels)
            {
                var userModel = _userService.GetUser(photoCommentModel.UserModelID);
                photoCommentViewModel.Add(_modelConverter.GetViewModel(photoCommentModel, userModel));
            }

            return Json(photoCommentViewModel);
        }
    }
    
}
