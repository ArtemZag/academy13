using BinaryStudio.PhotoGallery.Core.NotificationsUtils;
using BinaryStudio.PhotoGallery.Core.UserUtils;
using Microsoft.Practices.Unity;
using NUnit.Framework;
using FluentAssertions;

namespace BinaryStudio.PhotoGalery.Core.Tests
{
    [TestFixture]
    internal class CryptoProviderTest
    {
        private ICryptoProvider cryptoProvider;

        [SetUp]
        public void Setup()
        {
            IUnityContainer container = Bootstrapper.Initialise();
            cryptoProvider = container.Resolve<ICryptoProvider>();
        }

        [Test]
        public void PasswordShouldBeEqualWithEnryptedVersion()
        {
            // setup
            const string PASSWORD = "abc123";

            // body
            string salt = cryptoProvider.Salt;
            string encryptedVersion = cryptoProvider.CreateHashForPassword(PASSWORD, salt);

            // tear down
            bool isEqual = cryptoProvider.IsPasswordsEqual(PASSWORD, encryptedVersion, salt);
            isEqual.Should().Be(true);
        }
    }
}
