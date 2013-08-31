using System;
using System.Linq;
using System.Security.Authentication;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.Security;
using AttributeRouting;
using AttributeRouting.Helpers;
using AttributeRouting.Web.Mvc;
using BinaryStudio.PhotoGallery.Domain.Exceptions;
using BinaryStudio.PhotoGallery.Domain.Services;
using BinaryStudio.PhotoGallery.Models;
using BinaryStudio.PhotoGallery.Web.CustomStructure;
using OAuth2;
using OAuth2.Client;
using OAuth2.Models;

namespace BinaryStudio.PhotoGallery.Web.Controllers
{
    [RoutePrefix("OAuth")]
    public class OAuthController : BaseController
    {
        private const string ProviderNameKey = "providerName";
        private const string RegOauth = "oauth";
        private readonly AuthorizationRoot _authorizationRoot;
        private readonly IUserService _userService;


        private string ProviderName
        {
            get { return (string)Session[ProviderNameKey]; }
            set { Session[ProviderNameKey] = value; }
        }

        private bool SetOauth
        {
            get { return (bool)Session[RegOauth]; }
            set { Session[RegOauth] = value; }
        }



        public OAuthController(IUserService userService)
        {
            _authorizationRoot = new AuthorizationRoot();
            _userService = userService;
        }

        


        [GET("callback")]
        public ActionResult Auth()
        {
            UserInfo info = GetClient().GetUserInfo(Request.QueryString);

            if (SetOauth.HasNoValue())
            {
                SetOauth = false;
            }
            if (SetOauth)
            {
                _userService.SetAuthInfoForUser(User.Id, info.ProviderName, info.Id);
                SetOauth = false;
            }
            try
            {
                var userId = _userService.GetUserBySocialAccount(info.ProviderName, info.Id);


                UserModel userModel = _userService.GetUser(userId);

                var serializer = new JavaScriptSerializer();
                var serializeModel = new UserInfoSerializeModel
                {
                    Id = userModel.Id,
                    Email = userModel.Email,
                    IsAdmin = userModel.IsAdmin
                };

                string serializedUserData = serializer.Serialize(serializeModel);

                var authTicket = new FormsAuthenticationTicket(
                    1,
                    userModel.Id.ToString(),
                    DateTime.Now,
                    DateTime.Now.AddDays(30),
                    true,
                    serializedUserData
                    );

                string encryptedTicket = FormsAuthentication.Encrypt(authTicket);

                var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);

                System.Web.HttpContext.Current.Response.Cookies.Add(cookie);


                return RedirectToAction("Index", "Home");
            }
            catch (Exception)
            {

                throw;
            }

        }

        /// <summary>
        ///     Redirect to login url of selected provider.
        /// </summary>
        [GET("login/{providerName}")]
        public RedirectResult Login([FromUri]string providerName)
        {
            ProviderName = providerName;
            return new RedirectResult(GetClient().GetLoginLinkUri());
        }

        private IClient GetClient()
        {
            return _authorizationRoot.Clients.First(c => c.Name.ToLower() == ProviderName);
        }


        /// <summary>
        ///     Redirect to login url of selected provider.
        /// </summary>
        [GET("setaccount/{providerName}")]
        public RedirectResult SetAccount([FromUri]string providerName)
        {
            ProviderName = providerName;
            SetOauth = true;
            //_userService.AddAuthInfoForUser(User.Id, ProviderName);

            return new RedirectResult(GetClient().GetLoginLinkUri());
        }
    }
}