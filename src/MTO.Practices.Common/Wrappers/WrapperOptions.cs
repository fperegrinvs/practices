namespace MTO.Practices.Common.Wrappers
{
    /// <summary>
    /// The wrapper options.
    /// </summary>
    public class WrapperOptions
    {
        /// <summary>
        /// Nome da credencial armazenada no contexto atual.
        /// </summary>
        public string CredentialName { get; set; }

        /// <summary>
        /// Nome do serviço a ser acessado
        /// </summary>
        public string ServiceName { get; set; }
    }
}
