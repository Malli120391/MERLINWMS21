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
    public partial class MaterialTrackingReport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        
        protected void lnkGetReport_Click(object sender, EventArgs e)
        {


            if (txtSerialNo.Text != "" || txtBatchNo.Text != "")
            {
                              

                rvMaterialTrackingReport.Visible = true;
                rvMaterialTrackingReport.Visible = true;
                rvMaterialTrackingReport.ServerReport.ReportServerUrl = new Uri(CommonLogic.Application("ReportServerUrl"));
                rvMaterialTrackingReport.ServerReport.ReportPath = CommonLogic.Application("ReportPath") + "MaterialTrackingReport";
                rvMaterialTrackingReport.ShowParameterPrompts = false;
                rvMaterialTrackingReport.ShowPrintButton = true;

                Microsoft.Reporting.WebForms.ReportParameter[] reportParameterCollection = new Microsoft.Reporting.WebForms.ReportParameter[2];

                reportParameterCollection[0] = new Microsoft.Reporting.WebForms.ReportParameter("SerialNumber", txtSerialNo.Text != "" ? txtSerialNo.Text : null);
                reportParameterCollection[1] = new Microsoft.Reporting.WebForms.ReportParameter("BatchNumber", txtBatchNo.Text != "" ? txtBatchNo.Text : null);

                rvMaterialTrackingReport.ServerReport.SetParameters(reportParameterCollection);
                rvMaterialTrackingReport.ServerReport.Refresh();
            }
            else
            {
                resetError("Enter Serial No. or Batch No.", true);
            }
        }

        private void resetError(string error, bool isError)
        {

            ScriptManager.RegisterStartupScript(this, this.GetType(), "test", "showStickyToast(" + (isError.ToString().ToLower() == "true" ? "false" : "true") + ",\"" + error + "\");", true);

        }

        protected void lnkPrint_Click(object sender, EventArgs e)
        { 

        }
    }
}