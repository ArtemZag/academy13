namespace BinaryStudio.PhotoGallery.Core.PathUtils
{
    public interface IUrlUtil
    {
        string BuildPhotoViewUrl(int photoId);

        string BuildAlbumViewUrl(int albumId);

        string BuildUserViewUrl(int userId);
    }
}
