using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MRLWMSC21Common;
using Microsoft.Reporting.WebForms;
using System.IO;

namespace MRLWMSC21.mReports
{
    public partial class NonConformanceReport : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            ReportCommon.LoadDropDownfromXML(ddlPrinters, "PrinterIPSettings.xml");                   
            
        }

        protected void lnkGetReport_Click(object sender, EventArgs e)
        {
            

            if (txtkitCode.Text != "" && txtJobOrderRefNo.Text != "" && txtNCRefNo.Text != "")
            {
                string url;
                url = CommonLogic.Application("MRLWMSC21WCF") + DB.GetSqlS("select DefaultValue as s from SYS_SysConfigKey where SysConfigKey='NCAnalysisSheetLocation'");
                if (DB.GetSqlN("SELECT ISNULL((select TOP 1 NCReferenceNumber  from MFG_RoutingDetailsActivityCapture WHERE ProductionOrderHeaderID=" + hifJobRefNoNumber.Value + " AND NCReferenceNumber=" + txtNCRefNo.Text.Trim() + " AND IsNCAnalysisAttached=1 ) ,0) AS N ") != 0)
                {
                    url += txtNCRefNo.Text + ".pdf";
                    hyRefDoc.Visible = true;
                    ViewState["url"] = url;
                    
                }
                else
                {
                    hyRefDoc.Visible = false;
                }

                rvNonConformanceReport.Visible = true;
                rvNonConformanceReport.ServerReport.ReportServerUrl = new Uri(CommonLogic.Application("ReportServerUrl"));
                rvNonConformanceReport.ServerReport.ReportPath = CommonLogic.Application("ReportPath") + "NonConformanceReport_New";                
                rvNonConformanceReport.ShowPrintButton = true;

                Microsoft.Reporting.WebForms.ReportParameter[] reportParameterCollection = new Microsoft.Reporting.WebForms.ReportParameter[2];

                reportParameterCollection[0] = new Microsoft.Reporting.WebForms.ReportParameter("NCRefNo", (hifNCRefNo.Value != "" ? hifNCRefNo.Value : null));
                reportParameterCollection[1] = new Microsoft.Reporting.WebForms.ReportParameter("ProductionOrderHeaderID", hifJobRefNoNumber.Value);

                rvNonConformanceReport.ServerReport.SetParameters(reportParameterCollection);
                rvNonConformanceReport.ServerReport.Refresh();
            }
            else
            {
                resetError("Select Kit Code, Job Order Ref. No. & NC Ref. No.", true);
            }

        }
      

        protected void lnkPrint_Click(object sender, EventArgs e)
        {

        }

        private void resetError(string error, bool isError)
        {

            ScriptManager.RegisterStartupScript(this, this.GetType(), "test", "showStickyToast(" + (isError.ToString().ToLower() == "true" ? "false" : "true") + ",\"" + error + "\");", true);

        }

        protected void lnkRefdoc_Click(object sender, EventArgs e)
        {
            

            if (txtNCRefNo.Text.Trim() == "" && txtJobOrderRefNo.Text.Trim() == "" && txtkitCode.Text.Trim() == "")
            {
                return;
            }
            else
            {
                //CommonLogic.Application("MRLWMSC21WCF") + "/SFTResources/NCAnalysisSheet/";
                ////string filename = txtNCRefNo.Text + ".pdf";
                //string path = MapPath(url);
                //byte[] bts = System.IO.File.ReadAllBytes(url);
                //Response.Clear();
                //Response.ClearHeaders();
                //Response.AddHeader("Content-Type", "application/pdf");
                //Response.AddHeader("Content-Length", bts.Length.ToString());
                //Response.AddHeader("Content-Disposition", "attachment;filename=\"" +txtNCRefNo.Text + ".pdf"+ "\"");
                

                System.Net.HttpWebRequest objRequest = (System.Net.HttpWebRequest)System.Net.HttpWebRequest.Create(ViewState["url"].ToString());
                System.Net.HttpWebResponse objResponse = (System.Net.HttpWebResponse)objRequest.GetResponse();
                int bufferSize = 1;
                Response.Clear();
                Response.ClearHeaders();
                Response.ClearContent();
                Response.AppendHeader("Content-Disposition:", "attachment; filename=NCReferenceDocument.pdf");
                Response.AppendHeader("Content-Length", objResponse.ContentLength.ToString());
                Response.ContentType = "application/pdf";                
             

                byte[] byteBuffer = new byte[bufferSize + 1];
                MemoryStream memStrm = new MemoryStream(byteBuffer, true);
                Stream strm = objRequest.GetResponse().GetResponseStream();
                byte[] bytes = new byte[bufferSize + 1];

                while (strm.Read(byteBuffer, 0, byteBuffer.Length) > 0)
                {
                    Response.BinaryWrite(memStrm.ToArray());
                    Response.Flush();
                }

                Response.Close();
                Response.End();
                memStrm.Close();
                memStrm.Dispose();
                strm.Dispose();

            } 
            
        }        

    }
}