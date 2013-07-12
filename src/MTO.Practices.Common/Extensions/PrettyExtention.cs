namespace MTO.Practices.Common.Extensions
{
    using System;

    public static class PrettyExtention
    {
        public static string ToPrettyString(this TimeSpan timeSpan)
        {
            return System.Text.RegularExpressions.Regex.Replace(GetPrettyDate(timeSpan), "(Há|atrás)", string.Empty).Trim();
        }

        public static string ToPrettyString(this DateTime dateTime)
        {
            TimeSpan s = DateTime.Now.Subtract(dateTime);

            if (s.TotalSeconds < 60)
            {
                return "agora mesmo";
            }

            return GetPrettyDate(s);
        }

        static string GetPrettyDate(TimeSpan s)
        {
            // 1.
            // Get total number of days elapsed.
            int dayDiff = (int)s.TotalDays;

            // 2.
            // Get total number of seconds elapsed.
            int secDiff = (int)s.TotalSeconds;

            // 3.
            // Don't allow out of range values.
            if (dayDiff < 0 || dayDiff >= 31)
            {
                return null;
            }

            // 4.
            // Handle same-day times.
            if (dayDiff == 0)
            {
                // A.
                // Less than one minute ago.
                if (secDiff < 60)
                {
                    return secDiff + " segundos";
                }

                // B.
                // Less than 2 minutes ago.
                if (secDiff < 120)
                {
                    return "Há 1 minuto";
                }

                // C.
                // Less than one hour ago.
                if (secDiff < 3600)
                {
                    return string.Format("{0} minutos atrás",
                        Math.Round((double)secDiff / 60));
                }

                // D.
                // Less than 2 hours ago.
                if (secDiff < 7200)
                {
                    return "1 hora atrás";
                }

                // E.
                // Less than one day ago.
                if (secDiff < 86400)
                {
                    return string.Format("{0} horas atrás",
                        Math.Floor((double)secDiff / 3600));
                }
            }

            // 5.
            // Handle previous days.
            if (dayDiff == 1)
            {
                return "1 dia atrás";
            }

            if (dayDiff < 7)
            {
                return string.Format("{0} dias atrás",
                dayDiff);
            }

            if (dayDiff < 31)
            {
                return string.Format("{0} semanas atrás",
                Math.Ceiling((double)dayDiff / 7));
            }

            return null;
        }
    }
}
