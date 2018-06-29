using System;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace ContainerCrypto
{
    public class CertStringCryptoService : IStringCryptoService
    {
        RSA publicKey;
        RSA privateKey;

        public string Decrypt(string base64EncodedTextToDecrypt)
        {
            EnsurePrivateKey();
            var contentToDecrypt = Convert.FromBase64String(base64EncodedTextToDecrypt);
            var decryptedBytes = privateKey.Decrypt(contentToDecrypt, RSAEncryptionPadding.Pkcs1);
            return Encoding.Default.GetString(decryptedBytes);
        }

        public string Encrypt(string plainTextToEncrypt)
        {
            EnsurePublicKey();
            var contentToEncrypt = Encoding.Default.GetBytes(plainTextToEncrypt);
            var encryptedBytes = publicKey.Encrypt(contentToEncrypt, RSAEncryptionPadding.Pkcs1);
            return Convert.ToBase64String(encryptedBytes);
        }

        private void EnsurePublicKey()
        {
            if (publicKey == null)
            {
                try
                {
                    LoadKeysFromCertificateFile();
                    if (publicKey == null)
                    {
                        throw new CryptographicException("Public key cannot be loaded.");
                    }
                }
                catch (Exception ex)
                {
                    throw new CryptographicException("Could not load keys from certificate file.", ex);
                }
            }
        }

        private void EnsurePrivateKey()
        {
            if (privateKey == null)
            {
                try
                {
                    LoadKeysFromCertificateFile();
                    if (privateKey == null)
                    {
                        throw new CryptographicException("Private key cannot be loaded.");
                    }
                }
                catch (Exception ex)
                {
                    throw new CryptographicException("Could not load keys from certificate file.", ex);
                }
            }
        }

        private void LoadKeysFromCertificateFile()
        {
            // Get password from file
            string password = File.ReadAllLines(
                EnvironmentVariables.FullPathToCertificatePfxPasswordFile,
                Encoding.Default)[0];
            password = password.Replace("\0", string.Empty);

            // Load certificate from file
            X509Certificate2 certificate = new X509Certificate2(
                EnvironmentVariables.FullPathToCertificatePfxFile, password);

            publicKey = certificate.GetRSAPublicKey();
            privateKey = certificate.GetRSAPrivateKey();
        }
    }
}
