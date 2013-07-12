namespace MTO.Practices.WebModule
{
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;

    /// <summary>
    /// Registro de um módulo do sistema
    /// </summary>
    public abstract class ModuleRegistration : AreaRegistration
    {
        /// <summary>
        /// Initializes static members of the <see cref="ModuleRegistration"/> class.
        /// </summary>
        static ModuleRegistration()
        {
            PermissionList = new List<AreaPermission>();
            Navigation = new List<Menu>();
        }

        /// <summary>
        /// Barra de navegação do admin
        /// </summary>
        public static List<Menu> Navigation { get; private set; } 

        /// <summary>
        /// Função usada para verifica se o usuário está autorizado a desempenhar umaa atividade
        /// </summary>
        public static Func<string, bool> IsAuthorizedFunc { get; set; }

        /// <summary>
        /// Função para verificar se  o usuário está autenticado
        /// </summary>
        public static Func<bool> IsAuthenticatedFunc { get; set; }

        /// <summary>
        /// Lista de permissões disponíveis no sistema
        /// </summary>
        public static List<AreaPermission> PermissionList { get; private set; }
    }
}
