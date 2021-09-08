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
    public partial class SupplierInvoiceValueNew : System.Web.UI.Page
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
                DesignLogic.SetInnerPageSubHeading(this.Page, "Orders / Supplier Invoice Value Report");
            }
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static List<SupplierInvoiceValue> GetBillingReportList(string FromDate, string ToDate)
        {
           FromDate = FromDate == "" ? "null" : FromDate;
           ToDate = ToDate == "" ? "null" : ToDate;

            List<SupplierInvoiceValue> GetBillingReport = new List<SupplierInvoiceValue>();
            GetBillingReport = new SupplierInvoiceValueNew().GetGITReport(FromDate, ToDate);
            return GetBillingReport;
        }

        private List<SupplierInvoiceValue> GetGITReport(string FromDate, string ToDate)
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
                //Lines added by meena 
                DateTime str2 = Convert.ToDateTime(ToDate);
                ToDate = str2.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                //ToDate = DateTime.ParseExact(ToDate.Replace("-", "/"), "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");
            }

           FromDate = FromDate == "null" ? FromDate : DB.SQuote(FromDate);
           ToDate = ToDate == "null" ? ToDate : DB.SQuote(ToDate);

            List<SupplierInvoiceValue> lst = new List<SupplierInvoiceValue>();
            
            string Query = " EXEC [sp_RPT_GetSupplierInvoiceValue] @FromDate = " + FromDate + ",  @ToDate = " + ToDate + ",@AccountID_New="+ cp.AccountID + ",@TenantID_New=" + cp.TenantID;            
            DataSet DS = DB.GetDS(Query, false);
            if (DS.Tables[0].Rows.Count != 0)
            {
                foreach (DataRow row in DS.Tables[0].Rows)
                {
                    SupplierInvoiceValue GR = new SupplierInvoiceValue();
                    GR.StoreRefNo = row["StoreRefNo"].ToString();
                    GR.ShipmentRcvdOn = row["ShipmentReceivedOn"].ToString();
                    GR.Tenant = row["TenantName"].ToString();
                    GR.Supplier = row["SupplierName"].ToString();
                    GR.PONumber = row["PONumber"].ToString();
                    GR.InvoiceNo = row["InvoiceNumber"].ToString();
                    GR.InvoiceDate = row["InvoiceDate"].ToString();
                    GR.Invoicevalue = row["InvoiceValue"].ToString();
                    GR.Currency = row["Code"].ToString();
                    //GR.ExRate = row["ExchangeRate"].ToString();


                    lst.Add(GR);
                }
            }
            return lst;
        }

        public class SupplierInvoiceValue
        {
            public string StoreRefNo { get; set; }
            public string ShipmentRcvdOn { get; set; }
            public string Tenant { get; set; }
            public string Supplier { get; set; }
            public string PONumber { get; set; }
            public string InvoiceNo { get; set; }
            public string InvoiceDate { get; set; }
            public string Invoicevalue { get; set; }
            public string Currency { get; set; }
           // public string ExRate { get; set; }





        }
    }
}