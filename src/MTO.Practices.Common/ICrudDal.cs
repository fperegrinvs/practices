namespace MTO.Practices.Common
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Interface mãe para operações de CRUD
    /// </summary>
    /// <typeparam name="T"> Classe de dados referente ao Dal </typeparam>
    /// <typeparam name="TId"> Tipo de dados da chave do objeeto referente ao Dal </typeparam>
    public interface ICrudDal<T, TId>
        where T : class
    {
        /// <summary>
        /// Recupera um objeto pelo seu identificador
        /// </summary>
        /// <param name="id">O identificador</param>
        /// <returns>O objeto</returns>
        T Get(TId id);

        /// <summary>
        /// Recupera todos os objetos
        /// </summary>
        /// <returns>Os objeto</returns>
        List<T> GetAll();

        /// <summary>
        /// Insere novo objeto
        /// </summary>
        /// <param name="obj">O objeto</param>
        void Insert(T obj);

        /// <summary>
        /// Atualiza o objeto no repositório
        /// </summary>
        /// <param name="obj">O objeto</param>
        void Update(T obj);

        /// <summary>
        /// Remove um objeto pelo seu identificador
        /// </summary>
        /// <param name="id">O identificador do objeto</param>
        void Remove(TId id);

        /// <summary>
        /// Remove um objeto pelo seu identificador
        /// </summary>
        /// <param name="obj">O objeto</param>
        void Remove(T obj);
    }
}
