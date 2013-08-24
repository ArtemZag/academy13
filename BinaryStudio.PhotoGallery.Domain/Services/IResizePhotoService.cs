using System.Collections.Generic;
using BinaryStudio.PhotoGallery.Core.PathUtils;
using BinaryStudio.PhotoGallery.Models;

namespace BinaryStudio.PhotoGallery.Domain.Services
{
    public interface IResizePhotoService
        // TODO By Mikhail:
        // TODO Alexander, this service is used only for resizing,
        // TODO why are you using here userId, albumId etc. ?
        // TODO To get any paths you must use PathUtil
        // TODO Please, immediately refactor this service!
    {
        //todo Warning: dont use AvatarSize.Original
        string GetUserAvatar(int userId, AvatarSize size);

        IEnumerable<string> GetUserAlbumThumbnails(int userId, int albumId);

        //Высота колажа = heightOfOneLineInTheCollage*numberOfLines
        string GetCollage(int userId, int albumId, int collageWidth, int heightOfOneLineInTheCollage, int numberOfLines);

        IEnumerable<PhotoModel> GetAvailablePhotos(int userId, int albumId);

        string GetThumbnail(int userId, int albumId, PhotoModel model, int maxHeight);
    }
}
