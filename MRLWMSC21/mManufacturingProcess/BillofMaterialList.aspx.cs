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
// Usecase Ref.: BOM List_UC_005
// DevelopedBy : Naresh P
// CreatedOn   : 05/05/2014
// Modified On : 24/03/2015


namespace MRLWMSC21.mManufacturingProcess
{
    public partial class BillofMaterialList : System.Web.UI.Page
    {
        public CustomPrincipal cp = HttpContext.Current.User as CustomPrincipal;

        protected void Page_PreInit(object sender, EventArgs e)
        {
            Page.Theme = "Inbound";

        }

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!cp.IsInAnyRoles(CommonLogic.GetRolesAllowed(CommonLogic.GetRolesAllowedForthisPage("Bill of Material List"))))
            {
                Response.Redirect("../Login.aspx?eid=6");
            }

            if (!IsPostBack)
            {
                DesignLogic.SetInnerPageSubHeading(this.Page, "Bill of Material List");

                ViewState["BoMHeaderList"] = "EXEC [dbo].[sp_MFG_GetBoMHeaderList] @BOMRefNumber=''";
                Build_BOMRefNumberList(Build_BOMRefNumberList());
            }
        }

        protected void lnkGetData_Click(object sender, EventArgs e)
        {
            String BOMRefNumber = txtBOMRefNumberSearch.Text.Trim();
            if (BOMRefNumber == "BOM Ref. No. ...")
                BOMRefNumber = "";
            ViewState["BoMHeaderList"] = "EXEC [dbo].[sp_MFG_GetBoMHeaderList] @BOMRefNumber='" + BOMRefNumber + "'";
            Build_BOMRefNumberList(Build_BOMRefNumberList());

            txtBOMRefNumberSearch.Text = "BOM Ref. No. ...";
        }

        protected void gvPROList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvBoMList.PageIndex = e.NewPageIndex;
            Build_BOMRefNumberList(Build_BOMRefNumberList());
        }

        private DataSet Build_BOMRefNumberList()
        {
            String cMdPROHeaderList = ViewState["BoMHeaderList"].ToString();
            DataSet dsPROHeaderList = DB.GetDS(cMdPROHeaderList,false);

            lblToatalBOMs.Text = "Total Items [ "+dsPROHeaderList.Tables[0].Rows.Count+" ]";

            if (dsPROHeaderList.Tables[0].Rows.Count == 0) {
                resetError("No BOM is available", true);
            }

            return dsPROHeaderList;
        }

        private void Build_BOMRefNumberList(DataSet dsPROHeaderList)
        {
            gvBoMList.DataSource = dsPROHeaderList;
            gvBoMList.DataBind();
            dsPROHeaderList.Dispose();
        }

        protected void imgbtngvBoMList_Click(object sender, ImageClickEventArgs e)
        {
            try
            {      
                String[] hiddencolumns = {""};
                gvBoMList.AllowPaging = false;
                this.Build_BOMRefNumberList(this.Build_BOMRefNumberList());
                CommonLogic.ExporttoExcel(gvBoMList, "BillofMaterialList", hiddencolumns);
            }
            catch (Exception ex)
            {
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);

                ScriptManager.RegisterStartupScript(this, this.GetType(), "test", "showStickyToast("+true+",\"Error While Exporting Data\");", true);
            }

        }
        public override void VerifyRenderingInServerForm(Control control)
        {
            /* Verifies that the control is rendered */
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
       
    }
}