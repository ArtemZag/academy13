using BinaryStudio.PhotoGallery.Core.Helpers;
using BinaryStudio.PhotoGallery.Core.IOUtils;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;

namespace BinaryStudio.PhotoGallery.Core.Tests
{
    [TestFixture]
    public class FileHelperTests
    {
        private readonly IFileHelper _fileHelper;
        private readonly IFileWrapper _fileWrapper;

        public FileHelperTests()
        {
            _fileWrapper = Substitute.For<IFileWrapper>();
            _fileHelper = new FileHelper(_fileWrapper);
        }

        [Test]
        public void FileShouldBeDetectedAsImageFile()
        {
            // setup
            var imageWithTxtExtension = string.Format(@"{0}\{1}", "Content", "imageAsText.txt");
            var normalImageFile = string.Format(@"{0}\{1}", "Content", "img.jpg");
            
            _fileWrapper.Exists(null).ReturnsForAnyArgs(true);

            // body
            var realImageFileWithTextExtension = _fileHelper.IsImageFile(imageWithTxtExtension);
            var realImageFileWithNormalExtension = _fileHelper.IsImageFile(normalImageFile);

            // tear down
            realImageFileWithTextExtension.Should().BeTrue();
            realImageFileWithNormalExtension.Should().BeTrue();
        }

        [Test]
        public void FileShouldBeFailToDetectTextFileAsImageFile()
        {
            // setup
            var emptyFile = string.Format(@"{0}\{1}", "Content", "empty");
            var textFile = string.Format(@"{0}\{1}", "Content", "text.txt");

            _fileWrapper.Exists(null).ReturnsForAnyArgs(true);

            // body
            var emptyFileIsNotAnImageFile = _fileHelper.IsImageFile(emptyFile);
            var textFileIsNotAnImageFile = _fileHelper.IsImageFile(textFile);

            // tear down
            emptyFileIsNotAnImageFile.Should().BeFalse();
            textFileIsNotAnImageFile.Should().BeFalse();
        }

        [Test]
        public void TwoEqualFilesShouldBeDetectedAsEqual()
        {
            // setup
            const string firstFile = @"Content\img.jpg";
            const string secondFile = @"Content\img.jpg";

            // body
            _fileHelper.Equals(firstFile, secondFile);

            // tear down
            _fileHelper.Equals(firstFile, secondFile).Should().BeTrue();
        }

        [Test]
        public void TwoNotEqualFilesShouldBeDetectedAsNotEqual()
        {
            // setup
            const string firstFile = @"Content\img.jpg";
            const string secondFile = @"Content\text.txt";

            // body
            var filesAreEqual = _fileHelper.Equals(firstFile, secondFile);

            // tear down
            filesAreEqual.Should().BeFalse();
        }
    }
}
