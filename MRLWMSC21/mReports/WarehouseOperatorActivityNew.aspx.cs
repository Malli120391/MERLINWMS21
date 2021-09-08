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
    public partial class WarehouseOperatorActivityNew : System.Web.UI.Page
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
                DesignLogic.SetInnerPageSubHeading(this.Page, "Warehouse / Operator Activity Report");
            }
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static List<WarehouseOperatorActivity> GetBillingReportList(string FromDate, string ToDate, int TenantId, int WareHouse,int UserID)
        {
            FromDate = FromDate == "" ? "null" : FromDate;
            ToDate = ToDate == "" ? "null" : ToDate;

            List<WarehouseOperatorActivity> GetBillingReport = new List<WarehouseOperatorActivity>();
            GetBillingReport = new WarehouseOperatorActivityNew().GetGITReport(FromDate, ToDate,TenantId,WareHouse, UserID);
            return GetBillingReport;
        }

        private List<WarehouseOperatorActivity> GetGITReport(string FromDate, string ToDate, int TenantId, int WareHouse,int UserID)
        {

            cp = HttpContext.Current.User as CustomPrincipal;
            if (FromDate != "" && FromDate !="null")
            {
                FromDate = DateTime.ParseExact(FromDate.Replace("-", "/"), "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");
            }
            if (ToDate != "" && ToDate !="null")
            {
                ToDate = (DateTime.ParseExact(ToDate.Replace("-", "/"), "dd/MM/yyyy", CultureInfo.InvariantCulture)).AddDays(1).ToString("MM/dd/yyyy");
            }
            if (ToDate == "" || ToDate == "null")
            { ToDate = ""; }

                if (FromDate != "" && ToDate == "")
            {
                ToDate =   DateTime.Now.AddDays(1).ToString("MM/dd/yyyy");
            }
            FromDate =FromDate=="null"?FromDate:DB.SQuote(FromDate);
            ToDate = ToDate == "null" ? ToDate : DB.SQuote(ToDate);
           
            List<WarehouseOperatorActivity> lst = new List<WarehouseOperatorActivity>();
            string Query = "  EXEC [sp_RPT_GetWarehouseOperatorActivity] @FromDate = " + FromDate + ",  @ToDate = " + ToDate + ",@AccountID_New=" + cp.AccountID + ",@TenantID_New=" + TenantId+ ", @WareHouseId  = "+WareHouse+ ",@UserID_New="+ UserID + "";
             

            DataSet DS = DB.GetDS(Query, false);
            if (DS.Tables[0].Rows.Count != 0)
            {
                foreach (DataRow row in DS.Tables[0].Rows)
                {
                    WarehouseOperatorActivity GR = new WarehouseOperatorActivity();

                    GR.Operator = row["FirstName"].ToString();
                    GR.RoleAssigned = row["UserRole"].ToString();
                    GR.Warehouse = row["WHCode"].ToString();
                    GR.TotalLineItems = row["TotalLineItems"].ToString();
                    GR.TotalQuantity = Convert.ToDecimal(row["TotalQuantity"].ToString());
                    GR.GoodsInLineItems = row["GoodsInLineItems"].ToString();
                    GR.GoodsInQuantity = Convert.ToDecimal(row["GoodsInQuantity"].ToString());
                    GR.GoodsOutLineItems = row["GoodsOutLineItems"].ToString();
                    GR.GoodsOutQuantity = row["GoodsOutQuantity"].ToString() == "" ? Convert.ToDecimal("0") : Convert.ToDecimal(row["GoodsOutQuantity"].ToString());


                    lst.Add(GR);
                }
            }
            return lst;
        }

        public class WarehouseOperatorActivity
        {

            public string Operator { get; set; }
            public string RoleAssigned { get; set; }
            public string Warehouse { get; set; }
            public string TotalLineItems { get; set; }
            public decimal TotalQuantity { get; set; }
            public string GoodsInLineItems { get; set; }
            public decimal GoodsInQuantity { get; set; }
            public string GoodsOutLineItems { get; set; }
            public decimal GoodsOutQuantity { get; set; }





        }
    }
}