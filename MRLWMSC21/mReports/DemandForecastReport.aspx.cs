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
    public partial class DemandForecastReport : System.Web.UI.Page
    {
        protected CustomPrincipal cp = HttpContext.Current.User as CustomPrincipal;

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void lnkGetReport_Click(object sender, EventArgs e)
        {
            
                rvDemandForecastReport.Visible = true;
                rvDemandForecastReport.ServerReport.ReportServerUrl = new Uri(CommonLogic.Application("ReportServerUrl"));
                rvDemandForecastReport.ServerReport.ReportPath = CommonLogic.Application("ReportPath") + "DemandForecastReport";
                rvDemandForecastReport.ShowParameterPrompts = false;
                rvDemandForecastReport.ShowPrintButton = true;
               
                rvDemandForecastReport.ServerReport.Refresh();           

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