namespace MTO.Practices.Common
{
    using System.Globalization;
    using System.Text;

    public class TextFormatter
    {

        public static string RemoverAcentosEspacos(string texto)
        {
            if (!string.IsNullOrEmpty(texto))
            {
                string s = texto.Normalize(NormalizationForm.FormD);

                StringBuilder sb = new StringBuilder();

                for (int k = 0; k < s.Length; k++)
                {
                    UnicodeCategory uc = CharUnicodeInfo.GetUnicodeCategory(s[k]); 
                    if (uc != UnicodeCategory.NonSpacingMark)
                    {
                        if (s[k] == ' ')
                        {
                            sb.Append('-');
                        }
                        else
                        {
                            sb.Append(s[k]);
                        }
                    }
                }
                return sb.ToString();
            }

            return texto;
        }

        public static string RemoverAcentos(string texto)
        {
            string comAcentos = "ÄÅÁÂÀÃäáâàãÉÊËÈéêëèÍÎÏÌíîïìÖÓÔÒÕöóôòõÜÚÛüúûùÇç";
            string semAcentos = "AAAAAAaaaaaEEEEeeeeIIIIiiiiOOOOOoooooUUUuuuuCc";
            for (int i = 0; i < comAcentos.Length; i++)
            {
                texto = texto.Replace(comAcentos[i].ToString(), semAcentos[i].ToString());
            }
            return texto;
        }
    }
}
