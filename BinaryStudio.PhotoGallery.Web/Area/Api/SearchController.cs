using System.Net;
using System.Net.Http;
using System.Web.Http;
using BinaryStudio.PhotoGallery.Core.PathUtils;
using BinaryStudio.PhotoGallery.Domain.Services.Search;
using BinaryStudio.PhotoGallery.Domain.Services.Search.Results;
using BinaryStudio.PhotoGallery.Web.Utils;
using BinaryStudio.PhotoGallery.Web.ViewModels.Search;

namespace BinaryStudio.PhotoGallery.Web.Area.Api
{
    public class SearchController : ApiController
    {
        private readonly IModelConverter modelConverter;
        private readonly IPathUtil pathUtil;
        private readonly ISearchService searchService;

        public SearchController(ISearchService searchService, IModelConverter modelConverter, IPathUtil pathUtil)
        {
            this.searchService = searchService;
            this.modelConverter = modelConverter;
            this.pathUtil = pathUtil;
        }

        public HttpResponseMessage GetSearch([FromUri] SearchViewModel searchViewModel)
        {
            SearchArguments searchArguments = modelConverter.GetModel(searchViewModel);

            SearchResult result = searchService.Search(searchArguments);

            pathUtil.BuildPhotoDirectoryPath();

            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
    }
}