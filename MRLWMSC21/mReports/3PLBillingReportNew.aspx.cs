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
    public partial class _3PLBillingReportNew : System.Web.UI.Page
    {
        protected void Page_PreInit(object sender, EventArgs e)
        {
            Page.Theme = "Outbound";
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DesignLogic.SetInnerPageSubHeading(this.Page, "3PL Billing Reports");
            }
        }
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static List<BillingReport> GetBillingReportList(string TanentID, string FromDate, string ToDate)
        {
            //TanentID = TanentID == "" ? "0" : TanentID;

            List<BillingReport> GetBillingReport = new List<BillingReport>();
            GetBillingReport = new _3PLBillingReportNew().GetBillngRPT(TanentID, FromDate, ToDate);
            return GetBillingReport;
        }

        private List<BillingReport> GetBillngRPT(string TanentID, string FromDate, string ToDate)
        {
            if (FromDate != "")
            {
                FromDate = DateTime.ParseExact(FromDate.Replace("-", "/"), "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");
            }
            if (ToDate != "")
            {
                ToDate = DateTime.ParseExact(ToDate.Replace("-", "/"), "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");
            }
            

            List<BillingReport> lst = new List<BillingReport>();
            string Query = "EXEC sp_RPT_3PLInboundBillingReport @TenantID = " + TanentID + ", @StartDate = " + DB.SQuote(FromDate) + ", @EndDate = "+DB.SQuote(ToDate)+"";
            DataSet DS = DB.GetDS(Query, false);
            if (DS.Tables[0].Rows.Count != 0)
            {
                foreach (DataRow row in DS.Tables[0].Rows)
                {
                    BillingReport BR = new BillingReport();
                    BR.TatentName = row["TenantName"].ToString();
                    BR.StoreRefNo = row["StoreRefNo"].ToString();
                    BR.SupplierName = row["SupplierName"].ToString();
                    BR.PONumber = row["PONumber"].ToString();
                    BR.Receipt = row["Receipt"].ToString();
                    BR.Services = row["Service/Material"].ToString();
                    BR.UoM = row["UoM"].ToString();
                    BR.UnitCost = Convert.ToDecimal(row["UnitCost"].ToString());
                    BR.UnitCostAfterDisc = Convert.ToDecimal(row["UnitCostAfterDiscount"].ToString());
                    BR.Quantity = Convert.ToDecimal(row["Quantity"].ToString());
                    BR.TotalCost = Convert.ToDecimal(row["TotalCost"].ToString());
                    BR.TotalCostAfterDisc = Convert.ToDecimal(row["TotalCostAfterDiscount"].ToString());
                    BR.TotalCostAfterDiscWithOutTax = Convert.ToDecimal(row["TotalCostAfterDiscountWithTaxes"].ToString());
                    BR.Tax =row["Tax"].ToString();
                    lst.Add(BR);
                }
            }
            return lst;
        }


        public class BillingReport
        {
            public string TatentName { get; set; }
            public string StoreRefNo { get; set; }
            public string SupplierName { get; set; }
            public string PONumber { get; set; }
            public string Receipt { get; set; }
            public string Services { get; set; }
            public string UoM { get; set; }
            public decimal UnitCost { get; set; }
            public decimal UnitCostAfterDisc { get; set; }
            public decimal Quantity { get; set; }
            public decimal TotalCost { get; set; }
            public decimal TotalCostAfterDisc { get; set; }
            public decimal TotalCostAfterDiscWithOutTax { get; set; }
            public string Tax { get; set; }
        }
    }
}