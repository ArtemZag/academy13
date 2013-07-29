using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using BinaryStudio.PhotoGallery.Database.ModelInterfaces;
using BinaryStudio.PhotoGallery.Models;
using NSubstitute;

namespace BinaryStudio.PhotoGallery.Domain.Tests.Mocked.ModelRepositories
{
    internal class TestUserRepository : IUserRepository
    {
        // for NSubstitute
        private readonly IUserRepository mockedRepository;
        private readonly List<UserModel> models;

        public TestUserRepository(MockedContext mockedContext)
        {
            models = mockedContext.Users;

            mockedRepository = Substitute.For<IUserRepository>();
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
            mockedRepository.Contains(Arg.Any<Expression<Func<UserModel, bool>>>())
                            .Returns(info =>
                                {
                                    bool result = false;

                                    var expression = (Expression<Func<UserModel, bool>>) info[0];
                                    Func<UserModel, bool> lambda = expression.Compile();

                                    foreach (UserModel userModel in models)
                                    {
                                        result = lambda(userModel);
                                        if (result)
                                        {
                                            break;
                                        }
                                    }

                                    return result;
                                });

            return mockedRepository.Contains(predicate);
        }

        public UserModel Find(params object[] keys)
        {
            throw new NotImplementedException();
        }

        public UserModel Find(Expression<Func<UserModel, bool>> predicate)
        {
            mockedRepository.Find(Arg.Any<Expression<Func<UserModel, bool>>>())
                            .Returns(info =>
                                {
                                    UserModel result = null;

                                    var expression = (Expression<Func<UserModel, bool>>) info[0];
                                    Func<UserModel, bool> lambda = expression.Compile();

                                    foreach (UserModel userModel in models)
                                    {
                                        if (lambda(userModel))
                                        {
                                            result = userModel;
                                        }
                                    }

                                    return result;
                                });

            return mockedRepository.Find(predicate);
        }

        public UserModel Add(UserModel item)
        {
            mockedRepository.Add(Arg.Any<UserModel>()).Returns(info =>
                {
                    var user = (UserModel) info[0];

                    models.Add(user);

                    return user;
                });

            return mockedRepository.Add(item);
        }

        public void Delete(UserModel item)
        {
            mockedRepository.Delete(Arg.Do<UserModel>(model =>
                {
                    int index = -1;

                    for (int i = 0; i < models.Count; i++)
                    {
                        if (string.Equals(models[i].Email, model.Email))
                        {
                            index = i;
                        }
                    }

                    if (index != -1)
                    {
                        models.RemoveAt(index);
                    }
                }));

            mockedRepository.Delete(item);
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