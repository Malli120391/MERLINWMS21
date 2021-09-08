using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MRLWMSC21.Generic_Class.Class
{
    public class GenericFunctions
    {
        public string ConvertDateToString(string InputDate, string Seperator)
        {

            string _sRefDate = string.Empty;

            _sRefDate = InputDate.Replace(Seperator, "/");
            // shpDate = shpDate.Split('/')[0] + shpDate.Split('/')[1] + "/" + "/" + shpDate.Split('/')[2];

            int _iYearPart = Convert.ToInt32(_sRefDate.Split('/')[2]);
            int _iMonthPart = Convert.ToInt32(_sRefDate.Split('/')[1]);
            int _iDayPart = Convert.ToInt32(_sRefDate.Split('/')[0]);

            switch (_sRefDate.Split('/')[1].ToUpper())
            {
                case "JAN":
                    _iMonthPart = 1;
                    break;
                case "FEB":
                    _iMonthPart = 2;
                    break;
                case "MAR":
                    _iMonthPart = 3;
                    break;
                case "APR":
                    _iMonthPart = 4;
                    break;
                case "MAY":
                    _iMonthPart = 5;
                    break;
                case "JUN":
                    _iMonthPart = 6;
                    break;
                case "JUL":
                    _iMonthPart = 7;
                    break;
                case "AUG":
                    _iMonthPart = 8;
                    break;
                case "SEP":
                    _iMonthPart = 9;
                    break;
                case "OCT":
                    _iMonthPart = 10;
                    break;
                case "NOV":
                    _iMonthPart = 11;
                    break;
                case "DEC":
                    _iMonthPart = 12;
                    break;
            }

            string _OutputFormat = new DateTime(_iYearPart, _iMonthPart, _iDayPart).ToShortDateString();

            return _OutputFormat;
        }
    }
}