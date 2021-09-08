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
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MRLWMSC21.mReports
{
    public partial class SupplierReturnsNew : System.Web.UI.Page
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
                DesignLogic.SetInnerPageSubHeading(this.Page, "Returns / Supplier Returns Report");
            }
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static List<SupplierReturns> GetBillingReportList(string FromDate, string ToDate, int TenantId, int WareHouse)
        {
            FromDate = FromDate == "" ? "null" : FromDate;
            ToDate = ToDate == "" ? "null" : ToDate;

            List<SupplierReturns> GetBillingReport = new List<SupplierReturns>();
            GetBillingReport = new SupplierReturnsNew().GetGITReport(FromDate, ToDate,TenantId,WareHouse);
            return GetBillingReport;
        }

        private List<SupplierReturns> GetGITReport(string FromDate, string ToDate, int TenantId, int WareHouse)
        {
            cp = HttpContext.Current.User as CustomPrincipal;
            if (FromDate == "null" || FromDate == null)
            {
                FromDate = "";

            }
            else if (FromDate != "" )
            {
                FromDate = DateTime.ParseExact(FromDate, "dd-MMM-yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");
            }
            if (ToDate == "null" || ToDate == null)
            {
                ToDate = "";
            }
            else if (ToDate != "")
            {
                ToDate = DateTime.ParseExact(ToDate, "dd-MMM-yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");
            }

            FromDate = FromDate == "null" ? FromDate : DB.SQuote(FromDate);
            ToDate = ToDate == "null" ? ToDate : DB.SQuote(ToDate);


            List<SupplierReturns> lst = new List<SupplierReturns>();
            string Query = "  EXEC [sp_RPT_GetSupplierReturns] @FromDate = " + FromDate + ",  @ToDate = " + ToDate + ",@AccountID_New=" + cp.AccountID + ",@TenantID_New=" + TenantId+ ", @WareHouseId = "+WareHouse+"";
             //@FromDate=NULL, @ToDate=NULL

            DataSet DS = DB.GetDS(Query, false);
            if (DS.Tables[0].Rows.Count != 0)
            {
                foreach (DataRow row in DS.Tables[0].Rows)
                {
                    SupplierReturns GR = new SupplierReturns();

                    GR.Supplier = row["SupplierName"].ToString();
                    GR.Tenant = row["TenantName"].ToString();
                    GR.PartNumber = row["MCode"].ToString();
                    GR.InvoiceNo = row["InvoiceNumber"].ToString();
                    GR.ReturnDate = row["ReturnDate"].ToString();
                    GR.ReturnQty = Convert.ToDecimal(row["ReturnQuantity"].ToString());
                    GR.UoM = row["UoM/Qty"].ToString();


                    lst.Add(GR);
                }
            }
            return lst;
        }

        public class SupplierReturns
        {

            public string Supplier { get; set; }
            public string Tenant { get; set; }
            public string PartNumber { get; set; }
            public string InvoiceNo { get; set; }
            public string ReturnDate { get; set; }
            public decimal ReturnQty { get; set; }
            public string UoM { get; set; }





        }
    }
}