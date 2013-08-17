using System;
using System.Security.Cryptography;
using System.Text;

namespace BinaryStudio.PhotoGallery.Core.UserUtils
{
    public class CryptoProvider : ICryptoProvider
    {
        private const int SALT_SIZE = 32;

        public bool IsPasswordsEqual(string enteredPassword, string encryptedPasswordFromDb, string salt)
        {
            return string.Equals(CreateHashForPassword(enteredPassword, salt), encryptedPasswordFromDb);
        }

        public string GetNewSalt()
        {
            return Randomizer.GetString(SALT_SIZE);
        }

        public string CreateHashForPassword(string password, string salt)
        {
            var hash = GetHash(password);

            hash = GetHash(hash + salt);

            return GetHash(hash + salt);
        }

        public string GetHash(string originalString)
        {
            var md5 = new MD5CryptoServiceProvider();
            var encoding = Encoding.UTF8;

            var endcodedString = md5.ComputeHash(encoding.GetBytes(originalString));

            // ToBase64String function add '==' in the end of encruptedString 
            // There are an excess symbols and are must be deleted
            return Convert.ToBase64String(endcodedString).TrimEnd('=').TrimEnd('=');
        }
    }
}