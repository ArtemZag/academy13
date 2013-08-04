using System.Collections.Generic;

namespace BinaryStudio.PhotoGallery.Core.SocialNetworkUtils
{
    public interface ISocialNetwork
    {
        /// <summary>
        /// Creates album in social network without adding photos.
        /// </summary>
        /// <param name="albumName">Album name</param>
        /// <param name="description">Description for album</param>
        /// <param name="token">User's access token for social network</param>
        /// <returns>album ID</returns>
        string CreateAlbum(string albumName, string description, string token);

        /// <summary>
        /// Adds photo collection to album in social network. If album does not exist, creates album first.
        /// </summary>
        /// <param name="photos">Collection of photo</param>
        /// <param name="albumName">Album name</param>
        /// <param name="token">User's access token for social network</param>
        void AddPhotosToAlbum(IEnumerable<string> photos, string albumName, string token);
    }
}
