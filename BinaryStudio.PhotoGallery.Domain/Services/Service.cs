using System;
using BinaryStudio.PhotoGallery.Database;
using BinaryStudio.PhotoGallery.Database.ModelInterfaces;
using BinaryStudio.PhotoGallery.Models;

namespace BinaryStudio.PhotoGallery.Domain.Services
{
    internal class Service : IUserService, IAlbumService
    {
        private readonly IUnitOfWorkFactory workFactory;

        public Service(IUnitOfWorkFactory workFactory)
        {
            this.workFactory = workFactory;
        }

        public void RegisterUser(UserModel user)
        {
            using (IUnitOfWork unitOfWork = workFactory.GetUnitOfWork())
            {
                IUserRepository userRepository = unitOfWork.Users;

                userRepository.Create(user);
                //todo: it needs some exceptions checking
            }
        }

        public void UpdateUser(UserModel user)
        {
            using (IUnitOfWork unitOfWork = workFactory.GetUnitOfWork())
            {
                IUserRepository userRepository = unitOfWork.Users;

                userRepository.Update(user);
                //todo: it needs some exceptions checking
            }
        }

        public bool CheckUser(UserModel user)
        {
            using (IUnitOfWork unitOfWork = workFactory.GetUnitOfWork())
            {
                IUserRepository userRepository = unitOfWork.Users;

                return userRepository.Contains(model => model.Equals(user));
            }
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