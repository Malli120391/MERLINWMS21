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
    public partial class InwardInspectonReport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ReportCommon.LoadDropDownfromXML(ddlPrinters, "PrinterIPSettings.xml");
        }

        protected void lnkGetReport_Click(object sender, EventArgs e)
        {
            if (txtStoreRefNo.Text != "")
            {
                rvInwardInspectionReport.Visible = true;
                rvInwardInspectionReport.ServerReport.ReportServerUrl = new Uri(CommonLogic.Application("ReportServerUrl"));
                rvInwardInspectionReport.ServerReport.ReportPath = CommonLogic.Application("ReportPath") + "InwardInspectionReport2";
                rvInwardInspectionReport.ShowParameterPrompts = false;
                rvInwardInspectionReport.ShowPrintButton = true;
                
                Microsoft.Reporting.WebForms.ReportParameter[] reportParameterCollection = new Microsoft.Reporting.WebForms.ReportParameter[1];

                reportParameterCollection[0] = new Microsoft.Reporting.WebForms.ReportParameter("InboundID", hifStoreRefNo.Value.Trim() != "" ? hifStoreRefNo.Value.Trim() : null);

                
                rvInwardInspectionReport.ServerReport.SetParameters(reportParameterCollection);
                rvInwardInspectionReport.ServerReport.Refresh();
            }
            else
            {
                resetError("Select Inspection Report Number", true);
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