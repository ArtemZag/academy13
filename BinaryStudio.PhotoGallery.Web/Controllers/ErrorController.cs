using System;
using System.Web.Mvc;
using AttributeRouting;
using AttributeRouting.Web.Mvc;
using BinaryStudio.PhotoGallery.Web.Extensions;

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

	    [GET("accessdenied")]
	    public ActionResult AccessDenied()
	    {
	        Response.StatusCode = 403;
	        return View();
	    }

        [GET("error")]
        public ActionResult Error(Exception exception)
        {
            this.AddCriticalError("An unhandled exception occured: " + exception.Message);
            return View("Error", exception);
        }
    }
}
