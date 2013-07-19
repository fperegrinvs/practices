namespace MTO.Practices.Common.Extensions
{
    using System;

    /// <summary>
    /// The date time extensions.
    /// </summary>
    public static class DateTimeExtensions
    {
        /// <summary>
        /// Converte de datetime para o formato ddmmyyyy em decimal
        /// </summary>
        /// <param name="date">data a ser convertida</param>
        /// <returns>resultado da conversão</returns>
        public static decimal ToDecimal(this DateTime date)
        {
            decimal result = (date.Day * 1000000) + (date.Month * 10000) + date.Year;
            return result;
        }
    }
}
