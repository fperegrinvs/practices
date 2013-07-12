namespace MTO.Practices.Common.WCF
{
    using System;
    using System.Configuration;
    using System.ServiceModel.Configuration;

    /// <summary>
    /// Extensor de behavior que adiciona o Inspector
    /// </summary>
    public class InspectorBehaviorExtension : BehaviorExtensionElement
    {
        /// <summary>
        /// O nome da propriedade que chaveia captura de respostas
        /// </summary>
        private const string CaptureResponsePropertyName = "captureResponse";

        /// <summary>
        /// Gets the type of behavior.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Type"/>.
        /// </returns>
        public override Type BehaviorType
        {
            get
            {
                return typeof(ClientMessageInspector);
            }
        }

        /// <summary>
        /// Indica se devemos capturar respostas, porque estas geralmente são grandes e ocuparão muita memória
        /// </summary>
        [ConfigurationProperty(CaptureResponsePropertyName)]
        public string CaptureResponse
        {
            get
            {
                return base[CaptureResponsePropertyName] as string;
            }

            set
            {
                base[CaptureResponsePropertyName] = value;
            }
        }

        /// <summary>
        /// Creates a behavior extension based on the current configuration settings.
        /// </summary>
        /// <returns>
        /// The behavior extension.
        /// </returns>
        protected override object CreateBehavior()
        {
            return new ClientMessageInspector(this.CaptureResponse.ToBoolean());
        }
    }
}
