namespace BinaryStudio.PhotoGallery.Core.UserUtils
{
    public interface ICryptoProvider
    {
        string Salt { get; }

        string CreateHashForPassword(string password, string salt);

        bool IsPasswordsEqual(string enteredPassword, string encryptedPasswordFromDb, string salt);

    }
}
