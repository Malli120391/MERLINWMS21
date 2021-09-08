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
using System.IO;
using System.Text;

namespace MRLWMSC21.mReports
{
    public partial class MaterialTrackingReportNew : System.Web.UI.Page
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
                DesignLogic.SetInnerPageSubHeading(this.Page, "Inventory / Material Traceability Report");
            }
        }
        //[WebMethod]
        //[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        //public static FullList GetBillingReportList(string Serialno, string Batchno, int TenantId, int WareHouseId, string MaterialID)
        //{
        //    Serialno = Serialno == "" ? "null" : Serialno;
        //    Batchno = Batchno == "" ? "null" : Batchno;
        //    FullList l = new FullList();
        //    l = new MaterialTrackingReportNew().GetBillngRPT(Serialno, Batchno, cp.AccountID, TenantId, WareHouseId, MaterialID);
        //    return l;
        // }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static FullList GetBillingReportList(int TenantId, int WareHouseId, string MaterialID)
        {
            FullList l = new FullList();
            l = new MaterialTrackingReportNew().GetBillngRPT(cp.AccountID, TenantId, WareHouseId, MaterialID);
            return l;
        }
        //private FullList GetBillngRPT(string Serialno, string Batchno,int accountid, int TenantId, int WareHouseId,string MaterialID)
        private FullList GetBillngRPT(int accountid, int TenantId, int WareHouseId, string MaterialID)
        {          
            List<MaterialTrackingReportIn> lst = new List<MaterialTrackingReportIn>();
            List<MaterialTrackingReportOut> lst1 = new List<MaterialTrackingReportOut>();
            //string Query = " EXEC [sp_RPT_GetMaterialTracking_In] @BatchNumber=" + DB.SQuote(Batchno) + ", @SerialNumber= " + DB.SQuote(Serialno) + ",@MaterialMasterID=" + MaterialID + ",@AccountID_New=" + accountid + ",@TenantID_New=" + TenantId + ", @WareHouseId = " + WareHouseId + ",@NofRecordsPerPage=200000,@Rownumber=1,@IsExport=0; ";
            //Query += " EXEC [sp_RPT_GetMaterialTracking_Out] @BatchNumber=" + DB.SQuote(Batchno) + ", @SerialNumber= " + DB.SQuote(Serialno) + ",@MaterialMasterID=" + MaterialID + ",@AccountID_New=" + accountid + ",@TenantID_New=" + TenantId + ", @WareHouseId = "+WareHouseId + ",@NofRecordsPerPage=200000,@Rownumber=1,@IsExport=0; ";

            string Query = " EXEC [sp_RPT_GetMaterialTracking_In] @MaterialMasterID=" + MaterialID + ",@IsExport=0,@TenantID=" + TenantId + ", @WareHouseId = " + WareHouseId + "; ";
            Query += " EXEC [sp_RPT_GetMaterialTracking_Out] @MaterialMasterID=" + MaterialID + ",@IsExport=0,@TenantID=" + TenantId + ", @WareHouseId = " + WareHouseId + "";

            DataSet DS = DB.GetDS(Query, false);
            if (DS.Tables[0].Rows.Count != 0)
            {
                foreach (DataRow row in DS.Tables[0].Rows)
                {
                    MaterialTrackingReportIn BR = new MaterialTrackingReportIn();
                    BR.PONumber = row["PONumber"].ToString();
                    BR.InvoiceNumber = row["InvoiceNumber"].ToString();
                    BR.Tenant = row["TenantName"].ToString();
                    BR.Supplier = row["SupplierName"].ToString();
                    BR.StoreRefNo = row["StoreRefNo"].ToString();
                    BR.PartNo = row["MCode"].ToString();
                    BR.ReceivedQty = row["Quantity"].ToString();
                    BR.PODate = row["PODate"].ToString();
                    BR.InvoiceDate = row["InvoiceDate"].ToString();
                    //BR.location= row["location"].ToString();
                    //BR.sloc = row["Code"].ToString();

                    lst.Add(BR);
                }
            }

            if (DS.Tables[1].Rows.Count != 0)
            {
                foreach (DataRow row in DS.Tables[1].Rows)
                {
                    MaterialTrackingReportOut BR1 = new MaterialTrackingReportOut();
                    BR1.SONumber = row["SONumber"].ToString();
                    BR1.SODate = row["SODate"].ToString();
                    BR1.CustomerPoNo = row["CustPONumber"].ToString();
                    BR1.Customer = row["CustomerName"].ToString();
                    BR1.PartNo = row["MCode"].ToString();
                    BR1.PickedQty = row["Quantity"].ToString();
                    BR1.OutboundNumber = row["OBDNumber"].ToString();
                    lst1.Add(BR1);
                }
            }
            FullList l = new FullList();
            l.one = lst;
            l.two = lst1;
            return l;
        }

        public void DTToExcel(DataSet ds)
        {
            string FileName = "MaterialTraceabilityReport" + DateTime.Now.Day + "" + DateTime.Now.Month + "" + DateTime.Now.Year + "_" + DateTime.Now.Hour + "" + DateTime.Now.Minute + "" + DateTime.Now.Second;
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

            string Heading = "Material Traceability Report";
            //string imageFile = MRLWMSC21Common.CommonLogic.SafeMapPath("~/Images/inventrax.jpg");
            string file = DB.GetSqlS("SELECT LogoPath AS S FROM GEN_Account WHERE AccountID=" + cp.AccountID);
            string headerTable = @"<Table><tr><td rowspan='4'><img width='10%'  src='" + HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Host + HttpContext.Current.Request.ApplicationPath+ "/TPL/AccountLogos/" + file + "'/></td><td colspan='9' align='center' style='text-align:center;'><div style='color:Maroon;font-size:15pt;font-weight:700;width:100%;background-color:Lightgrey;'>" + Heading + "</div></td></Table>";
            response.Write(headerTable);
            response.Write("<br/>");
            response.Write("&nbsp;");
            if (ds.Tables[0].Rows.Count > 0)
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

                string TableHeading1 = "Inbound Receipt";
                string headerTable1 = @"<Table><tr><td colspan='9' align='center' style='text-align:center;'><div style='color:black;font-size:14pt;font-weight:700;width:100%;'>" + TableHeading1 + "</div></td></Table>";
                response.Write(headerTable1);
                string style = @"<style> td { mso-number-format:\@;} </style>";
                response.Write(style);
                response.Write(sw1.ToString());
                response.Write("&nbsp;");
                dg1.Dispose();
            }
            if (ds.Tables[1].Rows.Count > 1)
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
                string TableHeading1 = "Outbound Delivery";
                string headerTable2 = @"<Table><tr><td colspan='9' align='center' style='text-align:center;'><div style='color:black;font-size:14pt;font-weight:700;width:100%;'>" + TableHeading1 + "</div></td></Table>";
                response.Write(headerTable2);
                //Heading = "Week-2";
                string style = @"<style> td { mso-number-format:\@;} </style>";
                response.Write(style);
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
                if (txtMaterial.Text == "" || hdnMaterial.Value == "0")
                {
                    resetError("Please select Material", true);
                    return;
                }
                //var serialno = txtserailno.Text;
                //var batchno = txtbatchno.Text;

                //if (serialno == "" && batchno == "")
                //{
                //    resetError("Please Enter Serial No./Batch No.",true);
                //    return;
                //}
                //if (serialno == "") {
                //    txtserailno.Text = "null";
                //}
                //if (batchno == "")
                //{
                //    txtbatchno.Text = "null";
                //}
                if (txtTenant.Text == "") { hdnTenant.Value = "0"; } else { hdnTenant.Value = hdnTenant.Value; }
                if (txtWarehouse.Text == "") { hdnWarehouse.Value = "0"; } else { hdnWarehouse.Value = hdnWarehouse.Value; }
                if (txtMaterial.Text == "") { hdnMaterial.Value = "0"; } else { hdnMaterial.Value = hdnMaterial.Value; }
                CustomPrincipal cp1 = HttpContext.Current.User as CustomPrincipal;
                //DataSet ds = DB.GetDS("EXEC [dbo].[sp_RPT_WarehouseStockInformation] @WarehouseID=" + hdnWarehouse.Value + ", @TenantID=" + hdnTenant.Value + ",@ToDate='" + txtDate.Text + "',@MaterialMasterID=" + hdnItem.Value, false);
                //string Query = " EXEC [sp_RPT_GetMaterialTracking_In] @BatchNumber=" + DB.SQuote(txtbatchno.Text) + ", @SerialNumber= " + DB.SQuote(txtserailno.Text) + ",@MaterialMasterID=" + hdnMaterial.Value + ",@AccountID_New=" + cp1.AccountID + ",@TenantID_New=" + hdnTenant.Value + ", @WareHouseId = " + hdnWarehouse.Value + ",@NofRecordsPerPage=200000,@Rownumber=1,@IsExport=1; ";
                //Query += " EXEC [sp_RPT_GetMaterialTracking_Out] @BatchNumber=" + DB.SQuote(txtbatchno.Text) + ", @SerialNumber= " + DB.SQuote(txtserailno.Text) + ",@MaterialMasterID=" + hdnMaterial.Value + ",@AccountID_New=" + cp1.AccountID + ",@TenantID_New=" + hdnTenant.Value + ", @WareHouseId = " + hdnWarehouse.Value + ",@NofRecordsPerPage=200000,@Rownumber=1,@IsExport=1;  ";

                string Query = " EXEC [sp_RPT_GetMaterialTracking_In] @MaterialMasterID=" + hdnMaterial.Value + ",@TenantID=" + hdnTenant.Value + ", @IsExport=1,@WareHouseId = " + hdnWarehouse.Value + "; ";
                Query += " EXEC [sp_RPT_GetMaterialTracking_Out] @MaterialMasterID=" + hdnMaterial.Value + ",@TenantID=" + hdnTenant.Value + ",@IsExport=1,@WareHouseId = " + hdnWarehouse.Value + "";


                DataSet ds = DB.GetDS(Query, false);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DTToExcel(ds);
                }
                if (ds.Tables[1].Rows.Count > 0)
                {
                    DTToExcel(ds);
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



        public class MaterialTrackingReportIn
        {
            public string PONumber { get; set; }
            public string InvoiceNumber { get; set; }
            public string Tenant { get; set; }
            public string Supplier { get; set; }
            public string StoreRefNo { get; set; }
            public string PartNo { get; set; }
            public string ReceivedQty { get; set; }
            public string location { get; set; }
            public string sloc { get; set; }
            public string PODate { get; set; }
            public string InvoiceDate { get; set; }

        }

        public class MaterialTrackingReportOut
        {
            public string SONumber { get; set; }
            public string SODate { get; set; }
            public string CustomerPoNo { get; set; }
            public string Customer { get; set; }
            public string PartNo { get; set; }
            public string PickedQty { get; set; }
            public string OutboundNumber { get; set; }

        }

        public class FullList

        {
            public List<MaterialTrackingReportIn> one { get; set; }
            public List<MaterialTrackingReportOut> two { get; set; }
        }
    }
}