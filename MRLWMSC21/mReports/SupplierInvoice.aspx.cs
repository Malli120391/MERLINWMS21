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
    public partial class SupplierInvoice : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            ReportCommon.LoadDropDownfromXML(ddlPrinters, "PrinterIPSettings.xml");

            /*
            if (!IsPostBack)
            {
                 
                ReportCommon.LoadDropDownfromXML(ddlPrinters, "PrinterIPSettings.xml");




                rvSUPINVReport.ServerReport.ReportServerUrl = new Uri(CommonLogic.Application("ReportServerUrl"));
                rvSUPINVReport.ServerReport.ReportPath = CommonLogic.Application("ReportPath") + "SupplierInvoice";

                rvSUPINVReport.ShowParameterPrompts = false;
                rvSUPINVReport.ShowPrintButton = true;


                rvSUPINVReport.ServerReport.Refresh();
            } */
        }

        protected void lnkGetReport_Click(object sender, EventArgs e)
        {
            rvSUPINVReport.ServerReport.ReportServerUrl = new Uri(CommonLogic.Application("ReportServerUrl")); 
            rvSUPINVReport.ServerReport.ReportPath = CommonLogic.Application("ReportPath") + "SupplierInvoice";
            rvSUPINVReport.ShowParameterPrompts = false;
            rvSUPINVReport.ShowPrintButton = true;

            // Below code demonstrate the Parameter passing method. User only if you have parameters into the reports.
            Microsoft.Reporting.WebForms.ReportParameter[] reportParameterCollection = new Microsoft.Reporting.WebForms.ReportParameter[2];

            string _fromdate = DB.GetSqlS("select min(InvoiceDate) from ORD_SupplierInvoice");
            if (_fromdate == "")
                _fromdate = "01/01/2000";

            reportParameterCollection[0] = new Microsoft.Reporting.WebForms.ReportParameter("FromDate", (txtFromDate.Text == "") ? _fromdate : ReportCommon.FormatDate(txtFromDate.Text, '/'));
            reportParameterCollection[1] = new Microsoft.Reporting.WebForms.ReportParameter("ToDate", (txtToDate.Text == "") ? string.Format("{0:MM/dd/yyyy}", DateTime.Now) : ReportCommon.FormatDate(txtToDate.Text, '/'));
          


            // reportParameterCollection[0] = new Microsoft.Reporting.WebForms.ReportParameter("FromDate", ReportCommon.FormatDate(txtFromDate.Text, '/'));
            // reportParameterCollection[1] = new Microsoft.Reporting.WebForms.ReportParameter("ToDate", ReportCommon.FormatDate(txtToDate.Text, '/'));
           

            rvSUPINVReport.ServerReport.SetParameters(reportParameterCollection);
            rvSUPINVReport.ServerReport.Refresh();
        }

        protected void lnkPrint_Click(object sender, EventArgs e)
        {

        }



    }
}