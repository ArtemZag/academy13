namespace BinaryStudio.PhotoGallery.Core.UserUtils
{
    public interface ICryptoProvider
    {
        string EncryptString(string originalString);

        string CreateHashForPassword(string password, string solt);

        bool IsPasswordsEqual(string enteredPassword, string encryptedPasswordFromDb, string solt);
    }
}
