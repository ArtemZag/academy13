using System.Collections.Generic;
using BinaryStudio.PhotoGallery.Core.Helpers;
using BinaryStudio.PhotoGallery.Models;

namespace BinaryStudio.PhotoGallery.Domain.Utils
{
    public interface IStorage
    {
        string GetAlbumPath(AlbumModel album);

        string GetAlbumPath(PhotoModel photo);

        string GetOriginalPhotoPath(PhotoModel photo);

        /// <summary>
        /// Returns pathes for all format directories in /thumbnail directory.
        /// </summary>
        IEnumerable<string> GetThumbnailFormatDirectories(PhotoModel photo);

        /// <summary>
        /// Returns pathes of temporary directories for each user. 
        /// </summary>
        IEnumerable<string> GetTemporaryDirectories();
    }
}
