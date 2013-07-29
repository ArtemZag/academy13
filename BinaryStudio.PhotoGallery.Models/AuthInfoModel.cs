using System.ComponentModel.DataAnnotations;


namespace BinaryStudio.PhotoGallery.Models
{
    /// <summary>
    /// The class that represents types of user's authentications.
    /// </summary>
    public class AuthInfoModel
    {
        public enum ProviderType
        {
            Local,
            Google
        }

        public AuthInfoModel()
        {
        }

        /// <summary>
        /// Main constructor with important fields
        /// </summary>
        /// <param name="userId">user id</param>
        /// <param name="authProvider">[local][google][facebook]</param>
        public AuthInfoModel(int userId, string authProvider)
        {
            UserModelId = userId;
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
        /// Gets or sets token for work with social webs
        /// </summary>
        public string AuthProviderToken { get; set; }

        public int UserModelId { get; set; }
    }
}