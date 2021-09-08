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
    public partial class MaterialReplenishmentNew : System.Web.UI.Page
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
                DesignLogic.SetInnerPageSubHeading(this.Page, "Inventory / Material Replenishment Report");
            }
        }
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static List<MaterialReplenishment> GetBillingReportList(string PartNo, int TenantId, int WareHouseId)
        {
            PartNo = PartNo == "" ? "null" : DB.SQuote(PartNo);
            List<MaterialReplenishment> GetBillingReport = new List<MaterialReplenishment>();
            GetBillingReport = new MaterialReplenishmentNew().GetBillngRPT(PartNo,TenantId, WareHouseId);
            return GetBillingReport;
        }

        private List<MaterialReplenishment> GetBillngRPT(string PartNo, int TenantId, int WareHouseId)
        {
            cp = HttpContext.Current.User as CustomPrincipal;
            List<MaterialReplenishment> lst = new List<MaterialReplenishment>();
            string Query = "EXEC [sp_RPT_GetMaterialReplenishment] @MCode = " + PartNo + ",@AccountID_New=" + cp.AccountID + ",@TenantID_New=" +TenantId+ ", @WareHouseId = "+WareHouseId+"";
            DataSet DS = DB.GetDS(Query, false);
            if (DS.Tables[0].Rows.Count != 0)
            {
                foreach (DataRow row in DS.Tables[0].Rows)
                {
                    MaterialReplenishment BR = new MaterialReplenishment();
                    BR.PartNumber = row["PartNo"].ToString();
                    BR.Tenant = row["TenantName"].ToString();
                    BR.Supplier = row["SupplierName"].ToString();
                    BR.MaterialType = row["MType"].ToString();
                    BR.Description = row["MDescription"].ToString();
                    //BR.PlannedDeliveryinDays = row["PlannedDeliveryTime"].ToString();
                    //BR.ExpectedUnitCost = Convert.ToDecimal(row["ExpectedUnitCost"].ToString());
                    BR.InitialOrderQty = Convert.ToDecimal(row["InitialOrderQuantity"].ToString());
                    BR.AvailableQty = Convert.ToDecimal(row["AvailableQTY"].ToString());
                    BR.ReorderPoint = Convert.ToDecimal(row["ReorderPoint"].ToString());
                    BR.Location = row["Location"].ToString();

                    lst.Add(BR);
                }
            }
            return lst;
        }


        public class MaterialReplenishment
        {
            public string PartNumber { get; set; }
            public string Tenant { get; set; }
            public string Supplier { get; set; }
            public string MaterialType { get; set; }
            public string Description { get; set; }
            public string PlannedDeliveryinDays { get; set; }
            public decimal ExpectedUnitCost { get; set; }
            public decimal InitialOrderQty { get; set; }
            public decimal AvailableQty { get; set; }
            public decimal ReorderPoint { get; set; }
            public string Location { get; set; }
           
        }
    }
}