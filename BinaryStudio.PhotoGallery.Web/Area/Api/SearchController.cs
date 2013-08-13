using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BinaryStudio.PhotoGallery.Domain.Services;
using BinaryStudio.PhotoGallery.Domain.Services.Search;
using BinaryStudio.PhotoGallery.Domain.Services.Search.Results;
using BinaryStudio.PhotoGallery.Models;
using BinaryStudio.PhotoGallery.Web.Utils;
using BinaryStudio.PhotoGallery.Web.ViewModels.Search;

namespace BinaryStudio.PhotoGallery.Web.Area.Api
{
    public class SearchController : ApiController
    {
        private readonly IModelConverter modelConverter;
        private readonly ISearchService searchService;
        private readonly IUserService userService;

        public SearchController(ISearchService searchService, IModelConverter modelConverter, IUserService userService)
        {
            this.searchService = searchService;
            this.modelConverter = modelConverter;
            this.userService = userService;
        }

        public HttpResponseMessage GetSearch([FromUri] SearchViewModel searchViewModel)
        {
            string usersEmail = User.Identity.Name;
            UserModel user = userService.GetUser(usersEmail);

            SearchArguments searchArguments = modelConverter.GetModel(searchViewModel, user.Id);

            SearchResult result = searchService.Search(searchArguments);

            var resultViewModel = new SearchResultViewModel
            {
                Items = result.Value.Select(found => modelConverter.GetViewModel(found)),
                CacheToken = result.CacheToken
            };

            return Request.CreateResponse(HttpStatusCode.OK, resultViewModel);
        }
    }
}