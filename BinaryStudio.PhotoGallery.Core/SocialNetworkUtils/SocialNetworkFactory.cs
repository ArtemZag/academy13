using BinaryStudio.PhotoGallery.Core.SocialNetworkUtils.Vkontakte;


namespace BinaryStudio.PhotoGallery.Core.SocialNetworkUtils
{
    public class SocialNetworkFactory : ISocialNetworkFactory
    {

        public ISocialNetwork getFacebookUnit()
        {
            return null;
        }

        public ISocialNetwork getVK()
        {
            return new VK();
        }
    }
}
