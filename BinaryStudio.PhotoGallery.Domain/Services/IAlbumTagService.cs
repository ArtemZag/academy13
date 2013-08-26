using System.Collections.Generic;
using BinaryStudio.PhotoGallery.Models;

namespace BinaryStudio.PhotoGallery.Domain.Services
{
    public interface IAlbumTagService
    {
        IEnumerable<AlbumTagModel> GetTags(int albumId);

        void SetTags(int albumId, ICollection<AlbumTagModel> tags);
    }
}
