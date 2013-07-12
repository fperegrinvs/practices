namespace MTO.Practices.Common.Extensions
{
    using System;
    using System.IO;
    using System.Runtime.Serialization.Formatters.Binary;

    /// <summary>
    /// Extensores de Object
    /// </summary>
    public static class ObjectExtension
    {
        public static Stream ToStream(this object obj)
        {
            if (obj == null)
                return null;

            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream ms = new MemoryStream();
            bf.Serialize(ms, obj);
            return ms;
        }
    }
}
