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

namespace MRLWMSC21.mReports
{
    public partial class IBSummaryReport : System.Web.UI.Page
    {
        public CustomPrincipal cp1 = HttpContext.Current.User as CustomPrincipal;
        public static CustomPrincipal cp;

        protected void Page_PreInit(object sender, EventArgs e)
        {
            cp = HttpContext.Current.User as CustomPrincipal;
            Page.Theme = "Inbound";
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DesignLogic.SetInnerPageSubHeading(this.Page, "IB Summary");
            }

        }

        [WebMethod]
        public static string GetIBLOGSummaryData(IBLOGSummarySearch obj, int PageSize,int PageIndex,int TenantID, int WarehouseID, DateTime FromDate, DateTime ToDate,string StoreRefNo)
        {
            obj.FromDate = obj.FromDate == "" ? "null" : DB.SQuote(obj.FromDate);
            obj.ToDate = obj.ToDate == "" ? "null" : DB.SQuote(obj.ToDate);
            try
            {
                DataSet ds = DB.GetDS("Exec [dbo].[USP_RPT_InboundSummaryReport_New] @InboundId="+obj.InboundID + ",@tenantid="+ TenantID + ",@Warehouseid="+ WarehouseID + ",@FromDate=" + obj.FromDate + ",@ToDate=" + obj.ToDate + ",@PageSize="+ PageSize + ",@PageIndex="+ PageIndex, false);

                return JsonConvert.SerializeObject(ds);
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        [WebMethod]
        public static string DownloadExcelForLog(IBLOGSummarySearch obj)
        {
            try
            {
                obj.FromDate = obj.FromDate == "" ? "null" : DB.SQuote(obj.FromDate);
                obj.ToDate = obj.ToDate == "" ? "null" : DB.SQuote(obj.ToDate);
                string fileName = "InboundSummaryReport" + DateTime.Now.Day + "" + DateTime.Now.Month + "" + DateTime.Now.Year + "_" + DateTime.Now.Hour + "" + DateTime.Now.Minute + "" + DateTime.Now.Second;
                DataSet ds = DB.GetDS("Exec [dbo].[USP_RPT_InboundSummaryReport_New] @tenantid=" + obj.tenantid + ", @Warehouseid=" + obj.Warehouseid + ", @IsForExcel=1,@FromDate=" + obj.FromDate + ",@ToDate=" + obj.ToDate + ",@InboundId=" + obj.InboundID, false);
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    MRLWMSC21Common.CommonLogic.ExportExcelDataForReports(ds.Tables[0], fileName, new List<int>(), "Inbound Summary Report");
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

                //    // MRLWMSC21Common.ExcellCommon.ExportCyclecountDetailsData(ds.Tables[0], fileName, new List<int>());
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

    
        public class IBLOGSummarySearch
        {
            public string VehicleId { get; set; }
            public string InvoiceNo { get; set; }
            public string FromDate { get; set; }
            public string ToDate { get; set; }
            public int InboundID { get; set; }
            public int tenantid { get; set; }
            public int Warehouseid { get; set; }
        }
    }
}