namespace MTO.Practices.Common
{
    /// <summary>
    /// Interface padrão para Classes Factory
    /// </summary>
    public interface IFactory
    {
        /// <summary>
        /// Cria instancia de classe com base na interface que implementa.
        /// </summary>
        /// <typeparam name="T">A interface da classe a ser instanciada</typeparam>
        /// <returns>Instância da classe desejada.</returns>
        T Resolve<T>() where T : class;
    }
}
