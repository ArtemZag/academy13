namespace BinaryStudio.PhotoGallery.Domain.Services.Search.Items
{
    public class PhotoFoundItem : IFoundItem
    {
        public string PhotoName { get; set; }

        public ItemType Type
        {
            get { return ItemType.Photo; }
        }

        public int Relevance { get; set; }
    }
}