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

namespace MRLWMSC21.mReports
{
    public partial class CustomerReturnsNew : System.Web.UI.Page
    {
        public static CustomPrincipal cp = HttpContext.Current.User as CustomPrincipal;
        protected void Page_PreInit(object sender, EventArgs e)
        {
            Page.Theme = "Outbound";
            cp = HttpContext.Current.User as CustomPrincipal;
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DesignLogic.SetInnerPageSubHeading(this.Page, "Returns / Customer Returns Report");
            }
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static List<CustomerReturns> GetBillingReportList(string FromDate, string ToDate, int TenantId, int WareHouse)
        {
            FromDate = FromDate == "" ? "null" : FromDate;
            ToDate = ToDate == "" ? "null" : ToDate;

            List<CustomerReturns> GetBillingReport = new List<CustomerReturns>();
            GetBillingReport = new CustomerReturnsNew().GetGITReport(FromDate, ToDate,TenantId,WareHouse);
            return GetBillingReport;
        }

        private List<CustomerReturns> GetGITReport(string FromDate, string ToDate, int TenantId, int WareHouse)
        {
            cp = HttpContext.Current.User as CustomPrincipal;
            if (FromDate==null || FromDate == "null")
            {
                FromDate = "";
            }
            else if (FromDate != "")
            {
                FromDate = DateTime.ParseExact(FromDate, "dd-MMM-yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");
            }
            if (ToDate == null || ToDate == "null")
            {
                ToDate = "";
            }
            else if(ToDate != "")
            {
                ToDate = DateTime.ParseExact(ToDate, "dd-MMM-yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");
            }

            FromDate = FromDate == "null" ? FromDate : DB.SQuote(FromDate);
            ToDate = ToDate == "null" ? ToDate : DB.SQuote(ToDate);


            List<CustomerReturns> lst = new List<CustomerReturns>();
            string Query = " EXEC [sp_RPT_CustomerReturns] @FromDate = " + FromDate + ",  @ToDate = " + ToDate + ",@AccountID_New=" + cp.AccountID + ",@TenantID_New=" +TenantId+ ", @WareHouseId = "+WareHouse+"";

            DataSet DS = DB.GetDS(Query, false);
            if (DS.Tables[0].Rows.Count != 0)
            {
                foreach (DataRow row in DS.Tables[0].Rows)
                {
                    CustomerReturns GR = new CustomerReturns();

                    GR.Customer = row["CustomerName"].ToString();
                    GR.PartNumber = row["MCode"].ToString();
                    GR.SONumber = row["SONumber"].ToString();
                    GR.ReturnDate = row["ReturnDate"].ToString();
                    GR.ReturnQty = Convert.ToDecimal(row["ReturnQuantity"].ToString());
                    GR.UoM = row["UoM/Qty"].ToString();


                    lst.Add(GR);
                }
            }
            return lst;
        }

        public class CustomerReturns
        {

            public string Customer { get; set; }
           
            public string PartNumber { get; set; }
            public string SONumber { get; set; }
            public string ReturnDate { get; set; }
            public decimal ReturnQty { get; set; }
            public string UoM { get; set; }





        }
    
    }
}