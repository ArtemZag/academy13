using System.Collections.Generic;
using System.Web.Mvc;
using AttributeRouting;
using AttributeRouting.Web.Mvc;
using BinaryStudio.PhotoGallery.Domain.Exceptions;
using BinaryStudio.PhotoGallery.Domain.Services;
using BinaryStudio.PhotoGallery.Models;
using BinaryStudio.PhotoGallery.Web.ViewModels.PhotoPage;

namespace BinaryStudio.PhotoGallery.Web.Controllers
{
    [Authorize]
    [RoutePrefix("PhotoComment")]
    public class PhotoCommentController : Controller
    {
        private readonly IPhotoCommentService _photoCommentService;
        private readonly IUserService _userService;

        public PhotoCommentController(IUserService userService, IPhotoCommentService photoCommentService)
        {
            _userService = userService;
            _photoCommentService = photoCommentService;
        }


        [POST]
        public ActionResult GetPhotoComments(int photoID, int begin, int end)
        {
            return Json(GetPhotoCommentsViewModel(_userService.GetUserId(User.Identity.Name), photoID, begin, end));
        }

        [POST]
        public ActionResult AddPhotoComment(NewCommentViewModel newCommentViewModel)
        {
            var newPhotoCommentModel = new PhotoCommentModel(_userService.GetUserId(User.Identity.Name),
                newCommentViewModel.PhotoID,
                newCommentViewModel.NewComment, newCommentViewModel.Reply);
            try
            {
                _photoCommentService.AddPhotoComment(_userService.GetUserId(User.Identity.Name), newPhotoCommentModel);
            }
            catch (NoEnoughPrivileges)
            {
                // Return information about this misstake to UI
                throw;
            }


            // Needs refactoring
            // begin = 0 last = 100
            return
                Json(GetPhotoCommentsViewModel(_userService.GetUserId(User.Identity.Name), newCommentViewModel.PhotoID,
                    0, 100));
        }


        /// <summary>
        ///     Gets photo comments and converts them to PhotoCommentViewModel
        /// </summary>
        private List<PhotoCommentViewModel> GetPhotoCommentsViewModel(int userID, int photoID, int begin, int end)
        {
            IEnumerable<PhotoCommentModel> photoCommentModels;
            var photoCommentViewModels = new List<PhotoCommentViewModel>();

            try
            {
                photoCommentModels = _photoCommentService.GetPhotoComments(userID, photoID, begin, end);
            }
            catch (NoEnoughPrivileges)
            {
                // TODO Return information about this misstake to UI
                throw;
            }

            foreach (PhotoCommentModel photoCommentModel in photoCommentModels)
            {
                UserModel userModel = _userService.GetUser(photoCommentModel.UserModelId);

                photoCommentViewModels.Add(PhotoCommentViewModel.FromModel(photoCommentModel, userModel));
            }

            return photoCommentViewModels;
        }
    }
}