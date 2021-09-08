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
    public partial class JobOrder : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ReportCommon.LoadDropDownfromXML(ddlPrinters, "PrinterIPSettings.xml");
           /* if (IsPostBack)
            {



                rvJobOrder.ServerReport.ReportServerUrl = new Uri(CommonLogic.Application("ReportServerUrl"));
                rvJobOrder.ServerReport.ReportPath = CommonLogic.Application("ReportPath") + "GITReport";

                rvJobOrder.ShowParameterPrompts = false;
                rvJobOrder.ShowPrintButton = true;


                rvJobOrder.ServerReport.Refresh();
            }  */
        }

        protected void lnkGetReport_Click(object sender, EventArgs e)
        {
            rvJobOrder.ServerReport.ReportServerUrl = new Uri(CommonLogic.Application("ReportServerUrl"));
            rvJobOrder.ServerReport.ReportPath = CommonLogic.Application("ReportPath") + "JobOrder";
            rvJobOrder.ShowParameterPrompts = false;
            rvJobOrder.ShowPrintButton = true;

            // Below code demonstrate the Parameter passing method. User only if you have parameters into the reports.
            Microsoft.Reporting.WebForms.ReportParameter[] reportParameterCollection = new Microsoft.Reporting.WebForms.ReportParameter[2];


            String _fromdate = DB.GetSqlS("select min(InvoiceDate) from ORD_SupplierInvoice");
            if (_fromdate == "")
                _fromdate = "01/01/2000";
            reportParameterCollection[0] = new Microsoft.Reporting.WebForms.ReportParameter("FromDate", (txtFromDate.Text == "") ? _fromdate : ReportCommon.FormatDate(txtFromDate.Text, '/'));
            reportParameterCollection[1] = new Microsoft.Reporting.WebForms.ReportParameter("Todate", (txtToDate.Text == "") ? string.Format("{0:MM/dd/yyyy}", DateTime.Now) : ReportCommon.FormatDate(txtToDate.Text, '/'));

            /* List<ReportParameter> reportParameterCollection = new List<ReportParameter>();

             ReportParameter p1 = new ReportParameter("FromDate");

             if(txtFromDate.Text == "") 
                 p1.Values.Add(null);
             else
                 p1.Values.Add(txtFromDate.Text);
            
             reportParameterCollection.Add(p1);

             ReportParameter p2 = new ReportParameter("ToDate");
            
             if(txtToDate.Text == "")
                 p2.Values.Add(null);
             else
                 p2.Values.Add(txtToDate.Text);
             reportParameterCollection.Add(p2);
            

             //ReportViewer1.ServerReport.SetParameters(myParams);*/

            rvJobOrder.ServerReport.SetParameters(reportParameterCollection);
            rvJobOrder.ServerReport.Refresh();
        }

        protected void lnkPrint_Click(object sender, EventArgs e)
        {

        }



    }
}