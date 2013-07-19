namespace MTO.Practices.Common.Serializers
{
    using System.IO;
    using System.Runtime.Serialization.Json;
    using System.Text;

    /// <summary>
    /// Serializador javascript
    /// </summary>
    public class JsonSerializer
    {
        /// <summary>
        /// Serializa um objeto para JSON
        /// </summary>
        /// <param name="element">
        /// Elemento a ser serializado
        /// </param>
        /// <typeparam name="T">
        /// Tipo do objeto
        /// </typeparam>
        /// <returns>
        /// string com o objeto serializado
        /// </returns>
        public static string Serialize<T>(T element) where T : class
        {
            var serializer = new DataContractJsonSerializer(typeof(T));
            var ms = new MemoryStream();
            serializer.WriteObject(ms, element);
            return Encoding.UTF8.GetString(ms.ToArray());
        }

        /// <summary>
        /// Serializa um objeto para JSON
        /// </summary>
        /// <param name="element">
        /// Elemento a ser serializado
        /// </param>
        /// <typeparam name="T">
        /// Tipo do objeto
        /// </typeparam>
        /// <returns>
        /// string com o objeto serializado
        /// </returns>
        public static byte[] SerializeToBytes<T>(T element) where T : class
        {
            var serializer = new DataContractJsonSerializer(typeof(T));
            var ms = new MemoryStream();
            serializer.WriteObject(ms, element);
            return ms.ToArray();
        }

        /// <summary>
        /// Serializa um objeto para JSON
        /// </summary>
        /// <param name="serializedElement">
        /// Elemento a ser deserializado
        /// </param>
        /// <typeparam name="T">
        /// Tipo do objeto
        /// </typeparam>
        /// <returns>
        /// objeto deserializado
        /// </returns>
        public static T DeSerialize<T>(string serializedElement) where T : class
        {
            var ms = new MemoryStream(Encoding.Unicode.GetBytes(serializedElement));
            var serializer = new DataContractJsonSerializer(typeof(T));
            var element = serializer.ReadObject(ms) as T;
            ms.Close();
            return element;
        }

        /// <summary>
        /// Serializa um objeto para JSON
        /// </summary>
        /// <param name="serializedElement">
        /// Elemento a ser deserializado
        /// </param>
        /// <typeparam name="T">
        /// Tipo do objeto
        /// </typeparam>
        /// <returns>
        /// objeto deserializado
        /// </returns>
        public static T DeSerializeBytes<T>(byte[] serializedElement) where T : class
        {
            var ms = new MemoryStream(serializedElement);
            var serializer = new DataContractJsonSerializer(typeof(T));
            var element = serializer.ReadObject(ms) as T;
            ms.Close();
            return element;
        }
    }
}
