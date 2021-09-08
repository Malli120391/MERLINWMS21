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
    public partial class PerformanceReport : System.Web.UI.Page
    {

        protected void Page_PreInit(object sender, EventArgs e)
        {
            Page.Theme = "Outbound";
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DesignLogic.SetInnerPageSubHeading(this.Page, "Pick Performance Report");
            }

        }




        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
       
        public static List<Pick_Performance> GetPerformanceList(string FromDate, int TenantId, int WareHouse,string ToDate)
        {
            FromDate = FromDate == "" ? "null" : FromDate;
            ToDate = ToDate == "" ? "null" : ToDate;

            List<Pick_Performance> GetReport = new List<Pick_Performance>();
            GetReport = new PerformanceReport().GetPerformanceData(FromDate, TenantId, WareHouse, ToDate);
            return GetReport;
        }


        
        private List<Pick_Performance> GetPerformanceData(string FromDate, int TenantId, int WareHouse,string ToDate)
        {
            if (FromDate != "" && FromDate != "null")
            {
              //  FromDate = DateTime.ParseExact(FromDate.Replace("-", "/"), "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");
                FromDate = DateTime.ParseExact(FromDate, "dd-MMM-yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");
            }
            if (ToDate != "" && ToDate != "null")
            {
                //ToDate = DateTime.ParseExact(ToDate.Replace("-", "/"), "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");
                ToDate = DateTime.ParseExact(ToDate, "dd-MMM-yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");
            }

            FromDate = FromDate == "null" ? FromDate : DB.SQuote(FromDate);
            ToDate = ToDate == "null" ? ToDate : DB.SQuote(ToDate);

            List<Pick_Performance> lst = new List<Pick_Performance>();
            string Query = "EXEC [dbo].[sp_OBD_GETPickedPerformence] @FromDate=" + FromDate + ", @ToDate=" + ToDate + ", @TenantId = " + TenantId+ ", @WareHouseId = "+WareHouse+"";
            DataSet DS = DB.GetDS(Query, false);
            if (DS.Tables[0].Rows.Count != 0)
            {
                foreach (DataRow row in DS.Tables[0].Rows)
                {
                    Pick_Performance OR = new Pick_Performance();
                    OR.PickingLines = row["PickingLines"].ToString();                   
                    OR.WHCode = row["WHCode"].ToString();
                    OR.LocationZoneCode = row["LocationZoneCode"].ToString();
                    OR.UserName = row["UserName"].ToString();
                    OR.PickedOn = row["PickedOn"].ToString();
                    OR.PickingPerformence = row["PickingPerformence"].ToString();
                    lst.Add(OR);
                }
            }
            return lst;
        }

        public class Pick_Performance
        {
            public string PickingLines { get; set; }
            public string PickingPerformence { get; set; }
            public string WHCode { get; set; }
            public string LocationZoneCode { get; set; }
            public string UserName { get; set; }
            public string PickedOn { get; set; }


        }
    }
}