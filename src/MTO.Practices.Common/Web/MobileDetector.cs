using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MTO.Practices.Common.Web
{
    /// <summary>
    /// Detector de UserAgent Mobile baseado no Hands.dll
    /// </summary>
    public static class MobileDetector
    {
        /// <summary>
        /// Indica se o UserAgent é de mobile
        /// </summary>
        /// <param name="req">Request</param>
        /// <returns>Verdadeiro caso seja mobile</returns>
        public static bool IsMobileUserAgent(this System.Web.HttpRequestBase req)
        {
            if (req == null || req.UserAgent == null)
            {
                return false;
            }

            // Senhor, perdoai-nos pelo código abaixo.
            // Foi o Reflector quem fez.
            // Aguardaremos ansiosamente a chegada do messias das expessões regulares que irá transformar esses métodos estáticos absurdos em operadores booleanos legíveis
            string instance = req.UserAgent.ToLower();
            if (Conversions.ToBoolean(Operators.AndObject(Operators.AndObject(Operators.AndObject(Operators.AndObject(Operators.AndObject(Operators.OrObject(instance.Contains("ipod"), instance.Contains("iphone")), instance.Contains("mobile")), instance.Contains("applewebkit")), Operators.NotObject(instance.Contains("nokia"))), instance.Contains("mac")), instance.Contains("safari"))))
            {
                return true;
            }

            if (Conversions.ToBoolean((((((Conversions.ToBoolean(instance.StartsWith("nokia")) || Conversions.ToBoolean(instance.Contains("android"))) || (Conversions.ToBoolean(instance.Contains("motorola")) || Conversions.ToBoolean(instance.Contains("windowsce")))) || ((Conversions.ToBoolean(instance.Contains("midp-")) || Conversions.ToBoolean(instance.Contains("iemobile"))) || (Conversions.ToBoolean(instance.Contains("symbian")) || Conversions.ToBoolean(instance.Contains("ipaq"))))) || (((Conversions.ToBoolean(instance.Contains("mobilephone")) || Conversions.ToBoolean(instance.Contains("samsung"))) || (Conversions.ToBoolean(instance.StartsWith("benq")) || Conversions.ToBoolean(instance.StartsWith("panasonic")))) || ((Conversions.ToBoolean(instance.StartsWith("qtek")) || Conversions.ToBoolean(instance.StartsWith("palm"))) || (Conversions.ToBoolean(instance.StartsWith("philips")) || Conversions.ToBoolean(instance.StartsWith("acs-")))))) || ((((Conversions.ToBoolean(instance.StartsWith("mitsu/")) || Conversions.ToBoolean(instance.StartsWith("pantec"))) || (Conversions.ToBoolean(instance.StartsWith("nec-")) || Conversions.ToBoolean(instance.StartsWith("kwc")))) || ((Conversions.ToBoolean(instance.StartsWith("sharp")) || Conversions.ToBoolean(instance.StartsWith("mobileexplorer"))) || (Conversions.ToBoolean(instance.StartsWith("docomo")) || Conversions.ToBoolean(instance.StartsWith("sagem"))))) || (((Conversions.ToBoolean(instance.StartsWith("sch-")) || Conversions.ToBoolean(instance.StartsWith("sec-"))) || (Conversions.ToBoolean(instance.StartsWith("lg/")) || Conversions.ToBoolean(instance.StartsWith("lge")))) || ((Conversions.ToBoolean(instance.StartsWith("alcatel")) || Conversions.ToBoolean(instance.StartsWith("sendo"))) || (Conversions.ToBoolean(instance.StartsWith("ericsson")) || Conversions.ToBoolean(instance.StartsWith("mot-"))))))) || (((((Conversions.ToBoolean(instance.StartsWith("sie-")) || Conversions.ToBoolean(instance.Contains("smartphone"))) || (Conversions.ToBoolean(instance.StartsWith("panasonic")) || Conversions.ToBoolean(instance.StartsWith("wep")))) || ((Conversions.ToBoolean(instance.StartsWith("doris")) || Conversions.ToBoolean(instance.StartsWith("opwv"))) || (Conversions.ToBoolean(instance.StartsWith("owg")) || Conversions.ToBoolean(instance.StartsWith("brew"))))) || (((Conversions.ToBoolean(instance.StartsWith("jpluck")) || Conversions.ToBoolean(instance.StartsWith("klondike"))) || (Conversions.ToBoolean(instance.StartsWith("upg")) || Conversions.ToBoolean(Operators.AndObject(Operators.AndObject(instance.StartsWith("mozilla"), instance.Contains("palm")), Operators.NotObject(instance.Contains("macintosh")))))) || ((Conversions.ToBoolean(Operators.AndObject(Operators.AndObject(instance.StartsWith("mozilla"), instance.Contains("ppc")), Operators.NotObject(instance.Contains("macintosh")))) || Conversions.ToBoolean(Operators.AndObject(Operators.AndObject(instance.StartsWith("mozilla"), instance.Contains("symbian")), Operators.NotObject(instance.Contains("macintosh"))))) || (Conversions.ToBoolean(Operators.AndObject(Operators.AndObject(instance.StartsWith("mozilla"), instance.Contains("nokia")), Operators.NotObject(instance.Contains("macintosh")))) || Conversions.ToBoolean(Operators.AndObject(Operators.AndObject(instance.StartsWith("mozilla"), instance.Contains("smartphone")), Operators.NotObject(instance.Contains("macintosh")))))))) || ((((Conversions.ToBoolean(Operators.AndObject(instance.StartsWith("opera"), instance.Contains("opera-mini"))) || Conversions.ToBoolean(Operators.AndObject(instance.StartsWith("opera"), instance.Contains("opera mini")))) || (Conversions.ToBoolean(instance.StartsWith("sonyericsson")) || Conversions.ToBoolean(instance.StartsWith("sec-")))) || ((Conversions.ToBoolean(instance.StartsWith("lg-")) || Conversions.ToBoolean(instance.StartsWith("blackberry"))) || (Conversions.ToBoolean(instance.StartsWith("samsung")) || Conversions.ToBoolean(instance.StartsWith("htc"))))) || (Conversions.ToBoolean(Operators.AndObject(instance.Contains("sti"), instance.Contains("ctv"))) || Conversions.ToBoolean(instance.StartsWith("ct65")))))))
            {
                return true;
            }

            return false;
        }

        private static class Conversions
        {
            public static bool ToBoolean(bool boolean)
            {
                return boolean;
            }
        }

        private static class Operators
        {
            public static bool AndObject(bool item1, bool item2)
            {
                return item1 && item2;
            }

            public static bool OrObject(bool item1, bool item2)
            {
                return item1 || item2;
            }

            public static bool NotObject(bool obj)
            {
                return !obj;
            }
        }
    }
}
