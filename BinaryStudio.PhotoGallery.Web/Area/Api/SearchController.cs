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
        private readonly ISearchModelConverter _searchModelConverter;
        private readonly ISearchService _searchService;
        private readonly IUserService _userService;

        public SearchController(ISearchService searchService, ISearchModelConverter searchModelConverter, IUserService userService)
        {
            _searchService = searchService;
            _searchModelConverter = searchModelConverter;
            _userService = userService;
        }

        [GET("")]
        public HttpResponseMessage GetSearch([FromUri] SearchViewModel searchViewModel)
        {
            string usersEmail = User.Identity.Name;
            UserModel user = _userService.GetUser(usersEmail);

            SearchArguments searchArguments = _searchModelConverter.GetModel(searchViewModel, user.Id);

            SearchResult result = _searchService.Search(searchArguments);

            var resultViewModel = new SearchResultViewModel
            {
                Items = result.Value.Select(found => _searchModelConverter.GetViewModel(found)),
                SearchCacheToken = result.SearchCacheToken
            };

            return Request.CreateResponse(HttpStatusCode.OK, resultViewModel);
        }
    }
}