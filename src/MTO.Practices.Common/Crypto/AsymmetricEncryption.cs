namespace MTO.Practices.Common.Crypto
{
    using System;
    using System.Security.Cryptography;
    using System.Text;

    /// <summary>
    /// Critografia assimétrica usando RSA
    /// </summary>
    /// <remarks>
    /// Adaptado de http://www.csharpdeveloping.net/Snippet/how_to_encrypt_decrypt_using_asymmetric_algorithm_rsa
    /// </remarks>
    public static class AsymmetricEncryption
    {
        /// <summary>
        /// Exclui padding na criptografia
        /// </summary>
        private const bool OptimalAsymmetricEncryptionPadding = false;

        /// <summary>
        /// Cria chave publica e privada
        /// </summary>
        /// <param name="keySize">tamanho das chaves</param>
        /// <param name="publicKey">chave publica (saida)</param>
        /// <param name="publicAndPrivateKey">chave privada (saida)</param>
        public static void GenerateKeys(int keySize, out string publicKey, out string publicAndPrivateKey)
        {
            using (var provider = new RSACryptoServiceProvider(keySize))
            {
                publicKey = provider.ToXmlString(false);
                publicAndPrivateKey = provider.ToXmlString(true);
            }
        }

        /// <summary>
        /// Criptografa texto
        /// </summary>
        /// <param name="text">texto a ser encriptado</param>
        /// <param name="publicKeyXml">xml com o conteúdo da chave</param>
        /// <returns>texto encriptografado</returns>
        public static string EncryptText(string text, string publicKeyXml)
        {
            var encrypted = Encrypt(Encoding.UTF8.GetBytes(text), publicKeyXml);
            return Convert.ToBase64String(encrypted);
        }

        /// <summary>
        /// Criptografa texto (em bytes)
        /// </summary>
        /// <param name="data">dados a serem encriptados</param>
        /// <param name="publicKeyXml">xml da chave</param>
        /// <returns>byte array com os dados encriptados</returns>
        public static byte[] Encrypt(byte[] data, string publicKeyXml)
        {
            if (data == null || data.Length == 0)
            {
                throw new ArgumentException("Data are empty", "data");
            }

            if (string.IsNullOrEmpty(publicKeyXml))
            {
                throw new ArgumentException("Key is null or empty", "publicKeyXml");
            }

            using (var provider = new RSACryptoServiceProvider())
            {
                provider.FromXmlString(publicKeyXml);
                return provider.Encrypt(data, OptimalAsymmetricEncryptionPadding);
            }
        }

        /// <summary>
        /// Descriptografa texto
        /// </summary>
        /// <param name="text">texto a ser decodificado</param>
        /// <param name="publicAndPrivateKeyXml">xml com a chave privada</param>
        /// <returns>texto descriptografado</returns>
        public static string DecryptText(string text, string publicAndPrivateKeyXml)
        {
            var decrypted = Decrypt(Convert.FromBase64String(text), publicAndPrivateKeyXml);
            return Encoding.UTF8.GetString(decrypted);
        }

        /// <summary>
        /// Descriptografa array de bytes
        /// </summary>
        /// <param name="data">dados a serem decodificados</param>
        /// <param name="publicAndPrivateKeyXml">conteúdo da chave</param>
        /// <returns>dados decodificados</returns>
        public static byte[] Decrypt(byte[] data, string publicAndPrivateKeyXml)
        {
            if (data == null || data.Length == 0)
            {
                throw new ArgumentException("Data are empty", "data");
            }

            if (string.IsNullOrEmpty(publicAndPrivateKeyXml))
            {
                throw new ArgumentException("Key is null or empty", "publicAndPrivateKeyXml");
            }

            using (var provider = new RSACryptoServiceProvider())
            {
                provider.FromXmlString(publicAndPrivateKeyXml);
                return provider.Decrypt(data, OptimalAsymmetricEncryptionPadding);
            }
        }
    }
}
