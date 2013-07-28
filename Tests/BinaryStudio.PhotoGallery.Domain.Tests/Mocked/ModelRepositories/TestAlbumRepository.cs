using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using BinaryStudio.PhotoGallery.Database.ModelInterfaces;
using BinaryStudio.PhotoGallery.Models;

namespace BinaryStudio.PhotoGallery.Domain.Tests.Mocked.ModelRepositories
{
    internal class TestAlbumRepository : IAlbumRepository
    {
        // for NSubstitute
        private readonly IUserRepository mockedRepository;
        private readonly List<AlbumModel> models;

        public IQueryable<AlbumModel> All()
        {
            throw new NotImplementedException();
        }

        public IQueryable<AlbumModel> Filter(Expression<Func<AlbumModel, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public bool Contains(Expression<Func<AlbumModel, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public AlbumModel Find(params object[] keys)
        {
            throw new NotImplementedException();
        }

        public AlbumModel Find(Expression<Func<AlbumModel, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public AlbumModel Add(AlbumModel item)
        {
            throw new NotImplementedException();
        }

        public void Delete(AlbumModel item)
        {
            throw new NotImplementedException();
        }

        public void Delete(Expression<Func<AlbumModel, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public void Update(AlbumModel item)
        {
            throw new NotImplementedException();
        }

        public int Count { get; private set; }
        public void Add(string albumName, int ownerId)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            // NOP
        }
    }
}
