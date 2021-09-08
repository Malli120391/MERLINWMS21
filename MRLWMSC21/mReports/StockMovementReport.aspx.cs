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
    public partial class StockMovementReport : System.Web.UI.Page
    {
        public CustomPrincipal cp = HttpContext.Current.User as CustomPrincipal;

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void lnkGetReport_Click(object sender, EventArgs e)
        {
            if (ObdFromDate.Text != "" && ObdToDate.Text != "")
            {
                rvStockMovementReport.Visible = true;
                rvStockMovementReport.ServerReport.ReportServerUrl = new Uri(CommonLogic.Application("ReportServerUrl"));
                rvStockMovementReport.ServerReport.ReportPath = CommonLogic.Application("ReportPath") + "StockMovementReport";
                rvStockMovementReport.ShowParameterPrompts = false;
                rvStockMovementReport.ShowPrintButton = true;

                List<ReportParameter> _reportParameterCollection = new List<ReportParameter>();


                //ReportParameter paramMCode = new ReportParameter("MaterialMasterID");
                //if (hifMCode.Value != "" && txtMCode.Text != "")
                //    paramMCode.Values.Add(hifMCode.Value);
                //else
                //    paramMCode.Values.Add(null);
                //_reportParameterCollection.Add(paramMCode);

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


                rvStockMovementReport.ServerReport.SetParameters(_reportParameterCollection);
                rvStockMovementReport.ServerReport.Refresh();
            }
            else
            {
                resetError("Enter From Date & To Date", true);
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