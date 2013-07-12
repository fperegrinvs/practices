namespace MTO.Practices.Common
{
    using System;
    using System.Linq;
    using System.Transactions;

    using MTO.Practices.Common.Entity;

    /// <summary>
    /// Gerencia uma unidade de trabalho de persistência com recurso de transação global e 
    /// </summary>
    public class UnitOfWorkGlobalTransaction : IDisposable
    {
        protected UnitOfWork UnitOfWork { get; set; }

        protected TransactionScope TransactionScope { get; set; }
        
        public UnitOfWorkGlobalTransaction()
        {
            if (UnitOfWork.InProgress)
            {
                throw new Exception("Não é possível abrir Unit of Work global dentro de Unit of Work normal.");
            }

            this.UnitOfWork = new UnitOfWork();
            this.TransactionScope = BatchTransaction.Create();
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
