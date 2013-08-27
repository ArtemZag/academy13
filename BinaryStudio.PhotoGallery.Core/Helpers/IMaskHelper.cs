namespace BinaryStudio.PhotoGallery.Core.Helpers
{
    public interface IMaskHelper
    {
        bool IsSet<T>(T flags, T flag) where T : struct;
        void Set<T>(ref T flags, T flag) where T : struct;
        void Unset<T>(ref T flags, T flag) where T : struct;
    }
}