namespace BinaryStudio.PhotoGallery.Core.PhotoUtils
{
    public interface IPhotoProcessor
    {
        /// <summary>
        ///     Creates thumbnails of all formats for original photo
        /// </summary>
        void CreateThumbnails(int userId, int albumId, int photoId, string format);

        /// <summary>
        ///     Creates thumbnail of all formats for original avatar
        /// </summary>
        void CreateAvatarThumbnails(int userId);
    }
}