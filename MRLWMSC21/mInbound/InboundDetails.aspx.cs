using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using MRLWMSC21Common;
using Microsoft.Web;
using System.Text;
using System.IO;
using System.Globalization;
using System.Security.Principal;
using System.Net.Mail;
using System.Net;
using System.Threading;
using System.Web.Services;
using System.Web.UI.HtmlControls;
using Newtonsoft.Json;
using System.Collections;
//using System.Text;

// Module Name : InboundDetails Under Inbound
// Usecase Ref.: Inbound shipment_UC_011 
// DevelopedBy : Naresh P
// CreatedOn   : 25/10/2013
// Modified On : 25/03/2015



namespace MRLWMSC21.mInbound
{
    public partial class InboundDetails : System.Web.UI.Page
    {

        int TenantID = 0;
        int RequestedBy = 0;
        int CreatedBy = 0;
        protected InboundTrack IBTrack;
        protected int InboundID = 0;
        public CustomPrincipal cp = HttpContext.Current.User as CustomPrincipal;
        string ProjectedVehicleSql = "";
        string ActualVehicleSql = "";
        string PoInvoiceSql = "";
        string DiscrepancySql = "";
        string LoginUserID = "";
        string TenantRootDir = "";
        string InboundPath = "";
        string InbShpDelNotePath = "";
        string InbShpVerifNotePath = "";
        string InbDescPath = "";
        string TenantCurrency = "";
        int SupplierID = 0;

        ArrayList categoryIDList = new ArrayList();
        // Set page theme

        public static CustomPrincipal customPriciple = null;
        protected void Page_PreInit(object sender, EventArgs e)
        {
            Page.Theme = "Inbound";
        }

        protected void Page_Init(object sender, EventArgs e)
        {

            cp = HttpContext.Current.User as CustomPrincipal;
            CreatedBy = cp.UserID;
            TenantID = cp.TenantID;
            RequestedBy = cp.UserID;
            LoginUserID = cp.UserID.ToString();
            ViewState["LoginUserID"] = TenantID;
            ViewState["TenantID"] = TenantID;

            // Load warehouses
            LoadWHCheckBoxList();

            if (CommonLogic.QueryString("ibdid") != "")
            {
                InboundID = Convert.ToInt32(CommonLogic.QueryString("ibdid"));
                if (DB.GetSqlN("select InboundID as N from INB_Inbound where IsActive=1 and IsDeleted=0  and InboundID=" + InboundID) != 0)
                {
                    IBTrack = new InboundTrack(InboundID);

                    if (IBTrack != null)
                        hifShipmentType.Value = IBTrack.ShipmentTypeID.ToString();
                    else
                        hifShipmentType.Value = "0";

                    //lblInboundStatus.Text = DB.GetSqlS("select inboundstatus as s from INB_InboundStatus where inboundstatusid=" + IBTrack.InBoundStatusID);
                }

               

            }

            //if (CommonLogic.QueryString("ibdid") != "" && CommonLogic.QueryString("edittype") != "")
            //{
            //    //hidAccordionIndex.Value = CommonLogic.QueryString("edittype");
            //}
            //else
            //{
            //    //hidAccordionIndex.Value = "0";
            //}


        }
        
        protected void Page_Load(object sender, EventArgs e)
        {
            
            //hifTenantID.Value = cp.TenantID.ToString();

            //Get Tenant Currency
            //TenantCurrency = DB.GetSqlS(" select GEN_C.Code AS S from GEN_Tenant GEN_T left join GEN_Currency GEN_C on GEN_C.CurrencyID=GEN_T.CurrencyID where GEN_T.TenantID=" + cp.TenantID);
            if (cp.AccountID != 0)
            {
                DataSet dsAccount = DB.GetDS("SELECT AccountID,Account FROM GEN_Account WHERE IsActive=1 AND IsDeleted=0 AND AccountID=" + cp.AccountID, false);
                hdnAccount.Value = dsAccount.Tables[0].Rows[0][0].ToString();
                txtAccount.Text = dsAccount.Tables[0].Rows[0][1].ToString();
                txtAccount.Enabled = false;
            }
            

            TenantCurrency = DB.GetSqlS(" select TOP 1 GEN_C.Code AS S from TPL_Tenant_AddressBook TNT_A left join GEN_Currency GEN_C on GEN_C.CurrencyID=TNT_A.CurrencyID WHERE TNT_A.TenantID=" + (hidTenantID.Value==""?"0":hidTenantID.Value));

            // Set default currency value
            ltClCurrency.Text = TenantCurrency;
            ltFrCurrency.Text = TenantCurrency;

            string query = "EXEC [dbo].[sp_TPL_GetTenantDirectoryInfo] @TypeID=1";

            DataSet dsPath = DB.GetDS(query.ToString(), false);

            TenantRootDir = dsPath.Tables[0].Rows[4][0].ToString();
            InboundPath = dsPath.Tables[0].Rows[0][0].ToString();
            InbShpDelNotePath = dsPath.Tables[0].Rows[2][0].ToString();
            InbShpVerifNotePath = dsPath.Tables[0].Rows[3][0].ToString();
            InbDescPath = dsPath.Tables[0].Rows[1][0].ToString();

            // System Admin, Super Admin, Inbound Initiator, IB Store Incharge, Store Manager, Operator
            if (!cp.IsInAnyRoles(CommonLogic.GetRolesAllowed(CommonLogic.GetRolesAllowedForthisPage("New Inbound"))))
            {
                Response.Redirect("../Login.aspx?eid=6");
            }


            if (CommonLogic.QueryString("ibdid") != "")
            {
                InboundID = Convert.ToInt32(CommonLogic.QueryString("ibdid"));

                lnkCancel.Visible = false;

                lnkSaveInbound.Text = "Update" + CommonLogic.btnfaUpdate;

                lnkCancel.Text = "Clear" + CommonLogic.btnfaClear;

                if (DB.GetSqlN("select InboundStatusID AS N from INB_Inbound where IsActive=1 AND IsDeleted=0 AND InboundID=" + InboundID) >= 26)
                {

                    lnkCancelReceived.Visible = false;
                    lnkCancelVerified.Visible = false;

                }

                if (DB.GetSqlN("select InboundID as N from INB_Inbound where IsActive=1 and IsDeleted=0  and InboundID=" + InboundID) != 0)
                    IBTrack = new InboundTrack(InboundID);
                else
                {
                    resetError("Invalid request", true);
                    return;
                }
                ViewState["LoginUserID"] = TenantID;
                ViewState["TenantID"] = TenantID;
                hifInboundID.Value = InboundID.ToString();
                btnGetnewStrRefNo.Visible = false;
                //  atcSupplier.Enabled = false;

                ValidateCharge();
                //if (IBTrack.StoreRefNumber != "")
                //{
                //    ValidateInboundStatus();
                //}
                
            }
            else
            {
                IBTrack = new InboundTrack();
                lnkCancel.Text = "Clear" + CommonLogic.btnfaClear;
                lnkSaveInbound.Text = "Initiate Shipment " + CommonLogic.btnfaSave;
                lnkAddNewTPLActivity.Visible = false;

            }


            if (CommonLogic.QueryString("ibdid") != "" && CommonLogic.QueryString("ibupdate") == "oknofileuploaderr")
            {
               // resetError("Shipment is updated, but no document is attached", false);
               // lblStatusMessage.Text = "Shipment is updated, but no document is attached";


            }
            else if (CommonLogic.QueryString("ibupdate") == "dbupdateerr")
                lblStatusMessage.Text = "Error: Duplicate Store Ref. No.";
                //resetError("Error: Duplicate Store Ref. No.", true);

            else if (CommonLogic.QueryString("ibupdate") == "updateok")
            {
                //lblStatusMessage.Text = "Successfully updated the inbound received data";
               resetError("Successfully updated", false);
            }


            if (CommonLogic.GoodINStatus(IBTrack.InboundTrackingID) == false)
            {
                ViewState["PendingGoodsINListSQL"] = " EXEC [sp_INV_GetPendingGoodsINList] @InboundID=" + IBTrack.InboundTrackingID;

                DataSet dsGoddsIN = gvPendingGoodsINList_buildGridData();

                if (dsGoddsIN.Tables[0].Rows.Count != 0)
                {
                    lnkViewPendingGoodsINList.Visible = true;
                    lnkViewPendingGoodsINList.OnClientClick = "openDialog('Add/Edit Kit Items', '" + lnkViewPendingGoodsINList.ClientID + "');";
                   // GrnPendingStatus();

                }
                else {
                  //  GrnPendingStatus();
                }

                this.gvPendingGoodsINList_buildGridData(gvPendingGoodsINList_buildGridData());

            }

            if (Session["GrnMessage"]!=null)
            {
                resetError("GRN Updated Successfully", false);
                Session.Remove("GrnMessage");
            }


            if (!IsPostBack)
            {
                divWHCheckBoxList.Focus();
                Page.Validate();

                //Sets the Sub Header
                DesignLogic.SetInnerPageSubHeading(this, "New Inbound");

                LoadPriorityLevel();
                LoadConsignmentNoteType();
                LoadFrieghtCompany();
                LoadWHStores();


                if (CommonLogic.QueryStringNativeInt("ibdid") != 0)
                {
                    LoadIBDetails();
                    //LoadShipmentDetails();
                    //LoadShipmentReceivedDetails();
                    //LoaddockDetails();
                    //LoadRTRValue();



                }

                if (CommonLogic.QueryStringNativeInt("ibdid") != 0)
                {

              


                }
            }
            LoadDockGrid();

        }

        private void ValidateCharge()
        {
            if (IBTrack.InBoundStatusID >= 4)
            {
                if (DB.GetSqlN("select COUNT(*) N from INB_Inbound IBT JOIN TPL_Tenant_Transaction_Accessorial_Capture TAC ON TAC.TransactionID=IBT.InboundID AND TAC.IsActive=1 AND TAC.IsDeleted=0 AND TransactionTypeID=1 WHERE  IBT.IsDeleted=0 AND IBT.IsActive=1 AND IBT.InboundID=" + CommonLogic.QueryString("ibdid")) == 0)
                {
                    string Error = "Activity Tariff details are not specified.";
                    lblChargesMsg.Text = "<font class=\"noticeMsg\">NOTE:</font>&nbsp;&nbsp;&nbsp" + Error + " </font>";
                }
                else
                    lblChargesMsg.Text = "";
            }
        }

        private void ValidateInboundStatus()
        {
            lblInboundStatus.Text = DB.GetSqlS("SELECT InboundStatus S FROM INB_Inbound IBT JOIN INB_InboundStatus INS ON INS.InboundStatusID=IBT.InboundStatusID WHERE IBT.IsDeleted=0 AND IBT.IsActive=1 AND IBT.InboundID=" + CommonLogic.QueryString("ibdid"));
            upnlBasicDetails.UpdateMode = UpdatePanelUpdateMode.Conditional;
            upnlBasicDetails.Update();
            
        }
        private void GrnPendingStatus()
        {
            string query = "select * from INV_GoodsMovementDetails where TransactionDocID=" + CommonLogic.QueryString("ibdid");
            DataSet ds = DB.GetDS(query, false);
            if (ds.Tables[0].Rows.Count != 0)
            {
                string Query = "update INB_Inbound set InboundStatusID=5 where InboundStatusID=3 and InboundID=" + CommonLogic.QueryString("ibdid");
                DataSet _ds = DB.GetDS(Query, false);

                ValidateInboundStatus();


            }

        }

        #region ------------------ Basic Data  ------------------

        protected void LoadRTRValue()
        {
            cp = HttpContext.Current.User as CustomPrincipal;

            try
            {
                if (DB.GetSqlN("select count(*) AS N from ORD_PODetails ORD_POD JOIN ORD_SupplierInvoiceDetails ORD_SUPID ON ORD_SUPID.PODetailsID=ORD_POD.PODetailsID AND ORD_SUPID.IsActive=1 AND ORD_SUPID.IsDeleted=0 JOIN  INB_Inbound_ORD_SupplierInvoice INB_SUPI on INB_SUPI.SupplierInvoiceID=ORD_SUPID.SupplierInvoiceID and ORD_POD.POHeaderID=INB_SUPI.POHeaderID AND  INB_SUPI.IsDeleted=0 and INB_SUPI.IsActive=1 where INB_SUPI.InboundID=" + IBTrack.InboundTrackingID + " and ORD_POD.IsDeleted=0 and ORD_POD.IsActive=1") != 0)
                    hlnkRTR.Text = "RTR [ " + DB.GetSqlN("select count(*) AS N from ORD_PODetails ORD_POD JOIN ORD_SupplierInvoiceDetails ORD_SUPID ON ORD_SUPID.PODetailsID=ORD_POD.PODetailsID AND ORD_SUPID.IsActive=1 AND ORD_SUPID.IsDeleted=0 JOIN  INB_Inbound_ORD_SupplierInvoice INB_SUPI on INB_SUPI.SupplierInvoiceID=ORD_SUPID.SupplierInvoiceID and ORD_POD.POHeaderID=INB_SUPI.POHeaderID AND  INB_SUPI.IsDeleted=0 and INB_SUPI.IsActive=1 where INB_SUPI.InboundID=" + IBTrack.InboundTrackingID + " and ORD_POD.IsDeleted=0 and ORD_POD.IsActive=1") + " ] ";

                hlnkRTR.Text += "&nbsp; <image src=\"../Images/redarrowright.gif\"></image>";
                hlnkRTR.NavigateUrl = "RTReport.aspx?ibdid=" + CommonLogic.QueryStringNativeInt("ibdid")+"&TN="+txtTenant.Text;
            }
            catch (Exception ex)
            {

                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);

            }

        }
        protected void loadPOGRN()
        {
            LoadWHChkList();

         

            int Ref_WHID = DB.GetSqlN("select IB_RefWarehouse_DetailsID  AS N from INB_RefWarehouse_Details where  IsActive=1 and IsDeleted=0  and  InboundID=" + IBTrack.InboundTrackingID.ToString() + "and WarehouseID=" + ddlStores.SelectedValue);

            char[] seps = new char[] { ',' };
            String[] ArrStores = { };
            String ReferedWHs = IBTrack.ReferedStoreIDs;
            if (ReferedWHs != null)
            {
                if (IBTrack.ReferedStoreIDs.EndsWith(","))
                    ReferedWHs = IBTrack.ReferedStoreIDs.Substring(0, IBTrack.ReferedStoreIDs.Length - 1);

                ArrStores = CommonLogic.FilterSpacesInArrElements(ReferedWHs.Split(seps));
            }
            ProjectedVehicleSql = "EXEC [dbo].[sp_INB_GetProjectedVehicleDetails]  @InboundID=" + CommonLogic.QueryString("ibdid");
            ViewState["PrVListSQL"] = ProjectedVehicleSql;
            this.GENPV_buildGridData(this.GENPV_buildGridData());


            ActualVehicleSql = "[dbo].[sp_INB_GetActualVehicleDetails]   @InboundTracking_WarehouseID=" + DB.GetSqlN("select InboundTracking_WarehouseID As N from INB_InboundTracking_Warehouse where IsActive=1 and IsDeleted=0  and InboundID=" + CommonLogic.QueryString("ibdid")) + ", @Ref_WHID=" + CommonLogic.IIF(Ref_WHID != 0, Ref_WHID.ToString(), "0");
            ViewState["AVListSQL"] = ActualVehicleSql;
            this.GENAV_buildGridData(this.GENAV_buildGridData());

            //PoInvoiceSql = "EXEC [dbo].[sp_ORD_GetPOInvoiceDetails]   @InboundID=" + CommonLogic.QueryString("ibdid") + ",@TenantID=" + cp.TenantID;

            PoInvoiceSql = "EXEC [dbo].[sp_ORD_GetPOInvoiceDetails]   @InboundID=" + CommonLogic.QueryString("ibdid") + ",@TenantID=" + IBTrack.TenantID;
            ViewState["POInvListSQL"] = PoInvoiceSql;
            this.POInvoice_buildGridData(this.POInvoice_buildGridData());

            DiscrepancySql = "EXEC  [dbo].[sp_INB_DiscrepancyDetails]   @IBRefered_WHID=" + CommonLogic.IIF(Ref_WHID != 0, Ref_WHID.ToString(), "0");
            ViewState["DiscrepancyListSQL"] = DiscrepancySql;
            this.DiscrepancyDetails_buildGridData(this.DiscrepancyDetails_buildGridData());

            ViewState["TPLInboundChargeSQL"] = "[dbo].[sp_3PL_GetDataforAccessoryTransaction] @TransactionID=" + CommonLogic.IIF(CommonLogic.QueryString("ibdid") == "", "0", CommonLogic.QueryString("ibdid") + ",@ActivityRateTypeID=3") + ",@TenantID=" + hidTenantID.Value;
            this.TPLInboundCharges_buildGridData(this.TPLInboundCharges_buildGridData());


            ViewState["TPLInboundRecvChargeSQL"] = "[dbo].[sp_3PL_GetDataforAccessoryTransaction] @TransactionID=" + CommonLogic.IIF(CommonLogic.QueryString("ibdid") == "", "0", CommonLogic.QueryString("ibdid") + ",@ActivityRateTypeID=2") + ",@TenantID=" + hidTenantID.Value;
            this.TPLInboundReceivingCharges_buildGridData(this.TPLInboundReceivingCharges_buildGridData());

            pnlShipmentDetails.Visible = true;
            pnlPOInvDetails.Visible = true;

            if (DB.GetSqlS("select Convert(nvarchar, ShipmentExpectedDate,105) AS S from INB_Inbound where IsActive=1 and IsDeleted=0 and InboundID=" + IBTrack.InboundTrackingID.ToString()) != "")
            {
                pnlShipmentReceived.Visible = true;
                upnlTPLInboundCharge.Visible = true;
                NewPanelVisible.Visible = true;
                //Added on 03-01-2018
                pnlGRNDetails.Visible = true;

            }

            int DisplayVerificationDetails = DB.GetSqlN("select top 1  InboundTracking_WarehouseID AS N from INB_InboundTracking_Warehouse where IsActive=1 and IsDeleted=0  and InboundID=" + IBTrack.InboundTrackingID.ToString());

            if (DisplayVerificationDetails != 0)
            {
                pnlShipmentReceived.Visible = true;
                lnkAddAVNew.Visible = true;
                LoadShipmentVerificationData();
                //pnlshipmentVerification.Visible = true;
                upnlTPLReceivingCharge.Visible = true;
                LoadDiscrFormDetails();
            }

            if (DB.GetSqlTinyInt("select top 1 HasDiscrepancy AS TI  from INB_InboundTracking_Warehouse where IsActive=1 and IsDeleted=0  and IB_RefWarehouse_DetailsID IN(SELECT IB_RefWarehouse_DetailsID FROM INB_RefWarehouse_Details where IsActive=1 and IsDeleted=0  and InboundID=" + IBTrack.InboundTrackingID.ToString() + ") AND HasDiscrepancy=1") != 0)
            {
                pnlDiscInfo.Visible = true;
                //  chkHasDiscrepancy.Checked = false;
                //  chkHasDiscrepancy.Visible = false;
            }

            if (DB.GetSqlN("select count(ShipmentVerifiedOn) AS N from INB_InboundTracking_Warehouse where IsActive=1 and IsDeleted=0  and InboundID=" + IBTrack.InboundTrackingID.ToString()) == ArrStores.Length)
            {
                pnlGRNDetails.Visible = true;
                //Added on 03-01-2018
                //  pnlshipmentVerification.Visible = true;
            }

            //if (DB.GetSqlN("select GRNDoneBy AS N from INB_Inbound where IsActive=1 and IsDeleted=0  and InboundID=" + IBTrack.InboundTrackingID.ToString()) != 0)
            if (DB.GetSqlN("SELECT IBG.GRNUpdatedBy N FROM INB_Inbound IBT  JOIN INB_GRNUpdate IBG ON IBT.InboundID=IBG.InboundID AND IBG.IsDeleted=0  WHERE IBT.IsDeleted=0 AND IBT.IsActive=1 AND IBT.InboundStatusID=6 AND IBT.InboundID=" + IBTrack.InboundTrackingID.ToString()) != 0)
            {
                pnlGRNDetails.Visible = true;
                //  gvGRNDetails.Columns[5].Visible = false;

                //Added on 03-01-2018
                //pnlshipmentVerification.Visible = true;
            }

            //Added on 03-01-2018

            if (IBTrack.InBoundStatusID == 6 || IBTrack.InBoundStatusID == 26 || IBTrack.InBoundStatusID == 3)
            {
                pnlGRNDetails.Visible = true;
                pnlshipmentVerification.Visible = true;
                pnlDiscInfo.Visible = true;
            }

            if (IBTrack.InBoundStatusID == 2)
            {
                txtShipmentReceivedDate.Text = (System.DateTime.UtcNow).ToString("dd-MMM-yyyy");
            }
        }
        protected void LoadWHChkList()
        {


            IDataReader WHReader = DB.GetRS("select WarehouseID from INB_RefWarehouse_Details where IsActive=1 and IsDeleted=0 and  InboundID=" + IBTrack.InboundTrackingID.ToString());

            int WHID;
            int temp = 0; // Initialize temp Variable
            while (WHReader.Read())
            {

                WHID = DB.RSFieldInt(WHReader, "WarehouseID"); // Get WH ID 

                int ChildCout = divWHCheckBoxList.Controls.Count; // Get Child Count

                for (int i = 1; i < ChildCout; i++)
                {

                    CheckBoxList chkList = (CheckBoxList)divWHCheckBoxList.Controls[i].Controls[0]; // get checkbox list object

                    int innerCount = chkList.Items.Count;

                    for (int j = 0; j < innerCount; j++)
                    {

                        if (chkList.Items[j].Value == WHID.ToString())
                        {
                            chkList.Items[j].Selected = true; // enable checkbox's
                            chkList.Items[j].Enabled = false;
                            temp = i;
                        }
                        else
                        {
                            chkList.Items[j].Enabled = false;
                            temp = i;
                        }
                    }
                    if (temp == 0 || temp != i)
                        ((Panel)divWHCheckBoxList.Controls[i]).Enabled = false;  // disable other checkbox's 

                }

            }

            WHReader.Close();

        }

        protected void LoadIBDetails()
        {
            if (IBTrack.StoreRefNumber != null)
            {
                try
                {
            

                    txtStrRefNo.Text = IBTrack.StoreRefNumber;
                    txtDocRcvDate.Text = IBTrack.DocReceivedDate;
                    txtAccount.Text = IBTrack.Account;
                    hdnAccount.Value = Convert.ToString(IBTrack.AccountID);
                    txtWarehouse.Text = IBTrack.WHName;
                    hdnWarehouse.Value = IBTrack.ReferedStoreIDs;

                    //IDataReader ShipmentReader = DB.GetRS("select * from GEN_ShipmentType where IsActive=1 and IsDeleted=0  and ShipmentTypeID=" + IBTrack.ShipmentTypeID.ToString());

                    //if (ShipmentReader.Read())
                    //{
                    //    atcShipmentType.Text = ShipmentReader["ShipmentType"].ToString();
                    //    hifShipmentType.Value = ShipmentReader["ShipmentTypeID"].ToString();
                    //}

                    //ShipmentReader.Close();

                    atcShipmentType.Text = IBTrack.ShipmentType;
                    hifShipmentType.Value = IBTrack.ShipmentTypeID.ToString();

                    txtTenant.Text = IBTrack.TenantName;
                    hidTenantID.Value = IBTrack.TenantID.ToString();

                    //IDataReader SupplierReader = DB.GetRS("select * from MMT_Supplier where IsActive=1 and IsDeleted=0  and SupplierID=" + IBTrack.Supplier);

                    //if (SupplierReader.Read())
                    //{
                    //    atcSupplier.Text = SupplierReader["SupplierName"].ToString();
                    //    hifSupplier.Value = SupplierReader["SupplierID"].ToString();
                    //    SupplierID = DB.RSFieldInt(SupplierReader, "SupplierID");
                    //}

                    //SupplierReader.Close();


                    atcSupplier.Text = IBTrack.SupplierName;
                    hifSupplier.Value = IBTrack.Supplier.ToString();
                    SupplierID = Convert.ToInt32(IBTrack.Supplier);

                    ddlConsignmentNoteType.SelectedValue = IBTrack.ConsignmentNoteTypeID.ToString();

                    LoadConsignmentNoteValues(IBTrack.ConsignmentNoteTypeID.ToString());
                    txtConsignmentNoteTypeValue.Text = IBTrack.ConsignmentNoteTypeValue;
                    txtConsignmentNoteTypeDate.Text = IBTrack.ConsignmentNoteTypeDate;

                    txtNoofPackages.Text = IBTrack.NoofPackagesInDocument.ToString();
                    txtGrossWeight.Text = IBTrack.GrossWeight;
                    txtCBM.Text = IBTrack.CBM.ToString();

                    //rblChargesRequired.SelectedValue = IBTrack.IsChargesRequired.ToString();



                    //IDataReader ClearenceCompanyReader = DB.GetRS("select * from GEN_ClearanceCompany where IsActive=1 and IsDeleted=0  and ClearanceCompanyID=" + IBTrack.ClearenceCompanyID.ToString());

                    //if (ClearenceCompanyReader.Read())
                    //{
                    //    atcClearenceCompany.Text = ClearenceCompanyReader["ClearanceCompany"].ToString();
                    //    hifClearenceCompany.Value = ClearenceCompanyReader["ClearanceCompanyID"].ToString();
                    //}

                    //ClearenceCompanyReader.Close();


                    atcClearenceCompany.Text = IBTrack.ClearanceCompany;
                    hifClearenceCompany.Value = IBTrack.ClearenceCompanyID.ToString();


                    txtClearenceCompanyInvoice.Text = IBTrack.ClearenceInvoiceNo;
                    txtClearenceInvoiceDate.Text = IBTrack.ClearenceInvoiceDate;
                    txtClearenceCompanyAmount.Text = CommonLogic.IIF(IBTrack.ClearenceAmount.ToString() != "0.000", IBTrack.ClearenceAmount.ToString(), "");


                    ddlFreightCompany.SelectedValue = IBTrack.FreightCompanyID.ToString();
                    txtFreightInvoice.Text = IBTrack.FreightInvoiceNo;
                    txtFreightInvoiceDate.Text = IBTrack.FreightInvoiceDate;

                    txtFreightAmount.Text = CommonLogic.IIF(IBTrack.FreightAmount.ToString() != "0.000", IBTrack.FreightAmount.ToString(), "");

                    ddlPriorityLevel.SelectedValue = IBTrack.PriorityLevel.ToString();



                    if (IBTrack.PriorityDateTime != null)
                    {
                        txtPriorityDate.Text = Convert.ToDateTime(IBTrack.PriorityDateTime).ToString("dd/MM/yyyy");
                        txtTimeEntry.Text = Convert.ToDateTime(IBTrack.PriorityDateTime).ToString("hh:mm tt");
                    }


                    txtRemarks_Ini.Text = IBTrack.RemarksBy_Ini;

                    //String sFileName = CommonLogic._GetAttatchmentFile(TenantRootDir + DB.GetSqlS("select UniqueID AS S from GEN_Tenant where TenantID=" + TenantID) + InboundPath + InbShpDelNotePath, IBTrack.StoreRefNumber);

                    String sFileName = CommonLogic._GetAttatchmentFile(TenantRootDir + IBTrack.TenantID + InboundPath + InbShpDelNotePath, IBTrack.StoreRefNumber);


                    if (sFileName != "")
                    {
                        String Path = "../ViewImage.aspx?path=" + sFileName;
                        divDNAtachment.InnerHtml = "<img src=\"../Images/blue_menu_icons/attachment.png\"  />";
                        divDNAtachment.InnerHtml += "<a style=\"text-decoration:none;\" href=\"#\" onclick=\" OpenImage(' " + Path + " ')  \" > View Shipment Doc. </a>";
                        divDNAtachment.InnerHtml += "<img src=\"../Images/redarrowright.gif\"   />";
                    }

                    LoadShipmentDetails();
                    LoadShipmentReceivedDetails();
                   // LoaddockDetails();
                    LoadRTRValue();
                    loadPOGRN();
                    ValidateInboundStatus();

                    if (gvPOInvoice.Rows.Count != 0 || gvPOInvoice.Rows.Count != 0)
                    {
                        atcSupplier.Enabled = false;
                        txtTenant.Enabled = false;
                        txtDocRcvDate.Enabled = false;
                        txtWarehouse.Enabled = false;
                        atcShipmentType.Enabled = false;
                    }
                }
                catch (Exception ex)
                {
                    CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
                    resetError("Error while loading inbound details", true);
                    return;
                }
            }
            else
            {
                resetError("No Details Available", true);
                return;
            }
        }

        private void LoadPriorityLevel()
        {
            ddlPriorityLevel.Items.Clear();
            ddlPriorityLevel.Items.Add(new ListItem("Normal", "0"));
            ddlPriorityLevel.Items.Add(new ListItem("Medium", "1"));
            ddlPriorityLevel.Items.Add(new ListItem("High", "2"));
            ddlPriorityLevel.Items.Add(new ListItem("Highest", "3"));

        }

        protected void LoadWHCheckBoxList()
        {

            IDataReader WHGroups = DB.GetRS("select WarehouseGroupCode from GEN_WarehouseGroup where  IsActive=1 AND isDeleted=0  group by WarehouseGroupCode");

            while (WHGroups.Read())
            {
                IDataReader WHList = DB.GetRS("select GEN_W.WarehouseID,WHCode from GEN_Warehouse GEN_W  left join GEN_WarehouseGroup GEN_WG on GEN_WG.WarehouseGroupID=GEN_W.WarehouseGroupID where  GEN_W.isActive=1 AND GEN_W.isDeleted=0 AND   GEN_WG.WarehouseGroupCode=" + DB.SQuote(DB.RSField(WHGroups, "WarehouseGroupCode")));

                Panel chkPanels = new Panel();

                CheckBoxList indvWHCheckBoxList = new CheckBoxList();
                indvWHCheckBoxList.RepeatLayout = RepeatLayout.Flow;
               
                Label Prefix = new Label();



                while (WHList.Read())
                {

                    indvWHCheckBoxList.Items.Add(new ListItem(DB.RSField(WHList, "WHCode"), DB.RSFieldInt(WHList, "WarehouseID").ToString()));

                }


                indvWHCheckBoxList.ID = "chk" + WHGroups["WarehouseGroupCode"];
                indvWHCheckBoxList.AutoPostBack = true;
                indvWHCheckBoxList.RepeatColumns = 8;
                indvWHCheckBoxList.CellPadding = 4;
                indvWHCheckBoxList.CellSpacing = 4;                
                indvWHCheckBoxList.RepeatLayout = RepeatLayout.Table;
                indvWHCheckBoxList.ToolTip = WHGroups["WarehouseGroupCode"].ToString();
                indvWHCheckBoxList.SelectedIndexChanged += new EventHandler(chkStoresAssociated_SelectedIndexChanged);

                chkPanels.ID = "pnl" + WHGroups["WarehouseGroupCode"];
                chkPanels.CssClass = "cbpnl";
                // chkPanels.GroupingText = WHGroups["WarehouseGroupCode"].ToString();
                chkPanels.Width = 900;
                //chkPanels.ForeColor = System.Drawing.Color.DarkCyan;


                chkPanels.Controls.Add(indvWHCheckBoxList);


                uPanelWHList.ContentTemplateContainer.Controls.Add(chkPanels);

                AsyncPostBackTrigger trigger = new AsyncPostBackTrigger();

                trigger.ControlID = "chk" + WHGroups["WarehouseGroupCode"];
                trigger.EventName = "SelectedIndexChanged";

                uPanelWHList.Triggers.Add(trigger);


                divWHCheckBoxList.Controls.Add(chkPanels);


                WHList.Close();

                indvWHCheckBoxList = null;
                chkPanels = null;

            }

            WHGroups.Close();


        }

        protected void chkStoresAssociated_SelectedIndexChanged(object sender, EventArgs e)
        {
            CheckBoxList chkitem = ((CheckBoxList)sender);

            String checkedItemID = chkitem.Parent.ID;
            int TotalControlsCountInDiv = divWHCheckBoxList.Controls.Count;
            if (chkitem.SelectedValue == "")
            {
                resetError("Please select atleast one store", true);
                txtStrRefNo.Text = "";
                for (int indvItem = 1; indvItem < TotalControlsCountInDiv; indvItem++)
                    ((Panel)divWHCheckBoxList.Controls[indvItem]).Enabled = true;
            }
            else
            {
                lblStoreStatus.Text = "";

                for (int indvItem = 1; indvItem < TotalControlsCountInDiv; indvItem++)
                    if (divWHCheckBoxList.Controls[indvItem].ID != checkedItemID)
                        ((Panel)divWHCheckBoxList.Controls[indvItem]).Enabled = false;
            }

            btnGetnewStrRefNo.Focus();
            Page.Validate("InitiateShipment");

        }

        protected void btnGetnewStrRefNo_Click(object sender, ImageClickEventArgs e)
        {
            //txtDocRcvDate.Focus();
          
            int ChildCount = divWHCheckBoxList.Controls.Count;
            string WarehouseID = "";
            for (int i = 1; i < ChildCount; i++)
            {
                WarehouseID = ((CheckBoxList)((Panel)divWHCheckBoxList.Controls[i]).Controls[0]).SelectedValue;
                if (WarehouseID != "")
                    break;
            }

            if (WarehouseID != "")
            {
                int StoreRefNoLength = 0;
                IDataReader StrRefNoLengthReader = DB.GetRS("select SysConfigValue from SYS_SystemConfiguration SYS_SC left join SYS_SysConfigKey SYS_SCK on  SYS_SCK.SysConfigKeyID=SYS_SC.SysConfigKeyID where SYS_SCK.SysConfigKey='inbounddetails.aspx.cs.StoreRefNo_Length' and SYS_SCK.IsActive=1");
                if (StrRefNoLengthReader.Read())
                    StoreRefNoLength = Convert.ToInt32(DB.RSField(StrRefNoLengthReader, "SysConfigValue")); // get StoreRefNo length for logged Tenant
                StrRefNoLengthReader.Close();

                string WHGroupCode = "";
                IDataReader WHGroupCodeReader = DB.GetRS("select WarehouseGroupCode from GEN_Warehouse GEN_W  left join GEN_WarehouseGroup GEN_WG on GEN_WG.WarehouseID=GEN_W.WarehouseID where  GEN_WG.WarehouseID=" + WarehouseID);
                if (WHGroupCodeReader.Read())
                    WHGroupCode = WHGroupCodeReader["WarehouseGroupCode"].ToString(); // get WHGroupCode for specific WH
                WHGroupCodeReader.Close();


                //String vStoreRefNUmber = DB.GetSqlS("select top 1  StoreRefNo AS S from INB_Inbound where  TenantID=" + TenantID + " and StoreRefNo like '" + WHGroupCode + "%' order by InboundID DESC "); // Get Previous StoreRefNo

                String vStoreRefNUmber = DB.GetSqlS("select top 1  StoreRefNo AS S from INB_Inbound where  TenantID=" + IBTrack.TenantID + " and StoreRefNo like '" + WHGroupCode + "%' order by InboundID DESC "); // Get Previous StoreRefNo


                String vNewStoreRefNUmber = "";

                try
                {
                    vNewStoreRefNUmber = Convert.ToString(WHGroupCode);  //Assign WHGroupCode

                    vNewStoreRefNUmber += "" + (Convert.ToInt16(DateTime.UtcNow.Year) % 100);           //add year code to StoreRefNUmber
                    //vNewStoreRefNUmber += "" + (2014 % 100);
                    int length = Convert.ToInt32(StoreRefNoLength);                   //get prifix length
                    int power = (Int32)Math.Pow((double)10, (double)(length - 1));          //get minvalue of prifix length
                    String OldStoreRefNumber = "";
                    String newvalue = "";
                    try
                    {
                        OldStoreRefNumber = Convert.ToString(vStoreRefNUmber);           //get last StoreRefNumber if exists 
                    }
                    catch (Exception ex)
                    {
                        CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
                    }

                    if (OldStoreRefNumber != "" && vNewStoreRefNUmber.Equals(OldStoreRefNumber.Substring(0, vNewStoreRefNUmber.Length)))        //get same year StoreRefNo if StoreRefNo is exists
                    {
                        String temp = OldStoreRefNumber.Substring(vNewStoreRefNUmber.Length, length);                            //get number of last prifix
                        Int32 number = Convert.ToInt32(temp);
                        number++;


                        while (power > 1)                                                                           //add '0' to number at left side for get 
                        {
                            if (number / power > 0)
                            {
                                //newvalue += number;
                                break;
                            }
                            newvalue += "0";
                            power /= 10;
                        }
                        newvalue += "" + number;
                    }
                    else
                    {                                                                                           //other wise generate first number 
                        for (int i = 0; i < length - 1; i++)
                            newvalue += "0";
                        newvalue += "1";
                    }

                    vNewStoreRefNUmber += newvalue; // New StoreREfNumber

                    txtStrRefNo.Text = vNewStoreRefNUmber;

                    //txtDocRcvDate.Focus();
                }
                catch (Exception ex)
                {
                    CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
                    resetError("Error while generating Store Ref.#", true);
                }
            }
            else
            {
                txtStrRefNo.Text = "";
                divWHCheckBoxList.Focus();
                resetError("Select atleast one store", true);

            }

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

        protected void lnkCancel_Click(object sender, EventArgs e)
        {

            //if (DB.GetSqlN("select InboundStatusID AS N from INB_Inbound where InboundID=" + IBTrack.InboundTrackingID.ToString()) >= 3)
            //{
            //    lblStatusMessage.Text = " This shipment is already received, cannot modify the shipment details <br/>";
            //    return;
            //}

            //if (IBTrack.InBoundStatusID >= 3)
            //{
            //    lblStatusMessage.Text = " This shipment is already received, cannot modify the shipment details <br/>";
            //    return;
            //}

            //Response.Redirect("InboundSearch.aspx");
            txtDocRcvDate.Text = "";
            atcShipmentType.Text = "";
            txtTenant.Text = "";
            atcSupplier.Text = "";
            ddlConsignmentNoteType.SelectedValue = "0";
            txtConsignmentNoteTypeValue.Text = "";
            txtConsignmentNoteTypeDate.Text = "";
            txtNoofPackages.Text = "";
            txtGrossWeight.Text = "";
            txtCBM.Text = "";
            atcClearenceCompany.Text = "";
            txtClearenceCompanyInvoice.Text = "";
            txtClearenceInvoiceDate.Text = "";
            txtClearenceCompanyAmount.Text = "";
            ddlFreightCompany.SelectedValue = "0";
            txtFreightInvoice.Text = "";
            txtFreightInvoiceDate.Text = "";
            txtFreightAmount.Text = "";
            ddlPriorityLevel.SelectedValue = "0";
            txtPriorityDate.Text = "";
            txtTimeEntry.Text = "";          
            txtRemarks_Ini.Text = "";
            fucInboundDeliveryNote.Attributes.Clear();

        }

        protected void lnkSaveInbound_Click(object sender, EventArgs e)
        {
    
            //if (!cp.IsInAnyRoles("7"))
            //{

            //    resetError("Only IB Initiator can initiate this shipment", true);
            //    return;

            //}

            Page.Validate("InitiateShipment");

            if (Page.IsValid == false)
            {
                //lblStatusMessage.Text = "Please check for mandatory fields";

                if (txtDocRcvDate.Text == "")
                {
                    txtDocRcvDate.Attributes["style"] = "border-bottom: 1.5px solid red !important;";
                }
                else
                {
                    txtDocRcvDate.Attributes["style"] = "border-bottom: 1.5px solid green !important;";
                }

                if(atcShipmentType.Text=="")
                {
                    atcShipmentType.Attributes["style"] = "border-bottom: 1.5px solid red !important;";
                }
                else
                {
                    atcShipmentType.Attributes["style"] = "border-bottom: 1.5px solid green !important;";
                }

                if (txtTenant.Text == "")
                {
                    txtTenant.Attributes["style"] = "border-bottom: 1.5px solid red !important;";
                }
                else
                {
                    txtTenant.Attributes["style"] = "border-bottom: 1.5px solid green !important;";
                }

                if (atcSupplier.Text == "")
                {
                    atcSupplier.Attributes["style"] = "border-bottom: 1.5px solid red !important;";
                }
                else
                {
                    atcSupplier.Attributes["style"] = "border-bottom: 1.5px solid green !important;";
                }

                if (txtAccount.Text == "")
                {
                    txtAccount.Attributes["style"] = "border-bottom: 1.5px solid red !important;";
                }
                else
                {
                    txtAccount.Attributes["style"] = "border-bottom: 1.5px solid green !important;";
                }

                if (txtWarehouse.Text == "")
                {
                    txtWarehouse.Attributes["style"] = "border-bottom: 1.5px solid red !important;";
                }
                else
                {
                    txtWarehouse.Attributes["style"] = "border-bottom: 1.5px solid green !important;";
                }

                resetError("Please check for mandatory fields", true);
                return;
            }


            //if (DB.GetSqlN("select InboundStatusID AS N from INB_Inbound where InboundID=" + IBTrack.InboundTrackingID.ToString()) >= 3)
            //{
            //    resetError("Cannot modify the shipment details, as shipment is received", true);
            //    return;
            //}

            if (IBTrack.InBoundStatusID >= 3)
            {
                resetError("Cannot modify the shipment details, as shipment is received", true);
                return;
            }

            //int SupplierID = Convert.ToInt32(hifSupplier.Value);
            //if (DB.GetSqlN("select top 1 POHeaderID AS N from ORD_POHeader where IsActive=1 and IsDeleted=0 and POStatusID=1 and SupplierID=" + SupplierID) == 0)
            ////if (DB.GetSqlN("select top 1 POHeaderID AS N from ORD_POHeader where IsActive=1 and IsDeleted=0 and POStatusID=1 and SupplierID=" + Request.Form[hifSupplier.UniqueID]) == 0)
            //{
            //    resetError("No open PO'S are configured to this supplier", true);
            //    return;
            //}

            //if (CommonLogic.QueryString("ibdid") != "")
            //{
            //    if (DB.GetSqlN("select top 1 InboundID AS N from INB_Inbound_ORD_SupplierInvoice where IsActive=1 AND IsDeleted=0 AND InboundID=" + CommonLogic.QueryString("ibdid")) != 0)
            //    {
            //        resetError("Once the PO line items are configured, shipment details cannot be modified ", true);
            //        return;
            //    }
            //}

            String RefredStores = "";

            int ChildCount = divWHCheckBoxList.Controls.Count;

            //for (int i = 1; i < ChildCount; i++)
            //{

            //    if (((Panel)divWHCheckBoxList.Controls[i]).Visible)
            //    {
            //        CheckBoxList ChkList = ((CheckBoxList)(divWHCheckBoxList.Controls[i].Controls[0]));

            //        int CheckListCount = ChkList.Items.Count;

            //        for (int j = 0; j < CheckListCount; j++)
            //        {

            //            ListItem vitem = (ListItem)(ChkList.Items[j]);

            //            if (vitem.Selected)
            //            {
            //                RefredStores += vitem.Value + ",";
            //            }
            //        }


            //    }
            //}


            //if (RefredStores.Length > 0)
            //{
            //    RefredStores = RefredStores.Substring(0, RefredStores.Length - 1);
            //}

            //if (RefredStores == "")
            //{
            //    resetError("Please select atleast one store", true);
            //    return;
            //}

            StringBuilder sqlIBUpsert = new StringBuilder(3000);

            try
            {
                RefredStores = hdnWarehouse.Value;
                IBTrack.ReferedStoreIDs = RefredStores;
                IBTrack.StoreRefNumber = txtStrRefNo.Text;
                string updatedon = DateTime.UtcNow.ToString("MM/dd/yyyy hh:mm:ss");
                //DateTime other = DateTime.SpecifyKind(dateTime, DateTimeKind.Utc);

                //string date = txtDocRcvDate.Text + " " + "";
                IBTrack.DocReceivedDate = (txtDocRcvDate.Text != "" ? DateTime.ParseExact(txtDocRcvDate.Text + " " + DateTime.UtcNow.ToString("hh:mm:ss.fff"), "dd-MMM-yyyy hh:mm:ss.fff", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy hh:mm:ss.fff") : null);
                //IBTrack.DocReceivedDate = (txtDocRcvDate.Text != "" ? DateTime.ParseExact(txtDocRcvDate.Text.Trim(), "dd-MMM-yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy") : null);
                //DateTime other = DateTime.SpecifyKind(Convert.ToDateTime(IBTrack.DocReceivedDate), DateTimeKind.Utc);


                IBTrack.ShipmentTypeID = Convert.ToInt32(Request.Form[hifShipmentType.UniqueID]);
                IBTrack.Supplier = Request.Form[hifSupplier.UniqueID];
                IBTrack.ConsignmentNoteTypeID = Convert.ToInt32(CommonLogic.IIF(ddlConsignmentNoteType.SelectedValue != "0", ddlConsignmentNoteType.SelectedValue, "0"));
                IBTrack.ConsignmentNoteTypeValue = CommonLogic.IIF(txtConsignmentNoteTypeValue.Text != "", txtConsignmentNoteTypeValue.Text, "");
                IBTrack.UpdatedBy = cp.UserID;
                IBTrack.UpdatedOn = updatedon;


                IBTrack.ConsignmentNoteTypeDate = (txtConsignmentNoteTypeDate.Text != "" ? DateTime.ParseExact(txtConsignmentNoteTypeDate.Text.Trim(), "dd-MMM-yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy") : null);


                IBTrack.NoofPackagesInDocument = Convert.ToInt32((txtNoofPackages.Text == "" ? "0" : txtNoofPackages.Text));
                IBTrack.GrossWeight = (txtGrossWeight.Text == "" ? "0" : txtGrossWeight.Text);
                IBTrack.CBM = Convert.ToDecimal((txtCBM.Text == "" ? "0" : txtCBM.Text));
                IBTrack.ClearenceCompanyID = Convert.ToInt32(CommonLogic.IIF(Request.Form[hifClearenceCompany.UniqueID].ToString() != "", Request.Form[hifClearenceCompany.UniqueID].ToString(), "0"));
                IBTrack.ClearenceInvoiceNo = txtClearenceCompanyInvoice.Text;


                IBTrack.ClearenceInvoiceDate = (txtClearenceInvoiceDate.Text != "" ? DateTime.ParseExact(txtClearenceInvoiceDate.Text.Trim(), "dd-MMM-yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy") : null);


                IBTrack.ClearenceAmount = Convert.ToDecimal(CommonLogic.IIF(txtClearenceCompanyAmount.Text != "", txtClearenceCompanyAmount.Text, "0"));

                IBTrack.FreightCompanyID = Convert.ToInt32(CommonLogic.IIF(ddlFreightCompany.SelectedValue != "0", ddlFreightCompany.SelectedValue, "0"));
                IBTrack.FreightInvoiceNo = txtFreightInvoice.Text;



                IBTrack.FreightInvoiceDate = (txtFreightInvoiceDate.Text != "" ? DateTime.ParseExact(txtFreightInvoiceDate.Text.Trim(), "dd-MMM-yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy") : null);


                IBTrack.FreightAmount = Convert.ToDecimal(CommonLogic.IIF(txtFreightAmount.Text != "", txtFreightAmount.Text, "0"));

                IBTrack.RemarksBy_Ini = txtRemarks_Ini.Text;
                IBTrack.PriorityLevel = Convert.ToInt32(ddlPriorityLevel.SelectedValue);


                IBTrack.PriorityDateTime = ((txtPriorityDate.Text.Trim() != "" && txtTimeEntry.Text.Trim() != "") ? DateTime.ParseExact(txtPriorityDate.Text.Trim() + " " + txtTimeEntry.Text.Trim(), "dd-MMM-yyyy hh:mm tt", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy hh:mm tt") : null);

                IBTrack.CreatedBy = CreatedBy;
                //IBTrack.TenantID = TenantID;

                IBTrack.TenantID = Convert.ToInt16(hidTenantID.Value);




                if (CommonLogic.QueryString("ibdid") == "")
                {


                    int SQlCheckIfIBExists = DB.GetSqlN("Select count(StoreRefNo) as N from INB_InBound Where StoreRefNo=" + DB.SQuote(txtStrRefNo.Text));
                    if (SQlCheckIfIBExists == 0)
                    {
                        
                        if (IBTrack.InitiateIBTracking() != 0)
                        {

                        }
                        else
                        {
                            Response.Redirect("InboundDetails.aspx?ibupdate=dbupdateerr",false);
                        }

                        if (fucInboundDeliveryNote.HasFile)
                        {
                            String FileExtenion = Path.GetExtension(fucInboundDeliveryNote.FileName);

                            if (FileExtenion == ".pdf")
                            {

                                //if (CommonLogic.UploadFile(TenantRootDir + DB.GetSqlS("select UniqueID AS S from GEN_Tenant where TenantID=" + TenantID) + InboundPath + InbShpDelNotePath, fucInboundDeliveryNote, IBTrack.StoreRefNumber + Path.GetExtension(fucInboundDeliveryNote.FileName)) == false)
                                if (CommonLogic.UploadFile(TenantRootDir + hidTenantID.Value + InboundPath + InbShpDelNotePath, fucInboundDeliveryNote, IBTrack.StoreRefNumber + Path.GetExtension(fucInboundDeliveryNote.FileName)) == false)
                                {
                                    //Error in uploading the delivery note file
                                    Response.Redirect("InboundDetails.aspx?ibupdate=okfileuploaderr");
                                }

                                Response.Redirect("InboundDetails.aspx?ibdid=" + IBTrack.InboundTrackingID.ToString() + "&ibupdate=updateok"); // add to this stmt #invoice_det
                            }
                            else
                            {

                                if (IBTrack.InboundTrackingID != 0)
                                {
                                    Response.Redirect("InboundDetails.aspx?ibdid=" + IBTrack.InboundTrackingID.ToString() + "&ibupdate=oknofileuploaderr");
                                }
                                else
                                {
                                    resetError("Please attach a valid file", true);
                                }
                                return;
                            }
                        }
                        else
                        {
                            //resetError("Shipment is updated ,but no document attached",false);

                            // Response.Redirect("InboundDetails.aspx?ibdid=" + IBTrack.InboundTrackingID.ToString());
                           // resetError("Inbound Intiated Successfully", false);
                            Response.Redirect("InboundDetails.aspx?ibdid=" + IBTrack.InboundTrackingID.ToString() + "&ibupdate=oknofileuploaderr",false);// add to this  #invoice_det
                            Response.AddHeader("1", "Shipment is updated ,but no document attached");
                            


                        }

                    }
                    else
                    {
                        resetError("'Store Ref. Number' already exists. Please change the 'Store Ref. Number' and try again", true);
                    }
                }

                else
                {

                    //UPDATE EXISTING INBOUND 


                    if (IBTrack.InitiateIBTracking() != 0)
                    {

                    }
                    else
                    {
                        Response.Redirect("InboundDetails.aspx?ibdid=" + IBTrack.InboundTrackingID.ToString() + "&ibupdate=dbupdateerr",false);
                    }

                    //----Send email Alert  to Coordinator if Charges required is changed to "To be Estimated" 

                    if (fucInboundDeliveryNote.HasFile)
                    {

                        String FileExtenion = Path.GetExtension(fucInboundDeliveryNote.FileName);

                        if (FileExtenion == ".pdf")
                        {

                            //if (CommonLogic.UploadFile(TenantRootDir + DB.GetSqlS("select UniqueID AS S from GEN_Tenant where TenantID=" + TenantID) + InboundPath + InbShpDelNotePath, fucInboundDeliveryNote, IBTrack.StoreRefNumber + Path.GetExtension(fucInboundDeliveryNote.FileName)) == false)
                            if (CommonLogic.UploadFile(TenantRootDir + IBTrack.TenantID + InboundPath + InbShpDelNotePath, fucInboundDeliveryNote, IBTrack.StoreRefNumber + Path.GetExtension(fucInboundDeliveryNote.FileName)) == false)
                            {
                                Response.Redirect("InboundDetails.aspx?ibdid=" + IBTrack.InboundTrackingID.ToString() + "&ibupdate=okfileuploaderr");
                            }
                            else
                            {
                                Response.Redirect("InboundDetails.aspx?ibdid=" + IBTrack.InboundTrackingID.ToString() + "&ibupdate=updateok");
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
                        resetError("Successfully updated", false);
                        //Response.Redirect("InboundDetails.aspx?ibdid=" + IBTrack.InboundTrackingID.ToString() + "&ibupdate=updateok",false);
                        
                    }
                }




            }
            catch (Exception ex)
            {
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
                resetError("Error while updating initiate inbound", true);
            }




        }

        protected void LoadConsignmentNoteType()
        {

            try
            {
               
                IDataReader ConsmntReader = DB.GetRS("select ConsignmentNoteTypeID,ConsignmentNoteType, ConsignmentNoteTypeCode from INB_ConsignmentNoteType where IsActive=1 and IsDeleted=0");

                ddlConsignmentNoteType.Items.Add(new ListItem("Select Consignment Note Type", "0"));

                while (ConsmntReader.Read())
                {
                    ddlConsignmentNoteType.Items.Add(new ListItem(ConsmntReader["ConsignmentNoteTypeCode"] + "-" + ConsmntReader["ConsignmentNoteType"], ConsmntReader["ConsignmentNoteTypeID"].ToString()));
                }

                ConsmntReader.Close();
            }
            catch (Exception ex)
            {

                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
                resetError("Error while loading consignment note type", true);
                return;
            }

        }

        protected void ddlConsignmentNoteType_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadConsignmentNoteValues(ddlConsignmentNoteType.SelectedValue);
            txtConsignmentNoteTypeValue.Focus();
        }

        protected void LoadConsignmentNoteValues(string selectedValue)
        {

            switch (selectedValue)
            {
                case "1":
                    lblConsignmentNoteType.InnerText = "AWB Number:";
                    lblConsignmentNoteTypeDate.InnerText = "AWB Date:";
                    break;
                case "2":
                    lblConsignmentNoteType.InnerText = "BoL Number:";
                    lblConsignmentNoteTypeDate.InnerText = "BoL Date:";
                    break;

            }

        }

        protected void LoadFrieghtCompany()
        {

            try
            {
   
                //IDataReader FreightReader = DB.GetRS("select FreightCompany,FreightCompanyID from GEN_FreightCompany where IsActive=1 and IsDeleted=0 AND TenantID = case when  " + cp.TenantID.ToString() + "=0  then TenantID else " + cp.TenantID.ToString() + " end ");
                IDataReader FreightReader = DB.GetRS("select FreightCompany,FreightCompanyID from GEN_FreightCompany where IsActive=1 and IsDeleted=0 AND AccountID=" + cp.AccountID);

                ddlFreightCompany.Items.Add(new ListItem("Select Freight Company", "0"));

                while (FreightReader.Read())
                {
                    ddlFreightCompany.Items.Add(new ListItem(FreightReader["FreightCompany"].ToString(), FreightReader["FreightCompanyID"].ToString()));
                }

                FreightReader.Close();
            }
            catch (Exception ex)
            {

                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);

            }

        }

        #endregion ------------------ Basic Data  ------------------

        #region ---- Shipment Details ------

        protected void LoadShipmentDetails()
        {
            if (IBTrack.ShipmentExpectedDate != null)
            {
                txtShipmentExpectedDate.Text = IBTrack.ShipmentExpectedDate;
            }
            else
            {
                txtShipmentExpectedDate.Text = Convert.ToDateTime(DateTime.UtcNow, CultureInfo.CurrentCulture).ToString("dd-MMM-yyyy").ToString(); 
            }

        }



        protected void lnkUpdateShipmentDetails_Click(object sender, EventArgs e)
        {


            Page.Validate("ShipmentDetails");
            cp = HttpContext.Current.User as CustomPrincipal;
            if (Page.IsValid == false)
            {
                return;
            }

            //if (!cp.IsInAnyRoles("7"))
            //{

            //    resetError("Only IB Initiator can update this shipment", true);
            //    return;

            //}

            //if (DB.GetSqlN("select InboundStatusID AS N from INB_Inbound where IsActive=1 and IsDeleted=0  and InboundID=" + IBTrack.InboundTrackingID.ToString()) >= 3)
            //{
            //    resetError("Cannot change the expected date, as shipment is received <br/>", true);
            //    return;
            //}

            if (IBTrack.InBoundStatusID >= 3)
            {
                resetError("Cannot change the expected date, as shipment is received <br/>", true);
                return;
            }

            if (DB.GetSqlN("select top 1 InboundID AS N from INB_Inbound_ORD_SupplierInvoice where IsActive=1 and IsDeleted=0 and InboundID=" + IBTrack.InboundTrackingID.ToString()) == 0)
            {
                resetError("Add atleast one PO/Invoice line item", true);
                return;
            }


            StringBuilder sbUpdateShipment = new StringBuilder();

            sbUpdateShipment.Append("Update INB_Inbound SET ");


            //sbUpdateShipment.Append("ShipmentExpectedDate=" + (txtShipmentExpectedDate.Text != "" ? DB.SQuote(DateTime.ParseExact(txtShipmentExpectedDate.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString()) : null));

            string shpDate = txtShipmentExpectedDate.Text.Trim();
            //IBTrack.DocReceivedDate = (txtDocRcvDate.Text != "" ? DateTime.ParseExact(shpDate + " " + DateTime.UtcNow.ToString("hh:mm:ss.fff"), "dd-MMM-yyyy hh:mm:ss.fff", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy hh:mm:ss.fff") : null);
            //shpDate = ((shpDate != "") ? DateTime.ParseExact(shpDate, "dd-MMM-yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy") : null);
            shpDate = (shpDate != "" ? DateTime.ParseExact(shpDate + " " + DateTime.UtcNow.ToString("hh:mm:ss.fff"), "dd-MMM-yyyy hh:mm:ss.fff", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy hh:mm:ss.fff") : null);
            if (shpDate != "" && (shpDate.Contains("/") || shpDate.Contains("-")))
            {
                //DateTime _dtShipmentExpDate;
                //if (DateTime.TryParse(shpDate, out _dtShipmentExpDate))
                //{
                //    shpDate = _dtShipmentExpDate.ToString();
                //}
                //else
                //{
                //    shpDate = shpDate.Replace("-", "/");
                //    shpDate = shpDate.Split('/')[1] + "/" + shpDate.Split('/')[0] + "/" + shpDate.Split('/')[2];
                //}
            }
            else
                //shpDate = DateTime.UtcNow.ToString("MM/dd/yyyy");
                shpDate = DateTime.UtcNow.ToString("MM/dd/yyyy hh:mm:ss.fff");

            sbUpdateShipment.Append("ShipmentExpectedDate=" + DB.SQuote(shpDate));

            if (IBTrack.InBoundStatusID == 1)
            {
                sbUpdateShipment.Append(",InboundStatusID=2");
            }

            sbUpdateShipment.Append(" WHERE InboundID=" + IBTrack.InboundTrackingID.ToString());
            try
            {
                DB.ExecuteSQL(sbUpdateShipment.ToString());

                // Record Transaction Log Info.
                //InOutID=1 for Inbound , AuditActivityID=3 for ShipmentExpected AND InBoundStatusID = 2 
                // Audit.LogTranscationAudit(cp.UserID.ToString(), 1, 3, IBTrack.InboundTrackingID, 2);

                resetError("Shipment expected details successfully updated", false);
                // Response.Redirect("InboundDetails.aspx?ibdid="+IBTrack.InboundTrackingID.ToString()+"&ibupdate=shipexptdok");
               // upnlShipmentDetails.Update();
                //upnlBasicDetails.Update();
                //upnlTPLInboundCharge.Update();
                //upnlRcv.Update();
                pnlShipmentReceived.Visible = true;
                txtShipmentReceivedDate.Text = DateTime.UtcNow.ToString("dd-MMM-yyyy");
                txtOffLoadingTime.Text = DateTime.UtcNow.ToString("hh:mm tt");
                                         //DB.GetSqlS("SELECT FORMAT((SELECT[dbo].[UDF_GetWarehouseTime] ('" + DateTime.UtcNow.ToString("hh:mm tt") + "', (SELECT WarehouseID FROM INB_RefWarehouse_Details WHERE IsActive=1 AND IsDeleted=0 AND InboundID=" + IBTrack.InboundTrackingID + "))),'hh:mm tt') AS S");
                upnlTPLInboundCharge.Visible = true;
                lnkAddNewTPLActivity.Visible = true;

                ValidateInboundStatus();
                upnlRcvDockMgmt.UpdateMode = UpdatePanelUpdateMode.Conditional;
                upnlRcvDockMgmt.Update();
                upnlTPLInboundCharge.UpdateMode = UpdatePanelUpdateMode.Conditional;
                upnlTPLInboundCharge.Update();
                upnlRcv.UpdateMode = UpdatePanelUpdateMode.Conditional;
                upnlRcv.Update();
                NewPanelVisible.Visible = true;
                //Response.Redirect("InboundDetails.aspx?ibdid=" + IBTrack.InboundTrackingID.ToString());


                }
            catch (Exception ex)
            {
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);

                resetError("Error while updating shipment expected details", true);
            }



        }


      




        #endregion ---- Shipment Details ------

        #region -------- PO / Invoice Details Grid  --------------


        protected DataSet POInvoice_buildGridData()
        {
            string sql = ViewState["POInvListSQL"].ToString();
            DataSet ds = DB.GetDS(sql, false);

            return ds;
        }

        protected void POInvoice_buildGridData(DataSet ds)
        {
            gvPOInvoice.DataSource = ds.Tables[0];
            gvPOInvoice.DataBind();
            ds.Dispose();
        }

        protected void lnkAddNewPoItems_Click(object sender, EventArgs e)
        {
            cp = HttpContext.Current.User as CustomPrincipal;
            //if (!cp.IsInAnyRoles("7"))
            //{

            //    resetError("Only IB Initiator can update this shipment", true);
            //    return;

            //}

            ViewState["gvPOInvIsInsert"] = false;

            //if (DB.GetSqlN("select InboundStatusID AS N from INB_Inbound where InboundID=" + IBTrack.InboundTrackingID.ToString()) >= 3)
            //{
            //    resetError("Cannot change the shipment details, as shipment is received <br/>", true);
            //    return;
            //}

            if (IBTrack.InBoundStatusID == 3)
            {
                resetError("Cannot change the shipment details, as shipment is received <br/>", true);
                return;
            }
            if (IBTrack.InBoundStatusID == 2)
            {
                resetError("Cannot change the shipment details, as shipment is Expected <br/>", true);
                return;
            }
            //if (IBTrack.InBoundStatusID >= 2)
            //{
            //    resetError("Cannot change the shipment details, as shipment is Expected <br/>", true);
            //    return;
            //}
            if (IBTrack.InBoundStatusID == 6 )
            {
                resetError("Cannot change the shipment details, as GRN Updated <br/>", true);
                return;
            }
            if (IBTrack.InBoundStatusID == 4)
            {
                resetError("Cannot change the shipment details, as shipment is Verified <br/>", true);
                return;
            }

            if (DB.GetSqlN(" select InboundTracking_WarehouseID AS N from INB_InboundTracking_Warehouse where InboundID=" + IBTrack.InboundTrackingID.ToString() + " AND IsActive=1 AND IsDeleted=0") != 0)
            {
                resetError(" Cannot change the shipment details, as shipment is received <br/>", true);
                return;
            }

            if (DB.GetSqlN("select POHeaderID AS N from ORD_POHeader where IsActive=1 and IsDeleted=0 and POStatusID<=2 and   SupplierID=" + IBTrack.Supplier + "  and  TenantID=" + IBTrack.TenantID) == 0)
            {
                resetError("No PO's are configured to this supplier <br/>", true);
                return;
            }


            //  hifInvNumberID.Value = "0";

            StringBuilder sql = new StringBuilder(2500);
            try
            {

                gvPOInvoice.EditIndex = 0;
                gvPOInvoice.PageIndex = 0;

                DataSet dsPOInvoice = this.POInvoice_buildGridData();
                DataRow row = dsPOInvoice.Tables[0].NewRow();
                row["InvoiceNumber"] = "";
                row["Inbound_SupplierInvoiceID"] = 0;
                dsPOInvoice.Tables[0].Rows.InsertAt(row, 0);


                this.POInvoice_buildGridData(dsPOInvoice);

                //this.resetError("New PO Invoice line item added.", false);

                ViewState["gvPOInvIsInsert"] = true;

                lnkAddNewPoItems.Visible = false;

                gvPOInvoice.Focus();
            }
            catch (Exception ex)
            {
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);

                this.resetError("Error while inserting new record ", true);

            }



        }

        protected void gvPOInvoice_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

            ViewState["gvPOInvIsInsert"] = false;

            gvPOInvoice.PageIndex = e.NewPageIndex;
            gvPOInvoice.EditIndex = -1;
            this.POInvoice_buildGridData(this.POInvoice_buildGridData());


        }

        protected void gvPOInvoice_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.DataItem == null)
                return;

            DataRow row = ((DataRowView)e.Row.DataItem).Row;

            //if (DB.GetSqlN("select InboundStatusID AS N from INB_Inbound where InboundID=" + IBTrack.InboundTrackingID.ToString()) >= 3)
            //{

            //    e.Row.Controls[10].Visible = false;
            //}

            if (IBTrack.InBoundStatusID >= 3)
            {
                e.Row.Controls[10].Visible = false;
            }


            if ((e.Row.RowState & DataControlRowState.Edit) == DataControlRowState.Edit)
            {
                TextBox txtInvNumber = (TextBox)e.Row.FindControl("txtPONumber");
                txtInvNumber.Focus();



                //  hifInvNumberID.Value = CommonLogic.IIF(row["SupplierInvoiceID"].ToString()!="",row["SupplierInvoiceID"].ToString(),"0");

            }

            // hifInvNumberID.Value = CommonLogic.IIF(row["SupplierInvoiceID"].ToString() != "", row["SupplierInvoiceID"].ToString(), "0");

        }

        protected void gvPOInvoice_RowCommand(object sender, GridViewCommandEventArgs e)
        {


            if (e.CommandName == "DeleteItem")
            {
                ViewState["gvPOInvIsInsert"] = false;
                gvPOInvoice.EditIndex = -1;
                int iden = Localization.ParseNativeInt(e.CommandArgument.ToString());
                this.deleteRowPermPOInv(iden);
            }

        }

        protected void gvPOInvoice_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {

           

            //if (!cp.IsInAnyRoles("7"))
            //{

            //    resetError("Only IB Initiator can update this shipment", true);
            //    return;

            //}

            //if (DB.GetSqlN("select InboundStatusID AS N from INB_Inbound where isActive=1 AND IsDeleted=0 AND InboundID=" + IBTrack.InboundTrackingID.ToString()) >= 3)
            //{
            //    resetError("Cannot change the shipment details, as shipment is received <br/>", true);
            //    return;
            //}

            //if (IBTrack.InBoundStatusID >= 3)
            //{
            //    resetError("Cannot change the shipment details, as shipment is received <br/>", true);
            //    return;
            //}


            if (IBTrack.InBoundStatusID >= 2)
            {
                resetError("Cannot change the shipment details, as shipment is Expected <br/>", true);
                return;
            }

            if (DB.GetSqlN(" select InboundTracking_WarehouseID AS N from INB_InboundTracking_Warehouse where InboundID=" + IBTrack.InboundTrackingID.ToString() + " AND IsActive=1 AND IsDeleted=0") != 0)
            {
                resetError("Cannot change the shipment details, as shipment is received <br/>", true);
                return;
            }


            ViewState["gvPOInvIsInsert"] = false;
            GridViewRow row = gvPOInvoice.Rows[e.RowIndex];
             
            string ponum= ((TextBox)row.FindControl("txtPONumber")).Text;
            string Invoice = ((TextBox)row.FindControl("txtInvoiceNumber")).Text;
            if (ponum=="" || ponum==null || Invoice=="" || Invoice==null)
            {
                resetError("Please check all mandatory fields", true);
                return;
            }

            if (row != null)
            {

                string ltPONumber = ((TextBox)row.FindControl("txtPONumber")).Text;
                string ltInvNumber = ((TextBox)row.FindControl("txtInvoiceNumber")).Text;
                string ltIBPOInvDetID = ((Literal)row.FindControl("ltHidInbound_SupplierInvoiceID")).Text;
                string updatedon = DateTime.UtcNow.ToString("MM/dd/yyyy hh:mm:ss");
                int updatedby=cp.UserID;
                int POHeaderID = DB.GetSqlN("select POHeaderID AS N from ORD_POHeader where IsActive=1 and IsDeleted=0  and TenantID=" + IBTrack.TenantID + "  and PONumber=" + DB.SQuote(ltPONumber));

                if (POHeaderID == 0)
                {
                    this.resetError("PO Number does not exist", true);

                    return;
                }

                int InvoiceHeaderID = DB.GetSqlN("select SupplierInvoiceID AS N from ORD_SupplierInvoice ORD_SUP where ORD_SUP.IsActive=1 and ORD_SUP.IsDeleted=0 AND ORD_SUP.POHeaderID=" + POHeaderID + " AND ORD_SUP.SupplierID=" + IBTrack.Supplier + " AND ORD_SUP.InvoiceNumber='" + ltInvNumber + "'");


                if (InvoiceHeaderID == 0)
                {
                    this.resetError("Invoice Number does not exist", true);

                    return;
                }


                if (CommonLogic.GetPOInvoiceLineItemsCount(POHeaderID, InvoiceHeaderID, Convert.ToInt16(hidTenantID.Value)) == 0)
                {

                    this.resetError("No PO line items are configured to this invoice", true);
                    return;
                }





                StringBuilder sqlPOInvDetails = new StringBuilder(2500);

                sqlPOInvDetails.Append(" DECLARE @NewUpdateInboundPOInvoiceID int; ");
                sqlPOInvDetails.Append(" EXEC  [dbo].[sp_INB_UpsertPOInvoiceDetails]  @Inbound_SupplierInvoiceID=" + ltIBPOInvDetID + ",@POHeaderID=" + POHeaderID + ",@InboundID=" + IBTrack.InboundTrackingID + ",@SupplierInvoiceID=" + InvoiceHeaderID + ",@CreatedBy=" + CreatedBy + ",@UpdatedOn='" +updatedon  + "',@NewInbound_SupplierInvoiceID=@NewUpdateInboundPOInvoiceID OUTPUT  ;   select @NewUpdateInboundPOInvoiceID AS N");

                try
                {
                    DB.ExecuteSQL(sqlPOInvDetails.ToString());

                    this.resetError("Successfully Updated", false);
                    

                    gvPOInvoice.EditIndex = -1;

                    this.POInvoice_buildGridData(this.POInvoice_buildGridData());

                    lnkAddNewPoItems.Visible = true;

                    LoadRTRValue();

                    //if (gvPOInvoice.Rows.Count != 0 || gvPOInvoice.Rows.Count != 0)
                    //{
                    //    atcSupplier.Enabled = false;
                    //    txtTenant.Enabled = false;
                    //    txtWarehouse.Enabled = false;
                    //    txtDocRcvDate.Enabled = false;
                    //    atcShipmentType.Enabled = false;
                    //}

                    ScriptManager.RegisterStartupScript(this, this.GetType(), "test11", "disableControls(" + gvPOInvoice.Rows.Count + ");", true);
                }
                catch (SqlException sqlex)
                {
                    CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), sqlex.Source, sqlex.Message, sqlex.StackTrace);

                    if (sqlex.ErrorCode == -2146232060)
                    {
                        this.resetError("This invoice is already updated ", true);
                        return;
                    }
                }
                catch (Exception ex)
                {
                    CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);

                    this.resetError("Error while updating PO/Invoice details", true);
                }
            }




        }

        protected void gvPOInvoice_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            /*
            if (Localization.ParseBoolean(ViewState["gvPOInvIsInsert"].ToString()))
            {
                GridViewRow row = gvPOInvoice.Rows[e.RowIndex];
                if (row != null)
                {
                    int iden = Convert.ToInt32(((Literal)row.FindControl("ltHidInbound_SupplierInvoiceID")).Text);
                    deleteRowPermPOInv(iden);
                    resetError("Line item canceled", true);
                    lnkAddNewPoItems.Visible = true;
                }
            }

            ViewState["gvPOInvIsInsert"] = false;*/


            gvPOInvoice.EditIndex = -1;
            this.POInvoice_buildGridData(this.POInvoice_buildGridData());

            lnkAddNewPoItems.Visible = true;
        }

        protected void gvPOInvoice_RowEditing(object sender, GridViewEditEventArgs e)
        {


            ViewState["gvPOInvIsInsert"] = false;

            gvPOInvoice.EditIndex = e.NewEditIndex;



            this.POInvoice_buildGridData(this.POInvoice_buildGridData());

            lnkAddNewPoItems.Visible = false;
        }

        protected void lnkDeletePOInvItem_Click(object sender, EventArgs e)
        {
           


            //if (!cp.IsInAnyRoles("7"))
            //{

            //    resetError("Only IB Initiator can update this shipment", true);
            //    return;

            //}


            //if (DB.GetSqlN("select InboundStatusID AS N from INB_Inbound where InboundID=" + IBTrack.InboundTrackingID.ToString()) >= 3)
            //{
            //    resetError("Cannot change the shipment details, as shipment is received  <br/>", true);
            //    return;
            //}

            //if (IBTrack.InBoundStatusID >= 3)
            //{
            //    resetError("Cannot change the shipment details, as shipment is received  <br/>", true);
            //    return;
            //}

            if (IBTrack.InBoundStatusID == 2)
            {
                resetError("Cannot change the shipment details, as shipment is Expected  <br/>", true);
                return;
            }
            if (IBTrack.InBoundStatusID == 3)
            {
                resetError("Cannot change the shipment details, as shipment is received <br/>", true);
                return;
            }
           
            //if (IBTrack.InBoundStatusID >= 2)
            //{
            //    resetError("Cannot change the shipment details, as shipment is Expected <br/>", true);
            //    return;
            //}
            if (IBTrack.InBoundStatusID == 6)
            {
                resetError("Cannot change the shipment details, as GRN Updated <br/>", true);
                return;
            }
            if (IBTrack.InBoundStatusID == 4)
            {
                resetError("Cannot change the shipment details, as shipment is Verified <br/>", true);
                return;
            }

            if (DB.GetSqlN(" select InboundTracking_WarehouseID AS N from INB_InboundTracking_Warehouse where InboundID=" + IBTrack.InboundTrackingID.ToString() + " AND IsActive=1 AND IsDeleted=0") != 0)
            {
                resetError("Cannot change the shipment details, as shipment is received  <br/>", true);
                return;
            }

            string rfidIDs = "";
            string POIDs = "";

            bool chkBox = false;

            StringBuilder sqlDeleteString = new StringBuilder();
            sqlDeleteString.Append("BEGIN TRAN ");

            //'Navigate through each row in the GridView for checkbox items
            foreach (GridViewRow gv in gvPOInvoice.Rows)
            {

                CheckBox isDelete = (CheckBox)gv.FindControl("chkIsDeletePOInvItems");

                if (isDelete == null)
                    return;

                if (isDelete.Checked)
                {
                    chkBox = true;
                    // Concatenate GridView items with comma for SQL Delete
                    rfidIDs = ((Literal)gv.FindControl("ltHidInbound_SupplierInvoiceID")).Text.ToString();
                    POIDs = ((Literal)gv.FindControl("ltHidInbound_POID")).Text.ToString();

                    int POHeaderID = DB.GetSqlN("select POHeaderID AS N from ORD_POHeader where IsActive=1 and IsDeleted=0  and TenantID=" + IBTrack.TenantID + "  and PONumber=" + DB.SQuote(POIDs));

                    if (rfidIDs != "")
                        sqlDeleteString.Append(" Delete from INB_Inbound_ORD_SupplierInvoice Where Inbound_SupplierInvoiceID=" + rfidIDs + "");
                    if (POIDs != "")
                        sqlDeleteString.Append(" Update  ORD_POHeader  SET POStatusID = 1 Where POHeaderID=" + POHeaderID + ";");

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
                resetError("Successfully deleted the selected line items", false);                

            }
            catch (Exception ex)
            {
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
                resetError("Error while deleting PO/Invoice details", true);
            }

            this.POInvoice_buildGridData(this.POInvoice_buildGridData());
            if (gvPOInvoice.Rows.Count != 0 || gvPOInvoice.Rows.Count != 0)
            {
                atcSupplier.Enabled = false;
                txtTenant.Enabled = false;
                txtDocRcvDate.Enabled = false;
                txtWarehouse.Enabled = false;
                atcShipmentType.Enabled = false;
            }
            else
            {
                atcSupplier.Enabled = true;
                txtTenant.Enabled = true;
                txtDocRcvDate.Enabled = true;
                txtWarehouse.Enabled = true;
                atcShipmentType.Enabled = true;
            }




        }

        protected void deleteRowPermPOInv(int iden)
        {
            
            StringBuilder sql = new StringBuilder(2500);
            sql.Append("DELETE from INB_Inbound_ORD_SupplierInvoice where Inbound_SupplierInvoiceID=" + iden.ToString());
            try
            {
                DB.ExecuteSQL(sql.ToString());
                this.POInvoice_buildGridData(this.POInvoice_buildGridData());

            }
            catch (Exception ex)
            {
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
                this.resetError("Error while deleting PO/Invoice details", true);
            }
        }


        #endregion -------- PO / Invoice Details Grid  --------------

        #region ---- Shipment Received Details -----

        protected void LoadShipmentReceivedDetails()
        {

            IDataReader dr = DB.GetRS("select WarehouseID from INB_RefWarehouse_Details INB_R left join INB_InboundTracking_Warehouse INB_I on INB_I.IB_RefWarehouse_DetailsID=INB_R.IB_RefWarehouse_DetailsID where INB_I.InboundID=" + IBTrack.InboundTrackingID.ToString());

            if (dr.Read())
            {
                ddlStores.SelectedValue = dr["WarehouseID"].ToString();
            }

            dr.Close();

            IDataReader dr1 = DB.GetRS(" select [dbo].[UDF_GetWarehouseTime] (ShipmentReceivedOn,IB_REF.WarehouseID)ShipmentReceivedOn,FORMAT([dbo].[UDF_GetWarehouseTime](Offloadtime,WarehouseID),'hh:mm:ss')Offloadtime from INB_InboundTracking_Warehouse ITR JOIN INB_RefWarehouse_Details IB_REF ON IB_REF.InboundID = ITR.InboundID where ITR.InboundID=" + IBTrack.InboundTrackingID.ToString());
            //IDataReader dr1 = DB.GetRS(" select ShipmentReceivedOn, Offloadtime from INB_InboundTracking_Warehouse ITR JOIN INB_RefWarehouse_Details IB_REF ON IB_REF.InboundID = ITR.InboundID where ITR.InboundID=" + IBTrack.InboundTrackingID.ToString());

            if (dr1.Read())
            {
                txtShipmentReceivedDate.Text = Convert.ToDateTime(Convert.ToDateTime((dr1["ShipmentReceivedOn"].ToString())).ToShortDateString(), CultureInfo.CurrentCulture).ToString("dd-MMM-yyyy").ToString();

                txtOffLoadingTime.Text = Convert.ToDateTime((dr1["Offloadtime"].ToString())).ToShortTimeString();
            }

            lblShipmentReceivedWH.Text = CommonLogic.IIF(ddlStores.SelectedItem.Text != "Select Store", ddlStores.SelectedItem.Text, "");

            dr1.Close();



        }

        protected void LoadWHStores()
        {
            cp = HttpContext.Current.User as CustomPrincipal;
            try
            {
             

                IDataReader WHReader = DB.GetRS("select WHCode,INB_R.WarehouseID from INB_RefWarehouse_Details INB_R Left Join GEN_Warehouse GEN_W on GEN_W.WarehouseID=INB_R.WarehouseID where INB_R.IsDeleted=0 AND INB_R.IsActive=1 and GEN_W.isDeleted=0 AND GEN_W.isActive=1  AND InboundID=" + IBTrack.InboundTrackingID.ToString() + " and AccountID="+cp.AccountID);

                ddlStores.Items.Add(new ListItem("Select Store", "0"));

                while (WHReader.Read())
                {
                    //=================Commneted By MD.Prasad===================//
                    //if (cp.IsInWarehouse(WHReader["WarehouseID"].ToString()))
                    //{
                    ddlStores.Items.Add(new ListItem(WHReader["WHCode"].ToString(), WHReader["WarehouseID"].ToString()));
                    //}
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

        protected void lnkCancelReceived_Click(object sender, EventArgs e)
        {

            String IB_RefWH_DetailsID = DB.GetSqlN("select IB_RefWarehouse_DetailsID AS N from INB_RefWarehouse_Details where IsActive=1 and IsDeleted=0 and WarehouseID=" + ddlStores.SelectedValue + " and InboundID=" + IBTrack.InboundTrackingID.ToString()).ToString();


            if (DB.GetSqlN("select top 1 IB_RefWarehouse_DetailsID AS N from INB_InboundTracking_Warehouse where IB_RefWarehouse_DetailsID=" + IB_RefWH_DetailsID) != 0)
            {
                resetError("This shipment is already received", true);
                return;
            }

            ddlStores.SelectedValue = "0";
            txtOffLoadingTime.Text = "";
        }

        protected void lnkShipmentReceived_Click(object sender, EventArgs e)
        {

            cp = HttpContext.Current.User as CustomPrincipal;
            //if (!cp.IsInAnyRoles("21"))
            //{

            //    resetError("Only IB Store-Incharge can update this shipment", true);
            //    return;

            //}
            if (DB.GetSqlN("SELECT TOP 1 DockID AS N FROM INB_Inbound_Dock WHERE InboundID = " + IBTrack.InboundTrackingID+ "AND IsActive = 1 AND IsDeleted = 0") == 0)
            {
                resetError("Please Provide Docking Information..", true);
                return;
            }

            if (DB.GetSqlN("select top 1 InboundID AS N from INB_Inbound_ORD_SupplierInvoice where IsActive=1 and IsDeleted=0 and InboundID=" + IBTrack.InboundTrackingID.ToString()) == 0)
            {
                resetError("No PO's are configured to this supplier", true);
                return;
            }


            Page.Validate("updateShipmentReceived");

            if (Page.IsValid == false)
            {
                resetError("Please enter mandatory fields" , true);
                return;
            }

            if (ddlStores.SelectedValue == "0")
            {
                resetError("Please select Store", true);
                return;
            }

            StringBuilder SQLUpdateInboundTracking_WareHouse = new StringBuilder();

            String IB_RefWH_DetailsID = DB.GetSqlN("select IB_RefWarehouse_DetailsID AS N from INB_RefWarehouse_Details where IsActive=1 and IsDeleted=0 and WarehouseID=" + ddlStores.SelectedValue + " and InboundID=" + IBTrack.InboundTrackingID.ToString()).ToString();


            if (DB.GetSqlN("select top 1 IB_RefWarehouse_DetailsID AS N from INB_InboundTracking_Warehouse where IB_RefWarehouse_DetailsID=" + IB_RefWH_DetailsID) != 0)
            {
                upnlTPLReceivingCharge.Visible = true;
                resetError("This shipment is already received", true);
                return;
            }


            char[] seps = new char[] { ',' };

            //=================Commneted By MD.Prasad===================// String ReferedWHs = IBTrack.ReferedStoreIDs.Substring(0, IBTrack.ReferedStoreIDs.Length - 1);
            String ReferedWHs = IBTrack.ReferedStoreIDs.Substring(0, IBTrack.ReferedStoreIDs.Length);

            String[] ArrStores = CommonLogic.FilterSpacesInArrElements(ReferedWHs.Split(seps));

            StringBuilder sbUpdateShipment = new StringBuilder();

            sbUpdateShipment.Append("Update INB_Inbound SET ");


            String ShipmentReceivedDate;

            string ShipDate = txtShipmentReceivedDate.Text.Trim();
            //if (ShipDate != "" && (ShipDate.Contains("/") || ShipDate.Contains("-")))
            //{
            //    ShipDate = ShipDate.Replace("-", "/");
            //}



            ShipmentReceivedDate = ((ShipDate != "" && txtOffLoadingTime.Text.Trim() != "") ? DateTime.ParseExact(ShipDate + " " + txtOffLoadingTime.Text.Trim(), "dd-MMM-yyyy hh:mm tt", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy hh:mm tt") : null);
            //  ShipmentReceivedDate = DateTime.ParseExact(ShipDate,  CultureInfo.InvariantCulture)ToString("MM/dd/yyyy hh:mm tt");
            string updatedon = DateTime.UtcNow.ToString("MM/dd/yyyy hh:mm:ss");

            SQLUpdateInboundTracking_WareHouse.Append("DECLARE @NewUpdateInboundTracking_WarehouseID int;    ");
            SQLUpdateInboundTracking_WareHouse.Append("EXEC [sp_INB_UpsertInboundTracking_Warehouse]    @InboundTracking_WarehouseID=0");
            SQLUpdateInboundTracking_WareHouse.Append(",@InboundID=" + IBTrack.InboundTrackingID.ToString());//
            SQLUpdateInboundTracking_WareHouse.Append(",@IB_RefWarehouse_DetailsID=" + IB_RefWH_DetailsID);
            SQLUpdateInboundTracking_WareHouse.Append(",@UserID=" + CreatedBy);
            SQLUpdateInboundTracking_WareHouse.Append(",@ShipmentReceivedOn=" + (ShipmentReceivedDate == null ? "NULL" : DB.SQuote(ShipmentReceivedDate)));
            SQLUpdateInboundTracking_WareHouse.Append(",@Offloadtime=" + (txtOffLoadingTime.Text != "" ? DB.SQuote(txtOffLoadingTime.Text) : "NULL"));
            SQLUpdateInboundTracking_WareHouse.Append(",@ProcessedOn=NULL");
            SQLUpdateInboundTracking_WareHouse.Append(",@ShipmentVerifiedOn=NULL");
            SQLUpdateInboundTracking_WareHouse.Append(",@Disc_PackagesReceived=NULL");
            SQLUpdateInboundTracking_WareHouse.Append(",@Disc_Remarks=NULL");
            SQLUpdateInboundTracking_WareHouse.Append(",@Disc_CheckedBy=NULL");
            SQLUpdateInboundTracking_WareHouse.Append(",@Disc_CheckedDate=NULL");
            SQLUpdateInboundTracking_WareHouse.Append(",@Disc_VerifiedBy=NULL");
            SQLUpdateInboundTracking_WareHouse.Append(",@Disc_VerifiedDate=NULL");
            SQLUpdateInboundTracking_WareHouse.Append(",@DiscrepancyStatusID=NULL");
            SQLUpdateInboundTracking_WareHouse.Append(",@HasDiscrepancy=0");
            SQLUpdateInboundTracking_WareHouse.Append(",@Remarks=NULL");
            SQLUpdateInboundTracking_WareHouse.Append(",@CreatedBy=" + CreatedBy);
            SQLUpdateInboundTracking_WareHouse.Append(",@UpdatedOn=" + DB.SQuote(updatedon));

            SQLUpdateInboundTracking_WareHouse.Append(",@NewInboundTracking_WarehouseID=@NewUpdateInboundTracking_WarehouseID OUTPUT  Select @NewUpdateInboundTracking_WarehouseID AS N;");


            try
            {
                if (DB.GetSqlN(SQLUpdateInboundTracking_WareHouse.ToString()) != 0)
                {

                    int count = DB.GetSqlN("SELECT COUNT(*)  FROM INB_SuggestedPutaway WHERE InboundID = " +IBTrack.InboundTrackingID.ToString() + " AND IsDeleted  =0 AND IsActive =  1");

                    //if(count == 0)
                    //{
                    //    DB.ExecuteSQL(" EXEC [dbo].[USP_TRN_GeneratePutawaySuggestions] @InboundID = " + IBTrack.InboundTrackingID.ToString() + "");
                    //}

                   // SendShipmentReceivedEmail(IBTrack.StoreRefNumber);

                   

               


                    //int Count = DB.GetSqlN("SELECT COUNT(*) AS N  FROM INB_SuggestedPutaway WHERE InboundID = "+ IBTrack.InboundTrackingID.ToString() + "");
                    if(count == 0)
                    {
                        DB.ExecuteSQL("EXEC[dbo].[USP_TRN_GeneratePutawaySuggestions] @InboundID = " + IBTrack.InboundTrackingID.ToString() + "");
                    }




                    resetError("Shipment received details successfully updated  ", false);

                    upnlBasicDetails.Update();
                    upnlTPLReceivingCharge.Update();
                    upnlGRNDetails.Update();
                    upnlTPLReceivingCharge.Visible = true;
                    ValidateInboundStatus();
                }
                else
                {
                    resetError("Error while updating shipment received details ", true);
                    return;
                }


            }
            catch (Exception ex)
            {
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
                resetError("Error while updating shipment received details ", true);
                return;
            }

            if (ArrStores.Length > 1)
            {

                if (DB.GetSqlN("Select Count(IB_RefWarehouse_DetailsID) as N from INB_InboundTracking_Warehouse WHERE  InboundID=" + IBTrack.InboundTrackingID.ToString()) == ArrStores.Length)
                {
                    sbUpdateShipment.Append(" InboundStatusID=3");
                }
                else
                {
                    sbUpdateShipment.Append(" InboundStatusID=2"); // If not the last store to update 
                }

            }
            else
            {

                sbUpdateShipment.Append(" InboundStatusID=3");

            }

            sbUpdateShipment.Append(" WHERE InboundID=" + IBTrack.InboundTrackingID.ToString());

           

            try
            {
                DB.ExecuteSQL(sbUpdateShipment.ToString());

                // added without shipment verification.

                //DB.ExecuteSQL("Update INB_Inbound set InboundStatusID=3 where InboundID=" + IBTrack.InboundTrackingID.ToString());
                pnlGRNDetails.Visible = true;

                resetError("Shipment received details successfully updated", false);
                pnlShipmentReceived.Visible = true;
                lnkAddAVNew.Visible = true;

                //Shipment verification place changed 

                //pnlshipmentVerification.Visible = true;

                upnlTPLReceivingCharge.Visible = true;

                txtShipmentVerifiedDate.Text = DateTime.UtcNow.ToString("dd-MMM-yyyy");
                ValidateInboundStatus();

            }
            catch (Exception ex)
            {
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
                //lblLocalerror.Text = ex.ToString();
            }


        }


        public bool SendShipmentReceivedEmail(String StoreRefNo)
        {


            try
            {
              
                MailMessage mailMSG = new MailMessage();


                mailMSG.From = new MailAddress(CommonLogic.Application("SenderEmail"));

                IDataReader drUsers = DB.GetRS("select Usr.Email from Mail_ReportName_User MRN JOIN GEN_User Usr ON Usr.UserID=MRN.UserID where ReportNameID=" + CommonLogic.GetReportNameID("Shipment Received"));

                while (drUsers.Read())
                {
                    mailMSG.ReplyToList.Add(DB.RSField(drUsers, "Email"));
                }

                drUsers.Close();

                mailMSG.CC.Add("kamalakar@inventrax.com");

                mailMSG.IsBodyHtml = true;

                String MailBody = "";

                MailBody += "<h1>Shipment is received with the StoreRef.No.:" + StoreRefNo + "</h1>";

                mailMSG.Body = MailBody;



                if (AppLogic.SendMailInWebMail(mailMSG))
                {
                    return true;
                }
                else
                {
                    return false;
                }







            }
            catch (Exception ex)
            {
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
                return false;

            }

        }


        protected void ddlStores_SelectedIndexChanged(object sender, EventArgs e)
        {

            divShpVeriNote.InnerHtml = "";


            String IB_RefWH_DetailsID = DB.GetSqlN("select IB_RefWarehouse_DetailsID AS N from INB_RefWarehouse_Details where IsActive=1 and IsDeleted=0 and WarehouseID=" + ddlStores.SelectedValue + " and InboundID=" + IBTrack.InboundTrackingID.ToString()).ToString();

            lblShipmentReceivedWH.Text = CommonLogic.IIF(ddlStores.SelectedItem.Text != "Select Store", ddlStores.SelectedItem.Text, "");
            lblVerfShipmentReceivedWH.Text = CommonLogic.IIF(ddlStores.SelectedItem.Text != "Select Store", ddlStores.SelectedItem.Text, "");



            if (ddlStores.SelectedItem.Text != "Select Store")
            {

                IDataReader rsShipRecv = DB.GetRS(" select ShipmentReceivedOn,Offloadtime from INB_InboundTracking_Warehouse where InboundID=" + IBTrack.InboundTrackingID.ToString() + " and IB_RefWarehouse_DetailsID=" + IB_RefWH_DetailsID);

                if (rsShipRecv.Read())
                {
                    txtShipmentReceivedDate.Text = Convert.ToDateTime(Convert.ToDateTime((rsShipRecv["ShipmentReceivedOn"].ToString())).ToShortDateString(), CultureInfo.CurrentCulture).ToString("dd-MMM-yyyy").ToString();

                    txtOffLoadingTime.Text = Convert.ToDateTime((rsShipRecv["Offloadtime"].ToString())).ToShortTimeString();
                }
                else
                {
                    txtShipmentReceivedDate.Text = Convert.ToDateTime(DateTime.UtcNow.ToShortDateString(), CultureInfo.CurrentCulture).ToString("dd-MMM-yyyy").ToString();
                }


                rsShipRecv.Close();

                if (DB.GetSqlN("select InboundTracking_WarehouseID AS N from INB_InboundTracking_Warehouse where InboundID=" + IBTrack.InboundTrackingID.ToString() + " and IB_RefWarehouse_DetailsID=" + IB_RefWH_DetailsID) != 0)
                    lnkAddAVNew.Visible = true;

                ActualVehicleSql = "[dbo].[sp_INB_GetActualVehicleDetails]   @InboundTracking_WarehouseID=" + DB.GetSqlN("select InboundTracking_WarehouseID As N from INB_InboundTracking_Warehouse where InboundID=" + CommonLogic.QueryString("ibdid") + " and  IB_RefWarehouse_DetailsID=" + IB_RefWH_DetailsID) + ", @Ref_WHID=" + DB.GetSqlN("select IB_RefWarehouse_DetailsID As N from INB_InboundTracking_Warehouse where InboundID=" + CommonLogic.QueryString("ibdid") + " and IB_RefWarehouse_DetailsID=" + IB_RefWH_DetailsID);
                ViewState["AVListSQL"] = ActualVehicleSql;
                this.GENAV_buildGridData(this.GENAV_buildGridData());


                int Ref_WHID = DB.GetSqlN("select IB_RefWarehouse_DetailsID  AS N from INB_RefWarehouse_Details where InboundID=" + IBTrack.InboundTrackingID + "and WarehouseID=" + ddlStores.SelectedValue);
                DiscrepancySql = "EXEC  [dbo].[sp_INB_DiscrepancyDetails]   @IBRefered_WHID=" + Ref_WHID;
                ViewState["DiscrepancyListSQL"] = DiscrepancySql;
                this.DiscrepancyDetails_buildGridData(this.DiscrepancyDetails_buildGridData());

                if (DB.GetSqlN("select InboundTracking_WarehouseID AS N from INB_InboundTracking_Warehouse where InboundID=" + IBTrack.InboundTrackingID.ToString() + " and IB_RefWarehouse_DetailsID=" + IB_RefWH_DetailsID) != 0)
                {

                    pnlDiscInfo.Visible = true;
                   // chkHasDiscrepancy.Checked = false;
                   // chkHasDiscrepancy.Visible = false;
                }
                else
                {
                    pnlDiscInfo.Visible = false;
                  //  chkHasDiscrepancy.Checked = false;
                  //  chkHasDiscrepancy.Visible = false;
                }


                LoadDiscrFormDetails();

                LoadShipmentVerificationData();

            }
            else
            {

                lnkAddAVNew.Visible = false;
                txtOffLoadingTime.Text = "";

                ActualVehicleSql = "[dbo].[sp_INB_GetActualVehicleDetails]   @InboundTracking_WarehouseID=0, @Ref_WHID=0";
                ViewState["AVListSQL"] = ActualVehicleSql;
                this.GENAV_buildGridData(this.GENAV_buildGridData());

                DiscrepancySql = "EXEC  [dbo].[sp_INB_DiscrepancyDetails]   @IBRefered_WHID=" + 0;
                ViewState["DiscrepancyListSQL"] = DiscrepancySql;
                this.DiscrepancyDetails_buildGridData(this.DiscrepancyDetails_buildGridData());
            }



            //if (DB.GetSqlN("select top 1 TransactionDocID AS N from INV_GoodsMovementDetails where GoodsMovementTypeID=1 AND TransactionDocID=" + IBTrack.InboundTrackingID + " AND IsDeleted=0 AND (IsDamaged=1 OR HasDiscrepancy=1)") != 0)
            //{
            //    pnlDiscInfo.Visible = true;
            //    chkHasDiscrepancy.Checked = false;
            //    chkHasDiscrepancy.Visible = false;
            //}



        }



        #endregion ---- Shipment Received Details -----

        #region -----  Projected Vehile Grid    ------

        protected DataSet GENPV_buildGridData()
        {
            string sql = ViewState["PrVListSQL"].ToString();
            DataSet ds = DB.GetDS(sql, false);

            return ds;
        }

        // Get ProjectedVehicle Grid data from DB
        protected void GENPV_buildGridData(DataSet ds)
        {
            gvProjectedVehicle.DataSource = ds.Tables[0];
            gvProjectedVehicle.DataBind();
            ds.Dispose();
        }

        protected void lnkPrjVehicle_Click(object sender, EventArgs e)
        {
            cp = HttpContext.Current.User as CustomPrincipal;
            //if (!cp.IsInAnyRoles("7"))
            //{

            //    resetError("Only IB Initiator can update this shipment", true);
            //    return;

            //}


            if (DB.GetSqlN("select top 1 InboundID AS N from INB_Inbound_ORD_SupplierInvoice where IsActive=1 and IsDeleted=0 and InboundID=" + IBTrack.InboundTrackingID.ToString()) == 0)
            {
                resetError("Add atleast one PO line item", true);
                return;
            }


            ViewState["gvPVIsInsert"] = false;

            StringBuilder sql = new StringBuilder(2500);

            // Insert new Item in the  INB_ProjectedEquipment  Table
            /* sql.Append("INSERT INTO INB_ProjectedEquipment (InboundID) values (");
             sql.Append(IBTrack.InboundTrackingID.ToString());
             sql.Append(")");*/

            try
            {
                /*
                DB.ExecuteSQL(sql.ToString());

                gvProjectedVehicle.EditIndex = gvProjectedVehicle.Rows.Count;
                 */

                gvProjectedVehicle.EditIndex = 0;
                gvProjectedVehicle.PageIndex = 0;

                DataSet dsProjectedVehicle = this.GENPV_buildGridData();
                DataRow row = dsProjectedVehicle.Tables[0].NewRow();
                row["EquipmentID"] = 0;
                row["ProjectedEquipmentID"] = 0;
                dsProjectedVehicle.Tables[0].Rows.InsertAt(row, 0);

                this.GENPV_buildGridData(dsProjectedVehicle);

                // this.resetError("New Projected Vehicle line item added", false);
                ViewState["gvPVIsInsert"] = true;
                ViewState["gvPVItemNotUpdated"] = true;

                lnkPrjVehicle.Visible = false;
                upnlRcvDockMgmt.Update();
                upnlBasicDetails.Update();
            }
            catch (Exception ex)
            {
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
                this.resetError("Error while inserting new record ", true);

            }


        }

        protected void gvProjectedVehicle_RowDataBound(object sender, GridViewRowEventArgs e)
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

        protected void gvProjectedVehicle_RowCommand(object sender, GridViewCommandEventArgs e)
        {


            if (e.CommandName == "DeleteItem")
            {
                ViewState["gvPVIsInsert"] = false;
                gvProjectedVehicle.EditIndex = -1;
                int iden = Localization.ParseNativeInt(e.CommandArgument.ToString());
                this.deleteRowPermE(iden);
            }
        }

        protected void gvProjectedVehicle_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            //if (!cp.IsInAnyRoles("7"))
            //{

            //    resetError("Only IB Initiator can update this shipment", true);
            //    return;

            //}

            ViewState["gvPVIsInsert"] = false;
            GridViewRow row = gvProjectedVehicle.Rows[e.RowIndex];

            if (row != null)
            {

                string ltPrEquipmentID = ((Literal)row.FindControl("lthidProjectedVehicleID")).Text;
                string ddlEquipmentID = ((DropDownList)row.FindControl("ddlEquipmentType")).SelectedValue.ToString();
                string vtxtProjectedValue = ((TextBox)row.FindControl("txtProjectedValue")).Text;
                decimal projectval = Convert.ToDecimal(vtxtProjectedValue);
                if(projectval == 0)
                {
                    resetError("Please enter valid projected value", true);
                    return;
                }

                StringBuilder sqlUoMDetails = new StringBuilder(2500);

                /*
                sqlUoMDetails.Append("UPDATE INB_ProjectedEquipment SET ");
                sqlUoMDetails.Append("EquipmentID=" + ddlEquipmentID + ",");
                sqlUoMDetails.Append("ProjectedValue=" + vtxtProjectedValue + ",");
                sqlUoMDetails.Append("CreatedBy=" + CreatedBy);
                sqlUoMDetails.Append("  WHERE ProjectedEquipmentID=" + ltPrEquipmentID);
                 */

                sqlUoMDetails.Append("DECLARE @NewUpdateProjectedEquipmentID int;   EXEC   [dbo].[sp_INB_UpsertProjectedEquipment]   @ProjectedEquipmentID=" + ltPrEquipmentID + ",@InboundID=" + IBTrack.InboundTrackingID + " ,@EquipmentID=" + ddlEquipmentID + ",@ProjectedValue=" + vtxtProjectedValue + ",@CreatedBy=" + CreatedBy + ",@NewProjectedEquipmentID=@NewUpdateProjectedEquipmentID OUTPUT; select @NewUpdateProjectedEquipmentID AS N");



                try
                {
                    DB.ExecuteSQL(sqlUoMDetails.ToString());
                    this.resetError("Successfully updated", false);
                    gvProjectedVehicle.EditIndex = -1;
                    ViewState["gvPVItemNotUpdated"] = false;
                    this.GENPV_buildGridData(this.GENPV_buildGridData());
                    lnkPrjVehicle.Visible = true;
                }
                catch (Exception ex)
                {
                    ViewState["gvPVItemNotUpdated"] = true;
                    CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
                    this.resetError("Error while updating projected vehicle details", true);
                }




            }

        }

        protected void gvProjectedVehicle_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            /*
            if (Localization.ParseBoolean(ViewState["gvPVIsInsert"].ToString()))
            {
                GridViewRow row = gvProjectedVehicle.Rows[e.RowIndex];
                if (row != null)
                {
                    int iden = Convert.ToInt32(((Literal)row.FindControl("lthidProjectedVehicleID")).Text);
                    deleteRowPermE(iden);
                    lnkPrjVehicle.Visible = true;
                }
            }

            ViewState["gvPVIsInsert"] = false;
             */



            gvProjectedVehicle.EditIndex = -1;
            this.GENPV_buildGridData(this.GENPV_buildGridData());
            lnkPrjVehicle.Visible = true;
        }

        protected void gvProjectedVehicle_RowEditing(object sender, GridViewEditEventArgs e)
        {

            ViewState["gvPVIsInsert"] = false;

            gvProjectedVehicle.EditIndex = e.NewEditIndex;
            ViewState["gvPVItemNotUpdated"] = false;
            this.GENPV_buildGridData(this.GENPV_buildGridData());
            lnkPrjVehicle.Visible = false;

        }



        protected void lnkDeleteRFItem_Click(object sender, EventArgs e)
        {

            //if (!cp.IsInAnyRoles("7"))
            //{

            //    resetError("Only IB Initiator can update this shipment", true);
            //    return;

            //}


            string rfidIDs = "";

            bool chkBox = false;

            StringBuilder sqlDeleteString = new StringBuilder();
            sqlDeleteString.Append("BEGIN TRAN ");

            //'Navigate through each row in the GridView for checkbox items
            foreach (GridViewRow gv in gvProjectedVehicle.Rows)
            {

                CheckBox isDelete = (CheckBox)gv.FindControl("chkIsDeleteRFItem");

                if (isDelete == null)
                    return;

                if (isDelete.Checked)
                {
                    chkBox = true;
                    // Concatenate GridView items with comma for SQL Delete
                    rfidIDs = ((Literal)gv.FindControl("lthidProjectedVehicleID")).Text.ToString();



                    if (rfidIDs != "")
                        sqlDeleteString.Append(" Delete INB_ProjectedEquipment Where ProjectedEquipmentID=" + rfidIDs + ";");

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
                resetError("Successfully deleted the selected line items", false);

            }
            catch (Exception ex)
            {
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
                resetError("Error while deleting selected line items", true);
            }

            this.GENPV_buildGridData(this.GENPV_buildGridData());


        }

        protected void deleteRowPermE(int iden)
        {
            StringBuilder sql = new StringBuilder(2500);
            sql.Append("DELETE from INB_ProjectedEquipment where ProjectedEquipmentID=" + iden.ToString());
            try
            {
                DB.ExecuteSQL(sql.ToString());
                this.GENPV_buildGridData(this.GENPV_buildGridData());

            }
            catch (Exception ex)
            {
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
                this.resetError("Error while deleting projected vehicle details", true);
            }
        }

        #endregion -----  Projected Vehile Grid    ------

        #region -----  Actual Vehile Grid    ------

        protected DataSet GENAV_buildGridData()
        {
            string sql = ViewState["AVListSQL"].ToString();
            DataSet ds = DB.GetDS(sql, false);

            return ds;
        }

        // Get ActualVehicle Grid data from DB
        protected void GENAV_buildGridData(DataSet ds)
        {
            gvActualVehicle.DataSource = ds.Tables[0];
            gvActualVehicle.DataBind();
            ds.Dispose();
        }

        protected void lnkAddAVNew_Click(object sender, EventArgs e)
        {
            cp = HttpContext.Current.User as CustomPrincipal;
            //if (!cp.IsInAnyRoles("21"))
            //{

            //    resetError("Only IB Store-Incharge can update this shipment", true);
            //    return;

            //}


            String IB_RefWH_DetailsID = DB.GetSqlN("select IB_RefWarehouse_DetailsID AS N from INB_RefWarehouse_Details where IsActive=1 and IsDeleted=0 and WarehouseID=" + ddlStores.SelectedValue + " and InboundID=" + IBTrack.InboundTrackingID.ToString()).ToString();

            if (DB.GetSqlS("select CONVERT(nvarchar(20), ShipmentReceivedOn) AS S  from INB_InboundTracking_Warehouse where IB_RefWarehouse_DetailsID =" + IB_RefWH_DetailsID) == "")
            {
                resetError("Shipment not received yet", true);
                return;
            }




            ViewState["gvAVIsInsert"] = false;

            StringBuilder sql = new StringBuilder(2500);


            /*
            // Insert new Item in the  INB_ActualEquipment  Table
            sql.Append("INSERT INTO INB_ActualEquipment (InboundTracking_WarehouseID) values (");
            sql.Append(DB.GetSqlN("select InboundTracking_WarehouseID As N from INB_InboundTracking_Warehouse where IB_RefWarehouse_DetailsID=" + IB_RefWH_DetailsID + "  and InboundID=" + IBTrack.InboundTrackingID.ToString()));
            sql.Append(")");*/


            try
            {
                /*
                DB.ExecuteSQL(sql.ToString());

                gvActualVehicle.EditIndex = gvActualVehicle.Rows.Count;
                 */

                gvActualVehicle.EditIndex = 0;
                gvActualVehicle.PageIndex = 0;

                DataSet dsActualVehicle = this.GENAV_buildGridData();
                DataRow row = dsActualVehicle.Tables[0].NewRow();
                row["EquipmentID"] = 0;
                row["ActualEquipmentID"] = 0;
                dsActualVehicle.Tables[0].Rows.InsertAt(row, 0);


                ActualVehicleSql = "[dbo].[sp_INB_GetActualVehicleDetails]   @InboundTracking_WarehouseID=" + DB.GetSqlN("select InboundTracking_WarehouseID As N from INB_InboundTracking_Warehouse where IsActive=1 and IsDeleted=0  and InboundID=" + CommonLogic.QueryString("ibdid")) + ", @Ref_WHID=" + CommonLogic.IIF(IB_RefWH_DetailsID != "0", IB_RefWH_DetailsID.ToString(), "0");
                ViewState["AVListSQL"] = ActualVehicleSql;
                this.GENAV_buildGridData(dsActualVehicle);

                // this.resetError("New Actual Vehicle line item added", false);
                ViewState["gvAVIsInsert"] = true;
                ViewState["gvAVItemNotUpdated"] = true;

                lnkAddAVNew.Visible = false;

            }
            catch (Exception ex)
            {
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
                this.resetError("Error while inserting New Actual Vehicle line item", true);

            }


        }

        protected void gvActualVehicle_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.DataItem == null)
                return;

            if ((e.Row.RowState & DataControlRowState.Edit) == DataControlRowState.Edit)
            {

                DropDownList ddlEquipment = (DropDownList)e.Row.FindControl("ddlActualEquipment");
                Literal vltEquipmentID = (Literal)e.Row.FindControl("ltEquipmentID");
                CommonLogic.LoadEquipment(ddlEquipment, "Select Equipment Type", "0", cp.TenantID);
                ddlEquipment.SelectedValue = vltEquipmentID.Text;

                ddlEquipment.Focus();

            }
        }

        protected void gvActualVehicle_RowCommand(object sender, GridViewCommandEventArgs e)
        {


            if (e.CommandName == "DeleteItem")
            {
                ViewState["gvAVIsInsert"] = false;
                gvActualVehicle.EditIndex = -1;
                int iden = Localization.ParseNativeInt(e.CommandArgument.ToString());
                this.deleteRowPermEAV(iden);
            }
        }

        protected void gvActualVehicle_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {

            //if (!cp.IsInAnyRoles("21"))
            //{

            //    resetError("Only IB Store-Incharge can update this shipment", true);
            //    return;

            //}

            ViewState["gvAVIsInsert"] = false;
            GridViewRow row = gvActualVehicle.Rows[e.RowIndex];

            if (row != null)
            {

                string ltPrEquipmentID = ((Literal)row.FindControl("lthidActualEquipmentID")).Text;
                string ddlEquipmentID = ((DropDownList)row.FindControl("ddlActualEquipment")).SelectedValue.ToString();
                string vtxtProjectedValue = ((TextBox)row.FindControl("txtActualValue")).Text;
                decimal projval = Convert.ToDecimal(vtxtProjectedValue);

                if(projval == 0)
                {
                    this.resetError("Please enter valid value", true);
                    return;
                }
                StringBuilder sqlUoMDetails = new StringBuilder(2500);
                /*
                sqlUoMDetails.Append("UPDATE INB_ActualEquipment SET ");
                sqlUoMDetails.Append("EquipmentID=" + ddlEquipmentID + ",");
                sqlUoMDetails.Append("ActualValue=" + vtxtProjectedValue);
                sqlUoMDetails.Append("  WHERE ActualEquipmentID=" + ltPrEquipmentID);*/

                String IB_RefWH_DetailsID = DB.GetSqlN("select IB_RefWarehouse_DetailsID AS N from INB_RefWarehouse_Details where IsActive=1 and IsDeleted=0 and WarehouseID=" + ddlStores.SelectedValue + " and InboundID=" + IBTrack.InboundTrackingID.ToString()).ToString();

                String IBTrackWHID = DB.GetSqlN("select InboundTracking_WarehouseID As N from INB_InboundTracking_Warehouse where IB_RefWarehouse_DetailsID=" + IB_RefWH_DetailsID + "  and InboundID=" + IBTrack.InboundTrackingID.ToString()).ToString();

                sqlUoMDetails.Append(" DECLARE @NewUpdateProjectedEquipmentID int;   EXEC [dbo].[sp_INB_UpsertActualVehile]  @ActualEquipmentID=" + ltPrEquipmentID + ",@InboundTracking_WarehouseID=" + IBTrackWHID + ",@EquipmentID=" + ddlEquipmentID + ",@ActualValue=" + vtxtProjectedValue + ",@NewActualEquipmentID=@NewUpdateProjectedEquipmentID OUTPUT; select @NewUpdateProjectedEquipmentID AS N");

                try
                {
                    DB.ExecuteSQL(sqlUoMDetails.ToString());
                    this.resetError("Successfully Updated", false);
                    gvActualVehicle.EditIndex = -1;
                    ViewState["gvAVItemNotUpdated"] = false;
                    this.GENAV_buildGridData(this.GENAV_buildGridData());
                    lnkAddAVNew.Visible = true;
                }
                catch (Exception ex)
                {
                    ViewState["gvAVItemNotUpdated"] = true;
                    CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
                    this.resetError("Error while updating Actual Vehicle details", true);
                }
            }

        }

        protected void gvActualVehicle_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            /*
            if (Localization.ParseBoolean(ViewState["gvAVIsInsert"].ToString()))
            {
                GridViewRow row = gvActualVehicle.Rows[e.RowIndex];
                if (row != null)
                {
                    int iden = Convert.ToInt32(((Literal)row.FindControl("lthidActualEquipmentID")).Text);
                    deleteRowPermEAV(iden);
                    lnkAddAVNew.Visible = true;
                }
            }

            ViewState["gvAVIsInsert"] = false;
             */

            gvActualVehicle.EditIndex = -1;
            this.GENAV_buildGridData(this.GENAV_buildGridData());
            lnkAddAVNew.Visible = true;
        }

        protected void gvActualVehicle_RowEditing(object sender, GridViewEditEventArgs e)
        {

            ViewState["gvAVIsInsert"] = false;

            gvActualVehicle.EditIndex = e.NewEditIndex;

            ViewState["gvAVItemNotUpdated"] = false;

            this.GENAV_buildGridData(this.GENAV_buildGridData());
            lnkAddAVNew.Visible = false;
        }

        protected void lnkDeleteAVItem_Click(object sender, EventArgs e)
        {

            //if (!cp.IsInAnyRoles("21"))
            //{

            //    resetError("Only IB Store-Incharge can update this shipment", true);
            //    return;

            //}

            string rfidIDs = "";

            bool chkBox = false;

            StringBuilder sqlDeleteString = new StringBuilder();
            sqlDeleteString.Append("BEGIN TRAN ");

            //'Navigate through each row in the GridView for checkbox items
            foreach (GridViewRow gv in gvActualVehicle.Rows)
            {
                CheckBox isDelete = (CheckBox)gv.FindControl("chkIsDeleteRFItem1");

                if (isDelete == null)
                    return;

                if (isDelete.Checked)
                {
                    chkBox = true;
                    // Concatenate GridView items with comma for SQL Delete
                    rfidIDs = ((Literal)gv.FindControl("lthidActualEquipmentID")).Text.ToString();



                    if (rfidIDs != "")
                        sqlDeleteString.Append(" Delete from INB_ActualEquipment Where ActualEquipmentID=" + rfidIDs + ";");

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

            this.GENAV_buildGridData(this.GENAV_buildGridData());

        }



        protected void deleteRowPermEAV(int iden)
        {
            StringBuilder sql = new StringBuilder(2500);
            sql.Append("DELETE from INB_ActualEquipment where ActualEquipmentID=" + iden.ToString());
            try
            {
                DB.ExecuteSQL(sql.ToString());
                this.GENAV_buildGridData(this.GENAV_buildGridData());

            }
            catch (Exception ex)
            {
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
                this.resetError("Error while deleting Actual Vehicle details", true);
            }
        }


        #endregion -----  Actual Vehile Grid    ------

        #region ----- Shipment Verification Details ------

        protected void LoadShipmentVerificationData()
        {
            cp = HttpContext.Current.User as CustomPrincipal;
            try
            {
               
                lblVerfShipmentReceivedWH.Text = CommonLogic.IIF(ddlStores.SelectedItem.Text != "Select Store", ddlStores.SelectedItem.Text, "");

                if (ddlStores.SelectedItem.Text != "Select Store")
                {

                    lblVerfShipmentReceivedWH.Text = CommonLogic.IIF(ddlStores.SelectedItem.Text != "Select Store", ddlStores.SelectedItem.Text, "");

                    String IB_RefWH_DetailsID = DB.GetSqlN("select IB_RefWarehouse_DetailsID AS N from INB_RefWarehouse_Details where IsActive=1 and IsDeleted=0 and WarehouseID=" + ddlStores.SelectedValue + " and InboundID=" + IBTrack.InboundTrackingID.ToString()).ToString();

                    if (IB_RefWH_DetailsID != "0")
                    {

                        IDataReader VerificationReader = DB.GetRS("select * from INB_InboundTracking_Warehouse where InboundID=" + IBTrack.InboundTrackingID.ToString() + " and IB_RefWarehouse_DetailsID=" + IB_RefWH_DetailsID);

                        if (!VerificationReader.Read())
                        {

                            txtShipmentVerifiedDate.Text = DateTime.UtcNow.ToString("dd-MMM-yyyy");
                            txtVerificationRemarks.Text = "";
                            txtPackPhyRcvd.Text = "";
                            txtDiscRemarks.Text = "";
                            atcCheckedBy.Text = "";
                            hifCheckedBy.Value = "";
                            txtCheckedDate.Text = DateTime.UtcNow.ToString("dd-MMM-yyyy");
                            txtVerifiedBy.Text = "";
                            txtVerifiedDate.Text = DateTime.UtcNow.ToString("dd-MMM-yyyy");

                            lblVerfShipmentReceivedWH.Text = CommonLogic.IIF(ddlStores.SelectedItem.Text != "Select Store", ddlStores.SelectedItem.Text, "");
                        }
                        else
                        {

                            txtShipmentVerifiedDate.Text = (DB.RSFieldDateTime(VerificationReader, "ShipmentVerifiedOn") == DateTime.MinValue ? DateTime.UtcNow.ToString("dd-MMM-yyyy") : DB.RSFieldDateTime(VerificationReader, "ShipmentVerifiedOn").ToString("dd-MMM-yyyy"));
                            txtVerificationRemarks.Text = DB.RSField(VerificationReader, "Remarks");
                            txtPackPhyRcvd.Text = DB.RSField(VerificationReader, "Disc_PackagesReceived");
                            txtDiscRemarks.Text = DB.RSField(VerificationReader, "Disc_Remarks");
                            atcCheckedBy.Text = DB.GetSqlS("select FirstName AS S from GEN_User where TenantID=" + TenantID + " and IsActive=1" + "and UserID=" + DB.RSFieldInt(VerificationReader, "Disc_CheckedBy").ToString());
                            hifCheckedBy.Value = DB.RSFieldInt(VerificationReader, "Disc_CheckedBy").ToString();
                            txtCheckedDate.Text = (DB.RSFieldDateTime(VerificationReader, "Disc_CheckedDate") == DateTime.MinValue ? DateTime.UtcNow.ToString("dd-MMM-yyyy") : DB.RSFieldDateTime(VerificationReader, "Disc_CheckedDate").ToString("dd-MMM-yyyy"));

                            txtVerifiedBy.Text = DB.RSField(VerificationReader, "Disc_VerifiedBy");

                            txtVerifiedDate.Text = (DB.RSFieldDateTime(VerificationReader, "Disc_VerifiedDate") == DateTime.MinValue ? DateTime.UtcNow.ToString("dd-MMM-yyyy") : DB.RSFieldDateTime(VerificationReader, "Disc_VerifiedDate").ToString("dd-MMM-yyyy"));
                        }

                        //String sFileName = CommonLogic._GetAttatchmentFile(TenantRootDir + DB.GetSqlS("select UniqueID AS S from GEN_Tenant where TenantID=" + TenantID) + InboundPath + InbShpVerifNotePath, IBTrack.StoreRefNumber + "_" + ddlStores.SelectedValue);

                        String sFileName = CommonLogic._GetAttatchmentFile(TenantRootDir +  IBTrack.TenantID + InboundPath + InbShpVerifNotePath, IBTrack.StoreRefNumber + "_" + ddlStores.SelectedValue);


                        if (sFileName != "")
                        {
                            String Path = "../ViewImage.aspx?path=" + sFileName;
                            divShpVeriNote.InnerHtml = "<img src=\"../Images/blue_menu_icons/attachment.png\"  />";
                            divShpVeriNote.InnerHtml += "<a style=\"text-decoration:none;\" href=\"#\" onclick=\" OpenImage(' " + Path + " ')  \" > View Verification Doc. </a>";
                            divShpVeriNote.InnerHtml += "<img src=\"../Images/redarrowright.gif\"   />";
                        }

                        VerificationReader.Close();

                    }

                }

                else
                {

                    txtShipmentVerifiedDate.Text = "";
                    txtVerificationRemarks.Text = "";
                    txtPackPhyRcvd.Text = "";
                    txtDiscRemarks.Text = "";
                    atcCheckedBy.Text = "";
                    hifCheckedBy.Value = "";
                    txtCheckedDate.Text = "";
                    txtVerifiedBy.Text = "";
                    txtVerifiedDate.Text = "";
                    lblVerfShipmentReceivedWH.Text = CommonLogic.IIF(ddlStores.SelectedItem.Text != "Select Store", ddlStores.SelectedItem.Text, "");
                }



            }
            catch (Exception ex)
            {
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
                resetError(" Error while loading verification details", true);
            }

        }

        protected void chkHasDiscrepancy_CheckedChanged(object sender, EventArgs e)
        {
            if (chkHasDiscrepancy.Checked)
            {

                String IB_RefWH_DetailsID = DB.GetSqlN("select IB_RefWarehouse_DetailsID AS N from INB_RefWarehouse_Details where IsActive=1 and IsDeleted=0 and WarehouseID=" + ddlStores.SelectedValue + " and InboundID=" + IBTrack.InboundTrackingID.ToString()).ToString();


                if (DB.GetSqlS("select CONVERT(nvarchar(20), ShipmentReceivedOn) AS S  from INB_InboundTracking_Warehouse where IsActive=1 and IsDeleted=0  and IB_RefWarehouse_DetailsID =" + IB_RefWH_DetailsID) == "")
                {
                    resetError("Shipment not received yet", true);
                    return;
                }


                if (DB.GetSqlS("select CONVERT(nvarchar(20), ShipmentVerifiedOn) AS S  from INB_InboundTracking_Warehouse where IsActive=1 and IsDeleted=0  and IB_RefWarehouse_DetailsID =" + IB_RefWH_DetailsID) != "")
                {
                    resetError("This shipment is already verified", true);
                    return;
                }


                //if (DB.GetSqlN("Declare  @GoodsINStatus int; EXEC [sp_INV_CheckGoodsINStatus] @InboundID=" + IBTrack.InboundTrackingID + ",@Status=@GoodsINStatus out; select @GoodsINStatus as N;") == 0)
                //{
                //    resetError("Not yet 'Goods-IN' ", true);
                //    return;
                //}



                pnlDiscInfo.Visible = true;
                LoadDiscrFormDetails();



            }
            else
                pnlDiscInfo.Visible = false;


        }

        protected void LoadDiscrFormDetails()
        {

            try
            {
               
                if (ddlStores.SelectedItem.Text != "Select Store")
                {

                    String IB_RefWH_DetailsID = DB.GetSqlN("select IB_RefWarehouse_DetailsID AS N from INB_RefWarehouse_Details where IsActive=1 and IsDeleted=0 and WarehouseID=" + ddlStores.SelectedValue + " and InboundID=" + IBTrack.InboundTrackingID.ToString()).ToString();

                    if (IB_RefWH_DetailsID != "0")
                    {
                        ltSupplierVal.Text = IBTrack.SupplierName;
                        ltNoofPackagesInDocumentVal.Text = IBTrack.NoofPackagesInDocument.ToString();
                        ltStrrefNoVal.Text = IBTrack.StoreRefNumber;
                        ltStoreVal.Text = DB.GetSqlS(" select GEN_W.WHCode AS S from INB_RefWarehouse_Details INB_R  left join GEN_Warehouse GEN_W on GEN_W.WarehouseID=INB_R.WarehouseID where IB_RefWarehouse_DetailsID=" + IB_RefWH_DetailsID + " and InboundID=" + IBTrack.InboundTrackingID.ToString());
                        ltCCVal.Text = DB.GetSqlS("select ClearanceCompany AS S from GEN_ClearanceCompany where IsActive=1 and ClearanceCompanyID=" + IBTrack.ClearenceCompanyID);
                        ltShipmentRcvdVal.Text = DB.GetSqlS("select convert(nvarchar(50),ShipmentReceivedOn) AS S  from INB_InboundTracking_Warehouse where InboundID=" + IBTrack.InboundTrackingID + " and IB_RefWarehouse_DetailsID=" + IB_RefWH_DetailsID);
                        ltShipmentRcvdVal.Text = ltShipmentRcvdVal.Text != "" ? Convert.ToDateTime(Convert.ToDateTime(ltShipmentRcvdVal.Text).ToShortDateString(), CultureInfo.CurrentCulture).ToString("dd-MMM-yyyy").ToString() : Convert.ToDateTime(DateTime.UtcNow, CultureInfo.CurrentCulture).ToString("dd-MMM-yyyy").ToString();



                        ltAWBVal.Text = IBTrack.ConsignmentNoteTypeValue;
                    }

                }
                else
                {
                    ltStoreVal.Text = "";
                    ltShipmentRcvdVal.Text = "";
                }


            }
            catch (Exception ex)
            {
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
                resetError("Error while loading discrepancy details", true);
            }


        }

        protected void btnGDRPrint_Click(object sender, ImageClickEventArgs e)
        {

        }

        protected DataSet DiscrepancyDetails_buildGridData()
        {
            string sql = ViewState["DiscrepancyListSQL"].ToString();
            DataSet ds = DB.GetDS(sql, false);
            return ds;
        }

        protected void DiscrepancyDetails_buildGridData(DataSet ds)
        {
            gvDiscpreancyRecords.DataSource = ds.Tables[0];
            gvDiscpreancyRecords.DataBind();
            ds.Dispose();
        }

        protected void lnkAddNewDiscrepancy_Click(object sender, EventArgs e)
        {
          
            //if (!cp.IsInAnyRoles("21"))
            //{

            //    resetError("Only IB Store-Incharge can update this shipment", true);
            //    return;

            //}



            int Ref_WHID = DB.GetSqlN("select IB_RefWarehouse_DetailsID  AS N from INB_RefWarehouse_Details where IsActive=1 and IsDeleted=0 and InboundID=" + IBTrack.InboundTrackingID + " and  WarehouseID=" + ddlStores.SelectedValue);

            if (DB.GetSqlS("select CONVERT(nvarchar(20), ShipmentVerifiedOn) AS S  from INB_InboundTracking_Warehouse where IsActive=1 and IsDeleted=0 and IB_RefWarehouse_DetailsID =" + Ref_WHID) != "")
            {
                resetError("Cannot change the shipment details, as shipment is verified", true);
                return;
            }


            //if (DB.GetSqlN("Declare  @GoodsINStatus int; EXEC [sp_INV_CheckGoodsINStatus] @InboundID=" + IBTrack.InboundTrackingID + ",@Status=@GoodsINStatus out; select @GoodsINStatus as N;") == 0)
            //{
            //    resetError("Goods-IN is not yet performed", true);
            //    return;
            //}





            ViewState["gvDescpIsInsert"] = false;

            StringBuilder sql = new StringBuilder(2500);



            /*
            int LineItemCount = DB.GetSqlN("SELECT MAX(LineNumber) AS N  FROM  INB_Discrepancy " + DB.GetNoLock() + " WHERE IB_RefWarehouse_DetailsID=" + Ref_WHID);

            LineItemCount = LineItemCount + 1;


            sql.Append("INSERT INTO INB_Discrepancy (LineNumber,CreatedBy,IB_RefWarehouse_DetailsID) values (");
            sql.Append(LineItemCount + "," + CreatedBy + "," + Ref_WHID);
            sql.Append(")");*/

            try
            {
                /* DB.ExecuteSQL(sql.ToString());

                 gvDiscpreancyRecords.EditIndex = gvDiscpreancyRecords.Rows.Count;*/

                gvDiscpreancyRecords.EditIndex = 0;
                gvDiscpreancyRecords.PageIndex = 0;

                DataSet dsDiscpreancyRecords = this.DiscrepancyDetails_buildGridData();
                DataRow row = dsDiscpreancyRecords.Tables[0].NewRow();
                row["InvoiceNumber"] = "";
                row["DiscrepancyID"] = 0;
                dsDiscpreancyRecords.Tables[0].Rows.InsertAt(row, 0);

                this.DiscrepancyDetails_buildGridData(dsDiscpreancyRecords);

                // this.resetError("New Discrepancy line item added", false);

                ViewState["gvDescpIsInsert"] = true;
                ViewState["DescpNotUpdated"] = true;

                lnkAddNewDiscrepancy.Visible = false;

            }
            catch (Exception ex)
            {
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
                this.resetError("Error while inserting discrepancy line item", true);

            }



        }

        protected void gvDiscpreancyRecords_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.DataItem == null)
            {
                return;
            }
            else
            {
                // FileUpload flUpload = e.Row.FindControl("descfile_upload") as FileUpload;
                // ScriptManager.GetCurrent(this).RegisterPostBackControl(flUpload); 
            }

            if ((e.Row.RowState & DataControlRowState.Edit) == DataControlRowState.Edit)
            {
                TextBox txtMCode = (TextBox)e.Row.FindControl("txtDInvoiceNumber");
                txtMCode.Focus();

            }
            if (!(e.Row.RowState == DataControlRowState.Edit) && e.Row.RowType == DataControlRowType.DataRow)
            {
                Literal damage = (Literal)e.Row.FindControl("ltDiscAtatchment");
                string ltDiscrepancyID = ((Literal)e.Row.FindControl("ltDiscrepancyID")).Text;

                //String sFileName = CommonLogic._GetAttatchmentFile(TenantRootDir + DB.GetSqlS("select UniqueID AS S from GEN_Tenant where TenantID=" + TenantID) + InboundPath + InbDescPath, IBTrack.StoreRefNumber + "_" + ltDiscrepancyID);

                String sFileName = CommonLogic._GetAttatchmentFile(TenantRootDir +  IBTrack.TenantID + InboundPath + InbDescPath, IBTrack.StoreRefNumber + "_" + ltDiscrepancyID);
                if (sFileName != "")
                {
                    String Path = "../ViewImage.aspx?path=" + sFileName;
                    damage.Text = "<nobr><img src=\"../Images/blue_menu_icons/attachment.png\"  />";
                    damage.Text += "<a style=\"text-decoration:none;\" href=\"#\" onclick=\" OpenImage(' " + Path + " ')  \" >View Dis. Doc.</a>";
                    damage.Text += "<img src=\"../Images/redarrowright.gif\"   /></nobr>";
                }
            }

        }

        protected void gvDiscpreancyRecords_RowEditing(object sender, GridViewEditEventArgs e)
        {

            ViewState["gvDescpIsInsert"] = false;

            gvDiscpreancyRecords.EditIndex = e.NewEditIndex;

            ViewState["DescpNotUpdated"] = false;

            this.DiscrepancyDetails_buildGridData(this.DiscrepancyDetails_buildGridData());

            lnkAddNewDiscrepancy.Visible = false;

        }

        protected void gvDiscpreancyRecords_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
          
            string TenantId = hidTenantID.Value;
            //if (!cp.IsInAnyRoles("21"))
            //{

            //    resetError("Only IB Store-Incharge can update this shipment", true);
            //    return;

            //}
            if (chkHasDiscrepancy.Checked == false)
            {
                resetError("Please check Has Discrepancy.", true);
                return;
            }



            int Ref_WHID = DB.GetSqlN("select IB_RefWarehouse_DetailsID  AS N from INB_RefWarehouse_Details where IsActive=1 AND IsDeleted=0 AND InboundID=" + IBTrack.InboundTrackingID + " and  WarehouseID=" + ddlStores.SelectedValue);


            if (DB.GetSqlS("select CONVERT(nvarchar(20), ShipmentVerifiedOn) AS S  from INB_InboundTracking_Warehouse where IsActive=1 AND IsDeleted=0 AND IB_RefWarehouse_DetailsID =" + Ref_WHID) != "")
            {
                resetError("Cannot change the shipment details, as shipment is verified", true);
                return;
            }


            ViewState["gvDescpIsInsert"] = false;
            GridViewRow row = gvDiscpreancyRecords.Rows[e.RowIndex];

            if (row != null)
            {

                string ltInvNumber = ((TextBox)row.FindControl("txtDInvoiceNumber")).Text;
                string ltDescPONumber = ((TextBox)row.FindControl("txtDescPONumber")).Text;

                string ltDiscrepancyID = ((Literal)row.FindControl("ltDiscrepancyID")).Text;
                string txtDamageDescription = ((TextBox)row.FindControl("txtDamage")).Text;
                string txtRecvQty = ((TextBox)row.FindControl("txtDReceivedQty")).Text;
                string MMID = ((HiddenField)row.FindControl("hifDescMCodeID")).Value;
                string txtMCode = ((TextBox)row.FindControl("txtDMCode")).Text;
                string POLineNo = ((TextBox)row.FindControl("txtLineNumber")).Text;

                FileUpload descFileUpload = ((FileUpload)row.FindControl("descfile_upload"));


                int vDescPOHeaderID = DB.GetSqlN("select POHeaderID as N from ORD_POHeader where IsActive=1 and IsDeleted=0 and PONumber=" + DB.SQuote(ltDescPONumber) + " and TenantID=" + Convert.ToInt32(TenantId) + "");
                if (vDescPOHeaderID == 0)
                {
                    this.resetError("PO Number does not exist", true);
                    return;
                }

                int InvHeaderID = DB.GetSqlN("select SupplierInvoiceID AS N from ORD_SupplierInvoice ORD_SUP  JOIN ORD_POHeader ORD_POH ON   ORD_POH.POHeaderID=ORD_SUP.POHeaderID AND ORD_POH.IsActive=1 AND ORD_POH.IsDeleted=0 AND  ORD_POH.POHeaderID=" + vDescPOHeaderID + "  AND ORD_POH.SupplierID=" + IBTrack.Supplier + " where ORD_SUP.IsActive=1 and ORD_SUP.IsDeleted=0 and ORD_SUP.InvoiceNumber=" + DB.SQuote(ltInvNumber));

                if (InvHeaderID == 0)// Check Inv Number
                {
                    this.resetError("Invoice Number does not exist", true);
                    ViewState["DescpNotUpdated"] = false;
                    return;
                }




                int POHeaderID = DB.GetSqlN("select POHeaderID AS N from ORD_SupplierInvoice where IsActive=1 and IsDeleted=0 and SupplierInvoiceID=" + InvHeaderID);



                int vMMID = DB.GetSqlN("Select MaterialMasterID AS N from MMT_MaterialMaster where IsActive=1 and IsDeleted=0 and MCode=" + DB.SQuote(txtMCode) + " and TenantID=" + Convert.ToInt32(TenantId) + "");




                //if (DB.GetSqlN("select  MMT_MM.MaterialMasterID AS N from INV_GoodsMovementDetails INV_GMD JOIN MMT_MaterialMaster MMT_MM ON MMT_MM.MaterialMasterID=INV_GMD.MaterialMasterID AND MMT_MM.IsActive=1 AND MMT_MM.IsDeleted=0 JOIN ORD_SupplierInvoiceDetails ORD_SD  ON ORD_SD.SupplierInvoiceDetailsID=INV_GMD.POSODetailsID AND  ORD_SD.IsActive=1 AND ORD_SD.IsDeleted=0  where ( INV_GMD.IsDamaged=1 OR INV_GMD.HasDiscrepancy=1 OR INV_GMD.IsNonConfirmity=1 ) AND INV_GMD.IsDeleted=0 AND INV_GMD.TransactionDocID=" + InboundID + " AND INV_GMD.GoodsMovementTypeID=1 AND ORD_SD.SupplierInvoiceID=" + InvHeaderID + "   and MMT_MM.MCode ='" + txtMCode + "'") == 0)
                //{
                //    this.resetError("This part number is not configured in PO line items", true);
                //    ViewState["DescpNotUpdated"] = false;
                //    return;
                //}


                if (DB.GetSqlN("select distinct ORD_POD.LineNumber AS N from ORD_PODetails ORD_POD  LEFT JOIN MMT_MaterialMaster MMT_M on MMT_M.MaterialMasterID=ORD_POD.MaterialMasterID and MMT_M.IsDeleted=0 and MMT_M.IsActive=1 and MMT_M.IsAproved=1 LEFT JOIN ORD_SupplierInvoiceDetails ORD_SUPID ON ORD_SUPID.PODetailsID=ORD_POD.PODetailsID AND ORD_SUPID.IsActive=1 AND ORD_SUPID.IsDeleted=0 LEFT JOIN INB_Inbound_ORD_SupplierInvoice INB_Sup on  INB_Sup.SupplierInvoiceID=ORD_SUPID.SupplierInvoiceID AND INB_Sup.POHeaderID=ORD_POD.POHeaderID and INB_Sup.IsActive=1 and INB_Sup.IsDeleted=0 where  ORD_POD.POHeaderID=" + vDescPOHeaderID + " AND ORD_POD.IsDeleted=0 and ORD_POD.IsActive=1  and  INB_Sup.SupplierInvoiceID=" + InvHeaderID + " AND ORD_POD.MaterialMasterID=" + MMID + "  AND InboundID=" + InboundID + " AND ORD_POD.LineNumber=" + POLineNo) == 0)
                {
                    this.resetError("Invalid PO line number", true);
                    return;
                }

               
                StringBuilder sqlDescpRecords = new StringBuilder(2500);

                sqlDescpRecords.Append("DECLARE @NewUpdateInboundDiscID int;  EXEC [sp_INB_UpsertDiscrepancyDetails]    @DiscrepancyID=" + ltDiscrepancyID);
                sqlDescpRecords.Append(",@IB_RefWarehouse_DetailsID=" + Ref_WHID);
                sqlDescpRecords.Append(",@MaterialMasterID=" + vMMID);
                sqlDescpRecords.Append(",@SupplierInvoiceID=" + InvHeaderID);
                sqlDescpRecords.Append(",@LineNumber=" + POLineNo);
                sqlDescpRecords.Append(",@ReceivedQuantity=" + txtRecvQty);
                sqlDescpRecords.Append(",@DiscrepancyDescription=" + DB.SQuote(txtDamageDescription));
                sqlDescpRecords.Append(",@CreatedBy=" + CreatedBy);
                sqlDescpRecords.Append(",@NewInbound_DiscrepancyID=@NewUpdateInboundDiscID OUTPUT;  select @NewUpdateInboundDiscID AS N");

                string Query_DESC = "EXEC [dbo].[sp_INB_UpsertDiscrepancy] @IB_RefWarehouse_DetailsID = " + Ref_WHID + " , @MaterialMasterID = "+ vMMID + " , "+
                                    " @SupplierInvoiceID = "+ InvHeaderID + ", @LineNumber = "+ POLineNo + ", @ReceivedQuantity= "+ txtRecvQty + ", "+
                                    " @DiscrepancyDescription = "+DB.SQuote(txtDamageDescription) + ", @CreatedBy="+ CreatedBy + "";
               
                try
                {
                    //ltDiscrepancyID = DB.GetSqlN(sqlDescpRecords.ToString()).ToString();
                    int Result_Desc = DB.GetSqlN(Query_DESC);
                    if(Result_Desc == 1)
                    {
                        this.resetError("Successfully Updated", false);
                    }
                    else if(Result_Desc == -1)
                    {
                         this.resetError("No excess and shortage for this Item", true);
                        return;
                    }
                    else if (Result_Desc == -2)
                    {
                        this.resetError("Total Qty. is more than Invoice Qty.", true);
                        return;
                    }
                    else if (Result_Desc == -3)
                    {
                        this.resetError("Quantity is more than excess received qty.", true);
                        return;
                    }
                    if (descFileUpload.HasFile)
                    {
                        String FileExtenion = Path.GetExtension(descFileUpload.FileName);
                        //if (CommonLogic.UploadFile(TenantRootDir + DB.GetSqlS("select UniqueID AS S from GEN_Tenant where TenantID=" + TenantID) + InboundPath + InbDescPath, descFileUpload, IBTrack.StoreRefNumber + "_" + ltDiscrepancyID + Path.GetExtension(descFileUpload.FileName)) == false)

                        if (CommonLogic.UploadFile(TenantRootDir +  IBTrack.TenantID + InboundPath + InbDescPath, descFileUpload, IBTrack.StoreRefNumber + "_" + ltDiscrepancyID + Path.GetExtension(descFileUpload.FileName)) == false)
                        {
                            //Error in uploading the delivery note file
                            resetError("Error while uploading file", true);
                            return;
                        }
                    }
                    gvDiscpreancyRecords.EditIndex = -1;

                    this.DiscrepancyDetails_buildGridData(this.DiscrepancyDetails_buildGridData());

                    ViewState["DescpNotUpdated"] = false;

                    lnkAddNewDiscrepancy.Visible = true;
                }
                catch (SqlException sqlex)
                {
                    CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), sqlex.Source, sqlex.Message, sqlex.StackTrace);
                    if (sqlex.ErrorCode == -2146232060)
                    {
                        this.resetError("This line item is already updated", true);
                        return;
                    }
                }
                catch (Exception ex)
                {
                    ViewState["DescpNotUpdated"] = true;
                    CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
                    this.resetError("Error while updating discrepancy details", true);
                }
            }


        }

        protected void gvDiscpreancyRecords_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

            ViewState["gvDescpIsInsert"] = false;

            gvDiscpreancyRecords.PageIndex = e.NewPageIndex;
            gvDiscpreancyRecords.EditIndex = -1;
            this.DiscrepancyDetails_buildGridData(this.DiscrepancyDetails_buildGridData());
        }

        protected void gvDiscpreancyRecords_Sorting(object sender, GridViewSortEventArgs e)
        {

        }

        protected void gvDiscpreancyRecords_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            /*
            if (Localization.ParseBoolean(ViewState["gvDescpIsInsert"].ToString()) || Localization.ParseBoolean(ViewState["DescpNotUpdated"].ToString()))
            {
                GridViewRow row = gvDiscpreancyRecords.Rows[e.RowIndex];
                if (row != null)
                {
                    int iden = Convert.ToInt32(((Literal)row.FindControl("ltDiscrepancyID")).Text);
                    deleteRowPermDescRecord(iden);
                    resetError("Line Item Canceled",true);
                    lnkAddNewDiscrepancy.Visible = true;
                }
            }

            ViewState["gvDescpIsInsert"] = false;
            */
            gvDiscpreancyRecords.EditIndex = -1;
            this.DiscrepancyDetails_buildGridData(this.DiscrepancyDetails_buildGridData());
            lnkAddNewDiscrepancy.Visible = true;

        }

        protected void lnkCancelVerified_Click(object sender, EventArgs e)
        {


            //if (!cp.IsInAnyRoles("21"))
            //{

            //    resetError("Only IB Store-Incharge can update this shipment", true);
            //    return;

            //}


            String IB_RefWH_DetailsID = DB.GetSqlN("select IB_RefWarehouse_DetailsID AS N from INB_RefWarehouse_Details where IsActive=1 and IsDeleted=0 and WarehouseID=" + ddlStores.SelectedValue + " and InboundID=" + IBTrack.InboundTrackingID.ToString()).ToString();

            if (DB.GetSqlS("select CONVERT(nvarchar(20), ShipmentVerifiedOn) AS S  from INB_InboundTracking_Warehouse where IsActive=1 and IsDeleted=0  and IB_RefWarehouse_DetailsID =" + IB_RefWH_DetailsID) != "")
            {
                resetError("This shipment is already verified", true);
                return;
            }

            txtPackPhyRcvd.Text = "";
            atcCheckedBy.Text = "";
            txtCheckedDate.Text = "";
            txtVerifiedBy.Text = "";
            txtVerifiedDate.Text = "";
            txtDiscRemarks.Text = "";
            txtVerificationRemarks.Text = "";
            string ss = txtVerificationRemarks.Text;
            //Response.Redirect("InboundDetails.aspx?ibdid=" + IBTrack.InboundTrackingID.ToString() + "&ibupdate=updateok"); // add to this stmt #invoice_det
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
           
            //if (!cp.IsInAnyRoles("21"))
            //{

            //    resetError("Only IB Store-Incharge can update this shipment", true);
            //    return;

            //}

            int Ref_WHID = DB.GetSqlN("select IB_RefWarehouse_DetailsID  AS N from INB_RefWarehouse_Details where IsActive=1 and IsDeleted=0 and InboundID=" + IBTrack.InboundTrackingID + " and  WarehouseID=" + ddlStores.SelectedValue);

            if (DB.GetSqlS("select CONVERT(nvarchar(20), ShipmentVerifiedOn) AS S  from INB_InboundTracking_Warehouse where IsActive=1 and IsDeleted=0 and IB_RefWarehouse_DetailsID =" + Ref_WHID) != "")
            {
                resetError("Cannot change the shipment details, as shipment is verified", true);
                return;
            }


            string rfidIDs = "";

            bool chkBox = false;

            StringBuilder sqlDeleteString = new StringBuilder();
            sqlDeleteString.Append("BEGIN TRAN ");

            //'Navigate through each row in the GridView for checkbox items
            foreach (GridViewRow gv in gvDiscpreancyRecords.Rows)
            {

                CheckBox isDelete = (CheckBox)gv.FindControl("deleteRec");

                if (isDelete == null)
                    return;

                if (isDelete.Checked)
                {
                    chkBox = true;
                    // Concatenate GridView items with comma for SQL Delete
                    rfidIDs = ((Literal)gv.FindControl("ltDiscrepancyID")).Text.ToString();



                    if (rfidIDs != "")
                        sqlDeleteString.Append(" Delete from INB_Discrepancy Where DiscrepancyID=" + rfidIDs + ";");

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
                resetError("Successfully deleted the selected line items", false);

            }
            catch (Exception ex)
            {
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
                resetError("Error while deleting discrepancy line items", true);
            }

            this.DiscrepancyDetails_buildGridData(this.DiscrepancyDetails_buildGridData());





        }

        protected void deleteRowPermDescRecord(int iden)
        {
        
            //if (!cp.IsInAnyRoles("21"))
            //{

            //    resetError("Only IB Store-Incharge can update this shipment", true);
            //    return;

            //}


            StringBuilder sql = new StringBuilder(2500);
            sql.Append("DELETE from INB_Discrepancy where DiscrepancyID=" + iden.ToString());
            try
            {
                DB.ExecuteSQL(sql.ToString());
                this.DiscrepancyDetails_buildGridData(this.DiscrepancyDetails_buildGridData());

            }
            catch (Exception ex)
            {
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
                this.resetError("Error while deleting discrepancy line items", true);
            }

        }

        protected void lnkUpdateShipmentVerified_Click(object sender, EventArgs e)
        {
            

            // lnkUpdateShipmentVerified.OnClientClick = "openDialog('Add/Edit Kit Items', '" + lnkUpdateShipmentVerified.ClientID + "');"; 

            // shipment verification has disabled on 3/1/2018  ---

            //if (!cp.IsInAnyRoles("21"))
            //{

            //    resetError("Only IB Store-Incharge can update this shipment", true);
            //    return;

            //}


            Page.Validate("updateShipmentVerified");

            if (Page.IsValid == false)
            {
                return;
            }


            // Modified by swamy on 27/MAY/2020
            int Result_GRN = DB.GetSqlN("SELECT COUNT(*) as N from vAvailableStock where InboundID=" + IBTrack.InboundTrackingID + " AND Quantity>0 AND SYS_SlocID=1");
            if (Result_GRN != 0)
            {
                resetError("GRN is not at done for received items", true);
                return;
            }


            int HasDesc = DB.GetSqlN("EXEC GET_Discrepancy_VEREFIY @INBOUNDID = "+ IBTrack.InboundTrackingID + "");
            if(HasDesc == -4)
            {
                resetError("This shipment has discrepancy. Please add discrepancy line items ", true);
                return;
            }
            else if(HasDesc == -5)
            {
                resetError("This shipment has discrepancy. Please add discrepancy line items ", true);
                return;
            }
            else if(HasDesc == -3)
            {
                resetError("Goods-IN is not yet performed", true);
                return;
            }
            else if (HasDesc == -6)
            {
                resetError("Discrepancy verification not at done", true);
                return;
            }


           

            char[] seps = new char[] { ',' };

            String ReferedWHs = IBTrack.ReferedStoreIDs.Substring(0, IBTrack.ReferedStoreIDs.Length - 1);

            String[] ArrStores = CommonLogic.FilterSpacesInArrElements(ReferedWHs.Split(seps));



            try
            {
                String IB_RefWH_DetailsID = DB.GetSqlN("select IB_RefWarehouse_DetailsID AS N from INB_RefWarehouse_Details where IsActive=1 and IsDeleted=0 and WarehouseID=" + ddlStores.SelectedValue + " and InboundID=" + IBTrack.InboundTrackingID.ToString()).ToString();


                if (DB.GetSqlS("select CONVERT(nvarchar(50), ShipmentReceivedOn) AS S  from INB_InboundTracking_Warehouse where IsActive=1 and IsDeleted=0  and IB_RefWarehouse_DetailsID =" + IB_RefWH_DetailsID) == "")
                {
                    resetError("Shipment not received yet", true);
                    return;
                }
                if (DB.GetSqlN("EXEC [sp_CheckPalletPutaway] @TransactionDocId=" + IBTrack.InboundTrackingID) == 0)
                {
                    resetError("Please put Away the Container", true);
                    return;
                }

                if (DB.GetSqlS("select CONVERT(nvarchar(20), ShipmentVerifiedOn) AS S  from INB_InboundTracking_Warehouse where IsActive=1 and IsDeleted=0  and IB_RefWarehouse_DetailsID =" + IB_RefWH_DetailsID) != "")
                {
                    resetError("This shipment is already verified", true);
                    return;
                }


                if (DB.GetSqlN("select top 1 GoodsMovementDetailsID AS N from INV_GoodsMovementDetails where IsDeleted=0 AND TransactionDocID=" + IBTrack.InboundTrackingID) == 0)
                {
                    resetError("Goods-IN is not yet performed", true);
                    return;
                }

                //if (DB.GetSqlN("Declare  @GoodsINStatus int; EXEC [sp_INV_CheckGoodsINStatus] @InboundID=" + IBTrack.InboundTrackingID + ",@Status=@GoodsINStatus out; select @GoodsINStatus as N;") == 0)
                //{
                //    //resetError("Goods-IN is not yet performed", true);
                //    resetError("GRN is not yet performed for All LineItems", true);
                //    return;
                //}

                //String sFileName = CommonLogic._GetAttatchmentFile(TenantRootDir + DB.GetSqlS("select UniqueID AS S from GEN_Tenant where TenantID=" + TenantID) + InboundPath + InbShpVerifNotePath, IBTrack.StoreRefNumber + "_" + ddlStores.SelectedValue);

                //String sFileName = CommonLogic._GetAttatchmentFile(TenantRootDir +  IBTrack.TenantID + InboundPath + InbShpVerifNotePath, IBTrack.StoreRefNumber + "_" + ddlStores.SelectedValue);

                //if (sFileName == "")
                //{
                    
                //    if (!fileupShipverificationNote.HasFile)
                //    {
                //        //resetError("Sorry, verification sheet not attached", true);
                //        //return;
                //    }
                //    if (fileupShipverificationNote.HasFile)
                //    {
                //        resetError("Sorry, verification sheet not attached", true);
                //        return;
                //    }
                //   // return;
                //}

                if (DB.GetSqlN("EXEC [dbo].[sp_INV_InwardQCCheck] @InboundID=" + IBTrack.InboundTrackingID + ",@SupplierInvoiceID=0") != 0)
                {
                    //resetError("For this invoice QC capture yet to complete ", true);
                    resetError("QC yet to be captured ", true);
                    return;
                }
                StringBuilder sqlUpdate = new StringBuilder();

                String ShipmentVerifiedDate;

                if (txtShipmentVerifiedDate.Text.Trim() != "")
                    //ShipmentVerifiedDate =  DB.SQuote(CommonLogic.GetConfigValue(cp.TenantID, "DateFormat") == "dd/MM/yyyy" ? DateTime.ParseExact(txtShipmentVerifiedDate.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy") : txtShipmentVerifiedDate.Text.Trim());
                    // ShipmentVerifiedDate = CommonLogic.FormatDate(txtShipmentVerifiedDate.Text, '/');
                    ShipmentVerifiedDate = txtShipmentVerifiedDate.Text;
                else
                    ShipmentVerifiedDate = null;


                String ShprcvdDate = DB.GetSqlS("select CONVERT(nvarchar, ShipmentReceivedOn,103) AS S  from INB_InboundTracking_Warehouse where IsActive=1 and IsDeleted=0  and IB_RefWarehouse_DetailsID =" + IB_RefWH_DetailsID);

                DateTime shipRcvdDt = DateTime.ParseExact(ShprcvdDate, "dd/MM/yyyy", CultureInfo.InvariantCulture); //Convert.ToDateTime(CommonLogic.FormatDate(ShprcvdDate, '/')); 

                DateTime shipVrfDt = DateTime.ParseExact(txtShipmentVerifiedDate.Text, "dd-MMM-yyyy", CultureInfo.InvariantCulture); //Convert.ToDateTime(CommonLogic.FormatDate(txtShipmentVerifiedDate.Text, '/'));

                if (shipVrfDt < shipRcvdDt)
                {
                    resetError("Verification date cannot deceed shipment received date", true);
                    return;
                }

                ShipmentVerifiedDate = (ShipmentVerifiedDate != "" ? DateTime.ParseExact(ShipmentVerifiedDate + " " + DateTime.UtcNow.ToString("hh:mm:ss.fff"), "dd-MMM-yyyy hh:mm:ss.fff", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy hh:mm:ss.fff") : null);
                sqlUpdate.Append("Update INB_InboundTracking_Warehouse set ShipmentVerifiedOn=" + (ShipmentVerifiedDate == null ? null : DB.SQuote(ShipmentVerifiedDate)));

                sqlUpdate.Append(",Remarks=" + DB.SQuote(txtVerificationRemarks.Text.ToString()));
              //  sqlUpdate.Append(",Disc_PackagesReceived=" + CommonLogic.IIF(txtPackPhyRcvd.Text != "", DB.SQuote(txtPackPhyRcvd.Text), "NULL"));

              //  sqlUpdate.Append(",Disc_Remarks=" + DB.SQuote(txtDiscRemarks.Text));

                //string checkedBy = Request.Form[hifCheckedBy.UniqueID];

                //if (checkedBy != null)
                //    sqlUpdate.Append(",Disc_CheckedBy=" + checkedBy);
                //else
                //    sqlUpdate.Append(",Disc_CheckedBy=NULL");



                String CheckedDate;

                //if (txtCheckedDate.Text.Trim() != "")
                //    //CheckedDate = DB.SQuote(CommonLogic.GetConfigValue(cp.TenantID, "DateFormat") == "dd/MM/yyyy" ? DateTime.ParseExact(txtCheckedDate.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy") : txtCheckedDate.Text.Trim());
                //    CheckedDate = CommonLogic.FormatDate(txtCheckedDate.Text, '/');
                //else
                //    CheckedDate = null;



                //sqlUpdate.Append(",Disc_CheckedDate=" + (CheckedDate == null ? "null" : DB.SQuote(CheckedDate)));

               // sqlUpdate.Append(",Disc_VerifiedBy=" + CommonLogic.IIF(txtVerifiedBy.Text != "", DB.SQuote(txtVerifiedBy.Text), "NULL"));






                String VerifiedDate;

                //if (txtVerifiedDate.Text.Trim() != "")
                //    //VerifiedDate =DB.SQuote(CommonLogic.GetConfigValue(cp.TenantID, "DateFormat") == "dd/MM/yyyy" ? DateTime.ParseExact(txtVerifiedDate.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy") : txtVerifiedDate.Text.Trim());
                //    VerifiedDate = CommonLogic.FormatDate(txtVerifiedDate.Text, '/');
                //else
                //    VerifiedDate = null;

                //if (chkHasDiscrepancy.Checked)
                //{

                //    if (DB.GetSqlN("select top 1 TransactionDocID AS N from INV_GoodsMovementDetails where GoodsMovementTypeID=1 AND TransactionDocID=" + IBTrack.InboundTrackingID + " AND IsDeleted=0 AND (IsDamaged=1 OR HasDiscrepancy=1 OR IsNonConfirmity=1)") == 0)
                //    {

                //        resetError("This shipment  does not have any discrepancies. Please remove discrepancy checkmark", true);
                //        return;
                //    }
                //    else
                //    {

                //        //(txtOBDDate.Text != "" ? DateTime.ParseExact(txtOBDDate.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy") : null);

                //        DateTime CheckedDt = DateTime.ParseExact(txtCheckedDate.Text.Trim(), "dd-MMM-yyyy", CultureInfo.InvariantCulture); // Convert.ToDateTime(CommonLogic.GetConfigValue(cp.TenantID, "DateFormat") == "dd/MM/yyyy" ? DateTime.ParseExact(txtCheckedDate.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy") : txtCheckedDate.Text.Trim());

                //        if (CheckedDt < shipRcvdDt)
                //        {
                //            resetError("Checked date cannot deceed shipment received date", true);
                //            return;
                //        }

                //        DateTime VerifiedDt = DateTime.ParseExact(txtVerifiedDate.Text.Trim(), "dd-MMM-yyyy", CultureInfo.InvariantCulture);  //Convert.ToDateTime(CommonLogic.GetConfigValue(cp.TenantID, "DateFormat") == "dd/MM/yyyy" ? DateTime.ParseExact(txtVerifiedDate.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy") : txtVerifiedDate.Text.Trim());

                //        if (VerifiedDt < shipRcvdDt)
                //        {
                //            resetError("Verified date cannot deceed shipment received date", true);
                //            return;
                //        }

                //    }



                //}


                int HasDiscrepancy = chkHasDiscrepancy.Checked == true ? 1 : 0;




              //  sqlUpdate.Append(",HasDiscrepancy=" + HasDiscrepancy);

              //  sqlUpdate.Append(",Disc_VerifiedDate=" + (VerifiedDate == null ? "null" : DB.SQuote(VerifiedDate)));


                sqlUpdate.Append("  where IB_RefWarehouse_DetailsID= " + IB_RefWH_DetailsID);


                //if (DB.GetSqlN("EXEC [dbo].[sp_INV_InwardQCCheck] @InboundID=" + IBTrack.InboundTrackingID + ",@SupplierInvoiceID=0") != 0)
                //{
                //    //resetError("For this invoice QC capture yet to complete ", true);
                //    resetError("QC yet to be captured ", true);
                //    return;
                //}



              

                String FileExtenion = Path.GetExtension(fileupShipverificationNote.FileName);

                if (FileExtenion != "")
                {
                    if (FileExtenion == ".pdf")
                    {


                        //if (CommonLogic.UploadFile(TenantRootDir + DB.GetSqlS("select UniqueID AS S from GEN_Tenant where TenantID=" + TenantID) + InboundPath + InbShpVerifNotePath, fileupShipverificationNote, IBTrack.StoreRefNumber + "_" + ddlStores.SelectedValue + Path.GetExtension(fileupShipverificationNote.FileName)) == false)

                        if (CommonLogic.UploadFile(TenantRootDir + IBTrack.TenantID + InboundPath + InbShpVerifNotePath, fileupShipverificationNote, IBTrack.StoreRefNumber + "_" + ddlStores.SelectedValue + Path.GetExtension(fileupShipverificationNote.FileName)) == false)
                        {
                            // Response.Redirect("InboundDetails.aspx?ibdid=" + IBTrack.InboundTrackingID.ToString() + "&ibupdate=okfileuploaderr");
                        }
                    }
                    else
                    {
                        resetError("Please attach a valid file", true);
                        return;
                    }
                }
                DB.ExecuteSQL(sqlUpdate.ToString());
                if (IBTrack.ShipmentTypeID==11)
                {
                    DB.ExecuteSQL("update MMT_KitJobOrderHeader set JobOrderStatusID=5	where inboundid=" + IBTrack.InboundTrackingID);
                }

                resetError("Shipment verification details successfully updated", false);
                //if (cbisdesc.Checked == true)
                //{
                   
                //    string Quy = "[dbo].[sp_INB_UpsertDiscrepancyDetails_New] @InboundID = " + IBTrack.InboundTrackingID + ", @Remarks= " + DB.SQuote(txtVerificationRemarks.Text) + ", @UserID=" + cp.UserID + "";
                //    try
                //    {
                //        DB.ExecuteSQL(Quy);
                //    }
                //    catch(Exception ex)
                //    {

                //    }
                //}

                ValidateCharge();

                pnlShipmentReceived.Visible = true;
                lnkAddAVNew.Visible = true;
                LoadShipmentVerificationData();
               // pnlshipmentVerification.Visible = true;
                upnlTPLReceivingCharge.Visible = true;
                LoadDiscrFormDetails();

                

                if (DB.GetSqlTinyInt("select HasDiscrepancy AS TI from INB_InboundTracking_Warehouse where IsActive=1 and IsDeleted=0  and InboundID=" + IBTrack.InboundTrackingID.ToString() + " and IB_RefWarehouse_DetailsID=" + IB_RefWH_DetailsID) != 0)
                {
                    pnlDiscInfo.Visible = true;
                  //  chkHasDiscrepancy.Checked = false;
                  //  chkHasDiscrepancy.Visible = false;

                }

                if (DB.GetSqlN("select count(ShipmentVerifiedOn) AS N from INB_InboundTracking_Warehouse where IsActive=1 and IsDeleted=0  and InboundID=" + IBTrack.InboundTrackingID.ToString()) == ArrStores.Length)
                {

                    // status changed 4 to 6 on 3-1-2018
                    DB.ExecuteSQL("Update INB_Inbound set InboundStatusID=26 where InboundID=" + IBTrack.InboundTrackingID.ToString());

                    pnlGRNDetails.Visible = true;

                }

                //  To Close Po  modified on 03-01-2017

                DB.ExecuteSQL("EXEC [dbo].[sp_INB_ClosePOStatus] @InboundID=" + IBTrack.InboundTrackingID + ",@POStatusID=4,@Status=1, @UpdatedBy=" + cp.UserID.ToString());

                ValidateInboundStatus();

            }
            catch (Exception ex)
            {
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
                resetError("Error while updating verification details", true);
            }



        }







        #endregion ----- Shipment Verification Details ------

        #region ----- GRN  GridView -----------------


        protected DataSet gvGRNDetails_buildGridData()
        {
            string sql = ViewState["GRNListSQL"].ToString();
            DataSet ds = DB.GetDS(sql, false);
            return ds;
        }

        //protected void gvGRNDetails_buildGridData(DataSet ds)
        //{
        //    gvGRNDetails.DataSource = ds.Tables[0];
        //    gvGRNDetails.DataBind();


        //    ds.Dispose();
        //    int InbstatusId = IBTrack.InBoundStatusID;
        //    if (InbstatusId == 6 || InbstatusId == 26)
        //    {
        //        gvGRNDetails.Columns[8].Visible = false;
        //        lnkAddNewGRN.Visible = false;
        //    }
        //    else
        //    {
        //        gvGRNDetails.Columns[8].Visible = true;
        //        lnkAddNewGRN.Visible = true;
        //    }
                
        //}

        //protected void lnkAddNewGRN_Click(object sender, EventArgs e)
        //{
            
        //    //if (!cp.IsInAnyRoles("21"))
        //    //{

        //    //    resetError("Only IB Store-Incharge can update this shipment", true);
        //    //    return;

        //    //}

        //    if (DB.GetSqlN("SELECT InboundStatusID N FROM INB_Inbound WHERE IsDeleted=0 AND IsActive=1 AND InboundID=" + CommonLogic.QueryString("ibdid")) == 6)
        //    {
        //        resetError("Cannot add the GRN details, as GRN is updated <br/>", true);
        //        pnlshipmentVerification.Visible = true;
        //        pnlDiscInfo.Visible = true;
        //        return;
        //    }

        //    ViewState["gvGRNIsInsert"] = false;
        //    /*
            
        //    if (DB.GetSqlN("select top 1 InboundID AS N from INB_InboundTracking_Warehouse where  HasDiscrepancy=1 AND IsActive=1 AND IsDeleted=0 AND InboundID=" + IBTrack.InboundTrackingID) != 0)
        //    {
        //        this.resetError("Discrepancy shipments not allowed to update GRN", true);
        //        return;
        //    }*/

        //    StringBuilder sql = new StringBuilder(2500);

        //    try
        //    {


        //        gvGRNDetails.EditIndex = 0;
        //        gvGRNDetails.PageIndex = 0;

        //        DataSet dsGRNRecords = this.gvGRNDetails_buildGridData();
        //        DataRow row = dsGRNRecords.Tables[0].NewRow();
        //        row["GRNUpdateID"] = 0;
                
        //        // Auto Generated GRN number
        //        row["GRNNumber"] = DateTime.Now.ToString("yyMMddhhmmss");
        //        // end
        //        dsGRNRecords.Tables[0].Rows.InsertAt(row, 0);

        //        this.gvGRNDetails_buildGridData(dsGRNRecords);

        //        // this.resetError("New GRN line item added", false);

        //        ViewState["gvGRNIsInsert"] = true;

        //    }
        //    catch (Exception ex)
        //    {
        //        CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
        //        this.resetError("Error while inserting new line item", true);

        //    }


        //}

        //protected void gvGRNDetails_PageIndexChanging(object sender, GridViewPageEventArgs e)
        //{
        //    ViewState["gvGRNIsInsert"] = false;

        //    gvGRNDetails.PageIndex = e.NewPageIndex;
        //    gvGRNDetails.EditIndex = -1;
        //    this.gvGRNDetails_buildGridData(this.gvGRNDetails_buildGridData());
        //}

        public static string DataObj="";
        //protected void gvGRNDetails_RowUpdating(object sender, GridViewUpdateEventArgs e)
        //{
          



        //    //if (!cp.IsInAnyRoles("21"))
        //    //{

        //    //    resetError("Only IB Store-Incharge can update this shipment", true);
        //    //    gvGRNDetails.EditIndex = -1;
        //    //    this.gvGRNDetails_buildGridData(this.gvGRNDetails_buildGridData());
        //    //    return;

        //    //}

        //    //ValidateInboundStatus();

        //    ViewState["gvGRNIsInsert"] = false;
        //    /*
        //    if (DB.GetSqlN("select top 1 InboundID AS N from INB_InboundTracking_Warehouse where  HasDiscrepancy=1 AND IsActive=1 AND IsDeleted=0 AND InboundID=" + IBTrack.InboundTrackingID) != 0)
        //    {
        //        this.resetError("Discrepancy shipments not allowed to update GRN", true);
        //        return;
        //    }*/

        //    GridViewRow row = gvGRNDetails.Rows[e.RowIndex];

        //    if (row != null)
        //    {

        //        string ltInvNumber = ((TextBox)row.FindControl("txtInvoiceNumber")).Text;
        //        string ltPONumber = ((TextBox)row.FindControl("txtPONumber")).Text;

        //        string ltGRNUpdateID = ((Literal)row.FindControl("ltHidInbound_GRNUpdateID")).Text;

        //        string ltGRNNumber = ((TextBox)row.FindControl("txtGRNNumber")).Text;
        //        string ltGRNDoneBy = ((TextBox)row.FindControl("txtGRNUpdatedBy")).Text;
        //        string ltGRNDate = ((TextBox)row.FindControl("txtGRNDate")).Text;

        //        int POHeaderID = DB.GetSqlN("select POHeaderID AS N from ORD_POHeader where IsActive=1 AND IsDeleted=0 AND AccountID="+cp.AccountID+" AND PONumber='" + ltPONumber + "'");

        //        if (POHeaderID == 0)
        //        {
        //            resetError("PO Number does not exist", true);
        //            gvGRNDetails.EditIndex = -1;
        //            this.gvGRNDetails_buildGridData(this.gvGRNDetails_buildGridData());
        //            return;
        //        }




        //        int InvHeaderID = DB.GetSqlN("select SupplierInvoiceID AS N from ORD_SupplierInvoice ORD_SUP  JOIN ORD_POHeader ORD_POH ON   ORD_POH.POHeaderID=ORD_SUP.POHeaderID AND ORD_POH.IsActive=1 AND ORD_POH.IsDeleted=0 AND ORD_POH.SupplierID=" + IBTrack.Supplier + " where ORD_SUP.IsActive=1 and ORD_SUP.IsDeleted=0 and ORD_SUP.InvoiceNumber=" + DB.SQuote(ltInvNumber) + " AND ORD_SUP.POHeaderID=" + POHeaderID);

        //        if (InvHeaderID == 0)// Check Inv Number
        //        {
        //            this.resetError("Invoice Number does not exist", true);
        //            gvGRNDetails.EditIndex = -1;
        //            this.gvGRNDetails_buildGridData(this.gvGRNDetails_buildGridData());
        //            return;
        //        }



        //        //if (DB.GetSqlN("select top 1 MMT_MM.MaterialMasterID AS N from INV_GoodsMovementDetails INV_GMD JOIN MMT_MaterialMaster MMT_MM ON MMT_MM.MaterialMasterID=INV_GMD.MaterialMasterID AND MMT_MM.IsActive=1 AND MMT_MM.IsDeleted=0 JOIN ORD_SupplierInvoiceDetails ORD_SD  ON ORD_SD.SupplierInvoiceDetailsID=INV_GMD.POSODetailsID AND  ORD_SD.IsActive=1 AND ORD_SD.IsDeleted=0  where INV_GMD.AsIs=0 AND ( INV_GMD.IsNonConfirmity=1 OR INV_GMD.IsDamaged=1 OR INV_GMD.HasDiscrepancy=1  ) AND INV_GMD.IsDeleted=0 AND INV_GMD.TransactionDocID=" + IBTrack.InboundTrackingID + " AND INV_GMD.GoodsMovementTypeID=1 AND ORD_SD.SupplierInvoiceID=" + InvHeaderID) != 0)
        //        //{
        //        //    this.resetError("GRN cannot be updated for discrepancy shipments", true);
        //        //    return;
        //        //}

                

        //        // int POHeaderID = DB.GetSqlN("select POHeaderID AS N from ORD_SupplierInvoice where IsActive=1 and IsDeleted=0 and SupplierInvoiceID=" + InvHeaderID );

        //        if (CommonLogic.GetUserID(ltGRNDoneBy) == 0)
        //        {
        //            this.resetError("User does not exist", true);
        //            gvGRNDetails.EditIndex = -1;
        //            this.gvGRNDetails_buildGridData(this.gvGRNDetails_buildGridData());
        //            return;
        //        }
        //        //if (DB.GetSqlN("EXEC [dbo].[sp_INV_InwardQCCheck] @InboundID=" + IBTrack.InboundTrackingID + ",@SupplierInvoiceID=0") != 0)
        //        //{
        //        //    //resetError("For this invoice QC capture yet to complete ", true);
        //        //    resetError("QC yet to be captured ", true);
        //        //    return;
        //        //}
        //        //  commented for temporary

        //        if (DB.GetSqlN("select GRNUpdateID AS N from INB_GRNUpdate where GRNUpdateID=" + ltGRNUpdateID + " AND IsActive=1 AND IsDeleted=0") != 0)
        //        {
        //            this.resetError("GRN is already updated", true);
        //            gvGRNDetails.EditIndex = -1;
        //            this.gvGRNDetails_buildGridData(this.gvGRNDetails_buildGridData());
        //            return;
        //        }

        //        StringBuilder xml = new StringBuilder();
        //        xml.Append("<root>");
        //        int noxmldata = 0;
        //        //string dt = hdnPOLines.Value.ToString();
        //        //polineContainer.Style.

        //        foreach (GridViewRow gr in GVPOLineItems.Rows)
        //        {

        //            Literal ltMspId = (Literal)gr.FindControl("ltMaterialMasterID");
        //            Literal ltLineNumber = (Literal)gr.FindControl("ltLineNumber");
        //            Literal ltQuantity = (Literal)gr.FindControl("ltQuantity");
        //            //CheckBox cbIsRequired = (CheckBox)gr.FindControl("chkIsDelete");
        //            HtmlInputCheckBox cbIsRequired = (HtmlInputCheckBox)gr.FindControl("chkIsDelete");
        //            if (cbIsRequired.Checked)
        //            {
        //                xml.Append("<data>");
        //                xml.Append("<MaterialMasterID>" + ltMspId.Text.ToString() + "</MaterialMasterID>");
        //                xml.Append("<LineNumber>" + ltLineNumber.Text.ToString() + "</LineNumber>");
        //                xml.Append("<Quantity>" + ltQuantity.Text.ToString() + "</Quantity>");
        //                xml.Append("<CreatedBy>" + cp.UserID.ToString() + "</CreatedBy>");
        //                xml.Append("</data>");
        //                noxmldata = 1;
        //            }
        //        }

        //        if (noxmldata == 0)
        //        {
        //            resetError("Select atleast one LineItem", true);
        //            xml.Append("<data>");
        //            xml.Append("</data>");
        //            gvGRNDetails.EditIndex = -1;
        //            this.gvGRNDetails_buildGridData(this.gvGRNDetails_buildGridData());
        //            return;
        //        }

        //        xml.Append("</root>");
        //        StringBuilder sqlDescpRecords = new StringBuilder(2500);
        //        string updatedon = DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss");
        //        sqlDescpRecords.Append(" DECLARE @NewUpdateInboundDiscID int;  EXEC [sp_INB_UpsertGRNUpdate]    @GRNUpdateID=" + ltGRNUpdateID);
        //        sqlDescpRecords.Append(",@InboundID=" + IBTrack.InboundTrackingID);
        //        sqlDescpRecords.Append(",@POHeaderID=" + POHeaderID);
        //        sqlDescpRecords.Append(",@SupplierInvoiceID=" + InvHeaderID);
        //        sqlDescpRecords.Append(",@GRNNumber=" + DB.SQuote(ltGRNNumber));
        //        sqlDescpRecords.Append(",@GRNUpdatedBy=" + CommonLogic.GetUserID(ltGRNDoneBy));
        //        sqlDescpRecords.Append(",@UpdatedOn=" + DB.SQuote(updatedon));                
        //        sqlDescpRecords.Append(",@GRNDate=" + (ltGRNDate != "" ? DB.SQuote(DateTime.ParseExact(ltGRNDate.Trim(), "dd-MMM-yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy")) : "NULL"));
        //        sqlDescpRecords.Append(",@CreatedBy=" + CreatedBy); 
        //        // for mm and line number
        //        //sqlDescpRecords.Append(",@MaterialMasterID=" + hifMaterialId.Value.ToString());
        //        //sqlDescpRecords.Append(",@LineNumber=" + hifgrnpolinenumber.Value.ToString());
        //        sqlDescpRecords.Append(",@InputDataXML =" +DB.SQuote(xml.ToString()));
        //        // end
        //        sqlDescpRecords.Append(",@Result=@NewUpdateInboundDiscID OUTPUT;  select @NewUpdateInboundDiscID AS N");
        //          try
        //        {

        //            //if (DB.GetSqlN("EXEC [dbo].[sp_INV_InwardQCCheck] @InboundID=" + IBTrack.InboundTrackingID + ",@SupplierInvoiceID=" + InvHeaderID) != 0)
        //            //{
        //            //    resetError("QC yet to be captured ", true);
        //            //    gvGRNDetails.EditIndex = -1;
        //            //    this.gvGRNDetails_buildGridData(this.gvGRNDetails_buildGridData());
        //            //    return;
        //            //}



        //            if (DB.GetSqlN(sqlDescpRecords.ToString()) == 0)
        //            {
        //                resetError("Error while updating grn details", true);
        //                gvGRNDetails.EditIndex = -1;
        //                this.gvGRNDetails_buildGridData(this.gvGRNDetails_buildGridData());
        //                return;
        //            }

        //            //if (DB.GetSqlN("select Count(*) AS [N] from INB_Inbound_ORD_SupplierInvoice where InboundID=" + IBTrack.InboundTrackingID + " AND IsActive=1 AND IsDeleted=0") == DB.GetSqlN("select Count(*) AS [N] from INB_GRNUpdate where InboundID=" + IBTrack.InboundTrackingID + " AND  IsActive=1 AND IsDeleted=0"))
        //            //{
        //            //    DB.ExecuteSQL("update INB_Inbound set InboundStatusID=6 where IsActive=1 AND IsDeleted=0 AND InboundID=" + IBTrack.InboundTrackingID);
        //            //}

        //            // DB.ExecuteSQL("Update INV_GoodsMovementDetails set IsActive=1 where IsDeleted=0 AND GoodsMovementTypeID=1 AND TransactionDocID=" + IBTrack.InboundTrackingID + " AND POSODetailsID IN ( select ORD_SUPID.SupplierInvoiceDetailsID from INB_GRNUpdate INB_GRN  JOIN ORD_SupplierInvoiceDetails ORD_SUPID ON ORD_SUPID.SupplierInvoiceID=INB_GRN.SupplierInvoiceID AND ORD_SUPID.IsActive=1 AND ORD_SUPID.IsDeleted=0 where INB_GRN.InboundID=" + IBTrack.InboundTrackingID + " AND INB_GRN.IsActive=1 and INB_GRN.IsDeleted=0 AND INB_GRN.SupplierInvoiceID=" + InvHeaderID + " AND INB_GRN.POHeaderID=" + POHeaderID + ")");

        //            // DB.ExecuteSQL("Update  INV_GoodsMovementDetails set IsActive=1 where IsDeleted=0 AND GoodsMovementTypeID=2 AND GoodsMovementDetailsID IN ( select SOGoodsMovementDetailsID from INV_PO_GoodsOutLink where  IsDeleted=0 AND SODetailsID IS NULL AND POGoodsMovementDetailsID IN ( select GoodsMovementDetailsID from INV_GoodsMovementDetails where GoodsMovementTypeID=1 AND IsDeleted=0 AND TransactionDocID="+IBTrack.InboundTrackingID+"  AND  POSODetailsID IN ( select ORD_SUPID.SupplierInvoiceDetailsID from INB_GRNUpdate INB_GRN JOIN ORD_SupplierInvoiceDetails ORD_SUPID ON ORD_SUPID.SupplierInvoiceID=INB_GRN.SupplierInvoiceID AND ORD_SUPID.IsActive=1 AND ORD_SUPID.IsDeleted=0 where INB_GRN.InboundID="+IBTrack.InboundTrackingID+" AND INB_GRN.IsActive=1 and INB_GRN.IsDeleted=0 AND INB_GRN.SupplierInvoiceID="+InvHeaderID+" AND INB_GRN.POHeaderID="+POHeaderID+" ) ))");

        //            // DB.ExecuteSQL("update INV_PO_GoodsOutLink set IsActive=1 where  IsDeleted=0 AND SODetailsID IS NULL AND  POGoodsMovementDetailsID IN ( select GoodsMovementDetailsID from INV_GoodsMovementDetails where GoodsMovementTypeID=1 AND IsDeleted=0 AND TransactionDocID=" + IBTrack.InboundTrackingID + "  AND  POSODetailsID IN ( select ORD_SUPID.SupplierInvoiceDetailsID from INB_GRNUpdate INB_GRN  JOIN ORD_SupplierInvoiceDetails ORD_SUPID ON ORD_SUPID.SupplierInvoiceID=INB_GRN.SupplierInvoiceID AND ORD_SUPID.IsActive=1 AND ORD_SUPID.IsDeleted=0 where INB_GRN.InboundID=" + IBTrack.InboundTrackingID + " AND INB_GRN.IsActive=1 and INB_GRN.IsDeleted=0 AND INB_GRN.SupplierInvoiceID=" + InvHeaderID + " AND INB_GRN.POHeaderID=" + POHeaderID + " )   )");

        //            // DB.ExecuteSQL("update INV_GoodsMovementDetails  set  IsNonConfirmity=case when IsNonConfirmity is null then 0 else IsNonConfirmity end,AsIs=case when AsIs is null then 0 else AsIs end,IsPositiveRecall=case when IsPositiveRecall is null then 0 else IsPositiveRecall end    WHERE GoodsMovementTypeID=1 AND  IsDeleted=0 AND IsDamaged=0 AND HasDiscrepancy=0 and TransactionDocID=" + IBTrack.InboundTrackingID + " AND MaterialMasterID NOT IN ( select InGMD.MaterialMasterID from MMT_MaterialMaster_QualityParameters MMT_QC JOIN INV_GoodsMovementDetails InGMD ON InGMD.MaterialMasterID =MMT_QC.MaterialMasterID AND TransactionDocID=" + IBTrack.InboundTrackingID + " AND InGMD.GoodsMovementTypeID=1 where MMT_QC.IsActive=1 AND MMT_QC.IsDeleted=0 group by InGMD.MaterialMasterID)");



        //            //  commented for temporary on 2-1-2018
        //            //DB.ExecuteSQL("EXEC [dbo].[sp_INB_ClosePOStatus] @InboundID=" + IBTrack.InboundTrackingID + ",@POStatusID=4,@Status=1");

        //            this.resetError("GRN details successfully updated", false);

        //            Session["GrnMessage"] = 1;

        //            Session["CHECKED_ITEMS"] = null;
        //            //if (DB.GetSqlN("Declare  @GoodsINStatus int; EXEC [sp_INV_CheckGoodsINStatus] @InboundID=" + IBTrack.InboundTrackingID + ",@Status=@GoodsINStatus out; select @GoodsINStatus as N;") == 1)
        //            //{
        //            //    pnlshipmentVerification.Visible = true;
        //            //}
        //            upnlShipmentVerification.UpdateMode = UpdatePanelUpdateMode.Conditional;
        //            upnlShipmentVerification.Update();

        //            //if (IBTrack.InBoundStatusID == 6 || IBTrack.InBoundStatusID == 26)
        //            //{
        //            //    pnlGRNDetails.Visible = true;
        //            //    pnlshipmentVerification.Visible = true;
        //            //}

        //            //gvGRNDetails_buildGridData(gvGRNDetails_buildGridData());

        //            ValidateInboundStatus();

        //            //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "test11", "GRNUpdateMessage(1);", true);

        //            gvGRNDetails.EditIndex = -1;
        //            this.gvGRNDetails_buildGridData(this.gvGRNDetails_buildGridData());

        //            //ScriptManager.RegisterStartupScript(this, this.GetType(), "test11", "GRNUpdateMessage(" + GVPOLineItems.Rows.Count + ");", true);

        //            //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "test11", "GRNUpdateMessage(" + GVPOLineItems.Rows.Count + ");", true);

        //            //added on 03-01-2018
        //            // Response.Redirect(HttpContext.Current.Request.RawUrl.ToString());

        //            upnlGRNDetails.UpdateMode = UpdatePanelUpdateMode.Conditional;
        //            upnlGRNDetails.Update();
        //            Response.Redirect("InboundDetails.aspx?ibdid=" + IBTrack.InboundTrackingID.ToString()); // add to this stmt #invoice_det

        //        }
        //        catch (SqlException sqlex)
        //        {
        //            CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), sqlex.Source, sqlex.Message, sqlex.StackTrace);

        //            if (sqlex.ErrorCode == -2146232060)
        //            {
        //                this.resetError("This line item is already updated", true);
        //                return;
        //            }

        //        }
        //        catch (Exception ex)
        //        {
        //            CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
        //            this.resetError("Error while updating GRN details", true);
        //        }
        //    }



        //}

        //protected void gvGRNDetails_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        //{
        //    /*
        //    if (Localization.ParseBoolean(ViewState["gvGRNIsInsert"].ToString()))
        //    {
        //        GridViewRow row = gvGRNDetails.Rows[e.RowIndex];
        //        if (row != null)
        //        {
        //            int iden = Convert.ToInt32(((Literal)row.FindControl("ltHidInbound_GRNUpdateID")).Text);
        //            deleteGRNRow(iden);
        //            resetError("Line Item Canceled", true);

        //        }
        //    }

        //    ViewState["gvGRNIsInsert"] = false;
        //    */
        //    gvGRNDetails.EditIndex = -1;
        //    this.gvGRNDetails_buildGridData(this.gvGRNDetails_buildGridData());
        //}

        //protected void gvGRNDetails_RowEditing(object sender, GridViewEditEventArgs e)
        //{
            

        //    ViewState["gvGRNIsInsert"] = false;
        //    gvGRNDetails.EditIndex = e.NewEditIndex;
        //    this.gvGRNDetails_buildGridData(this.gvGRNDetails_buildGridData());
            

        //    //GridViewRow gvRow = (GridViewRow)gvGRNDetails.Rows[e.NewEditIndex];
        //    //TextBox txtPOHeaderID = (TextBox)gvRow.FindControl("txtPOHeaderID");

        //    //if (txtPOHeaderID.Text.ToString() != "")
        //    //{
        //    //    resetError("GRN Updated", true);
        //    //    Control control1 = gvGRNDetails.Rows[0].Cells[8].Controls[0];

        //    //    LinkButton lit = (LinkButton)control1;



        //    //    lit.Enabled = false;

        //    //    //Button brn = (Button)control1;
        //    //    //brn.Enabled = false;
        //    //    return;
        //    //}
        //    //else
        //    //{
        //    //  gvGRNDetails.EditIndex = e.NewEditIndex;
        //    //    //gvGRNDetails.EditIndex = -1;
        //    //    this.gvGRNDetails_buildGridData(this.gvGRNDetails_buildGridData());
        //    //}

        //}

        //protected void lnkDeleteGRNItem_Click(object sender, EventArgs e)
        //{
            
        //    //if (!cp.IsInAnyRoles("21"))
        //    //{

        //    //    resetError("Only IB Store-Incharge can update this shipment", true);
        //    //    return;

        //    //}

        //    string rfidIDs = "";

        //    bool chkBox = false;

        //    StringBuilder sqlDeleteString = new StringBuilder();
        //    sqlDeleteString.Append("BEGIN TRAN ");

        //    //'Navigate through each row in the GridView for checkbox items
        //    foreach (GridViewRow gv in gvGRNDetails.Rows)
        //    {

        //        CheckBox isDelete = (CheckBox)gv.FindControl("chkIsDeleteGRNItems");

        //        if (isDelete == null)
        //            return;

        //        if (isDelete.Checked)
        //        {
        //            chkBox = true;
        //            // Concatenate GridView items with comma for SQL Delete
        //            rfidIDs = ((Literal)gv.FindControl("ltHidInbound_GRNUpdateID")).Text.ToString();



        //            if (rfidIDs != "")
        //                sqlDeleteString.Append(" DELETE from INB_GRNUpdate where GRNUpdateID=" + rfidIDs + ";");

        //        }
        //    }


        //    sqlDeleteString.Append(" COMMIT ");


        //    // Execute SQL Query only if checkboxes are checked to avoid any error with initial null string
        //    try
        //    {

        //        if (chkBox)
        //        {
        //            DB.ExecuteSQL(sqlDeleteString.ToString());
        //        }
        //        resetError("Successfully deleted the selected line items", false);

        //    }
        //    catch (Exception ex)
        //    {
        //        CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
        //        resetError("Error while deleting selected line items", true);
        //    }

        //    this.gvGRNDetails_buildGridData(this.gvGRNDetails_buildGridData());


        //}

        //protected void deleteGRNRow(int iden)
        //{

           
        //    //if (!cp.IsInAnyRoles("21"))
        //    //{

        //    //    resetError("Only IB Store-Incharge can update this shipment", true);
        //    //    return;

        //    //}


        //    StringBuilder sql = new StringBuilder(2500);
        //    sql.Append("DELETE from INB_GRNUpdate where GRNUpdateID=" + iden.ToString());
        //    try
        //    {
        //        DB.ExecuteSQL(sql.ToString());
        //        this.gvGRNDetails_buildGridData(this.gvGRNDetails_buildGridData());

        //    }
        //    catch (Exception ex)
        //    {
        //        CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
        //        this.resetError("Error while deleting selected line items", true);
        //    }

        //}

        //protected void gvGRNDetails_RowDataBound(object sender, GridViewRowEventArgs e)
        //{

        //    if ((e.Row.RowState & DataControlRowState.Edit) == DataControlRowState.Edit)
        //    {
        //        TextBox txtInvNumber = (TextBox)e.Row.FindControl("txtPONumber");
        //        txtInvNumber.Focus();

                


        //    }
           

        //}



        #endregion ----- GRN  GridView -----------------

        #region ---- Pending Goods-IN List --------



        protected DataSet gvPendingGoodsINList_buildGridData()
        {
            string sql = ViewState["PendingGoodsINListSQL"].ToString();
            DataSet ds = DB.GetDS(sql, false);

            return ds;
        }

        protected void gvPendingGoodsINList_buildGridData(DataSet ds)
        {
            gvPendingGoodsINList.DataSource = ds.Tables[0];
            gvPendingGoodsINList.DataBind();
            ds.Dispose();

            ScriptManager.RegisterStartupScript(this, this.GetType(), "PendingGoodsINList", "unblockDialog();", true);
        }

        protected void gvPendingGoodsINList_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }

        protected void gvPendingGoodsINList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {


            gvPendingGoodsINList.PageIndex = e.NewPageIndex;
            gvPendingGoodsINList.EditIndex = -1;
            this.gvPendingGoodsINList_buildGridData(this.gvPendingGoodsINList_buildGridData());
        }


        #endregion ---- Pending Goods-IN List --------



        protected void lnkViewPendingGoodsINList_Click(object sender, EventArgs e)
        {
            gvPendingGoodsINList_buildGridData(gvPendingGoodsINList_buildGridData());
        }

        #region ---- 3PL Billing List--------

        protected DataSet TPLInboundCharges_buildGridData()
        {
            string sql = ViewState["TPLInboundChargeSQL"].ToString();
            DataSet ds = DB.GetDS(sql, false);
            return ds;
        }
        protected void TPLInboundCharges_buildGridData(DataSet ds)
        {
            gvTPLInboundCharges.Columns[6].FooterText = "Sum:" + (from row in ds.Tables[0].AsEnumerable().Where(row => row.Field<object>("TotalCostAfterDiscount") != null)
                                                                  select row.Field<decimal>("TotalCostAfterDiscount")).Sum().ToString("0.00");
            gvTPLInboundCharges.DataSource = ds;
            gvTPLInboundCharges.DataBind();
            ds.Dispose();
        }


        protected void gvTPLInboundCharges_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow && e.Row.RowIndex == gvTPLInboundCharges.EditIndex)
            {

            }
        }

        protected void gvTPLInboundCharges_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvTPLInboundCharges.EditIndex = e.NewEditIndex;
            this.TPLInboundCharges_buildGridData(this.TPLInboundCharges_buildGridData());
        }

        protected void gvTPLInboundCharges_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {

          
            Page.Validate("vgRequired3pl");
            if (!Page.IsValid)
            {
                return;
            }

            GridViewRow row = gvTPLInboundCharges.Rows[e.RowIndex];

            if (row != null)
            {
                string vTenantTransactionAccessorialCaptureID = ((Literal)row.FindControl("ltTenantTransactionAccessorialCaptureID")).Text;
                string vActivityRateID = ((HiddenField)row.FindControl("hifActivityRateID")).Value;
                string Quantity = ((TextBox)row.FindControl("txtQuantity")).Text;
                decimal qty = Convert.ToDecimal(Quantity);
                if(qty == 0)
                {
                    resetError("Please enter valid Quantity", true);
                    return;
                }

                if (vActivityRateID == "")
                {
                    resetError("Not a valid Activity Tariff", true);
                    return;
                }

                //if (DB.GetSqlN("select TenantTransactionAccessorialCaptureID N from TPL_Tenant_Transaction_Accessorial_Capture where IsDeleted=0 AND TransactionID=" + CommonLogic.QueryString("ibdid") + " AND ActivityRateID=" + vActivityRateID + " AND TransactionTypeID=1 and TenantTransactionAccessorialCaptureID!=" + vTenantTransactionAccessorialCaptureID) != 0)
                //{
                //    resetError("Activity Rate already added", true);
                //    return;
                //}

                StringBuilder QueryForUpsertTPLInbound = new StringBuilder();

                QueryForUpsertTPLInbound.Append("DECLARE @UpdateResult int ; ");
                QueryForUpsertTPLInbound.Append(" EXEC [dbo].[sp_TPL_UpsertAccessoryTransactionCapture] ");
                QueryForUpsertTPLInbound.Append(" @TransactionTypeID=1,@TransactionID =" + CommonLogic.QueryString("ibdid"));
                QueryForUpsertTPLInbound.Append(" ,@ActivityRateID=" + vActivityRateID);
                QueryForUpsertTPLInbound.Append(" ,@Quantity=" + Quantity);
                QueryForUpsertTPLInbound.Append(" ,@CreatedBy=" + cp.UserID);
                QueryForUpsertTPLInbound.Append(" ,@TenantTransactionAccessorialCaptureID=" + CommonLogic.IIF(vTenantTransactionAccessorialCaptureID == "", "0", vTenantTransactionAccessorialCaptureID));
                QueryForUpsertTPLInbound.Append(" ,@Result=@UpdateResult OUTPUT     ;");
                QueryForUpsertTPLInbound.Append("  select @UpdateResult AS N");

                try
                {
                    int Result = DB.GetSqlN(QueryForUpsertTPLInbound.ToString());

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

                gvTPLInboundCharges.EditIndex = -1;
                this.TPLInboundCharges_buildGridData(this.TPLInboundCharges_buildGridData());
            }
        }

        protected void gvTPLInboundCharges_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvTPLInboundCharges.EditIndex = -1;
            this.TPLInboundCharges_buildGridData(this.TPLInboundCharges_buildGridData());
        }

        protected void gvTPLInboundCharges_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvTPLInboundCharges.PageIndex = e.NewPageIndex;
            this.TPLInboundCharges_buildGridData(this.TPLInboundCharges_buildGridData());
        }

        protected void lnkAddNewTPLActivity_Click(object sender, EventArgs e)
        {
            try
            {
                gvTPLInboundCharges.EditIndex = 0;
                gvTPLInboundCharges.PageIndex = 0;

                DataSet dsInboundCharges = TPLInboundCharges_buildGridData();
                DataRow row = dsInboundCharges.Tables[0].NewRow();
                row["TenantTransactionAccessorialCaptureID"] = 0;
                dsInboundCharges.Tables[0].Rows.InsertAt(row, 0);
                this.TPLInboundCharges_buildGridData(dsInboundCharges);
            }
            catch (Exception ex)
            {
                this.resetError("Error while inserting", true);
            }
        }

        protected void btnDeleteTPLInboundCharge_Click(object sender, EventArgs e)
        {
            string gvIDs = "";
            bool chkBox = false;
            foreach (GridViewRow gv in gvTPLInboundCharges.Rows)
            {
                CheckBox deleteChkBxItem = (CheckBox)gv.FindControl("deleteTPLInboundCharge");
                if (deleteChkBxItem.Checked)
                {
                    chkBox = true;
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
                    this.TPLInboundCharges_buildGridData(this.TPLInboundCharges_buildGridData());
                    ValidateCharge();
                }
                catch (Exception ex)
                {
                    resetError("Error deleting the selected records", true);
                }
            }
        }

        #endregion ---- 3PL Billing List--------


        #region ---- 3PL Shipment Receiving Charges List--------

        protected DataSet TPLInboundReceivingCharges_buildGridData()
        {
            string sql = ViewState["TPLInboundRecvChargeSQL"].ToString();
            DataSet ds = DB.GetDS(sql, false);
            return ds;
        }

        protected void TPLInboundReceivingCharges_buildGridData(DataSet ds)
        {
            gvTPLReceivingCharges.Columns[5].FooterText = "Sum:" + (from row in ds.Tables[0].AsEnumerable().Where(row => row.Field<object>("TotalCostAfterDiscount") != null)
                                                                  select row.Field<decimal>("TotalCostAfterDiscount")).Sum().ToString("0.00");
            gvTPLReceivingCharges.DataSource = ds;
            gvTPLReceivingCharges.DataBind();
            ds.Dispose();
        }

        protected void gvTPLReceivingCharges_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow && e.Row.RowIndex == gvTPLReceivingCharges.EditIndex)
            {

            }
        }

        protected void gvTPLReceivingCharges_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvTPLReceivingCharges.EditIndex = e.NewEditIndex;
            this.TPLInboundReceivingCharges_buildGridData(this.TPLInboundReceivingCharges_buildGridData());
        }

        protected void gvTPLReceivingCharges_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            cp = HttpContext.Current.User as CustomPrincipal;
            Page.Validate("vgRequiredReceiving");
            if (!Page.IsValid)
            {
                return;
            }

            GridViewRow row = gvTPLReceivingCharges.Rows[e.RowIndex];

            if (row != null)
            {
                string vRcvTenantTransactionAccessorialCaptureID = ((Literal)row.FindControl("ltRcvTenantTransactionAccessorialCaptureID")).Text;
                string vRcvActivityRateID = ((HiddenField)row.FindControl("hifRcvActivityRateID")).Value;
                string Quantity = ((TextBox)row.FindControl("txtRcvQuantity")).Text;
                decimal qty = Convert.ToDecimal(Quantity);


                if (vRcvActivityRateID == "")
                {
                    resetError("Not a valid Activity", true);
                    return;
                }
                if(qty == 0)
                {
                    resetError("Please enter Valid Quantity", true);
                    return;
                }

                //if (DB.GetSqlN("select TenantTransactionAccessorialCaptureID N from TPL_Tenant_Transaction_Accessorial_Capture where IsDeleted=0 AND TransactionID=" + CommonLogic.QueryString("ibdid") + " AND ActivityRateID=" + vRcvActivityRateID + " AND TransactionTypeID=1 and TenantTransactionAccessorialCaptureID!=" + vRcvTenantTransactionAccessorialCaptureID) != 0)
                //{
                //    resetError("Activity Rate already added", true);
                //    return;
                //}

                StringBuilder QueryForUpsertTPLInbound = new StringBuilder();

                QueryForUpsertTPLInbound.Append("DECLARE @UpdateResult int ; ");
                QueryForUpsertTPLInbound.Append(" EXEC [dbo].[sp_TPL_UpsertAccessoryTransactionCapture] ");
                QueryForUpsertTPLInbound.Append(" @TransactionTypeID=1,@TransactionID =" + CommonLogic.QueryString("ibdid"));
                QueryForUpsertTPLInbound.Append(" ,@ActivityRateID=" + vRcvActivityRateID);
                QueryForUpsertTPLInbound.Append(" ,@Quantity=" + Quantity);
                QueryForUpsertTPLInbound.Append(" ,@CreatedBy=" + cp.UserID);
                QueryForUpsertTPLInbound.Append(" ,@TenantTransactionAccessorialCaptureID=" + CommonLogic.IIF(vRcvTenantTransactionAccessorialCaptureID == "", "0", vRcvTenantTransactionAccessorialCaptureID));
                QueryForUpsertTPLInbound.Append(" ,@Result=@UpdateResult OUTPUT     ;");
                QueryForUpsertTPLInbound.Append("  select @UpdateResult AS N");

                try
                {
                    int Result = DB.GetSqlN(QueryForUpsertTPLInbound.ToString());

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

                gvTPLReceivingCharges.EditIndex = -1;
                this.TPLInboundReceivingCharges_buildGridData(this.TPLInboundReceivingCharges_buildGridData());
            }
        }

        protected void gvTPLReceivingCharges_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvTPLReceivingCharges.EditIndex = -1;
            this.TPLInboundReceivingCharges_buildGridData(this.TPLInboundReceivingCharges_buildGridData());
        }

        protected void gvTPLReceivingCharges_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvTPLReceivingCharges.PageIndex = e.NewPageIndex;
            this.TPLInboundReceivingCharges_buildGridData(this.TPLInboundReceivingCharges_buildGridData());
        }

        protected void lnkADDNewReceivingActivity_Click(object sender, EventArgs e)
        {
            try
            {
                gvTPLReceivingCharges.EditIndex = 0;
                gvTPLReceivingCharges.PageIndex = 0;

                DataSet dsReceivingCharges = TPLInboundReceivingCharges_buildGridData();
                DataRow row = dsReceivingCharges.Tables[0].NewRow();
                row["TenantTransactionAccessorialCaptureID"] = 0;
                dsReceivingCharges.Tables[0].Rows.InsertAt(row, 0);
                this.TPLInboundReceivingCharges_buildGridData(dsReceivingCharges);
            }
            catch (Exception ex)
            {
                this.resetError("Error while inserting", true);
            }
        }

        protected void btnDeleteTPLRecvCharge_Click(object sender, EventArgs e)
        {
            string gvIDs = "";
            bool chkBox = false;
            foreach (GridViewRow gv in gvTPLReceivingCharges.Rows)
            {
                CheckBox deleteChkBxItem = (CheckBox)gv.FindControl("deleteTPLReceivingCharge");
                if (deleteChkBxItem.Checked)
                {
                    chkBox = true;
                    gvIDs += ((Label)gv.FindControl("RecRecvID")).Text.ToString() + ",";
                }
            }


            if (chkBox)
            {
                try
                {
                    string deleteSQL = "update  [TPL_Tenant_Transaction_Accessorial_Capture] set IsDeleted=1  where TenantTransactionAccessorialCaptureID in (" + gvIDs.Substring(0, gvIDs.LastIndexOf(",")) + ")";
                    DB.ExecuteSQL(deleteSQL);
                    resetError("Successfully deleted the selected records", false);
                    this.TPLInboundReceivingCharges_buildGridData(this.TPLInboundReceivingCharges_buildGridData());

                    ValidateCharge();
                }
                catch (Exception ex)
                {
                    resetError("Error deleting the selected records", true);
                }
            }
        }


        #endregion ---- 3PL Shipment Receiving Charges List--------

        protected void Unnamed_Click(object sender, EventArgs e)
        {

        }


        //  ====================== Commented By M.D.Prasad ================================//
        // protected void btnlineitems_Click(object sender, EventArgs e)
        // {
        //     try
        //     {
        //         GVPOLineItems.DataSource = null;

        //         GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
        //         int index = gvRow.RowIndex;
        //         TextBox txtPONumber = (TextBox)gvRow.FindControl("txtPOHeaderID");
        //         HiddenField txtInvoiceNumber = (HiddenField)gvRow.FindControl("hifGRNSupplierInvoiceID");
        //         Literal ltGrnUpdateId = (Literal)gvRow.FindControl("ltHidInbound_GRNUpdateID");

        //         string InboundId = CommonLogic.QueryString("ibdid");
        //         string PoHeaderId = hifGRNPOHeaderID.Value.ToString() !="" ? hifGRNPOHeaderID.Value.ToString() : txtPONumber.Text.ToString();
        //         string InvoiceHeaderId = hifInvoiceId.Value.ToString() != "" ? hifInvoiceId.Value.ToString() : txtInvoiceNumber.Value.ToString();

        //         string Grnflag = txtPONumber.Text.ToString()==""?"0":"1";

        //         if (PoHeaderId != "" && InvoiceHeaderId != "")
        //         {
        //             upnlSupplierItemListDialog.UpdateMode = UpdatePanelUpdateMode.Conditional;
        //             upnlSupplierItemListDialog.Update();

        //             DataSet ds = DB.GetDS("EXEC dbo.[sp_INB_GET_GRNUpdateLineItems] @InboundID= " + InboundId + " ,@POHeaderID=" + PoHeaderId + ",@SupplierInvoiceID=" + InvoiceHeaderId + ", @Grnflag="+Grnflag + ",@GRNUpdateID=" + ltGrnUpdateId.Text.ToString(), false);
        //             GVPOLineItems.DataSource = ds.Tables[0];
        //             GVPOLineItems.DataBind();
        //         }

        //         if (Grnflag=="1")
        //         {
        //             ScriptManager.RegisterStartupScript(this, this.GetType(), "test", "PartialGRN(" + GVPOLineItems.Rows.Count + ");", true);
        //         }

        //     }
        //     catch (Exception ex)
        //     {

        //     }
        //}
        //  ====================== Commented By M.D.Prasad ================================//



        //  ====================== Commented By M.D.Prasad ================================//
        //protected void gvGRNDetails_RowCommand(object sender, GridViewCommandEventArgs e)
        //{
        //    if (e.CommandName.ToString() == "popup")
        //    {
        //        //GridViewRow gr = gvGRNDetails.Rows[] 

        //        int rowIndex = Convert.ToInt32(e.CommandArgument);

        //        //Reference the GridView Row.
        //        GridViewRow gvRow = gvGRNDetails.Rows[rowIndex];

        //        Literal ltMspId = (Literal)gvRow.FindControl("Literal1");
        //        TextBox txtPONumber = (TextBox)gvRow.FindControl("txtPOHeaderID");

        //        //Fetch value of Name.
        //        //string name = (gvRow.FindControl("txtName") as TextBox).Text;

        //    }

        //    //Button btnEdit = (Button)sender;
        //    //GridViewRow Grow = (GridViewRow)btnEdit.NamingContainer;
        //    //TextBox txtledName = (TextBox)Grow.FindControl("txtPOHeaderID");
        //}

        //  ====================== Commented By M.D.Prasad ================================//


        //  ====================== Added By M.D.Prasad ================================//
        //protected void gvGRNDetails_RowCommand(object sender, GridViewCommandEventArgs e)
        //{

        //    if (e.CommandName.ToString() == "popup")
        //    {
        //        Session["CHECKED_ITEMS"] = null;
        //        //GridViewRow gr = gvGRNDetails.Rows[] 

        //        try
        //        {
        //            int rowIndex = Convert.ToInt32(e.CommandArgument);

        //            //Reference the GridView Row.
        //            GridViewRow gvRow = gvGRNDetails.Rows[rowIndex];

        //            Literal ltMspId = (Literal)gvRow.FindControl("Literal1");


        //            TextBox txtPONumber = (TextBox)gvRow.FindControl("txtPOHeaderID");
        //            HiddenField txtInvoiceNumber = (HiddenField)gvRow.FindControl("hifGRNSupplierInvoiceID");
        //            Literal ltGrnUpdateId = (Literal)gvRow.FindControl("ltHidInbound_GRNUpdateID");

        //            string InboundId = CommonLogic.QueryString("ibdid");
        //            string PoHeaderId = hifGRNPOHeaderID.Value.ToString() != "" ? hifGRNPOHeaderID.Value.ToString() : txtPONumber.Text.ToString();
        //            string InvoiceHeaderId = hifInvoiceId.Value.ToString() != "" ? hifInvoiceId.Value.ToString() : txtInvoiceNumber.Value.ToString();

        //            string Grnflag = txtPONumber.Text.ToString() == "" ? "0" : "1";

        //            ViewState["POLineItems"] = "EXEC dbo.[sp_INB_GET_GRNUpdateLineItems] @InboundID= " + InboundId + " ,@POHeaderID=" + PoHeaderId + ",@SupplierInvoiceID=" + InvoiceHeaderId + ", @Grnflag="+Grnflag + ",@GRNUpdateID=" + ltGrnUpdateId.Text.ToString();

        //            if (PoHeaderId != "" && InvoiceHeaderId != "")
        //            {
        //                upnlSupplierItemListDialog.UpdateMode = UpdatePanelUpdateMode.Conditional;
        //                upnlSupplierItemListDialog.Update();
        //                this.Build_POLineItems(this.Build_POLineItems());

        //                //DataSet ds = DB.GetDS(ViewState["POLineItems"].ToString(), false);
        //                //if (ds != null)
        //                //{
        //                //    string divContent = "";
        //                //    divContent += "<table class='dataTables-example' id='tbl'>";
        //                //    divContent += "<thead>";
        //                //    divContent += "<tr>";
        //                //    divContent += "<th>Part No.</th><th>Line Number</th><th>Quantity</th><th>Action</th>";
        //                //    divContent += "</tr>";
        //                //    divContent += "</thead>";
        //                //    divContent += "<tbody>";
        //                //    foreach (DataRow row in ds.Tables[0].Rows)
        //                //    {
        //                //        divContent += "<tr>";
        //                //        divContent += "<td>" + row["MCode"].ToString() + "<input type='hidden' id='hgnMaterailID' value='" + row["MaterialMasterID"].ToString() + "'/></td>";
        //                //        divContent += "<td>" + row["LineNumber"].ToString() + "</td>";
        //                //        divContent += "<td>" + row["Quantity"].ToString() + "</td>";
        //                //        divContent += "<td><input type='checkbox' class='chkLineItems' data-masterid='" + row["MaterialMasterID"].ToString() + "' data-lineno='"+ row["LineNumber"].ToString() + "' data-qty='" + row["Quantity"].ToString() + "'/></td>";
        //                //        divContent += "</tr>";
        //                //    }
        //                //    divContent += "</tbody>";
        //                //    divContent += "</table>";
        //                //    polineContainer.InnerHtml = divContent;
        //                //    ScriptManager.RegisterStartupScript(this, this.GetType(), "test", "SetTableSettings("+JsonConvert.SerializeObject(ds)+");", true);
        //                //}
        //            }

        //            if (Grnflag == "1")
        //            {
        //                ScriptManager.RegisterStartupScript(this, this.GetType(), "test", "PartialGRN(" + GVPOLineItems.Rows.Count + ");", true);
        //            }
        //        }
        //        catch (Exception exp)
        //        {
        //            throw exp;
        //        }

        //    }

        //}

        //  ====================== Added By M.D.Prasad ================================//
        private DataSet Build_POLineItems()
        {
            String sCmdSupplerMaterialList = ViewState["POLineItems"].ToString();
            DataSet dsPOLineItems = DB.GetDS(sCmdSupplerMaterialList, false);
            return dsPOLineItems;
        }

        private void Build_POLineItems(DataSet dsPOLineItems)
        {
            GVPOLineItems.DataSource = dsPOLineItems;
            GVPOLineItems.DataBind();
            dsPOLineItems.Dispose();
        }

        //Dock Release
        protected void lnkdockupdate_Click(object sender, EventArgs e)
        {

            cp = HttpContext.Current.User as CustomPrincipal;
            Page.Validate("dockDetails");

            if (Page.IsValid == false)
            {
                resetError("Please enter Manadatory fields", true);
                return;
            }

            if(hdfdock.Value == "" || hdfdock.Value == "0" || hdfdock.Value== null)
            {
                resetError("Please select Dock", true);
                return;
            }

            //if (!cp.IsInAnyRoles("7"))
            //{

            //    resetError("Only IB Initiator can update this shipment", true);
            //    return;

            //}
            int inbdockid = 0;
            //int DockExits = DB.GetSqlN("Select COUNT(InboundDockID) AS N from INB_Inbound_Dock where InboundID=" + IBTrack.InboundTrackingID + " and DockID = " + hdfdock.Value + " and IsActive  = 1 AND IsDeleted = 0");
           //inbdockid = DB.GetSqlN("Select InboundDockID AS N from INB_Inbound_Dock where InboundID=" + IBTrack.InboundTrackingID+ " and DockID = "+ hdfdock.Value + " and VehicleRegNo ='"+ txtVehicleNo.Text + "' and IsActive  = 1 AND IsDeleted = 0");
            inbdockid = DB.GetSqlN("Select InboundDockID AS N from INB_Inbound_Dock where InboundID=" + IBTrack.InboundTrackingID + " and DockID = " + hdfdock.Value + "  and IsActive  = 1 AND IsDeleted = 0");
           // int singleDock = DB.GetSqlN("Select InboundDockID AS N from INB_Inbound_Dock where InboundID=" + IBTrack.InboundTrackingID + " and DockID = " + hdfdock.Value + " and VehicleRegNo <>'" + txtVehicleNo.Text + "' and IsActive  = 1 AND IsDeleted = 0");
            if (IBTrack.InBoundStatusID >= 6)
            {
                //resetError("Dock already Updated <br/>", true);
                resetError("Unable to update the Dock <br/>", true);
                return;
            }
            //if (DockExits>0 && singleDock == 0)
            //{
            //    resetError("Can not add Multiple Vehicles for a Dock <br/>", true);
            //    return;
            //}
            string createdon = DateTime.UtcNow.ToString("yyyy-MM-dd hh:mm:ss");
            if (IBTrack.InboundTrackingID != null)
            {
                StringBuilder sbUpdatedock = new StringBuilder();
                sbUpdatedock.Append("Exec sp_INB_Upsert_inbounddock ");

                sbUpdatedock.Append("@InboundID=" + IBTrack.InboundTrackingID);
                sbUpdatedock.Append(",@InboundDockID=" + inbdockid);
                sbUpdatedock.Append(",@DockID=" + hdfdock.Value);
                sbUpdatedock.Append(",@VehicleRegNo=" + DB.SQuote(txtVehicleNo.Text));
                sbUpdatedock.Append(",@DriverName=" + DB.SQuote(txtdriver.Text));
                sbUpdatedock.Append(",@CreatedOn=" + DB.DateQuote(createdon));
                sbUpdatedock.Append(",@CreatedBy=" + cp.UserID);
                try
                {
                    DB.ExecuteSQL(sbUpdatedock.ToString());
                    resetError("Dock details successfully updated", false);
                    //pnlShipmentReceived.Visible = true;
                    //txtShipmentReceivedDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    //txtOffLoadingTime.Text = DateTime.Now.ToString("hh:mm tt");
                    upnlTPLInboundCharge.Visible = true;
                    lnkAddNewTPLActivity.Visible = true;

                    //ValidateInboundStatus();

                    upnlBasicDetails.Update();
                   // upnlTPLInboundCharge.Update();

                }
                catch (Exception ex)
                {
                    CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
                    resetError("Error while updating dock details", true);
                }
            }
            else
            {
                resetError("Error while updating dock details", true);
            }
            //Get Docking Data and Bind to Grid
            LoadDockGrid();
            txtdock.Text = "";
            txtVehicleNo.Text = "";
            txtdriver.Text = "";
            hdfdock.Value = "";

        }
        protected void LoadDockGrid()
        {
            //Get Docking Data and Bind to Grid
            string Query = "EXEC[dbo].[sp_INB_GETInbDocks] @InboundID = " + IBTrack.InboundTrackingID + "";
            DataSet DS = DB.GetDS(Query, false);
            GridDockVeh.DataSource = DS.Tables[0];
            GridDockVeh.DataBind();
           
        }
        protected void LoaddockDetails()
        {
            DataSet ds = DB.GetDS("Select Inb.Dockid,doc.DockNo,Inb.VehicleRegNo,Inb.DriverName from inb_inbound_dock Inb join GEN_Dock doc on Inb.DockID=doc.DockID  where Inb.IsActive=1 AND Inb.IsDeleted=0 AND doc.IsActive=1 AND doc.IsDeleted=0 AND Inb.InboundID=" + IBTrack.InboundTrackingID,false);
            if (IBTrack.InboundTrackingID != 0)
            {
                if (ds.Tables[0].Rows.Count != 0)
                {

                    hdfdock.Value = ds.Tables[0].Rows[0]["Dockid"].ToString();
                    txtdock.Text = ds.Tables[0].Rows[0]["DockNo"].ToString();
                    txtVehicleNo.Text = ds.Tables[0].Rows[0]["VehicleRegNo"].ToString();
                    txtdriver.Text = ds.Tables[0].Rows[0]["DriverName"].ToString();
                }
                else
                {
                    hdfdock.Value = "";
                    txtdock.Text = "";
                    txtVehicleNo.Text = "";
                    txtdriver.Text = "";
                }

            }
            else
            {
                
            }

        }

        [WebMethod]
        public static List<Discrepancy> getdiscrepancylist(int InboundID)
        {
            List<Discrepancy> lst = new List<Discrepancy>();
            string Query = "EXEC[dbo].[sp_INB_DiscrepancyList] @InboundID = "+ InboundID + "";
            DataSet DS = DB.GetDS(Query, false);
            if(DS.Tables[0].Rows.Count != 0)
            {
                foreach(DataRow row in DS.Tables[0].Rows)
                {
                    Discrepancy DSY = new Discrepancy();
                    DSY.InboundiD = Convert.ToInt32(row["InboundID"]);
                    DSY.PoHeaderID = Convert.ToInt32(row["POHeaderID"]);
                    DSY.PONumber = row["PONumber"].ToString();
                    DSY.LinenumberCount = row["LinenumberCount"].ToString();
                    DSY.TotalInvoiceQty = Convert.ToDecimal(row["TotalInvoiceQuantity"]);
                    DSY.TotalPendingQty = Convert.ToDecimal(row["TotalPendingQty"]);
                    DSY.Discrepancystatus = row["POLINESTATUS"].ToString();
                    DSY.Isselected = false;
                    DSY.discrepancylineitem = Getdiscrepancylineitem(DSY.InboundiD, DSY.PoHeaderID);
                    lst.Add(DSY);
                }
            }
            return lst;
        }

        public static List<DiscrepancyLineitems> Getdiscrepancylineitem(int InboundID, int POHeaderID)
        {
            List<DiscrepancyLineitems> dli = new List<DiscrepancyLineitems>();
            string Query = "EXEC [dbo].[sp_INB_POWISE_Discrepancy] @InboundID = "+ InboundID + ",@POHeaderID =  "+ POHeaderID + "";
            DataSet DS = DB.GetDS(Query, false);
            if (DS.Tables[0].Rows.Count != 0)
            {
                foreach (DataRow row in DS.Tables[0].Rows)
                {
                    DiscrepancyLineitems DLI = new DiscrepancyLineitems();
                    DLI.MCode = row["MCode"].ToString();
                    DLI.SupplierInvoice = row["InvoiceNumber"].ToString();
                    DLI.LineNumber = row["LineNumber"].ToString();
                    DLI.InvoiceQty = Convert.ToDecimal(row["InvoiceQuantity"]);
                    DLI.ReceivedQty = Convert.ToDecimal(row["QTY"]);
                    DLI.Pendingqty = Convert.ToDecimal(row["PendingQty"]);
                    dli.Add(DLI);
                }
            }
            return dli;
        }





        public class Discrepancy
        {
            public int InboundiD { get; set; }
            public int PoHeaderID { get; set; }
            public string PONumber { get; set; }
            public string LinenumberCount { get; set; }
            public string Discrepancystatus { get; set; }
            public decimal TotalInvoiceQty { get; set; }
            public decimal TotalPendingQty { get; set; }
            public Boolean Isselected { get; set; }
            public List<DiscrepancyLineitems> discrepancylineitem { get; set; }

        }
        public class DiscrepancyLineitems
        {
            public string MCode { get; set; }
            public string LineNumber { get; set; }
            public string SupplierInvoice { get; set; }
            public decimal InvoiceQty { get; set; }
            public decimal ReceivedQty { get; set; }
            public decimal Pendingqty { get; set; }
        }

        protected void lnkdescsubmit_Click(object sender, EventArgs e)
        {
            cp = HttpContext.Current.User as CustomPrincipal;
            Page.Validate("updateShipmentVerified1");

            if (chkHasDiscrepancy.Checked == false)
            {
                resetError("Please check Has Discrepancy.", true);
                return;
            }


            if (Page.IsValid == false)
            {
                resetError("Please check the mandatory fields", true);
                return;
            }
            if(hifCheckedBy.Value == "")
            {
                resetError("Please select Checked By", true);
                return;
            }


            string Query = "UPDATE INB_InboundTracking_Warehouse SET HasDiscrepancy = 1, Disc_Remarks = "+DB.SQuote(txtDiscRemarks.Text) +" , "+"" +
                            " Disc_CheckedBy = "+hifCheckedBy.Value+" , Disc_CheckedDate = "+DB.SQuote(txtCheckedDate.Text.Trim())+", Disc_VerifiedBy = "+DB.SQuote(txtVerifiedBy.Text) +", "+
                            " Disc_VerifiedDate = "+ DB.SQuote(txtVerifiedDate.Text.Trim()) + ", UpdatedOn = GETUTCDATE(), UpdatedBy = "+cp.UserID+" WHERE InboundID  = "+ IBTrack.InboundTrackingID + "";

            DB.ExecuteSQL(Query);
            resetError("Successfully updated", false);
        }

        //======================= Added by M.D.Prasad for Discrepency =========================//
        [WebMethod]
        public static string getDiscrepency(int IbdId)
        {
            DataSet ds = DB.GetDS("EXEC [dbo].[USP_CheckDiscrepency_Inbound] @InboundID =" + IbdId, false);
            return JsonConvert.SerializeObject(ds);
        }
        
        //protected void GVPOLineItems_PageIndexChanging(object sender, GridViewPageEventArgs e)
        //{
        //    //GVPOLineItems.PageIndex = e.NewPageIndex;
        //    //GVPOLineItems.EditIndex = -1;
        //    //this.Build_POLineItems(this.Build_POLineItems());

        //    RememberOldValues();
        //    GVPOLineItems.PageIndex = e.NewPageIndex;
        //    this.Build_POLineItems(this.Build_POLineItems());
        //    RePopulateValues();
        //}
        //======================= Added by M.D.Prasad for Discrepency =========================//

        private void RememberOldValues()
        {     
           
            int index = -1;
            foreach (GridViewRow row in GVPOLineItems.Rows)
            {
                index = (int)GVPOLineItems.DataKeys[row.RowIndex].Value;
                bool result = ((CheckBox)row.FindControl("chkIsDelete")).Checked;

                // Check in the Session
                if (Session["CHECKED_ITEMS"] != null)
                    categoryIDList = (ArrayList)Session["CHECKED_ITEMS"];
                if (result)
                {
                    if (!categoryIDList.Contains(index))
                        categoryIDList.Add(index);
                }
                else
                    categoryIDList.Remove(index);
            }
            if (categoryIDList != null && categoryIDList.Count > 0)
                Session["CHECKED_ITEMS"] = categoryIDList;
        }

        private void RePopulateValues()
        {
            ArrayList categoryIDList = (ArrayList)Session["CHECKED_ITEMS"];
            if (categoryIDList != null && categoryIDList.Count > 0)
            {
                foreach (GridViewRow row in GVPOLineItems.Rows)
                {
                    int index = (int)GVPOLineItems.DataKeys[row.RowIndex].Value;
                    if (categoryIDList.Contains(index))
                    {
                        CheckBox myCheckBox = (CheckBox)row.FindControl("chkIsDelete");
                        myCheckBox.Checked = true;
                    }
                }
            }
        }

        protected void selectAll_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chckheader = (CheckBox)GVPOLineItems.HeaderRow.FindControl("selectAll");

            foreach (GridViewRow row in GVPOLineItems.Rows)

            {

                CheckBox chckrw = (CheckBox)row.FindControl("chkIsDelete");

                if (chckheader.Checked == true)

                {
                    chckrw.Checked = true;
                }
                else

                {
                    chckrw.Checked = false;
                }

            }
        }



        protected void GridDockVeh_RowBound(object sender, GridViewCommandEventArgs e)
        {
            GridViewRow rowSelect = (GridViewRow)(((Button)e.CommandSource).NamingContainer);
            int rowindex = rowSelect.RowIndex;
        }
        protected void lnkDeleteDock_Click(object sender, EventArgs e)
        {
            cp = HttpContext.Current.User as CustomPrincipal;
            string DockID = ((LinkButton)sender).CommandArgument.ToString();

            
            string Query = "EXEC[dbo].[sp_INB_DeleteInbDocks] @InboundDockID = " + DockID + ", @UserID = " + cp.UserID;
            int i = DB.GetSqlN(Query);
            LoadDockGrid();
            txtdock.Text = "";
            txtVehicleNo.Text = "";
            txtdriver.Text = "";
            hdfdock.Value = "";
            if(i==1)
            {
                resetError("Successfully removed...", false);
            }
            else
            {
                resetError("Dock can not be removed at this stage", true);
            }
            
        }

        protected void lnkEditDock_Click(object sender, EventArgs e)
        {

            string DockID = ((LinkButton)sender).CommandArgument.ToString();

            string Query = "EXEC [dbo].[sp_INB_GetInbDock]  @InboundDockID = " + DockID + " ";


            DataSet ds = DB.GetDS(Query, false);
            if (ds.Tables[0].Rows.Count != 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    txtdock.Text= row["DockNo"].ToString();
                    txtVehicleNo.Text = row["VehicleRegNo"].ToString();
                    txtdriver.Text = row["DriverName"].ToString();
                    hdfdock.Value= row["DockID"].ToString();
                }
            }

        }


        [WebMethod]
        public static List<BL.GRNDetails> FetchGRNDataForInbound(string InboundId, string PoheaderID, string supplierinvoiceid)
        {
            List<BL.GRNDetails> grnData = new List<BL.GRNDetails>();
            try
            {
                grnData = new BL.Inbound().FetchGRNDataForInbound(InboundId, PoheaderID, supplierinvoiceid);
                return grnData;
            }
            catch (Exception ex)
            {
                return grnData;
            }


        }

        [WebMethod]
        public static string CreateGRN(List<BL.GRNDetails> ReceivingInfo, string InboundId, string PoheaderID, string supplierinvoiceid, string remarks)
        {
            try
            {
                customPriciple = HttpContext.Current.User as CustomPrincipal;
                
                
               return new BL.Inbound().CreateGRNEntryAndPostDatatoSAP(InboundId, PoheaderID, supplierinvoiceid, ReceivingInfo, customPriciple, remarks);


            }
            catch (Exception ex)
            {
                return "Error " + ex.Message;
            }


        }
        [WebMethod]
        public static string GET_InboundStatus(string InboundID)
        {
            try
            {
                string Status = DB.GetSqlS("SELECT InboundStatus S FROM INB_Inbound IBT JOIN INB_InboundStatus INS ON INS.InboundStatusID=IBT.InboundStatusID WHERE IBT.IsDeleted=0 AND IBT.IsActive=1 AND IBT.InboundID=" + InboundID);
                return Status;
            }
            catch (Exception ex)
            {
                return "Error : " + ex;
            }
        }

        [WebMethod]
        public static string GetGRNData(int inboundid)
        {
            try
            {
                return new BL.Inbound().GetGRNData(inboundid);
            }
            catch (Exception ex)
            {
                return null;
            }

        }


    }
}