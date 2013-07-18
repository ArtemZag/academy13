using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BinaryStudio.PhotoGallery.Web.ViewModels
{
    public class AuthorizationViewModel
    {
        public string LoginOrEmail { get; set; }
        public string Password { get; set; }
    }
}