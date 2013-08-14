namespace BinaryStudio.PhotoGallery.Domain.Services.Search.Results
{
    public interface IFound
    {
        ItemType Type { get; }

        int Relevance { get; set; }
    }
}