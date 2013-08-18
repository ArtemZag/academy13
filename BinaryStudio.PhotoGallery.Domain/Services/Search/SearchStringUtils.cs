using System;
using System.Collections.Generic;
using System.Linq;

namespace BinaryStudio.PhotoGallery.Domain.Services.Search
{
    internal static class SearchStringUtils
    {
        /// <summary>
        ///     Split by space and ToLower for each element.
        /// </summary>
        public static IEnumerable<string> SplitSearchString(this string searchString)
        {
            return searchString.Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries)
                .Distinct()
                .Select(s => s.ToLower());
        }
    }
}