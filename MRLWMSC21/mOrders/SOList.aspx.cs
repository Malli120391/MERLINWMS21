using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using MRLWMSC21Common;
using System.Globalization;
using System.Resources;

namespace MRLWMSC21.mOrders
{
    //Author    :   Gvd Prasad
    //Created On:   27-Oct-2013
    //Use case ID:  WO list_UC_009

    public partial class SOList : System.Web.UI.Page
    {
        public CustomPrincipal cp = HttpContext.Current.User as CustomPrincipal;

        protected void page_PreInit(object sender, EventArgs e)
        {
            Page.Theme = "Orders";
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //if (!cp.IsInAnyRoles(CommonLogic.GetRolesAllowed(CommonLogic.GetRolesAllowedForthisPage("Sales Order List"))))
            //{
            //    Response.Redirect("../Login.aspx?eid=6");
            //}
            DesignLogic.SetInnerPageSubHeading(this.Page, "Sales Order List");
            cp = HttpContext.Current.User as CustomPrincipal;
            if (!IsPostBack)
            {
                txtTenant.Enabled = CommonLogic.CheckSuperAdmin(txtTenant, cp, hifTenant);

                if (cp.TenantID != 0)
                {
                    gvSOList.Columns[1].Visible = false;
                }

                string Tenant = txtTenant.Text.Trim();
                if (Tenant == "Tenant...")
                    Tenant = "";

               // BuildDropDown(ddlSelectStatus, "select StatusName,SOStatusID from ORD_SOStatus where IsDeleted=0 and IsActive=1", "StatusName", "SOStatusID");
                BuildDropDown(ddlSelectStatus, "Exec [dbo].[USP_GetSOStatus] ", "StatusName", "SOStatusID");
                ddlSelectStatus.SelectedValue = "0";
                String sql = "[dbo].[sp_ORD_SOHeaderList] @SONumber='',@SOStatusID=" + ddlSelectStatus.SelectedValue + ",@Tenant=" + DB.SQuote(Tenant)+", @AccountID_New="+cp.AccountID+ ",@TenantID_New="+cp.TenantID+ ",@UserTypeID_New="+cp.UserTypeID+ ",@UserID_New="+cp.UserID+"";
                ViewState["sql"]= sql;
                setGridData();
            }

        }

        private void setGridData()
        {
            cp = HttpContext.Current.User as CustomPrincipal;
            try
            {

                String cmdGetSOitems = (String)ViewState["sql"];
                DataSet dsGetSOitems = DB.GetDS(cmdGetSOitems, false);
                gvSOList.DataSource = dsGetSOitems;
                gvSOList.DataBind();
                dsGetSOitems.Dispose();
                if (gvSOList.Rows.Count == 0)
                {
                    resetBlkLableError("No sales order is available", true);
                }
            }
            catch (Exception ex)
            {
                resetBlkLableError("Error while Build Data",true);
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
            }
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

        protected void LnkUpdateIsActive_Click(object sender, EventArgs e)
        {
            cp = HttpContext.Current.User as CustomPrincipal;
            String Active = "";
            String notActive = "";
            CheckBox box;
            Boolean check = false;
            foreach (GridViewRow row in gvSOList.Rows)
            {
                box = (CheckBox)row.FindControl("ChkIsActive");
                if (box.Checked)
                {
                    check = true;
                    Active += (((Literal)row.FindControl("SOHeaderID")).Text + ",");
                }
                else
                {
                    notActive += (((Literal)(row.FindControl("SOHeaderID"))).Text + ",");
                }

            }



            if (check)
            {
                if (Active.Length != 0)
                {
                    Active = Active.Substring(0, Active.LastIndexOf(","));
                    //CommonSBQueue.SendSqlUpsertMessage("updateapprove", "update ORD_POHeader set IsActive=1 where POHeaderID in (" + Active + ")");
                    //CommonSBQueue.GetUpsertMessage("updateapprove");
                    DB.ExecuteSQL("update ORD_SOHeader set IsActive=1 where SOHeaderID in (" + Active + ")");
                }
            }
            //CommonSBQueue.SendSqlUpsertMessage("updatenotapprove", "update ORD_POHeader set IsActive=0 where POHeaderID in (" + notActive + ")");
            //CommonSBQueue.GetUpsertMessage("updatenotapprove");
            if (notActive.Length != 0)
            {
                notActive = notActive.Substring(0, notActive.LastIndexOf(","));
                DB.ExecuteSQL("update ORD_SOHeader set IsActive=0 where SOHeaderID in (" + notActive + ")");
            }
            //String sql = "";
            //sql += ("[dbo].[sp_ORD_SOHeaderList] ");
            //sql += ("@SONumber='" + SoNumberSearch.Text.Trim() + "',@SOStatusID=" + ddlSelectStatus.SelectedValue + ",@Tenantid="+hifTenant.Value);

            //ViewState["sql"] = sql;
            setGridData();


        }

        protected Boolean GetBool(String IsChecked)
        {
            if (IsChecked != "" && Convert.ToBoolean(Convert.ToInt32(IsChecked)))
                  return true;
            else
                    return false;
        }

        protected void gvSOList_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            cp = HttpContext.Current.User as CustomPrincipal;
            GridViewRow row = gvSOList.Rows[e.RowIndex];
            String str = ((Literal)row.FindControl("SOHeaderID")).Text;
            try
            {
                DB.ExecuteSQL("[dbo].[sp_ORD_DeleteSOItems] @SODetailsID=0,@SOHeaderID=" + str+ ",@UpdatedBy="+cp.UserID.ToString());
            }
            catch (Exception ex)
            {
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
            }
            //ViewState["sql"] = "[dbo].[sp_ORD_SOHeaderList] @SONumber='',@SOStatusID=" + ddlSelectStatus.SelectedValue + ",@Tenantid="+hifTenant.Value;
           
            setGridData();
        }

        protected void lnkNewForm_Click(object sender, EventArgs e)
        {
            Response.Redirect("SalesOrderInfo.aspx");
        }

        protected void gvSOList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvSOList.PageIndex = e.NewPageIndex;
            //ViewState["sql"] = "[dbo].[sp_ORD_SOHeaderList] @SONumber='" + (SoNumberSearch.Text.Trim() == "SO Number..." ? "" : SoNumberSearch.Text.Trim()) + "',@SOStatusID=" + ddlSelectStatus.SelectedValue + ",@Tenantid="+hifTenant.Value;

            setGridData();
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

                dropdown.Items.Add(new ListItem("ALL", "0"));
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

        protected void lnkGetData_Click(object sender, EventArgs e)
        {

            //test case ID (TC_011)
            //Searches for the WO details with the provided WO number

            //test case ID (TC_009)
            //Searches for the WO details with the provided Status also
            String Tenant = txtTenant.Text.Split('-')[0];
            cp = HttpContext.Current.User as CustomPrincipal;
            if (Tenant == "Tenant...")
                Tenant = "";

            ViewState["sql"] = "[dbo].[sp_ORD_SOHeaderList] @SONumber='" + (SoNumberSearch.Text.Trim() == "SO Number..." ? "" : SoNumberSearch.Text.Trim()) + "',@SOStatusID=" + ddlSelectStatus.SelectedValue + ",@AccountID_New=" + cp.AccountID + ",@TenantID_New=" + cp.TenantID + ",@UserTypeID_New=" + cp.UserTypeID + ",@UserID_New=" + cp.UserID + ",@Tenant=" + DB.SQuote(Tenant);

            setGridData();
        }

        //protected void imgbtngvSOList_Click(object sender, ImageClickEventArgs e)
            protected void imgbtngvSOList_Click(object sender, EventArgs e)
        {
            //test case ID (TC_015)
            //export to excel 

            //String[] hiddencolumns = {"Edit"};
            //gvSOList.AllowPaging = false;
            //this.setGridData();
            //CommonLogic.ExporttoExcel(gvSOList, "SalesOrderList", hiddencolumns);
            cp = HttpContext.Current.User as CustomPrincipal;
            gvSOList.AllowPaging = false;
            string[] hiddencolumns = { "Edit" };
            setGridData();
            CommonLogic.ExporttoExcel(gvSOList, "SalesOrderList", hiddencolumns);
        }

        public override void VerifyRenderingInServerForm(Control control)
        {
            /* Verifies that the control is rendered */
        }

        protected void gvSOList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //======================= Added By M.D.Prasad For View Only Condition ======================//
            cp = HttpContext.Current.User as CustomPrincipal;
            string[] hh = cp.Roles;
            for (int i = 0; i < hh.Length; i++)
            {
                if (cp.UserTypeID == 3 && Convert.ToInt32(hh[i]) == 4)
                {
                    lnkadd.Visible = false;
                    imgbtngvSOList.Visible = false;
                }
            }
            //======================= Added By M.D.Prasad For View Only Condition ======================//
            if ((e.Row.RowType == DataControlRowType.DataRow) && (e.Row.RowState == DataControlRowState.Normal) || (e.Row.RowState == DataControlRowState.Alternate))
            {
                if (cp.TenantID != 0)
                {
                    gvSOList.Columns[8].Visible = false;
                }

                //======================= Added By M.D.Prasad For View Only Condition ======================//
                for (int i = 0; i < hh.Length; i++)
                {
                    if (cp.UserTypeID == 3 && Convert.ToInt32(hh[i]) == 4)
                    {
                        gvSOList.Columns[8].Visible = false;
                    }
                }
                //======================= Added By M.D.Prasad For View Only Condition ======================//
            }
        }
        
       
    }
}