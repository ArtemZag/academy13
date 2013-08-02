using System.Collections.Generic;
using System.IO;
using System.Text;
using Facebook;


namespace BinaryStudio.PhotoGallery.Core.SocialNetworkUtils.Facebook
{
    public static class FB
    {
        private const string AppID = "659826524046756";
        private const string AppSecret = "1e75119f703323257a5ebcbafe3687e6";
        private const string RedirectionURL = "http://localhost:57367/Account/FacebookCallBack/";

        public static string FirstName { get; private set; }
        public static string LastName { get; private set; }
        public static string Email { get; private set; }

        /// <summary>
        /// Creates album in social network without adding photos.
        /// </summary>
        /// <param name="albumName">Album name</param>
        /// <param name="description">Description for album</param>
        /// <param name="token">User's access token for social network</param>
        /// <returns>album ID</returns>
        public static string CreateAlbum(string albumName, string description, string token)
        {
            var facebookClient = new FacebookClient(token);
            var albumParameters = new Dictionary<string, object>();

            albumParameters["message"] = description;
            albumParameters["name"] = albumName;

            dynamic album = facebookClient.Post("/me/albums", albumParameters);

            return album.id;
        }

        /// <summary>
        /// Adds photo collection to album in social network. If album does not exist, creates album first.
        /// </summary>
        /// <param name="photos">Collection of photo</param>
        /// <param name="albumName">Album name</param>
        /// <param name="token">User's access token for social network</param>
        public static void AddPhotosToAlbum(IEnumerable<string> photos, string albumName, string token)
        {
            var facebookClient = new FacebookClient(token);
            var albumID = "";

            //Reads album infos
            dynamic albums = facebookClient.Get("/me/albums");
            foreach (dynamic albumInfo in albums.data)
            {
                if (albumInfo.name != albumName) continue;
                albumID = albumInfo.id;
                break;
            }

            if (string.IsNullOrEmpty(albumID))
            {
                albumID = CreateAlbum(albumName, "Created by Bingally", token);
            }

            foreach (var photo in photos)
            {
                Stream attachement = new FileStream(photo, FileMode.Open);
                var parameters = new Dictionary<string, object>();
                parameters["message"] = "uploaded using Bingally";
                parameters["file"] = new FacebookMediaStream
                    {
                        ContentType = "image/jpeg",
                        FileName = Randomizer.GetString(8) + ".jpg"
                    }.SetValue(attachement);

                facebookClient.PostTaskAsync(albumID + "/photos", parameters);
            }

            /*//Get the Pictures inside the album this gives JASON objects list that has photo attributes 
            // described here http://developers.facebook.com/docs/reference/api/photo/
            dynamic albumsPhotos = facebookClient.Get(albumInfo.id + "/photos");*/
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
            stringBuilder.Append("&response_type=code&scope=email,user_photos");

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

        /// <summary>
        /// Gets list of all avaible album's names from facebook.com
        /// </summary>
        /// <param name="token">Unic user token from facebook</param>
        /// <returns>List of album names represented at facebook.com</returns>
        public static IEnumerable<string> GetListOfAlbums(string token)
        {
            var facebookClient = new FacebookClient(token);
            var albumList = new List<string>();

            dynamic albums = facebookClient.Get("/me/albums");
            foreach (dynamic albumInfo in albums.data)
            {
                if (!albumList.Contains(albumInfo.name))
                {
                    albumList.Add(albumInfo.name);
                }
            }

            return albumList;
        }
    }
}
