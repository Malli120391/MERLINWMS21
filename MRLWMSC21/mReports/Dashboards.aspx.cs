using MRLWMSC21Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MRLWMSC21.mReports
{
    public partial class Dashboards : System.Web.UI.Page
    {
        public CustomPrincipal cp = HttpContext.Current.User as CustomPrincipal;
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        [WebMethod]
        public static string GetPercentageReportData(int WareHouseID)
        {
            //DataSet ds = DB.GetDS("[dbo].[sp_KPI_AverageDispatchCycleTime]", false);
            //DataSet ds = DB.GetDS("[dbo].[sp_KPI_OrderAccuracyRPT] @WHID=0", false);
            DataSet ds = DB.GetDS("[dbo].[sp_KPI_OrderAccuracyRPT] @WHID="+ WareHouseID, false);
            return JsonConvert.SerializeObject(ds);
        }

    }
}