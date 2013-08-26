using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Security.Policy;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace BinaryStudio.PhotoGallery.Web.Infrastructure.Extensions
{
    public static class HtmlHelperExtentions
    {
        /// <summary>
        /// Renders ActionLink with any html in content.
        /// </summary>
        /// <param name="helper">HTML helper instance.</param>
        /// <param name="action">The action name.</param>
        /// <param name="controller">The controller name.</param>
        /// <param name="innerContent">The inner tag.</param>
        /// <param name="htmlAttributes">Additional HTML attributes (can be specified via properties of anonymous object).</param>
        /// <returns>Html code with link tag and tag in it content.</returns>
        public static string ActionLink(this HtmlHelper helper, string action, string controller, MvcHtmlString innerContent, object htmlAttributes = null)
        {
            var link = helper.ActionLink("[replaceInnerHTML]", action, controller, htmlAttributes).ToHtmlString();
            return link.Replace("[replaceInnerHTML]", innerContent.ToString());
        }

        /// <summary>
        /// Returns code of image tag using provided values.
        /// </summary>
        /// <typeparam name="T">Type of view model.</typeparam>
        /// <param name="helper">HTML helper instance.</param>
        /// <param name="src">Path to image.</param>
        /// <param name="width">Width of image.</param>
        /// <param name="height">Height of image.</param>
        /// <param name="alt">Value for "alt" attribute.</param>
        /// <param name="htmlAttributes">Additional HTML attributes (can be specified via properties of anonymous object).</param>
        /// <returns>HTML code of image tag.</returns>
        public static MvcHtmlString Image<T>(this HtmlHelper<T> helper, string src, int width, int height, string alt, object htmlAttributes = null)
        {
            var tag = new TagBuilder("img");

            tag.Attributes.Add("src", src);
            tag.Attributes.Add("width", width.ToString(CultureInfo.InvariantCulture));
            tag.Attributes.Add("height", height.ToString(CultureInfo.InvariantCulture));
            tag.Attributes.Add("alt", alt);
            tag.MergeAttributes(htmlAttributes.ToDictionary(), false);

            return MvcHtmlString.Create(tag.ToString(TagRenderMode.SelfClosing));
        }

        public static IDictionary<string, object> ToDictionary(this object source, BindingFlags bindingAttr = BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance)
        {
            return source.GetType().GetProperties(bindingAttr).ToDictionary
            (
                propInfo => propInfo.Name,
                propInfo => propInfo.GetValue(source, null)
            );

        }

    }
}
