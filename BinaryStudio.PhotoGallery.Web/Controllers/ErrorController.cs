using System.Web.Mvc;
using AttributeRouting;
using AttributeRouting.Web.Mvc;

namespace BinaryStudio.PhotoGallery.Web.Controllers
{
	[RoutePrefix("")]
    public class ErrorController : BaseController
    {
		[GET("notfound")]
        public ActionResult NotFound()
		{
		    Response.StatusCode = 404;
            return View();
        }
    }
}
