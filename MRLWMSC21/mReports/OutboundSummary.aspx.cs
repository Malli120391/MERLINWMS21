using ClosedXML.Excel;
using MRLWMSC21Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MRLWMSC21.mReports
{
    public partial class OutboundSummary : System.Web.UI.Page
    {
        public CustomPrincipal cp;
        public static CustomPrincipal cp1;
        protected void Page_Load(object sender, EventArgs e)
        {
            cp = (CustomPrincipal)HttpContext.Current.User;
            cp1 = (CustomPrincipal)HttpContext.Current.User;
            if (!IsPostBack)
            {
                DesignLogic.SetInnerPageSubHeading(this.Page, "Outbound Summary");
            }
        }
        [WebMethod]
        public static string getOutBoundSummary(string FromDate, string ToDate, int PageSize, int PageIndex, int Customerid, int Outboundid, int TenantID, int WarehouseID, string FromTime, string ToTime, string LoadSHID, string SOHID)
        {
           cp1 = HttpContext.Current.User as CustomPrincipal;
            try
            {
                FromDate = FromDate == "" ? "NULL" : FromDate;
                ToDate = ToDate == "" ? "NULL" : ToDate;
                FromTime = FromTime == "" ? "NULL" : FromTime;
                ToTime = ToTime == "" ? "NULL" : ToTime;

                string fromDateTime = ""; string toDateTime = "";

                DateTime dt = DateTime.ParseExact(FromDate + " " + FromTime, "dd-MMM-yyyy hh:mm tt", CultureInfo.InvariantCulture);
                DateTime dt1 = DateTime.ParseExact(ToDate + " " + ToTime, "dd-MMM-yyyy hh:mm tt", CultureInfo.InvariantCulture);
                int dtm = Convert.ToInt32(Math.Abs((dt - dt1).TotalDays));
                if (dtm > 7)
                {
                    return "0";
                }
                fromDateTime = DB.SQuote(DateTime.ParseExact(FromDate + " " + FromTime, "dd-MMM-yyyy hh:mm tt", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy hh:mm tt"));
                toDateTime = DB.SQuote(DateTime.ParseExact(ToDate + " " + ToTime, "dd-MMM-yyyy hh:mm tt", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy hh:mm tt"));

                var obj = cp1;
                //DataSet ds = DB.GetDS("[sp_RPT_GetOutboundSummaryReport] @TenantID=" + TenantID + ",@WarehouseID=" + WarehouseID + ", @FromDate=" + fromDateTime + ", @ToDate=" + toDateTime + ", @CustomerID=" + Customerid + ",@OutboundID=" + Outboundid + ",@PageSize=" + PageSize + ",@PageIndex=" + PageIndex, false);
                DataSet ds = DB.GetDS("[sp_RPT_GetOutboundSummaryReport] @TenantID=" + TenantID + ",@WarehouseID=" + WarehouseID + ", @FromDate=" + fromDateTime + ", @ToDate=" + toDateTime + ", @CustomerID=" + Customerid + ",@OutboundID=" + Outboundid + ",@PageSize=" + PageSize + ",@PageIndex=" + PageIndex + ",@SOHeaderId=" + SOHID + ",@LoadSheetID=" + LoadSHID, false);
                return JsonConvert.SerializeObject(ds);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [WebMethod]
        public static string DownLoadExcelDataForItems(string FromDate, string ToDate, int Customerid, int Outboundid, int TenantID, int WarehouseID, string FromTime, string ToTime, string LoadSHID, string SOHID)
        {
            // int result = 0;
            string strUploadPath = System.Web.HttpContext.Current.Server.MapPath("..\\ExcelData");
            try
            {
                FromDate = FromDate == "" ? "NULL" : FromDate;
                ToDate = ToDate == "" ? "NULL" : ToDate;
                FromTime = FromTime == "" ? "NULL" : FromTime;
                ToTime = ToTime == "" ? "NULL" : ToTime;
                cp1 = HttpContext.Current.User as CustomPrincipal;
                string fromDateTime = ""; string toDateTime = "";

                DateTime dt = DateTime.ParseExact(FromDate + " " + FromTime, "dd-MMM-yyyy hh:mm tt", CultureInfo.InvariantCulture);
                DateTime dt1 = DateTime.ParseExact(ToDate + " " + ToTime, "dd-MMM-yyyy hh:mm tt", CultureInfo.InvariantCulture);
                int dtm = Convert.ToInt32(Math.Abs((dt - dt1).TotalDays));
                if (dtm > 7)
                {
                    return "0";
                }

                fromDateTime = DB.SQuote(DateTime.ParseExact(FromDate + " " + FromTime, "dd-MMM-yyyy hh:mm tt", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy hh:mm tt"));
                toDateTime = DB.SQuote(DateTime.ParseExact(ToDate + " " + ToTime, "dd-MMM-yyyy hh:mm tt", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy hh:mm tt"));

                string fileName = "OutboundSummary_" + DateTime.Now.Day + "" + DateTime.Now.Month + "" + DateTime.Now.Year + "_" + DateTime.Now.Hour + "" + DateTime.Now.Minute + "" + DateTime.Now.Second;
                DataSet ds = DB.GetDS("[sp_RPT_GetOutboundSummaryReport] @TenantID=" + TenantID + ",@WarehouseID=" + WarehouseID + ",@CustomerID=" + Customerid + ",@OutboundID=" + Outboundid + ", @IsExcel=1,@FromDate=" + fromDateTime + ", @ToDate=" + toDateTime + ",@PageSize=200000,@PageIndex=1,@SOHeaderId=" + SOHID + ",@LoadSheetID=" + LoadSHID, false);
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    MRLWMSC21Common.CommonLogic.ExportExcelDataForReports(ds.Tables[0], fileName, new List<int>(), "Outbound Summary Report");
                    return fileName;
                }
                else
                {
                    return "No Data Found";
                }
                // MRLWMSC21Common.ExcellCommon.ExportCyclecountDetailsData(ds.Tables[0], fileName, new List<int>());
                //using (XLWorkbook wb = new XLWorkbook())
                //{
                //    wb.Worksheets.Add(ds);
                //    wb.SaveAs(strUploadPath + "\\" + fileName + ".xlsx");
                //}
                //return fileName;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}