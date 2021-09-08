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
    class GRNEmailService :ICommonService
    {
        public void Execute()
        {
            FetchOpenGRNOrders();
        }

        private void FetchOpenGRNOrders()
        {
            try
            {
                string strQuery = " EXEC [dbo].[sp_Email_GetPendingInbound] ";
                DataSet ds = MRLWMSC21Common.DB.GetDS(strQuery, false);
                if (ds != null && ds.Tables[0] != null && ds.Tables[0].Rows.Count != 0)
                {

                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        PrepareEmail(dr["InboundID"].ToString(), dr["WHCode"].ToString(), dr["WHName"].ToString(), dr["StoreRefNo"].ToString(), dr["TenantName"].ToString());
                    }
                }

            }
            catch(Exception ex)
            {
                
                LogWriter.WriteLog("Exception in method GRNEmailService.FetchOpenGRNOrders()  " + ex.Message);
                ex.Source = "GRNEmailService.FetchOpenGRNOrders()";
                LogWriter.WriteLog("Exception in method GRNEmailService.FetchOpenGRNOrders()  " + ex.Message);

                MailUtility mail = new MailUtility();
                mail.SendExceptionEmail(ex);
            }

            

        }

        private void PrepareEmail(string InboundID,string wareHouseCode,string wareHouseName,string storeRefNum,string TenantName)
        {
            try
            {

                string _Environment = ConfigurationManager.AppSettings["ApplicationEnvironment"];
                string strQuery = "exec [dbo].[sp_Email_GetGRNData] " + InboundID;

                DataSet ds = MRLWMSC21Common.DB.GetDS(strQuery, false);
                if (ds != null && ds.Tables.Count==2 && ds.Tables[0].Rows.Count != 0)
                {
                    string folderPath = ConfigurationManager.AppSettings["ExcellFolderPath"]; 
                    string fileName = folderPath+storeRefNum + ".pdf";


                    #region  Section for Excell Creation
                        //IExcellGenerator excell = new GRNExcellGenerator();
                        //excell.Create(ds.Tables[0], TenantName, wareHouseCode + " - " + wareHouseName, fileName);

                        IPDFGenerator PDF = new GRNPDFGenerator();
                        PDF.Create(ds.Tables[0], TenantName, wareHouseCode + " - " + wareHouseName, fileName);
    
                       System.Threading.Thread.Sleep(30000);
                    #endregion





                    StringBuilder str = new StringBuilder();

                    str.Append("<br/><br/>");
                    str.Append("<h1>GRN Notification from "+ _Environment + " </h1>");
                    str.Append("<h3>Warehouse : "+ wareHouseName+"</h3>");
                    str.Append("<br/><br/><br/> ");



                    str.Append("<table style='width: 1000px' border='1'>");

                    str.Append("<tr><th>Received Date</th><th>Order Number</th><th>Material</th><th>Desc.</th><th>Quantity</th> <th>Batch#</th><th>MFG. Date</th><th>Exp. Date</th></tr>");


                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        str.Append("<tr>");
                        str.Append("<td style='width:10%'>" + dr["ReceivedDate"].ToString() + "</td>");
                        str.Append("<td style='width:20%'>" + dr["PONumber"].ToString() + "</td>");
                        str.Append("<td style='width:10%'>" + dr["MCode"].ToString() + "</td>");
                        str.Append("<td style='width:20%'>" + dr["MDescription"].ToString() + "</td>");
                        str.Append("<td style='width:10%'>" + dr["Quantity"].ToString() + "</td>");
                        str.Append("<td style='width:10%'>" + dr["BatchNo"].ToString() + "</td>");
                        str.Append("<td style='width:10%'>" + dr["MfgDate"].ToString() + "</td>");
                        str.Append("<td style='width:10%'>" + dr["ExpiryDate"].ToString() + "</td>");
                        str.Append("</tr>");
                    }
                    str.Append("<br/>");
                    str.Append("</table>");

                 
                    MailUtility mail = new MailUtility();
                    string _Date = DateTime.Now.ToString("dd-MMM-yyyy");

                    string _Time = DateTime.Now.ToString("hh:mm:ss tt");


                    DataTable dtEmail = ds.Tables[1];
                    if(dtEmail!=null && dtEmail.Rows.Count != 0)
                    {
                        string toEmailList = dtEmail.Rows[0]["ToEmailList"].ToString();
                        string ccEmailList = dtEmail.Rows[0]["CCEmailList"].ToString();
                        mail.SendGeneralEmail(str.ToString(), "GRN Notification", toEmailList, _Environment + _Date + "  " + _Time + "[  GRN Email Notification  Ref: " + storeRefNum + "  ]", fileName, ccEmailList);
                        MRLWMSC21Common.DB.ExecuteSQL("update INB_Inbound set IsEmailSent=1 where InboundID=" + InboundID);
                    }
                    else
                    {
                        Exception ex = new Exception("No email address menctioned for Inb Ref: " + storeRefNum);
                        mail = new MailUtility();
                        mail.SendExceptionEmail(ex);
                    }
                    

                    
                }
            }
            catch(Exception ex)
            {
                LogWriter.WriteLog("Exception in method GRNEmailService.PrepareMailBody()  " + ex.Message);
                ex.Source = "GRNEmailService.PrepareMailBody()";
                LogWriter.WriteLog("Exception in method GRNEmailService.PrepareMailBody()  " + ex.Message);

                MailUtility mail = new MailUtility();
                mail.SendExceptionEmail(ex);
            }
            

        }
    }
}
