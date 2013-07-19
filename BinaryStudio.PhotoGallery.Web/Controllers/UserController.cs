using System.Net;
using System.Net.Http;
using System.Web.Http;
using BinaryStudio.PhotoGallery.Domain.Services;
using BinaryStudio.PhotoGallery.Models;
using BinaryStudio.PhotoGallery.Web.Utils;
using BinaryStudio.PhotoGallery.Web.ViewModels;

namespace BinaryStudio.PhotoGallery.Web.Controllers
{
    public class UserController : ApiController
    {
        private IUserService userService;

        /// <summary>
        /// Resolves IUserService.
        /// </summary>
        public UserController(IUserService userService)
        {
            this.userService = userService;
        }

        /// <summary>
        /// Registrates user. POST api/registration
        /// </summary>
        public HttpResponseMessage PostRegistration(RegistrationViewModel registrationViewModel)
        {
            UserModel userModel = ModelConverter.ToModel(registrationViewModel);

            userService.RegisterUser(userModel);

            return new HttpResponseMessage(HttpStatusCode.Created);
        }

        /// <summary>
        /// Updates user. PUT api/updateuser
        /// </summary>
        public HttpResponseMessage PutUpdateUser(RegistrationViewModel registrationViewModel)
        {
            UserModel userModel = ModelConverter.ToModel(registrationViewModel);

            userService.UpdateUser(userModel);

            return new HttpResponseMessage(HttpStatusCode.OK);
        }
    }
}