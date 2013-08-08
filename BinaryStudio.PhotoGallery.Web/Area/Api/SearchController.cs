using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AttributeRouting;
using BinaryStudio.PhotoGallery.Domain.Services.Search;
using BinaryStudio.PhotoGallery.Domain.Services.Search.Items;
using BinaryStudio.PhotoGallery.Web.Utils;
using BinaryStudio.PhotoGallery.Web.ViewModels.Search;

namespace BinaryStudio.PhotoGallery.Web.Area.Api
{
    [RoutePrefix("Api/Search")]
    public class SearchController : ApiController
    {
        private readonly IModelConverter modelConverter;
        private readonly ISearchService searchService;

        public SearchController(ISearchService searchService, IModelConverter modelConverter)
        {
            this.searchService = searchService;
            this.modelConverter = modelConverter;
        }

        [HttpGet]
        public HttpResponseMessage Search([FromBody] SearchViewModel searchViewModel)
        {
            SearchArguments searchArguments = modelConverter.GetModel(searchViewModel);

            IEnumerable<IFoundItem> result = searchService.Search(searchArguments);

            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
    }
}