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
    class WHStockEMailSerice : ICommonService
    {
        public void Execute()
        {
            FetchPendingTenantListForWHSummary();
        }


        private void FetchPendingTenantListForWHSummary()
        {
            try
            {

                LogWriter.WriteLog("Block Entry Method  FetchPendingTenantListForWHSummary");
                // need to change this query to Pending tenant List

                string strQuery = " [dbo].[sp_Email_GetEmailScheduledEmails] @ReportType='WHStockInfo' ";
                DataSet ds = MRLWMSC21Common.DB.GetDS(strQuery, false);
                if (ds != null && ds.Tables[0] != null && ds.Tables[0].Rows.Count != 0)
                {

                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        PrepareEmail(dr["TenantID"].ToString(), dr["TenantCode"].ToString(), dr["ID"].ToString(), dr["WarehouseID"].ToString());
                    }
                }

            }
            catch (Exception ex)
            {

                LogWriter.WriteLog("Exception in method WHStockEMailSerice.FetchPendingTenantListForWHSummary()  " + ex.Message);
                ex.Source = "WHStockEMailSerice.FetchPendingTenantListForWHSummary()";
                LogWriter.WriteLog("Exception in method WHStockEMailSerice.FetchPendingTenantListForWHSummary()  " + ex.Message);

                MailUtility mail = new MailUtility();
                mail.SendExceptionEmail(ex);
            }



        }

        private void PrepareEmail(string tenantID, string tenantCode, string emailStatusID, string warehouseID)
        {
            try
            {

                LogWriter.WriteLog("Block Entry Method   WHStockEMailSerice.PrepareEmail");
                string _Environment = ConfigurationManager.AppSettings["ApplicationEnvironment"];
                string strQuery = "EXEC [Email_RPT_WarehouseStockInformation] @TenantID=" + tenantID+ ",@WarehouseID="+ warehouseID;

                DataSet ds = MRLWMSC21Common.DB.GetDS(strQuery, false);
                ds.Tables[0].TableName = "WarehouseSummary";
                if (ds != null && ds.Tables[0] != null && ds.Tables[0].Rows.Count != 0)
                {


                    string folderPath = ConfigurationManager.AppSettings["ExcellFolderPath"];
                    string fileName = folderPath + tenantCode + "_WHSummary_" + DateTime.Now.Day + "" + DateTime.Now.Month + "" + DateTime.Now.Year+"_"+DateTime.Now.Hour+""+ ".pdf";

                    DataTable dtEmail = ds.Tables[1];
                    String WHName = dtEmail.Rows[0]["WHName"].ToString();
                    #region  Section for Excell Creation

                    //IExcellGenerator excell = new WHStockExcellGenerator();
                    //    excell.Create(ds.Tables[0], tenantCode, "", fileName);

                    IPDFGenerator PDF = new WHStockPDFGenerator();
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
                        str.Append("<h1>Warehouse Summary Notification from Shipper on" + DateTime.Now.Day + " " + DateTime.Today.ToString("MMMM") + "" + DateTime.Now.Year + " </h1>");

                        str.Append("<br/><br/><br/> ");



                        mail.SendGeneralEmail(str.ToString(), "Warehouse Summary", toEmailList, _Environment + _Date + "  " + _Time + "[Warehouse SUMMARY:]", fileName, ccEmailList);

                        MRLWMSC21Common.DB.ExecuteSQL("update Gen_EmailExecutionStatus set IsEmailSent=1 where ID=" + emailStatusID);
                    }
                    else
                    {
                        Exception ex = new Exception("No email address menctioned for WH Summary report Tenant:" + tenantCode);
                        mail = new MailUtility();
                        mail.SendExceptionEmail(ex);
                    }







                }
            }
            catch (Exception ex)
            {
                LogWriter.WriteLog("Exception in method WHStockEMailSerice.PrepareMailBody()  " + ex.Message);
                ex.Source = "WHStockEMailSerice.PrepareMailBody()";
                LogWriter.WriteLog("Exception in method WHStockEMailSerice.PrepareMailBody()  " + ex.Message);

                MailUtility mail = new MailUtility();
                mail.SendExceptionEmail(ex);
            }


        }
    }
}
