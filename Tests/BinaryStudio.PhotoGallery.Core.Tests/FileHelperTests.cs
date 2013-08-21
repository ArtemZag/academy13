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
        private readonly IFileHelper fileHelper;
        private readonly IFileWrapper fileWrapper;

        public FileHelperTests()
        {
            fileWrapper = Substitute.For<IFileWrapper>();
            fileHelper = new FileHelper(fileWrapper);
        }

        [Test]
        public void FileShouldBeDetectedAsImageFile()
        {
            // setup
            var imageWithTxtExtension = string.Format(@"{0}\{1}", "Content", "imageAsText.txt");
            var normalImageFile = string.Format(@"{0}\{1}", "Content", "img.jpg");
            
            fileWrapper.Exists(null).ReturnsForAnyArgs(true);

            // body
            var realImageFileWithTextExtension = fileHelper.IsImageFile(imageWithTxtExtension);
            var realImageFileWithNormalExtension = fileHelper.IsImageFile(normalImageFile);

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

            fileWrapper.Exists(null).ReturnsForAnyArgs(true);

            // body
            var emptyFileIsNotAnImageFile = fileHelper.IsImageFile(emptyFile);
            var textFileIsNotAnImageFile = fileHelper.IsImageFile(textFile);

            // tear down
            emptyFileIsNotAnImageFile.Should().BeFalse();
            textFileIsNotAnImageFile.Should().BeFalse();
        }

        [Test]
        public void TwoEqualFilesShouldBeDetectedAsEqual()
        {
            // setup
            const string FIRST_FILE = @"Content\img.jpg";
            const string SECOND_FILE = @"Content\img.jpg";

            // body
            fileHelper.Equals(FIRST_FILE, SECOND_FILE);

            // tear down
            fileHelper.Equals(FIRST_FILE, SECOND_FILE).Should().BeTrue();
        }

        [Test]
        public void TwoNotEqualFilesShouldBeDetectedAsNotEqual()
        {
            // setup
            const string FIRST_FILE = @"Content\img.jpg";
            const string SECOND_FILE = @"Content\text.txt";

            // body
            var filesAreEqual = fileHelper.Equals(FIRST_FILE, SECOND_FILE);

            // tear down
            filesAreEqual.Should().BeFalse();
        }
    }
}
