using System.Globalization;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using BinaryStudio.PhotoGallery.Core.Extensions;

namespace BinaryStudio.PhotoGallery.Web.Extensions
{
    public static class HtmlHelperExtentions
    {
        /// <summary>
        ///     Returns code of link tag using provided values.
        /// </summary>
        /// <param name="helper">HTML helper instance.</param>
        /// <param name="linkText">The inner text of the anchor element.</param>
        /// <param name="actionName">The name of the action.</param>
        /// <param name="controllerName">The name of the controll.</param>
        /// <param name="htmlAttributes">Additional HTML attributes (can be specified via properties of anonymous object).</param>
        /// <returns>HTML code of link tag.</returns>
        public static MvcHtmlString ActionLink(
            this HtmlHelper helper,
            string linkText,
            string actionName,
            string controllerName,
            object htmlAttributes = null)
        {
            return
                new MvcHtmlString(
                    helper.ActionLink(linkText, actionName, controllerName, null, htmlAttributes)
                        .ToHtmlString());
        }

        /// <summary>
        ///     Returns code of link tag using provided values.
        /// </summary>
        /// <param name="helper">HTML helper instance.</param>
        /// <param name="imageSrc">Path to image.</param>
        /// <param name="actionName">The name of the action.</param>
        /// <param name="controllerName">The name of the controll.</param>
        /// <param name="htmlAttributes">Additional HTML attributes (can be specified via properties of anonymous object).</param>
        /// <returns>HTML code of link tag with image inside.</returns>
        public static MvcHtmlString ImageLink(
            this HtmlHelper helper,
            string imageSrc,
            string actionName,
            string controllerName,
            object htmlAttributes = null)
        {
            var imageTag = new TagBuilder("img");

            var urlHelper = new UrlHelper(helper.ViewContext.RequestContext);

            imageTag.Attributes.Add("src", urlHelper.Content(imageSrc));

            var actionLink = helper.ActionLink("[replaceInnerHTML]", actionName, controllerName, null, htmlAttributes)
                    .ToHtmlString();

            return new MvcHtmlString(actionLink.Replace("[replaceInnerHTML]", imageTag.ToString()));
        }

        /// <summary>
        ///     Returns code of link tag using provided values.
        /// </summary>
        /// <param name="helper">HTML helper instance.</param>
        /// <param name="imageSrc">Path to image.</param>
        /// <param name="linkHref">Path for link</param>
        /// <param name="htmlAttributes">Additional HTML attributes (can be specified via properties of anonymous object).</param>
        /// <returns>HTML code of link tag with image inside.</returns>
        public static MvcHtmlString ImageLink(
            this HtmlHelper helper,
            string imageSrc,
            string linkHref,
            object htmlAttributes = null)
        {
            var imageTag = new TagBuilder("img");

            var urlHelper = new UrlHelper(helper.ViewContext.RequestContext);

            imageTag.Attributes.Add("src", urlHelper.Content(imageSrc));

            var linkTag = new TagBuilder("a")
            {
                InnerHtml = imageTag.ToString()
            };
            linkTag.Attributes.Add("href", linkHref);
            linkTag.MergeAttributes(htmlAttributes.ToDictionary(), false);
            

            return MvcHtmlString.Create(linkTag.ToString(TagRenderMode.Normal));
        }

        /// <summary>
        ///     Returns code of image tag using provided values.
        /// </summary>
        /// <typeparam name="T">Type of view model.</typeparam>
        /// <param name="helper">HTML helper instance.</param>
        /// <param name="imageSrc">Path to image.</param>
        /// <param name="width">Width of image.</param>
        /// <param name="height">Height of image.</param>
        /// <param name="alt">Value for "alt" attribute.</param>
        /// <param name="htmlAttributes">Additional HTML attributes (can be specified via properties of anonymous object).</param>
        /// <returns>HTML code of image tag.</returns>
        public static MvcHtmlString Image<T>(
            this HtmlHelper<T> helper,
            string imageSrc,
            int width,
            int height,
            string alt,
            object htmlAttributes = null)
        {
            var imageTag = new TagBuilder("img");

            var urlHelper = new UrlHelper(helper.ViewContext.RequestContext);

            imageTag.Attributes.Add("src", urlHelper.Content(imageSrc));

            imageTag.Attributes.Add("width", width.ToString(CultureInfo.InvariantCulture));
            imageTag.Attributes.Add("height", height.ToString(CultureInfo.InvariantCulture));
            imageTag.Attributes.Add("alt", alt);
            imageTag.MergeAttributes(htmlAttributes.ToDictionary(), false);

            return MvcHtmlString.Create(imageTag.ToString(TagRenderMode.SelfClosing));
        }

        /// <summary>
        ///     Returns code of image tag using provided values.
        /// </summary>
        /// <typeparam name="T">Type of view model.</typeparam>
        /// <param name="helper">HTML helper instance.</param>
        /// <param name="imageSrc">Path to image.</param>
        /// <param name="htmlAttributes">Additional HTML attributes (can be specified via properties of anonymous object).</param>
        /// <returns>HTML code of image tag.</returns>
        public static MvcHtmlString Image<T>(this HtmlHelper<T> helper, string imageSrc, object htmlAttributes = null)
        {
            var imageTag = new TagBuilder("img");

            var urlHelper = new UrlHelper(helper.ViewContext.RequestContext);

            imageTag.Attributes.Add("src", urlHelper.Content(imageSrc));

            imageTag.MergeAttributes(htmlAttributes.ToDictionary(), false);

            return MvcHtmlString.Create(imageTag.ToString(TagRenderMode.SelfClosing));
        }
    }
}