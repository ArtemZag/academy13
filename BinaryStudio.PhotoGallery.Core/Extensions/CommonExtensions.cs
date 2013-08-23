using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace BinaryStudio.PhotoGallery.Core.Extensions
{
    public static class CommonExtensions
    {
        public static IDictionary<string, object> ToDictionary(this object source,
            BindingFlags bindingAttr = BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance)
        {
            if (source == null)
            {
                return new Dictionary<string, object>();
            }

            return source.GetType().GetProperties(bindingAttr).ToDictionary
                (
                    propInfo => propInfo.Name,
                    propInfo => propInfo.GetValue(source, null)
                );
        }
    }
}