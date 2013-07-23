using System;
using System.Security.Cryptography;
using System.Text;

namespace BinaryStudio.PhotoGallery.Core.UserUtils
{
    public class CryptoProvider : ICryptoProvider
    {
        private const string SOLT = "JQWZueQCkJ";

        public string Solt
        {
            get { return SOLT; }
        }

        public bool IsPasswordsEqual(string enteredPassword, string solt, string encryptedPasswordFromDb)
        {
            return string.Equals(CreateHashForPassword(enteredPassword, solt), encryptedPasswordFromDb);
        }

        public string CreateHashForPassword(string password, string solt)
        {
            string hash = EncryptString(password);

            hash = EncryptString(hash + solt);

            return EncryptString(hash + solt);
        }

        private string EncryptString(string originalString)
        {
            var md5 = new MD5CryptoServiceProvider();
            Encoding encoding = Encoding.UTF8;

            byte[] endcodedString = md5.ComputeHash(encoding.GetBytes(originalString));

            return Convert.ToBase64String(endcodedString);
        }
    }
}