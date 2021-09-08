using ClosedXML.Excel;
using MRLWMSC21Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace FalconWMS.mReports
{
    public partial class LoadSheetReport : System.Web.UI.Page
    {
        public CustomPrincipal cp1 = HttpContext.Current.User as CustomPrincipal;
        public static CustomPrincipal cp;

        protected void Page_PreInit(object sender, EventArgs e)
        {
            cp = HttpContext.Current.User as CustomPrincipal;
            Page.Theme = "Outbound";
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DesignLogic.SetInnerPageSubHeading(this.Page, "Load Sheet Report");
            }

        }

        [WebMethod]
        public static string GetLoadSheetSummaryData(LoadSheetSearch obj, int PageSize, int PageIndex, int TenantID, int OuboundID, int LoadSheetHeaderID)
        {
            cp = HttpContext.Current.User as CustomPrincipal;
            try
            {
                DataSet ds = DB.GetDS("Exec [dbo].[SP_GET_LoadSheetList] @UserID="+cp.UserID+", @TenantID=" + TenantID + ", @OutBoundId=" + obj.OutboundID + ", @LoadSheetID=" + obj.LoadSheetHeaderID + ", @StatusID=" + obj.StatusID + ", @PageSize=" + PageSize + ",@PageIndex=" + PageIndex, false);
                return JsonConvert.SerializeObject(ds);
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        [WebMethod]
        public static string DownloadExcelForLog(int LoadSheetID)
        {
            cp = HttpContext.Current.User as CustomPrincipal;
            try
            {
                string fileName = "LoadSheetReport" + DateTime.Now.Day + "" + DateTime.Now.Month + "" + DateTime.Now.Year + "_" + DateTime.Now.Hour + "" + DateTime.Now.Minute + "" + DateTime.Now.Second;
                DataSet ds = DB.GetDS("Exec [dbo].[SP_GET_OBD_LoadedQty] @LoadHeaderId=" + LoadSheetID, false);
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    //FalconCommon.CommonLogic.ExportExcelDataForReports(ds.Tables[0], fileName, new List<int>(), "Load Sheet Report");
                    MRLWMSC21Common.CommonLogic.ExportExcelData(ds.Tables[0], fileName, new List<int>());

                    return fileName;
                }
                else
                {
                    return "No Data Found";
                }
                //obj.FromDate = obj.FromDate == "" ? "null" : DB.SQuote(obj.FromDate);
                //obj.ToDate = obj.ToDate == "" ? "null" : DB.SQuote(obj.ToDate);
                //string fileName = "";
                //string strUploadPath = System.Web.HttpContext.Current.Server.MapPath("..\\ExcelData");
                //try
                //{
                //    fileName = "IBSummaryReport_" + DateTime.Now.Day + "" + DateTime.Now.Month + "" + DateTime.Now.Year + "_" + DateTime.Now.Hour + "" + DateTime.Now.Minute + "" + DateTime.Now.Second;
                //    DataSet ds = DB.GetDS("Exec [dbo].[USP_RPT_InboundSummaryReport_New] @tenantid=" + obj.tenantid + ", @Warehouseid=" + obj.Warehouseid + ", @IsForExcel=1,@FromDate=" + obj.FromDate + ",@ToDate=" + obj.ToDate + ",@InboundId=" + obj.InboundID, false);

                //    // FalconCommon.ExcellCommon.ExportCyclecountDetailsData(ds.Tables[0], fileName, new List<int>());
                //    using (XLWorkbook wb = new XLWorkbook())
                //    {
                //        wb.Worksheets.Add(ds);
                //        wb.SaveAs(strUploadPath + "\\" + fileName+".xlsx");
                //    }

                //}
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [WebMethod]
        public static string GetLoadSheetData(int LoadSheetID)
        {
            cp = HttpContext.Current.User as CustomPrincipal;
            try
            {
                DataSet ds = DB.GetDS("Exec [dbo].[SP_GET_LoadSheetDetails] @LoadHeaderId=" + LoadSheetID, false);
                return JsonConvert.SerializeObject(ds);
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        

        public class LoadSheetSearch
        {
            public int OutboundID { get; set; }
            public int tenantid { get; set; }
            public int LoadSheetHeaderID { get; set; }
            public int StatusID { get; set; }
        }
    }
}