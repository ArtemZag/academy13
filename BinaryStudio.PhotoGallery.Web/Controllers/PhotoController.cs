using System.Web.Mvc;
using AttributeRouting;
using AttributeRouting.Web.Mvc;
using BinaryStudio.PhotoGallery.Core;
using BinaryStudio.PhotoGallery.Core.SocialNetworkUtils.Facebook;

namespace BinaryStudio.PhotoGallery.Web.Controllers
{
    [Authorize]
    [RoutePrefix("photo")]
    public class PhotoController : Controller
    {
        [POST("facebook/{photoId:int}")]
        public ActionResult FbSync(int photoId)
        {
            /* var photoModel = _photoService.GetPhoto(Int32.Parse(photoId));
            var photoPath = new List<string>();
*/
            return Redirect(FB.CreateAuthURL(Randomizer.GetString(16)));
            //FB.AddPhotosToAlbum(photoPath,"MakTest",);
        }

        [GET("{photoId:int}")]
        public ActionResult Index(int photoId)
        {
            ViewBag.PhotoID = photoId;
            return View();
        }
    }
}