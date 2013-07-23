using System;
using System.Security.Cryptography;
using System.Text;

namespace BinaryStudio.PhotoGallery.Core.UserUtils
{
    public class CryptoProvider : ICryptoProvider
    {
        public string EncryptString(string originalString)
        {
            var md5 = new MD5CryptoServiceProvider();

            var encoding = Encoding.UTF8;

            var endcodedString = md5.ComputeHash(encoding.GetBytes(originalString));

            return Convert.ToBase64String(endcodedString);
        }

        public string CreateHashForPassword(string password, string solt)
        {
            var hash = this.EncryptString(password);

            hash = this.EncryptString(hash + solt);

            return this.EncryptString(hash + solt);
        }

        public bool IsPasswordsEqual(string enteredPassword, string solt, string encryptedPasswordFromDb)
        {
            return this.CreateHashForPassword(enteredPassword, solt) == encryptedPasswordFromDb;
        }
    }
}
