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
    public partial class ActiveStockReport : System.Web.UI.Page
    {
       protected CustomPrincipal cp = HttpContext.Current.User as CustomPrincipal;

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void lnkGetReport_Click(object sender, EventArgs e)
        {            

            rvMaterialTrackingReport.Visible = true;                
            rvMaterialTrackingReport.ServerReport.ReportServerUrl = new Uri(CommonLogic.Application("ReportServerUrl"));
            rvMaterialTrackingReport.ServerReport.ReportPath = CommonLogic.Application("ReportPath") + "ActiveStockReport_New";
            rvMaterialTrackingReport.ShowParameterPrompts = false;
            rvMaterialTrackingReport.ShowPrintButton = true;
                

            Microsoft.Reporting.WebForms.ReportParameter[] reportParameterCollection = new Microsoft.Reporting.WebForms.ReportParameter[3];

            //reportParameterCollection[0] = new Microsoft.Reporting.WebForms.ReportParameter("SerialNo", txtSerialNo.Text != "" ? txtSerialNo.Text : null);
            //reportParameterCollection[1] = new Microsoft.Reporting.WebForms.ReportParameter("BatchNo", txtBatchNo.Text != "" ? txtBatchNo.Text : null);
            //reportParameterCollection[2] = new Microsoft.Reporting.WebForms.ReportParameter("MCode", txtMCode.Text != "" ? txtMCode.Text : null);
            //reportParameterCollection[3] = new Microsoft.Reporting.WebForms.ReportParameter("Location", txtLocation.Text != "" ? txtLocation.Text : null);

            reportParameterCollection[0] = new Microsoft.Reporting.WebForms.ReportParameter("MaterialMasterID", (hifMCode.Value != "" && txtMCode.Text != "" ? hifMCode.Value : null));
            reportParameterCollection[1] = new Microsoft.Reporting.WebForms.ReportParameter("LocationID", (hifLocation.Value != "" && txtLocation.Text != "" ? hifLocation.Value : null));
            reportParameterCollection[2] = new Microsoft.Reporting.WebForms.ReportParameter("MTypeID", (hifMType.Value != "" && txtMType.Text != "" ? hifMType.Value : null));
            

            rvMaterialTrackingReport.ServerReport.SetParameters(reportParameterCollection);                
            rvMaterialTrackingReport.ServerReport.Refresh();
            
        }


        protected void lnkPrint_Click(object sender, EventArgs e)
        {

        }
    }
}