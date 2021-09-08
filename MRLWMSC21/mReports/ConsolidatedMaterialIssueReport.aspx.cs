using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Microsoft.Reporting.WebForms;
using MRLWMSC21Common;

namespace MRLWMSC21.mReports
{
    public partial class ConsolidatedMaterialIssueReport : System.Web.UI.Page 
    {
        public CustomPrincipal cp = HttpContext.Current.User as CustomPrincipal;

        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        protected void lnkGetReport_Click(object sender, EventArgs e)
        {
            rvMaterialIssueReport.Visible = true;
            rvMaterialIssueReport.ServerReport.ReportServerUrl = new Uri(CommonLogic.Application("ReportServerUrl"));
            rvMaterialIssueReport.ServerReport.ReportPath = CommonLogic.Application("ReportPath") + "ConsolidatedMaterialIssueReport";
            rvMaterialIssueReport.ShowParameterPrompts = false;
            rvMaterialIssueReport.ShowPrintButton = true;

            List<ReportParameter> _reportParameterCollection = new List<ReportParameter>();

            //ReportParameter paramPOH = new Microsoft.Reporting.WebForms.ReportParameter("ProductionOrderHeaderID", (hifJobRefNoNumber.Value !="0" && txtJobOrderRefNo.Text != "" ? hifJobRefNoNumber.Value : "0"));
            //_reportParameterCollection.Add(paramPOH);

            ReportParameter paramKitCode = new ReportParameter("KitCode");
            if (hifKitCode.Value != "" && txtKitCode.Text != "")
                paramKitCode.Values.Add(txtKitCode.Text);
            else
                paramKitCode.Values.Add(null);
            _reportParameterCollection.Add(paramKitCode);


            ReportParameter paramSONumber = new ReportParameter("SOHeaderID");
            if (hifSONumber.Value != "" && atcSONumber.Text != "")
                paramSONumber.Values.Add(hifSONumber.Value);
            else
                paramSONumber.Values.Add(null);
            _reportParameterCollection.Add(paramSONumber);


            ReportParameter paramMCode = new ReportParameter("MaterialMasterID");
            if (hifMCode.Value != "" && txtMCode.Text != "")
                paramMCode.Values.Add(hifMCode.Value);
            else
                paramMCode.Values.Add(null);
            _reportParameterCollection.Add(paramMCode);

            ReportParameter paramFromDate = new ReportParameter("FromDate");
            if (ObdFromDate.Text.ToString() != "")
                paramFromDate.Values.Add(ReportCommon.FormatDate(ObdFromDate.Text, '/'));
            else
                paramFromDate.Values.Add(null);
            _reportParameterCollection.Add(paramFromDate);


            ReportParameter paramToDate = new ReportParameter("ToDate");
            if (ObdToDate.Text.ToString() != "")
                paramToDate.Values.Add(ReportCommon.FormatDate(ObdToDate.Text, '/'));
            else
                paramToDate.Values.Add(null);
            _reportParameterCollection.Add(paramToDate);


            rvMaterialIssueReport.ServerReport.SetParameters(_reportParameterCollection);
            rvMaterialIssueReport.ServerReport.Refresh();

        }

        protected void lnkPrint_Click(object sender, EventArgs e)
        {

        }

    }
}