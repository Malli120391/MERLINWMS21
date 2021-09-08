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
    public partial class GITReportNew : System.Web.UI.Page
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
                DesignLogic.SetInnerPageSubHeading(this.Page, " Inbound / GIT Report");
            }
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static List<GITReport> GetBillingReportList(string FromDate, string ToDate)
        {
            FromDate = FromDate == "" ? "null" : FromDate;
            ToDate = ToDate == "" ? "null" : ToDate;

            List<GITReport> GetBillingReport = new List<GITReport>();
            GetBillingReport = new GITReportNew().GetGITReport(FromDate, ToDate);
            return GetBillingReport;
        }

        private List<GITReport> GetGITReport(string FromDate, string ToDate)
        {
            if (FromDate != "" && FromDate !="null")
            {
                // FromDate = DateTime.ParseExact(FromDate, "dd-M-yy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");
                //Lines added by meena 
                DateTime str1 = Convert.ToDateTime(FromDate);
                FromDate = str1.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                //  FromDate = DateTime.ParseExact(FromDate.Replace("-", "/"), "dd-M-yy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");
            }
            if (ToDate != "" && ToDate !="null")
            {
                //Lines added by meena 
                DateTime str2 = Convert.ToDateTime(ToDate);
                ToDate = str2.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
               // ToDate = DateTime.ParseExact(ToDate, "dd-M-yy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");
               // ToDate = DateTime.ParseExact(ToDate.Replace("-", "/"), "dd-M-yy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");
            }

            FromDate = FromDate == "null" ? FromDate : DB.SQuote(FromDate);
            ToDate = ToDate == "null" ? ToDate : DB.SQuote(ToDate);

            List<GITReport> lst = new List<GITReport>();
            //string Query = "EXEC sp_RPT_3PLInboundBillingReport @TenantID = " + TanentID + ", @StartDate = " + DB.SQuote(FromDate) + ", @EndDate = " + DB.SQuote(ToDate) + "";
            string Query = " EXEC [sp_RPT_GetGoodsInTransit] @FromDate = " + FromDate + ",  @ToDate = " + ToDate + ",@AccountID_New=" + cp.AccountID + ",@TenantID_New=" + cp.TenantID;
            DataSet DS = DB.GetDS(Query, false);
            if (DS.Tables[0].Rows.Count != 0)
            {
                foreach (DataRow row in DS.Tables[0].Rows)
                {
                    GITReport GR = new GITReport();
                    GR.StoreRefNo = row["StoreRefNo"].ToString();
                    GR.DocRcvdDate = row["DocReceivedDate"].ToString();
                    GR.Tenant = row["TenantName"].ToString();
                    GR.Supplier = row["SupplierName"].ToString();
                    GR.BLorAWBNO = row["B/L or Airway Bill No"].ToString();
                    GR.BLorAWBDate = row["B/L or Airway Bill Date"].ToString();
                    GR.InvoiceNO = row["InvoiceNumber"].ToString();
                    GR.InvoiceDate = row["InvoiceDate"].ToString();
                    GR.Invoicevalue =row["InvoiceValue"].ToString();
                    GR.Currency = row["Code"].ToString();
                    GR.ExRate = row["ExchangeRate"].ToString();
                    
                   
                    lst.Add(GR);
                }
            }
            return lst;
        }

        public class GITReport
        {
            public string StoreRefNo { get; set; }
            public string DocRcvdDate { get; set; }
            public string Tenant { get; set; }
            public string Supplier { get; set; }
            public string BLorAWBNO { get; set; }
            public string BLorAWBDate { get; set; }
            public string InvoiceNO { get; set; }
            public string InvoiceDate { get; set; }
            public string Invoicevalue { get; set; }
            public string Currency { get; set; }
            public string ExRate { get; set; }




           
        }
    }
}