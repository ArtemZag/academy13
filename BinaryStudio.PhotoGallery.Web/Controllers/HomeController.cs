using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AttributeRouting;
using AttributeRouting.Web.Mvc;
using BinaryStudio.PhotoGallery.Domain.Services;
using BinaryStudio.PhotoGallery.Models;
using BinaryStudio.PhotoGallery.Web.Utils;
using BinaryStudio.PhotoGallery.Web.ViewModels;

namespace BinaryStudio.PhotoGallery.Web.Controllers
{
    [Authorize] // Only authorized users can access this controller
    [RoutePrefix("Home")]
    public class HomeController : Controller
    {
        private readonly IPhotoService photoService;
        private readonly IUserService userService;

        public HomeController(IUserService userService, IPhotoService photoService)
        {
            this.photoService = photoService;
            this.userService = userService;
        }

        /// <summary>
        ///     Main user page (click on "bingally")
        /// </summary>
        /// <returns>page with flow of public pictures</returns>
        [GET("Index/{photoNum}")]
        public ActionResult Index()
        {
            IEnumerable<PhotoModel> viewmodels = photoService.GetPhotos(User.Identity.Name, 0, 30);
            return View(new InfoViewModel
                {
                    UserEmail = User.Identity.Name,
                    Photos = viewmodels.Select(ModelConverter.TestGetViewModel).ToList()
                });
        }

        //[POST("GetPhotosViaAjax")]
        //public ActionResult GetPhotosViaAjax(int startIndex = 0, int endIndex = 30)
        //{
        //    gModel.Photos.AddRange(_photoService.GetPhotos(User.Identity.Name, startIndex, endIndex)
        //                                        .Select(ModelConverter.GetViewModel).ToList());
        //    gModel.PortionSubmit = true;
        //    return Json(gModel);
        //}

        [HttpPost]
        public ActionResult GetPhotosViaAjax(int startIndex, int endIndex)
        {
            IEnumerable<PhotoViewModel> photos = photoService.GetPhotos(User.Identity.Name, startIndex, endIndex)
                                                              .Select(ModelConverter.TestGetViewModel);
            return Json(photos);
        }

        [GET("ToPhoto/{albumId}/{photoId}")]
        public ActionResult ToPhoto(int albumId, int photoId)
        {
            return View("Album", new AlbumViewModel());
        }

        /// <summary>
        ///     Gallery page
        /// </summary>
        /// <returns>page with all users photos, sorted by date</returns>
        [GET("Gallery")]
        public ActionResult Gallery()
        {
            return View();
        }

        /// <summary>
        ///     Album page
        /// </summary>
        /// <returns>page with all users albums</returns>
        [GET("Albums")]
        public ActionResult Albums()
        {
            return View();
        }

        /// <summary>
        ///     Gruops page
        /// </summary>
        /// <returns>page with all users groups</returns>
        [GET("Groups")]
        public ActionResult Groups()
        {
            return View();
        }
    }
}