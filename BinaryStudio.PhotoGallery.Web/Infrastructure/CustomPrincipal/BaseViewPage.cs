using System.Web.Mvc;

namespace BinaryStudio.PhotoGallery.Web
{
    public abstract class BaseViewPage : WebViewPage
    {
        public new virtual CustomPrincipal User
        {
            get { return base.User as CustomPrincipal; }
        }
    }

    public abstract class BaseViewPage<TModel> : WebViewPage<TModel>
    {
        public new virtual CustomPrincipal User
        {
            get { return base.User as CustomPrincipal; }
        }
    }
}