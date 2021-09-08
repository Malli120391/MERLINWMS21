using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace MRLWMSC21.TPL.Invoice
{
    public class CommonFunctions
    {

        public static string FilePath = System.Web.HttpContext.Current.Server.MapPath("~/" + "/TPL/Log/FunctionalLog");
        
        public static DateTime StringToDatetime(string datetimeStr)
        {
            return Convert.ToDateTime(datetimeStr);
        }

        public static string dateTimetoString(DateTime datetime)
        {
            try
            {
                return datetime.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss");
            }
            catch (Exception e)
            {
                return "";
            }
        }

        public static string dateTimetoString(DateTime datetime, string format)
        {
            try
            {
                return datetime.ToString(format);
            }
            catch (Exception e)
            {
                return "";
            }
        }

        public static void logWrite(string message)
        {

            try
            {
                using (var fileStream = new FileStream(FilePath, FileMode.Append, FileAccess.Write, FileShare.None))
                using (var bw = new StreamWriter(fileStream))
                {
                    bw.Write(message + "\n");
                }
            }
            catch (IOException ex)
            {
                throw new INVTPLException("Exception while writing logs: " + ex.Message);
            }

        }


        public static void writeFunctionalLog(string functionalText)
        {
            
            try
            {
                using (var fileStream = new FileStream(FilePath + "_" + DateTime.Now.ToString("yyyy-MM-dd") + ".html", FileMode.Append, FileAccess.Write, FileShare.None))
                using (var bw = new StreamWriter(fileStream))
                {
                    bw.Write("<Error> <br> "+functionalText + "[" + DateTime.Now.ToString("dd MMM yyyy") + "] " + Environment.NewLine+" <br> </Error>");
                }
            }
            catch (IOException ex)
            {
                //throws exception 
            }

        }

        public static decimal getUomConversionValue(int fromUomID,int toUomID,decimal value) 
        {
            return 0.00m;
        }

        
        public static decimal getCommonUoMValue(int fromUomID,int toUomID,decimal value)
        {

            
            /*  1	IN	Inch
                2	"3	Cubic inch
                34	M3	Cubic meter
             
                11	CM	Centimeter
                19	FT	Foot
                28	KM	Kilometer
                
                33	MTR	Meter
                35	MG	Milligram
                36	MI	Mile
                75	M2	Square Meter
                
                20	G	Gram
                26	KG	Kilogram
                29	KT	Kilotonne
             */


            return 0.00m;
        }

    }

}