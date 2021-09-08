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
    public partial class DemandForecastReportNew : System.Web.UI.Page
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
                DesignLogic.SetInnerPageSubHeading(this.Page, "Inventory / Demand Forecast Report");
            }
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static List<DemandForecastReport> GetBillingReportList()
        {
            List<DemandForecastReport> GetBillingReport = new List<DemandForecastReport>();
            GetBillingReport = new DemandForecastReportNew().GetBillngRPT();
            return GetBillingReport;
        }

        private List<DemandForecastReport> GetBillngRPT()
        {


            List<DemandForecastReport> lst = new List<DemandForecastReport>();
            string Query = "EXEC [sp_RPT_DemandForecastReport] @AccountID_New=" + cp.AccountID + ",@TenantID_New=" + cp.TenantID;
            DataSet DS = DB.GetDS(Query, false);
            if (DS.Tables[0].Rows.Count != 0)
            {
                foreach (DataRow row in DS.Tables[0].Rows)
                {
                    DemandForecastReport OR = new DemandForecastReport();
                    OR.MaterialCode = row["MCode"].ToString();
                    OR.AvailableQty = row["AvalQty"].ToString();
                    OR.ReorderPoint = row["ReorderPoint"].ToString();
                    OR.PlannedDeliveryTime = row["PlannedDeliveryTime"].ToString();
                    OR.M1 = row["1"].ToString();
                    OR.M2 = row["2"].ToString();
                    OR.M3 = row["3"].ToString();
                    OR.M4 = row["4"].ToString();
                    lst.Add(OR);
                }
            }
            return lst;
        }

        public class DemandForecastReport
        {
            public string MaterialCode { get; set; }
            public string AvailableQty { get; set; }
            public string ReorderPoint { get; set; }
            public string PlannedDeliveryTime { get; set; }
            public string M1 { get; set; }
            public string M2 { get; set; }
            public string M3 { get; set; }
            public string M4 { get; set; }





        }
    }
}