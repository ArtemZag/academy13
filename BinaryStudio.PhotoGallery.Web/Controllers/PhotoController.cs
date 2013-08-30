using System.Web.Mvc;
using AttributeRouting;
using AttributeRouting.Web.Mvc;
using BinaryStudio.PhotoGallery.Core;
using BinaryStudio.PhotoGallery.Core.SocialNetworkUtils.Facebook;

namespace BinaryStudio.PhotoGallery.Web.Controllers
{
    [Authorize]
    [RoutePrefix("photo")]
    public class PhotoController : BaseController
    {
        [GET("{photoId:int}")]
        public ActionResult Index(int photoId)
        {
            ViewBag.PhotoID = photoId;
            return View();
        }
    }
}