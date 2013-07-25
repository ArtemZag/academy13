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
        public static string ContentDir
        {
            get
            {
                const string contentVirtualRoot = "~/Content";
                return VirtualPathUtility.ToAbsolute(contentVirtualRoot);
            }
        }

        public static string ImageDir
        {
            get { return string.Format("{0}/{1}", ContentDir,"Images/"); }
        }

        public static string CssDir
        {
            get { return string.Format("{0}/{1}", ContentDir, "Css"); }
        }
  
        public static string ImageUrl(string imageFile)
        {
            string result = string.Format("{0}/{1}", ImageDir, imageFile);
            return result;
        }

        public static string CssUrl(string cssFile)
        {
            string result = string.Format("{0}/{1}", CssDir, cssFile);
            return result;
        }
    }
}
