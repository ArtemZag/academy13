using System.Web.Mvc;
using BinaryStudio.PhotoGallery.Web.CustomStructure;

namespace BinaryStudio.PhotoGallery.Web.Controllers
{
    public class BaseController : Controller
    {
        protected virtual new CustomPrincipal User
        {
            get { return HttpContext.User as CustomPrincipal; }
        }
    }
}
