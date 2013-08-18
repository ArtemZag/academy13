using System.Web.Mvc;
using AttributeRouting;
using AttributeRouting.Web.Mvc;
using BinaryStudio.PhotoGallery.Core;
using BinaryStudio.PhotoGallery.Core.SocialNetworkUtils.Facebook;

namespace BinaryStudio.PhotoGallery.Web.Controllers
{
    [Authorize]
    [RoutePrefix("Photo")]
    public class PhotoController : Controller
    {
        [HttpPost]
        public ActionResult FbSync(string photoID)
        {
            /* var photoModel = _photoService.GetPhoto(Int32.Parse(photoID));
            var photoPath = new List<string>();
*/
            return Redirect(FB.CreateAuthURL(Randomizer.GetString(16)));
            //FB.AddPhotosToAlbum(photoPath,"MakTest",);
        }

        [GET("{photoId}")]
        public ActionResult Index(string photoId)
        {
            ViewBag.PhotoID = photoId;
            return View();
        }
    }
}