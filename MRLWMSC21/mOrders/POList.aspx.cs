using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using MRLWMSC21Common;
using System.Globalization;

namespace MRLWMSC21.mOrders

{
    //Author    :   Gvd Prasad
    //Created On:   25-Oct-2013
    //Use case ID:  PO list_UC_007


    public partial class POList : System.Web.UI.Page
    {
        public CustomPrincipal cp = HttpContext.Current.User as CustomPrincipal;

        protected void Page_PreInit(object sender, EventArgs e)
        {
            Page.Theme = "Orders";
        }

       
        protected void Page_Load(object sender, EventArgs e)
        {
            //if (!cp.IsInAnyRoles(CommonLogic.GetRolesAllowed(CommonLogic.GetRolesAllowedForthisPage("Purchase Order List"))))
            //{
            //    Response.Redirect("../Login.aspx?eid=6");
            //}
            cp = HttpContext.Current.User as CustomPrincipal;
            pnlHeaderRow.DefaultButton = lnkGetData.UniqueID;
            if (!IsPostBack)
            {
               
                DesignLogic.SetInnerPageSubHeading(this.Page, "Inward Order List");
                BuildDropDown(ddlSelectStatus, "select StatusName,POStatusID from ORD_POStatus", "StatusName", "POStatusID");
                ddlSelectStatus.SelectedValue = "0";

                if (cp.TenantID != 0)
                {
                   // gvPOList.Columns[1].Visible = false;
                }

                txtTenant.Enabled = CommonLogic.CheckSuperAdmin(txtTenant, cp, hifTenant);
                string Tenant=txtTenant.Text.Trim();
                if (Tenant == "Tenant...")
                    Tenant = "";

                String sql = "[dbo].[sp_ORD_POHeaderList] @PONumber='',@POStatusID=" + ddlSelectStatus.SelectedValue + ",@Tenant=" + DB.SQuote(Tenant);

                sql = sql + ",@AccountID_New = " + cp.AccountID.ToString() + ",@UserTypeID_New = " + cp.UserTypeID.ToString() + ",@TenantID_New = " + cp.TenantID.ToString() + ",@UserID_New=" + cp.UserID.ToString();

                ViewState["listqueue"] = sql;
                setGridData();
            }
        }

        protected void lnkGetData_Click(object sender, EventArgs e)
        {
            //test case ID (TC_009)
            //Select PO number
            cp = HttpContext.Current.User as CustomPrincipal;
            String sql = txtPONumber.Text;
            if (txtPONumber.Text == "Inward Number...")
                sql = "";

            String Tenant = txtTenant.Text.Split('-')[0];
            if (Tenant == "Tenant...")
                Tenant = "";

            //test case ID (TC_012)
            //Select PO status

            //Query for get List of records based on our requirement

            ViewState["listqueue"] = ("[dbo].[sp_ORD_POHeaderList] @PONumber='" + sql + "',@POStatusID=" + ddlSelectStatus.SelectedValue + ",@Tenant=" + DB.SQuote(Tenant)+", @AccountID_New = " + cp.AccountID.ToString() + ", @UserTypeID_New = " + cp.UserTypeID.ToString() + ", @TenantID_New = " + cp.TenantID.ToString() + ", @UserID_New = " + cp.UserID.ToString());



            setGridData();
        }

        private void setGridData()
        {
            cp = HttpContext.Current.User as CustomPrincipal;
            try
            {
                String sql = ViewState["listqueue"].ToString();
                DataSet dsPOList = DB.GetDS(sql, false);
                gvPOList.DataSource = dsPOList;
                gvPOList.DataBind();
                dsPOList.Dispose();
                if (gvPOList.Rows.Count == 0)
                {
                    resetBlkLableError("No PO is available", true);
                }
            }
            catch (Exception ex)
            {
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
            }
        }

        public Boolean GetBool(String IsChecked)
        {
            if (IsChecked != "")
            {
                if (Convert.ToBoolean(Convert.ToInt32(IsChecked)))
                    return true;
                else
                    return false;
            }
            else
            {
                return false;
            }
        }

        protected void lnkUpdateIsActive_Click(object sender, EventArgs e)
        {
            cp = HttpContext.Current.User as CustomPrincipal;
            String Active="";
            String notActive="";
            CheckBox box;
            Boolean check=false;
            foreach (GridViewRow row in gvPOList.Rows)
            {
                box = (CheckBox)row.FindControl("chkIsActive");
                if (box.Checked)
                {
                    check = true;
                    Active += (((Literal)row.FindControl("ItPOHeaderID")).Text + ",");
                }
                else
                {
                    notActive += (((Literal)(row.FindControl("ItPOHeaderID"))).Text + ",");
                }

            }
            


            if (check)
            {
                if (Active.Length != 0)
                {
                    Active = Active.Substring(0, Active.LastIndexOf(","));
                    //CommonSBQueue.SendSqlUpsertMessage("updateapprove", "update ORD_POHeader set IsActive=1 where POHeaderID in (" + Active + ")");
                    //CommonSBQueue.GetUpsertMessage("updateapprove");
                    DB.ExecuteSQL("update ORD_POHeader set IsActive=1 where POHeaderID in (" + Active + ")");
                }
            }
            //CommonSBQueue.SendSqlUpsertMessage("updatenotapprove", "update ORD_POHeader set IsActive=0 where POHeaderID in (" + notActive + ")");
            //CommonSBQueue.GetUpsertMessage("updatenotapprove");
            if (notActive.Length != 0)
            {
                notActive = notActive.Substring(0, notActive.LastIndexOf(","));
                DB.ExecuteSQL("update ORD_POHeader set IsActive=0 where POHeaderID in (" + notActive + ")");
            }

            //String sql = "[dbo].[sp_ORD_POHeaderList] @PONumber='" + (txtPONumber.Text.Trim() == "PO Number..." ? "" : txtPONumber.Text.Trim()) + "',@POStatusID=" + ddlSelectStatus.SelectedValue + ",@TenantID=" + hifTenant.Value;
            
            //ViewState["listqueue"] = sql;
            setGridData();


        }

        protected void resetBlkLableError(string error, bool isError)
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

        protected void gvPOList_Sorting(object sender, GridViewSortEventArgs e)
        {
            cp = HttpContext.Current.User as CustomPrincipal;
            DataSet dsGetPOList=null;
            try
            {
                dsGetPOList = gvPOList.DataSource as DataSet;
            }
            catch (Exception ex)
            {
                resetBlkLableError("Error while loading",true);
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
            }

            gvPOList.DataSource = dsGetPOList;
            gvPOList.DataBind();
            dsGetPOList.Dispose();
        }

        protected void gvPOList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            cp = HttpContext.Current.User as CustomPrincipal;
            gvPOList.PageIndex = e.NewPageIndex;

            //String sql = "[dbo].[sp_ORD_POHeaderList] @PONumber='" + txtPONumber.Text.Trim() + "',@POStatusID=" + ddlSelectStatus.SelectedValue + ",@TenantID=" + hifTenant.Value;
            //Session["listqueue"] = sql;
            setGridData();


        }

        protected void gvPOList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //if (e.Row.RowType == DataControlRowType.DataRow && !(e.Row.RowState == DataControlRowState.Edit))
            //======================= Added By M.D.Prasad For View Only Condition ======================//
            cp = HttpContext.Current.User as CustomPrincipal;
            string[] hh = cp.Roles;
            for (int i = 0; i < hh.Length; i++)
            {
                if (cp.UserTypeID == 3 && Convert.ToInt32(hh[i]) == 4)
                {
                    LinkButton1.Visible = false;
                    imgbtngvPOList.Visible = false;
                }
            }
            //======================= Added By M.D.Prasad For View Only Condition ======================//
            if ((e.Row.RowType == DataControlRowType.DataRow) && (e.Row.RowState == DataControlRowState.Normal) || (e.Row.RowState == DataControlRowState.Alternate))
            {
                if (cp.TenantID != 0)
                {
                    gvPOList.Columns[9].Visible = false;
                }

                //======================= Added By M.D.Prasad For View Only Condition ======================//
                for (int i = 0; i < hh.Length; i++)
                {
                    if (cp.UserTypeID == 3 && Convert.ToInt32(hh[i]) == 4)
                    {
                        gvPOList.Columns[10].Visible = false;
                    }
                }
                //======================= Added By M.D.Prasad For View Only Condition ======================//
            }
        }

        protected void gvPOList_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            cp = HttpContext.Current.User as CustomPrincipal;
            GridViewRow row= gvPOList.Rows[e.RowIndex];
          String str= ((Literal)row.FindControl("ItPOHeaderID")).Text;
          //CommonSBQueue.SendSqlDSMessage("Deletepoheader", "[dbo].[sp_ORD_DeletePOItems] @PODetailsID=0,@POHeaderID="+str);
          //CommonSBQueue.GetSqlRSMessage("Deletepoheader");

          DB.GetDS("[dbo].[sp_ORD_DeletePOItems] @PODetailsID=0,@POHeaderID=" + str + ",@UpdatedBy=" + cp.UserID.ToString(), false);

          //String sql = "[dbo].[sp_ORD_POHeaderList] @PONumber='',@POStatusID=" + ddlSelectStatus.SelectedValue + ",@TenantID=" + hifTenant.Value;
          //Session["listqueue"] = sql;
          setGridData();

        }

        protected void InkNewForm_Click(object sender, EventArgs e)
        {
            Response.Redirect("PODetailsInfo.aspx");
        }

        private void BuildDropDown(DropDownList dropdown, String sql, String ListName, String ListValue)
        {
            cp = HttpContext.Current.User as CustomPrincipal;
            dropdown.Items.Clear();

            dropdown.Items.Add(new ListItem());
            try
            {
                dropdown.Items.Clear();

                IDataReader dropdownReader = DB.GetRS(sql);

                dropdown.Items.Add(new ListItem("Select PO Status", "0"));
                while (dropdownReader.Read())
                {
                    dropdown.Items.Add(new ListItem(dropdownReader[ListName].ToString(), dropdownReader[ListValue].ToString()));
                }
                dropdownReader.Close();
            }
            catch (Exception ex)
            {
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
            }
        }

        //protected void imgbtngvPOList_Click(object sender, ImageClickEventArgs e)
            protected void imgbtngvPOList_Click(object sender, EventArgs e)
        {
            //test case ID (TC_015)
            //Downloads and details should be displayed in excel format
            cp = HttpContext.Current.User as CustomPrincipal;
            String[] hiddencolumns = {"Edit"};
            gvPOList.AllowPaging = false;
            this.setGridData();
           // gvPOList.Columns[9].Visible = false;
            CommonLogic.ExporttoExcel(gvPOList, "PurchaseOrderList", hiddencolumns);
        }

        public override void VerifyRenderingInServerForm(Control control)
        {
            /* Verifies that the control is rendered */
        }
       
    }
}