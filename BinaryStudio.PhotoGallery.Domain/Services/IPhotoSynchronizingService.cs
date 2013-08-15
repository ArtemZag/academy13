using System.Collections.Generic;
using BinaryStudio.PhotoGallery.Models;

namespace BinaryStudio.PhotoGallery.Domain.Services
{
    public interface IPhotoSynchronizingService
    {
        void Initialize(int albumId, int userId, int maxHeight);
        void SyncOriginalAndThumbnailImages();
        bool DeleteThumbnailsIfOriginalNotExist(List<string> models);
        bool CreateThumbnailsIfNotExist(List<string> models);
        string GetUserAvatar();
        string CreateCollageIfNotExist(int width,int rows);
        IEnumerable<string> GetThumbnails();
        string MakeCollage(int width, int rows);
        string GetRandomCollageFileName();
        IEnumerable<string> GetEnumerator(IEnumerable<string> thumbnails);
        List<AlbumModel> GetAlbumsForSyncronize();
        List<PhotoModel> GetPhotosForSyncronizeByModelAndUserId(int userId,int albumId);
    }
}