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
        private readonly List<AlbumModel> models; 

        public TestAlbumRepository(MockedContext context)
        {
            models = context.Albums;
        }

        public void Dispose()
        {
            // NOP
        }

        public IQueryable<AlbumModel> All()
        {
            throw new NotImplementedException();
        }

        public IQueryable<AlbumModel> Filter(Expression<Func<AlbumModel, bool>> predicate)
        {
            Func<AlbumModel, bool> lambda = predicate.Compile();

            return models.Where(lambda).AsQueryable();
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
            AlbumModel result = null;

            Func<AlbumModel, bool> lambda = predicate.Compile();

            foreach (AlbumModel albumModel in models)
            {
                if (lambda(albumModel))
                {
                    result = albumModel;
                }
            }

            return result;
        }

        public AlbumModel Add(AlbumModel item)
        {
            models.Add(item);

            return item;
        }

        public void Delete(AlbumModel item)
        {
            int index = -1;

            for (int i = 0; i < models.Count; i++)
            {
                if (string.Equals(models[i].Id, item.Id))
                {
                    index = i;
                }
            }

            if (index != -1)
            {
                models[index].IsDeleted = true;
            }
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
    }
}