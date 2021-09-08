using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Text;
using MRLWMSC21Common;
using System.Threading;


// Module Name : Manufacturing
// Usecase Ref.: Workstation List_UC_003
// DevelopedBy : Naresh P
// CreatedOn   : 05/05/2014
// Modified On : 24/03/2015


namespace MRLWMSC21.mManufacturingProcess
{
    public partial class WorkCenterList : System.Web.UI.Page
    {
        CustomPrincipal cp = HttpContext.Current.User as CustomPrincipal;

        String WCSql = "";

        protected void Page_PreInit(object sender, EventArgs e)
        {
            Page.Theme = "Inbound";
        }


        protected void Page_Load(object sender, EventArgs e)
        {


            if (!cp.IsInAnyRoles(CommonLogic.GetRolesAllowed(CommonLogic.GetRolesAllowedForthisPage("Workstation List"))))
            {
                Response.Redirect("../Login.aspx?eid=6");
            }

            if (!IsPostBack) {

                DesignLogic.SetInnerPageSubHeading(this.Page, "Workstation List");

                LoadDropDown(ddlWrkcGroup, "select WorkCenterGroup,WorkCenterGroupID from MFG_WorkCenterGroup  where IsActive=1 AND IsDeleted=0", "WorkCenterGroup", "WorkCenterGroupID", "ALL");

                WCSql = "EXEC [sp_MFG_GetWorkCenterList]  @WorkCenter='',@WorkCenterGroupID="+ddlWrkcGroup.SelectedValue;

                ViewState["WCListSql"] = WCSql;

                this.gvWCList_buildGridData(gvWCList_buildGridData());
            }

        }


        protected DataSet gvWCList_buildGridData()
        {
            lblRecordCount.Text = "";
            string sql = ViewState["WCListSql"].ToString();
            
            DataSet ds = DB.GetDS(sql, false);

            lblRecordCount.Text = "Total Items [" + ds.Tables[0].Rows.Count.ToString() + "]";


            
            if (lblRecordCount.Text == "[0]")
            {
                lblRecordCount.Text = "";
            }
             

            return ds;
        }

       
        protected void gvWCList_buildGridData(DataSet ds)
        {
            gvWCList.DataSource = ds.Tables[0];
            gvWCList.DataBind();
            ds.Dispose();
        }



        protected void gvWCList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvWCList.PageIndex = e.NewPageIndex;
            gvWCList.EditIndex = -1;
            this.gvWCList_buildGridData(this.gvWCList_buildGridData());
        }

        protected void lnkSearch_Click(object sender, EventArgs e)
        {

            if (txtSearchWorkCenter.Text == "Search Workstation...")
                txtSearchWorkCenter.Text = "";


            WCSql = "EXEC [sp_MFG_GetWorkCenterList]  @WorkCenter=" + DB.SQuote(txtSearchWorkCenter.Text) + ",@WorkCenterGroupID="+ddlWrkcGroup.SelectedValue ;

            ViewState["WCListSql"] = WCSql;

            this.gvWCList_buildGridData(gvWCList_buildGridData());

            txtSearchWorkCenter.Text = "Search Workstation...";
        }

        protected void lnkDeletePOInvItem_Click(object sender, EventArgs e)
        {


            string rfidIDs = "";

            bool chkBox = false;

            StringBuilder sqlDeleteString = new StringBuilder();
            sqlDeleteString.Append("BEGIN TRAN ");

            //'Navigate through each row in the GridView for checkbox items
            foreach (GridViewRow gv in gvWCList.Rows)
            {

                CheckBox isDelete = (CheckBox)gv.FindControl("chkIsDeletePOInvItems");
                if (isDelete.Checked)
                {
                    chkBox = true;
                    // Concatenate GridView items with comma for SQL Delete
                    rfidIDs = ((Literal)gv.FindControl("ltWorkCenterID")).Text.ToString();



                    if (rfidIDs != "")
                    {

                        if (DB.GetSqlN("select top 1 ProductionOrder_WorkCenterID  AS N from MFG_ProductionOrder_WorkCenter PO_WC where PO_WC.IsActive=1 AND PO_WC.IsDeleted=0  AND   PO_WC.WorkCenterID=" + rfidIDs) != 0)
                        {
                            resetError("This workstation is configured in production process you cannot modify these details ", true);
                            return;
                        }



                        sqlDeleteString.Append("Delete  from MFG_WorkCenter_GEN_User where WorkCenterID=" + rfidIDs + ";");
                        sqlDeleteString.Append(" Delete from MFG_WorkCenter Where WorkCenterID=" + rfidIDs + ";");
                      
                        
                    }

                }
            }


            sqlDeleteString.Append(" COMMIT ");


            // Execute SQL Query only if checkboxes are checked to avoid any error with initial null string
            try
            {

                if (chkBox)
                {
                    DB.ExecuteSQL(sqlDeleteString.ToString());
                }
                resetError("Successfully deleted the selected line items", false);

            }
            catch (Exception ex)
            {
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
                resetError("Error deleting workstation details", true);
            }

            this.gvWCList_buildGridData(this.gvWCList_buildGridData());


        }

        protected void resetError(string error, bool isError)
        {
            /*
            string str = "<font class=\"noticeMsg\">NOTICE:</font>&nbsp;&nbsp;&nbsp;";
            if (isError)
                str = "<font class=\"errorMsg\">ERROR:</font>&nbsp;&nbsp;&nbsp;";

            if (error.Length > 0)
                str += error + "";
            else
                str = "";
            lblStatusMessage.Text = str;*/

            ScriptManager.RegisterStartupScript(this, this.GetType(), "test", "showStickyToast(" + (isError.ToString().ToLower() == "true" ? "false" : "true") + ",\"" + error + "\");", true);


        }


        public void LoadDropDown(DropDownList ddllist, String SqlQry, String strListName, String strListValue, string defaultValue)
        {
            // Initially clear all DropDownList Items
            ddllist.Items.Clear();
            ddllist.Items.Add(new ListItem(defaultValue, "0")); // Add Default value to the dropdown

            IDataReader rsList = DB.GetRS(SqlQry);

            while (rsList.Read())
            {

                ddllist.Items.Add(new ListItem(rsList[strListName].ToString(), rsList[strListValue].ToString()));

            }

            rsList.Close();
        }

        protected void imgbtngvWCList_Click(object sender, ImageClickEventArgs e)
        {
            String[] hiddencolumns = { "Delete","" };
            gvWCList.AllowPaging = false;
            this.gvWCList_buildGridData(gvWCList_buildGridData());
            CommonLogic.ExporttoExcel(gvWCList, "Workstation List", hiddencolumns);
        }
        public override void VerifyRenderingInServerForm(Control control)
        {
            /* Verifies that the control is rendered */
        }
    }
}