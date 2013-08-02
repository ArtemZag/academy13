using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AttributeRouting;
using AttributeRouting.Web.Mvc;
using BinaryStudio.PhotoGallery.Domain.Services;
using BinaryStudio.PhotoGallery.Models;
using BinaryStudio.PhotoGallery.Web.Utils;

namespace BinaryStudio.PhotoGallery.Web.Controllers
{
    [Authorize]
    [RoutePrefix("Album")]
    public class PhotoController : Controller
    {
        private IPhotoService _photoService;
        private IUserService _userService;

        public PhotoController(IUserService userService, IPhotoService photoService)
        {
            _userService = userService;
            _photoService = photoService;
        }


        [HttpPost]
        public ActionResult GetPhotos(string albumName, int begin, int last)
        {
            var photoModels = _photoService.GetPhotos(User.Identity.Name, albumName, begin, last);
          

            return Json(photoModels.Select(ModelConverter.GetViewModel).ToList());
        }

        [GET("photo{photoID}")]
        public ActionResult Index(string photoID)
        {

            return View();
        }



    }
}
