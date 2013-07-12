namespace MTO.Practices.Common.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public static class CustomMergeExtension
    {
        public delegate void DeleteAction<in T>(T item);

        public delegate void UpdateAction<in T>(T item);

        public delegate void InsertAction<in T>(T item);

        public delegate T CombineAction<T>(T a, T b);

        /// <summary>
        /// Combina duas coleções, removendo da primeira coleção os itens que não existem na segunda coleção e adicionando na primeira coleção os itens que só existem na segunda coleção.
        /// </summary>
        /// <typeparam name="T">Determina o tipo da coleção.</typeparam>
        /// <param name="first">Coleção principal.</param>
        /// <param name="second">Coleção secundária que será mesclada a coleção principal.</param>
        /// <param name="comparer">Determina como será feito a comparação entre os itens das coleções.</param>
        /// <param name="combineAction">Combina dois objetos.</param>
        /// <param name="deleteAction">Método que irá excluir os itens da coleção principal.</param>
        /// <param name="updateAction">Método que irá atualizar os itens da coleção principal.</param>
        /// <param name="insertAction">Método que irá adicionar os itens da coleção secundária na coleção principal.</param>
        /// <returns>Nova coleção com dados da coleção principal e secundária.</returns>
        public static IEnumerable<T> CustomMerge<T>(this IEnumerable<T> first, IEnumerable<T> second, IEqualityComparer<T> comparer, CombineAction<T> combineAction, DeleteAction<T> deleteAction, UpdateAction<T> updateAction, InsertAction<T> insertAction)
        {
            if (first == null)
            {
                throw new ArgumentNullException("first");
            }

            var deleted = first.Except(second, comparer).ToList();
            for (int i = 0; i < deleted.Count(); i++)
            {
                var item = deleted.ElementAt(i);
                deleteAction.Invoke(item);
            }

            var modified = first.Intersect(second, comparer).ToList();
            for (int i = 0; i < modified.Count(); i++)
            {
                var item = modified.ElementAt(i);
                var secondItem = second.Single(n => comparer.Equals(n, item));
                item = combineAction(item, secondItem);
                updateAction.Invoke(item);
            }

            var added = second.Except(first, comparer).ToList();
            for (int i = 0; i < added.Count(); i++)
            {
                var item = added.ElementAt(i);
                insertAction.Invoke(item);
            }

            return first.ToList();
        }
    }
}
