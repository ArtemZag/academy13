using System.Collections.Generic;
using BinaryStudio.PhotoGallery.Models;

namespace BinaryStudio.PhotoGallery.Domain.Tests.Mocked
{
    internal class MockedContext
    {
        public MockedContext()
        {
            Users = new List<UserModel>();
        }

        public List<UserModel> Users { get; set; } 
    }
}
