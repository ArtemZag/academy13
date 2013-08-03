using BinaryStudio.PhotoGallery.Core.IOUtils;
using FluentAssertions;
using NUnit.Framework;

namespace BinaryStudio.PhotoGalery.Core.Tests
{
    [TestFixture]
    public class FileWrapperTests
    {
        private FileWrapper _fileWrapper;

        [SetUp]
        public void SetUp()
        {
            _fileWrapper = new FileWrapper();
        }

        [Test]
        public void TwoEqualFilesShouldBeDetectedAsEqual()
        {
            // setup
            const string firstFile = @"Content\img.jpg";
            const string secondFile = @"Content\img.jpg";

            // body
            var filesAreEqual = _fileWrapper.Equals(firstFile, secondFile);

            // tear down
            filesAreEqual.Should().BeTrue();
        }

        [Test]
        public void TwoNotEqualFilesShouldBeDetectedAsNotEqual()
        {
            // setup
            const string firstFile = @"Content\img.jpg";
            const string secondFile = @"Content\text.txt";

            // body
            var filesAreEqual = _fileWrapper.Equals(firstFile, secondFile);

            // tear down
            filesAreEqual.Should().BeFalse();
        }
    }
}
