using System.Diagnostics;
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
            imageTag.Attributes.Add("src", imageSrc);

            string actionLink =
                helper.ActionLink("[replaceInnerHTML]", actionName, controllerName, null, htmlAttributes)
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
            imageTag.Attributes.Add("src", imageSrc);

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
        /// <param name="src">Path to image.</param>
        /// <param name="width">Width of image.</param>
        /// <param name="height">Height of image.</param>
        /// <param name="alt">Value for "alt" attribute.</param>
        /// <param name="htmlAttributes">Additional HTML attributes (can be specified via properties of anonymous object).</param>
        /// <returns>HTML code of image tag.</returns>
        public static MvcHtmlString Image<T>(
            this HtmlHelper<T> helper,
            string src,
            int width,
            int height,
            string alt,
            object htmlAttributes = null)
        {
            var tag = new TagBuilder("img");

            tag.Attributes.Add("src", src);
            tag.Attributes.Add("width", width.ToString(CultureInfo.InvariantCulture));
            tag.Attributes.Add("height", height.ToString(CultureInfo.InvariantCulture));
            tag.Attributes.Add("alt", alt);
            tag.MergeAttributes(htmlAttributes.ToDictionary(), false);

            return MvcHtmlString.Create(tag.ToString(TagRenderMode.SelfClosing));
        }

        /// <summary>
        ///     Returns code of image tag using provided values.
        /// </summary>
        /// <typeparam name="T">Type of view model.</typeparam>
        /// <param name="helper">HTML helper instance.</param>
        /// <param name="src">Path to image.</param>
        /// <param name="htmlAttributes">Additional HTML attributes (can be specified via properties of anonymous object).</param>
        /// <returns>HTML code of image tag.</returns>
        public static MvcHtmlString Image<T>(this HtmlHelper<T> helper, string src, object htmlAttributes = null)
        {
            var tag = new TagBuilder("img");

            tag.Attributes.Add("src", src);
            tag.MergeAttributes(htmlAttributes.ToDictionary(), false);

            return MvcHtmlString.Create(tag.ToString(TagRenderMode.SelfClosing));
        }
    }
}