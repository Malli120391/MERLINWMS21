using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MRLWMSC21Common;
using System.Data.SqlClient;
using System.Data;
using System.Text;
using System.Net.Mail;
using System.Net.Mime;
using System.IO;
using System.Web.Services;
using System.Web.Script.Services;
using System.Globalization;
using Newtonsoft.Json;

namespace MRLWMSC21.mYardManagement
{
    public partial class VehicleAvailability : System.Web.UI.Page
    {
        public CustomPrincipal cp = HttpContext.Current.User as CustomPrincipal;
        public static CustomPrincipal cp1 = HttpContext.Current.User as CustomPrincipal;
        public String vid = CommonLogic.QueryString("vid");
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DesignLogic.SetInnerPageSubHeading(this.Page, "Vehicle Availabity & Downtime");
                if ((CommonLogic.QueryString("vid") != "") && (CommonLogic.QueryString("vid") != "null"))
                {
                    //resetError("Hello" + vid, false);
                    GetAvailabilityDownlist(Convert.ToInt32(vid));
                }
            }


        }
        [WebMethod]
        public static string GetAvailabilityDownlist(int VehicleID)
        {
            string Json = "";
            try
            {
                DataSet ds = DB.GetDS("[dbo].[Usp_GET_YM_MST_VehiclesAvailDownList] @VehicleID=" + VehicleID, false);
                Json = JsonConvert.SerializeObject(ds);
            }
            catch (Exception ex)
            {
                Json = "Failed";
            }
            return Json;
        }

        [WebMethod]
        public static string UpsertVehicleAvailability(string AvailFrom, string AvailTo,string VehicleID, int VehicleAvailbilityID)
        {
            StringBuilder sqlUoMDetails = new StringBuilder(2500);
            sqlUoMDetails.Append("EXEC  [dbo].[sp_INB_Upsert_VehicleAvailability]    @YM_MST_Vehicle_ID=" + Convert.ToInt32(VehicleID) + ",@YM_TRN_VehicleAvailability_ID="+ VehicleAvailbilityID + ",@AvailableFrom='" + AvailFrom + "',@AvailableTo='" + AvailTo + "',@CreatedBy=" + cp1.UserID );
            DB.ExecuteSQL(sqlUoMDetails.ToString());
            return "";
        }
        [WebMethod]
        public static string DeleteVehAvailability(string StrId)
        {
            string Status;
            try
            {

                CustomPrincipal customprinciple = HttpContext.Current.User as CustomPrincipal;
                StringBuilder sb = new StringBuilder();
                sb.Append(" Exec [dbo].[YM_TRN_DELETE_VehicleAvailability]");
                sb.Append("@PK=" + StrId);
                sb.Append(",@UpdatedBy=" + customprinciple.UserID.ToString());
                DB.ExecuteSQL(sb.ToString());
                Status = "success";
            }
            catch (Exception ex)
            {
                Status = "Failed";
            }

            return Status;
        }

        [WebMethod]
        public static string DeleteVehDowntime(string StrId)
        {
            string Status;
            try
            {

                CustomPrincipal customprinciple = HttpContext.Current.User as CustomPrincipal;
                StringBuilder sb = new StringBuilder();
                sb.Append(" Exec [dbo].[YM_TRN_DELETE_VehicleDowntime]");
                sb.Append("@PK=" + StrId);
                sb.Append(",@UpdatedBy=" + customprinciple.UserID.ToString());
                DB.ExecuteSQL(sb.ToString());
                Status = "success";
            }
            catch (Exception ex)
            {
                Status = "Failed";
            }

            return Status;
        }

        [WebMethod]
        public static string UpsertVehicleDowntime(string DowntimeFrom, string DowntimeTo,string Description,int VehicleID,int VehicleDownID)
        {
            StringBuilder sqlUoMDetails = new StringBuilder(2500);
            sqlUoMDetails.Append("EXEC  [dbo].[sp_INB_Upsert_VehicleDowntime]    @YM_MST_Vehicle_ID=" + Convert.ToInt32(VehicleID) + ",@TM_TRN_VehicleDowntime_ID=" + VehicleDownID + ",@DowntimeFrom='" + DowntimeFrom + "',@DowntimeTo='" + DowntimeTo + "',@Description='"+ Description + "',@CreatedBy=" + cp1.UserID);
            DB.ExecuteSQL(sqlUoMDetails.ToString());
            return "";
        }
    }
}