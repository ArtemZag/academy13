using System.Collections.Generic;
using BinaryStudio.PhotoGallery.Database;
using BinaryStudio.PhotoGallery.Models;

namespace BinaryStudio.PhotoGallery.Domain.Utils
{
    public interface IStorage
    {
        string GetAlbumPath(AlbumModel album, IUnitOfWork unitOfWork);

        string GetOriginalPhotoPath(PhotoModel photo, IUnitOfWork unitOfWork);

        /// <summary>
        ///     Returnds pathes to thumbnails of all formats for specified photo.
        /// </summary>
        IEnumerable<string> GetThumnailsPaths(PhotoModel photo, IUnitOfWork unitOfWork);
    }
}