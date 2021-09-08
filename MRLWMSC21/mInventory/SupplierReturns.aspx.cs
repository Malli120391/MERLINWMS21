using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using MRLWMSC21Common;

namespace MRLWMSC21.mInventory
{
    //Author    :   Gvd Prasad
    //Created On:   25-Feb-2014
    //Use case ID : Supplier Returns_UC_021

    public partial class SupplierReturns : System.Web.UI.Page
    {
        public CustomPrincipal cp = HttpContext.Current.User as CustomPrincipal;

        protected void page_Init(object sender, EventArgs e)
        {
            AddDynamicColumns();
        }

        protected void Page_PreInit(object sender, EventArgs e)
        {
            Page.Theme = "Inventory";
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //txtTenant.Enabled = CommonLogic.CheckSuperAdmin(txtTenant, cp, hifTenant);
            if (!cp.IsInAnyRoles(CommonLogic.GetRolesAllowed(CommonLogic.GetRolesAllowedForthisPage("Supplier Returns"))))
            {
                //test case ID (TC_008)
                //Only authorized user can access supplier returns page

                Response.Redirect("Login.aspx?eid=6");
            }
            if (!IsPostBack)
            {
                DesignLogic.SetInnerPageSubHeading(this.Page, "Supplier Returns");
                ViewState["SupplierReturns"] = "[dbo].[sp_INV_GetSupplierReturnMaterial] @POHeaderID=" + hifPONumber.Value + ",@SupplierInvoiceID=" + hifSupplierNumber.Value + ",@WarehouseID=" + hifStore.Value;
                Build_gvSupplierReturns(Build_gvSupplierReturns());
            }
        }

        protected void lnkGetDetails_Click(object sender, EventArgs e)
        {
            //test case ID (TC_014)
            //show all Material received details

            if (atcPONumber.Text.Trim().Length > 0 && hifPONumber.Value.Trim().Length > 0 && atcSupplierNumber.Text.Trim().Length > 0 && hifSupplierNumber.Value.Trim().Length > 0 && atcStore.Text.Trim().Length > 0 && hifStore.Value.Trim().Length > 0)
            {
                ViewState["SupplierReturns"] = "[dbo].[sp_INV_GetSupplierReturnMaterial] @POHeaderID=" + hifPONumber.Value + ",@SupplierInvoiceID=" + hifSupplierNumber.Value + ",@WarehouseID=" + hifStore.Value;
                Build_gvSupplierReturns(Build_gvSupplierReturns());
            }
            else
            {
                resetError("PO Number, Supplier Invoice and Store cannot be empty",true,false);
            }
        }

        public String Getimage(String IsChecked)
        {
            if (IsChecked != "")
            {
                if (Convert.ToBoolean(Convert.ToInt32(IsChecked)))
                    return "../Images/blue_menu_icons/check_mark.png";
                else
                    return null;
            }
            else
            {
                return null;
            }
        }

        private DataSet Build_gvSupplierReturns()
        {
            DataSet dsSupplierReturns = null;
            String sCmdSupplierReturns = ViewState["SupplierReturns"].ToString();
            try
            {
                dsSupplierReturns = DB.GetDS(sCmdSupplierReturns, false);
            }
            catch (Exception ex)
            {
                resetError("Error while loading",true,false);
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
            }
            return dsSupplierReturns;
        }

        private void Build_gvSupplierReturns(DataSet dsSupplierReturns)
        {
            gvSupplierReturns.DataSource = dsSupplierReturns;
            gvSupplierReturns.DataBind();
            gvSupplierReturns.ShowHeaderWhenEmpty = true;
            gvSupplierReturns.Dispose();
        }

        private void AddDynamicColumns()
        {
            TemplateField field;
            try
            {
                IDataReader rsMSPList = DB.GetRS("sp_GEN_GetAllMaterialStorageParameters @ParameterUsageTypeID=1,@TenantID=" + cp.TenantID);

                while (rsMSPList.Read())
                {
                    field = new TemplateField();

                    field.HeaderTemplate = new DynamicallyTemplatedGridViewHandler(ListItemType.Header, DB.RSField(rsMSPList, "DisplayName"), "Literal");

                    switch (DB.RSField(rsMSPList, "ControlType"))
                    {
                        case "DropDownList":
                            {
                                field.ItemTemplate = new DynamicallyTemplatedGridViewHandler(ListItemType.Item, DB.RSField(rsMSPList, "ParameterName"), "Literal");

                                field.ItemTemplate = new DynamicallyTemplatedGridViewHandler(ListItemType.Item, DB.RSField(rsMSPList, "ParameterName"), "LiteralWithID");
                                break;
                            }
                        default:
                            {
                                field.ItemTemplate = new DynamicallyTemplatedGridViewHandler(ListItemType.Item, DB.RSField(rsMSPList, "ParameterName"), "Literal");
                                break;
                            }
                    }

                    field.HeaderStyle.Width = 150;

                    field.ItemStyle.Width = 150;

                    gvSupplierReturns.Columns.Add(field);
                }

                rsMSPList.Close();
            }
            catch (Exception ex)
            {
                resetError("Error while loading",true,false);
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
            }
            field = new TemplateField();
            field.HeaderText = "Remarks";
            field.ItemTemplate = new DynamicallyTemplatedGridViewHandler(ListItemType.Item, "Remarks", "Literal");

            field = new TemplateField();
            field.HeaderTemplate = new DynamicallyTemplatedGridViewHandler(ListItemType.Header, "Select", "Literal");
            field.ItemTemplate = new DynamicallyTemplatedGridViewHandler(ListItemType.Item, "Select", "CheckBox");
            gvSupplierReturns.Columns.Add(field);

            field = new TemplateField();
            field.HeaderTemplate = new DynamicallyTemplatedGridViewHandler(ListItemType.Header, "Return Qty.", "Literal");
            field.ItemTemplate = new DynamicallyTemplatedGridViewHandler(ListItemType.Item, "RemainingQty", "TextBox");

            gvSupplierReturns.Columns.Add(field);

        }

        protected void lnkSupplierReturns_Click(object sender, EventArgs e)
        {

            //test case ID (TC_018)
            //create dummy outbound for return material

            bool isEmpty = true;

            StringBuilder MaterialList = new StringBuilder();
            StringBuilder MaterialUomList = new StringBuilder();
            StringBuilder QuantityList = new StringBuilder();
            StringBuilder kitPlannerIDs = new StringBuilder();
            StringBuilder LineNumbers = new StringBuilder();
            StringBuilder IsDamageds = new StringBuilder();
            StringBuilder HasDiscrepancys = new StringBuilder();
            StringBuilder IsNonConfirmitys = new StringBuilder();
            StringBuilder IsKitParent = new StringBuilder();
            StringBuilder AsIs = new StringBuilder();
            Decimal AvailableQty;
            Decimal ReturnQty;
            StringBuilder MaterialStorageParameterIDs = new StringBuilder();
            StringBuilder MaterialStorageParameterValues = new StringBuilder();
            String SONumber = GetSONumber();
            if (SONumber == "")
            {
                resetError("SO Number not created,cannot return", true,false);
                return;
            }
            String OBDNumber = GetOBDNumber();
            if (OBDNumber == "")
            {
                resetError("OBD Number not created,cannot return", true,false);
                return;
            }

            String CustomerPONo = GetCustomerPONo();
            if (CustomerPONo == "")
            {
                resetError("CustomerPONo not created,cannot return",true,false);
                return;
            }

            Literal ltLineNumber;
            Literal ltMaterialID;
            Literal ltMaterialUomID;
            TextBox txtMaterialQty;
            Literal ltKitID;
            Literal ltIsDamaged;
            Literal ltIsNonConfirmity;
            Literal ltHasDiscrepancy;
            Literal ltAsIs;
            Literal ltIsKitParent;
            Literal ltMaterialStorageParameterValue;
            DataTable dtMaterialStorageParameter = null;
            try
            {
                dtMaterialStorageParameter = DB.GetDS("sp_GEN_GetAllMaterialStorageParameters @ParameterUsageTypeID=1,@TenantID=" + cp.TenantID, false).Tables[0];
            }
            catch (Exception ex)
            {
                resetError("Error while loading",true,false);
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
                return;
            }

            foreach (GridViewRow row in gvSupplierReturns.Rows)
            {
                txtMaterialQty = (TextBox)row.FindControl("RemainingQty");
                if (((CheckBox)row.FindControl("Select")).Checked && txtMaterialQty.Text != "" && Convert.ToDecimal(txtMaterialQty.Text) != 0)
                {
                    isEmpty = false;
                    ltMaterialID = (Literal)row.FindControl("ltMaterialMasterID");
                    ltMaterialUomID = (Literal)row.FindControl("ltMaterial_iuomid");
                    ltLineNumber = (Literal)row.FindControl("ltLineNumber");
                    ltIsKitParent = (Literal)row.FindControl("ltIsKitParent");
                    ltIsDamaged = (Literal)row.FindControl("ltIsDamaged");
                    ltAsIs = (Literal)row.FindControl("ltAsIs");
                    ltIsNonConfirmity = (Literal)row.FindControl("ltIsNonConfirmity");
                    ltHasDiscrepancy = (Literal)row.FindControl("ltHasDiscrepancy");
                    
                    // ltMaterialUomID = (Literal)row.FindControl("ltKitPlannerID");
                    ltKitID = (Literal)row.FindControl("ltKitPlannerID");
                    AvailableQty = Convert.ToDecimal(((Literal)row.FindControl("ltRemainingQty")).Text);
                    ReturnQty = Convert.ToDecimal(txtMaterialQty.Text);
                    if (AvailableQty < ReturnQty)
                    {
                        //test case ID (TC_020)
                        //Entered qty. should be less than or equal to available qty.

                        resetError("Return quantity is more than available quantity at [" + ((Literal)row.FindControl("ltMCode")).Text + "]", true, false);
                        return;
                    }
                    if (ReturnQty == 0)
                    {
                        //test case ID (TC_019)
                        //Qty. must be greater than zero

                        resetError("Return Quantity is zero ,cannot return", true,false);
                        return;
                    }
                    LineNumbers.Append(ltLineNumber.Text+",");
                    MaterialList.Append(ltMaterialID.Text + ",");
                    MaterialUomList.Append(ltMaterialUomID.Text + ",");
                    QuantityList.Append(txtMaterialQty.Text + ",");
                    kitPlannerIDs.Append(ltKitID.Text + ",");
                    IsKitParent.Append(ltIsKitParent.Text+",");
                    IsDamageds.Append(ltIsDamaged.Text + ",");
                    AsIs.Append(ltAsIs.Text+',');
                    HasDiscrepancys.Append(ltHasDiscrepancy.Text+",");
                    IsNonConfirmitys.Append(ltIsNonConfirmity.Text+",");

                    foreach (DataRow dataRow in dtMaterialStorageParameter.Rows)
                    {
                        ltMaterialStorageParameterValue = (Literal)row.FindControl(dataRow["ParameterName"].ToString());
                        if (ltMaterialStorageParameterValue.Text != "")
                        {
                            if (dataRow["ControlType"].ToString() == "DropDownList")
                            {
                                String ControlID;
                                if (dataRow["ParameterName"].ToString() == "Plant")
                                    ControlID = "M" + dataRow["ParameterName"].ToString() + "ID";
                                else
                                    ControlID = dataRow["ParameterName"].ToString() + "ID";
                                MaterialStorageParameterIDs.Append(dataRow["MaterialStorageParameterID"].ToString() + "|");
                                ltMaterialStorageParameterValue = (Literal)row.FindControl(ControlID);
                                MaterialStorageParameterValues.Append(ltMaterialStorageParameterValue.Text + "|");
                            }
                            else
                            {
                                MaterialStorageParameterIDs.Append(dataRow["MaterialStorageParameterID"].ToString() + "|");
                                MaterialStorageParameterValues.Append(ltMaterialStorageParameterValue.Text + "|");
                            }
                        }

                    }
                    MaterialStorageParameterIDs.Append(",");
                    MaterialStorageParameterValues.Append(",");

                }
            }
            if (isEmpty)
            {
                resetError("No material is selected", true, false);
                return;
            }
            try
            {
                StringBuilder cmdSupplierReturn = new StringBuilder();
                cmdSupplierReturn.Append("DECLARE @NewGeneratedOutboundID int EXEC [dbo].[sp_INV_DummyOutboundForSupplierReturns] ");
                cmdSupplierReturn.Append("@OBDNumber=" + DB.SQuote(OBDNumber));
                cmdSupplierReturn.Append(",@SONumber=" + DB.SQuote(SONumber));
                cmdSupplierReturn.Append(",@CustomerPONo=" + DB.SQuote(CustomerPONo));
                cmdSupplierReturn.Append(",@CreatedBy=" + cp.UserID);
                cmdSupplierReturn.Append(",@IsDamageds="+DB.SQuote(IsDamageds.ToString()));
                cmdSupplierReturn.Append(",@HasDiscrepancys="+DB.SQuote(HasDiscrepancys.ToString()));
                cmdSupplierReturn.Append(",@IsNonConfirmitys="+DB.SQuote(IsNonConfirmitys.ToString()));
                cmdSupplierReturn.Append(",@IsKitParents="+DB.SQuote(IsKitParent.ToString()));
                cmdSupplierReturn.Append(",@AsIs_s="+DB.SQuote(AsIs.ToString()));
                cmdSupplierReturn.Append(",@LineNumbers="+DB.SQuote(LineNumbers.ToString()));
                cmdSupplierReturn.Append(",@SupplierInvoiceID=" + hifSupplierNumber.Value);
                cmdSupplierReturn.Append(",@Referred_Warehouse=" + hifStore.Value);
                cmdSupplierReturn.Append(",@KitIds=" + DB.SQuote(kitPlannerIDs.ToString()));
                cmdSupplierReturn.Append(",@MaterialMasterIDs=" + DB.SQuote(MaterialList.ToString()));
                cmdSupplierReturn.Append(",@Material_UoMIDs=" + DB.SQuote(MaterialUomList.ToString()));
                cmdSupplierReturn.Append(",@Qtys=" + DB.SQuote(QuantityList.ToString()));
                cmdSupplierReturn.Append(",@TenantID=" + hifTenant.Value);
                cmdSupplierReturn.Append(",@MSPIDs=" + DB.SQuote(MaterialStorageParameterIDs.ToString()));
                cmdSupplierReturn.Append(",@MSPValues=" + DB.SQuote(MaterialStorageParameterValues.ToString()));
                cmdSupplierReturn.Append(",@NewOutboundID=@NewGeneratedOutboundID output select @NewGeneratedOutboundID as N");


                int NewGeneratedOutboundID = DB.GetSqlN(cmdSupplierReturn.ToString());
                // if (NewGeneratedOutboundID != 0)
                resetError("Successfully returned the materials with OBD number :" + OBDNumber, false, false);
                // else
                //     resetError("not updated return material", true);
                Build_gvSupplierReturns(Build_gvSupplierReturns());
            }
            catch (Exception ex)
            {
                resetError("Error while updating", true,false);
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
            }
        }

        private String GetSONumber()
        {
            String SONumber = "";
            try
            {
                String NewSONumber = DB.GetSqlS("EXEC [sp_SYS_GetSystemConfigValue]   @SysConfigKey='dummysoheaderforsupplierreturn_prefix' ,@TenantID=" + cp.TenantID); //DB.GetSqlS("EXEC [sp_SYS_GetSystemConfigValue]   @SysConfigKey='inbounddetails.aspx.cs.StoreRefNo_Length' ,@TenantID=" + cp.TenantID); //"D" + DB.GetSqlS("select SysConfigValue as S from SYS_SystemConfiguration where TenantID=" + cp.TenantID + " and SysConfigKeyID= (select SysConfigKeyID from SYS_SysConfigKey where SysConfigKey=N'salesorder.aspx.cs.SO_Prefix') ");
                NewSONumber += "" + (Convert.ToInt16(DateTime.Now.Year) % 100);           //add year code to ponumber

                int length = Convert.ToInt16(DB.GetSqlS("EXEC [sp_SYS_GetSystemConfigValue]   @SysConfigKey='salesorder.aspx.cs.SO_Length' ,@TenantID=" + cp.TenantID)); //Convert.ToInt32(DB.GetSqlS("select SysConfigValue as S from SYS_SystemConfiguration as N where TenantID=" + cp.TenantID + " and SysConfigKeyID= (select SysConfigKeyID from SYS_SysConfigKey where SysConfigKey=N'salesorder.aspx.cs.SO_Length') "));

                //String OldSONumber = DB.GetSqlS("select TOP 1 SONumber as S from ORD_SOHeader where SOTypeID =7 and TenantID=" + cp.TenantID + "  ORDER BY SONumber desc ");
                String OldSONumber = DB.GetSqlS("select TOP 1 SONumber as S from ORD_SOHeader where SONumber like '"+NewSONumber+"%' ORDER BY SONumber desc ");

                

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

        private String GetOBDNumber()
        {
            String OBDNumber = "";
            try
            {

                int length = Convert.ToInt32(DB.GetSqlS("EXEC [sp_SYS_GetSystemConfigValue]   @SysConfigKey='OutboundDetails.aspx' ,@TenantID=" + cp.TenantID));

                

                String NewOBDNumber = DB.GetSqlS("EXEC [sp_SYS_GetSystemConfigValue]   @SysConfigKey='dummyoutboundforsupplierreturn_prefix' ,@TenantID=" + cp.TenantID) + (Convert.ToInt16(DateTime.Now.Year) % 100);           //add year code to ponumber

                String OldOBDNumber = DB.GetSqlS("select top 1 OBDNumber as S from OBD_Outbound where OBDNumber like '"+NewOBDNumber+"%' order by OBDNumber desc");

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

        private String GetCustomerPONo()
        {
            Random generateNo = new Random();
            return "DCUSPO" + generateNo.Next(1000, 10000);
        }

        private void resetError(string error, bool isError,bool isPermanent)
        {

            /*string str = "<font class=\"noticeMsg\">NOTICE:</font>&nbsp;&nbsp;&nbsp;";
            if (isError)
                str = "<font class=\"errorMsg\">ERROR:</font>&nbsp;&nbsp;&nbsp;";

            if (error.Length > 0)
                str += error + "";
            else
                str = "";


            ltStatus.Text = str;*/
            ScriptManager.RegisterStartupScript(this, this.GetType(), "test", "closeAsynchronus(); showStickyToast(" + (isError.ToString().ToLower() == "true" ? "false" : "true") + ",\"" + error + "\"," + isPermanent.ToString().ToLower() + ");", true);

        }

        protected void gvSupplierReturns_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow && (e.Row.RowState == DataControlRowState.Normal || e.Row.RowState == DataControlRowState.Alternate))
            {
                ((TextBox)e.Row.FindControl("RemainingQty")).Attributes.Add("onblur", "CheckIUoMQty(this)");
            }
        }
    }
}