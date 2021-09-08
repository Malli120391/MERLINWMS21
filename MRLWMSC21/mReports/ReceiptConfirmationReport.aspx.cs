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
    public partial class ReceiptConfirmationReport : System.Web.UI.Page
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
                DesignLogic.SetInnerPageSubHeading(this.Page, "Inbound / Receipt Confirmation Report");
            }
        }
        [WebMethod]
        public static string getData(string INBID)
        {
            try
            {
                DataSet ds = DB.GetDS("EXEC [dbo].[sp_INB_RPT_ReceivingConfirmationReport] @InboundID=" + INBID + ",@MCode=NULL", false);
                return JsonConvert.SerializeObject(ds);
            }
            catch (Exception ex)
            {
                return "Error : " + ex;
            }
        }

        [WebMethod]
        public static string getLogoName()
        {
            try
            {
                CustomPrincipal cp1 = HttpContext.Current.User as CustomPrincipal;
                string logo = DB.GetSqlS("SELECT LogoPath AS S FROM GEN_Account WHERE AccountID="+cp1.AccountID);
                return logo;
            }
            catch (Exception ex)
            {
                return "Error : " + ex;
            }
        }



        [WebMethod]
        public static string generateRCReport(string INBID,string TenantID) {
            try
            {
                CustomPrincipal cp1 = HttpContext.Current.User as CustomPrincipal;
                DataSet _dsResult = DB.GetDS("EXEC [dbo].[sp_INB_RPT_ReceivingConfirmationReport] @InboundID=" + INBID + ",@MCode=NULL", false);
                List<RCRHeader> _hData = new List<RCRHeader>();
                List<RCRData> _cData = new List<RCRData>();

                if (_dsResult.Tables.Count > 0)
                {
                    if (_dsResult.Tables[1].Rows != null)
                    {
                        DataRow row = _dsResult.Tables[1].Rows[0];
                        RCRHeader _rcrh = new RCRHeader
                        {
                            TenantName = row["TenantName"].ToString(),
                            Address1 = row["Address1"].ToString(),
                            ShipmentType = row["ShipmentType"].ToString(),
                            VehicleRegistrationNo = row["VehicleRegistrationNo"].ToString(),
                            StoreRefNo = row["StoreRefNo"].ToString(),
                            ShipmentReceivedOn = row["ShipmentReceivedOn"].ToString(),
                        };
                        _hData.Add(_rcrh);
                    }
                    foreach (DataRow _drrcrh in _dsResult.Tables[0].Rows)
                    {
                        RCRData _rcrData = new RCRData
                        {
                            MCode = _drrcrh["MCode"].ToString(),
                            MCodeAlternative1 = _drrcrh["MCodeAlternative1"].ToString(),
                            MCodeAlternative2 = _drrcrh["MCodeAlternative2"].ToString(),
                            MDescription = _drrcrh["MDescription"].ToString(),
                            BUoM = _drrcrh["BUoM"].ToString(),
                            InvoiceQuantity = _drrcrh["InvoiceQuantity"].ToString(),

                            ReceivedQty = _drrcrh["ReceivedQty"].ToString(),
                            GoodQty = _drrcrh["GoodQty"].ToString(),
                            DamagedQty = _drrcrh["DamagedQty"].ToString(),

                            ExcessQty = _drrcrh["ExcessQty"].ToString(),
                            ExcessOrShortQty = _drrcrh["DiscrepancyQty"].ToString(),
                            MVolume = _drrcrh["MVolume"].ToString(),
                            MfgDate = _drrcrh["MfgDate"].ToString(),
                            ExpDate = _drrcrh["ExpDate"].ToString(),

                        };
                        _cData.Add(_rcrData);
                    }
                    
                }
                mOutbound.UnloadPDFGenerator pdfGenerator = new mOutbound.UnloadPDFGenerator(_hData, _cData);
                string logo = DB.GetSqlS("SELECT LogoPath AS S FROM GEN_Account WHERE AccountID=" + cp1.AccountID);
                String filePath = pdfGenerator.GenerateRCRPDFData(mOutbound.UnloadTemplate.RCR_SHEET, logo,TenantID);

                return filePath;
            }
            catch (Exception ex)
            {
                return "Error : " + ex;
            }
        }

        public class RCRHeader {
            public string TenantName { set; get; }
            public string Address1 { set; get; }
            public string ShipmentType { set; get; }
            public string VehicleRegistrationNo { set; get; }
            public string StoreRefNo { set; get; }
            public string ShipmentReceivedOn { set; get; }
        }

        public class RCRData {
            public string MCode { set; get; }
            public string MCodeAlternative1 { set; get; }
            public string MCodeAlternative2 { set; get; }
            public string MDescription { set; get; }
            public string BUoM { set; get; }
            public string InvoiceQuantity { set; get; }
            public string ReceivedQty { set; get; }
            public string GoodQty { set; get; }
            public string DamagedQty { set; get; }
            public string ExcessQty { set; get; }
            public string ExcessOrShortQty { set; get; }
            public string MVolume { set; get; }
            public string MfgDate { set; get; }
            public string ExpDate { set; get; }
        }

        protected void lnkExportData_Click(object sender, EventArgs e)
        {
            try
            {
                DataSet ds = DB.GetDS("EXEC [dbo].[sp_INB_RPT_ReceivingConfirmationReport_Export] @InboundID=" + hdnInboundID.Value + ",@MCode=NULL", false);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DTToExcel(ds, hdnTenant.Value);
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

        public void DTToExcel(DataSet ds,string TenantID)
        {
            string FileName = "ReceiptConfirmationReport" + DateTime.Now.Day + "" + DateTime.Now.Month + "" + DateTime.Now.Year + "_" + DateTime.Now.Hour + "" + DateTime.Now.Minute + "" + DateTime.Now.Second;
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
            string Heading = "Receipt Confirmation Report";
            //string imageFile = MRLWMSC21Common.CommonLogic.SafeMapPath("~/Images/inventrax.jpg");
            string headerTable = @"<Table><tr ><td rowspan='8'><img width='14%' height='10px' src='" + HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Host +HttpContext.Current.Request.ApplicationPath + "/TPL/AccountLogos/" + file + "'/></td><td colspan='8' align='center' style='text-align:center;'><div style='color:Maroon;font-size:15pt;font-weight:700;width:100%;background-color:Lightgrey;'>" + Heading + "</div></td></tr></Table>";
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
                Content += "<td style='height:20px;'><b>Receipt ID : </b></td><td>" + ds.Tables[1].Rows[0]["StoreRefNo"].ToString() + "</td>";
                Content += "</tr>";
                Content += "<tr>";
                Content += "<td style='height:20px;'><b>Document Date : </b></td><td style='text-align:left !important;'>" + ds.Tables[1].Rows[0]["ShipmentReceivedOn"].ToString() + "</td>";
                Content += "</tr>";

                Content += "<tr>";
                Content += "<td style='height:20px;'><b>Company : </b></td><td>" + ds.Tables[1].Rows[0]["TenantName"].ToString() + "</td>";
                Content += "</tr>";

                Content += "<tr>";
                Content += "<td style='height:20px;'><b>Address : </b></td><td>" + ds.Tables[1].Rows[0]["Address1"].ToString() + "</td>";
                Content += "</tr>";

                Content += "<tr>";
                Content += "<td style='height:20px;'><b>Shipment Type : </b></td><td>" + ds.Tables[1].Rows[0]["ShipmentType"].ToString() + "</td>";
                Content += "</tr>";
                Content += "<tr>";
                Content += "<td style='height:20px;'><b>Truck No./TrallerID : </b></td><td>" + ds.Tables[1].Rows[0]["VehicleRegistrationNo"].ToString() + "</td>";
                Content += "</tr>";
                Content += "</table>";

                //headerTable = @"<Table><tr><td colspan='10' align='center' style='text-align:center;'><div style='color:Maroon;font-size:15pt;font-weight:700;width:100%;'>" + Heading + "</div></td></Table>";
                response.Write(Content);
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
                dt.Columns["BUoM"].ColumnName = "UoM";
                dt.Columns["MfgDate"].ColumnName = "Mfg. Date";
                dt.Columns["ExpDate"].ColumnName = "Exp. Date";
                dt.Columns["InvoiceQuantity"].ColumnName = "Expected Qty.";
                dt.Columns["ReceivedQty"].ColumnName = "Received Qty.";
                dt.Columns["GoodQty"].ColumnName = "Good Qty.";
                dt.Columns["DamagedQty"].ColumnName = "Damaged Qty."; 
                dt.Columns["ExcessQty"].ColumnName = "Excess Qty.";   
                dt.Columns["DiscrepancyQty"].ColumnName = "Short Qty.";
                dt.Columns["MVolume"].ColumnName = "Volume (CBM)";

                if (TenantID == "52")
                {
                    dt.Columns["MCodeAlternative1"].ColumnName = "Serial No.";
                    dt.Columns["MCodeAlternative2"].ColumnName = "PO Number";
                    dt.Columns["MDescription"].ColumnName = "Airway Bill No.";

                    dt.Columns.Remove("Mfg. Date");
                    dt.Columns.Remove("Exp. Date");
                    dt.Columns.Remove("ExcessOrShortQty");                   
                }
                else if (TenantID == "58")
                {
                    dt.Columns["MCodeAlternative1"].ColumnName = "Size";
                    dt.Columns["MCodeAlternative2"].ColumnName = "Collection";
                    dt.Columns["MDescription"].ColumnName = "Description";

                    dt.Columns.Remove("Mfg. Date");
                    dt.Columns.Remove("Exp. Date");
                    dt.Columns.Remove("ExcessOrShortQty");
                }
                else {
                    dt.Columns["MDescription"].ColumnName = "Description";
                    dt.Columns.Remove("MCodeAlternative1");
                    dt.Columns.Remove("MCodeAlternative2");
                    dt.Columns.Remove("ExcessOrShortQty");
                }

                //if (TenantID == "58" || TenantID == "52")
                //{
                //    dt.Columns["MCodeAlternative1"].ColumnName = "Size";
                //    dt.Columns["MCodeAlternative2"].ColumnName = "Collection";
                //    dt.Columns["MDescription"].ColumnName = "Description";
                //}
                //else
                //{                   
                //    dt.Columns["MCodeAlternative1"].ColumnName = "Serial No.";
                //    dt.Columns["MCodeAlternative2"].ColumnName = "PO Number";
                //    dt.Columns["MDescription"].ColumnName = "Airway Bill No.";
                //}

                dg2.DataSource = dt;//ds.Tables[0];
                dg2.DataBind();
                dg2.HeaderStyle.ForeColor = System.Drawing.Color.White;
                dg2.HeaderStyle.BackColor = System.Drawing.Color.SteelBlue;
                dg2.RenderControl(htw2);
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
    }
}