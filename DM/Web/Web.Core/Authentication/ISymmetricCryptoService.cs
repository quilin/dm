namespace Web.Core.Authentication
{
    public interface ISymmetricCryptoService
    {
        string Encrypt(string valueToEncrypt, string keyInBase64, string ivInBase64);
        string Decrypt(string valueToDecrypt, string keyInBase64, string ivInBase64);
    }
}