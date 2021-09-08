using MRLWMSC21Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Xml.Linq;

namespace MRLWMSC21.mInventory
{
    public partial class CycleCountDetails : System.Web.UI.Page
    {
        protected void page_PreInit(object sender, EventArgs e)
        {
            Page.Theme = "Inventory";
        }
        public CustomPrincipal cp = HttpContext.Current.User as CustomPrincipal;
        //protected void Page_Load(object sender, EventArgs e)
        //{
        //    if (!IsPostBack)
        //    {
        //        DesignLogic.SetInnerPageSubHeading(this.Page, "Cycle Count Details");
        //        hdnCreatedBy.Value = cp.UserID.ToString();
        //        hdnUpdatedBy.Value = cp.UserID.ToString();
        //    }
        //}

        [WebMethod]
        public static string GetList(int Id)
        {
            CustomPrincipal cp = HttpContext.Current.User as CustomPrincipal;
            DataSet ds = DB.GetDS("EXEC [dbo].[SP_GET_CCM_CNF_AccountCycleCounts] @AM_MST_Account_ID=" + cp.AccountID + ",@CCM_CNF_AccountCycleCount_ID=" + Id, false);
            return JsonConvert.SerializeObject(ds);

        }

        [WebMethod]
        public static string GetEditData(int Id)
        {
            CustomPrincipal cp = HttpContext.Current.User as CustomPrincipal;
            DataSet ds = DB.GetDS("EXEC [dbo].[SP_GET_CCM_CNF_AccountCycleCounts_List] @AM_MST_Account_ID=" + cp.AccountID + ",@CCM_CNF_AccountCycleCount_ID=" + Id + ",@UserId="+cp.UserID , false);
            return JsonConvert.SerializeObject(ds);

        }

        [WebMethod]
        public static string GetCounts(int CCID)
        {

            //StringBuilder sb1 = new StringBuilder();
            //sb1.Append(" SELECT CycleTimeInDays FROM CCM_MST_Freequency WHERE CCM_MST_Freequency_ID= ");
            //sb1.Append(" ( SELECT CCM_MST_Freequency_ID FROM CCM_CNF_AccountCycleCounts WHERE CCM_CNF_AccountCycleCount_ID=" + CCID + " AND IsActive=1 AND IsDeleted=0) AND IsActive=1 AND IsDeleted=0; ");
            //sb1.Append(" SELECT COUNT(*) Completed FROM CCM_TRN_CycleCounts WHERE CCM_CNF_AccountCycleCount_ID=" + CCID + " AND CCM_MST_CycleCountStatus_ID=4 AND IsActive=1 AND IsDeleted=0; ");
            //sb1.Append(" SELECT COUNT(*) TranCount FROM CCM_TRN_CycleCounts WHERE CCM_CNF_AccountCycleCount_ID =" + CCID + " AND IsActive=1 AND IsDeleted=0 AND CCM_MST_CycleCountStatus_ID=2 ");
            //sb1.Append("SELECT ValidThru FROM CCM_CNF_AccountCycleCounts WHERE IsActive=1 AND IsDeleted=0 AND CCM_CNF_AccountCycleCount_ID = " + CCID);

            //sb1.Append("SELECT CASE WHEN SUM(CASE WHEN TCC.CCM_MST_CycleCountStatus_ID = 4 THEN 1 ELSE 0 END) =CONVERT(INT,DATEDIFF(dd, ValidFrom, ValidThru)/CycleTimeInDays) THEN 1 ELSE 0 END AS StatusFlag"
            //            + " FROM CCM_TRN_CycleCounts TCC INNER JOIN CCM_CNF_AccountCycleCounts ACC ON ACC.CCM_CNF_AccountCycleCount_ID = TCC.CCM_CNF_AccountCycleCount_ID"
            //            + " AND TCC.CCM_CNF_AccountCycleCount_ID = "+ CCID + "  AND ACC.IsActive = 1 AND ACC.IsDeleted = 0 AND TCC.IsActive = 1 AND TCC.IsDeleted = 0"
            //            + " INNER JOIN CCM_MST_Freequency FRQ ON FRQ.CCM_MST_Freequency_ID = ACC.CCM_MST_Freequency_ID AND FRQ.IsActive = 1 AND FRQ.IsDeleted = 0"
            //            + " GROUP BY TCC.CCM_CNF_AccountCycleCount_ID, ValidFrom, ValidThru, CycleTimeInDays");


            StringBuilder sb1 = new StringBuilder();
            sb1.Append(" SELECT CycleTimeInDays FROM CCM_MST_Freequency WHERE CCM_MST_Freequency_ID= ");
            sb1.Append(" ( SELECT CCM_MST_Freequency_ID FROM CCM_CNF_AccountCycleCounts WHERE CCM_CNF_AccountCycleCount_ID=" + CCID + " AND IsActive=1 AND IsDeleted=0) AND IsActive=1 AND IsDeleted=0; ");
            sb1.Append(" SELECT COUNT(*) Completed FROM CCM_TRN_CycleCounts WHERE CCM_CNF_AccountCycleCount_ID=" + CCID + " AND CCM_MST_CycleCountStatus_ID=4 AND IsActive=1 AND IsDeleted=0; ");
            sb1.Append(" SELECT COUNT(*) TranCount FROM CCM_TRN_CycleCounts WHERE CCM_CNF_AccountCycleCount_ID =" + CCID + " AND IsActive=1 AND IsDeleted=0 AND CCM_MST_CycleCountStatus_ID=2 ");
            sb1.Append("SELECT ValidThru FROM CCM_CNF_AccountCycleCounts WHERE IsActive=1 AND IsDeleted=0 AND CCM_CNF_AccountCycleCount_ID = " + CCID);

            sb1.Append("SELECT CASE WHEN SUM(CASE WHEN TCC.CCM_MST_CycleCountStatus_ID = 4 THEN 1 ELSE 0 END) = CASE WHEN CONVERT(INT, DATEDIFF(dd, ValidFrom, ValidThru)/ CycleTimeInDays)"
                       + "  = 0 THEN 1 ELSE CONVERT(INT, DATEDIFF(dd, ValidFrom, ValidThru)/ CycleTimeInDays) END"
                       + "  THEN 1 ELSE 0 END AS StatusFlag"
                       + "  FROM CCM_TRN_CycleCounts TCC"
                       + "  INNER JOIN CCM_CNF_AccountCycleCounts ACC ON ACC.CCM_CNF_AccountCycleCount_ID = TCC.CCM_CNF_AccountCycleCount_ID"
                       + "  AND TCC.CCM_CNF_AccountCycleCount_ID = " + CCID + "  AND ACC.IsActive = 1 AND ACC.IsDeleted = 0 AND TCC.IsActive = 1 AND TCC.IsDeleted = 0"
                       + "  INNER JOIN CCM_MST_Freequency FRQ ON FRQ.CCM_MST_Freequency_ID = ACC.CCM_MST_Freequency_ID AND FRQ.IsActive = 1 AND FRQ.IsDeleted = 0"
                       + "  GROUP BY TCC.CCM_CNF_AccountCycleCount_ID, ValidFrom, ValidThru, CycleTimeInDays");



            DataSet ds = DB.GetDS(sb1.ToString(), false);
            int FreqCount = Convert.ToInt32(ds.Tables[0].Rows[0][0].ToString());
            int CompCount = Convert.ToInt32(ds.Tables[1].Rows[0][0].ToString());
            int TranCount = Convert.ToInt32(ds.Tables[2].Rows[0][0].ToString());
            string ValidTo = ds.Tables[3].Rows[0][0].ToString();
            int TotalComStatus = 0;
            if (ds.Tables[4].Rows.Count > 0)
            {
                // string s = ds.Tables[4].Rows[0]["StatusFlag"].ToString();
                TotalComStatus = ds.Tables[4].Rows[0][0].ToString() == "" ? 0 : Convert.ToInt32(ds.Tables[4].Rows[0][0].ToString());
            }
            return @FreqCount + "," + CompCount + "," + @TranCount + "," + ValidTo + "," + TotalComStatus;
        }

        [WebMethod]
        public static string MasterDetailsSet(string Sp_Set, string JSON, int CCID = 0, int TenantId = 0)
        {
            Sp_Set = "[dbo].[USP_SET_CCM_CNF_AccountCycleCounts]";
            Dictionary<dynamic, dynamic> values = JsonConvert.DeserializeObject<Dictionary<dynamic, dynamic>>(JsonConvert.DeserializeObject(JSON).ToString());

            StringBuilder sb = new StringBuilder();
            int count = 0;
            foreach (KeyValuePair<dynamic, dynamic> pair in values)
            {
                if (count != 0)
                    sb.Append(",");
                sb.Append(pair.Key + "=" + DB.SQuote(pair.Value));
                count++;
            }

            // DB.ExecuteSQL("EXEC " + Sp_Set + " " + sb.ToString());
            DataSet ds = DB.GetDS("EXEC " + Sp_Set + " " + sb.ToString() + ", @TenantId = " + TenantId + "", false);
            return JsonConvert.SerializeObject(ds);
        }

        [WebMethod]
        public static string MasterDetailsSetEntity(string Sp_Set, string JSON, int CCID = 0)
        {
            CustomPrincipal cp = HttpContext.Current.User as CustomPrincipal;
            Sp_Set = "[dbo].[USP_SET_CCM_CNF_AccountCycleCountsDetails]";

            Dictionary<dynamic, dynamic> values = JsonConvert.DeserializeObject<Dictionary<dynamic, dynamic>>(JsonConvert.DeserializeObject(JSON).ToString());

            StringBuilder sb = new StringBuilder();
            int count = 0;
            foreach (KeyValuePair<dynamic, dynamic> pair in values)
            {
                if (count != 0)
                    sb.Append(",");
                sb.Append(pair.Key + "=" + DB.SQuote(pair.Value));
                count++;
            }

            DB.ExecuteSQL("EXEC " + Sp_Set + " " + sb.ToString());


            return "";
        }


        [WebMethod]
        public static int DeleteItemsById(string SP_Del, string ID)
        {
            SP_Del = "[dbo].[USP_Delete_CCM_CNF_AccountCycleCountDetails]";
            CustomPrincipal cp1 = HttpContext.Current.User as CustomPrincipal;
            StringBuilder sb = new StringBuilder();

            //sb.Append(" @PK=" + ID + ", @LoggedInUserID=" + cp1.UserID+ ", @UTCTimestamp=" +DB.SQuote(DateTime.UtcNow.ToString()));
            sb.Append(" @PK=" + ID + ", @LoggedInUserID=" + cp1.UserID);
            DB.ExecuteSQL("EXEC " + SP_Del + " " + sb.ToString());


            return 0;

        }

        [WebMethod]
        public static string CheckCycleCountNameExits(int Id)
        {
            CustomPrincipal cp = HttpContext.Current.User as CustomPrincipal;
            DataSet ds = DB.GetDS("EXEC [dbo].[SP_GET_CCM_CNF_AccountCycleCounts_List] @AM_MST_Account_ID=" + cp.AccountID, false);
            return JsonConvert.SerializeObject(ds);

        }


        [WebMethod]
        public static string GetCountsNew(int CCID)
        {
            StringBuilder sb1 = new StringBuilder();

            sb1.Append(" SELECT COUNT(*) TranCount FROM CCM_TRN_CycleCounts WHERE CCM_CNF_AccountCycleCount_ID =" + CCID + " AND IsActive=1 AND IsDeleted=0 AND CCM_MST_CycleCountStatus_ID=2 ");
            sb1.Append(" SELECT COUNT(*) Completed FROM CCM_TRN_CycleCounts WHERE CCM_CNF_AccountCycleCount_ID=" + CCID + " AND CCM_MST_CycleCountStatus_ID=4 AND IsActive=1 AND IsDeleted=0; ");

            DataSet ds = DB.GetDS(sb1.ToString(), false);
            int TranCount = Convert.ToInt32(ds.Tables[0].Rows[0][0].ToString());
            int CompCount = Convert.ToInt32(ds.Tables[1].Rows[0][0].ToString());

            return "" + @TranCount + "," + CompCount;
        }

        //============================== Preferences ===============================//
        [WebMethod]
        public static string GetPreferences()
        {
            string Json = "";
            try
            {
                DataSet ds = DB.GetDS("[dbo].[GEN_MST_Get_PreferenceGroups_ForCycleCount] ", false);
                Json = JsonConvert.SerializeObject(ds);
            }
            catch (Exception ex)
            {
                Json = "Failed";
            }
            return Json;
        }
        [WebMethod]
        public static string SETPreferences(int UserID, string Inxml)
        {
            string Status = "";
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(" Exec [dbo].[USP_SET_GEN_TRN_Preferences] ");
                sb.Append(" @inputDataXml='" + Inxml + "'");
                sb.Append(" ,@LoggedUserID=" + UserID);
                //sb.ToString();
                DB.ExecuteSQL(sb.ToString());
                Status = "success";
            }
            catch (Exception ex)
            {
                Status = "Failed";
            }
            return Status;
        }

        //============================== Preferences ===============================//
    }
}