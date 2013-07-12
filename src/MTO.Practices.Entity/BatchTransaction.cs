namespace MTO.Practices.Common.Entity
{
    using System;
    using System.Transactions;

    /// <summary>
    /// Classe responsável pela criação e configuração das transações dos processos em lote do CMS
    /// </summary>
    public static class BatchTransaction
    {
        /// <summary>
        /// Cria uma transação
        /// </summary>
        /// <returns>A transação</returns>
        public static TransactionScope Create()
        {
            var transactionOptions = new TransactionOptions
            {
                IsolationLevel = IsolationLevel.ReadCommitted,
                Timeout = TransactionManager.MaximumTimeout
            };

            return new TransactionScope(TransactionScopeOption.Required, transactionOptions);
        }

        /// <summary>
        /// Suprime a transação atual para o escopo
        /// </summary>
        /// <returns>O escopo sem transação</returns>
        public static TransactionScope Supress()
        {
            var transactionOptions = new TransactionOptions
            {
                IsolationLevel = IsolationLevel.ReadCommitted,
                Timeout = TransactionManager.MaximumTimeout
            };

            return new TransactionScope(TransactionScopeOption.Suppress, transactionOptions);
        }
    }
}
