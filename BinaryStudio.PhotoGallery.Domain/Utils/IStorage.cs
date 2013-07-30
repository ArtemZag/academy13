using System.Collections.Generic;
using BinaryStudio.PhotoGallery.Core.PathUtils;
using BinaryStudio.PhotoGallery.Models;

namespace BinaryStudio.PhotoGallery.Domain.Utils
{
    public interface IStorage
    {
        string GetAlbumPath(AlbumModel album);

        string GetAlbumPath(PhotoModel photo);

        string GetOriginalPhotoPath(PhotoModel photo);

        /// <summary>
        /// Returns pathes for all format directories in photos/userId/albumId/thumbnail directory.
        /// </summary>
        IEnumerable<string> GetThumbnailDirectoryPath(PhotoModel photo);

        /// <summary>
        /// Returns pathes of temporary directories for each user. 
        /// </summary>
        IEnumerable<string> GetTemporaryDirectoriesPathes();

        /// <summary>
        /// For tests.
        /// </summary>
        IPathUtil PathUtil { set; }
    }
}
