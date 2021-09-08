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
    public partial class MaterialReplenishment : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ReportCommon.LoadDropDownfromXML(ddlPrinters, "PrinterIPSettings.xml");

            rvReplenishment.Visible = true;
            rvReplenishment.ServerReport.ReportServerUrl = new Uri(CommonLogic.Application("ReportServerUrl"));
            rvReplenishment.ServerReport.ReportPath = CommonLogic.Application("ReportPath") + "MaterialReplenishment";
            rvReplenishment.ShowParameterPrompts = false;
            rvReplenishment.ShowPrintButton = true;
       
        }

        protected void lnkGetReport_Click(object sender, EventArgs e)
        {
            rvReplenishment.Visible = true;
            rvReplenishment.ServerReport.ReportServerUrl = new Uri(CommonLogic.Application("ReportServerUrl"));
            rvReplenishment.ServerReport.ReportPath = CommonLogic.Application("ReportPath") + "MaterialReplenishment";
            rvReplenishment.ShowParameterPrompts = false;
            rvReplenishment.ShowPrintButton = true;

            // Below code demonstrate the Parameter passing method. User only if you have parameters into the reports.
            Microsoft.Reporting.WebForms.ReportParameter[] reportParameterCollection = new Microsoft.Reporting.WebForms.ReportParameter[1];

            reportParameterCollection[0] = new Microsoft.Reporting.WebForms.ReportParameter("MCode", txtMCode.Text != "" && hifMCode.Value!="" ? txtMCode.Text : null);
                                           

             //ReportViewer1.ServerReport.SetParameters(myParams);*/

            rvReplenishment.ServerReport.SetParameters(reportParameterCollection);
            rvReplenishment.ServerReport.Refresh();
        }

        protected void lnkPrint_Click(object sender, EventArgs e)
        {

        }
    }
}