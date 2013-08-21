using System.Web;
using System.Web.Http;

namespace BinaryStudio.PhotoGallery.Web.Area.Api
{
    public class BaseApiController : ApiController
    {
        protected virtual new CustomPrincipal User
        {
            get { return HttpContext.Current.User as CustomPrincipal; }
        }
    }
}