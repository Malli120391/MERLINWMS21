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
// Usecase Ref.: Workstation Type List_UC_001
// DevelopedBy : Naresh P
// CreatedOn   : 05/05/2014
// Modified On : 24/03/2015

namespace MRLWMSC21.mManufacturingProcess
{
    public partial class WorkGroupList : System.Web.UI.Page
    {
        CustomPrincipal cp = HttpContext.Current.User as CustomPrincipal;

        String WCSql = "";


        protected void Page_PreInit(object sender, EventArgs e)
        {
            Page.Theme = "Inbound";
        }


        protected void Page_Load(object sender, EventArgs e)
        {


            if (!cp.IsInAnyRoles(CommonLogic.GetRolesAllowed(CommonLogic.GetRolesAllowedForthisPage("Workstation Type List"))))
            {
                Response.Redirect("../Login.aspx?eid=6");
            }

            if (!IsPostBack)
            {

                DesignLogic.SetInnerPageSubHeading(this.Page, "Workstation Type List");

                WCSql = "EXEC [sp_MFG_GetWorkCenterGroupList]  @WorkCenterGroup=''" ;

                ViewState["WCGListSql"] = WCSql;


                this.gvWCList_buildGridData(gvWCList_buildGridData());
            }

        }


        protected DataSet gvWCList_buildGridData()
        {
            lblRecordCount.Text = "";
            string sql = ViewState["WCGListSql"].ToString();

            DataSet ds = DB.GetDS(sql, false);

            lblRecordCount.Text = "Total Items  [" + ds.Tables[0].Rows.Count.ToString() + "]";



            if (lblRecordCount.Text == "[0]")
            {
                lblRecordCount.Text = "";
            }


            return ds;
        }


        protected void gvWCList_buildGridData(DataSet ds)
        {
            gvWCGList.DataSource = ds.Tables[0];
            gvWCGList.DataBind();
            ds.Dispose();
        }

        protected void gvWCGList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            
            gvWCGList.PageIndex = e.NewPageIndex;
            gvWCGList.EditIndex = -1;
            this.gvWCList_buildGridData(this.gvWCList_buildGridData());
        }

        protected void lnkSearch_Click(object sender, EventArgs e)
        {
            if (txtSearchWorkCenter.Text == "Search Workstation Type...")
                txtSearchWorkCenter.Text = "";

            WCSql = "EXEC [sp_MFG_GetWorkCenterGroupList]  @WorkCenterGroup=" + DB.SQuote(txtSearchWorkCenter.Text);

            ViewState["WCGListSql"] = WCSql;


            this.gvWCList_buildGridData(gvWCList_buildGridData());
        }

        protected void lnkDeletePOInvItem_Click(object sender, EventArgs e)
        {


            string rfidIDs = "";

            bool chkBox = false;

            StringBuilder sqlDeleteString = new StringBuilder();
            sqlDeleteString.Append("BEGIN TRAN ");

            //'Navigate through each row in the GridView for checkbox items
            foreach (GridViewRow gv in gvWCGList.Rows)
            {

                CheckBox isDelete = (CheckBox)gv.FindControl("chkIsDeletePOInvItems");
                if (isDelete.Checked)
                {
                    chkBox = true;
                    // Concatenate GridView items with comma for SQL Delete
                    rfidIDs = ((Literal)gv.FindControl("ltWorkCenterGroupID")).Text.ToString();



                    if (rfidIDs != "")
                    {

                        if (DB.GetSqlN("select top 1 PO_WC.ProductionOrder_WorkCenterID  AS N from MFG_ProductionOrder_WorkCenter PO_WC JOIN MFG_WorkCenter WC ON WC.WorkCenterID=PO_WC.WorkCenterID JOIN MFG_WorkCenterGroup WG ON WG.WorkCenterGroupID=WC.WorkCenterGroupID where PO_WC.IsActive=1 AND PO_WC.IsDeleted=0  AND   WG.WorkCenterGroupID=" + rfidIDs) != 0)
                        {
                            resetError("This workstation type is configured in production process you cannot modify these details ", true);
                            return;
                        }

                        sqlDeleteString.Append(" Delete from MFG_WorkCenterGroupFinishedProducts Where WorkCenterGroupID=" + rfidIDs + ";");
                        sqlDeleteString.Append(" Delete from MFG_WorkCenterGroup Where WorkCenterGroupID=" + rfidIDs + ";");
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
                    resetError("Successfully deleted the selected line items", false);
                }
                

            }
            catch (Exception ex)
            {
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
                resetError("Error while deleting selected line items " , true);
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

        protected void imgbtngvWCGList_Click(object sender, ImageClickEventArgs e)
        {

            String[] hiddencolumns = { "Delete", ""};
            gvWCGList.AllowPaging = false;
            this.gvWCList_buildGridData(this.gvWCList_buildGridData());
            CommonLogic.ExporttoExcel(gvWCGList, "WorkstationType List", hiddencolumns);
        }
        // This is For Export tot Excel rendering Control
        public override void VerifyRenderingInServerForm(Control control)
        {
            /* Verifies that the control is rendered */
        }
    }
}