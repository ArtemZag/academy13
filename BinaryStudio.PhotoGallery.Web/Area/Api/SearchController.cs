using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AttributeRouting;
using AttributeRouting.Web.Mvc;
using BinaryStudio.PhotoGallery.Domain.Services;
using BinaryStudio.PhotoGallery.Domain.Services.Search;
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
            HttpResponseMessage responseMessage = null;

            string query = searchViewModel.SearchQuery;

            int begin = searchViewModel.Begin;
            int end = searchViewModel.End;

            // todo

            return responseMessage;
        }
    }
}