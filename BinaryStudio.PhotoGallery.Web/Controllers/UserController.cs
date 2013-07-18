using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using BinaryStudio.PhotoGallery.Domain.Services;
using BinaryStudio.PhotoGallery.Models;
using BinaryStudio.PhotoGallery.Web.Models;
using BinaryStudio.PhotoGallery.Web.Utils;

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
        public HttpResponseMessage PostRegistration(AuthInfoViewModel authViewModel)
        {
            UserModel userModel = ModelConverter.ToModel(authViewModel);

            userService.RegisterUser(userModel);

            return new HttpResponseMessage(HttpStatusCode.Created);
        }

        /// <summary>
        /// Updates user. PUT api/updateuser
        /// </summary>
        public HttpResponseMessage PutUpdateUser(AuthInfoViewModel authViewModel)
        {
            UserModel userModel = ModelConverter.ToModel(authViewModel);

            userService.UpdateUser(userModel);

            return new HttpResponseMessage(HttpStatusCode.OK);
        }
    }
}