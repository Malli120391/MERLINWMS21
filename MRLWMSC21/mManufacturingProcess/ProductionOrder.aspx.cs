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
using System.Globalization;
using System.Web.UI.HtmlControls;

namespace MRLWMSC21.mManufacturingProcess
{
    //Author    :   Gvd Prasad
    //Created On:   04-Feb-2014
    //Use case ID:  Job Order_UC_010

    public partial class ProductionOrder : System.Web.UI.Page
    {
        public CustomPrincipal cp = HttpContext.Current.User as CustomPrincipal;

        protected void page_Init(object sender, EventArgs e)
        {
            if (CommonLogic.QueryString("proid") != "")
            {
                AddWorkOrderReport();
            }
        }

        protected void Page_PreInit(object sender, EventArgs e)
        {
            Page.Theme = "Inbound";

        }

        protected void Page_Load(object sender, EventArgs e)
        {
             Page.Validate();

             if (!cp.IsInAnyRoles(CommonLogic.GetRolesAllowed(CommonLogic.GetRolesAllowedForthisPage("New Job Order"))))
            {
                //test case ID (TC_009)
                //Only authorized user can access job order page

                Response.Redirect("Login.aspx?eid=6");
            }
            Page.Form.DefaultButton = lnkUpdate.UniqueID;
           
            //atcPRORefNo.Focus();
            if (!IsPostBack)
            {
                lnkUpdate.Text = "Create JOB" + CommonLogic.btnfaSave;
                

                DesignLogic.SetInnerPageSubHeading(this.Page, "Job Order");

                ViewState["HeaderID"] = 0;
                if (CommonLogic.QueryString("proid") != "" && CommonLogic.QueryString("proid") != "0")
                {
                    if (CommonLogic.QueryString("sucess") == "True")
                    {
                        resetError("Successfully Updated", false, false);
                    }
                    lnkworkstationconfiguration.Visible = true;
                    Build_FormData();
                    CheckJobOrderQty();
                   this.gvProdDefcList_buildGridData(this.gvProdDefcList_buildGridData());
                   lnkUpdate.Text = "Update" + CommonLogic.btnfaUpdate;
                    
                }
                else
                {
                    if(CommonLogic.QueryString("sucess") == "False")
                    {
                        resetError("Error while Updating ", true, false);
                    }
                    lnkMaterialDeficiency.Visible = false;
                    tdJoborderDefc.Visible = false;
                    lnkReleaseJobOrder.Visible = false;
                    chkIsActive.Checked = true;
                    lnkAddNewLineItem.Visible = false;
                    //atcMaterialRevision.Focus();
                    lnkUpdate.Text = "Create JOB" + CommonLogic.btnfaSave;

                }
               // lmbWorkOrderReport.Enabled = false;
                tdlineitems.Visible = false;
                //txtkitCode.Enabled = false;
                //imgKitCode.Enabled = false;
                //imgKitCode.Visible = false;
            }
        }

        private void CheckJobOrderQty()
        {
            IDataReader rsProductionInfo=null;
            try
            {
                rsProductionInfo = DB.GetRS("sp_MFG_ProductionOrderVerification	@ProductionOrderHeaderID=" + CommonLogic.QueryString("proid"));
            }
            catch (Exception ex)
            {
                resetError("Error while loading",true,false);
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
                return;
            }

            if (rsProductionInfo.Read())
            {
                if (DB.RSFieldInt(rsProductionInfo, "SOPO_ProductionOrderID") == 0 && (DB.RSFieldInt(rsProductionInfo, "RoutingDetailsActivity_MaterilaMasterID") == 0 || DB.RSFieldInt(rsProductionInfo, "ProductionOrder_WorkCenterID") == 0))
                {
                    lnkMaterialDeficiency.Visible = false;
                    tdJoborderDefc.Visible = false;
                    //lnkReleaseJobOrder.Visible = false;
                    if (DB.RSFieldInt(rsProductionInfo, "RoutingDetailsActivity_MaterilaMasterID") == 0)
                    {
                        resetDialogBoxError("Materials are not configure in Routing", true);
                    }
                    else
                    {
                        resetDialogBoxError("Workstation is not configured", true);
                    }
                }
                else if (DB.RSFieldInt(rsProductionInfo, "SOPO_ProductionOrderID") == 0 && DB.RSFieldTinyInt(rsProductionInfo, "IsDeficiency") == 1)
                {
                    lnkMaterialDeficiency.Visible = true;
                    tdJoborderDefc.Visible = true;
                    if (DB.RSFieldTinyInt(rsProductionInfo, "IsAvailableQty") == 0)
                        //lnkReleaseJobOrder.Visible = false;
                        lnkUpdatePendingMaterial.Enabled = false;
                    else
                        //lnkReleaseJobOrder.Visible = true;
                        lnkUpdatePendingMaterial.Enabled = true;
                }
                else if (DB.RSFieldInt(rsProductionInfo, "SOPO_ProductionOrderID") == 0)
                {
                    lnkMaterialDeficiency.Visible = false;
                    tdJoborderDefc.Visible = false;
                    //lnkReleaseJobOrder.Visible = true;
                    lnkUpdatePendingMaterial.Enabled = true;
                }
                else if (DB.RSFieldInt(rsProductionInfo, "SOPO_ProductionOrderID") != 0)
                {
                    lnkMaterialDeficiency.Visible = false;
                    tdJoborderDefc.Visible = false;
                    //lnkReleaseJobOrder.Visible = false;
                    txtJobRefNo.Enabled = false;

                    atcMaterialRevision.Enabled = false;
                    atcSOHeader.Enabled = false;
                    txtProductionDate.Enabled = false;
                    txtDueDate.Enabled = false;
                    txtkitCode.Enabled = false;
                    //txtCoCnumber.Enabled = false;
                    atcProductionUoM.Enabled = false;
                    txtProductionQuantity.Enabled = false;
                    atcRoutingHeaderVersion.Enabled = false;
                    
                    chkIsActive.Enabled = false;
                    chkIsDeleted.Enabled = false;
                    txtStartDate.Enabled = false;
                    //txtManufacturingDate.Enabled = false;
                    atcProductionType.Enabled = false;
                    chkIsDeleted.Enabled = false;
                   // imgKitCode.Visible = false;
                }

                if (DB.RSFieldInt(rsProductionInfo, "RoutingDetailsActivityCaptureID") != 0)
                {
                    lnkAssignWorkOrder.Visible = false;
                }
            }
            rsProductionInfo.Close();

        }

        #region --------- Basic Data  -------------------------------

        private void Build_FormData()
        {
            IDataReader rsPROHeaderData = null;
            try
            {
                rsPROHeaderData = DB.GetRS("EXEC [dbo].[sp_MFG_GetProductionOrderHeader] @ProductionOrderHeaderID=" + CommonLogic.QueryString("proid"));
            }
            catch (Exception ex)
            {
                resetError("Error while Loading data", true, false);
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
                return;
            }
                if (rsPROHeaderData.Read())
                {
                    ltRoutingno.Text = DB.RSField(rsPROHeaderData, "RoutingRefNo");
                    atcMaterialRevision.Text = DB.RSField(rsPROHeaderData, "PRORefNo");
                    hifMaterialRevision.Value = DB.RSFieldInt(rsPROHeaderData, "MaterialMasterRevisionID").ToString();
                    txtJobRefNo.Text = DB.RSField(rsPROHeaderData, "JobOrderRefNo");

                    atcSOHeader.Text = DB.RSField(rsPROHeaderData, "SONumber");
                    hifSOHeader.Value = DB.RSFieldInt(rsPROHeaderData, "SOHeaderID").ToString();
                    ltPROStatus.Text = DB.RSField(rsPROHeaderData, "ProductionOrderStatus");
                    ViewState["RoutingType"] = DB.RSField(rsPROHeaderData, "RoutingDocumentType");

                    txtStartDate.Text = DB.RSFieldDateTime(rsPROHeaderData, "StartDate").ToString("dd/MM/yyyy");
                    txtDueDate.Text = DB.RSFieldDateTime(rsPROHeaderData, "DueDate").ToString("dd/MM/yyyy");
                    atcRoutingHeaderVersion.Text = DB.RSField(rsPROHeaderData, "RoutingHeaderRevision");
                    txtProductionDate.Text = DB.RSFieldDateTime(rsPROHeaderData, "ProductionOrderDate").ToString("dd/MM/yyyy");
                    hifRoutingHeaderVersion.Value = DB.RSFieldInt(rsPROHeaderData, "RoutingHeaderRevisionID").ToString();
                    atcProductionUoM.Text = DB.RSField(rsPROHeaderData, "UoM");
                    txtCoCnumber.Text = DB.RSField(rsPROHeaderData, "CoCNumber");
                    hifProductionUoM.Value = DB.RSFieldInt(rsPROHeaderData, "ProductionUoMID").ToString();
                    txtProductionQuantity.Text = DB.RSFieldDecimal(rsPROHeaderData, "ProductionQuantity").ToString();
                    txtRemarks.Text = DB.RSField(rsPROHeaderData, "Remarks");

                    hifQAAcceptanceBy.Value = DB.RSFieldInt(rsPROHeaderData, "QAAcceptanceByID").ToString();
                    atcQAAcceptanceBy.Text = DB.RSField(rsPROHeaderData, "QAAcceptanceBy");
                    hifproductionType.Value = DB.RSFieldInt(rsPROHeaderData, "ProductionOrderTypeID").ToString();
                    atcProductionType.Text = DB.RSField(rsPROHeaderData, "ProductionOrderType");
                    hifSupervisorAcceptanceBy.Value = DB.RSFieldInt(rsPROHeaderData, "SupervisorAcceptanceByID").ToString();
                    atcSupervisorAcceptanceBy.Text = DB.RSField(rsPROHeaderData, "SupervisorAcceptanceBy");
                    hifShopAcceptanceby.Value = DB.RSFieldInt(rsPROHeaderData, "ShopAcceptanceByID").ToString();
                    atcShopAcceptanceby.Text = DB.RSField(rsPROHeaderData, "ShopAcceptanceBy");
                    txtManufacturingDate.Text = (DB.RSFieldDateTime(rsPROHeaderData, "DateofManufacturing") != DateTime.MinValue ? DB.RSFieldDateTime(rsPROHeaderData, "DateofManufacturing").ToString("dd/MM/yyyy") : "");
                    txtkitCode.Text = DB.RSField(rsPROHeaderData, "KitCode");
                    txtAddtionalObservations.Text = DB.RSField(rsPROHeaderData, "AddtionalObservations");
                    //chkIsActive.Checked = Convert.ToBoolean(DB.RSFieldTinyInt(rsPROHeaderData, "IsActive"));
                    //chkIsDeleted.Checked = Convert.ToBoolean(DB.RSFieldTinyInt(rsPROHeaderData, "IsDeleted"));
                    lbqty.Text = DB.RSFieldDecimal(rsPROHeaderData, "ProductionQuantity").ToString();
                    ltprorefno.Text = DB.RSField(rsPROHeaderData, "PRORefNo");
                    txtContractNo.Text = DB.RSField(rsPROHeaderData, "ContractNo");
                    txtCoCDate.Text = (DB.RSFieldDateTime(rsPROHeaderData, "CoCDate") != DateTime.MinValue ? DB.RSFieldDateTime(rsPROHeaderData, "CoCDate").ToString("dd/MM/yyyy") : "");
                    chkIsActive.Checked = DB.RSFieldBool(rsPROHeaderData, "IsActive");
                    chkIsDeleted.Checked = DB.RSFieldBool(rsPROHeaderData, "IsDeleted");
                    hifStatus.Value = DB.RSFieldInt(rsPROHeaderData, "ProductionOrderStatusID").ToString();
                    if (DB.RSFieldInt(rsPROHeaderData, "ProductionOrderStatusID") == 2)
                    {
                        lnkChangeStatus.Visible = true;
                        lnkChangeStatus.Text = "On Hold " + CommonLogic.btnfaTransfer;
                        lnkChangeStatus.OnClientClick = "return confirm('Are you sure, you want to change the job order status to \\'On Hold\\'?');";
                    }
                    else if (DB.RSFieldInt(rsPROHeaderData, "ProductionOrderStatusID") == 4)
                    {
                        lnkChangeStatus.Visible = true;
                        lnkChangeStatus.Text = "On Process " + CommonLogic.btnfaTransfer;
                        lnkChangeStatus.OnClientClick = "return confirm('Are you sure, you want to change the job order status to \\'On Process\\'?');";
                    }

                    ViewState["ProductionOrderDetailsList"] = "EXEC [dbo].[sp_MFG_GetProductionOrderDetails] @ProductionOrderHeaderID=" + CommonLogic.QueryString("proid");
                    ViewState["HeaderID"] = CommonLogic.QueryString("proid");
                    IbutNew.Visible = false;
                    tdlineitems.Visible = true;
                 //   Build_ProductionOrderDetailsList(Build_ProductionOrderDetailsList());
                    lnkUpdate.Text = "Update"+CommonLogic.btnfaUpdate;


                    //if (cp.IsInAnyRoles("28") && DB.RSFieldInt(rsPROHeaderData, "ProductionOrderStatusID") == 2)
                    //{
                    //    ImgChangeRevision.Visible = true;
                        
                    //}

                }
                rsPROHeaderData.Close();
            
        }

        private DataSet Build_ProductionOrderDetailsList()
        {
            DataSet dsProductionOrderDetailslist = null;
            String cmdProductionOrderDetailsList = ViewState["ProductionOrderDetailsList"].ToString();
            try
            {
                dsProductionOrderDetailslist = DB.GetDS(cmdProductionOrderDetailsList, false);
            }
            catch (Exception ex)
            {
                resetError("Error while loading",true,false);
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
            }
            return dsProductionOrderDetailslist;
        }

        private void Build_ProductionOrderDetailsList(DataSet dsProductionOrderDetailslist)
        {
            gvPRODetailsList.DataSource = dsProductionOrderDetailslist;
            gvPRODetailsList.DataBind();
            dsProductionOrderDetailslist.Dispose();
        }

        protected void IbutNew_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                String NewPRONumber = DB.GetSqlS("EXEC sp_SYS_GetSystemConfigValue @SysConfigKey=N'productionorder.aspx.cs.PRO_Prefix',@TenantID=" + cp.TenantID);

                int length = Convert.ToInt32(DB.GetSqlS("EXEC sp_SYS_GetSystemConfigValue @SysConfigKey=N'productionorder.aspx.cs.PRO_Length',@TenantID=" + cp.TenantID));

                NewPRONumber += "" + (Convert.ToInt16(DateTime.Now.Year) % 100);           //add year code to ponumber

                //string HeaderID = ViewState["HeaderID"].ToString();
                String OldPRONumber = DB.GetSqlS("select top 1 JobOrderRefNo as S from MFG_ProductionOrderHeader where  JobOrderRefNo LIKE '" + NewPRONumber + "%'  order by JobOrderRefNo desc");

                
                int power = (Int32)Math.Pow((double)10, (double)(length - 1));          //getting minvalue of prifix length

                String newvalue = "";
                if (OldPRONumber != "" && NewPRONumber.Equals(OldPRONumber.Substring(0, NewPRONumber.Length)))        //if ponumber is existed and same year ponumber  enter
                {
                    String temp = OldPRONumber.Substring(NewPRONumber.Length, length);                            //getting number of last prifix
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

                NewPRONumber += newvalue;
                txtJobRefNo.Text = NewPRONumber;
                atcMaterialRevision.Focus();
            }
            catch (Exception ex)
            {
                resetError("Error while generating ProNumber", true, false);
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
            }
        }

        protected void gvPRODetailsList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            
                gvPRODetailsList.PageIndex = e.NewPageIndex;
                Build_ProductionOrderDetailsList(Build_ProductionOrderDetailsList());
           
        }

        protected void gvPRODetailsList_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvPRODetailsList.EditIndex = e.NewEditIndex;
            Build_ProductionOrderDetailsList(Build_ProductionOrderDetailsList());
        }

        protected void gvPRODetailsList_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            Page.Validate("UpdateGridItems");
            if (!IsValid)
                return;

            GridViewRow editRow = gvPRODetailsList.Rows[e.RowIndex];
            Literal ltProductionOrderDetailsID = (Literal)editRow.FindControl("ltProductionOrderDetailsID");
            Literal ltPROLineNumber = (Literal)editRow.FindControl("ltPROLineNumber");
            TextBox atcBOMHeader = (TextBox)editRow.FindControl("atcBOMHeader");

            HiddenField hifBOMHeader = (HiddenField)editRow.FindControl("hifBOMHeader");
            TextBox atcBoMUom_Qty = (TextBox)editRow.FindControl("atcBoMUom_Qty");
            HiddenField hifBoMUom_Qty = (HiddenField)editRow.FindControl("hifBoMUom_Qty");
            TextBox txtPROQuantity = (TextBox)editRow.FindControl("txtPROQuantity");

            StringBuilder cmdUpdatePRODetails = new StringBuilder();
            cmdUpdatePRODetails.Append("EXEC [dbo].[sp_MFG_UpsertProductionOrderDetails] ");
            cmdUpdatePRODetails.Append("@ProductionOrderDetailsID=" + ltProductionOrderDetailsID.Text);
            cmdUpdatePRODetails.Append(",@ProductionOrderHeaderID=" + ViewState["HeaderID"]);
            cmdUpdatePRODetails.Append(",@PROLineNumber=" + ltPROLineNumber.Text);
            cmdUpdatePRODetails.Append(",@BOMHeaderID=" + hifBOMHeader.Value);
            cmdUpdatePRODetails.Append(",@MaterialMaster_UoMID=" + hifBoMUom_Qty.Value);
            cmdUpdatePRODetails.Append(",@PROQuantity=" + txtPROQuantity.Text);
            cmdUpdatePRODetails.Append(",@CreatedBy=" + cp.UserID);
            try
            {
                DB.ExecuteSQL(cmdUpdatePRODetails.ToString());
                IbutNew.Visible = false;
                
                gvPRODetailsList.EditIndex = -1;
                Build_ProductionOrderDetailsList(Build_ProductionOrderDetailsList());

                resetError("Successfully Updated", false, false);
            }
            catch (Exception ex)
            {
                resetError("Error while updating", true, false);
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
            }

        }

        protected void gvPRODetailsList_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
           
                gvPRODetailsList.EditIndex = -1;
                Build_ProductionOrderDetailsList(Build_ProductionOrderDetailsList());
           
        }

        protected void lnkAddNewLineItem_Click(object sender, EventArgs e)
        {
            try
            {
                DataSet dsProductionOrderDetailslist = Build_ProductionOrderDetailsList();
                DataRow newRow = dsProductionOrderDetailslist.Tables[0].NewRow();
                newRow["ProductionOrderDetailsID"] = 0;
                int LineNumber = DB.GetSqlN("select top 1 PROLineNumber as N from MFG_ProductionOrderDetails where ProductionOrderHeaderID=" + ViewState["HeaderID"] + " and IsDeleted=0 and IsActive=1 order by PROLineNumber desc");
                newRow["PROLineNumber"] = ++LineNumber;
                dsProductionOrderDetailslist.Tables[0].Rows.InsertAt(newRow, 0);
                gvPRODetailsList.EditIndex = 0;
                gvPRODetailsList.PageIndex = 0;
                Build_ProductionOrderDetailsList(dsProductionOrderDetailslist);
            }
            catch (Exception ex)
            {
                resetError("Error while add new Item", true, false);
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
            }
        }

        protected void lnkClear_Click(object sender, EventArgs e)
        {
            //if (lnkUpdate.Text == "Update")
            //{
            //    Response.Redirect("ProductionOrder.aspx?proid=" + ViewState["HeaderID"].ToString());

            //}
            //else
            {
                Response.Redirect("ProductionOrderList.aspx");
            }
        }

        protected void lnkUpdate_Click(object sender, EventArgs e)
        {
            //test case ID (TC_044)
            //Updated user details


            Page.Validate("save");

            if (!IsValid)
            {
                return;
            }

            try
            {
                if (Convert.ToDecimal(txtProductionQuantity.Text) == 0)
                {
                    resetError("Quantity cannot be zero", true, false);
                    return; 
                }

                //Decimal RemainProductionQty = DB.GetSqlNDecimal("select top 1 SOD.SOQuantity-ISNULL(SUM(KT.ProductionQuantity),0) AS N from ORD_SODetails SOD left join(select KitCode,AVG(ProductionQuantity) as ProductionQuantity,min(SOHeaderID) as SOHeaderID from MFG_ProductionOrderHeader where IsDeleted=0 group by KitCode) as KT ON KT.KitCode!='"+txtkitCode.Text+"' and KT.SOHeaderID=SOD.SOHeaderID join MMT_MaterialMaster_Revision MMV ON MMV.MaterialMasterID=SOD.MaterialMasterID WHERE SOD.IsDeleted=0 and SOD.SOHeaderID="+hifSOHeader.Value+" AND MMV.MaterialMasterRevisionID="+hifMaterialRevision.Value+" GROUP BY SOD.SOQuantity");
                
                //if (RemainProductionQty < Convert.ToDecimal(txtProductionQuantity.Text))
                //{
                //    //test case ID (TC_052)
                //    //not allow more than wo line item quantity

                //    resetError("Job Order quantity is greater than WO quantity", true, false);
                //    return;
                //}
            }
            
            catch (Exception ex)
            {
                resetError("Error while updating", true, false);
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
            }

            StringBuilder cmdupdateProheader = new StringBuilder();
            cmdupdateProheader.Append("Declare @NewHeaderID int ");
            cmdupdateProheader.Append("EXEC [dbo].[sp_MFG_UpsertProductionOrderHeader] ");
            cmdupdateProheader.Append("@ProductionOrderHeaderID=" + ViewState["HeaderID"]);
            cmdupdateProheader.Append(",@MaterialMasterRevisionID=" + hifMaterialRevision.Value);
            cmdupdateProheader.Append(",@JobOrderRefNo="+(txtJobRefNo.Text.Trim()!=""?DB.SQuote(txtJobRefNo.Text.Trim()):"NULL"));
            cmdupdateProheader.Append(",@SOHeaderID=" + (hifSOHeader.Value != "" && atcSOHeader.Text != "" ? hifSOHeader.Value : "NULL"));
            cmdupdateProheader.Append(",@ProductionOrderStatusID=" + 1);
            cmdupdateProheader.Append(",@CoCNumber=" +(txtCoCnumber.Text.Trim()!=""? DB.SQuote(txtCoCnumber.Text):"NULL"));
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
                String ProductionDate = DB.SQuote(txtProductionDate.Text != "" ? DateTime.ParseExact(txtProductionDate.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy") : DateTime.Now.ToString("MM/dd/yyyy"));// DB.SQuote(DateTime.Now.ToString("dd/MM/yyyy"));// "";
                //if (txtProductionDate.Text.Trim() != "")
                //    ProductionDate = DB.SQuote(CommonLogic.GetConfigValue(cp.TenantID, "DateFormat") == "dd/MM/yyyy" ? DateTime.ParseExact(txtProductionDate.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy") : txtProductionDate.Text.Trim());
                //else
                //    ProductionDate = "NULL";
            String ManufacturingDate = "";
            if (txtManufacturingDate.Text.Trim() != "")
                ManufacturingDate = DB.SQuote(CommonLogic.GetConfigValue(cp.TenantID, "DateFormat") == "dd/MM/yyyy" ? DateTime.ParseExact(txtManufacturingDate.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy") : txtManufacturingDate.Text.Trim());
            else
                ManufacturingDate = "NULL";
            String CoCDate = "";
            if (txtCoCDate.Text.Trim() != "")
                CoCDate = DB.SQuote(CommonLogic.GetConfigValue(cp.TenantID, "DateFormat") == "dd/MM/yyyy" ? DateTime.ParseExact(txtCoCDate.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy") : txtCoCDate.Text.Trim());
            else
                CoCDate = "NULL";

            cmdupdateProheader.Append(",@CoCDate=" + CoCDate);
            cmdupdateProheader.Append(",@ContractNo="+(txtContractNo.Text.Trim()!=""?DB.SQuote(txtContractNo.Text.Trim()):"NULL"));
            cmdupdateProheader.Append(",@DateofManufacturing=" + ManufacturingDate);
            cmdupdateProheader.Append(",@ProductionOrderTypeID=" + (atcProductionType.Text != "" && hifproductionType.Value != "" ? hifproductionType.Value : "NULL"));
            cmdupdateProheader.Append(",@KitCode=" + (txtkitCode.Text != "" ? DB.SQuote(txtkitCode.Text) : "NULL"));
            cmdupdateProheader.Append(",@ShopAcceptanceByID=" + (atcShopAcceptanceby.Text != "" && hifShopAcceptanceby.Value != "" ? hifShopAcceptanceby.Value : "NULL"));
            cmdupdateProheader.Append(",@SupervisorAcceptanceByID=" + (atcSupervisorAcceptanceBy.Text != "" && hifSupervisorAcceptanceBy.Value != "" ? hifSupervisorAcceptanceBy.Value : "Null"));
            cmdupdateProheader.Append(",@QAAcceptanceByID=" + (atcQAAcceptanceBy.Text != "" && hifQAAcceptanceBy.Value != "" ? hifQAAcceptanceBy.Value : "NULL"));
            cmdupdateProheader.Append(",@AddtionalObservations=" + (txtAddtionalObservations.Text != "" ? DB.SQuote(txtAddtionalObservations.Text) : "NULL"));
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
                lnkAddNewLineItem.Visible = true;
                tdlineitems.Visible = true;
                lnkUpdate.Text = "Update"+CommonLogic.btnfaUpdate;
                updateStatus = true;
                //Response.Redirect("ProductionOrder.aspx?proid=" + ViewState["HeaderID"].ToString()+"&sucess=true");
               // ViewState["ProductionOrderDetailsList"] = "EXEC [dbo].[sp_MFG_GetProductionOrderDetails] @ProductionOrderHeaderID=" + ViewState["HeaderID"];
               // resetError("Update successfully",false);
                
            }
            catch (SqlException sqlex)
            {
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), sqlex.Source,sqlex.Message, sqlex.StackTrace);
                if (sqlex.Message.StartsWith("Cannot insert duplicate key row in object 'dbo.MFG_ProductionOrderHeader'"))
                    resetError("Job order ref., routing ref., and kit code combination already exists, cannot update", true, false);
                else
                    resetError("Error while updating", true, false);
                return;
            }
            catch (Exception ex)
            {
                updateStatus = false;
               // Response.Redirect("ProductionOrder.aspx?proid=" + ViewState["HeaderID"].ToString() + "&sucess=false");
                resetError("Error while updating",true,false);
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
                return;
            }
            if (IsDeleted == true)
            {
                Response.Redirect("ProductionOrderList.aspx");
            }

            Response.Redirect("ProductionOrder.aspx?proid=" + ViewState["HeaderID"].ToString() + "&sucess=" + updateStatus.ToString());

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

        protected void lnkIsDeleted_Click(object sender, EventArgs e)
        {
            int rowCount = gvPRODetailsList.Rows.Count;
            GridViewRow row;
            String PRODetailsIDs = "";
            for (int rowNo = 0; rowNo < rowCount; rowNo++)
            {
                row = gvPRODetailsList.Rows[rowNo];
                if (gvPRODetailsList.EditIndex != rowNo)
                {
                    if (((CheckBox)row.FindControl("chkisdeleted")).Checked)
                    {
                        PRODetailsIDs += ((Literal)row.FindControl("ltProductionOrderDetailsID")).Text + ",";

                    }
                }
            }

            try
            {
                DB.ExecuteSQL("Exec [dbo].[sp_MFG_DeleteProductionOrderDetails] @ProductionOrderDetailsIDs=" + DB.SQuote(PRODetailsIDs));
                gvPRODetailsList.EditIndex = -1;
                Build_ProductionOrderDetailsList(Build_ProductionOrderDetailsList());
                resetError("Deleted selected lineitems", false, false);
            }
            catch (Exception ex)
            {
                resetError("Error while deleting selected items", true, false);
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
            }
        }

        protected void AddWorkOrderReport()
        {

           
            String cmdWorkOrderReport = "EXEC [sp_MFG_GetWorkOrderReport] @ProductionOrderHeaderID=" + CommonLogic.QueryString("proid");
            try
            {
                IDataReader rsWorkOrderReport = DB.GetRS(cmdWorkOrderReport);
                HtmlTable tbWorkCenterReportDetails = new HtmlTable();
                tbWorkCenterReportDetails.ID = "tbWorkgroupDetails";
                tbWorkCenterReportDetails.Width = "100%";
                tbWorkCenterReportDetails.BgColor = "#F5FFFA";
                HtmlTableRow trWorkCenterDetails = null;

                DropDownList ddlWorkCenter = null;
                HtmlTableCell tdWorkCenterDetails = null;

                trWorkCenterDetails = new HtmlTableRow();
                trWorkCenterDetails.Height = "15px";
                trWorkCenterDetails.VAlign = "bottom";
                tdWorkCenterDetails = new HtmlTableCell();
                //tdWorkCenterDetails.Align = "center";
                Label lbName = new Label();
                lbName.Font.Size = 11;
                lbName.Text = "Workstation Type";
                lbName.ForeColor = System.Drawing.Color.RoyalBlue;
                tdWorkCenterDetails.Controls.Add(lbName);
                tdWorkCenterDetails.Width = "40%";
                trWorkCenterDetails.Controls.Add(tdWorkCenterDetails);

                tdWorkCenterDetails = new HtmlTableCell();
                tdWorkCenterDetails.Width = "40%";
                lbName = new Label();
                lbName.Text = "Workstation";
                lbName.Font.Size = 11;
                lbName.ForeColor = System.Drawing.Color.RoyalBlue;
                tdWorkCenterDetails.Controls.Add(lbName);
                trWorkCenterDetails.Controls.Add(tdWorkCenterDetails);

                tdWorkCenterDetails = new HtmlTableCell();
                lbName = new Label();
                lbName.Font.Size = 11;
                lbName.Text = "Status";
                lbName.ForeColor = System.Drawing.Color.RoyalBlue;
                tdWorkCenterDetails.Controls.Add(lbName);
                trWorkCenterDetails.Controls.Add(tdWorkCenterDetails);
                tbWorkCenterReportDetails.Controls.Add(trWorkCenterDetails);

                while (rsWorkOrderReport.Read())
                {
                    trWorkCenterDetails = new HtmlTableRow();
                    trWorkCenterDetails.Height = "40px";
                    tdWorkCenterDetails = new HtmlTableCell();
                    //tdWorkCenterDetails.Align = "center";
                    tdWorkCenterDetails.Controls.Add(new LiteralControl(DB.RSField(rsWorkOrderReport, "WorkCenterGroup")));
                    tdWorkCenterDetails.Width = "40%";
                    trWorkCenterDetails.Controls.Add(tdWorkCenterDetails);

                    tdWorkCenterDetails = new HtmlTableCell();
                    String[] WorkCenterDetailsList = DB.RSField(rsWorkOrderReport, "WorkCentersDetails").Split('œ');
                    if (WorkCenterDetailsList[0] == "")
                    {
                        continue;
                    }
                    ddlWorkCenter = new DropDownList();
                    ddlWorkCenter.Width = 120;
                    ddlWorkCenter.Items.Add(new ListItem("Select Workstation", ""));
                    ddlWorkCenter.ID = DB.RSField(rsWorkOrderReport, "WorkCenterGroup");
                    bool IsAssigned = false;
                    foreach (String WorkCenterDetail in WorkCenterDetailsList)
                    {
                        String[] WorkCenterData = WorkCenterDetail.Split('š');
                        ddlWorkCenter.Items.Add(new ListItem(WorkCenterData[0], WorkCenterData[4]));
                        if (WorkCenterData[2] != "")
                        {
                            IsAssigned = true;
                            ddlWorkCenter.SelectedValue = WorkCenterData[4];
                        }
                    }
                    tdWorkCenterDetails.Width = "40%";
                    tdWorkCenterDetails.Controls.Add(ddlWorkCenter);
                    trWorkCenterDetails.Controls.Add(tdWorkCenterDetails);

                    tdWorkCenterDetails = new HtmlTableCell();
                    if (IsAssigned)
                        tdWorkCenterDetails.Controls.Add(new LiteralControl("Assigned"));
                    else
                        tdWorkCenterDetails.Controls.Add(new LiteralControl("Not Assigned"));
                    trWorkCenterDetails.Controls.Add(tdWorkCenterDetails);
                    tbWorkCenterReportDetails.Controls.Add(trWorkCenterDetails);
                }
                rsWorkOrderReport.Close();

                dvWorkCenterdetails.Controls.Add(tbWorkCenterReportDetails);
            }
            catch (Exception ex)
            {
                resetError("Error while loading",true,false);
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
            }
        }

        protected void BuildWorkstationDetails()
        {
            try
            {
                String cmdWorkOrderReport = "EXEC [sp_MFG_GetWorkOrderReport] @ProductionOrderHeaderID=" + CommonLogic.QueryString("proid");
                IDataReader rsWorkOrderReport = DB.GetRS(cmdWorkOrderReport);
                DropDownList ddlWorkstationGroup;
                while (rsWorkOrderReport.Read())
                {
                    ddlWorkstationGroup = (DropDownList)dvWorkCenterdetails.FindControl(DB.RSField(rsWorkOrderReport, "WorkCenterGroup"));
                    if (ddlWorkstationGroup != null)
                    {
                        ddlWorkstationGroup.SelectedIndex = 0;
                        LiteralControl ltTextControl = (LiteralControl)ddlWorkstationGroup.Parent.Parent.Controls[2].Controls[0];
                        ltTextControl.Text = "Not Assigned";
                        foreach (String Workstationdeatils in DB.RSField(rsWorkOrderReport, "WorkCentersDetails").Split('œ').Where(o => o != "").Where(o => o.Split('š')[2] != ""))
                        {
                            ddlWorkstationGroup.SelectedValue = Workstationdeatils.Split('š')[4];
                            ltTextControl.Text = "Assigned";
                            break;
                        }
                    }

                }
                rsWorkOrderReport.Close();
            }
            catch (Exception ex)
            {
                resetError("Error while loading",true,false);
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
            }
        }

        protected void lnkAssignWorkOrder_Click(object sender, EventArgs e)
        {
            //test case ID (TC_055)
            //Successfully updated configure workcenters

            //tdWorkCenterdetails.Controls[0].GetType();

            HtmlTable tbWorkGroupDetails = (HtmlTable)dvWorkCenterdetails.FindControl("tbWorkgroupDetails");
            StringBuilder workCenterIDs = new StringBuilder();
            StringBuilder WorkCenterQties = new StringBuilder();
            Decimal ProductionQty = Convert.ToDecimal(lbqty.Text);
            decimal WorkCenterQty = 0;
            Boolean isFromWorkCenter = false;
            DropDownList ddlWorkCenter = null;
            Control htmlHeader = tbWorkGroupDetails.Controls[0];
            foreach (HtmlControl control in tbWorkGroupDetails.Controls)
                        {
                if (control == htmlHeader)
                    continue;
                ddlWorkCenter = (DropDownList)((HtmlTableCell)((HtmlTableRow)control).Controls[1]).Controls[0];
                if (ddlWorkCenter.SelectedValue != "")
                        {
                    workCenterIDs.Append(ddlWorkCenter.SelectedValue + ",");
                }
                    }
            if (workCenterIDs.ToString() == "")
            {
                //test case ID (TC_056)
                //select atleast one workstation

                resetDialogBoxError("No workstation is selected", true);
                        return;
            }
            try
            {
                DB.ExecuteSQL("EXEC [dbo].[sp_MFG_UpsertProductionOrder_WorkCenter] @ProductionOrderHeaderID=" + ViewState["HeaderID"] + ",@WorkCenterIDs=" + DB.SQuote(workCenterIDs.ToString()) + ",@Quantity=" + txtProductionQuantity.Text + ",@CreatedBy=" + cp.UserID);
                //resetDialogBoxError("Successfully Updated", false);

            }
            catch (Exception ex)
            {
                resetDialogBoxError("Error while updating", true);
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
                return;
            }
            Response.Redirect("ProductionOrder.aspx?proid=" + ViewState["HeaderID"].ToString() + "&sucess=" + true.ToString());

        }

        protected void resetDialogBoxError(string error, bool isError)
        {

            /*string str = "<font class=\"noticeMsg\">NOTICE:</font>&nbsp;&nbsp;&nbsp;";
            if (isError)
                str = "<font class=\"errorMsg\">ERROR:</font>&nbsp;&nbsp;&nbsp;";

            if (error.Length > 0)
                str += error + "";
            else
                str = "";


            ltDialogBoxStatus.Text = str;*/
            //
            ScriptManager.RegisterStartupScript(this, this.GetType(), "test", "closeAsynchronus(); showStickyToast(" + (isError.ToString().ToLower() == "true" ? "false" : "true") + ",\"" + error + "\");", true);

        }

        #endregion --------- Basic Data  -------------------------------

        #region ----  Production Order Materials Deficiency List Dialog  --------


        protected DataSet gvProdDefcList_buildGridData()
        {
            DataSet dsGetDeficiency = null;
            string cmdGetDeficiency = "[sp_MFG_ProductionOrderDeficiency] " + CommonLogic.QueryString("proid");
            try
            {
                dsGetDeficiency = DB.GetDS(cmdGetDeficiency, false);
            }
            catch (Exception ex)
            {
                resetError("Error while loading",true,false);
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
            }

            return dsGetDeficiency;
        }

        protected void gvProdDefcList_buildGridData(DataSet ds)
        {
            gvProdDefcList.DataSource = ds.Tables[0];
            gvProdDefcList.DataBind();
            ds.Dispose();

            ScriptManager.RegisterStartupScript(this, this.GetType(), "ProdUnblockDialog", "ProdUnblockDialog();", true);
        }

        protected void gvProdDefcList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.DataItem == null)
                return;

            Literal ltDeficiency = (Literal)e.Row.FindControl("ltDeficiency");

            if (ltDeficiency.Text != "0.00")
                e.Row.CssClass = "DeficiencyRow";


        }

        protected void gvProdDefcList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {


            gvProdDefcList.PageIndex = e.NewPageIndex;
            gvProdDefcList.EditIndex = -1;
            this.gvProdDefcList_buildGridData(this.gvProdDefcList_buildGridData());

        }

        protected void lnkMaterialDeficiency_Click(object sender, EventArgs e)
        {
            gvProdDefcList_buildGridData(gvProdDefcList_buildGridData());
        }

        #endregion ----  Production Order Materials Deficiency List Dialog  --------

        protected bool HasDeficiency(String SqlString)
        {

            bool status = false;

            try
            {

                DataSet ds = DB.GetDS(SqlString, false);

                if (ds == null)
                    return status;

                if (ds.Tables[0].Rows.Count == 0)
                {
                    status = true;
                }
            }
            catch (Exception ex)
            {
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
            }

            return status;
        }

        private void Build_PendingMaterialList()
        {
            DataSet dsGetPendingMaterial = null;
            String cMdGetpendingMaterial = "dbo.sp_MFG_GetRealeseMaterialListForJob " + CommonLogic.QueryString("proid");
            try
            {
                dsGetPendingMaterial = DB.GetDS(cMdGetpendingMaterial, false);
            }
            catch (Exception ex)
            {
                resetError("Error while loading",true,false);
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
            }
            gvPendingMaterial.DataSource = dsGetPendingMaterial;
            gvPendingMaterial.DataBind();
            dsGetPendingMaterial.Dispose();

            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pen_Mat_UnblockDialog", "Pen_Mat_UnblockDialog();", true);

        }

        protected void lnkReleaseJobOrder_Click(object sender, EventArgs e)
        {
            Build_PendingMaterialList();
        }

        private String GetSONumber()
        {
            String SONumber = "";
            try
            {
                String NewSONumber = DB.GetSqlS("EXEC [sp_SYS_GetSystemConfigValue]   @SysConfigKey='dummysoheaderforproduction_prefix' ,@TenantID=" + cp.TenantID); //DB.GetSqlS("select SysConfigValue as S from SYS_SystemConfiguration where TenantID=" + cp.TenantID + " and SysConfigKeyID= (select SysConfigKeyID from SYS_SysConfigKey where SysConfigKey=N'salesorder.aspx.cs.SO_Prefix') ");
                NewSONumber += "" + (Convert.ToInt16(DateTime.Now.Year) % 100);           //add year code to ponumber

                int length = Convert.ToInt16(DB.GetSqlS("EXEC [sp_SYS_GetSystemConfigValue] @SysConfigKey='salesorder.aspx.cs.SO_Length' ,@TenantID=" + cp.TenantID));// Convert.ToInt32(DB.GetSqlS("select SysConfigValue as S from SYS_SystemConfiguration as N where TenantID=" + cp.TenantID + " and SysConfigKeyID= (select SysConfigKeyID from SYS_SysConfigKey where SysConfigKey=N'salesorder.aspx.cs.SO_Length') "));

                String OldPONumber = DB.GetSqlS("select TOP 1 SONumber as S from ORD_SOHeader where SONumber like '"+NewSONumber+"%'  ORDER BY SONumber desc ");


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

                String OldOBDNumber = DB.GetSqlS("select top 1 OBDNumber AS S from OBD_Outbound where OBDNumber like '"+NewOBDNumber+"%' order by OBDNumber desc");

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


            /*
            String kitCode = "";
            if (atcSOHeader.Text != "" & hifSOHeader.Value != "" && atcRoutingHeaderVersion.Text != "" && hifRoutingHeaderVersion.Value != "")
            {
                try
                {
                    String NewNumber = "000";
                    kitCode = DB.GetSqlS("select RIGHT(LEFT(MM.CustomerPartNumber,CHARINDEX('-',MM.CustomerPartNumber)-1),4)+'|'+RIGHT(MM.CustomerPartNumber,2) AS S from MFG_RoutingHeader_Revision ROUV JOIN MMT_MaterialMaster_Revision MMV ON MMV.MaterialMasterRevisionID=ROUV.MaterialMasterRevisionID AND ROUV.IsActive=1 AND ROUV.IsDeleted=0 left JOIN MMT_MaterialMaster MM ON MM.MaterialMasterID=MMV.MaterialMasterID  WHERE 'RT175'=(select ProjectCode from ORD_SOHeader where SOHeaderID=" + hifSOHeader.Value + ") and ROUV.RoutingHeaderRevisionID=" + hifRoutingHeaderVersion.Value);
                    if (kitCode != "")
                    {
                        String RecentKitCode = DB.GetSqlS("SELECT top 1 RIGHT(KitCode,3) AS S FROM MFG_ProductionOrderHeader WHERE KitCode LIKE '" + kitCode + "%' order by KitCode desc");
                        if (RecentKitCode == "")
                        {
                            kitCode += "|001";
                        }
                        else
                        {
                            String nextValue = Convert.ToString(Convert.ToInt16(RecentKitCode) + 1);
                            kitCode += "|" + NewNumber.Substring(0, 3 - nextValue.Length) + nextValue;
                        }
                        txtkitCode.Text = kitCode;
                        txtkitCode.Enabled = false;
                        atcProductionType.Focus();
                    }

                }
                catch (Exception ex)
                {
                    resetError("Not valid Customerpartno. for BOM Material", true, false);
                }
            }
            else
            {
                resetError("WO Ref #. and BOM Header Version must not empty", true, false);
            }*/


            String WarehouseID = "1";


            if (WarehouseID != "" && hifproductionType.Value == "10")
            {
                int StoreRefNoLength = 0;
                IDataReader StrRefNoLengthReader =null;
                try
                {
                    StrRefNoLengthReader = DB.GetRS("select SysConfigValue from SYS_SystemConfiguration SYS_SC left join SYS_SysConfigKey SYS_SCK on  SYS_SCK.SysConfigKeyID=SYS_SC.SysConfigKeyID where TenantID=" + cp.TenantID + " and SYS_SCK.SysConfigKey='inbounddetails.aspx.cs.StoreRefNo_Length' and SYS_SCK.IsActive=1");
                }
                catch (Exception ex)
                {
                    resetError("Error while updating",true,false);
                    CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
                }
                if (StrRefNoLengthReader.Read())
                    StoreRefNoLength = Convert.ToInt32(DB.RSField(StrRefNoLengthReader, "SysConfigValue")); // get StoreRefNo length for logged Tenant
                StrRefNoLengthReader.Close();

                string WHGroupCode = "";

                String vStoreRefNUmber = DB.GetSqlS("select top 1 KitCode AS S from MFG_ProductionOrderHeader where ProductionOrderTypeID=10 order by ProductionOrderHeaderID desc"); // Get Previous StoreRefNo


                String vNewStoreRefNUmber = "";

                WHGroupCode = "PH";
                try
                {
                    vNewStoreRefNUmber = Convert.ToString(WHGroupCode);  //Assign WHGroupCode

                    vNewStoreRefNUmber += "" + (Convert.ToInt16(DateTime.Now.Year) % 100);           //add year code to StoreRefNUmber
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

                    txtkitCode.Text = vNewStoreRefNUmber;
                }
                catch (Exception ex)
                {

                    resetError("Error while generating kit code", true, false);
                    CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
                    return;
                }
            }
            else {


                resetError("Please select 'Job Order' type", true, false);
                return;
            }





        }

        protected void lnkclose_Click(object sender, EventArgs e)
        {
            CheckJobOrderQty();
        }

        protected void lnkUpdatePendingMaterial_Click(object sender, EventArgs e)
        {
            //test case ID (TC_058)
            //release materials with OBD number


            bool isEmpty = true;
            StringBuilder MaterialList = new StringBuilder();
            StringBuilder QuantityList = new StringBuilder();

            foreach (GridViewRow row in gvPendingMaterial.Rows)
            {
                if (((CheckBox)row.FindControl("chkSelectBox")).Checked)
                {
                    isEmpty = false;
                    MaterialList.Append(((HiddenField)row.FindControl("hidMaterialMaster")).Value + ",");
                    QuantityList.Append(((HiddenField)row.FindControl("hidPossibleQty")).Value + ",");
                }
            }
            if (isEmpty)
            {
                //test case ID (TC_059)
                //select atleast one material

                resetDialogBoxError("No material is selected", true);
                return;
            }
            StringBuilder sbDummyOutbound = new StringBuilder();

            try
            {

                String OBDNumber = GetOBDNumber();

                String SONumber = GetSONumber();

                String CUSPONumber = GetCUSPONumber();

                if (OBDNumber != "" && SONumber != "" && CUSPONumber != "")
                {

                    sbDummyOutbound.Append("DECLARE @NewUpdateOutboundID int; EXEC  [sp_MFG_DummyOutboundForProductionOrder]   @ProductionOrderHeaderID =" + CommonLogic.QueryString("proid") + ",@SONumber =" + DB.SQuote(SONumber) + ",@CusPONumber=" + DB.SQuote(CUSPONumber) + ",@OBDNumber =" + DB.SQuote(OBDNumber) + ",@MaterialList=" + DB.SQuote(MaterialList.ToString()) + ",@QuantityList=" + DB.SQuote(QuantityList.ToString()) + ",@CreatedBy = " + cp.TenantID + ",@NewOutboundID=@NewUpdateOutboundID OUTPUT; select @NewUpdateOutboundID AS N");

                    if (DB.GetSqlN(sbDummyOutbound.ToString()) != 0)
                    {
                        resetError("Released job order with OBD number:" + OBDNumber, false, true);

                        //lnkReleaseJobOrder.Visible = false;
                        //Page_Load(new object(), new EventArgs());
                        atcMaterialRevision.Enabled = false;
                        atcSOHeader.Enabled = false;
                        txtProductionDate.Enabled = false;
                        txtDueDate.Enabled = false;
                        txtkitCode.Enabled = false;
                        atcProductionUoM.Enabled = false;
                        txtProductionQuantity.Enabled = false;
                        atcRoutingHeaderVersion.Enabled = false;
                        chkIsActive.Enabled = false;
                        chkIsDeleted.Enabled = false;
                        txtStartDate.Enabled = false;
                        //txtManufacturingDate.Enabled = false;
                        atcProductionType.Enabled = false;
                        chkIsDeleted.Enabled = false;
                        imgKitCode.Visible = false;
                        Build_PendingMaterialList();

                    }
                    else
                    {
                        resetError("Error while updating release job order ", true, false);
                    }

                }

                Build_FormData();
            }
            catch (Exception ex)
            {
                resetError("Error while updating release job order", true, false);
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
            }
        }

        protected void gvPendingMaterial_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow && !(e.Row.RowState == DataControlRowState.Edit))
            {
                if (Convert.ToDecimal(((HiddenField)e.Row.FindControl("hidPossibleQty")).Value) <= 0)
                        e.Row.FindControl("chkSelectBox").Visible = false;
            }
        }

        protected void ImgChangeRevision_Click(object sender, ImageClickEventArgs e)
        {
            StringBuilder cMdCheckRoutingColne = new StringBuilder();
            cMdCheckRoutingColne.Append("select TOP 1 1 AS N from MFG_RoutingHeader_Revision ROUR ");
            cMdCheckRoutingColne.Append(" JOIN MFG_RoutingDetails ROUD ON ROUD.RoutingHeaderID=ROUR.RoutingHeaderID AND ROUD.IsDeleted=0 ");
            cMdCheckRoutingColne.Append(" JOIN MFG_RoutingDetailsActivity ROUDA ON ROUDA.RoutingDetailsID=ROUD.RoutingDetailsID AND ROUDA.IsDeleted=0 ");
            cMdCheckRoutingColne.Append(" JOIN MFG_RoutingClone ROUC ON ROUC.OldRoutingDetailsActivityID=ROUDA.RoutingDetailsActivityID ");
            cMdCheckRoutingColne.Append(" JOIN MFG_ProductionOrderHeader PROH ON PROH.RoutingHeaderRevisionID=ROUR.RoutingHeaderRevisionID ");
            cMdCheckRoutingColne.Append(" WHERE PROH.ProductionOrderHeaderID="+ViewState["HeaderID"]);

            int CheckRevisionClone = DB.GetSqlN(cMdCheckRoutingColne.ToString());

            if (CheckRevisionClone == 0)
            {
                resetDialogBoxError("No Clone Available for Current Routing",true);
                return;
            }
            String cMdChangeNewVersion = "DECLARE @GetResult INT EXEC dbo.sp_MFG_ChangeNewRevisionToJobOrder @ProductionOrderHeaderID=" + ViewState["HeaderID"] + ",@UpdatedBy="+cp.UserID.ToString() + " ,@Result=@GetResult  OUTPUT Select @GetResult as N";

            int Result =  DB.GetSqlN(cMdChangeNewVersion);

            bool updateStatus;
            if (Result == 1)
            {
                updateStatus = true;
            }
            else
            {
                updateStatus = false;
                resetDialogBoxError("Error while Updating",true);
                return;
            }
            Response.Redirect("ProductionOrder.aspx?proid=" + ViewState["HeaderID"].ToString() + "&sucess=" + updateStatus.ToString());

        }

        protected void lnkworkstationconfiguration_Click(object sender, EventArgs e)
        {
            BuildWorkstationDetails();
        }

        protected void lnkChangeStatus_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt16("0" + hifStatus.Value) == 2)
            {
                DB.ExecuteSQL("update MFG_ProductionOrderHeader set ProductionOrderStatusID=4 where ProductionOrderHeaderID= " + CommonLogic.QueryString("proid"));
                

            }
            else if (Convert.ToInt16("0" + hifStatus.Value) == 4)
            {
                DB.ExecuteSQL("update MFG_ProductionOrderHeader set ProductionOrderStatusID=2 where ProductionOrderHeaderID= " + CommonLogic.QueryString("proid"));


            }

            Response.Redirect("ProductionOrder.aspx?proid=" + ViewState["HeaderID"].ToString() + "&sucess=True");
        }

        
    }
}