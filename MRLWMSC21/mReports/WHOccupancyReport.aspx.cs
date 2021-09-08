using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Reflection;
using MRLWMSC21Common;
using System.Data;
using System.Globalization;
using Newtonsoft.Json;
using System.IO;
using System.Text;

namespace MRLWMSC21.mReports
{
    public partial class WHOccupancyReport : System.Web.UI.Page
    {
        public static CustomPrincipal cp = HttpContext.Current.User as CustomPrincipal;
        protected void Page_PreInit(object sender, EventArgs e)
        {
            cp = HttpContext.Current.User as CustomPrincipal;
            Page.Theme = "Outbound";
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DesignLogic.SetInnerPageSubHeading(this.Page, "Inventory /Warehouse Occupancy Report");
            }
        }
        [WebMethod]
        public static string GetWHData(string TenantID, string WHID, string date)
        {
            try
            {
                DataSet ds = DB.GetDS("EXEC [dbo].[sp_RPT_WarehouseOccupancyReport] @WarehouseID=" + WHID + ", @TenantID=" + TenantID + ",@Date='" + date + "',@IsExport=0", false);
                return JsonConvert.SerializeObject(ds);
            }
            catch (Exception ex)
            {
                return "Error : " + ex;
            }
        }


        [WebMethod]
        public static string genratePDF(string TenantID, string WHID, string date)
        {
            try
            {
                CustomPrincipal cp1 = HttpContext.Current.User as CustomPrincipal;
                DataSet _dsResult = DB.GetDS("EXEC [dbo].[sp_RPT_WarehouseOccupancyReport] @WarehouseID=" + WHID + ", @TenantID=" + TenantID + ",@Date='" + date + "',@IsExport=0", false);
                List<WHFirstTable> _fTable = new List<WHFirstTable>();
                List<WHSecondTable> _sTable = new List<WHSecondTable>();

                if (_dsResult.Tables.Count > 0)
                {
                    foreach (DataRow _drPick in _dsResult.Tables[0].Rows)
                    {
                        WHFirstTable _whf = new WHFirstTable
                        {
                            TenantName = _drPick["TenantName"].ToString(),
                            AvailableQty = _drPick["AvailableQty"].ToString(),
                            TotalVolume = _drPick["TotalVolume"].ToString(),
                            Occupancy = _drPick["Occupancy"].ToString(),
                            //Commodity = _drPick["Commodity"].ToString(),
                        };
                        _fTable.Add(_whf);
                    }
                    if (_dsResult.Tables[1].Rows != null)
                    {
                        DataRow row = _dsResult.Tables[1].Rows[0];
                        WHSecondTable _whs = new WHSecondTable
                        {
                            WarehouseVolume = row["WarehouseVolume"].ToString(),
                            OccupiedVolume = row["OccupiedVolume"].ToString(),
                            AvailableVolume = row["AvailableVolume"].ToString(),
                        };
                        _sTable.Add(_whs);
                    }
                }
                mOutbound.UnloadPDFGenerator pdfGenerator = new mOutbound.UnloadPDFGenerator(_fTable, _sTable);
                string logo = DB.GetSqlS("SELECT LogoPath AS S FROM GEN_Account WHERE AccountID=" + cp1.AccountID);
                String filePath = pdfGenerator.GenerateWHOCCPDFData(mOutbound.UnloadTemplate.WHOCC_SHEET, logo, WHID);

                return filePath;
            }
            catch (Exception ex)
            {
                return "Error : " + ex;
            }
        }

        public void DTToExcel(DataSet ds, string WHID)
        {
            string FileName = "WarehouseOccupancyReport" + DateTime.Now.Day + "" + DateTime.Now.Month + "" + DateTime.Now.Year + "_" + DateTime.Now.Hour + "" + DateTime.Now.Minute + "" + DateTime.Now.Second;
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

            string Heading = "Warehouse Occupancy Report";
            //string imageFile = MRLWMSC21Common.CommonLogic.SafeMapPath("~/Images/inventrax.jpg");
            string headerTable = @"<Table><tr><td><img width='19%' height='60px' src='" + HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Host + HttpContext.Current.Request.ApplicationPath + "/Images/inventrax.jpg'/></td><td colspan='2' align='center' style='text-align:center;'><div style='color:Maroon;font-size:15pt;font-weight:700;width:100%;background-color:Lightgrey;'>" + Heading + "</div></td></tr>";
            if (WHID == "1")
            {
                headerTable += "<tr><td></td><td colspan='2' align='center' style='text-align:center;'><div style='color:black;font-size:11pt;font-weight:600;width:100%;background-color:Lightgrey;'>Transcrate International Logistics <br/> 3PL Warehouse-01, Amghara </div></td></tr></Table>";
            }
            else if(WHID == "4027")
            {
                headerTable += "<tr><td></td><td colspan='2' align='center' style='text-align:center;'><div style='color:black;font-size:11pt;font-weight:600;width:100%;background-color:Lightgrey;'>Transcrate International Logistics <br/> 3PL Warehouse-01, Sharq </div></td></tr></Table>";
            }
            else
            {
                headerTable += "<tr><td></td><td colspan='2' align='center' style='text-align:center;'><div style='color:black;font-size:11pt !important;font-weight:600;width:100%;background-color:Lightgrey;'>Transcrate International Logistics <br/> 3PL Warehouse-02, Amghara </div></td></tr></Table>";
            }
            response.Write(headerTable);
            response.Write("<br/>&nbsp;");
            if (ds.Tables.Count > 0)
            {
                // create a string writer
                StringWriter sw1 = new StringWriter();
                HtmlTextWriter htw1 = new HtmlTextWriter(sw1);
                //instantiate a datagrid
                DataGrid dg1 = new DataGrid();
                dg1.DataSource = ds.Tables[0];
                dg1.DataBind();
                dg1.HeaderStyle.ForeColor = System.Drawing.Color.White;
                dg1.HeaderStyle.BackColor = System.Drawing.Color.SteelBlue;
                dg1.RenderControl(htw1);

                //string TableHeading1 = "Inbound Details";
                //string headerTable1 = @"<Table><tr><td colspan='10' align='center' style='text-align:center;'><div style='color:black;font-size:14pt;font-weight:700;width:100%;'>" + TableHeading1 + "</div></td></Table>";
                //response.Write(headerTable1);
                response.Write(sw1.ToString());
                response.Write("&nbsp;");
                dg1.Dispose();
            }
            if (ds.Tables.Count > 1)
            {
                StringWriter sw2 = new StringWriter();
                HtmlTextWriter htw2 = new HtmlTextWriter(sw2);
                //instantiate a datagrid
                DataGrid dg2 = new DataGrid();
                dg2.DataSource = ds.Tables[1];
                dg2.DataBind();
                dg2.HeaderStyle.ForeColor = System.Drawing.Color.White;
                dg2.HeaderStyle.BackColor = System.Drawing.Color.SteelBlue;
                dg2.RenderControl(htw2);
                //string TableHeading1 = "Outbound Details";
               // string headerTable2 = @"<Table><tr><td colspan='10' align='center' style='text-align:center;'><div style='color:black;font-size:14pt;font-weight:700;width:100%;'>" + TableHeading1 + "</div></td></Table>";
                //response.Write(headerTable2);
                //Heading = "Week-2";
                if (ds.Tables.Count == 2)
                    //Heading = "Consolidated Totals";

                    // response.Write(headerTable);
                    response.Write(sw2.ToString());
                response.Write("&nbsp;");
                dg2.Dispose();
            }

            ds.Dispose();
            response.End();
        }

        protected void lnkExportData_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtWarehouse.Text == "" || hdnWarehouse.Value == "0")
                {
                    resetError("Please Select Warehouse", true);
                    return;
                }
                if (txtDate.Text == "")
                {
                    resetError("Please Select Date", true);
                    return;
                }
              
                if (txtTenant.Text == "") { hdnTenant.Value = "0"; } else { hdnTenant.Value = hdnTenant.Value; }

                CustomPrincipal cp1 = HttpContext.Current.User as CustomPrincipal;
              
                DataSet ds = DB.GetDS("EXEC [dbo].[sp_RPT_WarehouseOccupancyReport] @WarehouseID=" + hdnWarehouse.Value + ", @TenantID=" + hdnTenant.Value + ",@Date='" + txtDate.Text + "',@IsExport=1", false);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DTToExcel(ds, hdnWarehouse.Value);
                }
                else
                {
                    resetError("No Data Found", true);
                    return;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        protected void resetError(string error, bool isError)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "test", "showStickyToast(" + (isError.ToString().ToLower() == "true" ? "false" : "true") + ",\"" + error + "\");", true);
        }

        public class WHFirstTable
        {
            public string TenantName { get; set; }
            public string AvailableQty { get; set; }
            public string TotalVolume { get; set; }
            public string Occupancy { get; set; }
            public string Commodity { get; set; }
        }
        public class WHSecondTable
        {
            public string WarehouseVolume { get; set; }
            public string OccupiedVolume { get; set; }
            public string AvailableVolume { get; set; }
        }
    }
}