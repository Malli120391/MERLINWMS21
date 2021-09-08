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
    public partial class MaterialActivityReport : System.Web.UI.Page
    {
        protected CustomPrincipal cp = HttpContext.Current.User as CustomPrincipal;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                txtMCode.Text = CommonLogic.QueryString("MCode");
                hifMCode.Value = CommonLogic.QueryString("MMID");

                if (txtMCode.Text != "")
                {
                    rvMaterialActivityReport.Visible = true;
                    rvMaterialActivityReport.ServerReport.ReportServerUrl = new Uri(CommonLogic.Application("ReportServerUrl"));
                    rvMaterialActivityReport.ServerReport.ReportPath = CommonLogic.Application("ReportPath") + "MaterialActivityReport";
                    rvMaterialActivityReport.ShowParameterPrompts = false;
                    rvMaterialActivityReport.ShowPrintButton = true;
                    Microsoft.Reporting.WebForms.ReportParameter[] reportParameterCollection = new Microsoft.Reporting.WebForms.ReportParameter[1];
                    reportParameterCollection[0] = new Microsoft.Reporting.WebForms.ReportParameter("MaterialMasterID", (hifMCode.Value != "" && txtMCode.Text != "" ? hifMCode.Value : "NULL"));

                    rvMaterialActivityReport.ServerReport.SetParameters(reportParameterCollection);
                    rvMaterialActivityReport.ServerReport.Refresh();
                }
                else
                {
                    resetError("Please Select Part Number", true);
                }

            }

        }

        protected void lnkGetReport_Click(object sender, EventArgs e)
        {
            if (txtMCode.Text != "")
            {
                rvMaterialActivityReport.Visible = true;
                rvMaterialActivityReport.ServerReport.ReportServerUrl = new Uri(CommonLogic.Application("ReportServerUrl"));
                rvMaterialActivityReport.ServerReport.ReportPath = CommonLogic.Application("ReportPath") + "MaterialActivityReport";
                rvMaterialActivityReport.ShowParameterPrompts = false;
                rvMaterialActivityReport.ShowPrintButton = true;
                Microsoft.Reporting.WebForms.ReportParameter[] reportParameterCollection = new Microsoft.Reporting.WebForms.ReportParameter[1];
                reportParameterCollection[0] = new Microsoft.Reporting.WebForms.ReportParameter("MaterialMasterID", (hifMCode.Value != "" && txtMCode.Text != "" ? hifMCode.Value : "NULL"));
                
                rvMaterialActivityReport.ServerReport.SetParameters(reportParameterCollection);
                rvMaterialActivityReport.ServerReport.Refresh();
            }
            else
            {
                resetError("Please Select Part Number", true);
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