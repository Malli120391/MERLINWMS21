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
    public partial class _3PLRateCard : System.Web.UI.Page
    {
       protected void Page_Load(object sender, EventArgs e)
        {
            ReportCommon.LoadDropDownfromXML(ddlPrinters, "PrinterIPSettings.xml");
            if (!IsPostBack)
            {
                if (CommonLogic.QueryString("tid") != "")
                {
                    rv3PLRateCardReport.Visible = true;
                    rv3PLRateCardReport.ServerReport.ReportServerUrl = new Uri(CommonLogic.Application("ReportServerUrl"));
                    rv3PLRateCardReport.ServerReport.ReportPath = CommonLogic.Application("ReportPath") + "3PL_RateCard";
                    rv3PLRateCardReport.ShowParameterPrompts = false;
                    rv3PLRateCardReport.ShowPrintButton = true;

                    Microsoft.Reporting.WebForms.ReportParameter[] reportParameterCollection = new Microsoft.Reporting.WebForms.ReportParameter[1];

                    reportParameterCollection[0] = new Microsoft.Reporting.WebForms.ReportParameter("TenantID", CommonLogic.QueryString("tid"));


                    rv3PLRateCardReport.ServerReport.SetParameters(reportParameterCollection);
                    rv3PLRateCardReport.ServerReport.Refresh();
                }
                else
                {
                    resetError("Select Tenant", true);
                }
            }
        }
        

        protected void lnkGetReport_Click(object sender, EventArgs e)
        {
            
            if (txtTenant.Text != "")
            {
                rv3PLRateCardReport.Visible = true;
                rv3PLRateCardReport.ServerReport.ReportServerUrl = new Uri(CommonLogic.Application("ReportServerUrl"));
                rv3PLRateCardReport.ServerReport.ReportPath = CommonLogic.Application("ReportPath") + "3PL_RateCard";
                rv3PLRateCardReport.ShowParameterPrompts = false;
                rv3PLRateCardReport.ShowPrintButton = true;

                Microsoft.Reporting.WebForms.ReportParameter[] reportParameterCollection = new Microsoft.Reporting.WebForms.ReportParameter[1];

                reportParameterCollection[0] = new Microsoft.Reporting.WebForms.ReportParameter("TenantID", (hifTenant.Value != "" && txtTenant.Text != "" ? hifTenant.Value : "NUll"));


                rv3PLRateCardReport.ServerReport.SetParameters(reportParameterCollection);
                rv3PLRateCardReport.ServerReport.Refresh();
            }
            else
            {
                resetError("Tenant", true);
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
