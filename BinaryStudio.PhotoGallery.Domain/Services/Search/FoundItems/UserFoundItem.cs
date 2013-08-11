namespace BinaryStudio.PhotoGallery.Domain.Services.Search.FoundItems
{
    public class UserFoundItem : IFoundItem
    {
        /// <summary>
        ///     Users name, that contains FirstName, SecondName and NickName
        /// </summary>
        public string Name { get; set; }

        public string Department { get; set; }

        public bool IsOnline { get; set; }

        public ItemType Type
        {
            get { return ItemType.User; }
        }

        public int Relevance { get; set; }
    }
}