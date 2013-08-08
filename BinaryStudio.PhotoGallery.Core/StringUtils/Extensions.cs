namespace BinaryStudio.PhotoGallery.Core.StringUtils
{
    public static class Extensions
    {
        public static string[] SplitBySpace(this string value)
        {
            return value.Split(' ');
        }
    }
}
