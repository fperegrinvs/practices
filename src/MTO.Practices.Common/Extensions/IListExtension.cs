namespace MTO.Practices.Common.Extensions
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Extensores de IList
    /// </summary>
    public static class ListExtension
    {
        /// <summary>
        /// Adiciona item ou cria nova lista
        /// </summary>
        /// <param name="list"> The list. </param>
        /// <param name="item"> The item. </param>
        /// <typeparam name="T"> O tipo do item da lista </typeparam>
        /// <returns> The IList. </returns>
        public static IList<T> AddOrCreate<T>(this IList<T> list, T item)
        {
            if (list == null)
            {
                list = new List<T>();
            }

            list.Add(item);

            return list;
        }

        /// <summary>
        /// Partition a list of elements into a smaller group of elements
        /// </summary>
        /// <typeparam name="T">Tipo da lista</typeparam>
        /// <param name="list">A lista</param>
        /// <param name="totalPartitions">Quantidade de partições da lista</param>
        /// <returns>Partições da lista</returns>
        public static List<T>[] Partition<T>(this List<T> list, int totalPartitions)
        {
            if (list == null)
                throw new ArgumentNullException("list");

            if (totalPartitions < 1)
                throw new ArgumentOutOfRangeException("totalPartitions");

            List<T>[] partitions = new List<T>[totalPartitions];

            int maxSize = (int)Math.Ceiling(list.Count / (double)totalPartitions);
            int k = 0;

            for (int i = 0; i < partitions.Length; i++)
            {
                partitions[i] = new List<T>();
                for (int j = k; j < k + maxSize; j++)
                {
                    if (j >= list.Count)
                        break;
                    partitions[i].Add(list[j]);
                }
                k += maxSize;
            }

            return partitions;
        }
    }
}
