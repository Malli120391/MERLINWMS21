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

namespace MRLWMSC21.mReports
{
    public partial class MaterialAgeingReport : System.Web.UI.Page
    {
        protected CustomPrincipal cp = HttpContext.Current.User as CustomPrincipal;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            BuildSiteDropDowns(ddlSite, "SELECT LocationZoneCode FROM INV_LocationZone where IsDeleted=0 and IsActive=1", "LocationZoneCode");

        }

        protected void lnkGetReport_Click(object sender, EventArgs e)
        {
            rvMaterialAgeing.Visible = true;
            rvMaterialAgeing.ServerReport.ReportServerUrl = new Uri(CommonLogic.Application("ReportServerUrl"));
            rvMaterialAgeing.ServerReport.ReportPath = CommonLogic.Application("ReportPath") + "MaterialAgeingReport";
            rvMaterialAgeing.ShowParameterPrompts = false;
            rvMaterialAgeing.ShowPrintButton = true;

            Microsoft.Reporting.WebForms.ReportParameter[] reportParameterCollection = new Microsoft.Reporting.WebForms.ReportParameter[3];

            //reportParameterCollection[0] = new Microsoft.Reporting.WebForms.ReportParameter("MaterialMasterID", hifMCode.Value != "" ? hifMCode.Value : null);
            reportParameterCollection[0] = new Microsoft.Reporting.WebForms.ReportParameter("AgeIndays", drpAgeing.SelectedValue);
            reportParameterCollection[1] = new Microsoft.Reporting.WebForms.ReportParameter("ZoneCode", ddlSite.SelectedValue != " " ? ddlSite.SelectedValue.ToString() : " ");
            reportParameterCollection[2] = new Microsoft.Reporting.WebForms.ReportParameter("MaterialMasterID", hifMCode.Value != "" ? hifMCode.Value : "0");

            rvMaterialAgeing.ServerReport.SetParameters(reportParameterCollection);
            rvMaterialAgeing.ServerReport.Refresh();
        }

        protected void lnkPrint_Click(object sender, EventArgs e)
        {

        }
        public void BuildSiteDropDowns(DropDownList ddlDropdown, String sql, String FildName)
        {
            IDataReader rsDropdown = DB.GetRS(sql);
            ddlDropdown.Items.Add(new ListItem("ALL", " "));
            while (rsDropdown.Read())
            {
                ddlDropdown.Items.Add(new ListItem(DB.RSField(rsDropdown, FildName), DB.RSField(rsDropdown, FildName)));
            }
            rsDropdown.Close();
        }
        
    }
}