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
    public partial class ReceiptPendingReport : System.Web.UI.Page
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
                DesignLogic.SetInnerPageSubHeading(this.Page, "Inventory /Receipt Pending Report");
            }
        }
        [WebMethod]
        public static string getReceiptPendingReport(string TenantID,string WHID,string InbID, string IsExport, string NoofRecords, string PageNo)
        {
            try
            {
                DataSet ds = DB.GetDS("EXEC [dbo].[sp_RPT_INV_GetPendingGoodsInList] @TenantID=" + TenantID + ",@WHID=" + WHID + ", @InboundID=" + InbID + ",@IsExport=" + IsExport + ",@NoofRecords=" + NoofRecords + ",@PageNo=" + PageNo, false);
                return JsonConvert.SerializeObject(ds);
            }
            catch (Exception ex)
            {
                return "Error : " + ex;
            }
        }

        [WebMethod]
        public static string getReceiptPendingReport_Export(string TenantID, string WHID, string InbID, string IsExport, string NoofRecords, string PageNo)
        {
            try
            {
                string fileName = "ReceiptPendingReport" + DateTime.Now.Day + "" + DateTime.Now.Month + "" + DateTime.Now.Year + "_" + DateTime.Now.Hour + "" + DateTime.Now.Minute + "" + DateTime.Now.Second;
                DataSet ds = DB.GetDS("EXEC [dbo].[sp_RPT_INV_GetPendingGoodsInList] @TenantID=" + TenantID + ",@WHID=" + WHID + ", @InboundID=" + InbID + ",@IsExport=" + IsExport + ",@NoofRecords=" + NoofRecords + ",@PageNo=" + PageNo, false);
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    MRLWMSC21Common.CommonLogic.ExportExcelDataForReports(ds.Tables[0], fileName, new List<int>(), "Receipt Pending Report");
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
    }
}