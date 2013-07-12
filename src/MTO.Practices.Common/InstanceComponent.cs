namespace MTO.Practices.Common
{
    /// <summary>
    /// Classe base para os business components
    /// </summary>
    /// <typeparam name="T"> Tipo da classe filha </typeparam>
    public class InstanceComponent<T> where T : class, new()
    {
        /// <summary>
        /// Singleton de instância da classe Publisher
        /// </summary>
        private static T instance;

        /// <summary>
        /// Singleton de instância da classe Publisher
        /// </summary>
        public static T Instance
        {
            get { return instance ?? (instance = new T()); }
            set { instance = value; }
        }
    }
}
