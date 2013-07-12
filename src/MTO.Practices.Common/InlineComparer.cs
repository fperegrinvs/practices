namespace MTO.Practices.Common
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Comparador inline
    /// </summary>
    public class InlineComparer<T> : IEqualityComparer<T>
    {
        /// <summary>
        /// Proriedade da funcao de comparação
        /// </summary>
        public Func<T, T, bool> Cmp { get; set; }

        public InlineComparer(Func<T, T, bool> cmp)
        {
            this.Cmp = cmp;
        }

        /// <summary>
        /// Compara dois objetos usando a função de comparação definida
        /// </summary>
        /// <param name="x">Objeto a ser comparado</param>
        /// <param name="y">Objeto a ser comparado</param>
        /// <returns>Verdadeiro caso objetos sejam iguais</returns>
        public bool Equals(T x, T y)
        {
            return this.Cmp(x, y);
        }

        /// <summary>
        /// Método da interface para prover hashcode do objeto. Não nos serve de nada, então fazemos hash simples.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int GetHashCode(T obj)
        {
            return obj.GetHashCode();
        }
    }
}
