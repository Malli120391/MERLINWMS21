using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using MRLWMSC21Common;

namespace MRLWMSC21.mOrders
{
    //Author    :   Gvd Prasad
    //Created On:   12-Apl-2014
    //Use case ID : Issue List_UC_017

    public partial class ToolManagementList : System.Web.UI.Page
    {
        public CustomPrincipal cp = HttpContext.Current.User as CustomPrincipal;

        protected void page_PreInit(object sender, EventArgs e)
        {
            Page.Theme = "Orders";
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!cp.IsInAnyRoles(CommonLogic.GetRolesAllowed(CommonLogic.GetRolesAllowedForthisPage("Issue List"))))
            {
                //test case ID (TC_008)
                //Only authorized user can access the WO list page

                Response.Redirect("../Login.aspx?eid=6");
            }
            DesignLogic.SetInnerPageSubHeading(this.Page, "Issue List");

            if (!IsPostBack)
            {

                BuildDropDown(ddlSelectStatus, "select StatusName,SOStatusID from ORD_SOStatus where IsDeleted=0 and IsActive=1", "StatusName", "SOStatusID");
                ddlSelectStatus.SelectedValue = "1";
                String sql = "[dbo].[sp_ORD_SOHeaderList] @SONumber='',@SOStatusID=" + ddlSelectStatus.SelectedValue + ",@IsToolManagement=1";
                ViewState["sql"] = sql;
                setGridData();
            }

        }

        private void setGridData()
        {
            try
            {
                String cmdGetToolsList = (String)ViewState["sql"];
                DataSet dsGetToolsList = DB.GetDS(cmdGetToolsList, false);
                gvTMOList.DataSource = dsGetToolsList;
                gvTMOList.DataBind();
                dsGetToolsList.Dispose();
                if(gvTMOList.Rows.Count==0)
                {
                    resetBlkLableError("No 'Issue' is available", true);
                }
            }
            catch (Exception ex)
            {
                resetBlkLableError("Error while Build Data",true);
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
            }
        }

        private void BuildDropDown(DropDownList dropdown, String sql, String ListName, String ListValue)
        {
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

        protected void gvTMOList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvTMOList.PageIndex = e.NewPageIndex;
            setGridData();
        }

        protected void lnkGetData_Click(object sender, EventArgs e)
        {
            //test case ID(TC_009)
            //Searches for the issue details with the provided issue number

            //test case ID(TC_012)
            //Searches for the issue details with the provided status

           ViewState["sql"]= ("[dbo].[sp_ORD_SOHeaderList] @SONumber='" + (SoNumberSearch.Text.Trim()=="Issue Ref.#..."?"":SoNumberSearch.Text.Trim()) + "',@SOStatusID=" + ddlSelectStatus.SelectedValue+ ",@IsToolManagement=1");
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

        protected void imgbtngvToolmgtlist_Click(object sender, ImageClickEventArgs e)
        {
            //test case ID (TC_015)
            //export to excel

            String[] hiddencolumns = { "" };
            gvTMOList.AllowPaging = false;
            this.setGridData();
            CommonLogic.ExporttoExcel(gvTMOList, "Issue Order", hiddencolumns);
        }

        public override void VerifyRenderingInServerForm(Control control)
       {
           /* Verifies that the control is rendered */
       }

    }
}