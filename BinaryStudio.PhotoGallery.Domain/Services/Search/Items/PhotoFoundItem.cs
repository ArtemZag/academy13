namespace BinaryStudio.PhotoGallery.Domain.Services.Search.Items
{
    public class PhotoFoundItem : IFoundItem
    {
        public int Id { get; set; }

        public int UserModelId { get; set; }

        public int AlbumId { get; set; }

        public string PhotoName { get; set; }

        public ItemType Type
        {
            get { return ItemType.Photo; }
        }

        public int Relevance { get; set; }
    }
}