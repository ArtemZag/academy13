using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace BinaryStudio.PhotoGallery.Core.Helpers
{
    public static class PathHelper
    {
        public static string ContentRoot
        {
            get
            {
                const string contentVirtualRoot = "~/Content";
                return VirtualPathUtility.ToAbsolute(contentVirtualRoot);
            }
        }

        public static string ImageRoot
        {
            get { return string.Format("{0}/{1}", ContentRoot,"Images"); }
        }

        public static string CssRoot
        {
            get { return string.Format("{0}/{1}", ContentRoot, "Css"); }
        }
  
        public static string ImageUrl(string imageFile)
        {
            string result = string.Format("{0}/{1}", ImageRoot, imageFile);
            return result;
        }

        public static string CssUrl(string cssFile)
        {
            string result = string.Format("{0}/{1}", CssRoot, cssFile);
            return result;
        }
    }
}
