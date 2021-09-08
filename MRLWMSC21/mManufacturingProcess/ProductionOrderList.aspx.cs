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

namespace MRLWMSC21.mManufacturingProcess
{
    //Author    :   Gvd Prasad
    //Created on:   23-Feb-2015

    public partial class ProductionOrderList : System.Web.UI.Page
    {
        DataTable dtInternalOrderList = new DataTable();

        public CustomPrincipal cp = HttpContext.Current.User as CustomPrincipal;

        protected void Page_PreInit(object sender, EventArgs e)
        {
            Page.Theme = "Inbound";

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!cp.IsInAnyRoles(CommonLogic.GetRolesAllowed(CommonLogic.GetRolesAllowedForthisPage("Job Order List"))))
            {
                Response.Redirect("../Login.aspx?eid=6");
            }

            if (!IsPostBack)
            {

                DesignLogic.SetInnerPageSubHeading(this.Page, "Job Order List");
                BuildStatus();
                BuildRoutingType();
                ddlStatus.SelectedValue = "2";
                ViewState["PROHeaderList"] = "EXEC [dbo].[sp_MFG_GetProductionOrderHeaderList] @PRORefNo=''" + ",@ProductionOrderStatus="+ddlStatus.SelectedValue;
                Build_PRORefNumberList(Build_PRORefNumberList());
            }
        }

        protected void gvPROList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvPROList.PageIndex = e.NewPageIndex;
            Build_PRORefNumberList(Build_PRORefNumberList());
        }

        protected void lnkGetData_Click(object sender, EventArgs e)
        {
            String PRORefNo = txtPRORefNoSearch.Text.Trim();
            String KitCode = txtKitCode.Text.Trim();
            if (PRORefNo == "Job Order Ref. #...")
                PRORefNo = "";
            if (KitCode == "Kit Code...")
            {
                KitCode = "";
            }
            ViewState["PROHeaderList"] = "EXEC [dbo].[sp_MFG_GetProductionOrderHeaderList] @PRORefNo='" + PRORefNo + "',@ProductionOrderStatus=" + ddlStatus.SelectedValue + ",@KitCode=" + (KitCode != "" ? DB.SQuote(KitCode) : "NULL") + ",@RoutingDocumentType="+ddlRoutingType.SelectedValue;
            Build_PRORefNumberList(Build_PRORefNumberList());
        }


        private DataSet Build_PRORefNumberList()
        {
            dtInternalOrderList.Columns.Add("InternalOrderHeaderID", typeof(String));
            dtInternalOrderList.Columns.Add("IORefNo", typeof(String));
            dtInternalOrderList.Columns.Add("CreatedOn",typeof(String));
            dtInternalOrderList.Columns.Add("Status",typeof(String));
            dtInternalOrderList.Columns.Add("CreatedBy",typeof(String));
            dtInternalOrderList.Columns.Add("Reason",typeof(string));
            dtInternalOrderList.Columns.Add("ProductionOrderHeaderID", typeof(string));
            DataSet cmdPROHeaderList = null;
            String cMdRoutingHeaderList = ViewState["PROHeaderList"].ToString();
            try
            {
                cmdPROHeaderList = DB.GetDS(cMdRoutingHeaderList, false);
            }
            catch (Exception ex)
            {
                resetError("Error while loading",true);
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
            }
            return cmdPROHeaderList;
        }

        private void Build_PRORefNumberList(DataSet cmdPROHeaderList)
        {
            gvPROList.DataSource = cmdPROHeaderList;
            gvPROList.DataBind();
            
            cmdPROHeaderList.Dispose();
            if (gvPROList.Rows.Count == 0)
            {
                resetError("No job order is available", true);
            }
        }

        protected void gvPROList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.DataItem == null)
            {
                return;
            }
           
            LinkButton lnkChildItems = (LinkButton)e.Row.FindControl("lnkViewMDeficiency");
            lnkChildItems.OnClientClick = "ProdOpenPrintDialog();";

            DataRow row = ((DataRowView)e.Row.DataItem).Row;
            String IsDeficiency = row["IsDeficiency"].ToString();

            if (IsDeficiency == "1")
                    e.Row.CssClass = "DeficiencyRow";

            if (e.Row.RowType == DataControlRowType.DataRow && !(e.Row.RowState == DataControlRowState.Edit))
            {
                GridView gvInternalOrderList = (GridView)e.Row.FindControl("gvInternalOrderList");
                String[] InternalOrderList = row["InternalOrdersList"].ToString().Split(',');
                DataRow drNewRow;
                dtInternalOrderList.Rows.Clear();
                if (InternalOrderList[0] != "")
                {
                    foreach (String InternalOrder in InternalOrderList)
                    {
                        String[] InternalOrderData = InternalOrder.Split('|');
                        drNewRow = dtInternalOrderList.NewRow();
                        drNewRow["InternalOrderHeaderID"] = InternalOrderData[0];
                        drNewRow["IORefNo"] = InternalOrderData[1];
                        drNewRow["CreatedOn"] = InternalOrderData[3];
                        drNewRow["Status"] = InternalOrderData[2];
                        drNewRow["CreatedBy"] = InternalOrderData[4];
                        drNewRow["Reason"] = InternalOrderData[5];
                        dtInternalOrderList.Rows.Add(drNewRow);

                    }
                }
                drNewRow = dtInternalOrderList.NewRow();
                drNewRow["InternalOrderHeaderID"] = 0;
                drNewRow["IORefNo"] = "New NC Order";
                drNewRow["CreatedOn"] = "";
                drNewRow["Status"] = "";
                drNewRow["ProductionOrderHeaderID"] = row["ProductionOrderHeaderID"].ToString();
                //dtInternalOrderList.Rows.Add(drNewRow);
                
                gvInternalOrderList.DataSource = dtInternalOrderList;
                gvInternalOrderList.DataBind();
                
            }
            /*if (e.Row.RowType == DataControlRowType.DataRow && !(e.Row.RowState == DataControlRowState.Edit))
            //if (e.Row.RowState == DataControlRowState.Normal)
            {
                TreeView trvInternalorder = (TreeView)e.Row.FindControl("trvInternalOrder");
                TreeNode trnInternalOrder;
                String[] InternalOrderList = row["InternalOrdersList"].ToString().Split(',');
                trvInternalorder.Nodes[0].Text = row["Prod Order Ref#"].ToString();
                if (InternalOrderList[0] != "")
                {
                    foreach (String InternalOrder in InternalOrderList)
                    {
                        trnInternalOrder = new TreeNode();
                        trnInternalOrder.Text=InternalOrder.Split('|')[1];
                        trnInternalOrder.NavigateUrl = "InternalOrder.aspx?ioid=" + InternalOrder.Split('|')[0];
                        trnInternalOrder.Value=InternalOrder.Split('|')[0];
                        trvInternalorder.Nodes[0].ChildNodes.Add(trnInternalOrder);
                    }
                    
                }
                trnInternalOrder = new TreeNode();
                trnInternalOrder.Text = "New Internal Order";
                trnInternalOrder.NavigateUrl = "InternalOrder.aspx?new=" + row["ProductionOrderHeaderID"].ToString();
                trnInternalOrder.Value =row["ProductionOrderHeaderID"].ToString();
                trvInternalorder.Nodes[0].ChildNodes.Add(trnInternalOrder);
               
            }*/

          
        }

        //protected void gvPROList_RowCommand(object sender, GridViewCommandEventArgs e)
        //{

        //    if (e.CommandName == "EditChildItems")
        //    {

        //        int ProHeadID = Localization.ParseNativeInt(e.CommandArgument.ToString());

        //        ViewState["ProHeadID"] = ProHeadID.ToString();


        //        ViewState["ProdDefcListSQL"] = "EXEC [dbo].[sp_MFG_ProductionOrderMaterialsDeficiency]  @ProductionOrderHeaderID=" + ProHeadID;
               
        //        this.gvProdDefcList_buildGridData(this.gvProdDefcList_buildGridData());


        //    }
        //}


        #region ----  Production Order Materials Deficiency List Dialog  --------


        //protected DataSet gvProdDefcList_buildGridData()
        //{
        //    string cmdGetDeficiency = ViewState["ProdDefcListSQL"].ToString();
        //    DataSet dsGetDeficiency=null;
        //    try
        //    {
        //        dsGetDeficiency = DB.GetDS(cmdGetDeficiency, false);
        //    }
        //    catch (Exception ex)
        //    {
        //        resetError("Error while loading",true);
        //        CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
        //    }

        //    return dsGetDeficiency;
        //}

        //protected void gvProdDefcList_buildGridData(DataSet ds)
        //{
        //    gvProdDefcList.DataSource = ds.Tables[0];
        //    gvProdDefcList.DataBind();
        //    ds.Dispose();
        //}

        //protected void gvProdDefcList_RowDataBound(object sender, GridViewRowEventArgs e)
        //{
        //    if (e.Row.DataItem == null)
        //        return;

        //    Literal ltDeficiency = (Literal)e.Row.FindControl("ltDeficiency");

        //    if (ltDeficiency.Text != "0.00")
        //        e.Row.CssClass = "DeficiencyRow";


        //}

        //protected void gvProdDefcList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        //{


        //    gvProdDefcList.PageIndex = e.NewPageIndex;
        //    gvProdDefcList.EditIndex = -1;
        //    this.gvProdDefcList_buildGridData(this.gvProdDefcList_buildGridData());

        //}

        protected void lnkMaterialDeficiency_Click(object sender, EventArgs e)
        {

        }


        #endregion ----  Production Order Materials Deficiency List Dialog  --------

        protected void imgbtngvPROList_Click(object sender, ImageClickEventArgs e)
        {
            String[] hiddencolumns = { "Status", "","","" };
            gvPROList.AllowPaging = false;
            Build_PRORefNumberList(Build_PRORefNumberList());
            gvPROList.Columns[11].Visible = true;
            CommonLogic.ExporttoExcel(gvPROList, "JOB ORDER LIST", hiddencolumns);
        }

        public override void VerifyRenderingInServerForm(Control control)
        {
            /* Verifies that the control is rendered */
        }

        protected void gvInternalOrderList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.DataItem == null && e.Row.Cells[0].Text == "&nbsp;")
            {
                e.Row.Visible = false;
                return;
            }

            if (e.Row.RowType == DataControlRowType.DataRow && !(e.Row.RowState == DataControlRowState.Edit))
            {
                DataRow row = ((DataRowView)e.Row.DataItem).Row;
                if (row["InternalOrderHeaderID"].ToString() == "0")
                {
                    HyperLink hylInternalOrder = (HyperLink)e.Row.FindControl("hylInternalOrder");
                    hylInternalOrder.NavigateUrl = "InternalOrder.aspx?new=" + row["ProductionOrderHeaderID"];
                    
                }
            }
        }

        private void resetError(string error, bool isError)
        {

            /*string str = "<font class=\"noticeMsg\">NOTICE:</font>&nbsp;&nbsp;&nbsp;";
            if (isError)
                str = "<font class=\"errorMsg\">ERROR:</font>&nbsp;&nbsp;&nbsp;";

            if (error.Length > 0)
                str += error + "";
            else
                str = "";


            ltPickStatus.Text = str;*/
            ScriptManager.RegisterStartupScript(this, this.GetType(), "test", "showStickyToast(" + (isError.ToString().ToLower() == "true" ? "false" : "true") + ",\"" + error + "\");", true);

        }

        protected String GetStatusImage(String StatusID)
        {
            String ImageUrl = "";
            if (StatusID == "1")
            {
                ImageUrl = "~/Images/blue_menu_icons/Grey-16.png";
            }
            else if (StatusID == "2")
            {
                ImageUrl = "~/Images/blue_menu_icons/Orange-16.png";
            }
            else if (StatusID == "3")
            {
                ImageUrl = "~/Images/blue_menu_icons/Green-16.png";
            }
            else if (StatusID == "4")
            {
                ImageUrl = "~/Images/blue_menu_icons/OnHold-16.png";
            }
            else if (StatusID == "5")
            {
                ImageUrl = "~/Images/blue_menu_icons/Yellow-16.png";
            }

            return ImageUrl;
        }

        private void BuildStatus()
        {
            try
            {
                ddlStatus.Items.Add(new ListItem("All", "0"));
                String cmdStatus = "select ProductionOrderStatus,ProductionOrderStatusID from MFG_ProductionOrderStatus where IsDeleted=0";
                IDataReader rsStatusList = DB.GetRS(cmdStatus);
                while (rsStatusList.Read())
                {
                    ddlStatus.Items.Add(new ListItem(DB.RSField(rsStatusList, "ProductionOrderStatus"), DB.RSFieldInt(rsStatusList, "ProductionOrderStatusID").ToString()));
                }
                rsStatusList.Close();
            }
            catch (Exception ex)
            {
                resetError("Error while loading",true);
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
            }
        }

        private void BuildRoutingType()
        {
            try
            {
                ddlRoutingType.Items.Add(new ListItem("All", "0"));
                String cmdRoutingType = "select RoutingDocumentTypeID,RoutingDocumentType from MFG_RoutingDocumentType where IsDeleted=0 and IsActive=1";
                IDataReader rsRoutingTypeList = DB.GetRS(cmdRoutingType);
                while (rsRoutingTypeList.Read())
                {
                    ddlRoutingType.Items.Add(new ListItem(DB.RSField(rsRoutingTypeList, "RoutingDocumentType"), DB.RSFieldInt(rsRoutingTypeList, "RoutingDocumentTypeID").ToString()));
                }
                rsRoutingTypeList.Close();
            }
            catch (Exception ex)
            {
                resetError("Error while loading",true);
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
            }
        }
    }
}