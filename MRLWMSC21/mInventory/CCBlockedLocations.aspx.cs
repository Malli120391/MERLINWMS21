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

namespace MRLWMSC21.mInventory
{
    //============== Created By M.D.Prasad On 05-May-2020 =======================//
    public partial class CCBlockedLocations : System.Web.UI.Page
    {
        public static CustomPrincipal cp = HttpContext.Current.User as CustomPrincipal;
        protected void page_PreInit(object sender, EventArgs e)
        {
            Page.Theme = "Inventory";
            cp = HttpContext.Current.User as CustomPrincipal;
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //DesignLogic.SetInnerPageSubHeading(this.Page, "Cycle Count Blocked Locations");
            }
        }

        [WebMethod]
        public static string GetList(string ACID, string CCTID)
        {
            try
            {
                DataSet ds = DB.GetDS("EXEC [dbo].[SP_GET_CycleCount_BlockedLocations] @CCM_CNF_AccountCycleCount_ID=" + ACID + ",@AM_MST_Account_ID=" + cp.AccountID + ",@CCM_TRN_CycleCount_ID=" + CCTID, false);
                return JsonConvert.SerializeObject(ds);
                //SET SP ewms.USP_SET_CCM_CNF_AccountCycleCounts
            }
            catch (Exception ex)
            {
                return "Error : " + ex;
            }
        }
    }
}