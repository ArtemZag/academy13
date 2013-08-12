namespace BinaryStudio.PhotoGallery.Domain.Services.Search.FoundItems
{
    public enum ItemType
    {
        Photo,
        User
    }

    public interface IFoundItem
    {
        ItemType Type { get; }

        int Relevance { get; set; }
    }
}