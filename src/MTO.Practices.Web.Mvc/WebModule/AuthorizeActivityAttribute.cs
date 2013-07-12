namespace MTO.Practices.WebModule
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;

    /// <summary>
    /// Atributo de autorização por atividades. Deve ser usado com enumeradores de atividades.
    /// </summary>
    public class AuthorizeActivityAttribute : AuthorizeAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorizeActivityAttribute"/> class.
        /// </summary>
        /// <param name="role"> A role. </param>
        public AuthorizeActivityAttribute(object role) : this(role, null, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorizeActivityAttribute"/> class.
        /// </summary>
        /// <param name="role"> A role. </param>
        /// <param name="role2"> Another role. </param>
        public AuthorizeActivityAttribute(object role, object role2) : this(role, role2, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorizeActivityAttribute"/> class.
        /// </summary>
        /// <param name="role"> A role. </param>
        /// <param name="role2"> Another role. </param>
        /// <param name="role3"> Yet another role. Should we be having so many? </param>
        public AuthorizeActivityAttribute(object role, object role2, object role3)
        {
            this.Roles = new List<string>();
            this.Roles.Add(GetTypeName(role.GetType()) + "." + role);

            if (role2 != null)
            {
                this.Roles.Add(GetTypeName(role2.GetType()) + "." + role2);
            }

            if (role3 != null)
            {
                this.Roles.Add(GetTypeName(role3.GetType()) + "." + role3);
            }
        }

        /// <summary>
        /// Roles necessárias para rodar o método
        /// </summary>
        protected new List<string> Roles { get; set; }

        /// <summary>
        /// Autoriza a action a rodar
        /// </summary>
        /// <param name="httpContext">Contexto web do usuário</param>
        /// <returns>Verdadeiro caso o usuário tenha permissão para executar</returns>
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (!ModuleRegistration.IsAuthenticatedFunc.Invoke())
            {
                return false;
            }

            // Se não tem nenhuma role nego acesso
            if (this.Roles == null || this.Roles.Count == 0)
            {
                throw new Exception("Atributo AuthorizeActivity criado sem parâmetros");
            }

            // Se o usuário possuir alguma role do atributo tá valendo
            if (this.Roles.Any(role => ModuleRegistration.IsAuthorizedFunc.Invoke(role)))
            {
                return true;
            }

            // Se o usuário não possuir nenhuma, retornamos a tela de permissão negada
            return false;
        }

        /// <summary>
        /// Método de tratamento de requisições não autorizadas
        /// </summary>
        /// <param name="filterContext">Contexto de filtragem</param>
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new RedirectToRouteResult("Unauthorized", null);
        }

        /// <summary>
        /// Recupera o nome do tipo como aparece na role
        /// </summary>
        /// <param name="t">O tipo</param>
        /// <returns>O nome</returns>
        private static string GetTypeName(Type t)
        {
            var name = t.ToString();
            return name.Substring(1 + name.LastIndexOf('.')).Replace("Activities", string.Empty).Replace("Enum", string.Empty);
        }
    }
}
