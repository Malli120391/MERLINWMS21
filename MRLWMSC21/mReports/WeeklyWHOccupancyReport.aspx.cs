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
using Newtonsoft.Json;

namespace MRLWMSC21.mReports
{
    public partial class WeeklyWHOccupancyReport : System.Web.UI.Page
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
                DesignLogic.SetInnerPageSubHeading(this.Page, " Inbound / Weekly Warehouse Occupancy Report");
            }
        }

        [WebMethod]
        public static string getWeeklyWHOccReport(string FromDate, string ToDate, string Tenant, string Warehouse)
        {
            try
            {
                DataSet ds = DB.GetDS("EXEC [DBO].[SP_RPT_WeeklyWHOccupancyReport] @TenantId=" + Tenant + ", @StartDate='" + FromDate + "', @EndDate='" + ToDate + "', @WarehouseID=" + Warehouse, false);
                //DataSet ds = DB.GetDS("EXEC [DBO].[SP_RPT_WeeklyWHOccupancyReport] @TenantId=57, @StartDate='2019-11-01', @EndDate='2019-11-30', @WarehouseID=4028", false);
                return JsonConvert.SerializeObject(ds);
            }
            catch (Exception ex)
            {
                return "Error : " + ex;
            }
        }
    }
}