namespace MTO.Practices.Common
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Contrato para classes DataContext
    /// </summary>
    public interface IDataContext
    {
        /// <summary>
        /// Salva as alterações caso não estejamos num contexto de Unit of Work
        /// </summary>
        /// <returns>Zero caso estejamos em UoW, número de rows alteradas caso contrário</returns>
        int SaveChanges();

        /// <summary>
        /// Consolida unidade de trabalho (roda SaveChanges do DbContext)
        /// </summary>
        void ConsolidateUnitOfWork();

        /// <summary>
        /// Consolida unidade de trabalho (roda SaveChanges do DbContext)
        /// </summary>
        void RollbackUnitOfWork();

        /// <summary>
        /// Ativa e desabilita o tracking de objetos
        /// </summary>
        bool TrackingEnabled { get; set; }
    }
}
