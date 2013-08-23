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
    public class SearchController : BaseApiController
    {
        private readonly ISearchModelConverter searchModelConverter;
        private readonly ISearchService searchService;

        public SearchController(ISearchService searchService, ISearchModelConverter searchModelConverter)
        {
            this.searchService = searchService;
            this.searchModelConverter = searchModelConverter;
        }

        [GET("")]
        public HttpResponseMessage GetSearch([FromUri] SearchViewModel searchViewModel)
        {
            SearchArguments searchArguments = searchModelConverter.GetModel(searchViewModel, User.Id);

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