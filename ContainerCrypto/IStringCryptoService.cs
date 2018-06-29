namespace ContainerCrypto
{
    public interface IStringCryptoService
    {
        string Encrypt(string plainTextToEncrypt);

        string Decrypt(string base64EncodedTextToDecrypt);
    }
}
