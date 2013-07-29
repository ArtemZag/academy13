using System.Collections.Generic;
using BinaryStudio.PhotoGallery.Models;

namespace BinaryStudio.PhotoGallery.Domain.Tests.Mocked
{
    internal class MockedContext
    {
        public MockedContext()
        {
            Users = new List<UserModel>();
            Albums = new List<AlbumModel>();
        }

        public List<UserModel> Users { get; set; }
        public List<AlbumModel> Albums { get; set; }
    }
}
