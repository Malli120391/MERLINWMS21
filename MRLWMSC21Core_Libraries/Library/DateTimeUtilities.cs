using System;

namespace MRLWMSC21Core.Library
{
    public class DateTimeUtilities
    {

        public static DateTime Min(DateTime a, DateTime b)
        {
            if (a < b)
            {
                return a;
            }
            return b;
        }

        public static DateTime Max(DateTime a, DateTime b)
        {
            if (a > b)
            {
                return a;
            }
            return b;
        }

        public static DateTime FormatDateTime(String Input)
        {
            return DateTime.Now;
        }
        
        public static DateTime FormatDateTime(DateTime Input)
        {
            return DateTime.Now;
        }


        public static String FormatAndReturnDateTimeString(String Input)
        {
            return string.Empty;
        }

        public static String FormatAndReturnDateTimeString(DateTime Input)
        {
            return string.Empty;
        }

        public static bool IsValidDateTime(string Input)
        {
            return true;
        }

        public static bool IsValidDateTime(string Input, out DateTime DateAndTime)
        {
            DateAndTime = DateTime.Now;        
            return true;
        }
    }
}
