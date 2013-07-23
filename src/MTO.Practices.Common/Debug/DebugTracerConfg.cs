namespace MTO.Practices.Common.Debug
{
    using System.Configuration;

    /// <summary>
    /// Tracer de eventos de debug
    /// </summary>
    public static class DebugTracerConf
    {
        public static bool TraceCacheKeys
        {
            get
            {
                return ConfigurationManager.AppSettings["TraceCacheKeys"] == "true";
            }
        }
    }
}
