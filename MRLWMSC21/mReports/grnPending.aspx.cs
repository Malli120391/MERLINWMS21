using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Reflection;
using MRLWMSC21Common;
using System.Data;
using System.Globalization;
using System.Web.Script.Services;
using System.Web.Services;
using Newtonsoft.Json;
using System.Text;

namespace MRLWMSC21.mReports
{
    public partial class grnPending : System.Web.UI.Page
    {
        public CustomPrincipal cp1 = HttpContext.Current.User as CustomPrincipal;
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
                DesignLogic.SetInnerPageSubHeading(this.Page, "Grn Pending");
            }
        }

    

        //=================================== Added By M.D.Prasad =========================================//
        [WebMethod]
        public static string getGRNPending(string Tenantid, string Mcode, string SupplierInvoiceID, string Warehouseid, string IsExport, string NoofRecords, string PageNo)
        {
            cp = HttpContext.Current.User as CustomPrincipal;
            try
            {
                Tenantid = Tenantid == "" ? "0" : Tenantid;
                Mcode = Mcode == "" ? "0" : Mcode;
                //  SupplierInvoiceID = SupplierInvoiceID == "" ? "0" : SupplierInvoiceID;
                DataSet ds = DB.GetDS("EXEC [sp_RPT_GRNPendingList] @AccountID=" + cp.AccountID + " ,@TenantID=" + Tenantid + ",@InboundID=0,@MaterialMasterID=" + Mcode + ",@PoHeaderID=0,@SupplierInvoiceID='" + SupplierInvoiceID + "', @Warehouseid = " + Warehouseid + ",@IsExport=" + 0 + ",@NofRecordsPerPage=" + NoofRecords + ",@Rownumber=" + PageNo, false);
                return JsonConvert.SerializeObject(ds);
            }
            catch (Exception ex)
            {
                return "Error : " + ex;
            }
        }

        [WebMethod]
        public static string getGRNPending_Export(string Tenantid, string Mcode, string SupplierInvoiceID, string Warehouseid, string IsExport, string NoofRecords, string PageNo)
        {
            cp = HttpContext.Current.User as CustomPrincipal;
            try
            {
                string fileName = "GRNPendingReport" + DateTime.Now.Day + "" + DateTime.Now.Month + "" + DateTime.Now.Year + "_" + DateTime.Now.Hour + "" + DateTime.Now.Minute + "" + DateTime.Now.Second;
                DataSet ds = DB.GetDS("EXEC [sp_RPT_GRNPendingList] @AccountID=" + cp.AccountID + " ,@TenantID=" + Tenantid + ",@InboundID=0,@MaterialMasterID=" + Mcode + ",@PoHeaderID=0,@SupplierInvoiceID='" + SupplierInvoiceID + "', @Warehouseid = " + Warehouseid + ",@IsExport=" + 1 + ",@NofRecordsPerPage=" + NoofRecords + ",@Rownumber=" + PageNo, false);
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    MRLWMSC21Common.CommonLogic.ExportExcelDataForReports(ds.Tables[0], fileName, new List<int>(),"GRN Pending Report");
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

        //=================================== Added By M.D.Prasad =========================================//


    }
}