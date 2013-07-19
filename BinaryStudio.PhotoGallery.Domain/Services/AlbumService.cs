using System;
using BinaryStudio.PhotoGallery.Database;
using BinaryStudio.PhotoGallery.Models;

namespace BinaryStudio.PhotoGallery.Domain.Services
{
    internal class AlbumService : Service, IAlbumService
    {
        public AlbumService(IUnitOfWorkFactory workFactory) : base(workFactory)
        {
        }

        public void AddAlbum(AlbumModel albumModel)
        {
            throw new NotImplementedException();
        }

        public void DeleteAlbum(AlbumModel albumModel)
        {
            throw new NotImplementedException();
        }
    }
}
