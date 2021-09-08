using MRLWMSC21Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MRLWMSC21.mReports
{
    public partial class WarehouseOccupancyReport : System.Web.UI.Page
    {
        protected void Page_PreInit(object sender, EventArgs e)
        {
            Page.Theme = "Outbound";
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DesignLogic.SetInnerPageSubHeading(this.Page, "WAREHOUSE OCCUPANCY REPORT");
            }
        }
     


        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static List<TotalWarehouseOccupancy> GetTotalOccupancyList(int WHID, string date)
        {

            List<TotalWarehouseOccupancy> GetWHOccupReport = new List<TotalWarehouseOccupancy>();
            GetWHOccupReport = new WarehouseOccupancyReport().GetTotalVolumes(WHID, date);
            return GetWHOccupReport;
        }
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static List<WarehouseOccupancy> GetWarehouseOccupancyList(int WHID, string date)
        {

            List<WarehouseOccupancy> GetWHOccupReport = new List<WarehouseOccupancy>();
            GetWHOccupReport = new WarehouseOccupancyReport().GetWHOCUReport(WHID, date);
            return GetWHOccupReport;
        }
        private List<WarehouseOccupancy> GetWHOCUReport(int WHID, string date)
        {
            List<WarehouseOccupancy> lst = new List<WarehouseOccupancy>();
            //string Query = " EXEC [sp_RPT_GetLogAuditData] @UserID= " + PartNO + ", @FromDate = " + DB.SQuote(MaterialType) + ", @ToDate = " + DB.SQuote(ToDate) + "";
            string Query = " EXEC [dbo].[sp_RPT_WarehouseOccupancyReport] @WarehouseID=" + WHID + ", @Date = '" + date + "'";

            DataSet DS = DB.GetDS(Query, false);
            if (DS.Tables[0].Rows.Count != 0)
            {
                foreach (DataRow row in DS.Tables[0].Rows)
                {
                    WarehouseOccupancy WO = new WarehouseOccupancy();

                    WO.Tenant = row["TenantName"].ToString();
                    WO.AvailableQty = string.IsNullOrEmpty(row["AvailableQty"].ToString()) ? 0 : Convert.ToDecimal(row["AvailableQty"].ToString());
                    WO.TotalVolume = string.IsNullOrEmpty(row["TotalVolume"].ToString()) ? 0 : Convert.ToDecimal(row["TotalVolume"].ToString());
                    WO.Occupancy = string.IsNullOrEmpty(row["Occupancy"].ToString()) ? 0 : Convert.ToDecimal(row["Occupancy"].ToString());
                   // WO.TotalWarehouseOccupancy = GetTotalVolumes(WHID, date);
                    lst.Add(WO);
                }
            }
            return lst;
        }
        public List<TotalWarehouseOccupancy> GetTotalVolumes(int WHID, string date)
        {
            List<TotalWarehouseOccupancy> lst = new List<TotalWarehouseOccupancy>();
            try
            {
                string Query = " EXEC [dbo].[sp_RPT_WarehouseOccupancyReport] @WarehouseID=" + WHID + ", @Date = '" + date + "'";
                DataSet DS = DB.GetDS(Query, false);
                if (DS.Tables.Count != 0 && DS.Tables[1].Rows.Count != 0)
                {
                    foreach (DataRow row in DS.Tables[1].Rows)
                    {

                        TotalWarehouseOccupancy TWO = new TotalWarehouseOccupancy();
                        TWO.TotalWarehouseVolume = Convert.ToDecimal(row["WarehouseVolume"]);
                        TWO.TotalOccupiedVolume = Convert.ToDecimal(row["OccupiedVolume"].ToString());
                        TWO.TotalAvailableVolume = Convert.ToDecimal(row["AvailableVolume"].ToString());
                    
                        lst.Add(TWO);
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return lst;
        }

        public class WarehouseOccupancy
        {
            public string Tenant { get; set; }
            public Decimal AvailableQty { get; set; }
            public Decimal TotalVolume { get; set; }
            public Decimal Occupancy { get; set; }
            //public List<TotalWarehouseOccupancy> TotalWarehouseOccupancy{ get; set; }
        }

        public class TotalWarehouseOccupancy
        {
            public Decimal TotalWarehouseVolume { get; set; }
            public Decimal TotalOccupiedVolume { get; set; }
            public Decimal TotalAvailableVolume { get; set; }
         
        }
    }
}