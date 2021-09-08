using MRLWMSC21Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MRLWMSC21.TPL
{
    public partial class AccountList : System.Web.UI.Page
    {
        public CustomPrincipal cp = HttpContext.Current.User as CustomPrincipal;
        string AccountListcmd = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
           // pnlHeaderRow.DefaultButton = lnkGetData.UniqueID;
            AccountListcmd = "EXEC [dbo].[USP_GEN_GetAccountInfo]";

            AccountListcmd = AccountListcmd + "@UserTypeID_New = " + cp.UserTypeID.ToString() + ",@TenantID_New = " + cp.TenantID.ToString() + ",@UserID_New=" + cp.UserID.ToString() + (hifAccount.Value.ToString().Equals("0") ? (cp.AccountID.ToString().Equals("0") ? "" : ", @AccountID_New = " + cp.AccountID.ToString()) : ", @AccountID_New = " + hifAccount.Value.ToString());

            if (!IsPostBack)
            {
                DesignLogic.SetInnerPageSubHeading(this.Page, "Account");
                //_sqlCommand = "EXEC [dbo].[sp_EAM_GetJobOrderList]";
                LoadAccounts(AccountListcmd);
                ValidateAccount(AccountListcmd);
                if (CommonLogic.QueryString("statid") == "Createsuccess")
                {
                    resetError("Successfully created", false);
                }
                if (CommonLogic.QueryString("statid") == "Updatesuccess")
                {
                    resetError("Successfully updated", false);
                }
            }
           
        }
        protected void resetError(string error, bool isError)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "showStickyToast", "showStickyToast(" + (isError.ToString().ToLower() == "true" ? "false" : "true") + ",\"" + error + "\");", true);
        }
        public void LoadAccounts(string sqlCommand)
        {
           
            gvACCList.DataSource = DB.GetDS(sqlCommand, false).Tables[0];
            gvACCList.DataBind();
  
        }

        public void ValidateAccount(string sqlCommand)
        {
            int Accountid = 0;
            string query = "";
            query = sqlCommand;
            DataSet ds = DB.GetDS(query, false);
            if (ds.Tables[0].Rows.Count != 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    Accountid = Convert.ToInt32(row["AccountID"].ToString());

                    if (Accountid == cp.AccountID &&Accountid!=0)
                    { 
                        lnkAddAccount.Visible = false;
                    }
                }
            }
        }
        protected void gvACCList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

            gvACCList.PageIndex = e.NewPageIndex;
            gvACCList.EditIndex = -1;
            LoadAccounts(AccountListcmd);
        }
        protected void lnkGetData_Click(object sender, EventArgs e)
        {
            string account = hifAccount.Value;
            if(txtAccount.Text=="")
            {
                account = "null";
            }
            try
            {

                // String sql = "EXEC [dbo].[USP_GEN_GetAccountInfo] @AccountID =" + Convert.ToInt32(account);

                String sql = "EXEC [dbo].[USP_GEN_GetAccountInfo] " + "@UserTypeID_New = " + cp.UserTypeID.ToString() + ",@TenantID_New = " + cp.TenantID.ToString() + ",@UserID_New=" + cp.UserID.ToString() + (txtAccount.Text.ToString().Contains("Search") ||   hifAccount.Value.ToString().Equals("0") ? (cp.AccountID.ToString().Equals("0") ? "" : ", @AccountID_New = " + cp.AccountID.ToString()) : ", @AccountID = " + account);
                DataSet dsACCList = DB.GetDS(sql, false);
                gvACCList.DataSource = dsACCList;
                gvACCList.DataBind();
                dsACCList.Dispose();
                if (gvACCList.Rows.Count == 0)
                {

                }
            }
            catch (Exception ex)
            {
                //CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
            }

        }

        protected void gvACCList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if ((e.Row.RowType == DataControlRowType.DataRow) && (e.Row.RowState == DataControlRowState.Normal) || (e.Row.RowState == DataControlRowState.Alternate))
            {
                //======================= Added By M.D.Prasad For View Only Condition ======================//
                string[] hh = cp.Roles;
                for (int i = 0; i < hh.Length; i++)
                {
                    if (cp.UserTypeID == 3 && Convert.ToInt32(hh[i]) == 4)
                    {
                        gvACCList.Columns[6].Visible = false;
                    }
                }
                //======================= Added By M.D.Prasad For View Only Condition ======================//
            }
        }
    }
}