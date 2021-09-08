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
    public partial class ActiveStockReportNew : System.Web.UI.Page
    {
        //public CustomPrincipal cp = HttpContext.Current.User as CustomPrincipal;
        protected CustomPrincipal cp = HttpContext.Current.User as CustomPrincipal;

        protected void Page_PreInit(object sender, EventArgs e)
        {
            Page.Theme = "Outbound";
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DesignLogic.SetInnerPageSubHeading(this.Page, "Inventory / Active Stock Report");
            }
        }
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static List<ActiveStockReport> GetBillingReportList(string MCode,string MaterialType, string Location)
        {
            MCode= MCode == "" ? "null" : MCode;
            MaterialType = MaterialType == "" ? "null" : MaterialType;
            Location=Location==""?"null": Location;

            List<ActiveStockReport> GetBillingReport = new List<ActiveStockReport>();
            GetBillingReport = new ActiveStockReportNew().GetBillngRPT(MCode,MaterialType, Location);
            return GetBillingReport;
        }

        private List<ActiveStockReport> GetBillngRPT(string MCode,string MaterialType, string Location)
        {
            
            List<ActiveStockReport> lst = new List<ActiveStockReport>();
            //string Query = " EXEC [sp_RPT_GetLogAuditData] @UserID= " + PartNO + ", @FromDate = " + DB.SQuote(MaterialType) + ", @ToDate = " + DB.SQuote(ToDate) + "";
            string Query = " EXEC [dbo].[sp_RPT_GetActiveStock_New] @MaterialMasterID=" + MCode+ ", @LocationID = "  +Location+ ", @MTypeID = "  +MaterialType+ ",@AccountID_New=" + cp.AccountID + ",@TenantID_New=" + cp.TenantID;
              
            DataSet DS = DB.GetDS(Query, false);
            if (DS.Tables[0].Rows.Count != 0)
            {
                foreach (DataRow row in DS.Tables[0].Rows)
                {
                    ActiveStockReport BR = new ActiveStockReport();

                    BR.PartNo = row["MCode"].ToString();
                    BR.Description = row["MDescription"].ToString();
                    BR.UoM = row["UoM"].ToString();
                    BR.Location = row["Location"].ToString();
                    BR.KitID = row["KitPlannerID"].ToString();
                    BR.InOH = row["InOH"].ToString();
                    BR.ObOH = row["OutOH"].ToString();
                    BR.AvlQty = row["AvlQty"].ToString();
                    //BR.BatchNo = row["BatchNo"].ToString();
                    //BR.ExpDate = row["ExpDate"].ToString();
                    //BR.MfgDate = row["MfgDate"].ToString();
                    //BR.Plant = row["Plant"].ToString();
                    //BR.SerialNO = row["SerialNo"].ToString();
                    //BR.StockType = row["StockType"].ToString();

                    lst.Add(BR);
                }
            }
            return lst;
        }


        public class ActiveStockReport
        {
            public string PartNo { get; set; }
            public string Description { get; set; }
            public string UoM { get; set; }
            public string Location { get; set; }
            public string KitID { get; set; }
            public string InOH { get; set; }
            public string ObOH { get; set; }
            public string AvlQty { get; set; }
            //public string BatchNo { get; set; }
            //public string ExpDate { get; set; }
            //public string MfgDate { get; set; }
            //public string Plant { get; set; }
            //public string SerialNO { get; set; }
            //public string StockType { get; set; }

        }
    }
}