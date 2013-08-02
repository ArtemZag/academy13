using System.Collections.Generic;
using BinaryStudio.PhotoGallery.Models;

namespace BinaryStudio.PhotoGallery.Core.SocialNetworkUtils.Facebook
{
    interface IFacebook
    {
        /// <summary>
        /// Adds album to user's facebook account
        /// </summary>
        /// <param name="album">Is adding album</param>
        void AddAlbum(AlbumModel album);

        /// <summary>
        /// Adds photos to user's facebook album
        /// </summary>
        /// <param name="photos">Collection of photos</param>
        /// <param name="album">Name of facebook album</param>
        void AddPhotos(IEnumerable<PhotoModel> photos, string album);

        /// <summary>
        /// Returns a valid access token for current user
        /// </summary>
        string GetAccessToken(string code);
    }
}
