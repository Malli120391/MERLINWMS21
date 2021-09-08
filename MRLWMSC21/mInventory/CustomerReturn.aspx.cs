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

namespace MRLWMSC21.mInventory
{
    //Author    :   Gvd Prasad
    //CreatedOn :   12-Feb-2014
    //Use case ID:  Customer Returns_UC_022


    public partial class CustomerReturn : System.Web.UI.Page
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
            if (!cp.IsInAnyRoles(CommonLogic.GetRolesAllowed(CommonLogic.GetRolesAllowedForthisPage("Customer Returns"))))
            {
                //test case ID (TC_004)
                //Customer returns page should be displayed

                Response.Redirect("Login.aspx?eid=6");
            }
            if (!IsPostBack)
            {
                DesignLogic.SetInnerPageSubHeading(this.Page, "Customer Returns");
                ViewState["cmdCustomerReturnData"] = "[dbo].[sp_INV_GetCustomerReturnsMaterial] @OutboundID=" + (hifOBDNumber.Value != "" ? hifOBDNumber.Value : "0") + ",@WarehouseID=" + (hifStoreNo.Value != "" ? hifStoreNo.Value : "0") + ",@Outbound_CustomerPOID=" + (hifInvoice.Value != "" ? hifInvoice.Value : "0");
                Build_ReturnsDataGrid(Build_ReturnsDataGrid());
            }
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

                    gvReturnsDetails.Columns.Add(field);
                }

                rsMSPList.Close();

                field = new TemplateField();
                field.HeaderTemplate = new DynamicallyTemplatedGridViewHandler(ListItemType.Header, "Select", "Literal");
                field.ItemTemplate = new DynamicallyTemplatedGridViewHandler(ListItemType.Item, "Select", "CheckBox");
                gvReturnsDetails.Columns.Add(field);

                field = new TemplateField();
                field.HeaderTemplate = new DynamicallyTemplatedGridViewHandler(ListItemType.Header, "Return Qty.", "Literal");
                field.ItemTemplate = new DynamicallyTemplatedGridViewHandler(ListItemType.Item, "Remaining", "TextBox");

                gvReturnsDetails.Columns.Add(field);
            }
            catch (Exception ex)
            {
                resetError("Error while loading MSPs",true,false);
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
            }

        }               

        protected void lnkGetData_Click(object sender, EventArgs e)
        {
            //test case ID (TC_014)
            //show all Picked material details

            if (atcOBDNumber.Text != "" && hifOBDNumber.Value != "" && atcStoreNo.Text != "" && hifStoreNo.Value != ""&& atcInvoice.Text != "" && hifInvoice.Value != "")
            {
                //test case ID (TC_016)
                //both must be select

                ViewState["cmdCustomerReturnData"] = "[dbo].[sp_INV_GetCustomerReturnsMaterial] @OutboundID=" + (hifOBDNumber.Value != "" ? hifOBDNumber.Value : "0") + ",@WarehouseID=" + (hifStoreNo.Value != "" ? hifStoreNo.Value : "0") + ",@Outbound_CustomerPOID=" + (hifInvoice.Value != "" ? hifInvoice.Value : "0");
                Build_ReturnsDataGrid(Build_ReturnsDataGrid());
            }
            else
            {
                resetError("OBD Number, Invoice and Store cannot be empty", true, false);

            }
        }
               
        private DataSet Build_ReturnsDataGrid()
        {
            DataSet dsCustomerReturnData = null;
            String cmdCustomerReturnData= ViewState["cmdCustomerReturnData"].ToString();
            try
            {
                dsCustomerReturnData = DB.GetDS(cmdCustomerReturnData, false);
            }
            catch (Exception ex)
            {
                resetError("Error while loading",true,false);
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
            }
            return dsCustomerReturnData;
        }

        private void Build_ReturnsDataGrid(DataSet dsCustomerReturnData)
        {
            gvReturnsDetails.DataSource = dsCustomerReturnData;
            gvReturnsDetails.DataBind();
            dsCustomerReturnData.Dispose();
        }

        protected void lnkCustomerReturn_Click(object sender, EventArgs e)
        {
            //test case ID (TC_018)
            //Create Dummy inbound for receive itemes

            bool isEmpty = true;
            StringBuilder MaterialList = new StringBuilder();
            StringBuilder MaterialUomList = new StringBuilder();
            StringBuilder lineNumbers = new StringBuilder();
            StringBuilder QuantityList = new StringBuilder();
            StringBuilder kitPlannerIDs = new StringBuilder();
            StringBuilder IsKitParents = new StringBuilder();
            Decimal RemainingQty;
            Decimal ReturnQty;
            StringBuilder MaterialStorageParameterIDs = new StringBuilder();
            StringBuilder MaterialStorageParameterValues = new StringBuilder();
            String PONumber = GetDummyPONumber();
            bool isValidation;
            isValidation = KitMaterialValidation();
            if (!isValidation)
            {
                return;
            }
            if (PONumber == "")
            {
                resetError("Error is generated while creating PO Number", true,false);
                return;
            }
            String InvoiceNo = GetDummyInvoiceNo();
            String StoreRefNo = GetDummyStoreRefNo();
            if (StoreRefNo == "")
            {
                resetError("Error is generated while Creating Store Ref no.", true,false);
                return;
            }
            Literal ltMaterialID;
            Literal ltLineNumber;
            Literal ltMaterialUomID;
            TextBox txtMaterialQty;
            Literal ltKitID;
            Literal ltIsKitParent;
            Literal ltMaterialStorageParameterValue;

            DataTable dtMaterialStorageParameter = null;
            try
            {
                dtMaterialStorageParameter = DB.GetDS("sp_GEN_GetAllMaterialStorageParameters @ParameterUsageTypeID=1,@TenantID=" + cp.TenantID, false).Tables[0];
            }
            catch (Exception ex)
            {
                resetError("Error while Update",true,false);
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
                return;
            }

            foreach (GridViewRow row in gvReturnsDetails.Rows)
            {
                txtMaterialQty = (TextBox)row.FindControl("Remaining");
                if (((CheckBox)row.FindControl("Select")).Checked && txtMaterialQty.Text != "" && Convert.ToDecimal(txtMaterialQty.Text)!=0)
                {
                    isEmpty = false;
                    ltMaterialID = (Literal)row.FindControl("ltMaterialID");
                    ltMaterialUomID = (Literal)row.FindControl("ltMaterialUomID");
                    ltLineNumber = (Literal)row.FindControl("ltLineNumber");
                    ltIsKitParent = (Literal)row.FindControl("ltIsKitParent");
                    // ltMaterialUomID = (Literal)row.FindControl("ltKitPlannerID");
                    ltKitID = (Literal)row.FindControl("ltKitPlannerID");
                    RemainingQty = Convert.ToDecimal(((Literal)row.FindControl("ltRemainingQty")).Text);
                    ReturnQty = Convert.ToDecimal(txtMaterialQty.Text);
                    if (RemainingQty < ReturnQty)
                    {
                        //test case ID (TC_019)
                        //Entered qty. should be less than or equal to picked qty.

                        resetError("Return quantity is more than available quantity at [" + ((Literal)row.FindControl("ltMCode")).Text + "]", true,false);
                        return;
                    }
                    if (ReturnQty == 0)
                    {
                        //test case id (TC_020)
                        //Qty. should be greater than zero

                        resetError("Return quantity is zero ,cannot return", true,false);
                        return;
                    }
                    lineNumbers.Append(ltLineNumber.Text+",");

                    MaterialList.Append(ltMaterialID.Text + ",");
                    MaterialUomList.Append(ltMaterialUomID.Text + ",");
                    QuantityList.Append(txtMaterialQty.Text + ",");
                    kitPlannerIDs.Append(ltKitID.Text + ",");
                    IsKitParents.Append(ltIsKitParent.Text + ",");

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
                //test case ID (TC_025)
                //select atleast one Material

                resetError("No material is selected", true, false);
                return;
            }
            try
            {
                StringBuilder cmdcustomerReturn = new StringBuilder();
                cmdcustomerReturn.Append("DECLARE @NewResult int EXEC [dbo].[sp_INV_DummyInboundForCustomerReturns] ");
                cmdcustomerReturn.Append("@OutboundID=" + hifOBDNumber.Value);
                cmdcustomerReturn.Append(",@StoreRefNo=" + DB.SQuote(StoreRefNo));
                cmdcustomerReturn.Append(",@PONumber=" + DB.SQuote(PONumber));
                cmdcustomerReturn.Append(",@LineNumbers="+DB.SQuote(lineNumbers.ToString()));
                cmdcustomerReturn.Append(",@InvoiceNumber=" + DB.SQuote(InvoiceNo));
                cmdcustomerReturn.Append(",@CreatedBy=" + cp.UserID);
                cmdcustomerReturn.Append(",@IsKitParents="+DB.SQuote(IsKitParents.ToString()));
                cmdcustomerReturn.Append(",@KitIds=" + DB.SQuote(kitPlannerIDs.ToString()));
                cmdcustomerReturn.Append(",@MaterialMasterIDs=" + DB.SQuote(MaterialList.ToString()));
                cmdcustomerReturn.Append(",@Material_UoMIDs=" + DB.SQuote(MaterialUomList.ToString()));
                cmdcustomerReturn.Append(",@Qtys=" + DB.SQuote(QuantityList.ToString()));
                cmdcustomerReturn.Append(",@MSPIDs=" + DB.SQuote(MaterialStorageParameterIDs.ToString()));
                cmdcustomerReturn.Append(",@MSPValues=" + DB.SQuote(MaterialStorageParameterValues.ToString()));
                cmdcustomerReturn.Append(",@TenantID=" + hifTenant.Value);
                cmdcustomerReturn.Append(",@Result=@NewResult output select @NewResult as N");
                int Result = DB.GetSqlN(cmdcustomerReturn.ToString());
                if (Result == 1)

                    resetError("Successfully returned the materials with store ref. no. :" + StoreRefNo, false, true);
                else
                    resetError("not updated return material", true,false);
                Build_ReturnsDataGrid(Build_ReturnsDataGrid());
            }
            catch (Exception ex)
            {
                resetError("Error while updating", true,false);
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
            }
        }

        private String GetDummyStoreRefNo()
        {

            string WarehouseID = hifStoreNo.Value;
            String vNewStoreRefNUmber = null;
                try
                {
                    String StoreRefNoLength = DB.GetSqlS("EXEC [sp_SYS_GetSystemConfigValue]   @SysConfigKey='inbounddetails.aspx.cs.StoreRefNo_Length' ,@TenantID=" + cp.TenantID);  //DB.GetSqlS("select SysConfigValue as S from SYS_SystemConfiguration SYS_SC left join SYS_SysConfigKey SYS_SCK on  SYS_SCK.SysConfigKeyID=SYS_SC.SysConfigKeyID where TenantID=" + cp.TenantID + " and SYS_SCK.SysConfigKey='inbounddetails.aspx.cs.StoreRefNo_Length' and SYS_SCK.IsActive=1");

                    vNewStoreRefNUmber = DB.GetSqlS("EXEC [sp_SYS_GetSystemConfigValue]   @SysConfigKey='dummyinboundforcustomerreturn_prefix' ,@TenantID=" + cp.TenantID) + DB.GetSqlS("select WarehouseGroupCode as S from GEN_Warehouse GEN_W  left join GEN_WarehouseGroup GEN_WG on GEN_WG.WarehouseGroupID=GEN_W.WarehouseGroupID where  GEN_W.WarehouseID=" + WarehouseID);

                    String OldStoreRefNumber = DB.GetSqlS("select top 1  StoreRefNo AS S from INB_Inbound where StoreRefNo like '" + vNewStoreRefNUmber + "%'"+
                        //and tenantid="+hifTenant.Value+" ---------------commented by because of uniqueness of store ref. no.
                        "order by InboundID DESC "); // Get Previous StoreRefNo


                    vNewStoreRefNUmber += (Convert.ToInt16(DateTime.Now.Year) % 100);           //add year code to StoreRefNUmber
                    //vNewStoreRefNUmber += "" + (2014 % 100);
                    int length = Convert.ToInt32(StoreRefNoLength);                   //get prifix length
                    int power = (Int32)Math.Pow((double)10, (double)(length - 1));          //get minvalue of prifix length
                    

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
                            vNewStoreRefNUmber += "0";
                            power /= 10;
                        }
                        vNewStoreRefNUmber += "" + number;
                    }
                    else
                    {                                                                                           //other wise generate first number 
                        for (int i = 0; i < length - 1; i++)
                            vNewStoreRefNUmber += "0";
                        vNewStoreRefNUmber += "1";
                    }
                }
                catch (Exception ex)
                {
                    vNewStoreRefNUmber = "";
                    CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
                   // resetError("Error while generating StoreRef#", true);
                }

                return vNewStoreRefNUmber;
        }

        private String GetDummyPONumber()
        {
            String NewPONumber="";
            try
            {
                NewPONumber = DB.GetSqlS("EXEC [sp_SYS_GetSystemConfigValue]   @SysConfigKey='dummypoheaderforcustomerreturn_prefix' ,@TenantID=" + cp.TenantID); //"D"+DB.GetSqlS("select SysConfigValue as S from SYS_SystemConfiguration where TenantID=" +cp.TenantID + " and SysConfigKeyID= (select SysConfigKeyID from SYS_SysConfigKey where SysConfigKey=N'purchaseorder.aspx.cs.PO_Prefix') ");
                NewPONumber += "" + (Convert.ToInt16(DateTime.Now.Year) % 100);           //add year code to ponumber

                int length = Convert.ToInt16(DB.GetSqlS("EXEC [sp_SYS_GetSystemConfigValue]   @SysConfigKey='purchaseorder.aspx.cs.PO_Length' ,@TenantID=" + cp.TenantID)); //Convert.ToInt32(DB.GetSqlS("select SysConfigValue as S from SYS_SystemConfiguration as N where TenantID=" + cp.TenantID + " and SysConfigKeyID= (select SysConfigKeyID from SYS_SysConfigKey where SysConfigKey=N'purchaseorder.aspx.cs.PO_Length') "));

               
                //String OldPONumber = DB.GetSqlS("select TOP 1 PONumber as S from ORD_POHeader where POTypeID=6 and TenantID=" + cp.TenantID + "  ORDER BY PONumber desc ");
                String OldPONumber = DB.GetSqlS("select TOP 1 PONumber as S from ORD_POHeader where PONumber like '"+NewPONumber+"%' ORDER BY PONumber desc ");
                

                int power = (Int32)Math.Pow((double)10, (double)(length - 1));          //getting minvalue of prifix length

                String newvalue = "";
                if (OldPONumber != "" && NewPONumber.Equals(OldPONumber.Substring(0, NewPONumber.Length)))        //if ponumber is existed and same year ponumber  enter
                {
                    String temp = OldPONumber.Substring(NewPONumber.Length, length);                            //getting number of last prifix
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

                NewPONumber += newvalue;
               // txtPONumber.Text = NewPONumber;
            }
            catch (Exception ex)
            {
                NewPONumber = "";
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
               // resetError(ex.ToString(), true);
            }
            return NewPONumber;
        }

        private String GetDummyInvoiceNo()
        {
            Random generateNo = new Random();
            return "DINV" + generateNo.Next(1000, 10000);
        }

        private void resetError(string error, bool isError,bool isPermanent)
        {

           /* string str = "<font class=\"noticeMsg\">NOTICE:</font>&nbsp;&nbsp;&nbsp;";
            if (isError)
                str = "<font class=\"errorMsg\">ERROR:</font>&nbsp;&nbsp;&nbsp;";

            if (error.Length > 0)
                str += error + "";
            else
                str = "";


            ltStatus.Text = str;*/
            ScriptManager.RegisterStartupScript(this, this.GetType(), "test", "closeAsynchronus(); showStickyToast(" + (isError.ToString().ToLower() == "true" ? "false" : "true") + ",\"" + error + "\"," + isPermanent.ToString().ToLower() + ");", true);

        }

        private bool KitMaterialValidation()
        {

            List<ReturnMaterialData> lineItemsList;
            IDictionary<int, List<ReturnMaterialData>> ReturnDeliveryData = new Dictionary<int, List<ReturnMaterialData>>();

            Literal ltMaterialID;
            Literal ltMaterialTotalQty;
            String MaterialQty;
            Literal ltKitID;
            int LineNumber;
            CheckBox chkChecked;
            ReturnMaterialData returnMaterial;
            foreach (GridViewRow row in gvReturnsDetails.Rows)
            {
                ltMaterialID = (Literal)row.FindControl("ltMaterialID");
                ltMaterialTotalQty = (Literal)row.FindControl("ltRemainingQty");
                MaterialQty = ((TextBox)row.FindControl("Remaining")).Text;
                if (MaterialQty == "")
                    MaterialQty = "0";
                ltKitID = (Literal)row.FindControl("ltKitPlannerID");
                chkChecked = (CheckBox)row.FindControl("Select");
                LineNumber = Convert.ToInt16(((Literal)row.FindControl("ltLineNumber")).Text);
                if (ReturnDeliveryData.ContainsKey(LineNumber))
                {
                    lineItemsList = ReturnDeliveryData[LineNumber];
                    int itemsCount = lineItemsList.Count;
                    bool isAvailable = false;
                    for (int index = 0; index < itemsCount; index++)
                    {
                        returnMaterial = lineItemsList[index];
                        if (returnMaterial.MaterialID == ltMaterialID.Text)
                        {

                            returnMaterial.AddQuantity(Convert.ToDecimal(ltMaterialTotalQty.Text));
                            if (chkChecked.Checked)
                            {
                                returnMaterial.AddReturnQty(Convert.ToDecimal(MaterialQty));
                                returnMaterial.Checked = true;
                            }
                            isAvailable = true;
                            break;

                        }
                    }
                    if (!isAvailable)
                    {
                        returnMaterial = new ReturnMaterialData();
                        returnMaterial.MaterialID = ltMaterialID.Text;
                        if (ltKitID.Text != "")
                            returnMaterial.KitPlannerID = Convert.ToInt32(ltKitID.Text);
                        returnMaterial.LineNumber = LineNumber;
                        returnMaterial.AddQuantity(Convert.ToDecimal(ltMaterialTotalQty.Text));
                        if (chkChecked.Checked)
                        {
                            returnMaterial.AddReturnQty(Convert.ToDecimal(MaterialQty));
                            returnMaterial.Checked = true;
                        }
                        lineItemsList.Add(returnMaterial);
                    }

                }
                else
                {
                    returnMaterial = new ReturnMaterialData();
                    returnMaterial.MaterialID = ltMaterialID.Text;
                    if (ltKitID.Text != "")
                        returnMaterial.KitPlannerID = Convert.ToInt32(ltKitID.Text);
                    returnMaterial.LineNumber = LineNumber;
                    returnMaterial.AddQuantity(Convert.ToDecimal(ltMaterialTotalQty.Text));
                    if (chkChecked.Checked)
                    {
                        returnMaterial.AddReturnQty(Convert.ToDecimal(MaterialQty));
                        returnMaterial.Checked = true;
                    }
                    lineItemsList = new List<ReturnMaterialData>();
                    lineItemsList.Add(returnMaterial);
                    ReturnDeliveryData.Add(LineNumber, lineItemsList);
                }
            }
            return MaterialQuantityValidation(ReturnDeliveryData);

        }

        private bool MaterialQuantityValidation(IDictionary<int, List<ReturnMaterialData>> ReturnMaterialMap)
        {
            ReturnMaterialData returnMaterial;
            int MaterialCount;
            bool isChecked;
            List<ReturnMaterialData> lineMaterial;
            ICollection<int> KeyList = ReturnMaterialMap.Keys;

            for (int Keyindex = KeyList.Count - 1; Keyindex >= 0; Keyindex--)
            {
                lineMaterial = ReturnMaterialMap[KeyList.ElementAt<int>(Keyindex)];
                isChecked = false;
                foreach (ReturnMaterialData checkReturnMaterial in lineMaterial)
                {
                    if (checkReturnMaterial.Checked)
                    {
                        isChecked = true;
                    }
                }
                if (!isChecked)
                {
                    ReturnMaterialMap.Remove(KeyList.ElementAt<int>(Keyindex));


                }
            }

            if (ReturnMaterialMap.Count == 0)
            {
                resetError("No material is selected", true, false);
                return false;
            }

            Decimal Ratio = 0;
            foreach (List<ReturnMaterialData> lineMaterials in ReturnMaterialMap.Values)
            {
                MaterialCount = lineMaterials.Count;
                if (MaterialCount > 1)
                {
                    for (int index = 0; index < MaterialCount; index++)
                    {
                        returnMaterial = lineMaterials[index];
                        if (returnMaterial.ReturnQty != 0)
                            if (index == 0)
                            {
                                Ratio = returnMaterial.Quantity / returnMaterial.ReturnQty;
                            }
                            else
                            {
                                if (Ratio != returnMaterial.Quantity / returnMaterial.ReturnQty)
                                {
                                    //test case ID (TC_024)
                                    //Mismatch occurred while returning the materials for kit items

                                    resetError("Kit Material ratio mismatched at KitID [" + returnMaterial.KitPlannerID + "]", true,false);
                                    return false;
                                }
                            }
                        else
                        {
                            resetError("Must return all kit items at KitID [" + returnMaterial.KitPlannerID + "]", true,false);
                            return false;
                        }
                    }
                }

            }
            return true;
        }

        protected void gvReturnsDetails_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow && (e.Row.RowState == DataControlRowState.Normal || e.Row.RowState == DataControlRowState.Alternate))
            {
                ((TextBox)e.Row.FindControl("Remaining")).Attributes.Add("onblur", "CheckIUoMQty(this)");
            }
        }
        
    }
}