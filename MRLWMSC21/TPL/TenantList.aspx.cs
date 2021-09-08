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

namespace MRLWMSC21.TPL
{
    public partial class TenantList : System.Web.UI.Page
    {
        //======================= Added By M.D.Prasad For View Only Condition ======================//
        public static CustomPrincipal cp1 = HttpContext.Current.User as CustomPrincipal;
        public static String[] UserRole;
        public static String UserRoledat="";
        public static int UserTypeID;
        //======================= Added By M.D.Prasad For View Only Condition ======================//
        protected void Page_PreInit(object sender, EventArgs e)
        {
            cp1 = HttpContext.Current.User as CustomPrincipal;

        }
        protected void Page_Load(object sender, EventArgs e)
        {
            cp1 = HttpContext.Current.User as CustomPrincipal;
            if (!IsPostBack)
            {
                DesignLogic.SetInnerPageSubHeading(this.Page, "Tenant");
                //======================= Added By M.D.Prasad For View Only Condition ======================//
                UserRole = cp1.Roles;
                UserRoledat = "";
                for (int i = 0; i < UserRole.Length; i++)
                {
                    UserRoledat = UserRoledat + UserRole[i].ToString() + ",";
                }
                UserTypeID = cp1.UserTypeID;
                //======================= Added By M.D.Prasad For View Only Condition ======================//
            }

        }
        [WebMethod]
        public static string GetTenantList(string Search,string orderbytext,int orderby)
        {

            string Json = "";
            string Tenantid = null;
            string selectMMList;
            try
            {               

                CustomPrincipal cp = HttpContext.Current.User as CustomPrincipal;

                selectMMList = "EXEC sp_TPL_GetTenantRegistrationDetails @orderby="+ orderby + ",@orderbyText=" + DB.SQuote(orderbytext) +",@searchData=" + DB.SQuote(Search) +",@TenantID=" + 0;

                selectMMList = selectMMList + ",@AccountID_New = " + cp.AccountID.ToString() + ",@UserTypeID_New = " + cp.UserTypeID.ToString() + ",@TenantID_New = " + cp.TenantID.ToString() + ",@UserID_New=" + cp.UserID.ToString();

                DataSet ds = DB.GetDS(selectMMList, false);
                Json = JsonConvert.SerializeObject(ds);
            }
            catch (Exception ex)
            {
                Json = "Failed";
            }
            return Json;
        }

        [WebMethod]
        public static string DeleteTenant(string StrId)
        {
            CustomPrincipal customprinciple = HttpContext.Current.User as CustomPrincipal;
            string Status;
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(" Exec [dbo].[GEN_DELETE_Customer]");
                sb.Append("@PK=" + StrId);
                sb.Append("@LoggedInUserID=" + customprinciple.UserID.ToString());
                DB.ExecuteSQL(sb.ToString());
                Status = "success";
            }
            catch (Exception ex)
            {
                Status = "Failed";
            }

            return Status;
        }
    }
}