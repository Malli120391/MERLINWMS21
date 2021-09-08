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
    public partial class WarehouseOperatorActivity : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //ReportCommon.LoadDropDownfromXML(ddlPrinters, "PrinterIPSettings.xml");
        }

         protected void lnkGetReport_Click(object sender, EventArgs e)
        {
            rvWarehouseOperatorActivity.Visible = true;
            rvWarehouseOperatorActivity.ServerReport.ReportServerUrl = new Uri(CommonLogic.Application("ReportServerUrl"));
            rvWarehouseOperatorActivity.ServerReport.ReportPath = CommonLogic.Application("ReportPath") + "WarehouseOperatorActivity";
            rvWarehouseOperatorActivity.ShowParameterPrompts = false;
            rvWarehouseOperatorActivity.ShowPrintButton = true;

            List<ReportParameter> _reportParameterCollection = new List<ReportParameter>();

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

            rvWarehouseOperatorActivity.ServerReport.SetParameters(_reportParameterCollection);
            rvWarehouseOperatorActivity.ServerReport.Refresh();

        }

        protected void lnkPrint_Click(object sender, EventArgs e)
        {

        }


    }
}
    
