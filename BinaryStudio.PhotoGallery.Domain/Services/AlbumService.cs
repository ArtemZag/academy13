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

        public bool CreateAlbum(AlbumModel album)
        {
            throw new NotImplementedException();
        }

        public bool UpdateAlbum(AlbumModel album)
        {
            throw new NotImplementedException();
        }

        public bool DeleteAlbum(AlbumModel album)
        {
            throw new NotImplementedException();
        }
    }
}
