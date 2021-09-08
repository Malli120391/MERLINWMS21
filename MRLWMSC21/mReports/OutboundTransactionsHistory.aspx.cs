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
    public partial class OutboundTransactionsHistory : System.Web.UI.Page
    {
        protected CustomPrincipal cp = HttpContext.Current.User as CustomPrincipal;

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void lnkGetReport_Click(object sender, EventArgs e)
        {
            
                rvOutboundTransactionsHistory.Visible = true;
                rvOutboundTransactionsHistory.ServerReport.ReportServerUrl = new Uri(CommonLogic.Application("ReportServerUrl"));
                rvOutboundTransactionsHistory.ServerReport.ReportPath = CommonLogic.Application("ReportPath") + "OutboundTransactionsHistory";
                rvOutboundTransactionsHistory.ShowParameterPrompts = false;
                rvOutboundTransactionsHistory.ShowPrintButton = true;

                //Microsoft.Reporting.WebForms.ReportParameter[] reportParameterCollection = new Microsoft.Reporting.WebForms.ReportParameter[1];
                //reportParameterCollection[0] = new Microsoft.Reporting.WebForms.ReportParameter("TransactionYear", txtYear.Text != "" ? txtYear.Text : null);

                //rvOutboundTransactionsHistory.ServerReport.SetParameters(reportParameterCollection);
                rvOutboundTransactionsHistory.ServerReport.Refresh();            

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