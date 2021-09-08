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
    public partial class Default : System.Web.UI.Page
    {
        public CustomPrincipal cp = HttpContext.Current.User as CustomPrincipal;
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        [WebMethod]
        public static string GetPercentageReportData(int WareHouseID)
        {
            try
            {
                DataSet ds = DB.GetDS("[dbo].[sp_KPI_Dashboard] @WAREHOUSEID=" + WareHouseID, false);
                return JsonConvert.SerializeObject(ds);
            }
            catch (Exception ex)
            {
                return "Error : " + ex;
            }
        }

        [WebMethod]
        public static string GetWarehouses(int AccountID)
        {
            try
            {
                DataSet ds = DB.GetDS("SELECT TOP 1 WarehouseID FROM GEN_Warehouse WHERE IsActive=1 AND IsDeleted=0 AND AccountID = " + AccountID, false);
                return JsonConvert.SerializeObject(ds);
            }
            catch (Exception ex)
            {
                return "Error : " + ex;
            }
        }

    }
}