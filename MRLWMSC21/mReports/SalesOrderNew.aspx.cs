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
    public partial class SalesOrderNew : System.Web.UI.Page
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
                DesignLogic.SetInnerPageSubHeading(this.Page, "Orders / Sales Order Report");
            }
        }
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static List<SalesOrder> GetBillingReportList(string SONumber, string SODate, string StatusId, string TypeID, string FromDate, string ToDate)
        {

            SONumber = SONumber == "" ? "null" :DB.SQuote(SONumber);
            SODate = SODate == "" ? "null" : SODate;
            FromDate = FromDate == "" ? "null" : FromDate;
            ToDate = ToDate == "" ? "null" : ToDate;
           
            StatusId = StatusId == "" ? "null" : StatusId;
            TypeID = TypeID == "" ? "null" : TypeID;
            
            List<SalesOrder> GetBillingReport = new List<SalesOrder>();
            GetBillingReport = new SalesOrderNew().GetBillngRPT(SONumber, SODate, StatusId, TypeID,FromDate, ToDate);
            return GetBillingReport;
        }

        private List<SalesOrder> GetBillngRPT(string SONumber, string SODate, string StatusId, string TypeID, string FromDate, string ToDate)
        {
            if (SODate != "" && SODate != "null")
            {
                //SODate = DateTime.ParseExact(SODate.Replace("-", "/"), "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");
                SODate = DateTime.ParseExact(SODate, "dd-MMM-yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");
            }
            if (FromDate != "" && FromDate != "null")
            {
                //FromDate = DateTime.ParseExact(FromDate.Replace("-", "/"), "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");
                FromDate = DateTime.ParseExact(FromDate, "dd-MMM-yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");
            }
            if (ToDate != "" && ToDate !="null")
            {
               // ToDate = DateTime.ParseExact(ToDate.Replace("-", "/"), "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");
                ToDate = DateTime.ParseExact(ToDate, "dd-MMM-yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");
            }
            SODate = SODate == "null" ? SODate : DB.SQuote(SODate);
            FromDate = FromDate == "null" ? FromDate : DB.SQuote(FromDate);
            ToDate = ToDate == "null" ? ToDate : DB.SQuote(ToDate);

            List<SalesOrder> lst = new List<SalesOrder>();
            string Query = " EXEC [sp_RPT_GetSOData] @SONumber= " + SONumber + ",@SODate= " + SODate + ",@SOStatusID=" + StatusId + ",@SOTypeID=" + TypeID +  ", @FromDate = " + FromDate + ", @ToDate = " + ToDate + ",@AccountID_New=" + cp.AccountID + ",@TenantID_New=" + cp.TenantID;
            DataSet DS = DB.GetDS(Query, false);
            if (DS.Tables[0].Rows.Count != 0)
            {
                foreach (DataRow row in DS.Tables[0].Rows)
                {
                    SalesOrder BR = new SalesOrder();
                    BR.SONumber = row["SONumber"].ToString();
                    BR.SODate = row["SODate"].ToString();
                    BR.Customer = row["CustomerName"].ToString();
                    BR.SOType = row["SOType"].ToString();
                    BR.Currency = row["Code"].ToString();
                    BR.GrossValue = row["GrossValue"].ToString();
                    BR.NetValue = row["NetValue"].ToString();
                    BR.Tax = row["SOTax"].ToString();
                    BR.Status = row["SOStatus"].ToString();
                    lst.Add(BR);
                }
            }
            return lst;
        }


        public class SalesOrder
        {
            public string SONumber { get; set; }
            public string SODate { get; set; }
            public string Customer { get; set; }
            public string SOType { get; set; }
            public string Currency { get; set; }
            public string GrossValue { get; set; }         
            public string NetValue { get; set; }
            public string Tax { get; set; }
            public string Status { get; set; }

        }
    }
}