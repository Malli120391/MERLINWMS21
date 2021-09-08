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

namespace MRLWMSC21.mInbound
{
    public partial class NewInboundSearch : System.Web.UI.Page
    {
        static List<int> lstLoadSheetList = new List<int>();
        public static CustomPrincipal cp1;




        protected void Page_Load(object sender, EventArgs e)
        {
            cp1 = HttpContext.Current.User as CustomPrincipal;
            if (!IsPostBack)
            {
                DesignLogic.SetInnerPageSubHeading(this.Page, "Search Inbound");
            }
        }

        [WebMethod]
        public static string getInboundData(string StartDate, string EndDate, string StoreID, string ShipmentTypeID, int ClearenceCompanyID, string ShipmentStatusID, string SearchText, string SearchField, int PaginationId, int PageSize,string TenantID)
        {
            try
            {
                cp1 = HttpContext.Current.User as CustomPrincipal;
                StartDate = StartDate == "" || StartDate == null ? "null" : DB.SQuote(StartDate);
                EndDate = EndDate == "" || EndDate == null ? "null" : DB.SQuote(EndDate);
                SearchText = SearchText == "" || EndDate == null ? "null" : DB.SQuote(SearchText);

                string query = "EXEC [sp_INB_SearchInbound_New]  @StartDate=" + StartDate + ",@EndDate=" + EndDate + ",@TenantID_New =" + TenantID + ",@UserID =" + cp1.UserID + ",@AccountID_New=" + cp1.AccountID + ",@StoreID ='" + StoreID + "',@ShipmentTypeID=" + ShipmentTypeID + ",@ClearenceCompanyID=" + ClearenceCompanyID + ",@ShipmentStatusID='" + ShipmentStatusID + "', @PageIndex = " + PaginationId + ",@PageSize = " + PageSize + ",@WarehouseID = '" + StoreID + "', @SearchText=" + SearchText + ",@SearchField='" + SearchField + "',@Qty=0,@GrossWeight=0";

                DataSet ds = DB.GetDS(query, false);
                return JsonConvert.SerializeObject(ds);
            }
            catch (Exception ex)
            {
                return "";
            }




        }

        [WebMethod]
        public static string ExcelExport(string StartDate, string EndDate, string StoreID, string ShipmentTypeID, int ClearenceCompanyID, string ShipmentStatusID, string SearchText, string SearchField, string PaginationId, string PageSize)
        {
            try
            {
                cp1 = HttpContext.Current.User as CustomPrincipal;
                StartDate = StartDate == "" || StartDate == null ? "null" : DB.SQuote(StartDate);
                EndDate = EndDate == "" || EndDate == null ? "null" : DB.SQuote(EndDate);
                SearchText = SearchText == "" || SearchText == null ? "null" : DB.SQuote(SearchText);
                PaginationId = PaginationId == "1" || PaginationId == null ? "null" : DB.SQuote(PaginationId);
                PageSize = PageSize == "1" || PageSize == null ? "null" : DB.SQuote(PageSize);



                string fileName = "InboundData" + DateTime.Now.Day + "" + DateTime.Now.Month + "" + DateTime.Now.Year + "_" + DateTime.Now.Hour + "" + DateTime.Now.Minute + "" + DateTime.Now.Second;
                string query = "EXEC [sp_INB_SearchInbound_New]  @StartDate=" + StartDate + ",@EndDate=" + EndDate + ",@TenantID_New =" + cp1.TenantID + ",@UserID =" + cp1.UserID + ",@AccountID_New=" + cp1.AccountID + ",@StoreID ='" + StoreID + "',@ShipmentTypeID=" + ShipmentTypeID + ",@ClearenceCompanyID=" + ClearenceCompanyID + ",@ShipmentStatusID='" + ShipmentStatusID + "', @PageIndex = " + PaginationId + ",@PageSize = " + PageSize + ",@WarehouseID = '" + StoreID + "', @SearchText=" + SearchText + ",@SearchField='" + SearchField + "',@Qty=0,@GrossWeight=0";

                DataSet ds = DB.GetDS(query, false);

                MRLWMSC21Common.CommonLogic.ExportExcelDataForReports(ds.Tables[1], fileName, new List<int>(), "Search Inbound");
                return fileName;
            }
            catch (Exception ex)
            {
                return "";
            }
        }
    }
}