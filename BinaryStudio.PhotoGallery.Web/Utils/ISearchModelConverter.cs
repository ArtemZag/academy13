using BinaryStudio.PhotoGallery.Domain.Services.Search;
using BinaryStudio.PhotoGallery.Domain.Services.Search.Results;
using BinaryStudio.PhotoGallery.Web.ViewModels.Search;

namespace BinaryStudio.PhotoGallery.Web.Utils
{
    public interface ISearchModelConverter
    {
        IFoundViewModel GetViewModel(IFound found);

        SearchArguments GetModel(SearchRequestViewModel searchViewModel, int userId);
    }
}
