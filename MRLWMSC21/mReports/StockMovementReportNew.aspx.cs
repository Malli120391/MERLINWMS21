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

namespace MRLWMSC21.mReports
{
    public partial class StockMovementReportNew : System.Web.UI.Page
    {
        public CustomPrincipal cp = HttpContext.Current.User as CustomPrincipal;
        protected void Page_PreInit(object sender, EventArgs e)
        {
            Page.Theme = "Outbound";
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DesignLogic.SetInnerPageSubHeading(this.Page, "Inventory / Stock Movement Report");
            }
        }
        protected void ResetError(string error, bool isError)
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


            // ScriptManager.RegisterStartupScript(this, this.GetType(), "test", "showStickyToast(" + (isError.ToString().ToLower() == "true" ? "false" : "true") + ",\"" + error + "\");", true);
            ClientScript.RegisterClientScriptBlock(this.GetType(), "ShowStickeyToastOnLogin", "showStickyToast(" + (isError.ToString().ToLower() == "true" ? "false" : "true") + ",\"" + error + "\");");

        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static List<StockMovementReport> GetBillingReportList(string FromDate, string ToDate)
        {
            FromDate = FromDate == "" ? "null" : FromDate;
            ToDate = ToDate == "" ? "null" : ToDate;
            List<StockMovementReport> GetBillingReport = new List<StockMovementReport>();
            GetBillingReport = new StockMovementReportNew().GetGITReport(FromDate, ToDate);
            return GetBillingReport;
        }

        private List<StockMovementReport> GetGITReport(string FromDate, string ToDate)
        {
            if (FromDate != "" && FromDate !="null")
            {
               // FromDate = DateTime.ParseExact(FromDate.Replace("-", "/"), "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");
                //Lines added by meena 
                DateTime str1 = Convert.ToDateTime(FromDate);
                FromDate = str1.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            }
            if (ToDate != "" && ToDate !="null")
            {
                // ToDate = DateTime.ParseExact(ToDate.Replace("-", "/"), "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");
                //Lines added by meena 
                DateTime str2 = Convert.ToDateTime(ToDate);
                ToDate = str2.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);

            }

            FromDate = FromDate == "null" ? FromDate : DB.SQuote(FromDate);
            ToDate = ToDate == "null" ? ToDate : DB.SQuote(ToDate);

            List<StockMovementReport> lst = new List<StockMovementReport>();
            
            string Query = "  EXEC [sp_RPT_GetStockMovementReport] @FromDate = " + FromDate + ",  @ToDate = " + ToDate + ",@AccountID_New=" + cp.AccountID + ",@TenantID_New=" + cp.TenantID;


            DataSet DS = DB.GetDS(Query, false);
            if (DS.Tables[0].Rows.Count != 0)
            {
                foreach (DataRow row in DS.Tables[0].Rows)
                {
                    StockMovementReport GR = new StockMovementReport();
                    GR.PartNo = row["Mcode"].ToString();
                    GR.Description = row["MDescription"].ToString();
                    GR.TransactionDate = row["tranDate"].ToString();
                    GR.UOM = row["UOM"].ToString();
                    GR.GoodsInQty = row["GoodsIN"].ToString();
                    GR.GoodsOutQty = row["GoodsOut"].ToString();
                    GR.AvailableQty = row["AvailQuantity"].ToString();
                    
                   
                    lst.Add(GR);
                }
            }
            else

            {
                ResetError("No Data Found", true);
               
            }
            return lst;
        }

        public class StockMovementReport
        {
            public string PartNo { get; set; }
            public string Description { get; set; }
            public string TransactionDate { get; set; }
            public string UOM { get; set; }
            public string GoodsInQty { get; set; }
            public string GoodsOutQty { get; set; }
            public string AvailableQty { get; set; }
           




           
        }
    }
    
}