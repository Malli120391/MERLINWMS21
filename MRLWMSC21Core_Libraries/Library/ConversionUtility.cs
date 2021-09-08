using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MRLWMSC21Core;

namespace MRLWMSC21Core.Library
{
    public static class ConversionUtility
    {
        public static int ConvertToInt(string sInput)
        {
            int _iResult = 0;

            if (!Int32.TryParse(sInput, out _iResult))
            { }//   throw (new Exception("Error Converting string to Integer in method - 'ConvertToInt'."));

            return _iResult;
        }


        public static decimal ConvertToDecimal(string sInput)
        {
            decimal _dResult = 0;

            if (!Decimal.TryParse(sInput, out _dResult))
            { }//     throw (new Exception("Error Converting string to Decimal in method - 'ConvertToDecimal'."));

            return _dResult;
        }


        public static DateTime ConvertToDateTime(string Input, string FormatSpecifier)
        {
            return DateTime.Now;
        }

        public static DateTime ConvertToDateTime(string Input)
        {
            return DateTime.Now;
        }

        public static string ConvertToDateString(DateTime Input)
        {
            return Input.ToString("dd-MMM-yyyy");
        }

        public static string ConvertToTimeString(DateTime Input)
        {
            return Input.ToString("hh:mm:ss tt");
        }

        public static string ConvertToDateTimeString (DateTime Input)
        {
            string _DateTime = string.Empty;

            _DateTime = Input.ToString("dd-MMM-yyyy hh:mm:ss tt");

            return _DateTime;
        }

        public static bool ConvertToBool(string value)
        {
                if (value == null)
                {
                    return false;
                }

                value = value.Trim();
                value = value.ToLower();
                if (value == "true")
                {
                    return true;
                }
               
                if (value == "t")
                {
                    return true;
                }

                if (value == "1")
                {
                    return true;
                }
                
                if (value == "yes")
                {
                    return true;
                }

                
                if (value == "y")
                {
                    return true;
                }
                return false;
        }
    }
}

