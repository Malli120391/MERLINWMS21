using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Data;
using System.Data.SqlClient;

using MRLWMSC21Common;

namespace MRLWMSC21.mOrders
{
    //Author    :   Gvd Prasad
    //Created On:   05-Apl-2014

    public partial class ToolIssueList : System.Web.UI.Page
    {

        public CustomPrincipal cp = HttpContext.Current.User as CustomPrincipal;
        
        protected void page_PreInit(object sender, EventArgs e)
        {
            Page.Theme = "Orders";
        }
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) {


                if (!cp.IsInAnyRoles(CommonLogic.GetRolesAllowed(CommonLogic.GetRolesAllowedForthisPage("Bill of Material"))))
                {
                    Response.Redirect("../Login.aspx?eid=6");
                }

                DesignLogic.SetInnerPageSubHeading(this.Page, "Tool Issues List");

                ViewState["IssueListSql"] = "EXEC [dbo].[sp_INV_GetToolDetails]";

                gvToolIssueList_buildGridData(gvToolIssueList_buildGridData());

            }
        }
        
        #region ------------------------- Tool Issue List -----------------------------
        
        protected DataSet gvToolIssueList_buildGridData()
        {
            DataSet dsGetToolList = null;
            string cmdGetToolList = ViewState["IssueListSql"].ToString();
            try
            {
                dsGetToolList = DB.GetDS(cmdGetToolList, false);
            }
            catch (Exception ex)
            {
                resetError("Error while loading",true);
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
            }
            return dsGetToolList;

        }

        protected void gvToolIssueList_buildGridData(DataSet ds)
        {
            gvToolIssueList.DataSource = ds;
            gvToolIssueList.DataBind();
            ds.Dispose();
        }
        
        protected void gvToolIssueList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

            gvToolIssueList.PageIndex = e.NewPageIndex;
            gvToolIssueList.EditIndex = -1;
            this.gvToolIssueList_buildGridData(this.gvToolIssueList_buildGridData());

        }
        
        protected void lnkDelSearch_Click(object sender, EventArgs e)
        {
            /*
            if (txtDelDocNumber.Text == "Search Delv. Doc.#...")
                txtDelDocNumber.Text = "";

            if (txtDelDocNumber.Text != "")
            {
                ViewState["DeliveriesInProcessSQLString"] = this.DeliveriesInProcessSQL + "@OBDNumber=" + DB.SQuote(txtDelDocNumber.Text) + ",@WarehouseIDs=" + DB.SQuote(String.Join(",", cp.Warehouses));
            }
            else
            {
                ViewState["DeliveriesInProcessSQLString"] = this.DeliveriesInProcessSQL + "@OBDNumber=NULL,@WarehouseIDs=" + DB.SQuote(String.Join(",", cp.Warehouses));
            }

            this.ObdPending_buildGridData(this.OBDPending_buildGridData());
            txtDelDocNumber.Text = "Search Delv. Doc.#...";
             */

        }
        
        protected void lnkIssueSearch_Click(object sender, EventArgs e)
        {
            String EmployeeID = "NULL";
            String MaterialMasterID = "NULL";
            if (atcEmployee.Text != "Search Employee ...")
            {
                MaterialMasterID = hifEmployee.Value;
            }

            if (atcEditMCode.Text != "Search Tool ...")
            {
                MaterialMasterID = hifmmid.Value;
            }

            ViewState["IssueListSql"] = " EXEC [dbo].[sp_INV_GetToolDetails]  @MaterialMasterID=" + MaterialMasterID + ",@CustomerID=" + EmployeeID;
            DataSet dsGetToolList = null;
            try
            {
                dsGetToolList = DB.GetDS(ViewState["IssueListSql"].ToString(), false);
            }
            catch (Exception ex)
            {
                resetError("Error while loading",true);
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
                return;
            }

            if (dsGetToolList.Tables[0].Rows.Count == 0)
            {
                resetError("No issue is available", true);
            }
            this.gvToolIssueList_buildGridData(this.gvToolIssueList_buildGridData());
        }
        
        protected void resetError(string error, bool isError)
        {

            /* string str = "<font class=\"noticeMsg\">NOTICE:</font>&nbsp;&nbsp;&nbsp;";
             if (isError)
                 str = "<font class=\"errorMsg\">ERROR:</font>&nbsp;&nbsp;&nbsp;";

             if (error.Length > 0)
                 str += error + "";
             else
                 str = "";


             lblGroupLabelStatus.Text = str;*/
            ScriptManager.RegisterStartupScript(this, this.GetType(), "test", "showStickyToast(" + (isError.ToString().ToLower() == "true" ? "false" : "true") + ",\"" + error + "\");", true);

        }

        #endregion ------------------------- Tool Issue List -----------------------------

    }
}