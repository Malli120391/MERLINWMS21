using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using MRLWMSC21Common;
using System.Globalization;
using System.IO;
using Microsoft.Reporting.WebForms;

namespace MRLWMSC21.mReports
{
    public partial class SpaceUtilizationReport : System.Web.UI.Page
    {
        public CustomPrincipal cp = HttpContext.Current.User as CustomPrincipal;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BuildSiteDropDowns(ddlWarehouse, "SELECT WHCode+'-'+Location AS WHCode, WarehouseID  FROM GEN_Warehouse where IsDeleted=0 and IsActive=1", "WHCode", "WarehouseID");
            }

        }

        protected void lnkGetReport_Click(object sender, EventArgs e)
        {
            if (txtFromDate.Text != "" && txtToDate.Text != "")
            {
                rvStockMovementReport.Visible = true;
                rvStockMovementReport.ServerReport.ReportServerUrl = new Uri(CommonLogic.Application("ReportServerUrl"));
                rvStockMovementReport.ServerReport.ReportPath = CommonLogic.Application("ReportPath") + "SpaceUtilizationReport";
                rvStockMovementReport.ShowParameterPrompts = false;
                rvStockMovementReport.ShowPrintButton = true;

                List<ReportParameter> _reportParameterCollection = new List<ReportParameter>();

                ReportParameter paramFromDate = new ReportParameter("StartDate");
                if (txtFromDate.Text.ToString() != "")
                    paramFromDate.Values.Add(ReportCommon.FormatDate(txtFromDate.Text, '/'));
                else
                    paramFromDate.Values.Add(null);
                _reportParameterCollection.Add(paramFromDate);


                ReportParameter paramToDate = new ReportParameter("EndDate");
                if (txtToDate.Text.ToString() != "")
                    paramToDate.Values.Add(ReportCommon.FormatDate(txtToDate.Text, '/'));
                else
                    paramToDate.Values.Add(null);
                _reportParameterCollection.Add(paramToDate);

                ReportParameter paramWarehouse = new Microsoft.Reporting.WebForms.ReportParameter("WarehouseID", ddlWarehouse.SelectedValue.ToString() != " " ? ddlWarehouse.SelectedValue.ToString() : " ");
                _reportParameterCollection.Add(paramWarehouse);

                rvStockMovementReport.ServerReport.SetParameters(_reportParameterCollection);
                rvStockMovementReport.ServerReport.Refresh();

            }
            else
            {
                resetError("Select From Date and To Date", true);
            }

        }

        private void resetError(string error, bool isError)
        {

            ScriptManager.RegisterStartupScript(this, this.GetType(), "test", "showStickyToast(" + (isError.ToString().ToLower() == "true" ? "false" : "true") + ",\"" + error + "\");", true);

        }

        protected void lnkPrint_Click(object sender, EventArgs e)
        {

        }

        public void BuildSiteDropDowns(DropDownList ddlDropdown, String sql, String FildName,String secondName)
        {
            IDataReader rsDropdown = DB.GetRS(sql);
            ddlDropdown.Items.Add(new ListItem("ALL", "0")); 
            while (rsDropdown.Read())
            {
                ddlDropdown.Items.Add(new ListItem(DB.RSField(rsDropdown, FildName), DB.RSFieldInt(rsDropdown, secondName).ToString()));
            }
            rsDropdown.Close();
        }

    }

}