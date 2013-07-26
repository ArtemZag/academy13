using System;
using System.Collections.Generic;
using System.Security.Policy;
using System.Text;
using BinaryStudio.PhotoGallery.Models;
using Facebook;

namespace BinaryStudio.PhotoGallery.Core.SocialNetworkUtils.Facebook
{
    public class FB : IFB
    {
        private const string AppID = "659826524046756";
        private const string AppSecret = "1e75119f703323257a5ebcbafe3687e6";
        private const string RedirectionURL = "http://localhost:57367/Account/FacebookCallBack/";

        public static string FirstName { get; private set; }
        public static string LastName { get; private set; }
        public static string Email { get; private set; }

        public void CreateAlbum(string albumName, string token)
        {
            throw new System.NotImplementedException();
        }

        public void AddPhotosToAlbum(IEnumerable<PhotoModel> photos, string albumName, string token)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Generate a URL-type string to make a request to facebook for CODE
        /// </summary>
        /// <param name="userSecret">user unic key</param>
        /// <returns>URL-tyle string request</returns>
        public static string CreateAuthURL(string userSecret)
        {
            var stringBuilder = new StringBuilder();

            stringBuilder.Append("https://www.facebook.com/dialog/oauth?client_id=");
            stringBuilder.Append(AppID);
            stringBuilder.Append("&client_secret=");
            stringBuilder.Append(AppSecret);
            stringBuilder.Append("&redirect_uri=");
            stringBuilder.Append(RedirectionURL);
            stringBuilder.Append(userSecret);
            stringBuilder.Append("&response_type=code&scope=email");

            return stringBuilder.ToString();
        }

        /// <summary>
        /// Recive a token from facebook
        /// </summary>
        /// <param name="userSecret">user unic key</param>
        /// <param name="code">unic user code from facebook</param>
        /// <returns>Token from facebook</returns>
        public static string GetAccessToken(string userSecret, string code)
        {
            var facebookClient = new FacebookClient();

            dynamic result = facebookClient.Post("oauth/access_token", new
            {
                client_id = AppID,
                client_secret = AppSecret,
                redirect_uri = RedirectionURL+userSecret,
                code = code
            });
           
            return result.access_token;
        }

        /// <summary>
        /// Update static properties FirstName, LastName, Email from facebook
        /// </summary>
        /// <param name="infos">param string(not available in moment)</param>
        /// <param name="token">unic user token from facebook</param>
        public static void GetAccountInfo(string infos, string token)
        {
            var facebookClient = new FacebookClient {AccessToken = token};

            dynamic me = facebookClient.Get("me?fields=first_name,last_name,email");

            FirstName = me.first_name;
            LastName = me.last_name;
            Email = me.email;
        }
    }
}
