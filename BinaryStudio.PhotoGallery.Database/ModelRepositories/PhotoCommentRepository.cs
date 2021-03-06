﻿using BinaryStudio.PhotoGallery.Database.ModelInterfaces;
using BinaryStudio.PhotoGallery.Models;

namespace BinaryStudio.PhotoGallery.Database.ModelRepositories
{
    internal class PhotoCommentRepository : BaseRepository<PhotoCommentModel>, IPhotoCommentRepository
    {
        public PhotoCommentRepository(DatabaseContext dataBaseContext)
            : base(dataBaseContext)
        {
        }

        public void Add(int ownerId, int photoId, string text, int repliedCommentId)
        {
            base.Add(new PhotoCommentModel(ownerId, photoId, text, repliedCommentId));
        }
    }
}
