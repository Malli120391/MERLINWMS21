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
    public partial class SupplierReturns : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            ReportCommon.LoadDropDownfromXML(ddlPrinters, "PrinterIPSettings.xml");

          /*  if (!IsPostBack)
            {

                ReportCommon.LoadDropDownfromXML(ddlPrinters, "PrinterIPSettings.xml");


                

                rvSUPReturnReport.ServerReport.ReportServerUrl = new Uri(CommonLogic.Application("ReportServerUrl"));
                rvSUPReturnReport.ServerReport.ReportPath = CommonLogic.Application("ReportPath") + "SupplierReturns";

                rvSUPReturnReport.ShowParameterPrompts = false;
                rvSUPReturnReport.ShowPrintButton = true;


                rvSUPReturnReport.ServerReport.Refresh();
            } */
        }

        protected void lnkGetReport_Click(object sender, EventArgs e)
        {
            rvSUPReturnReport.Visible = true;
            rvSUPReturnReport.ServerReport.ReportServerUrl = new Uri(CommonLogic.Application("ReportServerUrl"));
            rvSUPReturnReport.ServerReport.ReportPath = CommonLogic.Application("ReportPath") + "SupplierReturns";
            rvSUPReturnReport.ShowParameterPrompts = false;
            rvSUPReturnReport.ShowPrintButton = true;

            // Below code demonstrate the Parameter passing method. User only if you have parameters into the reports.
            /*
            Microsoft.Reporting.WebForms.ReportParameter[] reportParameterCollection = new Microsoft.Reporting.WebForms.ReportParameter[2];

            string _fromdate = DB.GetSqlS("select min(SODate) from ORD_SOHeader");
            if (_fromdate == "")
                _fromdate = "01/01/2000";

            reportParameterCollection[0] = new Microsoft.Reporting.WebForms.ReportParameter("FromDate", (txtFromDate.Text == "") ? _fromdate : ReportCommon.FormatDate(txtFromDate.Text, '/'));
            reportParameterCollection[1] = new Microsoft.Reporting.WebForms.ReportParameter("ToDate", (txtToDate.Text == "") ? string.Format("{0:MM/dd/yyyy}", DateTime.Now) : ReportCommon.FormatDate(txtToDate.Text, '/'));
            

            // reportParameterCollection[0] = new Microsoft.Reporting.WebForms.ReportParameter("FromDate", ReportCommon.FormatDate(txtFromDate.Text, '/'));
            // reportParameterCollection[1] = new Microsoft.Reporting.WebForms.ReportParameter("ToDate", ReportCommon.FormatDate(txtToDate.Text, '/'));
            */

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

            rvSUPReturnReport.ServerReport.SetParameters(_reportParameterCollection);
            rvSUPReturnReport.ServerReport.Refresh();
        }

        protected void lnkPrint_Click(object sender, EventArgs e)
        {

        }



    }
}