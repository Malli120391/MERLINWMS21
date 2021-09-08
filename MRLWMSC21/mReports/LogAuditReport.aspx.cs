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
    public partial class LogAuditReport : System.Web.UI.Page
    {
        public CustomPrincipal cp = HttpContext.Current.User as CustomPrincipal;

        protected void Page_Load(object sender, EventArgs e)
        {
            //ReportCommon.LoadDropDownfromXML(ddlPrinters, "PrinterIPSettings.xml");

        }

        protected void lnkGetReport_Click(object sender, EventArgs e)
        {
            if (txtEmployee.Text != "" || txtFromDate.Text != "" || txtToDate.Text != "")
            {
                rvLogAuditReport.Visible = true;
                rvLogAuditReport.ServerReport.ReportServerUrl = new Uri(CommonLogic.Application("ReportServerUrl"));
                rvLogAuditReport.ServerReport.ReportPath = CommonLogic.Application("ReportPath") + "LogAuditReport";
                rvLogAuditReport.ShowParameterPrompts = false;
                rvLogAuditReport.ShowPrintButton = true;

                List<ReportParameter> _reportParameterCollection = new List<ReportParameter>();


                ReportParameter paramUserID = new ReportParameter("UserID");
                if (txtEmployee.Text != "" && hifEmployee.Value != "")
                    paramUserID.Values.Add(hifEmployee.Value);
                else
                    paramUserID.Values.Add(null);
                _reportParameterCollection.Add(paramUserID);

                ReportParameter paramFromDate = new ReportParameter("FromDate");
                if (txtFromDate.Text.ToString() != "")
                    paramFromDate.Values.Add(ReportCommon.FormatDate(txtFromDate.Text, '/'));
                else
                    paramFromDate.Values.Add(null);
                _reportParameterCollection.Add(paramFromDate);

                ReportParameter paramToDate = new ReportParameter("ToDate");
                if (txtToDate.Text.ToString() != "")
                    paramToDate.Values.Add(ReportCommon.FormatDate(txtToDate.Text, '/'));
                else
                    paramToDate.Values.Add(null);
                _reportParameterCollection.Add(paramToDate);

                rvLogAuditReport.ServerReport.SetParameters(_reportParameterCollection);
                rvLogAuditReport.ServerReport.Refresh();
            }
            else
            {
                resetError("Select Employee or From Date or To Date", true); 
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