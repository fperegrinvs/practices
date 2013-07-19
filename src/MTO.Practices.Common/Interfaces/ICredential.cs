namespace MTO.Practices.Common.Interfaces
{
    /// <summary>
    /// The Credential interface.
    /// </summary>
    public interface ICredential
    {
        /// <summary>
        /// Url a ser acessada
        /// </summary>
        string Url { get; }

        /// <summary>
        /// Login do usuário
        /// </summary>
        string Login { get; }

        /// <summary>
        /// Password a ser utilizado
        /// </summary>
        string Password { get; }
    }
}
