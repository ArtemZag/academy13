using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BinaryStudio.PhotoGallery.Core.PathUtils;

namespace BinaryStudio.PhotoGallery.Domain.Services
{
    public interface IResizePhotoService
    {
        //todo Warning: dont use AvatarSize.Original
        string GetUserAvatar(int userId, AvatarSize size);

        IEnumerable<string> GetUserAlbumThumbnails(int userId, int albumId);

        string GetCollage(int userId, int albumId);

        string GetThumbnail(int userId, int albumId, int photoId);
    }
}
