
using System;
using System.Configuration;
using System.Xml;
using System.Web;
using System.IO;
using System.Globalization;
using System.Text.RegularExpressions;

namespace MRLWMSC21Common
{
    /// <summary>
    /// Summary description for Localization.
    /// </summary>
    public class Localization
    {
        static private CultureInfo USCulture = new CultureInfo("en-US");

        static private String WebConfigLocale = String.Empty;
        static private String SqlServerLocale = String.Empty;
        static private CultureInfo SqlServerCulture;

        public Localization() { }

        static public String GetWebConfigLocale()
        {
            if (WebConfigLocale.Length == 0)
            {
                //System.Web.Configuration.SystemWebSectionGroup sw = (System.Web.Configuration.SystemWebSectionGroup)(System.Web.Configuration.WebConfigurationManager.GetSection("system.web"));
                //WebConfigLocale = CheckLocaleSettingForProperCase(sw.Globalization.Culture);

                XmlDocument doc = new XmlDocument();
                doc.Load(CommonLogic.SafeMapPath("~/web.config")); // Always the top App web.config
                XmlNode node = doc.DocumentElement.SelectSingleNode("//globalization");
                WebConfigLocale = CheckLocaleSettingForProperCase(node.Attributes["culture"].InnerText);

            }
            return WebConfigLocale;
        }

        static public String GetPrimaryCurrency()
        {
            return StoreCurrency();
        }

        static public String GetUSLocale()
        {
            return "en-US";
        }

        static public System.Data.DataSet GetNonUSLocales()
        {
            return DB.GetDS("SELECT * FROM LocaleSetting  " + DB.GetNoLock() + " WHERE [Name]<>'en-US' ORDER BY displayorder,Description", true, System.DateTime.Now.AddHours(1));
        }

        static public String CheckLocaleSettingForProperCase(String LocaleSetting)
        {
            // make sure locale is xx-XX:
            int i = LocaleSetting.IndexOf("-");
            if (i != -1)
            {
                LocaleSetting = LocaleSetting.Substring(0, i) + "-" + LocaleSetting.Substring(i + 1, LocaleSetting.Length - (i + 1)).ToUpperInvariant();
            }
            return LocaleSetting;
        }

        static public String CheckCurrencySettingForProperCase(String CurrencySetting)
        {
            return CurrencySetting.ToUpperInvariant();
        }

        static public String GetSqlServerLocale()
        {
            if (SqlServerLocale.Length == 0)
            {
                SqlServerLocale = CommonLogic.Application("DBSQLServerLocaleSetting");
                SqlServerCulture = new CultureInfo(SqlServerLocale);
            }
            return SqlServerLocale;
        }

        static public String WeightUnits()
        {
            return AppLogic.AppConfig("Localization.WeightUnits");
        }

        static public String ShortDateFormat()
        {
            String tmp = CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern.ToUpperInvariant();
            //			tmp = tmp.Replace("M","MM").Replace("D","DD");
            //			tmp = tmp.Replace("MMM","MM").Replace("DDD","DD"); //.Replace("YYYY","YY");
            //			tmp = tmp.Replace("MMM","MM").Replace("DDD","DD"); //.Replace("YYYY","YY");
            return tmp;
        }

        static public string JSCalendarDateFormatSpec()
        {
            // see jscalendar/calendar-setup.js for more info. Typical format would be: " + Localization.JSCalendarDateFormatSpec() + "
            String tmp = ShortDateFormat();
            tmp = tmp.Replace("MM", "%m").Replace("M", "%m").Replace("DD", "%d").Replace("D", "%d").Replace("YYYY", "%Y").Replace("YY", "%Y");
            return tmp;
        }

        static public String JSCalendarLanguageFile()
        {
            return "calendar-" + GetWebConfigLocale().Substring(0, 2) + ".js";
        }

        static public String StoreCurrency()
        {
            String tmpS = AppLogic.AppConfig("Localization.StoreCurrency").ToUpperInvariant();
            if (tmpS.Length == 0)
            {
                tmpS = "USD"; // set some default
            }
            return tmpS;
        }

        static public String StoreCurrencyNumericCode()
        {
            String tmpS = AppLogic.AppConfig("Localization.StoreCurrencyNumericCode");
            if (tmpS.Length == 0)
            {
                tmpS = "840"; // set some default
            }
            return tmpS;
        }

        static public bool ParseBoolean(String s)
        {
            try
            {
                return System.Boolean.Parse(s);
            }
            catch
            {
                return false;
            }
        }

        static public int ParseUSInt(String s)
        {
            try
            {
                return System.Int32.Parse(s, USCulture);
            }
            catch
            {
                return 0;
            }
        }

        static public int ParseNativeInt(String s)
        {
            try
            {
                return Localization.ParseUSInt(s); // use default locale setting
            }
            catch
            {
                return 0;
            }
        }

        static public long ParseUSLong(String s)
        {
            try
            {
                return System.Int64.Parse(s, USCulture);
            }
            catch
            {
                return 0;
            }
        }

        static public long ParseNativeLong(String s)
        {
            try
            {
                return System.Int64.Parse(s); // use default locale setting
            }
            catch
            {
                return 0;
            }
        }

        static public Single ParseUSSingle(String s)
        {
            try
            {
                return System.Single.Parse(s, USCulture);
            }
            catch
            {
                return 0.0F;
            }
        }

        static public Single ParseNativeSingle(String s)
        {
            try
            {
                return System.Single.Parse(s); // use default locale setting
            }
            catch
            {
                return 0.0F;
            }
        }

        static public Double ParseUSDouble(String s)
        {
            try
            {
                return System.Double.Parse(s, USCulture);
            }
            catch
            {
                return 0.0F;
            }
        }

        static public Double ParseNativeDouble(String s)
        {
            try
            {
                return System.Double.Parse(s); // use default locale setting
            }
            catch
            {
                return 0.0F;
            }
        }

        static public decimal ParseUSCurrency(String s)
        {
            s = s.Replace("$", "");
            try
            {
                return System.Decimal.Parse(s, USCulture);
            }
            catch
            {
                return System.Decimal.Zero;
            }
        }

        static public decimal ParseNativeCurrency(String s)
        {
            try
            {
                return System.Decimal.Parse(s);
            }
            catch
            {
                return System.Decimal.Zero;
            }
        }

        static public decimal ParseUSDecimal(String s)
        {
            try
            {
                return System.Decimal.Parse(s, USCulture);
            }
            catch
            {
                return System.Decimal.Zero;
            }
        }

        static public decimal ParseNativeDecimal(String s)
        {
            try
            {
                return System.Decimal.Parse(s);
            }
            catch
            {
                return System.Decimal.Zero;
            }
        }

        static public DateTime ParseUSDateTime(String s)
        {
            try
            {
                return System.DateTime.Parse(s, USCulture);
            }
            catch
            {
                return System.DateTime.MinValue;
            }
        }

        static public DateTime ParseNativeDateTime(String s)
        {
            try
            {
                return System.DateTime.Parse(s); // use default locale setting
            }
            catch
            {
                return System.DateTime.MinValue;
            }
        }


        static public String ToUSShortDateString(DateTime dt)
        {
            if (dt == System.DateTime.MinValue)
            {
                return String.Empty;
            }
            return dt.ToString("MM/dd/yy"); //dt.Month.ToString().PadLeft(2,'0') + "/" + dt.Day.ToString().PadLeft(2,'0') + "/" + dt.Year.ToString().Substring(2,2);
        }

        static public String ToNativeShortDateString(DateTime dt)
        {
            if (dt == System.DateTime.MinValue)
            {
                return String.Empty;
            }
            return dt.ToShortDateString();
        }

        static public String ToUSDateTimeString(DateTime dt)
        {
            if (dt == System.DateTime.MinValue)
            {
                return String.Empty;
            }
            return dt.ToString("MM/dd/yyyy HH:mm:ss"); //dt.Month.ToString().PadLeft(2,'0') + "/" + dt.Day.ToString().PadLeft(2,'0') + "/" + dt.Year.ToString().Substring(2,2) + " " + dt.Hour.ToString().PadLeft(2,'0') + ":" + dt.Minute.ToString().PadLeft(2,'0') + ":" + dt.Second.ToString().PadLeft(2,'0') + "." + dt.Millisecond.ToString() + " " + dt.TimeOfDay;
        }

        static public String ToNativeDateTimeString(DateTime dt)
        {
            if (dt == System.DateTime.MinValue)
            {
                return String.Empty;
            }
            return dt.ToString(new CultureInfo(Localization.GetWebConfigLocale()));
        }

        static public String ToDBDateTimeString(DateTime dt)
        {
            return DateTimeStringForDB(dt);
        }

        static public String ToDBShortDateString(DateTime dt)
        {
            return DateStringForDB(dt);
        }

        static public DateTime ParseDBDateTime(String s)
        {
            try
            {
                return System.DateTime.Parse(s, SqlServerCulture);
            }
            catch
            {
                return System.DateTime.MinValue;
            }
        }
        
        static public Double ParseDBDouble(String theval)
        {
            try
            {
                return System.Double.Parse(theval, SqlServerCulture);
            }
            catch
            {
                return 0.0D;
            }
        }

        static public Single ParseDBSingle(String theval)
        {
            try
            {
                return System.Single.Parse(theval, SqlServerCulture);
            }
            catch
            {
                return 0.0F;
            }
        }

        static public Decimal ParseDBDecimal(String theval)
        {
            try
            {
                return System.Decimal.Parse(theval, SqlServerCulture);
            }
            catch
            {
                return 0.0M;
            }
        }


        static public DateTime ParseLocaleDateTime(String theval, String LocaleSetting)
        {
            try
            {
                return System.DateTime.Parse(theval, new CultureInfo(LocaleSetting));
            }
            catch
            {
                return System.DateTime.MinValue;
            }
        }
        
        static public Double ParseLocaleDouble(String theval, String LocaleSetting)
        {
            try
            {
                return System.Double.Parse(theval, new CultureInfo(LocaleSetting));
            }
            catch
            {
                return 0.0D;
            }
        }

        static public Single ParseLocaleSingle(String theval, String LocaleSetting)
        {
            try
            {
                return System.Single.Parse(theval, new CultureInfo(LocaleSetting));
            }
            catch
            {
                return 0.0F;
            }
        }

        static public Decimal ParseLocaleDecimal(String theval, String LocaleSetting)
        {
            try
            {
                return System.Decimal.Parse(theval, new CultureInfo(LocaleSetting));
            }
            catch
            {
                return 0.0M;
            }
        }


        // ----------------------------------------------------------------------------------------------
        // the following routines must (should) work in ALL locales, no matter what SQL Server setting is
        // ----------------------------------------------------------------------------------------------
        static public String DateStringForDB(DateTime dt)
        {
            return dt.ToString("yyyyMMdd");
        }

        static public String DateTimeStringForDB(DateTime dt)
        {
            return dt.ToString("s");
        }

        static public String CurrencyStringForDBWithoutExchangeRate(decimal amt)
        {
            String tmpS = amt.ToString("C", USCulture);
            if (tmpS.StartsWith("("))
            {
                tmpS = "-" + tmpS.Replace("(", "").Replace(")", "");
            }
            return tmpS.Replace("$", "").Replace(",", "");
        }

        static public String IntStringForDB(int amt)
        {
            return amt.ToString("G", USCulture).Replace(",", "");
        }

        static public String SingleStringForDB(Single amt)
        {
            return amt.ToString("G", USCulture).Replace(",", "");
        }

        static public String DoubleStringForDB(double amt)
        {
            return amt.ToString("G", USCulture).Replace(",", "");
        }

        static public String DecimalStringForDB(decimal amt)
        {
            return amt.ToString("G", USCulture).Replace(",", "");
        }

        // ------------------------------------------------------------------------------
        // W3C DateTime Formats:
        // ------------------------------------------------------------------------------
        public struct W3CDateTime
        {
            private DateTime dtime;
            private TimeSpan ofs;

            public W3CDateTime(DateTime dt, TimeSpan off)
            {
                ofs = off;
                dtime = dt;
            }

            public DateTime UtcTime
            {
                get { return dtime; }
            }

            public DateTime DateTime
            {
                get { return dtime + ofs; }
            }

            public TimeSpan UtcOffset
            {
                get { return ofs; }
            }

            static public W3CDateTime Parse(string s)
            {
                const string Rfc822DateFormat =
                          @"^((Mon|Tue|Wed|Thu|Fri|Sat|Sun), *)?(?<day>\d\d?) +" +
                          @"(?<month>Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec) +" +
                          @"(?<year>\d\d(\d\d)?) +" +
                          @"(?<hour>\d\d):(?<min>\d\d)(:(?<sec>\d\d))? +" +
                          @"(?<ofs>([+\-]?\d\d\d\d)|UT|GMT|EST|EDT|CST|CDT|MST|MDT|PST|PDT)$";
                const string W3CDateFormat =
                          @"^(?<year>\d\d\d\d)" +
                          @"(-(?<month>\d\d)(-(?<day>\d\d)(T(?<hour>\d\d):(?<min>\d\d)(:(?<sec>\d\d)(?<ms>\.\d+)?)?" +
                          @"(?<ofs>(Z|[+\-]\d\d:\d\d)))?)?)?$";

                string combinedFormat = string.Format(
                    @"(?<rfc822>{0})|(?<w3c>{1})", Rfc822DateFormat, W3CDateFormat);

                // try to parse it
                Regex reDate = new Regex(combinedFormat);
                Match m = reDate.Match(s);
                if (!m.Success)
                {
                    // Didn't match either expression. Throw an exception.
                    throw new FormatException("String is not a valid date time stamp.");
                }
                try
                {
                    bool isRfc822 = m.Groups["rfc822"].Success;
                    int year = int.Parse(m.Groups["year"].Value);
                    // handle 2-digit and 3-digit years
                    if (year < 1000)
                    {
                        if (year < 50) year = year + 2000; else year = year + 1999;
                    }

                    int month;
                    if (isRfc822)
                        month = ParseRfc822Month(m.Groups["month"].Value);
                    else
                        month = (m.Groups["month"].Success) ? int.Parse(m.Groups["month"].Value) : 1;

                    int day = m.Groups["day"].Success ? int.Parse(m.Groups["day"].Value) : 1;
                    int hour = m.Groups["hour"].Success ? int.Parse(m.Groups["hour"].Value) : 0;
                    int min = m.Groups["min"].Success ? int.Parse(m.Groups["min"].Value) : 0;
                    int sec = m.Groups["sec"].Success ? int.Parse(m.Groups["sec"].Value) : 0;
                    int ms = m.Groups["ms"].Success ? (int)Math.Round((1000 * double.Parse(m.Groups["ms"].Value))) : 0;

                    TimeSpan ofs = TimeSpan.Zero;
                    if (m.Groups["ofs"].Success)
                    {
                        if (isRfc822)
                            ofs = ParseRfc822Offset(m.Groups["ofs"].Value);
                        else
                            ofs = ParseW3COffset(m.Groups["ofs"].Value);
                    }
                    // datetime is stored in UTC
                    return new W3CDateTime(new DateTime(year, month, day, hour, min, sec, ms) - ofs, ofs);
                }
                catch (Exception ex)
                {
                    throw new FormatException("String is not a valid date time stamp.", ex);
                }
            }

            private static readonly string[] MonthNames = new string[]
	{
		"Jan", "Feb", "Mar", "Apr", "May", "Jun", 
		"Jul", "Aug", "Sep", "Oct", "Nov", "Dec"};

            private static int ParseRfc822Month(string monthName)
            {
                for (int i = 0; i < 12; i++)
                {
                    if (monthName == MonthNames[i])
                    {
                        return i + 1;
                    }
                }
                throw new ApplicationException("Invalid month name");
            }

            private static TimeSpan ParseRfc822Offset(string s)
            {
                if (s == string.Empty)
                    return TimeSpan.Zero;
                int hours = 0;
                switch (s)
                {
                    case "UT":
                    case "GMT":
                        break;
                    case "EDT": hours = -4; break;
                    case "EST":
                    case "CDT": hours = -5; break;
                    case "CST":
                    case "MDT": hours = -6; break;
                    case "MST":
                    case "PDT": hours = -7; break;
                    case "PST": hours = -8; break;
                    default:
                        if (s[0] == '+')
                        {
                            string sfmt = s.Substring(1, 2) + ":" + s.Substring(3, 2);
                            return TimeSpan.Parse(sfmt);
                        }
                        else
                            return TimeSpan.Parse(s.Insert(s.Length - 2, ":"));
                }
                return TimeSpan.FromHours(hours);
            }

            private static TimeSpan ParseW3COffset(string s)
            {
                if (s == string.Empty || s == "Z")
                    return TimeSpan.Zero;
                else
                {
                    if (s[0] == '+')
                        return TimeSpan.Parse(s.Substring(1));
                    else
                        return TimeSpan.Parse(s);
                }
            }
        }

        // ------------------------------------------------------------------------------
        // Type Formatting
        // ------------------------------------------------------------------------------
        public static string FormatDecimal2Places(decimal temp)
        {
            return temp.ToString("N2");
        }

        public static string FormatDecimal2Places(string temp)
        {
            decimal dec = Localization.ParseDBDecimal(temp);
            return dec.ToString("N2");
        }
    }
}
