using System.Web.Mvc;
using AttributeRouting.Web.Mvc;
using BinaryStudio.PhotoGallery.Web.ViewModels.Search;

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