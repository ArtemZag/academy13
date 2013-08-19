using BinaryStudio.PhotoGallery.Core.UserUtils;
using FluentAssertions;
using NUnit.Framework;

namespace BinaryStudio.PhotoGallery.Core.Tests
{
    [TestFixture]
    internal class CryptoProviderTests
    {
        private readonly ICryptoProvider cryptoProvider;

        public CryptoProviderTests()
        {
            cryptoProvider = new CryptoProvider();
        }

        [Test]
        public void PasswordShouldBeEqualWithEncryptedVersion()
        {
            // setup
            const string PASSWORD = "abc123";

            // body
            string salt = cryptoProvider.GetNewSalt();
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
            string salt = cryptoProvider.GetNewSalt();
            string twiceEcryptedVersion = cryptoProvider.CreateHashForPassword(PASSWORD, salt);
            twiceEcryptedVersion = cryptoProvider.CreateHashForPassword(twiceEcryptedVersion, salt);

            // tear down
            bool isEqual = cryptoProvider.IsPasswordsEqual(PASSWORD, twiceEcryptedVersion, salt);
            isEqual.Should().Be(false);
        }

        [Test]
        public void SaltsShouldBeNotEqualWithTwiceCalling()
        {
            // body
            string salt1 = cryptoProvider.GetNewSalt();
            string salt2 = cryptoProvider.GetNewSalt();

            // tear down
            bool isEqual = string.Equals(salt1, salt2);
            isEqual.Should().BeFalse();
        }
    }
}