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
    public partial class SupplierPerformanceNew : System.Web.UI.Page
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
                DesignLogic.SetInnerPageSubHeading(this.Page, "Warehouse / Supplier Performance Report");
            }
        }
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static List<SupplierPerformance> GetBillingReportList(string SupplierID,string PartNo, string TenantId,string WareHouseID,int PageIndex)
        {

            SupplierID = SupplierID == "" ? "0" : SupplierID;
            PartNo = PartNo == "" ? "0" : PartNo;
            List<SupplierPerformance> GetBillingReport = new List<SupplierPerformance>();
            GetBillingReport = new SupplierPerformanceNew().GetBillngRPT(SupplierID,PartNo, TenantId, WareHouseID, PageIndex);
            return GetBillingReport;
        }

        private List<SupplierPerformance> GetBillngRPT(string SupplierID, string PartNo,string TenantId,string WareHouseID,int PageIndex)
        {
            cp = HttpContext.Current.User as CustomPrincipal;
            List<SupplierPerformance> lst = new List<SupplierPerformance>();
            //string Query = "EXEC [sp_RPT_GetMaterialReplenishment] @MCode = " + PartNo + "";
            string Query = "EXEC [sp_RPT_GetSupplierPerformance] @SupplierID= " + SupplierID + ", @MaterialMasterID= "+PartNo+ ",@TenantID="+ TenantId + ",@Rownumber=" + PageIndex + ",@AccountID_New=" + cp.AccountID + ",@TenantID_New=" + cp.TenantID;
            DataSet DS = DB.GetDS(Query, false);
            if (DS.Tables[0].Rows.Count != 0)
            {
                foreach (DataRow row in DS.Tables[0].Rows)
                {
                    SupplierPerformance BR = new SupplierPerformance();
                    BR.Supplier = row["SupplierName"].ToString();
                    BR.TotalPO = row["TotalPOs"].ToString();
                    BR.TotalPOLineNo = row["TotalPOLineNos"].ToString();
                    BR.LineNo = row["TotalLineNosRcvd"].ToString();
                    BR.TotalPOQty = row["TotalPOQty"].ToString();
                    BR.ActualReceivedQty = Convert.ToDecimal(row["ActualQtyRcvd"].ToString());
                    BR.AcceptedQty = Convert.ToDecimal(row["AcceptedQty"].ToString());
                    BR.DamagedQty = Convert.ToDecimal(row["DamagedQty"].ToString());
                    BR.DiscrepancyQty = Convert.ToDecimal(row["DiscrepancyQty"].ToString());
                    BR.NCQty = Convert.ToDecimal(row["NCQty"].ToString());
                    lst.Add(BR);
                }
            }
            return lst;
        }


        public class SupplierPerformance
        {
            public string Supplier { get; set; }
            public string TotalPO { get; set; }
            public string TotalPOLineNo { get; set; }
            public string LineNo { get; set; }
            public string TotalPOQty { get; set; }
            public decimal ActualReceivedQty { get; set; }
            public decimal AcceptedQty { get; set; }
            public decimal DamagedQty { get; set; }
            public decimal DiscrepancyQty { get; set; }
            public decimal NCQty { get; set; }

        }
    }
}