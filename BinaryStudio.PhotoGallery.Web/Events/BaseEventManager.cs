using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BinaryStudio.PhotoGallery.Web.CustomStructure;

namespace BinaryStudio.PhotoGallery.Web.Events
{
    public class BaseEventManager 
    {
        public CustomPrincipal User
        {
            get { return HttpContext.Current.User as CustomPrincipal; }
        }
    }
}