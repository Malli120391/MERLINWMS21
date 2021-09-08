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
using System.Globalization;

namespace MRLWMSC21.mManufacturingProcess
{
    //Author    :   Gvd Prasad
    //Created On:   23-Jun-2014
    //Use case ID:  Phantom Job Order_UC_17

    public partial class PhantomJobOrder : System.Web.UI.Page
    {

        public CustomPrincipal cp = HttpContext.Current.User as CustomPrincipal;

        protected void page_Init(object sender, EventArgs e)
        {
            if (CommonLogic.QueryString("proid") != "")
            {
                AddDynamicColumns();
            }
        }

        protected void Page_PreInit(object sender, EventArgs e)
        {
            Page.Theme = "Inbound";

        }

        private void AddDynamicColumns()
        {
            IDataReader rsGetMaterialListForJob;
            TemplateField field;
            int RowNumber = 0;
            String cMdGetMaterialListForJob = "dbo.sp_MFG_GetMaterialsforJob " + CommonLogic.QueryString("proid");
            try
            {
                rsGetMaterialListForJob = DB.GetRS(cMdGetMaterialListForJob);
            }
            catch (Exception ex)
            {
                resetError("Error while loading",true,false);
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
                return;
            }
            while (rsGetMaterialListForJob.Read())
            {
                field = new TemplateField();
                field.HeaderTemplate = new JObDynamicTemplete(ListItemType.Header, DB.RSField(rsGetMaterialListForJob, "MCode"), DB.RSFieldDecimal(rsGetMaterialListForJob, "BOMQuantity"), true); //DB.RSField(rsGetMaterialListForJob,"MCode"); //"MCode"+(++RowNumber);
                //field.ItemTemplate = new JObDynamicTemplete("MCode", RowNumber);
                //gvPanthomMaterial.Columns.Add(field);

                //field = new TemplateField();
               // field.HeaderText = "BatchNo" + RowNumber;
                field.ItemTemplate = new JObDynamicTemplete(ListItemType.Item,"BatchNo", ++RowNumber);
                gvPanthomMaterial.Columns.Add(field);
            }
            rsGetMaterialListForJob.Close();

            field = new TemplateField();
            field.HeaderText = "Possible Phantom Qty.";
            field.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
            field.ItemTemplate = new JObDynamicTemplete(ListItemType.Item,"PossibleQty");
            gvPanthomMaterial.Columns.Add(field);

            field = new TemplateField();
            field.HeaderText = "";
            field.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
            field.ItemTemplate = new JObDynamicTemplete( ListItemType.Item,"CheckBox");
            gvPanthomMaterial.Columns.Add(field);

            field = new TemplateField();
            field.HeaderText = "Require Qty.";
            field.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
            field.ItemTemplate = new JObDynamicTemplete(ListItemType.Item,"TextBox");
            gvPanthomMaterial.Columns.Add(field);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Validate();

            if (!cp.IsInAnyRoles(CommonLogic.GetRolesAllowed(CommonLogic.GetRolesAllowedForthisPage("New Phantom Job Order"))))
            {
                //test case ID (TC_009)
                //Only authorized user can access phantom job order page

                Response.Redirect("Login.aspx?eid=6");
            }
            //Page.Form.DefaultButton = lnkUpdate.UniqueID;
            lnkUpdate.Text = "Create JOB" + CommonLogic.btnfaSave;
            atcPRORefNo.Focus();
            if (!IsPostBack)
            {

                DesignLogic.SetInnerPageSubHeading(this.Page, "Phantom Job Order");

                ViewState["HeaderID"] = 0;
                if (CommonLogic.QueryString("proid") != "" && CommonLogic.QueryString("proid") != "0")
                {
                    if (CommonLogic.QueryString("sucess") == "True")
                    {
                        resetError("Successfully Updated", false, false);
                    }
                    imgKitCode.Visible = false;
                    Build_FormData();
                    
                }
                else
                {
                    if (CommonLogic.QueryString("sucess") == "False")
                    {
                        resetError("Error while Updating ", true, false);
                    }
                    chkIsActive.Checked = true;
                    //atcPRORefNo.Focus();
                    lnkCreateOutBound.Visible = false;
                    pnlPanthomMaterial.Visible = false;
                    lbMaterialCombination.Visible = false;
                }
            }
        }

        private void Build_FormData()
        {
            try
            {
                IDataReader rsPROHeaderData = DB.GetRS("EXEC [dbo].[sp_MFG_GetProductionOrderHeader] @ProductionOrderHeaderID=" + CommonLogic.QueryString("proid"));
                if (rsPROHeaderData.Read())
                {
                    atcPRORefNo.Text = DB.RSField(rsPROHeaderData, "PRORefNo");
                    hifPRORefNo.Value = DB.RSFieldInt(rsPROHeaderData, "MaterialMasterRevisionID").ToString();
                    txtJobRefNo.Text = DB.RSField(rsPROHeaderData, "JobOrderRefNo");
                    //atcSOHeader.Text = DB.RSField(rsPROHeaderData, "SONumber");
                   // hifSOHeader.Value = DB.RSFieldInt(rsPROHeaderData, "SOHeaderID").ToString();
                    ltPROStatus.Text = DB.RSField(rsPROHeaderData, "ProductionOrderStatus");

                    txtStartDate.Text = DB.RSFieldDateTime(rsPROHeaderData, "StartDate").ToString("dd/MM/yyyy");
                    txtDueDate.Text = DB.RSFieldDateTime(rsPROHeaderData, "DueDate").ToString("dd/MM/yyyy");
                    atcRoutingHeaderVersion.Text = DB.RSField(rsPROHeaderData, "RoutingHeaderRevision");
                    txtProductionDate.Text = DB.RSFieldDateTime(rsPROHeaderData, "ProductionOrderDate").ToString("dd/MM/yyyy");
                    hifRoutingHeaderVersion.Value = DB.RSFieldInt(rsPROHeaderData, "RoutingHeaderRevisionID").ToString();
                    atcProductionUoM.Text = DB.RSField(rsPROHeaderData, "UoM");
                   // txtCoCnumber.Text = DB.RSField(rsPROHeaderData, "CoCNumber");
                    hifProductionUoM.Value = DB.RSFieldInt(rsPROHeaderData, "ProductionUoMID").ToString();
                    txtProductionQuantity.Text = DB.RSFieldDecimal(rsPROHeaderData, "ProductionQuantity").ToString();
                    txtRemarks.Text = DB.RSField(rsPROHeaderData, "Remarks");




                    //hifQAAcceptanceBy.Value = DB.RSFieldInt(rsPROHeaderData, "QAAcceptanceByID").ToString();
                    //atcQAAcceptanceBy.Text = DB.RSField(rsPROHeaderData, "QAAcceptanceBy");
                    //hifproductionType.Value = DB.RSFieldInt(rsPROHeaderData, "ProductionOrderTypeID").ToString();
                    //atcProductionType.Text = DB.RSField(rsPROHeaderData, "ProductionOrderType");
                    //hifSupervisorAcceptanceBy.Value = DB.RSFieldInt(rsPROHeaderData, "SupervisorAcceptanceByID").ToString();
                    //atcSupervisorAcceptanceBy.Text = DB.RSField(rsPROHeaderData, "SupervisorAcceptanceBy");
                    //hifShopAcceptanceby.Value = DB.RSFieldInt(rsPROHeaderData, "ShopAcceptanceByID").ToString();
                    //atcShopAcceptanceby.Text = DB.RSField(rsPROHeaderData, "ShopAcceptanceBy");
                    txtManufacturingDate.Text = DB.RSFieldDateTime(rsPROHeaderData, "DateofManufacturing").ToString("dd/MM/yyyy");
                    txtkitCode.Text = DB.RSField(rsPROHeaderData, "KitCode");
                    //txtAddtionalObservations.Text = DB.RSField(rsPROHeaderData, "AddtionalObservations");
                    chkIsActive.Checked = DB.RSFieldBool(rsPROHeaderData, "IsActive");
                    chkIsDeleted.Checked = DB.RSFieldBool(rsPROHeaderData, "IsDeleted");

                    ViewState["ProductionOrderDetailsList"] = "EXEC [dbo].[sp_MFG_GetProductionOrderDetails] @ProductionOrderHeaderID=" + CommonLogic.QueryString("proid");
                    ViewState["HeaderID"] = CommonLogic.QueryString("proid");
                    lnkUpdate.Text = "Update" + CommonLogic.btnfaUpdate ;
                    if (DB.RSFieldInt(rsPROHeaderData, "ProductionOrderStatusID") >= 2)
                    {

                        atcPRORefNo.Enabled = false;
                        txtProductionDate.Enabled = false;
                        txtDueDate.Enabled = false;
                        txtkitCode.Enabled = false;
                        atcProductionUoM.Enabled = false;
                        txtProductionQuantity.Enabled = false;
                        atcRoutingHeaderVersion.Enabled = false;
                        chkIsActive.Enabled = false;
                        chkIsDeleted.Enabled = false;
                        txtStartDate.Enabled = false;
                        txtManufacturingDate.Enabled = false;
                        chkIsDeleted.Enabled = false;
                        lnkUpdate.Enabled = false;
                        txtJobRefNo.Enabled = false;
                        lnkCreateOutBound.Visible = false;
                        pnlPanthomMaterial.Visible = false;
                        lbMaterialCombination.Visible = false;
                        
                    }
                    else
                    {
                        Build_CombinationMaterial(Build_CombinationMaterial());
                        if (gvPanthomMaterial.Rows.Count == 0)
                        {
                            lbMaterialCombination.Text = "Items not available in Store";
                            pnlPanthomMaterial.Visible = false;
                            lnkCreateOutBound.Visible = false;
                        }
                        else
                        {
                            lbMaterialCombination.Text = lbMaterialCombination.Text + " ["+gvPanthomMaterial.Rows.Count+"]";
                        }
                    }


                }
                rsPROHeaderData.Close();
            }
            catch (Exception ex)
            {
                resetError("Error while Loading data", true, false);
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
            }
        }

        private DataSet Build_CombinationMaterial()
        {
            DataSet dsGetMaterialCombination = null;
            String cMdGetMaterialCombination = "dbo.sp_MFG_GetPhanthomCombinations " + CommonLogic.QueryString("proid");
            try
            {
                dsGetMaterialCombination = DB.GetDS(cMdGetMaterialCombination, false);
            }
            catch (Exception ex)
            {
                resetError("Error while loading", true, false);
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
            }
            return dsGetMaterialCombination;
        }

        private void Build_CombinationMaterial(DataSet dsGetMaterialCombination)
        {
            gvPanthomMaterial.DataSource = dsGetMaterialCombination;
            gvPanthomMaterial.DataBind();
            dsGetMaterialCombination.Dispose();
        }

        protected void lnkUpdate_Click(object sender, EventArgs e)
        {
            //test case ID (TC_028)
            //Update user enter details

            Page.Validate("save");

            if (!IsValid)
            {
                return;
            }
                      

            StringBuilder cmdupdateProheader = new StringBuilder();
            cmdupdateProheader.Append("Declare @NewHeaderID int ");
            cmdupdateProheader.Append("EXEC [dbo].[sp_MFG_UpsertProductionOrderHeader] ");
            cmdupdateProheader.Append("@ProductionOrderHeaderID=" + ViewState["HeaderID"]);
            cmdupdateProheader.Append(",@MaterialMasterRevisionID=" + hifPRORefNo.Value);
            cmdupdateProheader.Append(",@JobOrderRefNo=" + (txtJobRefNo.Text.Trim() != "" ? DB.SQuote(txtJobRefNo.Text.Trim()) : "NULL"));
            cmdupdateProheader.Append(",@SOHeaderID=NULL");// + (hifSOHeader.Value != "" && atcSOHeader.Text != "" ? hifSOHeader.Value : "NULL"));
            cmdupdateProheader.Append(",@ProductionOrderStatusID=" + 1);
            cmdupdateProheader.Append(",@CoCNumber=null");// + DB.SQuote(txtCoCnumber.Text));
            cmdupdateProheader.Append(",@RoutingHeaderRevisionID=" + hifRoutingHeaderVersion.Value);
            cmdupdateProheader.Append(",@ProductionUoMID=" + hifProductionUoM.Value);
            cmdupdateProheader.Append(",@ProductionQuantity=" + txtProductionQuantity.Text.Trim());

            String StartDate = "";
            if (txtStartDate.Text.Trim() != "")
                StartDate = DB.SQuote(CommonLogic.GetConfigValue(cp.TenantID, "DateFormat") == "dd/MM/yyyy" ? DateTime.ParseExact(txtStartDate.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy") : txtStartDate.Text.Trim());
            else
                StartDate = "NULL";
            cmdupdateProheader.Append(",@StartDate=" + StartDate);
            String DueDate = "";
            if (txtDueDate.Text.Trim() != "")
                DueDate = DB.SQuote(CommonLogic.GetConfigValue(cp.TenantID, "DateFormat") == "dd/MM/yyyy" ? DateTime.ParseExact(txtDueDate.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy") : txtDueDate.Text.Trim());
            else
                DueDate = "NULL";
            String ProductionDate = "";
            if (txtProductionDate.Text.Trim() != "")
                ProductionDate = DB.SQuote(CommonLogic.GetConfigValue(cp.TenantID, "DateFormat") == "dd/MM/yyyy" ? DateTime.ParseExact(txtProductionDate.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy") : txtProductionDate.Text.Trim());
            else
                ProductionDate = "NULL";
            String ManufacturingDate = "";
            if (txtManufacturingDate.Text.Trim() != "")
                ManufacturingDate = DB.SQuote(CommonLogic.GetConfigValue(cp.TenantID, "DateFormat") == "dd/MM/yyyy" ? DateTime.ParseExact(txtManufacturingDate.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy") : txtManufacturingDate.Text.Trim());
            else
                ManufacturingDate = "NULL";
            cmdupdateProheader.Append(",@DateofManufacturing=" + ManufacturingDate);
            cmdupdateProheader.Append(",@CoCDate=null");
            cmdupdateProheader.Append(",@ContractNo=null");
            cmdupdateProheader.Append(",@ProductionOrderTypeID=7");// + (atcProductionType.Text != "" && hifproductionType.Value != "" ? hifproductionType.Value : "NULL"));
            cmdupdateProheader.Append(",@KitCode=" + (txtkitCode.Text != "" ? DB.SQuote(txtkitCode.Text) : "NULL"));
            cmdupdateProheader.Append(",@ShopAcceptanceByID=Null");// + (atcShopAcceptanceby.Text != "" && hifShopAcceptanceby.Value != "" ? hifShopAcceptanceby.Value : "NULL"));
            cmdupdateProheader.Append(",@SupervisorAcceptanceByID=null");// + (atcSupervisorAcceptanceBy.Text != "" && hifSupervisorAcceptanceBy.Value != "" ? hifSupervisorAcceptanceBy.Value : "Null"));
            cmdupdateProheader.Append(",@QAAcceptanceByID=null");// + (atcQAAcceptanceBy.Text != "" && hifQAAcceptanceBy.Value != "" ? hifQAAcceptanceBy.Value : "NULL"));
            cmdupdateProheader.Append(",@AddtionalObservations=null");// + (txtAddtionalObservations.Text != "" ? DB.SQuote(txtAddtionalObservations.Text) : "NULL"));
            cmdupdateProheader.Append(",@ProductionOrderDate=" + ProductionDate);
            cmdupdateProheader.Append(",@DueDate=" + DueDate);
            cmdupdateProheader.Append(",@Remarks=" + (txtRemarks.Text.Trim() != "" ? DB.SQuote(txtRemarks.Text.Trim()) : "NULL"));
            cmdupdateProheader.Append(",@CreatedBy=" + cp.UserID);
            cmdupdateProheader.Append(",@LastModifiedBy=" + cp.UserID);
            cmdupdateProheader.Append(",@IsActive=" + Convert.ToInt16(chkIsActive.Checked));
            cmdupdateProheader.Append(",@IsDeleted=" + Convert.ToInt16(chkIsDeleted.Checked));
            cmdupdateProheader.Append(",@NewProductionOrderHeaderID=@NewHeaderID output Select @NewHeaderID as N");
            Boolean updateStatus = false;
            Boolean IsDeleted = chkIsDeleted.Checked;
            try
            {
                ViewState["HeaderID"] = DB.GetSqlN(cmdupdateProheader.ToString());
                lnkUpdate.Text = "Update"+CommonLogic.btnfaUpdate;
                updateStatus = true;
                //Response.Redirect("ProductionOrder.aspx?proid=" + ViewState["HeaderID"].ToString()+"&sucess=true");
                // ViewState["ProductionOrderDetailsList"] = "EXEC [dbo].[sp_MFG_GetProductionOrderDetails] @ProductionOrderHeaderID=" + ViewState["HeaderID"];
                // resetError("Update successfully",false);

            }
            catch (SqlException sqlex)
            {
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), sqlex.Source, sqlex.Message, sqlex.StackTrace);
                if (sqlex.Message.StartsWith("Cannot insert duplicate key row in object 'dbo.MFG_ProductionOrderHeader'"))
                    resetError("Job ref.No.,RoutingRevision and KitCode Combination already exists,cannot update", true, false);
                else
                    resetError("Error while updating", true, false);
                return;
            }
            catch (Exception ex)
            {
                updateStatus = false;
                // Response.Redirect("ProductionOrder.aspx?proid=" + ViewState["HeaderID"].ToString() + "&sucess=false");
                resetError("Error while updating", true, false);
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
                return;
            }
            if (IsDeleted == true)
            {
                Response.Redirect("ProductionOrderList.aspx");
            }
            Response.Redirect("PhantomJobOrder.aspx?proid=" + ViewState["HeaderID"] + "&sucess=" + updateStatus.ToString());
        }

        protected void resetError(string error, bool isError, bool ispermanent)
        {

            /*string str = "<font class=\"noticeMsg\">NOTICE:</font>&nbsp;&nbsp;&nbsp;";
            if (isError)
                str = "<font class=\"errorMsg\">ERROR:</font>&nbsp;&nbsp;&nbsp;";

            if (error.Length > 0)
                str += error + "";
            else
                str = "";


            lblStatus.Text = str;*/
            ScriptManager.RegisterStartupScript(this, this.GetType(), "test", "closeAsynchronus(); showStickyToast(" + (isError.ToString().ToLower() == "true" ? "false" : "true") + ",\"" + error + "\"," + ispermanent.ToString().ToLower() + ");", true);

        }

        protected void lnkCreateOutBound_Click(object sender, EventArgs e)
        {
            //test case ID (TC_037)
            //create outbound and update combinations of material with SOHeader


            StringBuilder MaterialDetails = new StringBuilder();
            String OBDNumber;
            String SONumber;
            bool Isselected = false;
            if (!CheckMaterialQtyValid())
            {
                return;
            }
            int MaterialCount = (gvPanthomMaterial.Columns.Count - 4);// / 2;
            foreach (GridViewRow row in gvPanthomMaterial.Rows)
            {
                
                if (((CheckBox)row.FindControl("chkCheck")).Checked && ((TextBox)row.FindControl("txtQty")).Text != "")
                {
                    if (Convert.ToDecimal(((TextBox)row.FindControl("txtQty")).Text) > Convert.ToDecimal(((Literal)row.FindControl("PossibleQty")).Text))
                    {
                        //test case ID (TC_040)
                        //Require qty. should be less than or equal to possible phantom qty.

                        resetError("Entered quantity is more than the PossibleQty",true,false);
                        return;
                    }
                    Isselected = true;
                    for (int Matind = 1; Matind <= MaterialCount; Matind++)
                    {
                        MaterialDetails.Append(((HiddenField)row.FindControl("MaterialMasterID" + Matind)).Value + "!");
                        MaterialDetails.Append((Convert.ToDecimal(((TextBox)row.FindControl("txtQty")).Text) * Convert.ToDecimal(((HiddenField)row.FindControl("BOMQty" + Matind)).Value)).ToString() + "!");
                        MaterialDetails.Append(((Label)row.FindControl("BatchNo" + Matind)).Text + "|");
                    }
                    MaterialDetails.Append(",");
                }
            }
            if (!Isselected)
            {
                //test case ID (TC_038)
                //Select minumum one combination

                resetError("Select minimum one combination",true,false);
                return;
            }
            try
            {
                OBDNumber = GetOBDNumber();

                SONumber = GetSONumber();

                String CUSPONumber = GetCUSPONumber();

                StringBuilder cMdUpdatePhantomMaterials = new StringBuilder();
                cMdUpdatePhantomMaterials.Append("DECLARE @NewUpdateOutboundID int; ");
                cMdUpdatePhantomMaterials.Append("EXEC [dbo].[sp_MFG_DummyOutboundForPhantom] ");
                cMdUpdatePhantomMaterials.Append("@ProductionOrderHeaderID="+ViewState["HeaderID"]);
                cMdUpdatePhantomMaterials.Append(",@SONumber="+DB.SQuote(SONumber));
                cMdUpdatePhantomMaterials.Append(",@CusPONumber="+DB.SQuote(CUSPONumber));
                cMdUpdatePhantomMaterials.Append(",@OBDNumber="+DB.SQuote(OBDNumber));
                cMdUpdatePhantomMaterials.Append(",@MaterialDetails="+DB.SQuote(MaterialDetails.ToString()));
                cMdUpdatePhantomMaterials.Append(",@CreatedBy="+cp.UserID);
                cMdUpdatePhantomMaterials.Append(",@NewOutboundID=@NewUpdateOutboundID OUTPUT; select @NewUpdateOutboundID AS N");


                if (OBDNumber != "" && SONumber != "" && CUSPONumber != "")
                {
                    if (DB.GetSqlN(cMdUpdatePhantomMaterials.ToString()) != 0)
                    {
                        gvPanthomMaterial.Enabled = false;
                        lnkCreateOutBound.Visible = false;
                        atcPRORefNo.Enabled = false;
                        txtProductionDate.Enabled = false;
                        txtDueDate.Enabled = false;
                        txtkitCode.Enabled = false;
                        atcProductionUoM.Enabled = false;
                        txtProductionQuantity.Enabled = false;
                        atcRoutingHeaderVersion.Enabled = false;
                        chkIsActive.Enabled = false;
                        chkIsDeleted.Enabled = false;
                        txtStartDate.Enabled = false;
                        txtManufacturingDate.Enabled = false;
                        chkIsDeleted.Enabled = false;
                        lnkCreateOutBound.Visible = false;
                        lnkUpdate.Enabled = false;
                        resetError("Successfully updated with OBD number :" + OBDNumber, false, true);
                    }
                    else
                    {
                        resetError("Error while updating",true,false);
                    }
                }
            }
            catch
            {
                resetError("Error while updating",true,false);
            }


        }

        private bool CheckMaterialQtyValid()
        {
            List<MaterialDetails> MaterialList = new List<MaterialDetails>();
            int MaterialCount = (gvPanthomMaterial.Columns.Count - 4);// / 2;
            bool isExist = true;
            int MaterialMasterID;
            String BatchNo;
            Decimal TotalQty;
            Decimal ReqQty;
            Decimal BoMQty;


            foreach (GridViewRow row in gvPanthomMaterial.Rows)
            {
                if (((CheckBox)row.FindControl("chkCheck")).Checked && ((TextBox)row.FindControl("txtQty")).Text != "")
                {
                    ReqQty=Convert.ToDecimal(((TextBox)row.FindControl("txtQty")).Text);

                    for (int Matind = 1; Matind <= MaterialCount; Matind++)
                    {
                        MaterialMasterID=Convert.ToInt32(((HiddenField)row.FindControl("MaterialMasterID"+Matind)).Value);
                        BatchNo=((Label)row.FindControl("BatchNo"+Matind)).Text;
                        TotalQty = Convert.ToDecimal(((HiddenField)row.FindControl("TotalQty"+Matind)).Value);
                        BoMQty=Convert.ToDecimal(((HiddenField)row.FindControl("BoMQty"+Matind)).Value);
                        foreach (MaterialDetails MaterialInfo in MaterialList)
                        {
                            if(MaterialInfo.MaterialMasterID==MaterialMasterID && MaterialInfo.BatchNo==BatchNo)
                            {
                                if(BoMQty*ReqQty+MaterialInfo.RequreQty>MaterialInfo.TotalQty)
                                {
                                    resetError("Selected quantity of material with BatchNo: " + BatchNo + " is more than available quantity", true, false);
                                    return false;
                                }
                            }
                        }

                    }

                    for (int Matind = 1; Matind <= MaterialCount; Matind++)
                    {
                        MaterialMasterID=Convert.ToInt32(((HiddenField)row.FindControl("MaterialMasterID"+Matind)).Value);
                        BatchNo=((Label)row.FindControl("BatchNo"+Matind)).Text;
                        BoMQty=Convert.ToDecimal(((HiddenField)row.FindControl("BoMQty"+Matind)).Value);
                        TotalQty = Convert.ToDecimal(((HiddenField)row.FindControl("TotalQty" + Matind)).Value);
                        isExist = false;
                        //MaterialList.ForEach(delegate(MaterialDetails MaterialInfo)
                        foreach(MaterialDetails MaterialInfo in MaterialList)
                        {
                            if(MaterialInfo.MaterialMasterID==MaterialMasterID && MaterialInfo.BatchNo==BatchNo)
                            {
                                if(BoMQty*ReqQty+MaterialInfo.RequreQty<=MaterialInfo.TotalQty)
                                {
                                   MaterialInfo.RequreQty+=BoMQty*ReqQty;
                                   isExist = true;
                                    break;
                                }
                            }
                        }
                        if (!isExist)
                        {
                            MaterialList.Add(new MaterialDetails(MaterialMasterID, BatchNo, TotalQty, BoMQty * ReqQty));
                        }

                    }



                }
            }
            return true;
        }

        private String GetSONumber()
        {
            String SONumber = "";
            try
            {
                String NewSONumber = DB.GetSqlS("EXEC [sp_SYS_GetSystemConfigValue]   @SysConfigKey='dummysoheaderforproduction_prefix' ,@TenantID=" + cp.TenantID); //DB.GetSqlS("select SysConfigValue as S from SYS_SystemConfiguration where TenantID=" + cp.TenantID + " and SysConfigKeyID= (select SysConfigKeyID from SYS_SysConfigKey where SysConfigKey=N'salesorder.aspx.cs.SO_Prefix') ");
                NewSONumber += "" + (Convert.ToInt16(DateTime.Now.Year) % 100);           //add year code to ponumber

                int length = Convert.ToInt16(DB.GetSqlS("EXEC [sp_SYS_GetSystemConfigValue] @SysConfigKey='salesorder.aspx.cs.SO_Length' ,@TenantID=" + cp.TenantID));// Convert.ToInt32(DB.GetSqlS("select SysConfigValue as S from SYS_SystemConfiguration as N where TenantID=" + cp.TenantID + " and SysConfigKeyID= (select SysConfigKeyID from SYS_SysConfigKey where SysConfigKey=N'salesorder.aspx.cs.SO_Length') "));

                String OldPONumber = DB.GetSqlS("select TOP 1 SONumber as S from ORD_SOHeader where SONumber like '" + NewSONumber + "%'  ORDER BY SONumber desc ");


                int power = (Int32)Math.Pow((double)10, (double)(length - 1));          //getting minvalue of prifix length

                String newvalue = "";
                if (OldPONumber != "" && NewSONumber.Equals(OldPONumber.Substring(0, NewSONumber.Length)))        //if ponumber is existed and same year ponumber  enter
                {
                    String temp = OldPONumber.Substring(NewSONumber.Length, length);                            //getting number of last prifix
                    Int32 number = Convert.ToInt32(temp);
                    number++;


                    while (power > 1)                                                                           //add '0' to number at left side for get 
                    {
                        if (number / power > 0)
                        {
                            break;
                        }
                        newvalue += "0";
                        power /= 10;
                    }
                    newvalue += number;
                }
                else
                {                                                                                           //other wise generate first number 
                    for (int i = 0; i < length - 1; i++)
                        newvalue += "0";
                    newvalue += "1";
                }

                NewSONumber += newvalue;
                SONumber = NewSONumber;
            }
            catch (Exception ex)
            {
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
            }
            return SONumber;
        }

        private String GetOBDNumber()
        {
            String OBDNumber = "";
            try
            {

                int length = Convert.ToInt32(DB.GetSqlS("EXEC [sp_SYS_GetSystemConfigValue]   @SysConfigKey='OutboundDetails.aspx' ,@TenantID=" + cp.TenantID));

                

                String NewOBDNumber = DB.GetSqlS("EXEC [sp_SYS_GetSystemConfigValue]   @SysConfigKey='dummyoutboundforproduction_prefix' ,@TenantID=" + cp.TenantID) + (Convert.ToInt16(DateTime.Now.Year) % 100);           //add year code to ponumber

                String OldOBDNumber = DB.GetSqlS("select top 1 OBDNumber AS S from OBD_Outbound where OBDNumber like '" + NewOBDNumber + "%' order by OBDNumber desc");

                int power = (Int32)Math.Pow((double)10, (double)(length - 1));          //getting minvalue of prifix length

                String newvalue = "";
                if (OldOBDNumber != "" && NewOBDNumber.Equals(OldOBDNumber.Substring(0, NewOBDNumber.Length)))        //if ponumber is existed and same year ponumber  enter
                {
                    String temp = OldOBDNumber.Substring(NewOBDNumber.Length, length);                            //getting number of last prifix
                    Int32 number = Convert.ToInt32(temp);
                    number++;


                    while (power > 1)                                                                           //add '0' to number at left side for get 
                    {
                        if (number / power > 0)
                        {
                            break;
                        }
                        newvalue += "0";
                        power /= 10;
                    }
                    newvalue += number;
                }
                else
                {                                                                                           //other wise generate first number 
                    for (int i = 0; i < length - 1; i++)
                        newvalue += "0";
                    newvalue += "1";
                }

                NewOBDNumber += newvalue;
                OBDNumber = NewOBDNumber;
            }
            catch (Exception ex)
            {
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
            }
            return OBDNumber;
        }

        private String GetCUSPONumber()
        {
            Random generateNo = new Random();
            return "DCUSPO" + generateNo.Next(1000, 10000);
        }

        protected void imgKitCode_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                String NewPHNumber = DB.GetSqlS("EXEC sp_SYS_GetSystemConfigValue @SysConfigKey=N'phantomkitcode_prefix',@TenantID=" + cp.TenantID);

                int length = Convert.ToInt32(DB.GetSqlS("EXEC sp_SYS_GetSystemConfigValue @SysConfigKey=N'phantomkitcode_prefix_length',@TenantID=" + cp.TenantID));
                NewPHNumber += "" + (Convert.ToInt16(DateTime.Now.Year) % 100); 

                //string HeaderID = ViewState["HeaderID"].ToString();
                String OldPHNumber = DB.GetSqlS("select top 1 KitCode as S from MFG_ProductionOrderHeader where KitCode like '" + NewPHNumber + "%' order by KitCode desc");

                         //add year code to ponumber

                int power = (Int32)Math.Pow((double)10, (double)(length - 1));          //getting minvalue of prifix length

                String newvalue = "";
                if (OldPHNumber != "" && NewPHNumber.Equals(OldPHNumber.Substring(0, NewPHNumber.Length)))        //if ponumber is existed and same year ponumber  enter
                {
                    String temp = OldPHNumber.Substring(NewPHNumber.Length, length);                            //getting number of last prifix
                    Int32 number = Convert.ToInt32(temp);
                    number++;


                    while (power > 1)                                                                           //add '0' to number at left side for get 
                    {
                        if (number / power > 0)
                        {
                            break;
                        }
                        newvalue += "0";
                        power /= 10;
                    }
                    newvalue += number;
                }
                else
                {                                                                                           //other wise generate first number 
                    for (int i = 0; i < length - 1; i++)
                        newvalue += "0";
                    newvalue += "1";
                }

                NewPHNumber += newvalue;
                txtkitCode.Text = NewPHNumber;
            }
            catch (Exception ex)
            {
                resetError("Error while generating IONumber", true, false);
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
            }
            //txtProductionDate.Focus();
            txtStartDate.Focus();
        }

        protected void lnkClear_Click(object sender, EventArgs e)
        {
            Response.Redirect("ProductionOrderList.aspx");
        }

    }

    public class MaterialDetails
    {
        public MaterialDetails(int materialMasterID, String batchNo, Decimal totalQty, Decimal requreQty)
        {
            MaterialMasterID = materialMasterID;
            BatchNo = batchNo;
            TotalQty = totalQty;
            RequreQty = requreQty;
        }
        public int MaterialMasterID;
        public String BatchNo;
        public Decimal TotalQty;
        public Decimal RequreQty;
    }
}