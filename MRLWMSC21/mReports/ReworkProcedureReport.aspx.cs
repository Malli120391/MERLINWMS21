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
    public partial class ReworkProcedureReport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ReportCommon.LoadDropDownfromXML(ddlPrinters, "PrinterIPSettings.xml");
        }

        protected void lnkGetReport_Click(object sender, EventArgs e)
        {
            if (txtkitCode.Text != "" && txtJobOrderRefNo.Text != "" && txtNCRefNo.Text != "")
            {
                rvReworkProcedureReport.Visible = true;
                rvReworkProcedureReport.ServerReport.ReportServerUrl = new Uri(CommonLogic.Application("ReportServerUrl"));
                rvReworkProcedureReport.ServerReport.ReportPath = CommonLogic.Application("ReportPath") + "ReworkProcedureReport";
                rvReworkProcedureReport.ShowParameterPrompts = false;
                rvReworkProcedureReport.ShowPrintButton = true;

                Microsoft.Reporting.WebForms.ReportParameter[] reportParameterCollection = new Microsoft.Reporting.WebForms.ReportParameter[2];


                reportParameterCollection[0] = new Microsoft.Reporting.WebForms.ReportParameter("NCRefNo", (hifNCRefNo.Value != "" ? hifNCRefNo.Value : null));
                reportParameterCollection[1] = new Microsoft.Reporting.WebForms.ReportParameter("ProductionOrderHeaderID", hifJobRefNoNumber.Value);
                rvReworkProcedureReport.ServerReport.SetParameters(reportParameterCollection);
                rvReworkProcedureReport.ServerReport.Refresh();
            }
            else
            {
                resetError("Select Kit Code, Job Order Ref. No. & NC Ref. No.", true);
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