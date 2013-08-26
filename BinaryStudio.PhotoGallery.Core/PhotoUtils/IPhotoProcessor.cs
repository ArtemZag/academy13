﻿namespace BinaryStudio.PhotoGallery.Core.PhotoUtils
{
    public interface IPhotoProcessor
    {
        void CreateThumbnails(int userId, int albumId, int photoId, string format);

        void CreateAvatarThumbnails(int userId);
    }
}