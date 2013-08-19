using System;

namespace BinaryStudio.PhotoGallery.Domain.Services.Search.Results
{
    public class CommentFound : IFound
    {
        public int Id { get; set; }

        public int OwnerId { get; set; }

        public DateTime DateOfCreation { get; set; }

        public string Text { get; set; }

        public ItemType Type
        {
            get { return ItemType.Comment; }
        }

        public int Relevance { get; set; }
    }
}