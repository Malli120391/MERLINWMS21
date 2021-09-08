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
    public partial class SupplierPerformance : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ReportCommon.LoadDropDownfromXML(ddlPrinters, "PrinterIPSettings.xml");
        }
        protected void lnkGetReport_Click(object sender, EventArgs e)
        {
            rvSUPPerformanceReport.Visible = true;
            rvSUPPerformanceReport.ServerReport.ReportServerUrl = new Uri(CommonLogic.Application("ReportServerUrl"));
            rvSUPPerformanceReport.ServerReport.ReportPath = CommonLogic.Application("ReportPath") + "SupplierPerformanceReport";
            rvSUPPerformanceReport.ShowParameterPrompts = false;
            rvSUPPerformanceReport.ShowPrintButton = true;

            Microsoft.Reporting.WebForms.ReportParameter[] reportParameterCollection = new Microsoft.Reporting.WebForms.ReportParameter[2];
            reportParameterCollection[0] = new Microsoft.Reporting.WebForms.ReportParameter("SupplierID", txtSupplier.Text != "" & hifSupplier.Value != "" ? hifSupplier.Value : "0");
            reportParameterCollection[1] = new Microsoft.Reporting.WebForms.ReportParameter("MaterialMasterID", txtMCode.Text != "" & hifMCode.Value != "" ? hifMCode.Value : "0");
            

            // Below code demonstrate the Parameter passing method. User only if you have parameters into the reports.      
            //reportParameterCollection[0] = new Microsoft.Reporting.WebForms.ReportParameter("MCode", txtMCode.Text.Trim() != "" ? txtMCode.Text.Trim() : null);
            //ReportViewer1.ServerReport.SetParameters(myParams);*/

            rvSUPPerformanceReport.ServerReport.SetParameters(reportParameterCollection);
            rvSUPPerformanceReport.ServerReport.Refresh();
        }

        protected void lnkPrint_Click(object sender, EventArgs e)
        {

        }
    }
}