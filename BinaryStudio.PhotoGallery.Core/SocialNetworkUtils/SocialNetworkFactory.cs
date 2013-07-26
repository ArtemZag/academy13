using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BinaryStudio.PhotoGallery.Core.SocialNetworkUtils.Facebook;
using BinaryStudio.PhotoGallery.Core.SocialNetworkUtils.Vkontakte;

namespace BinaryStudio.PhotoGallery.Core.SocialNetworkUtils
{
    public class SocialNetworkFactory : ISocialNetworkFactory
    {

        public ISocialNetwork getFacebookUnit()
        {
            return new FB();
        }

        public ISocialNetwork getVK()
        {
            return new VK();
        }
    }
}
