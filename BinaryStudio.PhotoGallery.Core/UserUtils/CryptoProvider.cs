using System;
using System.Security.Cryptography;
using System.Text;
using BinaryStudio.PhotoGallery.Core.Helpers;

namespace BinaryStudio.PhotoGallery.Core.UserUtils
{
    internal class CryptoProvider : ICryptoProvider
    {
        private const int SALT_SIZE = 32;

        private readonly Randomizer _randomizer = new Randomizer();

        public bool IsPasswordsEqual(string enteredPassword, string encryptedPasswordFromDb, string salt)
        {
            return string.Equals(CreateHashForPassword(enteredPassword, salt), encryptedPasswordFromDb);
        }

        public string GetNewSalt()
        {
            return _randomizer.GetString(SALT_SIZE);
        }

        public string CreateHashForPassword(string password, string salt)
        {
            var hash = EncryptString(password);

            hash = EncryptString(hash + salt);

            return EncryptString(hash + salt);
        }

        private static string EncryptString(string originalString)
        {
            var md5 = new MD5CryptoServiceProvider();
            var encoding = Encoding.UTF8;

            var endcodedString = md5.ComputeHash(encoding.GetBytes(originalString));

            return Convert.ToBase64String(endcodedString);
        }
    }
}