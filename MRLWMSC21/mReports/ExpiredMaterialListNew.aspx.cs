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
    public partial class ExpiredMaterialListNew : System.Web.UI.Page
    {
        public static CustomPrincipal cp = HttpContext.Current.User as CustomPrincipal;
        protected void Page_PreInit(object sender, EventArgs e)
        {
            cp = HttpContext.Current.User as CustomPrincipal;
            Page.Theme = "Outbound";
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DesignLogic.SetInnerPageSubHeading(this.Page, "Inventory / Expired Material Report");
            }
        }

        [WebMethod]
        public static string GetExpMaterialList(string TenantId, string MMId, int WareHouseId, string IsExport, string NoofRecords, string PageNo)
        {
            cp = HttpContext.Current.User as CustomPrincipal;
            try
            {
                string Query = "EXEC [sp_RPT_GetExpiredMaterialList] @AccountID_New=" + cp.AccountID + ",@TenantID_New=" + TenantId + ",@mmiD=" + MMId + ",@WareHouseId = " + WareHouseId + ",@IsExport=" + 0 + ",@NoofRecords=" + NoofRecords + ",@PageNo=" + PageNo;
                DataSet ds = DB.GetDS(Query, false);
                return JsonConvert.SerializeObject(ds);
            }
            catch(Exception ex)
            {
                return "Error : " + ex;
            }
        }

        [WebMethod]
        public static string GetExpMaterialList_Export(string TenantId, string MMId, int WareHouseId, string IsExport, string NoofRecords, string PageNo)
        {
            cp = HttpContext.Current.User as CustomPrincipal;
            try
            {
                string fileName = "ExpMaterialList" + DateTime.Now.Day + "" + DateTime.Now.Month + "" + DateTime.Now.Year + "_" + DateTime.Now.Hour + "" + DateTime.Now.Minute + "" + DateTime.Now.Second;
                string Query = "EXEC [sp_RPT_GetExpiredMaterialList] @AccountID_New=" + cp.AccountID + ",@TenantID_New=" + TenantId + ",@mmiD=" + MMId + ",@WareHouseId = " + WareHouseId + ",@IsExport=" + 1 + ",@NoofRecords=" + NoofRecords + ",@PageNo=" + PageNo;
                DataSet ds = DB.GetDS(Query, false);
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    MRLWMSC21Common.CommonLogic.ExportExcelDataForReports(ds.Tables[0], fileName, new List<int>(), "Expired Material List");
                    return fileName;
                }
                else
                {
                    return "No Data Found";
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }







        //[WebMethod]
        //[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        //public static List<ExpiredMaterialList> GetBillingReportList(string TenantId, string MMId, int WareHouseId)
        //{
        //    List<ExpiredMaterialList> GetBillingReport = new List<ExpiredMaterialList>();
        //    GetBillingReport = new ExpiredMaterialListNew().GetBillngRPT(TenantId, MMId, WareHouseId);
        //    return GetBillingReport;
        //}

        //private List<ExpiredMaterialList> GetBillngRPT(string TenantId, string MMId, int WareHouseId)
        //{


        //    List<ExpiredMaterialList> lst = new List<ExpiredMaterialList>();
        //    string Query = "EXEC [sp_RPT_GetExpiredMaterialList] @AccountID_New=" + cp.AccountID + ",@TenantID_New=" + TenantId + ",@mmiD="+ MMId+ ", @WareHouseId = "+WareHouseId+"";
        //    DataSet DS = DB.GetDS(Query, false);
        //    if (DS.Tables[0].Rows.Count != 0)
        //    {
        //        foreach (DataRow row in DS.Tables[0].Rows)
        //        {
        //            ExpiredMaterialList OR = new ExpiredMaterialList();
        //            OR.PartNo = row["MCode"].ToString();
        //            OR.Tenant = row["TenantName"].ToString();
        //            OR.MaterialType = row["MType"].ToString();
        //            OR.Location = row["Location"].ToString();
        //            OR.AvailableQty = row["AvailableQty"].ToString();
        //            OR.UoM = row["UoM"].ToString();
        //            OR.Discrepancy = row["HasDiscrepancy"].ToString();
        //            OR.ExpiredOn = row["ExpiredOn"].ToString();
        //            lst.Add(OR);
        //        }
        //    }
        //    return lst;
        //}

        public class ExpiredMaterialList
        {
            public string PartNo { get; set; }
            public string Tenant { get; set; }
            public string MaterialType { get; set; }
            public string Location { get; set; }
            public string AvailableQty { get; set; }
            public string UoM { get; set; }
            public string Discrepancy { get; set; }
            public string ExpiredOn { get; set; }





        }
    }
}