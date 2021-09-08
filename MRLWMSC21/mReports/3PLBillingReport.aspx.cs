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
    public partial class _3PLStorageBillingReport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ReportCommon.LoadDropDownfromXML(ddlPrinters, "PrinterIPSettings.xml");
        }

        protected void lnkGetReport_Click(object sender, EventArgs e)
        {
            if(txtTenant.Text!="" && txtFromDate.Text!="" && txtToDate.Text!="")
            {
                rv3BillingReport.Visible = true;
                rv3BillingReport.ServerReport.ReportServerUrl = new Uri(CommonLogic.Application("ReportServerUrl"));
                rv3BillingReport.ServerReport.ReportPath = CommonLogic.Application("ReportPath") + "3PL_BillingReport";
                rv3BillingReport.ShowParameterPrompts = false;
                rv3BillingReport.ShowPrintButton = true;

           List<ReportParameter> _reportParameterCollection = new List<ReportParameter>();

                ReportParameter paramTenantID = new ReportParameter("TenantID");
                if (hifTenant.Value != "" && txtTenant.Text != "")
                    paramTenantID.Values.Add(hifTenant.Value);
                else
                    paramTenantID.Values.Add("NULL");
                _reportParameterCollection.Add(paramTenantID);


                ReportParameter paramFromDate = new ReportParameter("StartDate");
                if (txtFromDate.Text.ToString() != "")
                    paramFromDate.Values.Add(ReportCommon.FormatDate(txtFromDate.Text, '/'));
                else
                    paramFromDate.Values.Add(null);
                _reportParameterCollection.Add(paramFromDate);

                ReportParameter paramToDate = new ReportParameter("EndDate");
                if (txtToDate.Text.ToString() != "")
                    paramToDate.Values.Add(ReportCommon.FormatDate(txtToDate.Text, '/'));
                else
                    paramToDate.Values.Add(null);
                _reportParameterCollection.Add(paramToDate);

                rv3BillingReport.ServerReport.SetParameters(_reportParameterCollection);
                rv3BillingReport.ServerReport.Refresh();
            }
            else
            {
                resetError("Select Tenant, From Date and To Date", true);
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