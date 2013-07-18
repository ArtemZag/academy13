using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using BinaryStudio.PhotoGallery.Domain.Services;
using BinaryStudio.PhotoGallery.Models;
using BinaryStudio.PhotoGallery.Web.Filters;

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
        [ValidateModel]
        public HttpResponseMessage PostRegistration(AuthInfoModel user)
        {
            // ViewModel to Model convertation with check params? 
            // todo: IUserService using

            throw new NotImplementedException();
        }

        /// <summary>
        /// Updates user. PUT api/updateuser
        /// </summary>
        [ValidateModel]        
        public HttpResponseMessage PutUpdateUser(AuthInfoModel user)
        {
            // ViewModel to Model convertation with check params? 
            // todo: IUserService using

            throw new NotImplementedException();
        }
    }
}