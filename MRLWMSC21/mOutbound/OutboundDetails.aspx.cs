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
using System.IO;
using System.Globalization;
using System.Web.Services;
using Newtonsoft.Json;



// Module Name : OutboundDetails Under Outbound
// Usecase Ref.: Outbound_UC_015
// DevelopedBy : Naresh P
// CreatedOn   : 4/11/2013
// Modified On : 24/03/2015

namespace MRLWMSC21.mOutbound
{
    public partial class OutboundDetails : System.Web.UI.Page
    {

        int TenantID = 0;
        int RequestedBy = 0;
        int CreatedBy = 0;
        CustomPrincipal cp = HttpContext.Current.User as CustomPrincipal;
        public static CustomPrincipal cp1 = HttpContext.Current.User as CustomPrincipal;
        public static int OBDID;
        public static string OutBoundStatus;
        protected string selectGDRSQL = "";
        public OBDTrack OBDTrack;
        protected string OBDTrackID = "";
        string TenantRootDir = "";
        string OutboundPath = "";
        string OBD_DeliveryNotePath = "";
        string OBD_PickandCheckSheetPath = "";
        string OBD_PODPath = "";
        public static string UserName;

        string SOListSql = "";
        string RequiredVehicleSql = "";
        string UsedVehicleSql = "";

        public String todayDate = "";

        //public int OBDProductionStatus = 0;


        protected void Page_PreInit(object sender, EventArgs e)
        {
            Page.Theme = "Outbound";
        }



        protected void Page_Init(object sender, EventArgs e)
        {

            CreatedBy = cp.UserID;
            TenantID = cp.TenantID;
            RequestedBy = cp.UserID;
         
            ViewState["TenantID"] = TenantID;
            OBDID = Convert.ToInt32(CommonLogic.QueryString("obdid"));
            if (CommonLogic.QueryString("obdid") != "" && CommonLogic.QueryString("edittype") != "")
            {
                hidAccordionIndex.Value = CommonLogic.QueryString("edittype");
            }
            else
            {
                hidAccordionIndex.Value = "0";
            }

            //if (CommonLogic.QueryString("obdid") != "")
            //{
            //    int OutboundID = Convert.ToInt32(CommonLogic.QueryString("obdid"));
            //    if (DB.GetSqlN("select OutboundID as N from OBD_Outbound where IsActive=1 and IsDeleted=0  and OutboundID=" + OutboundID) != 0)
            //    {
            //        OBDTrack = new OBDTrack(OutboundID);
            //        lblOutboundStatus.Text =  OBDTrack.DeliveryStatus; 
            //    }
            //}


        }

        protected void Page_Load(object sender, EventArgs e)
        {

            //Roles Allowed: 1,2,3
            //if (!cp.IsInAnyRoles(CommonLogic.GetRolesAllowed(CommonLogic.GetRolesAllowedForthisPage("New Delivery"))))
            //{
            //    Response.Redirect("../Login.aspx?eid=6");
            //}

            todayDate = DateTime.UtcNow.ToString("dd/MM/yyyy");

            ViewState["TenantID"] = TenantID;

            // Add Max date to range validation 
            //rvtxtOBDDate.MaximumValue = DateTime.UtcNow.ToString("dd/MM/yyyy");

            string query = "EXEC [dbo].[sp_TPL_GetTenantDirectoryInfo] @TypeID=2";

            DataSet dsPath = DB.GetDS(query.ToString(), false);

            TenantRootDir = dsPath.Tables[0].Rows[4][0].ToString();
            OutboundPath = dsPath.Tables[0].Rows[0][0].ToString();
            OBD_DeliveryNotePath = dsPath.Tables[0].Rows[1][0].ToString();
            OBD_PickandCheckSheetPath = dsPath.Tables[0].Rows[2][0].ToString();
            OBD_PODPath = dsPath.Tables[0].Rows[3][0].ToString();


            if (CommonLogic.QueryString("obdid") != "")
            {
                OBDTrackID = CommonLogic.QueryString("obdid");

                lnkCancelOBD.Visible = false;

                lnkCancelSendToPGI.Visible = false;
                lnkPGICancel.Visible = false;
                lnkPackButCancel.Visible = false;
                //<!----------------Procedure Converting---------------->
                //  int DelivarySattusID = DB.GetSqlN("select DeliveryStatusID AS N from OBD_Outbound where IsActive=1 AND IsDeleted=0 AND OutboundID=" + OBDTrackID);
                int DelivarySattusID = DB.GetSqlN("Exec [dbo].[USP_GetDeliveryStatusIDByOBD] @OutboundID=" + OBDTrackID);
                if (DelivarySattusID == 4)
                {
                    lnkCancelSendToPGI.Visible = false;
                    lnkPGICancel.Visible = false;
                    lnkPackButCancel.Visible = false;
                    lnkCancelDelivery.Visible = false;
                    //upnl3pl.Visible = true; ==================== CommentedBy M.D.Prasad On 18-DEC-2019 ====================== For Hiding and Showing Panels
                }
                else if (DelivarySattusID >= 4)
                {
                    upnl3pl.Visible = true;
                    pnlPackagingdetails.Visible = true; //==================== CommentedBy M.D.Prasad On 18-DEC-2019 ====================== For Hiding and Showing Panels
                }

                // if (DB.GetSqlN("select OutboundID AS N from OBD_Outbound where IsActive=1 and IsDeleted=0 and OutboundID=" + OBDTrackID) == 0)
                if (DB.GetSqlN("Exec [dbo].[USP_GetOBDByID] @OutboundID=" + OBDTrackID) == 0)
                {
                    resetError("Invalid Request", true);
                    return;
                }
                if (!IsPostBack)
                {

                }
                OBDTrack = new OBDTrack(Convert.ToInt32(OBDTrackID));

                lnkSaveOBD.Text = "Update Delivery" + MRLWMSC21Common.CommonLogic.btnfaUpdate;

                //OBDProductionStatus = DB.GetSqlN("select OBD_CP.OutboundID AS N  from MFG_SOPO_ProductionOrder MFG_SPP JOIN OBD_Outbound_ORD_CustomerPO OBD_CP ON OBD_CP.SOHeaderID=MFG_SPP.SOPOHeaderID AND OBD_CP.IsActive=1 AND OBD_CP.IsDeleted=0 JOIN MFG_ProductionOrderHeader MFG_POH ON MFG_POH.ProductionOrderHeaderID=MFG_SPP.ProductionOrderHeaderID AND MFG_POH.IsActive=1 AND MFG_POH.IsDeleted=0 where MFG_SPP.SOPOTypeID=2 AND MFG_POH.ProductionOrderStatusID<>1 AND OBD_CP.OutBoundID="+OBDTrack.OBDTrackingID);

                ValidateCharge();

                //  ValidateOutboundStatus();
            }
            else
            {
                OBDTrack = new OBDTrack();
                lnkSaveOBD.Text = "Initiate Delivery" + MRLWMSC21Common.CommonLogic.btnfaSave;

            }


            if (CommonLogic.QueryString("obdupdate") == "okfileuploaderr")
                lblStatusMessage.Text = "Delivery Doc. Updated ,but error uploading Delivery Note.";
            else if (CommonLogic.QueryString("obdupdate") == "dbupdateerr")
                lblStatusMessage.Text = "Error: Duplicate Delv. Doc. Number.";
            else if (CommonLogic.QueryString("obdupdate") == "oknofileuploaderr")
                lblStatusMessage.Text = "Delivery Updated, but no Delivery Note File uploaded";
            else if (CommonLogic.QueryString("obdupdate") == "ok")
                lblStatusMessage.Text = "Delivery Updated , Now you can attach/add line items to this Delivery Doc.";
            else if (CommonLogic.QueryString("obdupdate") == "updateok")
                lblStatusMessage.Text = "Delivery details updated successfully";


            if (CommonLogic.GoodOutStatus(OBDTrack.OBDTrackingID) == false)
            {
                lnkViewPendingGoodsOutList.Visible = true;

                lnkViewPendingGoodsOutList.OnClientClick = "openDialog('Pending Goods-OUT List', '" + lnkViewPendingGoodsOutList.ClientID + "');";
                ViewState["PendingGoodsOutListSQL"] = " EXEC [sp_INV_GetPendingGoodsOutList_NEW] @OutboundID=" + OBDTrack.OBDTrackingID;

                this.gvPendingGoodsOutList_buildGridData(gvPendingGoodsOutList_buildGridData());
            }



            if (!IsPostBack)
            {
                ddlDeliveryDocType.Focus();
                Page.Validate();

                //Sets the Sub Header
                DesignLogic.SetInnerPageSubHeading(this, "Outbound Details");

                lnkAddNewUsedVehicle.Visible = false;
                txtDocRcvdDate.Text = DateTime.UtcNow.ToString("dd-MMM-yyyy");
                //txtDocRcvdDate.Text = OBDTrack.DocumentReceivedDate;
                txtDeliveryDate.Text = DateTime.UtcNow.ToString("dd-MMM-yyyy");
                //txtReceivedDelTimeEntry.Text = DateTime.UtcNow.ToString("hh:mm tt");



                LoadPriorityLevel();
                LoadDocType(ddlDeliveryDocType);
                // LoadWHOptions(cblStoresAssociated, ""); // No Need to load on page load. will load later on Tenant Selection though Ajax Call
                LoadInstructionMode(ddlInstructionMode);


                //atcRequestedBy.Text = cp.FirstName;
                //txtGDRApprovedBy.Text = cp.FirstName;
                //txtGDRPreparedBy.Text = cp.FirstName;
                //atcGDRRequestedBy.Text = cp.FirstName;
                hifRequestedBy.Value = cp.UserID.ToString();
                hifGDRApprovedByID.Value = cp.UserID.ToString();
                hifGDRPreparedByID.Value = cp.UserID.ToString();
                hifGDRRequestedByID.Value = cp.UserID.ToString();

                if (CommonLogic.QueryString("obdid") != "")
                {
                    if (OBDTrack.OBDNumber != null)
                    {
                        UserName = CommonLogic.GetUserName(OBDTrack.PGIDoneBy.ToString());
                        if (UserName != "")
                        {
                            pnlPackagingdetails.Visible = true;
                            //pnlReceivedDelivery.Visible = true;
                        }
                        LoadOBDetails();
                        //LoadWHOptions(cblStoresAssociated, OBDTrack.ReferedStoreIDs);
                        //LoadWHOptions_DDL(OBDTrack.TenantID);
                        //ddlStoresAssociated.Value = OBDTrack.TenantID.ToString();                   
                        hdnStoresAssociated.Value = OBDTrack.ReferedStoreIDs;
                        txtStoresAssociated.Text = DB.GetSqlS("SELECT top 1 WHCODE S FROM GEN_Warehouse WHERE WarehouseID IN (" + OBDTrack.ReferedStoreIDs + ")");
                        LoadHHTypes();
                        LoadWHStores();
                        LoadReceivedWHStores();

                        
                       
                        LoadPackingWHStores();
                        // pnlDelvDocLineItems.Visible = true;





                        // <!-----------Procedure Converting------------>
                        // int REF_WHID = DB.GetSqlN("select top 1 OB_RefWarehouse_DetailsID AS N from OBD_RefWarehouse_Details where IsActive=1 and IsDeleted=0 and OutboundID=" + OBDTrack.OBDTrackingID.ToString());
                        int REF_WHID = DB.GetSqlN("Exec [dbo].[USP_GetREF_WHID] @OutboundID=" + OBDTrack.OBDTrackingID.ToString());
                        // <!-----------Procedure Converting------------>
                        // int WHID = DB.GetSqlN("Select WarehouseID AS N from OBD_RefWarehouse_Details where OB_RefWarehouse_DetailsID=" + REF_WHID);
                        int WHID = DB.GetSqlN("Exec [dbo].[USP_GetWHIDByRefWHID]  @REFWHID = " + REF_WHID);



                        //SELECT FORMAT((SELECT[dbo].[UDF_GetWarehouseTime] ('05:27 PM',38)),'hh:mm tt') AS S

                        LoadPickNCheckDetails(REF_WHID, WHID);

                        if (OBDTrack.SentForPGI_DT != null)
                        {
                            txtOBDRcvdDate.Text = OBDTrack.SentForPGI_DT.Split(' ')[0];
                            //OBDRecvdTimeEntry.Text = OBDTrack.SentForPGI_DT.Split(' ')[1] + " " + OBDTrack.SentForPGI_DT.Split(' ')[2];
                            OBDRecvdTimeEntry.Text = OBDTrack.SentForPGI_DT.Split(' ')[1];
                        }
                        else
                        {
                            txtOBDRcvdDate.Text = DateTime.UtcNow.Date.ToString("dd-MMM-yyyy");
                            OBDRecvdTimeEntry.Text = DB.GetSqlS("SELECT FORMAT((SELECT[dbo].[UDF_GetWarehouseTime] ('" + DateTime.UtcNow.ToString("hh:mm tt") + "'," + WHID + ")),'hh:mm tt') AS S");
                        }

                        pnlPickNCheck.Visible = true;

                        char[] seps = new char[] { ',' };

                        String ReferedWHs = OBDTrack.ReferedStoreIDs.Substring(0, OBDTrack.ReferedStoreIDs.Length - 1);

                        String[] ArrStores = CommonLogic.FilterSpacesInArrElements(ReferedWHs.Split(seps));

                        //<!Procedure Converting--------------->
                        // if (DB.GetSqlN("select Count(PickedBy) AS N from OBD_OutboundTracking_Warehouse where OutboundID=" + OBDTrack.OBDTrackingID.ToString()) == ArrStores.Length)
                        //if(DB.GetSqlN("Exec [dbo].[USP_GetCountOBDTrack_WHID] @OutboundID =" + OBDTrack.OBDTrackingID.ToString()) == ArrStores.Length)
                        //{
                        if (OBDTrack.DeliveryStatusID <= 7 && OBDTrack.DeliveryStatusID >= 4)
                        {
                            LoadPGIDetails();

                            pnlPGIDetails.Visible = true;
                        }
                        if (OBDTrack.DeliveryStatusID == 4)
                        {
                            LoadReceivedDeliveryDetails(REF_WHID, WHID);
                        }

                        //}
                        //<!----------------Procedure Converting--------->
                        // int vREF_WHID = DB.GetSqlN("select OB_RefWarehouse_DetailsID AS N from OBD_RefWarehouse_Details where IsActive=1 and IsDeleted=0 and WarehouseID=" + ddlStores.SelectedValue + " and OutboundID=" + OBDTrack.OBDTrackingID.ToString());
                        int vREF_WHID = DB.GetSqlN("[dbo].[USP_GetREF_WHID]  @WarehouseID=" + ddlStores.SelectedValue + " ,@OutboundID=" + OBDTrack.OBDTrackingID.ToString());

                        // int OBD_Tracking_WHID = DB.GetSqlN("Select OutboundTracking_WarehouseID as N from OBD_OutboundTracking_Warehouse Where OutboundID =" + OBDTrack.OBDTrackingID.ToString() + " AND  OB_RefWarehouse_DetailsID=" + vREF_WHID);
                        int OBD_Tracking_WHID = DB.GetSqlN("Exec [dbo].[USP_GetOBDTrack_WHID] @OutboundID=" + OBDTrack.OBDTrackingID.ToString() + ",@REF_WHID=" + vREF_WHID);
                        // int sREF_WHID = DB.GetSqlN("select OB_RefWarehouse_DetailsID AS N from OBD_RefWarehouse_Details where IsActive=1 and IsDeleted=0 and WarehouseID=" + ddlReceivingStore.SelectedValue + " and OutboundID=" + OBDTrack.OBDTrackingID.ToString());
                        int sREF_WHID = DB.GetSqlN("[dbo].[USP_GetREF_WHID]  @WarehouseID=" + ddlReceivingStore.SelectedValue + " ,@OutboundID=" + OBDTrack.OBDTrackingID.ToString());

                        //  int sOBD_Tracking_WHID = DB.GetSqlN("Select OutboundTracking_WarehouseID as N from OBD_OutboundTracking_Warehouse Where OutboundID =" + OBDTrack.OBDTrackingID.ToString() + " AND  OB_RefWarehouse_DetailsID=" + sREF_WHID);
                        int sOBD_Tracking_WHID = DB.GetSqlN("Exec [dbo].[USP_GetOBDTrack_WHID] @OutboundID=" + OBDTrack.OBDTrackingID.ToString() + ",@REF_WHID=" + sREF_WHID);

                        // int mREF_WHID = DB.GetSqlN("select OB_RefWarehouse_DetailsID AS N from OBD_RefWarehouse_Details where IsActive=1 and IsDeleted=0 and WarehouseID=" + ddlPackingStores.SelectedValue + " and OutboundID=" + OBDTrack.OBDTrackingID.ToString());
                        int mREF_WHID = DB.GetSqlN("[dbo].[USP_GetREF_WHID]  @WarehouseID=" + ddlPackingStores.SelectedValue + " ,@OutboundID=" + OBDTrack.OBDTrackingID.ToString());




                        RequiredVehicleSql = "EXEC [dbo].[sp_OBD_GetRequiredVehicleDetails]  @OutboundTracking_WarehouseID=" + OBD_Tracking_WHID;
                        ViewState["ReqVListSQL"] = RequiredVehicleSql;
                        this.gvRequiredVehicle_buildGridData(this.gvRequiredVehicle_buildGridData());


                        UsedVehicleSql = "EXEC [dbo].[sp_OBD_GetUsedVehicleDetails]  @OutboundTracking_WarehouseID=" + sOBD_Tracking_WHID;
                        ViewState["UsedVListSQL"] = UsedVehicleSql;
                        this.gvUsedVehicle_buildGridData(this.gvUsedVehicle_buildGridData());


                        ViewState["3plINListSQL"] = "[dbo].[sp_TPL_GetDataforAccessoryTransaction] @TransactionTypeID=2, @TransactionID=" + CommonLogic.IIF(CommonLogic.QueryString("obdid") == "", "0", CommonLogic.QueryString("obdid")) + ",@TenantID=" + hifTenant.Value;
                        this.gv3pl_List_buildGridData(this.gv3pl_List_buildGridData());

                        //===Added By Priya=====/
                        //ViewState["PackingSlipHListSQL"] = "[dbo].[USP_GET_OBD_PackingSlip_Header_Info]  @OutBoundId=" + CommonLogic.IIF(CommonLogic.QueryString("obdid") == "", "0", CommonLogic.QueryString("obdid"));
                        //this.grdPackaging_buildGridData(this.grdPackaging_buildGridData());
                        //===Ended By Priya=====/
                        //<!PRocedure converting----------->
                        // if (DB.GetSqlN("select Count(PackedBy) AS N from OBD_OutboundTracking_Warehouse where OutboundID=" + OBDTrack.OBDTrackingID.ToString()) == ArrStores.Length)
                        if (DB.GetSqlN("[dbo].[USP_GetCountOBDTrack_WHID] @OutboundID =" + OBDTrack.OBDTrackingID.ToString()) == ArrStores.Length)
                        {
                            // LoadReceivedDeliveryDetails(REF_WHID, WHID);
                            pnlReceivedDelivery.Visible = true;
                            lnkAddNewUsedVehicle.Visible = true;
                            lblDeliveryStore.Text = ddlStores.SelectedItem.Text;
                        }

                        if (OBDTrack.DeliveryStatusID == 5 || OBDTrack.DeliveryStatusID == 4 || OBDTrack.DeliveryStatusID == 7 || OBDTrack.DeliveryStatusID == 6)
                        {
                            LoadPackingDetails(REF_WHID, WHID);
                            //pnlPackingDetails.Visible = true;
                            //pnlPackagingdetails.Visible = true;
                            //pnlReceivedDelivery.Visible = true;

                        }
                        if (OBDTrack.DeliveryStatusID == 6 || OBDTrack.DeliveryStatusID == 7 || OBDTrack.DeliveryStatusID == 4)
                        {
                            pnlPackagingdetails.Visible = true;
                            upnl3pl.Visible = true;
                        }
                        //if (OBDTrack.DeliveryStatusID == 7 || OBDTrack.DeliveryStatusID == 4)  //==================== CommentedBy M.D.Prasad On 18-DEC-2019 ====================== For Hiding and Showing Panels
                        if (OBDTrack.DeliveryStatusID >= 4)
                        {
                            //pnlReceivedDelivery.Visible = true;
                            pnlReceivedDelivery.Attributes.Add("style", "display:block");
                        }
                        if (OBDTrack.DeliveryStatusID >= 2)
                        {
                            gvSOLineItems.Columns[7].Visible = false;
                        }

                        /*
                        if (ddlDeliveryDocType.SelectedValue == "6")
                        {
                            upnlProdOrder.Visible = true;
                            pnlProdOrder.Visible = true;

                            upnlDelvDocLineItems.Visible = false;

                            SOListSql = "EXEC [dbo].[sp_OBD_GetProductionOrderLineItems] @OutboundID=" + CommonLogic.QueryString("obdid");
                            ViewState["ProLineItemsSQL"] = SOListSql;
                            this.gvProdOrderLineItems_buildGridData(this.gvProdOrderLineItems_buildGridData());
                        }
                        else
                        {
                            upnlProdOrder.Visible = false;
                            pnlDelvDocLineItems.Visible = true;

                            upnlDelvDocLineItems.Visible = true;

                            SOListSql = "EXEC [dbo].[sp_OBD_GetSOLineItems] @OutboundID=" + CommonLogic.QueryString("obdid");
                            ViewState["SOLineItemsSQL"] = SOListSql;
                            this.GVSOLineItems_buildGridData(this.GVSOLineItems_buildGridData());

                        }*/

                        upnlProdOrder.Visible = false;
                        pnlDelvDocLineItems.Visible = true;

                        upnlDelvDocLineItems.Visible = true;

                        SOListSql = "EXEC [dbo].[sp_OBD_GetSOLineItems] @OutboundID=" + CommonLogic.QueryString("obdid");
                        ViewState["SOLineItemsSQL"] = SOListSql;
                        this.GVSOLineItems_buildGridData(this.GVSOLineItems_buildGridData());
                        ValidateOutboundStatus();
                    }
                    else
                    {
                        resetError("No Details Available", true);
                        return;
                    }
                }
            }
        }

        private void ValidateCharge()
        {
            //if (DB.GetSqlN("select COUNT(*) N from OBD_Outbound where PGIDoneOn is not null and IsDeleted=0 and IsActive=1 and OutboundID=" + CommonLogic.QueryString("obdid")) > 0)
            if (DB.GetSqlN("[dbo].[USP_GetOBDPGIDone] @OutboundID = " + CommonLogic.QueryString("obdid")) > 0)
            {

                //  if (DB.GetSqlN("select COUNT(*) N from OBD_Outbound OBT JOIN TPL_Tenant_Transaction_Accessorial_Capture TAC ON TAC.TransactionID=OBT.OutboundID AND TAC.IsActive=1 AND TAC.IsDeleted=0 AND TransactionTypeID=2  WHERE  OBT.IsDeleted=0 AND OBT.IsActive=1 AND OBT.OutboundID=" + CommonLogic.QueryString("obdid")) == 0)
                if (DB.GetSqlN("Exec [dbo].[USP_GetCountOBDAccessorial_Capture] @OutboundID=" + CommonLogic.QueryString("obdid")) == 0)
                {
                    string Error = "Activity Tariff details are not specified.";
                    lblChargesMsg.Text = "<font class=\"noticeMsg\">NOTE:</font>&nbsp;&nbsp;&nbsp" + Error + " </font>";
                }
                else
                    lblChargesMsg.Text = "";
            }
        }
        public static string StaticValidateOutboundStatus()
        {
            string StaticOutBoundStatus = DB.GetSqlS("[dbo].[USP_GetDeliveryStatusByOBD] @OutboundID=" + OBDID);
            return StaticOutBoundStatus;
        }

        private void ValidateOutboundStatus()
        {

            // lblOutboundStatus.Text = DB.GetSqlS("SELECT DeliveryStatus S FROM OBD_Outbound OBT JOIN OBD_DeliveryStatus DS ON DS.DeliveryStatusID=OBT.DeliveryStatusID WHERE OBT.IsDeleted=0 AND OBT.IsActive=1 AND OBT.OutboundID=" + CommonLogic.QueryString("obdid"));
            lblOutboundStatus.Text = DB.GetSqlS("[dbo].[USP_GetDeliveryStatusByOBD] @OutboundID=" + CommonLogic.QueryString("obdid"));
            OutBoundStatus = lblOutboundStatus.Text;
        }

        #region ----- Initiate OBD ------

        private void LoadOBDetails()
        {

            //if (OBDTrack.OBDNumber != null)
            //{
            ddlDeliveryDocType.SelectedValue = OBDTrack.DocumentTypeID.ToString();
            txtOBDNumber.Text = OBDTrack.OBDNumber;
            DateTime abc = DateTime.ParseExact(OBDTrack.OBDDate.ToString(), "dd-MMM-yyyy", CultureInfo.InvariantCulture);
            if (OBDTrack.OBDDate != null)
                txtOBDDate.Text = abc.ToString("dd-MMM-yyyy");
            else
                txtOBDDate.Text = "";

            hifTenant.Value = OBDTrack.TenantID.ToString();
            txtTenant.Text = OBDTrack.TenantName;
            //atcCustomerName.Text = CommonLogic.GetCustomerName( OBDTrack.CustomerID.ToString());

            atcCustomerName.Text = OBDTrack.Customer.ToString();

            hifCustomerID.Value = OBDTrack.CustomerID.ToString();
            //atcRequestedBy.Text = CommonLogic.GetUserName(OBDTrack.RequestedByID.ToString());

            atcRequestedBy.Text = OBDTrack.RequestedBy.ToString();

            hifRequestedBy.Value = OBDTrack.RequestedByID.ToString();


            txtRemarks_Ini.Text = OBDTrack.RemByIni_OnCreation;

            ddlPriorityLevel.SelectedValue = OBDTrack.PriorityLevel.ToString();
            if (OBDTrack.PriorityDateTime != null)
            {

                txtPriorityDate.Text = Convert.ToDateTime(OBDTrack.PriorityDateTime).ToString("dd/MM/yyyy");
                txtTimeEntry.Text = Convert.ToDateTime(OBDTrack.PriorityDateTime).ToString("hh:mm tt");

            }

            String sFileName = CommonLogic._GetAttatchmentFile(TenantRootDir + hifTenant.Value + OutboundPath + OBD_DeliveryNotePath, OBDTrack.OBDNumber.ToString());


            if (sFileName != "")
            {
                String Path = "../ViewImage.aspx?path=" + sFileName;
                ltDNLink.Text = "<img src=\"../Images/blue_menu_icons/attachment.png\"  />";
                ltDNLink.Text += "<a style=\"text-decoration:none;\" href=\"#\" onclick=\" OpenImage(' " + Path + " ')  \" > View Delivery Doc. </a>";
                ltDNLink.Text += "<img src=\"../Images/redarrowright.gif\"   />";
            }


            //}

        }

        private void LoadPriorityLevel()
        {
            ddlPriorityLevel.Items.Clear();
            ddlPriorityLevel.Items.Add(new ListItem("Normal", "0"));
            ddlPriorityLevel.Items.Add(new ListItem("Medium", "1"));
            ddlPriorityLevel.Items.Add(new ListItem("High", "2"));
            ddlPriorityLevel.Items.Add(new ListItem("Highest", "3"));

        }

        private void LoadDocType(DropDownList ddlDocType)
        {

            ddlDocType.Items.Clear();

            //IDataReader rsDocType = DB.GetRS("Select DocumentTypeID,DocumentType from GEN_DocumentType Where IsActive =1 AND IsDeleted=0 AND DocumentTypeID NOT IN (6,7,8,9)  --and TenantID=" + TenantID);
            IDataReader rsDocType = DB.GetRS("Exec [dbo].[USP_GetDeliveryTypeForOBD]");

            ddlDocType.Items.Insert(0, "");
            while (rsDocType.Read())
            {

                ddlDocType.Items.Add(new ListItem(rsDocType["DocumentType"].ToString(), rsDocType["DocumentTypeID"].ToString()));

            }
            rsDocType.Close();

        }

        protected void ddlDeliveryDocType_SelectedIndexChanged(object sender, EventArgs e)
        {
            /* if (ddlDeliveryDocType.SelectedValue == "2")
             {
                 pnlGDRInfo.Visible = true;
                 txtInvoiceDate.Text = DateTime.UtcNow.ToString("dd/MM/yyyy");
                 txtGDRPreparedBy.Text = cp.FirstName + " " + cp.LastName;
             }
             else
             {
                 pnlGDRInfo.Visible = false;
             }
             */
        }

        public void LoadWHOptions_DDL(int TenantID)
        {
            DataTable dt = GetTenantStores(TenantID);
            //ddlStoresAssociated.DataSource = dt;
            //ddlStoresAssociated.DataTextField = "WHCode";
            //ddlStoresAssociated.DataValueField = "WarehouseID";
            //ddlStoresAssociated.DataBind();
            //ddlStoresAssociated.Items.Insert(0, new ListItem("--Select--", "0"));
        }

        public void LoadWHOptions(CheckBoxList varchkL, String SelectedItems)
        {

            varchkL.Items.Clear();

            IDataReader rsOptions = DB.GetRS("Select WareHouseID,WHCode,Location from GEN_WareHouse WHERE isActive=1 AND IsDeleted=0 AND WareHouseID IN (" + String.Join(",", cp.Warehouses) + ")" + DB.GetNoLock());

            char[] seps = new char[] { ',' };
            String[] SelItems = null;

            if (SelectedItems != null)
            {
                SelItems = CommonLogic.FilterSpacesInArrElements(SelectedItems.Split(seps));
            }
            //else
            //{
            //    rsOptions.Close();
            //    return;
            //}
            //SelItems.Contains("2");

            while (rsOptions.Read())
            {
                varchkL.Items.Add(new ListItem(DB.RSField(rsOptions, "WHCode") + " [" + DB.RSField(rsOptions, "Location") + "] ", rsOptions["WareHouseID"].ToString()));


                if (SelItems != null)
                {
                    if (SelItems.Contains(rsOptions["WareHouseID"].ToString()) == true)
                    {
                        varchkL.Items[varchkL.Items.Count - 1].Selected = true;
                    }
                }

            }
            rsOptions.Close();


        }

        public string GetSelectedCheckboxItems(CheckBoxList varChkList)
        {
            string selectedItems = "";
            foreach (ListItem li in varChkList.Items)
            {
                if (li.Selected == true)
                {
                    selectedItems += li.Value + ",";
                }
            }

            //To remove the last comma from the string
            if (selectedItems.EndsWith(","))
                selectedItems = selectedItems.Substring(0, selectedItems.Length - 1);

            return selectedItems;

        }

        protected void btnGetOBDGDR_OnClick(object sender, EventArgs e)
        {
            if (txtOBDNumber.Text == "")
            {
                string sqlText = "";
                txtOBDNumber.Text = "";

                if (ddlDeliveryDocType.SelectedValue != "6")
                {
                    // sqlText = "Select   CONVERT(varchar, CAST( MAX(OBDNumber)+ 1 as NUMERIC(20,0)))   as S from OBD_Outbound ";

                    string sqlOBDNumberlenght = DB.GetSqlS("EXEC [sp_SYS_GetSystemConfigValue]   @SysConfigKey='OutboundDetails.aspx' ,@TenantID=1");

                    sqlText = "SELECT TOP 1 CASE WHEN LEFT(OBDNumber,2)= RIGHT(CONVERT(NVARCHAR,datepart(year,getdate())),2)   THEN Convert(varchar,CONVERT(INT, OBDNumber)+1 )  ELSE RIGHT(CONVERT(NVARCHAR,datepart(year,getdate())),2)+stuff(convert(nvarchar,power(10," + sqlOBDNumberlenght + ")+1),1,1,'') END AS S FROM OBD_Outbound WHERE DocumentTypeID NOT IN (6,7,8,9) ORDER BY OBDNumber DESC";

                    txtOBDNumber.Text = DB.GetSqlS(sqlText).ToString();


                    //To Check the Start digits of  the  GDRNumber with the year digits 
                    if (txtOBDNumber.Text != "")
                    {

                    }
                    else
                    {
                        // If no OBD Records exist in the database
                        txtOBDNumber.Text = DateTime.UtcNow.Year.ToString().Substring(2, 2) + "000001";
                    }


                    /*
                    //If No Plant Delivery Note Records exist in the database
                    if (ddlDeliveryDocType.SelectedValue == "6" && txtOBDNumber.Text == "")
                    {
                        txtOBDNumber.Text = DateTime.UtcNow.Year.ToString().Substring(2, 2) + "000001";
                    }*/


                }
            }

            txtOBDDate.Focus();
        }

        protected void lnkCancelOBD_Click(object sender, EventArgs e)
        {
            Response.Redirect("OutboundSearch.aspx");
        }

        protected void lnkCloseOBD_Click(object sender, EventArgs e)
        {

        }

        protected void lnkCancelfromSystem_Click(object sender, EventArgs e)
        {

        }

        protected void lnkSaveOBD_Click(object sender, EventArgs e)
        {
            try
            {

                Page.Validate("createOBD");

                if (Page.IsValid == false)
                {
                    resetError("Please check for mandatory fields", true);
                    return;
                }

                //if (!cp.IsInAnyRoles("3"))
                //{

                //    resetError("Only OB-Initiator can access outbound delivery", true);
                //    return;

                //}
                try
                {
                    String FileExtenion = "";

                    if (fucOBDDeliveryNote.HasFile)
                    {

                        FileExtenion = Path.GetExtension(fucOBDDeliveryNote.FileName);

                        if (FileExtenion != ".pdf")
                        {
                            resetError("Please attach a valid file", true);
                            return;
                        }
                    }

                    //if (OBDProductionStatus != 0)
                    //{
                    //    resetError("Cannot modify the details, as production process is started", true);
                    //    return;
                    //}

                    if (OBDTrack.DeliveryStatusID == 4)
                    {
                        resetError("Outbound is already delivered", true);
                        return;
                    }

                    //if (DB.GetSqlN("select top 1 OutboundID AS N from OBD_Outbound_ORD_CustomerPO where  IsActive=1 AND IsDeleted=0  AND  OutboundID=" + OBDTrack.OBDTrackingID) != 0)
                    if (DB.GetSqlN(" [dbo].[USP_GetOBDBYCustomerPO] @OutboundID = " + OBDTrack.OBDTrackingID) != 0)

                    {
                        resetError("Once the sales order line items are mapped, cannot modify the header information", true);
                        return;
                    }


                    OBDTrack.OBDNumber = txtOBDNumber.Text.Trim();
                    OBDTrack.DocumentTypeID = Convert.ToInt32(ddlDeliveryDocType.SelectedValue.Trim());


                    /*
                     if (Convert.ToDateTime(OBDDate) > DateTime.Today)
                     {
                         lblStatusMessage.Text = "'Delivery Doc. Date' cannot exceed current server date.";
                         txtOBDDate.Focus();
                         return;
                     }
                     */

                    OBDTrack.OBDDate = (txtOBDDate.Text != "" ? DateTime.ParseExact(txtOBDDate.Text.Trim(), "dd-MMM-yyyy", CultureInfo.InvariantCulture).ToString("dd-MMM-yyyy") : null);

                    /*
                    if (ddlDeliveryDocType.SelectedValue == "2")
                    {
                        OBDTrack.CustomerName = txtCompanyName.Text.Trim();
                        OBDTrack.GDRPONo = txtPONumber.Text.Trim();
                        OBDTrack.GDRInvoiceNo = txtInvoiceNo.Text.Trim();
                        OBDTrack.GDRInvoiceDate = Convert.ToDateTime(txtInvoiceDate.Text.Trim());
                        OBDTrack.RemByIni_OnCreation = txtPDNRemarks.Text.Trim();

                    }
                     */

                    OBDTrack.CustomerName = atcCustomerName.Text.Trim();
                    OBDTrack.CustomerID = Convert.ToInt32(CommonLogic.IIF(Request.Form[hifCustomerID.UniqueID] != null && Request.Form[hifCustomerID.UniqueID] != "", Request.Form[hifCustomerID.UniqueID], "0"));
                    OBDTrack.RemByIni_OnCreation = txtRemarks_Ini.Text.Trim();

                    OBDTrack.RequestedByID = Convert.ToInt32(CommonLogic.IIF(Request.Form[hifRequestedBy.UniqueID] != null && Request.Form[hifRequestedBy.UniqueID] != "", Request.Form[hifRequestedBy.UniqueID], "0"));

                    if (Request.Form[hifDepartmentID.UniqueID] != null && Request.Form[hifDepartmentID.UniqueID] != "")
                    {
                        // OBDTrack.MMDepartmentID = Convert.ToInt32(Request.Form[hifDepartmentID.UniqueID]);
                    }
                    else
                    {
                        OBDTrack.MMDepartmentID = 0;
                    }

                    if (Request.Form[hifDivisionID.UniqueID] != null && Request.Form[hifDivisionID.UniqueID] != "")
                    {

                        // OBDTrack.MMDivisionID = Convert.ToInt32(Request.Form[hifDivisionID.UniqueID]);
                    }
                    else
                    {
                        OBDTrack.MMDivisionID = 0;
                    }


                    // OBDTrack.ReferedStoreIDs = GetSelectedCheckboxItems(cblStoresAssociated);
                    //OBDTrack.ReferedStoreIDs = ddlStoresAssociated.Value;
                    OBDTrack.ReferedStoreIDs = hdnStoresAssociated.Value;

                    if (OBDTrack.ReferedStoreIDs == "")
                    {
                        resetError("Please select atleast one store", true);
                        return;
                    }



                    //To retain the DeliveryStatusID if not a  new OBD OR Cancled OBD or Closed OBD
                    if (OBDTrack.DeliveryStatusID == 0 || OBDTrack.DeliveryStatusID == 3 || OBDTrack.DeliveryStatusID == 10)
                        OBDTrack.DeliveryStatusID = 1;

                    OBDTrack.PriorityLevel = Convert.ToInt32(ddlPriorityLevel.SelectedValue.Trim());


                    OBDTrack.PriorityDateTime = ((txtPriorityDate.Text.Trim() != "" && txtTimeEntry.Text.Trim() != "") ? DateTime.ParseExact(txtPriorityDate.Text.Trim() + " " + txtTimeEntry.Text.Trim(), "dd/MM/yyyy hh:mm tt", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy hh:mm tt") : null);

                    OBDTrack.InitiatedBy = cp.UserID;

                    OBDTrack.LastModifiedBy = cp.UserID;
                    OBDTrack.CreatedBy = cp.UserID;
                    OBDTrack.IsDNPublished = 0;
                    OBDTrack.IsReservationDelivery = 0;
                    OBDTrack.TenantID = Convert.ToInt16(hifTenant.Value);//TenantID;
                    OBDTrack.TenantName = txtTenant.Text;



                    if (CommonLogic.QueryString("obdid") == "")
                    {
                        ////////////////////////////////Create NEW OBD//////////////////////////////////////////

                        //<!--------------Procedure Converting------------>
                        // int SQlCheckIfOBDExists = DB.GetSqlN("Select count(OBDNumber) as N from OBD_Outbound     Where OBDNumber=" + DB.SQuote(txtOBDNumber.Text));
                        int SQlCheckIfOBDExists = DB.GetSqlN("[dbo].[USP_GetOBDByNumber] @OBDNumber = " + DB.SQuote(txtOBDNumber.Text));


                        if (SQlCheckIfOBDExists == 0)
                        {
                            int OutboundID = OBDTrack.InitiateOBDTracking();

                            // string OBDNumber = DB.GetSqlS("select OBDNumber S from OBD_Outbound where OutboundID="+OutboundID);
                            string OBDNumber = DB.GetSqlS("Exec [dbo].[USP_GetOBDNumberByID] @OutboundID = " + OutboundID);


                            if (OutboundID != 0)
                            {

                                //Audit.LogTranscationAudit(cp.UserID.ToString(), 2, 1, OBDTrack.OBDTrackingID, 1);

                            }
                            else
                            {
                                Response.Redirect("OutboundDetails.aspx?obdupdate=dbupdateerr");

                            }

                            if (fucOBDDeliveryNote.HasFile)
                            {



                                if (FileExtenion == ".pdf")
                                {
                                    if (CommonLogic.UploadFile(TenantRootDir + hifTenant.Value + OutboundPath + OBD_DeliveryNotePath, fucOBDDeliveryNote, OBDNumber + Path.GetExtension(fucOBDDeliveryNote.FileName)) == false)
                                    {
                                        //Error in uploading the delivery note file
                                        Response.Redirect("OutboundDetails.aspx?obdid=" + OBDTrack.OBDTrackingID.ToString() + "&obdupdate=okfileuploaderr");
                                    }
                                    else
                                    {
                                        Response.Redirect("OutboundDetails.aspx?obdid=" + OBDTrack.OBDTrackingID.ToString() + "&obdupdate=ok");
                                    }
                                }
                                else
                                {
                                    resetError("Please attach a valid file", true);
                                    return;
                                }
                            }
                            else
                            {

                                Response.Redirect("OutboundDetails.aspx?obdid=" + OBDTrack.OBDTrackingID.ToString() + "&obdupdate=oknofileuploaderr");
                            }

                        }
                        else
                        {
                            lblStatusMessage.Text = "'OBD Number' already exists. Please change the 'OBD Number' and try again";
                        }
                    }
                    else
                    {

                        ///////////////////////////////UPDATE EXISTING OBD ///////////////////////////////////////

                        if (OBDTrack.UpdateOBDTracking_Initiator())
                        {
                            // Audit.LogTranscationAudit(cp.UserID.ToString(), 2, 2, OBDTrack.OBDTrackingID, 1);
                        }
                        else
                        {
                            Response.Redirect("OutboundDetails.aspx?obdid=" + OBDTrack.OBDTrackingID.ToString() + "&obdupdate=dbupdateerr");
                        }


                        if (fucOBDDeliveryNote.HasFile)
                        {
                            //String FileExtenion = Path.GetExtension(fucOBDDeliveryNote.FileName);

                            if (FileExtenion == ".pdf")
                            {
                                if (CommonLogic.UploadFile(TenantRootDir + hifTenant.Value + OutboundPath + OBD_DeliveryNotePath, fucOBDDeliveryNote, OBDTrack.OBDNumber + Path.GetExtension(fucOBDDeliveryNote.FileName)) == false)
                                {
                                    //Error in uploading the delivery note file
                                    Response.Redirect("OutboundDetails.aspx?obdid=" + OBDTrack.OBDTrackingID.ToString() + "&obdupdate=okfileuploaderr");
                                }
                                else
                                {
                                    Response.Redirect("OutboundDetails.aspx?obdid=" + OBDTrack.OBDTrackingID.ToString() + "&obdupdate=updateok");
                                }
                            }
                            else
                            {
                                lblStatusMessage.Text = "Please attach a valid file";
                            }
                        }
                        else
                        {
                            Response.Redirect("OutboundDetails.aspx?obdid=" + OBDTrack.OBDTrackingID.ToString() + "&obdupdate=updateok");
                        }

                    }

                }
                catch (Exception ex)
                {
                    CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
                    resetError("Error while updating OBD details", true);
                    return;

                }
            }
            catch (Exception es)
            {
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), es.Source, es.Message, es.StackTrace);
                resetError("Error while updating OBD details", true);
                return;
            }


        }

        protected void btnGDRPrint_Click(object sender, ImageClickEventArgs e)
        {

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

        #endregion ----- Initiate OBD ------


        #region ---- GDR Grid View -----

        protected void lnkAddNew_Click(object sender, EventArgs e)
        {

        }

        protected void gvGDRRecords_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {

        }

        protected void gvGDRRecords_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }

        protected void gvGDRRecords_Sorting(object sender, GridViewSortEventArgs e)
        {

        }

        protected void gvGDRRecords_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

        }

        protected void gvGDRRecords_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {

        }

        protected void gvGDRRecords_RowEditing(object sender, GridViewEditEventArgs e)
        {

        }

        #endregion ---- GDR Grid View -----


        #region ------- SO Line Items  ---------- 

        protected DataSet GVSOLineItems_buildGridData()
        {
            string sql = ViewState["SOLineItemsSQL"].ToString();
            DataSet ds = DB.GetDS(sql, false);

            return ds;
        }

        protected void GVSOLineItems_buildGridData(DataSet ds)
        {
            gvSOLineItems.DataSource = ds.Tables[0];
            gvSOLineItems.DataBind();
            ds.Dispose();
        }

        protected void lnkAddDelvDocLineItem_Click(object sender, EventArgs e)
        {



            //if (!cp.IsInAnyRoles("3"))
            //{

            //    resetError("Only OB-Initiator can access outbound delivery", true);
            //    return;

            //}

            if (OBDTrack.DeliveryStatusID == 4)
            {
                resetError("Outbound is already delivered", true);
                return;
            }

            ViewState["gvSOIsInsert"] = false;

            StringBuilder sql = new StringBuilder(2500);

            /*
            sql.Append("INSERT INTO OBD_Outbound_ORD_CustomerPO (OutboundID,CreatedBy) values (");
            sql.Append(OBDTrack.OBDTrackingID.ToString() + "," + CreatedBy );
            sql.Append(")");*/



            try
            {
                /*
                DB.ExecuteSQL(sql.ToString());

                gvSOLineItems.EditIndex = gvSOLineItems.Rows.Count;
                 */

                //IDataReader dr = DB.GetRS("select * from INV_GoodsMovementDetails where TransactionDocID="+CommonLogic.QueryString("obdid")+" AND GoodsMovementTypeID=2 AND IsDeleted=0");
                //if (dr.Read())
                //{
                //    resetError("Cannot modify the delivery doc details, as 'Goods-Out' process is started", true);
                //    return;
                //}

                //dr.Close();

                //<!----------Procedure Converting--------------------->
                // if (DB.GetSqlN("select count(*) N from INV_GoodsMovementDetails where TransactionDocID=" + CommonLogic.QueryString("obdid") + " AND GoodsMovementTypeID=2 AND IsDeleted=0") > 0)
                if (DB.GetSqlN(" [dbo].[USP_GetCountGoodsMovementdata_OBDID] @OutboundID = " + CommonLogic.QueryString("obdid")) > 0)

                {
                    resetError("Cannot modify the delivery doc details, as 'Goods-Out' process is started", true);
                    return;
                }


                gvSOLineItems.EditIndex = 0;
                gvSOLineItems.PageIndex = 0;
                DataSet dsSOLineItems = this.GVSOLineItems_buildGridData();
                DataRow row = dsSOLineItems.Tables[0].NewRow();
                row["SONumber"] = "";
                row["Outbound_CustomerPOID"] = 0;
                row["SOHeaderID"] = 0;
                dsSOLineItems.Tables[0].Rows.InsertAt(row, 0);


                this.GVSOLineItems_buildGridData(dsSOLineItems);

                //this.resetError("New Work Order line item added", false);

                ViewState["gvSOIsInsert"] = true;
                ViewState["Not Updated"] = true;

                lnkAddDelvDocLineItem.Visible = false;

            }
            catch (Exception ex)
            {
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
                this.resetError("Error while inserting new record", true);

            }



        }

        protected void gvSOLineItems_Sorting(object sender, GridViewSortEventArgs e)
        {

        }

        protected void gvSOLineItems_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

            ViewState["gvSOIsInsert"] = false;

            gvSOLineItems.PageIndex = e.NewPageIndex;
            gvSOLineItems.EditIndex = -1;
            this.GVSOLineItems_buildGridData(this.GVSOLineItems_buildGridData());

        }

        protected void gvSOLineItems_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.DataItem == null)
                return;

            /*
            if (OBDTrack.DeliveryStatusID == 4)
            {
                //e.Row.Cells[7].Enabled = false;
                //e.Row.Enabled = false;

               // ((LinkButton)gvSOLineItems.Rows[e.Row.RowIndex].Cells[0].Controls[0]).Enabled = false;
                //LinkButton EditButton = (LinkButton)e.Row.Cells[7].Controls[0];
                //EditButton.Enabled = false;

                //((CommandField)gvSOLineItems.Columns[7]).Visible = false;

              //  BoundField bound = gvSOLineItems.Columns[7] as BoundField;
                //bound.InsertVisible = false;
                //bound.ReadOnly = true;
                
             }

             */
            if ((e.Row.RowState & DataControlRowState.Edit) == DataControlRowState.Edit)
            {
                TextBox txtSONumber = (TextBox)e.Row.FindControl("txtSONumber");
                txtSONumber.Focus();

                if (ddlDeliveryDocType.SelectedValue == "8")
                {
                    RequiredFieldValidator rfv = (RequiredFieldValidator)e.Row.FindControl("rfvtxtCustPONumber");
                    rfv.Enabled = false;
                }
                DataRow row = ((DataRowView)e.Row.DataItem).Row;
                hifSOHeaderID.Value = row["SOHeaderID"].ToString();

                hifActualCustomerPOID.Value = row["CustomerPOID"].ToString();
                hifActualInvoice.Value = row["InvoiceNo"].ToString();

            }

            if ((e.Row.RowState & DataControlRowState.Edit) != DataControlRowState.Edit)
            {
                //hifActualCustomerPOID.Value = "0";
                //hifActualInvoice.Value = "";

                // IDataReader dr = DB.GetRS("select * from INV_GoodsMovementDetails where TransactionDocID="+CommonLogic.QueryString("obdid")+" AND GoodsMovementTypeID=2 AND IsDeleted=0");
                IDataReader dr = DB.GetRS(" [dbo].[USP_GetGoodsMovementdata_OBDID] @OutboundID = " + CommonLogic.QueryString("obdid"));


                if (dr.Read())
                    e.Row.Controls[9].Controls[0].Visible = false;

                dr.Close();
            }

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //if (txtOBDNumber.Text.StartsWith("TM"))
                //{
                //    Literal SOHID = (Literal)e.Row.FindControl("ltSOHeaderID");
                //    HyperLink HypList = (HyperLink)e.Row.FindControl("hypSOTMList");
                //    HypList.NavigateUrl = "../mOrders/ToolManagement.aspx?tmoid=" + SOHID.Text;
                //}
            }


        }

        protected bool gvEditButtonStatus()
        {
            if (OBDTrack.DeliveryStatusID == 4)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        protected void gvSOLineItems_RowCommand(object sender, GridViewCommandEventArgs e)
        {



            if (e.CommandName == "DeleteItem")
            {
                ViewState["gvSOIsInsert"] = false;
                gvSOLineItems.EditIndex = -1;
                int iden = Localization.ParseNativeInt(e.CommandArgument.ToString());
                this.deleteRowPermSO(iden);
            }
        }

        protected void gvSOLineItems_RowEditing(object sender, GridViewEditEventArgs e)
        {

            ViewState["gvSOIsInsert"] = false;

            gvSOLineItems.EditIndex = e.NewEditIndex;

            ViewState["Not Updated"] = false;

            this.GVSOLineItems_buildGridData(this.GVSOLineItems_buildGridData());

            lnkAddDelvDocLineItem.Visible = false;


        }

        protected void gvSOLineItems_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {

            //if (!cp.IsInAnyRoles("3"))
            //{

            //    resetError("Only OB-Initiator can access outbound delivery", true);
            //    return;

            //}

            if (OBDTrack.DeliveryStatusID == 4)
            {
                resetError("Outbound is already delivered", true);
                return;
            }

            //if (OBDProductionStatus != 0)
            //{
            //    resetError("Cannot modify the details, as production process is started", true);
            //    return;
            //}

            ViewState["gvSOIsInsert"] = false;


            GridViewRow row = gvSOLineItems.Rows[e.RowIndex];
            TextBox txtsono = (TextBox)row.FindControl("txtSONumber");
            TextBox txtcustpono = (TextBox)row.FindControl("txtCustPONumber");
            TextBox txtinvno = (TextBox)row.FindControl("txtInvoiceNumber");

            if (txtsono.Text == "")
            {
                resetError("Please Select SO No.", true);
                txtsono.Focus();
                return;
            }

            if (txtcustpono.Text == "")
            {
                resetError("Please Select Cust PO No.", true);
                txtcustpono.Focus();
                return;
            }

            if (txtinvno.Text == "")
            {
                resetError("Please Select Invoice No.", true);
                txtinvno.Focus();
                return;
            }


            if (row != null)
            {

                string txtSONumber = ((TextBox)row.FindControl("txtSONumber")).Text;
                string txtCustPONumber = ((TextBox)row.FindControl("txtCustPONumber")).Text;
                string txtInvoiceNumber = ((TextBox)row.FindControl("txtInvoiceNumber")).Text;
                string lthidOutbound_CustomerPOID = ((Literal)row.FindControl("lthidOutbound_CustomerPOID")).Text;
                string ltHifInvoiceNumber = ((HiddenField)row.FindControl("vhifInvoiceNumber")).Value;

                if (!txtInvoiceNumber.Equals(ltHifInvoiceNumber))
                {
                    this.resetError("Invalid Invoice Number", true);
                    return;
                }

                //int SOHeaderID = DB.GetSqlN("select SOHeaderID AS N from ORD_SOHeader where SOStatusID=1 AND  IsActive=1 and IsDeleted=0  and TenantID=" + hifTenant.Value + "  and SONumber=" + DB.SQuote(txtSONumber));


                //if (SOHeaderID == 0)
                //{
                //    this.resetError("SO Number does not exist", true);
                //    ViewState["Not Updated"] = true;
                //    return;
                //}


                //if (DB.GetSqlN("select COUNT(*) AS N from ORD_SODetails where IsActive=1 AND IsDeleted=0 AND SOHeaderID=" + SOHeaderID) == 0)
                //{
                //    this.resetError("No SO line item is configured to this customer po", true);
                //    return;
                //}


                //int CustPOID = 0;

                //if (ddlDeliveryDocType.SelectedValue != "8")
                //{

                //     CustPOID = DB.GetSqlN("select CustomerPOID AS N from ORD_CustomerPO where IsActive=1 and IsDeleted=0 and CustPONumber=" + DB.SQuote(txtCustPONumber) + " and SOHeaderID=" + SOHeaderID);
                //    if (CustPOID == 0)
                //    {
                //        this.resetError("Customer PO Number does not exist", true);
                //        ViewState["Not Updated"] = true;
                //        return;
                //    }
                //}


                //if (DB.GetSqlN("select Outbound_CustomerPOID N from OBD_Outbound_ORD_CustomerPO where OutboundID=" + CommonLogic.QueryString("obdid") + " and IsDeleted=0 and SOHeaderID=" + hifSOHeaderID.Value + " and CustomerPOID=" + CustPOID + " and Outbound_CustomerPOID!=" + lthidOutbound_CustomerPOID + " and InvoiceNo=" + DB.SQuote(txtInvoiceNumber)) != 0)
                //{
                //    this.resetError("Invoice Number already exists", true);
                //    return;
                //}




                //StringBuilder sqlPOInvDetails = new StringBuilder(2500);

                //sqlPOInvDetails.Append(" DECLARE @NewUpdateoutboundSOID int;  EXEC [dbo].[sp_OBD_UpsertOutbound_CustomerPO]   @InvoiceNumber='"+txtInvoiceNumber+"',@Outbound_CustomerPOID=" + lthidOutbound_CustomerPOID + ",@OutboundID=" + OBDTrack.OBDTrackingID + ",@CustomerPOID=" + (CustPOID.ToString() == "0" ? "NULL" : CustPOID.ToString()) + ",@SOHeaderID=" + SOHeaderID + ",@CreatedBy=" + CreatedBy + ",@NewOutbound_CustomerPOID=@NewUpdateoutboundSOID OUTPUT; select @NewUpdateoutboundSOID AS N");


                StringBuilder sqlPOInvDetails = new StringBuilder(2500);

                sqlPOInvDetails.Append("DECLARE @NewUpdateoutboundSOID int; ");
                sqlPOInvDetails.Append("EXEC [dbo].[sp_OBD_UpsertOutbound_CustomerPO_New]  ");
                sqlPOInvDetails.Append("@OutboundID=" + OBDTrack.OBDTrackingID + ",");
                sqlPOInvDetails.Append("@SONumber=" + (txtSONumber == "" ? "NULL" : DB.SQuote(txtSONumber.ToString())) + ",");
                sqlPOInvDetails.Append("@CustomerPONumber=" + (txtCustPONumber.ToString() == "" ? "NULL" : DB.SQuote(txtCustPONumber)) + ",");
                sqlPOInvDetails.Append("@InvoiceNumber=" + (txtInvoiceNumber == "" ? "NULL" : DB.SQuote(txtInvoiceNumber)) + ",");
                sqlPOInvDetails.Append("@Outbound_CustomerPOID=" + lthidOutbound_CustomerPOID + ",");
                sqlPOInvDetails.Append("@CreatedBy=" + cp.UserID + ",");
                sqlPOInvDetails.Append("@TenantID=" + hifTenant.Value + ",");
                sqlPOInvDetails.Append("@NewOutbound_CustomerPOID=@NewUpdateoutboundSOID OUTPUT;  ");
                sqlPOInvDetails.Append("select @NewUpdateoutboundSOID AS N");


                try
                {
                    //DB.ExecuteSQL(sqlPOInvDetails.ToString());

                    int Result = DB.GetSqlN(sqlPOInvDetails.ToString());

                    if (Result == -1)
                    {
                        this.resetError("SO Number does not exist", true);
                        ViewState["Not Updated"] = true;
                        return;
                    }

                    else if (Result == -2)
                    {
                        this.resetError("Error while updating", true);
                        return;
                    }

                    else if (Result == -3)
                    {
                        this.resetError("No SO line item is configured to this customer po", true);
                        return;
                    }

                    else if (Result == -4)
                    {
                        this.resetError("Customer PO Number does not exist", true);
                        ViewState["Not Updated"] = true;
                        return;
                    }
                    else if (Result == -5)
                    {
                        this.resetError("Invoice Number already exists", true);
                        return;
                    }

                    else if (Result == -6)
                    {
                        this.resetError("Duplicate Invoice number", true);
                        return;
                    }

                    this.resetError("Successfully Updated", false);

                    gvSOLineItems.EditIndex = -1;

                    this.GVSOLineItems_buildGridData(this.GVSOLineItems_buildGridData());

                    ViewState["Not Updated"] = false;

                    lnkAddDelvDocLineItem.Visible = true;
                }
                catch (SqlException sqlex)
                {
                    CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), sqlex.Source, sqlex.Message, sqlex.StackTrace);
                    if (sqlex.ErrorCode == -2146232060)
                    {
                        ViewState["Not Updated"] = true;
                        this.resetError("SO number is already updated", true);
                    }
                }
                catch (Exception ex)
                {
                    ViewState["Not Updated"] = true;
                    CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
                    this.resetError("Error while updating work order details", true);
                }



            }


        }

        protected void gvSOLineItems_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            /*
            if (Localization.ParseBoolean(ViewState["gvSOIsInsert"].ToString()) )
            {
                GridViewRow row = gvSOLineItems.Rows[e.RowIndex];
                if (row != null)
                {
                    int iden = Convert.ToInt32(((Literal)row.FindControl("lthidOutbound_CustomerPOID")).Text);
                    deleteRowPermSO(iden);
                    resetError("Line item canceled", true);
                    lnkAddDelvDocLineItem.Visible = true;
                }
            }

            ViewState["gvSOIsInsert"] = false;
            */
            gvSOLineItems.EditIndex = -1;
            this.GVSOLineItems_buildGridData(this.GVSOLineItems_buildGridData());

            lnkAddDelvDocLineItem.Visible = true;


        }

        protected void lnkDeleteRFItem_Click(object sender, EventArgs e)
        {

            //if (!cp.IsInAnyRoles("3"))
            //{

            //    resetError("Only OB-Initiator can access the outbound delivery", true);
            //    return;

            //}
            if (OBDTrack.DeliveryStatusID == 4)
            {
                resetError("Outbound is already delivered", true);
                return;
            }
            if (OBDTrack.DeliveryStatusID >= 2)
            {
                this.GVSOLineItems_buildGridData(this.GVSOLineItems_buildGridData());
                resetError("Outbound is already released", true);
                return;
            }



            //if (OBDProductionStatus != 0)
            //{
            //    resetError("Cannot modify the details, as production process is started", true);
            //    return;
            //}

            //<!-----------Procedure Converting----------------->
            //IDataReader dr = DB.GetRS("select * from INV_GoodsMovementDetails where TransactionDocID=" + CommonLogic.QueryString("obdid") + " AND GoodsMovementTypeID=2 AND IsDeleted=0");
            IDataReader dr = DB.GetRS("Exec [dbo].[USP_GetGoodsMovementdata_OBDID] @OutboundID = " + CommonLogic.QueryString("obdid"));


            if (dr.Read())
            {
                resetError("Cannot modify the delivery doc. details, as 'Goods-OUT' process is started", true);
                return;
            }

            dr.Close();


            string rfidIDs = "";

            bool chkBox = false;

            StringBuilder sqlDeleteString = new StringBuilder();
            sqlDeleteString.Append("BEGIN TRAN ");

            //'Navigate through each row in the GridView for checkbox items
            foreach (GridViewRow gv in gvSOLineItems.Rows)
            {

                CheckBox isDelete = (CheckBox)gv.FindControl("chkIsDeleteRFItem");

                if (isDelete == null)
                    return;

                if (isDelete.Checked)
                {
                    chkBox = true;
                    // Concatenate GridView items with comma for SQL Delete
                    rfidIDs = ((Literal)gv.FindControl("lthidOutbound_CustomerPOID")).Text.ToString();
                    string SOHID = ((Literal)gv.FindControl("ltSOHeaderID")).Text.ToString();


                    if (rfidIDs != "")
                        //sqlDeleteString.Append(" Delete from OBD_Outbound_ORD_CustomerPO Where Outbound_CustomerPOID=" + rfidIDs + ";");
                        sqlDeleteString.Append(" UPDATE OBD_Outbound_ORD_CustomerPO SET IsActive=0,IsDeleted=1 WHERE SOHeaderID =" + SOHID + ";");

                }
            }


            sqlDeleteString.Append(" COMMIT ");


            // Execute SQL Query only if checkboxes are checked to avoid any error with initial null string
            try
            {

                if (chkBox)
                {
                    DB.ExecuteSQL(sqlDeleteString.ToString());
                    resetError("Successfully deleted the selected line items", false);
                }


            }
            catch (Exception ex)
            {
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
                resetError("Error while deleting selected line items", true);
            }

            this.GVSOLineItems_buildGridData(this.GVSOLineItems_buildGridData());


        }


        protected void deleteRowPermSO(int iden)
        {
            StringBuilder sql = new StringBuilder(2500);
            sql.Append("DELETE from OBD_Outbound_ORD_CustomerPO where Outbound_CustomerPOID=" + iden.ToString());
            try
            {
                DB.ExecuteSQL(sql.ToString());
                this.GVSOLineItems_buildGridData(this.GVSOLineItems_buildGridData());

            }
            catch (Exception ex)
            {
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
                this.resetError("Error while deleting SO line items", true);
            }
        }


        #endregion ------- SO Line Items  ----------



        #region ------- Prod Order Line Items Line Items  ----------

        protected DataSet gvProdOrderLineItems_buildGridData()
        {
            string sql = ViewState["ProLineItemsSQL"].ToString();
            DataSet ds = DB.GetDS(sql, false);

            return ds;
        }


        protected void gvProdOrderLineItems_buildGridData(DataSet ds)
        {
            gvProdOrder.DataSource = ds.Tables[0];
            gvProdOrder.DataBind();
            ds.Dispose();
        }

        protected void lnkAddProdDelvDocLineItem_Click(object sender, EventArgs e)
        {

            resetErrorProdOrderLineItems("", false);

            //if (!cp.IsInAnyRoles("3"))
            //{

            //    resetErrorProdOrderLineItems("Only OB-Initiator can access the outbound delivery", true);
            //    return;

            //}

            if (OBDTrack.DeliveryStatusID == 4)
            {
                resetErrorProdOrderLineItems("Sorry, This delivery is already done", true);
                return;
            }

            ViewState["gvProOrderIsInsert"] = false;

            StringBuilder sql = new StringBuilder(2500);

            sql.Append("INSERT INTO OBD_Outbound_MFG_ProductionOrderHeader (OutboundID,CreatedBy) values (");
            sql.Append(OBDTrack.OBDTrackingID.ToString() + "," + CreatedBy);
            sql.Append(")");

            try
            {
                DB.ExecuteSQL(sql.ToString());

                gvProdOrder.EditIndex = gvSOLineItems.Rows.Count;

                this.gvProdOrderLineItems_buildGridData(this.gvProdOrderLineItems_buildGridData());

                this.resetErrorProdOrderLineItems("New request form  added.", false);

                ViewState["gvProOrderIsInsert"] = true;
                ViewState["Not Updated"] = true;

                lnkAddProdDelvDocLineItem.Visible = false;

            }
            catch (Exception ex)
            {
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
                this.resetErrorProdOrderLineItems("Error while inserting new record : " + ex.ToString(), true);

            }



        }


        protected void gvProdOrderLineItems_Sorting(object sender, GridViewSortEventArgs e)
        {

        }

        protected void gvProdOrderLineItems_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

            ViewState["gvProOrderIsInsert"] = false;
            this.resetErrorProdOrderLineItems("", false);
            gvProdOrder.PageIndex = e.NewPageIndex;
            gvProdOrder.EditIndex = -1;
            this.gvProdOrderLineItems_buildGridData(this.gvProdOrderLineItems_buildGridData());

        }

        protected void gvProdOrderLineItems_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.DataItem == null)
                return;

            /*
            if (OBDTrack.DeliveryStatusID == 4)
            {
                //e.Row.Cells[7].Enabled = false;
                //e.Row.Enabled = false;

               // ((LinkButton)gvSOLineItems.Rows[e.Row.RowIndex].Cells[0].Controls[0]).Enabled = false;
                //LinkButton EditButton = (LinkButton)e.Row.Cells[7].Controls[0];
                //EditButton.Enabled = false;

                //((CommandField)gvSOLineItems.Columns[7]).Visible = false;

              //  BoundField bound = gvSOLineItems.Columns[7] as BoundField;
                //bound.InsertVisible = false;
                //bound.ReadOnly = true;
                
             }

             */
            if ((e.Row.RowState & DataControlRowState.Edit) == DataControlRowState.Edit)
            {
                TextBox txtSONumber = (TextBox)e.Row.FindControl("txtPRONumber");
                txtSONumber.Focus();
            }

        }

        protected bool gvProOrderEditButtonStatus()
        {
            if (OBDTrack.DeliveryStatusID == 4)
            {
                return false;
            }
            else
            {
                return true;
            }
        }



        protected void gvProdOrderLineItems_RowCommand(object sender, GridViewCommandEventArgs e)
        {

            this.resetErrorProdOrderLineItems("", false);

            if (e.CommandName == "DeleteItem")
            {
                ViewState["gvProOrderIsInsert"] = false;
                gvProdOrder.EditIndex = -1;
                int iden = Localization.ParseNativeInt(e.CommandArgument.ToString());
                this.deleteRowPermProdOrder(iden);
            }
        }

        protected void gvProdOrderLineItems_RowEditing(object sender, GridViewEditEventArgs e)
        {

            ViewState["gvProOrderIsInsert"] = false;

            gvProdOrder.EditIndex = e.NewEditIndex;

            ViewState["Not Updated"] = false;

            this.gvProdOrderLineItems_buildGridData(this.gvProdOrderLineItems_buildGridData());

            lnkAddProdDelvDocLineItem.Visible = false;


        }

        protected void gvProdOrderLineItems_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {

            //if (!cp.IsInAnyRoles("3"))
            //{

            //    resetErrorProdOrderLineItems("Only OB-Initiator can access the outbound delivery", true);
            //    return;

            //}

            if (OBDTrack.DeliveryStatusID == 4)
            {
                resetErrorProdOrderLineItems("Sorry, This delivery is already done", true);
                return;
            }

            ViewState["gvProOrderIsInsert"] = false;
            GridViewRow row = gvProdOrder.Rows[e.RowIndex];

            if (row != null)
            {

                string txtPRONumber = ((TextBox)row.FindControl("txtPRONumber")).Text;
                //string txtCustPONumber = ((TextBox)row.FindControl("txtPRONumber")).Text;
                string lthidOutbound_ProductionOrderHeaderID = ((Literal)row.FindControl("lthidOutbound_ProductionOrderHeaderID")).Text;

                int ProdHeaderID = DB.GetSqlN("select ProductionOrderHeaderID AS N from MFG_ProductionOrderHeader where IsActive=1 and IsDeleted=0  and TenantID=" + TenantID + "  and PRORefNo=" + DB.SQuote(txtPRONumber));

                //int CustPOID = DB.GetSqlN("select CustomerPOID AS N from ORD_CustomerPO where IsActive=1 and IsDeleted=0 and CustPONumber=" + DB.SQuote(txtCustPONumber) + " and ProdHeaderID=" + ProdHeaderID);

                if (ProdHeaderID == 0)
                {
                    this.resetErrorProdOrderLineItems("Invalid Production Order Number", true);
                    ViewState["Not Updated"] = true;
                    return;
                }

                /*
                if (CustPOID == 0)
                {
                    this.resetErrorProdOrderLineItems("Invalid Customer PO Number", true);
                    ViewState["Not Updated"] = true;
                    return;
                }*/


                StringBuilder sqlPOInvDetails = new StringBuilder(2500);

                sqlPOInvDetails.Append("UPDATE OBD_Outbound_MFG_ProductionOrderHeader SET ");
                sqlPOInvDetails.Append("ProductionOrderHeaderID=" + ProdHeaderID);


                sqlPOInvDetails.Append("  WHERE Outbound_ProductionOrderHeaderID=" + lthidOutbound_ProductionOrderHeaderID);


                try
                {
                    DB.ExecuteSQL(sqlPOInvDetails.ToString());

                    this.resetErrorProdOrderLineItems("Item updated", false);

                    gvProdOrder.EditIndex = -1;

                    this.gvProdOrderLineItems_buildGridData(this.gvProdOrderLineItems_buildGridData());

                    ViewState["Not Updated"] = false;

                    lnkAddProdDelvDocLineItem.Visible = true;
                }
                catch (Exception ex)
                {
                    ViewState["Not Updated"] = true;
                    CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
                    this.resetErrorProdOrderLineItems("Cannot update, please check for required fields", true);
                }



            }


        }

        protected void gvProdOrderLineItems_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {

            if (Localization.ParseBoolean(ViewState["gvProOrderIsInsert"].ToString()))
            {
                GridViewRow row = gvProdOrder.Rows[e.RowIndex];
                if (row != null)
                {
                    int iden = Convert.ToInt32(((Literal)row.FindControl("lthidOutbound_ProductionOrderHeaderID")).Text);
                    deleteRowPermProdOrder(iden);
                    resetErrorProdOrderLineItems("Line item canceled", true);
                    lnkAddProdDelvDocLineItem.Visible = true;
                }
            }

            ViewState["gvProOrderIsInsert"] = false;

            gvProdOrder.EditIndex = -1;
            this.gvProdOrderLineItems_buildGridData(this.gvProdOrderLineItems_buildGridData());

            lnkAddProdDelvDocLineItem.Visible = true;


        }

        protected void lnkDeleteProdOrderItem_Click(object sender, EventArgs e)
        {

            //if (!cp.IsInAnyRoles("3"))
            //{

            //    resetErrorProdOrderLineItems("Only OB-Initiator can access the outbound delivery", true);
            //    return;

            //}

            if (OBDTrack.DeliveryStatusID == 4)
            {
                resetErrorProdOrderLineItems("Outbound is already delivered", true);
                return;
            }

            string rfidIDs = "";

            bool chkBox = false;

            StringBuilder sqlDeleteString = new StringBuilder();
            sqlDeleteString.Append("BEGIN TRAN ");

            //'Navigate through each row in the GridView for checkbox items
            foreach (GridViewRow gv in gvProdOrder.Rows)
            {

                CheckBox isDelete = (CheckBox)gv.FindControl("chkIsDeleteRFItem");
                if (isDelete.Checked)
                {
                    chkBox = true;
                    // Concatenate GridView items with comma for SQL Delete
                    rfidIDs = ((Literal)gv.FindControl("lthidOutbound_ProductionOrderHeaderID")).Text.ToString();



                    if (rfidIDs != "")
                        sqlDeleteString.Append(" Delete from OBD_Outbound_MFG_ProductionOrderHeader Where Outbound_ProductionOrderHeaderID=" + rfidIDs + ";");

                }
            }


            sqlDeleteString.Append(" COMMIT ");


            // Execute SQL Query only if checkboxes are checked to avoid any error with initial null string
            try
            {

                if (chkBox)
                {
                    DB.ExecuteSQL(sqlDeleteString.ToString());
                }
                resetErrorProdOrderLineItems("Successfully deleted the selected items", false);

            }
            catch (Exception ex)
            {
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
                resetErrorProdOrderLineItems("Error updating data" + ex.ToString(), true);
            }

            this.gvProdOrderLineItems_buildGridData(this.gvProdOrderLineItems_buildGridData());


        }

        protected void resetErrorProdOrderLineItems(string error, bool isError)
        {

            string str = "<font class=\"noticeMsg\">NOTICE:</font>&nbsp;&nbsp;&nbsp;";
            if (isError)
                str = "<font class=\"errorMsg\">ERROR:</font>&nbsp;&nbsp;&nbsp;";

            if (error.Length > 0)
                str += error + "";
            else
                str = "";
            lblGvStatus.Text = str;

        }

        protected void deleteRowPermProdOrder(int iden)
        {
            StringBuilder sql = new StringBuilder(2500);
            sql.Append("DELETE from OBD_Outbound_MFG_ProductionOrderHeader where Outbound_ProductionOrderHeaderID=" + iden.ToString());
            try
            {
                DB.ExecuteSQL(sql.ToString());
                this.gvProdOrderLineItems_buildGridData(this.gvProdOrderLineItems_buildGridData());

            }
            catch (Exception ex)
            {
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
                this.resetErrorProdOrderLineItems("Couldn't delete from database: " + ex.ToString(), true);
            }
        }


        #endregion ------- Prod Order Line Items  ----------



        #region  ---- Pick N Check -----


        protected void LoadWHStores()
        {

            try
            {
                ddlStores.Items.Clear();

                //IDataReader WHReader = DB.GetRS("select WHCode,INB_R.WarehouseID from OBD_RefWarehouse_Details INB_R Left Join GEN_Warehouse GEN_W on GEN_W.WarehouseID=INB_R.WarehouseID where INB_R.IsDeleted=0 AND INB_R.IsActive=1 and  GEN_W.isActive=1 AND GEN_W.isDeleted=0 AND OutboundID=" + OBDTrack.OBDTrackingID.ToString());
                IDataReader WHReader = DB.GetRS("Exec  USP_OBDWareHouseDropDown @OutboundID=" + OBDTrack.OBDTrackingID.ToString());

                ddlStores.Items.Add(new ListItem("Select Store", "0"));

                while (WHReader.Read())
                {
                    // if ((DB.GetSqlN("select UserTypeID as N from gen_user where userid=" + cp.UserID)) == 1)
                    if ((DB.GetSqlN(" [dbo].[USP_GetUserTypeByID] @UserID = " + cp.UserID)) == 1)

                    {
                        ddlStores.Items.Add(new ListItem(WHReader["WHCode"].ToString(), WHReader["WarehouseID"].ToString()));
                    }
                    else
                    {
                        if (cp.IsInWarehouse(WHReader["WarehouseID"].ToString()))
                        {
                            ddlStores.Items.Add(new ListItem(WHReader["WHCode"].ToString(), WHReader["WarehouseID"].ToString()));

                        }
                    }

                }

                WHReader.Close();
            }
            catch (Exception ex)
            {
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
                resetError("Error while loading warehouse list", true);
                return;
            }

        }

        protected void LoadPickNCheckDetails(int REF_WHID, int WHID)
        {

            if (OBDTrack.DeliveryStatusID >= 4)
                lnkCancelSendToPGI.Visible = false;



            OBDTrack.LoadFromDB(OBDTrack.OBDTrackingID, REF_WHID);

            ddlStores.SelectedValue = WHID.ToString();

            atcCheckedBy.Text = CommonLogic.GetUserName(OBDTrack.CheckedBy);
            hifCheckedByID.Value = OBDTrack.CheckedBy;
            atcPickedBy.Text = CommonLogic.GetUserName(OBDTrack.PickedBy.ToString());
            hifPickedByID.Value = OBDTrack.PickedBy.ToString();
            txtAutoPGIDate.Text = OBDTrack.PGIDoneDate == null || OBDTrack.PGIDoneDate == "" ? "" : Convert.ToDateTime(OBDTrack.PGIDoneDate).ToString("dd-MMM-yyyy");
            txtAutoPGITime.Text = OBDTrack.PGIDoneDate == null || OBDTrack.PGIDoneDate == "" ? "" : Convert.ToDateTime(OBDTrack.PGIDoneDate).ToString("HH:mm");

            txtRemarks_StoreIncharge.Text = OBDTrack.RemBy_StoreIncharge;
            // txtNoOfLines.Text = CommonLogic.IIF(OBDTrack.NoofLines.ToString()=="0","",OBDTrack.NoofLines.ToString());
            //txtTotalQuantity.Text = CommonLogic.IIF(OBDTrack.TotalQuantity.ToString() == "0", "", OBDTrack.TotalQuantity.ToString());

            if (OBDTrack.OBDReceivedDT != null)
            {
                txtOBDRcvdDate.Text = Convert.ToDateTime(OBDTrack.OBDReceivedDT).ToString("dd-MMM-yyyy");
                //  OBDRecvdTimeEntry.Text = Convert.ToDateTime(OBDTrack.SentForPGI_DT).ToString("hh:mm tt");
            }


            if (ddlStores.SelectedItem.Text == "Select Store")
            {
                lblStoreName.Text = "";
                lblDeliveryStore.Text = "";
            }
            else
                lblStoreName.Text = ddlStores.SelectedItem.Text;

            if (OBDTrack.DocumentTypeID != 6 && OBDTrack.DocumentTypeID != 9)
            {
                hylPickNote.NavigateUrl = "DeliveryPickNote.aspx?obdid=" + OBDTrack.OBDTrackingID + "&TN=" + txtTenant.Text;
            }
            else
            {
                hylPickNote.NavigateUrl = "~/mManufacturingProcess/DeliveryPickNote.aspx?obdid=" + OBDTrack.OBDTrackingID + "&TN=" + txtTenant.Text;
            }


            String sFileName = CommonLogic._GetAttatchmentFile(TenantRootDir + hifTenant.Value + OutboundPath + OBD_PickandCheckSheetPath, OBDTrack.OBDNumber.ToString() + "_" + WHID.ToString());


            if (sFileName != "")
            {
                String Path = "../ViewImage.aspx?path=" + sFileName;
                ltPickCheckAttachments.Text = "<img src=\"../Images/blue_menu_icons/attachment.png\"  />";
                ltPickCheckAttachments.Text += "<a style=\"text-decoration:none;\" href=\"#\" onclick=\" OpenImage(' " + Path + " ')  \" > View Delivery Note Doc </a>";
                ltPickCheckAttachments.Text += "<img src=\"../Images/redarrowright.gif\"   />";
            }
            else
            {
                ltPickCheckAttachments.Text = "";
            }


        }

        protected void lnkCancelSendToPGI_Click(object sender, EventArgs e)
        {
            //if (!cp.IsInAnyRoles("4"))
            //{

            //    resetError("Only OB Store In-charge can access this shipment", true);
            //    return;

            //}


            ddlStores.SelectedValue = "0";
            // txtTotalQuantity.Text = "";
            //txtNoOfLines.Text = "";
            atcPickedBy.Text = "";
            atcCheckedBy.Text = "";
            txtRemarks_StoreIncharge.Text = "";
            txtOBDRcvdDate.Text = "";
            OBDRecvdTimeEntry.Text = "";


        }

        protected void lnkSendToPGI_Click(object sender, EventArgs e)
        {

            //if (!cp.IsInAnyRoles("4"))
            //{
            //    resetError("Only OB Store In-charge can access this shipment", true);
            //    return;
            //}


            DataSet ds = DB.GetDS("EXEC [sp_INV_GetPendingGoodsOutList_NEW] @OutboundID=" + OBDTrack.OBDTrackingID, false);
            if (ds != null)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    resetError("Please pick the Items", true);
                    return;
                }
            }


            Page.Validate("recieveOBD");

            if (Page.IsValid == false)
            {
                resetError("Please check for mandatory fields", true);
                return;
            }

            if (txtAutoPGIDate.Text == "")
            {
                resetError("Please check for mandatory fields", true);
                return;
            }


            if (txtAutoPGITime.Text == "")
            {
                resetError("Please check for mandatory fields", true);
                return;
            }


            int IsOBDpiking = DB.GetSqlTinyInt("SELECT IsVLPDPicking AS TI FROM OBD_Outbound WHERE OutboundID = " + OBDTrack.OBDTrackingID + "");
            if (IsOBDpiking == 1)
            {
                if (DB.GetSqlN("Declare  @GoodsOutStatus int; EXEC [sp_INV_CheckGoodsOutStatus] @OutboundID=" + OBDTrack.OBDTrackingID + ", @Status=@GoodsOutStatus out; select @GoodsOutStatus as N;") == 0)
                {
                    resetError("Goods Out is not yet performed", true);
                    return;
                }
            }

            if (ddlStores.SelectedValue == "0")
            {
                resetError("Please select atleast one store", true);
                ddlStores.Focus();
                return;
            }


            //IDataReader rsRefDetails = DB.GetRS("SELECT REFD.OB_RefWarehouse_DetailsID,isnull(OutboundTracking_WarehouseID,0) OutboundTracking_WarehouseID FROM  OBD_RefWarehouse_Details REFD "+
            //                                    "LEFT JOIN OBD_OutboundTracking_Warehouse OBTW  ON REFD.OB_RefWarehouse_DetailsID=OBTW.OB_RefWarehouse_DetailsID AND OBTW.OutboundID=REFD.OutboundID "+
            //                                    "WHERE REFD.WareHouseID=" + ddlStores.SelectedValue + " AND REFD.OutboundID=" + CommonLogic.QueryString("obdid"));


            IDataReader rsRefDetails = DB.GetRS("Exec USP_OBDRefWarehouseDropDown @WareHouseID=" + ddlStores.SelectedValue + " , @OutboundID=" + CommonLogic.QueryString("obdid"));




            //int REF_WHID = DB.GetSqlN("select OB_RefWarehouse_DetailsID AS N from OBD_RefWarehouse_Details where IsActive=1 and IsDeleted=0 and WarehouseID=" + ddlStores.SelectedValue + " and OutboundID=" + OBDTrack.OBDTrackingID.ToString());

            int REF_WHID = 0, OBDTrack_WHID = 0;

            while (rsRefDetails.Read())
            {
                REF_WHID = DB.RSFieldInt(rsRefDetails, "OB_RefWarehouse_DetailsID");
                OBDTrack_WHID = DB.RSFieldInt(rsRefDetails, "OutboundTracking_WarehouseID");
            }

            rsRefDetails.Close();
            if (OBDTrack.DeliveryStatusID <= 3)
            {

                if (IsOBDpiking == 0)
                {
                    //OBDTrack.UpdateOBDTracking_SentForPGI();
                    int NOBD = 0;
                    try
                    {
                        DataSet dsResult = DB.GetDS("EXEC [dbo].[sp_OBD_MoveStockOutByPickingData ] @OutboundID = " + OBDTrack.OBDTrackingID + ", @CreatedBy = " + cp.UserID + "", false);


                        if (dsResult != null && dsResult.Tables.Count != 0)
                        {
                            if (dsResult.Tables.Count == 2)
                            {
                                if (dsResult.Tables[0].Rows[0][0].ToString() == "-1")
                                {
                                    resetError("Picking is not completed", true);
                                    return;
                                }
                                else if (dsResult.Tables[0].Rows[0][0].ToString() == "-2")
                                {
                                    resetError("Picked quantity not available at dock location", true);
                                    return;
                                }
                                else if (dsResult.Tables[0].Rows[0][0].ToString() == "-3")
                                {
                                    resetError("There is an issue in while processing stock out", true);
                                    return;
                                }
                                else if (dsResult.Tables[0].Rows[0][0].ToString() == "-4")
                                {
                                    resetError("Unexpected error  while processing stock out", true);
                                    return;
                                }
                                else if (dsResult.Tables[0].Rows[0][0].ToString() == "-5")
                                {
                                    resetError("There is no pending quantity for stock out", true);
                                    return;
                                }
                            }
                        }
                        else
                        {
                            resetError("Error while processing stock out transaction, please contact support ", true);
                            return;
                        }


                    }
                    catch (Exception ex)
                    {
                        resetError("Error while updating ", true);
                        return;
                    }


                }

            }



            OBDTrack.TotalQuantity = 0;// Convert.ToDecimal((txtTotalQuantity.Text == "" ? "0" : txtTotalQuantity.Text));
            OBDTrack.NoofLines = 0;//Convert.ToInt32(txtNoOfLines.Text);
            OBDTrack.PickedBy = Convert.ToInt32(Request.Form[hifPickedByID.UniqueID]);
            OBDTrack.CheckedBy = Request.Form[hifCheckedByID.UniqueID];
            OBDTrack.RemBy_StoreIncharge = txtRemarks_StoreIncharge.Text;
            OBDTrack.StoreID = Convert.ToInt32(ddlStores.SelectedValue);
            OBDTrack.RefWHID = REF_WHID;

            Convert.ToDateTime(DateTime.UtcNow, CultureInfo.CurrentCulture).ToString("dd-MMM-yyyy").ToString();
            string shpDate = txtOBDRcvdDate.Text.Trim();
            if (shpDate != "" && (shpDate.Contains("/") || shpDate.Contains("-")))
            {
                DateTime _dtShipmentExpDate;
                if (DateTime.TryParse(shpDate, out _dtShipmentExpDate))
                {
                    shpDate = _dtShipmentExpDate.ToString("dd-MMM-yyyy hh:mm tt");
                }
                else
                {
                    shpDate = shpDate.Replace("-", "/");
                    shpDate = shpDate.Split('/')[1] + "/" + shpDate.Split('/')[0] + "/" + shpDate.Split('/')[2];
                }
            }

            OBDTrack.OBDReceivedDT = shpDate;
            //(txtOBDRcvdDate.Text != "" ? DateTime.ParseExact(txtOBDRcvdDate.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("dd-MMM-yyyy") : null); 
            OBDTrack.StoreInchargeID = cp.UserID;


            OBDTrack.SentForPGI_DT = ((txtOBDRcvdDate.Text.Trim() != "" && OBDRecvdTimeEntry.Text.Trim() != "") ? txtOBDRcvdDate.Text.Trim() + " " + OBDRecvdTimeEntry.Text.Trim() : "");
            //OBDTrack.SentForPGI_DT =  Convert.ToString(Convert.ToDateTime(((txtOBDRcvdDate.Text.Trim() != "" && OBDRecvdTimeEntry.Text.Trim() != "") ? DateTime.ParseExact(txtOBDRcvdDate.Text.Trim() + " " + OBDRecvdTimeEntry.Text.Trim(), "dd-MMM-yyyy hh:mm tt", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy hh:mm tt") : null)).ToUniversalTime());
            OBDTrack.TransferedtoStoreID = Convert.ToInt32(ddlStores.SelectedValue);
            //OBDTrack.OBDTrack_WHID = DB.GetSqlN("Select OutboundTracking_WarehouseID as N from OBD_OutboundTracking_Warehouse Where OutboundID =" + OBDTrack.OBDTrackingID.ToString() + " AND  OB_RefWarehouse_DetailsID=" + REF_WHID);

            OBDTrack.OBDTrack_WHID = OBDTrack_WHID;

            bool IsUpload = false;

            if (fuPickNCheckNotes.HasFile)
            {
                String FileExtenion = Path.GetExtension(fuPickNCheckNotes.FileName);

                if (FileExtenion == ".pdf")
                {
                    IsUpload = true;
                }
                else
                {
                    resetError("Please attach pdf document", true);
                    return;
                }
            }

            int Result = OBDTrack.UpdateOBDTracking_SentForPGI();

            if (Result == -2)
            {
                resetError("Add atleast one Work Order", true);
                //CommonLogic._DeleteAttatchment(TenantRootDir + hifTenant.Value + OutboundPath + OBD_PickandCheckSheetPath, OBDTrack.OBDNumber + "_" + ddlStores.SelectedValue + ".pdf");
                return;
            }

            else if (Result == -3)
            {
                resetError("Pick N Check is already done for the selected store (" + ddlStores.SelectedItem.Text + ") . Please check another store from the 'Pick Store' dropdown", true);
                return;
            }

            else if (Result == -1)
            {
                resetError("Error while updating", true);
                return;
            }

            else if (Result == 0)
            {
                //pnlPGIDetails.Visible = true;
                upnl3pl.Visible = true;
                pnlPackagingdetails.Visible = true;
                pnlReceivedDelivery.Visible = true;
                pnlReceivedDelivery.Attributes.Add("style", "display:block");
            }

            if (IsUpload)
            {
                if (CommonLogic.UploadFile(TenantRootDir + hifTenant.Value + OutboundPath + OBD_PickandCheckSheetPath, fuPickNCheckNotes, OBDTrack.OBDNumber + "_" + ddlStores.SelectedValue + Path.GetExtension(fuPickNCheckNotes.FileName)) == false)
                {
                }
            }

            //resetError("Pick N Check details successfully updated", false);

            LoadPickNCheckDetails(REF_WHID, Convert.ToInt32(ddlStores.SelectedValue));

            ValidateOutboundStatus();

            //if (OBDTrack.UpdateOBDTracking_SentForPGI())
            //{


            //    if (DB.GetSqlN(" select COUNT(PickedBy) AS N from OBD_OutboundTracking_Warehouse where OutboundID=" + OBDTrack.OBDTrackingID.ToString()) == ArrStores.Length)
            //    {
            //        DB.ExecuteSQL("     Update OBD_Outbound set DeliveryStatusID=5 where OutboundID=" + OBDTrack.OBDTrackingID.ToString());
            //    }


            //    //DB.ExecuteSQL("UPDATE INV_GoodsMovementDetails  SET IsActive=1 where  GoodsMovementTypeID=2 AND IsDeleted=0 AND TransactionDocID="+OBDTrack.OBDTrackingID+" AND POSODetailsID IN ( Select ORD_SOD.SODetailsID from OBD_Outbound_ORD_CustomerPO OBD_CPO  LEFT JOIN ORD_SODetails ORD_SOD ON ORD_SOD.SOHeaderID=OBD_CPO.SOHeaderID AND ORD_SOD.IsActive=1 AND ORD_SOD.IsDeleted=0 WHERE OBD_CPO.OutboundID="+OBDTrack.OBDTrackingID+" AND OBD_CPO.IsActive=1 AND OBD_CPO.IsDeleted=0)");
            //    //DB.ExecuteSQL("UPDATE INV_PO_GoodsOutLink SET IsActive=1 where IsDeleted=0 AND SODetailsID IN ( Select ORD_SOD.SODetailsID from OBD_Outbound_ORD_CustomerPO OBD_CPO  LEFT JOIN ORD_SODetails ORD_SOD ON ORD_SOD.SOHeaderID=OBD_CPO.SOHeaderID AND ORD_SOD.IsActive=1 AND ORD_SOD.IsDeleted=0 WHERE OBD_CPO.OutboundID="+OBDTrack.OBDTrackingID+" AND OBD_CPO.IsActive=1 AND OBD_CPO.IsDeleted=0)");


            //    resetError("Pick N Check details successfully updated", false);

            //    LoadPickNCheckDetails(REF_WHID, Convert.ToInt32(ddlStores.SelectedValue));

            //    if (DB.GetSqlN("select Count(PickedBy) AS N from OBD_OutboundTracking_Warehouse where OutboundID=" + OBDTrack.OBDTrackingID.ToString()) == ArrStores.Length)
            //        pnlPGIDetails.Visible = true;

            //}
            UpdatePGIDetails();
            LoadPGIDetails();

        }
        public void UpdatePGIDetails()
        {
            var val1 = txtAutoPGIDate.Text.ToString().Trim();
            var val3 = txtAutoPGITime.Text.ToString().Trim();
            var val2 = hifPickedByID.Value.ToString().Trim();

            //if (DB.GetSqlN("Declare  @GoodsOutStatus int; EXEC [sp_INV_CheckGoodsOutStatus] @OutboundID=" + OBDTrack.OBDTrackingID + ", @Status=@GoodsOutStatus out; select @GoodsOutStatus as N;") == 0)
            //{
            //    resetError("Not yet Goods-Out ", true);
            //    return;
            //}

            OBDTrack.RemByIni_AfterPGI = txtPGIRemarks.Text.Trim();
            OBDTrack.PGIDoneBy = Convert.ToInt32(Request.Form[hifPickedByID.UniqueID]); 

            string shpDate = val1;



            //OBDTrack.PGIDoneDate = ((shpDate != "" && val3 != "") ? DateTime.ParseExact(shpDate + " " + val3, "dd-MMM-yyyy hh:mm tt", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy hh:mm tt") : null);
            OBDTrack.PGIDoneDate = val1;
            // OBDTrack.PGIDoneDate = ((txtPGIDate.Text.Trim() != "" && txtPGITimeEntry.Text.Trim() != "") ? DateTime.ParseExact(txtOBDRcvdDate.Text.Trim() + " " + OBDRecvdTimeEntry.Text.Trim(), "dd-MMM-yyyy hh:mm tt", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy hh:mm tt") : null);

            //OBDTrack.PGIDoneDate = ((txtPGIDate.Text.Trim() != "" && txtPGITimeEntry.Text.Trim() != "") ? DB.SQuote(DateTime.ParseExact(shpDate + " " + txtPGITimeEntry.Text.Trim(), "dd/MM/yyyy hh:mm tt", CultureInfo.InvariantCulture).ToString("dd-MMM-yyyy hh:mm tt")) : "NULL");
            OBDTrack.StoreID = Convert.ToInt32(ddlStores.SelectedValue);
            OBDTrack.PackedBy = cp.UserID;

            // int REF_WHID = DB.GetSqlN("select OB_RefWarehouse_DetailsID AS N from OBD_RefWarehouse_Details where IsActive=1 and IsDeleted=0 and WarehouseID=" + ddlStores.SelectedValue + " and OutboundID=" + OBDTrack.OBDTrackingID.ToString());
            int REF_WHID = DB.GetSqlN("Exec[dbo].[USP_GetREF_WHID]  @WarehouseID = " + ddlStores.SelectedValue + " ,@OutboundID = " + OBDTrack.OBDTrackingID.ToString());


            OBDTrack.RefWHID = REF_WHID;

            try
            {
                int Result = OBDTrack.UpdateOBDTracking_PGIDone();

                if (Result == -2)
                {
                    LoadPackingDetails(OBDTrack.RefWHID, OBDTrack.StoreID);

                    // LoadReceivedDeliveryDetails(OBDTrack.RefWHID, OBDTrack.StoreID);
                    //pnlReceivedDelivery.Visible = true;
                    resetError("PGI is already updated", true);
                    ValidateCharge();
                    ValidateOutboundStatus();
                    return;
                }
                else if (Result == -1)
                {
                    resetError("Error while updating", true);
                    return;
                }

                if (OBDTrack.DocumentTypeID >= 6 && OBDTrack.DocumentTypeID <= 9)
                {
                    LoadPackingDetails(OBDTrack.RefWHID, OBDTrack.StoreID);

                    // LoadReceivedDeliveryDetails(OBDTrack.RefWHID, OBDTrack.StoreID);
                    //pnlReceivedDelivery.Visible = true;
                }


                string HUQuery = "EXEC [dbo].[SP_Upsert_HU_GoodsOut] @outboundID=" + OBDTrack.OBDTrackingID.ToString() + ",@UserID=" + cp.UserID;
                DataSet dsHu = DB.GetDS(HUQuery, false);

                if (dsHu.Tables[0].Rows[0][0].ToString() == "Done")
                {
                    resetError("PGI details successfully updated", false);
                }

                    

                ValidateCharge();
                ValidateOutboundStatus();

                //pnlPackingDetails.Visible = true;
                pnlPackagingdetails.Visible = true;
                pnlReceivedDelivery.Visible = true;
                upnl3pl.Visible = true;
                pnlReceivedDelivery.Attributes.Add("style", "display:block");
            }

            catch (Exception ex)
            {
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
                resetError("Error while updating PGI details", false);
                return;
            }
        }

        protected void ddlStores_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (ddlStores.SelectedItem.Text == "Select Store")
            {
                lblStoreName.Text = "";
            }
            else
            {
                lblStoreName.Text = ddlStores.SelectedItem.Text;
            }

            //  int REF_WHID = DB.GetSqlN("select OB_RefWarehouse_DetailsID AS N from OBD_RefWarehouse_Details where IsActive=1 and IsDeleted=0 and WarehouseID=" + ddlStores.SelectedValue + " and OutboundID=" + OBDTrack.OBDTrackingID.ToString());
            int REF_WHID = DB.GetSqlN("Exec [dbo].[USP_GetREF_WHID]  @WarehouseID=" + ddlStores.SelectedValue + " ,@OutboundID=" + OBDTrack.OBDTrackingID.ToString());

            LoadPickNCheckDetails(REF_WHID, Convert.ToInt32(ddlStores.SelectedValue));

            // int OBD_Tracking_WHID = DB.GetSqlN("Select OutboundTracking_WarehouseID as N from OBD_OutboundTracking_Warehouse Where OutboundID =" + OBDTrack.OBDTrackingID.ToString() + " AND  OB_RefWarehouse_DetailsID=" + REF_WHID);
            int OBD_Tracking_WHID = DB.GetSqlN("Exec [dbo].[USP_GetOBDTrack_WHID] @OutboundID=" + OBDTrack.OBDTrackingID.ToString() + ",@REF_WHID=" + REF_WHID);


            RequiredVehicleSql = "EXEC [dbo].[sp_OBD_GetRequiredVehicleDetails]  @OutboundTracking_WarehouseID=" + OBD_Tracking_WHID;
            ViewState["ReqVListSQL"] = RequiredVehicleSql;
            this.gvRequiredVehicle_buildGridData(this.gvRequiredVehicle_buildGridData());

        }






        #endregion  ---- Pick N Check -----


        #region -----  Required Vehicle Grid ------

        protected DataSet gvRequiredVehicle_buildGridData()
        {
            string sql = ViewState["ReqVListSQL"].ToString();
            DataSet ds = DB.GetDS(sql, false);
            return ds;
        }

        protected void gvRequiredVehicle_buildGridData(DataSet ds)
        {
            gvRequiredVehicle.DataSource = ds.Tables[0];
            gvRequiredVehicle.DataBind();
            ds.Dispose();

        }

        protected void lnkReqVehicle_Click(object sender, EventArgs e)
        {

            //if (!cp.IsInAnyRoles("4"))
            //{

            //    resetError("Only OB Store In-charge can access this shipment", true);
            //    return;

            //}

            //if (DB.GetSqlN("select top 1 Outbound_CustomerPOID AS N from OBD_Outbound_ORD_CustomerPO where IsActive=1 and IsDeleted=0 and OutboundID=" + OBDTrack.OBDTrackingID.ToString()) == 0)
            if (DB.GetSqlN("[dbo].[USP_GetCustomerPOIDForOBD] @OutboundID =" + OBDTrack.OBDTrackingID.ToString()) == 0)

            {
                resetError("Add atleast one Work Order", true);
                return;
            }


            ViewState["gvReqVIsInsert"] = false;

            // int vREF_WHID = DB.GetSqlN("select OB_RefWarehouse_DetailsID AS N from OBD_RefWarehouse_Details where IsActive=1 and IsDeleted=0 and WarehouseID=" + ddlStores.SelectedValue + " and OutboundID=" + OBDTrack.OBDTrackingID.ToString());
            int vREF_WHID = DB.GetSqlN("Exec[dbo].[USP_GetREF_WHID]  @WarehouseID = " + ddlStores.SelectedValue + " ,@OutboundID = " + OBDTrack.OBDTrackingID.ToString());

            // int OBD_Tracking_WHID = DB.GetSqlN("Select OutboundTracking_WarehouseID as N from OBD_OutboundTracking_Warehouse Where OutboundID =" + OBDTrack.OBDTrackingID.ToString() + " AND  OB_RefWarehouse_DetailsID=" + vREF_WHID);
            int OBD_Tracking_WHID = DB.GetSqlN("Exec [dbo].[USP_GetOBDTrack_WHID] @OutboundID=" + OBDTrack.OBDTrackingID.ToString() + ",@REF_WHID=" + vREF_WHID);


            if (OBD_Tracking_WHID == 0)
            {
                resetError("Please update pick & check details", true);
                return;
            }

            StringBuilder sql = new StringBuilder(2500);

            /*
            sql.Append("INSERT INTO OBD_RequiredEquipment (OutboundTracking_WarehouseID) values (");
            sql.Append(OBD_Tracking_WHID);
            sql.Append(")");*/

            try
            {
                /*
                DB.ExecuteSQL(sql.ToString());
                gvRequiredVehicle.EditIndex = gvRequiredVehicle.Rows.Count;*/

                gvRequiredVehicle.EditIndex = 0;
                gvRequiredVehicle.PageIndex = 0;

                DataSet dsRequiredVehicle = this.gvRequiredVehicle_buildGridData();
                DataRow row = dsRequiredVehicle.Tables[0].NewRow();
                row["EquipmentID"] = 0;
                row["RequiredEquipmentID"] = 0;
                dsRequiredVehicle.Tables[0].Rows.InsertAt(row, 0);


                RequiredVehicleSql = "EXEC [dbo].[sp_OBD_GetRequiredVehicleDetails]  @OutboundTracking_WarehouseID=" + OBD_Tracking_WHID;
                ViewState["ReqVListSQL"] = RequiredVehicleSql;
                this.gvRequiredVehicle_buildGridData(dsRequiredVehicle);

                //this.resetError("New Required Vehicle line item added", false);
                ViewState["gvReqVIsInsert"] = true;
                ViewState["gvRVItemNotUpdated"] = true;

                lnkReqVehicle.Visible = false;

            }
            catch (Exception ex)
            {
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
                this.resetError("Error while inserting new Required Vehicle line item ", true);

            }


        }

        protected void gvRequiredVehicle_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.DataItem == null)
                return;

            if ((e.Row.RowState & DataControlRowState.Edit) == DataControlRowState.Edit)
            {

                DropDownList ddlEquipment = (DropDownList)e.Row.FindControl("ddlEquipmentType");
                Literal vltEquipmentID = (Literal)e.Row.FindControl("ltEquipmentID");
                CommonLogic.LoadEquipment(ddlEquipment, "Select Equipment Type", "0", cp.TenantID);
                ddlEquipment.SelectedValue = vltEquipmentID.Text;

                ddlEquipment.Focus();


            }


        }

        protected void gvRequiredVehicle_RowCommand(object sender, GridViewCommandEventArgs e)
        {



            if (e.CommandName == "DeleteItem")
            {
                ViewState["gvReqVIsInsert"] = false;
                gvRequiredVehicle.EditIndex = -1;
                int iden = Localization.ParseNativeInt(e.CommandArgument.ToString());
                this.deleteRowPermReqV(iden);
            }


        }

        protected void gvRequiredVehicle_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            //if (!cp.IsInAnyRoles("4"))
            //{

            //    resetError("Only OB Store In-charge can access this shipment", true);
            //    return;

            //}

            ViewState["gvReqVIsInsert"] = false;
            GridViewRow row = gvRequiredVehicle.Rows[e.RowIndex];

            if (row != null)
            {

                string ltPrEquipmentID = ((Literal)row.FindControl("lthidRequiredVehicleID")).Text;
                string ddlEquipmentID = ((DropDownList)row.FindControl("ddlEquipmentType")).SelectedValue.ToString();
                string vtxtRequiredValue = ((TextBox)row.FindControl("txtRequiredValue")).Text;

                StringBuilder sqlReqVehcl = new StringBuilder(2500);

                /*
                sqlReqVehcl.Append("UPDATE OBD_RequiredEquipment SET ");
                sqlReqVehcl.Append("EquipmentID=" + ddlEquipmentID + ",");
                sqlReqVehcl.Append("RequiredValue=" + vtxtRequiredValue + ",");
                sqlReqVehcl.Append("CreatedBy=" + CreatedBy);
                sqlReqVehcl.Append("  WHERE RequiredEquipmentID=" + ltPrEquipmentID);*/
                //<!-------------Procedure Converting-------------->  
                //  int vREF_WHID = DB.GetSqlN("select OB_RefWarehouse_DetailsID AS N from OBD_RefWarehouse_Details where IsActive=1 and IsDeleted=0 and WarehouseID=" + ddlStores.SelectedValue + " and OutboundID=" + OBDTrack.OBDTrackingID.ToString());
                int vREF_WHID = DB.GetSqlN("Exec[dbo].[USP_GetREF_WHID]  @WarehouseID = " + ddlStores.SelectedValue + " ,@OutboundID = " + OBDTrack.OBDTrackingID.ToString());

                //  int OBD_Tracking_WHID = DB.GetSqlN("Select OutboundTracking_WarehouseID as N from OBD_OutboundTracking_Warehouse Where OutboundID =" + OBDTrack.OBDTrackingID.ToString() + " AND  OB_RefWarehouse_DetailsID=" + vREF_WHID);
                int OBD_Tracking_WHID = DB.GetSqlN("Exec [dbo].[USP_GetOBDTrack_WHID] @OutboundID=" + OBDTrack.OBDTrackingID.ToString() + ",@REF_WHID=" + vREF_WHID);


                sqlReqVehcl.Append(" DECLARE @NewUpdateoutboundSOID int;  EXEC [dbo].[sp_OBD_UpsertRequiredEquipment]   @RequiredEquipmentID=" + ltPrEquipmentID + ",@OutboundTracking_WarehouseID=" + OBD_Tracking_WHID + ",@EquipmentID=" + ddlEquipmentID + ",@RequiredValue=" + vtxtRequiredValue + ",@CreatedBy=" + CreatedBy + ",@NewRequiredEquipmentID=@NewUpdateoutboundSOID OUTPUT; select @NewUpdateoutboundSOID AS N");

                try
                {
                    DB.ExecuteSQL(sqlReqVehcl.ToString());
                    this.resetError("Successfully Updated", false);
                    gvRequiredVehicle.EditIndex = -1;
                    ViewState["gvRVItemNotUpdated"] = false;
                    this.gvRequiredVehicle_buildGridData(this.gvRequiredVehicle_buildGridData());
                    lnkReqVehicle.Visible = true;
                }
                catch (Exception ex)
                {
                    ViewState["gvRVItemNotUpdated"] = true;
                    CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
                    this.resetError("Error while updating Required Vehicle details", true);
                }




            }


        }

        protected void gvRequiredVehicle_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            /*
            if (Localization.ParseBoolean(ViewState["gvReqVIsInsert"].ToString()) )
            {
                GridViewRow row = gvRequiredVehicle.Rows[e.RowIndex];
                if (row != null)
                {
                    int iden = Convert.ToInt32(((Literal)row.FindControl("lthidRequiredVehicleID")).Text);
                    deleteRowPermReqV(iden);
                    lnkReqVehicle.Visible = true;
                }
            }

            ViewState["gvReqVIsInsert"] = false;
            */
            gvRequiredVehicle.EditIndex = -1;
            this.gvRequiredVehicle_buildGridData(this.gvRequiredVehicle_buildGridData());
            lnkReqVehicle.Visible = true;
        }

        protected void gvRequiredVehicle_RowEditing(object sender, GridViewEditEventArgs e)
        {
            ViewState["gvReqVIsInsert"] = false;

            gvRequiredVehicle.EditIndex = e.NewEditIndex;
            ViewState["gvRVItemNotUpdated"] = false;
            this.gvRequiredVehicle_buildGridData(this.gvRequiredVehicle_buildGridData());
            lnkReqVehicle.Visible = false;
        }

        protected void lnkDeleteReqVItem_Click(object sender, EventArgs e)
        {

            //if (!cp.IsInAnyRoles("4"))
            //{

            //    resetError("Only OB Store In-charge can access this shipment", true);
            //    return;

            //}

            string rfidIDs = "";

            bool chkBox = false;

            StringBuilder sqlDeleteString = new StringBuilder();
            sqlDeleteString.Append("BEGIN TRAN ");

            //'Navigate through each row in the GridView for checkbox items
            foreach (GridViewRow gv in gvRequiredVehicle.Rows)
            {

                CheckBox isDelete = (CheckBox)gv.FindControl("chkIsDeleteReqVItem");

                if (isDelete == null)
                    return;

                if (isDelete.Checked)
                {
                    chkBox = true;
                    // Concatenate GridView items with comma for SQL Delete
                    rfidIDs = ((Literal)gv.FindControl("lthidRequiredVehicleID")).Text.ToString();



                    if (rfidIDs != "")
                        sqlDeleteString.Append(" Delete OBD_RequiredEquipment Where RequiredEquipmentID=" + rfidIDs + ";");

                }
            }


            sqlDeleteString.Append(" COMMIT ");


            // Execute SQL Query only if checkboxes are checked to avoid any error with initial null string
            try
            {

                if (chkBox)
                {
                    DB.ExecuteSQL(sqlDeleteString.ToString());
                    resetError("Successfully deleted the selected line items", false);
                }


            }
            catch (Exception ex)
            {
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
                resetError("Error deleting selected line items", true);
            }

            this.gvRequiredVehicle_buildGridData(this.gvRequiredVehicle_buildGridData());


        }

        protected void deleteRowPermReqV(int iden)
        {
            StringBuilder sql = new StringBuilder(2500);
            sql.Append("DELETE from OBD_RequiredEquipment where RequiredEquipmentID=" + iden.ToString());
            try
            {
                DB.ExecuteSQL(sql.ToString());
                this.gvRequiredVehicle_buildGridData(this.gvRequiredVehicle_buildGridData());

            }
            catch (Exception ex)
            {
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
                this.resetError("Error deleting Required Vehicle details", true);
            }
        }




        #endregion -----  Required Vehicle Grid ------


        #region -----  PGI Details ------

        protected void LoadPGIDetails()
        {


            if (OBDTrack.DeliveryStatusID == 5)
                lnkPGICancel.Visible = true;

            atcPGIRequestedBy.Text = CommonLogic.GetUserName(OBDTrack.PGIDoneBy.ToString());
            HifPGIRequestedByID.Value = OBDTrack.PGIDoneBy.ToString();
            txtPGIRemarks.Text = OBDTrack.RemByIni_AfterPGI;
            //txtPGIDate.Text = (OBDTrack.PGIDoneDate == null ? DateTime.UtcNow.ToString("dd/MMM/yyyy") : Convert.ToDateTime(OBDTrack.PGIDoneDate).ToString("dd-MMM-yyyy"));

            //txtPGITimeEntry.Text = (OBDTrack.PGIDoneDate == null ? DateTime.UtcNow.ToString("hh:mm tt") : Convert.ToDateTime(OBDTrack.PGIDoneDate).ToString("hh:mm tt"));


            int REF_WHID = DB.GetSqlN("Exec [dbo].[USP_GetREF_WHID] @OutboundID=" + OBDTrack.OBDTrackingID.ToString());
            int WHID = DB.GetSqlN("Exec [dbo].[USP_GetWHIDByRefWHID]  @REFWHID = " + REF_WHID);

            //if (OBDTrack.PGIDoneDate != "" || OBDTrack.PGIDoneDate != null) 
            //{
            //    txtPGIDate.Text = OBDTrack.PGIDoneDate.Split(' ')[0];
            //    txtPGITimeEntry.Text = OBDTrack.PGIDoneDate.Split(' ')[1] + " " + OBDTrack.PGIDoneDate.Split(' ')[2];
            //}
            //else
            //{
            //    txtPGIDate.Text = DateTime.UtcNow.Date.ToString("dd-MMM-yyyy");
            //    txtPGITimeEntry.Text = DB.GetSqlS("SELECT FORMAT((SELECT[dbo].[UDF_GetWarehouseTime] ('" + DateTime.UtcNow.ToString("hh:mm tt") + "'," + WHID + ")),'hh:mm tt') AS S");
            //}

            //if (OBDTrack.DeliveryDate != "")
            //{
            //    txtDeliveryDate.Text = OBDTrack.DeliveryDate.Split(' ')[0];
            //    txtReceivedDelTimeEntry.Text = OBDTrack.DeliveryDate.Split(' ')[1] + "" + OBDTrack.DeliveryDate.Split(' ')[2];
            //}
            //else
            //{
            //    txtDeliveryDate.Text = DateTime.UtcNow.Date.ToString("dd-MMM-yyyy");
            //    txtReceivedDelTimeEntry.Text = DB.GetSqlS("SELECT FORMAT((SELECT[dbo].[UDF_GetWarehouseTime] ('" + DateTime.UtcNow.ToString("hh:mm tt") + "'," + WHID + ")),'hh:mm tt') AS S");
            //}

            OBDTrack = new OBDTrack(Convert.ToInt32(OBDTrackID));

        }

        protected void lnkPGICancel_Click(object sender, EventArgs e)
        {
            //if (!cp.IsInAnyRoles("3"))
            //{

            //    resetError("Only OB-Initiator can access the outbound delivery", true);
            //    return;

            //}


            txtPGIDate.Text = "";
            txtPGIRemarks.Text = "";
            txtPGITimeEntry.Text = "";
            atcPGIRequestedBy.Text = "";


        }

        protected void lnkPGIUpdate_Click(object sender, EventArgs e)
        {

            //if (!cp.IsInAnyRoles("3"))
            //{
            //    resetError("Only OB-Initiator can access the outbound delivery", true);
            //    return;
            //}
            var val1 = txtPGIDate.Text.ToString().Trim();
            var val3 = txtPGITimeEntry.Text.ToString().Trim();
            var val2 = HifPGIRequestedByID.Value.ToString().Trim();
            Page.Validate("PGIUpdate");

            if (Page.IsValid == false)
            {
                resetError("Please check for mandatory fields", true);
                return;
            }
            if (val2 == "0" || val2 == "")
            {
                resetError("Please check for mandatory fields", true);
                return;
            }

            //if (DB.GetSqlN("select PGIDoneBy AS N from OBD_Outbound where OutboundID=" + OBDTrack.OBDTrackingID.ToString()) != 0)
            //{
            //    resetError("PGI is already updated", true);
            //    return;
            //}

            if (DB.GetSqlN("Declare  @GoodsOutStatus int; EXEC [sp_INV_CheckGoodsOutStatus] @OutboundID=" + OBDTrack.OBDTrackingID + ", @Status=@GoodsOutStatus out; select @GoodsOutStatus as N;") == 0)
            {
                resetError("Not yet Goods-Out ", true);
                return;
            }

            OBDTrack.RemByIni_AfterPGI = txtPGIRemarks.Text.Trim();
            OBDTrack.PGIDoneBy = Convert.ToInt16(Request.Form[HifPGIRequestedByID.UniqueID]);

            string shpDate = val1;



            OBDTrack.PGIDoneDate = Convert.ToString(Convert.ToDateTime(((shpDate != "" && val3 != "") ? DateTime.ParseExact(shpDate + " " + val3, "dd-MMM-yyyy hh:mm tt", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy hh:mm tt") : null)).ToUniversalTime());
            //((shpDate != "" && val3 != "") ? DateTime.ParseExact(shpDate + " " + val3, "dd-MMM-yyyy hh:mm tt", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy hh:mm tt") : null);
            // OBDTrack.PGIDoneDate = ((txtPGIDate.Text.Trim() != "" && txtPGITimeEntry.Text.Trim() != "") ? DateTime.ParseExact(txtOBDRcvdDate.Text.Trim() + " " + OBDRecvdTimeEntry.Text.Trim(), "dd-MMM-yyyy hh:mm tt", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy hh:mm tt") : null);

            //OBDTrack.PGIDoneDate = ((txtPGIDate.Text.Trim() != "" && txtPGITimeEntry.Text.Trim() != "") ? DB.SQuote(DateTime.ParseExact(shpDate + " " + txtPGITimeEntry.Text.Trim(), "dd/MM/yyyy hh:mm tt", CultureInfo.InvariantCulture).ToString("dd-MMM-yyyy hh:mm tt")) : "NULL");
            OBDTrack.StoreID = Convert.ToInt32(ddlStores.SelectedValue);
            OBDTrack.PackedBy = cp.UserID;

            // int REF_WHID = DB.GetSqlN("select OB_RefWarehouse_DetailsID AS N from OBD_RefWarehouse_Details where IsActive=1 and IsDeleted=0 and WarehouseID=" + ddlStores.SelectedValue + " and OutboundID=" + OBDTrack.OBDTrackingID.ToString());
            int REF_WHID = DB.GetSqlN("Exec[dbo].[USP_GetREF_WHID]  @WarehouseID = " + ddlStores.SelectedValue + " ,@OutboundID = " + OBDTrack.OBDTrackingID.ToString());


            OBDTrack.RefWHID = REF_WHID;

            try
            {
                int Result = OBDTrack.UpdateOBDTracking_PGIDone();

                if (Result == -2)
                {
                    LoadPackingDetails(OBDTrack.RefWHID, OBDTrack.StoreID);

                    // LoadReceivedDeliveryDetails(OBDTrack.RefWHID, OBDTrack.StoreID);
                    //pnlReceivedDelivery.Visible = true;
                    resetError("PGI is already updated", true);
                    ValidateCharge();
                    ValidateOutboundStatus();
                    return;
                }
                else if (Result == -1)
                {
                    resetError("Error while updating", true);
                    return;
                }

                if (OBDTrack.DocumentTypeID >= 6 && OBDTrack.DocumentTypeID <= 9)
                {
                    LoadPackingDetails(OBDTrack.RefWHID, OBDTrack.StoreID);

                    // LoadReceivedDeliveryDetails(OBDTrack.RefWHID, OBDTrack.StoreID);
                    //pnlReceivedDelivery.Visible = true;
                }

                resetError("PGI details successfully updated", false);

                ValidateCharge();
                ValidateOutboundStatus();

                //pnlPackingDetails.Visible = true;
                pnlPackagingdetails.Visible = true;
                pnlReceivedDelivery.Visible = true;
                upnl3pl.Visible = true;
                pnlReceivedDelivery.Attributes.Add("style", "display:block");
            }

            catch (Exception ex)
            {
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
                resetError("Error while updating PGI details", false);
                return;
            }

            //StringBuilder sqlPGIUpdate = new StringBuilder();

            //sqlPGIUpdate.Append("Update  OBD_Outbound SET ");
            //sqlPGIUpdate.Append("RemByIni_AfterPGI=" + DB.SQuote(txtPGIRemarks.Text));
            //sqlPGIUpdate.Append(", PGIDoneBy=" + Request.Form[HifPGIRequestedByID.UniqueID]);


            //sqlPGIUpdate.Append(", PGIDoneOn=" + ((txtPGIDate.Text.Trim() != "" && txtPGITimeEntry.Text.Trim() != "") ? DB.SQuote(DateTime.ParseExact(txtPGIDate.Text.Trim() + " " + txtPGITimeEntry.Text.Trim(), "dd/MM/yyyy hh:mm tt", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy hh:mm tt")) : "NULL"));


            //sqlPGIUpdate.Append(" where OutboundID=" + OBDTrack.OBDTrackingID.ToString());

            //try
            //{

            //    DB.ExecuteSQL(sqlPGIUpdate.ToString());

            //    DB.ExecuteSQL("UPDATE INV_GoodsMovementDetails  SET IsActive=1 where  GoodsMovementTypeID=2 AND IsDeleted=0 AND TransactionDocID=" + OBDTrack.OBDTrackingID + " AND POSODetailsID IN ( Select ORD_SOD.SODetailsID from OBD_Outbound_ORD_CustomerPO OBD_CPO  LEFT JOIN ORD_SODetails ORD_SOD ON ORD_SOD.SOHeaderID=OBD_CPO.SOHeaderID  AND OBD_CPO.CustomerPOID=ORD_SOD.CustomerPOID AND ( OBD_CPO.InvoiceNo IS NULL OR OBD_CPO.InvoiceNo= ORD_SOD.InvoiceNo)  AND ORD_SOD.IsActive=1 AND ORD_SOD.IsDeleted=0 WHERE OBD_CPO.OutboundID=" + OBDTrack.OBDTrackingID + " AND OBD_CPO.IsActive=1 AND OBD_CPO.IsDeleted=0)");

            //    DB.ExecuteSQL("UPDATE INV_PO_GoodsOutLink SET IsActive=1 where IsDeleted=0 AND SODetailsID IN ( Select ORD_SOD.SODetailsID from OBD_Outbound_ORD_CustomerPO OBD_CPO  LEFT JOIN ORD_SODetails ORD_SOD ON ORD_SOD.SOHeaderID=OBD_CPO.SOHeaderID AND OBD_CPO.CustomerPOID=ORD_SOD.CustomerPOID AND ( OBD_CPO.InvoiceNo IS NULL OR OBD_CPO.InvoiceNo= ORD_SOD.InvoiceNo) AND ORD_SOD.IsActive=1 AND ORD_SOD.IsDeleted=0 WHERE OBD_CPO.OutboundID=" + OBDTrack.OBDTrackingID + " AND OBD_CPO.IsActive=1 AND OBD_CPO.IsDeleted=0)");

            //    DB.ExecuteSQL("Update OBD_Outbound set DeliveryStatusID=6 where OutboundID=" + OBDTrack.OBDTrackingID.ToString());

            //    if (OBDTrack.DocumentTypeID >= 6 && OBDTrack.DocumentTypeID <= 9)
            //    {

            //        UpdateDeliveryDetails();

            //        int REF_WHID = DB.GetSqlN("select OB_RefWarehouse_DetailsID AS N from OBD_RefWarehouse_Details where IsActive=1 and IsDeleted=0 and WarehouseID=" + ddlStores.SelectedValue + " and OutboundID=" + OBDTrack.OBDTrackingID.ToString());
            //        int WHID = DB.GetSqlN("Select WarehouseID AS N from OBD_RefWarehouse_Details where OB_RefWarehouse_DetailsID=" + REF_WHID);


            //        StringBuilder sbPacking = new StringBuilder();

            //        sbPacking.Append("Update  OBD_OutboundTracking_Warehouse   ");
            //        sbPacking.Append(" SET PackedOn=NULL ");
            //        sbPacking.Append(",PackedBy=" + cp.UserID);
            //        sbPacking.Append(",PackingRemarks=null");
            //        sbPacking.Append(" where OutboundID=" + OBDTrack.OBDTrackingID);
            //        sbPacking.Append(" AND OB_RefWarehouse_DetailsID=" + REF_WHID);


            //        DB.ExecuteSQL(sbPacking.ToString());

            //        LoadPackingDetails(REF_WHID, WHID);

            //        LoadReceivedDeliveryDetails(REF_WHID, WHID);
            //        pnlReceivedDelivery.Visible = true;
            //    }


            //    resetError("PGI details successfully updated", false);

            //    ValidateCharge();
            //    ValidateOutboundStatus();

            //    // pnlReceivedDelivery.Visible = true;
            //    pnlPackingDetails.Visible = true;
            //}
            //catch (Exception ex)
            //{
            //    CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
            //    resetError("Error while updating PGI details", false);
            //    return;
            //}


        }

        protected void lnkPackingSlipUpdate_Click(object sender, EventArgs e)
        {
            if (Page.IsValid == false)
            {
                resetError("Please check for mandatory fields", true);
                return;
            }
            StringBuilder sb = new StringBuilder();

            if ((DB.GetSqlN("select DeliveryStatusID as N from OBD_Outbound where OutboundID=" + OBDTrack.OBDTrackingID + " and DocumentTypeID=1")) == 6)
            {   //sb.Append("update OBD_Outbound set DeliveryStatusID = 7 where OutboundID = " + OBDTrack.OBDTrackingID);
                DB.ExecuteSQL("Update OBD_Outbound set DeliveryStatusID=7 where OutboundID=" + OBDTrack.OBDTrackingID);
                ValidateOutboundStatus();
                //pnlReceivedDelivery.Visible = true;
            }
            else
            {
                if (CommonLogic.GetUserName(OBDTrack.PGIDoneBy.ToString()) == "")
                {
                    resetError("PGI not yet performed", true);
                    return;
                }
            }

        }

        public void UpdateDeliveryDetails()
        {


            try
            {
                //<!-------------Procedure Converting-------------->

                //  int REF_WHID = DB.GetSqlN("select OB_RefWarehouse_DetailsID AS N from OBD_RefWarehouse_Details where IsActive=1 and IsDeleted=0 and WarehouseID=" + ddlStores.SelectedValue + " and OutboundID=" + OBDTrack.OBDTrackingID.ToString());
                int REF_WHID = DB.GetSqlN(" Exec[dbo].[USP_GetREF_WHID]  @WarehouseID = " + ddlStores.SelectedValue + " ,@OutboundID = " + OBDTrack.OBDTrackingID.ToString());


                //UPDATE  OBD Delivered Transaction
                StringBuilder strUpdateOBDTracking = new StringBuilder();
                strUpdateOBDTracking.Append("BEGIN TRANSACTION   ");
                strUpdateOBDTracking.Append(" UPDATE  OBD_OutboundTracking_Warehouse SET   ");
                strUpdateOBDTracking.Append("TransferedToWarehouseID=" + ddlStores.SelectedValue + ",");
                strUpdateOBDTracking.Append("InstructionModeID=NULL" + ",");
                strUpdateOBDTracking.Append("Requester=NULL" + ",");
                strUpdateOBDTracking.Append("DocumentNumber=NULL" + ",");
                strUpdateOBDTracking.Append("DocumentReceivedDate='" + DateTime.UtcNow.ToString("MM/dd/yyyy") + "',");
                strUpdateOBDTracking.Append("DeliveryDate='" + DateTime.UtcNow.ToString("MM/dd/yyyy hh:mm tt") + "',");
                strUpdateOBDTracking.Append("DeliveredBy=NULL" + ",");
                strUpdateOBDTracking.Append("DriverName=NULL" + ",");
                strUpdateOBDTracking.Append("ReceivedBy=NULL" + ",");
                strUpdateOBDTracking.Append("RemByDeliveryIncharge=NULL" + ",");
                strUpdateOBDTracking.Append("IsPODReceived=NULL");
                strUpdateOBDTracking.Append(" WHERE OutboundID=" + OBDTrack.OBDTrackingID);
                strUpdateOBDTracking.Append(" AND OB_RefWarehouse_DetailsID=" + REF_WHID);
                strUpdateOBDTracking.Append(" ;    Update OBD_Outbound set DeliveryStatusID=" + 4 + " where OutboundID=" + OBDTrack.OBDTrackingID);
                strUpdateOBDTracking.Append("    COMMIT");
                DB.ExecuteSQL(strUpdateOBDTracking.ToString());
                DB.ExecuteSQL("[dbo].[sp_INB_CloseSOStatus]  @OutboundID=" + OBDTrack.OBDTrackingID + ",@SOStatusID=4,@Status=1,@UpdatedBy=" + cp.UserID.ToString());



            }
            catch (Exception ex)
            {

                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);

                resetError("Error while updating delivery details", true);
            }

        }



        #endregion -----  PGI Details ------


        #region ------ Received Delivery --------

        protected void LoadReceivedWHStores()
        {

            try
            {
                ddlReceivingStore.Items.Clear();

                //IDataReader WHReader = DB.GetRS("select WHCode,INB_R.WarehouseID from OBD_RefWarehouse_Details INB_R Left Join GEN_Warehouse GEN_W on GEN_W.WarehouseID=INB_R.WarehouseID where INB_R.IsDeleted=0 AND INB_R.IsActive=1 and GEN_W.IsActive=1 AND GEN_W.IsDeleted=0 AND OutboundID=" + OBDTrack.OBDTrackingID.ToString());
                IDataReader WHReader = DB.GetRS("Exec USP_OBDWareHouseDropDown @OutboundID=" + OBDTrack.OBDTrackingID.ToString());
                ddlReceivingStore.Items.Add(new ListItem("Select Store", "0"));

                while (WHReader.Read())
                {
                    if ((DB.GetSqlN("select UserTypeID as N from gen_user where userid=" + cp.UserID)) == 1)
                    {
                        ddlReceivingStore.Items.Add(new ListItem(WHReader["WHCode"].ToString(), WHReader["WarehouseID"].ToString()));
                    }
                    else
                    {
                        if (cp.IsInWarehouse(WHReader["WarehouseID"].ToString()))
                            ddlReceivingStore.Items.Add(new ListItem(WHReader["WHCode"].ToString(), WHReader["WarehouseID"].ToString()));
                    }


                }

                WHReader.Close();
            }
            catch (Exception ex)
            {
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
                resetError("Error while loading warehouse list", true);
                return;
            }

        }


        protected void LoadPackingWHStores()
        {

            try
            {
                ddlPackingStores.Items.Clear();

                //IDataReader WHReader = DB.GetRS("select WHCode,INB_R.WarehouseID from OBD_RefWarehouse_Details INB_R Left Join GEN_Warehouse GEN_W on GEN_W.WarehouseID=INB_R.WarehouseID where INB_R.IsDeleted=0 AND INB_R.IsActive=1 and GEN_W.IsActive=1 AND GEN_W.isDeleted=0  AND  OutboundID=" + OBDTrack.OBDTrackingID.ToString());
                IDataReader WHReader = DB.GetRS("Exec USP_OBDWareHouseDropDown @OutboundID=" + OBDTrack.OBDTrackingID.ToString());

                ddlPackingStores.Items.Add(new ListItem("Select Store", "0"));

                while (WHReader.Read())
                {
                    if ((DB.GetSqlN("select UserTypeID as N from gen_user where userid=" + cp.UserID)) == 1)
                    {
                        ddlPackingStores.Items.Add(new ListItem(WHReader["WHCode"].ToString(), WHReader["WarehouseID"].ToString()));
                    }
                    else
                    {
                        if (cp.IsInWarehouse(WHReader["WarehouseID"].ToString()))
                            ddlPackingStores.Items.Add(new ListItem(WHReader["WHCode"].ToString(), WHReader["WarehouseID"].ToString()));
                    }


                }

                WHReader.Close();
            }
            catch (Exception ex)
            {
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
                resetError("Error while loading warehouse list", true);
                return;
            }

        }




        protected void LoadReceivedDeliveryDetails(int RefWHID, int WHID)
        {

            if (ddlReceivingStore.SelectedItem.Text == "Select Store")
            {
                lblDeliveryStore.Text = "";
            }
            else
                lblDeliveryStore.Text = ddlReceivingStore.SelectedItem.Text;


            if (OBDTrack.DeliveryStatusID == 7)
                lnkCancelDelivery.Visible = true;

            OBDTrack.LoadFromDB(OBDTrack.OBDTrackingID, RefWHID);


            ddlReceivingStore.SelectedValue = WHID.ToString();
            ddlInstructionMode.SelectedValue = OBDTrack.InstructionModeID.ToString();
            txtRequester.Text = CommonLogic.IIF(OBDTrack.Requester == "0", "", OBDTrack.Requester);
            //txtRequester.Text = OBDTrack.Requester;
            txtDocNumber.Text = OBDTrack.DocumentNumber;
            //txtDocRcvdDate.Text = (OBDTrack.DocumentReceivedDate == null || OBDTrack.DocumentReceivedDate == "" ? DateTime.UtcNow.ToString("dd-MMM-yyyy") : Convert.ToDateTime(OBDTrack.DocumentReceivedDate).ToString("dd-MMM-yyyy"));
            txtDocRcvdDate.Text = (OBDTrack.DocumentReceivedDate == null || OBDTrack.DocumentReceivedDate == "" ? Convert.ToDateTime(OBDTrack.OBDDate).ToString("dd-MMM-yyyy") : Convert.ToDateTime(OBDTrack.DocumentReceivedDate).ToString("dd-MMM-yyyy"));
            //txtDocRcvdDate.Text = OBDTrack.DocumentReceivedDate.ToString("dd/MM/yyyy");
            hifDeliveredByID.Value = OBDTrack.DeliveredBy.ToString();
            atcDeliveredBy.Text = CommonLogic.GetUserName(OBDTrack.DeliveredBy.ToString());
            txtDriverName.Text = OBDTrack.DriverName;
            txtRcvdBy.Text = OBDTrack.ReceivedBy;
            txtRemBy_DeliveryIncharge.Text = OBDTrack.RemBy_DeliveryIncharge;
            //txtDeliveryDate.Text = (OBDTrack.DeliveryDate == null || OBDTrack.DeliveryDate == "" ? DateTime.UtcNow.ToString("dd-MMM-yyyy") : Convert.ToDateTime(OBDTrack.DeliveryDate).ToString("dd-MMM-yyyy"));
            //txtDeliveryDate.Text = (OBDTrack.DeliveryDate == null || OBDTrack.DeliveryDate == "" ? Convert.ToDateTime(OBDTrack.OBDDate).ToString("dd-MMM-yyyy") : Convert.ToDateTime(OBDTrack.DeliveryDate).ToString("dd-MMM-yyyy"));
            ////txtDeliveryDate.Text = OBDTrack.DeliveryDate.ToString("dd/MM/yyyy");
            //txtReceivedDelTimeEntry.Text = (OBDTrack.DeliveryDate == null || OBDTrack.DeliveryDate == "" ? DateTime.UtcNow.ToString("hh:mm tt") : (Convert.ToDateTime(OBDTrack.DeliveryDate).ToString("hh:mm tt") == "12:00 AM" ? DateTime.UtcNow.ToString("hh:mm tt") : Convert.ToDateTime(OBDTrack.DeliveryDate).ToString("hh:mm tt")));

            //if (OBDTrack.DeliveryDate != "")
            //{
            //    txtDeliveryDate.Text = OBDTrack.DeliveryDate;
            //    txtReceivedDelTimeEntry.Text = OBDTrack.DelivertTime;
            //}
            //else
            //{
            //    txtDeliveryDate.Text = DateTime.UtcNow.Date.ToString("dd-MMM-yyyy");
            //    txtReceivedDelTimeEntry.Text = DB.GetSqlS("SELECT FORMAT((SELECT[dbo].[UDF_GetWarehouseTime] ('" + DateTime.UtcNow.ToString("hh:mm tt") + "'," + WHID + ")),'hh:mm tt') AS S");
            //}


            if (OBDTrack.IsDCRReceived == 1)
                IsDCRReceived.Checked = true;
            String sFileName = CommonLogic._GetAttatchmentFile(TenantRootDir + hifTenant.Value + OutboundPath + OBD_PODPath, OBDTrack.OBDNumber.ToString() + "_" + WHID.ToString());


            if (sFileName != "")
            {
                String Path = "../ViewImage.aspx?path=" + sFileName;
                ltPODAttacthment.Text = "<img src=\"../Images/blue_menu_icons/attachment.png\"  />";
                ltPODAttacthment.Text += "<a style=\"text-decoration:none;\" href=\"#\" onclick=\" OpenImage(' " + Path + " ')  \" > View Delivery Note Doc </a>";
                ltPODAttacthment.Text += "<img src=\"../Images/redarrowright.gif\"   />";
            }
            else
            {
                ltPODAttacthment.Text = "";
            }

        }

        protected void lnkCancelDelivery_Click(object sender, EventArgs e)
        {
            //if (!cp.IsInAnyRoles("5"))
            //{

            //    resetError("Sorry, only Delivery-Incharge can update this delivery", true);
            //    return;

            //}


            ddlReceivingStore.SelectedValue = "0";
            ddlInstructionMode.SelectedValue = "0";
            txtRequester.Text = "";
            txtDocNumber.Text = "";
            txtDocRcvdDate.Text = "";
            atcDeliveredBy.Text = "";
            txtDriverName.Text = "";
            txtRcvdBy.Text = "";
            txtRemBy_DeliveryIncharge.Text = "";


        }

        protected void ddlReceivingStore_SelectedIndexChanged(object sender, EventArgs e)
        {
            pnlReceivedDelivery.Attributes.Add("style", "display:block");
            if (ddlReceivingStore.SelectedItem.Text == "Select Store")
            {

                lblDeliveryStore.Text = "";
            }
            else
            {
                lblDeliveryStore.Text = ddlReceivingStore.SelectedItem.Text;
            }
            //<!--------------Procedure Converting--------------->
            //  int REF_WHID = DB.GetSqlN("select OB_RefWarehouse_DetailsID AS N from OBD_RefWarehouse_Details where IsActive=1 and IsDeleted=0 and WarehouseID=" + ddlReceivingStore.SelectedValue + " and OutboundID=" + OBDTrack.OBDTrackingID.ToString());
            int REF_WHID = DB.GetSqlN("Exec [dbo].[USP_GetREF_WHID]  @WarehouseID = " + ddlReceivingStore.SelectedValue + ", @OutboundID = " + OBDTrack.OBDTrackingID.ToString());

            //int WHID = DB.GetSqlN("Select WarehouseID AS N from OBD_RefWarehouse_Details where OB_RefWarehouse_DetailsID=" + REF_WHID);
            int WHID = DB.GetSqlN("Exec [dbo].[USP_GetWHIDByRefWHID]  @REFWHID=" + REF_WHID);


            // LoadReceivedDeliveryDetails(REF_WHID, WHID);

            lnkAddNewUsedVehicle.Visible = true;

            // int OBD_Tracking_WHID = DB.GetSqlN("Select OutboundTracking_WarehouseID as N from OBD_OutboundTracking_Warehouse Where OutboundID =" + OBDTrack.OBDTrackingID.ToString() + " AND  OB_RefWarehouse_DetailsID=" + REF_WHID);
            int OBD_Tracking_WHID = DB.GetSqlN("Exec [dbo].[USP_GetOBDTrack_WHID] @OutboundID=" + OBDTrack.OBDTrackingID.ToString() + ",@REF_WHID=" + REF_WHID);

            UsedVehicleSql = "EXEC [dbo].[sp_OBD_GetUsedVehicleDetails]  @OutboundTracking_WarehouseID=" + OBD_Tracking_WHID;
            ViewState["UsedVListSQL"] = UsedVehicleSql;
            this.gvUsedVehicle_buildGridData(this.gvUsedVehicle_buildGridData());


        }

        protected void lnkDelivery_Click(object sender, EventArgs e)
        {
            //if (!cp.IsInAnyRoles("5"))
            //{
            //    resetError("Sorry, only Delivery-Incharge can update this delivery", true);
            //    return;
            //}

            Page.Validate("Receivedelivery");

            if (Page.IsValid == false)
            {
                resetError("Please check for mandatory fields", true);
                return;

            }


            if (ddlReceivingStore.SelectedValue == "0")
            {
                resetError("Please select a store", true);
                ddlReceivingStore.Focus();
                return;
            }

            if (IsDCRReceived.Checked && !fucODeliveryConfirmReciept.HasFile)
            {
                resetError("Attach a valid file if 'Is Proof of Delivery (POD) received from the customer?' is checked", true);
                return;
            }

            if (OBDTrack.DeliveryStatusID == 4 && !IsDCRReceived.Checked)
            {
                resetError("Delivery is already made. Please attach the Proof of Delivery (POD) to process this request", true);
                return;
            }

            //<!-------------------Procedure Converting--------------->
            // int REF_WHID = DB.GetSqlN("select OB_RefWarehouse_DetailsID AS N from OBD_RefWarehouse_Details where IsActive=1 and IsDeleted=0 and WarehouseID=" + ddlReceivingStore .SelectedValue + " and OutboundID=" + OBDTrack.OBDTrackingID.ToString());
            int REF_WHID = DB.GetSqlN("Exec [dbo].[USP_GetREF_WHID]  @WarehouseID = " + ddlReceivingStore.SelectedValue + ", @OutboundID = " + OBDTrack.OBDTrackingID.ToString());

            //if (OBDTrack.IsDCRReceived != 0)
            //{
            //    if (DB.GetSqlN("Select DeliveredBy as N from OBD_OutboundTracking_Warehouse Where OutboundID =" + OBDTrack.OBDTrackingID.ToString() + " AND  OB_RefWarehouse_DetailsID=" + REF_WHID) != 0)
            //    {
            //        resetError("The selected store (" + ddlReceivingStore.SelectedItem.Text + ") already delivered  . Please check for another store from the 'Receiving Store' dropdown", true);
            //        return;
            //    }
            //}

            //if (IsDCRReceived.Checked && DB.GetSqlTinyInt("select IsPODReceived AS N from OBD_OutboundTracking_Warehouse where OB_RefWarehouse_DetailsID=" + REF_WHID + " AND OutboundID=" + OBDTrack.OBDTrackingID) == 1)
            //{
            //    resetError("POD already attached", true);
            //    return;
            //}

            OBDTrack.InstructionModeID = Convert.ToInt32(ddlInstructionMode.SelectedValue);
            OBDTrack.Requester = txtRequester.Text;
            OBDTrack.DocumentNumber = txtDocNumber.Text;
            string ShipDate = txtPackedOn.Text.Trim();

            // OBDTrack.DocumentReceivedDate = ((txtDocRcvdDate.Text.Trim() != "") ? DateTime.ParseExact(txtDocRcvdDate.Text.Trim() , "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy") : null);
            OBDTrack.DocumentReceivedDate = ((txtDocRcvdDate.Text.Trim() != "") ? DateTime.ParseExact(txtDocRcvdDate.Text.Trim(), "dd-MMM-yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy") : null);
            // OBDTrack.DocumentReceivedDate = (txtDocRcvdDate.Text != ""  ? DateTime.ParseExact(txtDocRcvdDate.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("dd-MMM-yyyy") : null); 
            OBDTrack.DeliveredBy = Convert.ToInt32(Request.Form[hifDeliveredByID.UniqueID]);
            OBDTrack.DriverName = txtDriverName.Text;
            //OBDTrack.DeliveryDate = Convert.ToString(Convert.ToDateTime(((txtDeliveryDate.Text.Trim() != "" && txtReceivedDelTimeEntry.Text.Trim() != "") ? DateTime.ParseExact(txtDeliveryDate.Text.Trim() + " " + txtReceivedDelTimeEntry.Text.Trim(), "dd-MMM-yyyy hh:mm tt", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy hh:mm tt") : null)).ToUniversalTime());
            OBDTrack.DeliveryDate = ((txtDeliveryDate.Text.Trim() != "" && txtReceivedDelTimeEntry.Text.Trim() != "") ? DateTime.ParseExact(txtDeliveryDate.Text.Trim() + " " + txtReceivedDelTimeEntry.Text.Trim(), "dd-MMM-yyyy hh:mm tt", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy hh:mm tt") : null);
            //OBDTrack.DeliveryDate = ((txtDeliveryDate.Text.Trim() != "" && txtReceivedDelTimeEntry.Text.Trim() != "") ? DateTime.ParseExact(txtDeliveryDate.Text.Trim() + "" + txtReceivedDelTimeEntry.Text.Trim(), "dd-MMM-yyyy hh:mm:tt", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy hh:mm:tt") : null);
            //((txtDeliveryDate.Text.Trim() != "" && txtReceivedDelTimeEntry.Text.Trim() != "") ? DateTime.ParseExact(txtDeliveryDate.Text.Trim() + " " + txtReceivedDelTimeEntry.Text.Trim(), "dd-MMM-yyyy hh:mm tt", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy hh:mm tt") : null);
            //OBDTrack.DeliveryDate = ((txtDeliveryDate.Text.Trim() != "" && txtReceivedDelTimeEntry.Text.Trim() != "") ? DateTime.ParseExact(txtDeliveryDate.Text.Trim() + " " + txtReceivedDelTimeEntry.Text.Trim(), "dd/MM/yyyy hh:mm tt", CultureInfo.InvariantCulture).ToString("dd-MMM-yyyy hh:mm tt") : null);
            OBDTrack.ReceivedBy = txtRcvdBy.Text;
            OBDTrack.RemBy_DeliveryIncharge = txtRemBy_DeliveryIncharge.Text;

            OBDTrack.RefWHID = REF_WHID;

            OBDTrack.TransferedtoStoreID = Convert.ToInt32(ddlReceivingStore.SelectedValue);
            Boolean DCRUploaded = false;

            if (fucODeliveryConfirmReciept.HasFile)
            {
                String FileExtenion = Path.GetExtension(fucODeliveryConfirmReciept.FileName);

                if (FileExtenion == ".pdf")
                {
                    DCRUploaded = true;
                }
                else
                {
                    resetError("Please attach a valid PDF file", true);
                    return;
                }
            }

            if (DCRUploaded)
            {
                OBDTrack.IsDCRReceived = 1;

            }
            else
                OBDTrack.IsDCRReceived = 0;

            OBDTrack.DeliveryStatusID = 4;

            int Result = OBDTrack.UpdateOBDTracking_Delivery();
            if (DCRUploaded && OBDTrack.DeliveryStatusID == 4)
            {
                if (CommonLogic.UploadFile(TenantRootDir + hifTenant.Value + OutboundPath + OBD_PODPath, fucODeliveryConfirmReciept, OBDTrack.OBDNumber + "_" + ddlReceivingStore.SelectedValue + Path.GetExtension(fucODeliveryConfirmReciept.FileName)) == false)
                {
                    resetError("Error while attaching POD", true);
                    return;
                }
                if (OBDTrack.UpdateOBDTracking_Delivery() == -1)
                {
                    resetError("Error while updating delivery details", true);
                    return;
                }
            }


            if (Result == -1)
            {
                resetError("Error while updating delivery details", true);
                return;
            }

            else if (Result == -2)
            {
                String sFileName = CommonLogic._GetAttatchmentFile(TenantRootDir + hifTenant.Value + OutboundPath + OBD_PODPath, OBDTrack.OBDNumber.ToString() + "_" + Convert.ToInt32(ddlReceivingStore.SelectedValue));
                if (sFileName.Contains(".pdf"))
                {
                    LoadReceivedDeliveryDetails(REF_WHID, Convert.ToInt32(ddlReceivingStore.SelectedValue));
                    resetError("POD already attached", true);
                }
                return;
            }

            else if (Result == -3)
            {
                resetError("The selected store (" + ddlReceivingStore.SelectedItem.Text + ") already delivered  . Please check for another store from the 'Receiving Store' dropdown", true);
                return;
            }
            LoadReceivedDeliveryDetails(REF_WHID, Convert.ToInt32(ddlReceivingStore.SelectedValue));
            resetError("Delivery details successfully updated", false);

            lnkAddNewUsedVehicle.Visible = true;

            ValidateOutboundStatus();

            //if (OBDTrack.UpdateOBDTracking_Delivered())
            //{

            //    if (ArrStores.Length > 1)
            //    {
            //        if (DB.GetSqlN("select COUNT(DeliveredBy) AS N from OBD_OutboundTracking_Warehouse where OutboundID=" + OBDTrack.OBDTrackingID.ToString()) == ArrStores.Length)
            //        {
            //            OBDTrack.DeliveryStatusID = 4;
            //        }
            //        else
            //        {
            //            OBDTrack.DeliveryStatusID = 7;
            //        }

            //    }
            //    else
            //    {

            //        OBDTrack.DeliveryStatusID = 4;
            //    }

            //    OBDTrack.UpdateOBDTracking_Delivered();
            //    if (IsDCRReceived.Checked && DB.GetSqlTinyInt("select IsPODReceived AS N from OBD_OutboundTracking_Warehouse where OB_RefWarehouse_DetailsID=" + REF_WHID + " AND OutboundID=" + OBDTrack.OBDTrackingID) == 1)
            //    {
            //        resetError("POD already attached", true);
            //        return;
            //    }

            //    if (IsDCRReceived.Checked)
            //    {
            //        if (fucODeliveryConfirmReciept.HasFile)
            //        {
            //            String FileExtenion = Path.GetExtension(fucODeliveryConfirmReciept.FileName);

            //            if (FileExtenion == ".pdf")
            //            {
            //                if (CommonLogic.UploadFile(TenantRootDir + hifTenant.Value + OutboundPath + OBD_PODPath, fucODeliveryConfirmReciept, OBDTrack.OBDNumber + "_" + ddlReceivingStore.SelectedValue + Path.GetExtension(fucODeliveryConfirmReciept.FileName)) == false)
            //                {

            //                    resetError("Error while attaching POD", true);
            //                    return;
            //                }

            //                DB.ExecuteSQL("update OBD_OutboundTracking_Warehouse set IsPODReceived=1 where OB_RefWarehouse_DetailsID=" + REF_WHID + " AND OutboundID=" + OBDTrack.OBDTrackingID);

            //            }
            //            else
            //            {
            //                resetError("Please attach a valid file", true);
            //            }
            //        }
            //    }


            //    LoadReceivedDeliveryDetails(REF_WHID, Convert.ToInt32(ddlReceivingStore.SelectedValue));

            //    resetError("Delivery details successfully updated", false);

            //}
            //lnkAddNewUsedVehicle.Visible = true;
        }

        protected void lnkCancelRecvfromSystem_Click(object sender, EventArgs e)
        {
            //if (!cp.IsInAnyRoles("5"))
            //{

            //    resetError("Sorry, only Delivery-Incharge can update this delivery", true);
            //    return;

            //}
        }

        private void LoadInstructionMode(DropDownList pddlInstructionMode)
        {

            pddlInstructionMode.Items.Clear();

            pddlInstructionMode.Items.Add(new ListItem("Select Instruction Mode", "0"));
            IDataReader rsInstructionMode = DB.GetRS("Select InstructionModeID, InstructionMode from OBD_InstructionMode Where IsActive =1 and IsDeleted=0 ");

            while (rsInstructionMode.Read())
            {
                pddlInstructionMode.Items.Add(new ListItem(rsInstructionMode["InstructionMode"].ToString(), rsInstructionMode["InstructionModeID"].ToString()));

            }
            rsInstructionMode.Close();



        }



        #endregion ------ Received Delivery --------


        #region -----  Used Vehicle Grid ------


        protected DataSet gvUsedVehicle_buildGridData()
        {
            string sql = ViewState["UsedVListSQL"].ToString();
            DataSet ds = DB.GetDS(sql, false);
            return ds;
        }

        protected void gvUsedVehicle_buildGridData(DataSet ds)
        {
            gvUsedVehicle.DataSource = ds.Tables[0];
            gvUsedVehicle.DataBind();
            ds.Dispose();

        }

        protected void lnkAddNewUsedVehicle_Click(object sender, EventArgs e)
        {
            //if (!cp.IsInAnyRoles("5"))
            //{

            //    resetError("Sorry, only Delivery-Incharge can update this delivery", true);
            //    return;

            //}


            ViewState["gvUsedVIsInsert"] = false;
            //Procedure Converting------------>
            //  int REF_WHID = DB.GetSqlN("select OB_RefWarehouse_DetailsID AS N from OBD_RefWarehouse_Details where IsActive=1 and IsDeleted=0 and WarehouseID=" + ddlStores.SelectedValue + " and OutboundID=" + OBDTrack.OBDTrackingID.ToString());
            int REF_WHID = DB.GetSqlN("Exec [dbo].[USP_GetREF_WHID]  @WarehouseID = " + ddlStores.SelectedValue + ", @OutboundID = " + OBDTrack.OBDTrackingID.ToString());

            //int OBD_Tracking_WHID = DB.GetSqlN("Select OutboundTracking_WarehouseID as N from OBD_OutboundTracking_Warehouse Where OutboundID =" + OBDTrack.OBDTrackingID.ToString() + " AND  OB_RefWarehouse_DetailsID=" + REF_WHID);
            int OBD_Tracking_WHID = DB.GetSqlN("Exec [dbo].[USP_GetOBDTrack_WHID] @OutboundID=" + OBDTrack.OBDTrackingID.ToString() + ",@REF_WHID=" + REF_WHID);

            if (OBD_Tracking_WHID == 0)
            {
                resetError("Please update delivery details", true);
                return;
            }

            StringBuilder sql = new StringBuilder(2500);

            /*
            sql.Append("INSERT INTO OBD_UsedEquipment (OutboundTracking_WarehouseID,CreatedBy) values (");
            sql.Append(OBD_Tracking_WHID+","+CreatedBy);
            sql.Append(")");*/



            try
            {
                /*
                DB.ExecuteSQL(sql.ToString());

                gvUsedVehicle.EditIndex = gvUsedVehicle.Rows.Count;*/

                gvUsedVehicle.EditIndex = 0;
                gvUsedVehicle.PageIndex = 0;
                DataSet dsUsedVehicle = this.gvUsedVehicle_buildGridData();
                DataRow row = dsUsedVehicle.Tables[0].NewRow();
                row["EquipmentID"] = 0;
                row["UsedEquipmentID"] = 0;
                dsUsedVehicle.Tables[0].Rows.InsertAt(row, 0);



                UsedVehicleSql = "EXEC [dbo].[sp_OBD_GetUsedVehicleDetails]  @OutboundTracking_WarehouseID=" + OBD_Tracking_WHID;
                ViewState["UsedVListSQL"] = UsedVehicleSql;
                this.gvUsedVehicle_buildGridData(dsUsedVehicle);

                // this.resetError("New Used Vehicle line item added", false);
                ViewState["gvUsedVIsInsert"] = true;
                ViewState["gvUVItemNotUpdated"] = true;

                lnkAddNewUsedVehicle.Visible = false;
            }
            catch (Exception ex)
            {
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
                this.resetError("Error while inserting new used vehicle line item", true);

            }



        }

        protected void gvUsedVehicle_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.DataItem == null)
                return;

            if ((e.Row.RowState & DataControlRowState.Edit) == DataControlRowState.Edit)
            {

                DropDownList ddlEquipment = (DropDownList)e.Row.FindControl("ddlEquipmentType");
                Literal vltEquipmentID = (Literal)e.Row.FindControl("ltEquipmentID");
                CommonLogic.LoadEquipment(ddlEquipment, "Select Equipment Type", "0", cp.TenantID);
                ddlEquipment.SelectedValue = vltEquipmentID.Text;

                ddlEquipment.Focus();


            }
        }

        protected void gvUsedVehicle_RowCommand(object sender, GridViewCommandEventArgs e)
        {

            if (e.CommandName == "DeleteItem")
            {
                ViewState["gvUsedVIsInsert"] = false;
                gvUsedVehicle.EditIndex = -1;
                int iden = Localization.ParseNativeInt(e.CommandArgument.ToString());
                this.deleteRowPermUsedV(iden);
            }
        }

        protected void gvUsedVehicle_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            //if (!cp.IsInAnyRoles("5"))
            //{

            //    resetError("Sorry, only Delivery Incharge can update this delivery", true);
            //    return;

            //}

            ViewState["gvUsedVIsInsert"] = false;
            GridViewRow row = gvUsedVehicle.Rows[e.RowIndex];

            if (row != null)
            {

                string ltPrEquipmentID = ((Literal)row.FindControl("lthidUsedVehicleID")).Text;
                string ddlEquipmentID = ((DropDownList)row.FindControl("ddlEquipmentType")).SelectedValue.ToString();
                string vtxtUsedValue = ((TextBox)row.FindControl("txtUsedValue")).Text;

                StringBuilder sqlReqVehcl = new StringBuilder(2500);

                /*
                sqlReqVehcl.Append("UPDATE OBD_UsedEquipment SET ");
                sqlReqVehcl.Append("EquipmentID=" + ddlEquipmentID + ",");
                sqlReqVehcl.Append("UsedValue=" + vtxtUsedValue);
                sqlReqVehcl.Append("  WHERE UsedEquipmentID=" + ltPrEquipmentID);*/

                //<!----------------Procedure Converting----------->
                //int REF_WHID = DB.GetSqlN("select OB_RefWarehouse_DetailsID AS N from OBD_RefWarehouse_Details where IsActive=1 and IsDeleted=0 and WarehouseID=" + ddlStores.SelectedValue + " and OutboundID=" + OBDTrack.OBDTrackingID.ToString());
                int REF_WHID = DB.GetSqlN("Exec [dbo].[USP_GetREF_WHID]  @WarehouseID = " + ddlStores.SelectedValue + ", @OutboundID = " + OBDTrack.OBDTrackingID.ToString());

                // int OBD_Tracking_WHID = DB.GetSqlN("Select OutboundTracking_WarehouseID as N from OBD_OutboundTracking_Warehouse Where OutboundID =" + OBDTrack.OBDTrackingID.ToString() + " AND  OB_RefWarehouse_DetailsID=" + REF_WHID);
                int OBD_Tracking_WHID = DB.GetSqlN("Exec [dbo].[USP_GetOBDTrack_WHID] @OutboundID=" + OBDTrack.OBDTrackingID.ToString() + ",@REF_WHID=" + REF_WHID);


                sqlReqVehcl.Append(" DECLARE @NewUpdateoutboundSOID int;  EXEC [dbo].[sp_OBD_UpsertUsedEquipment]   @UsedEquipmentID=" + ltPrEquipmentID + ",@OutboundTracking_WarehouseID =" + OBD_Tracking_WHID + ",@EquipmentID=" + ddlEquipmentID + ",@UsedValue=" + vtxtUsedValue + ",@CreatedBy=" + CreatedBy + ",@NewUsedEquipmentID=@NewUpdateoutboundSOID OUTPUT; select @NewUpdateoutboundSOID AS N");

                try
                {
                    DB.ExecuteSQL(sqlReqVehcl.ToString());
                    this.resetError("Successfully Updated", false);
                    gvUsedVehicle.EditIndex = -1;
                    ViewState["gvUVItemNotUpdated"] = false;
                    this.gvUsedVehicle_buildGridData(this.gvUsedVehicle_buildGridData());
                    lnkAddNewUsedVehicle.Visible = true;
                }
                catch (Exception ex)
                {
                    ViewState["gvUVItemNotUpdated"] = true;
                    CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
                    this.resetError("Error while updating Used Vehicle details", true);
                }




            }
        }

        protected void gvUsedVehicle_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            /*
            if (Localization.ParseBoolean(ViewState["gvUsedVIsInsert"].ToString()) )
            {
                GridViewRow row = gvUsedVehicle.Rows[e.RowIndex];
                if (row != null)
                {
                    int iden = Convert.ToInt32(((Literal)row.FindControl("lthidUsedVehicleID")).Text);
                    deleteRowPermUsedV(iden);
                    lnkAddNewUsedVehicle.Visible = true;
                }
            }

            ViewState["gvUsedVIsInsert"] = false;
            */
            gvUsedVehicle.EditIndex = -1;
            this.gvUsedVehicle_buildGridData(this.gvUsedVehicle_buildGridData());
            lnkAddNewUsedVehicle.Visible = true;

        }

        protected void gvUsedVehicle_RowEditing(object sender, GridViewEditEventArgs e)
        {
            ViewState["gvUsedVIsInsert"] = false;
            gvUsedVehicle.EditIndex = e.NewEditIndex;
            ViewState["gvUVItemNotUpdated"] = false;
            this.gvUsedVehicle_buildGridData(this.gvUsedVehicle_buildGridData());
            lnkAddNewUsedVehicle.Visible = false;
        }

        protected void lnkDeleteUsedVItem_Click(object sender, EventArgs e)
        {
            //if (!cp.IsInAnyRoles("5"))
            //{

            //    resetError("Sorry, only Delivery-Incharge can update this delivery", true);
            //    return;

            //}

            string rfidIDs = "";

            bool chkBox = false;

            StringBuilder sqlDeleteString = new StringBuilder();
            sqlDeleteString.Append("BEGIN TRAN ");

            //'Navigate through each row in the GridView for checkbox items
            foreach (GridViewRow gv in gvUsedVehicle.Rows)
            {

                CheckBox isDelete = (CheckBox)gv.FindControl("chkIsDeleteUsedVItem");
                if (isDelete == null)
                    return;
                if (isDelete.Checked)
                {
                    chkBox = true;
                    // Concatenate GridView items with comma for SQL Delete
                    rfidIDs = ((Literal)gv.FindControl("lthidUsedVehicleID")).Text.ToString();



                    if (rfidIDs != "")
                        sqlDeleteString.Append(" DELETE from OBD_UsedEquipment where UsedEquipmentID=" + rfidIDs + ";");

                }
            }


            sqlDeleteString.Append(" COMMIT ");


            // Execute SQL Query only if checkboxes are checked to avoid any error with initial null string
            try
            {

                if (chkBox)
                {
                    DB.ExecuteSQL(sqlDeleteString.ToString());
                    resetError("Successfully deleted the selected line items", false);
                }


            }
            catch (Exception ex)
            {
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
                resetError("Error deleteing the selected line items", true);
            }

            this.gvUsedVehicle_buildGridData(this.gvUsedVehicle_buildGridData());


        }

        protected void deleteRowPermUsedV(int iden)
        {
            StringBuilder sql = new StringBuilder(2500);
            sql.Append("DELETE from OBD_UsedEquipment where UsedEquipmentID=" + iden.ToString());
            try
            {
                DB.ExecuteSQL(sql.ToString());
                this.gvUsedVehicle_buildGridData(this.gvUsedVehicle_buildGridData());

            }
            catch (Exception ex)
            {
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
                this.resetError("Error deleteing the selected line items", true);
            }
        }
        #endregion -----  Used Vehicle Grid ------





        #region ---- Pending Goods-Out List --------


        protected DataSet gvPendingGoodsOutList_buildGridData()
        {
            string sql = ViewState["PendingGoodsOutListSQL"].ToString();
            DataSet ds = DB.GetDS(sql, false);
            return ds;
        }

        protected void gvPendingGoodsOutList_buildGridData(DataSet ds)
        {
            gvPendingGoodsOutList.DataSource = ds.Tables[0];
            gvPendingGoodsOutList.DataBind();
            ds.Dispose();

            ScriptManager.RegisterStartupScript(this, this.GetType(), "unblockDialog", "unblockDialog();", true);


        }

        protected void gvPendingGoodsOutList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvPendingGoodsOutList.PageIndex = e.NewPageIndex;
            gvPendingGoodsOutList.EditIndex = -1;
            this.gvPendingGoodsOutList_buildGridData(this.gvPendingGoodsOutList_buildGridData());
        }


        #endregion ---- Pending Goods-Out List --------

        protected void lnkViewPendingGoodsOutList_Click(object sender, EventArgs e)
        {
            gvPendingGoodsOutList_buildGridData(gvPendingGoodsOutList_buildGridData());
        }



        #region ------------ Packing Details --------------


        protected void ddlPackingStores_SelectedIndexChanged(object sender, EventArgs e)
        {

            try
            {
                //<!-----------Procedure Converting------------->
                // int REF_WHID = DB.GetSqlN("select OB_RefWarehouse_DetailsID AS N from OBD_RefWarehouse_Details where IsActive=1 and IsDeleted=0 and WarehouseID=" + ddlPackingStores.SelectedValue + " and OutboundID=" + OBDTrack.OBDTrackingID.ToString());
                int REF_WHID = DB.GetSqlN("Exec [dbo].[USP_GetREF_WHID]  @WarehouseID = " + ddlPackingStores.SelectedValue + ", @OutboundID = " + OBDTrack.OBDTrackingID.ToString());

                // int WHID = DB.GetSqlN("Select WarehouseID AS N from OBD_RefWarehouse_Details where OB_RefWarehouse_DetailsID=" + REF_WHID);
                int WHID = DB.GetSqlN(" Exec[dbo].[USP_GetWHIDByRefWHID]  @REFWHID=" + REF_WHID);


                LoadPackingDetails(REF_WHID, WHID);
            }
            catch (Exception ex)
            {
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
            }



        }

        public void LoadPackingDetails(int Ref_WID, int WHID)
        {

            try
            {

                if (OBDTrack.DeliveryStatusID == 6)
                    lnkPackButCancel.Visible = true;


                IDataReader drPacking = DB.GetRS(" select PackedBy,PackingRemarks,PackedOn from OBD_OutboundTracking_Warehouse where IsActive=1 AND IsDeleted=0 AND  OutboundID=" + OBDTrack.OBDTrackingID + " AND OB_RefWarehouse_DetailsID=" + Ref_WID);

                if (drPacking.Read())
                {

                    ddlPackingStores.SelectedValue = WHID.ToString();

                    hifPackedByID.Value = DB.RSFieldInt(drPacking, "PackedBy").ToString();
                    txtPackedBy.Text = CommonLogic.GetUserName(DB.RSFieldInt(drPacking, "PackedBy").ToString());

                    txtPackingRemarks.Text = DB.RSField(drPacking, "PackingRemarks");


                    if (DB.RSFieldDateTime(drPacking, "PackedOn") != DateTime.MinValue)
                    {
                        txtPackedOn.Text = DB.RSFieldDateTime(drPacking, "PackedOn").ToString("dd-MMM-yyyy");
                        txtPackedOnTime.Text = DB.RSFieldDateTime(drPacking, "PackedOn").ToString("hh:mm tt");
                    }
                    else
                    {
                        txtPackedOn.Text = DateTime.UtcNow.ToString("dd-MMM-yyyy");
                        txtPackedOnTime.Text = DateTime.UtcNow.ToString("hh:mm tt");
                    }
                }

                drPacking.Close();


            }
            catch (Exception ex)
            {

                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
                resetError("Error while loading packing details", true);
            }



        }

        protected void lnkPackButCancel_Click(object sender, EventArgs e)
        {

            try
            {


                ddlPackingStores.SelectedIndex = 0;
                txtPackedOn.Text = "";
                txtPackedOnTime.Text = "";
                txtPackedBy.Text = "";
                hifPackedByID.Value = "";

            }
            catch (Exception ex)
            {
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
            }

        }

        protected void lnkPackButSave_Click(object sender, EventArgs e)
        {

            try
            {
                Page.Validate("PackingDetails");

                if (!Page.IsValid)
                {
                    resetError("Please check for mandatory fields", true);
                    return;
                }

                if (ddlPackingStores.SelectedValue == "0")
                {
                    resetError("Please select a store", true);
                    ddlPackingStores.Focus();
                    return;
                }

                if (CommonLogic.GetUserName(OBDTrack.PGIDoneBy.ToString()) == "")
                {
                    resetError("PGI not yet performed", true);
                    return;
                }
                String ShipmentReceivedDate;

                string ShipDate = txtPackedOn.Text.Trim();

                OBDTrack.PackedOn = ((ShipDate != "" && txtPackedOnTime.Text.Trim() != "") ? DateTime.ParseExact(ShipDate + " " + txtPackedOnTime.Text.Trim(), "dd-MMM-yyyy hh:mm tt", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy hh:mm tt") : null).ToString();
                //  OBDTrack.PackedOn = ((txtPackedOn.Text.Trim() != "" && txtPackedOnTime.Text.Trim() != "") ? DB.SQuote(DateTime.ParseExact(txtPackedOn.Text.Trim() + " " + txtPackedOnTime.Text.Trim(), "dd/MM/yyyy hh:mm tt", CultureInfo.InvariantCulture).ToString("dd-MMM-yyyy hh:mm tt")) : "NULL");
                OBDTrack.PackedBy = (hifPackedByID.Value == "" ? 0 : Convert.ToInt16(hifPackedByID.Value));
                OBDTrack.PackingRemarks = txtPackingRemarks.Text.Trim();
                OBDTrack.StoreID = Convert.ToInt16(ddlStores.SelectedValue);

                int Result = OBDTrack.UpdateOBDTracking_Packing();
                if ((DB.GetSqlN("select DeliveryStatusID as N from OBD_Outbound where OutboundID=" + OBDTrack.OBDTrackingID + " and DocumentTypeID=7")) == 6)
                {
                    DB.ExecuteSQL("update OBD_Outbound set DeliveryStatusID=7 where OutboundID=" + OBDTrack.OBDTrackingID);
                    ValidateOutboundStatus();
                    resetError("Successfully Updated", false);
                    return;
                }

                if (Result == -2)
                {
                    resetError("Packing is already done for the selected  store (" + ddlPackingStores.SelectedItem.Text + ") . Please check for another store from the 'Packing Store' dropdown", true);
                    return;
                }

                //if (OBDTrack.DeliveryStatusID == 7)
                //{
                //    resetError("Packing details already updated", true);
                //    return;
                //}

                //if( CommonLogic.GetUserName(OBDTrack.PGIDoneBy.ToString())=="")
                //{
                //    resetError("PGI not yet performed", true);
                //    return;
                //}

                //int REF_WHID = DB.GetSqlN("select OB_RefWarehouse_DetailsID AS N from OBD_RefWarehouse_Details where IsActive=1 and IsDeleted=0 and WarehouseID=" + ddlPackingStores.SelectedValue + " and OutboundID=" + OBDTrack.OBDTrackingID.ToString());

                //if (DB.GetSqlN("Select PackedBy as N from OBD_OutboundTracking_Warehouse Where OutboundID =" + OBDTrack.OBDTrackingID.ToString() + " AND  OB_RefWarehouse_DetailsID=" + REF_WHID) != 0)
                //{
                //    resetError("Packing is already done for the selected  store (" + ddlPackingStores.SelectedItem.Text + ") . Please check for another store from the 'Packing Store' dropdown", true);
                //    return;
                //}

                //char[] seps = new char[] { ',' };

                //String ReferedWHs = OBDTrack.ReferedStoreIDs.Substring(0, OBDTrack.ReferedStoreIDs.Length - 1);

                //String[] ArrStores = CommonLogic.FilterSpacesInArrElements(ReferedWHs.Split(seps));

                //StringBuilder sbPacking = new StringBuilder();

                //sbPacking.Append("Update  OBD_OutboundTracking_Warehouse   ");
                //sbPacking.Append(" SET PackedOn= " + ((txtPackedOn.Text.Trim() != "" && txtPackedOnTime.Text.Trim() != "") ? DB.SQuote( DateTime.ParseExact(txtPackedOn.Text.Trim() + " " + txtPackedOnTime.Text.Trim(), "dd/MM/yyyy hh:mm tt", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy hh:mm tt") ) : "NULL"));
                //sbPacking.Append(",PackedBy=" + (hifPackedByID.Value == "" ? "NULL" : hifPackedByID.Value));
                //sbPacking.Append(",PackingRemarks="+DB.SQuote(txtPackingRemarks.Text));
                //sbPacking.Append(" where OutboundID="+OBDTrack.OBDTrackingID);
                //sbPacking.Append(" AND OB_RefWarehouse_DetailsID=" + REF_WHID);

                //DB.ExecuteSQL(sbPacking.ToString());

                //if (ArrStores.Length !=0)
                //{
                //    if (DB.GetSqlN("select COUNT(PackedBy) AS N from OBD_OutboundTracking_Warehouse where OutboundID=" + OBDTrack.OBDTrackingID.ToString()) == ArrStores.Length)
                //    {
                //        DB.ExecuteSQL("Update OBD_Outbound set DeliveryStatusID=7 where OutboundID=" + OBDTrack.OBDTrackingID);
                //    }

                //}

                resetError("Successfully Updated", false);

                ValidateOutboundStatus();

                //pnlReceivedDelivery.Visible = true;

            }

            catch (Exception ex)
            {
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
                resetError("Error while updating 'Packing Details'", true);
            }

        }

        protected void lnkPakingSlip_Click(object sender, EventArgs e)
        {

            try
            {


                //Response.Redirect("DeliveryPickNote.aspx?obdid=" + OBDTrack.OBDTrackingID + "&TN=" + OBDTrack.TenantName);
                Response.Redirect("DeliveryPackSlip.aspx?obdid=" + OBDTrack.OBDTrackingID);
            }
            catch (Exception ex)
            {
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);

            }


        }


        #endregion ------------ Packing Details --------------


        #region ---- 3pl List --------


        protected DataSet gv3pl_List_buildGridData()
        {
            string sql = ViewState["3plINListSQL"].ToString();
            DataSet ds = DB.GetDS(sql, false);
            return ds;
        }
        protected void gv3pl_List_buildGridData(DataSet ds)
        {
            //gv3pl_list.Columns[5].FooterText = "Sum:" + (from row in ds.Tables[0].AsEnumerable().Where(row => row.Field<object>("TotalCost") != null)
            //                                             select row.Field<decimal>("TotalCost")).Sum().ToString("0.00");
            gv3pl_list.DataSource = ds;
            gv3pl_list.DataBind();
            ds.Dispose();
        }

        protected void lnkadd3pl_Click(object sender, EventArgs e)
        {
            try
            {
                gv3pl_list.EditIndex = 0;
                gv3pl_list.PageIndex = 0;
                DataSet dsCharges = gv3pl_List_buildGridData();
                DataRow row = dsCharges.Tables[0].NewRow();
                row["TenantTransactionAccessorialCaptureID"] = 0;
                dsCharges.Tables[0].Rows.InsertAt(row, 0);
                this.gv3pl_List_buildGridData(dsCharges);
            }
            catch (Exception ex)
            {
                this.resetError("Error while inserting", true);
            }


        }

        protected void gv3pl_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }

        protected void gv3pl_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gv3pl_list.EditIndex = e.NewEditIndex;
            this.gv3pl_List_buildGridData(this.gv3pl_List_buildGridData());
        }

        protected void gv3pl_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            Page.Validate("vgRequired3pl");
            if (!Page.IsValid)
            {
                return;
            }

            GridViewRow row = gv3pl_list.Rows[e.RowIndex];

            if (row != null)
            {
                string TenantTransactionAccessorialCaptureID = ((HiddenField)row.FindControl("hifTenantTransactionAccessorialCaptureID")).Value;
                //string ChargeDetailID = ((HiddenField)row.FindControl("hifChargeDetailID")).Value;
                string ActivityRateID = ((HiddenField)row.FindControl("hifActivityRateID")).Value;
                string qty = ((TextBox)row.FindControl("txtQuantity")).Text;

                if (ActivityRateID == "")
                {
                    resetError("Not a valid Activity Tariff", true);
                    return;
                }

                //if (DB.GetSqlN("select TenantTransactionAccessorialCaptureID N from TPL_Tenant_Transaction_Accessorial_Capture where IsDeleted=0 AND TransactionID=" + CommonLogic.QueryString("obdid") + " AND ActivityRateID=" + ActivityRateID + " AND TransactionTypeID=2 and TenantTransactionAccessorialCaptureID!=" + TenantTransactionAccessorialCaptureID) != 0)
                //{
                //    resetError("Activity Rate already added", true);
                //    return;
                //}

                StringBuilder query = new StringBuilder();

                query.Append("DECLARE @UpdateResult int ; ");
                query.Append(" EXEC [dbo].[sp_TPL_UpsertAccessoryTransactionCapture] ");
                query.Append(" @TransactionTypeID=2,@TransactionID=" + CommonLogic.QueryString("obdid"));
                query.Append(" ,@ActivityRateID=" + ActivityRateID);
                query.Append(" ,@Quantity=" + qty);
                query.Append(" ,@CreatedBy=" + cp.UserID);
                query.Append(" ,@TenantTransactionAccessorialCaptureID=" + CommonLogic.IIF(TenantTransactionAccessorialCaptureID == "", "0", TenantTransactionAccessorialCaptureID));
                query.Append(" ,@Result=@UpdateResult OUTPUT     ;");
                query.Append("  select @UpdateResult AS N");

                try
                {
                    int Result = DB.GetSqlN(query.ToString());

                    if (Result == -3)
                    {
                        resetError("Activity Tariff already added", true);
                        return;
                    }
                    else if (Result == -2)
                    {
                        resetError(" Cannot update, as activity tariff is not within the configured effective dates", true);
                        return;
                    }
                    else if (Result == -1)
                    {
                        resetError("Error while updating", true);
                        return;
                    }
                    else if (Result > 0)
                    {
                        resetError("Successfully updated", false);
                    }

                    ValidateCharge();
                }
                catch (Exception ex)
                {
                    resetError("Error while updating", true);
                }

                gv3pl_list.EditIndex = -1;
                this.gv3pl_List_buildGridData(this.gv3pl_List_buildGridData());
            }
        }

        protected void gv3pl_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gv3pl_list.EditIndex = -1;
            this.gv3pl_List_buildGridData(this.gv3pl_List_buildGridData());
        }

        protected void gv3pl_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gv3pl_list.PageIndex = e.NewPageIndex;
            this.gv3pl_List_buildGridData(this.gv3pl_List_buildGridData());
        }

        protected void btnDelete3pl_Click(object sender, EventArgs e)
        {
            string gvIDs = "";
            bool chkBox = false;
            //'Navigate through each row in the GridView for checkbox items
            foreach (GridViewRow gv in gv3pl_list.Rows)
            {
                CheckBox deleteChkBxItem = (CheckBox)gv.FindControl("deleteRec");
                if (deleteChkBxItem.Checked)
                {
                    chkBox = true;
                    // Concatenate GridView items with comma for SQL Delete
                    gvIDs += ((Label)gv.FindControl("RecID")).Text.ToString() + ",";
                }
            }


            if (chkBox)
            {
                try
                {
                    string deleteSQL = "update  [TPL_Tenant_Transaction_Accessorial_Capture] set IsDeleted=1  where TenantTransactionAccessorialCaptureID in (" + gvIDs.Substring(0, gvIDs.LastIndexOf(",")) + ")";
                    DB.ExecuteSQL(deleteSQL);
                    resetError("Successfully deleted the selected records", false);
                    this.gv3pl_List_buildGridData(this.gv3pl_List_buildGridData());

                    ValidateCharge();
                }
                catch (Exception ex)
                {
                    resetError("Error deleting the selected records", true);
                }
            }
        }
        #endregion ---- 3pl List --------


        [WebMethod]
        public static string GetStoresAssociated(int TenantID)
        {
            DataTable dt = GetTenantStores(TenantID);
            return JsonConvert.SerializeObject(dt);
        }

        public static DataTable GetTenantStores(int TenantID)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Exec USP_WHTenantContDropDown @TenantID=" + TenantID);

            //sb.Append(" select TC.WarehouseID, WH.WHCode FROM TPL_Tenant_Contract TC   ");
            //sb.Append(" JOIN GEN_Warehouse WH ON TC.WarehouseID = WH.WarehouseID AND WH.IsActive = 1 AND WH.IsDeleted = 0   ");
            //sb.Append(" WHERE TenantID = " + TenantID + " AND TC.ISActive = 1 AND TC.IsDeleted = 0 AND GETDATE() BETWEEN EffectiveFrom AND EffectiveTo  ");
            DataTable dt = DB.GetDS(sb.ToString(), false).Tables[0];
            return dt;
        }

        #region
        protected void LoadHHTypes()
        {

            try
            {
                ddlHHType.Items.Clear();

                //IDataReader WHReader = DB.GetRS("select WHCode,INB_R.WarehouseID from OBD_RefWarehouse_Details INB_R Left Join GEN_Warehouse GEN_W on GEN_W.WarehouseID=INB_R.WarehouseID where INB_R.IsDeleted=0 AND INB_R.IsActive=1 and  GEN_W.isActive=1 AND GEN_W.isDeleted=0 AND OutboundID=" + OBDTrack.OBDTrackingID.ToString());
                IDataReader WHReader = DB.GetRS("Exec  USP_OBDHandlingTypeDropDown");

                ddlHHType.Items.Add(new ListItem("Select HandlingType", "0"));

                while (WHReader.Read())
                {
                    ddlHHType.Items.Add(new ListItem(WHReader["HandlingType"].ToString(), WHReader["HandlingTypeID"].ToString()));

                }

                WHReader.Close();
            }
            catch (Exception ex)
            {
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
                resetError("Error while loading Handling Type", true);
                return;
            }

        }
        [WebMethod]
        public static string UpsertPackingSlipAddlnInfo(string HandlingTypeId, string Maxweight, string MaxLength, string MaxWidth, string MaxHeight, string Remarks, string PackageCondition)
        {
            // var UpsertAddln = JsonConvert.DeserializeXmlNode(AddlnData, "root").OuterXml;
            int OutboundID = OBDID;
            string lblOutboundStatus = OutBoundStatus;
            int DelivarySattusID = DB.GetSqlN("select DeliveryStatusID AS N from OBD_Outbound where IsActive=1 AND IsDeleted=0 AND OutboundID=" + OutboundID);

            //CALL VALIDATIONS
            GetValidationInfo();
            //if (lblOutboundStatus == "Sent to Packing" && DelivarySattusID <= 7)
            if (DelivarySattusID <= 7)
            {
                string Maxvolume = (Convert.ToInt32(MaxLength) * Convert.ToInt32(MaxWidth) * Convert.ToInt32(MaxHeight)).ToString();
                // OutboundDetails obd = new OutboundDetails();
                StringBuilder sb = new StringBuilder();
                sb.Append("EXEC [sp_OBD_InsertPackingSlipNumber] @OutBoundId=" + OutboundID + ",@HandlingTypeId = " + HandlingTypeId + ",@Maxweight=" + Maxweight + ",@Maxvolume = " + Maxvolume + ",@Remarks = '" + Remarks + "' ,@PackageCondition = '" + PackageCondition + "', @CreatedBy=" + cp1.UserID);
                DB.ExecuteSQL(sb.ToString());
                //obd.grdPackaging_buildGridData();
                return "";
            }
            else
            {
                //resetError("Delivery challan is already updated. Please check for another store from the 'Packing Store' dropdown", true);
                return "1";
            }

        }

        [WebMethod]
        public static string GetValidationInfo()
        {
            OutboundDetails x = new OutboundDetails();
            //if (CommonLogic.GetUserName(OBDTrack.PGIDoneBy.ToString()) == "")
            //{
            //    resetError("PGI not yet performed", true);
            //    return;
            //}
            int OutboundID = OBDID;
            StringBuilder sb = new StringBuilder();
            //if ((DB.GetSqlN("select DeliveryStatusID as N from OBD_Outbound where OutboundID=" + OutboundID + " and DocumentTypeID=7")) == 6)
            //{
            //    DB.ExecuteSQL("update OBD_Outbound set DeliveryStatusID = 7 where OutboundID = " + OutboundID);

            //    StaticValidateOutboundStatus();

            //    //x.pnlReceivedDelivery.Visible = true;

            //    return "1";
            //}
            //else
            //{
            //    return "-2";
            //}
            return "1";
        }

        [WebMethod]
        public static string UpdatePackingSlipInformation()
        {
            StringBuilder sb = new StringBuilder();
            int OutboundID = OBDID;
            string retVal = "";
            if ((DB.GetSqlN("select DeliveryStatusID as N from OBD_Outbound where OutboundID=" + OutboundID + " and DocumentTypeID in (1,11)")) == 6)
            {   //sb.Append("update OBD_Outbound set DeliveryStatusID = 7 where OutboundID = " + OBDTrack.OBDTrackingID);
                DB.ExecuteSQL("Update OBD_Outbound set DeliveryStatusID=7 where OutboundID=" + OutboundID);
                //return "2";
                //OutBoundStatus = StaticValidateOutboundStatus();
                retVal = "2";
            }
            else if ((DB.GetSqlN("select DeliveryStatusID as N from OBD_Outbound where OutboundID=" + OutboundID + " and DocumentTypeID=7")) == 6)
            {
                DB.ExecuteSQL("update OBD_Outbound set DeliveryStatusID = 7 where OutboundID = " + OutboundID);

                //StaticValidateOutboundStatus();
                //x.pnlReceivedDelivery.Visible = true;

                //return "0";
                retVal = "5";
            }
            else if (DB.GetSqlN("select DeliveryStatusID as N from OBD_Outbound where OutboundID=" + OutboundID) == 7)
            {
                return "6";
            }
            else if (OutBoundStatus == "Delivered")
            {
                // return "1";
                retVal = "1";
            }

            //StaticValidateOutboundStatus();

            return retVal;
        }

        public static string GetValidationsInfo()
        {
            return "";
            //pnlReceivedDelivery.Visible = true;

            //int Result = OBDTrack.UpdateOBDTracking_Packing();
            //if ((DB.GetSqlN("select DeliveryStatusID as N from OBD_Outbound where OutboundID=" + OBDTrack.OBDTrackingID + " and DocumentTypeID=7")) == 6)
            //{
            //    DB.ExecuteSQL("update OBD_Outbound set DeliveryStatusID=7 where OutboundID=" + OBDTrack.OBDTrackingID);
            //    ValidateOutboundStatus();
            //    resetError("Successfully Updated", false);
            //    return;
            //}

            //if (Result == -2)
            //{
            //    resetError("Packing is already done for the selected  store (" + ddlPackingStores.SelectedItem.Text + ") . Please check for another store from the 'Packing Store' dropdown", true);
            //    return;
            //}
        }
        //[WebMethod]
        //public static string UpsertPackingSlipAddlnInfo(string HandlingTypeId, string Maxweight, string MaxLength, string MaxWidth, string MaxHeight, string Remarks, string PackageCondition)
        //{
        //    try
        //    {
        //        // var UpsertAddln = JsonConvert.DeserializeXmlNode(AddlnData, "root").OuterXml;
        //        int OutboundID = OBDID;
        //        string Maxvolume = (Convert.ToInt32(MaxLength) * Convert.ToInt32(MaxWidth) * Convert.ToInt32(MaxHeight)).ToString();
        //        // OutboundDetails obd = new OutboundDetails();
        //        StringBuilder sb = new StringBuilder();
        //        sb.Append("EXEC [sp_OBD_InsertPackingSlipNumber] @OutBoundId=" + OutboundID + ",@HandlingTypeId = " + HandlingTypeId + ",@Maxweight=" + Maxweight + ",@Maxvolume = " + Maxvolume + ",@Remarks = '" + Remarks + "' ,@PackageCondition = '" + PackageCondition + "', @CreatedBy=" + cp1.UserID);
        //        DB.ExecuteSQL(sb.ToString());
        //        //obd.grdPackaging_buildGridData();
        //        return "1";
        //    }
        //    catch(Exception ex)
        //    {
        //        return "0";
        //    }

        //}

        //USP_GET_OBD_PackingSlip_Header_Info

        protected DataSet grdPackaging_buildGridData()
        {
            string sql = ViewState["PackingSlipHListSQL"].ToString();
            DataSet ds = DB.GetDS(sql, false);
            return ds;
        }
        protected void grdPackaging_buildGridData(DataSet ds)
        {
            // gv3pl_list.Columns[5].FooterText = "Sum:" + (from row in ds.Tables[0].AsEnumerable().Where(row => row.Field<object>("TotalCost") != null)
            //  select row.Field<decimal>("TotalCost")).Sum().ToString("0.00");
            //grdPackaging.DataSource = ds;
            //grdPackaging.DataBind();
            //ds.Dispose();
        }

        [WebMethod]
        public static string DeletePackingSlipInfo(string PackingSlipNumber)
        {
            string Status;
            //int ExixtPSN = DB.GetSqlN("select Count(ADR.GEN_MST_Address_ID) AS N from GEN_MST_Addresses ADR join ORD_SODetails SOD on ADR.GEN_MST_Address_ID = SOD.GEN_MST_Address_ID where adr.GEN_MST_Address_ID = " + Convert.ToInt32(PackingSlipNumber));
            //if (ExixtPSN > 0)
            //{
            //    return "Exist";
            //}
            try
            {
                CustomPrincipal customprinciple = HttpContext.Current.User as CustomPrincipal;
                StringBuilder sb = new StringBuilder();
                sb.Append(" Exec [dbo].[USP_DELETE_PACKING_SLIPS]");
                sb.Append("@PSN='" + PackingSlipNumber + "'");
                //sb.Append(",@UpdatedBy=" + customprinciple.UserID.ToString());
                DB.ExecuteSQL(sb.ToString());
                Status = "success";
                //this.grdPackaging_buildGridData(this.grdPackaging_buildGridData());
            }
            catch (Exception ex)
            {
                Status = "Failed";
            }

            return Status;
        }

        //protected void btnDeletePSN_Click(object sender, EventArgs e)
        //{
        //    string gvIDs = "";
        //    //'Navigate through each row in the GridView for checkbox items
        //    foreach (GridViewRow gv in grdPackaging.Rows)
        //    {
        //        CheckBox deleteChkBxItem = (CheckBox)gv.FindControl("chkDelete");
        //        if (deleteChkBxItem.Checked)
        //        {
        //            // Concatenate GridView items with comma for SQL Delete
        //            gvIDs += ((Literal)gv.FindControl("ltHeaderID")).Text.ToString() + ",";
        //            //ValidateCharge();
        //        }

        //    }

        //    if (gvIDs != "")
        //    {
        //        string deleteSQL = " Exec [dbo].[USP_DELETE_PACKING_SLIPS] @PSN = '" + gvIDs.Substring(0, gvIDs.LastIndexOf(",")) + "'";
        //        DB.ExecuteSQL(deleteSQL);
        //        gvIDs = "";
        //        resetError("Successfully deleted the selected records", false);
        //    }
        //    this.grdPackaging_buildGridData(this.grdPackaging_buildGridData());
        //    //if (chkBox)
        //    //{
        //    //    try
        //    //    {
        //    //        string deleteSQL = " Exec [dbo].[USP_DELETE_PACKING_SLIPS] @PSN = '" + gvIDs.Substring(0, gvIDs.LastIndexOf(",")) + "'";
        //    //        DB.ExecuteSQL(deleteSQL);
        //    //        resetError("Successfully deleted the selected records", false);
        //    //        this.grdPackaging_buildGridData(this.grdPackaging_buildGridData());

        //    //        ValidateCharge();
        //    //    }
        //    //    catch (Exception ex)
        //    //    {
        //    //        resetError("Error deleting the selected records", true);
        //    //    }
        //    //}
        //}

        [WebMethod]
        public static string GetPSNMaterial(string Mcode)
        {
            DataTable dt = GetPSNMaterials(Mcode);
            return JsonConvert.SerializeObject(dt);
        }
        public static DataTable GetPSNMaterials(string Mcode)
        {
            int OutboundID = OBDID;
            StringBuilder sb = new StringBuilder();
            sb.Append("Exec USP_OutboundMaterialPickedQty @Item='" + Mcode + "',@OBDID=" + OutboundID);

            //sb.Append(" select TC.WarehouseID, WH.WHCode FROM TPL_Tenant_Contract TC   ");
            //sb.Append(" JOIN GEN_Warehouse WH ON TC.WarehouseID = WH.WarehouseID AND WH.IsActive = 1 AND WH.IsDeleted = 0   ");
            //sb.Append(" WHERE TenantID = " + TenantID + " AND TC.ISActive = 1 AND TC.IsDeleted = 0 AND GETDATE() BETWEEN EffectiveFrom AND EffectiveTo  ");
            DataTable dt = DB.GetDS(sb.ToString(), false).Tables[0];
            return dt;
        }
        [WebMethod]
        public static string GetPackingSlipNumberData()
        {
            DataTable dt = GetPackingSlipNumber();
            return JsonConvert.SerializeObject(dt);
        }

        [WebMethod]
        public static string GetPackingSlipData(int OutboundID)
        {

            // int OutboundID = OBDID;
            StringBuilder sb = new StringBuilder();
            //ViewState["PackingSlipHListSQL"] = "[dbo].[USP_GET_OBD_PackingSlip_Header_Info]  @OutBoundId=" + CommonLogic.IIF(CommonLogic.QueryString("obdid") == "", "0", CommonLogic.QueryString("obdid"));
            //this.grdPackaging_buildGridData(this.grdPackaging_buildGridData());
            sb.Append("select PackingSlipDetailsID,psd.PackingSlipHeaderID,psh.PackingSlipNo,psd.MaterialMasterID,PickedQuantity,PackedQty,"
                     + " (select UOM from GEN_UoM where UOMID=MaterialMaster_UoMID)MaterialMaster_UoMID,PackedUOM,MCode from OBD_PackingSlipDetails psd"
                     + " inner join MMT_MaterialMaster mm on  mm.MaterialMasterID=psd.MaterialMasterID"
                     + " inner join OBD_PackingSlipHeader psh on psd.PackingSlipHeaderID=psh.PackingSlipHeaderID and psh.IsActive=1 and psh.IsDeleted=0"
                     + " where psh.OutboubdID=" + OutboundID + " and  psd.ISDELETED=0 and  psd.ISACTIVE=1 ");
            DataTable dt = DB.GetDS(sb.ToString(), false).Tables[0];
            return JsonConvert.SerializeObject(dt);
        }
        public static DataTable GetPackingSlipNumber()
        {
            int OutboundID = OBDID;
            StringBuilder sb = new StringBuilder();
            //ViewState["PackingSlipHListSQL"] = "[dbo].[USP_GET_OBD_PackingSlip_Header_Info]  @OutBoundId=" + CommonLogic.IIF(CommonLogic.QueryString("obdid") == "", "0", CommonLogic.QueryString("obdid"));
            //this.grdPackaging_buildGridData(this.grdPackaging_buildGridData());
            sb.Append("Exec USP_GET_OBD_PackingSlip_Header_Info @OutBoundId=" + OutboundID);
            DataTable dt = DB.GetDS(sb.ToString(), false).Tables[0];
            return dt;
        }

        [WebMethod]
        public static string UpsertPackingSlipAddMaterialInfo(string PSNMaterial, string PickedQty, string PackedQty, string PSNHeaderId, string PackedUOM, string Itemvolume, string Itemweight)
        {
            // var UpsertAddln = JsonConvert.DeserializeXmlNode(AddlnData, "root").OuterXml;
            int OutboundID = OBDID;
            // OutboundDetails obd = new OutboundDetails();
            StringBuilder sb = new StringBuilder();
            sb.Append("EXEC [sp_OBD_InsertPackingSlipNumberMaterialNumber] @PSNHeaderId=" + PSNHeaderId + ",@Material='" + PSNMaterial + "',@PickedQty=" + PickedQty + ",@PackedQty = " + PackedQty + ", @CreatedBy=" + cp1.UserID + ", @PackedUOM='" + PackedUOM + "', @ItemVolume='" + Itemvolume + "', @ItemWeight='" + Itemweight + "'");
            DB.ExecuteSQL(sb.ToString());
            //obd.grdPackaging_buildGridData();
            return "";

        }
        [WebMethod]
        public static string GetPSNMaterialDetails(string PSNHeaderId)
        {
            DataTable dt = GetPSMaterialDetails(PSNHeaderId);
            return JsonConvert.SerializeObject(dt);
        }
        public static DataTable GetPSMaterialDetails(string PSNHeaderId)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Exec USP_GET_OBD_PackingSlip_Material_Info @PSNHeaderId=" + PSNHeaderId);
            DataTable dt = DB.GetDS(sb.ToString(), false).Tables[0];
            return dt;
        }

        [WebMethod]
        public static string DeletePSNMaterialDetails(int detId)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("EXEC [dbo].[USP_Delete_PSN_Material_Details] @PSDetailsID=" + detId);
            DB.ExecuteSQL(sb.ToString());
            return "";
        }
        [WebMethod]
        public static string DeletePSNDetails(string IDs)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("EXEC [dbo].[USP_DELETE_PACKING_SLIPS] @PSN= '" + IDs.Substring(0, IDs.LastIndexOf(",")) + "'");
            DB.ExecuteSQL(sb.ToString());
            return "";
        }

        [WebMethod]
        public static string[] LoadPSNMaterialItems(int OBDID)
        {
            List<string> mmList = new List<string>();
            string mmSql = "Exec USP_OutboundMaterialDropDown @OBDID =" + OBDID + ",@prefix =''";
            IDataReader rsMCodeList = DB.GetRS(mmSql);

            while (rsMCodeList.Read())
            {
                mmList.Add(string.Format("{0},{1}", rsMCodeList["Mcode"], rsMCodeList["MaterialMasterID"]));
            }

            rsMCodeList.Close();
            return mmList.ToArray();
        }


        [WebMethod]
        public static string GetPickMaterial(int OutboundID)
        {
            string Query = "EXEC USP_OutboundMaterialDropDown1 @OBDID =" + OutboundID + "";
            DataTable dt = DB.GetDS(Query, false).Tables[0];
            return JsonConvert.SerializeObject(dt);
        }
        #endregion
    }
}