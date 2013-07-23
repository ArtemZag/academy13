namespace BinaryStudio.PhotoGallery.Core.UserUtils
{
    public interface ICryptoProvider
    {
        string Solt { get; }

        string CreateHashForPassword(string password, string solt);

        bool IsPasswordsEqual(string enteredPassword, string encryptedPasswordFromDb, string solt);
    }
}
