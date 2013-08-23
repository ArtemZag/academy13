using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BinaryStudio.PhotoGallery.Core.PathUtils;
using BinaryStudio.PhotoGallery.Models;

namespace BinaryStudio.PhotoGallery.Domain.Services
{
    public interface IResizePhotoService
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
