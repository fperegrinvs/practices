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
            // olhar: http://blog.stevensanderson.com/2010/12/17/using-51degreesmobi-foundation-for-accurate-mobile-browser-detection-on-aspnet-mvc-3/
            string instance = req.UserAgent.ToLower();
            if (((instance.Contains("ipod") || instance.Contains("iphone"))
                && instance.Contains("mobile") && instance.Contains("applewebkit") && !instance.Contains("nokia")) && instance.Contains("mac") && instance.Contains("safari"))
            {
                return true;
            }

            if (((instance.StartsWith("nokia") || instance.Contains("android") || instance.Contains("motorola") || instance.Contains("windowsce") 
                || instance.Contains("midp-") || instance.Contains("iemobile") || instance.Contains("symbian") || instance.Contains("ipaq")
                || (((instance.Contains("mobilephone") || instance.Contains("samsung")) || (instance.StartsWith("benq") || instance.StartsWith("panasonic")))
                || ((instance.StartsWith("qtek") || instance.StartsWith("palm")) || (instance.StartsWith("philips") || instance.StartsWith("acs-"))))) 
                || ((((instance.StartsWith("mitsu/") || instance.StartsWith("pantec")) || (instance.StartsWith("nec-") || instance.StartsWith("kwc")))
                || ((instance.StartsWith("sharp") || instance.StartsWith("mobileexplorer")) || (instance.StartsWith("docomo") || instance.StartsWith("sagem"))))
                || (((instance.StartsWith("sch-") || instance.StartsWith("sec-")) || (instance.StartsWith("lg/") || instance.StartsWith("lge"))) 
                || ((instance.StartsWith("alcatel") || instance.StartsWith("sendo")) || (instance.StartsWith("ericsson") || instance.StartsWith("mot-"))))))
                || (((((instance.StartsWith("sie-") || instance.Contains("smartphone")) || (instance.StartsWith("panasonic") || instance.StartsWith("wep"))) 
                || ((instance.StartsWith("doris") || instance.StartsWith("opwv")) || (instance.StartsWith("owg") || instance.StartsWith("brew")))) 
                || (((instance.StartsWith("jpluck") || instance.StartsWith("klondike")) || (instance.StartsWith("upg") 
                || (instance.StartsWith("mozilla") && instance.Contains("palm") && !instance.Contains("macintosh"))))
                || ((((instance.StartsWith("mozilla") && instance.Contains("ppc")) && !instance.Contains("macintosh"))
                || ((instance.StartsWith("mozilla") && instance.Contains("symbian")) && !instance.Contains("macintosh")))
                || ((instance.StartsWith("mozilla") && instance.Contains("nokia") && !instance.Contains("macintosh"))
                || (instance.StartsWith("mozilla") && instance.Contains("smartphone") && !instance.Contains("macintosh"))))))
                || (((((instance.StartsWith("opera") && instance.Contains("opera-mini")) || (instance.StartsWith("opera") && instance.Contains("opera mini")))
                || (instance.StartsWith("sonyericsson") || instance.StartsWith("sec-"))) || ((instance.StartsWith("lg-") || instance.StartsWith("blackberry")) 
                || (instance.StartsWith("samsung") || instance.StartsWith("htc")))) || ((instance.Contains("sti") && instance.Contains("ctv")) || instance.StartsWith("ct65")))))
            {
                return true;
            }

            return false;
        }
    }
}
