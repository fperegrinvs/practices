// -----------------------------------------------------------------------
// <copyright file="GuidMapper.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace MTO.Practices.Common
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Text;

    using Omu.ValueInjecter;

    /// <summary>
    /// Mapeamento de Guid pro Injecter
    /// </summary>
    public class GuidMapper : ValueInjection
    {
        /// <summary>
        /// Injeta valores apenas para Guids diferentes de null
        /// </summary>
        /// <param name="source">Objeto origem</param>
        /// <param name="target">Objeto Destino</param>
        protected override void Inject(object source, object target)
        {
            var sprops = source.GetProps();
            var tprops = target.GetProps();

            for (int i = 0; i < tprops.Count; i++)
            {
                if (i > sprops.Count)
                    break;

                var sourceProp = sprops[i];
                var targetProp = tprops[i];

                var underlyingType = sourceProp.PropertyType;
                if (underlyingType.IsGenericType && underlyingType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
                {
                    var converter = new NullableConverter(underlyingType);
                    underlyingType = converter.UnderlyingType;
                }

                if (underlyingType == typeof(Guid))
                {
                    var sourceValue = sourceProp.GetValue(source);
                    if (sourceValue != null)
                    {
                        targetProp.SetValue(target, new Guid(sourceValue.ToString()));
                    }
                }
            }
        }
    }
}
