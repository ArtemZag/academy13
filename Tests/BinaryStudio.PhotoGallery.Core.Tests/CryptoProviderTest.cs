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
        public void PasswordShouldBeEqualWithEncryptedVersion()
        {
            // setup
            const string PASSWORD = "abc123";

            // body
            string salt = cryptoProvider.CalculateSalt();
            string encryptedVersion = cryptoProvider.CreateHashForPassword(PASSWORD, salt);

            // tear down
            bool isEqual = cryptoProvider.IsPasswordsEqual(PASSWORD, encryptedVersion, salt);
            isEqual.Should().Be(true);
        }

        [Test]
        public void PasswordShouldBeNoEqualWithTwiceEncryptedVersion()
        {
            // setup
            const string PASSWORD = "abc123";

            // body
            string salt = cryptoProvider.CalculateSalt();
            string twiceEcryptedVersion = cryptoProvider.CreateHashForPassword(PASSWORD, salt);
            twiceEcryptedVersion = cryptoProvider.CreateHashForPassword(twiceEcryptedVersion, salt);

            // tear down
            bool isEqual = cryptoProvider.IsPasswordsEqual(PASSWORD, twiceEcryptedVersion, salt);
            isEqual.Should().Be(false);
        }
    }
}
