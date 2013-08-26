using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AttributeRouting;
using AttributeRouting.Web.Mvc;
using BinaryStudio.PhotoGallery.Domain.Services.Search;
using BinaryStudio.PhotoGallery.Domain.Services.Search.Results;
using BinaryStudio.PhotoGallery.Web.Utils;
using BinaryStudio.PhotoGallery.Web.ViewModels.Search;

namespace BinaryStudio.PhotoGallery.Web.Area.Api
{
    [Authorize]
    [RoutePrefix("api/search")]
    public class SearchApiController : BaseApiController
    {
        private readonly ISearchModelConverter _searchModelConverter;
        private readonly ISearchService _searchService;

        public SearchApiController(ISearchService searchService, ISearchModelConverter searchModelConverter)
        {
            _searchService = searchService;
            _searchModelConverter = searchModelConverter;
        }

        [GET("")]
        public HttpResponseMessage GetSearch([FromUri] SearchViewModel searchViewModel)
        {
            SearchArguments searchArguments = _searchModelConverter.GetModel(searchViewModel, User.Id);

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