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

namespace MRLWMSC21.mReports
{
    public partial class ReOrderPoint : System.Web.UI.Page
    {

        public static CustomPrincipal cp;
        protected void Page_PreInit(object sender, EventArgs e)
        {
            cp = HttpContext.Current.User as CustomPrincipal;
            Page.Theme = "Outbound";
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DesignLogic.SetInnerPageSubHeading(this.Page, "Reorder point Report");
            }
        }


        [WebMethod]
        public static string GETOBData(string TenantID, string WHID,string MtypeID,string MgroupId,string MMId, string PaginationId, string PageSize)
        {
            cp = HttpContext.Current.User as CustomPrincipal;
            try
            {
                string query = "EXEC [dbo].[USP_GET_INV_ReorderQty] @TenantID =" + TenantID + ",@WHID=" + WHID + ",@MMID=" + MMId + ",@MTypeID=" + MtypeID + ",@MGroupID=" + MgroupId + ",@RowNumber=" + PaginationId + ",@NofRecordsPerPage=" + PageSize + ",@IsForExcel=0";
                DataSet ds = DB.GetDS(query, false);
                return JsonConvert.SerializeObject(ds);

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        [WebMethod]
        public static string ExportExcel(string TenantID, string WHID, string MtypeID, string MgroupId, string MMId)
        {
            cp = HttpContext.Current.User as CustomPrincipal;
            try
            {
                string FileName = "ReOrderPointReport" + DateTime.Now.Day + "" + DateTime.Now.Month + "" + DateTime.Now.Year + "_" + DateTime.Now.Hour + "" + DateTime.Now.Minute + "" + DateTime.Now.Second;
                string query = "EXEC [dbo].[USP_GET_INV_ReorderQty] @TenantID =" + TenantID + ",@WHID=" + WHID + ",@MMID=" + MMId + ",@MTypeID=" + MtypeID + ",@MGroupID=" + MgroupId + ",@IsForExcel=1";
                DataSet ds = DB.GetDS(query, false);
                DataTable dt = new DataTable();
                dt = ds.Tables[0];

                MRLWMSC21Common.CommonLogic.ExportLoadSheetInfo(ds, FileName, new List<int>());
                
                return FileName;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}