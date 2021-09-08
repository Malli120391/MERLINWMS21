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
    public partial class RateCardReport : System.Web.UI.Page
    {
        protected void Page_PreInit(object sender, EventArgs e)
        {
            Page.Theme = "Outbound";
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DesignLogic.SetInnerPageSubHeading(this.Page, "3PL Costing Report");
            }
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static FullList GetBillingReportList(string TenantID,string Fromdate, string Todate)
        {
             FullList l = new FullList();
            l = new RateCardReport().GetBillngRPT(TenantID, Fromdate, Todate);
            return l;
        }
        private FullList GetBillngRPT(string TenantID,string Fromdate,string Todate)
        {
            if (Fromdate != "" && Fromdate != "null")
            {
                Fromdate = DateTime.ParseExact(Fromdate.Replace("-", "/"), "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");
            }
            if (Todate != "" && Todate != "null")
            {
                Todate = DateTime.ParseExact(Todate.Replace("-", "/"), "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");
            }

            Fromdate = Fromdate == "null" ? Fromdate : DB.SQuote(Fromdate);
            Todate = Todate == "null" ? Todate : DB.SQuote(Todate);

            List<Rate_Card> lst = new List<Rate_Card>();
            string Query = "[dbo].[sp_RPT_RateCardReport] @TenantID= " + DB.SQuote(TenantID)+ ",@FromDate="+Fromdate+ ",@Todate="+Todate;
            DataSet DS = DB.GetDS(Query, false);
            if (DS.Tables[0].Rows.Count != 0)
            {
                foreach (DataRow row in DS.Tables[0].Rows)
                {
                    Rate_Card OR = new Rate_Card();
                    OR.TenantName = row["TenantName"].ToString();
                    OR.ActivityRateType = row["ActivityRateType"].ToString();
                    OR.ActivityRateName = row["ActivityRateName"].ToString();
                    OR.CostPrice = row["CostPrice"].ToString();
                   
                    lst.Add(OR);
                }
            }
            List<Rate_Card> lst1 = new List<Rate_Card>();
            if (DS.Tables[1].Rows.Count != 0)
            {
                foreach (DataRow row in DS.Tables[1].Rows)
                {
                    Rate_Card OR = new Rate_Card();
                    OR.TenantName = row["TenantName"].ToString();
                    OR.StoreRefNo = row["StoreRefNo"].ToString();
                    OR.ActivityRateName = row["ActivityRateName"].ToString();
                    OR.Quantity = row["Quantity"].ToString();
                    OR.CostPrice = row["UnitCost"].ToString();
                    lst1.Add(OR);
                }
            }

            List<Rate_Card> lst2 = new List<Rate_Card>();
            if (DS.Tables[2].Rows.Count != 0)
            {
                foreach (DataRow row in DS.Tables[2].Rows)
                {
                    Rate_Card OR = new Rate_Card();
                    OR.TenantName = row["TenantName"].ToString();
                    OR.OBDNumber = row["OBDNumber"].ToString();
                    OR.ActivityRateName = row["ActivityRateName"].ToString();
                    OR.Quantity = row["Quantity"].ToString();
                    OR.CostPrice = row["UnitCost"].ToString();
                    lst2.Add(OR);
                }
            }

            FullList l = new FullList();
            l.one = lst;
            l.two = lst1;
            l.three = lst2;

            return l;   
        }

        public class Rate_Card
        {
            public string TenantName { get; set; }
            public string ActivityRateName { get; set; }
            public string CostPrice { get; set; }
            public string ActivityRateType { get; set; }
            public string StoreRefNo { get; set; }
            public string Quantity { get; set; }
            public string OBDNumber { get; set; }
        }

        public class FullList
        {
            public List<Rate_Card> one { get; set; }
            public List<Rate_Card> two { get; set; }
           public List<Rate_Card> three { get; set; }

        }
    }
}