namespace MTO.Practices.Common.Entity
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Objects;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Text.RegularExpressions;

    using JSLogger.Data4;

    /// <summary>
    /// Extensores do DbContext do EF
    /// </summary>
    public static class DbContextExtensions
    {
        /// <summary>
        /// Deleta em lote, sem integridade transacional. O delete é executado instantaneamente.
        /// </summary>
        /// <typeparam name="T">O tipo</typeparam>
        /// <param name="context">O DbContext</param>
        /// <param name="query">A query de deleção</param>
        /// <remarks>
        /// Execução é feita sem integridade transacional. O delete é executado instantaneamente.
        /// </remarks>
        public static void DeleteBatch<T>(this DbContext context, IQueryable<T> query) where T : class
        {
            string sqlClause = GetClause<T>(context, query);
            context.Database.ExecuteSqlCommand(string.Format("DELETE {0}", sqlClause));
        }

        /// <summary>
        /// Executa insert usando Sql Bulk Insert
        /// </summary>
        /// <param name="ctx"> O contexto </param>
        /// <param name="entities"> The entities. </param>
        /// <param name="transaction">A transação da qual o bulk insert fará parte</param>
        /// <typeparam name="T"> O tipo da entidade sendo inserida </typeparam>
        /// <remarks>
        /// Execução é feita sem integridade transacional. O insert é executado instantaneamente.
        /// </remarks>
        public static void BulkInsert<T>(this DbContext ctx, IEnumerable<T> entities, SqlTransaction transaction = null) where T : class
        {
            try
            {
                // using (var conn = new SqlConnection(ctx.Database.Connection.ConnectionString))
                // {
                     var conn = ctx.Database.Connection;
                     if (conn.State == ConnectionState.Closed)
                     {
                         conn.Open();
                     }
                    
                    var tableName = ctx.GetTableName<T>();

                    var inserter = new BulkInserter<T>((SqlConnection)ctx.Database.Connection, tableName, 2000, SqlBulkCopyOptions.Default, transaction);
                    inserter.Insert(entities);
                    conn.Close();
                // }
            }
            catch (Exception ex)
            {
                Logger.Instance.LogException(ex);
                throw;
            }
        }

        /// <summary>
        /// Recupera o nome da tabela da entidade
        /// </summary>
        /// <typeparam name="T">O tipo da entidade</typeparam>
        /// <param name="context">O contexto</param>
        /// <returns>O nome da tabela</returns>
        public static string GetTableName<T>(this DbContext context) where T : class
        {
            ObjectContext objectContext = ((IObjectContextAdapter)context).ObjectContext;

            return objectContext.GetTableName<T>();
        }

        /// <summary>
        /// Recupera o nome da tabela da entidade
        /// </summary>
        /// <typeparam name="T">O tipo da entidade</typeparam>
        /// <param name="context">O contexto</param>
        /// <returns>O nome da tabela</returns>
        public static string GetTableName<T>(this ObjectContext context) where T : class
        {
            string sql = context.CreateObjectSet<T>().ToTraceString();
            Regex regex = new Regex("FROM (?<table>.*) AS");
            Match match = regex.Match(sql);

            string table = match.Groups["table"].Value;
            return table;
        }

        /// <summary>
        /// Monta a clausula de delete
        /// </summary>
        /// <typeparam name="T">O tipo da entidade</typeparam>
        /// <param name="context">O DbContext</param>
        /// <param name="clause">A clausula de select</param>
        /// <returns>A clausula de delete</returns>
        private static string GetClause<T>(DbContext context, IQueryable<T> clause) where T : class
        {
            const string Snippet = "FROM [dbo].[";

            var sql = clause.ToString();
            var sqlFirstPart = sql.Substring(sql.IndexOf(Snippet, System.StringComparison.OrdinalIgnoreCase));

            sqlFirstPart = sqlFirstPart.Replace("AS [Extent1]", string.Empty);
            sqlFirstPart = sqlFirstPart.Replace("[Extent1].", string.Empty);

            return sqlFirstPart;
        }
    }
}
