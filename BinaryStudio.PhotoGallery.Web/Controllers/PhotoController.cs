using System;
using System.Web.Mvc;
using AttributeRouting;
using AttributeRouting.Web.Mvc;
using BinaryStudio.PhotoGallery.Core;
using BinaryStudio.PhotoGallery.Core.SocialNetworkUtils.Facebook;
using BinaryStudio.PhotoGallery.Domain.Exceptions;
using BinaryStudio.PhotoGallery.Domain.Services;

namespace BinaryStudio.PhotoGallery.Web.Controllers
{
    [Authorize]
    [RoutePrefix("photo")]
    public class PhotoController : BaseController
    {
		private readonly IPhotoService _photoService;

		public PhotoController(IPhotoService photoService)
        {
            _photoService = photoService;
        }

        [POST("facebook/{photoId:int}")]
        public ActionResult FbSync(int photoId)
        {
            /* var photoModel = _photoService.GetPhoto(Int32.Parse(photoId));
            var photoPath = new List<string>();
*/
            return Redirect(FB.CreateAuthUrl(Randomizer.GetString(16)));
            //FB.AddPhotosToAlbum(photoPath,"MakTest",);
        }

        [GET("{photoId:int}")]
        public ActionResult Index(int photoId)
        {
	        try
	        {
				// This way we can know about privileges of the User
		        _photoService.GetPhoto(User.Id, photoId);
		        ViewBag.PhotoID = photoId;
		        ViewBag.UserID = User.Id;
		        return View();
	        }
			catch (NoEnoughPrivilegesException )
	        {
				return RedirectToAction("accessdenied", "Error");
	        }
        }
    }
}