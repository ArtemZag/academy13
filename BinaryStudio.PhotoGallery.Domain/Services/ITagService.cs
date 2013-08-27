using System.Collections.Generic;
using BinaryStudio.PhotoGallery.Models;

namespace BinaryStudio.PhotoGallery.Domain.Services
{
    public interface ITagService
    {
        IEnumerable<AlbumTagModel> GetAlbumTags(int albumId);

        void SetAlbumTags(int albumId, ICollection<AlbumTagModel> tags);
    }
}
