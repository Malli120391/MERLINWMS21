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

namespace MRLWMSC21.mManufacturingProcess
{
    //Author    :   Gvd Prasad
    //Created On:   23-Jul-2014
    //Use case ID:  Job Returns_UC_014

    public partial class ProductionReturns : System.Web.UI.Page
    {
        public CustomPrincipal cp = HttpContext.Current.User as CustomPrincipal;
        protected void page_Init(object sender, EventArgs e)
        {
            AddDynamicColumns();
        }

        protected void Page_PreInit(object sender, EventArgs e)
        {
            Page.Theme = "Inbound";
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!cp.IsInAnyRoles(CommonLogic.GetRolesAllowed(CommonLogic.GetRolesAllowedForthisPage("Job Returns"))))
            {
                //test case ID (TC_008)
                //Only authorized user can access job returns page

                Response.Redirect("Login.aspx?eid=6");
            }
            if (!IsPostBack)
            {
                DesignLogic.SetInnerPageSubHeading(this.Page, "JOb Returns");
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
                resetError("Error while loading",true);
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
                return;
            }

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

                gvProductionOrderReturns.Columns.Add(field);
            }

            rsMSPList.Close();

            field = new TemplateField();
            field.HeaderTemplate = new DynamicallyTemplatedGridViewHandler(ListItemType.Header, "Select", "Literal");
            field.ItemTemplate = new DynamicallyTemplatedGridViewHandler(ListItemType.Item, "Select", "CheckBox");
            gvProductionOrderReturns.Columns.Add(field);

            field = new TemplateField();
            field.HeaderTemplate = new DynamicallyTemplatedGridViewHandler(ListItemType.Header, "Return Qty.", "Literal");
            field.ItemTemplate = new DynamicallyTemplatedGridViewHandler(ListItemType.Item, "RemainingDocQty", "TextBox");

            gvProductionOrderReturns.Columns.Add(field);

        }

        private DataSet Build_gvProductionReturns()
        {
            DataSet dsGetStrockListForProduction = null;
            String cMdGetStockListForProduction = ViewState["GetStockListForProduction"].ToString();
            try
            {
                dsGetStrockListForProduction = DB.GetDS(cMdGetStockListForProduction, false);
            }
            catch (Exception ex)
            {
                resetError("Error while loading",true);
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
            }
            return dsGetStrockListForProduction;
        }

        private void Build_gvProductionReturns(DataSet dsGetStrockListForProduction)
        {
            gvProductionOrderReturns.DataSource = dsGetStrockListForProduction;
            gvProductionOrderReturns.DataBind();
            dsGetStrockListForProduction.Dispose();
        }

        protected void lnkReturn_Click(object sender, EventArgs e)
        {
            //test case ID (TC_016)
            //create dummy inbound for receive materils to store


            StringBuilder MaterialList = new StringBuilder();
            StringBuilder MaterialUomList = new StringBuilder();
            StringBuilder lineNumbers = new StringBuilder();
            StringBuilder QuantityList = new StringBuilder();
            StringBuilder kitPlannerIDs = new StringBuilder();
            StringBuilder IsKitParents = new StringBuilder();
            StringBuilder MaterialStorageParameterIDs = new StringBuilder();
            StringBuilder MaterialStorageParameterValues = new StringBuilder();
            Literal ltMaterialID;
            Literal ltLineNumber;
            Literal ltMaterialUomID;
            TextBox txtMaterialQty;
            Literal ltKitID;
            Literal ltIsKitParent;
            Literal ltRemainingDocQty;
            Literal ltMaterialStorageParameterValue;
            bool IsEmpty = true;
            String PONumber = GetDummyPONumber();
            if (PONumber == "")
            {
                resetError("Not updated",true);
                return;
            }
            String StoreRefNo = GetDummyStoreRefNo();
            if (StoreRefNo == "")
            {
                resetError("Not updated",true);
                return;
            }
            String InvoiceNumber = GetDummyInvoiceNo();
            if (InvoiceNumber == "")
            {
                resetError("Not updated",true);
                return;
            }
            DataTable dtMaterialStorageParameter = null;
            try
            {
                dtMaterialStorageParameter = DB.GetDS("sp_GEN_GetAllMaterialStorageParameters @ParameterUsageTypeID=1,@TenantID=" + cp.TenantID, false).Tables[0];
            }
            catch (Exception ex)
            {
                resetError("Error while updating",true);
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
                return;
            }

            foreach (GridViewRow row in gvProductionOrderReturns.Rows)
            {
                txtMaterialQty = ((TextBox)row.FindControl("RemainingDocQty"));
                if (((CheckBox)row.FindControl("Select")).Checked && txtMaterialQty.Text != "" && Convert.ToDecimal(txtMaterialQty.Text) != 0)
                {
                    IsEmpty = false;
                    ltMaterialID = (Literal)row.FindControl("ltMaterialMasterID");
                    ltMaterialUomID = (Literal)row.FindControl("ltIMaterialMaster_UoMID");
                    ltLineNumber = (Literal)row.FindControl("ltLineNumber");
                    ltIsKitParent = (Literal)row.FindControl("ltIsKitParent");
                    // ltMaterialUomID = (Literal)row.FindControl("ltKitPlannerID");
                    ltKitID = (Literal)row.FindControl("ltKitPlannerID");
                    ltRemainingDocQty = (Literal)row.FindControl("ltRemainingDocQty");
                    txtMaterialQty = (TextBox)row.FindControl("RemainingDocQty");
                    if (Convert.ToDecimal(ltRemainingDocQty.Text) < Convert.ToDecimal(txtMaterialQty.Text))
                    {
                        resetError("Return quantity is more than available quantity at material :" + ((Literal)row.FindControl("ltMCode")).Text, true);
                        return;
                    }

                    lineNumbers.Append(ltLineNumber.Text + ",");

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
            if (IsEmpty)
            {
                //test case ID (TC_018)
                //select atleast one material

                resetError("No material is selected", true);
                return;
            }

            try
            {
                StringBuilder cMdUpdateProductionReturns = new StringBuilder();
                cMdUpdateProductionReturns.Append("DECLARE @Result int   ");
                cMdUpdateProductionReturns.Append("EXEC sp_MFG_UpdateProductionOrderReturns  ");
                cMdUpdateProductionReturns.Append("@ProductionOrderHeaderID="+hifProductionOrder.Value);
                cMdUpdateProductionReturns.Append(",@PONumber="+DB.SQuote(PONumber));
                cMdUpdateProductionReturns.Append(",@StoreRefNo="+DB.SQuote(StoreRefNo));
                cMdUpdateProductionReturns.Append(",@InvoiceNumber="+DB.SQuote(InvoiceNumber));
                cMdUpdateProductionReturns.Append(",@LineNumbers="+DB.SQuote(lineNumbers.ToString()));
                cMdUpdateProductionReturns.Append(",@IsKitParents="+DB.SQuote(IsKitParents.ToString()));
                cMdUpdateProductionReturns.Append(",@KitIds="+DB.SQuote(kitPlannerIDs.ToString()));
                cMdUpdateProductionReturns.Append(",@MaterialMasterIDs="+DB.SQuote(MaterialList.ToString()));
                cMdUpdateProductionReturns.Append(",@Material_UoMIDs="+DB.SQuote(MaterialUomList.ToString()));
                cMdUpdateProductionReturns.Append(",@Qtys="+DB.SQuote(QuantityList.ToString()));
                cMdUpdateProductionReturns.Append(",@MSPIDs="+DB.SQuote(MaterialStorageParameterIDs.ToString()));
                cMdUpdateProductionReturns.Append(",@MSPValues="+DB.SQuote(MaterialStorageParameterValues.ToString()));
                cMdUpdateProductionReturns.Append(",@CreatedBy="+cp.UserID);
                cMdUpdateProductionReturns.Append(",@TenantID="+cp.TenantID);
                cMdUpdateProductionReturns.Append(",@NewInboundID=@Result OUTPUT SELECT @Result AS N");
                int Result = DB.GetSqlN(cMdUpdateProductionReturns.ToString());
                Build_gvProductionReturns(Build_gvProductionReturns());
                if (Result != 0)
                {
                    resetError("Successfully returned the materials with store ref. no. :" + StoreRefNo, false);
                }
                else
                {
                    resetError("Error while updating", true);
                }
            }
            catch (Exception ex)
            {
                resetError("Error while updating",true);
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


            ltstatus.Text = str;*/
            ScriptManager.RegisterStartupScript(this, this.GetType(), "test", "closeAsynchronus(); showStickyToast(" + (isError.ToString().ToLower() == "true" ? "false" : "true") + ",\"" + error + "\");", true);

        }

        protected void lnkGet_Click(object sender, EventArgs e)
        {
            //test case ID (TC_011)
            //show all material both joborder and internal order 

            if (hifProductionOrder.Value != "" && atcProductionOrder.Text != "")
            {
                ViewState["GetStockListForProduction"] = "sp_MFG_GetStockListForProduction @ProductionOrderHeaderID=" + hifProductionOrder.Value;
                Build_gvProductionReturns(Build_gvProductionReturns());
                //ltstatus.Text = "";
                if (gvProductionOrderReturns.Rows.Count == 0)
                {
                    resetError("Materials are not received",false);
                }
            }
            else
            {
                resetError("Select Job Order Ref. No.", true);
            }
        }

        private String GetDummyStoreRefNo()
        {

            string WarehouseID ="1";


            String vNewStoreRefNUmber = null;

            try
            {
                String StoreRefNoLength = DB.GetSqlS("EXEC [sp_SYS_GetSystemConfigValue]   @SysConfigKey='inbounddetails.aspx.cs.StoreRefNo_Length' ,@TenantID=" + cp.TenantID);// DB.GetSqlS("select SysConfigValue as S from SYS_SystemConfiguration SYS_SC left join SYS_SysConfigKey SYS_SCK on  SYS_SCK.SysConfigKeyID=SYS_SC.SysConfigKeyID where TenantID=" + cp.TenantID + " and SYS_SCK.SysConfigKey='inbounddetails.aspx.cs.StoreRefNo_Length' and SYS_SCK.IsActive=1");

                vNewStoreRefNUmber = DB.GetSqlS("EXEC [sp_SYS_GetSystemConfigValue]   @SysConfigKey='dummyinboundforproductionreturn_prefix' ,@TenantID=" + cp.TenantID) + DB.GetSqlS("select WarehouseGroupCode as S from GEN_Warehouse GEN_W  left join GEN_WarehouseGroup GEN_WG on GEN_WG.WarehouseID=GEN_W.WarehouseID where  GEN_WG.WarehouseID=" + WarehouseID);

                String OldStoreRefNumber = DB.GetSqlS("select top 1  StoreRefNo AS S from INB_Inbound where  TenantID=" + cp.TenantID + " and StoreRefNo like '" + vNewStoreRefNUmber + "%' order by InboundID DESC "); // Get Previous StoreRefNo


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

                //vNewStoreRefNUmber += newvalue; // New StoreREfNumber

                // txtStrRefNo.Text = vNewStoreRefNUmber;
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
            String NewPONumber = "";
            try
            {
                NewPONumber = DB.GetSqlS("EXEC [sp_SYS_GetSystemConfigValue]   @SysConfigKey='dummypoheaderforproductionreturn_prefix' ,@TenantID=" + cp.TenantID);//"DP" + DB.GetSqlS("select SysConfigValue as S from SYS_SystemConfiguration where TenantID=" + cp.TenantID + " and SysConfigKeyID= (select SysConfigKeyID from SYS_SysConfigKey where SysConfigKey=N'purchaseorder.aspx.cs.PO_Prefix') ");
                NewPONumber += "" + (Convert.ToInt16(DateTime.Now.Year) % 100);           //add year code to ponumber

                int length =Convert.ToInt16(DB.GetSqlS("EXEC [sp_SYS_GetSystemConfigValue]   @SysConfigKey='purchaseorder.aspx.cs.PO_Length' ,@TenantID=" + cp.TenantID)); //Convert.ToInt32(DB.GetSqlS("select SysConfigValue as S from SYS_SystemConfiguration as N where TenantID=" + cp.TenantID + " and SysConfigKeyID= (select SysConfigKeyID from SYS_SysConfigKey where SysConfigKey=N'purchaseorder.aspx.cs.PO_Length') "));

                //String OldPONumber = DB.GetSqlS("select TOP 1 PONumber as S from ORD_POHeader where POTypeID=7 and TenantID=" + cp.TenantID + "  ORDER BY PONumber desc ");
                String OldPONumber = DB.GetSqlS("select TOP 1 PONumber as S from ORD_POHeader where PONumber like '"+NewPONumber+"%' and TenantID=" + cp.TenantID + "  ORDER BY PONumber desc ");
                

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

        protected void gvProductionOrderReturns_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow && (e.Row.RowState == DataControlRowState.Normal || e.Row.RowState == DataControlRowState.Alternate))
            {
                ((TextBox)e.Row.FindControl("RemainingDocQty")).Attributes.Add("onblur", "CheckIUoMQty(this)");
            }
        }
    }
}