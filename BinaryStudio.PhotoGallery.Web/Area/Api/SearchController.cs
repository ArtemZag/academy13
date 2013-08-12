using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BinaryStudio.PhotoGallery.Domain.Services.Search;
using BinaryStudio.PhotoGallery.Domain.Services.Search.FoundItems;
using BinaryStudio.PhotoGallery.Web.Utils;
using BinaryStudio.PhotoGallery.Web.ViewModels.Search;

namespace BinaryStudio.PhotoGallery.Web.Area.Api
{
    internal class SearchController : ApiController
    {
        private readonly IModelConverter modelConverter;
        private readonly ISearchService searchService;

        public SearchController(ISearchService searchService, IModelConverter modelConverter)
        {
            this.searchService = searchService;
            this.modelConverter = modelConverter;
        }

        public HttpResponseMessage GetSearch([FromUri] SearchViewModel searchViewModel)
        {
            SearchArguments searchArguments = modelConverter.GetModel(searchViewModel);

            IEnumerable<IFoundItem> result = searchService.Search(searchArguments);

            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
    }
}