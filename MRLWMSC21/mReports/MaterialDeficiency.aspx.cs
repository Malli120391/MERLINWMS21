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
    public partial class MaterialDeficiency : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ReportCommon.LoadDropDownfromXML(ddlPrinters, "PrinterIPSettings.xml");

            rvMaterialDeficiency.Visible = true;
            rvMaterialDeficiency.ServerReport.ReportServerUrl = new Uri(CommonLogic.Application("ReportServerUrl"));
            rvMaterialDeficiency.ServerReport.ReportPath = CommonLogic.Application("ReportPath") + "MaterialDeficiency";
            rvMaterialDeficiency.ShowParameterPrompts = false;
            rvMaterialDeficiency.ShowPrintButton = true;
        }

        protected void lnkGetReport_Click(object sender, EventArgs e)
        {
                rvMaterialDeficiency.Visible = true;
                rvMaterialDeficiency.ServerReport.ReportServerUrl = new Uri(CommonLogic.Application("ReportServerUrl"));
                rvMaterialDeficiency.ServerReport.ReportPath = CommonLogic.Application("ReportPath") + "MaterialDeficiency";
                rvMaterialDeficiency.ShowParameterPrompts = false;
                rvMaterialDeficiency.ShowPrintButton = true;

                Microsoft.Reporting.WebForms.ReportParameter[] reportParameterCollection = new Microsoft.Reporting.WebForms.ReportParameter[1];

                reportParameterCollection[0] = new Microsoft.Reporting.WebForms.ReportParameter("ProductionOrderHeaderID", (hifJobRefNoNumber.Value != "" && txtJobOrderRefNo.Text != "" ? hifJobRefNoNumber.Value : null));


                rvMaterialDeficiency.ServerReport.SetParameters(reportParameterCollection);
                rvMaterialDeficiency.ServerReport.Refresh();
        }        

        protected void lnkPrint_Click(object sender, EventArgs e)
        {

        }


    }
}