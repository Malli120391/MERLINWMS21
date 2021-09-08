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
using System.IO;

// Module Name : RevertInbound Under Inbound
// Usecase Ref.: Revert Inbound_UC_013
// DevelopedBy : Naresh P
// Created On  : 13/11/2013
// Modified On : 24/03/2015


namespace MRLWMSC21.mInbound
{
    public partial class RevertInbound : System.Web.UI.Page
    {

        public CustomPrincipal cp = HttpContext.Current.User as CustomPrincipal;
        protected string SQLSearch = "EXEC SP_INB_RevertInbound  ";
        string TenantRootDir = "";
        string InboundPath = "";
        string InbShpDelNotePath = "";
        string InbShpVerifNotePath = "";
        string InbDescPath = "";
        
        // Set page theme
        protected void PagePre_Init(object sender, EventArgs e)
        {

            Page.Theme = "Inbound";

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            cp = HttpContext.Current.User as CustomPrincipal;
            // System Admin, Super Admin, IB Initiator, IB Store Incharge, Store Manager, Operator
            if (!cp.IsInAnyRoles(CommonLogic.GetRolesAllowed(CommonLogic.GetRolesAllowedForthisPage("Revert Inbound"))))
            {
                Response.Redirect("../Login.aspx?eid=6");
            }

            // Set Page sub heading
            DesignLogic.SetInnerPageSubHeading(this.Page, "Revert Inbound");

            string query = "EXEC [dbo].[sp_TPL_GetTenantDirectoryInfo] @TypeID=1";

            DataSet dsPath = DB.GetDS(query.ToString(), false);

            TenantRootDir = dsPath.Tables[0].Rows[4][0].ToString();
            InboundPath = dsPath.Tables[0].Rows[0][0].ToString();
            InbShpDelNotePath = dsPath.Tables[0].Rows[2][0].ToString();
            InbShpVerifNotePath = dsPath.Tables[0].Rows[3][0].ToString();
            InbDescPath = dsPath.Tables[0].Rows[1][0].ToString();

            if (!IsPostBack)
            {

                ViewState["ShipmentResultsSQLString"] = this.SQLSearch + " @StoreRefNo=NULL ,@TenantID="+hifTenant.Value + ",@AccountID_New=" + cp.AccountID+ ", @UserID_New = " + cp.UserID;

                this.ShipmentResults_buildGridData(this.ShipmentResults_buildGridData());


            }

        }

        // Search Store Ref. Number....
        protected void lnkSearchtext_Click(object sender, EventArgs e)
        {
            cp = HttpContext.Current.User as CustomPrincipal;
            if (txtsearchText.Text == "Search Store Ref.#...")
                txtsearchText.Text = "";

            if(txtTenant.Text == "Search Tenant..." || txtTenant.Text == "")
            {
                hifTenant.Value = "0";
            }

            if (txtsearchText.Text != "")
            {
                ViewState["ShipmentResultsSQLString"] = this.SQLSearch + " @StoreRefNo=" + DB.SQuote(txtsearchText.Text) + ",@TenantID="+ hifTenant.Value + ",@AccountID_New=" + cp.AccountID+ ",@UserID_New = " + cp.UserID; 
            }
            else {
                ViewState["ShipmentResultsSQLString"] = this.SQLSearch + " @StoreRefNo=NULL ,@TenantID=" + hifTenant.Value + ",@AccountID_New=" + cp.AccountID + ",@UserID_New = " + cp.UserID;
            }

            this.ShipmentResults_buildGridData(this.ShipmentResults_buildGridData());

            txtsearchText.Text = "Search Store Ref.#...";

        }

        public string GetRevertVerificationLink(String ShipmentVerifyDate)
        {
            if (ShipmentVerifyDate != "")
            {
                return "<br/><nobr><img src='../Images/redarrowleft.gif' border='0' /> Revert Receipt Conf. </nobr>";
            }
            else
                return "";

        }

        public string GetGRNUpdateLink(String InboundStatusID)
        {
            int vInbStatusID = Convert.ToInt32(InboundStatusID);
            if (vInbStatusID >= 4)
            {
                return "<nobr><img src='../Images/redarrowleft.gif' border='0' /> Revert GRN </nobr>";
            }
            else
                return "";

        }

        public string GetRevertReceivedLink(String ShipmentRcvdDate)
        {
            if (ShipmentRcvdDate != "")
            {
                return "<br/><nobr><img src='../Images/redarrowleft.gif' border='0' /> Revert Received </nobr>";
            }
            else
                return "";

        }

        public string GetRevertExpectedLink(String ShipmentExpectedDate)
        {
            if (ShipmentExpectedDate != "")
            {
                return "<br/><nobr><img src='../Images/redarrowleft.gif' border='0' /> Revert Expected </nobr>";
            }
            else
                return "";
        }

        public string GetStoreRefNoWithLink(String StoreRefNo, String TenantID)
        {
            // String sFileName = CommonLogic._GetAttatchmentFile(TenantRootDir + DB.GetSqlS("select UniqueID AS S from GEN_Tenant where TenantID=" + cp.TenantID) + InboundPath + InbShpDelNotePath, StoreRefNo);

            String sFileName = CommonLogic._GetAttatchmentFile(TenantRootDir + TenantID + InboundPath + InbShpDelNotePath, StoreRefNo);

            String ResValue = "";
            if (sFileName != "")
            {
                String Path = "../ViewImage.aspx?path=" + sFileName;

                ResValue += "<a style=\"text-decoration:none;\" href=\"#\" onclick=\" OpenImage(' " + Path + " ')  \" > " + StoreRefNo + " </a>";
                // ResValue += "<img src=\"../Images/redarrowright.gif\"   />";
            }
            else
            {
                ResValue = StoreRefNo;
            }


            return ResValue;
        }

        private bool CheckReceiptConfirmation(String InboundID, String RefWHID)
        {

            bool status = false;

            try
            {

                String VerificationStatus = DB.GetSqlS("select Convert(nvarchar(50),ShipmentVerifiedOn) AS S from INB_InboundTracking_Warehouse where IsActive=1 and IsDeleted=0 and IB_RefWarehouse_DetailsID=" + RefWHID);
                String ReceivedStatus = DB.GetSqlS("select Convert(nvarchar(50),ShipmentReceivedOn) AS S from INB_InboundTracking_Warehouse where IsActive=1 and IsDeleted=0 and IB_RefWarehouse_DetailsID=" + RefWHID);

                if (VerificationStatus != "" || ReceivedStatus != "")
                {
                    status = false;
                }
                else
                {
                    status = true;
                }


            }
            catch (Exception ex)
            {
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
                resetError("Error while updating", true);
            }

            return status;
        }

        private bool CheckReceivedStatus(String InboundID, String RefWHID)
        {

            bool status = false;

            try
            {

                String VerificationStatus = DB.GetSqlS("select Convert(nvarchar(50),ShipmentVerifiedOn) AS S from INB_InboundTracking_Warehouse where IsActive=1 and IsDeleted=0 and IB_RefWarehouse_DetailsID=" + RefWHID);


                if (VerificationStatus != "")
                {
                    status = false;
                }
                else
                {
                    status = true;
                }


            }
            catch (Exception ex)
            {
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
                resetError("Error while updating ", true);
            }

            return status;
        }


        #region -------------------------Shipment Results -----------------------------

        protected DataSet ShipmentResults_buildGridData()
        {
            string sql = ViewState["ShipmentResultsSQLString"].ToString();
            
            DataSet ds = DB.GetDS(sql, false);
            //ltSRRecordCount.Text = "[" + ds.Tables[0].Rows.Count.ToString() + "]";
            return ds;

        }

        protected void ShipmentResults_buildGridData(DataSet ds)
        {
            gvShipmentResults.DataSource = ds;
            gvShipmentResults.DataBind();
            ds.Dispose();
        }

        protected void gvShipmentResults_Sorting(object sender, GridViewSortEventArgs e)
        {
            ViewState["ShimpentPendingIsInsert"] = false;
            gvShipmentResults.EditIndex = -1;
           // ViewState["ShipmentResultsSort"] = e.SortExpression.ToString();
            //ViewState["ShipmentResultsSortOrder"] = (ViewState["ShipmentResultsSortOrder"].ToString() == "ASC" ? "DESC" : "ASC");
            this.ShipmentResults_buildGridData(this.ShipmentResults_buildGridData());
        }

        protected void gvShipmentResults_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            ViewState["ShipmentResultsIsInsert"] = false;
            
            gvShipmentResults.PageIndex = e.NewPageIndex;
            gvShipmentResults.EditIndex = -1;
            this.ShipmentResults_buildGridData(this.ShipmentResults_buildGridData());
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

        protected void gvShipmentResults_RowCommand(object sender, GridViewCommandEventArgs e)
        {

            cp = HttpContext.Current.User as CustomPrincipal;
            if (e.CommandName == "Page")
                return;

            StringBuilder sqlRevertTrans = new StringBuilder();

            ViewState["GDRIsInsert"] = false;
            gvShipmentResults.EditIndex = -1;

            if (e.CommandName == "EditChildItems")
            {
                ViewState["GRNListSQL"] = "EXEC sp_INB_GetGRNUpdateDetails @InboundID="+e.CommandArgument.ToString();
                this.gvGRNDetails_buildGridData(this.gvGRNDetails_buildGridData());
                return;
            }

            char[] seps = new char[] { '&' };
            String[] CmdArguments = e.CommandArgument.ToString().Split(seps);

            String vStoreRefNo = CmdArguments[0];
            String vInboundTrackingID = CmdArguments[1];

            InboundTrack IBTrack = new InboundTrack(Convert.ToInt32(vInboundTrackingID));


            char[] seps1 = new char[] { ',' };

            String ReferedWHs = IBTrack.ReferedStoreIDs.Substring(0, IBTrack.ReferedStoreIDs.Length - 1);

            String[] ArrStores = CommonLogic.FilterSpacesInArrElements(ReferedWHs.Split(seps1));


            String vRefStoreID = CmdArguments[2];

            if (vRefStoreID == "")
                vRefStoreID = "0";

            int statusid = IBTrack.InBoundStatusID;
            switch (e.CommandName)
            {


                case "RevertShipmmentReceived":
                    sqlRevertTrans.Clear();
                    if (CheckReceivedStatus(vInboundTrackingID, vRefStoreID) == false)
                    {
                        resetError("First revert Shipment Verification Details", true);
                        return;
                    }

                    sqlRevertTrans.Append("EXEC  [dbo].[sp_INB_RevertShipmmentReceived]   @InboundID="+vInboundTrackingID+ ",@UpdatedBy="+cp.UserID.ToString());


                    // added by lalitha on 26/04/2019
                    DataSet ds = DB.GetDS(" select * from INB_GRNUpdate  where InboundID=" + vInboundTrackingID + " and IsActive=1 and IsDeleted=0 AND ISNULL(IsCancelled,0)=0", false);
                    if (ds.Tables[0].Rows.Count != 0)
                    {
                        resetError("Unable to Revert Shipment Received Until Revert GRN updated ", true);
                        return;
                    }
                    if (statusid == 3)
                    {
                        try
                        {
                            DB.ExecuteSQL(sqlRevertTrans.ToString());
                            resetError("Shipment received details successfully reverted ", false);
                        }
                        catch (Exception ex)
                        {
                            CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
                            resetError("Error while reverting Shipment received details", true);
                        }
                    }

                    break;

                case "RevertShipmmentExpected":
                    sqlRevertTrans.Clear();

                    if (CheckReceiptConfirmation(vInboundTrackingID, vRefStoreID) == false)
                    {
                        resetError("First revert Shipment Received Details", true);
                        return;
                    }

                    sqlRevertTrans.Append("EXEC  [dbo].[sp_INB_RevertShipmmentExpected]  @InboundID="+vInboundTrackingID+ ", @UpdatedBy = "+ cp.UserID +"");
                   
                    try
                    {
                        DB.ExecuteSQL(sqlRevertTrans.ToString());
                        resetError("Shipment expected details successfully reverted ", false);
                    }
                    catch (Exception ex)
                    {
                        CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
                        resetError("Error while reverting Shipment expected details", true);
                    }
                    break;


              case "RevertShipmmentVerification":

                    sqlRevertTrans.Clear();

                    if (DB.GetSqlN("select InboundStatusID AS N from INB_Inbound where IsActive=1 AND IsDeleted=0 AND InboundID=" + vInboundTrackingID) <= 5)
                    {

                        if (DB.GetSqlN("select top 1 GRNUpdateID AS N from INB_GRNUpdate  where IsActive=1 AND IsDeleted=0 AND InboundID=" + vInboundTrackingID) != 0)
                        {
                            resetError("First revert GRN details", true);
                            return;
                        }

                        sqlRevertTrans.Append("EXEC [dbo].[sp_INB_RevertShipmmentVerification]  @InboundID=" + vInboundTrackingID + ",@RefWHID=" + vRefStoreID+ ",@UpdatedBy="+cp.UserID);

                        try
                        {
                            DB.ExecuteSQL(sqlRevertTrans.ToString());

                            //CommonLogic._DeleteAttatchment(TenantRootDir + DB.GetSqlS("select UniqueID AS S from TPL_Tenant where TenantID=" + IBTrack.TenantID) + InboundPath + InbShpVerifNotePath, DB.GetSqlS("select StoreRefNo AS S from  INB_Inbound where IsActive=1 AND IsDeleted=0 AND InboundID=" + vInboundTrackingID) + "_" + DB.GetSqlN("select WarehouseID AS N from INB_RefWarehouse_Details where IsActive=1 AND IsDeleted=0  AND IB_RefWarehouse_DetailsID=" + vRefStoreID) + ".pdf");

                            CommonLogic._DeleteAttatchment(TenantRootDir + IBTrack.TenantID + InboundPath + InbShpVerifNotePath, vStoreRefNo + "_" + DB.GetSqlN("select WarehouseID AS N from INB_RefWarehouse_Details where IsActive=1 AND IsDeleted=0  AND IB_RefWarehouse_DetailsID=" + vRefStoreID) + ".pdf");

                            //File.Delete("~/"+CommonLogic._GetAttatchmentFile(TenantRootDir + DB.GetSqlS("select UniqueID AS S from GEN_Tenant where TenantID=" + cp.TenantID) + InboundPath + InbShpVerifNotePath, DB.GetSqlS("select StoreRefNo AS S from  INB_Inbound where IsActive=1 AND IsDeleted=0 AND InboundID=" + vInboundTrackingID) + "_" + DB.GetSqlN("select WarehouseID AS N from INB_RefWarehouse_Details where IsActive=1 AND IsDeleted=0  AND IB_RefWarehouse_DetailsID=" + vRefStoreID)));

                            resetError("Shipment verification details successfully reverted", false);
                        }
                        catch (Exception ex)
                        {
                            CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
                            resetError("Error while reverting Shipment verification details", true);
                        }
                    }
                    else
                    {
                        resetError("First revert GRN details", true);
                        return;
                    
                    }

                    break;


                case "Close":

                    sqlRevertTrans.Clear();

                    if (DB.GetSqlN("Select count(InboundID) as N from INB_Inbound Where InBoundStatusID != 1  AND InboundID=" + vInboundTrackingID) > 0)
                    {
                        resetError("Sorry, you cannot close this shipment until you revert to 'Shipment Initiated' status", true);
                        return;
                    }

                    //if (DB.GetSqlN("select top 1 Inbound_SupplierInvoiceID AS N from INB_Inbound_ORD_SupplierInvoice where IsActive=1 AND IsDeleted=0  AND InboundID=" + vInboundTrackingID) != 0)
                    //{
                    //    resetError("Please delete 'PO/Invoice Details' before close this shipment ", true);
                    //    return;
                    //}

                    sqlRevertTrans.Append("BEGIN TRANSACTION ");
                    sqlRevertTrans.Append(" UPDATE INB_Inbound SET IsActive=0  WHERE InboundID=" + vInboundTrackingID);
                    sqlRevertTrans.Append(" ;delete from  INB_InboundTracking_Warehouse  WHERE InboundID=" + vInboundTrackingID);
                    sqlRevertTrans.Append(" ;delete from INB_Inbound_ORD_SupplierInvoice where InboundID=" + vInboundTrackingID);
                    sqlRevertTrans.Append(" ;COMMIT TRANSACTION ");

                    try
                    {
                        DB.ExecuteSQL(sqlRevertTrans.ToString());
                        
                        //CommonLogic._DeleteAttatchment(TenantRootDir + DB.GetSqlS("select UniqueID AS S from GEN_Tenant where TenantID=" + cp.TenantID) + InboundPath + InbShpDelNotePath, DB.GetSqlS("select StoreRefNo AS S from  INB_Inbound where IsActive=1 AND IsDeleted=0 AND InboundID=" + vInboundTrackingID) + "_" + DB.GetSqlN("select WarehouseID AS N from INB_RefWarehouse_Details where IsActive=1 AND IsDeleted=0  AND IB_RefWarehouse_DetailsID=" + vRefStoreID)+".pdf");

                        CommonLogic._DeleteAttatchment(TenantRootDir +  IBTrack.TenantID + InboundPath + InbShpDelNotePath, vStoreRefNo + "_" + DB.GetSqlN("select WarehouseID AS N from INB_RefWarehouse_Details where IsActive=1 AND IsDeleted=0  AND IB_RefWarehouse_DetailsID=" + vRefStoreID) + ".pdf");

                        resetError("Successfully Closed", false);
                    }
                    catch (Exception ex)
                    {
                        CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
                        resetError("Error while closing this shipment", true);
                    }

                    break;

                case "ReleaseInventory":
                    break;

            }

            this.ShipmentResults_buildGridData(this.ShipmentResults_buildGridData());

        }

        protected void gvShipmentResults_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.DataItem == null)
                return;

            LinkButton lnkChildItems = (LinkButton)e.Row.FindControl("lnkRevertGRN");
            lnkChildItems.OnClientClick = "ProdOpenDialog();"; 
        }


        #endregion -------------------------Shipment Results -----------------------------


        #region ----------------- GRN  GridView -----------------


        protected DataSet gvGRNDetails_buildGridData()
        {
            string sql = ViewState["GRNListSQL"].ToString();
            DataSet ds = DB.GetDS(sql, false);
            return ds;
        }

        protected void gvGRNDetails_buildGridData(DataSet ds)
        {
            gvGRNDetails.DataSource = ds.Tables[0];
            gvGRNDetails.DataBind();
            ds.Dispose();

            ScriptManager.RegisterStartupScript(this, this.GetType(), "GRNDetails", "unblockDialog();", true);
        }

        protected void gvGRNDetails_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
          
         
            gvGRNDetails.PageIndex = e.NewPageIndex;
            gvGRNDetails.EditIndex = -1;
            this.gvGRNDetails_buildGridData(this.gvGRNDetails_buildGridData());
        }

        protected void lnkRevertGRNItem_Click(object sender, EventArgs e)
        {
           
            string rfidIDs = "";
            string vInboundIDs = "";
            string vPOHeaderIDs = "";
            string vSupplierInvoiceIDs = "";
            int COUNT = 0;
            bool chkBox = false;
            cp = HttpContext.Current.User as CustomPrincipal;
            StringBuilder grnRevert = new StringBuilder();
           
            //'Navigate through each row in the GridView for checkbox items
            foreach (GridViewRow gv in gvGRNDetails.Rows)
            {

                CheckBox isDelete = (CheckBox)gv.FindControl("chkRevertGRNItems");
                Literal ltGRNHeader = (Literal)gv.FindControl("ltHidInbound_GRNUpdateID");
                Literal ltInboundID = (Literal)gv.FindControl("lthidInboundID");
                if (isDelete.Checked)
                {
                    COUNT++;
                    chkBox = true;
                    rfidIDs = ltGRNHeader.Text;
                    vInboundIDs = ltInboundID.Text;

                    if (rfidIDs != "")
                    {

                        //=========== Logic changed by swamy on 26 jun 2020  ================//
                        string sqlGrnRevert = "EXEC[dbo].[sp_INV_RevertGRNConfirmedStock]@GRNHeaderID = " + rfidIDs + ",@CreatedBy = " + cp.UserID;

                        if (DB.GetSqlN(sqlGrnRevert) != 1)
                        {
                            resetError("Once Goods-Out process is started, cannot revert the shipment details", true);
                            return;
                        }
                      
                    }

                }
            }
            if (COUNT == 0)
            {
                resetError("Please select  line item", true);
                return;
            }


            try
            {

                if (chkBox)
                {

                    DB.ExecuteSQL("EXEC [dbo].[sp_INB_ClosePOStatus] @InboundID=" + vInboundIDs + ",@POStatusID=2,@Status=1");

                    if (DB.GetSqlN("select Count(*) AS [N] from INB_GRNUpdate where InboundID=" + vInboundIDs + " AND  IsActive=1 AND IsDeleted=0 AND IsCancelled=0") == 0)
                    {
                        DB.ExecuteSQL("update INB_Inbound set InboundStatusID=3 where  InboundID=" + vInboundIDs);
                    }
                 
                     resetError("Successfully deleted the selected line items", false);
                }
              
            }
            catch (Exception ex)
            {
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
                resetError("Error while deleting selected line items", true);
            }

            this.gvGRNDetails_buildGridData(this.gvGRNDetails_buildGridData());

            this.ShipmentResults_buildGridData(this.ShipmentResults_buildGridData());


        }

        #endregion ----------------- GRN  GridView -----------------


    }
}