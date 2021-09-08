using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MRLWMSC21Common;
using Microsoft.Reporting.WebForms;

namespace MRLWMSC21.mReports
{
    public partial class JRPReport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ReportCommon.LoadDropDownfromXML(ddlPrinters, "PrinterIPSettings.xml");            
        }

        protected void lnkGetReport_Click(object sender, EventArgs e)
        {
            if (txtKitCode.Text != "" && txtJobOrderRefNo.Text != "")
            {
                rvJRPReport.Visible = true;
                rvJRPReport.ServerReport.ReportServerUrl = new Uri(CommonLogic.Application("ReportServerUrl"));
                rvJRPReport.ServerReport.ReportPath = CommonLogic.Application("ReportPath") + "JRPReport2";
                rvJRPReport.ShowParameterPrompts = false;
                rvJRPReport.ShowPrintButton = true;
                
                Microsoft.Reporting.WebForms.ReportParameter[] reportParameterCollection = new Microsoft.Reporting.WebForms.ReportParameter[1];               

                reportParameterCollection[0] = new Microsoft.Reporting.WebForms.ReportParameter("ProductionOrderHeaderID", (hifJobRefNoNumber.Value != "" && txtJobOrderRefNo.Text != "" ? hifJobRefNoNumber.Value : "NULL"));

                rvJRPReport.ServerReport.SetParameters(reportParameterCollection);
                rvJRPReport.ServerReport.Refresh();
            }
            else
            {
                resetError("Select Kit Code & Job Order Ref. No.", true);            
            }
        }

        private void resetError(string error, bool isError)
        {

            ScriptManager.RegisterStartupScript(this, this.GetType(), "test", "showStickyToast(" + (isError.ToString().ToLower() == "true" ? "false" : "true") + ",\"" + error + "\");", true);

        }

        protected void lnkPrint_Click(object sender, EventArgs e)
        {

        }



    }
}