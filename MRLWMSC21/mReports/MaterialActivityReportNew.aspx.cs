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
    public partial class MaterialActivityReportNew : System.Web.UI.Page
    {
        protected CustomPrincipal cp = HttpContext.Current.User as CustomPrincipal;
        protected void Page_PreInit(object sender, EventArgs e)
        {
            Page.Theme = "Outbound";
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DesignLogic.SetInnerPageSubHeading(this.Page, "Inventory / Material Activity Report");
            }
        }
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static FullList GetBillingReportList(int MCode)
        {
            //List<MaterialTrackingReportIn> GetBillingReport = new List<MaterialTrackingReportIn>();
            //GetBillingReport = new MaterialTrackingReportNew().GetBillngRPT(Serialno, Batchno);
            //Serialno = Serialno == "" ? "null" : Serialno;
            //Batchno = Batchno == "" ? "null" : Batchno;
            FullList l = new FullList();
            l = new MaterialActivityReportNew().GetBillngRPT(MCode);
            return l;
        }

        private FullList GetBillngRPT(int MCode)
        {



            List<MaterialActivityReportIn> lst = new List<MaterialActivityReportIn>();
            List<MaterialActivityReportOut> lst1 = new List<MaterialActivityReportOut>();
            string Query = "EXEC [sp_RPT_GetMaterialActivityReport_In] @MaterialMasterID=" + MCode  + ",@AccountID_New=" + cp.AccountID + ",@TenantID_New=" + cp.TenantID + ";";
            Query += " EXEC [sp_RPT_GetMaterialActivityReport_Out] @MaterialMasterID=" + MCode + ",@AccountID_New=" + cp.AccountID + ",@TenantID_New=" + cp.TenantID + ";";

            DataSet DS = DB.GetDS(Query, false);
            if (DS.Tables[0].Rows.Count != 0)
            {
                foreach (DataRow row in DS.Tables[0].Rows)
                {
                    MaterialActivityReportIn BR = new MaterialActivityReportIn();
                    BR.TransDate = row["CreatedOn"].ToString();
                    BR.Supplier = row["SupplierName"].ToString();
                    BR.PONumber = row["PONumber"].ToString();
                    BR.StoreRefNo = row["StoreRefNo"].ToString();
                    BR.UoM = row["UoM"].ToString();
                    BR.ReceivedQty = row["Quantity"].ToString();
                    BR.Location = row["Location"].ToString();
                    BR.MfgDate = row["MfgDate"].ToString();
                    BR.SerialNo = row["SerialNo"].ToString();
                    BR.BatchNo = row["BatchNo"].ToString();

                    lst.Add(BR);
                }
            }

            if (DS.Tables[1].Rows.Count != 0)
            {
                foreach (DataRow row in DS.Tables[1].Rows)
                {
                    MaterialActivityReportOut BR1 = new MaterialActivityReportOut();
                    BR1.TransDate = row["TransactionDate"].ToString();
                    BR1.Customer = row["CustomerName"].ToString();
                    BR1.SONumber = row["SONumber"].ToString();
                    BR1.OBDNo = row["OBDNumber"].ToString();
                    BR1.UoM = row["UoM"].ToString();
                    BR1.PickedQty = row["PickedQuantity"].ToString();
                    BR1.Location = row["Location"].ToString();
                    BR1.ExpDate = row["ExpDate"].ToString();
                    BR1.SerialNo = row["SerialNo"].ToString();
                    BR1.BatchNo = row["BatchNo"].ToString();

                    lst1.Add(BR1);
                }
            }
            FullList l = new FullList();
            l.one = lst;
            l.two = lst1;
            return l;
            //if (l =0)
            //{
            //    resetError("No data found", true);
                
            //}
        }



        public class MaterialActivityReportIn
        {
            public string TransDate { get; set; }
            public string Supplier { get; set; }
            public string PONumber { get; set; }
            public string StoreRefNo { get; set; }
            public string UoM { get; set; }
            public string ReceivedQty { get; set; }
            public string Location { get; set; }
            public string MfgDate { get; set; }
            public string SerialNo { get; set; }
            public string BatchNo { get; set; }

        }

        public class MaterialActivityReportOut
        {
            public string TransDate { get; set; }
            public string Customer { get; set; }
            public string SONumber { get; set; }
            public string OBDNo { get; set; }
            public string UoM { get; set; }
            public string PickedQty { get; set; }
            public string Location { get; set; }
            public string ExpDate { get; set; }
            public string SerialNo { get; set; }
            public string BatchNo { get; set; }

        }

        public class FullList
        {
            public List<MaterialActivityReportIn> one { get; set; }
            public List<MaterialActivityReportOut> two { get; set; }
        }
        private void resetError(string error, bool isError)
        {

            ScriptManager.RegisterStartupScript(this, this.GetType(), "test", "showStickyToast(" + (isError.ToString().ToLower() == "true" ? "false" : "true") + ",\"" + error + "\");", true);

        }
    }
}