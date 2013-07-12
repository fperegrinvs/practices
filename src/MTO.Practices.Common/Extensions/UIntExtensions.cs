namespace MTO.Practices.Common.Extensions
{
    using System;

    /// <summary>
    /// Extension methods para enumeradores.
    /// </summary>
    public static class UIntExtensions
    {
        /// <summary>
        /// Conta quantos bits estão "ligados" em um inteiro
        /// </summary>
        /// <param name="number">
        /// The value.
        /// </param>
        /// <returns>
        /// Número de bits ativados na flag
        /// </returns>
        public static int CountBits(this uint number)
        {
            number = number - ((number >> 1) & 0x55555555);
            number = (number & 0x33333333U) + ((number >> 2) & 0x33333333U);
            return (int)(unchecked(((number + (number >> 4)) & 0x0F0F0F0FU) * 0x1010101) >> 24);
        }

        /// <summary>
        /// Inverte os bits do inteiro.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <returns>
        /// Valor com a ordenação de bits invertida.
        /// </returns>
        public static uint ReverseBytes(this uint value)
        {
            return (value & 0x000000FFU) << 24 | (value & 0x0000FF00U) << 8 |
                   (value & 0x00FF0000U) >> 8 | (value & 0xFF000000U) >> 24;
        }

        /// <summary>
        /// Converte número para array de bytes.
        /// </summary>
        /// <param name="value">valor a ser convertido</param>
        /// <returns>array de bytes</returns>
        public static byte[] ToBytesBigEndian(this uint value)
        {
            value = ReverseBytes(value);
            return BitConverter.GetBytes(value);
        }
    }
}
