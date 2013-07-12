namespace MTO.Practices.Common.Extensions
{
    using System;
    using System.IO;
    using System.IO.Compression;
    using System.Text;

    /// <summary>
    /// Extensões para compactar e descompactar uma string
    /// </summary>
    public static class GZipExtensions
    {
        /// <summary>
        /// Converte uma string para um array de bytes comprimido
        /// </summary>
        /// <param name="text">texto a ser comprimido</param>
        /// <returns>array de bytes com o texto comprimido</returns>
        public static byte[] CompressGZip(this string text)
        {
            byte[] raw = Encoding.UTF8.GetBytes(text);
            return CompressGZip(raw);
        }

        /// <summary>
        /// Converte um array arbitrário de bytes
        /// </summary>
        /// <param name="raw">bytes a serem comprimidos</param>
        /// <returns>array de bytes comprimido</returns>
        public static byte[] CompressGZip(this byte[] raw)
        {
            if (raw == null)
            {
                throw new ArgumentNullException();
            }

            using (var memory = new MemoryStream())
            {
                using (GZipStream gzip = new GZipStream(memory, CompressionMode.Compress, true))
                {
                    gzip.Write(raw, 0, raw.Length);
                }

                return memory.ToArray();
            }
        }

        /// <summary>
        /// Descompacta array de bytes comprimido para uma string
        /// </summary>
        /// <param name="compressedText">array de bytes com dados comprimidos</param>
        /// <returns>string com dados descompactados</returns>
        public static string DecompressGZip(this byte[] compressedText)
        {
            using (GZipStream stream = new GZipStream(new MemoryStream(compressedText), CompressionMode.Decompress))
            {
                const int size = 4096;
                byte[] buffer = new byte[size];
                using (MemoryStream memory = new MemoryStream())
                {
                    int count = 0;
                    do
                    {
                        count = stream.Read(buffer, 0, size);
                        if (count > 0)
                        {
                            memory.Write(buffer, 0, count);
                        }
                    }
                    while (count > 0);
                    return Encoding.UTF8.GetString(memory.ToArray());
                }
            }
        }
    }
}
