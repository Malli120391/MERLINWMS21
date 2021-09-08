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
    public partial class WHStockInfoReport : System.Web.UI.Page
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
                DesignLogic.SetInnerPageSubHeading(this.Page, "Inventory / Warehouse Stock Information Report");
            }
        }

        [WebMethod]
        public static string GetWHData(string TenantID, string WHID, string date,string MID)
        {
            try
            {
                DataSet ds = DB.GetDS("EXEC [dbo].[sp_RPT_WarehouseStockInformation] @WarehouseID=" + WHID + ", @TenantID=" + TenantID + ",@ToDate='" + date + "',@MaterialMasterID=" + MID, false);
                return JsonConvert.SerializeObject(ds);
            }
            catch (Exception ex)
            {
                return "Error : " + ex;
            }
        }

        public void DTToExcel(DataSet ds, string TenantID,string WarehouseID)
        {
            cp = HttpContext.Current.User as CustomPrincipal;
            string FileName = "WarehouseStockInformationReport" + DateTime.Now.Day + "" + DateTime.Now.Month + "" + DateTime.Now.Year + "_" + DateTime.Now.Hour + "" + DateTime.Now.Minute + "" + DateTime.Now.Second;
            FileInfo f = new FileInfo(Server.MapPath("Downloads") + string.Format("\\{0}.xlsx", FileName));
            if (f.Exists)
                f.Delete(); // delete the file if it already exist.
            string file = DB.GetSqlS("SELECT LogoPath AS S FROM GEN_Account WHERE AccountID=" + cp.AccountID);
            HttpResponse response = HttpContext.Current.Response;

            response.Clear();
            response.Buffer = true;
            response.ClearHeaders();
            response.ClearContent();
            response.Charset = Encoding.UTF8.WebName;
            response.AddHeader("content-disposition", "attachment; filename=" + FileName + ".xls");
            response.AddHeader("Content-Type", "application/Excel");
            response.ContentType = "application/vnd.xlsx";
           
            string Heading = "Warehouse Stock Information Report"; 
            //string imageFile = MRLWMSC21Common.CommonLogic.SafeMapPath("~/Images/inventrax.jpg");
            string headerTable = @"<Table><tr><td rowspan='4'><img width='10%' height='4px' src='"+HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Host+ HttpContext.Current.Request.ApplicationPath + "/TPL/AccountLogos/" + file + "'/></td><td colspan='9' align='center' style='text-align:center;'><div style='color:Maroon;font-size:15pt;font-weight:700;width:100%;background-color:Lightgrey;'>" + Heading + "</div></td></tr>";

            /*if (WarehouseID == "1")
            {
                headerTable += "<tr><td></td><td colspan='9' align='center' style='text-align:center;'><div style='color:black;font-size:12pt;font-weight:700;width:100%;background-color:Lightgrey;'>3PL Warehouse - 01, Amghara</ div></td></tr></Table>";
            }
            else if (WarehouseID == "4027")
            {
                headerTable += "<tr><td></td><td colspan='9' align='center' style='text-align:center;'><div style='color:black;font-size:12pt;font-weight:700;width:100%;background-color:Lightgrey;'>3PL Warehouse - 01, Sharq</ div></td></tr></Table>";
            }
            else
            {
                headerTable += "<tr><td></td><td colspan='9' align='center' style='text-align:center;'><div style='color:black;font-size:12pt;font-weight:700;width:100%;background-color:Lightgrey;'>3PL Warehouse - 02, Amghara</div></td></tr></Table>";
            }*/
            headerTable += "<tr><td></td><td colspan='9' align='center' style='text-align:center;'><div style='color:black;font-size:12pt;font-weight:700;width:100%;background-color:Lightgrey;'>3PL Warehouse</div></td></tr></Table>";
            response.Write(headerTable);
            response.Write("&nbsp;");
            if (ds.Tables.Count > 1)
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
                Content += "<td style='height:40px;'><b>Company : </b></td><td>" + ds.Tables[1].Rows[0]["TenantName"].ToString() + "</td>";
                Content += "</tr>";
                Content += "<tr>";
                Content += "<td style='height:50px;'><b>Address : </b></td><td>" + ds.Tables[1].Rows[0]["Address1"].ToString() + "</td>";
                Content += "</tr>";
                Content += "<tr>";
                Content += "<td style='height:20px;'><b>Contact Person : </b></td><td>" + ds.Tables[1].Rows[0]["PCPName"].ToString() + "</td>";
                Content += "</tr>";
                Content += "<tr>";
                Content += "<td style='height:20px;'><b>Email ID : </b></td><td style='text-align:left'>" + ds.Tables[1].Rows[0]["PCPEmail"].ToString() + "</td>";
                Content += "</tr>";
                Content += "<tr>";
                Content += "<td style='height: 20px;'><b>Doc. Date : </b></td><td style='text-align:left'>" + ds.Tables[1].Rows[0]["CurrentDate"].ToString() + "</td>";
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

                dt.Columns["MCode"].ColumnName = "Item Code";
                dt.Columns["UoMQty"].ColumnName = "UoM/Qty.";
                dt.Columns["MfgDate"].ColumnName = "Mfg. Date";
                dt.Columns["ExpDate"].ColumnName = "Exp. Date";
                dt.Columns["BatchNo"].ColumnName = "Batch No.";

                dt.Columns["OnhandQty"].ColumnName = "On hand Qty.";
                dt.Columns["AllocatedQty"].ColumnName = "Allocated Qty.";
                dt.Columns["GoodQty"].ColumnName = "Good Qty.";
                dt.Columns["DamagedQty"].ColumnName = "Damaged Qty.";
                dt.Columns["ItemVolume"].ColumnName = "Volume (CBM)";

                if (TenantID == "58")
                {
                    dt.Columns["Size"].ColumnName = "Size";
                    dt.Columns["Collection"].ColumnName = "Collection";
                    dt.Columns["Cus-Design & Color"].ColumnName = "Cus-Design & Color";

                    dt.Columns.Remove("Mfg. Date");
                    dt.Columns.Remove("Exp. Date");
                    dt.Columns.Remove("Batch No.");
                }
                else if (TenantID == "52" || TenantID == "39")
                {
                    dt.Columns["SerialNo"].ColumnName = "Serial No.";
                    dt.Columns["PONumber"].ColumnName = "PO Number";
                    dt.Columns["AirwayBillNo"].ColumnName = "Airway Bill No.";

                    dt.Columns.Remove("Mfg. Date");
                    dt.Columns.Remove("Exp. Date");
                    dt.Columns.Remove("Batch No.");
                }
                else if (TenantID == "80")
                {
                    dt.Columns["SerialNo"].ColumnName = "Model";
                    dt.Columns["PONumber"].ColumnName = "Brand";
                    dt.Columns["AirwayBillNo"].ColumnName = "Description";
                    dt.Columns.Remove("Mfg. Date");
                    dt.Columns.Remove("Exp. Date");
                    dt.Columns.Remove("Batch No.");
                }
                else
                {
                    dt.Columns["AirwayBillNo"].ColumnName = "Description";

                    dt.Columns.Remove("PONumber");
                    dt.Columns.Remove("SerialNo");
                }

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

        protected void lnkExportData_Click(object sender, EventArgs e)
        {
            try
            {
                if (hdnCount.Value == "0")
                {
                    resetError("No Data Found", true);
                    return;
                }
                if (txtTenant.Text == "" || hdnTenant.Value == "0")
                {
                    resetError("Please select Tenant", true);
                    return;
                }
                if (txtWarehouse.Text == "" || hdnWarehouse.Value == "0")
                {
                    resetError("Please select Warehouse", true);
                    return;
                }
                if (txtDate.Text == "")
                {
                    resetError("Please select Date", true);
                    return;
                }
                DataSet ds = DB.GetDS("EXEC [dbo].[sp_RPT_WarehouseStockInformation] @WarehouseID=" + hdnWarehouse.Value + ", @TenantID=" + hdnTenant.Value + ",@ToDate='" + txtDate.Text + "',@MaterialMasterID=" + hdnItem.Value, false);
                if (ds.Tables[0].Rows.Count > 1)
                {
                    DTToExcel(ds, hdnTenant.Value, hdnWarehouse.Value);
                }
                else {
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
            /*
            string str = "<font class=\"noticeMsg\">NOTICE:</font>&nbsp;&nbsp;&nbsp;";
            if (isError)
                str = "<font class=\"errorMsg\">ERROR:</font>&nbsp;&nbsp;&nbsp;";

            if (error.Length > 0)
                str += error + "";
            else
                str = "";
            lblStatusMessage.Text = str;*/

            ScriptManager.RegisterStartupScript(this, this.GetType(), "test", "showStickyToast(" + (isError.ToString().ToLower() == "true" ? "false" : "true") + ",\"" + error + "\");", true);


        }

        private string GetAbsoluteUrl(string relativeUrl)
        {
            relativeUrl = relativeUrl.Replace("~/", string.Empty);
            string[] splits = Request.Url.AbsoluteUri.Split('/');
            if (splits.Length >= 2)
            {
                string url = splits[0] + "//";
                for (int i = 2; i < splits.Length - 1; i++)
                {
                    url += splits[i];
                    url += "/";
                }

                return url + relativeUrl;
            }
            return relativeUrl;
        }


        [WebMethod]
        public static string genratePDF(string TenantID, string WHID, string date,string MID)
        {
            try
            {
                CustomPrincipal cp1 = HttpContext.Current.User as CustomPrincipal;
                DataSet _dsResult = DB.GetDS("EXEC [dbo].[sp_RPT_WarehouseStockInformation] @WarehouseID=" + WHID + ", @TenantID=" + TenantID + ",@ToDate='" + date + "',@MaterialMasterID=" + MID, false);
                List<WHStockHTable> _whshTable = new List<WHStockHTable>();
                List<WHStockMTable> _whsmTable = new List<WHStockMTable>();
                String filePath = "";
                if (_dsResult.Tables.Count > 0)
                {
                    if (_dsResult.Tables[0].Rows != null)
                    {
                        if (_dsResult.Tables[0].Rows.Count > 1)
                        {
                            if (TenantID == "58")
                            {
                                foreach (DataRow row1 in _dsResult.Tables[0].Rows)
                                {
                                    WHStockMTable _whs = new WHStockMTable
                                    {
                                        ItemCode = row1["MCode"].ToString(),
                                        MCodeAlternative1 = row1["Size"].ToString(),
                                        MCodeAlternative2 = row1["Collection"].ToString(),
                                        Description = row1["Cus-Design & Color"].ToString(),

                                        Category = row1["Category"].ToString(),
                                        UoMQty = row1["UoMQty"].ToString(),
                                        MfgDate = row1["MfgDate"].ToString(),
                                        ExpDate = row1["ExpDate"].ToString(),
                                        BatchNo = row1["BatchNo"].ToString(),
                                        OnhandQty = row1["OnhandQty"].ToString(),
                                        AllocatedQty = row1["AllocatedQty"].ToString(),
                                        GoodQty = row1["GoodQty"].ToString(),
                                        DamagedQty = row1["DamagedQty"].ToString(),
                                        Volume = row1["ItemVolume"].ToString(),
                                    };
                                    _whsmTable.Add(_whs);
                                }
                            }
                            else if (TenantID == "52" || TenantID == "39" || TenantID == "80")
                            {
                                foreach (DataRow row2 in _dsResult.Tables[0].Rows)
                                {
                                    WHStockMTable _whs = new WHStockMTable
                                    {
                                        ItemCode = row2["MCode"].ToString(),
                                        MCodeAlternative1 = row2["SerialNo"].ToString(),
                                        MCodeAlternative2 = row2["PONumber"].ToString(),
                                        Description = row2["AirwayBillNo"].ToString(),

                                        Category = row2["Category"].ToString(),
                                        UoMQty = row2["UoMQty"].ToString(),
                                        MfgDate = row2["MfgDate"].ToString(),
                                        ExpDate = row2["ExpDate"].ToString(),
                                        BatchNo = row2["BatchNo"].ToString(),
                                        OnhandQty = row2["OnhandQty"].ToString(),
                                        AllocatedQty = row2["AllocatedQty"].ToString(),
                                        GoodQty = row2["GoodQty"].ToString(),
                                        DamagedQty = row2["DamagedQty"].ToString(),
                                        Volume = row2["ItemVolume"].ToString(),
                                    };
                                    _whsmTable.Add(_whs);
                                }
                            }
                            else
                            {

                                foreach (DataRow row3 in _dsResult.Tables[0].Rows)
                                {
                                    WHStockMTable _whs = new WHStockMTable
                                    {
                                        ItemCode = row3["MCode"].ToString(),
                                        MCodeAlternative1 = "",
                                        MCodeAlternative2 = "",
                                        Description = row3["AirwayBillNo"].ToString(),

                                        Category = row3["Category"].ToString(),
                                        UoMQty = row3["UoMQty"].ToString(),
                                        MfgDate = row3["MfgDate"].ToString(),
                                        ExpDate = row3["ExpDate"].ToString(),
                                        BatchNo = row3["BatchNo"].ToString(),
                                        OnhandQty = row3["OnhandQty"].ToString(),
                                        AllocatedQty = row3["AllocatedQty"].ToString(),
                                        GoodQty = row3["GoodQty"].ToString(),
                                        DamagedQty = row3["DamagedQty"].ToString(),
                                        Volume = row3["ItemVolume"].ToString(),
                                    };
                                    _whsmTable.Add(_whs);
                                }
                            }
                        }
                        else
                        {
                            filePath = "No Data Found";
                            return filePath;
                        }
                    }

                    if (_dsResult.Tables[1].Rows != null)
                    {
                        DataRow row = _dsResult.Tables[1].Rows[0];
                        WHStockHTable _whf = new WHStockHTable
                        {
                            TenantName = row["TenantName"].ToString(),
                            Address = row["Address1"].ToString(),
                            ContactPerson = row["PCPName"].ToString(),
                            EmailID = row["PCPEmail"].ToString(),
                            DocDate = row["CurrentDate"].ToString(),
                        };
                        _whshTable.Add(_whf);
                    }
                    mOutbound.UnloadPDFGenerator pdfGenerator = new mOutbound.UnloadPDFGenerator(_whshTable, _whsmTable);
                    string logo = DB.GetSqlS("SELECT LogoPath AS S FROM GEN_Account WHERE AccountID=" + cp1.AccountID);
                    filePath = pdfGenerator.GenerateWHStockPDFData(mOutbound.UnloadTemplate.WHStock_SHEET, logo, TenantID, WHID);
                }
                else
                {
                    filePath = "No Data Found";
                }
                return filePath;
            }
            catch (Exception ex)
            {
                return "Error : " + ex;
            }
        }

        public class WHStockHTable
        {
            public string TenantName { get; set; }
            public string Address { get; set; }
            public string ContactPerson { get; set; }
            public string EmailID { get; set; }
            public string DocDate { get; set; }
        }
        public class WHStockMTable
        {
            public string ItemCode { get; set; }
            public string MCodeAlternative1 { get; set; }
            public string MCodeAlternative2 { get; set; }
            public string Description { get; set; }
            public string Category { get; set; }
            public string UoMQty { get; set; }
            public string MfgDate { get; set; }
            public string ExpDate { get; set; }
            public string BatchNo { get; set; }
            public string OnhandQty { get; set; }
            public string AllocatedQty { get; set; }
            public string GoodQty { get; set; }
            public string DamagedQty { get; set; }
            public string Volume { get; set; }
        }
    }
}