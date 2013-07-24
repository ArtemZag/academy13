using System;
using System.Security.Cryptography;
using System.Text;

namespace BinaryStudio.PhotoGallery.Core.UserUtils
{
    public class CryptoProvider : ICryptoProvider
    {
        private const int SaltSize = 32;

        public string Salt
        {
            get { return RandomString(SaltSize); }
        }

        public bool IsPasswordsEqual(string enteredPassword, string encryptedPasswordFromDb, string salt)
        {
            return string.Equals(CreateHashForPassword(enteredPassword, salt), encryptedPasswordFromDb);
        }

        public string CreateHashForPassword(string password, string salt)
        {
            string hash = EncryptString(password);

            hash = EncryptString(hash + salt);

            return EncryptString(hash + salt);
        }

        private string EncryptString(string originalString)
        {
            var md5 = new MD5CryptoServiceProvider();
            Encoding encoding = Encoding.UTF8;

            byte[] endcodedString = md5.ComputeHash(encoding.GetBytes(originalString));

            return Convert.ToBase64String(endcodedString);
        }


        // This block of code generates a random string with settable length
        #region Random string generator
        private readonly Random _random = new Random();
        private const string Chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz1234567890";

        /// <summary>
        /// Generates a random string with settable length
        /// </summary>
        /// <param name="size">Length of random string</param>
        /// <returns></returns>
        private string RandomString(int size)
        {
            var stringBuilder = new StringBuilder(size);

            for (var i = 0; i < size; i++)
            {
                stringBuilder.Append(Chars[_random.Next(Chars.Length)]);
            }
            return stringBuilder.ToString();
        }
        #endregion
    }
}