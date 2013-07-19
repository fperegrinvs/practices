namespace MTO.Practices.Common.Logging
{
    using MTO.Practices.Common.Enumerators;

    /// <summary>
    /// Dados de um evento
    /// </summary>
    public class EventVO
    {
        /// <summary>
        /// Título do evento
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Identificador do cliente.
        /// </summary>
        public string StoreId { get; set; }

        /// <summary>
        /// Detalhes do evento.
        /// </summary>
        public string Details { get; set; }

        /// <summary>
        /// Nome da aplicação
        /// </summary>
        public string AppName { get; set; }

        /// <summary>
        /// Tipo de evento logado
        /// </summary>
        public LogTypeEnum? LogType { get; set; }
    }
}
