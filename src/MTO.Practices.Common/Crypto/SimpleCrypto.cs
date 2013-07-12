namespace MTO.Practices.Common.Crypto
{
    using System;
    using System.IO;
    using System.Security.Cryptography;
    using System.Text;

    /// <summary>
    /// Criptografia simples utilizando AES.
    /// Fonte: http://stackoverflow.com/questions/202011/encrypt-decrypt-string-in-net
    /// </summary>
    public class SimpleCrypto
    {
        /// <summary>
        /// Salt default para encriptação
        /// </summary>
        private static readonly byte[] Salt = Encoding.ASCII.GetBytes("o6806642kbM7d5");

        /// <summary>
        /// Encrypt the given string using AES.  The string can be decrypted using 
        /// DecryptStringAES().  The sharedSecret parameters must match.
        /// </summary>
        /// <param name="plainText">The text to encrypt.</param>
        /// <param name="sharedSecret">A password used to generate a key for encryption.</param>
        /// <returns>The encrypted string.</returns>
        public static string EncryptString(string plainText, string sharedSecret)
        {
            if (string.IsNullOrEmpty(plainText))
            {
                throw new ArgumentNullException("plainText");
            }

            if (string.IsNullOrEmpty(sharedSecret))
            {
                throw new ArgumentNullException("sharedSecret");
            }

            string outStr;                       // Encrypted string to return
            RijndaelManaged aesAlg = null;              // RijndaelManaged object used to encrypt the data.

            try
            {
                // generate the key from the shared secret and the salt
                var key = new Rfc2898DeriveBytes(sharedSecret, Salt);

                // Create a RijndaelManaged object
                // with the specified key and IV.
                aesAlg = new RijndaelManaged();
                aesAlg.Key = key.GetBytes(aesAlg.KeySize / 8);
                aesAlg.IV = key.GetBytes(aesAlg.BlockSize / 8);

                // Create a decrytor to perform the stream transform.
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for encryption.
                using (var memoryStream = new MemoryStream())
                {
                    using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        using (var wr = new StreamWriter(cryptoStream))
                        {
                            // Write all data to the stream.
                            wr.Write(plainText);
                        }
                    }

                    outStr = Convert.ToBase64String(memoryStream.ToArray());
                }
            }
            finally
            {
                // Clear the RijndaelManaged object.
                if (aesAlg != null)
                {
                    aesAlg.Clear();
                }
            }

            // Return the encrypted bytes from the memory stream.
            return outStr;
        }

        /// <summary>
        /// Decrypt the given string.  Assumes the string was encrypted using 
        /// EncryptStringAES(), using an identical sharedSecret.
        /// </summary>
        /// <param name="cipherText">The text to decrypt.</param>
        /// <param name="sharedSecret">A password used to generate a key for decryption.</param>
        /// <returns>The decrypted string</returns>
        public static string DecryptString(string cipherText, string sharedSecret)
        {
            if (string.IsNullOrEmpty(cipherText))
            {
                throw new ArgumentNullException("cipherText");
            }

            if (string.IsNullOrEmpty(sharedSecret))
            {
                throw new ArgumentNullException("sharedSecret");
            }

            // Declare the RijndaelManaged object
            // used to decrypt the data.
            RijndaelManaged aesAlg = null;

            // Declare the string used to hold
            // the decrypted text.
            string plaintext;

            try
            {
                // generate the key from the shared secret and the salt
                var key = new Rfc2898DeriveBytes(sharedSecret, Salt);

                // Create a RijndaelManaged object
                // with the specified key and IV.
                aesAlg = new RijndaelManaged();
                aesAlg.Key = key.GetBytes(aesAlg.KeySize / 8);
                aesAlg.IV = key.GetBytes(aesAlg.BlockSize / 8);

                // Create a decrytor to perform the stream transform.
                var decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for decryption.                
                byte[] bytes = Convert.FromBase64String(cipherText);
                using (var memoryStream = new MemoryStream(bytes))
                {
                    using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                    {
                        using (var rd = new StreamReader(cryptoStream))
                        {
                            // Read the decrypted bytes from the decrypting stream
                            // and place them in a string.
                            plaintext = rd.ReadToEnd();
                        }
                    }
                }
            }
            finally
            {
                // Clear the RijndaelManaged object.
                if (aesAlg != null)
                {
                    aesAlg.Clear();
                }
            }

            return plaintext;
        }
    }
}
