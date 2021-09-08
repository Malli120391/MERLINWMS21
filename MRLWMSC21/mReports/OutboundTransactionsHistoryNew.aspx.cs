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
    public partial class OutboundTransactionsHistoryNew : System.Web.UI.Page
    {
        public CustomPrincipal cp1 = HttpContext.Current.User as CustomPrincipal;
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
                DesignLogic.SetInnerPageSubHeading(this.Page, "Outbound / Outbound Transactions History");
            }
        }

        [WebMethod]
        public static string getOBDTransactionList(int TenantId, int MMId, string FromDate, string ToDate, int Warehouseid,string PageIndex,string PageSize)
        {
            cp = HttpContext.Current.User as CustomPrincipal;
            try
            {
                string Query = "EXEC [sp_RPT_GetOutboundTransactionsHistory] @AccountID_New=" + cp.AccountID + ",@TenantID_New=" + TenantId + ", @MaterialMasterId=" + MMId + ",@Warehouseid = " + Warehouseid + ",@NofRecordsPerPage=" + PageSize + ",@Rownumber=" + PageIndex + ",";

                if (FromDate.Trim() != "")
                {
                    Query += "@StartDate=" + DB.DateQuote(FromDate);
                }
                else
                {
                    Query += "@StartDate=NULL";
                }

                if (ToDate.Trim() != "")
                {
                    Query += ",@EndDate=" + DB.DateQuote(ToDate);
                }
                else if (FromDate != "" && ToDate == "")
                {
                    Query += ",@EndDate=" + DB.DateQuote(DateTime.Now);
                }
                else
                {
                    Query += ",@EndDate=NULL";
                }

                DataSet ds = DB.GetDS(Query, false);
                return JsonConvert.SerializeObject(ds);
            }
            catch (Exception ex)
            {
                return "Error : " + ex;
            }
        }

        protected void lnkExportData_Click(object sender, EventArgs e)
        {
            cp1 = HttpContext.Current.User as CustomPrincipal;
            try
            {
                //if (txtTenant.Text == "" || hdnTenant.Value == "0")
                //{
                //    resetError("Please select Tenant", true);
                //    return;
                //}
                if (txtWarehouse.Text == "" || hdnWarehouse.Value == "0")
                {
                    resetError("Please select Warehouse", true);
                    return;
                }
                if (txtPartnumber.Text == "" || hdnPartNo.Value == "0")
                {
                    hdnPartNo.Value = "0";
                }
                string Query = "EXEC [sp_RPT_GetOutboundTransactionsHistory] @AccountID_New=" + cp.AccountID + ",@TenantID_New=" + hdnTenant.Value + ", @MaterialMasterId=" + hdnPartNo.Value + ",@Warehouseid = " + hdnWarehouse.Value + ",@NofRecordsPerPage=200000,@Rownumber=1,";

                if (txtFromDate.Text.Trim() != "")
                {
                    Query += "@StartDate=" + DB.DateQuote(txtFromDate.Text);
                }
                else
                {
                    Query += "@StartDate=NULL";
                }

                if (txtToDate.Text.Trim() != "")
                {
                    Query += ",@EndDate=" + DB.DateQuote(txtToDate.Text);
                }
                else if (txtFromDate.Text.Trim() != "" && txtToDate.Text.Trim() == "")
                {
                    Query += ",@EndDate=" + DB.DateQuote(DateTime.Now);
                }
                else
                {
                    Query += ",@EndDate=NULL";
                }

                DataSet ds = DB.GetDS(Query, false);
                if (ds.Tables[0].Rows.Count > 0)
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

        public void DTToExcel(DataSet ds)
        {
            string FileName = "OutboundTransactionReport" + DateTime.Now.Day + "" + DateTime.Now.Month + "" + DateTime.Now.Year + "_" + DateTime.Now.Hour + "" + DateTime.Now.Minute + "" + DateTime.Now.Second;
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
            string Heading = "Outbound Transaction Report";
            string headerTable = @"<Table><tr><td rowspan='5'><img width='10%'  src='" + HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Host + HttpContext.Current.Request.ApplicationPath + "/TPL/AccountLogos/" + file + "'/></td><td colspan='14' align='center' style='text-align:center;'><div style='color:Maroon;font-size:15pt;font-weight:700;width:100%;background-color:Lightgrey;'>" + Heading + "</div></td></tr>";
            headerTable += "<tr><td></td><td colspan='14' align='center' style='text-align:center;'><div style='color:black;font-size:11pt;font-weight:700;width:100%;background-color:Lightgrey;'>Total Issued Quantity and No. of Outbound Transactions</ div></td></tr></Table>";
            response.Write(headerTable);
            response.Write("&nbsp;");
            if (ds.Tables.Count > 0)
            {
                // create a string writer
                StringWriter sw1 = new StringWriter();
                HtmlTextWriter htw1 = new HtmlTextWriter(sw1);
                //instantiate a datagrid
                DataGrid dg1 = new DataGrid();
                DataTable dt = new DataTable();

                dt = ds.Tables[0];

                dt.Columns["MCode"].ColumnName = "Item Code";
                dt.Columns["TenantCode"].ColumnName = "Tenant Code";
                dt.Columns["TrYear"].ColumnName = "Year of Trans.";

                dt.Columns["1"].ColumnName = "Jan";
                dt.Columns["2"].ColumnName = "Feb";
                dt.Columns["3"].ColumnName = "Mar";
                dt.Columns["4"].ColumnName = "Apr";
                dt.Columns["5"].ColumnName = "May";
                dt.Columns["6"].ColumnName = "Jun";

                dt.Columns["7"].ColumnName = "Jul";
                dt.Columns["8"].ColumnName = "Aug";
                dt.Columns["9"].ColumnName = "Sep";
                dt.Columns["10"].ColumnName = "Oct";
                dt.Columns["11"].ColumnName = "Nov";
                dt.Columns["12"].ColumnName = "Dec";

                dt.Columns.Remove("TotalRecords");
                dt.Columns.Remove("MaterialMasterID");
                dt.Columns.Remove("RID");

                dg1.DataSource = dt;//ds.Tables[0];
                dg1.DataBind();
                dg1.HeaderStyle.ForeColor = System.Drawing.Color.White;
                dg1.HeaderStyle.BackColor = System.Drawing.Color.SteelBlue;
                dg1.RenderControl(htw1);
                string style = @"<style> td { mso-number-format:\@;} </style>";
                response.Write(style);
                response.Write(sw1.ToString());
                response.Write("&nbsp;");
                dg1.Dispose();
            }
            ds.Dispose();
            response.End();
        }

        protected void resetError(string error, bool isError)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "test", "showStickyToast(" + (isError.ToString().ToLower() == "true" ? "false" : "true") + ",\"" + error + "\");", true);
        }


        //[WebMethod]
        //public static List<OutboundTransactionsHistory> GetBillingReportList(int TenantId, int MMId, string FromDate, string ToDate, int Warehouseid)
        //{
        //    List<OutboundTransactionsHistory> lst = new List<OutboundTransactionsHistory>();
        //    string Query = "EXEC [sp_RPT_GetOutboundTransactionsHistory] @AccountID_New=" + cp.AccountID + ",@TenantID_New=" + TenantId + ", @MaterialMasterId="+ MMId + ",@Warehouseid = "+ Warehouseid + ",";

        //    if (FromDate.Trim() != "")
        //    {
        //        Query += "@StartDate=" + DB.DateQuote(FromDate);
        //    }
        //    else
        //    {
        //        Query += "@StartDate=NULL";
        //    }

        //    if (ToDate.Trim() != "")
        //    {
        //        Query += ",@EndDate=" + DB.DateQuote(ToDate);
        //    }
        //    else if (FromDate != "" && ToDate == "")
        //    {
        //        Query += ",@EndDate=" + DB.DateQuote(DateTime.Now);
        //    }
        //    else
        //    {
        //        Query += ",@EndDate=NULL";
        //    }

        //    DataSet DS = DB.GetDS(Query, false);
        //    if (DS.Tables[0].Rows.Count != 0)
        //    {
        //        foreach (DataRow row in DS.Tables[0].Rows)
        //        {
        //            OutboundTransactionsHistory OR = new OutboundTransactionsHistory();
        //            OR.MaterialCode = row["MCode"].ToString();
        //            OR.YearofTrans = row["TrYear"].ToString();
        //            OR.Jan = row["1"].ToString();
        //            OR.Feb = row["2"].ToString();
        //            OR.Mar = row["3"].ToString();
        //            OR.Apr = row["4"].ToString();
        //            OR.May = row["5"].ToString();
        //            OR.June = row["6"].ToString();
        //            OR.July = row["7"].ToString();
        //            OR.Aug = row["8"].ToString();
        //            OR.Sep = row["9"].ToString();
        //            OR.Oct = row["10"].ToString();
        //            OR.Nov = row["11"].ToString();
        //            OR.Dec = row["12"].ToString();
        //            lst.Add(OR);
        //        }
        //    }
        //    return lst;
        //}

        //public class OutboundTransactionsHistory
        //{
        //    public string MaterialCode { get; set; }
        //    public string YearofTrans { get; set; }
        //    public string Jan { get; set; }
        //    public string Feb { get; set; }
        //    public string Mar { get; set; }
        //    public string Apr { get; set; }
        //    public string May { get; set; }
        //    public string June { get; set; }
        //    public string July { get; set; }
        //    public string Aug { get; set; }
        //    public string Sep { get; set; }
        //    public string Oct { get; set; }
        //    public string Nov { get; set; }
        //    public string Dec { get; set; }
        //}
    }
}