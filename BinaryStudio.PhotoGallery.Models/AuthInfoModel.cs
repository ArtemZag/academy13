using System.ComponentModel.DataAnnotations;


namespace BinaryStudio.PhotoGallery.Models
{
    /// <summary>
    /// The class that represents types of user's authentications.
    /// </summary>
    public class AuthInfoModel
    {
        /// <summary>
        /// Strings that represents type of auth.
        /// </summary>
        public const string LOCAL_PROFILE = "local";
        public const string GOOGLE_PROFILE = "google";

        /// <summary>
        /// Main constructor with important fields
        /// </summary>
        /// <param name="userID">user id</param>
        /// <param name="password">password</param>
        /// <param name="authProvider">[local][google][facebook]</param>
        public AuthInfoModel(int userID, string password, string authProvider)
        {
            UserModelID = userID;
            UserPassword = password;
            AuthProvider = authProvider;
        }

        /// <summary>
        /// Gets or sets the name of the user.
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// Gets or sets the name of the authentication service.
        /// </summary>
        public string AuthProvider { get; set; }

        /// <summary>
        /// Gets or sets the user's password.
        /// </summary>
        public string UserPassword { get; set; }

        /// <summary>
        /// Gets or sets token for work with social webs
        /// </summary>
        public string AuthProviderToken { get; set; }

        public int UserModelID { get; set; }
    }
}