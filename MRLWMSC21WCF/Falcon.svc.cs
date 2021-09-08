using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using MRLWMSC21Common;
using System.ServiceModel.Activation;
using System.IO;
using System.Collections;
using System.Xml;
namespace MRLWMSC21WCF
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Falcon" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Falcon.svc or Falcon.svc.cs at the Solution Explorer and start debugging.
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class Falcon : IFalcon
    {        
        private int errorCount = 0;
        private int successCount = 0;
        public void DoWork()
        {

        }
        public string PostIDocToFalcon(XmlDocument IdocInfo, string Client, string IdocNumber)
        {
            try
            {
                //LogWriter.WriteLog(inputData.ToString(), "Customer");
                String strQuery = "EXEC [dbo].[SP_Tally_InsertXMLInfo] @XMLInfo=" + DB.SQuote(IdocInfo.InnerXml);
                strQuery += ",@XMLName=" + DB.SQuote(IdocNumber);
                strQuery += ",@AccountName=" + DB.SQuote(Client);

                DB.ExecuteSQL(strQuery);
                //LogWriter.WriteLog(IdocInfo, "Success--" + successCount,"1");
                //LogWriter.WriteLog(IdocInfo, "Success--" + successCount,"1");
                successCount++;
                return "success";
            }
            catch (Exception ex)
            {

                String strQuery = "EXEC [dbo].[sp_SAP_InsertIDOC_FailedLog]@xml=" + DB.SQuote(IdocInfo.InnerXml);
                strQuery += ",@IdocName=" + DB.SQuote(IdocNumber);
                strQuery += ",@Client=" + DB.SQuote(Client);
                DB.ExecuteSQL(strQuery);


                string datetime = DateTime.Now.ToString();
                LogWriter.WriteLog(ex.ToString(), "Error--" + errorCount, "0");
                LogWriter.WriteLog(IdocInfo.InnerXml, "Error--" + errorCount, "0");
                errorCount++;
            }
            return "success";

        }

        public string PostWMSScanDataAsIDOC(string IdocInfo, string Client, string RefNumber, string IDOCName, string SiteCode)
        {

            try
            {
                String strQuery = "EXEC [dbo].[sp_FalconWMS_InsertIDOC]@xml=" + DB.SQuote(IdocInfo);
                strQuery += ",@IdocName=" + DB.SQuote(IDOCName);
                strQuery += ",@Client=" + DB.SQuote(Client);
                strQuery += ",@RefNumber=" + DB.SQuote(RefNumber);
                strQuery += ",@SiteCode=" + DB.SQuote(SiteCode);

                DB.ExecuteSQL(strQuery);
                return "success";
            }
            catch (Exception ex)
            {
                String strQuery = "EXEC [dbo].[sp_SAP_InsertIDOC_FailedLog]@xml=" + DB.SQuote(IdocInfo);
                strQuery += ",@IdocName=" + DB.SQuote(IDOCName);
                strQuery += ",@Client=" + DB.SQuote(Client);
                DB.ExecuteSQL(strQuery);
                string datetime = DateTime.Now.ToString();
                LogWriter.WriteLog(ex.ToString(), "Error--" + errorCount, "0");
                LogWriter.WriteLog(IdocInfo, "Error--" + errorCount, "0");
                errorCount++;
            }
            return "success";



        }
    }
}
