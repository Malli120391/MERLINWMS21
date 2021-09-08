using MRLWMSC21Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MRLWMSC21.mReports
{
    public partial class WeeklyBillingReport : System.Web.UI.Page
    {
        protected void Page_PreInit(object sender, EventArgs e)
        {
            Page.Theme = "Outbound";
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DesignLogic.SetInnerPageSubHeading(this.Page, "Billing Report");
            }
        }


        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        //public static List<WeeklyBilling> GetBillingReportList(string TenantId, string FromDate, string ToDate)
        public static FullList GetBillingReportList(string TenantId, string FromDate, string ToDate)

        {
            FullList l = new FullList();
            l = new WeeklyBillingReport().GetBillngRPT(TenantId, FromDate, ToDate);
            return l;

            //List<WeeklyBilling> GetBillingReport = new List<WeeklyBilling>();
            //GetBillingReport = new WeeklyBillingReport().GetBillngRPT(TenantId, FromDate, ToDate);
            //return GetBillingReport;
        }

        //private List<WeeklyBilling> GetBillngRPT(string TenantId, string FromDate, string ToDate)
        private FullList GetBillngRPT(string TenantId, string FromDate, string ToDate)
        {
            CustomPrincipal customprinciple = HttpContext.Current.User as CustomPrincipal;
            if (FromDate != "" && FromDate != "null")
            {
                //FromDate = DateTime.ParseExact(FromDate.Replace("-", "/"), "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");
                //Lines added by meena 
                DateTime str1 = Convert.ToDateTime(FromDate);
                FromDate = str1.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            }
            if (ToDate != "" && ToDate != "null")
            { 
                //Lines added by meena 
                DateTime str2 = Convert.ToDateTime(ToDate);
                ToDate = str2.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);

                //ToDate = DateTime.ParseExact(ToDate.Replace("-", "/"), "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");
            }

            FromDate = FromDate == "null" ? FromDate : DB.SQuote(FromDate);
            ToDate = ToDate == "null" ? ToDate : DB.SQuote(ToDate);


            decimal ToatalCharge = 0;
            decimal columncharge1 = 0;
            List<WeeklyBilling> lst = new List<WeeklyBilling>();
            // string Query = "EXEC [sp_RPT_GetOutboundDeliveries] @DocumentTypeID = " + DocumenttypeID + ", @DeliveryStatusID=" + DeliverystatusID + ", @FromDate = " + FromDate + ", @ToDate = " + ToDate + "";
            string Query = "[dbo].[SP_RPT_TPL_Weekly_BillingInfo_WeekCharge_Consolidated] @TenantID=" + TenantId + ",@StartDate=" + FromDate + ",@EndDate=" + ToDate + "";
            //EXEC [sp_RPT_GetOutboundDeliveries] @DocumentTypeID=null, @DeliveryStatusID=5, @FromDate=null, @ToDate=null
            DataSet DS = DB.GetDS(Query, false);
            if (DS.Tables[1].Rows.Count != 0)
            {
                foreach (DataRow row in DS.Tables[1].Rows)
                {
                    WeeklyBilling BR = new WeeklyBilling();
                    BR.Week = row["Week"].ToString();
                    BR.Charge = row["Charge"].ToString();
                    BR.QTY = row["QTY"].ToString();
                    BR.UnitCost = row["UnitCost"].ToString();
                    columncharge1 += Convert.ToDecimal(row["Charge1"].ToString());
                    ToatalCharge += Convert.ToDecimal(row["Charge1"].ToString());


                    lst.Add(BR);
                }


            }
            decimal ColumnTotal = 0;
            List<WeeklyBilling> lst1 = new List<WeeklyBilling>();
            string query = "[dbo].[SP_RPT_TPL_Weekly_BillingInfo_WeekCharge_Consolidated] @TenantID=" + TenantId + ",@StartDate=" + FromDate + ",@EndDate=" + ToDate + "";
            //EXEC [sp_RPT_GetOutboundDeliveries] @DocumentTypeID=null, @DeliveryStatusID=5, @FromDate=null, @ToDate=null
            DataSet dS = DB.GetDS(query, false);
            if (dS.Tables[0].Rows.Count != 0)
            {
                foreach (DataRow row in dS.Tables[0].Rows)
                {
                    WeeklyBilling BR = new WeeklyBilling();
                    BR.Week = row["Week"].ToString();
                    BR.Charge = row["Charge"].ToString();                    
                    ColumnTotal += Convert.ToDecimal(row["Charge1"].ToString());
                    ToatalCharge += Convert.ToDecimal(row["Charge1"].ToString());
                    lst1.Add(BR);
                }
            }

            List<WeeklyBilling> lst2 = new List<WeeklyBilling>();
            string _query = "[dbo].[SP_RPT_TPL_Weekly_BillingInfo_WeekCharge_Consolidated] @TenantID=" + TenantId + ",@StartDate=" + FromDate + ",@EndDate=" + ToDate + "";
            //EXEC [sp_RPT_GetOutboundDeliveries] @DocumentTypeID=null, @DeliveryStatusID=5, @FromDate=null, @ToDate=null
            DataSet _dS = DB.GetDS(_query, false);
            if (_dS.Tables[2].Rows.Count != 0)
            {
                foreach (DataRow row in DS.Tables[2].Rows)
                {
                    WeeklyBilling BR = new WeeklyBilling();
                    BR.Week = row["Week"].ToString();
                    BR.Charge = row["Charge"].ToString();
                    ToatalCharge += Convert.ToDecimal(row["Charge1"].ToString());
                    lst2.Add(BR);
                }
            }

            List<WeeklyBilling> lst3 = new List<WeeklyBilling>();
            string _Query = "select te.TenantName,ta.Address1,ta.Address2,ta.City,ta.State,ta.ZIP from TPL_Tenant_AddressBook  ta join TPL_Tenant te on ta.TenantID = te.TenantID where AddressBookTypeID = 2 and te.TenantID = " +TenantId;
            //EXEC [sp_RPT_GetOutboundDeliveries] @DocumentTypeID=null, @DeliveryStatusID=5, @FromDate=null, @ToDate=null
            DataSet _ds = DB.GetDS(_Query, false);
            if (_ds.Tables[0].Rows.Count != 0)
            {
                foreach (DataRow row in _ds.Tables[0].Rows)
                {
                    WeeklyBilling BR = new WeeklyBilling();
                    BR.TenantName = row["TenantName"].ToString();
                    BR.Address1 = row["Address1"].ToString();
                    BR.Address2 = row["Address2"].ToString();
                    BR.City = row["City"].ToString();
                    BR.State = row["State"].ToString();
                    BR.ZIP = row["ZIP"].ToString();
                    
                    lst3.Add(BR);
                }
            }
            List<WeeklyBilling> lst4 = new List<WeeklyBilling>();
            string QUERY = "select * from GEN_Account where AccountID="+customprinciple.AccountID;
            DataSet _DS = DB.GetDS(QUERY, false);
            if (_DS.Tables[0].Rows.Count != 0)
            {
                foreach (DataRow row in _DS.Tables[0].Rows)
                {
                    WeeklyBilling br = new WeeklyBilling();
                    br.Account = row["Account"].ToString();
                    br.ComapanyName = row["CompanyLegalName"].ToString();
                    lst4.Add(br);

                }
            }

            FullList l = new FullList();
            l.one = lst;
            l.two = lst1;
            l.three = lst2;
            l.four = lst3;
            l.five = lst4;
            l.TotalCharge = ToatalCharge;
            l.ColumnTotal = ColumnTotal;
            l.Column = columncharge1;
            return l;
        }

        public class WeeklyBilling
        {
            public string Week { get; set; }
            public string Charge { get; set; }
            public string TenantName { get; set; }
            public string Address1 { get; set; }
            public string Address2 { get; set; }
            public string City { get; set; }
            public string State { get; set; }
            public string ZIP { get; set; }
            public string Account { get; set; }
            public string ComapanyName { get; set; }
            public string QTY { get; set; }
            public string UnitCost { get; set; }


        }

        public class FullList
        {
            public List<WeeklyBilling> one { get; set; }
            public List<WeeklyBilling> two { get; set; }
            public List<WeeklyBilling> three { get; set; }
            public List<WeeklyBilling> four { get; set; }
            public List<WeeklyBilling> five { get; set; }
            public decimal TotalCharge { get; set; }
            public decimal ColumnTotal { get; set; }
            public decimal Column { get; set; }
        }



        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static List<MRLWMSC21.mReports.GetDataListModel.Tenant> GetTenants(string tenant)
        {
            try
            {
                List<MRLWMSC21.mReports.GetDataListModel.Tenant> lstTenant = new List<GetDataListModel.Tenant>();
                GetDataListBL data = new GetDataListBL();
                lstTenant = data.GetTenant(tenant);
                return lstTenant;
            }
            catch (Exception ex)
            {
                return null;
            }
        }



    }
}