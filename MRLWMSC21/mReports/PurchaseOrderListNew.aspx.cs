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
    public partial class PurchaseOrderListNew : System.Web.UI.Page
        
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
                DesignLogic.SetInnerPageSubHeading(this.Page, "Orders / Purchase Order Report");
            }
        }
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static List<PurchageOrderList> GetBillingReportList(string TenantID,string PONumber, string PODate, string StatusId, string TypeID, string SupplierId, string FromDate, string ToDate)
        {
            PONumber = PONumber == "" ? "null" : DB.SQuote(PONumber);
            PODate = PODate == "" ? "null" : PODate;
            
            StatusId = StatusId == "" ? "null" : StatusId;
            TypeID = TypeID == "" ? "null" : TypeID;
            SupplierId=SupplierId==""?"null":SupplierId;
            FromDate = FromDate == "" ? "null" : FromDate;
            ToDate = ToDate == "" ? "null" : ToDate;


            List<PurchageOrderList> GetBillingReport = new List<PurchageOrderList>();
            GetBillingReport = new PurchaseOrderListNew().GetBillngRPT(TenantID,PONumber, PODate, StatusId, TypeID,SupplierId,FromDate,ToDate);
            return GetBillingReport;
        }

        private List<PurchageOrderList> GetBillngRPT(string TenantID, string PONumber, string PODate, string StatusId, string TypeID, string SupplierId, string FromDate, string ToDate)
        {
            if (PODate != "" && PODate !="null")
            {
                //  PODate = DateTime.ParseExact(PODate.Replace("-", "/"), "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");
                //Lines added by meena 
                DateTime str3 = Convert.ToDateTime(PODate);
                PODate = str3.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);

            }
            if (FromDate != "" && FromDate !="null")
            {
                //FromDate = DateTime.ParseExact(FromDate.Replace("-", "/"), "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");
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

            PODate = PODate == "null" ? PODate : DB.SQuote(PODate);
            FromDate = FromDate == "null" ? FromDate : DB.SQuote(FromDate);
            ToDate = ToDate == "null" ? ToDate : DB.SQuote(ToDate);

            List<PurchageOrderList> lst = new List<PurchageOrderList>();
            string Query = "  EXEC [sp_RPT_GetPOData]  @PONumber= " + PONumber+",@PODate= "+PODate+ ",@StatusID="+StatusId+",@POTypeID="+TypeID+",@SupplierID ="+SupplierId+", @FromDate = " + FromDate + ", @ToDate = " + ToDate + ",@AccountID_New=" + cp.AccountID + ",@TenantID_New=" + TenantID;
           
            DataSet DS = DB.GetDS(Query, false);
            if (DS.Tables[0].Rows.Count != 0)
            {
                foreach (DataRow row in DS.Tables[0].Rows)
                {
                    PurchageOrderList BR = new PurchageOrderList();
                    BR.PONumber = row["PONumber"].ToString();
                    BR.PODate = row["PODate"].ToString();
                    BR.Tenant = row["TenantName"].ToString();
                    BR.Supplier = row["Supplier"].ToString();
                    BR.POType = row["POType"].ToString();
                    BR.TotalValue = row["TotalValue"].ToString();
                    BR.Currency = row["Code"].ToString();
                    BR.ExRate = row["ExchangeRate"].ToString();
                    BR.Tax = row["POTax"].ToString();
                    BR.Status = row["POStatus"].ToString();

                    lst.Add(BR);
                }
            }
            return lst;
        }


        public class PurchageOrderList
        {

            public string PONumber { get; set; }
            public string PODate { get; set; }
            public string Tenant { get; set; }
            public string Supplier { get; set; }
            public string POType { get; set; }
            public string TotalValue { get; set; }
            public string Currency { get; set; }
            public string ExRate { get; set; }
            public string Tax { get; set; }
            public string Status { get; set; }

        }
    }
}