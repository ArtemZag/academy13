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
        /// Returnds pathes to thumbnails of all formats for specified photo. 
        /// </summary>
        IEnumerable<string> GetThumnailsPaths(PhotoModel photo); 

        /// <summary>
        /// Returns pathes of temporary directories for each user. 
        /// </summary>
        IEnumerable<string> GetTemporaryDirectoriesPaths();
            
        /// <summary>
        /// For tests.
        /// </summary>
        IPathUtil PathUtil { set; }
    }
}
