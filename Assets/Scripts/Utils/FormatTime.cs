using System;
using System.Globalization;

namespace Utils.Extensions
{
    public static class FormatTime
    {
        public static string HoursStringFormat(int totalHours)
        {
            var hours = totalHours / 3600;
            var minutes = totalHours % 3600 / 60;
            var seсonds = totalHours % 60;
            return $"{hours:00}h {minutes:00}m";
        }

        public static string DayAndHoursToString(int totalSeconds)
        {
            var days = totalSeconds / 86400;
            var hours = totalSeconds % 86400 / 3600;
            
            return $"{days:00}d {hours:00}h";
        }
        
        public static string MinutesStringFormat(int totalSeconds)
        {
            var minutes = totalSeconds / 60;
            var seconds = totalSeconds % 60;

            return $"{minutes:00}m{seconds:00}s";
        }
        
        public static int GetSecondsFromHoursInt(int totalHours)
        {
            return totalHours * 3600;
        }
        
        public static int GetSecondsFromMinutesInt(int totalMinutes)
        {
            return totalMinutes * 60;
        }

        public static DateTime StrToDateTime(string strTime)
        {
            var aaa = Convert.ToDateTime(strTime);
            return aaa;
        }
        
        public static string DateTimeToStr(DateTime time)
        {
            return time.ToString(CultureInfo.CurrentCulture);
        }
    }
}