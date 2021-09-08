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
    public partial class HULiveStock : System.Web.UI.Page
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
                DesignLogic.SetInnerPageSubHeading(this.Page, "Inventory / HU Live Stock");
            }
        }

        [WebMethod]
        public static string getHUStockDetails(string WHID, string TenantID, string MID, string BatchNo, string SerialNo, string MfgDate, string ExpDate, string ProjectRefNo, string MRP, string ReportType, string PageNo, string PageSize)
        {
            try {
                DataSet ds = DB.GetDS("EXEC [dbo].[SP_GET_HU_LiveStock_Report] @WareHouseID=" + WHID + ",@TenantID=" + TenantID + ",@MaterialMasterID=" + MID + ",@BatchNo='" + BatchNo + "',@SerialNo='" + SerialNo + "',@MfgDate='" + MfgDate + "',@ExpDate='" + ExpDate + "',@ProjectRefNo='" + ProjectRefNo + "',@MRP='" + MRP + "',@ReportTypeID=" + ReportType + ",@PageNo=" + PageNo + ",@PageSize=" + PageSize, false);
                return JsonConvert.SerializeObject(ds);
            }
            catch (Exception ex)
            {
                return "Error" + ex;
            }
        }

        [WebMethod]
        public static string getHUStockDetails_Export(string WHID, string TenantID, string MID, string BatchNo, string SerialNo, string MfgDate, string ExpDate, string ProjectRefNo, string MRP, string ReportType, string PageNo, string PageSize)
        {
            try
            {
                //string fileName = "OutboundSummary_" + DateTime.Now.Day + "" + DateTime.Now.Month + "" + DateTime.Now.Year + "_" + DateTime.Now.Hour + "" + DateTime.Now.Minute + "" + DateTime.Now.Second;
                //DataSet ds = DB.GetDS("[sp_RPT_GetOutboundSummaryReport] @TenantID=" + TenantID + ",@WarehouseID=" + WarehouseID + ",@CustomerID=" + Customerid + ",@OutboundID=" + Outboundid + ", @IsExcel=1,@FromDate=" + fromDateTime + ", @ToDate=" + toDateTime + ",@PageSize=200000,@PageIndex=1,@SOHeaderId=" + SOHID + ",@LoadSheetID=" + LoadSHID, false);
                //if (ds != null && ds.Tables[0].Rows.Count > 0)
                //{
                //    MRLWMSC21Common.CommonLogic.ExportExcelDataForReports(ds.Tables[0], fileName, new List<int>(), "Outbound Summary Report");
                //    return fileName;
                //}
                //else
                //{
                //    return "No Data Found";
                //}
                string fileName = "HULiveStock_" + DateTime.Now.Day + "" + DateTime.Now.Month + "" + DateTime.Now.Year + "_" + DateTime.Now.Hour + "" + DateTime.Now.Minute + "" + DateTime.Now.Second;
                DataSet ds = DB.GetDS("EXEC [dbo].[SP_GET_HU_LiveStock_Report] @WareHouseID=" + WHID + ",@TenantID=" + TenantID + ",@MaterialMasterID=" + MID + ",@BatchNo='" + BatchNo + "',@SerialNo='" + SerialNo + "',@MfgDate='" + MfgDate + "',@ExpDate='" + ExpDate + "',@ProjectRefNo='" + ProjectRefNo + "',@MRP='" + MRP + "',@ReportTypeID=" + ReportType + ",@PageNo=" + PageNo + ",@PageSize=" + PageSize, false);
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    DataTable dt = new DataTable();
                    dt = ds.Tables[0];
                    dt.Columns.Remove("SYS_SlocID");
                    dt.Columns.Remove("RID");
                    dt.Columns.Remove("TotalRecords");
                    //ds.Tables.Add(dt);
                    MRLWMSC21Common.CommonLogic.ExportExcelDataForReports(dt, fileName, new List<int>(), "HU Live Stock Report");
                    return fileName;
                }
                else
                {
                    return "No Data Found";
                }
            }
            catch (Exception ex)
            {
                return "Error" + ex;
            }
        }
    }
}