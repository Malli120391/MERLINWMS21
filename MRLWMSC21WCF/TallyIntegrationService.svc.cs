using MRLWMSC21Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Xml;


namespace MRLWMSC21WCF
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "TallyIntegrationService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select TallyIntegrationService.svc or TallyIntegrationService.svc.cs at the Solution Explorer and start debugging.
    public class TallyIntegrationService : ITallyIntegrationService
    {
        private int successCount;
        int errorCount;

        public void DoWork()
        {
        }

        public string PostIDocToFalconString(String IdocInfo, String Client, String IdocNumber)
        {
            try
            {
                //LogWriter.WriteLog(inputData.ToString(), "Customer");
                String strQuery = "EXEC [dbo].[SP_Tally_InsertXMLInfo] @XMLInfo=" + DB.SQuote(IdocInfo);
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

                String strQuery = "EXEC [dbo].[sp_SAP_InsertIDOC_FailedLog]@xml=" + DB.SQuote(IdocInfo);
                strQuery += ",@IdocName=" + DB.SQuote(IdocNumber);
                strQuery += ",@Client=" + DB.SQuote(Client);
                DB.ExecuteSQL(strQuery);


                string datetime = DateTime.Now.ToString();
                LogWriter.WriteLog(ex.ToString(), "Error--" + errorCount);
                LogWriter.WriteLog(IdocInfo, "Error--" + errorCount);
                errorCount++;
            }
            return "success";
        }

        public string PostIDocToFalconXML(MemoryStream xmlDocument, String Client, String IdocNumber)
        {

            XmlDocument IdocInfo = new XmlDocument();
            IdocInfo.Save(xmlDocument);

            //IdocInfo.Load(new StringReader(xmlData));

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
                LogWriter.WriteLog(ex.ToString(), "Error--" + errorCount);
                LogWriter.WriteLog(IdocInfo.InnerXml, "Error--" + errorCount);
                errorCount++;
            }
            return "success";
        }
    }
}
