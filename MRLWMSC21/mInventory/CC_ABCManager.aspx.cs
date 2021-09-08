using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Security.Principal;
using System.Text;
using MRLWMSC21Common;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Collections.Generic;


namespace MRLWMSC21.mInventory
{
    public partial class CC_ABCManager : System.Web.UI.Page
    {
        #region Global Variables
        protected String CC_ABC_ListSQL =String.Empty;
        protected String gvCycleCountManagerListSQL = String.Empty;
        protected String gvCCMatListSQL = String.Empty;
        protected String PrintCCLineItemsListSQL = String.Empty;
        #endregion


     

        CustomPrincipal customprinciple = HttpContext.Current.User as CustomPrincipal;

        protected void Page_PreInit(object sender, EventArgs e)
        {
            Page.Theme = "blue_theme";
        }

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!customprinciple.IsInAnyRoles(CommonLogic.GetRolesAllowed(CommonLogic.GetRolesAllowedForthisPage("Material Movement"))))
            {
                Response.Redirect("../Login.aspx?eid=6");
            }
                       

            if (!IsPostBack)
            {
                CommonLogic.LoadStores(ddlWarehouse, "", 0, false);
                CommonLogic.LoadCycleClass(ddlCycleClass, "");
                CommonLogic.LoadUsers(ddlHandlerID, "Select Handler", "0");


                lnkAddNewCC.OnClientClick = "openDialogAndBlock('Add Cycle Count', '" + lnkAddNewCC.ClientID + "')";
                txtMCode.Attributes["onclick"] = "javascript:this.focus();";
                txtMCode.Attributes["onfocus"] = "javascript:this.select();";

            }
        }

        protected void lnkGetCycleClassData_Click(object sender, EventArgs e)
        {
            Page.Validate("GetCCData");

            string ware = ddlWarehouse.SelectedValue;
            string CycleClassID = ddlCycleClass.SelectedValue;
            if (ware == "0")
            {
                resetError("Please Select Ware House", true);
                return;
            }

            if (CycleClassID == "0")
            {
                resetError("Please Select Cycle Class", true);
                return;
            }

            gvCycleCountManagerListSQL = "[sp_CC_GetCycleCountList] @WarehouseID =" + ddlWarehouse.SelectedValue + ",@CycleClassID=" + DB.SQuote(ddlCycleClass.SelectedValue);
            ViewState["gvCycleCountManagerSQL"] = gvCycleCountManagerListSQL;
            this.gvCycleCountManager_buildGridData(this.gvCycleCountManager_buildGridData());


            CC_ABC_ListSQL = "[sp_CC_GetCycleDataBy_Class_WH] @WarehouseID =" + ddlWarehouse.SelectedValue + ", @CycleClassID=" + DB.SQuote(ddlCycleClass.SelectedValue) + ", @StartDate='01/01/2012',@EndDate=" + DB.DateQuote(DateTime.Now.ToString("MM/dd/yyyy")) + ",@MaterialCode=" + DB.SQuote(txtMCode.Text.Trim()) + ",@CycleCountID=0";
            ViewState["gvCCABCSQL"] = CC_ABC_ListSQL;
            this.gvCCABC_buildGridData(this.gvCCABC_buildGridData());

        }


      

        protected void resetBlkPOQtyError(string error, bool isError)
        {

            string str = "<font class=\"noticeMsg\">NOTICE:</font>&nbsp;&nbsp;&nbsp;";
            if (isError)
                str = "<font class=\"errorMsg\">ERROR:</font>&nbsp;&nbsp;&nbsp;";

            if (error.Length > 0)
                str += error + "";
            else
                str = "";


            ltStatus.Text = str;


        }



        #region -------------------------Grid View Cycle Count ABC -----------------------------

        protected DataSet gvCycleCountManager_buildGridData()
        {
            string sqlCoomand = ViewState["gvCycleCountManagerSQL"].ToString();
            // sql += ",@SortString= " + DB.SQuote(ViewState["gvCCABCSort"].ToString()) + ",@SortOrder=" + DB.SQuote(ViewState["gvCCABCSortOrder"].ToString());

            DataSet ds = DB.GetDS(sqlCoomand, false);
            //ltSITRecordCount.Text = "[" + ds.Tables[0].Rows.Count.ToString() + "]";
            //lblGroupLabelStatus.Text = "Total Labels Required : " + DB.RowFieldInt(ds.Tables[1].Rows[0], "TotalLables");
            return ds;

        }

        protected void gvCycleCountManager_buildGridData(DataSet dataset)
        {
            gvCycleCountManager.DataSource = dataset.Tables[0];
            gvCycleCountManager.DataBind();
            dataset.Dispose();

            if (dataset.Tables[0].Rows.Count == 0)
            {
                divCCRecordCount.InnerHtml= "";
                resetError("No items exists for the selected warehouse and cycle class category.", true);
                //resetBlkPOQtyError("No items exists for the selected warehouse and cycle class category.", false);
            }
            else
            {
                divCCRecordCount.InnerHtml= "";
                divCCRecordCount.InnerHtml += " <font color='#00A382'>";
                divCCRecordCount.InnerHtml += ddlWarehouse.SelectedItem.Text + " / Cycle Class -" + ddlCycleClass.SelectedValue + "  [" + dataset.Tables[0].Rows.Count + " Items]";
                divCCRecordCount.InnerHtml += "</font>";
                resetBlkPOQtyError("", false);
            }
        }


        protected void gvCycleCountManager_Sorting(object sender, GridViewSortEventArgs e)
        {
            ViewState["gvCycleCountManagerIsInsert"] = false;
            gvCycleCountManager.EditIndex = -1;
            ViewState["gvCycleCountManagerSort"] = e.SortExpression.ToString();
            ViewState["gvCycleCountManagerSortOrder"] = (ViewState["gvCycleCountManagerSortOrder"].ToString() == "ASC" ? "DESC" : "ASC");
            this.gvCycleCountManager_buildGridData(this.gvCycleCountManager_buildGridData());
        }


        protected void gvCycleCountManager_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.DataItem == null)
                return;

            LinkButton lnkModify = (LinkButton)e.Row.FindControl("lnkModify");
            LinkButton lnkPrintCCJobList = (LinkButton)e.Row.FindControl("lnkPrintCCJobList");
            ImageButton lnkStartStop = (ImageButton)e.Row.FindControl("lnkStartStop");
            Literal ltInitDate = (Literal)e.Row.FindControl("ltInitDate");
            Literal ltCloseDate = (Literal)e.Row.FindControl("ltCloseDate");
            Literal ltControlFlag = (Literal)e.Row.FindControl("ltControlFlag");


            if ((e.Row.RowState & DataControlRowState.Normal) == DataControlRowState.Normal)
            {
                if ((ltControlFlag.Text == "0" || ltControlFlag.Text == "") && ltInitDate.Text == "" && ltCloseDate.Text == "")
                {
                    lnkStartStop.ImageUrl = "../images/start.gif";
                    lnkStartStop.CommandName = "start";
                    lnkStartStop.ToolTip = "Start";
                    lnkModify.OnClientClick = "openDialogAndBlock('Edit Cycle Count', '" + lnkModify.ClientID + "')";
                }
                else if (ltControlFlag.Text == "1")
                {
                    lnkStartStop.ImageUrl = "../images/stop.gif";
                    lnkStartStop.CommandName = "stop";
                    lnkStartStop.ToolTip = "Stop";
                    lnkModify.OnClientClick = "openDialogAndBlock('Edit Cycle Count', '" + lnkModify.ClientID + "')";
                }
                else
                {
                    lnkStartStop.ImageUrl = "../images/CCclose.gif";
                    lnkStartStop.CommandName = "close";
                    lnkStartStop.ToolTip = "Closed";
                    lnkStartStop.Enabled = false;
                    lnkModify.Enabled = false;
                }

               
                lnkPrintCCJobList.OnClientClick = "openPrintCCListDialogAndBlock('Print Cycle Count List', '" + lnkPrintCCJobList.ClientID + "')";
            }
        }

        protected void gvCycleCountManager_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            ViewState["gvCycleCountManagerIsInsert"] = false;
            this.resetBlkPOQtyError("", false);
            gvCycleCountManager.PageIndex = e.NewPageIndex;
            gvCycleCountManager.EditIndex = -1;
            this.gvCycleCountManager_buildGridData(this.gvCycleCountManager_buildGridData());
        }

        protected void gvCycleCountManager_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            GridViewRow row = ((Control)e.CommandSource).NamingContainer as GridViewRow;
            String vCCID = e.CommandArgument.ToString();



            if (e.CommandName == "ModifyCCMatList")
            {
                gvCCMatListSQL = "[sp_CC_GetCycleDataBy_Class_WH] @WarehouseID =" + ddlWarehouse.SelectedValue + ",@CycleClassID=" + DB.SQuote(ddlCycleClass.SelectedValue) + ", @StartDate='01/01/2012',@EndDate=" + DB.DateQuote(DateTime.Now.ToString("MM/dd/yyyy")) + ",@MaterialCode=" + DB.SQuote(txtMCode.Text.Trim()) + ",@CycleCountID=" + vCCID;
                ViewState["gvCCMatListSQL"] = gvCCMatListSQL;
                this.gvCCMatList_buildGridData(this.gvCCMatList_buildGridData());

                FillEditCCMMListForm(vCCID);
                RegisterStartupScript("jsUnblockDialog", "unblockDialog();");
            }
            else if (e.CommandName == "CopyCCMatList")
            {

                SqlParameter[] SQLParms = {
                                            new SqlParameter("@CycleCountID",Convert.ToInt32(vCCID)),
                                            new SqlParameter("@CreatedBy", customprinciple.UserID.ToString())
                                        };
                try
                {
                    DB.ExecuteStoredProcedure("sp_CC_CopyCCWithMatItems", SQLParms);
                }
                catch (Exception ex)
                {
                    resetBlkPOQtyError(ex.ToString(), true);
                }

                //To Refresh the Grind after Copying
                string script = "__doPostBack(\"" + btnRefreshgvCycleCountManager.ClientID + "\", \"\");";
                RegisterStartupScript("jsGridRefresh", script);   //Trigger async grid refresh on Save

                this.gvCycleCountManager_buildGridData(this.gvCycleCountManager_buildGridData());
                //this.gvCCABC_buildGridData(this.gvCCABC_buildGridData());

            }
            else if (e.CommandName == "start")
            {

                try
                {
                    DB.ExecuteSQL("UPDATE QCC_CycleCount SET CCDateInitiated= GETDATE(), IsOn=1 WHERE CycleCountID=" + vCCID);
                    this.gvCycleCountManager_buildGridData(this.gvCycleCountManager_buildGridData());
                }
                catch (Exception ex)
                {
                }
            }
            else if (e.CommandName == "stop")
            {

                try
                {
                    DB.ExecuteSQL("UPDATE QCC_CycleCount SET CCDateClosed= GETDATE(),IsOn=0 WHERE CycleCountID=" + vCCID);
                    this.gvCycleCountManager_buildGridData(this.gvCycleCountManager_buildGridData());
                }
                catch (Exception ex)
                {
                }
            }
            else if (e.CommandName == "PrintCCList")
            {
                FillCycleCountMatList(vCCID);
                RegisterStartupScript("jsUnblockDialog", "unblockPrintCCListDialog();");
                AddGridHeaderList(vCCID, gvCCPrintCCList);
                PrintCCLineItemsListSQL = "[sp_CC_GetCCLineItemsListToPrint] @CycleCountID=" + vCCID;
                ViewState["PrintCCLineItemsListSQL"] = this.PrintCCLineItemsListSQL;
                this.PrintCCLineItemsList_buildGridData(this.PrintCCLineItemsList_buildGridData());

            }


        }

        //Add Header Namees
        private void AddGridHeaderList(String CCID,GridView gv)
        {
            IDataReader dsDynamicMSp = DB.GetRS("dbo.sp_CC_GetCCMaterialStorageParameters @CycleCountID="+CCID+",@TenantID="+customprinciple.TenantID);

            StringBuilder HeaderText = new StringBuilder();
            HeaderText.Append("<table width='100%' border=\"1\" align=\"center\" cellpadding='1'  style=\"border-collapse:collapse;border: 1px solid black;\"><tr><td align=\"left\" width='100'>Location</td>");

            while (dsDynamicMSp.Read())
            {
                HeaderText.Append("<td align=\"left\" width='100'>" + DB.RSField(dsDynamicMSp, "ParameterName") + "</td>");
            }
            HeaderText.Append("<td width='50' align=\"left\" >IsDam</td><td align=\"left\" width='50'>Available</td></tr></table>");
            gv.Columns[2].HeaderText = HeaderText.ToString();
        }

        private void FillCycleCountMatList(String vCycleCountID)
        {
            IDataReader rsCC = DB.GetRS("[sp_CC_GetCycleCountDetails] @CycleCountID=" + vCycleCountID);

            while (rsCC.Read())
            {
                ltCCDetails.Text = "<b>Cycle Count #:</b>" + vCycleCountID + "&nbsp;&nbsp; <b>Warehouse:</b>" + DB.RSField(rsCC, "WHCode") + "&nbsp;&nbsp; <b>Cycle Class:</b>" + DB.RSField(rsCC, "CycleClass") + "&nbsp;&nbsp; <b>Handler:</b>" + DB.RSField(rsCC, "Handler");
                ltCCDetails.Text += "<b>Initiated On:</b>" + DB.RSFieldDateTime(rsCC, "CCDateInitiated").ToString("dd/MM/yy") + "&nbsp;&nbsp; <b>Initiated By:</b>" + DB.RSField(rsCC, "CreatedBy");
            }
            rsCC.Close();
        }

        protected void btnRefreshgvCycleCountManager_Click(object sender, EventArgs e)
        {
            this.gvCycleCountManager_buildGridData(this.gvCycleCountManager_buildGridData());
        }

        private void FillEditCCMMListForm(String CycleCountID)
        {

            IDataReader rs = DB.GetRS("Select * from QCC_CycleCount Where CycleCountID=" + CycleCountID);
            while (rs.Read())
            {
                lthidCCID.Text = CycleCountID;
                txtCCName.Text = DB.RSField(rs, "Name");
                ddlHandlerID.SelectedValue = DB.RSFieldInt(rs, "HandlerID").ToString();
            }
            rs.Close();
        }

        private void RegisterStartupScript(string key, string script)
        {
            ScriptManager.RegisterStartupScript(phrJsRunner, phrJsRunner.GetType(), key, script, true);
        }



        private void TriggerClientGridRefresh()
        {
            string script = "__doPostBack(\"" + btnRefreshGrid.ClientID + "\", \"\");";
            script += "__doPostBack(\"" + btnRefreshgvCycleCountManager.ClientID + "\", \"\");";
            RegisterStartupScript("jsGridRefresh", script);   //Trigger async grid refresh on Save


        }

        private void HideCCMMListForm()
        {
            ClearEditCCMListForm();
            RegisterStartupScript("jsCloseDialg", "closeDialog();");    //Close dialog on client side
        }




        protected void btnRefreshGrid_Click(object sender, EventArgs e)
        {
            this.gvCCABC_buildGridData(this.gvCCABC_buildGridData());
        }

        private void ClearEditCCMListForm()
        {
            //Empty out text boxes
            var textBoxes = new List<Control>();
            FindControlsOfType(this.phrEditCC, typeof(TextBox), textBoxes);

            foreach (TextBox textBox in textBoxes)
                textBox.Text = "";

            //Clear validators
            var validators = new List<Control>();
            FindControlsOfType(this.phrEditCC, typeof(BaseValidator), validators);

            foreach (BaseValidator validator in validators)
                validator.IsValid = true;
        }

        private void ClearCCPrintListForm()
        {
            //Empty out text boxes
            var textBoxes = new List<Control>();
            FindControlsOfType(this.phrPrintCCList, typeof(TextBox), textBoxes);

            foreach (TextBox textBox in textBoxes)
                textBox.Text = "";

            //Clear validators
            var validators = new List<Control>();
            FindControlsOfType(this.phrPrintCCList, typeof(BaseValidator), validators);

            foreach (BaseValidator validator in validators)
                validator.IsValid = true;
        }


        static public void FindControlsOfType(Control root, Type controlType, List<Control> list)
        {
            if (root.GetType() == controlType || root.GetType().IsSubclassOf(controlType))
            {
                list.Add(root);
            }

            //Skip input controls
            if (!root.HasControls())
                return;

            foreach (Control control in root.Controls)
            {
                FindControlsOfType(control, controlType, list);
            }
        }
        private void resetError(string error, bool isError)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "test", "showStickyToast(" + (isError.ToString().ToLower() == "true" ? "false" : "true") + ",\"" + error + "\");", true);

        }
        protected void lnkAddNewCC_Click(object sender, EventArgs e)
        {
            //if (ddlWarehouse.SelectedIndex == 0)
            //{
            //    resetError("Please select 'Warehouse'", true);
            //    return;
            //}
            //if (ddlCycleClass.SelectedIndex == 0)
            //{
            //    resetError("Please select 'Cycle Class'", true);
            //    return;
            //}


            lthidCCID.Text = "";
            ClearEditCCMListForm();
            gvCCMatListSQL = "[sp_CC_GetCycleDataBy_Class_WH] @WarehouseID =" + ddlWarehouse.SelectedValue + ", @CycleClassID=" + ddlCycleClass.SelectedValue + ", @StartDate='01/01/2012',@EndDate=" + DB.DateQuote(DateTime.Now.ToString("MM/dd/yyyy")) + ",@MaterialCode='',@CycleCountID=0";
            ViewState["gvCCMatListSQL"] = gvCCMatListSQL;
            this.gvCCMatList_buildGridData(this.gvCCMatList_buildGridData());
            RegisterStartupScript("jsUnblockDialog", "unblockDialog();");
        }

        protected void lnkSaveCCWithMMList_Click(object sender, EventArgs e)
        {
            Page.Validate("CCCreation");
            if (!IsValid)
            {
                return;
            }

            // If New CycleCountID
            if (lthidCCID.Text == "")
                lthidCCID.Text = "0";

            int vSupplier =0;
            Int32 vMMDepartmentID = 0;



            DataTable dt = getSelectedMatList(Convert.ToInt32(lthidCCID.Text));
            //string sql = "exec sp_CC_UpsertCCWithMatItems @CycleCountID=" + Convert.ToInt32(lthidCCID.Text) + ",@Name='," + txtCCName.Text.Trim() + "',@WarehouseID=" + ddlWarehouse.SelectedValue + ",@CycleClassID=" + ddlCycleClass.SelectedValue.Trim() + ",@SupplierID=" + vSupplier + ",@MMDepartmentID=" + vMMDepartmentID + ",@HandlerID=" + ddlHandlerID.SelectedValue.Trim() + ",@CreatedBy=" + cp.UserID.ToString() + ",@MatAssigned="+dt+"";

            SqlParameter[] SQLParms = {
                                new SqlParameter("@CycleCountID",Convert.ToInt32(lthidCCID.Text)),
                                new SqlParameter("@Name", txtCCName.Text.Trim()),
                                new SqlParameter("@WarehouseID", ddlWarehouse.SelectedValue),
                                new SqlParameter("@CycleClassID", ddlCycleClass.SelectedValue.Trim()),
                                new SqlParameter("@SupplierID", vSupplier),
                                new SqlParameter("@MMDepartmentID", vMMDepartmentID),
                                new SqlParameter("@HandlerID", ddlHandlerID.SelectedValue.Trim()),
                                new SqlParameter("@CreatedBy", customprinciple.UserID.ToString()),
                                 new SqlParameter("@TenantID",customprinciple.TenantID),
                                new SqlParameter("@MatAssigned",dt)

                            };


            try
            {
                //DB.ExecuteSQL(sql);
                DB.ExecuteStoredProcedure("sp_CC_UpsertCCWithMatItems", SQLParms);
            }
            catch (Exception ex)
            {
                resetBlkPOQtyError(ex.ToString(), true);
            }


            HideCCMMListForm();
            TriggerClientGridRefresh();
            this.gvCycleCountManager_buildGridData(this.gvCycleCountManager_buildGridData());
            this.gvCCABC_buildGridData(this.gvCCABC_buildGridData());

        }

        private DataTable getSelectedMatList(int CycleCountID)
        {
            ArrayList arr;
            if (ViewState["Data"] != null)
            {
                arr = (ArrayList)ViewState["Data"];
                string MMID;

                foreach (GridViewRow gv in gvCCMatList.Rows)
                {
                    CheckBox isSelected = (CheckBox)gv.FindControl("chkSelforCC");
                    MMID = ((Literal)gv.FindControl("lthidMMID")).Text.ToString();
                    if (isSelected.Checked)
                    {

                        if (!arr.Contains(MMID))
                        {
                            arr.Add(MMID);
                        }
                    }
                    else
                    {
                        if (arr.Contains(MMID))
                        {
                            arr.Remove(MMID);
                        }
                    }
                }
                ViewState["Data"] = arr;
            }
            else
            {
                arr = new ArrayList();
                string MMID;

                foreach (GridViewRow gv in gvCCMatList.Rows)
                {
                    CheckBox isSelected = (CheckBox)gv.FindControl("chkSelforCC");
                    if (isSelected.Checked)
                    {
                        MMID = ((Literal)gv.FindControl("lthidMMID")).Text.ToString();
                        arr.Add(MMID);
                        //ViewState["Data"] = arr;

                    }
                }
            }

            string MaterialID = "";
            DataTable resDT = new DataTable();

            resDT.Columns.Add("CycleCountID", System.Type.GetType("System.Int32"));
            resDT.Columns.Add("MaterialMasterID", System.Type.GetType("System.Int32"));
            object[] vals = new object[2];
            for (int i = 0; i < arr.Count; i++)
            {
                MaterialID = (String)arr[i];
                vals[0] = CycleCountID;
                vals[1] = MaterialID;
                resDT.Rows.Add(vals);
            }

            //'Navigate through each row in the GridView for checkbox items
            /*foreach (GridViewRow gv in gvCCMatList.Rows)
            {
                CheckBox isSelected = (CheckBox)gv.FindControl("chkSelforCC");
                if (isSelected.Checked)
                {
                    MaterialID = ((Literal)gv.FindControl("lthidMMID")).Text.ToString();
                    object[] vals = new object[2];
                    vals[0] = CycleCountID;
                    vals[1] = MaterialID;
                    resDT.Rows.Add(vals);

                }
            }*/
            return resDT;
        }

        protected void lnkCancleCCWithMMLis_Click(object sender, EventArgs e)
        {
            HideCCMMListForm();
            TriggerClientGridRefresh();
            this.gvCCABC_buildGridData(this.gvCCABC_buildGridData());
        }

        protected void lnkCloseCCPrint_Click(object sender, EventArgs e)
        {
            RegisterStartupScript("jsCloseDialg", "closePrintCCListDialog();");    //Close dialog on client side

        }

        #endregion

        #region -------------------------Grid View CC GVList -----------------------------


        protected DataSet gvCCMatList_buildGridData()
        {
            string sql = ViewState["gvCCMatListSQL"].ToString();
            // sql += ",@SortString= " + DB.SQuote(ViewState["gvCCABCSort"].ToString()) + ",@SortOrder=" + DB.SQuote(ViewState["gvCCABCSortOrder"].ToString());

            DataSet ds = DB.GetDS(sql, false);
            //ltSITRecordCount.Text = "[" + ds.Tables[0].Rows.Count.ToString() + "]";
            //lblGroupLabelStatus.Text = "Total Labels Required : " + DB.RowFieldInt(ds.Tables[1].Rows[0], "TotalLables");
            return ds;

        }

        protected void gvCCMatList_buildGridData(DataSet ds)
        {
            gvCCMatList.DataSource = ds.Tables[0];
            gvCCMatList.DataBind();
            ds.Dispose();

        }


        protected void gvCCMatList_Sorting(object sender, GridViewSortEventArgs e)
        {
            ViewState["gvCCMatListInsert"] = false;
            gvCCMatList.EditIndex = -1;
            ViewState["gvCCMatListSort"] = e.SortExpression.ToString();
            ViewState["gvCCMatListSortOrder"] = (ViewState["gvCCMatListSortOrder"].ToString() == "ASC" ? "DESC" : "ASC");
            this.gvCCMatList_buildGridData(this.gvCCMatList_buildGridData());
        }


        protected void gvCCMatList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            ArrayList arr;
            if (ViewState["Data"] != null)
            {
                string MMID;
                arr = (ArrayList)ViewState["Data"];
                foreach (GridViewRow gv in gvCCMatList.Rows)
                {
                    CheckBox isSelected = (CheckBox)gv.FindControl("chkSelforCC");
                    MMID = ((Literal)gv.FindControl("lthidMMID")).Text.ToString();
                    if (arr.Contains(MMID))
                    {
                        isSelected.Checked = true;
                    }
                }

            }
        }

        protected void gvCCMatList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            ArrayList arr;
            if (ViewState["Data"] != null)
            {
                arr = (ArrayList)ViewState["Data"];
                string MMID;

                foreach (GridViewRow gv in gvCCMatList.Rows)
                {
                    CheckBox isSelected = (CheckBox)gv.FindControl("chkSelforCC");
                    MMID = ((Literal)gv.FindControl("lthidMMID")).Text.ToString();
                    if (isSelected.Checked)
                    {

                        if (!arr.Contains(MMID))
                        {
                            arr.Add(MMID);
                        }
                    }
                    else
                    {
                        if (arr.Contains(MMID))
                        {
                            arr.Remove(MMID);
                        }
                    }
                }
                ViewState["Data"] = arr;
            }
            else
            {
                arr = new ArrayList();
                string MMID;

                foreach (GridViewRow gv in gvCCMatList.Rows)
                {
                    CheckBox isSelected = (CheckBox)gv.FindControl("chkSelforCC");
                    if (isSelected.Checked)
                    {
                        MMID = ((Literal)gv.FindControl("lthidMMID")).Text.ToString();
                        arr.Add(MMID);
                        ViewState["Data"] = arr;

                    }
                }
            }
            ViewState["gvCCMatListInsert"] = false;
            this.resetBlkPOQtyError("", false);
            gvCCMatList.PageIndex = e.NewPageIndex;
            gvCCMatList.EditIndex = -1;
            this.gvCCMatList_buildGridData(this.gvCCMatList_buildGridData());
        }

        #endregion

        #region -------------------------Grid View Cycle Count ABC - Item List -----------------------------


        protected DataSet gvCCABC_buildGridData()
        {
            string sql = ViewState["gvCCABCSQL"].ToString();
            // sql += ",@SortString= " + DB.SQuote(ViewState["gvCCABCSort"].ToString()) + ",@SortOrder=" + DB.SQuote(ViewState["gvCCABCSortOrder"].ToString());

            DataSet ds = DB.GetDS(sql, false);
            //ltSITRecordCount.Text = "[" + ds.Tables[0].Rows.Count.ToString() + "]";
            //lblGroupLabelStatus.Text = "Total Labels Required : " + DB.RowFieldInt(ds.Tables[1].Rows[0], "TotalLables");
            return ds;

        }

        protected void gvCCABC_buildGridData(DataSet ds)
        {
            gvCCABC.DataSource = ds.Tables[0];
            gvCCABC.DataBind();
            ds.Dispose();



            if (ds.Tables[0].Rows.Count == 0)
            {

                divCCRecordCount.InnerHtml = "";
                resetError("No items exists for the selected warehouse and cycle class category.", true);
                //resetBlkPOQtyError("No items exists for the selected warehouse and cycle class category.", false);
            }
            else
            {
                divCCRecordCount.InnerHtml = "";
                divCCRecordCount.InnerHtml+= " <font color='#00A382'>";
                divCCRecordCount.InnerHtml+= ddlWarehouse.SelectedItem.Text + " / Cycle Class -" + ddlCycleClass.SelectedValue + "  [" + ds.Tables[0].Rows.Count + " Items]";
                divCCRecordCount.InnerHtml+="</font>";
                resetBlkPOQtyError("", false);
            }
        }


        protected void gvCCABC_Sorting(object sender, GridViewSortEventArgs e)
        {
            ViewState["ShimpentPendingIsInsert"] = false;
            gvCCABC.EditIndex = -1;
            ViewState["gvCCABCSort"] = e.SortExpression.ToString();
            ViewState["gvCCABCSortOrder"] = (ViewState["gvCCABCSortOrder"].ToString() == "ASC" ? "DESC" : "ASC");
            this.gvCCABC_buildGridData(this.gvCCABC_buildGridData());
        }



        #region -------------------------CycleCount Monthwise Report - Item List (Swamy)-----------------------------

        protected void gvCCABC_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            Literal ltJan = (Literal)e.Row.FindControl("ltJan");
            Literal ltFeb = (Literal)e.Row.FindControl("ltFeb");
            Literal ltMar = (Literal)e.Row.FindControl("ltMar");
            Literal ltApr = (Literal)e.Row.FindControl("ltApr");
            Literal ltMay = (Literal)e.Row.FindControl("ltMay");
            Literal ltJun = (Literal)e.Row.FindControl("ltJun");
            Literal ltJul = (Literal)e.Row.FindControl("ltJul");
            Literal ltAug = (Literal)e.Row.FindControl("ltAug");
            Literal ltSep = (Literal)e.Row.FindControl("ltSep");
            Literal ltOct = (Literal)e.Row.FindControl("ltOct");
            Literal ltNov = (Literal)e.Row.FindControl("ltNov");
            Literal ltDec = (Literal)e.Row.FindControl("ltDec");

            string ccid;
            string monthdata;
            string variation;
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string monthsdata = ((Literal)e.Row.FindControl("ltmothnsdata")).Text;
                if (monthsdata != "")
                {
                    string[] data = monthsdata.Split(',');
                    foreach (string tempdata in data)
                    {
                        string[] ccmonth = tempdata.Split('|');
                        ccid = ccmonth[0];
                        monthdata = ccmonth[1];
                        variation = ccmonth[2];

                        switch (monthdata.Substring(0, 3))
                        {
                            case "Jan":
                                ltJan.Text += "<a href='javascript:PrintCCTallyReport(" + ccid + ")'> CCID:" + ccid + " on " + monthdata + "[" + variation + "]</a><br/>";
                                break;
                            case "Feb":
                                ltFeb.Text += "<a href='javascript:PrintCCTallyReport(" + ccid + ")'> CCID:" + ccid + " on " + monthdata + "[" + variation + "]</a><br/>";
                                break;
                            case "Mar":
                                ltMar.Text += "<a href='javascript:PrintCCTallyReport(" + ccid + ")'> CCID:" + ccid + " on " + monthdata + "[" + variation + "]</a><br/>";
                                break;
                            case "Apr":
                                ltApr.Text += "<a href='javascript:PrintCCTallyReport(" + ccid + ")'> CCID:" + ccid + " on " + monthdata + "[" + variation + "]</a><br/>";
                                break;
                            case "May":
                                ltMay.Text += "<a href='javascript:PrintCCTallyReport(" + ccid + ")'> CCID:" + ccid + " on " + monthdata + "[" + variation + "]</a><br/>";
                                break;
                            case "Jun":
                                ltJun.Text += "<a href='javascript:PrintCCTallyReport(" + ccid + ")'> CCID:" + ccid + " on " + monthdata + "[" + variation + "]</a><br/>";
                                break;
                            case "Jul":
                                ltJul.Text += "<a href='javascript:PrintCCTallyReport(" + ccid + ")'> CCID:" + ccid + " on " + monthdata + "[" + variation + "]</a><br/>";
                                break;
                            case "Aug":
                                ltAug.Text += "<a href='javascript:PrintCCTallyReport(" + ccid + ")'> CCID:" + ccid + " on " + monthdata + "[" + variation + "]</a><br/>";
                                break;
                            case "Sep":
                                ltSep.Text += "<a href='javascript:PrintCCTallyReport(" + ccid + ")'> CCID:" + ccid + " on " + monthdata + "[" + variation + "]</a><br/>";
                                break;
                            case "Oct":
                                ltOct.Text += "<a href='javascript:PrintCCTallyReport(" + ccid + ")'> CCID:" + ccid + " on " + monthdata + "[" + variation + "]</a><br/>";
                                break;
                            case "Nov":
                                ltNov.Text += "<a href='javascript:PrintCCTallyReport(" + ccid + ")'> CCID:" + ccid + " on " + monthdata + "[" + variation + "]</a><br/>";
                                //'javascript:pageMethodConcept.callServerSideMethod();'
                                //'javascript:PrintCCTallyReport()'*/
                                /* LinkButton lnkbutoct = new LinkButton();
                                lnkbutoct.PostBackUrl = "CC_CycleCountTallyReport.aspx?ccid=" + ccid;
                                lnkbutoct.Text = "CC[" + ccid + "] on " + monthdata + "<br/>";
                                ltNov.Controls.Add(lnkbutoct);*/
                                break;
                            case "Dec":
                                /*ddlDec.Visible = true;
                               ddlDec.Items.Add(new ListItem("cc["+ccid+"] on"+monthdata,ccid));*/
                                ltDec.Text += "<a href='javascript:PrintCCTallyReport(" + ccid + ")'> CCID:" + ccid + " on " + monthdata + "[" + variation + "]</a><br/>";
                                break;
                            default:
                                break;

                        }
                    }
                }

            }
        }
        protected void lnkgo_Click(object sender, EventArgs e)
        {
            string ccid = txtCyclecountID.Text;
            
            FillCycleCountDetaisl(ccid);
            RegisterStartupScript("jsUnblockDialog", "PrintFinalReport();");
          
        }
        //for filling CyccleCount Details in lblCCdetails Label
        public void FillCycleCountDetaisl(String vCycleCountID)
        {

            IDataReader rsCC = DB.GetRS("[sp_CC_GetCycleCountDetails] @CycleCountID=" + vCycleCountID);


            if (rsCC.Read())
            {
                lblCCDetails.Text = "<b>Cycle Count #:</b>" + vCycleCountID + "&nbsp;&nbsp; <b>Warehouse:</b>" + DB.RSField(rsCC, "WHCode") + "&nbsp;&nbsp; <b>Cycle Class:</b>" + DB.RSField(rsCC, "CycleClass") + "&nbsp;&nbsp; <b>Handler:</b>" + DB.RSField(rsCC, "Handler");
                lblCCDetails.Text += "<br/><b>Initiated On:</b>" + DB.RSFieldDateTime(rsCC, "CCDateInitiated").ToString("dd/MM/yy") + "&nbsp;&nbsp; <b>Initiated By:</b>" + DB.RSField(rsCC, "CreatedBy");
                FillTallyReport(vCycleCountID);
            }
            else
            {

                FillTallyReport(vCycleCountID);
                lblCCDetails.Text = "Invalid CycleCOunt";
                lblmsg.Text = "";

            }
            rsCC.Close();
        }
        private void AddGridHeaderListforTallyReport(String CCID, GridView gv)
        {
            IDataReader dsDynamicMSp = DB.GetRS("dbo.sp_CC_GetCCMaterialStorageParameters @CycleCountID=" + CCID + ",@TenantID=" + customprinciple.TenantID);

            StringBuilder HeaderText = new StringBuilder();
            HeaderText.Append("<table border=\"1\" align=\"center\" cellpadding='1'  style=\"border-collapse:collapse;border: 1px solid black;\"><tr><td align=\"left\" width='100'>Location</td>");
            while (dsDynamicMSp.Read())
            {
                HeaderText.Append("<td align=\"left\" width=100'>" + DB.RSField(dsDynamicMSp, "ParameterName") + "</td>");
            }
            HeaderText.Append("<td width='100' align=\"left\" >CountQty</td><td align=\"left\" width='100' >AvlQty</td></tr></table>");
            gv.Columns[2].HeaderText = HeaderText.ToString();
        }
        public void FillTallyReport(String vCycleCountID)
        {
            string queryforcyclecountreport = "exec [dbo].[sp_CC_GetCycleCountTallyReport]@CycleCountID=" + vCycleCountID;
            try
            {
                AddGridHeaderListforTallyReport(vCycleCountID, gvCCreport);

                DataSet ds = DB.GetDS(queryforcyclecountreport, false);
                if (ds.Tables[0].Rows.Count > 0)
                {

                    gvCCreport.DataSource = ds;
                    gvCCreport.DataBind();
                    ds.Dispose();
                    lblmsg.Text = "";

                }
                else
                {

                    gvCCreport.DataSource = ds.Tables[0];
                    gvCCreport.DataBind();
                    ds.Dispose();
                    lblmsg.Text = "No Items Are Recorded In This CycleCount";
                }
            }
            catch (Exception exe)
            {

            }
        }
        protected void gvCCreport_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                String vltSplitLocation = ((Literal)e.Row.Cells[2].FindControl("ltSplitLocationforReport")).Text;

                Literal ltLocation = (Literal)e.Row.Cells[2].FindControl("ltLocationforReport");
                ltLocation.Text = DisplayInTable(vltSplitLocation);
            }
        }
        public String DisplayInTable(String prmData)
        {
            if (prmData != "")
            {
                StringBuilder sbResult = new StringBuilder();


                char[] Locationseps = new char[] { ',' };
                String[] Locations = prmData.Split(Locationseps);

                char[] StockTypeseps = new char[] { '|' };
                String[] StockValues;
                if (Locations.Length != 0)
                {
                    sbResult.Append("<table border='1' width='100%' class='tablealignment'>");

                    foreach (String Loc in Locations)
                    {
                        StockValues = Loc.Split(StockTypeseps);
                        if (StockValues[2].ToString() == "-555.00")
                        {
                            sbResult.Append("<tr><td colspan='4'> MM Item is not configured to handle multiple UoM's for this Delv. Doc. Pls contact MMAdmin . </td></tr>");
                        }
                        else
                        {
                            sbResult.Append("<tr>");
                          
                            for (var i = 0; i < StockValues.Length; i++)
                            {
                                sbResult.Append("<td align='left' width='100'>");
                                sbResult.Append(StockValues[i]);
                                sbResult.Append("</td>");
                            }
                            sbResult.Append("</tr>");
                        }
                    }

                    sbResult.Append("</table>");
                }

                return sbResult.ToString();
            }
            else
            {
                return "No Item in DB ";
            }
        }
        #endregion
        protected void gvCCABC_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            ViewState["ShipmentPendingIsInsert"] = false;
            this.resetBlkPOQtyError("", false);
            gvCCABC.PageIndex = e.NewPageIndex;
            gvCCABC.EditIndex = -1;
            this.gvCCABC_buildGridData(this.gvCCABC_buildGridData());
        }

        #endregion



        #region -------------------------Update Line Items -----------------------------


        protected void resetLineItemError(string error, bool isError)
        {

            string str = "<font class=\"noticeMsg\">NOTICE:</font>&nbsp;&nbsp;&nbsp;";
            if (isError)
                str = "<font class=\"errorMsg\">ERROR:</font>&nbsp;&nbsp;&nbsp;";

            if (error.Length > 0)
                str += error + "";
            else
                str = "";


            ltCCDetails.Text = str;


        }


        protected DataSet PrintCCLineItemsList_buildGridData()
        {
            string sql = ViewState["PrintCCLineItemsListSQL"].ToString();
            // sql += ",@SortString= " + DB.SQuote(ViewState["PrintCCLineItemsListSort"].ToString()) + ",@SortOrder=" + DB.SQuote(ViewState["PrintCCLineItemsListSortOrder"].ToString());

            DataSet ds = DB.GetDS(sql, false);
            //ltSITRecordCount.Text = "[" + ds.Tables[0].Rows.Count.ToString() + "]";
            //lblGroupLabelStatus.Text = "Total Labels Required : " + DB.RowFieldInt(ds.Tables[1].Rows[0], "TotalLables");
            ltCCDetails.Text += "<b> Total Items : </b>" + ds.Tables[0].Rows.Count.ToString();
            return ds;

        }

        protected void PrintCCLineItemsList_buildGridData(DataSet ds)
        {
            gvCCPrintCCList.DataSource = ds.Tables[0];
            gvCCPrintCCList.DataBind();
            ds.Dispose();
        }


        protected void gvCCPrintCCList_Sorting(object sender, GridViewSortEventArgs e)
        {
            ViewState["ShimpentPendingIsInsert"] = false;
            gvCCPrintCCList.EditIndex = -1;
            //ViewState["PrintCCLineItemsListSort"] = e.SortExpression.ToString();
            //ViewState["PrintCCLineItemsListSortOrder"] = (ViewState["PrintCCLineItemsListSortOrder"].ToString() == "ASC" ? "DESC" : "ASC");
            this.PrintCCLineItemsList_buildGridData(this.PrintCCLineItemsList_buildGridData());
        }

        protected void gvCCPrintCCList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                String vltSplitLocation = ((Literal)e.Row.Cells[2].FindControl("ltSplitLocation")).Text;
                Literal ltLocation = (Literal)e.Row.Cells[2].FindControl("ltLocation");
                string vMMID = ((Literal)e.Row.Cells[1].FindControl("ltMMID")).Text;

                ltLocation.Text = DisplayInTable(e.Row, vltSplitLocation, vMMID);

            }

        }

        protected void gvCCPrintCCList_OnRowCommand(object sender, GridViewCommandEventArgs e)
        {

            if (e.CommandName == "UpdateLineItemQty")
            {
                //Set the rowindex
                int rowIndex = Convert.ToInt32(e.CommandArgument);

                GridViewRow row = gvCCPrintCCList.Rows[rowIndex];

                String valMCode = ((Literal)row.FindControl("ltMCode")).Text;
                TextBox valQty = (TextBox)row.FindControl("txtPickedQty");


            }

        }
        protected void gvCCPrintCCList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

            ViewState["ShipmentPendingIsInsert"] = false;
            this.resetLineItemError("", false);
            gvCCPrintCCList.PageIndex = e.NewPageIndex;
            gvCCPrintCCList.EditIndex = -1;
            this.PrintCCLineItemsList_buildGridData(this.PrintCCLineItemsList_buildGridData());

        }


        protected void gvCCPrintCCList_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
        }

        protected void gvCCPrintCCList_RowEditing(object sender, GridViewEditEventArgs e)
        {
            ViewState["POLineQtyIsInsert"] = false;
            gvCCPrintCCList.EditIndex = e.NewEditIndex;
            this.PrintCCLineItemsList_buildGridData(this.PrintCCLineItemsList_buildGridData());
            gvCCPrintCCList.Rows[gvCCPrintCCList.EditIndex].Cells[0].FindControl("txtLineNo").Focus();

        }

        protected void gvCCPrintCCList_RowCancelEditing(object sender, GridViewCancelEditEventArgs e)
        {
        }

 
        #endregion



        public String DisplayInTable(GridViewRow gvRow, String prmData, String MaterialMasterID)
        {
            if (prmData != "")
            {
                StringBuilder sbResult = new StringBuilder();


                char[] Locationseps = new char[] { ',' };
                String[] Locations = prmData.Split(Locationseps);

                char[] StockTypeseps = new char[] { '|' };
                String[] StockValues;
                if (Locations.Length != 0)
                {
                    sbResult.Append("<table width='100%' border='1' class='tablealignment'>");
                    foreach (String Loc in Locations)
                    {
                        StockValues = Loc.Split(StockTypeseps);
                        if (StockValues[2].ToString() == "-555.00")
                        {
                            sbResult.Append("<tr><td colspan='4'> MM Item is not configured to handle multiple UoM's for this Delv. Doc. Pls contact MMAdmin . </td></tr>");
                        }
                        else
                        {
                            sbResult.Append("<tr>");
                            for (var i = 0; i < StockValues.Length - 4; i++)
                            {
                                sbResult.Append("<td align='left' width='100'>");
                                sbResult.Append(StockValues[i]);
                                sbResult.Append("</td>");
                            }
                            sbResult.Append("<td align='left' width='50'>" + StockValues[(StockValues.Length - 4)] + "</td>");
                            sbResult.Append("<td align='left' width='50' ><div class=\"NoPrint\">" + StockValues[(StockValues.Length - 4) + 3] + "</div></td>");
                            sbResult.Append("</tr>");

                           
                        }
                    }

                    sbResult.Append("</table>");
                }

                return sbResult.ToString();
            }
            else
            {
                return "No Item in DB ";
            }
        }

        protected void lnkCCItemSearch_Click1(object sender, EventArgs e)
        {
            if (txtMCode.Text.Trim() == "Part Number ..")
                txtMCode.Text = "";

            CC_ABC_ListSQL = "[sp_CC_GetCycleDataBy_Class_WH] @WarehouseID =" + ddlWarehouse.SelectedValue + ", @CycleClassID=" + DB.SQuote(ddlCycleClass.SelectedValue) + ", @StartDate='01/01/2012',@EndDate=" + DB.DateQuote(DateTime.Now.ToString("MM/dd/yyyy")) + ",@MaterialCode=" + DB.SQuote(txtMCode.Text.Trim()) + ",@CycleCountID=0";
            ViewState["gvCCABCSQL"] = CC_ABC_ListSQL;
            this.gvCCABC_buildGridData(this.gvCCABC_buildGridData());
        }
    }
}
