using System.Collections.Generic;
using BinaryStudio.PhotoGallery.Core.PathUtils;

namespace BinaryStudio.PhotoGallery.Domain.Services
{
    public interface IResizePhotoService
    {
        //todo Warning: dont use AvatarSize.Original
        string GetUserAvatar(int userId, AvatarSize size);

        IEnumerable<string> GetUserAlbumThumbnails(int userId, int albumId);

        string GetCollage(int userId, int albumId, int collageWidth, int heightOfOneLineInTheCollage, int numberOfLines);

        string GetThumbnail(int userId, int albumId, string photoName, int maxHeight);
    }
}
