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

namespace MRLWMSC21.mOutbound
{
    public partial class OBDRevert : System.Web.UI.Page
    {
        //protected void Page_Load(object sender, EventArgs e)
        //{

        //}
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
                DesignLogic.SetInnerPageSubHeading(this.Page, "Outbound / OBD Revert");
            }
        }
        [WebMethod]
        public static string GetOBDList(int TenantId, int WareHouseId, int NoofRecords, int PageNo, int StatusID, string FromDate, string ToDate, int CategoryID, string SearchText)
        {
            try
            {
                string Query = "EXEC [USP_OBD_TRN_GetOBDRevertList] @TenantId=" + TenantId + ",@WareHouseId = " + WareHouseId + ",@NoofRecords=" + NoofRecords + ",@PageNo=" + PageNo + ",@StatusID=" + StatusID + ",@FromDate ='" + FromDate + "',@ToDate='" + ToDate + "',@CategoryID=" + CategoryID + ",@SearchText='" + SearchText + "'";
                DataSet ds = DB.GetDS(Query, false);
                return JsonConvert.SerializeObject(ds);
            }
            catch (Exception ex)
            {
                return "Error : " + ex;
            }
        }

        [WebMethod]
        public static string setOBDRevert(string OBDNumber, int DeliveryTypeID)
        {
            try
            {
                cp = HttpContext.Current.User as CustomPrincipal;
                string Query = "EXEC [SP_SET_OBD_Revert_New] @DeliveryStatusId=0,@OutboundId = 0,@OBDNumber ='" + OBDNumber + "',@CreatedBy='" + cp.UserID + "',@DeliveryStatusTypeID=" + DeliveryTypeID;
                int ds = DB.GetSqlN(Query);
                return JsonConvert.SerializeObject(ds);
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(-5);
            }
        }
        [WebMethod]
        public static string GetOBDDetails(int OutboundID)
        {
            try
            {
                cp = HttpContext.Current.User as CustomPrincipal;
                string Query = "EXEC [USP_OBD_TRN_GetOBDDetailsRevert] @OutboundID=" + OutboundID;
                DataSet ds = DB.GetDS(Query, false);
                return JsonConvert.SerializeObject(ds);
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(-5);
            }
        }

        [WebMethod]
        public static string OBDLinePickRevert(int AssignedID, int RevertQty, int RevertTypeID)
        {
            try
            {
                cp = HttpContext.Current.User as CustomPrincipal;
                string Query = "EXEC [SP_OBD_Revert_LineItems] @AssignedID=" + AssignedID + ",@Qty_Revert =" + RevertQty + ",@RevertTypeID ='" + RevertTypeID + "',@CreatedBy=" + cp.UserID;
                string ds = DB.GetSqlS(Query);
                return JsonConvert.SerializeObject(ds);
            }
            catch (Exception ex)
            {
                return "Error : Please Contact Inventrax Support Team..!";
            }
        }
    }
}