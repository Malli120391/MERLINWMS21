using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MRLWMSC21Common;
using System.Globalization;
using Microsoft.Reporting.WebForms;

namespace MRLWMSC21.mReports
{
    public partial class PurchageOrderList : System.Web.UI.Page
    {
        public CustomPrincipal cp = HttpContext.Current.User as CustomPrincipal;

        protected void Page_Load(object sender, EventArgs e)
        {
            ReportCommon.LoadDropDownfromXML(ddlPrinters, "PrinterIPSettings.xml");            
           
        }

        protected void lnkGetReport_Click(object sender, EventArgs e)
        {
            rvMRReport.Visible = true;
            rvMRReport.ServerReport.ReportServerUrl = new Uri(CommonLogic.Application("ReportServerUrl"));
            rvMRReport.ServerReport.ReportPath = CommonLogic.Application("ReportPath") + "PurchaseOrder";
            rvMRReport.ShowParameterPrompts = false;
            rvMRReport.ShowPrintButton = true;

            // Below code demonstrate the Parameter passing method. User only if you have parameters into the reports.
            /*Microsoft.Reporting.WebForms.ReportParameter[] reportParameterCollection = new Microsoft.Reporting.WebForms.ReportParameter[7];
            
            String PODate;
            if (txtPODate.Text.Trim() != "")
                PODate = DB.SQuote(CommonLogic.GetConfigValue(cp.TenantID, "DateFormat") == "dd/MM/yyyy" ? DateTime.ParseExact(txtPODate.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy") : txtPODate.Text.Trim());
            else
                PODate = "NULL";

            reportParameterCollection[0] = new Microsoft.Reporting.WebForms.ReportParameter("PONumber", (hifPONumber.Value != "" && atcPONumber.Text != "" ? hifPONumber.Value : "NULL"));
            reportParameterCollection[1] = new Microsoft.Reporting.WebForms.ReportParameter("PODate", (txtPODate.Text.ToString()!="") ? ReportCommon.FormatDate(txtPODate.Text, '/'):"NULL");
            reportParameterCollection[2] = new Microsoft.Reporting.WebForms.ReportParameter("StatusID", (atcPOStatus.Text != "" && hifPOStatus.Value != "" ? hifPOStatus.Value : "NULL"));
            reportParameterCollection[3] = new Microsoft.Reporting.WebForms.ReportParameter("POTypeID", (atcPOType.Text != "" && hifPOType.Value != "" ? hifPOType.Value : "NULL"));
            reportParameterCollection[4] = new Microsoft.Reporting.WebForms.ReportParameter("SupplierID", (atcSupplier.Text != "" && hifSupplier.Value != "" ? hifSupplier.Value : "NULL"));
            reportParameterCollection[5] = new Microsoft.Reporting.WebForms.ReportParameter("FromDate", (txtFromDate.Text.ToString()!="") ? ReportCommon.FormatDate(txtFromDate.Text, '/') : "NULL");
            reportParameterCollection[6] = new Microsoft.Reporting.WebForms.ReportParameter("ToDate", (txtToDate.Text != "") ? ReportCommon.FormatDate(txtToDate.Text, '/') : "04/10/2014");
            */
            List<ReportParameter> _reportParameterCollection = new List<ReportParameter>();

            ReportParameter paramPONumber = new ReportParameter("PONumber");
            if (hifPONumber.Value != "" && atcPONumber.Text != "")
                paramPONumber.Values.Add(atcPONumber.Text);
            else
                paramPONumber.Values.Add(null);
            _reportParameterCollection.Add(paramPONumber);

            ReportParameter paramPODate = new ReportParameter("PODate");
            if (txtPODate.Text.ToString() != "")
                paramPODate.Values.Add(ReportCommon.FormatDate(txtPODate.Text, '/'));
            else
                paramPODate.Values.Add(null);
            _reportParameterCollection.Add(paramPODate);

            ReportParameter paramStatusID = new ReportParameter("StatusID");
            if (atcPOStatus.Text != "" && hifPOStatus.Value != "")
                paramStatusID.Values.Add(hifPOStatus.Value);
            else
                paramStatusID.Values.Add(null);
            _reportParameterCollection.Add(paramStatusID);

            ReportParameter paramPOTypeID = new ReportParameter("POTypeID");
            if (atcPOType.Text != "" && hifPOType.Value != "" )
                paramPOTypeID.Values.Add(hifPOType.Value);
            else
                paramPOTypeID.Values.Add(null);
            _reportParameterCollection.Add(paramPOTypeID);

            ReportParameter paramSupplierID = new ReportParameter("SupplierID");
            if (atcSupplier.Text != "" && hifSupplier.Value != "")
                paramSupplierID.Values.Add(hifSupplier.Value);
            else
                paramSupplierID.Values.Add(null);
            _reportParameterCollection.Add(paramSupplierID);

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

            rvMRReport.ServerReport.SetParameters(_reportParameterCollection);
            rvMRReport.ServerReport.Refresh();
        }

        protected void lnkPrint_Click(object sender, EventArgs e)
        {

        }


    }
}