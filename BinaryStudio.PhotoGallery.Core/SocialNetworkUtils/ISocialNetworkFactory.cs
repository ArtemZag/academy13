using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinaryStudio.PhotoGallery.Core.SocialNetworkUtils
{
    public interface ISocialNetworkFactory
    {
        ISocialNetwork getFacebookUnit();
        ISocialNetwork getVK();
    }
}
