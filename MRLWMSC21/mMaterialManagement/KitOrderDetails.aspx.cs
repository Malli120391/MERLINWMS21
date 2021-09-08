using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MRLWMSC21Common;

namespace MRLWMSC21.mMaterialManagement
{
    public partial class KitOrderDetails : System.Web.UI.Page
    {
        public CustomPrincipal cp = HttpContext.Current.User as CustomPrincipal;
        public string headerid;
        protected void Page_PreInit(object sender, EventArgs e)
        {
            cp = HttpContext.Current.User as CustomPrincipal;
            Page.Theme = "Orders";//_sit";
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                LoadDropDown(ddlkittypes, "select KitTypeID,KitType from MMT_KitType where IsActive=1 and IsDeleted=0 and  KitTypeID=2", "KitType", "KitTypeID", "Select KitType");
            

                if (CommonLogic.QueryString("jobid") != "")
                {
                    ViewState["jobid"] = CommonLogic.QueryString("jobid");
                    ViewState["deatils"] = "[SP_GetJobOrderHeaderDeatils] @JOBId = " + CommonLogic.QueryString("jobid");
                    lnkUpdate.Visible = false;
                    btnclear.Visible = false;
                    ddlkittypes.Enabled = false;
                    txtTenant.Enabled = false;
                    FillFormData();
                    


                }
                else
                {
                    ddlkittypes.Enabled = true;
                    txtTenant.Enabled = true;
                    lnkAddNewKitItems.Visible = false;
                    btninitiateorder.Visible = false;
                }
            }
          
            if (CommonLogic.QueryString("jobid") != "")
            {
                int status = DB.GetSqlN("select KitJobOrderStatusID as N from MMT_KitJobOrderHeader where KitJobOrderHeaderID=" + CommonLogic.QueryString("jobid"));
              
                if (status == 2)
                {
                    lnkAddNewKitItems.Visible = false;
                    btninitiateorder.Visible = false;
                }
                else if (status == 3)
                {
                    btninitiateorder.Visible = false;
                    lnkAddNewKitItems.Visible = false;
                }
                else
                {
                    lnkAddNewKitItems.Visible = true;
                    lnkAddNewKitItems.Visible = true;
                    btninitiateorder.Visible = true;
                }
                headerid = CommonLogic.QueryString("jobid");
            }
            else
            {
                headerid ="0";
            }

            
        }   
        public void FillFormData()
        {
            String sql = ViewState["deatils"].ToString();
            DataSet details = DB.GetDS(sql, false);
            try
            {
                foreach (DataRow row in details.Tables[0].Rows)
                {

                        txtrefno.Text = row["KitJobOrderRefNo"].ToString();
                    ddlkittypes.SelectedValue = (row["KitTypeID"]).ToString();
                    txtstatus.Text = (row["KitJobOrderStatus"]).ToString();
                    txtTenant.Text= row["tenant"].ToString();
                    hidTenantID.Value= row["TenantId"].ToString();
                    //Email = (row["email"]).ToString(),
                    //Password = (row["Password"]).ToString(),
                    //RoleId = Convert.ToInt32(row["UserRoleID"]),
                    //IsActive = Convert.ToBoolean(row["IsActive"])


                }
                setGridData();

            }
            catch (Exception e)
            {
            }
        }
        public void setGridData()
        {
            String sql = ViewState["deatils"].ToString();
            DataSet details = DB.GetDS(sql, false);
            deatilslist.DataSource = details.Tables[1];
            deatilslist.DataBind();
            deatilslist.Dispose();
        }
        protected void lnkUpdate_Click(object sender, EventArgs e)
        {
            if(ddlkittypes.SelectedValue.ToString()=="0")
            {
                resetError("Please Select Kit type", true);
                return;
            }
            if(txtTenant.Text.ToString().Trim()=="")
            {
                resetError("Please Select Tenat", true);
                return;
            }
            else if(hidTenantID.Value.ToString()=="")
            {
                resetError("Please Select Tenat", true);
                return;
            }
            int value = DB.GetSqlN("exec [SP_UpsertJobOrderHeader] @KitTypeId="+ ddlkittypes.SelectedValue.ToString()+ ",@userid="+cp.UserID+ ",@tenatId="+ hidTenantID.Value.ToString());
            if (value>0)
            {
                resetError("Succesfully Created", false);
                Response.Redirect("../mMaterialManagement/KitOrdersList.aspx", false);
            }
        }
        public void LoadDropDown(DropDownList ddllist, String SqlQry, String strListName, String strListValue, string defaultValue)
        {
            // Initially clear all DropDownList Items
            ddllist.Items.Clear();
           // ddllist.Items.Add(new ListItem(defaultValue, "0")); // Add Default value to the dropdown

            IDataReader rsList = DB.GetRS(SqlQry);

            while (rsList.Read())
            {

                ddllist.Items.Add(new ListItem(rsList[strListName].ToString(), rsList[strListValue].ToString()));

            }

            rsList.Close();
        }
        protected void resetError(string error, bool isError)
        {

            /*string str = "<font class=\"noticeMsg\">NOTICE:</font>&nbsp;&nbsp;&nbsp;";
            if (isError)
                str = "<font class=\"errorMsg\">ERROR:</font>&nbsp;&nbsp;&nbsp;";

            if (error.Length > 0)
                str += error + "";
            else
                str = "";


            lblStatus.Text = str;*/
            ScriptManager.RegisterStartupScript(this, this.GetType(), "test", "showStickyToast(" + (isError.ToString().ToLower() == "true" ? "false" : "true") + ",\"" + error + "\");", true);

        }
        protected void gvPOList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

            deatilslist.PageIndex = e.NewPageIndex;

            //String sql = "[dbo].[sp_ORD_POHeaderList] @PONumber='" + txtPONumber.Text.Trim() + "',@POStatusID=" + ddlSelectStatus.SelectedValue + ",@TenantID=" + hifTenant.Value;
            //Session["listqueue"] = sql;
            setGridData();


        }
        protected void deatilslist_Sorting(object sender, GridViewSortEventArgs e)
        {
            DataSet dsGetPOList = null;
            try
            {
                dsGetPOList = deatilslist.DataSource as DataSet;
            }
            catch (Exception ex)
            {
                resetBlkLableError("Error while loading", true);
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
            }

            deatilslist.DataSource = dsGetPOList;
            deatilslist.DataBind();
            dsGetPOList.Dispose();
        }
        protected void resetBlkLableError(string error, bool isError)
        {


            ScriptManager.RegisterStartupScript(this, this.GetType(), "test", "showStickyToast(" + (isError.ToString().ToLower() == "true" ? "false" : "true") + ",\"" + error + "\");", true);

        }

        protected void deatilslist_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            deatilslist.PageIndex = e.NewPageIndex;
           // setGridData();
            //String sql = "[dbo].[sp_ORD_POHeaderList] @PONumber='" + txtPONumber.Text.Trim() + "',@POStatusID=" + ddlSelectStatus.SelectedValue + ",@TenantID=" + hifTenant.Value;
            //Session["listqueue"] = sql;

        }
        protected void lnkAddNewKitItemsInitiateTransfer_Click(object sender, EventArgs e)
        {
            try
            {
                if ((DB.GetSqlN("select count(*) as N from MMT_KitJobOrderDetails where KitJobOrderHeaderID=" + CommonLogic.QueryString("jobid") + " and IsDeleted=0 and IsActive=1")) == 0)
                {
                    resetError("No Items For This Job Order", true);
                    return;
                }
                DB.ExecuteSQL("[USP_TRN_GenerateKittingTransferJobOrders] @KitJobOrderHeaderID=" + ViewState["jobid"].ToString() + ",@LoggedInUserID=" + cp.UserID);
                resetBlkLableError("Succesfully Initiated", false);
                int status = DB.GetSqlN("select KitJobOrderStatusID as N from MMT_KitJobOrderHeader where KitJobOrderHeaderID=" + CommonLogic.QueryString("jobid"));
                if (status == 3)
                {
                    btninitiateorder.Visible = false;
                    lnkAddNewKitItems.Visible = false;
                }
                FillFormData();

            }
            catch(Exception e1)
            {
                resetBlkLableError("Error while initiating Transfer", true);
            }
           
        }
            protected void lnkAddNewKitItems_Click(object sender, EventArgs e)
        {
            StringBuilder sql = new StringBuilder(2500);
            try
            {

                deatilslist.EditIndex = 0;
                deatilslist.PageIndex = 0;

                DataSet dsPOInvoice = this.deatilslist_buildGridData();
                DataRow row = dsPOInvoice.Tables[1].NewRow();
                row["KitJobOrderDetailsID"] = 0;
                row["Quantity"] = 0;
                dsPOInvoice.Tables[1].Rows.InsertAt(row, 0);


                this.deatilslist_buildGridData(dsPOInvoice);

                //this.resetError("New PO Invoice line item added.", false);

                //ViewState["deatils"] = true;

                lnkAddNewKitItems.Visible = false;

                deatilslist.Focus();
            }
            catch (Exception ex)
            {
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);

                this.resetError("Error while inserting new record ", true);

            }

        }
        protected DataSet deatilslist_buildGridData()
        {
            string sql = ViewState["deatils"].ToString();
            DataSet ds = DB.GetDS(sql, false);

            return ds;
        }
        protected void deatilslist_buildGridData(DataSet ds)
        {
            deatilslist.DataSource = ds.Tables[1];
            deatilslist.DataBind();
            ds.Dispose();
        }

        protected void deatilslist_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {

                if ((DB.GetSqlN("select KitJobOrderStatusID as N from MMT_KitJobOrderHeader where KitJobOrderHeaderID=" + CommonLogic.QueryString("jobid"))) >= 2)
                {
                    resetError("Job already Initiated you cannot Edit Kit Items", true);
                    deatilslist.EditIndex = -1;

                    this.deatilslist_buildGridData(this.deatilslist_buildGridData());

                    lnkAddNewKitItems.Visible = true;
                    return;
                }
                GridViewRow row = deatilslist.Rows[e.RowIndex];
                if (row != null)
                {
                    string ltPONumber = ((TextBox)row.FindControl("txtMcode")).Text;
                    string qty = ((TextBox)row.FindControl("txtQuantity")).Text;
                    int detailsid = Convert.ToInt32(((Label)row.FindControl("itdeatilsid")).Text);
                    string mmid = hifDynaMCodeId.Value.ToString();

                    if (ltPONumber.Trim() == "")
                    {
                        resetError("Please Select Parent Part No.", true);
                        return;
                    }
                    if (qty.Trim() == "")
                    {
                        resetError("Please Enter Qty.", true);
                        return;
                    }
                    int value = DB.GetSqlN("[dbo].[SP_UpsertJobOrderHeaderKitDeatils] @detailsid=" + detailsid + ",@HeaderID=" + CommonLogic.QueryString("jobid") + ",@KitPlannerId=" + mmid + ",@Qty=" + qty + ",@userid=" + cp.UserID);
                    this.resetError("Successfully Updated", false);
                    deatilslist.EditIndex = -1;

                    this.deatilslist_buildGridData(this.deatilslist_buildGridData());

                    lnkAddNewKitItems.Visible = true;

                }
            }
            catch (Exception e1)
            {
                resetBlkLableError("Error while updating", true);
            }

           
            

            }

        protected void deatilslist_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            deatilslist.EditIndex = -1;
            this.deatilslist_buildGridData(this.deatilslist_buildGridData());

            lnkAddNewKitItems.Visible = true;
        }

        protected void deatilslist_RowEditing(object sender, GridViewEditEventArgs e)
        {
            deatilslist.EditIndex = e.NewEditIndex;



            this.deatilslist_buildGridData(this.deatilslist_buildGridData());

            lnkAddNewKitItems.Visible = false;
        }

        protected void lnkDeletePOInvItem_Click(object sender, EventArgs e)
        {
            string rfidIDs = "";

            bool chkBox = false;
            if ((DB.GetSqlN("select KitJobOrderStatusID as N from MMT_KitJobOrderHeader where KitJobOrderHeaderID=" + CommonLogic.QueryString("jobid"))) >= 2)
            {
                resetError("Job already Initiated you cannot Edit Kit Items", true);


                this.deatilslist_buildGridData(this.deatilslist_buildGridData());


                return;
            }


            StringBuilder sqlDeleteString = new StringBuilder();
            sqlDeleteString.Append("BEGIN TRAN ");
            foreach (GridViewRow gv in deatilslist.Rows)
            {

                CheckBox isDelete = (CheckBox)gv.FindControl("chkIsDeletePOInvItems");

                if (isDelete == null)
                    return;

                if (isDelete.Checked)
                {
                    chkBox = true;
                    // Concatenate GridView items with comma for SQL Delete
                    rfidIDs = ((Label)gv.FindControl("lbldetailsid")).Text.ToString();



                    if (rfidIDs != "")
                        sqlDeleteString.Append(" Delete from MMT_KitJobOrderDetails Where KitJobOrderDetailsID=" + rfidIDs + ";");

                }
            }
            sqlDeleteString.Append(" COMMIT ");
            try
            {

                if (chkBox)
                {
                    DB.ExecuteSQL(sqlDeleteString.ToString());
                }
                resetError("Successfully deleted the selected Kit items", false);

            }
            catch (Exception ex)
            {
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
                resetError("Error while deleting Kit Items", true);
            }

            this.deatilslist_buildGridData(this.deatilslist_buildGridData());
        }
        //protected void lnkAddNewPoItems_Click(object sender, EventArgs e)
        //{
        //}

    }
}