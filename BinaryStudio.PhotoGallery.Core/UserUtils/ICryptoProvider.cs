namespace BinaryStudio.PhotoGallery.Core.UserUtils
{
    public interface ICryptoProvider
    {
        string CalculateSalt();

        string CreateHashForPassword(string password, string salt);

        bool IsPasswordsEqual(string enteredPassword, string encryptedPasswordFromDb, string salt);
    }
}
