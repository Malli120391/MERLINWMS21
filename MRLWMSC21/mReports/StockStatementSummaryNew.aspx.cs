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

namespace MRLWMSC21.mReports
{
    public partial class StockStatementSummaryNew : System.Web.UI.Page
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
                DesignLogic.SetInnerPageSubHeading(this.Page, "Inventory / Stock Statement Summary");
            }
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static List<Stock_Statement_Summary> GetBillingReportList(string FromDate, string ToDate, string MTypeID, string mcode, int tenantid, int accountid, int Warehouseid, string PageNo, string PageSize, string IsExport)
        {
            FromDate = FromDate == "" ? "null" : FromDate;
            ToDate = ToDate == "" ? "null" : ToDate;
            MTypeID = MTypeID == "" ? "0" : MTypeID;
            mcode = mcode == "" ? "0" : mcode;

            List<Stock_Statement_Summary> GetBillingReport = new List<Stock_Statement_Summary>();
            GetBillingReport = new StockStatementSummaryNew().GetGITReport(FromDate, ToDate, MTypeID, mcode, tenantid, accountid, Warehouseid, PageNo, PageSize, IsExport);
            return GetBillingReport;
        }

        private List<Stock_Statement_Summary> GetGITReport(string FromDate, string ToDate, string MTypeID, string mcode, int tenantid, int accountid, int Warehouseid, string PageNo, string PageSize, string IsExport)
        {
            if (FromDate != "" && FromDate != "null")
            {
                FromDate = DateTime.ParseExact(FromDate.Replace("-", "/"), "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");
            }
            if (ToDate != "" && ToDate != "null")
            {
                ToDate = DateTime.ParseExact(ToDate.Replace("-", "/"), "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");
            }

            FromDate = FromDate == "null" ? FromDate : DB.SQuote(FromDate);
            ToDate = ToDate == "null" ? ToDate : DB.SQuote(ToDate);

            List<Stock_Statement_Summary> lst = new List<Stock_Statement_Summary>();

            string Query = "EXEC [dbo].[sp_RPT_GetStockStatementSummary]  @fromDate= " + FromDate + ", @ToDate= " + ToDate + ", @MTypeID=" + MTypeID + " ,@MaterialMasterID=" + mcode + ",@TenantID=" + cp.TenantID + ",@accountID =" + cp.AccountID + ", @WareHouseId = " + Warehouseid + ",@PageNo=" + PageNo + ",@NoofRecords=" + PageSize + ",@IsExport=" + IsExport;

            DataSet DS = DB.GetDS(Query, false);
            if (DS.Tables[0].Rows.Count != 0)
            {
                foreach (DataRow row in DS.Tables[0].Rows)
                {
                    Stock_Statement_Summary GR = new Stock_Statement_Summary();
                    GR.PartNo = row["MCode"].ToString();
                    GR.Description = row["MDescription"].ToString();
                    GR.UOM = row["UoM/Qty"].ToString();
                    GR.OpeningStock = row["OpenStockQty"].ToString();
                    GR.Inbound = row["InQty"].ToString();
                    GR.Outbound = row["OutQty"].ToString();
                    GR.ClosingStock = row["ClosingStockQty"].ToString();
                    GR.StockDifference = row["Diff"].ToString();


                    lst.Add(GR);
                }
            }
            return lst;
        }

        public class Stock_Statement_Summary
        {
            public string PartNo { get; set; }
            public string Description { get; set; }
            public string UOM { get; set; }
            public string OpeningStock { get; set; }
            public string Inbound { get; set; }
            public string Outbound { get; set; }
            public string ClosingStock { get; set; }
            public string StockDifference { get; set; }






        }
        
    }
}