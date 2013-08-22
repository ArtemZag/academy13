using System.Web.Mvc;

namespace BinaryStudio.PhotoGallery.Web
{
    public abstract class BaseViewPage : WebViewPage
    {
        public virtual CustomPrincipal User
        {
            get { return base.User as CustomPrincipal; }
        }
    }

    public abstract class BaseViewPage<TModel> : WebViewPage<TModel>
    {
        public virtual CustomPrincipal User
        {
            get { return base.User as CustomPrincipal; }
        }
    }
}