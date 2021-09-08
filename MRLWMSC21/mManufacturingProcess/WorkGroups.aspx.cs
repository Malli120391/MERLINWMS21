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

// Module Name : Manufacturing
// Usecase Ref.: Workstation Type Details_UC_002
// DevelopedBy : Naresh P
// CreatedOn   : 05/05/2014
// Modified On : 24/03/2015


namespace MRLWMSC21.mManufacturingProcess
{
    public partial class WorkGroups : System.Web.UI.Page
    {
        CustomPrincipal cp = HttpContext.Current.User as CustomPrincipal;

        String WCGSqlString= " " ; //"EXEC [sp_MFG_GetWorkCenterGroupFinishedProducts] @WorkCenterGroupID=1"
        
        protected void Page_PreInit(object sender, EventArgs e)
        {
            Page.Theme = "Inbound";
        }


        protected void Page_Load(object sender, EventArgs e)
        {


            if (!cp.IsInAnyRoles(CommonLogic.GetRolesAllowed(CommonLogic.GetRolesAllowedForthisPage("New Workstation Type"))))
            {
                Response.Redirect("../Login.aspx?eid=6");
            }


            ViewState["TenantID"] = cp.TenantID.ToString();

            if (CommonLogic.QueryString("update") == "success")
                lblStatus.Text="Successfully Updated";

            if (!IsPostBack) {
                DesignLogic.SetInnerPageSubHeading(this.Page, "Workstation Type Details");
                LoadDropDown(ddlCompany, "select SupplierID,SupplierName from MMT_Supplier where  IsActive=1 AND IsDeleted=0", "SupplierName", "SupplierID", "Select Company");


                if (CommonLogic.QueryString("wgid") != "0" && CommonLogic.QueryString("wgid") != "")
                {
                    LoadWCGroupDetails();

                    lnkCancel.Visible = false;

                    lnkWCGroup.Text = "Update" + CommonLogic.btnfaUpdate;

                    this.WCGSqlString = "EXEC [sp_MFG_GetWorkCenterGroupFinishedProducts] @WorkCenterGroupID=" + CommonLogic.QueryString("wgid");

                    ViewState["FinishedProductSQL"] = WCGSqlString;

                    this.gvFinishedProduct_buildGridData(this.gvFinishedProduct_buildGridData());

                }
                else {
                    lnkWCGroup.Text = "Create Workstation Type" + CommonLogic.btnfaSave;
                }


            }


            if (CommonLogic.QueryString("wgid") != "0" && CommonLogic.QueryString("wgid") != "")
            {
               // lnkAddFProduct.Visible = true;

               
            }
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

        public void LoadWCGroupDetails() {

            try
            {

                IDataReader drWC = DB.GetRS("select top 1 * from MFG_WorkCenterGroup  where IsActive=1 AND IsDeleted=0 AND WorkCenterGroupID=" + CommonLogic.QueryString("wgid"));

                drWC.Read();

                txtGroupName.Text = DB.RSField(drWC, "WorkCenterGroup");
                
                ddlCompany.SelectedValue = DB.RSFieldInt(drWC, "SupplierID").ToString();

                txtDescription.Text = DB.RSField(drWC, "Description");

                txtGroupRefNo.Text = DB.RSField(drWC, "WorkCenterGroupCode");

                drWC.Close();
            }
            catch (Exception ex)
            {
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
                resetError("Error while loading workstation type details", true);
            }
        }

        protected void lnkWCGroup_Click(object sender, EventArgs e)
        {

            Page.Validate("InitiateWG");

            if (!Page.IsValid)
                return;


            if (DB.GetSqlN("select top 1 PO_WC.ProductionOrder_WorkCenterID  AS N from MFG_ProductionOrder_WorkCenter PO_WC JOIN MFG_WorkCenter WC ON WC.WorkCenterID=PO_WC.WorkCenterID JOIN MFG_WorkCenterGroup WG ON WG.WorkCenterGroupID=WC.WorkCenterGroupID where PO_WC.IsActive=1 AND PO_WC.IsDeleted=0  AND   WG.WorkCenterGroupID=" + CommonLogic.IIF(CommonLogic.QueryString("wgid") != "", CommonLogic.QueryString("wgid"), "0")) != 0)
            {
                resetError("Production process is started, cannot modify these details ", true);
                return;
            }


            StringBuilder sbSqlString = new StringBuilder();


            sbSqlString.Append("DECLARE @UpdatedWorkCenterGroupID int  ");
            sbSqlString.Append("EXEC [sp_MFG_UpsertWorkCenterGroup]  @WorkCenterGroupID=" + CommonLogic.IIF(CommonLogic.QueryString("wgid") != "", CommonLogic.QueryString("wgid"), "0"));
            sbSqlString.Append(",@WorkCenterGroup= "+DB.SQuote(txtGroupName.Text));
            sbSqlString.Append(",@WorkCenterGroupCode= " + DB.SQuote(txtGroupRefNo.Text));
            
            sbSqlString.Append(",@SupplierID="+ CommonLogic.IIF(ddlCompany.SelectedValue!="0",ddlCompany.SelectedValue,"NULL"));
            sbSqlString.Append(",@Description="+DB.SQuote(txtDescription.Text));
            sbSqlString.Append(" ,@CreatedBy="+cp.UserID);
            sbSqlString.Append(" ,@NewWorkCenterGroupID = @UpdatedWorkCenterGroupID output select @UpdatedWorkCenterGroupID as N");

            int WorkCenterGroupID = 0;

            try
            {
                WorkCenterGroupID = DB.GetSqlN(sbSqlString.ToString());
            }
            catch (Exception ex)
            {
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
                resetError("Error while updating workstation type details", true);
            }

            Response.Redirect("WorkGroups.aspx?wgid=" + WorkCenterGroupID + "&update=success");

        }

        protected void lnkCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("WorkGroupList.aspx");
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


        #region ------------  Finished Product Grid  -----------------


        protected void lnkAddFProduct_Click(object sender, EventArgs e)
        {
            ViewState["gvFinishedProductIsInsert"] = false;

            StringBuilder sql = new StringBuilder(2500);

            try
            {
                gvFinishedProduct.EditIndex = 0; // If ORDER BY Clause in SQL on MMId is DESC
                DataSet dsParent = this.gvFinishedProduct_buildGridData();
                DataRow row = dsParent.Tables[0].NewRow();
                row["MCode"] = "";
                row["WorkCenterGroupFinishedProductsID"] = 0;
                row["SourceLocationID"] = 0;
                row["DestinationLocationID"] = 0;
                row["FailedProductionDestinationLocationID"] = 0;
                
                row["LineNumber"] = DB.GetSqlN("select Max(LineNumber) AS N from MFG_WorkCenterGroupFinishedProducts where WorkCenterGroupID="+CommonLogic.QueryString("wgid")) + 1;
                dsParent.Tables[0].Rows.InsertAt(row, 0);


                this.gvFinishedProduct_buildGridData(dsParent);
                this.resetErrorFProduct("Add New Line Item.", false);
                ViewState["gvFinishedProductIsInsert"] = true;
                

                

            }
            catch (Exception ex)
            {
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
                this.resetErrorFProduct("Error while updating <br/>" + ex.ToString(), true);
            }
        }

        protected DataSet gvFinishedProduct_buildGridData()
        {
            string sql = ViewState["FinishedProductSQL"].ToString();
            DataSet ds = DB.GetDS(sql, false);

            return ds;
        }

        protected void gvFinishedProduct_buildGridData(DataSet ds)
        {
            gvFinishedProduct.DataSource = ds.Tables[0];
            gvFinishedProduct.DataBind();
            ds.Dispose();
        }

        protected void gvFinishedProduct_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

        }

        protected void gvFinishedProduct_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.DataItem == null)
                return;

            if ((e.Row.RowState & DataControlRowState.Edit) == DataControlRowState.Edit)
            {
                //DropDownList ddlBOMRefNo = (DropDownList)e.Row.FindControl("ddlBOMRefNo");
                Literal ltBoMHederID = (Literal)e.Row.FindControl("ltBoMHederID");
               // LoadDropDown(ddlBOMRefNo, "select BOMRefNumber,BOMHeaderID from MFG_BOMHeader where IsActive=1 AND IsDeleted=0", "BOMRefNumber", "BOMHeaderID", "Select BOM");
              //  ddlBOMRefNo.SelectedValue = ltBoMHederID.Text;

                /*
                DropDownList ddlWCGroup = (DropDownList)e.Row.FindControl("ddlWCGroup");
                Literal ltHidWorkCenterGroupID = (Literal)e.Row.FindControl("ltHidWorkCenterGroupID");
                LoadDropDown(ddlWCGroup, "select WorkCenterGroup,WorkCenterGroupID from MFG_WorkCenterGroup where IsActive=1 AND IsDeleted=0", "WorkCenterGroup", "WorkCenterGroupID", "Select WorkCenterGroup");
                ddlWCGroup.SelectedValue = ltHidWorkCenterGroupID.Text;
                 */

                DropDownList ddlSrcLocations = (DropDownList)e.Row.FindControl("ddlSourceLocations");
                Literal ltSrcLocationID = (Literal)e.Row.FindControl("ltSrcLocationID");
                LoadDropDown(ddlSrcLocations, "select LocationName,MfgLocationID from MFG_Location where IsActive=1 AND IsDeleted=0", "LocationName", "MfgLocationID", "Select Location");
                ddlSrcLocations.SelectedValue = ltSrcLocationID.Text;

                DropDownList ddlDestLocations = (DropDownList)e.Row.FindControl("ddlDestinationLocations");
                Literal ltDestLocationID = (Literal)e.Row.FindControl("ltDestLocationID");
                LoadDropDown(ddlDestLocations, "select LocationName,MfgLocationID from MFG_Location where IsActive=1 AND IsDeleted=0", "LocationName", "MfgLocationID", "Select Location");
                ddlDestLocations.SelectedValue = ltDestLocationID.Text;

                DropDownList ddlFaildDestLocations = (DropDownList)e.Row.FindControl("ddlFaildDestinationLocations");
                Literal ltFaildDestLocationID = (Literal)e.Row.FindControl("ltFaildDestLocationID");
                LoadDropDown(ddlFaildDestLocations, "select LocationName,MfgLocationID from MFG_Location where IsActive=1 AND IsDeleted=0", "LocationName", "MfgLocationID", "Select Location");
                ddlFaildDestLocations.SelectedValue = ltFaildDestLocationID.Text;

            }
        }

        protected void gvFinishedProduct_RowCommand(object sender, GridViewCommandEventArgs e)
        {

            if (e.CommandName == "DeleteItem")
            {
                ViewState["gvFinishedProductIsInsert"] = false;
                gvFinishedProduct.EditIndex = -1;
                int iden = Localization.ParseNativeInt(e.CommandArgument.ToString());
                this.deleteRowPermE(iden);
            }
        }

      

        protected void gvFinishedProduct_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            

            ViewState["gvFinishedProductIsInsert"] = false;
            GridViewRow row = gvFinishedProduct.Rows[e.RowIndex];

            if (row != null)
            {

                String lthidWorkCenterGroupFinishedProductsID = ((Literal)row.FindControl("lthidWorkCenterGroupFinishedProductsID")).Text;
                String ltLineNumber = ((Literal)row.FindControl("ltLineNumber")).Text;
                String ddlBOMRefNo = ((DropDownList)row.FindControl("ddlBOMRefNo")).SelectedValue;
              //  String ddlWCGroup = ((DropDownList)row.FindControl("ddlWCGroup")).SelectedValue;
                String txtMCode = ((TextBox)row.FindControl("txtMCode")).Text;
                String ddlSrcLocations = ((DropDownList)row.FindControl("ddlSourceLocations")).SelectedValue;
                String ddlDestLocations = ((DropDownList)row.FindControl("ddlDestinationLocations")).SelectedValue;
                String ddlFaildLocations = ((DropDownList)row.FindControl("ddlFaildDestinationLocations")).SelectedValue;

                int vMMID = CommonLogic.GetMMID(txtMCode);

                if (vMMID == 0)
                {
                    resetErrorFProduct("Invalid Part#",true);
                    return;
                }


                StringBuilder sbSqlUpdate = new StringBuilder();

                sbSqlUpdate.Append("EXEC  [dbo].[sp_MFG_UpsertWorkCenterGroupFinishedProducts]   ");
                sbSqlUpdate.Append(" @WorkCenterGroupFinishedProductsID="+lthidWorkCenterGroupFinishedProductsID);
                sbSqlUpdate.Append(" ,@LineNumber="+ltLineNumber);
                sbSqlUpdate.Append(" ,@BOMHeaderID=" + Request.Form[hifBoMRevID.ClientID].ToString());
                sbSqlUpdate.Append(" ,@WorkCenterGroupID="+CommonLogic.QueryString("wgid"));
                sbSqlUpdate.Append(" ,@FinishedProduct_MaterialMasterID="+vMMID);
                sbSqlUpdate.Append(" ,@SourceLocationID=" + ddlSrcLocations);
                sbSqlUpdate.Append(" ,@DestinationLocationID=" + ddlDestLocations);
                sbSqlUpdate.Append(" ,@FailedProductionDestinationLocationID=" + ddlFaildLocations);
                sbSqlUpdate.Append(" ,@CreatedBy="+cp.UserID);

                try
                {
                    DB.ExecuteSQL(sbSqlUpdate.ToString());

                    resetErrorFProduct("Successfully Updated", false);

                    gvFinishedProduct.EditIndex = -1;

                    this.gvFinishedProduct_buildGridData(this.gvFinishedProduct_buildGridData());

                   
                }
                catch (Exception ex) {
                    CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
                    resetErrorFProduct("Error while updating",true);
                }

                

            }
        }

        protected void gvFinishedProduct_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {

            if (Localization.ParseBoolean(ViewState["gvFinishedProductIsInsert"].ToString()))
            {
                GridViewRow row = gvFinishedProduct.Rows[e.RowIndex];
                if (row != null)
                {
                    int iden = Convert.ToInt32(((Literal)row.FindControl("lthidWorkCenterGroupFinishedProductsID")).Text);
                    deleteRowPermE(iden);

                }
            }

            ViewState["gvFinishedProductIsInsert"] = false;
            gvFinishedProduct.EditIndex = -1;
            this.gvFinishedProduct_buildGridData(this.gvFinishedProduct_buildGridData());
        }

        protected void gvFinishedProduct_RowEditing(object sender, GridViewEditEventArgs e)
        {
            ViewState["gvFinishedProductIsInsert"] = false;

            gvFinishedProduct.EditIndex = e.NewEditIndex;

            this.gvFinishedProduct_buildGridData(this.gvFinishedProduct_buildGridData());
        }

        protected void lnkDeletePOInvItem_Click(object sender, EventArgs e)
        {


            string rfidIDs = "";

            bool chkBox = false;

            StringBuilder sqlDeleteString = new StringBuilder();
            sqlDeleteString.Append("BEGIN TRAN ");

            //'Navigate through each row in the GridView for checkbox items
            foreach (GridViewRow gv in gvFinishedProduct.Rows)
            {

                CheckBox isDelete = (CheckBox)gv.FindControl("chkIsDeletePOInvItems");

                if (isDelete == null)
                    return;

                if (isDelete.Checked)
                {
                    chkBox = true;
                    // Concatenate GridView items with comma for SQL Delete
                    rfidIDs = ((Literal)gv.FindControl("lthidWorkCenterGroupFinishedProductsID")).Text.ToString();



                    if (rfidIDs != "")
                        sqlDeleteString.Append(" Delete from MFG_WorkCenterGroupFinishedProducts Where WorkCenterGroupFinishedProductsID=" + rfidIDs + ";");

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
                resetErrorFProduct("Successfully deleted the selected items", false);

            }
            catch (Exception ex)
            {
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
                resetErrorFProduct("Error while deleting" + ex.ToString(), true);
            }

            this.gvFinishedProduct_buildGridData(this.gvFinishedProduct_buildGridData());


        }

        protected void deleteRowPermE(int iden)
        {

            StringBuilder sql = new StringBuilder(2500);
            sql.Append("delete from MFG_WorkCenterGroupFinishedProducts where WorkCenterGroupFinishedProductsID=" + iden.ToString());
            try
            {
                DB.ExecuteSQL(sql.ToString());
                this.gvFinishedProduct_buildGridData(this.gvFinishedProduct_buildGridData());

            }
            catch (Exception ex)
            {
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
                this.resetErrorFProduct("Cannot delete from database: " + ex.ToString(), true);
            }
        }

        protected void resetErrorFProduct(string error, bool isError)
        {

            string str = "<font class=\"noticeMsg\">NOTICE:</font>&nbsp;&nbsp;&nbsp;";
            if (isError)
                str = "<font class=\"errorMsg\">ERROR:</font>&nbsp;&nbsp;&nbsp;";

            if (error.Length > 0)
                str += error + "";
            else
                str = "";

            lblFinshedProductStatus.Text = str + "</font>";

        }

        #endregion ------------  Finished Product Grid  -----------------
    }
}