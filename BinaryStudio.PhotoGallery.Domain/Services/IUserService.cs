using BinaryStudio.PhotoGallery.Models;

namespace BinaryStudio.PhotoGallery.Domain.Services
{
    public interface IUserService
    {
        void RegisterUser(UserModel user);

        void UpdateUser(UserModel user);

        bool CheckUser(UserModel user);
    }
}