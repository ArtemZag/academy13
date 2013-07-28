using BinaryStudio.PhotoGallery.Core.Helpers;
using NUnit.Framework;
using FluentAssertions;

namespace BinaryStudio.PhotoGalery.Core.Tests
{
    [TestFixture]
    internal class FileHelperTests
    {
        [Test]
        public void FileShouldBeDetectedAsImageFile()
        {
            // setup
            const string fileName = "somefile.txt";

            // body
            var isImageFile = FileHelper.IsImageFile(fileName);

            // tear down
            isImageFile.Should().BeTrue();
        }
    }
}
