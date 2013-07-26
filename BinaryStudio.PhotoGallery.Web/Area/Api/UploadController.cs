using System.ComponentModel;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using AttributeRouting;
using AttributeRouting.Web.Mvc;

namespace BinaryStudio.PhotoGallery.Web.Area.Api
{
	[RoutePrefix("Api/Upload")]
    public class UploadController : ApiController
    {
        [POST]
        public HttpResponseMessage Index(Container containers, HttpPostedFileBase file)
        {

            return new HttpResponseMessage(HttpStatusCode.OK);
        }
    }
}
