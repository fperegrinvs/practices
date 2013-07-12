// -----------------------------------------------------------------------
// <copyright file="GeoLocation.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Net;
using System.Web;
using Subgurim.Controles;

namespace MTO.Practices.Common.GeoLocation
{
    using MTO.Practices.Common.Debug;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class GeoLocation
    {
        private const string CookieName = "GeoLocationSite";

        /// <summary>
        /// Retorna Cookie para GeoLocalizacao
        /// </summary>
        /// <returns>Cidade e Regiao</returns>
        public static string[] CurrentCity
        {
            get
            {
                // Tentamos pegar primeiro dos Items
                var city = Context.Current.Get<string>(CookieName);

                // Se não tem ainda, buscar do cookie
                if (string.IsNullOrEmpty(city))
                {
                    var cookie = Context.Current.RequestCookie(CookieName);

                    if (cookie != null)
                    {
                        city = cookie.Value;
                    }
                }

                string[] location = null;
                if (city != null)
                {
                    location = city.Split('_');
                }

                return location;
            }
            set
            {
                var city = value[0] + "_" + value[1];

                // Setamos em cookie
                var httpCookie = new HttpCookie(CookieName)
                    {
                        Value = city,
                        Expires = DateTime.Now.AddDays(1)
                    };


                Context.Current.ResponseCookie(httpCookie);

                // E no Items também
                Context.Current.Set(CookieName, city);

                DebugTracer.Log(string.Format("Criado cookie GeoLocationSite valor {0} expirando em {1}", httpCookie.Value, httpCookie.Expires), "SetCurrentCity");
            }
        }

        /// <summary>
        /// Método de Geo Location da MaxMind
        /// </summary>
        /// <param name="ip">
        /// IP
        /// </param>
        /// <param name="caminho">
        /// The caminho.
        /// </param>
        /// <returns>
        /// Location
        /// </returns>
        public static Location ReturnLocation(string ip, string caminho)
        {
            Location location = null;
            if (ip != null && ip.Length > 3)
            {
                try
                {
                    var service = new LookupService(caminho);
                    location = service.getLocation(ip);
                }
                catch (Exception ex)
                {
                    Logger.Instance.LogError(ex);
                    DebugTracer.Log("Erro ao recuperar localização", "ReturnLocation");
                    location = null;
                }
            }

            return location;
        }

        /// <summary>
        /// Método de Geo Location da MaxMind
        /// </summary>
        /// <param name="ip">
        /// IP
        /// </param>
        /// <param name="caminho">
        /// The caminho.
        /// </param>
        /// <returns>
        /// Retorna Cidade e Estado respectivamente
        /// </returns>
        public static string[] ReturnLocationString(string ip, string caminho)
        {
            Location location = ReturnLocation(ip, caminho);
            if (location != null)
            {
                return new string[] { location.city, location.region };
            }

            return null;
        }

        /// <summary>
        /// Converte Estado para Capital do Estado, quando o GEO IP não traz a cidade correspondente ao Slot
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        public static string ReturnCapital(int state)
        {
            /*
                BR,01,"Acre"
                BR,02,"Alagoas"
                BR,03,"Amapa"
                BR,04,"Amazonas"
                BR,05,"Bahia"
                BR,06,"Ceara"
                BR,07,"Distrito Federal"
                BR,08,"Espirito Santo"
                BR,11,"Mato Grosso do Sul"
                BR,13,"Maranhao"
                BR,14,"Mato Grosso"
                BR,15,"Minas Gerais"
                BR,16,"Para"
                BR,17,"Paraiba"
                BR,18,"Parana"
                BR,20,"Piaui"
                BR,21,"Rio de Janeiro"
                BR,22,"Rio Grande do Norte"
                BR,23,"Rio Grande do Sul"
                BR,24,"Rondonia"
                BR,25,"Roraima"
                BR,26,"Santa Catarina"
                BR,27,"Sao Paulo"
                BR,28,"Sergipe"
                BR,29,"Goias"
                BR,30,"Pernambuco"
                BR,31,"Tocantins"
             */
            string capital;
            switch (state)
            {
                case 1:
                    capital = "RBR";
                    break;
                case 2:
                    capital = "MCZ";
                    break;
                case 3:
                    capital = "MCP";
                    break;
                case 4:
                    capital = "MAO";
                    break;
                case 5:
                    capital = "SSA";
                    break;
                case 6:
                    capital = "FOR";
                    break;
                case 7:
                    capital = "BSB";
                    break;
                case 8:
                    capital = "VIX";
                    break;
                case 11:
                    capital = "CGR";
                    break;
                case 13:
                    capital = "SLZ";
                    break;
                case 14:
                    capital = "CGB";
                    break;
                case 15:
                    capital = "BHZ";
                    break;
                case 16:
                    capital = "BEL";
                    break;
                case 17:
                    capital = "JPA";
                    break;
                case 18:
                    capital = "CWB";
                    break;
                case 20:
                    capital = "THE";
                    break;
                case 21:
                    capital = "RIO";
                    break;
                case 22:
                    capital = "NAT";
                    break;
                case 23:
                    capital = "POA";
                    break;
                case 24:
                    capital = "PVH";
                    break;
                case 25:
                    capital = "BVB";
                    break;
                case 26:
                    capital = "FLN";
                    break;
                case 27:
                    capital = "SAO";
                    break;
                case 28:
                    capital = "AJU";
                    break;
                case 29:
                    capital = "GYN";
                    break;
                case 30:
                    capital = "REC";
                    break;
                case 31:
                    capital = "PMW";
                    break;
                default:
                    capital = string.Empty;
                    break;
            }

            return capital;
        }
    }
}