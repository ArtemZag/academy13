using System.Web.Http;
using System.Web.Mvc;
using BinaryStudio.PhotoGallery.Domain.Services;
using BinaryStudio.PhotoGallery.Models;

namespace BinaryStudio.PhotoGallery.Web.Controllers
{
    public class UserController : ApiController
    {
        private IUserService userService;

        /// <summary>
        /// Resolves IUserService.
        /// </summary>
        public UserController()
        {
            userService = DependencyResolver.Current.GetService<IUserService>();
        }

        /// <summary>
        /// Registrates user. POST api/registration
        /// </summary>
        public void PostRegistration(UserModel user)
        {
            // todo: IUserService use
        }

        /// <summary>
        /// Updates user. PUT api/updateuser
        /// </summary>
        public void PutUpdateUser(UserModel user)
        {
            // todo: IUserService use
        }
    }
}