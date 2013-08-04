using System.Collections.Generic;
using BinaryStudio.PhotoGallery.Models;

namespace BinaryStudio.PhotoGallery.Domain.Services
{
    public interface ISearchService
    {
        IEnumerable<UserModel> SearchUsers(string searchQuery, int begin, int end);

        IEnumerable<AlbumModel> SearchAlbums(string searchQuery, int begin, int end);

        IEnumerable<PhotoModel> SearchPhotos(string searchQuery, int begin, int end);
    }
}
