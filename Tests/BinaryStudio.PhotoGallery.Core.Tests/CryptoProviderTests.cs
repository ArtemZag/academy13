using BinaryStudio.PhotoGallery.Core.UserUtils;
using Microsoft.Practices.Unity;
using NUnit.Framework;
using FluentAssertions;

namespace BinaryStudio.PhotoGalery.Core.Tests
{
    [TestFixture]
    internal class CryptoProviderTests
    {
        private ICryptoProvider _cryptoProvider;

        [SetUp]
        public void Setup()
        {
            IUnityContainer container = Bootstrapper.Initialise();
            _cryptoProvider = container.Resolve<ICryptoProvider>();
        }

        [Test]
        public void PasswordShouldBeEqualWithEncryptedVersion()
        {
            // setup
            const string PASSWORD = "abc123";

            // body
            string salt = _cryptoProvider.GetNewSalt();
            string encryptedVersion = _cryptoProvider.CreateHashForPassword(PASSWORD, salt);

            // tear down
            bool isEqual = _cryptoProvider.IsPasswordsEqual(PASSWORD, encryptedVersion, salt);
            isEqual.Should().Be(true);
        }

        [Test]
        public void PasswordShouldBeNoEqualWithTwiceEncryptedVersion()
        {
            // setup
            const string PASSWORD = "abc123";

            // body
            string salt = _cryptoProvider.GetNewSalt();
            string twiceEcryptedVersion = _cryptoProvider.CreateHashForPassword(PASSWORD, salt);
            twiceEcryptedVersion = _cryptoProvider.CreateHashForPassword(twiceEcryptedVersion, salt);

            // tear down
            bool isEqual = _cryptoProvider.IsPasswordsEqual(PASSWORD, twiceEcryptedVersion, salt);
            isEqual.Should().Be(false);
        }

        [Test]
        public void SaltsShouldBeNotEqualWithTwiceCalling()
        {
            // body
            string salt1 = _cryptoProvider.GetNewSalt();
            string salt2 = _cryptoProvider.GetNewSalt();

            // tear down
            bool isEqual = string.Equals(salt1, salt2);
            isEqual.Should().BeFalse();
        }
    }
}
