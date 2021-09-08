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
    public partial class ConsolidatedWorkInProcessReport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ReportCommon.LoadDropDownfromXML(ddlPrinters, "PrinterIPSettings.xml");
        }
        protected void lnkGetReport_Click(object sender, EventArgs e)
        {
            
                rvConsolidatedWorkInProcessReport.Visible = true;
                rvConsolidatedWorkInProcessReport.ServerReport.ReportServerUrl = new Uri(CommonLogic.Application("ReportServerUrl"));
                rvConsolidatedWorkInProcessReport.ServerReport.ReportPath = CommonLogic.Application("ReportPath") + "ConsolidatedWorkInProcessReport";
                rvConsolidatedWorkInProcessReport.ShowParameterPrompts = false;
                rvConsolidatedWorkInProcessReport.ShowPrintButton = true;

                // Microsoft.Reporting.WebForms.ReportParameter[] reportParameterCollection = new Microsoft.Reporting.WebForms.ReportParameter[1];

                // reportParameterCollection[0] = new Microsoft.Reporting.WebForms.ReportParameter("ProductionOrderHeaderID", (hifJobRefNoNumber.Value != "" && txtJobOrderRefNo.Text != "" ? hifJobRefNoNumber.Value : "NULL"));

                // rvConsolidatedWorkInProcessReport.ServerReport.SetParameters(reportParameterCollection);
                rvConsolidatedWorkInProcessReport.ServerReport.Refresh();
            
        }          

        protected void lnkPrint_Click(object sender, EventArgs e)
        {

        }



    }
}