namespace MTO.Practices.Common.Helper
{
    using System.Globalization;

    /// <summary>
    /// métodos de validação
    /// </summary>
    public static class Validation
    {
        /// <summary>
        /// Realiza a validação do CNPJ
        /// </summary>
        /// <param name="cnpj">
        /// The cnpj.
        /// </param>
        /// <returns>
        /// True caso o cnpj seja válido, false em caso contrário
        /// </returns>
        public static bool ValidaCNPJ(string cnpj)
        {
            var multiplicador1 = new[] { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            var multiplicador2 = new[] { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            cnpj = cnpj.Trim();
            cnpj = cnpj.Replace(".", string.Empty).Replace("-", string.Empty).Replace("/", string.Empty);
            if (cnpj.Length != 14)
            {
                return false;
            }

            var tempCnpj = cnpj.Substring(0, 12);
            var soma = 0;
            for (int i = 0; i < 12; i++)
            {
                soma += int.Parse(tempCnpj[i].ToString(CultureInfo.InvariantCulture)) * multiplicador1[i];
            }

            var resto = soma % 11;
            if (resto < 2)
            {
                resto = 0;
            }
            else
            {
                resto = 11 - resto;
            }

            var digito = resto.ToString(CultureInfo.InvariantCulture);
            tempCnpj = tempCnpj + digito;
            soma = 0;
            for (var i = 0; i < 13; i++)
            {
                soma += int.Parse(tempCnpj[i].ToString(CultureInfo.InvariantCulture)) * multiplicador2[i];
            }

            resto = soma % 11;
            if (resto < 2)
            {
                resto = 0;
            }
            else
            {
                resto = 11 - resto;
            }

            digito = digito + resto.ToString(CultureInfo.InvariantCulture);
            return cnpj.EndsWith(digito);
        }

        /// <summary>
        /// Valida um CPF
        /// </summary>
        /// <param name="cpf">cpf a ser validado</param>
        /// <returns>true caso o cpf seja válido, false em caso contrário</returns>
        public static bool ValidaCpf(string cpf)
        {
            var multiplicador1 = new[] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            var multiplicador2 = new[] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            cpf = cpf.Trim();
            cpf = cpf.Replace(".", string.Empty).Replace("-", string.Empty);
            if (cpf.Length != 11)
            {
                return false;
            }

            var tempCpf = cpf.Substring(0, 9);
            var soma = 0;

            for (int i = 0; i < 9; i++)
            {
                soma += int.Parse(tempCpf[i].ToString(CultureInfo.InvariantCulture)) * multiplicador1[i];
            }

            var resto = soma % 11;
            if (resto < 2)
            {
                resto = 0;
            }
            else
            {
                resto = 11 - resto;
            }

            var digito = resto.ToString(CultureInfo.InvariantCulture);
            tempCpf = tempCpf + digito;
            soma = 0;
            for (var i = 0; i < 10; i++)
            {
                soma += int.Parse(tempCpf[i].ToString(CultureInfo.InvariantCulture)) * multiplicador2[i];
            }

            resto = soma % 11;
            if (resto < 2)
            {
                resto = 0;
            }
            else
            {
                resto = 11 - resto;
            }

            digito = digito + resto.ToString(CultureInfo.InvariantCulture);
            return cpf.EndsWith(digito);
        }
    }
}
