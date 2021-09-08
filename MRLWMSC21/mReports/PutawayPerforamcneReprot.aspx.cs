using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MRLWMSC21Common;
using Newtonsoft.Json;
using System.Web.Script.Services;
using System.Web.Services;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Globalization;

namespace MRLWMSC21.mReports
{
    public partial class PutawayPerforamcneReprot : System.Web.UI.Page
    {

        protected void Page_PreInit(object sender, EventArgs e)
        {
            Page.Theme = "Outbound";
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DesignLogic.SetInnerPageSubHeading(this.Page, "Performance / Putaway Performance Report");
            }
        }


        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        //public static List<Current_Stock_Report> GetBillingReportList(string Tenantid, string Mcode, string Mtypeid, string BatchNo, string Location, string kitId)
        public static List<Put_Away_Report> GetBillingReportList(string FromDate, string ToDate, int TenantId, int WareHouse)
        {
            FromDate = FromDate == "" ? "null" : FromDate;
            ToDate = ToDate == "" ? "null" : ToDate;

            List<Put_Away_Report> GetBillingReport = new List<Put_Away_Report>();
            GetBillingReport = new PutawayPerforamcneReprot().GetBillngRPT(FromDate, ToDate, TenantId, WareHouse);
            return GetBillingReport;
        }


        // public static List<Current_Stock_Report> GetBillngRPT(string Tenantid, string Mcode, string Mtypeid, string BatchNo, string Location, string kitId)
        private List<Put_Away_Report> GetBillngRPT(string FromDate, string ToDate, int TenantId, int WareHouse)
        {
            if (FromDate != "" && FromDate != "null")
            {
                // FromDate = DateTime.ParseExact(FromDate.Replace("-", "/"), "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");
                FromDate = DateTime.ParseExact(FromDate, "dd-MMM-yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");
            }
            if (ToDate != "" && ToDate != "null")
            {
               // ToDate = DateTime.ParseExact(ToDate.Replace("-", "/"), "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");
                ToDate = DateTime.ParseExact(ToDate, "dd-MMM-yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");
            }

            FromDate = FromDate == "null" ? FromDate : DB.SQuote(FromDate);
            ToDate = ToDate == "null" ? ToDate : DB.SQuote(ToDate);

            List<Put_Away_Report> lst = new List<Put_Away_Report>();
            string Query = "EXEC [dbo].[USP_PutawayPerformenceRPT]  @WindowStart=" + FromDate + ",@WindowEnd=" + ToDate + ", @TenantId = "+TenantId+ " , @WareHouseId = "+WareHouse+"";
            DataSet DS = DB.GetDS(Query, false);
            if (DS.Tables[0].Rows.Count != 0)
            {
                foreach (DataRow row in DS.Tables[0].Rows)
                {
                    Put_Away_Report OR = new Put_Away_Report();
                    OR.TotalInwardWorkLines = row["TotalInwardWorkLines"].ToString();
                    OR.WorkItemsCompleted = row["WorkItemsCompleted"].ToString();
                    OR.WorkCompletedPercent = row["WorkCompletedPercent"].ToString();
                    OR.WorkItemsPanding = row["WorkItemsPanding"].ToString();
                    OR.WorkPendingPercent = row["WorkPendingPercent"].ToString();
                    lst.Add(OR);
                }
            }
            return lst;
        }

        public class Put_Away_Report
        {
            public string TotalInwardWorkLines { get; set; }
            public string WorkItemsCompleted { get; set; }
            public string WorkCompletedPercent { get; set; }
            public string WorkItemsPanding { get; set; }
            public string WorkPendingPercent { get; set; }


        }
    }
}