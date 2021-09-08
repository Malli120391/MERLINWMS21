using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MRLWMSC21Common;
using System.Globalization;
using Microsoft.Reporting.WebForms;

namespace MRLWMSC21.mReports
{
    public partial class NonConfirmityReport : System.Web.UI.Page
    {
        public CustomPrincipal cp = HttpContext.Current.User as CustomPrincipal;

        protected void Page_Load(object sender, EventArgs e)
        {
            
            ReportCommon.LoadDropDownfromXML(ddlPrinters, "PrinterIPSettings.xml");
            rvNCReport.ServerReport.ReportServerUrl = new Uri(CommonLogic.Application("ReportServerUrl"));
            rvNCReport.ServerReport.ReportPath = CommonLogic.Application("ReportPath") + "InwardNCReport";
            rvNCReport.ShowParameterPrompts = false;
            rvNCReport.ShowPrintButton = true;

        }

        protected void lnkGetReport_Click(object sender, EventArgs e)
        {
            if (txtPONumber.Text != "")
            {
                rvNCReport.Visible = true;
                rvNCReport.ServerReport.ReportServerUrl = new Uri(CommonLogic.Application("ReportServerUrl"));
                rvNCReport.ServerReport.ReportPath = CommonLogic.Application("ReportPath") + "InwardNCReport";
                rvNCReport.ShowParameterPrompts = false;
                rvNCReport.ShowPrintButton = true;

                // Below code demonstrate the Parameter passing method. User only if you have parameters into the reports.
                Microsoft.Reporting.WebForms.ReportParameter[] reportParameterCollection = new Microsoft.Reporting.WebForms.ReportParameter[1];


                // reportParameterCollection[0] = new Microsoft.Reporting.WebForms.ReportParameter("PONumber", (txtPONumber.Text.Trim() != "" ? txtPONumber.Text.Trim() : "NULL"));

                reportParameterCollection[0] = new Microsoft.Reporting.WebForms.ReportParameter("POHeaderID", (hifPONumber.Value != "" && txtPONumber.Text != "" ? hifPONumber.Value : "NULL"));



                //ReportViewer1.ServerReport.SetParameters(myParams);*/

                rvNCReport.ServerReport.SetParameters(reportParameterCollection);
                rvNCReport.ServerReport.Refresh();
            }
            else
            { 
                resetError("Select PO Number", true);
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