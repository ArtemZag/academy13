using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using BinaryStudio.PhotoGallery.Database.ModelInterfaces;
using BinaryStudio.PhotoGallery.Models;

namespace BinaryStudio.PhotoGallery.Domain.Tests.Mocked.ModelRepositories
{
    internal class TestPhotoRepository : IPhotoRepository
    {
        private readonly List<PhotoModel> models;

        public TestPhotoRepository(MockedContext context)
        {
            models = context.Photos;
        }

        public void Dispose()
        {
            // NOP
        }

        public IQueryable<PhotoModel> All()
        {
            throw new NotImplementedException();
        }

        public IQueryable<PhotoModel> Filter(Expression<Func<PhotoModel, bool>> predicate)
        {
            Func<PhotoModel, bool> lambda = predicate.Compile();

            return models.Where(lambda).AsQueryable();
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
            PhotoModel result = null;

            Func<PhotoModel, bool> lambda = predicate.Compile();

            foreach (PhotoModel photoModel in models)
            {
                if (lambda(photoModel))
                {
                    result = photoModel;
                }
            }

            return result;
        }

        public PhotoModel Add(PhotoModel item)
        {
            models.Add(item);

            return item;
        }

        public void Delete(PhotoModel item)
        {
            int index = -1;

            for (int i = 0; i < models.Count; i++)
            {
                if (Equals(models[i].Id, item.Id))
                {
                    index = i;
                }
            }

            if (index != -1)
            {
                models[index].IsDeleted = true;
            }
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
    }
}