using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AttributeRouting;
using AttributeRouting.Web.Http;
using BinaryStudio.PhotoGallery.Domain.Services;
using BinaryStudio.PhotoGallery.Domain.Services.Search;
using BinaryStudio.PhotoGallery.Domain.Services.Search.Results;
using BinaryStudio.PhotoGallery.Models;
using BinaryStudio.PhotoGallery.Web.Utils;
using BinaryStudio.PhotoGallery.Web.ViewModels.Search;

namespace BinaryStudio.PhotoGallery.Web.Area.Api
{
    [Authorize]
    [RoutePrefix("api/search")]
    public class SearchController : ApiController
    {
        private readonly ISearchModelConverter searchModelConverter;
        private readonly ISearchService searchService;
        private readonly IUserService userService;

        public SearchController(ISearchService searchService, ISearchModelConverter searchModelConverter, IUserService userService)
        {
            this.searchService = searchService;
            this.searchModelConverter = searchModelConverter;
            this.userService = userService;
        }

        [GET("")]
        public HttpResponseMessage GetSearch([FromUri] SearchViewModel searchViewModel)
        {
            string usersEmail = User.Identity.Name;
            UserModel user = userService.GetUser(usersEmail);

            SearchArguments searchArguments = searchModelConverter.GetModel(searchViewModel, user.Id);

            SearchResult result = searchService.Search(searchArguments);

            var resultViewModel = new SearchResultViewModel
            {
                Items = result.Value.Select(found => searchModelConverter.GetViewModel(found)),
                SearchCacheToken = result.SearchCacheToken
            };

            return Request.CreateResponse(HttpStatusCode.OK, resultViewModel);
        }
    }
}