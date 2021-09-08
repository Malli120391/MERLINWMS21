using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using MRLWMSC21Common;
using System.Web.UI.HtmlControls;
using System.Globalization;

namespace MRLWMSC21.mInventory
{

    //Author    : Gvd Prasad
    //Created On: 20-Dec-2013
    //Use case ID:  Active Stock_UC_019
   
    public partial class ActivityReport : System.Web.UI.Page
    {
        public CustomPrincipal cp = HttpContext.Current.User as CustomPrincipal;
        public int FromCartonID = 0;
        protected void page_PreInit(object sender, EventArgs e)
        {
            Page.Theme = "Inventory";
        }

        protected void page_Init(object sender, EventArgs e)
        {
            AddDynamicColumns(CommonLogic.QueryString("mmid"));
            AddMspTextBoxs(CommonLogic.QueryString("mmid"));

            if (CommonLogic.QueryString("TransactionType")=="1")
            {
                //Change Header text as for Inbound Details
                SetInboundHeaderValue();
                gvActiveReport.Columns[0].HeaderText = "Store Ref. No.";
                gvActiveReport.Columns[2].HeaderText = "PO Number";
                gvActiveReport.Columns[7].HeaderText = "Qty. Received in BUoM";

            }
            else
            {
                //Change Header text as for Outbound Details
                gvActiveReport.Columns[1].Visible = false;
                gvActiveReport.Columns[0].HeaderText = "OBD Number";
                gvActiveReport.Columns[2].HeaderText = "SO Number";
                gvActiveReport.Columns[7].HeaderText = "Qty. Issued in BUoM";
            }

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //if (!cp.IsInAnyRoles(CommonLogic.GetRolesAllowed(CommonLogic.GetRolesAllowedForthisPage("Active Stock"))))
            //{
            //    //Arrange Search MSPs In Web Page 
            //    // This control is for Dynamic Control adding

            //    Response.Redirect("../Login.aspx?eid=6");
            //}
            if (!IsPostBack)
            {
                
                ltMcode.Text = DB.GetSqlS("select MCode + isnull(' [' + OEMPartNo+']','')  as S from MMT_MaterialMaster where MaterialMasterID=" + CommonLogic.QueryString("mmid"));
                
                if (CommonLogic.QueryString("TransactionType") == "1")
                {                   
                    DesignLogic.SetInnerPageSubHeading(this.Page, "Inbound Activity Report");
                }
                else
                {
                    DesignLogic.SetInnerPageSubHeading(this.Page, "Outbound Activity Report");
                }
                Build_gvActiveReport(Build_gvActiveReport());
            }
        }

        private void SetInboundHeaderValue()
        {
            int Mtype = DB.GetSqlN("select MTypeID as N from MMT_MaterialMaster where MaterialMasterID=" + CommonLogic.QueryString("mmid"));
            if (Mtype == 7)
            {
                gvCustomerDetails.Columns[1].HeaderText = "SO Number";
            }
            else if (Mtype == 9)
            {
                gvCustomerDetails.Columns[1].HeaderText = "SO Number";
            }
            else if (Mtype == 12)
            {
                gvCustomerDetails.Columns[1].HeaderText = "TMO Number";
            }
        }

        protected void gvActiveReport_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvActiveReport.EditIndex = e.NewEditIndex;
            Build_gvActiveReport(Build_gvActiveReport());
        }

        protected void gvActiveReport_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvActiveReport.EditIndex = -1;
            Build_gvActiveReport(Build_gvActiveReport());
        }

        protected void gvActiveReport_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {

            GridViewRow gvRow = gvActiveReport.Rows[e.RowIndex];
            String GoodsMovementDetailsID = ((Literal)gvRow.FindControl("ltGoodsMovementDetailsID")).Text;
            String IsDeleted = (Convert.ToInt16(((CheckBox)gvRow.FindControl("chkIsDeleted")).Checked)).ToString();
            if (CommonLogic.QueryString("TransactionType") == "1")
            {
                Decimal TotalQuantity = Convert.ToDecimal(((Literal)gvRow.FindControl("ltreceivedqty")).Text);
                Decimal AvialableQuantity = Convert.ToDecimal(((Literal)gvRow.FindControl("QtyinBUoM")).Text);
               
                if (TotalQuantity != AvialableQuantity)
                {
                    resetError("Quantity picked,cannot edit",true);
                    return;
                }
            }
           
            StringBuilder UpdateGoodsMovementDetails = new StringBuilder();
            UpdateGoodsMovementDetails.Append("[dbo].[sp_INV_UpsertActivityReport]  ");
            UpdateGoodsMovementDetails.Append("@GoodsMovementDetailsID="+GoodsMovementDetailsID);

            UpdateGoodsMovementDetails.Append(",@IsDeleted=" + IsDeleted);
            UpdateGoodsMovementDetails.Append(",@UpdatedBy=" + cp.UserID.ToString());
            try
            {
                DB.ExecuteSQL(UpdateGoodsMovementDetails.ToString());
                gvActiveReport.EditIndex = -1;
                Build_gvActiveReport(Build_gvActiveReport());
                resetError("Updated successfully",false);
            }
            catch (Exception ex)
            {
                resetError("Error while updating",true);
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
            }
        }

        private DataSet Build_gvActiveReport()
        {
            DataSet dsActivityReport = null;
            try
            {
                String sCmdAtivityReport = "sp_INV_GetActivityReportForMaterial @MaterialMasterID=" + CommonLogic.QueryString("mmid") + ",@GoodsMovementTypeID=" + CommonLogic.QueryString("TransactionType") + ",@TenantID=" + CommonLogic.QueryString("tid");
                dsActivityReport = DB.GetDS(sCmdAtivityReport, false);
            }
            catch (Exception ex)
            {
                resetError("Error while loading",true);
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
            }
            return dsActivityReport;
        }

        private void Build_gvActiveReport(DataSet dsActivity)
        {
            gvActiveReport.DataSource = dsActivity;
            gvActiveReport.DataBind();
            dsActivity.Dispose();
        }

        private void AddDynamicColumns(String mmid)
        {

            TemplateField field;
            try
            {
                IDataReader rsMSPList = DB.GetRS("[sp_ORD_GetMaterialStorageParameters]  @MaterialMasterID=" + mmid + ",@TenantID=" + cp.TenantID);

                while (rsMSPList.Read())
                {
                    field = new TemplateField();
                    field.HeaderText = DB.RSField(rsMSPList, "DisplayName");
                    field.HeaderTemplate = new DynamicallyTemplatedGridViewHandler(ListItemType.Header, DB.RSField(rsMSPList, "DisplayName"), "Literal");

                    field.ItemTemplate = new DynamicallyTemplatedGridViewHandler(ListItemType.Item, DB.RSField(rsMSPList, "ParameterName"), "Literal");


                    gvActiveReport.Columns.Add(field);


                }

                rsMSPList.Close();

                field = new TemplateField();
                field.HeaderTemplate = new DynamicallyTemplatedGridViewHandler(ListItemType.Header, "Active", "Literal");
                field.ItemTemplate = new DynamicallyTemplatedGridViewHandler(ListItemType.Item, "IsDeleted", "Literal");
                field.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                field.EditItemTemplate = new DynamicallyTemplatedGridViewHandler(ListItemType.EditItem, "IsDeleted", "CheckBox");
                gvActiveReport.Columns.Add(field);



                if (CommonLogic.QueryString("TransactionType") == "1" && cp.TenantID == 0)
                {
                    //Add Stock Adjustment functionality for inbound detetials

                    field = new TemplateField();
                    //field.ItemTemplate.

                    //field.HeaderText = "Split Material";

                    //field.ItemTemplate = new DynamicallyTemplatedGridViewHandler(ListItemType.Item, new EventHandler(lnkSplitLocation), "Split", "LinkButton", "Stock Adjustment");
                    //field.EditItemTemplate = new DynamicallyTemplatedGridViewHandler(ListItemType.EditItem, "Split", "Empty");
                    //field.ItemStyle.Width = 100;
                    ////if (cp.TenantID != 0)
                    //{
                    //    gvActiveReport.Columns.Add(field);
                    //}


                    field = new TemplateField();
                    field.HeaderTemplate = new DynamicallyTemplatedGridViewHandler(ListItemType.Header, "Delete", "Literal");
                    field.ItemTemplate = new DynamicallyTemplatedGridViewHandler(ListItemType.Item, "Delete", "CheckBox");
                    field.EditItemTemplate = new DynamicallyTemplatedGridViewHandler(ListItemType.EditItem, "Delete", "Empty");


                    CommandField cmdfield = new CommandField();
                    cmdfield.CausesValidation = true;
                    cmdfield.ButtonType = ButtonType.Link;
                    cmdfield.CancelText = "Cancel";

                    cmdfield.FooterStyle.Font.Underline = false;
                    cmdfield.EditText = "<nobr> Edit <img src='../Images/redarrowright.gif' border='0' /></nobr>";
                    cmdfield.ShowEditButton = true;
                    cmdfield.ControlStyle.Font.Underline = false;

                    cmdfield.ValidationGroup = "gridrequirequantity";
                    cmdfield.UpdateImageUrl = "icons/update.gif";
                    cmdfield.UpdateText = "Update";


                    // gvActiveReport.Columns.Add(cmdfield);

                }
            }
            catch (Exception ex)
            {
                resetError("Error while add Material Storage Parameters,try again", true);
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
            }
        }

        protected void lnkSplitLocation(object sender, EventArgs e)
        {



        }
        
        protected void lnkSplit_Click(object sender, EventArgs e)
        {
            //test case ID (TC_035)
            //Stock adjustment based on user specification

            //if (!cp.IsInAnyRoles(CommonLogic.GetRolesAllowed("13")))
            //{
            //    //test case ID(TC_048)
            //    //Only MMAdmin can split the stock

            //    resetError("Only MMAdmin can adjust the stock",true);
            //    return;
            //}
            Page.Validate("splitQuantity");
            if (!Page.IsValid)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "test", "closeAsynchronus();", true);

                return;
            }
            Decimal SplitQty =Convert.ToDecimal (txtSplitQty.Text.Trim());
            String kitID = lbkitid.Text.Trim();
            Decimal TotalQty = Convert.ToDecimal (hifTotalQty.Value);
            String ISDamg=(Convert.ToInt16 (chkIsDamag.Checked)).ToString();
            String HasDisc = (Convert.ToInt16(chkDisc.Checked)).ToString();
            String IsNon=(Convert.ToInt16(chkisNonconformity.Checked)).ToString();
            String Location = txtLocation.Text.Trim();
            String ToCantainID = hdncarton.Value.Trim(); 
            String FromCantainerID = hiffromcartonID.Value.Trim(); 
            String GoodsMovementDetailsID = hifGoodsMovementDetailsID.Value.Trim();

            if (ToCantainID == FromCantainerID)
            {
                resetSplitWindowError("From Carton and To Carton are same.", true);
                return;
            }

            if (DB.GetSqlN("sp_INV_IsInSupplierReturns @GoodsMovementDetailsID=" + GoodsMovementDetailsID) != 0)
            {
                //test case ID (TC_047)
                // material is in supplier returns, we dont stock adjustment

                resetSplitWindowError("Cannot split, as selected material is returned to the supplier", true);
                return;
            }
            int LocationID = DB.GetSqlN("select LocationID as N from INV_Location where IsDeleted=0 and Location=" + DB.SQuote(Location));
            if (LocationID == 0)
            {
                //test case ID (TC_046)
                //Not a valid location

                resetSplitWindowError("Not a valid location", true);
                return;
            }
            if (TotalQty < SplitQty)
            {
                //test case ID (TC_036)
                //Split qty. should be less than or equal to available qty.

                resetSplitWindowError("Cannot split, as split quantity should be less than or equal to available quantity ", true);
                return;
            }
            if (SplitQty<=0)
            {
                resetSplitWindowError("Cannot split, as quantity should be greater than zero", true);
                return;
            }
            if (TotalQty == 0)
            {
                //test case ID (TC_040)
                //Qty. must be greater than zero

                resetSplitWindowError("Not available quantity, cannot split", true);
                return;
            }

            int CycleCountOn = DB.GetSqlN("select convert(int,max(qcc.IsOn)) as N from QCC_CycleCount qcc left join QCC_CycleCountDetails qccd on qccd.CycleCountID=qcc.CycleCountID  where qccd.MaterialMasterID=" + CommonLogic.QueryString("mmid"));
            if (CycleCountOn == 1)
            {
                //test case ID (TC_045)
                //Cannot split, as material is in cycle count

                resetError("Cannot split, as this material is in 'Cycle Count' ", true);
                return;
            }

            IDataReader rsConfigureMSP = DB.GetRS("[sp_ORD_GetMaterialStorageParameters]  @MaterialMasterID=" + CommonLogic.QueryString("mmid") + ",@TenantID=" + cp.TenantID);
            String ControlID = "";
            String value = "";
            String MSPIDs = "";
            String MSPValues = "";
            while (rsConfigureMSP.Read())
            {
                String ControlType = DB.RSField(rsConfigureMSP, "ControlType");


                if (ControlType == "DropDownList")
                {
                    ControlID = "ddl" + DB.RSField(rsConfigureMSP, "ParameterName");
                    value = ((DropDownList)tbsplitmaterial.FindControl(ControlID)).SelectedValue;

                }
                else if (ControlType == "TextBox")
                {
                    ControlID = "txt" + DB.RSField(rsConfigureMSP, "ParameterName");
                    TextBox txtTextBox = (TextBox)tbsplitmaterial.FindControl(ControlID);
                    if ( DB.RSField(rsConfigureMSP, "ParameterName") == "ExpDate")
                    {

                        if (txtTextBox != null && txtTextBox.Text != "")
                        {
                            DateTime expiredate = DateTime.ParseExact(txtTextBox.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                            int result = DateTime.Compare(expiredate, DateTime.Now);
                            if (result < 0)
                            {
                                //test case ID (TC_044)
                                //Item is expired, cannot split

                                resetError("Item is expired, cannot split", true);
                                rsConfigureMSP.Close();
                                return;
                            }
                        }

                    }
                    else if (DB.RSField(rsConfigureMSP, "ParameterName") == "MfgDate")
                    {

                        if (txtTextBox != null && txtTextBox.Text != "")
                        {
                            DateTime expiredate = DateTime.ParseExact(txtTextBox.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                            int result = DateTime.Compare(expiredate, DateTime.Now);
                            if (result > 0)
                            {
                                //test case ID (TC_043)
                                //Mfg. date should not exceed system date, cannot received

                                resetError("Mfg. date should not exceed system date, cannot received", true);
                                rsConfigureMSP.Close();
                                return;
                            }
                        }
                    }
                    else if (DB.RSField(rsConfigureMSP, "ParameterName") == "SerialNo")
                    {

                        if (txtTextBox != null && txtTextBox.Text != "")
                        {

                            if (Convert.ToDecimal(SplitQty) != 1)
                            {
                                //test case ID (TC_041)
                                //Qty. should be one for serial no. 

                                resetError("Quantity should be 1 for serial no., cannot split", true);
                                rsConfigureMSP.Close();
                                return;
                            }
                            if (DB.GetSqlN("select GMDMSP.GoodsMovementDetails_MaterialStorageParameterID as N from INV_GoodsMovementDetails GMD JOIN INV_GoodsMovementDetails_MMT_MaterialStorageParameter GMDMSP ON GMD.GoodsMovementDetailsID=GMDMSP.GoodsMovementDetailsID AND GMDMSP.MaterialStorageParameterID=3 AND GMDMSP.IsDeleted=0 WHERE GMD.GoodsMovementTypeID=1 AND GMD.IsDeleted=0 and GMD.MaterialMasterID=" + CommonLogic.QueryString("mmid") + "  AND CONVERT(NVARCHAR,GMDMSP.Value)=" + DB.SQuote(txtTextBox.Text)) != 0)
                            {
                                //test case ID (TC_042)
                                //Serial no. already exists, cannot split

                                resetError("Serial no. already exists, cannot split", true);
                                return;
                            }

                        }
                    }
                    value = ((TextBox)tbsplitmaterial.FindControl(ControlID)).Text.Trim();
                }
                if (value != "")
                {
                    MSPIDs += DB.RSFieldInt(rsConfigureMSP, "MaterialStorageParameterID") + ",";
                    MSPValues += value + ",";
                }
            }
            rsConfigureMSP.Close();

            StringBuilder splitMaterial = new StringBuilder();
            splitMaterial.Append("[dbo].[sp_INV_StockInMaterial_Split] ");
            splitMaterial.Append("@GoodsMovementDetailsID=" + GoodsMovementDetailsID);
            splitMaterial.Append(",@LocationID=" + LocationID);
            splitMaterial.Append(",@KitPlannerID=" + (kitID!="0"?kitID:"NULL"));
            splitMaterial.Append(",@DocQty=" + SplitQty);
            splitMaterial.Append(",@IsDamaged=" + ISDamg);
            splitMaterial.Append(",@HasDiscrepancy=" + HasDisc);
            splitMaterial.Append(",@IsNonConfirmity="+IsNon);
            splitMaterial.Append(",@LastModifiedBy=" + cp.UserID);
            splitMaterial.Append(",@MSPIDs=" + DB.SQuote(MSPIDs));
            splitMaterial.Append(",@MSPValues=" + DB.SQuote(MSPValues));
            splitMaterial.Append(",@FromCartonID=" +(FromCantainerID));
            splitMaterial.Append(",@ToCartonID=" + (ToCantainID));
            splitMaterial.Append(",@UserID=" + (cp.UserID));

            try
            {
                DB.ExecuteSQL(splitMaterial.ToString());
                Build_gvActiveReport(Build_gvActiveReport());
                resetSplitWindowError("Stock adjustment successfully posted",false);
                BuildDialogBox(GoodsMovementDetailsID);
            }
            catch (Exception ex)
            {
                resetSplitWindowError("Error while stock adjustment", true);
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
            }

        }

        private void AddMspTextBoxs(String mmid)
        {
            //Add Dinamic MSP Controls to Stock Adjustment Dailogbox


            try
            {
                IDataReader drMatrialMsp = DB.GetRS("[sp_ORD_GetMaterialStorageParameters]  @MaterialMasterID=" + mmid + ",@TenantID=" + cp.TenantID);

                int mspCount = 0;
                HtmlTableRow htRow = null;
                HtmlTableCell htCell;
                TextBox txttextbox;
                Literal ltliteral;
                DropDownList ddlDropDownList;
                while (drMatrialMsp.Read())
                {


                    if (mspCount++ % 2 == 0)
                    {
                        //Arrange left cell in the row

                        htRow = new HtmlTableRow();
                        htCell = new HtmlTableCell();
                        htCell.Align = "left";
                        htCell.Width = "50%";
                        ltliteral = new Literal();
                        ltliteral.ID = "lt" + DB.RSField(drMatrialMsp, "ParameterName");
                        ltliteral.Text = DB.RSField(drMatrialMsp, "DisplayName") + ":<br />";

                        String ControlType = DB.RSField(drMatrialMsp, "ControlType");
                        RequiredFieldValidator validator = new RequiredFieldValidator();
                        validator.ValidationGroup = "splitQuantity";
                        validator.ID = "rfv" + DB.RSField(drMatrialMsp, "ParameterName");
                        validator.Display = ValidatorDisplay.Dynamic;
                        validator.ErrorMessage = "*";
                        if (ControlType == "DropDownList")
                        {

                            ddlDropDownList = new DropDownList();

                            ddlDropDownList.ID = "ddl" + DB.RSField(drMatrialMsp, "ParameterName");
                            ddlDropDownList.Width = 120;
                            BuildDynamicDropDown(ddlDropDownList, DB.RSField(drMatrialMsp, "DataSource"), DB.RSField(drMatrialMsp, "ParameterName"));

                            validator.ControlToValidate = ddlDropDownList.ID;
                            if (DB.RSFieldTinyInt(drMatrialMsp, "IsRequired") == 1)
                                htCell.Controls.Add(validator);
                            htCell.Controls.Add(ltliteral);
                            htCell.Controls.Add(ddlDropDownList);
                        }
                        else if (ControlType == "TextBox")
                        {
                            txttextbox = new TextBox();
                            txttextbox.EnableTheming = false;
                            txttextbox.ID = "txt" + DB.RSField(drMatrialMsp, "ParameterName");
                            txttextbox.Width = 120;
                            txttextbox.CssClass = "txt_Blue_Small";

                            txttextbox.EnableTheming = false;
                            if (DB.RSField(drMatrialMsp, "ParameterDataType") == "Nvarchar")
                                txttextbox.Attributes.Add("onKeypress", "return checkSpecialChar(event)");
                            else if (DB.RSField(drMatrialMsp, "ParameterDataType") == "Integer")
                                txttextbox.Attributes.Add("onKeypress", "return checkNum(event)");
                            else if (DB.RSField(drMatrialMsp, "ParameterDataType") == "Decimal")
                                txttextbox.Attributes.Add("onKeypress", "return checkDec(event)");
                            else if (DB.RSField(drMatrialMsp, "ParameterDataType") == "DateTime")
                                txttextbox.CssClass = "DateBoxCSS_small";
                            validator.ControlToValidate = txttextbox.ID;
                            validator.ControlToValidate = txttextbox.ID;
                            if (DB.RSFieldTinyInt(drMatrialMsp, "IsRequired") == 1)
                                htCell.Controls.Add(validator);
                            htCell.Controls.Add(ltliteral);
                            htCell.Controls.Add(txttextbox);
                        }
                       
                        htRow.Cells.Add(htCell);
                    }
                    else
                    {
                        //arrange right side in row

                        htCell = new HtmlTableCell();
                        htCell.Align = "left";
                        htCell.Width = "100%";
                        txttextbox = new TextBox();
                        ltliteral = new Literal();
                        ltliteral.ID = "lt" + DB.RSField(drMatrialMsp, "ParameterName");
                        ltliteral.Text = DB.RSField(drMatrialMsp, "DisplayName") + ":<br />";

                        String ControlType = DB.RSField(drMatrialMsp, "ControlType");

                        RequiredFieldValidator validator = new RequiredFieldValidator();
                        validator.ValidationGroup = "splitQuantity";
                        validator.ID = "rfv" + DB.RSField(drMatrialMsp, "ParameterName");
                        validator.Display = ValidatorDisplay.Dynamic;
                        validator.ErrorMessage = "*";
                        if (ControlType == "DropDownList")
                        {
                            ddlDropDownList = new DropDownList();
                            ddlDropDownList.ID = "ddl" + DB.RSField(drMatrialMsp, "ParameterName");
                            ddlDropDownList.Width = 120;
                            BuildDynamicDropDown(ddlDropDownList, DB.RSField(drMatrialMsp, "DataSource"), DB.RSField(drMatrialMsp, "ParameterName"));
                            validator.ControlToValidate = ddlDropDownList.ID;
                            if (DB.RSFieldTinyInt(drMatrialMsp, "IsRequired") == 1)
                                htCell.Controls.Add(validator);
                            htCell.Controls.Add(ltliteral);

                            htCell.Controls.Add(ddlDropDownList);
                        }
                        else if (ControlType == "TextBox")
                        {
                            txttextbox = new TextBox();
                            txttextbox.ID = "txt" + DB.RSField(drMatrialMsp, "ParameterName");
                            txttextbox.Width = 120;
                            txttextbox.EnableTheming = false;
                            txttextbox.CssClass="txt_Blue_Small";
                            if (DB.RSField(drMatrialMsp, "ParameterDataType") == "Nvarchar")
                                txttextbox.Attributes.Add("onKeypress", "return checkSpecialChar(event)");
                            else if (DB.RSField(drMatrialMsp, "ParameterDataType") == "Integer")
                                txttextbox.Attributes.Add("onKeypress", "return checkNum(event)");
                            else if (DB.RSField(drMatrialMsp, "ParameterDataType") == "Decimal")
                                txttextbox.Attributes.Add("onKeypress", "return checkDec(event)");
                            else if (DB.RSField(drMatrialMsp, "ParameterDataType") == "DateTime")
                                txttextbox.CssClass = "DateBoxCSS_small";
                            validator.ControlToValidate = txttextbox.ID;
                            if (DB.RSFieldTinyInt(drMatrialMsp, "IsRequired") == 1)
                                htCell.Controls.Add(validator);
                            htCell.Controls.Add(ltliteral);
                            htCell.Controls.Add(txttextbox);
                        }
                       
                        htRow.Cells.Add(htCell);
                        tbsplitmaterial.Rows.Add(htRow);
                        htRow = new HtmlTableRow();
                        htCell = new HtmlTableCell();
                        htCell.Controls.Add(new LiteralControl("&nbsp;"));
                        htRow.Cells.Add(htCell);
                        tbsplitmaterial.Rows.Add(htRow);
                    }


                }
                if (mspCount % 2 != 0)
                {
                    tbsplitmaterial.Rows.Add(htRow);
                }
                drMatrialMsp.Close();
            }
            catch (Exception ex)
            {
                resetError("Error while Load controls",true);
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
            }
        }

        private void BuildDynamicDropDown(DropDownList dropdown, String sql, String HeaderName)
        {
            dropdown.Items.Clear();

            dropdown.Items.Add(new ListItem());
            try
            {
                dropdown.Items.Clear();
                IDataReader dropdownReader = DB.GetRS(sql);
                dropdown.Items.Add(new ListItem("Select " + HeaderName, ""));

                while (dropdownReader.Read())
                {
                    dropdown.Items.Add(new ListItem(dropdownReader[0].ToString(), dropdownReader[1].ToString()));
                }
                dropdownReader.Close();
            }
            catch (Exception ex)
            {
                resetError("Error whils loading",true);
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
            }
        }

        public String Getimage(String IsChecked)
        {
            if (IsChecked != "")
            {
                if (Convert.ToBoolean(Convert.ToInt32(IsChecked)))
                    return "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAADAAAAAwCAYAAABXAvmHAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAAMySURBVGhD7Zm9axRBGMYPvwo7vyr1rxCDVukkuBdnDu5Qk0ZBrW3UYHNdzkJQTCDG/AUx7Kwx0SpgfWBjVKz9akxirYKc77N5D8LeO3ezu7Nrsw/8YHO7877PfOzM7KRWqVKl/Go9b+2fDBvngkjdr0cqDIz+WDf6J13/iaFr+u1DfI+ewbPtdnsfF/9/UpE6TeYekLnv9Uj3UmH0N5SdCBunOFx5qr+sH6fWfEat+Vs0lwLEIBYQk8MXKzI+RexIZvJAvbiF2JzGv2icH6IkS1Jyn1COp2cWbx3ktH5E3XuYuvm1lLAQjH6FnJw+n+KWL9N8H6NXvfREGcPGBoYT28imIGxMS4HLRV1mO+k0sdw8QUNnWw5aHpidtNHH2Ja70H1SwKK4tjEl/g6oIefZlpuwwlKh3IuUK3e7N3rdv496Dz/dFu/DS7AWnGR7o4UlXgpUBH3zb3uPY4ZUosP2hgubrHifIgTxTdI8wN/XN6YHnqUh/cVpA0jmzycLF4HNPH6XngdUiTG2aRd1FW2J5QC+yGI+xugZtmkXPWTEwp7IbB4YvcI27aJuog8PobAHcpmPUe/Zpl1US/qSkgoPMmzuTpLffDwTbbNNu1znf0x3rgZ8mAfwxjbtcqkAzLsa8WUeOFVg1BDCHO1q6E73pjfzwGkI4UWRCu/FpVV9m48xepNt2kUVCMXCCWyVgPFCzAOnaTTFQmarRCHmiYuRusc27cKhk1TYhlSJvfgyHxM2zrJNu+LNXKS+igEs2Crh07zzZg6iYdSRggwjWQlc412Qns2GmmV7o4XjPpf1IEn/5fVu3uhfqT5oIKrAghhsBP1ZSLqXHTXHttyFD2ksHHLAMlE/Liw3j7KtdJo0+qoctDwC02ixnWyqG7UoBS4Fo5+wjezC0SIFWhUTFAnlxD9O2EY+4aCVxuK6mKgA6N1b83a42xcOWss57FJz42/GD3Ba/wrCxhWqyJacPDuIGby41OQ0xYqn2Pksi90AtEih1YP14AiHL0+8Yneo9T6L5oawW0bNpl5hixA2WWRojFpzhlghNsncDnpot5fo2uh3uIctMXaVzhuzSpUqDVGt9g9nUxYfKM5I0wAAAABJRU5ErkJggg==";
                else
                    return null;
            }
            else
            {
                return null;
            }
        }

        protected void gvActiveReport_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.DataItem == null)
                return;

            if ((!((e.Row.RowState & DataControlRowState.Edit) == DataControlRowState.Edit)) && CommonLogic.QueryString("TransactionType") == "1")
            {
                e.Row.FindControl("QtyinBUoM").Visible = false;
                DataRow row = ((DataRowView)e.Row.DataItem).Row;
                String id= row["GoodsMovementDetailsID"].ToString();

                String IsDeleted = row["IsDeleted"].ToString();

                ((Literal)e.Row.FindControl("IsDeleted")).Text = CheckCanceled(IsDeleted);

                if (cp.TenantID == 0)
                {
                    //LinkButton lnkSplit = (LinkButton)e.Row.FindControl("lnkSplit");
                    //lnkSplit.CommandArgument = row["GoodsMovementDetailsID"].ToString();
                    //lnkSplit.CommandName = "Split";
                    //lnkSplit.OnClientClick = " openDialog('" + id + "');";
                }
                
            }
            else if ((!((e.Row.RowState & DataControlRowState.Edit) == DataControlRowState.Edit)) && CommonLogic.QueryString("TransactionType") == "2")
            {
                e.Row.FindControl("lnkQtyinBUoM").Visible = false;
                DataRow row = ((DataRowView)e.Row.DataItem).Row;
                String IsDeleted = row["IsDeleted"].ToString();

                ((Literal)e.Row.FindControl("IsDeleted")).Text = CheckCanceled(IsDeleted);
               
            }

        }

        private String CheckCanceled(String checkValue)
        {
            if (checkValue.Trim() == "1")
                return "Canceled";
            else
                return "<img width='20' src=\"data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAADAAAAAwCAYAAABXAvmHAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAAMySURBVGhD7Zm9axRBGMYPvwo7vyr1rxCDVukkuBdnDu5Qk0ZBrW3UYHNdzkJQTCDG/AUx7Kwx0SpgfWBjVKz9akxirYKc77N5D8LeO3ezu7Nrsw/8YHO7877PfOzM7KRWqVKl/Go9b+2fDBvngkjdr0cqDIz+WDf6J13/iaFr+u1DfI+ewbPtdnsfF/9/UpE6TeYekLnv9Uj3UmH0N5SdCBunOFx5qr+sH6fWfEat+Vs0lwLEIBYQk8MXKzI+RexIZvJAvbiF2JzGv2icH6IkS1Jyn1COp2cWbx3ktH5E3XuYuvm1lLAQjH6FnJw+n+KWL9N8H6NXvfREGcPGBoYT28imIGxMS4HLRV1mO+k0sdw8QUNnWw5aHpidtNHH2Ja70H1SwKK4tjEl/g6oIefZlpuwwlKh3IuUK3e7N3rdv496Dz/dFu/DS7AWnGR7o4UlXgpUBH3zb3uPY4ZUosP2hgubrHifIgTxTdI8wN/XN6YHnqUh/cVpA0jmzycLF4HNPH6XngdUiTG2aRd1FW2J5QC+yGI+xugZtmkXPWTEwp7IbB4YvcI27aJuog8PobAHcpmPUe/Zpl1US/qSkgoPMmzuTpLffDwTbbNNu1znf0x3rgZ8mAfwxjbtcqkAzLsa8WUeOFVg1BDCHO1q6E73pjfzwGkI4UWRCu/FpVV9m48xepNt2kUVCMXCCWyVgPFCzAOnaTTFQmarRCHmiYuRusc27cKhk1TYhlSJvfgyHxM2zrJNu+LNXKS+igEs2Crh07zzZg6iYdSRggwjWQlc412Qns2GmmV7o4XjPpf1IEn/5fVu3uhfqT5oIKrAghhsBP1ZSLqXHTXHttyFD2ksHHLAMlE/Liw3j7KtdJo0+qoctDwC02ixnWyqG7UoBS4Fo5+wjezC0SIFWhUTFAnlxD9O2EY+4aCVxuK6mKgA6N1b83a42xcOWss57FJz42/GD3Ba/wrCxhWqyJacPDuIGby41OQ0xYqn2Pksi90AtEih1YP14AiHL0+8Yneo9T6L5oawW0bNpl5hixA2WWRojFpzhlghNsncDnpot5fo2uh3uIctMXaVzhuzSpUqDVGt9g9nUxYfKM5I0wAAAABJRU5ErkJggg==\" />";
        }

        protected void gvActiveReport_RowCommand(object sender, GridViewCommandEventArgs e)
        {
          
            String  id= e.CommandName.ToString();
            if (e.CommandName.ToString() == "Split")
            {
                String GoodsMovementDetailsID = e.CommandArgument.ToString();
                lblStatus_split.Text = ""; 
                BuildDialogBox(GoodsMovementDetailsID);
               

            }
            else if (e.CommandName.ToString() == "CustomerDetails")
            {
                String GoodsMovementDetailsID = e.CommandArgument.ToString();
                ViewState["GoodsMovementDetailsID"] = GoodsMovementDetailsID;
                Build_CustomerDetailsGrid(Build_CustomerDetailsGrid());
            }
        }

        private DataSet Build_CustomerDetailsGrid()
        {
            DataSet dsCustomerDetails = null;
            try
            {
                dsCustomerDetails = DB.GetDS("sp_INV_GetPickedDetailsFromOutBound @GoodsMovementDetailsID=" + ViewState["GoodsMovementDetailsID"].ToString(), false);

            }
            catch (Exception ex)
            {
                resetError("Error while load Customer Details",true);
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
            }
            return dsCustomerDetails;
        }

        private void Build_CustomerDetailsGrid(DataSet dsCustomerDetails)
        {
            gvCustomerDetails.DataSource = dsCustomerDetails;
            gvCustomerDetails.DataBind();
            dsCustomerDetails.Dispose();

            ScriptManager.RegisterStartupScript(this, this.GetType(), "GRNDetails", "unblockCusDialog();", true);

        }

        protected void resetError(string error, bool isError)
        {

           /* string str = "<font class=\"noticeMsg\">NOTICE:</font>&nbsp;&nbsp;&nbsp;";
            if (isError)
                str = "<font class=\"errorMsg\">ERROR:</font>&nbsp;&nbsp;&nbsp;";

            if (error.Length > 0)
                str += error + "";
            else
                str = "";


            lbStatus.Text = str;*/
            ScriptManager.RegisterStartupScript(this, this.GetType(), "test", "closeAsynchronus(); showStickyToast(" + (isError.ToString().ToLower() == "true" ? "false" : "true") + ",\"" + error + "\");", true);

        }
                
        protected void resetSplitWindowError(string error, bool isError)
        {

            /*string str = "<font class=\"noticeMsg\">NOTICE:</font>&nbsp;&nbsp;&nbsp;";
            if (isError)
                str = "<font class=\"errorMsg\">ERROR:</font>&nbsp;&nbsp;&nbsp;";

            if (error.Length > 0)
                str += error + "";
            else
                str = "";


            lblStatus_split.Text = str;*/
            ScriptManager.RegisterStartupScript(this, this.GetType(), "test", "closeAsynchronus(); showStickyToast(" + (isError.ToString().ToLower() == "true" ? "false" : "true") + ",\"" + error + "\");", true);

        }

        private void BuildDialogBox(String GoodsMovementDetailsID)
        {
            IDataReader rsGoodsMovementDetails = DB.GetRS("[dbo].[sp_INV_GetActivityReportForMaterial]	 @MaterialMasterID =" + CommonLogic.QueryString("mmid") + ",@GoodsMovementTypeID=" + CommonLogic.QueryString("TransactionType") + ",@GoodsMovementDetailsID=" + GoodsMovementDetailsID + ",@TenantID=" + CommonLogic.QueryString("tid"));
            if (rsGoodsMovementDetails.Read())
            {
                //  ltinvuomqty.Text = DB.RSFieldDecimal(rsGoodsMovementDetails, "TUoMQty").ToString();
                hifGoodsMovementDetailsID.Value = DB.RSFieldInt(rsGoodsMovementDetails, "GoodsMovementDetailsID").ToString();
                txtcarton.Text = DB.RSField(rsGoodsMovementDetails, "CartonCode").ToString();
                txtLocation.Text = DB.RSField(rsGoodsMovementDetails, "Location").ToString();
                hiffromcartonID.Value = (DB.RSFieldInt(rsGoodsMovementDetails, "CartonID").ToString());
                hdncarton.Value = (DB.RSFieldInt(rsGoodsMovementDetails, "CartonID").ToString());
                ltBuomqty.Text = DB.RSField(rsGoodsMovementDetails, "BUoM") + "/" + DB.RSFieldDecimal(rsGoodsMovementDetails, "BUoMQty");
                ltMuomqty.Text = DB.RSField(rsGoodsMovementDetails, "MUoM") + "/" + DB.RSFieldDecimal(rsGoodsMovementDetails, "MUoMQty");
                hifMuomQty.Value = DB.RSFieldDecimal(rsGoodsMovementDetails, "MUoMQty").ToString();
                hifBUoMQty.Value = DB.RSFieldDecimal(rsGoodsMovementDetails, "BUoMQty").ToString();
                hifConversion.Value = DB.RSFieldDecimal(rsGoodsMovementDetails, "CF").ToString();
                txtSplitQty.Text = DB.RSFieldDecimal(rsGoodsMovementDetails, "availableDocQty").ToString();
                hifconversionInMUoM.Value = DB.RSFieldDecimal(rsGoodsMovementDetails, "CFInMUoM").ToString();
                if (DB.RSFieldInt(rsGoodsMovementDetails, "MeasurementTypeID") != 0)
                {
                    hifMeasurementType.Value = "1";
                    //String cmdGetConversionValues = "select	convert(decimal(18,6),ISNULL(MTD.ConvesionValue,1)*IIF(ISNULL(IMPM.PowerOf,0)-ISNULL(MPM.PowerOf,0)<0,1/POWER(convert(float,10),ISNULL(MPM.PowerOf,0)-ISNULL(IMPM.PowerOf,0)),POWER(convert(float,10),ISNULL(IMPM.PowerOf,0)-ISNULL(MPM.PowerOf,0)))) TotalConversion,"

                    //                                   + " convert(decimal(18,6),ISNULL(MMTD.ConvesionValue,1)*IIF(ISNULL(IMPM.PowerOf,0)-ISNULL(MMPM.PowerOf,0)<0,1/POWER(convert(float,10),ISNULL(MMPM.PowerOf,0)-ISNULL(IMPM.PowerOf,0)),POWER(convert(float,10),ISNULL(IMPM.PowerOf,0)-ISNULL(MMPM.PowerOf,0)))) MinPickConversion "
                    //                                   + " from GEN_UoM BUOM "
                    //                                   + " join MES_MeasurementMaster MTM on MTM.MeasurementID=BUOM.MeasurementID"
                    //                                   + " LEFT JOIN MES_MatricPrefixMaster MPM ON MTM.ConversionTypeID=1 and MPM.MetricPrifixID=BUOM.MatricPrifixID"

                    //                                   + " JOIN GEN_UoM IUOM ON IUOM.UoMID=" + DB.RSFieldInt(rsGoodsMovementDetails, "TUoMID")
                    //                                   + " left join MES_MatricPrefixMaster IMPM ON IMPM.MetricPrifixID=IUOM.MatricPrifixID "
                    //                                   + " LEFT join MES_MeasurementDetails MTD on MTD.MeasurementID=IUOM.MeasurementID  AND MTD.ToMesurementID=MTM.MeasurementID "

                    //                                   + " JOIN GEN_UoM MUOM ON MUOM.UoMID=" + DB.RSFieldInt(rsGoodsMovementDetails, "MUoMID")
                    //                                   + " left join MES_MatricPrefixMaster MMPM ON MMPM.MetricPrifixID=MUOM.MatricPrifixID "
                    //                                   + " LEFT join MES_MeasurementDetails MMTD on MMTD.MeasurementID=IUOM.MeasurementID  AND MMTD.ToMesurementID=MUOM.MeasurementID "

                    //                                   + " WHERE BUOM.UoMID=" + DB.RSFieldInt(rsGoodsMovementDetails, "BUoMID");

                    //IDataReader rsGetConversionValues = DB.GetRS(cmdGetConversionValues);
                    //if (rsGetConversionValues.Read())
                    //{
                    //    hifconversionInMUoM.Value = (DB.RSFieldDecimal(rsGetConversionValues, "MinPickConversion") * DB.RSFieldDecimal(rsGoodsMovementDetails, "TUoMQty") / DB.RSFieldDecimal(rsGoodsMovementDetails, "MUoMQty")).ToString();
                    //}
                    //rsGetConversionValues.Close();





                }



                lbkitid.Text = DB.RSFieldInt(rsGoodsMovementDetails, "KitPlannerID").ToString();
                if (DB.RSFieldInt(rsGoodsMovementDetails, "KitPlannerID") == 0)
                {
                    ltKitID.Visible = false;
                    lbkitid.Visible = false;
                }
                else
                {
                    ltKitID.Visible = true;
                    lbkitid.Visible = true;
                }
                ltInvUoMQty.Text = DB.RSField(rsGoodsMovementDetails, "TUoM").ToString() + "/" + DB.RSFieldDecimal(rsGoodsMovementDetails, "TUoMQty").ToString();
                hifTotalQty.Value = DB.RSFieldDecimal(rsGoodsMovementDetails, "availableDocQty").ToString();
                IDataReader rsConfigureMSP = DB.GetRS("[sp_ORD_GetMaterialStorageParameters] @MaterialMasterID=" + CommonLogic.QueryString("mmid") + ",@TenantID=" + cp.TenantID);
                while (rsConfigureMSP.Read())
                {
                    String ControlType = DB.RSField(rsConfigureMSP, "ControlType");
                    String ControlID = "";

                    if (ControlType == "DropDownList")
                    {
                        ControlID = "ddl" + DB.RSField(rsConfigureMSP, "ParameterName");
                        DropDownList dropdown = ((DropDownList)tbsplitmaterial.FindControl(ControlID));//.SelectedValue = DB.RSField(rsGoodsMovementDetails, DB.RSField(rsConfigureMSP, "ParameterName"));

                        dropdown.SelectedIndex = dropdown.Items.IndexOf(dropdown.Items.FindByText(DB.RSField(rsGoodsMovementDetails, DB.RSField(rsConfigureMSP, "ParameterName"))));
                    }
                    else
                    {
                        ControlID = "txt" + DB.RSField(rsConfigureMSP, "ParameterName");
                        ((TextBox)tbsplitmaterial.FindControl(ControlID)).Text = DB.RSField(rsGoodsMovementDetails, DB.RSField(rsConfigureMSP, "ParameterName"));
                    }


                }
                rsConfigureMSP.Close();

                chkDisc.Checked = Convert.ToBoolean(DB.RSFieldTinyInt(rsGoodsMovementDetails, "HasDiscrepancy"));
                chkIsDamag.Checked = Convert.ToBoolean(DB.RSFieldTinyInt(rsGoodsMovementDetails, "IsDamaged"));
                chkisNonconformity.Checked = Convert.ToBoolean(DB.RSFieldTinyInt(rsGoodsMovementDetails, "IsNonConfirmity"));
                //chkAsIs.Checked = Convert.ToBoolean(DB.RSFieldTinyInt(rsGoodsMovementDetails, "AsIs"));
            }
            rsGoodsMovementDetails.Close();

               
        }

        protected void gvCustomerDetails_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvCustomerDetails.PageIndex = e.NewPageIndex;
            Build_CustomerDetailsGrid(Build_CustomerDetailsGrid());
        }
        
    }
}