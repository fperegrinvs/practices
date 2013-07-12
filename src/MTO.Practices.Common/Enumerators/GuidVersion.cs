namespace MTO.Practices.Common.Enumerators
{
    /// <summary>
    /// Indica a versão de um Guid
    /// </summary>
    public enum GuidVersion
    {
        /// <summary>
        /// Conceptually, the original (version 1) generation scheme for UUIDs was to concatenate the UUID version with the MAC address of the computer 
        /// that is generating the UUID, and with the number of 100-nanosecond intervals since the adoption of the Gregorian calendar in the West
        /// </summary>
        TimeBased = 0x01,

        /// <summary>
        /// Version 2 UUIDs are similar to Version 1 UUIDs, with the upper byte of the clock sequence replaced by the identifier for a "local domain"
        /// (typically either the "POSIX UID domain" or the "POSIX GID domain") and the first 4 bytes of the timestamp replaced by the user's POSIX UID or 
        /// GID (with the "local domain" identifier indicating which it is)
        /// </summary>
        Reserved = 0x02,

        /// <summary>
        /// Version 3 UUIDs use a scheme deriving a UUID via MD5 from a URL, a fully qualified domain name, an object identifier, a distinguished name
        /// (DN as used in Lightweight Directory Access Protocol), or on names in unspecified namespaces. 
        /// </summary>
        NameBased = 0x03,

        /// <summary>
        /// Version 4 UUIDs use a scheme relying only on random numbers. This algorithm sets the version number as well as two reserved bits.
        /// All other bits are set using a random or pseudorandom data source.
        /// </summary>
        Random = 0x04
    }
}
