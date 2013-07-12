using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MTO.Practices.Common.Extensions
{
    /// <summary>
    /// Classe de extensores de Array
    /// </summary>
    public static class ArrayExtensions
    {
        /// <summary>
        /// Cria um subarray a partir de um array
        /// </summary>
        /// <typeparam name="T">O tipo do array</typeparam>
        /// <param name="data">o array</param>
        /// <param name="index">o índice inicial do subarray</param>
        /// <param name="length">o tamanho do subarray</param>
        /// <returns>o subarray</returns>
        public static T[] SubArray<T>(this T[] data, int index, int length)
        {
            T[] result = new T[length];
            Array.Copy(data, index, result, 0, length);
            return result;
        }
    }
}
