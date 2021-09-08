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
    public partial class Stock_Statement_Summary : System.Web.UI.Page
    {
        public CustomPrincipal cp = HttpContext.Current.User as CustomPrincipal;

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void lnkGetReport_Click(object sender, EventArgs e)
        {
            if (txtFromDate.Text != "" && txtToDate.Text != "")
            {
                rvStockMovementReport.Visible = true;
                rvStockMovementReport.ServerReport.ReportServerUrl = new Uri(CommonLogic.Application("ReportServerUrl"));
                rvStockMovementReport.ServerReport.ReportPath = CommonLogic.Application("ReportPath") + "StockStatementSummary";
                rvStockMovementReport.ShowParameterPrompts = false;
                rvStockMovementReport.ShowPrintButton = true;

                List<ReportParameter> _reportParameterCollection = new List<ReportParameter>();

                ReportParameter paramFromDate = new ReportParameter("fromDate");
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


                ReportParameter paramMType = new Microsoft.Reporting.WebForms.ReportParameter("MTypeID", drpMType.SelectedValue);
                _reportParameterCollection.Add(paramMType);

                ReportParameter paramMCode = new Microsoft.Reporting.WebForms.ReportParameter("MaterialMasterID", hifMCode.Value != "" ? hifMCode.Value : "0");
                _reportParameterCollection.Add(paramMCode);


                rvStockMovementReport.ServerReport.SetParameters(_reportParameterCollection); 
                rvStockMovementReport.ServerReport.Refresh();
            }
            else
            {
                resetError("Select Opening Date and Closing Date", true);
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