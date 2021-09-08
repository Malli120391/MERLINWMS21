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


// Module Name : Manufacturing
// Usecase Ref.: Routing List_UC_007
// DevelopedBy : Naresh P
// CreatedOn   : 05/05/2014
// Modified On : 24/03/2015


namespace MRLWMSC21.mManufacturingProcess
{
    public partial class RoutingList : System.Web.UI.Page
    {
        public CustomPrincipal cp = HttpContext.Current.User as CustomPrincipal;

        protected void Page_PInit(object sender, EventArgs e)
        {
            Page.Theme = "blue_theme";

        }

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!cp.IsInAnyRoles(CommonLogic.GetRolesAllowed(CommonLogic.GetRolesAllowedForthisPage("Routing List"))))
            {
                Response.Redirect("../Login.aspx?eid=6");
            }

            if (!IsPostBack)
            {

                DesignLogic.SetInnerPageSubHeading(this.Page, "Routing List");

                LoadDropDown(ddlRoutingDocType, "select RoutingDocumentTypeID,RoutingDocumentType from MFG_RoutingDocumentType where IsActive=1 AND IsDeleted=0", "RoutingDocumentType", "RoutingDocumentTypeID", "Route Type - All");

                ViewState["BoMHeaderList"] = "EXEC [dbo].[sp_MFG_GetRoutingHeaderList] @RoutingRefNo='',@RoutingDocType="+ddlRoutingDocType.SelectedValue;
                Build_RoutingRefNumberList(Build_RoutingRefNumberList());
            }
        }


        // CommonLogic for Loading DropDownList
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


        protected void lnkGetData_Click(object sender, EventArgs e)
        {
            String RoutingRefNo = txtRoutingRefNoSearch.Text.Trim();
            if (RoutingRefNo == "Routing Ref. No...")
                RoutingRefNo = "";
            ViewState["BoMHeaderList"] = "EXEC [dbo].[sp_MFG_GetRoutingHeaderList] @RoutingRefNo='" + RoutingRefNo + "',@RoutingDocType="+ddlRoutingDocType.SelectedValue;
            Build_RoutingRefNumberList(Build_RoutingRefNumberList());
        }

        

        private DataSet Build_RoutingRefNumberList()
        {
            String cMdRoutingHeaderList = ViewState["BoMHeaderList"].ToString();
            DataSet dsPROHeaderList = DB.GetDS(cMdRoutingHeaderList, false);

            lblTotalRoutings.Text = "Total Items [ " + dsPROHeaderList.Tables[0].Rows.Count + " ] ";

            if (dsPROHeaderList.Tables[0].Rows.Count == 0) {
                resetError("No routing is available", true);
            }

            return dsPROHeaderList;
        }

        private void Build_RoutingRefNumberList(DataSet cMdRoutingHeaderList)
        {
            gvRoutingList.DataSource = cMdRoutingHeaderList;
            gvRoutingList.DataBind();
            cMdRoutingHeaderList.Dispose();
        }

        protected void gvRoutingList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvRoutingList.PageIndex = e.NewPageIndex;
            Build_RoutingRefNumberList(Build_RoutingRefNumberList());
        }

        protected void imgbtngvRoutingList_Click(object sender, ImageClickEventArgs e)
        {
            try
            {      
                String[] hiddencolumns = { "" };
                gvRoutingList.AllowPaging = false;
                Build_RoutingRefNumberList(Build_RoutingRefNumberList());
                CommonLogic.ExporttoExcel(gvRoutingList, "RoutingList", hiddencolumns);
            }
            catch (Exception ex)
            {
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
                resetError("Error While Exporting Data", true);
            }
        }
        public override void VerifyRenderingInServerForm(Control control)
        {
            /* Verifies that the control is rendered */
        }
        protected void resetError(string error, bool isError)
        {
           ScriptManager.RegisterStartupScript(this, this.GetType(), "test", "showStickyToast(" + (isError.ToString().ToLower() == "true" ? "false" : "true") + ",\"" + error + "\");", true);
        }


    }
}