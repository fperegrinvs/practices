namespace MTO.Practices.Common
{
    using System;
    using System.ComponentModel;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// Classe de métodos auxiliares para trabalhar com enumeradores
    /// </summary>
    public static class EnumHelper
    {
        /// <summary>
        /// Recupera o valor do atributo Description do campo do enum
        /// </summary>
        /// <typeparam name="T">Tipo do enum</typeparam>
        /// <param name="enumFieldObject">Campo do enum</param>
        /// <returns>Valor do atributo</returns>
        public static string GetDescription<T>(this T enumFieldObject) where T : struct
        {
            var enumType = typeof(T);
            var name = enumFieldObject.ToString();

            return GetDescription(enumType, name);
        }

        /// <summary>
        /// Recupera o valor do atributo Description do campo do enum
        /// </summary>
        /// <returns>Valor do atributo</returns>
        public static string GetDescription(Type enumType, string name)
        {
            if (enumType == null)
            {
                throw new ArgumentNullException("enumType");
            }

            var field = enumType.GetFields(BindingFlags.Static | BindingFlags.GetField | BindingFlags.Public).First(
                x => x.GetValue(null).ToString() == name);
            foreach (DescriptionAttribute currAttr in field.GetCustomAttributes(typeof(DescriptionAttribute), true))
            {
                return currAttr.Description;
            }

            throw new InvalidEnumArgumentException("Valor do Enum não possui DisplayName: " + name);
        }
    }
}
