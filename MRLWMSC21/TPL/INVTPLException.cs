using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace MRLWMSC21.TPL
{
    
    public class INVTPLException : Exception
    {

        public string message;
        //public static string filePath = @"D:\Inventrax369\Education\TPLInvoice\InvoiceLog";
        
        public static string filePath = System.Web.HttpContext.Current.Server.MapPath("~/" + "/TPL/Log/InvoiceLog");
        public static IDictionary<int, IDictionary<int, int>> rateConfig;

        public int _GroupID;
        public int _RateTypeID;
        public int _SpaceUtilizationID;

        public INVTPLException(string message)
        {
            this.message = message;
            //writeExceptionLog(message);
        }

        public INVTPLException(string message,int groupID,int rateTypeID,int spaceUtilizationID)
        {
            this.message = message;
            this._GroupID = groupID;
            this._RateTypeID = rateTypeID;
            this._SpaceUtilizationID = spaceUtilizationID;
            writeExceptionLog(message);
        }

        public void writeExceptionLog(string exceptionText)
        {

            try
            {
                using (var fileStream = new FileStream(filePath+"_"+DateTime.Now.ToString("yyyy-MM-dd")+".txt", FileMode.Append, FileAccess.Write, FileShare.None))
                using (var bw = new StreamWriter(fileStream))
                {
                    bw.Write(message + "[" + DateTime.Now + "] " + Environment.NewLine);
                }
            }
            catch (IOException ex)
            {
                //throws exception 
            }

        }

        public void nullRateAssign(string requiredRateInfo) 
        {
            writeExceptionLog(requiredRateInfo);
        }

        public void emptyRates(string message) 
        {
            writeExceptionLog(message);
        }

        public void suggestedRateInfo(int groupID, int rateTypeID, int spaceUtilizationID) 
        {
            
        }

    }

}