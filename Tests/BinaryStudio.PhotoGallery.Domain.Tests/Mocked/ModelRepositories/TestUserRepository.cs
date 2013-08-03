using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using BinaryStudio.PhotoGallery.Database.ModelInterfaces;
using BinaryStudio.PhotoGallery.Models;

namespace BinaryStudio.PhotoGallery.Domain.Tests.Mocked.ModelRepositories
{
    internal class TestUserRepository : IUserRepository
    {
        private readonly List<UserModel> models;

        public TestUserRepository(MockedContext context)
        {
            models = context.Users;
        }

        public void Dispose()
        {
            // NOP
        }

        public IQueryable<UserModel> All()
        {
            throw new NotImplementedException();
        }

        public IQueryable<UserModel> Filter(Expression<Func<UserModel, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public bool Contains(Expression<Func<UserModel, bool>> predicate)
        {
            bool result = false;

            Func<UserModel, bool> lambda = predicate.Compile();

            foreach (UserModel userModel in models)
            {
                result = lambda(userModel);
                if (result)
                {
                    break;
                }
            }

            return result;
        }

        public UserModel Find(params object[] keys)
        {
            throw new NotImplementedException();
        }

        public UserModel Find(Expression<Func<UserModel, bool>> predicate)
        {
            UserModel result = null;

            Func<UserModel, bool> lambda = predicate.Compile();

            foreach (UserModel userModel in models)
            {
                if (lambda(userModel))
                {
                    result = userModel;
                }
            }

            return result;
        }

        public UserModel Add(UserModel item)
        {
            models.Add(item);

            return item;
        }

        public void Delete(UserModel item)
        {
            int index = -1;

            for (int i = 0; i < models.Count; i++)
            {
                if (string.Equals(models[i].Email, item.Email))
                {
                    index = i;
                }
            }

            if (index != -1)
            {
                models.RemoveAt(index);
            }
        }

        public void Delete(Expression<Func<UserModel, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public void Update(UserModel item)
        {
            throw new NotImplementedException();
        }

        public int Count { get; private set; }
    }
}