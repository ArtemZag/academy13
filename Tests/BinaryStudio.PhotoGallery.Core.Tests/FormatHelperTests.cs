using BinaryStudio.PhotoGallery.Core.Helpers;
using NUnit.Framework;
using FluentAssertions;

namespace BinaryStudio.PhotoGalery.Core.Tests
{
    [TestFixture]
    internal class FormatHelperTests
    {
        [Test]
        public void FileShouldBeDetectedAsImageFile()
        {
            // setup
            var imageWithTxtExtension = string.Format(@"{0}\{1}", "Content", "imageAsText.txt");
            var normalImageFile = string.Format(@"{0}\{1}", "Content", "img.jpg");

            // body
            var isTextFormatReallyImage = FormatHelper.IsImageFile(imageWithTxtExtension);
            var isImageFormatReallyImage = FormatHelper.IsImageFile(normalImageFile);

            // tear down
            isTextFormatReallyImage.Should().BeTrue();
            isImageFormatReallyImage.Should().BeTrue();
        }

        [Test]
        public void FileShouldBeFailToDetectTextFileAsImageFile()
        {
            // setup
            var emptyFile = string.Format(@"{0}\{1}", "Content", "empty");
            var textFile = string.Format(@"{0}\{1}", "Content", "text.txt");

            // body
            var emptyFileIsImage = FormatHelper.IsImageFile(emptyFile);
            var textFileIsImage = FormatHelper.IsImageFile(textFile);

            // tear down
            emptyFileIsImage.Should().BeFalse();
            textFileIsImage.Should().BeFalse();
        }
    }
}
