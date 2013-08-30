namespace BinaryStudio.PhotoGallery.Core.PathUtils
{
    public interface IUrlUtil
    {
        /// <returns>Pattern: /photo/photoId</returns>
        string BuildPhotoViewUrl(int photoId);

        /// <returns>Pattern: /album/albumId</returns>
        string BuildAlbumViewUrl(int albumId);

        /// <returns>Pattern: /user/userId</returns>
        string BuildUserViewUrl(int userId);
    }
}
