namespace System
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;

    /// <summary>
    /// Extensores de String
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Caracteres de início de caminho
        /// </summary>
        private static readonly char[] PathStartChars = new[] { '/', '~' };

        /// <summary> Converte string para booleano respeitando um valor padrão caso a conversão falhe. </summary>
        /// <param name="input"> The input. </param>
        /// <param name="defaultValue"> The default value. </param>
        /// <returns> The <see cref="bool"/>. </returns>
        public static bool ToBoolean(this string input, bool defaultValue = false)
        {
            bool b;
            return bool.TryParse(input, out b) ? b : defaultValue;
        }

        /// <summary>
        /// Calcula o hash md5 da string e retorna em formato GUID
        /// </summary>
        /// <param name="source">A string</param>
        /// <returns>O guid do md5 da string</returns>
        public static Guid Md5Hash(this string source)
        {
            var md5 = MD5.Create();

            var inputBytes = Encoding.ASCII.GetBytes(source);
            byte[] hash = md5.ComputeHash(inputBytes);

            return new Guid(hash);
        }

        /// <summary>
        /// Returns a value indicating whether the specified String object occurs within this string. 
        /// A parameter specifies the type of search to use.
        /// Source: http://stackoverflow.com/questions/444798/case-insensitive-containsstring
        /// </summary>
        /// <param name="source">This string.</param>
        /// <param name="value">The string to seek.</param>
        /// <param name="comp">One of the enumeration values that specifies the rules for the search.</param>
        /// <returns>True if the specified String objects occurs within this string.</returns>
        public static bool Contains(this string source, string value, StringComparison comp)
        {
            return source.IndexOf(value, comp) >= 0;
        }

        /// <summary>
        /// Returns a new string in which all the occurrences of the specified substring are replaced with the specified replacement string.
        /// Source: http://www.codeproject.com/KB/string/fastestcscaseinsstringrep.aspx
        /// </summary>
        /// <param name="original">The original string</param>
        /// <param name="pattern">The search pattern</param>
        /// <param name="replacement">The replacement string</param>
        /// <param name="comparisonType">One of the enumeration values that specifies the rules for the search.</param>
        /// <returns>The new string</returns>
        public static string Replace(this string original, string pattern, string replacement, StringComparison comparisonType)
        {
            return Replace(original, pattern, replacement, comparisonType, -1);
        }

        /// <summary>
        /// Keeps a string withing a certain size. Fills the ending with '...' if the string exceeds maximum length.
        /// </summary>
        /// <param name="text">The string</param>
        /// <param name="length">Maximum length (must be greater than 3)</param>
        /// <returns>The abridged string</returns>
        public static string StopAt(this string text, int length)
        {
            if (text != null && length > 3 && text.Length > length)
            {
                return text.Substring(0, length - 3) + "...";
            }

            return text;
        }

        /// <summary>
        /// Converte string para int nullable. Retorna null caso conversão falhe.
        /// </summary>
        /// <param name="str">String de entrada</param>
        /// <returns>Inteiro nullable</returns>
        public static int? ToIntNullable(this string str)
        {
            int i;
            if (int.TryParse(str, out i))
            {
                return i;
            }

            return null;
        }

        /// <summary>
        /// Converte string para guid nullable. Retorna null caso conversão falhe.
        /// </summary>
        /// <param name="str">String de entrada</param>
        /// <returns>Guid nullable</returns>
        public static Guid? ToGuidNullable(this string str)
        {
            Guid guid;
            if (Guid.TryParse(str, out guid))
            {
                return guid;
            }

            return null;
        }

        /// <summary>
        /// Converte para int
        /// </summary>
        /// <param name="str"> The str. </param>
        /// <param name="defValue"> Valor default caso conversao falhe </param>
        /// <returns> The <see cref="int"/>. </returns>
        public static int ToInt(this string str, int defValue = 0)
        {
            int val;
            if (int.TryParse(str, out val))
            {
                return val;
            }

            return defValue;
        }

        /// <summary>
        /// Remove query strings da string. Significa manter tudo até o primeiro ? encontrado.
        /// </summary>
        /// <param name="url">A string</param>
        /// <returns>A string sem as query strings</returns>
        public static string RemoveQueryString(this string url)
        {
            var i = url.IndexOf('?');
            if (i > 0)
            {
                return url.Substring(0, i);
            }

            return url;
        }

        /// <summary>
        /// Remove os caracteres / e ~ do início de um caminho
        /// </summary>
        /// <param name="path">O caminho</param>
        /// <returns>O caminho sem /  e ~ no início</returns>
        public static string TrimPathStart(this string path)
        {
            return path.TrimStart(PathStartChars);
        }

        /// <summary>
        /// Remove caracteres / do final de uma string
        /// </summary>
        /// <param name="path">O caminho</param>
        /// <returns>A string sem / no final</returns>
        public static string TrimPathEnd(this string path)
        {
            return path.TrimEnd('/');
        }

        /// <summary>
        /// Concatena dois segmentos de caminho tratando / entre os dois e ~ no começo do segundo segmento.
        /// </summary>
        /// <param name="path">O segmento do início</param>
        /// <param name="otherPath">O segmento do final</param>
        /// <returns>Os segmentos </returns>
        public static string JoinPath(this string path, string otherPath)
        {
            return path.TrimPathEnd() + '/' + otherPath.TrimPathStart();
        }

        /// <summary>
        /// Verifica se uma string contem alguma das strings informadas
        /// </summary>
        /// <param name="text">A string que estamos verificando</param>
        /// <param name="strings">Strings que podem estar no conteúdo</param>
        /// <returns>Verdadeiro caso alguma string esteja contida na string informada</returns>
        public static bool ContainsAny(this string text, string[] strings)
        {
            return strings.Any(text.Contains);
        }

        /// <summary>
        /// Aplica HtmlDecode na string
        /// </summary>
        /// <param name="text">A string</param>
        /// <returns>A string com escapes HTML decodificados</returns>
        public static string HtmlDecode(this string text)
        {
            return Net.WebUtility.HtmlDecode(text);
        }

        /// <summary>
        /// Returns a new string in which all the occurrences of the specified substring are replaced with the specified replacement string.
        /// Source: http://www.codeproject.com/KB/string/fastestcscaseinsstringrep.aspx
        /// </summary>
        /// <param name="original">The original string</param>
        /// <param name="pattern">The search pattern</param>
        /// <param name="replacement">The replacement string</param>
        /// <param name="comparisonType">One of the enumeration values that specifies the rules for the search.</param>
        /// <param name="stringBuilderInitialSize">The initial size of the string builder used for generating the new string.</param>
        /// <returns>The new string</returns>
        private static string Replace(this string original, string pattern, string replacement, StringComparison comparisonType, int stringBuilderInitialSize)
        {
            if (original == null)
            {
                return null;
            }

            if (string.IsNullOrEmpty(pattern))
            {
                return original;
            }


            int posCurrent = 0;
            int lenPattern = pattern.Length;
            int idxNext = original.IndexOf(pattern, comparisonType);
            var result = new StringBuilder(stringBuilderInitialSize < 0 ? Math.Min(4096, original.Length) : stringBuilderInitialSize);

            while (idxNext >= 0)
            {
                result.Append(original, posCurrent, idxNext - posCurrent);
                result.Append(replacement);

                posCurrent = idxNext + lenPattern;

                idxNext = original.IndexOf(pattern, posCurrent, comparisonType);
            }

            result.Append(original, posCurrent, original.Length - posCurrent);

            return result.ToString();
        }
    }
}
