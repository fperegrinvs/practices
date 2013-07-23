namespace MTO.Practices.Common.Debug
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;
    using System.Web;

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
