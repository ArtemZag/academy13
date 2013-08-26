using System.Web.Http;
using AttributeRouting;
using BinaryStudio.PhotoGallery.Domain.Services;
using BinaryStudio.PhotoGallery.Web.Filters;

namespace BinaryStudio.PhotoGallery.Web.Area.Api
{
    [AdminAuthorize]
	[RoutePrefix("api/admin")]
    public class AdminApiController : ApiController
    {
	    private readonly IUserService _userService;

	    public AdminApiController(IUserService userService)
	    {
	        _userService = userService;
	    }
    }
}
