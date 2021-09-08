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
using System.Globalization;

namespace FalconAdmin._3PLBilling
{
    public partial class TariffCreation : System.Web.UI.Page
    {
        string gvTenants = "";
        public CustomPrincipal cp = HttpContext.Current.User as CustomPrincipal;
        public Dictionary<string, string> ActivityRate = new Dictionary<string, string>();
        protected void Page_Load(object sender, EventArgs e)
        {
            cp = HttpContext.Current.User as CustomPrincipal;

            if (!IsPostBack)
            {
                ViewState["TPLTariffGroupList"] = "EXEC [sp_TPL_GetDataForTariffCreation]  @TariffID=1, @SearchText='',@UserId=" + cp.UserID;
                TariffGroupList_buildGridData(TariffGroupList_buildGridData());

                ViewState["TPLActivityRateType"] = "EXEC [sp_TPL_GetDataForTariffCreation]  @TariffID=2, @SearchText='',@ActivityRateType='',@UserId=" + cp.UserID;
                ActivityRateType_buildGridData(ActivityRateType_buildGridData());

                ViewState["TPLActivityRate"] = "EXEC [sp_TPL_GetDataForTariffCreation]  @TariffID=3, @SearchText='',@ActivityRateType='',@UserId="+cp.UserID;
                ActivityRate_buildGridData(ActivityRate_buildGridData());


            }
            else
            {
                string parameter = Request["__EVENTARGUMENT"];
                if (parameter == "ChangeActivityRateTab")
                {
                    lnkChangeTabs_Click(sender, e);
                }
                else if (parameter == "ChangeActivityRateTypeTab")
                {
                    LoadActivityType();
                }
            }
        }

        private void LoadActivityType() 
        {
            cp = HttpContext.Current.User as CustomPrincipal;
            //txtActivityTariffType2.Text = "Tariff Sub-Group...";
            ViewState["TPLActivityRateType"] = "EXEC [sp_TPL_GetDataForTariffCreation]  @TariffID=2, @SearchText='',@ActivityRateType='',@UserId="+cp.UserID;
            ActivityRateType_buildGridData(ActivityRateType_buildGridData());
        }
        #region -------------------------Tariff Groups -----------------------------

        private DataSet TariffGroupList_buildGridData()
        {
            string TariffGroupListsql = ViewState["TPLTariffGroupList"].ToString();
            DataSet ds = DB.GetDS(TariffGroupListsql, false);
            return ds;
        }

        private void TariffGroupList_buildGridData(DataSet cMdTariffGroupList)
        {
            foreach (DataRow row in cMdTariffGroupList.Tables[0].Rows)
            {
                ActivityRate.Add(row["ActivityRateGroupID"].ToString(), row["ActivityRateGroup"].ToString());
            }
            ViewState["ActivityRate"] = ActivityRate;
            gvTPLTariffGroups.DataSource = cMdTariffGroupList;
            gvTPLTariffGroups.DataBind();
            cMdTariffGroupList.Dispose();
        }

        protected void gvTPLTariffGroups_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow && e.Row.RowIndex == gvTPLTariffGroups.EditIndex)
            {
                 DataRow row = ((DataRowView)e.Row.DataItem).Row;
                //((DropDownList)e.Row.FindControl("ddlActive")).SelectedValue = row["IsActive"].ToString();
            }
        }

        protected void gvTPLTariffGroups_RowEditing(object sender, GridViewEditEventArgs e)
        {
            //System.Threading.Thread.Sleep(2000);
           // resetErrorForTariffGroup("", false);
            gvTPLTariffGroups.EditIndex = e.NewEditIndex;
            TariffGroupList_buildGridData(TariffGroupList_buildGridData());
        }

        protected void gvTPLTariffGroups_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
           // resetErrorForTariffGroup("", false);
            gvTPLTariffGroups.EditIndex = -1;
            TariffGroupList_buildGridData(TariffGroupList_buildGridData());
        }

        protected void gvTPLTariffGroups_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
           // resetErrorForTariffGroup("", false);
            gvTPLTariffGroups.PageIndex = e.NewPageIndex;
            TariffGroupList_buildGridData(TariffGroupList_buildGridData());
        }

        protected void gvTPLTariffGroups_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            // resetErrorForTariffGroup("", false);
            cp = HttpContext.Current.User as CustomPrincipal;
            Page.Validate("vRequiredTariffGroup");
            if (!Page.IsValid)
            {
                return;
            }

            GridViewRow row = gvTPLTariffGroups.Rows[e.RowIndex];

            if (row != null)
            {

                string vActivityRateGroupID = ((Literal)row.FindControl("ltActivityRateGroupID")).Text;
                string VActivityRateGroup = ((TextBox)row.FindControl("txtActivityRateGroup")).Text;
                string VRateGroupDescription = ((TextBox)row.FindControl("txtRateGroupDescription")).Text;
                //string IsActive = ((DropDownList)row.FindControl("ddlActive")).SelectedValue;
                //string IsDeleted = ((DropDownList)row.FindControl("ddlDelete")).SelectedValue;

                string chkActive = ((CheckBox)row.FindControl("chkActive")).Checked == true ? "1" : "0";

                //if (DB.GetSqlS("SELECT ActivityRateGroup S from TPL_Activity_RateGroup WHERE IsDeleted=0 AND IsActive=1").ToLower() == VActivityRateGroup.ToLower() && vActivityRateGroupID=="0")
                //{
                //    resetErrorForTariffGroup("ActivityRateGroup already created", true);
                //    return;
                //}
                int ActivityRateGroupID = 0;
                int ActivityRateGroupID1 = 0;
                if (vActivityRateGroupID == "" || vActivityRateGroupID == "0")
                {
                    ActivityRateGroupID = (DB.GetSqlN("SELECT Count(ActivityRateGroupID) AS N from TPL_Activity_RateGroup WHERE IsDeleted=0 AND ActivityRateGroup LIKE '%" + VActivityRateGroup + "%'"));
                    if (ActivityRateGroupID != 0)
                    {
                        resetErrorForTariffGroup("ActivityRate Group already created", true);
                        return;
                    }
                }
                if (vActivityRateGroupID != "" || vActivityRateGroupID != "0")
                {
                    ActivityRateGroupID1 = (DB.GetSqlN("SELECT Count(ActivityRateGroupID) AS N from TPL_Activity_RateGroup WHERE IsDeleted=0 and ActivityRateGroupID!="+ vActivityRateGroupID + " AND ActivityRateGroup LIKE '%" + VActivityRateGroup + "%'"));
                    if (ActivityRateGroupID1 != 0)
                    {
                        resetErrorForTariffGroup("ActivityRate Group already created", true);
                        return;
                    }
                }

                StringBuilder QueryForActivityGroup = new StringBuilder();
                QueryForActivityGroup.Append(" EXEC [dbo].[sp_TPL_UpsertActivityRateGroup] ");
                QueryForActivityGroup.Append(" @ActivityRateGroup=" + DB.SQuote(VActivityRateGroup));
                QueryForActivityGroup.Append(",@RateGroupDescription=" + CommonLogic.IIF(VRateGroupDescription == "", "NULL", DB.SQuote(VRateGroupDescription)));
                QueryForActivityGroup.Append(",@CreatedBy=" + cp.UserID);
                QueryForActivityGroup.Append(",@IsActive=" + chkActive);
                QueryForActivityGroup.Append(",@IsDeleted=0");
                QueryForActivityGroup.Append(",@ActivityRateGroupID=" + vActivityRateGroupID);
                try
                {
                    DB.ExecuteSQL(QueryForActivityGroup.ToString());
                    resetErrorForTariffGroup("Successfully updated", false);
                }
                catch (Exception ex)
                {
                    resetErrorForTariffGroup("Error while updating", true);
                }

                gvTPLTariffGroups.EditIndex = -1;

                this.TariffGroupList_buildGridData(this.TariffGroupList_buildGridData());
                ActivityRateType_buildGridData(ActivityRateType_buildGridData());
                ActivityRate_buildGridData(ActivityRate_buildGridData());

            }
        }

        protected void lnkAddNewTariffGroup_Click(object sender, EventArgs e)
        {
            try
            {
                resetErrorForTariffGroup("", false);
                gvTPLTariffGroups.EditIndex = 0;
                gvTPLTariffGroups.PageIndex = 0;

                DataSet dsTariffGroups = TariffGroupList_buildGridData();
                DataRow row = dsTariffGroups.Tables[0].NewRow();
                row["ActivityRateGroupID"] = 0;
                dsTariffGroups.Tables[0].Rows.InsertAt(row, 0);
                this.TariffGroupList_buildGridData(dsTariffGroups);
                //((DropDownList)gvTPLTariffGroups.Rows[0].FindControl("ddlDelete")).SelectedValue = "0";
            }
            catch (Exception ex)
            {
                this.resetErrorForTariffGroup("Error while inserting", true);
            }
        }

        protected void lnkTariffGroupSearch_Click(object sender, EventArgs e)
        {
            //resetErrorForTariffGroup("", false);
            cp = HttpContext.Current.User as CustomPrincipal;
            string text = txtActivitytariffGroup1.Text.Trim();
            if (text == "Tariff Group...")
                text = "";

            ViewState["TPLTariffGroupList"] = "EXEC [sp_TPL_GetDataForTariffCreation]  @TariffID=1 ,@UserId="+cp.UserID+",@SearchText=" + DB.SQuote(text);

            this.TariffGroupList_buildGridData(this.TariffGroupList_buildGridData());
            //hifSelectedTabID.Value=hifTarrifGrouid.Value;
            //hifTabsActivityRateType.Value = hifTarrifGrouid.Value;
            AddDynamicTabs(ltActivityGroups, hifSelectedTabID, "Activity");
            AddDynamicTabs(ltDynamicTabsActivityRateType, hifTabsActivityRateType, "ActivityType");
            ActivityRateType_buildGridData(ActivityRateType_buildGridData());
            ActivityRate_buildGridData(ActivityRate_buildGridData());
        }

        //protected void resetErrorForTariffGroup(string error, bool isError)
        //{

        //    string str = "<font class=\"noticeMsg\">NOTICE:</font>&nbsp;&nbsp;&nbsp;";
        //    if (isError)
        //        str = "<font class=\"errorMsg\">ERROR:</font>&nbsp;&nbsp;&nbsp;";

        //    if (error.Length > 0)
        //        str += error + "";
        //    else
        //        str = "";

        //    lblTariffGroup.Text = str + "</font>";

        //}

        protected void resetErrorForTariffGroup(string error, bool isError)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "showStickyToast", "showStickyToast(" + (isError.ToString().ToLower() == "true" ? "false" : "true") + ",\"" + error + "\");", true);
        }

        public string GetCheckValueOrDeleted(String IsChecked)
        {

            String resultValue = "";
            if (IsChecked != "")
            {
                if (Convert.ToBoolean(Convert.ToInt32(IsChecked)))
                    resultValue = "<img width='22'  src=\"data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAADAAAAAwCAYAAABXAvmHAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAAMySURBVGhD7Zm9axRBGMYPvwo7vyr1rxCDVukkuBdnDu5Qk0ZBrW3UYHNdzkJQTCDG/AUx7Kwx0SpgfWBjVKz9akxirYKc77N5D8LeO3ezu7Nrsw/8YHO7877PfOzM7KRWqVKl/Go9b+2fDBvngkjdr0cqDIz+WDf6J13/iaFr+u1DfI+ewbPtdnsfF/9/UpE6TeYekLnv9Uj3UmH0N5SdCBunOFx5qr+sH6fWfEat+Vs0lwLEIBYQk8MXKzI+RexIZvJAvbiF2JzGv2icH6IkS1Jyn1COp2cWbx3ktH5E3XuYuvm1lLAQjH6FnJw+n+KWL9N8H6NXvfREGcPGBoYT28imIGxMS4HLRV1mO+k0sdw8QUNnWw5aHpidtNHH2Ja70H1SwKK4tjEl/g6oIefZlpuwwlKh3IuUK3e7N3rdv496Dz/dFu/DS7AWnGR7o4UlXgpUBH3zb3uPY4ZUosP2hgubrHifIgTxTdI8wN/XN6YHnqUh/cVpA0jmzycLF4HNPH6XngdUiTG2aRd1FW2J5QC+yGI+xugZtmkXPWTEwp7IbB4YvcI27aJuog8PobAHcpmPUe/Zpl1US/qSkgoPMmzuTpLffDwTbbNNu1znf0x3rgZ8mAfwxjbtcqkAzLsa8WUeOFVg1BDCHO1q6E73pjfzwGkI4UWRCu/FpVV9m48xepNt2kUVCMXCCWyVgPFCzAOnaTTFQmarRCHmiYuRusc27cKhk1TYhlSJvfgyHxM2zrJNu+LNXKS+igEs2Crh07zzZg6iYdSRggwjWQlc412Qns2GmmV7o4XjPpf1IEn/5fVu3uhfqT5oIKrAghhsBP1ZSLqXHTXHttyFD2ksHHLAMlE/Liw3j7KtdJo0+qoctDwC02ixnWyqG7UoBS4Fo5+wjezC0SIFWhUTFAnlxD9O2EY+4aCVxuK6mKgA6N1b83a42xcOWss57FJz42/GD3Ba/wrCxhWqyJacPDuIGby41OQ0xYqn2Pksi90AtEih1YP14AiHL0+8Yneo9T6L5oawW0bNpl5hixA2WWRojFpzhlghNsncDnpot5fo2uh3uIctMXaVzhuzSpUqDVGt9g9nUxYfKM5I0wAAAABJRU5ErkJggg==\"  />";
            }

            return resultValue;
        }

        protected void lnkDeleteActivityTariffGroup_Click(object sender, EventArgs e)
        {
            string gvIDs = "";
            bool chkBox = false;
            cp = HttpContext.Current.User as CustomPrincipal;
            StringBuilder sqlDeleteString = new StringBuilder();

            foreach (GridViewRow gv in gvTPLTariffGroups.Rows)
            {
                CheckBox isDelete = (CheckBox)gv.FindControl("chkIsDelete");
                if (isDelete.Checked)
                {
                    chkBox = true;
                    gvIDs += ((Literal)gv.FindControl("ltActivityRateGroupID")).Text.ToString() + ",";
                }
            }

            gvTenants = DB.GetSqlS("SELECT TenantName S FROM TPL_Tenant_Activity_Rate ACR JOIN TPL_Tenant TNT ON TNT.TenantID=ACR.TenantID AND TNT.IsActive=1 AND TNT.IsDeleted=0 WHERE ACR.IsDeleted=0 AND ACR.IsActive=1 AND ACR.ActivityRateID in (" + gvIDs.Substring(0, gvIDs.LastIndexOf(",")) + ")");

            if (DB.GetSqlN("SELECT COUNT(*) N FROM TPL_Tenant_Activity_Rate WHERE IsDeleted=0 AND ActivityRateID in (" + gvIDs.Substring(0, gvIDs.LastIndexOf(",")) + ")") > 0)
            {
                resetErrorActivityRate("This Activity Tariff Is assigned to Tenants: " + gvTenants, true);
                return;
            }

            if (chkBox)
            {
                sqlDeleteString.Append("BEGIN TRAN ");

                sqlDeleteString.Append("UPDATE TPL_Activity_RateGroup SET IsDeleted=1 WHERE ActivityRateGroupID IN (" + gvIDs.Substring(0, gvIDs.LastIndexOf(",")) + ")");

                sqlDeleteString.Append(" COMMIT ");
            }
            try
            {
                if (chkBox)
                {
                    DB.ExecuteSQL(sqlDeleteString.ToString());
                }
                resetErrorForTariffGroup("Successfully deleted the selected items", false);

            }
            catch (Exception ex)
            {
                resetErrorForTariffGroup("Error Deleting data" + ex.ToString(), true);
            }

            ViewState["TPLTariffGroupList"] = "EXEC [sp_TPL_GetDataForTariffCreation]  @TariffID=1,@UserId ="+cp.UserID+", @SearchText=''";
            this.TariffGroupList_buildGridData(this.TariffGroupList_buildGridData());

        }

        #endregion -------------------------Tariff Groups -----------------------------


        #region ------------------------- Activities Rate Types -----------------------------

        private DataSet ActivityRateType_buildGridData()
        {
            string ActivityRateTypesql = ViewState["TPLActivityRateType"].ToString() + ",@ActivityRateGroupID=" + hifTabsActivityRateType.Value;
            DataSet dsActivityRateType = DB.GetDS(ActivityRateTypesql, false);
            return dsActivityRateType;
        }

        private void ActivityRateType_buildGridData(DataSet cMdActivityRateType)
        {
            gvActivityRateType.DataSource = cMdActivityRateType;
            gvActivityRateType.DataBind();
            cMdActivityRateType.Dispose();

            //AddDynamicTabs();
            AddDynamicTabs(ltDynamicTabsActivityRateType, hifTabsActivityRateType, "ActivityType");
        }

        private void AddDynamicTabs(Literal _literal,HiddenField hidenField,string type)//
        {
            ActivityRate = (Dictionary<string, string>)ViewState["ActivityRate"];
            StringBuilder DynamicTabs = new StringBuilder();
            DynamicTabs.Append("<ul class=\"tabs\">");
            foreach (KeyValuePair<string, string> row in ActivityRate)
            {
                DynamicTabs.Append("<li  class=\" " + (row.Key == hidenField.Value ? "selected" : "") + " \">");
                DynamicTabs.Append("<a id=\"tab" + row.Key + "\" class=\"DynamicTabs\" onclick=SelecteTab(" + row.Key + ",'" + type + "')>" + row.Value + "</a>");
                DynamicTabs.Append("</li>");
            }
            DynamicTabs.Append("</ul>");
            _literal.Text = DynamicTabs.ToString();
        }
        
        protected void gvActivityRateType_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow && e.Row.RowIndex == gvActivityRateType.EditIndex)
            {
                DataRow row = ((DataRowView)e.Row.DataItem).Row;
                // ((HiddenField)e.Row.FindControl("hifARTActivityRateGroup")).Value = row["ActivityRateGroupID"].ToString();
                //((DropDownList)e.Row.FindControl("ddlActive")).SelectedValue = row["IsActive"].ToString();
            }
        }

        protected void gvActivityRateType_RowEditing(object sender, GridViewEditEventArgs e)
        {
          //  resetErrorActivityRateType("", false);
            gvActivityRateType.EditIndex = e.NewEditIndex;
            ActivityRateType_buildGridData(ActivityRateType_buildGridData());
        }

        
        protected void gvActivityRateType_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
           // resetErrorActivityRateType("", false);
            gvActivityRateType.EditIndex = -1;
            ActivityRateType_buildGridData(ActivityRateType_buildGridData());
        }

        protected void gvActivityRateType_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
          //  resetErrorActivityRateType("", false);
            gvActivityRateType.PageIndex = e.NewPageIndex;
            ActivityRateType_buildGridData(ActivityRateType_buildGridData());
        }

        protected void gvActivityRateType_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            // resetErrorActivityRateType("", false);
            cp = HttpContext.Current.User as CustomPrincipal;
            Page.Validate("vRequiredActivityRateType");
            if (!Page.IsValid)
            {
                return;
            }

            GridViewRow row = gvActivityRateType.Rows[e.RowIndex];

            if (row != null)
            {
                //string hifARTActivityRateGroup = ((HiddenField)row.FindControl("hifARTActivityRateGroup")).Value;
                string hifServiceType = ((HiddenField)row.FindControl("hifServiceType")).Value; 
                string hifInOut = ((HiddenField)row.FindControl("hifInOut")).Value;

                string vActivityRateTypeID = ((Literal)row.FindControl("ltActivityRateTypeID")).Text;
                string vActivityRateType = ((TextBox)row.FindControl("txtActivityRateType")).Text; 
                //string IsActive = ((DropDownList)row.FindControl("ddlActive")).SelectedValue;
                //string IsDeleted = ((DropDownList)row.FindControl("ddlDelete")).SelectedValue;

                string chkActive = ((CheckBox)row.FindControl("chkActive")).Checked == true ? "1" : "0";
                int ActivityRatetype = 0;
                int ActivityRatetype1 = 0;
                if (vActivityRateTypeID == "" || vActivityRateTypeID == "0")
                {
                    ActivityRatetype = (DB.GetSqlN("SELECT Count(ActivityRateType) AS N from TPL_Activity_RateType WHERE IsDeleted = 0 AND ActivityRateTypeID=" + vActivityRateTypeID+ " and ActivityRateType LIKE '%" + vActivityRateType.ToLower() + "%'"));
                    if (ActivityRatetype != 0)
                    {
                        resetErrorForTariffGroup("ActivityRate Group already created", true);
                        return;
                    }
                }
                if (vActivityRateTypeID != "" || vActivityRateTypeID != "0")
                {
                    //if (DB.GetSqlS("SELECT ActivityRateType S from TPL_Activity_RateType WHERE IsDeleted=0 AND IsActive=1").ToLower() == vActivityRateType.ToLower() && vActivityRateTypeID == "0")
                    ActivityRatetype1 = (DB.GetSqlN("SELECT Count(ActivityRateType) AS N from TPL_Activity_RateType WHERE IsDeleted=0 AND ServiceTypeID='"+ hifServiceType+ "' AND ActivityRateTypeID!=" + vActivityRateTypeID + " and ActivityRateType LIKE '%" + vActivityRateType.ToLower() + "%'"));
                    if (ActivityRatetype1 != 0)
                    {
                        {
                            resetErrorActivityRateType("ActivityRate Type already created", true);
                            return;
                        }
                    }
                }

                //if(vActivityRateTypeID!="" || vActivityRateTypeID!="0")

                //if (DB.GetSqlS("SELECT ActivityRateType S from TPL_Activity_RateType where ActivityRateTypeID != "+vActivityRateTypeID+" and TPL_Activity_RateType = LIKE '%" + vActivityRateType.ToLower() + "%'""));
                //{
                //    resetErrorActivityRateType("ActivityRateType already created", true);
                //    return;
                //}

                StringBuilder QueryForRateType = new StringBuilder();
                QueryForRateType.Append(" EXEC [dbo].[sp_TPL_UpsertActivityRateType] ");
                QueryForRateType.Append("@ActivityRateGroupID=" + hifTabsActivityRateType.Value);
                QueryForRateType.Append(",@ActivityRateType=" + DB.SQuote(vActivityRateType));
                QueryForRateType.Append(",@ServiceTypeID=" + hifServiceType);
                QueryForRateType.Append(",@InOutID=" + hifInOut);
                QueryForRateType.Append(",@CreatedBy=" + cp.UserID);
                QueryForRateType.Append(",@IsActive=" + chkActive);
                QueryForRateType.Append(",@IsDeleted=0");
                QueryForRateType.Append(",@ActivityRateTypeID=" + vActivityRateTypeID);

               
                try
                {
                    DB.ExecuteSQL(QueryForRateType.ToString());
                    resetErrorActivityRateType("Successfully updated", false);
                }
                catch (Exception ex)
                {
                    resetErrorActivityRateType("Error while updating", true);
                }

                gvActivityRateType.EditIndex = -1;

                ActivityRateType_buildGridData(ActivityRateType_buildGridData());
                ActivityRate_buildGridData(ActivityRate_buildGridData());

            }
        }

        protected void lnkAddnewActivityRateType_Click(object sender, EventArgs e)
        {
            try
            {
                //resetErrorActivityRateType("", false);
                gvActivityRateType.EditIndex = 0;
                gvActivityRateType.PageIndex = 0;

                DataSet dsActivityRateType = ActivityRateType_buildGridData();
                DataRow row = dsActivityRateType.Tables[0].NewRow();
                row["ActivityRateTypeID"] = 0;
                dsActivityRateType.Tables[0].Rows.InsertAt(row, 0);
                this.ActivityRateType_buildGridData(dsActivityRateType);
                //((DropDownList)gvActivityRateType.Rows[0].FindControl("ddlDelete")).SelectedValue = "0";
            }
            catch (Exception ex)
            {
                this.resetErrorActivityRateType("Error while inserting", true);
            }
        }

        protected void lnkActivityRateTypeSearch_Click(object sender, EventArgs e)
        {
            // resetErrorActivityRateType("", false);

            cp = HttpContext.Current.User as CustomPrincipal;
            string TariffType = txtActivityTariffType2.Text.Trim();
            if (TariffType == "Tariff Sub-Group...")
                TariffType = "";

            ViewState["TPLActivityRateType"] = "EXEC [sp_TPL_GetDataForTariffCreation]  @TariffID=2,@UserId ="+cp.UserID+", @SearchText=" + DB.SQuote(TariffType);

            this.ActivityRateType_buildGridData(ActivityRateType_buildGridData());
            
        }

        //protected void resetErrorActivityRateType(string error, bool isError)
        //{

        //    string str = "<font class=\"noticeMsg\">NOTICE:</font>&nbsp;&nbsp;&nbsp;";
        //    if (isError)
        //        str = "<font class=\"errorMsg\">ERROR:</font>&nbsp;&nbsp;&nbsp;";

        //    if (error.Length > 0)
        //        str += error + "";
        //    else
        //        str = "";

        //    lblActivityrateType.Text = str + "</font>";

        //}
        protected void resetErrorActivityRateType(string error, bool isError)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "showStickyToast", "showStickyToast(" + (isError.ToString().ToLower() == "true" ? "false" : "true") + ",\"" + error + "\");", true);
        }
        protected void resetErrorActivityRate(string error, bool isError)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "showStickyToast", "showStickyToast(" + (isError.ToString().ToLower() == "true" ? "false" : "true") + ",\"" + error + "\");", true);
        }
        protected void lnkDeleteActivityTariffType_Click(object sender, EventArgs e)
        {
            string gvIDs = "";
            bool chkBox = false;
            cp = HttpContext.Current.User as CustomPrincipal;
            StringBuilder sqlDeleteString = new StringBuilder();

            foreach (GridViewRow gv in gvTPLTariffGroups.Rows)
            {
                CheckBox isDelete = (CheckBox)gv.FindControl("chkIsDelete");
                if (isDelete.Checked)
                {
                    chkBox = true;
                    gvIDs += ((Literal)gv.FindControl("ltActivityRateTypeID")).Text.ToString() + ",";
                }
            }

            if (chkBox)
            {
                sqlDeleteString.Append("BEGIN TRAN ");

                sqlDeleteString.Append("UPDATE TPL_Activity_Rate SET IsDeleted=1 WHERE ActivityRateTypeID IN (" + gvIDs.Substring(0, gvIDs.LastIndexOf(",")) + ")");

                sqlDeleteString.Append(" COMMIT ");
            }
            try
            {
                if (chkBox)
                {
                    DB.ExecuteSQL(sqlDeleteString.ToString());
                }
                resetErrorForTariffGroup("Successfully deleted the selected items", false);

            }
            catch (Exception ex)
            {
                resetErrorForTariffGroup("Error Deleting data" + ex.ToString(), true);
            }

            ViewState["TPLActivityRateType"] = "EXEC [sp_TPL_GetDataForTariffCreation]  @TariffID=2,@UserId ="+cp.UserID+", @SearchText=''";
            this.ActivityRateType_buildGridData(ActivityRateType_buildGridData());
        }
        
        #endregion -------------------------Activities Rate Types-----------------------------

        #region -------------------------Tariff  List -----------------------------

        private DataSet ActivityRate_buildGridData()
        {

            string ActivityRatesql = ViewState["TPLActivityRate"].ToString() + ",@ActivityRateGroupID=" + hifSelectedTabID.Value + ",@AccountID=" + cp.AccountID;
            DataSet dsActivityRate = DB.GetDS(ActivityRatesql, false);
            return dsActivityRate;
        }


        public bool Getstatus(string isdefault)
        {
            return (isdefault == "1");
            //return true;
        }

        private void ActivityRate_buildGridData(DataSet cMdActivityRate)
        {
            gvActivityRate.DataSource = cMdActivityRate;
            gvActivityRate.DataBind();
            cMdActivityRate.Dispose();

            AddDynamicTabs(ltActivityGroups, hifSelectedTabID,"Activity");
            //ActivityRate = (Dictionary<string, string>)ViewState["ActivityRate"];
            //StringBuilder DynamicTabs = new StringBuilder();
            //DynamicTabs.Append("<ul class=\"tabs\">");
            //foreach (KeyValuePair<string, string> row in ActivityRate)
            //{
            //    DynamicTabs.Append("<li  class=\" " + (row.Key == hifSelectedTabID.Value ? "selected" : "") + " \">");
            //    DynamicTabs.Append("<a id=\"tab" + row.Key + "\" class=\"DynamicTabs\" onclick=SelecteTab(" + row.Key + ",'Activity')>" + row.Value + "</a>");
            //    DynamicTabs.Append("</li>");
            //}
            //DynamicTabs.Append("</ul>");
            //ltActivityGroups.Text = DynamicTabs.ToString();
        }

        protected void gvActivityRate_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.DataItem == null )
                return;

            string query = "select COUNT(*) AS N  from TPL_Tenant_Activity_Rate TAR JOIN TPL_Tenant TEN ON TAR.TenantID = TEN.TenantID where TEN.AccountID = " + cp.AccountID + "AND TAR.IsActive=1 AND TAR.IsDeleted=0 AND FORMAT(GETDATE(),'yyyy-MM-dd') BETWEEN FORMAT(TAR.EffectiveFrom,'yyyy-MM-dd') AND  FORMAT(TAR.EffectiveTo,'yyyy-MM-dd')";
            int AllocId = 0;
            AllocId = DB.GetSqlN(query);
            LinkButton lnkTenantTariff = (LinkButton)e.Row.FindControl("lnkTenantTariff");
            if (AllocId == 0)
            {
                lnkTenantTariff.Enabled = false;
            }
            else
            {
                lnkTenantTariff.OnClientClick = "openDialog('Tenant Tariff', '" + lnkTenantTariff.ClientID + "');";

                if (e.Row.RowType == DataControlRowType.DataRow && e.Row.RowIndex == gvActivityRate.EditIndex)
                {
                    DataRow row = ((DataRowView)e.Row.DataItem).Row;
                    //((DropDownList)e.Row.FindControl("ddlActive")).SelectedValue = row["IsActive"].ToString();
                    //((DropDownList)e.Row.FindControl("ddlIsDefaultRate")).SelectedValue = row["IsDefaultRate"].ToString();
                }
            }
        }

       

        protected void gvActivityRate_RowEditing(object sender, GridViewEditEventArgs e)
        {
          //  resetErrorActivityRate("", false);
            gvActivityRate.EditIndex = e.NewEditIndex;
            ActivityRate_buildGridData(ActivityRate_buildGridData());
        }

        protected void gvActivityRate_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            
        }

        protected void gvActivityRate_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            //resetErrorActivityRate("", false);
            //gvActivityRate.EditIndex = -1;
            //ActivityRate_buildGridData(ActivityRate_buildGridData());
        }

        protected void gvActivityRate_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
          //  resetErrorActivityRate("", false);
            gvActivityRate.PageIndex = e.NewPageIndex;
             ActivityRate_buildGridData(ActivityRate_buildGridData());
            foreach (GridViewRow gv in gvActivityRate.Rows)
            {
                CheckBox isDelete = (CheckBox)gv.FindControl("chkIsDelete");
                isDelete.Visible = true;
            }

        }

        protected void lnkActivityRateSearch_Click(object sender, EventArgs e)
        {
           // resetErrorActivityRate("", false);

            string TariffType = txtActivityTariffType3.Text.Trim();
            if (TariffType == "Tariff Sub-Group...")
                TariffType = "";

            string Tariff = txtActivityTariff3.Text.Trim();
            if (Tariff == "Tariff...")
                Tariff = "";
            string WareHouseID = hidWareHouseID.Value;
            if(WareHouseID == "0" || WareHouseID == null || WareHouseID == "")
                WareHouseID = "0";
            ViewState["TPLActivityRate"] = "EXEC [sp_TPL_GetDataForTariffCreation]  @TariffID=3, @SearchText=" + DB.SQuote(TariffType) + ",@ActivityRateType=" + DB.SQuote(Tariff) + ",@UserId="+cp.UserID+",@WareHouseID =" + WareHouseID;
            ActivityRate_buildGridData(ActivityRate_buildGridData());

             
        }

        protected void lnkAddNewActivityRate_Click(object sender, EventArgs e)
        {
            try
            {
              //  resetErrorActivityRate("", false);
                gvActivityRate.EditIndex = 0;
                gvActivityRate.PageIndex = 0;

                DataSet dsActivityRate = ActivityRate_buildGridData();
                DataRow row = dsActivityRate.Tables[0].NewRow();
                row["ActivityRateID"] = 0;
                row["IsDefaultRate"] = 0;
                row["IsActive"] = 0;
                row["WarehouseID"] = 0;
                row["WareHouse"] = "";
                dsActivityRate.Tables[0].Rows.InsertAt(row, 0);
                this.ActivityRate_buildGridData(dsActivityRate);
                //((DropDownList)gvActivityRate.Rows[0].FindControl("ddlDelete")).SelectedValue = "0";
            }
            catch (Exception ex)
            {
                this.resetErrorActivityRate("Error while inserting", true);
            }
        }

        //protected void resetErrorActivityRate(string error, bool isError)
        //{

        //    string str = "<font class=\"noticeMsg\">NOTICE:</font>&nbsp;&nbsp;&nbsp;";
        //    if (isError)
        //        str = "<font class=\"errorMsg\">ERROR:</font>&nbsp;&nbsp;&nbsp;";

        //    if (error.Length > 0)
        //        str += error + "";
        //    else
        //        str = "";

        //    lblActivityRate.Text = str + "</font>";

        //}

        protected void lnkDeleteActivityRate_Click(object sender, EventArgs e)
        {
            string gvIDs = "";
            bool chkBox = false;

            StringBuilder sqlDeleteString = new StringBuilder();

            foreach (GridViewRow gv in gvActivityRate.Rows)
            {
                CheckBox isDelete = (CheckBox)gv.FindControl("chkIsDelete");
                if (isDelete.Checked)
                {
                    chkBox = true;
                    gvIDs += ((Literal)gv.FindControl("ltActivityRateID")).Text.ToString() + ",";
                }
            }

            //gvTenants=DB.GetSqlS("SELECT TenantName S FROM TPL_Tenant_Activity_Rate ACR JOIN TPL_Tenant TNT ON TNT.TenantID=ACR.TenantID AND TNT.IsActive=1 AND TNT.IsDeleted=0 WHERE ACR.IsDeleted=0 AND ACR.IsActive=1 AND ACR.ActivityRateID in ("+ gvIDs.Substring(0, gvIDs.LastIndexOf(",")) +")");

            gvTenants = "EXEC [sp_TPL_GetTenantTariffAllocation] @AccountID="+cp.AccountID+", @ActivityRateID=" + DB.SQuote(gvIDs.Substring(0, gvIDs.LastIndexOf(","))); 

            DataSet dsTenants = DB.GetDS(gvTenants, false);

            if (dsTenants.Tables[1].Rows[0]["TenantName"].ToString() != "" && dsTenants.Tables[1].Rows.Count!=0)
            {
                resetErrorActivityRate("This Activity Tariff Is assigned to Tenants: \"" + dsTenants.Tables[1].Rows[0]["TenantName"].ToString()+"\"", true);
                return;
            }

            if (chkBox)
            {
                sqlDeleteString.Append("BEGIN TRAN ");

                sqlDeleteString.Append("UPDATE TPL_Activity_Rate SET IsDeleted=1,DeletedOn=GETDATE() WHERE ActivityRateID IN (" + gvIDs.Substring(0, gvIDs.LastIndexOf(",")) + ")");

                sqlDeleteString.Append(" COMMIT ");
            }
            try
            {
                if (chkBox)
                {
                    DB.ExecuteSQL(sqlDeleteString.ToString());
                }
                resetErrorActivityRate("Successfully deleted the selected items", false);

            }
            catch (Exception ex)
            {
                resetErrorActivityRate("Error Deleting data" + ex.ToString(), true);
            }

            ViewState["TPLActivityTariff"] = "EXEC [sp_TPL_GetDataForTariffCreation]  @TariffID=3, @SearchText='',@ActivityRateType='',@UserId="+cp.UserID;
            this.ActivityRate_buildGridData(this.ActivityRate_buildGridData());
        }

        protected void gvActivityRate_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "TenantTariff")
            {
                int iden = Localization.ParseNativeInt(e.CommandArgument.ToString().Split(',')[0]);

                ViewState["TenantTariffRate"] = "EXEC [sp_TPL_GetTenantTariffAllocation] @AccountID="+cp.AccountID+ ", @ActivityRateID=" + DB.SQuote(iden.ToString()); 

                this.LoadTenantTariffData(this.LoadTenantTariffData());
            }
        }


        protected void lnkActivityRateEdit_Click(object sender, EventArgs e)
        {
            int newIndex = ((GridViewRow)((LinkButton)sender).Parent.Parent).RowIndex;
          //  resetErrorActivityRate("", false);
            gvActivityRate.EditIndex = newIndex;
            ActivityRate_buildGridData(ActivityRate_buildGridData());
        }

        protected void lnkActivityRateUpdate_Click(object sender, EventArgs e)
        {
            int RowIndex = ((GridViewRow)((LinkButton)sender).Parent.Parent).RowIndex;
          //  resetErrorActivityRate("", false);

            Page.Validate("vRequiredActivityRate");
            if (!Page.IsValid)
            {
                return;
            }

            GridViewRow row = gvActivityRate.Rows[RowIndex];

            if (row != null)
            {
                StringBuilder QueryForActivityRate = new StringBuilder();
                String EffectiveFromDate = "";
                String EffectiveToDate = "";
                int HistoryID = 0;
                string hifWareHouseID = ((HiddenField)row.FindControl("hifwarehouse")).Value;
                string CostPrice = ((TextBox)row.FindControl("txtCostPrice")).Text;
                string hifARActivityRateType = ((HiddenField)row.FindControl("hifARActivityRateType")).Value;

                if(hifARActivityRateType == "")
                {
                    hifARActivityRateType = "0";
                }

                string hifUoMID = ((HiddenField)row.FindControl("hifUoMID")).Value;
                string hifCurrency = ((HiddenField)row.FindControl("hifCurrency")).Value;

                string ltActivityRateID = ((Literal)row.FindControl("ltActivityRateID")).Text;
                string vActivityRateName = ((TextBox)row.FindControl("txtActivityRateName")).Text;
                string vActivityRateDescription = ((TextBox)row.FindControl("txtActivityRateDescription")).Text;

                string vUnitCost = ((TextBox)row.FindControl("txtUnitCost")).Text;
                TextBox txtEffectiveFrom = (TextBox)row.FindControl("txtEffectiveFrom");

                string HSNSAC = ((TextBox)row.FindControl("txthsnsaccode")).Text;

                string CGST = "0";
                string SGST = "0";
                string IGST = "0"; 
                string CESS = "0";

                //  CGST = Int16(((TextBox)row.FindControl("txtCGST")).Text);
                CGST = (((TextBox)row.FindControl("txtCGST")).Text) == "" ? "0" : (((TextBox)row.FindControl("txtCGST")).Text);
                SGST = (((TextBox)row.FindControl("txtSGST")).Text) == "" ? "0" : (((TextBox)row.FindControl("txtSGST")).Text);
                IGST = (((TextBox)row.FindControl("txtIGST")).Text) == "" ? "0" : (((TextBox)row.FindControl("txtIGST")).Text);
                CESS = (((TextBox)row.FindControl("txtCess")).Text) == "" ? "0" : (((TextBox)row.FindControl("txtCess")).Text);


                //TextBox txtEffectiveTo = (TextBox)row.FindControl("txtEffectiveTo");

                //string IsActive = ((DropDownList)row.FindControl("ddlActive")).SelectedValue;
                //string IsDeleted = ((DropDownList)row.FindControl("ddlDelete")).SelectedValue;

                //string IsDefaultRate = ((DropDownList)row.FindControl("ddlIsDefaultRate")).SelectedValue;

                string chkActive = ((CheckBox)row.FindControl("chkActive")).Checked == true ? "1" : "0";
                string chkIsDefaultRate = ((CheckBox)row.FindControl("chkIsDefaultRate")).Checked == true ? "1" : "0";


                //if (DB.GetSqlS("SELECT ActivityRateName S FROM TPL_Activity_Rate WHERE IsDeleted=0 AND IsActive=1 AND ActivityRateName="+DB.SQuote(vActivityRateName)).ToLower() == vActivityRateName.ToLower() && ltActivityRateID == "0")
                //{
                //    resetErrorActivityRate("Activity Tariff already exists", true);
                //    return;
                //}

                if (txtEffectiveFrom.Text.Trim() != "")
                    EffectiveFromDate = CommonLogic.GetConfigValue(cp.TenantID, "DateFormat") == "dd/MM/yyyy" ? DateTime.ParseExact(txtEffectiveFrom.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy") : txtEffectiveFrom.Text.Trim();
                else
                    EffectiveFromDate = "NULL";


               // if (txtEffectiveTo.Text.Trim() != "")
                //    EffectiveToDate = CommonLogic.GetConfigValue(cp.TenantID, "DateFormat") == "dd/MM/yyyy" ? DateTime.ParseExact(txtEffectiveTo.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy") : txtEffectiveTo.Text.Trim();
               // else
                    EffectiveToDate = null;

                //if ((DB.GetSqlN("SELECT ActivityRateID N FROM TPL_Activity_Rate WHERE ActivityRateName=" + DB.SQuote(vActivityRateName) + " AND IsDeleted=0 AND CONVERT(nvarchar(30),EffectiveFrom,101)=" + DB.SQuote(EffectiveFromDate) + " AND CONVERT(nvarchar(30),EffectiveTo,101)=" + DB.SQuote(EffectiveToDate)) > 0))
                //{
                //    resetErrorActivityRate("Activity Tariff already exists", true);
                //    return;
                //}

                if (ltActivityRateID != "0")
                {
                    if (DB.GetSqlN("EXEC [sp_TPL_GetDataForRateHistory] @UoMID=" + hifUoMID + ",@UnitCost=" + DB.SQuote(vUnitCost) + " ,@CurrencyID=" + hifCurrency + " ,@IsDefaultRate =" + DB.SQuote(chkIsDefaultRate) + ",@EffectiveFrom =" + DB.DateQuote(EffectiveFromDate) + ",@EffectiveTo=null,@ActivityRateID=" + ltActivityRateID) == 1)
                    {
                        HistoryID = 1;
                    }
                }

                QueryForActivityRate.Append(" EXEC [dbo].[sp_TPL_UpsertActivityRate] ");
                QueryForActivityRate.Append(" @ActivityRateTypeID=" + hifARActivityRateType);
                QueryForActivityRate.Append(",@ActivityRateName=" + DB.SQuote(vActivityRateName));
                QueryForActivityRate.Append(",@UoMID=" + hifUoMID);
                QueryForActivityRate.Append(",@UnitCost=" + vUnitCost);
                QueryForActivityRate.Append(",@CurrencyID=" + hifCurrency);
                QueryForActivityRate.Append(",@ActivityRateDescription=" + CommonLogic.IIF(vActivityRateDescription == "", "NULL", DB.SQuote(vActivityRateDescription)));

                //QueryForActivityRate.Append(",@EffectiveFrom=" + EffectiveFromDate);
                //QueryForActivityRate.Append(",@EffectiveTo=" + EffectiveToDate);

                QueryForActivityRate.Append(",@EffectiveFrom=" + DB.DateQuote(EffectiveFromDate));
                QueryForActivityRate.Append(",@EffectiveTo=null");

                QueryForActivityRate.Append(",@IsDefaultRate=" + chkIsDefaultRate);
                QueryForActivityRate.Append(",@CreatedBy=" + cp.UserID);
                QueryForActivityRate.Append(",@IsActive=" + chkActive);
                QueryForActivityRate.Append(",@IsDeleted=0");
                QueryForActivityRate.Append(",@ActivityRateID=" + ltActivityRateID);
                QueryForActivityRate.Append(",@HistoryID=" + HistoryID);
                QueryForActivityRate.Append(",@WareHouseID = " + hifWareHouseID);
                QueryForActivityRate.Append(",@CostPrice = " + CostPrice);
                QueryForActivityRate.Append(",@CGST = " + CGST);
                QueryForActivityRate.Append(",@SGST = " + SGST);
                QueryForActivityRate.Append(",@IGST = " + IGST);
                QueryForActivityRate.Append(",@CESS = " + CESS);
                QueryForActivityRate.Append(",@HSCSACCODE = " + DB.SQuote(HSNSAC));


                try
                {
                    DB.ExecuteSQL(QueryForActivityRate.ToString());
                    resetErrorActivityRate("Successfully updated", false);
                }
                catch (Exception ex)
                {
                    //resetErrorActivityRate("Error while updating", true);
                    resetErrorActivityRate("Activity Tariff already exists", true);
                }

                gvActivityRate.EditIndex = -1;

                ActivityRate_buildGridData(ActivityRate_buildGridData());
            }
        }

        protected void lnkActivityRateCancel_Click(object sender, EventArgs e)
        {
          //  resetErrorActivityRate("", false);
            gvActivityRate.EditIndex = -1;
            ActivityRate_buildGridData(ActivityRate_buildGridData());
        }

        #endregion -------------------------Tariff  List-----------------------------

        protected void lnkChangeTabs_Click(object sender, EventArgs e)
        {
            //txtActivityTariffType3.Text = "Tariff Sub-Group...";
            //txtActivityTariff3.Text = "Tariff...";
            cp = HttpContext.Current.User as CustomPrincipal;
            ViewState["TPLActivityRate"] = "EXEC [sp_TPL_GetDataForTariffCreation]  @TariffID=3, @SearchText='',@ActivityRateType='',@UserId="+cp.UserID;
            ActivityRate_buildGridData(ActivityRate_buildGridData());
        }

        #region -------------------------Tenant Tariff  List -----------------------------

        private DataSet LoadTenantTariffData()
        {
            string sql = ViewState["TenantTariffRate"].ToString();
            DataSet ds = DB.GetDS(sql, false);
            return ds;
        }

        private void LoadTenantTariffData(DataSet ds)
        {
            gvTenantTariffAllocation.DataSource = ds.Tables[0];
            gvTenantTariffAllocation.DataBind();
           ds.Dispose();

            ScriptManager.RegisterStartupScript(this, this.GetType(), "unblockDialog", "NProgress.done();unblockDialog();", true);
        }

        protected void gvTenantTariffAllocation_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //if (e.Row.RowType == DataControlRowType.DataRow && (e.Row.RowIndex == gvTenantTariffAllocation.EditIndex))
            //{
            //}
        }

        protected void gvTenantTariffAllocation_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvTenantTariffAllocation.PageIndex = e.NewPageIndex;
            LoadTenantTariffData(LoadTenantTariffData());
        }

        #endregion -------------------------Tenant Tariff  List-----------------------------

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "dialog", "closeDialog()", true);

            this.ActivityRate_buildGridData(ActivityRate_buildGridData());
        }

        protected void lnkbtntariffgroup_Click(object sender, EventArgs e)
        {
            try
            {
               // resetErrorActivityRate("", false);
                gvTPLTariffGroups.EditIndex = 0;
                gvTPLTariffGroups.PageIndex = 0;

                DataSet dsActivityRate = TariffGroupList_buildGridData();
                DataRow row = dsActivityRate.Tables[0].NewRow();
                row["ActivityRateGroupID"] = 0;
                row["ActivityRateGroup"] = "";
                row["IsActive"] = 0;
                dsActivityRate.Tables[0].Rows.InsertAt(row, 0);
                this.TariffGroupList_buildGridData(dsActivityRate);
                //((DropDownList)gvActivityRate.Rows[0].FindControl("ddlDelete")).SelectedValue = "0";
            }
            catch (Exception ex)
            {
                this.resetErrorActivityRate("Error while inserting", true);
            }
        }

        protected void lnkbtntariffsubgroup_Click(object sender, EventArgs e)
        {
            try
            {
               // resetErrorActivityRate("", false);
                gvActivityRateType.EditIndex = 0;
                gvActivityRateType.PageIndex = 0;

                DataSet dsActivityRate = ActivityRateType_buildGridData();
                DataRow row = dsActivityRate.Tables[0].NewRow();
                row["ActivityRateTypeID"] = 0;
                row["ActivityRateType"] = "";
                row["ServiceType"] = "";
                row["ServiceTypeID"] = 0;
                row["InOutType"] = "";
                row["InOutID"] = 0;
                row["IsActive"] = 0;
                dsActivityRate.Tables[0].Rows.InsertAt(row, 0);
                this.ActivityRateType_buildGridData(dsActivityRate);
                //((DropDownList)gvActivityRate.Rows[0].FindControl("ddlDelete")).SelectedValue = "0";
            }
            catch (Exception ex)
            {
                this.resetErrorActivityRate("Error while inserting", true);
            }
        }

        

       


    }
}