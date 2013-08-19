using System.Collections.Generic;
using System.Diagnostics;

namespace BinaryStudio.PhotoGallery.Core.EnumerableExtensions
{
    public static class Extensions
    {
        public static void ToDebugOutput<TType>(this IEnumerable<TType> elements) 
        {
            foreach (TType element in elements)
            {
                Debug.WriteLine(element);
            }
        }
    }
}
