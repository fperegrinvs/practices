namespace MTO.Practices.Common.Interfaces
{
    using System;

    /// <summary>
    /// Repositório de usuário para validação de acesso
    /// </summary>
    public interface IUserStore
    {
        /// <summary>
        /// Verifica se um usuário existe e está habilitado
        /// </summary>
        /// <param name="uid">Id do usuário</param>
        /// <returns>Verdadeiro caso usuário exista</returns>
        bool UserExistsAndEnabled(Guid uid);
    }
}
