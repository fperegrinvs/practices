namespace MTO.Practices.Common.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web.Script.Serialization;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public static class ToJsonExtension
    {
        /// <summary>
        /// Serializa objeto para json
        /// </summary>
        /// <param name="object">O objeto a ser serializado</param>
        /// <returns>A string contendo o json</returns>
        public static string ToJson(this object @object)
        {
            var serializer = new JavaScriptSerializer();
            serializer.MaxJsonLength = int.MaxValue;
            return serializer.Serialize(@object);
        }

        /// <summary>
        /// Desserializa objeto a partir de um json
        /// </summary>
        /// <typeparam name="T">Tipo do objeto serializado</typeparam>
        /// <param name="json">a string json</param>
        /// <returns>O objeto desserializado</returns>
        public static T FromJson<T>(this string json)
        {
            var serializer = new JavaScriptSerializer();
            serializer.MaxJsonLength = int.MaxValue;
            return serializer.Deserialize<T>(json);
        }
    }
}
