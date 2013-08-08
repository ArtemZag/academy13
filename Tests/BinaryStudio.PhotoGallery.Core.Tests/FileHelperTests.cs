using BinaryStudio.PhotoGallery.Core.Helpers;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;

namespace BinaryStudio.PhotoGalery.Core.Tests
{
    [TestFixture]
    public class FileHelperTests
    {
        private IFileHelper _fileHelper;

        [SetUp]
        public void SetUp()
        {
            _fileHelper = Substitute.For<IFileHelper>();
        }

        [Test]
        public void FileShouldBeDetectedAsImageFile()
        {
            // setup
            var imageWithTxtExtension = string.Format(@"{0}\{1}", "Content", "imageAsText.txt");
            var normalImageFile = string.Format(@"{0}\{1}", "Content", "img.jpg");

            // body
            _fileHelper.IsImageFile(imageWithTxtExtension).Returns(true);
            _fileHelper.IsImageFile(normalImageFile).Returns(true);

            // tear down
            _fileHelper.IsImageFile(imageWithTxtExtension).Should().BeTrue();
            _fileHelper.IsImageFile(normalImageFile).Should().BeTrue();
        }

        [Test]
        public void FileShouldBeFailToDetectTextFileAsImageFile()
        {
            // setup
            var emptyFile = string.Format(@"{0}\{1}", "Content", "empty");
            var textFile = string.Format(@"{0}\{1}", "Content", "text.txt");

            // body
            _fileHelper.IsImageFile(emptyFile).Returns(false);
            _fileHelper.IsImageFile(textFile).Returns(false);

            // tear down
            _fileHelper.IsImageFile(emptyFile).Should().BeFalse();
            _fileHelper.IsImageFile(textFile).Should().BeFalse();
        }
    }
}
