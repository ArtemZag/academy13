using System.Web.Http;
using AttributeRouting;

namespace BinaryStudio.PhotoGallery.Web.Area.Api
{
    [Authorize]
	[RoutePrefix("Api/Album")]
    public class AlbumController : ApiController
    {
    }
}
