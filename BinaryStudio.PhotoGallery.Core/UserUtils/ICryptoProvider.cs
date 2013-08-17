namespace BinaryStudio.PhotoGallery.Core.UserUtils
{
    public interface ICryptoProvider
    {
        string GetNewSalt();

        string CreateHashForPassword(string password, string salt);

        string GetHash(string originalString);

        bool IsPasswordsEqual(string enteredPassword, string encryptedPasswordFromDb, string salt);
    }
}
