using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using MRLWMSC21Common;
using System.Web.UI.HtmlControls;
using System.Globalization;

namespace MRLWMSC21.mInventory
{
    public partial class NCReportForGoodsIn : System.Web.UI.Page
    {

        public CustomPrincipal cp = HttpContext.Current.User as CustomPrincipal;

        protected void page_PreInit(object sender, EventArgs e)
        {
            Page.Theme = "Inventory";
        }

        protected void page_Init(object sender, EventArgs e)
        {
            gvNCReport.Columns[2].HeaderText = "<div style=\"position:relative;top:-2px;left:-2px;\">  <table width=\"100.6%\" border=\"1\" cellspacing=\"0\" rules=\"all\" style=\"border-collapse:collapse;\">  <tr><td colspan=\"3\" align=\"center\">QC Result</td></tr>   <tr ><td width=\"20%\" align=\"center\">Ref. No.</td><td width=\"60%\" align=\"center\">QC Parameters </td><td width=\"20%\" align=\"center\">Status</td></tr></table></div>";
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DesignLogic.SetInnerPageSubHeading(this.Page, "Inward NC Report");
            }
        }

        protected void lnkGet_Click(object sender, EventArgs e)
        {
            if ((txtRefNo.Text.Trim() == "" || txtRefNo.Text.Split('-').Count() == 2) && (atcPOHeader.Text != "" && hifPOHeader.Value != ""))
            {
                Build_gvNCReport(Build_gvNCReport());
            }
            else if (txtRefNo.Text != "" && txtRefNo.Text.Split('-').Count() != 2)
            {
                resetError("Ref. No. not valid,check once", true);
            }
            else
            {
                resetError("Select Ref. No. or PO Number",true);
            }
        }

        private DataSet Build_gvNCReport()
        {
            String GetNCReport = "sp_INV_GetNCGoodsInReportForPOHeader @POHeaderID=" + hifPOHeader.Value + ",@RefNo="+(txtRefNo.Text.Trim()!=""? DB.SQuote(txtRefNo.Text.Trim()):"null");
            DataSet dsGetNCReport = DB.GetDS(GetNCReport,false);
            return dsGetNCReport;
        }

        private void Build_gvNCReport(DataSet dsGetNCReport)
        {
            gvNCReport.DataSource = dsGetNCReport;
            gvNCReport.DataBind();
            dsGetNCReport.Dispose();
            if (gvNCReport.Rows.Count == 0)
            {
                resetError("No NC items to this criteria", false);
            }
        }

        protected void gvNCReport_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if ((e.Row.RowType == DataControlRowType.DataRow) && !(e.Row.RowState == DataControlRowState.Edit))
            {
                DataRow row = ((DataRowView)e.Row.DataItem).Row;
                String QCValues = row["QCValues"].ToString();
                String[] SerialNoList = QCValues.Split(',');
                String GenerateTable = "<div style=\"position:relative;top:-2px;left:-2px;\"> <table cellpadding=\"0\" cellspacing=\"0\" width=\"100.6%\" align=\"center\" valign=\"center\" border=\"1\" cellspacing=\"0\" rules=\"all\" style=\"border-collapse:collapse;\">";//<tr ><td width=\"20%\" align=\"center\">Ref No.</td><td width=\"60%\" align=\"center\">QC Parameters </td><td width=\"20%\" align=\"center\">Status</td></tr>";
                foreach (String SerialNo in SerialNoList)
                {
                    String[] SerialNoDetails= SerialNo.Split('|');
                    GenerateTable += "<tr><td  width=\"20%\" align=\"center\">" + SerialNoDetails[0] + "</td><td width=\"60%\" align=\"center\">";
                    String[] SerialNoDetailsList = SerialNoDetails[1].Split('!');

                    GenerateTable += "<table width=\"100%\">";
                    foreach (String SerialNoDetailspick in SerialNoDetailsList)
                    {
                        GenerateTable += "<tr><td width=\"30%\">" + SerialNoDetailspick.Split(':')[0] + "</td><td width=\"70%\">:" + SerialNoDetailspick.Split(':')[1] + "</td></tr>";
                    }
                    GenerateTable += "</table>";

                    GenerateTable += "</td><td width=\"20%\" align=\"center\">" + SerialNoDetails[2] + "</td>";
                }
                GenerateTable += "</table></div>";
                ((Literal)e.Row.FindControl("ltQCResult")).Text = GenerateTable;
            }
        }

        protected void gvNCReport_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvNCReport.PageIndex = e.NewPageIndex;
            Build_gvNCReport(Build_gvNCReport());
        }

        private void resetError(string error, bool isError)
        {

            /* string str = "<font class=\"noticeMsg\">NOTICE:</font>&nbsp;&nbsp;&nbsp;";
             if (isError)
                 str = "<font class=\"errorMsg\">ERROR:</font>&nbsp;&nbsp;&nbsp;";

             if (error.Length > 0)
                 str += error + "";
             else
                 str = "";


             lblDailogStatus.Text = str;*/
            ScriptManager.RegisterStartupScript(this, this.GetType(), "test", "showStickyToast(" + (isError.ToString().ToLower() == "true" ? "false" : "true") + ",\"" + error + "\");", true);

        }
    }
}