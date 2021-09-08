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
using System.Net.Mail;
using System.Net.Mime;
using System.IO;
using System.Web.Services;
using System.Web.Script.Services;
using System.Globalization;
using Newtonsoft.Json;


// Module Name : Material Management
// Usecase Ref.: Item Master Details_UC_004
// DevelopedBy : Naresh P
// Created On  : 04/10/2013 
// Modified On : 24/03/2015


namespace MRLWMSC21.mMaterialManagement
{
    public partial class MaterialMasterRequest : System.Web.UI.Page
    {

        string UoMSql;
        public CustomPrincipal cp = HttpContext.Current.User as CustomPrincipal;
        int TenantID = 0;
        int CreatedBy = 0;
        int RequestedBy = 0;
        String SOCheck = "";
        String POCheck = "";
        string TenantRootDir = "";
        string MMTPath = "";
        string TenantCurrency = "";
        int mNewMMID = 0;
        int BinReplishementID = 0;
        public String mid = CommonLogic.QueryString("mid");
        
        // Set page Theme
        protected void Page_PreInit(object sender, EventArgs e)
        {
            Page.Theme = "MasterData";
        }

        public void Page_Error(Object sender, EventArgs e)
        {
            // Implementation here

        }

        //This method fires when page loading
        protected void Page_Load(object sender, EventArgs e)
        {

            CreatedBy = cp.UserID;
            TenantID = cp.TenantID;
            RequestedBy = cp.UserID;

            String MMID = CommonLogic.QueryString("mid").ToString() == "" ? "0" : CommonLogic.QueryString("mid").ToString();

            ViewState["MaterialMasterID"] = MMID;

            // This page is allowed for users having these (MMAdmin, Sales Coordinator, Purchase Coordinator, System Admin,Super Admin, Store Manager) roles
            if (!cp.IsInAnyRoles(CommonLogic.GetRolesAllowed(CommonLogic.GetRolesAllowedForthisPage("Item Master Request"))))
            {
                Response.Redirect("../Login.aspx?eid=6");
            }

            //Get Tenant Currency
            //TenantCurrency = DB.GetSqlS(" select GEN_C.Code AS S from GEN_Tenant GEN_T left join GEN_Currency GEN_C on GEN_C.CurrencyID=GEN_T.CurrencyID where GEN_T.TenantID=" + cp.TenantID);

            TenantCurrency = DB.GetSqlS(" select TOP 1 GEN_C.Code AS S from TPL_Tenant_AddressBook TNT_A left join GEN_Currency GEN_C on GEN_C.CurrencyID=TNT_A.CurrencyID WHERE TNT_A.TenantID=" + (hifTenant.Value == "" ? "0" : hifTenant.Value));

            ltLoggedUserCurrency.Text = TenantCurrency;

            //ViewState["TenantID"] = TenantID;

            ViewState["MMID"] = CommonLogic.QueryString("mid").ToString();

            hifMMID.Value = MMID;

            //Get Tenant Root Directory
            TenantRootDir = DB.GetSqlS("select DefaultValue AS S from SYS_SystemConfiguration SYS_C left join SYS_SysConfigKey SYS_K on SYS_K.SysConfigKeyID=SYS_C.SysConfigKeyID where SysConfigKey='TenantContentPath'");

            // Get MaterialMaster Path
            MMTPath = DB.GetSqlS(" select DefaultValue AS S from SYS_SystemConfiguration SYS_C left join SYS_SysConfigKey SYS_K on SYS_K.SysConfigKeyID=SYS_C.SysConfigKeyID where SysConfigKey='MaterialManagementPath'");

            if (DB.GetSqlS("select SysConfigValue AS S from SYS_SystemConfiguration SYS_SC left join SYS_SysConfigKey SYS_SCK on  SYS_SCK.SysConfigKeyID=SYS_SC.SysConfigKeyID where SYS_SCK.SysConfigKey='IsDecimalInventoryAllowed' and SYS_SCK.IsActive=1") == "true")
                ltgvUoMStatus.Text = "Decimal Inventory is enabled";

            if (!IsPostBack)
            {
                // Validate required fields
                Page.Validate("UpdateMaterial");

                //Load suppliers list in supplier attachments
                LoadSid();

                DesignLogic.SetInnerPageSubHeading(this.Page, "Item Master Request");

                // Load Material Master Storage Parameter CheckBox List
                LoadMspCheckBoxList();

                ViewState["TenantID"] = TenantID;
                ViewState["MaterialMasterID"] = MMID;

                //Load Material Types
                LoadDropDown(ddlMTypeID, "Select MTypeID, MType + ' - ' + Description as MTypeDesc from MMT_MType Where IsActive =1", "MTypeDesc", "MTypeID", "Select Type");

                // As per the requirements specified fields are enabled or disabled based on page request
                if ((CommonLogic.QueryString("mid") == "") || (CommonLogic.QueryString("edittype") == "copy"))
                {
                    ltMCodeLabel.Visible = true;
                    //txtMCode.Visible = false;
                    //rfvMaterialMasterCode.Enabled = false;
                    ltMCodeAlternativeLabel.Visible = false;
                    txtMCodeAlternative.Visible = false;
                    ltMCodeAlternativeLabel2.Visible = false;
                    txtMCodeAlternative2.Visible = false;
                    chkIsActive.Visible = false;
                    chkIsApproved.Visible = false;
                    txtSearchMCode.Visible = false;
                    lnkSearchMaterial.Visible = false;
                    pnlMMCodeDetails.Visible = true;
                    lnkAddQCParameters.Visible = false;

                    lnkAddSupplier.Visible = false;

                    pnlMMQCParameters.Visible = false;
                    lnkAddRvsnHistory.Visible = false;

                    rfvddlMTypeID.Enabled = true;
                    rfvCatalogNumber.Enabled = true;

                    //lnkAddTenant.Visible = false;
                    linkmspsave.Visible = false;

                    upnlTenant.Visible = false;

                    lnkUpdateMSPs.Visible = false;

                    if (CommonLogic.QueryString("edittype") == "copy")
                    {
                        //pnlUoMMeasurementGrid.Visible = false; //Prasad
                        pnlSupplier.Visible = false;
                        pnlRvsnHistory.Visible = false;
                        lnkAddRvsnHistory.Visible = false;
                        mmRevision.Visible = false;
                        lblvieweattachment.Visible = false;

                        //pnlTenantList.Visible = false;
                        upnlTenant.Visible = false;
                    }

                    lnkSendRequest.Text = "Send Request" + CommonLogic.btnfaSave;
                    lnkSendRequest2.Text = "Send Request" + CommonLogic.btnfaSave;

                    lnkupdateDimention.Enabled = false;
                    lnkUpdateTenant.Enabled = false;
                }
                else
                {
                    lnkupdateDimention.Enabled = true;
                    //lnkUpdateTenant.Enabled = true;
                    pnlMMCodeDetails.Visible = true;
                    lnkSendRequest.Text = "Update Material" + CommonLogic.btnfaUpdate;
                    lnkSendRequest2.Text = "Update Material" + CommonLogic.btnfaUpdate;
                    // txtMCode.Enabled = false;
                    lnkUoMConfig.Visible = true;
                    //pnlUoMMeasurementGrid.Visible = true; //Prasad
                    // txtMCode.Enabled = false;
                    // txtMfgPartNo.Enabled = false;
                    // rfvMaterialMasterCode.Enabled = true;

                    lnkAddQCParameters.Visible = true;

                    lnkAddSupplier.Visible = true;
                    lnkAddRvsnHistory.Visible = false;
                    lblvieweattachment.Visible = true;
                    mmRevision.Visible = true;

                    lnkButCancel.Visible = false;
                    lnkButCancel2.Visible = false;
                    linkmspsave.Visible = true;
                    //lnkAddTenant.Visible = true;

                }

                if (CommonLogic.QueryStringNativeInt("mid") != 0 && CommonLogic.QueryString("copyok") == "success")
                {
                    //lblStatus.Text = "Successfully submitted the request, but alert email was not sent <br/>";
                    resetError("Successfully submitted the request, but alert email was not sent <br/>", false);
                }

                if (CommonLogic.QueryStringNativeInt("mid") != 0 && CommonLogic.QueryString("status") == "success")
                {
                   // lblStatus.Text = "Successfully submitted the request, but alert email was not sent <br/>";
                    resetError("Successfully submitted the request, but alert email was not sent <br/>", false);
                }

                //Load UoM Grid if MaterialMasterID!=0
                if (CommonLogic.QueryStringNativeInt("mid") != 0)
                {

                    UoMSql = "EXEC [dbo].[sp_GEN_GetUoMDetails] @MaterialMasterID=" + CommonLogic.QueryString("mid");

                    ViewState["UoMListSQL"] = UoMSql;

                    this.GENUoM_buildGridData(this.GENUoM_buildGridData());

                    if (gvUoM.Rows.Count > 0)
                    {
                        Build_FromMeasurement(((HiddenField)gvUoM.Rows[0].FindControl("hifMeasuretypeID")).Value);
                        ddlMeadureType.SelectedValue = ((HiddenField)gvUoM.Rows[0].FindControl("hifMeasuretypeID")).Value;
                        ddlMeadureType.Enabled = false;
                    }


                    ValidItemDimention();

                    ValidateTenant();
                    

                    ViewState["QCParametersSQL"] = "EXEC [dbo].[sp_MMT_GetMaterialMaster_QualityParameters]  @MaterialMasterID=" + CommonLogic.QueryString("mid");

                    this.gvQCParameters_buildGridData(this.gvQCParameters_buildGridData());


                    ViewState["SupplierListSQL"] = "EXEC [dbo].[sp_MMT_GetMaterialMaster_Supplier]  @MaterialMasterID=" + CommonLogic.QueryString("mid");

                    gvSupplierDetails_buildGridData(gvSupplierDetails_buildGridData());


                    //  for msp list

                    ViewState["mspListSQL"] = "EXEC [dbo].[MMT_GET_MSP]  @MaterialMasterID =" + CommonLogic.QueryString("mid");

                    gvmspList_buildGridData(gvmspList_buildGridData());

                    // End

                    ViewState["RvsnHistoryListSQL"] = "EXEC [dbo].[SP_MMT_GetMaterialMaster_Revision]  @MaterialMasterID=" + CommonLogic.QueryString("mid");
                    this.gvRvsnHistory_buildGridData(this.gvRvsnHistory_buildGridData());

                    mmRevision.Text = DB.GetSqlS("select top 1  '<nobr>'+ Revision + '<nobr/>' AS S from MMT_MaterialMaster_Revision where IsActive=1 AND IsDeleted=0 AND MaterialMasterID=" + CommonLogic.QueryString("mid") + "  order by  MaterialMasterRevisionID  desc ");


                    ViewState["TenantListSQL"] = "EXEC [dbo].[sp_TPL_GetTenantMaterialData]  @MaterialMasterID=" + CommonLogic.QueryString("mid");

                    TenantList_buildGridData(TenantList_buildGridData());

                    if (gvTenantList.Rows.Count == 0)
                    {
                        lnkAddSupplier.Visible = false;
                    }
                    else
                        lnkAddSupplier.Visible = true;
                }


                // Load  MaterialMaster data when MaterialMasterID!=0
                if (CommonLogic.QueryString("mid") != "")
                {
                    lnkbtnsave.Visible = true;
                    this.gvItemwiseloc(this.gvItemwiseloc());
                    LoadMMItemData(CommonLogic.QueryString("mid"));
                    if (CommonLogic.QueryString("mid") != "" && CommonLogic.QueryString("edittype") != "copy")
                    {

                        if (ddlMTypeID.SelectedValue == "8" || ddlMTypeID.SelectedValue == "9")
                        {
                            pnlRvsnHistory.Visible = true;
                            mmRevision.Visible = true;
                        }
                        else
                        {
                            pnlRvsnHistory.Visible = false;
                            mmRevision.Visible = false;
                        }
                    }


                }

                if (CommonLogic.QueryString("mid") == "" || CommonLogic.QueryString("edittype") == "copy")
                {
                    Atachment.Visible = false;
                }
                else
                {
                    Atachment.Visible = true;
                }
                LoadDropDown(ddldocumenttype, "select FileTypeID,FileType from MMT_FileType where isdeleted=0 and isactive=1", "FileType", "FileTypeID", "Select Attachment Type");
            }

            middailogbox();
        }

        private void ValidItemDimention() {
            //lnkupdateDimention.Enabled = (DB.GetSqlN("select isnull((select top 1 TTSu.SpaceUtilizationID as N from TPL_Tenant_MaterialMaster TTM " +
            //                                        " join TPL_Tenant_SpaceUtilization TTSU on TTM.TenantID=TTSU.TenantID and TTSU.IsActive=1 and TTSU.IsDeleted=0 " +
            //                                        " where TTM.IsActive=1 and TTM.IsDeleted=0 and TTM.MaterialMasterID=" + CommonLogic.QueryString("mid") + " and TTSU.SpaceUtilizationID!=4),0) as N") == 0);

            lnkupdateDimention.Enabled = (DB.GetSqlN("SELECT TOP 1 1 N FROM TPL_Tenant_MaterialMaster WHERE IsDeleted=0 AND MaterialMasterID="+CommonLogic.QueryString("mid")+" AND MaterialSpaceUtilizationID<>4")==0);
        }

        private void ValidateTenant()
        {
            SOCheck = DB.GetSqlS("EXEC [dbo].[sp_GEN_CheckSO] @MaterialMasterID=" + CommonLogic.QueryStringNativeInt("mid") + ",@TenantID=" + TenantID);
            POCheck = DB.GetSqlS("EXEC [dbo].[sp_GEN_CheckPO] @MaterialMasterID=" + CommonLogic.QueryStringNativeInt("mid") + ",@TenantID=" + TenantID);

            if (DB.GetSqlN("select COUNT(*) N from MMT_MaterialMaster_Supplier where IsDeleted=0 and MaterialMasterID=" + CommonLogic.QueryString("mid")) > 0)
            {
                txtTenant.Enabled = false;
                txtSpaceUtilization.Enabled = false;
            }
            else
            {
                txtTenant.Enabled = true;
                txtSpaceUtilization.Enabled = true;
            }

            if (SOCheck != "" || POCheck != "")
            {
               // lnkUpdateTenant.Enabled = false;
            }
            else
                lnkUpdateTenant.Enabled = true;

        }
        #region -------------------- Basic Data  ----------------------------------

        // Load MaterialMasterItem Data
        public void LoadMMItemData(String MMItemID)
        {
            String _MCode = "";

            SOCheck = DB.GetSqlS("EXEC [dbo].[sp_GEN_CheckSO] @MaterialMasterID=" + CommonLogic.QueryStringNativeInt("mid") + ",@TenantID=" + TenantID);
            POCheck = DB.GetSqlS("EXEC [dbo].[sp_GEN_CheckPO] @MaterialMasterID=" + CommonLogic.QueryStringNativeInt("mid") + ",@TenantID=" + TenantID);

            if (CommonLogic.QueryString("edittype") != "copy")
            {

                if (SOCheck != "" || POCheck != "")
                {
                    atcPlantID.Enabled = false;
                    ddlMTypeID.Enabled = false;
                    atcStorageConditionID.Enabled = false;
                    chkIsActive.Enabled = false;
                    chkIsApproved.Enabled = false;

                }
            }

            // Load Basic Data
            // IDataReader rsMMITem = DB.GetRS("Select * from MMT_MaterialMaster MMT_M  left join MMT_MPlant MMT_P on MMT_M.MPlantID=MMT_P.MPlantID   left join MMT_MType MMT_MT on MMT_M.MTypeID= MMT_MT.MTypeID  left join MMT_StorageCondition MMT_STR on MMT_STR.StorageConditionID=MMT_M.StorageConditionID   left join MMT_MGroup MMT_G on MMT_G.MGroupID=MMT_M.MGroupID  left join MMT_ProductCategory MMT_MP on MMT_MP.ProductCategoryID=MMT_M.ProductCategoryID    Where MMT_M.MaterialMasterID =" + MMItemID);

            IDataReader rsMMITem = DB.GetRS("[dbo].[sp_MMT_GetMaterialMasterData] @MaterialMasterID=" + MMItemID + ",@TenantID=" + cp.TenantID);

            while (rsMMITem.Read())
            {
                //txtMCode.Text = DB.RSField(rsMMITem, "MCode");
                _MCode = DB.RSField(rsMMITem, "MCode");

                if (CommonLogic.QueryString("edittype") != "copy")
                {
                    txtMCodeAlternative.Text = DB.RSField(rsMMITem, "MCodeAlternative1");
                    txtMCodeAlternative2.Text = DB.RSField(rsMMITem, "MCodeAlternative2");
                }


                txtMfgPartNo.Text = DB.RSField(rsMMITem, "MCode");
                txtCustomerPartNumber.Text = DB.RSField(rsMMITem, "CustomerPartNumber");
                txtOEMPartNo.Text = DB.RSField(rsMMITem, "OEMPartNo");


                txtDescription.Text = DB.RSField(rsMMITem, "MDescription");
                txtDescriptionLong.Text = DB.RSField(rsMMITem, "MDescriptionLong");


                atcMTypeID.Text = DB.RSField(rsMMITem, "MType") + "-" + DB.RSField(rsMMITem, "Description");
                hifMTypeID.Value = DB.RSFieldInt(rsMMITem, "MTypeID").ToString();

                ddlMTypeID.SelectedValue = DB.RSFieldInt(rsMMITem, "MTypeID").ToString(); // for MSP List


                LoadMMMspList(CommonLogic.QueryString("mid"), DB.RSFieldInt(rsMMITem, "MTypeID").ToString());// Load MSP's for Unique MMID

                atcPlantID.Text = DB.RSField(rsMMITem, "Plant");
                hifPlantID.Value = DB.RSFieldInt(rsMMITem, "MPlantID").ToString();


                atcStorageConditionID.Text = CommonLogic.IIF(DB.RSFieldInt(rsMMITem, "StorageConditionID") != 0, DB.RSField(rsMMITem, "StorageCondition").ToString(), ""); // ** //
                hifStorageConditionID.Value = DB.RSFieldInt(rsMMITem, "StorageConditionID").ToString();

                txtIndustry.Text = CommonLogic.IIF(DB.RSFieldInt(rsMMITem, "IndustryID") != 0, DB.RSField(rsMMITem, "Industry").ToString(), ""); // ** //
                hdnIndustry.Value = DB.RSFieldInt(rsMMITem, "IndustryID").ToString();



                chkIsActive.Checked = Convert.ToBoolean(DB.RSFieldTinyInt(rsMMITem, "IsActive"));
                chkIsApproved.Checked = Convert.ToBoolean(DB.RSFieldTinyInt(rsMMITem, "IsAproved"));
                txtRemarks.Text = DB.RSField(rsMMITem, "Remarks");
                txtMLength.Text = CommonLogic.IIF((DB.RSFieldInt(rsMMITem, "MLength") != 0), DB.RSFieldInt(rsMMITem, "MLength").ToString(), null);
                txtMHeight.Text = CommonLogic.IIF((DB.RSFieldInt(rsMMITem, "MHeight") != 0), DB.RSFieldInt(rsMMITem, "MHeight").ToString(), null);
                txtMWidth.Text = CommonLogic.IIF((DB.RSFieldInt(rsMMITem, "MWidth") != 0), DB.RSFieldInt(rsMMITem, "MWidth").ToString(), null);
                txtMWeight.Text = CommonLogic.IIF((DB.RSFieldDecimal(rsMMITem, "MWeight") != 0), DB.RSFieldDecimal(rsMMITem, "MWeight").ToString(), null);
                txtCapacityperBin.Text = CommonLogic.IIF((DB.RSFieldDecimal(rsMMITem, "CapacityPerBin") != 0), DB.RSFieldDecimal(rsMMITem, "CapacityPerBin").ToString(), null);
                
                atcMaterialGroup.Text = CommonLogic.IIF(DB.RSFieldInt(rsMMITem, "MGroupID") != 0, DB.RSField(rsMMITem, "MaterialGroup").ToString(), ""); //* *//
                hifMaterialGroup.Value = DB.RSFieldInt(rsMMITem, "MGroupID").ToString();



                atcProductCategories.Text = DB.RSField(rsMMITem, "ProductCategory").ToString(); // ** //
                hifProductCategories.Value = DB.RSFieldInt(rsMMITem, "ProductCategoryID").ToString();


                lthidIsFirstEdit.Text = DB.RSFieldInt(rsMMITem, "IsFirstEdit").ToString();


                atcSalesUoM.Text = CommonLogic.IIF(DB.RSFieldInt(rsMMITem, "SUoMID") != 0, DB.RSField(rsMMITem, "SUoM").ToString() + "-" + DB.RSField(rsMMITem, "SUoMDescription").ToString(), "");
                hifSalesUoM.Value = DB.RSFieldInt(rsMMITem, "SUoMID").ToString();

                txtSalesConvsFactor.Text = DB.RSFieldDecimal(rsMMITem, "SUoMQty").ToString();


                atcPurchaseUom.Text = CommonLogic.IIF(DB.RSFieldInt(rsMMITem, "PUoMID") != 0, DB.RSField(rsMMITem, "PurUoM").ToString() + "-" + DB.RSField(rsMMITem, "PurUoMDescription"), "");
                hifPurchaseUom.Value = DB.RSFieldInt(rsMMITem, "PUoMID").ToString();

                txtPurchaseConvsFactor.Text = DB.RSFieldDecimal(rsMMITem, "PUoMQty").ToString();


                txtReorderPoint.Text = DB.RSFieldDecimal(rsMMITem, "ReorderPoint").ToString();
                txtreordermax.Text = DB.RSFieldDecimal(rsMMITem, "ReorderQtyMax").ToString();
                txtreordermin.Text = DB.RSFieldDecimal(rsMMITem, "ReorderQtyMin").ToString();
                txtMaximumStockLevel.Text = DB.RSFieldDecimal(rsMMITem, "MaximumStockLevel").ToString();
                txtMinimumStockLevel.Text = DB.RSFieldDecimal(rsMMITem, "MinimumStockLevel").ToString();


                txtRefContainer.Text = CommonLogic.IIF(DB.RSFieldInt(rsMMITem, "StorageConditionID") != 0, DB.RSField(rsMMITem, "StorageCondition").ToString(), "");
                txtMinShelfLife.Text = DB.RSFieldInt(rsMMITem, "MinShelfLifeinDays").ToString();
                txtTotalShelfLife.Text = DB.RSFieldInt(rsMMITem, "TotalShelfLifeinDays").ToString();


                txtStandardPrice.Text = DB.RSField(rsMMITem, "StandardPrice");
                atcUoM.Text = CommonLogic.IIF(DB.RSFieldInt(rsMMITem, "AUoMID") != 0, DB.RSField(rsMMITem, "AUoM").ToString() + "-" + DB.RSField(rsMMITem, "AUoMDescription"), "");
                hifUoM.Value = DB.RSFieldInt(rsMMITem, "AUoMID").ToString();


                txtPrUoM.Text = CommonLogic.IIF(DB.RSFieldInt(rsMMITem, "PrUoMID") != 0, DB.RSField(rsMMITem, "PrUoM").ToString() + "-" + DB.RSField(rsMMITem, "PrUoMDescription"), "");
                hifPrUoMID.Value = DB.RSFieldInt(rsMMITem, "PrUoMID").ToString();

                txtPrUoMQty.Text = DB.RSFieldDecimal(rsMMITem, "PrUoMQty").ToString();



                txtTenant.Text = DB.RSField(rsMMITem, "TenantName");
                txtTenantPartNo.Text = DB.RSField(rsMMITem, "TenantPartNo");
                txtSpaceUtilization.Text = DB.RSField(rsMMITem, "SpaceUtilization");
                txtPrice.Text = DB.RSFieldDecimal(rsMMITem, "Price").ToString();
                chkIsInsurance.Checked = Convert.ToBoolean(DB.RSFieldTinyInt(rsMMITem, "IsInsurance"));

                hifTenant.Value = DB.RSFieldInt(rsMMITem, "TenantID").ToString();
                hifSpaceUtilization.Value = DB.RSFieldInt(rsMMITem, "MaterialSpaceUtilizationID").ToString();

            }

            rsMMITem.Close();


            if (CommonLogic.QueryString("edittype") != "copy")
            {
                //Get Attatchment file
                //String sFileName = CommonLogic._GetAttatchmentFile(TenantRootDir + DB.GetSqlS("select UniqueID AS S from GEN_Tenant where TenantID=" + TenantID) + MMTPath, _MCode);

                String sFileName = CommonLogic._GetAttatchmentFile(TenantRootDir + hifTenant.Value + MMTPath, _MCode);

                if (sFileName != "")
                {
                    ltPicHolder.Text = "<img border='0' height='140' width='180' src='../" + sFileName + "'/>";
                }
                else
                {
                    ltPicHolder.Text = "";
                }
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

        // This method fires when User Clicks Search MaterialCode Button
        protected void lnkSearchMaterial_Click(object sender, EventArgs e)
        {

            if (txtSearchMCode.Text.Trim() != "")
            {
                string SqlQry = "SELECT Min(MaterialMasterID) as N from MMT_MaterialMaster Where MCode=" + DB.SQuote(txtSearchMCode.Text.Trim());

                int vMMID = DB.GetSqlN(SqlQry);

                if (vMMID != 0)// Check for Valid Material Code
                    Response.Redirect("MaterialMasterRequest.aspx?mid=" + vMMID); // Redirect to MaterialMasterRequest.aspx  if MCode correct
                else
                    resetError("Material does not exist", true); // Displays ErrorMsg when MCode is Invalid

            }
            else
            {
                resetError("Please enter a valid part number", true); // Displays ErrorMsg when MCode is Invalid
            }

        }

        // This method fires when UoM is repeated
        public Boolean IsUoMRepeated(int[] ArrUoM)
        {
            Boolean result = false;

            if (ArrUoM.Length != 0)
            {
                for (int UpArrItem = 0; UpArrItem < ArrUoM.Length; UpArrItem++)
                {
                    for (int LoArrItem = 0; LoArrItem < ArrUoM.Length; LoArrItem++)
                    {
                        if (UpArrItem != LoArrItem)
                        {
                            if (ArrUoM[UpArrItem] == ArrUoM[LoArrItem])
                            {
                                result = true;
                                return result;

                            }
                        }

                    }
                }
            }

            return result;

        }

        // This method fires when User clicks SenRequest  (or) Update Button 
        protected void lnkSendRequest_Click(object sender, EventArgs e)
        {

            decimal reordermin = 0;
            decimal reordermax = 0;
            decimal minshelf = 0;
            decimal maxshelf = 0;

            if (txtreordermin.Text != "")
            {
                reordermin = Convert.ToDecimal(txtreordermin.Text.ToString());
            }
            else
            {
               reordermin = 0;
            }

            if (txtreordermax.Text != "")
            {
                reordermax = Convert.ToDecimal(txtreordermax.Text.ToString());
            }
            else
            {
                reordermax = 0;
            }

            if (txtMinShelfLife.Text != "")
            {
                minshelf = Convert.ToDecimal(txtMinShelfLife.Text.ToString());
            }
            else
            {
                minshelf = 0;
            }

            if (txtTotalShelfLife.Text != "")
            {
                maxshelf= Convert.ToDecimal(txtTotalShelfLife.Text.ToString());
            }
            else
            {
                maxshelf = 0;
            }




            if (reordermin > reordermax)
            {
                resetError("Reorder Min. Qty. cannot be Greater than Reorder Max. Qty.",true);
                return;
            }

            if (minshelf > maxshelf)
            {
                resetError("Min. Shelf Life cannot be Greater than Total Shelf Life", true);
                return;
            }

            Page.Validate("UpdateMaterial");


            if (!Page.IsValid)
            {
                resetError("Please check for mandatory fields", true);
                return;
            }

            if (!cp.IsInAnyRoles("13"))
            {
                this.resetError("Only MMAdmin can access this details", true);
                return;
            }

            if (CommonLogic.QueryString("mid") != "" && CommonLogic.QueryString("edittype") != "copy")
            {

                //if (txtMCode.Text != txtMfgPartNo.Text)
                //{
                //    resetError("Invalid Part#", true);
                //    return;
                //}
            }

            if (CommonLogic.QueryString("mid") != "" && CommonLogic.QueryString("edittype") != "copy")
            {
                SOCheck = DB.GetSqlS("EXEC [dbo].[sp_GEN_CheckSO] @MaterialMasterID=" + CommonLogic.QueryStringNativeInt("mid") + ",@TenantID=" + TenantID);
                POCheck = DB.GetSqlS("EXEC [dbo].[sp_GEN_CheckPO] @MaterialMasterID=" + CommonLogic.QueryStringNativeInt("mid") + ",@TenantID=" + TenantID);
            }


            if (SOCheck != "" || POCheck != "")
            {
                this.resetError("Cannot update, as this material is configured in  " + CommonLogic.IIF(SOCheck != "", "SO Numbers : ", "") + SOCheck + " " + CommonLogic.IIF(POCheck != "", "PO Numbers : ", "") + POCheck, true);
                return;
            }

            if (CommonLogic.QueryString("mid") != "" && CommonLogic.QueryString("edittype") != "copy")
            {

                if (DB.GetSqlN("select MaterialMasterID AS N from MFG_RoutingHeader_Revision MFG_RV JOIN MMT_MaterialMaster_Revision MMT_MR ON MMT_MR.MaterialMasterRevisionID=MFG_RV.MaterialMasterRevisionID AND MMT_MR.IsActive=1 AND MMT_MR.IsDeleted=0 where MMT_MR.MaterialMasterID=" + CommonLogic.QueryString("mid") + " AND MMT_MR.IsActive=1 AND MMT_MR.IsDeleted=0") != 0)
                {
                    resetError("Cannot update, as this material is in use", true);
                    return;
                }
            }

            int ParentMMID = 0;

            string MCatlogNumber = "";

            if (ddlMTypeID.SelectedValue == "11")
            {
                if (Request.Form[hifProductCategories.UniqueID] != "3")
                {
                    resetError("Invalid product category", true);
                    return;
                }
            }


            StringBuilder sqlInsert = new StringBuilder();
            sqlInsert.Append("Declare @Result int ");
            sqlInsert.Append("EXEC [dbo].[sp_MMT_UpsertMaterialMasterItem]    ");

            //Basic Data
            sqlInsert.Append("@MPlantID=" + CommonLogic.IIF(hifPlantID.Value != "", Request.Form[hifPlantID.UniqueID], "null"));

            sqlInsert.Append(",@MTypeID=" + CommonLogic.IIF(ddlMTypeID.SelectedValue != "", ddlMTypeID.SelectedValue, "null"));

            sqlInsert.Append(",@CustomerPartNumber=" + CommonLogic.IIF(txtCustomerPartNumber.Text != "", DB.SQuote(txtCustomerPartNumber.Text), "null"));

            sqlInsert.Append(",@OEMPartNumber=" + CommonLogic.IIF(txtOEMPartNo.Text != "", DB.SQuote(txtOEMPartNo.Text), "null"));

            sqlInsert.Append(",@StorageConditionID=" + CommonLogic.IIF(Request.Form[hifStorageConditionID.UniqueID] != "", Request.Form[hifStorageConditionID.UniqueID], "NULL"));

            sqlInsert.Append(",@MDescription=" + CommonLogic.IIF(txtDescription.Text != "", DB.SQuote(txtDescription.Text), "null"));

            sqlInsert.Append(",@MDescriptionLong=" + CommonLogic.IIF(txtDescriptionLong.Text != "", DB.SQuote(txtDescriptionLong.Text), "null"));

            sqlInsert.Append(",@Remarks=" + DB.SQuote(txtRemarks.Text));

            //Item Dimentions
            sqlInsert.Append(",@MLength=" + CommonLogic.IIF(txtMLength.Text != "", txtMLength.Text, "null"));
            sqlInsert.Append(",@MHeight=" + CommonLogic.IIF(txtMHeight.Text != "", txtMHeight.Text, "null"));
            sqlInsert.Append(",@MWidth=" + CommonLogic.IIF(txtMWidth.Text != "", txtMWidth.Text, "null"));
            sqlInsert.Append(",@MWeight=" + CommonLogic.IIF(txtMWeight.Text != "", txtMWeight.Text, "null"));
            //Capacity Bin
            sqlInsert.Append(",@Cbin=" + CommonLogic.IIF(txtCapacityperBin.Text != "", txtCapacityperBin.Text, "null"));

            /* MMSalesData Table related Parameters */

            sqlInsert.Append(",@SUoMID=" + CommonLogic.IIF((Request.Form[hifSalesUoM.UniqueID] != "" && Request.Form[hifSalesUoM.UniqueID] != "0"), Request.Form[hifSalesUoM.UniqueID], "NULL"));
            sqlInsert.Append(",@SUoMQty=" + CommonLogic.IIF(txtSalesConvsFactor.Text != "", txtSalesConvsFactor.Text, "null"));

            sqlInsert.Append(",@PrUoMID=" + CommonLogic.IIF((Request.Form[hifPrUoMID.UniqueID] != "" && Request.Form[hifPrUoMID.UniqueID] != "0"), Request.Form[hifPrUoMID.UniqueID], "NULL"));
            sqlInsert.Append(",@PrUoMQty=" + CommonLogic.IIF(txtPrUoMQty.Text != "", txtPrUoMQty.Text, "null"));


            /* MMPurchaseData Table related Parameters */

            sqlInsert.Append(",@PUoMID=" + CommonLogic.IIF((Request.Form[hifPurchaseUom.UniqueID] != "" && Request.Form[hifPurchaseUom.UniqueID] != "0"), Request.Form[hifPurchaseUom.UniqueID], "NULL"));
            sqlInsert.Append(",@PUoMQty=" + CommonLogic.IIF(txtPurchaseConvsFactor.Text != "", txtPurchaseConvsFactor.Text, "null"));
            //sqlInsert.Append(",@CurrencyID=" + CommonLogic.IIF((Request.Form[hifCurrency.UniqueID] != "" && Request.Form[hifCurrency.UniqueID] != "0"), Request.Form[hifCurrency.UniqueID], "NULL")); 
            // sqlInsert.Append(",@POrderText=" + DB.SQuote(txtOrderText.Text));
            sqlInsert.Append(",@CurrencyID=NULL");
            sqlInsert.Append(",@POrderText=''");

            /* MMRequirementPlanning  Table related Parameters*/
            sqlInsert.Append(",@ReorderPoint=" + CommonLogic.IIF(txtReorderPoint.Text != "", txtReorderPoint.Text, "null"));
            sqlInsert.Append(",@ReorderQtyMax=" + CommonLogic.IIF(txtreordermax.Text != "", txtreordermax.Text, "null"));
            sqlInsert.Append(",@ReorderQtyMin=" + CommonLogic.IIF(txtreordermin.Text != "", txtreordermin.Text, "null"));
            sqlInsert.Append(",@MaximumStockLevel=" + CommonLogic.IIF(txtMaximumStockLevel.Text != "", txtMaximumStockLevel.Text, "null"));
            sqlInsert.Append(",@MinimumStockLevel=" + CommonLogic.IIF(txtMinimumStockLevel.Text != "", txtMinimumStockLevel.Text, "null"));

            /* MMGeneralPlantData related Parameters*/
            sqlInsert.Append(",@MinShelfLifeinDays=" + CommonLogic.IIF(txtMinShelfLife.Text != "", txtMinShelfLife.Text, "null"));
            sqlInsert.Append(",@TotalShelfLifeinDays=" + CommonLogic.IIF(txtTotalShelfLife.Text != "", txtTotalShelfLife.Text, "null"));



            /*  MMAccounting Table related Parameters*/
            sqlInsert.Append(",@StandardPrice=" + CommonLogic.IIF(txtStandardPrice.Text != "", DB.SQuote(txtStandardPrice.Text), "null"));
            sqlInsert.Append(",@UoMID=" + CommonLogic.IIF((Request.Form[hifUoM.UniqueID] != "" && Request.Form[hifUoM.UniqueID] != "0"), Request.Form[hifUoM.UniqueID], "NULL"));


            //Data Capture Requirements /Others
            //sqlInsert.Append(",@MGroupID=" + CommonLogic.IIF(Request.Form[ hifMaterialGroup.UniqueID] != "", Request.Form[ hifMaterialGroup.UniqueID], "NULL"));
            sqlInsert.Append(",@MGroupID=NULL");
            sqlInsert.Append(",@ProductCategoryID=" + Request.Form[hifProductCategories.UniqueID]); // * *//


            // General Data
            sqlInsert.Append(",@TenantID=" + TenantID);
            sqlInsert.Append(",@CreatedBy=" + CreatedBy);
            sqlInsert.Append(",@IndustryID=" + CommonLogic.IIF(Request.Form[hdnIndustry.UniqueID] != "", Request.Form[hdnIndustry.UniqueID], "NULL"));


            //Get CatlogNumber
            MCatlogNumber = txtMfgPartNo.Text;

            //Check Catlog Number length
            if (txtMfgPartNo.Text.Length < 3 || txtMfgPartNo.Text.Length > 17)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('Minimum length of part no. is 3');", true);
                return;
            }

            //for Inserting New Record OR Inserting New Record through Copy option from Material Request List page
            if ((CommonLogic.QueryString("mid") == "") || (CommonLogic.QueryString("edittype") == "copy"))
            {
                sqlInsert.Append(",@RequestedBy=" + RequestedBy);
                sqlInsert.Append(",@IsAproved=1");

                bool IsCatlogNoCorrect = checkMCode(MCatlogNumber); // Check Material Code in DB

                if (IsCatlogNoCorrect)
                {
                    //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('Part Number already exists');", true);
                    resetError("Part Number already exists", true);
                    return;
                }
                else
                {
                    sqlInsert.Append(",@MCode=" + DB.SQuote(MCatlogNumber));
                }

                sqlInsert.Append(",@MCodeAlternative1=NULL");
                sqlInsert.Append(",@MCodeAlternative2=NULL");

                sqlInsert.Append(",@MaterialMasterID=0");
                sqlInsert.Append(",@IsActive=1");
                sqlInsert.Append(",@IsFirstEdit=0");
                sqlInsert.Append(",@LastEditedByID=0");
            }

                //for Updating the existing Record
            else
            {
                sqlInsert.Append(",@RequestedBy=" + RequestedBy);
                sqlInsert.Append(",@MCodeAlternative1=" + CommonLogic.IIF(txtMCodeAlternative.Text != "", DB.SQuote(txtMCodeAlternative.Text), "null"));
                sqlInsert.Append(",@MCodeAlternative2=" + CommonLogic.IIF(txtMCodeAlternative2.Text != "", DB.SQuote(txtMCodeAlternative2.Text), "null"));

                if (chkIsApproved.Checked && chkIsActive.Checked)
                {


                    sqlInsert.Append(",@IsAproved=" + Convert.ToInt32(chkIsApproved.Checked));
                    sqlInsert.Append(",@IsActive=" + Convert.ToInt32(chkIsActive.Checked));

                }
                else
                {

                    SOCheck = DB.GetSqlS("EXEC [dbo].[sp_GEN_CheckSO] @MaterialMasterID=" + CommonLogic.QueryStringNativeInt("mid") + ",@TenantID=" + TenantID);
                    POCheck = DB.GetSqlS("EXEC [dbo].[sp_GEN_CheckPO] @MaterialMasterID=" + CommonLogic.QueryStringNativeInt("mid") + ",@TenantID=" + TenantID);


                    if (SOCheck != "" || POCheck != "")
                    {
                        resetError("Cannot 'InActive', as this material is configured in" + " SO Numbers:" + SOCheck + " " + CommonLogic.IIF(POCheck != "", "PO Numbers :", "") + POCheck, true);
                        return;
                    }
                    else
                    {


                        if (DB.GetSqlN("select top 1 KitPlannerID AS N from MMT_KitPlanner where ParentMaterialMasterID=" + CommonLogic.QueryStringNativeInt("mid") + "  and IsActive=1 and IsDeleted=0 and TenantID=" + TenantID) != 0)
                        {

                            resetError("Cannot 'InActive', as this is active in kit", true);
                            return;

                        }
                        if (DB.GetSqlN("select top 1 KitPlannerID AS N from MMT_KitPlannerDetail where ChildMMID=" + CommonLogic.QueryStringNativeInt("mid") + " AND IsActive=1 AND IsDeleted=0") != 0)
                        {
                            resetError("Cannot 'InActive', as this is active in kit", true);
                            return;
                        }

                        sqlInsert.Append(",@IsAproved=" + Convert.ToInt32(chkIsApproved.Checked));
                        sqlInsert.Append(",@IsActive=" + Convert.ToInt32(chkIsActive.Checked));

                    }

                }



                sqlInsert.Append(",@MCode=" + DB.SQuote(txtMfgPartNo.Text.Trim()));




                sqlInsert.Append(",@MaterialMasterID=" + CommonLogic.QueryString("mid"));

                if ((lthidIsFirstEdit.Text.Trim() == "0") || (lthidIsFirstEdit.Text.Trim() == ""))
                {
                    lthidIsFirstEdit.Text = "1";
                    sqlInsert.Append(",@IsFirstEdit=1");
                }
                else
                {
                    lthidIsFirstEdit.Text = (Convert.ToInt32(lthidIsFirstEdit.Text) + 1).ToString();
                    sqlInsert.Append(",@IsFirstEdit=" + lthidIsFirstEdit.Text);
                }

                //sqlInsert.Append(",@LastEditedByID=" + TenantID);//cp.UserID.ToString()

                sqlInsert.Append(",@LastEditedByID=" + cp.UserID.ToString());//cp.UserID.ToString()
            }

            sqlInsert.Append(",@Result = @Result OUTPUT");
            sqlInsert.Append("     Select N=@Result");

            try
            {

                int vResult = DB.GetSqlN(sqlInsert.ToString());

                mNewMMID = vResult;

                if (vResult > 0)
                {
                    UpdateMsp_LastClick(); // Update MaterialStorageParameter List when requested data successfully submitted.

                    //For successfull New Record Insertion and Copy record and Insertion
                    //Send email Aler to MM Admin
                    // resetError("Successfully submitted the request, but alert email was not sent <br/>", false);



                     Response.Redirect("MaterialMasterRequest.aspx?mid=" + vResult + "&status=success");

                    //ClientScript.RegisterStartupScript(GetType(), "Javascript", "javascript:UpsertIndustries(" + vResult + "); ", true);

                    return;

                }
                else if (vResult == 0)
                {
                    UpdateMsp_LastClick(); // Update MaterialStorageParameter List when requested data successfully submitted.

                    //   IList<HttpPostedFile> fileNames = fuItemPicture.PostedFiles;


                    if (fuItemPicture.HasFile)
                    {
                        String FileExtenion = Path.GetExtension(fuItemPicture.FileName);

                        if (FileExtenion == ".jpeg" || FileExtenion == ".jpg" || FileExtenion == ".gif" || FileExtenion == ".png")
                        {

                            //CommonLogic.UploadFile(TenantRootDir + DB.GetSqlS("select UniqueID AS S from GEN_Tenant where TenantID=" + TenantID) + MMTPath, fuItemPicture, MCatlogNumber + Path.GetExtension(fuItemPicture.FileName));
                            CommonLogic.UploadFile(TenantRootDir + hifTenant.Value + MMTPath, fuItemPicture, MCatlogNumber + Path.GetExtension(fuItemPicture.FileName));
                        }
                        else
                        {
                            resetError("Please upload a valid file  <br/>", true);
                            return;
                        }
                    }

                    //For successfull Edit and Updation of the Record
                    Response.Redirect("MaterialMasterList.aspx? statid=success", false);

                    resetError("Successfully submitted the request <br/>", false);

                    //ClientScript.RegisterStartupScript(GetType(), "Javascript", "javascript:UpsertIndustries(" + vResult + "); ", true);
                }

                else if (vResult == -3)
                {
                    //For Duplication of Material Master Code while (Insertion or Copy and Insertion)

                    resetError("Part number already exists<br/>", false);
                }

            }
            catch (Exception ex)
            {
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
                resetError("Part number already exists<br/>", true);
            }

        }

        // This Method Checks  MCode exits in DB or Not
        private bool checkMCode(string MCatlogNumber)
        {
            bool state = false;
            try
            {
                string sqlQuery = "select * from MMT_MaterialMaster where MCode=" + DB.SQuote(MCatlogNumber);
                IDataReader dr = DB.GetRS(sqlQuery);
                if (dr.Read()) // Check MCode
                    state = true;
            }
            catch (Exception ex)
            {
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
                resetError("Error submitting the data<br/>", true);
            }

            return state;
        }

        // This Method used for get selected checkbox items
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
            return selectedItems;

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

        // This method sends email notification to the allowed users
        private String SendEmailNotification(String vMatTokenNo, Int32 vFromUserID, String RequestType, String SubjectMatter)
        {

            return null;
        }


        protected void lnkButCancel_Click(object sender, EventArgs e)
        {

            if (CommonLogic.QueryString("edittype") == "copy")
            {
                Response.Redirect("MaterialMasterList.aspx");

                return;

            }

            if (CommonLogic.QueryStringNativeInt("mid") != 0)
            {
                //resetError("", true);
                if (txtIndustry.Text.Trim() != "")
                {
                    txtIndustry.Text = "";
                }

                return;
            }

            Response.Redirect("MaterialMasterList.aspx");

        }


        #endregion  ----------------- Basic Data  --------------------

        #region ------------------ Gen UoM -----------------------


        protected DataSet GENUoM_buildGridData()
        {
            string sql = ViewState["UoMListSQL"].ToString();
            DataSet ds = DB.GetDS(sql, false);

            return ds;
        }

        // Get UoM Grid data from DB
        protected void GENUoM_buildGridData(DataSet ds)
        {
            gvUoM.DataSource = ds.Tables[0];
            gvUoM.DataBind();
            ds.Dispose();


            // MODIFIED BY NARESH  01/04/2015 enable after editing done
            if (gvUoM.Rows.Count == 0)
            {
                ddlMeadureType.Enabled = true;
            }
            else if (gvUoM.Rows.Count == 1)
            {

                ddlMeadureType.Enabled = false;
            }
            else if (gvUoM.Rows.Count == 2)
            {
                if (ddlMeadureType.SelectedValue != "0")
                {
                    gvUoM.Rows[0].FindControl("chkIsDeleteRFItem").Visible = false;
                    gvUoM.Rows[0].Cells[4].Controls[0].Visible = false;
                }
                else
                {
                    if (gvUoM.EditIndex != 0)
                    {
                        gvUoM.Rows[0].FindControl("chkIsDeleteRFItem").Visible = false;
                        gvUoM.Rows[0].Cells[4].Controls[0].Visible = false;
                    }
                }
                ddlMeadureType.Enabled = false;
            }
            else
            {
                gvUoM.Rows[0].FindControl("chkIsDeleteRFItem").Visible = false;
                gvUoM.Rows[0].Cells[4].Controls[0].Visible = false;
                gvUoM.Rows[1].FindControl("chkIsDeleteRFItem").Visible = false;
                gvUoM.Rows[1].Cells[4].Controls[0].Visible = false;
                ddlMeadureType.Enabled = true;
            }


        }

        // This method fires when user clicks CreateNew Button In Uom Grid
        protected void lnkUoMConfig_Click(object sender, EventArgs e)
        {
            if (!cp.IsInAnyRoles("13"))
            {
                this.resetError("Only MMAdmin can access this details", true);
                return;
            }

            string uomTypeSql = "select count(UoMTypeID) as N from MMT_MaterialMaster_GEN_UoM where  IsActive=1 and IsDeleted=0 and   MaterialMasterID=" + CommonLogic.QueryString("mid");
            string genUomSql = "select count(UoMType) as N from GEN_UoMType where IsActive=1 and IsDeleted=0 ";

            int uomTypeCount = DB.GetSqlN(uomTypeSql);
            int genUomCount = DB.GetSqlN(genUomSql);

            if (uomTypeCount == genUomCount) // check all uom's are configured or not 
            {

                this.resetError("Cannot add new UoM. All UoM's are configured ", true);
                return;
            }
            else
            {

                ViewState["gvUoMIsInsert"] = true;

                StringBuilder sql = new StringBuilder(2500);

                // Insert new Item in the  MMT_MaterialMaster_GEN_UoM  Table
                //sql.Append("INSERT INTO MMT_MaterialMaster_GEN_UoM (MaterialMasterID) values (");
                //sql.Append(CommonLogic.QueryStringNativeInt("mid").ToString());
                //sql.Append(")");

                try
                {
                    //DB.ExecuteSQL(sql.ToString());

                    gvUoM.EditIndex = gvUoM.Rows.Count;
                    gvUoM.PageIndex = 0;

                    // gvUoM.EditIndex = 0;

                    DataSet dsUoM = this.GENUoM_buildGridData();
                    DataRow row = dsUoM.Tables[0].NewRow();
                    row["MaterialMaster_UoMID"] = 0;
                    //row["MaterialMasterID"] = CommonLogic.QueryStringNativeInt("mid").ToString();
                    row["UoMTypeID"] = 0;
                    row["UoMID"] = 0;
                    row["MaterialMasterID"] = CommonLogic.QueryStringNativeInt("mid");


                    dsUoM.Tables[0].Rows.Add(row);

                    this.GENUoM_buildGridData(dsUoM);

                    //  this.resetError("New UoM line item added", false);
                    ViewState["gvUoMIsInsert"] = true;

                    lnkUoMConfig.Visible = false;

                }
                catch (Exception ex)
                {
                    lnkUoMConfig.Visible = true;
                    CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
                    this.resetError("Error while inserting new UoM", true);

                }
            }
        }

        // This method fires when user clicks page index
        protected void gvUoM_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

            ViewState["gvUoMIsInsert"] = false;



            gvUoM.PageIndex = e.NewPageIndex;

            gvUoM.EditIndex = -1;

            this.GENUoM_buildGridData(this.GENUoM_buildGridData());

        }


        protected void gvUoM_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.DataItem == null)
                return;

            if ((e.Row.RowState & DataControlRowState.Edit) == DataControlRowState.Edit)
            {
                if (gvUoM.EditIndex == 0) // Show BUoM Only when user clicks BUoM edit button in UoMGrid
                {

                    DropDownList ddlvUoMType = (DropDownList)e.Row.FindControl("ddlvUoMType");
                    Literal vlthidgvUoMID = (Literal)e.Row.FindControl("lthidUoMTypeID");
                    string vlthidUoMType = ((Literal)e.Row.FindControl("ltUoMType")).Text;
                    LoadDropDown(ddlvUoMType, "Select UoMType,UoMTypeID from GEN_UoMType where IsActive=1 and IsDeleted=0 and UoMTypeID=1", "UoMType", "UoMTypeID", "Select UoM Type");
                    ddlvUoMType.SelectedValue = vlthidgvUoMID.Text;
                    DropDownList ddlUoMConfig = (DropDownList)e.Row.FindControl("ddlUoMConfig");
                    Literal vlthidUoMD = (Literal)e.Row.FindControl("lthidUoMD");
                    LoadDropDown(ddlUoMConfig, "Select * from GEN_UoM where IsActive=1 and IsDeleted=0 and MeasurementTypeID" + (ddlMeadureType.SelectedValue != "0" ? "=" + ddlMeadureType.SelectedValue + " and MeasurementID is not null " : " is null") + " order by UoM", "UoM", "UoMID", "Select UoM");
                    ddlUoMConfig.SelectedValue = vlthidUoMD.Text;

                    ddlvUoMType.Focus();
                }

                else if (gvUoM.EditIndex == 1) // Show MUoM Only when user clicks MUoM edit button in UoMGrid
                {

                    DropDownList ddlvUoMType = (DropDownList)e.Row.FindControl("ddlvUoMType");
                    Literal vlthidgvUoMID = (Literal)e.Row.FindControl("lthidUoMTypeID");
                    string vlthidUoMType = ((Literal)e.Row.FindControl("ltUoMType")).Text;
                    LoadDropDown(ddlvUoMType, "Select UoMType,UoMTypeID from GEN_UoMType where IsActive=1 and IsDeleted=0 and UoMTypeID=2", "UoMType", "UoMTypeID", "Select UoM Type");
                    ddlvUoMType.SelectedValue = vlthidgvUoMID.Text;
                    DropDownList ddlUoMConfig = (DropDownList)e.Row.FindControl("ddlUoMConfig");
                    Literal vlthidUoMD = (Literal)e.Row.FindControl("lthidUoMD");
                    //LoadDropDown(ddlUoMConfig, "Select * from GEN_UoM where IsActive=1 and IsDeleted=0", "UoM", "UoMID", "Select UoM");// comment by Prasad
                    LoadDropDown(ddlUoMConfig, "GetUOMList @preUoMID=" + ((Literal)gvUoM.Rows[0].FindControl("lthidUoMD")).Text + ",@UoMTypeID=" + (gvUoM.EditIndex + 1), "UoM", "UoMID", "Select UoM");
                    ddlUoMConfig.SelectedValue = vlthidUoMD.Text;
                    ddlvUoMType.Focus();

                }
                else
                {
                    IDataReader UomReader = DB.GetRS("select UoMTypeID from MMT_MaterialMaster_GEN_UoM where IsActive=1 and IsDeleted=0 and MaterialMasterID=" + CommonLogic.QueryStringNativeInt("mid") + "  and UoMTypeID is not null");

                    if (UomReader.Read()) // If  UoM's are already configured to MMID
                    {
                        DropDownList ddlvUoMType = (DropDownList)e.Row.FindControl("ddlvUoMType");
                        Literal vlthidgvUoMID = (Literal)e.Row.FindControl("lthidUoMTypeID");
                        string vlthidUoMType = ((Literal)e.Row.FindControl("ltUoMType")).Text;
                        LoadDropDown(ddlvUoMType, "Select UoMType,UoMTypeID from GEN_UoMType where IsActive=1 and IsDeleted=0 and UoMType=" + DB.SQuote(vlthidUoMType) + " or  UoMTypeID not in (select UoMTypeID from MMT_MaterialMaster_GEN_UoM where IsActive=1 and IsDeleted=0 and MaterialMasterID=" + CommonLogic.QueryStringNativeInt("mid") + "  and UoMTypeID is not null)", "UoMType", "UoMTypeID", "Select UoM Type");
                        ddlvUoMType.SelectedValue = vlthidgvUoMID.Text;
                        DropDownList ddlUoMConfig = (DropDownList)e.Row.FindControl("ddlUoMConfig");
                        Literal vlthidUoMD = (Literal)e.Row.FindControl("lthidUoMD");
                        //LoadDropDown(ddlUoMConfig, "Select * from GEN_UoM where IsActive=1 and IsDeleted=0", "UoM", "UoMID", "Select UoM");
                        LoadDropDown(ddlUoMConfig, "GetUOMList @preUoMID=" + ((Literal)gvUoM.Rows[1].FindControl("lthidUoMD")).Text + ",@UoMTypeID=" + (gvUoM.EditIndex + 1), "UoM", "UoMID", "Select UoM");
                        ddlUoMConfig.SelectedValue = vlthidUoMD.Text;
                        ddlvUoMType.Focus();
                    }
                    else // If No UoM's are configured to MMID
                    {
                        DropDownList ddlvUoMType = (DropDownList)e.Row.FindControl("ddlvUoMType");
                        Literal vlthidgvUoMID = (Literal)e.Row.FindControl("lthidUoMTypeID");
                        string vlthidUoMType = ((Literal)e.Row.FindControl("ltUoMType")).Text;
                        LoadDropDown(ddlvUoMType, "Select UoMType,UoMTypeID from GEN_UoMType where IsActive=1 and IsDeleted=0 and UoMTypeID=1", "UoMType", "UoMTypeID", "Select UoM Type");
                        ddlvUoMType.SelectedValue = vlthidgvUoMID.Text;
                        DropDownList ddlUoMConfig = (DropDownList)e.Row.FindControl("ddlUoMConfig");
                        Literal vlthidUoMD = (Literal)e.Row.FindControl("lthidUoMD");
                        //LoadDropDown(ddlUoMConfig, "Select * from GEN_UoM where IsActive=1 and IsDeleted=0", "UoM", "UoMID", "Select UoM");
                        LoadDropDown(ddlUoMConfig, "GetUOMList @preUoMID=" + ((Literal)gvUoM.Rows[0].FindControl("lthidUoMD")).Text + ",@UoMTypeID=" + (gvUoM.EditIndex + 1), "UoM", "UoMID", "Select UoM");
                        ddlUoMConfig.SelectedValue = vlthidUoMD.Text;
                        ddlvUoMType.Focus();
                    }

                    UomReader.Close();
                }

            }
        }

        // This method fires when user clicks edit , update (or) cancel buttons in UoM Grid
        protected void gvUoM_RowCommand(object sender, GridViewCommandEventArgs e)
        {


            if (e.CommandName == "DeleteItem")
            {
                ViewState["gvUoMIsInsert"] = false;
                gvUoM.EditIndex = -1;
                int iden = Localization.ParseNativeInt(e.CommandArgument.ToString());
                this.deleteRowPermE(iden);
            }

        }

        // This method fires when any row updated in UoM Grid
        protected void gvUoM_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            if (!cp.IsInAnyRoles("13"))
            {
                this.resetError("Only MMAdmin can access this details", true);
                return;
            }

            ViewState["gvUoMIsInsert"] = false;
            GridViewRow row = gvUoM.Rows[e.RowIndex];


            string ltMMT_GUoMID = ((Literal)row.FindControl("lthidMMT_MaterialMaster_UoMID")).Text;
            string UoMTypeID = ((DropDownList)row.FindControl("ddlvUoMType")).SelectedValue.ToString();
            string UoMID = ((DropDownList)row.FindControl("ddlUoMConfig")).SelectedValue.ToString();
            decimal UoMQty = 0;


            // Enable after editing done
            if (DB.GetSqlN("EXEC [dbo].[sp_MMT_CheckIsUoMConfigured] @MaterialMasterUoMID=" + ltMMT_GUoMID) != 0)
            {

                resetError("Cannot update, as UoM is already used in some transactions", true);
                return;

            }
            int Uoms= DB.GetSqlN("select count(UoMID) AS N from MMT_MaterialMaster_GEN_UoM where MaterialMasterID = " + ViewState["MMID"] +" and UoMID = " + UoMID);
         //   int uomcount = Convert.ToInt32(Uoms).ToString();
            if (DB.GetSqlN("select count(UoMID) AS N from MMT_MaterialMaster_GEN_UoM where MaterialMasterID = " + ViewState["MMID"] +" and UoMID = " + UoMID) !=0)
                    {
                resetError("Uom already Configured", true);
                return;

            }

            try
            {

                UoMQty = Convert.ToDecimal(((TextBox)row.FindControl("txtUoMQty")).Text);

            }
            catch (Exception)
            {
                resetError("Please enter valid qty. ", true);
                return;
            }


            if (UoMQty == 0) // UoM Qty always greater than 0
            {
                resetError("Qty. cannot be 0", true);
                return;

            }
            String SysConfgValue = "";

            // Get sysconfig value for UoM Is Decimal's allowed or not
            SysConfgValue = DB.GetSqlS("select top 1 SysConfigValue as N from SYS_SystemConfiguration SYS_SC left join SYS_SysConfigKey SYS_SCK on  SYS_SCK.SysConfigKeyID=SYS_SC.SysConfigKeyID where SYS_SCK.SysConfigKey='IsDecimalInventoryAllowed' and SYS_SCK.IsActive=1");

            if (row != null && ddlMeadureType.SelectedValue == "0")
            {

                if (gvUoM.EditIndex == 0) // Check BUoM Qty should be lessthan MUoM Qty
                {
                    if (gvUoM.Rows.Count > 2)
                    {
                        resetError("When Alt.UoM's are configured, cannot edit BUoM & Mpick", true);
                        return;
                    }


                }
                if (gvUoM.EditIndex == 1) 
                {
                   
                    try
                    {
                        string BUoMID = ((Literal)gvUoM.Rows[0].FindControl("lthidUoMD")).Text.ToString();

                        string MUoMID = ((DropDownList)row.FindControl("ddlUoMConfig")).SelectedValue.ToString();
                        
                        decimal BUoMQty = Convert.ToDecimal(((Literal)gvUoM.Rows[0].FindControl("ltUoMQty")).Text);

                        // If BUoM & MUoM'a are equal
                        if (BUoMID.Equals(MUoMID))
                        {
                            if (UoMQty > BUoMQty)  // Here UoMQty is Min. Pick. Qty
                            {
                                resetError("Quantity should be less than or equal to BUoM qty.", true);
                                return;
                            }

                            if ((BUoMQty / UoMQty) > 100) // Here UoMQty is Min. Pick. Qty
                            {
                                resetError("When BUoM & MUoM's are same, ( BUoM Qty./MUoM Qty.) value should be less than or equal to 100", true);
                                return;
                            }

                            if ((BUoMQty % UoMQty) != 0) // Here UoMQty is Min. Pick. Qty
                            {
                                resetError("When BUoM & MUoM's are same, MUoM qty. should be multiple of BUoM qty.", true);
                                return;
                            }

                        }
                        else
                        {
                            if (BUoMQty > 100)
                            {
                                resetError("When BUoM & MUoM's are different, BUoM qty. should be less than or equal to 100", true);
                                return;
                            }
                        }

                        
                       
                    }
                    catch (Exception ex)
                    {
                        resetError("Error while updating UoM details", true);
                        return;
                    }
                   
                }
                if (gvUoM.Rows.Count > 2)  // 
                {
                    if (gvUoM.EditIndex == 1)
                    {
                        resetError("Sorry, AltUoM's are already configured", true);
                        return;
                    }
                }

                if (gvUoM.EditIndex >= 2)
                {
                    try
                    {
                        string BUoMID = ((Literal)gvUoM.Rows[0].FindControl("lthidUoMD")).Text.ToString();

                        decimal BUoMQty = Convert.ToDecimal(((Literal)gvUoM.Rows[0].FindControl("ltUoMQty")).Text);

                        decimal MinPickQty = Convert.ToDecimal(((Literal)gvUoM.Rows[1].FindControl("ltUoMQty")).Text);
                        string MinPickUoMID = ((Literal)gvUoM.Rows[1].FindControl("lthidUoMD")).Text.ToString();

                        decimal AltUoMQty = Convert.ToDecimal(((TextBox)row.FindControl("txtUoMQty")).Text);
                        string AltUoMID = ((DropDownList)row.FindControl("ddlUoMConfig")).SelectedValue.ToString();


                        //if (MinPickUoMID.Equals(AltUoMID))
                        //{
                        //    resetError("MUoM & AltUoM's cannot be same ", true);
                        //    return;
                        //}


                        if (BUoMID.Equals(MinPickUoMID) && BUoMID.Equals(AltUoMID) && MinPickUoMID.Equals(AltUoMID))
                        {

                            if ((AltUoMQty % MinPickQty) != 0)
                            {
                                resetError("When MUoM & Alt.UoM's are same, Alt.UoM qty. should be multiple of MUoM qty.", true);
                                return;
                            }

                        }
                        else
                        if (BUoMID.Equals(AltUoMID))
                        {
                            if ((AltUoMQty % BUoMQty) != 0)
                            {
                                resetError("When BUoM & Alt.UoM's are same, Alt.UoM qty. should be multiple of BUoM qty.", true);
                                return;
                            }

                        }
                        



                    }
                    catch (Exception ex)
                    {
                        CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
                        resetError("Error while updating UoM details", true);
                        return;
                    }
                }

            }
            else if (row != null && ddlMeadureType.SelectedValue != "0")
            {

                if (gvUoM.EditIndex == 1) 
                {

                    try
                    {
                        string BUoMID = ((Literal)gvUoM.Rows[0].FindControl("lthidUoMD")).Text.ToString();

                        string MUoMID = ((DropDownList)row.FindControl("ddlUoMConfig")).SelectedValue.ToString();

                        decimal BUoMQty = Convert.ToDecimal(((Literal)gvUoM.Rows[0].FindControl("ltUoMQty")).Text);

                        string ltMMT_BUoMID = ((Literal)gvUoM.Rows[0].FindControl("lthidMMT_MaterialMaster_UoMID")).Text;

                        String SqlString = "declare @quantity decimal(18,2)=" + UoMQty + ";"
                                           +" select MBUoM.UoMQty/@quantity*ISNULL(BMTD.ConvesionValue,1)*IIF(ISNULL(BMPM.PowerOf,0)-ISNULL(MMPM.PowerOf,0)<0,1/POWER(convert(float,10),ISNULL(MMPM.PowerOf,0)-ISNULL(BMPM.PowerOf,0)),POWER(convert(float,10),ISNULL(BMPM.PowerOf,0)-ISNULL(MMPM.PowerOf,0))) AS N from GEN_UoM MUOM "
                                           +" left join MES_MatricPrefixMaster MMPM ON MMPM.MetricPrifixID=MUOM.MatricPrifixID "
                                           + " JOIN MMT_MaterialMaster_GEN_UoM MBUoM on MBUoM.MaterialMaster_UoMID=" + ltMMT_BUoMID //-- BUoMID 
                                           +" JOIN GEN_UoM BUOM ON BUOM.UoMID=MBUoM.UoMID "
                                           +" left join MES_MatricPrefixMaster BMPM ON BMPM.MetricPrifixID=BUOM.MatricPrifixID "
                                           +" LEFT join MES_MeasurementDetails BMTD on BMTD.MeasurementID=BUOM.MeasurementID  AND BMTD.ToMesurementID=MUOM.MeasurementID "
                                           + " WHERE   MUOM.UoMID=" + MUoMID; //--MinUoMID


                        if(DB.GetSqlNSingle(SqlString)>100)
                        {
                            resetError("Conversion value should be less than or equal to 100",true);
                            return;
                        }
                        


                        // If BUoM & MUoM'a are equal
                        if (BUoMID.Equals(MUoMID))
                        {

                            if (UoMQty > BUoMQty)  // Here UoMQty is Min. Pick. Qty
                            {
                                resetError("Quantity should be less than or equal to BUoM qty.", true);
                                return;
                            }

                            if ((BUoMQty / UoMQty) > 100) // Here UoMQty is Min. Pick. Qty
                            {
                                resetError("When BUoM & MUoM's are same, ( BUoM Qty./MUoM Qty.) value should be less than or equal to 100", true);
                                return;
                            }


                            if ((BUoMQty % UoMQty) !=0 ) // Here UoMQty is Min. Pick. Qty
                            {
                                resetError("When BUoM & MUoM's are same, MUoM qty. should be multiple of BUoM qty.", true);
                                return;
                            }



                        }
                        else
                        {
                            if (BUoMQty > 100)
                            {
                                resetError("When BUoM & MUoM's are different, BUoM qty. should be less than or equal to 100", true);
                                return;
                            }
                        }
                        

                    }
                    catch (Exception ex)
                    {
                        resetError("Error while updating UoM details", true);
                        return;
                    }

                }

                if (gvUoM.Rows.Count > 2)
                {
                    if (gvUoM.EditIndex == 1)
                    {
                        resetError("Sorry, AltUoM's are already configured", true);
                        return;
                    }
                }

                if (gvUoM.EditIndex >= 2)
                {
                    try
                    {
                        string BUoMID = ((Literal)gvUoM.Rows[0].FindControl("lthidUoMD")).Text.ToString();

                        decimal BUoMQty = Convert.ToDecimal(((Literal)gvUoM.Rows[0].FindControl("ltUoMQty")).Text);

                        decimal MinPickQty = Convert.ToDecimal(((Literal)gvUoM.Rows[1].FindControl("ltUoMQty")).Text);
                        string MinPickUoMID = ((Literal)gvUoM.Rows[1].FindControl("lthidUoMD")).Text.ToString();

                        decimal AltUoMQty = Convert.ToDecimal(((TextBox)row.FindControl("txtUoMQty")).Text);
                        string AltUoMID = ((DropDownList)row.FindControl("ddlUoMConfig")).SelectedValue.ToString();


                        //if (MinPickUoMID.Equals(AltUoMID))
                        //{
                        //    resetError("MUoM & AltUoM's cannot be same ", true);
                        //    return;
                        //}


                        if (BUoMID.Equals(MinPickUoMID) && BUoMID.Equals(AltUoMID) && MinPickUoMID.Equals(AltUoMID))
                        {

                            if ((AltUoMQty % MinPickQty) != 0)
                            {
                                resetError("When MUoM & Alt.UoM's are same, Alt.UoM qty. should be multiple of MUoM qty.", true);
                                return;
                            }

                        }else
                        if (BUoMID.Equals(AltUoMID))
                        {
                            if ((AltUoMQty % BUoMQty) != 0)
                            {
                                resetError("When BUoM & Alt.UoM's are same, Alt.UoM qty. should be multiple of BUoM qty.", true);
                                return;
                            }

                        }


                    }
                    catch (Exception ex)
                    {
                        CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
                        resetError("Error while updating UoM details", true);
                        return;
                    }
                }
            }

            StringBuilder sqlUoMDetails = new StringBuilder(2500);

            sqlUoMDetails.Append("EXEC  [dbo].[sp_MMT_UpsertMaterialMaster_GEN_UoM]    @MaterialMaster_UoMID=" + ltMMT_GUoMID + ",@TenantID=" + TenantID + ",@MaterialMasterID=" + CommonLogic.QueryStringNativeInt("mid") + ",@UoMTypeID=" + UoMTypeID + ",@UoMID=" + UoMID + ",@UoMQty=" + UoMQty + ",@CreatedBy=" + CreatedBy);

            try
            {
                DB.ExecuteSQL(sqlUoMDetails.ToString());
                this.resetError("Successfully Updated", false);
                gvUoM.EditIndex = -1;
                this.GENUoM_buildGridData(this.GENUoM_buildGridData());
                lnkUoMConfig.Visible = true;
            }
            catch (Exception ex)
            {
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
                lnkUoMConfig.Visible = false;
                this.resetError("Cannot update UoM details. Please check for required fields", true);
            }
            //}
            //else
            //{

            //    string ltMMT_GUoMID = ((Literal)row.FindControl("lthidMMT_MaterialMaster_UoMID")).Text;
            //    string UoMTypeID = ((DropDownList)row.FindControl("ddlvUoMType")).SelectedValue.ToString();
            //    string UoMID = ((DropDownList)row.FindControl("ddlUoMConfig")).SelectedValue.ToString();
            //    string txtQty = ((TextBox)row.FindControl("txtUoMQty")).Text;

            //    if (txtQty == "0")
            //    {
            //        resetError("Qty can't be 0", true);
            //        return;
            //    }

            //    StringBuilder sqlUoMDetails1 = new StringBuilder(2500);


            //    sqlUoMDetails1.Append("EXEC  [dbo].[sp_MMT_UpsertMaterialMaster_GEN_UoM]    @MaterialMaster_UoMID=" + ltMMT_GUoMID + ",@TenantID=" + TenantID + ",@MaterialMasterID=" + CommonLogic.QueryStringNativeInt("mid") + ",@UoMTypeID=" + UoMTypeID + ",@UoMID=" + UoMID + ",@UoMQty=" + txtQty + ",@CreatedBy=" + CreatedBy);

            //    StringBuilder sqlUoMDetails2 = new StringBuilder(2500);

            //    sqlUoMDetails2.Append("Insert into MMT_MaterialMaster_GEN_UoM(UoMTypeID,UoMID,UoMQty,MaterialMasterID,CreatedBy,TenantID) values(");
            //    sqlUoMDetails2.Append(2 + ",");
            //    sqlUoMDetails2.Append(UoMID + ",");
            //    sqlUoMDetails2.Append(txtQty + ",");
            //    sqlUoMDetails2.Append(CommonLogic.QueryStringNativeInt("mid") + ",");
            //    sqlUoMDetails2.Append(CreatedBy + ",");
            //    sqlUoMDetails2.Append(TenantID + ")");
            //    try
            //    {
            //        DB.ExecuteSQL(sqlUoMDetails1.ToString());
            //        DB.ExecuteSQL(sqlUoMDetails2.ToString());
            //        this.resetError("Successfully Updated", false);
            //        gvUoM.EditIndex = -1;
            //        this.GENUoM_buildGridData(this.GENUoM_buildGridData());
            //        lnkUoMConfig.Visible = true;
            //    }
            //    catch (Exception ex)
            //    {
            //        lnkUoMConfig.Visible = false;
            //        this.resetError("Cannot update UoM details. Please check for required fields", true);
            //    }


            //}





        }

        // This method fires when RowCancelingEdit in UoM Grid
        protected void gvUoM_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {

            if (Localization.ParseBoolean(ViewState["gvUoMIsInsert"].ToString()))
            {
                GridViewRow row = gvUoM.Rows[e.RowIndex];
                if (row != null)
                {
                    int iden = Convert.ToInt32(((Literal)row.FindControl("lthidMMT_MaterialMaster_UoMID")).Text);
                    deleteRowPermE(iden);
                    lnkUoMConfig.Visible = true;
                }
            }

            ViewState["gvUoMIsInsert"] = false;

            gvUoM.EditIndex = -1;
            this.GENUoM_buildGridData(this.GENUoM_buildGridData());
            lnkUoMConfig.Visible = true;


        }

        // This method fires when any row Editing in UoM Grid
        protected void gvUoM_RowEditing(object sender, GridViewEditEventArgs e)
        {
            ViewState["gvUoMIsInsert"] = false;

            gvUoM.EditIndex = e.NewEditIndex;

            this.GENUoM_buildGridData(this.GENUoM_buildGridData());

            lnkUoMConfig.Visible = false;

        }

        // This method fires when user clicks  delete button 
        protected void lnkDeleteRFItem_Click(object sender, EventArgs e)
        {
            if (!cp.IsInAnyRoles("13"))
            {
                this.resetError("Only MMAdmin can access this details", true);
                return;
            }


            if (DB.GetSqlN("select MaterialMasterID AS N from MFG_RoutingHeader_Revision MFG_RV JOIN MMT_MaterialMaster_Revision MMT_MR ON MMT_MR.MaterialMasterRevisionID=MFG_RV.MaterialMasterRevisionID AND MMT_MR.IsActive=1 AND MMT_MR.IsDeleted=0 where MMT_MR.MaterialMasterID=" + CommonLogic.QueryString("mid") + " AND MMT_MR.IsActive=1 AND MMT_MR.IsDeleted=0") != 0)
            {
                resetError("Cannot delete, as this material is in use ", true);
                return;
            }


            string rfidIDs = "";

            bool chkBox = false;

            StringBuilder sqlDeleteString = new StringBuilder();
            sqlDeleteString.Append("BEGIN TRAN ");



            //'Navigate through each row in the GridView for checkbox items
            foreach (GridViewRow gv in gvUoM.Rows)
            {

                CheckBox isDelete = (CheckBox)gv.FindControl("chkIsDeleteRFItem");

                if (isDelete == null)
                    return;

                // Concatenate GridView items with comma for SQL Delete
                rfidIDs = ((Literal)gv.FindControl("lthidMMT_MaterialMaster_UoMID")).Text.ToString();

                if (isDelete.Checked)
                {
                    if (DB.GetSqlN("EXEC [dbo].[sp_MMT_CheckIsUoMConfigured] @MaterialMasterUoMID=" + rfidIDs) != 0)
                    {
                        chkBox = false;

                        resetError("Cannot delete, as UoM is already used in some transactions", true);

                        return;


                    }
                    else
                    {

                        if (gv.RowIndex == 0)
                        {
                            if (gvUoM.Rows.Count == 1)
                            {
                                chkBox = true;
                            }
                            else
                            {
                                resetError("Please delete MinPick before deleting BUoM ", true);
                                chkBox = false;
                                break;
                            }
                        }
                        else
                            if (gv.RowIndex == 1)
                            {

                                if (gvUoM.Rows.Count > 2)
                                {
                                    resetError("Please delete AltUoM's before deleting MinPicK", true);
                                    break;
                                }
                                else
                                {
                                    chkBox = true;
                                }
                            }
                            else
                                chkBox = true;
                    }






                    if (rfidIDs != "")
                        sqlDeleteString.Append(" Update MMT_MaterialMaster_GEN_UoM set IsDeleted=1 Where MaterialMaster_UoMID=" + rfidIDs + ";");

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
                resetError("Error while deleting", true);
            }

            this.GENUoM_buildGridData(this.GENUoM_buildGridData());


        }


        // This method fires when any item delete from UoM Grid
        protected void deleteRowPermE(int iden)
        {
            StringBuilder sql = new StringBuilder(2500);
            sql.Append("DELETE from MMT_MaterialMaster_GEN_UoM where MaterialMaster_UoMID=" + iden.ToString());
            try
            {
                DB.ExecuteSQL(sql.ToString());
                this.GENUoM_buildGridData(this.GENUoM_buildGridData());

            }
            catch (Exception ex)
            {
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
                this.resetError("Error while deleting UoM details", true);
            }
        }



        public double IsMultipleofMinPick(decimal altQty, decimal minpickQty)
        {

            string vAltQty = "";
            string vMinpickQty = "";
            double result = 0;
            try
            {
                bool isIntegerAltQty = (decimal)((int)altQty) == altQty;
                bool isIntegerMinpickQty = (decimal)((int)minpickQty) == minpickQty;
                /*
                if (isIntegerAltQty && isIntegerMinpickQty)
                {
                    status = true;
                }else
                {*/

                if (!isIntegerAltQty)
                    if (altQty.ToString().Split('.').Length == 2)
                        vAltQty = altQty.ToString().Split('.')[1].Substring(0, altQty.ToString().Split('.')[1].Length);

                if (!isIntegerMinpickQty)
                    if (minpickQty.ToString().Split('.').Length == 2)
                        vMinpickQty = minpickQty.ToString().Split('.')[1].Substring(0, minpickQty.ToString().Split('.')[1].Length);

                double ddd = 0;
                if (!isIntegerAltQty)
                {
                    ddd = Convert.ToDouble(altQty) * Math.Pow(10, vAltQty.Length);
                }
                else
                {
                    ddd = Convert.ToDouble(altQty);
                }


                double sss = 0;
                if (!isIntegerMinpickQty)
                {
                    sss = Convert.ToDouble(minpickQty) * Math.Pow(10, vMinpickQty.Length);
                }
                else
                {
                    sss = Convert.ToDouble(minpickQty);
                }


                //result = ( ( Convert.ToDouble(altQty) * Math.Pow(10, vAltQty.Length)) % (Convert.ToDouble(minpickQty) * Math.Pow(10, vMinpickQty.Length)));

                result = ddd % sss;

                // }

                return result;

            }
            catch (Exception ex)
            {
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
                return result;
            }


        }



        #endregion ---------------- Gen UoM  ----------------------------

        #region -------------- start MSP CheckBox List   ---------------------------

        // This method Loads MaterialStorageParameter CheckBox List
        protected void LoadMspCheckBoxList()
        {

            // Get data from MaterialStorageParameter
            IDataReader rsChkList = DB.GetRS("select ParameterName,MaterialStorageParameterID from  MMT_MaterialStorageParameter where IsActive=1 AND IsDeleted=0");

            while (rsChkList.Read())
            {
                mspChkBoxList.Items.Add(new ListItem(rsChkList["ParameterName"].ToString(), rsChkList["MaterialStorageParameterID"].ToString()));
                mspIsRequiredCheckBoxList.Items.Add(new ListItem("IsRequired", rsChkList["MaterialStorageParameterID"].ToString()));
            }
            rsChkList.Close();

        }


        // This method fires when ddlMTypeID SelectedIndexChanged
        protected void ddlMTypeID_SelectedIndexChanged(object sender, EventArgs e)
        {
            hifMtypeIDp.Value = ddlMTypeID.SelectedValue;

            LoadMspList(Convert.ToInt32(ddlMTypeID.SelectedValue));

            if (CommonLogic.QueryString("mid") != "")
            {

                if (ddlMTypeID.SelectedValue == "8" || ddlMTypeID.SelectedValue == "9")
                {
                    pnlRvsnHistory.Visible = true;
                    mmRevision.Visible = true;
                }
                else
                {
                    pnlRvsnHistory.Visible = false;
                    mmRevision.Visible = false;
                }
            }

            atcStorageConditionID.Focus();
            Page.Validate();
        }

        // This method Loads MaterialStorageParameter types from DB
        protected void LoadMspList(int vMTypeID)
        {
            IDataReader mspReader = DB.GetRS("select MMT_Msp.MaterialStorageParameterID,ParameterName from MMT_MaterialStorageParameter MMT_Msp   left join MMT_MType_MMT_MaterialStorageParameter MMT_M_Msp on MMT_M_Msp.MaterialStorageParameterID=MMT_Msp.MaterialStorageParameterID  AND MMT_M_Msp.IsActive=1 AND MMT_M_Msp.IsDeleted=0  where   MMT_Msp.IsActive=1 AND MMT_Msp.IsDeleted=0 AND MMT_M_Msp.MTypeID=" + vMTypeID);
            int count = mspChkBoxList.Items.Count;
            int vIsRequiredCount = mspIsRequiredCheckBoxList.Items.Count;
            for (int i = 0; i < count; i++)
            {
                mspChkBoxList.Items[i].Selected = false; //Disable checked list item
                mspChkBoxList.Items[i].Enabled = true;  // Enable unchecked list item
            }
            for (int i = 0; i < vIsRequiredCount; i++)
            {
                mspIsRequiredCheckBoxList.Items[i].Selected = false; //Disable checked list item
                mspIsRequiredCheckBoxList.Items[i].Enabled = true;  // Enable unchecked list item
            }


            while (mspReader.Read())
            {
                mspChkBoxList.Items.FindByValue(DB.RSFieldInt(mspReader, "MaterialStorageParameterID").ToString()).Selected = true; //Disable checked list item
                //mspChkBoxList.Items[DB.RSFieldInt(mspReader, "MaterialStorageParameterID") - 1].Enabled = false; // Enable unchecked list item

            }
            mspReader.Close();
        }

        //Load Msp's configured in MMList 
        protected void LoadMMMspList(string MMID, string MTypeID)
        {

            IDataReader mspReader = DB.GetRS("select MMT_MSP.ParameterName,MMT_M.MaterialStorageParameterID,MMT_M.IsRequired from MMT_MaterialMaster_MMT_MaterialStorageParameter MMT_M left join MMT_MaterialStorageParameter MMT_MSP on MMT_MSP.MaterialStorageParameterID=MMT_M.MaterialStorageParameterID where  MMT_MSP.IsActive=1 AND MMT_MSP.IsDeleted=0 AND  MaterialMasterID=" + MMID);
            int count = mspChkBoxList.Items.Count;
            for (int i = 0; i < count; i++)
            {
                mspChkBoxList.Items[i].Selected = false; //Disable checked list item
                mspChkBoxList.Items[i].Enabled = true;  // Enable unchecked list item
            }

            while (mspReader.Read())
            {
                mspChkBoxList.Items.FindByValue(DB.RSFieldInt(mspReader, "MaterialStorageParameterID").ToString()).Selected = true;
                //mspChkBoxList.Items[DB.RSFieldInt(mspReader, "MaterialStorageParameterID") - 1].Selected = true; // Enable unchecked Msp list item
                if (DB.RSFieldTinyInt(mspReader, "IsRequired") == 1)
                {
                    mspIsRequiredCheckBoxList.Items.FindByValue(DB.RSFieldInt(mspReader, "MaterialStorageParameterID").ToString()).Selected = true;
                    //mspIsRequiredCheckBoxList.Items[DB.RSFieldInt(mspReader, "MaterialStorageParameterID") - 1].Selected = true; // Enable unchecked Is Required list item
                }
            }

            mspReader.Close();

            mspReader.Close();

            //Load Default Msp's
            LoadDefaultMspList(MMID, MTypeID);
        }

        // Load default MSP's to the specific material
        protected void LoadDefaultMspList(string MMID, string MTypeID)
        {

            IDataReader mspReader = DB.GetRS("select MMT_Msp.ParameterName,MMT_Msp.MaterialStorageParameterID from MMT_MaterialStorageParameter MMT_Msp left join MMT_MType_MMT_MaterialStorageParameter MMT_M_Msp on MMT_M_Msp.MaterialStorageParameterID=MMT_Msp.MaterialStorageParameterID AND MMT_M_Msp.IsActive=1 AND MMT_M_Msp.IsDeleted=0 where  MMT_Msp.IsActive=1 AND MMT_Msp.IsDeleted=0 AND  MMT_M_Msp.MTypeID=" + MTypeID);
            int count = mspChkBoxList.Items.Count;
            /* for (int i = 0; i < count; i++)
             {
                 mspChkBoxList.Items[i].Selected = false; //Disable checked list item
                 mspChkBoxList.Items[i].Enabled = true;  // Enable unchecked list item
             }*/
            while (mspReader.Read())
            {
                mspChkBoxList.Items.FindByValue(DB.RSFieldInt(mspReader, "MaterialStorageParameterID").ToString()).Selected = true; //Disable checked list item
                // mspChkBoxList.Items[DB.RSFieldInt(mspReader, "MaterialStorageParameterID") - 1].Enabled = false; // Enable unchecked list item

            }

            mspReader.Close();

        }

        // This method Updates MaterialStorageParameter
        protected void UpdateMsp_LastClick()
        {
            SOCheck = DB.GetSqlS("EXEC [dbo].[sp_GEN_CheckSO] @MaterialMasterID=" + CommonLogic.QueryStringNativeInt("mid") + ",@TenantID=" + TenantID);
            POCheck = DB.GetSqlS("EXEC [dbo].[sp_GEN_CheckPO] @MaterialMasterID=" + CommonLogic.QueryStringNativeInt("mid") + ",@TenantID=" + TenantID);


            if (SOCheck != "" || POCheck != "")
            {
                resetError("Cannot update MSP'S, as this material is configured in" + " SO Numbers:" + SOCheck + " " + CommonLogic.IIF(POCheck != "", "PO Numbers :", "") + POCheck, true);
                return;
            }

            string MSPIDs = "";
            foreach (ListItem listItem in mspChkBoxList.Items) // Get data from CheckBox List
            {

                if (listItem.Selected)
                {
                    MSPIDs += listItem.Value + ",";
                }

            }
            string IsRequiredMSPIDs = "";

            foreach (ListItem listItem in mspIsRequiredCheckBoxList.Items) // Get data from CheckBox List
            {

                if (listItem.Selected)
                {
                    IsRequiredMSPIDs += listItem.Value + ","; // Is Required Msp ID's
                }

            }

            // When Item is Cheked
            try
            {
                StringBuilder sqlMspUpsert = new StringBuilder();

                sqlMspUpsert.Append("EXEC [dbo].[sp_MMT_UpsertMaterialMaster_MaterialStorageParameter]      ");
                sqlMspUpsert.Append("@MaterialMasterID=" + ((CommonLogic.QueryStringNativeInt("mid") != 0 && CommonLogic.QueryString("edittype") != "copy") ? CommonLogic.QueryStringNativeInt("mid") : mNewMMID));
                sqlMspUpsert.Append(",@MaterialStorageParameterIDs=" + DB.SQuote(MSPIDs));
                sqlMspUpsert.Append(",@TenantID=" + TenantID);
                sqlMspUpsert.Append(",@CreatedBy=" + CreatedBy);
                sqlMspUpsert.Append(",@IsRequiredMSPIDs=" + DB.SQuote(IsRequiredMSPIDs));


                DB.ExecuteSQL(sqlMspUpsert.ToString()); // Execute SP

                resetError("Successfully Updated", false);

                sqlMspUpsert.Clear(); // Clear Data in sqlMspUpsert

            }
            catch (Exception ex)
            {
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
                resetError("Error while updating MSP's", true);
            }
        }


        #endregion ---------------- start MSP CheckBox List  ------------------------

        #region ----------  Quality Parameters  -----------------------------

        // Get QC parameters based on part number
        protected DataSet gvQCParameters_buildGridData()
        {
            string sql = ViewState["QCParametersSQL"].ToString();
            DataSet ds = DB.GetDS(sql, false);

            return ds;
        }

        // Bind QC parameters to grid view
        protected void gvQCParameters_buildGridData(DataSet ds)
        {
            gvQCParameters.DataSource = ds.Tables[0];
            gvQCParameters.DataBind();
            ds.Dispose();
        }

        // This method fires when user clicks add qc parameters button
        protected void lnkAddQCParameters_Click(object sender, EventArgs e)
        {
            ViewState["gvQCIsInsert"] = false;
            StringBuilder sql = new StringBuilder(2500);

            try
            {
                gvQCParameters.EditIndex = 0;
                gvQCParameters.PageIndex = 0;
                DataSet dsParent = this.gvQCParameters_buildGridData();
                DataRow row = dsParent.Tables[0].NewRow();
                row["MaterialMaster_QualityParameterID"] = 0;
                row["IsRequired"] = 0;
                dsParent.Tables[0].Rows.InsertAt(row, 0);
                this.gvQCParameters_buildGridData(dsParent);
                //  this.resetError("New QC line item added", false);
                ViewState["RoutQCIsInsert"] = true;

            }
            catch (Exception ex)
            {
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
                this.resetError("Error while inserting new QC line item", true);
            }
        }

        protected void gvQCParameters_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            ViewState["gvQCIsInsert"] = false;



            gvQCParameters.PageIndex = e.NewPageIndex;

            gvQCParameters.EditIndex = -1;

            this.gvQCParameters_buildGridData(this.gvQCParameters_buildGridData());
        }

        protected void gvQCParameters_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.DataItem == null)
                return;

            if ((e.Row.RowState & DataControlRowState.Edit) == DataControlRowState.Edit)
            {
                TextBox txtMaxTolerance = ((TextBox)e.Row.FindControl("txtMaxTolerance"));
                TextBox txtMinTolerance = ((TextBox)e.Row.FindControl("txtMinTolerance"));
                string ltQCParameterName = ((TextBox)e.Row.FindControl("txtQCParameter")).Text;

                if (ltQCParameterName == "Remarks")
                {
                    txtMaxTolerance.Style.Add("display", "none");
                    txtMinTolerance.Style.Add("display", "none");
                }
                else
                {
                    txtMaxTolerance.Style.Add("display", "inline");
                    txtMinTolerance.Style.Add("display", "inline");
                }
            }
        }

        protected void gvQCParameters_RowEditing(object sender, GridViewEditEventArgs e)
        {
            ViewState["gvQCIsInsert"] = false;

            gvQCParameters.EditIndex = e.NewEditIndex;

            this.gvQCParameters_buildGridData(this.gvQCParameters_buildGridData());
        }

        // This method fires when user updates any qc parameter
        protected void gvQCParameters_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {

            ViewState["gvQCIsInsert"] = false;
            GridViewRow row = gvQCParameters.Rows[e.RowIndex];

            if (row != null)
            {
                /*
                SOCheck = DB.GetSqlS("EXEC [dbo].[sp_GEN_CheckSO] @MaterialMasterID=" + CommonLogic.QueryStringNativeInt("mid") + ",@TenantID=" + TenantID);
                POCheck = DB.GetSqlS("EXEC [dbo].[sp_GEN_CheckPO] @MaterialMasterID=" + CommonLogic.QueryStringNativeInt("mid") + ",@TenantID=" + TenantID);


                if (SOCheck != "" || POCheck != "")
                {
                    resetError("couldn't update because this material configured in" + CommonLogic.IIF(SOCheck != "", "SONumbers :", "") + SOCheck + " " + CommonLogic.IIF(POCheck != "", "PONumbers :", "") + POCheck, true);
                    return;
                }*/

                TextBox txtMaxTolerance = ((TextBox)row.FindControl("txtMaxTolerance"));
                TextBox txtMinTolerance = ((TextBox)row.FindControl("txtMinTolerance"));

                string vParameterName = ((TextBox)row.FindControl("txtQCParameter")).Text;
                string vMMT_QualityParameterID = ((Literal)row.FindControl("lthidMaterialMaster_QualityParameterID")).Text;
                string vIsRequired = ((CheckBox)row.FindControl("chkIsRequired")).Checked == true ? "1" : "0";




                string vtxtMinValue = ((TextBox)row.FindControl("txtMinTolerance")).Text == "" ? "0" : ((TextBox)row.FindControl("txtMinTolerance")).Text;
                string vtxtMaxValue = ((TextBox)row.FindControl("txtMaxTolerance")).Text == "" ? "0" : ((TextBox)row.FindControl("txtMaxTolerance")).Text;

                decimal vMinTollerance = Convert.ToDecimal(vtxtMinValue);
                decimal vMaxTollerance = Convert.ToDecimal(vtxtMaxValue);


                int vQualityParameterID = DB.GetSqlN("select top 1 QualityParameterID AS N from  QCC_QualityParameters where IsActive=1 AND IsDeleted=0 AND ParameterName=" + DB.SQuote(vParameterName));

                if (vQualityParameterID == 0)
                {
                    this.resetError("Quality Parameter does not exist", true);
                    return;

                }

                if (vQualityParameterID == 21)
                {
                    vtxtMinValue = "0";
                    vtxtMaxValue = "0";
                }

                if (DB.GetSqlN("select QualityParameterID AS N from QCC_QualityParameters where ( ParameterDataTypeID=2 OR ParameterDataTypeID=1 ) AND  QualityParameterID=" + vQualityParameterID) != 0)
                {
                    if (vMaxTollerance == 0 || vMaxTollerance == 0)
                    {
                        this.resetError("Tolerance value cannot be 0 or empty", true);
                        if (vQualityParameterID != 21)
                        {
                            txtMaxTolerance.Style.Add("display", "inline");
                            txtMinTolerance.Style.Add("display", "inline");
                        }
                        return;
                    }

                    if (vMaxTollerance < vMinTollerance)
                    {
                        this.resetError(" Max. tolerance value should be greater than min. tolerance value", true);
                        if (vQualityParameterID != 21)
                        {
                            txtMaxTolerance.Style.Add("display", "inline");
                            txtMinTolerance.Style.Add("display", "inline");
                        }
                        return;
                    }

                }


                StringBuilder sql = new StringBuilder(2500);



                sql.Append("EXEC  [dbo].[sp_MMT_UpsertMaterialMaster_QualityParameters]   ");
                sql.Append("@MaterialMaster_QualityParameterID=" + vMMT_QualityParameterID);
                sql.Append(",@MaterialMasterID=" + CommonLogic.QueryString("mid"));
                sql.Append(",@QualityParameterID=" + vQualityParameterID);
                sql.Append(",@IsRequired=" + vIsRequired);
                sql.Append(",@Cratedby=" + cp.UserID);
                sql.Append(",@MinTolerance=" + (vtxtMinValue != "0" ? DB.SQuote(vtxtMinValue) : "0.00"));
                sql.Append(",@MaxTolerance=" + (vtxtMaxValue != "0" ? DB.SQuote(vtxtMaxValue) : "0.00"));



                try
                {
                    DB.ExecuteSQL(sql.ToString());

                    gvQCParameters.EditIndex = -1;

                    this.gvQCParameters_buildGridData(this.gvQCParameters_buildGridData());

                    this.resetError("Successfully Updated", false);


                }
                catch (SqlException sqlex)
                {
                    CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), sqlex.Source, sqlex.Message, sqlex.StackTrace);

                    if (sqlex.ErrorCode == -2146232060)
                    {
                        this.resetError("Parameter name already exists", true);

                        return;
                    }
                }
                catch (Exception ex)
                {
                    CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
                    this.resetError("Error while updating QC parameters", true);

                    return;
                }

            }






        }

        protected void gvQCParameters_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {

            if (Localization.ParseBoolean(ViewState["gvQCIsInsert"].ToString()))
            {
                GridViewRow row = gvQCParameters.Rows[e.RowIndex];
                if (row != null)
                {
                    int iden = Convert.ToInt32(((Literal)row.FindControl("lthidMaterialMaster_QualityParameterID")).Text);
                    deleteQCRow(iden);

                }
            }

            ViewState["gvQCIsInsert"] = false;

            gvQCParameters.EditIndex = -1;
            this.gvQCParameters_buildGridData(this.gvQCParameters_buildGridData());
        }

        protected void gvQCParameters_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }

        // This method fires when user deletes any qc parameter
        protected void lnkQCDelete_Click(object sender, EventArgs e)
        {


            string rfidIDs = "";

            bool chkBox = false;

            StringBuilder sqlDeleteString = new StringBuilder();
            sqlDeleteString.Append("BEGIN TRAN ");

            // Check if material is configured in PO/SO
            SOCheck = DB.GetSqlS("EXEC [dbo].[sp_GEN_CheckSO] @MaterialMasterID=" + CommonLogic.QueryStringNativeInt("mid") + ",@TenantID=" + TenantID);
            POCheck = DB.GetSqlS("EXEC [dbo].[sp_GEN_CheckPO] @MaterialMasterID=" + CommonLogic.QueryStringNativeInt("mid") + ",@TenantID=" + TenantID);


            //'Navigate through each row in the GridView for checkbox items
            foreach (GridViewRow gv in gvQCParameters.Rows)
            {

                CheckBox isDelete = (CheckBox)gv.FindControl("chkChildIsDelete");

                if (isDelete == null)
                    return;

                if (isDelete.Checked)
                {
                    // As material is configured in PO/SO user cannot delete 
                    if (SOCheck != "" || POCheck != "")
                    {
                        chkBox = false;
                        resetError("Cannot delete, as this material is configured in " + CommonLogic.IIF(POCheck != "", "PO Numbers :", "") + POCheck, true);

                        break;


                    }
                    else
                    {

                        chkBox = true;

                    }


                    // Concatenate GridView items with comma for SQL Delete
                    rfidIDs = ((Literal)gv.FindControl("lthidMaterialMaster_QualityParameterID")).Text.ToString();



                    if (rfidIDs != "")
                        sqlDeleteString.Append(" DELETE from MMT_MaterialMaster_QualityParameters where MaterialMaster_QualityParameterID=" + rfidIDs + ";");

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
                resetError("Error while deleting line items", true);
            }

            this.gvQCParameters_buildGridData(this.gvQCParameters_buildGridData());



        }

        protected void deleteQCRow(int iden)
        {
            StringBuilder sql = new StringBuilder(2500);
            sql.Append("DELETE from MMT_MaterialMaster_QualityParameters where MaterialMaster_QualityParameterID=" + iden.ToString());
            try
            {
                DB.ExecuteSQL(sql.ToString());
                this.gvQCParameters_buildGridData(this.gvQCParameters_buildGridData());

            }
            catch (Exception ex)
            {
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
                this.resetError("Error while deleting ", true);
            }
        }

        // Get check image
        public String Getimage(String IsChecked)
        {


            if (IsChecked != "")
            {
                if (Convert.ToBoolean(Convert.ToInt32(IsChecked)))
                    return "<img src=\"../Images/blue_menu_icons/check_mark.png\" />";
                else
                    return "";
            }
            else
            {
                return "";
            }


        }


        #endregion --------------- Quality Parameters   ---------------------

        #region ------------File Attachments -----------------

        // Load configured suppliers
        private void LoadSid()
        {

            if (mid != "")
            {
                String query = "select Sup.SupplierName,Sup.SupplierID from  MMT_MaterialMaster_Supplier MMT_Sup JOIN MMT_Supplier Sup  ON Sup.SupplierID=MMT_Sup.SupplierID AND Sup.isActive=1 AND Sup.isDeleted=0 where MMT_Sup.isActive=1 AND MMT_Sup.isDeleted=0 AND MMT_Sup.MaterialMasterID=" + mid;
                LoadDropDown(ddlsid, query, "SupplierName", "SupplierID", "Select Supplier");
            }
        }

        // Load supplier attachments dialog box
        protected void middailogbox()
        {

            if (mid != null && mid != "")
            {
                lblvieweattachment.Text = "<a style=\"text-decoration:none;\" href=\"javascript:openDialog(\'View Attachment Files\')\">   View Attachments  <span  class=\"space fa fa-external-link\"></span> </a>";
                //String loacalfolder = TenantRootDir + DB.GetSqlS("select UniqueID AS S from GEN_Tenant where TenantID=" + TenantID) + MMTPath + mid;
                String loacalfolder = TenantRootDir + hifTenant.Value + MMTPath + mid;

                //String midSavepath = System.Web.HttpContext.Current.Server.MapPath("~/") + loacalfolder;
                String midSavepath = System.Web.HttpContext.Current.Server.MapPath("~\\" + loacalfolder); ;
                String sidpath = "";

                trvmaterialattachment.Nodes[0].ChildNodes.Clear();

                TreeNode trnInternalOrder;
                TreeNode trnsubInternalOrder;
                trvmaterialattachment.Nodes[0].Text = txtMfgPartNo.Text;
                //DB.GetSqlS("select Mcode as S from MMT_MaterialMaster where MaterialMasterID = " + mid + " and isactive=1 and isdeleted=0");
                IDataReader rsGetSupplierList = DB.GetRS("select mmsup.SupplierID,sup.SupplierName from MMT_MaterialMaster_Supplier mmsup join MMT_Supplier sup on sup.SupplierID=mmsup.SupplierID  where MaterialMasterID=" + mid + " and sup.isactive=1 and sup.isdeleted=0  and mmsup.IsDeleted=0");

                while (rsGetSupplierList.Read())
                {
                    trnInternalOrder = new TreeNode();
                    trnInternalOrder.Expanded = false;
                    trnInternalOrder.Text = DB.RSField(rsGetSupplierList, "SupplierName");
                    sidpath = midSavepath + "//" + DB.RSFieldInt(rsGetSupplierList, "SupplierID");
                    String Attachmentname = "";
                    if (Directory.Exists(sidpath))
                    {
                        string[] Attachmentlist = Directory.GetFiles(sidpath);
                        foreach (String Attachment in Attachmentlist)
                        {
                            Attachmentname = Path.GetFileName(Attachment);
                            trnsubInternalOrder = new TreeNode();
                            trnsubInternalOrder.Text = Attachmentname;
                            trnsubInternalOrder.NavigateUrl = Attachmentname.EndsWith(".pdf") ? String.Format("DisplayPDF.aspx?sid={0}&mid={1}&filename={2}&tid={3}", DB.RSFieldInt(rsGetSupplierList, "SupplierID"), mid, Attachmentname,hifTenant.Value) : "../" + loacalfolder + "/" + DB.RSFieldInt(rsGetSupplierList, "SupplierID") + "/" + Attachmentname;
                            trnsubInternalOrder.Expanded = false;
                            trnInternalOrder.ChildNodes.Add(trnsubInternalOrder);

                        }
                    }
                    else
                    {

                        trnsubInternalOrder = new TreeNode();
                        trnsubInternalOrder.Expanded = false;
                        trnsubInternalOrder.Text = "Empty";
                        trnInternalOrder.ChildNodes.Add(trnsubInternalOrder);
                    }
                    trvmaterialattachment.Nodes[0].ChildNodes.Add(trnInternalOrder);
                }
            }
            else
                lblvieweattachment.Text = "";
        }


        #endregion ------------File Attachments -----------------

        #region ----------- Supplier Details ---------------

        // This method fires when user configure any new supplier
        protected void lnkAddSupplier_Click(object sender, EventArgs e)
        {

            ViewState["gvSupplierIsInsert"] = false;
            StringBuilder sql = new StringBuilder(2500);

            try
            {
                gvSupplierDetails.EditIndex = 0;
                gvSupplierDetails.PageIndex = 0;

                DataSet dsParent = this.gvSupplierDetails_buildGridData();
                DataRow row = dsParent.Tables[0].NewRow();
                row["MaterialMaster_SupplierID"] = 0;
                dsParent.Tables[0].Rows.InsertAt(row, 0);
                this.gvSupplierDetails_buildGridData(dsParent);
                // this.resetError("New Supplier line item added", false);
                ViewState["gvSupplierIsInsert"] = true;
                String query = "select Sup.SupplierName,Sup.SupplierID from  MMT_MaterialMaster_Supplier MMT_Sup JOIN MMT_Supplier Sup  ON Sup.SupplierID=MMT_Sup.SupplierID AND Sup.isActive=1 AND Sup.isDeleted=0 where MMT_Sup.isActive=1 AND MMT_Sup.isDeleted=0 AND MMT_Sup.MaterialMasterID=" + mid;
                LoadDropDown(ddlsid, query, "SupplierName", "SupplierID", "Select Supplier");
            }
            catch (Exception ex)
            {
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
                this.resetError("Error while inserting new Supplier ", true);
            }
        }

        protected DataSet gvSupplierDetails_buildGridData()
        {
            string sql = ViewState["SupplierListSQL"].ToString();
            DataSet ds = DB.GetDS(sql, false);

            return ds;

        }

        protected DataSet gvmspList_buildGridData()
        {
            string sql = ViewState["mspListSQL"].ToString();
            DataSet ds = DB.GetDS(sql, false);

            return ds;

        }

        protected void gvSupplierDetails_buildGridData(DataSet ds)
        {
            if (ds != null)
            {
                gvSupplierDetails.DataSource = ds.Tables[0];
                gvSupplierDetails.DataBind();
                ds.Dispose();


            }
        }


        protected void gvmspList_buildGridData(DataSet ds)
        {
            if (ds != null)
            {
                gvMsp.DataSource = ds.Tables[0];
                gvMsp.DataBind();
                ds.Dispose();
                
            }
        }

        protected void gvSupplierDetails_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            ViewState["gvSupplierIsInsert"] = false;

            gvSupplierDetails.PageIndex = e.NewPageIndex;

            gvSupplierDetails.EditIndex = -1;

            this.gvSupplierDetails_buildGridData(this.gvSupplierDetails_buildGridData());
        }

        protected void gvSupplierDetails_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvSupplierDetails.EditIndex = -1;

            this.gvSupplierDetails_buildGridData(this.gvSupplierDetails_buildGridData());
        }

        protected void gvSupplierDetails_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }

        protected void gvSupplierDetails_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow && e.Row.RowIndex == gvSupplierDetails.EditIndex)
            {
                string ltSupplierID = ((Literal)e.Row.FindControl("ltSupplierID")).Text;
                TextBox atcSupplierName = ((TextBox)e.Row.FindControl("atcSupplierName"));

                //SOCheck = DB.GetSqlS("EXEC [dbo].[sp_GEN_CheckSO] @MaterialMasterID=" + CommonLogic.QueryStringNativeInt("mid") + ",@TenantID="+TenantID);
                POCheck = DB.GetSqlS("EXEC [dbo].[sp_GEN_CheckPOForDelete] @MaterialMasterID=" + CommonLogic.QueryStringNativeInt("mid") + ",@SupplierIDs=" + DB.SQuote(ltSupplierID.ToString()));

                if (POCheck != "")
                {
                    atcSupplierName.Enabled = false;
                }
            }
        }

        protected void gvSupplierDetails_RowEditing(object sender, GridViewEditEventArgs e)
        {

            ViewState["gvSupplierIsInsert"] = false;

            gvSupplierDetails.EditIndex = e.NewEditIndex;

            this.gvSupplierDetails_buildGridData(this.gvSupplierDetails_buildGridData());

        }

        // This method fires when user updates any supplier
        protected void gvSupplierDetails_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {


            GridViewRow row = gvSupplierDetails.Rows[e.RowIndex];

            if (row != null)
            {

                /*
                
                SOCheck = DB.GetSqlS("EXEC [dbo].[sp_GEN_CheckSO] @MaterialMasterID=" + CommonLogic.QueryStringNativeInt("mid") + ",@TenantID=" + TenantID);
                POCheck = DB.GetSqlS("EXEC [dbo].[sp_GEN_CheckPO] @MaterialMasterID=" + CommonLogic.QueryStringNativeInt("mid") + ",@TenantID=" + TenantID);


                if ( POCheck != "")
                {
                    resetError("Cannot update, as this material is configured in" + CommonLogic.IIF(POCheck != "", "PO Numbers :", "") + POCheck, true);
                    return;
                }*/



                string ltMMT_SupID = ((Literal)row.FindControl("lthidMMT_MaterialMaster_SupplierID")).Text;

                String ltSupplierName = ((TextBox)row.FindControl("atcSupplierName")).Text;

                string txtInitialOrdQty = ((TextBox)row.FindControl("txtInitialOrdQty")).Text;

                string txtDeliveryTime = ((TextBox)row.FindControl("txtDeliveryTime")).Text;

                string txtUnitCost = ((TextBox)row.FindControl("txtUnitCost")).Text;
                string txtSupplierPartNumber = ((TextBox)row.FindControl("txtSupplierPartNumber")).Text;

                string txtCurrency1 = ((TextBox)row.FindControl("atcInvCurrency")).Text;



                decimal vInitialOrderQty = Convert.ToDecimal(txtInitialOrdQty == "" ? "0" : txtInitialOrdQty);
                decimal vDeliveryTime = Convert.ToDecimal(txtDeliveryTime == "" ? "0" : txtDeliveryTime);
                decimal vUnitCost = Convert.ToDecimal(txtUnitCost == "" ? "0" : txtUnitCost);



                int vSUPID = DB.GetSqlN("select SupplierID AS N from MMT_Supplier where IsActive=1 AND Isdeleted=0 AND SupplierName='" + ltSupplierName + "'");

                if (vSUPID == 0)
                {
                    resetError("This supplier doesn't exist", true);
                    return;
                }

                if (DB.GetSqlN("select MaterialMaster_SupplierID N from MMT_MaterialMaster_Supplier WHERE MaterialMasterID=" + CommonLogic.QueryString("mid") + " AND SupplierID=" + vSUPID + " AND MaterialMaster_SupplierID!=" + ltMMT_SupID + " AND IsDeleted=0") != 0)
                {
                    this.resetError("Supplier already exists", true);
                    return;
                }

                /*
                if (vInitialOrderQty == 0 || vDeliveryTime == 0 || vUnitCost == 0)
                {
                    resetError("The value can not be 0 or empty",true);
                    return;
                }*/
                int CurrencyID = 0;

                if (txtCurrency1 != "")
                {
                    CurrencyID = DB.GetSqlN("select CurrencyID AS N from GEN_Currency where IsActive=1 AND IsDeleted=0 AND Code+'" + "-" + "'+Currency='" + txtCurrency1 + "'");
                    if (CurrencyID == 0)
                    {
                        resetError("Currency does not exist", true);
                        return;
                    }
                }




                StringBuilder sqlSupDetails = new StringBuilder(2500);

                sqlSupDetails.Append("EXEC [dbo].[sp_MMT_UpsertMaterialMaster_Supplier]   ");
                sqlSupDetails.Append("@MaterialMaster_SupplierID=" + ltMMT_SupID);
                sqlSupDetails.Append(",@MaterialMasterID=" + CommonLogic.QueryString("mid"));
                sqlSupDetails.Append(",@SupplierID=" + vSUPID);
                sqlSupDetails.Append(",@TenantID=" + TenantID);
                sqlSupDetails.Append(",@ExpectedUnitCost=" + vUnitCost);
                sqlSupDetails.Append(",@PlannedDeliveryTime=" + vDeliveryTime);
                sqlSupDetails.Append(",@InitialOrderQuantity=" + vInitialOrderQty);
                sqlSupDetails.Append(",@CurrencyID=" + (CurrencyID == 0 ? "NULL" : CurrencyID.ToString()));
                sqlSupDetails.Append(",@CreatedBy=" + CreatedBy);
                sqlSupDetails.Append(",@SupplierPartNo=" + (txtSupplierPartNumber == "" ? "Null" : DB.SQuote(txtSupplierPartNumber)));


                try
                {


                    DB.ExecuteSQL(sqlSupDetails.ToString());
                    gvSupplierDetails.EditIndex = -1;

                    ViewState["gvSupplierIsInsert"] = false;

                    this.gvSupplierDetails_buildGridData(this.gvSupplierDetails_buildGridData());

                    LoadSid();
                    this.resetError("Successfully Updated", false);

                    ValidateTenant();
                }
                catch (SqlException sqlex)
                {
                    CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), sqlex.Source, sqlex.Message, sqlex.StackTrace);
                    if (sqlex.ErrorCode == -2146232060)
                    {
                        this.resetError("Supplier already exists", true);

                        return;
                    }
                }
                catch (Exception ex)
                {
                    CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
                    resetError("Error while updating supplier details", true);
                }
            }
            LoadSid();
            
        }

        // This method fires when user deletes any supplier
        protected void lnkSupDelete_Click(object sender, EventArgs e)
        {
            // Check if material is configured in PO/SO
            //SOCheck = DB.GetSqlS("EXEC [dbo].[sp_GEN_CheckSO] @MaterialMasterID=" + CommonLogic.QueryStringNativeInt("mid") + ",@TenantID=" + TenantID);
            //POCheck = DB.GetSqlS("EXEC [dbo].[sp_GEN_CheckPO] @MaterialMasterID=" + CommonLogic.QueryStringNativeInt("mid") + ",@TenantID=" + TenantID);


            //// As material is configured in PO/SO user cannot delete
            //if (SOCheck != "" || POCheck != "")
            //{
            //    resetError("Cannot delete, because this material is configured in " + " SO Numbers:" + SOCheck + " " + CommonLogic.IIF(POCheck != "", " PO Numbers :", "") + POCheck, true);
            //    return;
            //}

            bool chkBox = false;
            string rfidIDs = "";
            string SupplierIDs = "";

            StringBuilder sqlDeleteString = new StringBuilder();
            foreach (GridViewRow gv in gvSupplierDetails.Rows)
            {
                CheckBox isDelete = (CheckBox)gv.FindControl("chkIsDelete");

                if (isDelete == null)
                    return;

                if (isDelete.Checked)
                {
                    chkBox = true;
                    rfidIDs += ((Literal)gv.FindControl("lthidMMT_MaterialMaster_SupplierID")).Text.ToString() + ",";
                    SupplierIDs += ((Literal)gv.FindControl("ltSupplierID")).Text.ToString() + ",";
                }
            }

            if (chkBox)
            {

                SOCheck = DB.GetSqlS("EXEC [dbo].[sp_GEN_CheckSO] @MaterialMasterID=" + CommonLogic.QueryStringNativeInt("mid") + ",@TenantID="+TenantID);
                POCheck = DB.GetSqlS("EXEC [dbo].[sp_GEN_CheckPOForDelete] @MaterialMasterID=" + CommonLogic.QueryStringNativeInt("mid") + ",@SupplierIDs=" + DB.SQuote(SupplierIDs.ToString()));

                if (SOCheck != "" || POCheck != "")
                {
                    this.resetErrorForTenant("Cannot delete, as this Supplier is configured in  " + CommonLogic.IIF(SOCheck != "", "SO Numbers : ", "") + SOCheck + " " + CommonLogic.IIF(POCheck != "", "PO Numbers : ", "") + POCheck, true);
                    return;
                }

                sqlDeleteString.Append("UPDATE MMT_MaterialMaster_Supplier SET IsDeleted=1 WHERE MaterialMaster_SupplierID IN (" + rfidIDs.Substring(0, rfidIDs.LastIndexOf(",")) + ")");
            }

            try
            {
                DB.ExecuteSQL(sqlDeleteString.ToString());
                this.gvSupplierDetails_buildGridData(this.gvSupplierDetails_buildGridData());
                resetError("Successfully deleted the selected line items", false);
            }


            catch (Exception ex)
            {
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
                resetError("Error while deleting selected line items", true);
            }

            ValidateTenant();

            this.gvSupplierDetails_buildGridData(this.gvSupplierDetails_buildGridData());
            LoadSid();

        }

        protected void deleteSupplierRow(int iden)
        {
            StringBuilder sql = new StringBuilder(2500);
            sql.Append("DELETE from MMT_MaterialMaster_Supplier where MaterialMaster_SupplierID=" + iden.ToString());
            try
            {
                DB.ExecuteSQL(sql.ToString());
                this.gvSupplierDetails_buildGridData(this.gvSupplierDetails_buildGridData());

            }
            catch (Exception ex)
            {
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
                this.resetError("Error while deleting selected line items", true);
            }
        }

        #endregion ----------- Supplier Details ---------------

        #region -----------   MM Revision History      ---------------------


        // Load rivision history  from DB
        protected DataSet gvRvsnHistory_buildGridData()
        {
            string sql = ViewState["RvsnHistoryListSQL"].ToString();
            DataSet ds = DB.GetDS(sql, false);

            return ds;

        }

        protected void gvRvsnHistory_buildGridData(DataSet ds)
        {
            if (ds != null)
            {
                gvRvsnHistory.DataSource = ds.Tables[0];
                gvRvsnHistory.DataBind();
                ds.Dispose();

                mmRevision.Text = DB.GetSqlS("select top 1 Revision AS S from MMT_MaterialMaster_Revision where IsActive=1 AND IsDeleted=0 AND MaterialMasterID=" + CommonLogic.QueryString("mid") + "  order by  MaterialMasterRevisionID  desc ");
            }
        }

        // This method fires when user configure any new rivision
        protected void lnkAddRvsnHistory_Click(object sender, EventArgs e)
        {
            ViewState["gvRevisionIsInsert"] = false;
            StringBuilder sql = new StringBuilder(2500);

            try
            {
                gvRvsnHistory.EditIndex = 0;

                DataSet dsParent = this.gvRvsnHistory_buildGridData();
                DataRow row = dsParent.Tables[0].NewRow();
                row["MaterialMasterRevisionID"] = 0;
                dsParent.Tables[0].Rows.InsertAt(row, 0);
                this.gvRvsnHistory_buildGridData(dsParent);
                //this.resetError("New Revision line item added", false);
                ViewState["gvRevisionIsInsert"] = true;

            }
            catch (Exception ex)
            {
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
                this.resetError("Error while inserting new line item" + ex.ToString(), true);
            }

        }

        protected void gvRvsnHistory_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            ViewState["gvRevisionIsInsert"] = false;

            gvRvsnHistory.PageIndex = e.NewPageIndex;

            gvRvsnHistory.EditIndex = -1;

            this.gvRvsnHistory_buildGridData(this.gvRvsnHistory_buildGridData());
        }

        protected void gvRvsnHistory_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            /*
            if (Localization.ParseBoolean(ViewState["gvRevisionIsInsert"].ToString()))
            {
                GridViewRow row = gvRvsnHistory.Rows[e.RowIndex];
                if (row != null)
                {
                    int iden = Convert.ToInt32(((Literal)row.FindControl("lthidMaterialMasterRevisionID")).Text);
                    deleteRvsnRow(iden);

                }
            }

            ViewState["gvRevisionIsInsert"] = false;
            */
            gvRvsnHistory.EditIndex = -1;

            this.gvRvsnHistory_buildGridData(this.gvRvsnHistory_buildGridData());
        }

        protected void gvRvsnHistory_RowEditing(object sender, GridViewEditEventArgs e)
        {

            ViewState["gvRevisionIsInsert"] = false;

            gvRvsnHistory.EditIndex = e.NewEditIndex;

            this.gvRvsnHistory_buildGridData(this.gvRvsnHistory_buildGridData());

        }

        // This method fires when user updates any rivision
        protected void gvRvsnHistory_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {

            try
            {
                GridViewRow row = gvRvsnHistory.Rows[e.RowIndex];

                if (row != null)
                {



                    string lthidMaterialMasterRevisionID = ((Literal)row.FindControl("lthidMaterialMasterRevisionID")).Text;

                    String txtRevision = ((TextBox)row.FindControl("txtRevision")).Text;

                    string txtDescription = ((TextBox)row.FindControl("txtDescription")).Text;

                    string txtEffectiveDate = ((TextBox)row.FindControl("txtEffectiveDate")).Text;

                    string CurrDate = DateTime.Now.ToString("dd/MM/yyyy");


                    if (!DateGreaterOrEqual(DateTime.ParseExact(txtEffectiveDate.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture), DateTime.ParseExact(CurrDate, "dd/MM/yyyy", CultureInfo.InvariantCulture)))
                    {
                        resetError("Effective date should be greater than or equal to system date", true);
                        return;
                    }


                    String OldRevision = DB.GetSqlS("select top 1 Revision AS S from MMT_MaterialMaster_Revision where IsActive=1 AND IsDeleted=0 AND MaterialMasterRevisionID <> " + lthidMaterialMasterRevisionID + "  AND MaterialMasterID=" + CommonLogic.QueryString("mid") + " order by Revision desc");


                    int compareValue = 0;

                    compareValue = String.Compare(txtRevision, OldRevision, StringComparison.Ordinal);

                    if (compareValue <= 0)
                    {
                        resetError("'New Revision' must be greater than the previous revision. Previous revision:" + OldRevision, true);
                        return;
                    }


                    if (OldRevision != "")
                    {
                        if (lthidMaterialMasterRevisionID == "0")

                            //if (!DateGreaterOrEqual(ConvertToDateTime(txtEffectiveDate.Trim()), ConvertToDateTime(DB.GetSqlS("select top 1  convert(nvarchar(50),EffectiveDate,103)  AS S from MMT_MaterialMaster_Revision where IsActive=1 AND IsDeleted=0 AND MaterialMasterID=" + CommonLogic.QueryString("mid") + " order by Revision desc"))))
                            //{
                            //    resetError("Effective date should be greater than previous revision effective date", true);
                            //    return;
                            //}
                            if (!DateGreaterOrEqual(DateTime.ParseExact(txtEffectiveDate.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture), DateTime.ParseExact(DB.GetSqlS("select top 1  convert(nvarchar(50),EffectiveDate,103)  AS S from MMT_MaterialMaster_Revision where IsActive=1 AND IsDeleted=0 AND MaterialMasterID=" + CommonLogic.QueryString("mid") + " order by Revision desc"), "dd/MM/yyyy", CultureInfo.InvariantCulture)))
                            {
                                resetError("Effective date should be greater than previous revision effective date", true);
                                return;
                            }
                    }



                    if (DB.GetSqlN("EXEC [dbo].[sp_GEN_CheckColumnValueInAllTables]  @SearchFieldID=" + lthidMaterialMasterRevisionID + ",@ColumnName='MaterialMasterRevisionID',@TableName='MMT_MaterialMaster_Revision'") != 0)
                    {
                        this.resetError("Cannot update, as this revision is in use", true);
                        return;
                    }

                    /*
                    if (DB.GetSqlN("select MFG1.MaterialMasterRevisionID AS N from MFG_BOMHeader_Revision MFG1 ,MFG_ProductionOrderHeader MFG2 ,MFG_RoutingHeader_Revision MFG3 where MFG1.MaterialMasterRevisionID=" + lthidMaterialMasterRevisionID + " OR  MFG2.MaterialMasterRevisionID=" + lthidMaterialMasterRevisionID + " OR  MFG3.MaterialMasterRevisionID=" + lthidMaterialMasterRevisionID) != 0)
                    {
                    
                        this.resetError("Can not update,This Revision is in use", true);
                        return;
                    }*/


                    StringBuilder sqlSupDetails = new StringBuilder(2500);

                    sqlSupDetails.Append("EXEC [dbo].[SP_MMT_UpsertMaterialMaster_Revision]   ");
                    sqlSupDetails.Append("@MaterialMasterRevisionID=" + lthidMaterialMasterRevisionID);
                    sqlSupDetails.Append(",@MaterialMasterID=" + CommonLogic.QueryString("mid"));
                    sqlSupDetails.Append(",@Revision=" + DB.SQuote(txtRevision));
                    //sqlSupDetails.Append(",@EffectiveDate=" + DB.SQuote(ConvertToDateTime(txtEffectiveDate).Date.ToString("dd/MM/yyyy")));
                    sqlSupDetails.Append(",@EffectiveDate=" + (txtEffectiveDate != "" ? DB.SQuote( DateTime.ParseExact(txtEffectiveDate.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy")) : "NULL"));
                    sqlSupDetails.Append(",@Description=" + DB.SQuote(txtDescription));
                    sqlSupDetails.Append(",@CreatedBy=" + CreatedBy);

                    try
                    {


                        DB.ExecuteSQL(sqlSupDetails.ToString());

                        gvRvsnHistory.EditIndex = -1;

                        ViewState["gvRevisionIsInsert"] = false;

                        this.gvRvsnHistory_buildGridData(this.gvRvsnHistory_buildGridData());

                        this.resetError("Successfully Updated", false);


                    }
                    catch (SqlException sqlex)
                    {

                        CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), sqlex.Source, sqlex.Message, sqlex.StackTrace);
                        if (sqlex.ErrorCode == -2146232060)
                        {
                            this.resetError("Revision already exists", true);

                            return;
                        }
                    }
                    catch (Exception ex)
                    {
                        CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
                        resetError("Error while updating revision details", true);
                    }
                }
            }
            catch (Exception ex)
            {
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
                resetError("Error while updating revision details", true);
                return;

            }

        }

        private DateTime ConvertToDateTime(string strDateTime)
        {
            DateTime dtFinaldate; string sDateTime;
            try {
                //dtFinaldate = Convert.ToDateTime(strDateTime); 

                string[] sDate = strDateTime.Split('/');
                sDateTime = sDate[1] + '/' + sDate[0] + '/' + sDate[2];
                dtFinaldate = Convert.ToDateTime(sDateTime);
            }
            catch (Exception e)
            {
                string[] sDate = strDateTime.Split('/');
                sDateTime = sDate[1] + '/' + sDate[0] + '/' + sDate[2];
                dtFinaldate = Convert.ToDateTime(sDateTime);
            }
            return dtFinaldate;
        }

        private bool DateGreaterOrEqual(DateTime dt1, DateTime dt2)
        {
            return DateTime.Compare(dt1.Date, dt2.Date) >= 0;
        }

        // This method fires when user deletes any rivision
        protected void lnkRvsnDelete_Click(object sender, EventArgs e)
        {

            bool chkBox = false;

            StringBuilder sqlDeleteString = new StringBuilder();
            foreach (GridViewRow gv in gvRvsnHistory.Rows)
            {
                CheckBox isDelete = (CheckBox)gv.FindControl("chkIsDelete");

                if (isDelete == null)
                    return;

                if (isDelete.Checked)
                {
                    string rfidIDs = ((Literal)gv.FindControl("lthidMaterialMasterRevisionID")).Text.ToString();
                    chkBox = true;
                    if (rfidIDs != "")
                        sqlDeleteString.Append(" DELETE from MMT_MaterialMaster_Revision where MaterialMasterRevisionID=" + rfidIDs + ";");
                }
            }
            try
            {

                if (chkBox)
                {

                    DB.ExecuteSQL(sqlDeleteString.ToString());

                    this.gvRvsnHistory_buildGridData(this.gvRvsnHistory_buildGridData());

                    resetError("Successfully deleted the selected line items", false);

                }


            }
            catch (SqlException ex)
            {
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
                if (ex.ErrorCode == -2146232060)
                {
                    resetError("Cannot delete, as this revision is in use", true);
                    return;
                }
            }

            catch (Exception ex)
            {
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
                resetError("Error while deleting selected line items", true);
            }

            this.gvRvsnHistory_buildGridData(this.gvRvsnHistory_buildGridData());

        }

        protected void deleteRvsnRow(int iden)
        {
            StringBuilder sql = new StringBuilder(2500);
            sql.Append("DELETE from MMT_MaterialMaster_Revision where MaterialMasterRevisionID=" + iden.ToString());
            try
            {
                DB.ExecuteSQL(sql.ToString());
                this.gvRvsnHistory_buildGridData(this.gvRvsnHistory_buildGridData());

            }
            catch (Exception ex)
            {
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
                this.resetError("Error while deleting line items", true);
            }
        }


        #endregion -----------   MM Revision History      ---------------------

        #region ----------- Standard UoM Conversion ---------------

        // This method fires when user selects any measurement type
        protected void ddlMeadureType_SelectedIndexChanged(object sender, EventArgs e)
        {
            Build_FromMeasurement(ddlMeadureType.SelectedValue);

            if (ddlToMeasurement.SelectedValue == "0" && ddlFromMeasurement.SelectedValue == "0")
                lbConversion.Text = "";
        }

        // This method fires when user selects from measurement
        protected void ddlFromMeasurement_SelectedIndexChanged(object sender, EventArgs e)
        {
            //String[] FromMeasurements = ddlFromMeasurement.SelectedValue.Split(',');
            //ddlToMeasurement.Items.Clear();
            //StringBuilder stGetToConversion = new StringBuilder();
            //stGetToConversion.Append("select ISNULL(MTD.ConvesionValue,1)*IIF(ISNULL(MPM1.PowerOf,0)-ISNULL(MPM.PowerOf,0)<0,1/POWER(convert(float,10),ISNULL(MPM.PowerOf,0)-ISNULL(MPM1.PowerOf,0)),POWER(convert(float,10),ISNULL(MPM1.PowerOf,0)-ISNULL(MPM.PowerOf,0))) ConversionValue, "
            //                        + "IIF(MPM.MetricPrifixID IS NOT NULL, MPM.MetricPrifixName+' '+MTM.MeasurementName,MTM.MeasurementName) Measurement "
            //                        + "from MES_MeasurementMaster MTM "
            //                        + "LEFT JOIN MES_MatricPrefixMaster MPM ON MTM.ConversionTypeID=1 "
            //                        + "left join MES_MatricPrefixMaster MPM1 ON MPM1.MetricPrifixID=" + FromMeasurements[0]
            //                        + "LEFT join MES_MeasurementDetails MTD on MTD.MeasurementID=" + FromMeasurements[1] + " AND MTD.ToMesurementID=MTM.MeasurementID "
            //                        + "where MTM.MeasurementTypeID=" + ddlMeadureType.SelectedValue + " ORDER BY Measurement");

            //IDataReader rsGetToMeasurement = DB.GetRS(stGetToConversion.ToString());
            //ddlToMeasurement.Items.Add(new ListItem("Select", "0"));
            //while (rsGetToMeasurement.Read())
            //{
            //    ddlToMeasurement.Items.Add(new ListItem(DB.RSField(rsGetToMeasurement, "Measurement"), DB.RSFieldDouble(rsGetToMeasurement, "ConversionValue").ToString()));
            //}
            //rsGetToMeasurement.Close();

            ddlToMeasurement.SelectedIndex = 0;
            lbConversion.Text = "";


        }

        // This method fires when user selects any from measurement
        public void Build_FromMeasurement(String MeasurementID)
        {
            ddlFromMeasurement.Items.Clear();
            ddlToMeasurement.Items.Clear();
            //IDataReader rsGetFromMeasurement = DB.GetRS("select convert(nvarchar,isnull(MPM.MetricPrifixID,0))+','+convert(nvarchar,MTM.MeasurementID)MeasurementID,IIF(MPM.MetricPrifixID IS NOT NULL, MPM.MetricPrifixName+' '+MTM.MeasurementName,MTM.MeasurementName) Measurement from MES_MeasurementMaster MTM LEFT JOIN MES_MatricPrefixMaster MPM ON MTM.ConversionTypeID=1 where MeasurementTypeID=" + MeasurementType + " ORDER BY Measurement");
            IDataReader rsGetFromMeasurement = DB.GetRS("select convert(nvarchar,isnull(MPM.MetricPrifixID,0))+','+convert(nvarchar,MTM.MeasurementID)MeasurementID,IIF(MPM.MetricPrifixID IS NOT NULL, MPM.MetricPrifixName+' '+MTM.MeasurementName,MTM.MeasurementName) Measurement from MES_MeasurementMaster MTM LEFT JOIN MES_MatricPrefixMaster MPM ON MTM.ConversionTypeID=1 where MeasurementTypeID=" + MeasurementID + " ORDER BY Measurement");
            ddlFromMeasurement.Items.Add(new ListItem("Select", "0"));
            ddlToMeasurement.Items.Add(new ListItem("Select", "0"));
            while (rsGetFromMeasurement.Read())
            {
                ddlFromMeasurement.Items.Add(new ListItem(DB.RSField(rsGetFromMeasurement, "Measurement"), DB.RSField(rsGetFromMeasurement, "MeasurementID")));
                ddlToMeasurement.Items.Add(new ListItem(DB.RSField(rsGetFromMeasurement, "Measurement"), DB.RSField(rsGetFromMeasurement, "MeasurementID")));
            }
            rsGetFromMeasurement.Close();
            //ddlToMeasurement.Items.Clear();
        }

        // This method toggles the uom conversion are
        protected void lnkUoMConversion_Click(object sender, EventArgs e)
        {
            if (trMeasurements.Visible == true)
            {
                tdConversion.Visible = false;
                trMeasurements.Visible = false;
            }
            else
            {
                tdConversion.Visible = true;
                trMeasurements.Visible = true;
            }
        }

        // This method fires when user selects to measurement
        protected void ddlToMeasurement_SelectedIndexChanged(object sender, EventArgs e)
        {
            lbConversion.Text = "";

            String[] FromMeasurements = ddlFromMeasurement.SelectedValue.Split(',');
            String[] ToMeasurements = ddlToMeasurement.SelectedValue.Split(',');

            if (FromMeasurements.Length == 2 && ToMeasurements.Length == 2)
            {
                String CmdConversion = "select ISNULL(MTD.ConvesionValue,1)*IIF(ISNULL(MPM1.PowerOf,0)-ISNULL(MPM.PowerOf,0)<0,1/POWER(convert(float,10),ISNULL(MPM.PowerOf,0)-ISNULL(MPM1.PowerOf,0)),POWER(convert(float,10),ISNULL(MPM1.PowerOf,0)-ISNULL(MPM.PowerOf,0))) N "
                                        + "from MES_MeasurementMaster MTM "
                                        + "LEFT JOIN MES_MatricPrefixMaster MPM ON MTM.ConversionTypeID=1 and MPM.MetricPrifixID=" + ToMeasurements[0]
                                        + "left join MES_MatricPrefixMaster MPM1 ON MPM1.MetricPrifixID=" + FromMeasurements[0]
                                        + "LEFT join MES_MeasurementDetails MTD on MTD.MeasurementID=" + FromMeasurements[1] + " AND MTD.ToMesurementID=MTM.MeasurementID "
                                        + "where MTM.MeasurementTypeID=" + ddlMeadureType.SelectedValue + " and MTM.MeasurementID=" + ToMeasurements[1];
                IDataReader rsGetConversion = DB.GetRS(CmdConversion);
                if (rsGetConversion.Read())
                {
                    lbConversion.Text = " Conversion : " + DB.RSFieldDouble(rsGetConversion, "N");
                }
                rsGetConversion.Close();
            }
            else
            {
                lbConversion.Text = "";
            }
            //ddlToMeasurement.Items.Clear();
            //StringBuilder stGetToConversion = new StringBuilder();
            //stGetToConversion.Append("select ISNULL(MTD.ConvesionValue,1)*IIF(ISNULL(MPM1.PowerOf,0)-ISNULL(MPM.PowerOf,0)<0,1/POWER(convert(float,10),ISNULL(MPM.PowerOf,0)-ISNULL(MPM1.PowerOf,0)),POWER(convert(float,10),ISNULL(MPM1.PowerOf,0)-ISNULL(MPM.PowerOf,0))) ConversionValue, "
            //                        + "IIF(MPM.MetricPrifixID IS NOT NULL, MPM.MetricPrifixName+' '+MTM.MeasurementName,MTM.MeasurementName) Measurement "
            //                        + "from MES_MeasurementMaster MTM "
            //                        + "LEFT JOIN MES_MatricPrefixMaster MPM ON MTM.ConversionTypeID=1 "
            //                        + "left join MES_MatricPrefixMaster MPM1 ON MPM1.MetricPrifixID=" + FromMeasurements[0]
            //                        + "LEFT join MES_MeasurementDetails MTD on MTD.MeasurementID=" + FromMeasurements[1] + " AND MTD.ToMesurementID=MTM.MeasurementID "
            //                        + "where MTM.MeasurementTypeID=" + ddlMeadureType.SelectedValue + " ORDER BY Measurement");

            //IDataReader rsGetToMeasurement = DB.GetRS(stGetToConversion.ToString());
            //ddlToMeasurement.Items.Add(new ListItem("Select"));
            //while (rsGetToMeasurement.Read())
            //{
            //    ddlToMeasurement.Items.Add(new ListItem(DB.RSField(rsGetToMeasurement, "Measurement"), DB.RSFieldDouble(rsGetToMeasurement, "ConversionValue").ToString()));
            //}
            //rsGetToMeasurement.Close();
            //ScriptManager.RegisterStartupScript(this, this.GetType(), "test", "window.open(\"http://www.w3schools.com\", \"_blank\", \"toolbar=yes, scrollbars=yes, resizable=yes, top=500, left=500, width=400, height=400\");", true);

        }


        #endregion --------- Standard UoM Conversion -----------------


        #region ----------- Tenant Details ---------------

        private DataSet TenantList_buildGridData()
        {
            string TenantListsql = ViewState["TenantListSQL"].ToString();
            DataSet dsTenantList = DB.GetDS(TenantListsql, false);
            return dsTenantList;
        }

        private void TenantList_buildGridData(DataSet cMdTenantList)
        {
            gvTenantList.DataSource = cMdTenantList;
            gvTenantList.DataBind();
            cMdTenantList.Dispose();
        }

        protected void lnkAddTenant_Click(object sender, EventArgs e)
        {
            try
            {
                gvTenantList.EditIndex = 0;
                gvTenantList.PageIndex = 0;

                DataSet dsTenantList = TenantList_buildGridData();
                DataRow row = dsTenantList.Tables[0].NewRow();
                row["TenantMaterialMasterID"] = 0;
                row["IsInsurance"] = 0;
                dsTenantList.Tables[0].Rows.InsertAt(row, 0);
                this.TenantList_buildGridData(dsTenantList);
            }
            catch (Exception ex)
            {
                this.resetErrorForTenant("Error while inserting", true);
            }
        }

        protected void gvTenantList_Sorting(object sender, GridViewSortEventArgs e)
        {

        }

        protected void gvTenantList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvTenantList.PageIndex = e.NewPageIndex;
            TenantList_buildGridData(TenantList_buildGridData());
        }

        protected void gvTenantList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow && e.Row.RowIndex == gvTenantList.EditIndex)
            {
                string ltTenantID = ((Literal)e.Row.FindControl("ltTenantID")).Text;

                string TenantCheck = DB.GetSqlS("EXEC [dbo].[sp_GEN_CheckTenantForDelete] @MaterialMasterID=" + CommonLogic.QueryStringNativeInt("mid") + ",@TenantIDs=" + DB.SQuote(ltTenantID.ToString()));
                TextBox txtCompanyName = ((TextBox)e.Row.FindControl("txtCompanyName"));

                if (TenantCheck != "")
                {
                    txtCompanyName.Enabled = false;
                }
            }
        }

        protected void gvTenantList_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvTenantList.EditIndex = e.NewEditIndex;
            TenantList_buildGridData(TenantList_buildGridData());
        }

        protected void gvTenantList_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvTenantList.EditIndex = -1;
            TenantList_buildGridData(TenantList_buildGridData());
        }

        protected void gvTenantList_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            Page.Validate("vRequiredTenant");
            if (!Page.IsValid)
            {
                return;
            }

            GridViewRow row = gvTenantList.Rows[e.RowIndex];

            
            //DataRow row = ((DataRowView)e.Row.DataItem).Row;

            if (row != null)
            {

                string hifTenantID = ((HiddenField)row.FindControl("hifTenantID")).Value;
                string ltTenantMaterialMasterID = ((Literal)row.FindControl("ltTenantMaterialMasterID")).Text;

                //string MaterialShapeid = ((HiddenField)row.FindControl("hifMaterialShapeID")).Value;
                //string MaterialShape = ((TextBox)row.FindControl("txtMaterialShape")).Text;

                string vTenantPartNo = ((TextBox)row.FindControl("txtTenantPartNo")).Text;
                string vPrice = ((TextBox)row.FindControl("txtPrice")).Text;
                string chkIsInsurance = ((CheckBox)row.FindControl("chkIsInsurance")).Checked == true ? "1" : "0";

                string vSpaceUtilizationID = ((HiddenField)row.FindControl("hifSpaceUtilizationID")).Value;

                if (hifTenantID == "")
                {
                    resetErrorForTenant("This Tenant doesn't exist", true);
                    return;
                }

                if (vSpaceUtilizationID != "4")
                {
                    if (DB.GetSqlN("SELECT COUNT(*) N FROM MMT_MaterialMaster WHERE MaterialMasterID=" + CommonLogic.QueryString("mid") + " AND IsDeleted=0 AND (MLength IS NOT NULL AND MHeight IS NOT NULL AND MWidth IS NOT NULL AND MWeight IS NOT NULL)") == 0)
                    {
                        resetErrorForTenant("Please enter 'Item Dimensions' values, to add the 'Tenant'", true);
                        return;
                    }
                }

                //if (MaterialShapeid == "")
                //{
                //    resetErrorForTenant("Material Shape doesn't exist", true);
                //    return;
                //}

                if (DB.GetSqlN("select TenantMaterialMasterID N  from TPL_Tenant_MaterialMaster where TenantID=" + hifTenantID + " and MaterialMasterID=" + CommonLogic.QueryString("mid") + " and IsDeleted=0 and TenantMaterialMasterID!=" + ltTenantMaterialMasterID) != 0)
                {
                    resetErrorForTenant("Tenant already exists", true);
                    return;
                }

                StringBuilder QueryForUpsertTenant = new StringBuilder();
                QueryForUpsertTenant.Append(" EXEC [dbo].[sp_TPL_UpsertTenantMaterial] ");
                QueryForUpsertTenant.Append(" @TenantID=" + hifTenantID);
                QueryForUpsertTenant.Append(",@MaterialMasterID=" + CommonLogic.QueryString("mid"));
                QueryForUpsertTenant.Append(",@TenantPartNo=" + CommonLogic.IIF(vTenantPartNo != "", DB.SQuote(vTenantPartNo), "NULL"));
                QueryForUpsertTenant.Append(",@Price=" + CommonLogic.IIF(vPrice != "", DB.SQuote(vPrice), "NULL"));
                QueryForUpsertTenant.Append(",@IsInsurance=" + chkIsInsurance);
                QueryForUpsertTenant.Append(",@CreatedBy=" + cp.UserID);
                QueryForUpsertTenant.Append(",@TenantMaterialMasterID=" + ltTenantMaterialMasterID);
                //QueryForUpsertTenant.Append(",@MaterialShapeid=" + MaterialShapeid);
                QueryForUpsertTenant.Append(",@SpaceUtilizationID=" + vSpaceUtilizationID);

                try
                {
                    DB.ExecuteSQL(QueryForUpsertTenant.ToString());
                    ValidItemDimention();
                    resetErrorForTenant("Successfully updated", false);
                }
                catch (Exception ex)
                {
                    resetErrorForTenant("Error while updating", true);
                }

                gvTenantList.EditIndex = -1;
                this.TenantList_buildGridData(this.TenantList_buildGridData());

                lnkAddSupplier.Visible = true;
            }
        }

        protected void resetErrorForTenant(string error, bool isError)
        {

            ScriptManager.RegisterStartupScript(this, this.GetType(), "test", "showStickyToast(" + (isError.ToString().ToLower() == "true" ? "false" : "true") + ",\"" + error + "\");", true);
        }

        protected void lnkTenantDelete_Click(object sender, EventArgs e)
        {
            string gvIDs = "";
            string TenantIDs = "";
            bool chkBox = false;

            StringBuilder sqlDeleteString = new StringBuilder();

            foreach (GridViewRow gv in gvTenantList.Rows)
            {
                CheckBox isDelete = (CheckBox)gv.FindControl("chkIsDelete");
                if (isDelete.Checked)
                {
                    chkBox = true;
                    gvIDs += ((Literal)gv.FindControl("ltTenantMaterialMasterID")).Text.ToString() + ",";

                    TenantIDs += ((Literal)gv.FindControl("ltTenantID")).Text.ToString() + ",";
                }
            }

            if (chkBox)
            {
                string TenantCheck = DB.GetSqlS("EXEC [dbo].[sp_GEN_CheckTenantForDelete] @MaterialMasterID=" + CommonLogic.QueryStringNativeInt("mid") + ",@TenantIDs=" + DB.SQuote(TenantIDs.ToString()));

                if (TenantCheck != "")
                {
                    this.resetErrorForTenant("Cannot delete, as this Tenant is configured with  " + CommonLogic.IIF(TenantCheck != "", "Suppliers : ", "") + TenantCheck, true);
                    return;
                }

                //if (SOCheck != "" || POCheck != "")
                //{
                //    this.resetErrorForTenant("Cannot delete, as this Tenant is configured in  " + CommonLogic.IIF(SOCheck != "", "SO Numbers : ", "") + SOCheck + " " + CommonLogic.IIF(POCheck != "", "PO Numbers : ", "") + POCheck, true);
                //    return;
                //}

                sqlDeleteString.Append("BEGIN TRAN ");

                sqlDeleteString.Append("UPDATE TPL_Tenant_MaterialMaster SET IsDeleted=1 WHERE TenantMaterialMasterID IN (" + gvIDs.Substring(0, gvIDs.LastIndexOf(",")) + ")");

                sqlDeleteString.Append(" COMMIT ");
            }

            try
            {
                if (chkBox)
                {
                    DB.ExecuteSQL(sqlDeleteString.ToString());
                }
                resetErrorForTenant("Successfully deleted the selected items", false);

            }
            catch (Exception ex)
            {
                resetErrorForTenant("Error Updating data" + ex.ToString(), true);
            }

            this.TenantList_buildGridData(this.TenantList_buildGridData());

            if (gvTenantList.Rows.Count == 0)
            {
                lnkAddSupplier.Visible = false;
            }
            else
                lnkAddSupplier.Visible = true;

            ValidItemDimention();
        }

        #endregion ----------- Tenant Details ---------------

        protected void lnkupdateDimention_Click(object sender, EventArgs e)
        {
            System.Text.StringBuilder query = new StringBuilder();
            query.Append(" UPDATE MMT_MaterialMaster ");
            query.Append(" set MLength="+(txtMLength.Text==""?"null":txtMLength.Text));
            query.Append(" , MHeight=" + (txtMHeight.Text == "" ? "null" : txtMHeight.Text));
            query.Append(" , MWidth =" + (txtMWidth.Text == "" ? "null" : txtMWidth.Text));
            query.Append(" , MWeight=" + (txtMWeight.Text == "" ? "null" : txtMWeight.Text));
            query.Append(" , CapacityPerBin=" + (txtCapacityperBin.Text == "" ? "null" : txtCapacityperBin.Text));
            query.Append(" where MaterialMasterID=" + ViewState["MaterialMasterID"]);
            try 
            {
                DB.ExecuteSQL(query.ToString());
                resetError("Successfully updated the item dimensions", false);
            }
            catch (Exception ex)
            {
                resetError("Error while updating", true);
            }
        }

        protected void lnkUpdateTenant_Click(object sender, EventArgs e)
        {
            Page.Validate("UpdateTenant");
            if (!Page.IsValid)
            {
                return;
            }

            if (hifTenant.Value == "")
            {
                resetErrorForTenant("This Tenant doesn't exist", true);
                return;
            }

            //if (DB.GetSqlN("select COUNT(*) N from MMT_MaterialMaster_Supplier where IsDeleted=0 and MaterialMasterID="+CommonLogic.QueryString("mid")) > 0)
            //{
            //    resetErrorForTenant("Supplier is added to this Tenant, Please delete the Supplier and modify the Tenant", true);
            //    return;
            //}

            if (hifSpaceUtilization.Value != "4")
            {
                if (DB.GetSqlN("SELECT COUNT(*) N FROM MMT_MaterialMaster WHERE MaterialMasterID=" + CommonLogic.QueryString("mid") + " AND IsDeleted=0 AND (MLength IS NOT NULL AND MHeight IS NOT NULL AND MWidth IS NOT NULL AND MWeight IS NOT NULL)") == 0)
                {
                    resetErrorForTenant("Please enter 'Item Dimensions' values, to add the 'Tenant'", true);
                    return;
                }
            }

            StringBuilder QueryForUpsertTenant = new StringBuilder();
            QueryForUpsertTenant.Append(" EXEC [dbo].[sp_TPL_UpsertTenantMaterial] ");
            QueryForUpsertTenant.Append(" @TenantID=" + hifTenant.Value);
            QueryForUpsertTenant.Append(",@MaterialMasterID=" + CommonLogic.QueryString("mid"));
            QueryForUpsertTenant.Append(",@TenantPartNo=" + CommonLogic.IIF(txtTenantPartNo.Text != "", DB.SQuote(txtTenantPartNo.Text.Trim()), "NULL"));
            QueryForUpsertTenant.Append(",@Price=" + CommonLogic.IIF(txtPrice.Text != "", DB.SQuote(txtPrice.Text.Trim()), "NULL"));
            QueryForUpsertTenant.Append(",@IsInsurance=" + Convert.ToInt32(chkIsInsurance.Checked));
            QueryForUpsertTenant.Append(",@CreatedBy=" + cp.UserID);
            QueryForUpsertTenant.Append(",@TenantMaterialMasterID=0");
            //QueryForUpsertTenant.Append(",@MaterialShapeid=" + MaterialShapeid);
            QueryForUpsertTenant.Append(",@SpaceUtilizationID=" + hifSpaceUtilization.Value);

            try
            {
                DB.ExecuteSQL(QueryForUpsertTenant.ToString());
                ValidItemDimention();
                resetErrorForTenant("Successfully updated", false);
            }
            catch (Exception ex)
            {
                resetErrorForTenant("Error while updating", true);
            }

            gvTenantList.EditIndex = -1;
            this.TenantList_buildGridData(this.TenantList_buildGridData());

            lnkAddSupplier.Visible = true;
            }

        protected void lnkUpdateMSPs_Click(object sender, EventArgs e)
        {
            if (DB.GetSqlN("select MaterialMasterID N from MMT_MaterialMaster where MaterialMasterID=" + CommonLogic.QueryString("mid"))!=0)
            {
                SOCheck = DB.GetSqlS("EXEC [dbo].[sp_GEN_CheckSO] @MaterialMasterID=" + CommonLogic.QueryStringNativeInt("mid") + ",@TenantID=" + TenantID);
                POCheck = DB.GetSqlS("EXEC [dbo].[sp_GEN_CheckPO] @MaterialMasterID=" + CommonLogic.QueryStringNativeInt("mid") + ",@TenantID=" + TenantID);


                if (SOCheck != "" || POCheck != "")
                {
                    resetError("Cannot update MSP'S, as this material is configured in" + " SO Numbers:" + SOCheck + " " + CommonLogic.IIF(POCheck != "", "PO Numbers :", "") + POCheck, true);
                    return;
                }
                DB.ExecuteSQL("UPDATE MMT_MaterialMaster set MtypeID=" + ddlMTypeID.SelectedValue + " WHERE MaterialMasterID=" + CommonLogic.QueryString("mid"));
                UpdateMsp_LastClick();

                LoadMMMspList(CommonLogic.QueryString("mid"), ddlMTypeID.SelectedValue);// Load MSP's for Unique MMID
                
            }
        }

        protected void lnkbtnsave_Click(object sender, EventArgs e)
        {
            try
            {


                int MMID = Convert.ToInt32(ViewState["MaterialMasterID"].ToString());
                if (MMID != 0)
                {
                    if (txtlocation.Text == "")
                    {
                        resetError("Please Select Location", true);
                        return;
                    }
                    if (hiflocation.Value == "" && hiflocation.Value == "0")
                    {
                        resetError("Please Select Location", true);
                        return;
                    }
                    if (txtMinimumStockLevel.Text == "")
                    {
                        resetError("Please enter minimum stock level", true);
                        return;
                    }
                    if(txtMaximumStockLevel.Text == "")
                    {
                        resetError("Please enter maximum stock level", true);
                        return;
                    }                    

                    int LocationID = DB.GetSqlN("SELECT COUNT(*) AS N FROM MMT_BinReplenishmentPlan WHERE MaterialMasterID =  "+ MMID + " AND LocationID = "+ hiflocation.Value + "");

                   
                    if (hifbinid.Value != "" && hifbinid.Value != "0" && hifbinid.Value != null)
                    {
                        if(hifexstingloc.Value == hiflocation.Value)
                        {
                            BinReplishementID = Convert.ToInt32(hifbinid.Value);
                        }
                        else
                        {
                            if (LocationID != 0)
                            {
                                resetError("Location already update", true);
                                return;
                            }
                            BinReplishementID = Convert.ToInt32(hifbinid.Value);
                        }
                    }
                    else
                    {
                        if (LocationID != 0)
                        {
                            resetError("Location already update", true);
                            return;
                        }
                        BinReplishementID = 0;
                    }

                    decimal minqty = Convert.ToDecimal(txtMinimumStockLevel.Text);
                    decimal maxqty = Convert.ToDecimal(txtMaximumStockLevel.Text);
                    if(maxqty <= minqty)
                    {
                        resetError("Min Qty is more than Max qty", true);
                        return;
                    }

                    DB.ExecuteSQL("EXEC [dbo].[sp_MMT_UpsertBinReplenishmentPlan] @BinReplenishment="+ BinReplishementID + ", @MaterialMasterID = " + MMID + ", @LocationID = " + hiflocation.Value + ", @MinimumStockLevelQty = " + txtMinimumStockLevel.Text + ", @MaximumStockLevelQty = " + txtMaximumStockLevel.Text + ", @UserID = " + cp.UserID + "");
                    lnkbtnsave.Text = "Save" + CommonLogic.btnfaSave;
                    BinReplishementID = 0;
                    hifbinid.Value = "0";
                    this.gvItemwiseloc(this.gvItemwiseloc());
                    resetError("Successfully Update", false);
                    txtMinimumStockLevel.Text = "";
                    txtMaximumStockLevel.Text = "";
                    txtlocation.Text = "";
                    hifexstingloc.Value = "";
                }
                return;


            }
            catch (Exception ex)
            {
                resetError("Error while Insert, Error is: " + ex, true);
            }
        }

        protected DataSet gvItemwiseloc()
        {
            
            try
            {

                string Query = "[dbo].[GET_ITEMWISE_BINLOCATIONLIST] @MaterialMasterID = " + CommonLogic.QueryString("mid") + "";

                DataSet DS = DB.GetDS(Query, false);
                return DS;
            }
            catch (Exception ex)
            {
                // ------------------ added Exception log by durga on  (21/04/2017)
                return null;

            }
           
        }

        protected void gvItemwiseloc(DataSet ds)
        {
            if(ds.Tables[0].Rows.Count != 0)
            {
                gvbinreplishment.DataSource = ds;
                gvbinreplishment.DataBind();
            }
          
        }

        // for msp configuration to material  -- gopi
        protected void linkmspsave_Click(object sender, EventArgs e)
        {
            try
            {
                string MaterialId = CommonLogic.QueryString("mid").ToString();
                int IsRequired;
                StringBuilder xml = new StringBuilder();
                xml.Append("<root>");


                foreach (GridViewRow gr in gvMsp.Rows)
                {
                    IsRequired = 0;
                    Literal ltMspId = (Literal)gr.FindControl("ltMaterialStorageParameterID");
                    CheckBox cbIsRequired = (CheckBox)gr.FindControl("chkMSPIsRequired");
                    if (cbIsRequired.Checked)
                    {
                        IsRequired = 1;
                    }
                    xml.Append("<data>");
                    xml.Append("<MaterialMasterID>" + MaterialId + "</MaterialMasterID>");
                    xml.Append("<MaterialStorageParameterID>" + ltMspId.Text.ToString() + "</MaterialStorageParameterID>");
                    xml.Append("<IsRequired>" + IsRequired + "</IsRequired>");
                    xml.Append("<CreatedBy>" + cp.UserID.ToString() + "</CreatedBy>");
                    xml.Append("</data>");
                }

                xml.Append("</root>");

                DB.ExecuteSQL("EXEC [dbo].[MaterialMaster_SET_MaterialStorageParameter] @MSPXML=" + DB.SQuote(xml.ToString()));

                resetError("Saved Successfully", false);

                gvmspList_buildGridData(gvmspList_buildGridData());

            }
            catch (Exception exp)
            {
                throw exp;
            }
        }
       

        [WebMethod]
        public static string GetPreferences()
        {
            string Json = "";
            try
            {
                DataSet ds = DB.GetDS("[dbo].[GEN_MST_Get_PreferenceGroups] ", false);
                Json = JsonConvert.SerializeObject(ds);
            }
            catch (Exception ex)
            {
                Json = "Failed";
            }
            return Json;
        }
         [WebMethod]
        public static string SETPreferences(int UserID, string Inxml)
        {
            string Status = "";
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(" Exec [dbo].[USP_SET_GEN_TRN_Preferences] ");
                sb.Append(" @inputDataXml='" + Inxml + "'");
                sb.Append(" ,@LoggedUserID=" + UserID);
                DB.ExecuteSQL(sb.ToString());
                Status = "success";
            }
            catch (Exception ex)
            {
                Status = "Failed";
            }
            return Status;
        }


         [WebMethod]
         public static string GetIndustries()
         {
             string Json = "";
             try
             {
                 DataSet ds = DB.GetDS("[dbo].[GEN_MST_Get_Material_Industries] ", false);
                 Json = JsonConvert.SerializeObject(ds);
             }
             catch (Exception ex)
             {
                 Json = "Failed";
             }
             return Json;
         }

         [WebMethod]
         public static string SETIndustries(int UserID, string Inxml, int MM_MST_Material_ID)
         {
             string Status = "";
             try
             {
                 StringBuilder sb = new StringBuilder();
                 sb.Append(" Exec [dbo].[USP_SET_Industry_Attributes] ");
                 sb.Append(" @AinputDataXml='" + Inxml + "'");
                 sb.Append(" ,@LoggedUserID=" + UserID);
                 //sb.Append(" ,@LanguageType='en'");
                 sb.Append(" ,@MM_MST_Material_ID=" + MM_MST_Material_ID);
                 DB.ExecuteSQL(sb.ToString());
                 Status = "success";
                 //ClientScript.RegisterStartupScript(GetType(), "Javascript", "javascript:UpsertIndustries(" + vResult + "); ", true);
             }
             catch (Exception ex)
             {
                 Status = "Failed";
             }
             return Status;
         }


        protected void gvbinreplishment_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

        }

        protected void gvbinreplishment_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }

        protected void gvbinreplishment_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }

        protected void gvbinreplishment_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {

        }

        protected void gvbinreplishment_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {

        }

        protected void gvbinreplishment_RowEditing(object sender, GridViewEditEventArgs e)
        {

        }

        protected void gvbinreplishment_RowUpdating1(object sender, GridViewUpdateEventArgs e)
        {

        }

        protected void linkdelete_Click(object sender, EventArgs e)
        {

        }

        protected void lintedit_Click(object sender, EventArgs e)
        {
            int Editindex = ((GridViewRow)((LinkButton)sender).Parent.Parent).RowIndex;
            gvbinreplishment.EditIndex = Editindex;
            GridViewRow gvRow = gvbinreplishment.Rows[Editindex];
            Label ltlocationheader = (Label)gvRow.FindControl("ltlocationheader");
            Label lblMMID = (Label)gvRow.FindControl("lblMMID");
            Label lblLocID = (Label)gvRow.FindControl("lbllocation");
            Label lblbinreplishment = (Label)gvRow.FindControl("lblBinRepid");
            Label ltminimumstocklevel = (Label)gvRow.FindControl("ltminimumstocklevel");
            Label ltmaximumstocklevel = (Label)gvRow.FindControl("ltmaximumstocklevel");
            BinReplishementID = Convert.ToInt32(lblbinreplishment.Text);
            hifbinid.Value = lblbinreplishment.Text;
            txtMinimumStockLevel.Text = ltminimumstocklevel.Text;
            txtMaximumStockLevel.Text = ltmaximumstocklevel.Text;
            txtlocation.Text = ltlocationheader.Text;
            hiflocation.Value = lblLocID.Text;
            hifexstingloc.Value = lblLocID.Text;
            lnkbtnsave.Text = "Update" + CommonLogic.btnfaUpdate;

        }

        
    }
    }

