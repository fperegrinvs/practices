
namespace MTO.Practices.Common
{
    using System;
    using MTO.Practices.Common.Enumerators;

    /// <summary>
    /// DateTimeUtil
    /// </summary>
    public static class DateTimeUtil
    {
        public static DateTime GetDateTimeWithWeekDay(int days, WeekDayEnum weekDay)
        {
            var date = DateTime.Today;
            if (days > 0)
            {
                date = date.AddDays(days);
            }

            if (!date.DayOfWeek.ToString().Equals(weekDay.ToString()) && weekDay != WeekDayEnum.Desabilitado)
            {
                var dayOfWeekTodayInt = (int)(WeekDayEnum)date.DayOfWeek;
                var weekDayInt = (int)weekDay;

                var numberOfDays = weekDayInt - dayOfWeekTodayInt;
                if (dayOfWeekTodayInt >= weekDayInt)
                {
                    numberOfDays += 7;
                }

                date = date.AddDays(numberOfDays);
            }

            return date;
        }
    }
}
