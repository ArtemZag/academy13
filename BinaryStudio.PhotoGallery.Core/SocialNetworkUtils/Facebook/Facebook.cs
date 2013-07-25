using System;
using System.Collections.Generic;
using BinaryStudio.PhotoGallery.Models;
using Facebook;

namespace BinaryStudio.PhotoGallery.Core.SocialNetworkUtils.Facebook
{
    /// <summary>
    /// Class 
    /// </summary>
    public class Facebook : IFacebook
    {
        public void AddAlbum(AlbumModel album)
        {
            throw new NotImplementedException();
        }

        public void AddPhotos(IEnumerable<PhotoModel> photos, string album)
        {
            throw new NotImplementedException();
        }

        public string GetAccessToken(string code)
        {
            var fb = new FacebookClient();
            dynamic result = fb.Post("oauth/access_token", new
            {
                client_id = "659826524046756",
                client_secret = "1e75119f703323257a5ebcbafe3687e6",
                redirect_uri = RedirectUri.AbsoluteUri,
                code = code
            });

            var accessToken = result.access_token;

            // Store the access token in the session
            //Session["AccessToken"] = accessToken;

            // update the facebook client with the access token so 
            // we can make requests on behalf of the user
            fb.AccessToken = accessToken;

            // Get the user's information
            dynamic me = fb.Get("me?fields=first_name,last_name,id,email");
            string email = me.email;

            return email;
        }

        public static string GetAuthAddress()
        {
            var fb = new FacebookClient();
            var loginUrl = fb.GetLoginUrl(new
                {
                    client_id = "659826524046756",
                    client_secret = "1e75119f703323257a5ebcbafe3687e6",
                    redirect_uri = RedirectUri.AbsoluteUri,
                    response_type = "code",
                    scope = "email" // Add other permissions as needed
                });

            return loginUrl.AbsoluteUri; // Redirect(loginUrl.AbsoluteUri);
        }

        private static Uri RedirectUri
        {
            get
            {
                var uriBuilder = new UriBuilder()
                    {
                        Query = null,
                        Fragment = null,
                        Path = "Account/FacebookCallback",
                        Port = 57367
                    };

                return uriBuilder.Uri;
            }
        }
    }
}
