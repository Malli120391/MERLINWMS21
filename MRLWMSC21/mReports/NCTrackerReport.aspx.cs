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
    public partial class NCTrackerReport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ReportCommon.LoadDropDownfromXML(ddlPrinters, "PrinterIPSettings.xml");
        }

        protected void lnkGetReport_Click(object sender, EventArgs e)
        {
            rvNCTrackerReport.Visible = true;
            rvNCTrackerReport.ServerReport.ReportServerUrl = new Uri(CommonLogic.Application("ReportServerUrl"));
            rvNCTrackerReport.ServerReport.ReportPath = CommonLogic.Application("ReportPath") + "NCTrackerReport";
            rvNCTrackerReport.ShowParameterPrompts = false;
            rvNCTrackerReport.ShowPrintButton = true;

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


            rvNCTrackerReport.ServerReport.SetParameters(_reportParameterCollection);
            rvNCTrackerReport.ServerReport.Refresh();

        }

        protected void lnkPrint_Click(object sender, EventArgs e)
        {

        }

    }
}