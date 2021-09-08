using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Configuration;
using Inventrax_CommonBackGroundService.BL.EmailHandler;
using Inventrax_CommonBackGroundService.BL.ExcellGenerator;
using Inventrax_CommonBackGroundService.BL.PDFGenerator;

namespace Inventrax_CommonBackGroundService.BL.EmailHandler
{
    class OBDSummaryEmailService :ICommonService
    {
        public void Execute()
        {
            FetchPendingTenantListForOBDSummary();

            
        }




        private void FetchPendingTenantListForOBDSummary()
        {
            try
            {
                // need to change this query to Pending tenant List
                string strQuery = " [dbo].[sp_Email_GetEmailScheduledEmails] @ReportType='OBDSummary' ";
                DataSet ds = MRLWMSC21Common.DB.GetDS(strQuery, false);
                if (ds != null && ds.Tables[0] != null && ds.Tables[0].Rows.Count != 0)
                {

                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        PrepareEmail(dr["TenantID"].ToString() ,dr["TenantCode"].ToString(), dr["ID"].ToString(), dr["WarehouseID"].ToString());
                    }
                }

            }
            catch (Exception ex)
            {

                LogWriter.WriteLog("Exception in method OBDSummaryEmailService.FetchPendingTenantListForOBDSummary()  " + ex.Message);
                ex.Source = "OBDSummaryEmailService.FetchPendingTenantListForOBDSummary()";
                LogWriter.WriteLog("Exception in method OBDSummaryEmailService.FetchPendingTenantListForOBDSummary()  " + ex.Message);

                MailUtility mail = new MailUtility();
                mail.SendExceptionEmail(ex);
            }



        }

        private void PrepareEmail(string tenantID,string tenantCode,string emailStatusID,string WarehouseID)
        {
            try
            {

                string _Environment = ConfigurationManager.AppSettings["ApplicationEnvironment"];
                string strQuery = "EXEC [dbo].[SP_RPT_GetOutboundSummaryReportEmail] @TenantId=" + tenantID+",@WarehouseID= "+WarehouseID;

                DataSet ds = MRLWMSC21Common.DB.GetDS(strQuery, false);
                ds.Tables[0].TableName = "OBDSummary";

                DataTable dtEmail = ds.Tables[1];

                

                String WHName = dtEmail.Rows[0]["WHName"].ToString();
                if (ds != null && ds.Tables.Count == 2 && ds.Tables[0].Rows.Count != 0)
                {


                    string folderPath = ConfigurationManager.AppSettings["ExcellFolderPath"];
                    string fileName = folderPath + tenantCode+"_OBDSummary_" +DateTime.Now.Day+""+DateTime.Now.Month+""+DateTime.Now.Year+"_"+ DateTime.Now.Hour + ".pdf";


                    #region  Section for Excell Creation
                        //IExcellGenerator excell = new OBDSummaryExcellGenerator();
                        //excell.Create(ds.Tables[0], tenantCode, "", fileName);

                        IPDFGenerator PDF = new OBDSummaryPDFGenerator();
                        PDF.Create(ds.Tables[0], tenantCode, WHName, fileName);

                       System.Threading.Thread.Sleep(30000);

                    #endregion

                    MailUtility mail = new MailUtility();
                    string _Date = DateTime.Now.ToString("dd-MMM-yyyy");

                    string _Time = DateTime.Now.ToString("hh:mm:ss tt");

               
                    if (dtEmail != null && dtEmail.Rows.Count != 0)
                    {
                        string toEmailList = dtEmail.Rows[0]["ToEmailList"].ToString();
                        string ccEmailList = dtEmail.Rows[0]["CCEmailList"].ToString();

                        StringBuilder str = new StringBuilder();

                        str.Append("<br/><br/>");
                        str.Append("<h1>OBD Summary Notification from Shipper on" + DateTime.Now.Day + " " + DateTime.Today.ToString("MMMM") + "" + DateTime.Now.Year + " </h1>");
                        
                        str.Append("<br/><br/><br/> ");




                        mail.SendGeneralEmail(str.ToString(), "OBD Summary", toEmailList, _Environment + _Date + "  " + _Time + "[OBD SUMMARY]", fileName,ccEmailList);
                        


                        LogWriter.WriteLog("update Gen_EmailExecutionStatus set IsEmailSent = 1 where ID =" + emailStatusID);

                        MRLWMSC21Common.DB.ExecuteSQL("update Gen_EmailExecutionStatus set IsEmailSent=1 where ID=" + emailStatusID);

                   
                    }
                    else
                    {
                        Exception ex = new Exception("No email address menctioned for OBD Summary report Tenant:" + tenantCode);
                        mail = new MailUtility();
                        mail.SendExceptionEmail(ex);
                    }
                 

                }
            }
            catch (Exception ex)
            {
                LogWriter.WriteLog("Exception in method OBDSummaryEmailService.PrepareMailBody()  " + ex.Message);
                ex.Source = "OBDSummaryEmailService.PrepareMailBody()";
                LogWriter.WriteLog("Exception in method OBDSummaryEmailService.PrepareMailBody()  " + ex.Message);

                MailUtility mail = new MailUtility();
                mail.SendExceptionEmail(ex);
            }


        }
    }
}
