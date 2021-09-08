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
using System.Data.SqlClient;
using System.Configuration;
using System.Text;
using Newtonsoft.Json;  

namespace MRLWMSC21.mReports
{
    public partial class SpaceUtilizationReportNew : System.Web.UI.Page
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
                DesignLogic.SetInnerPageSubHeading(this.Page, "Inventory / Space Utilization Report");
            }
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static List<SpaceUtilizationReport> GetBillingReportList(string warehouseid,string FromDate, string ToDate)
        {
            FromDate = FromDate == "" ? "null" : FromDate;
            ToDate = ToDate == "" ? "null" : ToDate;
            warehouseid = warehouseid == "" ? "0" : warehouseid;

            List<SpaceUtilizationReport> GetBillingReport = new List<SpaceUtilizationReport>();
            GetBillingReport = new SpaceUtilizationReportNew().GetGITReport(warehouseid, FromDate, ToDate);
            return GetBillingReport;
        }


        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string GetSpaceList(string warehouseid, string FromDate, string ToDate)
        {
            DataSet DS = new DataSet();
            DS = DB.GetDS("EXEC [sp_RPT_GetSpaceUtilizationReport_NEW]  @WarehouseID=" + warehouseid + ", @StartDate='" + FromDate + "', @EndDate='" + ToDate + "'", false);

            string JOSON = DataTableToJSONWithJSONNet(DS.Tables[0]);

           return JOSON;
        }

    

        public static string DataTableToJSONWithJSONNet(DataTable table)
        {
            string JSONString = string.Empty;
            JSONString = JsonConvert.SerializeObject(table);
            return JSONString;
        }  

        private List<SpaceUtilizationReport> GetGITReport(string warehouseid, string FromDate, string ToDate)
        {
            if (FromDate != "" && FromDate !="null")
            {
                FromDate = DateTime.ParseExact(FromDate.Replace("-", "/"), "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");
            }
            if (ToDate != "" && ToDate !="null")
            {
                ToDate = DateTime.ParseExact(ToDate.Replace("-", "/"), "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");
            }

            FromDate=FromDate=="null"?FromDate: DB.SQuote(FromDate);
            ToDate = ToDate == "null" ? ToDate : DB.SQuote(ToDate);
            List<SpaceUtilizationReport> lst = new List<SpaceUtilizationReport>();
            string Query = "  EXEC [sp_RPT_GetSpaceUtilizationReport]  @WarehouseID=" + warehouseid + ",  @StartDate= " + FromDate + ",  @EndDate= " + ToDate + ",@AccountID_New=" + cp.AccountID + ",@TenantID_New=" + cp.TenantID;
          

            DataSet DS = DB.GetDS(Query, false);
            if (DS.Tables[0].Rows.Count != 0)
            {
                foreach (DataRow row in DS.Tables[0].Rows)
                {
                    SpaceUtilizationReport GR = new SpaceUtilizationReport();

                    GR.Zone = row["Zone"].ToString();
                    GR.Date = row["MDate"].ToString();
                    GR.UsedSpace = row["UsedSpace"].ToString();
                    


                    lst.Add(GR);
                }
            }
            return lst;
        }

        public class SpaceUtilizationReport
        {

            public string Zone { get; set; }
            public string Date { get; set; }
            public string UsedSpace { get; set; }
            
        }




        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static List<MRLWMSC21.mReports.GetDataListModel.Warehouse> GetWarehouses()
        {
            try
            {
                List<MRLWMSC21.mReports.GetDataListModel.Warehouse> ltswarehouse = new List<MRLWMSC21.mReports.GetDataListModel.Warehouse>();
                GetDataListBL get = new GetDataListBL();
                ltswarehouse = get.Getwarehouse();
                return ltswarehouse;
            }
            catch (Exception ex)
            {

                return null;
            }

        }
    }
}