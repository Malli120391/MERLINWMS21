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
    //Author    :   Gvd prasad
    //Created On:   22-Feb-2014

    public partial class InternalOrder : System.Web.UI.Page
    {

        public CustomPrincipal cp = HttpContext.Current.User as CustomPrincipal;
        
        protected void Page_PreInit(object sender, EventArgs e)
        {
            Page.Theme = "Inbound";

        }

        protected void page_Init(object sender, EventArgs e)
        {
            AddDynamicColumns();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Validate();
            if (!cp.IsInAnyRoles(CommonLogic.GetRolesAllowed(CommonLogic.GetRolesAllowedForthisPage("New NC Order"))))
            {
                Response.Redirect("Login.aspx?eid=6");
            }
            //Page.Form.DefaultButton = lnkUpdate.UniqueID;
            //atcProductionOrderHeader.Focus();
            if (!IsPostBack)
            {
                DesignLogic.SetInnerPageSubHeading(this.Page, "NC Order Details");
                lnkUpdate.Text = "Create Nc" + CommonLogic.btnfaSave;
                ViewState["HeaderID"] = 0;
                if (CommonLogic.QueryString("ioid") != "")
                {
                    IbutNew.Visible = false;
                    Build_FormData();
                    InternalOrderVerification();
                   // Build_DeficiencyMaterialList(Build_DeficiencyMaterialList());
                }
                else if (CommonLogic.QueryString("new") != "")
                {
                    try
                    {
                        hifProductionOrderHeader.Value = CommonLogic.QueryString("new");
                        IDataReader rsJopDetails = DB.GetRS("select MM.MCode+'-'+MMR.Revision as JObDetails,KitCode from MFG_ProductionOrderHeader PROH JOIN MMT_MaterialMaster_Revision MMR ON MMR.MaterialMasterRevisionID=PROH.MaterialMasterRevisionID AND MMR.IsActive=1 AND MMR.IsDeleted=0 JOIN MMT_MaterialMaster MM ON MM.MaterialMasterID=MMR.MaterialMasterID  where ProductionOrderHeaderID=" + CommonLogic.QueryString("new"));
                        rsJopDetails.Read();
                        atcProductionOrderHeader.Text = DB.RSField(rsJopDetails, "JObDetails");
                        atckitCode.Text = DB.RSField(rsJopDetails, "KitCode");
                        hifkitCode.Value = DB.RSField(rsJopDetails, "KitCode");
                        rsJopDetails.Close();
                        atcProductionOrderHeader.Enabled = false;
                        atckitCode.Enabled = false;
                        tdlineitems.Visible = false;
                        chkIsActive.Checked = true;
                        IbutNew.Focus();
                        lnkAddNewLineItem.Visible = false;
                        lnkDeficiency.Visible = false;
                        lnkCreateOutbount.Visible = false;
                        pnlgvInternalOrderDetails.Visible = false;
                    }
                    catch (Exception ex)
                    {
                        resetError("Error while Build data",true,false);
                        CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
                    }
                   // Page.Form.DefaultButton = IbutNew.UniqueID;
                    //IbutNew.Focus();
                }
                else
                {
                    tdlineitems.Visible = false;
                    IbutNew.Focus();
                    chkIsActive.Checked = true;
                    lnkAddNewLineItem.Visible = false;
                    lnkDeficiency.Visible = false;
                    lnkCreateOutbount.Visible = false;
                    pnlgvInternalOrderDetails.Visible = false;
                    //atcProductionOrderHeader.Focus();
                    //Page.Form.DefaultButton = IbutNew.UniqueID;
                    //IbutNew.Focus();
                }
            }
        }

        private void Build_FormData()
        {
            try
            {
                IDataReader rsGetHeaderData = DB.GetRS("EXEC [dbo].[sp_MFG_GetInternalOrderHeader] @InternalOrderHeaderID=" + CommonLogic.QueryString("ioid"));
                if (rsGetHeaderData.Read())
                {
                    txtIORefNo.Text = DB.RSField(rsGetHeaderData, "IORefNo");
                    atcProductionOrderHeader.Text = DB.RSField(rsGetHeaderData, "PRORefNo");
                    hifProductionOrderHeader.Value = DB.RSFieldInt(rsGetHeaderData, "ProductionOrderHeaderID").ToString();
                    atcWorkCenter.Text = DB.RSField(rsGetHeaderData, "WorkCenter");
                    hifWorkCenter.Value = DB.RSFieldInt(rsGetHeaderData, "WorkCenterID").ToString();
                    atcRoutingoperation.Text = DB.RSField(rsGetHeaderData, "OperationNumber");
                    atckitCode.Text = DB.RSField(rsGetHeaderData, "KitCode");
                    hifkitCode.Value = DB.RSField(rsGetHeaderData, "KitCode");
                    hifRouotingOperation.Value = DB.RSFieldInt(rsGetHeaderData, "RoutingDetailsID").ToString();
                    atcActivity.Text = DB.RSField(rsGetHeaderData, "ActivityCode");
                    hifActivity.Value = DB.RSFieldInt(rsGetHeaderData, "RoutingDetailsActivityID").ToString();
                    atcRequestedBy.Text = DB.RSField(rsGetHeaderData, "RequestedBy");
                    hifRequestedBy.Value = DB.RSFieldInt(rsGetHeaderData, "RequestedByID").ToString();
                    lbNcStatustext.Text = DB.RSField(rsGetHeaderData, "InternalOrderStatus");
                    atcReasonForInternalOrderRequest.Text = DB.RSField(rsGetHeaderData, "ReasonForInternalOrderRequest");
                    hifReasonForInternalOrderRequest.Value = DB.RSFieldInt(rsGetHeaderData, "ReasonForInternalOrderRequestID").ToString();
                    txtRemarks.Text = DB.RSField(rsGetHeaderData, "Remarks");
                    chkIsActive.Checked = DB.RSFieldBool(rsGetHeaderData, "IsActive");
                    chkIsDeleted.Checked = DB.RSFieldBool(rsGetHeaderData, "IsDeleted");

                    ViewState["HeaderID"] = CommonLogic.QueryString("ioid");
                    ViewState["InternalOrderDetailsList"] = "EXEC [dbo].[sp_MFG_GetInternalOrderDetails] @InternalOrderHeaderID=" + ViewState["HeaderID"];
                    IbutNew.Enabled = false;
                    tdlineitems.Visible = true;

                    Build_InternalOrderList(Build_InternalOrderList());
                    if (gvIODetailsList.Rows.Count == 0)
                        pnlgvInternalOrderDetails.Visible = false;
                    lnkUpdate.Text = "Update" + CommonLogic.btnfaUpdate;
                }
                rsGetHeaderData.Close();
            }
            catch (Exception ex)
            {
                resetError("Error while loading", true,false);
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
            }

        }

        private void InternalOrderVerification()
        {
            try
            {
                IDataReader rsIOVerification = DB.GetRS("sp_MFG_InternalOrderVerification @InternalOrderHeaderID=" + CommonLogic.QueryString("ioid"));
                if (rsIOVerification.Read())
                {
                    if (DB.RSFieldInt(rsIOVerification, "SOPO_ProductionOrderID") != 0)
                    {
                        if (DB.RSFieldInt(rsIOVerification, "InternalOrderHeader_SOHeaderID") == 0 && DB.RSFieldInt(rsIOVerification, "InternalOrderDetailsID") != 0 && DB.RSFieldTinyInt(rsIOVerification, "IsDeficiency") == 0)
                        {
                            lnkDeficiency.Visible = false;
                            lnkCreateOutbount.Visible = true;
                        }
                        else if (DB.RSFieldInt(rsIOVerification, "InternalOrderHeader_SOHeaderID") != 0)
                        {
                            lnkDeficiency.Visible = false;
                            lnkCreateOutbount.Visible = false;
                            txtIORefNo.Enabled = false;
                            lnkAddNewLineItem.Enabled = false;
                            lnkUpdate.Enabled = false;
                            IbutNew.Enabled = false;
                            atcProductionOrderHeader.Enabled = false;
                            atcWorkCenter.Enabled = false;
                            atcRoutingoperation.Enabled = false;
                            atcActivity.Enabled = false;
                            atcReasonForInternalOrderRequest.Enabled = false;
                            atcRequestedBy.Enabled = false;
                            gvIODetailsList.Enabled = false;
                            chkIsActive.Enabled = false;
                            chkIsDeleted.Enabled = false;
                        }
                        else if (DB.RSFieldTinyInt(rsIOVerification, "IsDeficiency") == 1)
                        {
                            lnkDeficiency.Visible = true;
                            if (DB.RSFieldTinyInt(rsIOVerification, "IsAvailableQty") == 1)
                                lnkCreateOutbount.Visible = true;
                            else
                                lnkCreateOutbount.Visible = false;
                        }
                        else if (DB.RSFieldInt(rsIOVerification, "InternalOrderDetailsID") != 0)
                        {
                            lnkDeficiency.Visible = false;
                            lnkCreateOutbount.Visible = false;

                        }
                        else
                        {
                            lnkDeficiency.Visible = false;
                            lnkCreateOutbount.Visible = false;
                        }
                    }
                    else if (DB.RSFieldTinyInt(rsIOVerification, "IsDeficiency") == 1)
                    {
                        lnkDeficiency.Visible = true;
                        lnkCreateOutbount.Visible = false;
                    }
                    else
                    {
                        lnkDeficiency.Visible = false;
                        lnkCreateOutbount.Visible = false;
                    }
                }

                rsIOVerification.Close();
            }catch( Exception ex)
            {
                resetError("Error while loading",true,false);
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
            }
        }

        protected void gvIODetailsList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
           
                gvIODetailsList.PageIndex = e.NewPageIndex;
                Build_InternalOrderList(Build_InternalOrderList());
           
        }

        protected void gvIODetailsList_RowEditing(object sender, GridViewEditEventArgs e)
        {
           
                gvIODetailsList.EditIndex = e.NewEditIndex;
                Build_InternalOrderList(Build_InternalOrderList());
           
        }

        protected void gvIODetailsList_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            Page.Validate("UpdateGridItems");
            if (!IsValid)
                return;

            GridViewRow editRow= gvIODetailsList.Rows[e.RowIndex];
            Literal ltInternalOrderDetailsID = (Literal)editRow.FindControl("ltInternalOrderDetailsID");
           // Literal ltIOLineNumber = (Literal)editRow.FindControl("ltIOLineNumber");
            TextBox atcMaterialMaster = (TextBox)editRow.FindControl("atcMaterialMaster");
            HiddenField hifMaterialMaster = (HiddenField)editRow.FindControl("hifMaterialMaster");
            TextBox atcMaterialMaster_IOUoMID = (TextBox)editRow.FindControl("atcMaterialMaster_IOUoMID");
            HiddenField hifMaterialMaster_IOUoMID = (HiddenField)editRow.FindControl("hifMaterialMaster_IOUoMID");
            TextBox txtIOQuantity = (TextBox)editRow.FindControl("txtIOQuantity");
            if (Convert.ToDecimal(txtIOQuantity.Text) <= 0)
            {
                resetError("Quantity zero or below not update",true,false);
                return;
            }
            StringBuilder cmdUpdateIODetails = new StringBuilder();
            cmdUpdateIODetails.Append("EXEC [dbo].[sp_MFG_UpsertInternalOrderDetails] ");
            cmdUpdateIODetails.Append("@InternalOrderDetailsID="+ltInternalOrderDetailsID.Text);
            cmdUpdateIODetails.Append(",@InternalOrderHeaderID="+ViewState["HeaderID"]);
           // cmdUpdateIODetails.Append(",@IOLineNumber="+(ltIOLineNumber.Text!=""?ltIOLineNumber.Text:"NULL"));
            cmdUpdateIODetails.Append(",@MaterialMasterID="+hifMaterialMaster.Value);
            cmdUpdateIODetails.Append(",@MaterialMaster_IOUoMID="+hifMaterialMaster_IOUoMID.Value);
            cmdUpdateIODetails.Append(",@IOQuantity="+txtIOQuantity.Text.Trim());
            cmdUpdateIODetails.Append(",@CreatedBy="+cp.UserID);

            String command = "";
            String ids = "";
            String values = "";
            String MSPID = "";
            String mspsql = "EXEC [sp_ORD_GetMaterialStorageParameters]  @MaterialMasterID=" + hifMaterialMaster.Value + ",@TenantID=" + cp.TenantID;
            IDataReader rsMCodeMSP = null;
            try
            {
                rsMCodeMSP = DB.GetRS(mspsql);
            }
            catch (Exception ex)
            {
                resetError("Error while updating",true,false);
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
                return;
            }
            while (rsMCodeMSP.Read())
            {
                command = DB.RSField(rsMCodeMSP, "ControlType");
                if (command == "DropDownList")
                {
                    MSPID = "ddl" + DB.RSField(rsMCodeMSP, "ParameterName");
                    String value = ((DropDownList)editRow.FindControl(MSPID)).SelectedValue;
                    if (value != "")
                    {
                        ids += DB.RSFieldInt(rsMCodeMSP, "MaterialStorageParameterID") + ",";
                        values += value + ",";
                    }


                }

                else if (command == "TextBox")
                {

                    MSPID = "txt" + DB.RSField(rsMCodeMSP, "ParameterName");
                    String value = ((TextBox)editRow.FindControl(MSPID)).Text;
                    if (value != "")
                    {
                        ids += DB.RSFieldInt(rsMCodeMSP, "MaterialStorageParameterID") + ",";
                        values += value + ",";
                    }
                }
            }
            rsMCodeMSP.Close();

            cmdUpdateIODetails.Append(",@MaterialStorageParameterIDs=" + DB.SQuote(ids));
            cmdUpdateIODetails.Append(",@MaterialStorageParameterValues=" + DB.SQuote(values));

            try
            {
                DB.ExecuteSQL(cmdUpdateIODetails.ToString());
                gvIODetailsList.EditIndex = -1;
                Build_InternalOrderList(Build_InternalOrderList());
                resetgridError("Successfully Updated", false);
                InternalOrderVerification();
            }
            catch (Exception ex)
            {
                resetgridError("Error while updating", true);
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
            }

        }

        protected void gvIODetailsList_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
          
                gvIODetailsList.EditIndex = -1;
                Build_InternalOrderList(Build_InternalOrderList());
                if (gvIODetailsList.Rows.Count == 0)
                {
                    pnlgvInternalOrderDetails.Visible = false;
                }
          
        }

        protected void lnkClear_Click(object sender, EventArgs e)
        {
            Response.Redirect("InternalOrderList.aspx");
        }

        protected void lnkUpdate_Click(object sender, EventArgs e)
        {
            Page.Validate("save");
            if (!IsValid)
                return;

            StringBuilder cmdUpdateIOHeader = new StringBuilder();
            cmdUpdateIOHeader.Append("DECLARE @NewHeaderID int ");
            cmdUpdateIOHeader.Append(" EXEC [dbo].[sp_MFG_UpsertInternalOrderHeader] ");
            cmdUpdateIOHeader.Append("@InternalOrderHeaderID="+ViewState["HeaderID"]);
            cmdUpdateIOHeader.Append(",@IORefNo="+DB.SQuote(txtIORefNo.Text.Trim())); 
            cmdUpdateIOHeader.Append(",@ProductionOrderHeaderID="+hifProductionOrderHeader.Value);
            cmdUpdateIOHeader.Append(",@WorkCenterID="+hifWorkCenter.Value);
            cmdUpdateIOHeader.Append(",@RoutingDetailsID="+(hifRouotingOperation.Value!=""&&atcRoutingoperation.Text!=""?hifRouotingOperation.Value:"NULL"));
            cmdUpdateIOHeader.Append(",@RoutingDetailsActivityID="+(hifActivity.Value!=""&&atcActivity.Text!=""?hifActivity.Value:"NULL"));
            cmdUpdateIOHeader.Append(",@RequestedBy="+(hifRequestedBy.Value!=""&&atcRequestedBy.Text!=""?hifRequestedBy.Value:"NULL"));
            cmdUpdateIOHeader.Append(",@ReasonForInternalOrderRequestID="+hifReasonForInternalOrderRequest.Value);
            cmdUpdateIOHeader.Append(",@Remarks="+(txtRemarks.Text.Trim()!=""?DB.SQuote(txtRemarks.Text.Trim()):"NULL"));
            cmdUpdateIOHeader.Append(",@CreatedBy="+cp.UserID);
            cmdUpdateIOHeader.Append(",@IsActive=" + Convert.ToInt16(chkIsActive.Checked));
            cmdUpdateIOHeader.Append(",@IsDeleted=" + Convert.ToInt16(chkIsDeleted.Checked));
            cmdUpdateIOHeader.Append(",@NewInternalOrderHeaderID=@NewHeaderID output Select @NewHeaderID as N");
            Boolean IsDeleted = chkIsDeleted.Checked;
            try
            {
                ViewState["HeaderID"] = DB.GetSqlN(cmdUpdateIOHeader.ToString());
                if (IsDeleted == true)
                {
                    Response.Redirect("InternalOrderList.aspx");
                }

                lnkAddNewLineItem.Visible = true;
                tdlineitems.Visible = true;
                lnkUpdate.Text = "Update"+CommonLogic.btnfaUpdate;
                ViewState["InternalOrderDetailsList"] = "EXEC [dbo].[sp_MFG_GetInternalOrderDetails] @InternalOrderHeaderID=" + ViewState["HeaderID"];
                resetError("Successfully Updated", false, false);
            }
            catch (SqlException sqlex)
            {
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), sqlex.Source, sqlex.Message, sqlex.StackTrace);
                //if (sqlex.ErrorCode == -2146232060)
                if (sqlex.Message.StartsWith("Violation of UNIQUE KEY constraint 'UK_IORefNo'"))
                {
                    resetError("NC Ref. No. already exists,regenerate NC Ref. No. and update", true, false);
                    return;
                }
                resetError("Error while updating", true,false);
            }
            catch (Exception ex)
            {
                resetError("Error while updating", true,false);
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
            }
           


        }

        protected void lnkAddNewLineItem_Click(object sender, EventArgs e)
        {
           
                DataSet dsInternalorderList = Build_InternalOrderList();
                DataRow newRow = dsInternalorderList.Tables[0].NewRow();
                int LineNumber = DB.GetSqlN("select top 1 IOLineNumber as N from MFG_InternalOrderDetails where IsDeleted=0 and IsActive=1 and InternalOrderHeaderID=" + ViewState["HeaderID"] + " order by IOLineNumber desc");

                newRow["InternalOrderDetailsID"] = 0;
                //newRow["IOLineNumber"] = ++LineNumber;
                newRow["MaterialMaster_IOUoMID"] = 0;
                dsInternalorderList.Tables[0].Rows.InsertAt(newRow, 0);
                gvIODetailsList.PageIndex = 0;
                gvIODetailsList.EditIndex = 0;
                pnlgvInternalOrderDetails.Visible = true;
                Build_InternalOrderList(dsInternalorderList);
           
        }

        protected void IbutNew_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                String NewIONumber = DB.GetSqlS("EXEC sp_SYS_GetSystemConfigValue @SysConfigKey=N'internalorder.aspx.cs.IO_Prefix',@TenantID=" + cp.TenantID);

                int length = Convert.ToInt32(DB.GetSqlS("EXEC sp_SYS_GetSystemConfigValue @SysConfigKey=N'internalorder.aspx.cs.IO_Length',@TenantID=" + cp.TenantID));


                //string HeaderID = ViewState["HeaderID"].ToString();
                String OldIONumber = DB.GetSqlS("select top 1 IORefNo as S from MFG_InternalOrderHeader  order by IORefNo desc");

                NewIONumber += "" + (Convert.ToInt16(DateTime.Now.Year) % 100);           //add year code to ponumber

                int power = (Int32)Math.Pow((double)10, (double)(length - 1));          //getting minvalue of prifix length

                String newvalue = "";
                if (OldIONumber != "" && NewIONumber.Equals(OldIONumber.Substring(0, NewIONumber.Length)))        //if ponumber is existed and same year ponumber  enter
                {
                    String temp = OldIONumber.Substring(NewIONumber.Length, length);                            //getting number of last prifix
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

                NewIONumber += newvalue;
                txtIORefNo.Text = NewIONumber;
            }
            catch (Exception ex)
            {
                resetError("Error while generating IONumber", true,false);
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
            }
            if (!atckitCode.Enabled)
                atcWorkCenter.Focus();
            else
                atckitCode.Focus();
            //Page.Form.DefaultButton = lnkUpdate.UniqueID;
           // IbutNew.Focus();
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
            ScriptManager.RegisterStartupScript(this, this.GetType(), "test", "showStickyToast(" + (isError.ToString().ToLower() == "true" ? "false" : "true") + ",\"" + error + "\"," + ispermanent.ToString().ToLower() + ");", true);

        }

        protected void resetgridError(string error, bool isError)
        {

            /*string str = "<font class=\"noticeMsg\">NOTICE:</font>&nbsp;&nbsp;&nbsp;";
            if (isError)
                str = "<font class=\"errorMsg\">ERROR:</font>&nbsp;&nbsp;&nbsp;";

            if (error.Length > 0)
                str += error + "";
            else
                str = "";


            ltGridStatus.Text = str;*/
            ScriptManager.RegisterStartupScript(this, this.GetType(), "test", "showStickyToast(" + (isError.ToString().ToLower() == "true" ? "false" : "true") + ",\"" + error + "\");", true);

        }

        private DataSet Build_InternalOrderList()
        {
            DataSet dsInternalorderList = null;
            String cmdInternalOrderList = ViewState["InternalOrderDetailsList"].ToString();
            try
            {
                dsInternalorderList = DB.GetDS(cmdInternalOrderList, false);
            }
            catch (Exception ex)
            {
                resetError("Error while loading",true,false);
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
            }
            return dsInternalorderList;
        }

        private void Build_InternalOrderList(DataSet dsInternalorderList)
        {
            gvIODetailsList.DataSource = dsInternalorderList;
            gvIODetailsList.DataBind();
            dsInternalorderList.Dispose();
        }

        protected void lnkIsDeleted_Click(object sender, EventArgs e)
        {
           
        }

        protected void lnkCreateOutbount_Click(object sender, EventArgs e)
        {
            String sbDummyOutbound;

            try
            {

                String OBDNumber = GetOBDNumber();

                String SONumber = GetSONumber();

                String CUSPONumber = GetCUSPONumber();

                if (OBDNumber != "" && SONumber != "" && CUSPONumber != "")
                {

                    sbDummyOutbound = "DECLARE @NewUpdateOutboundID int; EXEC  [sp_MFG_CreateDummyOutbountForInternalOrder]   @InternalOrderHeaderID=" + CommonLogic.QueryString("ioid") + " ,@SONumber =" + DB.SQuote(SONumber) + ",@CusPONumber=" + DB.SQuote(CUSPONumber) + ",@OBDNumber =" + DB.SQuote(OBDNumber) + ",@CreatedBy = " + cp.UserID + ",@Tenant=" + cp.TenantID + ",@NewOutboundID=@NewUpdateOutboundID OUTPUT; select @NewUpdateOutboundID AS N";

                    if (DB.GetSqlN(sbDummyOutbound.ToString()) != 0)
                    {
                        resetError("Released NC order with OBDNumber:" + OBDNumber, false, true);

                    }
                    else
                    {
                        resetError("Error while updating release NC order ", true, false);
                    }

                }

                Build_FormData();
            }
            catch (Exception ex)
            {
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
               // resetError("Error while updating release job order", true, false);
            }
        }

        private void AddDynamicColumns()
        {
            IDataReader rsMSPList = null;
            TemplateField field;
            try
            {
                rsMSPList = DB.GetRS("sp_GEN_GetAllMaterialStorageParameters @ParameterUsageTypeID=1,@TenantID=" + cp.TenantID);
            }
            catch (Exception ex)
            {
                resetError("Error while loading",true,false);
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
                return;
            }

            while (rsMSPList.Read())
            {
                field = new TemplateField();

                field.HeaderTemplate = new DynamicallyTemplatedGridViewHandler(ListItemType.Header, DB.RSField(rsMSPList, "DisplayName"), "Literal");

                field.ItemTemplate = new DynamicallyTemplatedGridViewHandler(ListItemType.Item, DB.RSField(rsMSPList, "ParameterName"), "Literal");
                
                field.HeaderStyle.Width = 150;

                field.ItemStyle.Width = 150;
                              
                field.EditItemTemplate = new DynamicallyTemplatedGridViewHandler(ListItemType.EditItem, DB.RSField(rsMSPList, "ParameterName"), DB.RSField(rsMSPList, "ControlType"), DB.RSField(rsMSPList, "ParameterDataType"), DB.RSField(rsMSPList, "DataSource"), DB.RSField(rsMSPList, "MinTolerance"), DB.RSField(rsMSPList, "MaxTolerance"));

                gvIODetailsList.Columns.Add(field);
            }

            rsMSPList.Close();

            field = new TemplateField();
            field.HeaderTemplate = new DynamicallyTemplatedGridViewHandler(ListItemType.Header, "Delete", "Literal");
            field.ItemTemplate = new DynamicallyTemplatedGridViewHandler(ListItemType.Item, "Delete", "CheckBox");
            field.EditItemTemplate = new DynamicallyTemplatedGridViewHandler(ListItemType.EditItem, "Delete", "Empty");
            LinkButton button = new LinkButton();
            button.ID = "deleteButton";
            button.Text = "Delete";
            button.ForeColor = System.Drawing.Color.Blue;
            button.OnClientClick = "return confirm('Are you Sure want to delete?')";
            button.Click += new EventHandler(DeleteItems);
            button.Font.Underline = false;
            field.FooterTemplate = new DynamicallyTemplatedGridViewHandler(ListItemType.Footer, button);


            gvIODetailsList.Columns.Add(field);


            BoundField editfield;
            editfield = new BoundField();
            editfield.DataField = "EditName";
            editfield.ReadOnly = true;
            editfield.Visible = false;
            gvIODetailsList.Columns.Add(editfield);

            CommandField cmdfield = new CommandField();


            cmdfield.CausesValidation = true;
            cmdfield.ButtonType = ButtonType.Link;
            cmdfield.CancelText = "Cancel";
            cmdfield.FooterStyle.Font.Underline = false;
            cmdfield.EditText = "<nobr> Edit <img src='../Images/redarrowright.gif' border='0' /></nobr>";
            cmdfield.ShowEditButton = true;
            cmdfield.ControlStyle.Font.Underline = false;
            cmdfield.ItemStyle.Font.Underline = false;
            cmdfield.UpdateImageUrl = "icons/update.gif";
            cmdfield.UpdateText = "Update";


            gvIODetailsList.Columns.Add(cmdfield);

        }

        protected void DeleteItems(object sender, EventArgs e)
        {
            int rowCount = gvIODetailsList.Rows.Count;
            GridViewRow row;
            String IODetailsIDs = "";
            for (int rowNo = 0; rowNo < rowCount; rowNo++)
            {
                row = gvIODetailsList.Rows[rowNo];
                if (gvIODetailsList.EditIndex != rowNo)
                {
                    if (((CheckBox)row.FindControl("Delete")).Checked)
                    {
                        IODetailsIDs += ((Literal)row.FindControl("ltInternalOrderDetailsID")).Text + ",";

                    }
                }
            }

            try
            {
                DB.ExecuteSQL("Exec [dbo].[sp_MFG_DeleteInternalOrderDetails] @InternalOrderDetailsIDs=" + DB.SQuote(IODetailsIDs)+ ",@UpdatedBy="+cp.UserID.ToString());
                gvIODetailsList.EditIndex = -1;
                Build_InternalOrderList(Build_InternalOrderList());
                if(gvIODetailsList.Rows.Count==0)
                    pnlgvInternalOrderDetails.Visible = false;
                resetgridError("Successfully deleted the selected line items", false);
                InternalOrderVerification();
            }
            catch (Exception ex)
            {
                resetgridError("Error while deleting", true);
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
            }
            
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

        private String GetSONumber()
        {
            String SONumber = "";
            try
            {
                String NewSONumber = DB.GetSqlS("EXEC [sp_SYS_GetSystemConfigValue]   @SysConfigKey='dummysoheaderforinternalorder_prefix' ,@TenantID=" + cp.TenantID);
                NewSONumber += "" + (Convert.ToInt16(DateTime.Now.Year) % 100);

                int length = Convert.ToInt32(DB.GetSqlS("select SysConfigValue as S from SYS_SystemConfiguration as N where TenantID=" + cp.TenantID + " and SysConfigKeyID= (select SysConfigKeyID from SYS_SysConfigKey where SysConfigKey=N'salesorder.aspx.cs.SO_Length') "));

                //String OldSONumber = DB.GetSqlS("select TOP 1 SONumber as S from ORD_SOHeader where SOTypeID =6 and TenantID=" + cp.TenantID + "  ORDER BY SONumber desc ");
                String OldSONumber = DB.GetSqlS("select TOP 1 SONumber as S from ORD_SOHeader where SONumber like '"+NewSONumber+"%' ORDER BY SONumber desc ");

                           //add year code to ponumber

                int power = (Int32)Math.Pow((double)10, (double)(length - 1));          //getting minvalue of prifix length

                String newvalue = "";
                if (OldSONumber != "" && NewSONumber.Equals(OldSONumber.Substring(0, NewSONumber.Length)))        //if ponumber is existed and same year ponumber  enter
                {
                    String temp = OldSONumber.Substring(NewSONumber.Length, length);                            //getting number of last prifix
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

        private String GetCUSPONumber()
        {
            Random generateNo = new Random();
            return "DCUSPO" + generateNo.Next(1000, 10000);
        }

        protected void lnkDeficiency_Click(object sender, EventArgs e)
        {
            Build_DeficiencyMaterialList(Build_DeficiencyMaterialList());
            
            
        }

        protected void gvNCDefcList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvNCDefcList.PageIndex = e.NewPageIndex;
            Build_DeficiencyMaterialList(Build_DeficiencyMaterialList());
        }

        protected void gvNCDefcList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.DataItem == null)
                return;

            Literal ltDeficiency = (Literal)e.Row.FindControl("ltDeficiency");

            if (ltDeficiency.Text != "0.00")
                e.Row.CssClass = "DeficiencyRow";
        }

        private DataSet Build_DeficiencyMaterialList()
        {
            DataSet dsGetDeficiencyMaterialList = null;
            String cMdGetDeficiencyMaterialList = "sp_MFG_InternalOrderMaterialDeficiency @InternalOrderHeaderID="+ViewState["HeaderID"];
            try
            {
                dsGetDeficiencyMaterialList = DB.GetDS(cMdGetDeficiencyMaterialList, false);
            }
            catch (Exception ex)
            {
                resetError("Error while loading",true,false);
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
            }
            return dsGetDeficiencyMaterialList;
        }

        private void Build_DeficiencyMaterialList(DataSet dsGetDeficiencyMaterialList)
        {
            gvNCDefcList.DataSource = dsGetDeficiencyMaterialList;
            gvNCDefcList.DataBind();
            dsGetDeficiencyMaterialList.Dispose();

            ScriptManager.RegisterStartupScript(this, this.GetType(), "NCUnblockDialog", "NCUnblockDialog();", true);
        }
    }
}