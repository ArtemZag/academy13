using System.Web.Mvc;
using AttributeRouting.Web.Mvc;
using BinaryStudio.PhotoGallery.Domain.Services;
using BinaryStudio.PhotoGallery.Web.ViewModels;

namespace BinaryStudio.PhotoGallery.Web.Controllers
{
    public class SearchController : Controller
    {
        [GET("Search")]
        public ActionResult Search(string searchQuery)
        {
            return View(new SearchViewModel
                {
                    SearchQuery = searchQuery
                });
        }
    }
}