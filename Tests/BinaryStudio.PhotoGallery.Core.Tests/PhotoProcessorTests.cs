using BinaryStudio.PhotoGallery.Core.PhotoUtils;
using NUnit.Framework;
using System.Drawing;
using FluentAssertions;

namespace BinaryStudio.PhotoGalery.Core.Tests
{
    [TestFixture]
    public class PhotoProcessorTests
    {
        [Test]
        public void ResizedImageShouldBeCorrect()
        {
            Image testImage = new Bitmap(1000,500);
            Image resizedImage = PhotoProcessor.ImageResize(testImage, 250);
            resizedImage.Should().NotBeNull();
            resizedImage.Height.Should().Be(250);
            resizedImage.Width.Should().Be(500);
        }
    }
}
