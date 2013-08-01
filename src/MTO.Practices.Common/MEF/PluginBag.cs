using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MTO.Practices.Common.MEF
{
    public abstract class PluginBag
    {
        protected void Load(string pattern = null)
        {
            PluginLoader.LoadPluginsFor(this, pattern);
        }
    }
}
