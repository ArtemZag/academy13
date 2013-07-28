using System;
using System.Linq;
using System.Linq.Expressions;
using BinaryStudio.PhotoGallery.Database.ModelInterfaces;
using BinaryStudio.PhotoGallery.Models;

namespace BinaryStudio.PhotoGallery.Domain.Tests.Mocked.ModelRepositories
{
    internal class TestPhotoRepository : IPhotoRepository
    {
        public IQueryable<PhotoModel> All()
        {
            throw new NotImplementedException();
        }

        public IQueryable<PhotoModel> Filter(Expression<Func<PhotoModel, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public bool Contains(Expression<Func<PhotoModel, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public PhotoModel Find(params object[] keys)
        {
            throw new NotImplementedException();
        }

        public PhotoModel Find(Expression<Func<PhotoModel, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public PhotoModel Add(PhotoModel item)
        {
            throw new NotImplementedException();
        }

        public void Delete(PhotoModel item)
        {
            throw new NotImplementedException();
        }

        public void Delete(Expression<Func<PhotoModel, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public void Update(PhotoModel item)
        {
            throw new NotImplementedException();
        }

        public int Count { get; private set; }
        public void Add(int albumId, int ownerId)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            // NOP
        }
    }
}
