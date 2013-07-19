using BinaryStudio.PhotoGallery.Models;

namespace BinaryStudio.PhotoGallery.Domain.Services
{
    public interface IUserService
    {
        bool RegisterUser(UserModel userModel);

        bool UpdateUser(UserModel userModel);

        bool DeleteUser(UserModel userModel);

        bool CheckUser(UserModel userModel);
    }
}