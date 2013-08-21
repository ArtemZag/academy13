using System.Web.Mvc;

namespace BinaryStudio.PhotoGallery.Web
{
    public class BaseController : Controller
    {
        protected virtual new CustomPrincipal User
        {
            get { return HttpContext.User as CustomPrincipal; }
        }
    }
}
