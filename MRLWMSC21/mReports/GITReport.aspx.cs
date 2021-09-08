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
    public partial class GITReport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ReportCommon.LoadDropDownfromXML(ddlPrinters, "PrinterIPSettings.xml");

            /*if (IsPostBack)
            {             

               rvGITReport.ServerReport.ReportServerUrl = new Uri(CommonLogic.Application("ReportServerUrl"));
                rvGITReport.ServerReport.ReportPath = CommonLogic.Application("ReportPath") + "GITReport";

                rvGITReport.ShowParameterPrompts = false;
                rvGITReport.ShowPrintButton = true;


                rvGITReport.ServerReport.Refresh();
            }*/
        }

        protected void lnkGetReport_Click(object sender, EventArgs e)
        {
            rvGITReport.Visible = true;
            rvGITReport.ServerReport.ReportServerUrl = new Uri(CommonLogic.Application("ReportServerUrl"));
            rvGITReport.ServerReport.ReportPath = CommonLogic.Application("ReportPath") + "GITReport";
            rvGITReport.ShowParameterPrompts = false;
            rvGITReport.ShowPrintButton = true;            

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
           

            rvGITReport.ServerReport.SetParameters(_reportParameterCollection);
            rvGITReport.ServerReport.Refresh();

        }

        protected void lnkPrint_Click(object sender, EventArgs e)
        {

        }

    }
}