using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MRLWMSC21Common;
using System.Data;
using System.Data.SqlClient;
using System.Text;


// Module Name : Manufacturing
// Usecase Ref.: Workstation Details_UC_004
// DevelopedBy : Naresh P
// CreatedOn   : 05/05/2014
// Modified On : 24/03/2015


namespace MRLWMSC21.mManufacturingProcess
{
    public partial class WorkCenters : System.Web.UI.Page
    {

        CustomPrincipal cp = HttpContext.Current.User as CustomPrincipal;

        protected void Page_PreInit(object sender, EventArgs e)
        {
            Page.Theme = "Inbound";

        }
        protected void Page_Load(object sender, EventArgs e)
        {


            if (!cp.IsInAnyRoles(CommonLogic.GetRolesAllowed(CommonLogic.GetRolesAllowedForthisPage("New Workstation"))))
            {
                Response.Redirect("../Login.aspx?eid=6");
            }

            if (CommonLogic.QueryString("update") == "success")
               lblWCStatus.Text="Successfully updated";

            if (!IsPostBack) {
                DesignLogic.SetInnerPageSubHeading(this.Page, "Workstation Details");
                LoadDropDown(ddlResourceType, "select ResourceTypeID,ResourceType from MFG_ResourceType where IsActive=1 and IsDeleted=0", "ResourceType", "ResourceTypeID", "Select Resource Type");
                LoadDropDown(ddlWorkGroup, "select WorkCenterGroup,WorkCenterGroupID from MFG_WorkCenterGroup  where IsActive=1 AND IsDeleted=0", "WorkCenterGroup", "WorkCenterGroupID", "Select Workstation Type");
                LoadListBox(listUsers, "select UserID,FirstName from GEN_User where IsActive=1 AND IsDeleted=0 AND TenantID=" + cp.TenantID, "FirstName", "UserID");

                if (CommonLogic.QueryString("wcid") != "0" && CommonLogic.QueryString("wcid") != "")
                {

                    LoadWCDetails();
                    LoadUsersList();
                    lnkButSave.Text = "Update" + CommonLogic.btnfaUpdate;
                    lnkCancel.Visible = false;
                    ddlWorkGroup.Enabled = false;
                }
                else {
                    lnkButSave.Text = "Create Workstation" + CommonLogic.btnfaSave;
                }
            }
        }

        #region ------------- Basic Data   ---------------------
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

        protected void lnkButSave_Click(object sender, EventArgs e)
        {

            Page.Validate("InitiateWC");

            if (!Page.IsValid)
                return;
            /*
            if (DB.GetSqlN("select top 1 ProductionOrder_WorkCenterID  AS N from MFG_ProductionOrder_WorkCenter PO_WC where PO_WC.IsActive=1 AND PO_WC.IsDeleted=0  AND   PO_WC.WorkCenterID=" + CommonLogic.IIF(CommonLogic.QueryString("wcid") != "", CommonLogic.QueryString("wcid"), "0")) != 0)
            {
                resetError("This workstation is configured in production process you cannot modify these details ", true);
                return;
            }*/

            if (GetListBoxSelStringInComma(listUsers).Length == 0)
            {
                resetError("Please select atleast one user", true);
                return;
            }
            
            StringBuilder sbSqlString = new StringBuilder();

            sbSqlString.Append("DECLARE @UpdatedWorkCenterID int  ");
            sbSqlString.Append("EXEC [sp_MFG_UpsertWorkCenter]  @WorkCenterID="+CommonLogic.IIF(CommonLogic.QueryString("wcid")!="",CommonLogic.QueryString("wcid"),"0"));
            sbSqlString.Append(",@WorkCenter="+DB.SQuote(txtWCName.Text));
            sbSqlString.Append(",@WorkCenterRefNo=" + DB.SQuote(txtWCCode.Text));
            sbSqlString.Append(",@WorkCenterGroupID=" + CommonLogic.IIF(ddlWorkGroup.SelectedValue != "0", ddlWorkGroup.SelectedValue, "NULL"));
            sbSqlString.Append(",@ResourceTypeID="+CommonLogic.IIF(ddlResourceType.SelectedValue!="0",ddlResourceType.SelectedValue,"NULL"));
            sbSqlString.Append(",@Description="+DB.SQuote(txtDescription.Text));
            sbSqlString.Append(",@WorkingPeriod=" + Convert.ToInt32(txtWRPeriod.Text != "" ? txtWRPeriod.Text : "0"));
            sbSqlString.Append(",@EfficiencyFactor=" + Convert.ToDecimal(txtEFFactor.Text != "" ? txtEFFactor.Text : "0"));
            sbSqlString.Append(",@CapacityPerCycle="+Convert.ToInt32( txtCapacityPerCycle.Text!=""?txtCapacityPerCycle.Text:"0"));
            sbSqlString.Append(",@TimeForOneCycleInHours=" + Convert.ToDecimal(txtTimefor1Cycle.Text != "" ? txtTimefor1Cycle.Text : "0"));
            sbSqlString.Append(",@TimeBeforeProd=" + Convert.ToDecimal(txtTimeBeforeProd.Text != "" ? txtTimeBeforeProd.Text : "0"));
            sbSqlString.Append(",@TimeAfterProd=" + Convert.ToDecimal(txtTimeAfterProd.Text != "" ? txtTimeAfterProd.Text : "0"));
            sbSqlString.Append(",@WorkCenterProduct=NULL");
            sbSqlString.Append(",@CostPerHour="+Convert.ToDecimal(txtCostPerHour.Text!=""?txtCostPerHour.Text:"0"));
            sbSqlString.Append(",@HourAccount=NULL");
            sbSqlString.Append(",@CostPerCycle="+Convert.ToDecimal( txtCostPerCycle.Text!=""?txtCostPerCycle.Text:"0"));
            sbSqlString.Append(",@CycleAccount=NULL");
            sbSqlString.Append(",@AnalyticJournal=NULL");
            sbSqlString.Append(",@GeneralAccount=NULL");
            sbSqlString.Append(",@UserIDs=" + DB.SQuote(GetListBoxSelStringInComma(listUsers)));
            sbSqlString.Append(",@CreatedBy="+cp.UserID);
            sbSqlString.Append(",@IsActive=" + (chkActive.Checked==true?1:0));
            sbSqlString.Append(" ,@NewWorkCenterID = @UpdatedWorkCenterID output select @UpdatedWorkCenterID as N");

            int WorkCenterID=0;

            try
            {
                WorkCenterID = DB.GetSqlN(sbSqlString.ToString());
            }
            catch (SqlException sqlex)
            {
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), sqlex.Source, sqlex.Message, sqlex.StackTrace);

                if (sqlex.ErrorCode == -2146232060)
                {
                    this.resetError("Workstation already exists", true);
                    return;
                }
            }
            catch (Exception ex) {
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
                resetError("Error while updating workstation details", true);

                return;
            }

            Response.Redirect("WorkCenters.aspx?wcid=" + WorkCenterID + "&update=success");
        }

        protected void lnkCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("WorkCenterList.aspx");
        }

        protected void LoadWCDetails() {

            try
            {

                IDataReader drWC = DB.GetRS("select top 1  * from MFG_WorkCenter where  IsDeleted=0 AND WorkCenterID=" + CommonLogic.QueryString("wcid"));

                drWC.Read();

                txtWCName.Text = DB.RSField(drWC, "WorkCenter");
                txtWCCode.Text = DB.RSField(drWC, "WorkCenterRefNo");
                chkActive.Checked = DB.RSFieldBool(drWC, "IsActive");
                ddlResourceType.SelectedValue = DB.RSFieldInt(drWC, "ResourceTypeID").ToString();
                txtWRPeriod.Text = DB.RSFieldInt(drWC, "WorkingPeriod").ToString();
                // ddlCompany.SelectedValue = DB.RSFieldInt(drWC, "").ToString();
                txtCapacityPerCycle.Text = DB.RSFieldInt(drWC, "CapacityPerCycle").ToString();
                txtCostPerHour.Text = DB.RSFieldDecimal(drWC, "CostPerHour").ToString();
                txtTimefor1Cycle.Text = DB.RSFieldDecimal(drWC, "TimeForOneCycleInHours").ToString();
                txtCostPerCycle.Text = DB.RSFieldDecimal(drWC, "CostPerCycle").ToString();
                txtTimeBeforeProd.Text = DB.RSFieldDecimal(drWC, "TimeBeforeProd").ToString();
                txtTimeAfterProd.Text = DB.RSFieldDecimal(drWC, "TimeAfterProd").ToString();
                txtEFFactor.Text = DB.RSFieldDecimal(drWC, "EfficiencyFactor").ToString();
                txtDescription.Text = DB.RSField(drWC, "Description");
                ddlWorkGroup.SelectedValue = DB.RSFieldInt(drWC, "WorkCenterGroupID").ToString();
                
            
                drWC.Close();
            }
            catch (Exception ex) {
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
                resetError("Error while loading workstation details", true);
            }
        }

        private void LoadUsersList(){

            IDataReader drWCUsers = DB.GetRS(" EXEC [sp_MFG_GetWorkCenterUsers] @WorkCenterID=" + CommonLogic.QueryString("wcid"));

            drWCUsers.Read();

            LoadDefaultListBoxValues(listUsers, DB.RSField(drWCUsers, "UserIDs"));


            drWCUsers.Close();
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

        #endregion ------------- Basic Data   ---------------------


        protected void LoadListBox(ListBox listBox, string query, string fieldtodisplay, string fieldvalue)
        {

            try
            {
                IDataReader ListReder = DB.GetRS(query);

                while (ListReder.Read())
                {

                    listBox.Items.Add(new ListItem(ListReder[fieldtodisplay].ToString(), ListReder[fieldvalue].ToString()));
                }

                ListReder.Close();

            }
            catch (Exception ex)
            {
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
                resetError("Error while loading user data", true);
            }

        }

        protected void LoadDefaultListBoxValues(ListBox listBox, string ids)
        {

            try
            {
                String[] idList = ids.Split(',');
                listBox.SelectionMode = ListSelectionMode.Multiple;
                int count = listBox.Items.Count;
                for (int pos = 0; pos < count; pos++)
                {
                    listBox.Items[pos].Selected = false;

                }

                foreach (String value in idList)
                {
                    if (value.Trim() != "")
                        listBox.Items[listBox.Items.IndexOf(listBox.Items.FindByValue(value.Trim()))].Selected = true;

                }
            }
            catch (Exception ex)
            {
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
            }
        }

        public string GetListBoxSelStringInComma(ListBox vListbox)
        {
            string selectedItems = "";
            if (vListbox.Items.Count > 0)
            {
                for (int i = 0; i < vListbox.Items.Count; i++)
                {
                    if (vListbox.Items[i].Selected)
                    {
                        if (selectedItems == "")
                            selectedItems = vListbox.Items[i].Value;
                        else
                            selectedItems += "," + vListbox.Items[i].Value;
                    }
                }
            }
            return selectedItems;
        }

    }
}