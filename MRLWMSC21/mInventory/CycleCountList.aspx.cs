using MRLWMSC21Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MRLWMSC21.mInventory
{
    public partial class CycleCountList : System.Web.UI.Page
    {
        public CustomPrincipal cp = HttpContext.Current.User as CustomPrincipal;
        public static CustomPrincipal cp1 = HttpContext.Current.User as CustomPrincipal;
        protected void page_PreInit(object sender, EventArgs e)
        {
            Page.Theme = "Inventory";
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // DesignLogic.SetInnerPageSubHeading(this.Page, "Cycle Count List");
            }
        }

        [WebMethod]
        public static string GetList(int Id)
        {
             cp1= HttpContext.Current.User as CustomPrincipal;
            try
            {
                CustomPrincipal cp = HttpContext.Current.User as CustomPrincipal;
                DataSet ds = DB.GetDS("EXEC [dbo].[SP_GET_CCM_CNF_AccountCycleCounts_List] @AM_MST_Account_ID=" + cp.AccountID+",@UserId = "+cp.UserID, false);
                return JsonConvert.SerializeObject(ds);
            }
            catch (Exception ex)
            {
                return "Error : " + ex;
            }
        }

        [WebMethod]
        public static string DeleteItemsById(string ID)
        {
            try
            {
                string SP_Del = "[dbo].[USP_Delete_CCM_CNF_AccountCycleCounts]";
                CustomPrincipal cp1 = HttpContext.Current.User as CustomPrincipal;
                StringBuilder sb = new StringBuilder();

                sb.Append(" @PK=" + ID + ", @LoggedInUserID=" + cp1.UserID + ", @UTCTimestamp=" + DB.SQuote(DateTime.UtcNow.ToString("dd-MMM-yyyy hh:mm:ss")));
                DB.ExecuteSQL("EXEC " + SP_Del + " " + sb.ToString());
                return "Success";
            }
            catch (Exception ex)
            {
                return "Error : " + ex;
            }
        }

        [WebMethod]
        public static string GetCounts(int CCID)
        {
             cp1= HttpContext.Current.User as CustomPrincipal;
            try
            {
                StringBuilder sb1 = new StringBuilder();

                sb1.Append(" SELECT COUNT(*) TranCount FROM CCM_TRN_CycleCounts WHERE CCM_CNF_AccountCycleCount_ID =" + CCID + " AND IsActive=1 AND IsDeleted=0 AND CCM_MST_CycleCountStatus_ID=2 ");
                sb1.Append(" SELECT COUNT(*) Completed FROM CCM_TRN_CycleCounts WHERE CCM_CNF_AccountCycleCount_ID=" + CCID + " AND CCM_MST_CycleCountStatus_ID=4 AND IsActive=1 AND IsDeleted=0; ");

                DataSet ds = DB.GetDS(sb1.ToString(), false);
                int TranCount = Convert.ToInt32(ds.Tables[0].Rows[0][0].ToString());
                int CompCount = Convert.ToInt32(ds.Tables[1].Rows[0][0].ToString());

                return "" + @TranCount + "," + CompCount;
            }
            catch (Exception ex)
            {
                return "Error : " + ex;
            }
        }
    }
}