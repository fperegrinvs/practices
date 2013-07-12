namespace MTO.Practices.Common.Configuration
{
    using System;
    using System.Collections.Specialized;
    using System.Configuration;

    /// <summary>
    /// Configuracao da aplicação
    /// </summary>
    public class GlobalConfig : InstanceComponent<GlobalConfig>
    {
        /// <summary>
        /// The app settings.
        /// </summary>
        private static NameValueCollection appSettings = ConfigurationManager.AppSettings;

        /// <summary>
        /// Gets the file purge delay in minutes. Default = 5
        /// </summary>
        public int FilePurgeDelay
        {
            get
            {
                return appSettings["FilePurgeDelay"].ToInt(5);
            }
        }

        /// <summary>
        /// Se o purge de akamai deve ser habilitado
        /// </summary>
        public bool PurgeAkamai
        {
            get
            {
                return appSettings["AkamaiPurgeEnabled"].ToBoolean(true);
            }
        }

        /// <summary>
        /// Se devemos consolidar ao aprovar. Default = false.
        /// </summary>
        public bool PreRenderOnPublish
        {
            get
            {
                return appSettings["PreRenderOnPublish"].ToBoolean();
            }
        }

        /// <summary>
        /// Se devemos criar thread para consolidar ao aprovar. Default = true.
        /// </summary>
        public bool ConsolidateAsync
        {
            get
            {
                return appSettings["ConsolidateAsync"].ToBoolean(true);
            }
        }

        /// <summary>
        /// O tempo em minutos que devemos aguardar para efetuar purge do akamai após alterar conteúdo. Default = 10min
        /// </summary>
        public int AkamaiPurgeDelay
        {
            get
            {
                return appSettings["AkamaiPurgeDelay"].ToIntNullable() ?? 10;
            }
        }

        /// <summary>
        /// Offset de agendamento: tempo em minutos que será somado ao agendamento para adiantar ou atrasar o uso de controle assíncrono. Default = 4 horas
        /// </summary>
        /// <remarks>Esse valor deve estar de acordo com o tempo do cache externo (Akamai, etc) à aplicação, para garantir que o agendamento entre no ar na hora certa.</remarks>
        public int ScheduledControlOffset
        {
            get
            {
                return appSettings["ScheduledControlOffset"].ToIntNullable() ?? (-4 * 60);
            }
        }

        /// <summary>
        /// Id do Deploy (aparece no rodape do admin)
        /// </summary>
        public string DeployId
        {
            get
            {
                return appSettings["DeployId"] ?? string.Empty;
            }
        }
    }
}
