using MRLWMSC21Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MRLWMSC21.mInventory
{
    public partial class CycleCountReport : System.Web.UI.Page
    {
        public static CustomPrincipal cp = HttpContext.Current.User as CustomPrincipal;
        public CustomPrincipal cp2 = HttpContext.Current.User as CustomPrincipal;
        protected void page_PreInit(object sender, EventArgs e)
        {
            Page.Theme = "Inventory";
            cp = HttpContext.Current.User as CustomPrincipal;
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //DesignLogic.SetInnerPageSubHeading(this.Page, "Cycle Count Report");
            }
        }
        [WebMethod]
        public static string GetList(string CTID, string PageIndex, string PageSize, string LocationID, string MaterialMasterID)
        {
            
            try
            {
                CustomPrincipal cp1 = HttpContext.Current.User as CustomPrincipal;
                DataSet ds = DB.GetDS("EXEC [dbo].[sp_CCM_CycleCountReport] @CCM_TRN_CycleCount_ID=" + CTID + ",@AccountID=" + cp1.AccountID + ",@Rownumber=" + PageIndex + ",@NofRecordsPerPage=" + PageSize + ",@LocationID=" + LocationID + ",@MaterialMasterID=" + MaterialMasterID, false);
                return JsonConvert.SerializeObject(ds);
                //SET SP ewms.USP_SET_CCM_CNF_AccountCycleCounts
            }
            catch (Exception ex)
            {
                return "Error : " + ex;
            }
        }

        protected void lnkExportData_Click(object sender, EventArgs e)
        {
  
            if (hdnCount.Value == "0")
            {
                resetError("No Data Found", true);
                return;
            }
            if (hdnWarehouse.Value == "0" || txtWarehouse.Text == "")
            {
                resetError("Please select Warehouse", true);
                return;
            }
            if (CCM_CNF_AccountCycleCount_ID.Value == "0" || txtCycleCountName.Text == "")
            {
                resetError("Please select Cycle Count Name", true);
                return;
            }
            if (CycleCountCode.Value == "0" || txtCycleCountCode.Text == "")
            {
                resetError("Please select Cycle Count Code", true);
                return;
            }
            if (txtLocation.Text == "" || hdnLocationID.Value == "0")
            {
                hdnLocationID.Value = "0";
            }
            if (txtPartnumber.Text == "" || hdnMID.Value == "0")
            {
                hdnMID.Value = "0";
            }
            string PageIndex = "1";
            string PageSize = "300000";
            CustomPrincipal cp1 = HttpContext.Current.User as CustomPrincipal;
            DataSet ds = DB.GetDS("EXEC [dbo].[sp_CCM_CycleCountReport] @CCM_TRN_CycleCount_ID=" + CycleCountCode.Value + ",@AccountID=" + cp1.AccountID + ",@Rownumber=" + PageIndex + ",@NofRecordsPerPage=" + PageSize + ",@LocationID=" + hdnLocationID.Value + ",@MaterialMasterID=" + hdnMID.Value, false);
            if (ds.Tables[0].Rows.Count > 1)
            {
                DTToExcel(ds, hdnWarehouse.Value);
            }
            else
            {
                resetError("No Data Found", true);
                return;
            }
        }

        public void DTToExcel(DataSet ds, string WarehouseID)
        {
            string FileName = "CycleCountReport_" + DateTime.Now.Day + "" + DateTime.Now.Month + "" + DateTime.Now.Year + "_" + DateTime.Now.Hour + "" + DateTime.Now.Minute + "" + DateTime.Now.Second;
            FileInfo f = new FileInfo(Server.MapPath("Downloads") + string.Format("\\{0}.xlsx", FileName));
            if (f.Exists)
                f.Delete(); // delete the file if it already exist.

            HttpResponse response = HttpContext.Current.Response;

            response.Clear();
            response.Buffer = true;
            response.ClearHeaders();
            response.ClearContent();
            response.Charset = Encoding.UTF8.WebName;
            response.AddHeader("content-disposition", "attachment; filename=" + FileName + ".xls");
            response.AddHeader("Content-Type", "application/Excel");
            response.ContentType = "application/vnd.xlsx";
            string file = DB.GetSqlS("SELECT LogoPath AS S FROM GEN_Account WHERE AccountID=" + cp.AccountID);
            string Heading = "Cycle Count Report";
            string warehouse = DB.GetSqlS("SELECT WHName+'-'+WHCode S FROM GEN_Warehouse WHERE IsActive=1 AND IsDeleted=0 AND WarehouseID =" + WarehouseID);
            //string imageFile = MRLWMSC21Common.CommonLogic.SafeMapPath("~/Images/inventrax.jpg");
            string headerTable = @"<Table><tr><td rowspan='4'><img width='14%' height='10%'  src='" + HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Host + HttpContext.Current.Request.ApplicationPath + "/TPL/AccountLogos/" + file + "'/></td><td colspan='6' align='center' style='text-align:center;'><div style='color:black;font-size:15pt;font-weight:700;width:100%;background-color:Lightgrey;'>" + Heading + "</div></td></tr>";

            headerTable += "<tr><td></td><td colspan='3' align='center' style='text-align:center;'><div style='color:black;font-size:12pt;font-weight:700;width:100%;background-color:Lightgrey;'>Warehouse : " + warehouse + "</ div></td></tr></Table>";

            response.Write(headerTable);
            response.Write("&nbsp;");
            if (ds.Tables.Count > 0)
            {
                // create a string writer
                StringWriter sw1 = new StringWriter();
                HtmlTextWriter htw1 = new HtmlTextWriter(sw1);
                //instantiate a datagrid
                DataGrid dg1 = new DataGrid();
                dg1.HeaderStyle.ForeColor = System.Drawing.Color.White;
                dg1.HeaderStyle.BackColor = System.Drawing.Color.SteelBlue;
                dg1.RenderControl(htw1);
                string Content = "";
                Content += " <table style='width:100%;'>";
                Content += "<tr>";
                Content += "<td style='height:30px;'><b>Cycle Count Type </b></td><td> <b>:</b>&emsp;" + ds.Tables[0].Rows[0]["CycleCountName"].ToString() + "</td>";
                //Content += "</tr>";
                //Content += "<tr>";
                Content += "<td></td><td><b>Cycle Count Name </b></td><td><b>:</b>&emsp;" + ds.Tables[0].Rows[0]["AccountCycleCountName"].ToString() + "</td>";
                Content += "</tr>";
                Content += "<tr>";
                Content += "<td style='height:30px;'><b>Cycle Count Seq. Code </b></td><td><b>:</b>&emsp;" + ds.Tables[0].Rows[0]["CycleCountCode"].ToString() + "</td>";
                //Content += "</tr>";
                //Content += "<tr>";
                Content += "<td></td><td><b>Seq. No. </b></td><td style='text-align:left'><b>:</b>&emsp;" + ds.Tables[0].Rows[0]["SeqNo"].ToString() + "</td>";

                Content += "</tr>";
                Content += "<tr>";
                Content += "<td style='height:30px;'><b>Frequency </b></td><td style='text-align:left'><b>:</b>&emsp;" + ds.Tables[0].Rows[0]["FrequencyName"].ToString() + "</td>";
                Content += "<td></td><td><b>Status </b></td><td style='text-align:left'><b>:</b>&emsp;" + ds.Tables[0].Rows[0]["StatusName"].ToString() + "</td>";
                Content += "</tr>";
                Content += "</table>";

                //headerTable = @"<Table><tr><td colspan='10' align='center' style='text-align:center;'><div style='color:Maroon;font-size:15pt;font-weight:700;width:100%;'>" + Heading + "</div></td></Table>";
                response.Write(Content);
                string style = @"<style> td { mso-number-format:\@;} </style>";
                response.Write(style);
                response.Write(sw1.ToString());
                response.Write("&nbsp;");
                dg1.Dispose();
            }
            if (ds.Tables.Count > 0)
            {
                StringWriter sw2 = new StringWriter();
                HtmlTextWriter htw2 = new HtmlTextWriter(sw2);
                //instantiate a datagrid
                DataGrid dg2 = new DataGrid();


                DataTable dt = new DataTable();

                dt = ds.Tables[0];

                //dt.Columns["AccountCycleCountName"].ColumnName = "Cycle Count Name";
                //dt.Columns["SeqNo"].ColumnName = "Seq. No.";
                dt.Columns["LogicalQuantity"].ColumnName = "Logical Qty.";
                dt.Columns["PhysicalQuantity"].ColumnName = "Physical Qty.";
                dt.Columns["MCode"].ColumnName = "Part Number";
                //dt.Columns["Carton Code"].ColumnName = "Container";
                dt.Columns["UserName"].ColumnName = "User Name";

                dt.Columns.Remove("AccountCode");
                //dt.Columns.Remove("ActivityTimestamp");
                dt.Columns.Remove("CCM_CNF_AccountCycleCount_ID");
                dt.Columns.Remove("Carton Code");
                dt.Columns.Remove("CCM_MST_CycleCountStatus_ID");
                dt.Columns.Remove("CCM_TRN_CycleCount_ID");
                dt.Columns.Remove("CycleCountName");

                dt.Columns.Remove("AccountCycleCountName");
                dt.Columns.Remove("SeqNo");

                dt.Columns.Remove("CycleTimeInDays");
                dt.Columns.Remove("FrequencyName");
                dt.Columns.Remove("LocationID");

                dt.Columns.Remove("MaterialMasterID");
                dt.Columns.Remove("RID");

                dt.Columns.Remove("StatusName");
                dt.Columns.Remove("TotalRecords");
                dt.Columns.Remove("ValidFrom");

                dt.Columns.Remove("ValidThru");
                dt.Columns.Remove("CycleCountCode");
                dt.Columns.Remove("Project Ref. No.");
                dt.Columns.Remove("MRP");

                dg2.DataSource = dt;//ds.Tables[0];


                //dg2.DataSource = ds.Tables[0];
                dg2.DataBind();
                dg2.HeaderStyle.ForeColor = System.Drawing.Color.White;
                dg2.HeaderStyle.BackColor = System.Drawing.Color.SteelBlue;
                dg2.RenderControl(htw2);


                //Heading = "Week-2";
                if (ds.Tables.Count == 2)
                { }
                //Heading = "Consolidated Totals";

                // response.Write(headerTable);
                string style = @"<style> td { mso-number-format:\@;} </style>";
                response.Write(style);
                response.Write(sw2.ToString());
                response.Write("&nbsp;");
                dg2.Dispose();
            }

            ds.Dispose();
            response.End();
        }

        protected void resetError(string error, bool isError)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "test", "showStickyToast(" + (isError.ToString().ToLower() == "true" ? "false" : "true") + ",\"" + error + "\");", true);
        }
    }
}