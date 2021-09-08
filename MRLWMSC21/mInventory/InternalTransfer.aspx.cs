using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MRLWMSC21Common;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using System.Web.UI.HtmlControls;
using System.Globalization;

namespace MRLWMSC21.mInventory
{
    public partial class InternalTransfer : System.Web.UI.Page
    {
        //Author    :   Gvd Prasad
        //Created On:   04-Mar-2014
        //use case ID:  Inter Store Transfer_UC_020

        public CustomPrincipal cp = HttpContext.Current.User as CustomPrincipal;
        
        protected void page_PreInit(object sender, EventArgs e)
        {
            Page.Theme = "Inventory";
        }

        protected void page_Init(object sender, EventArgs e)
        {
            String MMID = CommonLogic.QueryString("mmid");

            if (MMID != "")
            {
                //AddDynamicColumns(MMID);

            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //if (!cp.IsInAnyRoles(CommonLogic.GetRolesAllowed(CommonLogic.GetRolesAllowedForthisPage("Inter Store Transfer"))))
            //{

            //    //test case ID (TC_008)
            //    //Only authorized use can access inter store transfer page

            //    Response.Redirect("../Login.aspx?eid=6");
            //}

            if (!IsPostBack)
            {
               // DesignLogic.SetInnerPageSubHeading(this.Page, "Inter Store Transfer");
                txtTenant.Enabled = CommonLogic.CheckSuperAdmin(txtTenant, cp, hifTenant);
                GetStorageLocations();
                if (CommonLogic.QueryString("mmid") != "")
                {
                    String cmdMaterialDetails = "EXEC [sp_TPL_GetDataForInternalTransfer]	@MaterialMasterID=" + CommonLogic.QueryString("mmid") + ",@TenantID=" + CommonLogic.QueryString("tid")+", @WarehouseId="+CommonLogic.QueryString("WHId");


                    try
                    {
                        IDataReader rsMaterialDetails = DB.GetRS(cmdMaterialDetails);
                        if (rsMaterialDetails.Read())
                        {
                            hifTenant.Value = DB.RSFieldInt(rsMaterialDetails, "TenantID").ToString();
                            txtTenant.Text = DB.RSField(rsMaterialDetails, "TenantName");
                            ltTenant.Text = DB.RSField(rsMaterialDetails, "TenantName");
                            ltMcode.Text = DB.RSField(rsMaterialDetails, "MCode") + (DB.RSField(rsMaterialDetails, "OEMPartNo") != "" ? " [ " + DB.RSField(rsMaterialDetails, "OEMPartNo") + "]" : "");
                            ltBMUoM.Text = DB.RSField(rsMaterialDetails, "BUoM") + "/" + DB.RSFieldDecimal(rsMaterialDetails, "BUoMQty");
                            ltMMUoM.Text = DB.RSField(rsMaterialDetails, "MUoM") + "/" + DB.RSFieldDecimal(rsMaterialDetails, "MUoMQty");
                            atcMaterialMaster.Text = DB.RSField(rsMaterialDetails, "MCode");
                            hifMaterialMaster.Value = DB.RSFieldInt(rsMaterialDetails, "MaterialMasterID").ToString();
                            hifMaterialCategory.Value = DB.RSFieldInt(rsMaterialDetails, "ProductCategoryID").ToString();
                            hifBUoMQty.Value = DB.RSFieldDecimal(rsMaterialDetails, "BUoMQty").ToString();
                            hifConvesionValueINMuom.Value = DB.RSFieldDecimal(rsMaterialDetails, "CFInMUoM").ToString();
                            txtWH.Text = DB.RSField(rsMaterialDetails, "WHCode");
                            hifWarehouseId.Value = DB.RSFieldInt(rsMaterialDetails, "WarehouseID").ToString();
                            hifConversionType.Value = "0";

                            if (DB.RSFieldInt(rsMaterialDetails, "MeasurementTypeID") != 0)
                            {
                                hifConversionType.Value = "1";
                            }


                            Build_gvInternalTransfer(Build_gvInternalTransfer());
                            if (gvInternalTransfer.Rows.Count == 0)
                            {
                                resetError("No stock available", true);
                            }
                            tbMaterialDetails.Visible = true;

                        }
                        else
                        {
                            resetError("Not valid material", true); 
                        }
                        rsMaterialDetails.Close();
                        
                    }
                    catch (Exception ex)
                    {
                        resetError("Error while loading",true);
                        CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
                    }
                }
                
            }
        }

        public DataSet GetStorageLocations()
        {
            DataSet ds = null;
            try
            {
                ds = DB.GetDS("Exec SP_INV_GET_STORAGELOCATION ", false);
                ddlStorageLocationID.AppendDataBoundItems = true;
                ddlStorageLocationID.Items.Add(new ListItem("-- Please Select --", "0"));
                ddlStorageLocationID.DataSource = ds.Tables[0];
                ddlStorageLocationID.DataTextField = "Code";
                ddlStorageLocationID.DataValueField = "Id";
                ddlStorageLocationID.DataBind();

            }
            catch (Exception ex)
            {

            }
            return ds;
        }

        private void AddDynamicColumns(String MMID)
        {
            TemplateField field;
            try
            {
                IDataReader rsMSPList = DB.GetRS("[sp_ORD_GetMaterialStorageParameters] @MaterialMasterID=" + MMID + ",@TenantID=" + cp.TenantID);
                while (rsMSPList.Read())
                {
                    field = new TemplateField();
                    field.HeaderText = DB.RSField(rsMSPList, "DisplayName");
                    field.HeaderTemplate = new DynamicallyTemplatedGridViewHandler(ListItemType.Header, DB.RSField(rsMSPList, "DisplayName"), "Literal");
                    field.HeaderStyle.Width = 150;
                   /* switch (DB.RSField(rsMSPList, "ControlType"))
                    {
                        case "DropDownList":
                            {
                                field.ItemTemplate = new DynamicallyTemplatedGridViewHandler(ListItemType.Item, DB.RSField(rsMSPList, "ParameterName"), "Literal");
                                
                               // field.ItemTemplate = new DynamicallyTemplatedGridViewHandler(ListItemType.Item, DB.RSField(rsMSPList, "ParameterName"), "LiteralWithID");
                                break;
                            }
                        default:
                            {
                                field.ItemTemplate = new DynamicallyTemplatedGridViewHandler(ListItemType.Item, DB.RSField(rsMSPList, "ParameterName"), "Literal");
                                break;
                            }
                    }*/
                    field.ItemTemplate = new DynamicallyTemplatedGridViewHandler(ListItemType.Item, DB.RSField(rsMSPList, "ParameterName"), "Literal");
                    gvInternalTransfer.Columns.Add(field);
                }
                rsMSPList.Close();
                field = new TemplateField();
                field.HeaderText = "Select One";
                field.ItemTemplate = new DynamicallyTemplatedGridViewHandler(ListItemType.Item, "SelectOne", "RadioButton");
                gvInternalTransfer.Columns.Add(field);
            }
            catch (Exception ex)
            {
                resetError("Error while add Material Storage Parameter", true);
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
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


            ltStatus.Text = str;*/
            ScriptManager.RegisterStartupScript(this, this.GetType(), "test", "closeAsynchronus(); showStickyToast(" + (isError.ToString().ToLower() == "true" ? "false" : "true") + ",\"" + error + "\");", true);

        }

        protected void lnkGet_Click(object sender, EventArgs e)
        {
            //test case ID (TC_009)
            //Get Available Quantity with locations of the material

            if(hifTenant.Value == "" || txtTenant.Text == "") {
                resetError("Please select Tenant", true);
            }
            if (hifMaterialMaster.Value != "" && atcMaterialMaster.Text != "Part Number..." && atcMaterialMaster.Text != "")
            {
                if (hifWarehouseId.Value != ""&& txtWH.Text!="")
                {
                    Response.Redirect("InternalTransfer.aspx?mmid=" + hifMaterialMaster.Value + "&tid=" + hifTenant.Value + "&WHId=" + hifWarehouseId.Value);
                }
                else
                {
                    resetError("Please select Warehouse", true);
                }
            }
            else
            {
                resetError("Please select material", true);
            }
            
           
            
        }

        protected void lnkTransfer_Click(object sender, EventArgs e)
        {
            //test case ID (TC_015)
            //Transfer to select location with enter quantity

            /// Below Validations Added by Lalitha //////

            if (hifTenant.Value == "" || hifTenant.Value == "0" || txtTenant.Text == null || txtTenant.Text == null)
            {
                resetError("Please Select Tenant", true);
                return;
            }
            if (atcMaterialMaster.Text == "" || atcMaterialMaster.Text == null || hifMaterialMaster.Value == "" || hifMaterialMaster.Value == "0")
            {
                resetError("Please Select Material", true);
                return;
            }
            if (txtWH.Text == "" || txtWH.Text == null || hifWarehouseId.Value == "" || hifWarehouseId.Value == "0")
            {
                resetError("Please Select Warehouse", true);
                return;
            }

            /// Below Validations Added by Lalitha //////
            /// 

            if ( txtQuantity.Text.Trim() == "")
            {
                //test case ID (TC_017)
                //Must select Both Location and quantity

                resetError("Please enter quantity", true);
                return;
            }

            // if(txtLocation.Text.Trim() == "")
            if (txtLocation.Text.Trim() == "" || hiflocationID.Value == "" || hiflocationID.Value == null || hiflocationID.Value == "0")
            {
                resetError("Please Select Location", true);
                return;
            }
            if (txtCarton.Text == "")
            {
                resetError("Please Select Pallet", true);
                return;
            }
            //if (ddlStorageLocationID.SelectedValue.ToString() == "0")
            //{
            //    resetError("Please Select Storage Location.", true);
            //    return;
            //}

            int cartonID = 0;
            if (txtCarton.Text != "")
            {
                cartonID = DB.GetSqlN("SELECT CartonID N FROM INV_Carton WHERE IsDeleted=0 AND IsActive=1 AND CartonCode=" + DB.SQuote(txtCarton.Text));
                if (cartonID == 0)
                {
                    resetError("Not a valid carton", true);
                    return;
                }

            }



            //int locationId = 0;
            //try
            //{
            //    locationId = DB.GetSqlN("select LocationID as N from INV_Location where  IsDeleted=0 and IsActive=1 and Location =" + DB.SQuote(txtLocation.Text));
            //}
            //catch (Exception ex)
            //{
            //    resetError("Error while Transfer Location",true);
            //    CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
            //}

            //if (locationId == 0)
            //{
            //    resetError("Not valid location",true);
            //    return;
            //}
            //int check = 0;
            bool isEmpty = true;
            foreach (GridViewRow row in gvInternalTransfer.Rows)
            {
                RadioButton RD = ((RadioButton)row.FindControl("rdselect"));
              //  string selectedValue = Request.Form["MyRadioButton"];
               
                CheckBox checkbox= ((CheckBox)row.FindControl("SelectOne"));
                if (RD.Checked == true)
                {
                    isEmpty = false;
                    
                    Literal ltQuantity = ((Literal)row.FindControl("ltQuantity"));
                    Literal ltStorageLocationID = ((Literal)row.FindControl("ltStorageLocationID"));
                    Literal ltFromLocation = ((Literal)row.FindControl("ltLocation"));

                    Literal ltMFGDate = ((Literal)row.FindControl("ltmfgDate"));
                    Literal ltExpDate = ((Literal)row.FindControl("ltExpDate"));
                    Literal ltSerialNumber = ((Literal)row.FindControl("ltSerialNo"));
                    Literal ltBatchNumber = ((Literal)row.FindControl("ltBatchNo"));
                    Literal ltProjectRef = ((Literal)row.FindControl("ltProjectRefNo"));
                    Literal ltFromSloc = ((Literal)row.FindControl("ltCode"));
                    Literal ltContainer = ((Literal)row.FindControl("ltCartonCode"));
                    


                    if (ltFromLocation.Text == txtLocation.Text)
                    {
                        resetError("From Location and To Location  should not be same", true);
                        return;
                    }

                   
                    string txtToLocation = txtLocation.Text;
                    Literal ltStorageLocation = ((Literal)row.FindControl("ltStorageLocationID"));
                 
                    
                    Decimal TransferQty = Convert.ToDecimal(txtQuantity.Text);
                    Decimal AvailableQty = Convert.ToDecimal(ltQuantity.Text);
                    Literal ltSerialNo = ((Literal)row.FindControl("ltSerialNo"));
                    Control ctSerialNo = row.FindControl("SerialNo");
                    if (ltSerialNo.Text != "")
                    {
                        if (ltSerialNo.Text.Length > 0)
                        {
                            if (Convert.ToDouble(txtQuantity.Text) != 1)
                            {
                                resetError("Quantity should be 1 for serial no., cannot transfer", true);
                                return;
                            }
                        }
                    }


                    //try
                    //{
                    //    if (DB.GetSqlN("sp_INV_IsInSupplierReturns @MMID="+ hifMaterialMaster.Value+ " ,@GoodsMovementDetailsIDs=" + DB.SQuote(ltGoodsMovementDetailsIDs.Text)) != 0)
                    //    {
                    //        //test case ID (TC_025)
                    //        //Cannot transfer, as selected material is returned to supplier

                    //        resetError("Material is in supplier returns, cannot transfer", true);
                    //        return;
                    //    }
                    //}
                    //catch (Exception ex)
                    //{
                    //    resetError("Error while Transfer Location",true);
                    //    CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
                    //    return;
                    //}


                    if (TransferQty <= 0)
                    {
                        resetError("Cannot transfer quantity less than or equal to zero", true);
                        return;
                    }
                    else if (TransferQty > AvailableQty)
                    {
                        //test case ID (TC_018)
                        //Transferred qty. should be less than or equal to available qty.

                        resetError("Cannot transfer quantity greater than available quantity", true);
                        return;
                    }

                    //try
                    //{
                    //    int CycleCountOn = DB.GetSqlN("select convert(int,max(qcc.IsOn)) as N from QCC_CycleCount qcc left join QCC_CycleCountDetails qccd on qccd.CycleCountID=qcc.CycleCountID  where qccd.MaterialMasterID=" + CommonLogic.QueryString("mmid"));
                    //    if (CycleCountOn == 1)
                    //    {
                    //        //test case ID (TC_024)
                    //        //Cannot transfer, as material is in cycle count

                    //        resetError("Cannot transfer, as this material is in 'Cycle Count' ", true);
                    //        return;
                    //    }
                    //}
                    //catch (Exception ex)
                    //{
                    //    resetError("Error while Transfer Location", true);
                    //    CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
                    //    return;
                    //}

                    string Location1 = DB.GetSqlS("select Location as S from INV_Location where DisplayLocationCode =" + DB.SQuote(ltFromLocation.Text) + " ");

                    StringBuilder cMdTransferQuantity = new StringBuilder();
                    cMdTransferQuantity.Append("EXEC  [dbo].[Upsert_BinToBinTransfer_HHT]");
                    cMdTransferQuantity.Append("@FromLocation =" + DB.SQuote(Location1));
                    cMdTransferQuantity.Append(",@Quantity=" + DB.SQuote(TransferQty.ToString()));
                    cMdTransferQuantity.Append(",@BatchNo=" + DB.SQuote(ltBatchNumber.Text));
                    cMdTransferQuantity.Append(",@ExpDate=" + DB.SQuote(ltExpDate.Text));
                    cMdTransferQuantity.Append(",@MfgDate=" + DB.SQuote(ltMFGDate.Text));
                    cMdTransferQuantity.Append(",@SerialNo=" + DB.SQuote(ltSerialNo.Text));
                    cMdTransferQuantity.Append(",@ProjectRefNo =" + DB.SQuote(ltProjectRef.Text));
                    cMdTransferQuantity.Append(",@CreatedBy=" + DB.SQuote(cp.UserID.ToString()));
                    cMdTransferQuantity.Append(",@MCODE=" +DB.SQuote( atcMaterialMaster.Text));
                    cMdTransferQuantity.Append(",@TLoc=" +DB.SQuote(txtLocation.Text));
                    cMdTransferQuantity.Append(",@FromCarton=" +DB.SQuote( ltContainer.Text));
                    cMdTransferQuantity.Append(",@ToCarton =" +DB.SQuote( txtCarton.Text) );
                    cMdTransferQuantity.Append(",@FromSLOC=" +DB.SQuote( ltFromSloc.Text));
                    cMdTransferQuantity.Append(",@TenantID=" + hifTenant.Value);
                    cMdTransferQuantity.Append(",@WarehouseID=" + hifWarehouseId.Value);
                    try
                    {
                        DB.ExecuteSQL(cMdTransferQuantity.ToString());
                        Build_gvInternalTransfer(Build_gvInternalTransfer());
                        resetError("Successfully Updated", false);
                        txtLocation.Text = "";
                        txtCarton.Text = "";
                        txtQuantity.Text = "";
                    }
                    catch (Exception ex)
                    {
                        resetError("Error while transferring material", true);
                        CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
                    }
                    return;
                }
                
            }
            if (isEmpty)
            {
                resetError("No material is selected", true);
            }
        }

        private DataSet Build_gvInternalTransfer()
        {
            DataSet dsTotallistForMaterial = null;
            String cMdTotalListForMaterial = "sp_INV_GetAvailableStockForMaterila1	@MaterialMasterID=" + CommonLogic.QueryString("mmid") + ",@TenantID="+hifTenant.Value+", @AccountID_New="+cp.AccountID+ ",@TenantID_New="+cp.TenantID+ ", @WarehouseID=" + CommonLogic.QueryString("WHId") + "";
            try
            {
                dsTotallistForMaterial = DB.GetDS(cMdTotalListForMaterial, false);
            }
            catch (Exception ex)
            {
                resetError("Error while loading",true);
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
            }
            return dsTotallistForMaterial;
        }

        private void Build_gvInternalTransfer(DataSet dsTotallistForMaterial)
        {
            gvInternalTransfer.DataSource = dsTotallistForMaterial; 
            gvInternalTransfer.DataBind();
            dsTotallistForMaterial.Dispose();
        }

        protected void gvInternalTransfer_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvInternalTransfer.PageIndex = e.NewPageIndex;
            Build_gvInternalTransfer(Build_gvInternalTransfer());
        }
        
        public String Getimage(String IsChecked)
        {
            if (IsChecked != "")
            {
                if (IsChecked=="Yes")
                    return "../Images/blue_menu_icons/check_mark.png";
                else
                    return null;
            }
            else
            {
                return null;
            }
        }

        protected void lnkGenerateNewContainer_Click(object sender, EventArgs e)
        {
            string cartonCode = getNewCarton();

            if (cartonCode != null)
            {
                resetError("Successfully Created", false);
               // txtCarton.Text = cartonCode;
            }
            else
            {
                resetError("Error while generating new container", true);
                txtCarton.Text = "";
            }
        }

        public string getNewCarton()
        {

            StringBuilder query = new StringBuilder();

            query.Append("DECLARE @NewUpdateCarton nvarchar(50);   ");
            query.Append("EXEC [dbo].[sp_INV_CartonCreation]    ");
            query.Append("@InboundID=0" + ",");
            query.Append("@UserId=" + cp.UserID + ",");
            query.Append("@NewCarton=@NewUpdateCarton OUTPUT select @NewUpdateCarton AS S");

            try
            {
                return DB.GetSqlS(query.ToString());
            }
            catch (Exception e)
            {
                return null;
            }
        }

    
        protected void txtCarton_TextChanged(object sender, EventArgs e)
        {

         
        }
        }
    }
