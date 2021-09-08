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
    public partial class OperatorPerformance : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ReportCommon.LoadDropDownfromXML(ddlPrinters, "PrinterIPSettings.xml");
        }

        protected void lnkGetReport_Click(object sender, EventArgs e)
        {

            rvOperatorPerformanceReport.Visible = true;
            rvOperatorPerformanceReport.ServerReport.ReportServerUrl = new Uri(CommonLogic.Application("ReportServerUrl"));
            rvOperatorPerformanceReport.ServerReport.ReportPath = CommonLogic.Application("ReportPath") + "OperatorPerformanceReport";
            rvOperatorPerformanceReport.ShowParameterPrompts = false;
            rvOperatorPerformanceReport.ShowPrintButton = true;

            Microsoft.Reporting.WebForms.ReportParameter[] reportParameterCollection = new Microsoft.Reporting.WebForms.ReportParameter[2];

            reportParameterCollection[0] = new Microsoft.Reporting.WebForms.ReportParameter("UserID", (hifOperator.Value != "" && txtOperator.Text != "" ? hifOperator.Value : "0"));
            reportParameterCollection[1] = new Microsoft.Reporting.WebForms.ReportParameter("UserRoleID", (hifRole.Value != "" && txtRole.Text != "" ? hifRole.Value : "0"));

            rvOperatorPerformanceReport.ServerReport.SetParameters(reportParameterCollection);
            rvOperatorPerformanceReport.ServerReport.Refresh();
        }

        protected void lnkPrint_Click(object sender, EventArgs e)
        {

        }


    }
}