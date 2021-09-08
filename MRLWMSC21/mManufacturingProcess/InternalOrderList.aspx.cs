using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using MRLWMSC21Common;

namespace MRLWMSC21.mManufacturingProcess
{
    //Author    :   Gvd Prasad
    //Created On:   12-Feb-2014
    //Use case ID:  NC Order List_UC_012

    public partial class InternalOrderList : System.Web.UI.Page
    {
        public CustomPrincipal cp = HttpContext.Current.User as CustomPrincipal;

        protected void Page_PreInit(object sender, EventArgs e)
        {
            Page.Theme = "Inbound";

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!cp.IsInAnyRoles(CommonLogic.GetRolesAllowed(CommonLogic.GetRolesAllowedForthisPage("NC Order List"))))
            {
                //test case ID (TC_007)
                //Only authorized user can access NC order list page

                Response.Redirect("../Login.aspx?eid=6");
            }
            if (!IsPostBack)
            {
                DesignLogic.SetInnerPageSubHeading(this.Page, "NC Order List");

                ViewState["InternalOrderList"] = "EXEC [dbo].[sp_MFG_GetInternalOrderHeaderList] @IORefNo=''";
                Build_InternalOrderList(Build_InternalOrderList());
            }
        }

        protected void lnkGetData_Click(object sender, EventArgs e)
        {
            //test case ID (TC_009)
            //Searches for the NC details with the provided NC ref. number

            String InternalOrederRefNumber = txtIORefNoSearch.Text.Trim();
            if (InternalOrederRefNumber == "NC Ref. No...")
                InternalOrederRefNumber = "";
            ViewState["InternalOrderList"] = "EXEC [dbo].[sp_MFG_GetInternalOrderHeaderList] @IORefNo='" + InternalOrederRefNumber + "'";
            Build_InternalOrderList(Build_InternalOrderList());
        }

        private DataSet Build_InternalOrderList()
        {
            DataSet dsInternalList = null;
            String cmdInternalList = ViewState["InternalOrderList"].ToString();
            try
            {
                dsInternalList = DB.GetDS(cmdInternalList, false);
            }
            catch (Exception ex)
            {
                resetError("Error while loading",true);
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
            }
            return dsInternalList;
        }

        private void Build_InternalOrderList(DataSet dsInternalList)
        {
            gvInternalOrderList.DataSource = dsInternalList;
            gvInternalOrderList.DataBind();
            dsInternalList.Dispose();
        }

        protected void gvInternalOrderList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvInternalOrderList.PageIndex = e.NewPageIndex;
            Build_InternalOrderList(Build_InternalOrderList());
        }
                
        public override void VerifyRenderingInServerForm(Control control)
        {
            /* Verifies that the control is rendered */
        }

        protected void resetError(string error, bool isError)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "test", "showStickyToast(" + (isError.ToString().ToLower() == "true" ? "false" : "true") + ",\"" + error + "\");", true);
        }

        protected void gvInternalOrderList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow && !(e.Row.RowState == DataControlRowState.Edit))
            {
                DataRow row = ((DataRowView)e.Row.DataItem).Row;
                if (row["IsDeficiency"].ToString() == "1")
                    e.Row.CssClass = "DeficiencyRow";
            }
        }

        protected void imgbtngvInternalOrderList_Click(object sender, ImageClickEventArgs e)
        {
            //test case ID (TC_013)
            //export to excel

            try
            {

                String[] hiddencolumns = { "" };
                gvInternalOrderList.AllowPaging = false;
                Build_InternalOrderList(Build_InternalOrderList());
                CommonLogic.ExporttoExcel(gvInternalOrderList, "InternalOrderList", hiddencolumns);
            }
            catch (Exception ex)
            {
                resetError("Error While Exporting Data", true);
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
            }
        }

    }
}