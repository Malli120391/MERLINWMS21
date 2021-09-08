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
using System.Threading;

namespace MRLWMSC21.mInventory
{
    //Author: Gvd Prasad
    //Created On:   24/Dec/2013
    //use case ID:  Goods-OUT_UC_018

    public partial class PickItem : System.Web.UI.Page
    {
        LinkButton button;
        public CustomPrincipal cp = HttpContext.Current.User as CustomPrincipal;

        protected void page_PreInit(object sender, EventArgs e)
        {
            Page.Theme = "Inventory";
        }

        protected void page_Init(object sender, EventArgs e)
        {
            String MMID = CommonLogic.QueryString("mmid");
            String Outboundid = CommonLogic.QueryString("obdid");
            String LineNumber = CommonLogic.QueryString("LineNum");
            String LocationID = CommonLogic.QueryString("locid");
            String kitid = CommonLogic.QueryString("kitid");
            String soid = CommonLogic.QueryString("soid");
            //AddMspDropDowns(OBDNumber,MCode,LineNumber,Location);
            if (MMID != "" && Outboundid != "" && LineNumber != "" && LocationID != "" && kitid != "" && soid != "")
            {
                AddDynamicColumnsForStordInList(MMID);
                AddDynamicColumnsForPickedList(MMID);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Validate();

            DesignLogic.SetInnerPageSubHeading(this.Page, "Goods Issue (Out)");
            if (!cp.IsInAnyRoles(CommonLogic.GetRolesAllowed(CommonLogic.GetRolesAllowedForthisPage("Goods Issue (OUT)"))))
            {
                //test case ID (TC_009)
                //Only authorized user can access goods-out page

                Response.Redirect("../Login.aspx?eid=6");
            }
            if (!IsPostBack)
            {

                CommonLogic.LoadPrinters(ddlNetworkPrinter);
                CommonLogic.LoadLabelSizes(ddlLabelSize);
                String MMID = CommonLogic.QueryString("mmid");
                String Outboundid = CommonLogic.QueryString("obdid");
                String LineNumber = CommonLogic.QueryString("LineNum");
                String LocationID = CommonLogic.QueryString("locid");
                String kitid = CommonLogic.QueryString("kitid");
                String soid = CommonLogic.QueryString("soid");
                if (MMID.Length > 0 && Outboundid.Length > 0 && LineNumber.Length > 0 && LocationID.Length > 0 && kitid.Length > 0 && soid.Length > 0)
                {
                    BuildPickItemDetails();
                }
                else
                {
                    pnlgoodsOut.Visible = false;
                    resetError("Url is not correct, try again", true);
                }
            }
        }

        private void BuildPickItemDetails()
        {
            cp = HttpContext.Current.User as CustomPrincipal;
            try
            {
                IDataReader rsPickitemDetails = InventoryCommonClass.GetPickItemDetailsForOBDNumber(CommonLogic.QueryString("obdid"), CommonLogic.QueryString("mmid"), CommonLogic.QueryString("LineNum"), CommonLogic.QueryString("locid"), CommonLogic.QueryString("kitid"), CommonLogic.QueryString("soid"));
                if (rsPickitemDetails.Read())
                {
                    ltMaterialCode.Text = DB.RSField(rsPickitemDetails, "MCode");
                    ltDescription.Text = DB.RSField(rsPickitemDetails, "MDescription");
                    ltMaterialGroup.Text = DB.RSField(rsPickitemDetails, "materialGroup");
                    ViewState["DocumentTypeID"] = DB.RSFieldInt(rsPickitemDetails, "DocumentTypeID");
                    ViewState["SODetailsID"] = DB.RSFieldInt(rsPickitemDetails, "SODetailsID");
                    ViewState["IsWriteOffs"] = DB.RSFieldInt(rsPickitemDetails, "IsWriteOffs");
                    hifSuomQtyID.Value = DB.RSFieldInt(rsPickitemDetails, "MaterialMaster_SUoMID").ToString();
                    ltKitID.Text = DB.RSFieldInt(rsPickitemDetails, "kitPlannerID").ToString();
                    if (DB.RSFieldInt(rsPickitemDetails, "kitPlannerID") == 0)
                    {
                        ltKitID.Visible = false;
                        lbKitID.Visible = false;
                    }
                    ltBUoMQty.Text = DB.RSField(rsPickitemDetails, "BUoM") + "/" + DB.RSFieldDecimal(rsPickitemDetails, "BUoMQty");
                    ltMUoMQty.Text = DB.RSField(rsPickitemDetails, "MUoM") + "/" + DB.RSFieldDecimal(rsPickitemDetails, "MUoMQty");
                    ltSUoMQty.Text = DB.RSField(rsPickitemDetails, "SUoM") + "/" + DB.RSFieldDecimal(rsPickitemDetails, "SUoMQty");
                    hifMUoMQty.Value = DB.RSFieldDecimal(rsPickitemDetails, "MUoMQty").ToString();
                    hifBUoMQty.Value = DB.RSFieldDecimal(rsPickitemDetails, "BUoMQty").ToString();
                    ltDelvDocQty.Text = DB.RSFieldDecimal(rsPickitemDetails, "SOQuantity").ToString();

                    // Calculate Conversion Factor

                    hifMeasurementType.Value = DB.RSFieldInt(rsPickitemDetails, "MeasurementTypeID").ToString();

                    ltQtyperuom.Text = Math.Round(DB.RSFieldDecimal(rsPickitemDetails, "CF"), 3).ToString();
                    hifConversion.Value = DB.RSFieldDecimal(rsPickitemDetails, "CF").ToString();
                    hifconversionInMUoM.Value = DB.RSFieldDecimal(rsPickitemDetails, "CFInMUoM").ToString();
                    ltTotalQuantity.Text = Math.Round(DB.RSFieldDecimal(rsPickitemDetails, "CF") * DB.RSFieldDecimal(rsPickitemDetails, "SOQuantity"), 2).ToString();

                    if (DB.RSFieldInt(rsPickitemDetails, "MeasurementTypeID") == 0) // Is Other Measurement or not
                    {
                        if (DB.RSField(rsPickitemDetails, "SUoM") == DB.RSField(rsPickitemDetails, "MUoM") && DB.RSField(rsPickitemDetails, "MUoM") == DB.RSField(rsPickitemDetails, "BUoM"))
                        {
                            //ltQtyperuom.Text = Math.Round(DB.RSFieldDecimal(rsPickitemDetails, "MUoMQty") / DB.RSFieldDecimal(rsPickitemDetails, "BUoMQty"), 2).ToString();
                            //hifConversion.Value = ltQtyperuom.Text;
                            //hifconversionInMUoM.Value = ltQtyperuom.Text;
                            ViewState["ConvertionFactor"] = "# Convertion: MUoMQty/BUoMQty :" + ltQtyperuom.Text;
                            //ltTotalQuantity.Text = Math.Round(DB.RSFieldDecimal(rsPickitemDetails, "MUoMQty") / DB.RSFieldDecimal(rsPickitemDetails, "BUoMQty") * DB.RSFieldDecimal(rsPickitemDetails, "SOQuantity"), 2).ToString();
                        }
                        else if (DB.RSField(rsPickitemDetails, "SUoM") == DB.RSField(rsPickitemDetails, "MUoM"))
                        {
                            //ltQtyperuom.Text = Math.Round(DB.RSFieldDecimal(rsPickitemDetails, "MUoMQty") * (DB.RSFieldDecimal(rsPickitemDetails, "SUoMQty") / DB.RSFieldDecimal(rsPickitemDetails, "MUoMQty")), 2).ToString();
                            //hifConversion.Value = ltQtyperuom.Text;
                            //hifconversionInMUoM.Value = DB.RSFieldDecimal(rsPickitemDetails, "MUoMQty").ToString();
                            ViewState["ConvertionFactor"] = " # Convertion:  MUoMQty*SUoMQty/MUoMQty:" + ltQtyperuom.Text;
                            //ltTotalQuantity.Text = Math.Round(DB.RSFieldDecimal(rsPickitemDetails, "SOQuantity") * DB.RSFieldDecimal(rsPickitemDetails, "SUoMQty"), 2).ToString();
                        }
                        else if (DB.RSField(rsPickitemDetails, "MUoM") == DB.RSField(rsPickitemDetails, "BUoM"))
                        {

                            //ltQtyperuom.Text = Math.Round(DB.RSFieldDecimal(rsPickitemDetails, "MUoMQty") / DB.RSFieldDecimal(rsPickitemDetails, "BUoMQty") * DB.RSFieldDecimal(rsPickitemDetails, "SUoMQty"), 2).ToString();
                            //hifConversion.Value = ltQtyperuom.Text;
                            //hifconversionInMUoM.Value = Math.Round(DB.RSFieldDecimal(rsPickitemDetails, "MUoMQty") / DB.RSFieldDecimal(rsPickitemDetails, "BUoMQty"), 2).ToString();
                            ViewState["ConvertionFactor"] = "# Convertion: MUoMQty/BUoMQty*SUoMQty:" + ltQtyperuom.Text;
                            //ltTotalQuantity.Text = Math.Round(DB.RSFieldDecimal(rsPickitemDetails, "SOQuantity") * (DB.RSFieldDecimal(rsPickitemDetails, "MUoMQty") / DB.RSFieldDecimal(rsPickitemDetails, "BUoMQty") * DB.RSFieldDecimal(rsPickitemDetails, "SUoMQty")), 2).ToString();
                        }
                        else
                        {
                            //ltQtyperuom.Text = Math.Round(DB.RSFieldDecimal(rsPickitemDetails, "MUoMQty") * DB.RSFieldDecimal(rsPickitemDetails, "SUoMQty"), 2).ToString();
                            //hifConversion.Value = ltQtyperuom.Text;
                            //hifconversionInMUoM.Value = DB.RSFieldDecimal(rsPickitemDetails, "MUoMQty").ToString();
                            ViewState["ConvertionFactor"] = "# Convertion: MUoMQty*SUoMQty :" + ltQtyperuom.Text;
                            //ltTotalQuantity.Text = Math.Round(DB.RSFieldDecimal(rsPickitemDetails, "SOQuantity") * DB.RSFieldDecimal(rsPickitemDetails, "MUoMQty") * DB.RSFieldDecimal(rsPickitemDetails, "SUoMQty"), 2).ToString();
                        }
                    }
                    else// Measurement type
                    {
                        //decimal ConversionValue = DB.GetSqlNDecimal("select convert(decimal(18,2),ISNULL(MTD.ConvesionValue,1)*IIF(ISNULL(MPM1.PowerOf,0)-ISNULL(MPM.PowerOf,0)<0,1/POWER(convert(float,10),ISNULL(MPM.PowerOf,0)-ISNULL(MPM1.PowerOf,0)),POWER(convert(float,10),ISNULL(MPM1.PowerOf,0)-ISNULL(MPM.PowerOf,0)))) N "
                        //                                         + " from GEN_UoM BUOM "
                        //                                         + " JOIN GEN_UoM IUOM ON IUOM.UoMID= " + DB.RSFieldInt(rsPickitemDetails, "SUoMID")
                        //                                        + " join MES_MeasurementMaster MTM on MTM.MeasurementID=BUOM.MeasurementID "
                        //                                        + " LEFT JOIN MES_MatricPrefixMaster MPM ON MTM.ConversionTypeID=1 and MPM.MetricPrifixID=BUOM.MatricPrifixID "
                        //                                        + " left join MES_MatricPrefixMaster MPM1 ON MPM1.MetricPrifixID=IUOM.MatricPrifixID "
                        //                                        + " LEFT join MES_MeasurementDetails MTD on MTD.MeasurementID=IUOM.MeasurementID  AND MTD.ToMesurementID=MTM.MeasurementID "
                        //                                        + "WHERE BUOM.UoMID=" + DB.RSFieldInt(rsPickitemDetails, "BUoMID"));

                        //String cmdGetConversionValues = "select	convert(decimal(18,6),ISNULL(MTD.ConvesionValue,1)*IIF(ISNULL(IMPM.PowerOf,0)-ISNULL(MPM.PowerOf,0)<0,1/POWER(convert(float,10),ISNULL(MPM.PowerOf,0)-ISNULL(IMPM.PowerOf,0)),POWER(convert(float,10),ISNULL(IMPM.PowerOf,0)-ISNULL(MPM.PowerOf,0)))) TotalConversion,"

                        //                               + " convert(decimal(18,6),ISNULL(MMTD.ConvesionValue,1)*IIF(ISNULL(IMPM.PowerOf,0)-ISNULL(MMPM.PowerOf,0)<0,1/POWER(convert(float,10),ISNULL(MMPM.PowerOf,0)-ISNULL(IMPM.PowerOf,0)),POWER(convert(float,10),ISNULL(IMPM.PowerOf,0)-ISNULL(MMPM.PowerOf,0)))) MinPickConversion "
                        //                               + " from GEN_UoM BUOM "
                        //                               + " join MES_MeasurementMaster MTM on MTM.MeasurementID=BUOM.MeasurementID"
                        //                               + " LEFT JOIN MES_MatricPrefixMaster MPM ON MTM.ConversionTypeID=1 and MPM.MetricPrifixID=BUOM.MatricPrifixID"

                        //                               + " JOIN GEN_UoM IUOM ON IUOM.UoMID=" +DB.RSFieldInt(rsPickitemDetails, "SUoMID")
                        //                               + " left join MES_MatricPrefixMaster IMPM ON IMPM.MetricPrifixID=IUOM.MatricPrifixID "
                        //                               + " LEFT join MES_MeasurementDetails MTD on MTD.MeasurementID=IUOM.MeasurementID  AND MTD.ToMesurementID=MTM.MeasurementID "

                        //                               + " JOIN GEN_UoM MUOM ON MUOM.UoMID=" +DB.RSFieldInt(rsPickitemDetails, "MUoMID")
                        //                               + " left join MES_MatricPrefixMaster MMPM ON MMPM.MetricPrifixID=MUOM.MatricPrifixID "
                        //                               + " LEFT join MES_MeasurementDetails MMTD on MMTD.MeasurementID=IUOM.MeasurementID  AND MMTD.ToMesurementID=MUOM.MeasurementID "

                        //                               + " WHERE BUOM.UoMID=" +DB.RSFieldInt(rsPickitemDetails, "BUoMID");

                        //IDataReader rsGetConversionValues = DB.GetRS(cmdGetConversionValues);
                        //if (rsGetConversionValues.Read())
                        //{
                        //    ltQtyperuom.Text = Math.Round(DB.RSFieldDecimal(rsGetConversionValues, "TotalConversion") * DB.RSFieldDecimal(rsPickitemDetails, "SUoMQty") / DB.RSFieldDecimal(rsPickitemDetails, "BUoMQty"), 3).ToString();
                        //    hifConversion.Value = Math.Round(DB.RSFieldDecimal(rsGetConversionValues, "TotalConversion") * DB.RSFieldDecimal(rsPickitemDetails, "SUoMQty") / DB.RSFieldDecimal(rsPickitemDetails, "BUoMQty"), 6).ToString();
                        //    hifconversionInMUoM.Value = (DB.RSFieldDecimal(rsGetConversionValues, "MinPickConversion") * DB.RSFieldDecimal(rsPickitemDetails, "SUoMQty") / DB.RSFieldDecimal(rsPickitemDetails, "MUoMQty")).ToString();
                        ViewState["ConvertionFactor"] = "# Convertion: Measurement Conversion:" + ltQtyperuom.Text;
                        //    ltTotalQuantity.Text = Math.Round(DB.RSFieldDecimal(rsPickitemDetails, "SOQuantity") * DB.RSFieldDecimal(rsGetConversionValues, "TotalConversion") * DB.RSFieldDecimal(rsPickitemDetails, "SUoMQty") / DB.RSFieldDecimal(rsPickitemDetails, "BUoMQty"), 2).ToString();
                        //}
                        //rsGetConversionValues.Close();



                    }
                    ViewState["gvPOQuantityList"] = "EXEC dbo.sp_INV_GetStockInList @MaterialMasterID=" + CommonLogic.QueryString("mmid") + ",@LocationID=" + CommonLogic.QueryString("locid") + ",@KitPlannerID=" + CommonLogic.QueryString("kitid") + ",@OutBoundID=" + CommonLogic.QueryString("obdid") + ",@SOHeaderID=" + CommonLogic.QueryString("soid") + ",@LineNumber=" + CommonLogic.QueryString("LineNum");
                    Build_gvPOQuantityilst(Build_gvPOQuantityilst());

                    ViewState["gvPickedItems"] = "EXEC dbo.sp_INV_GetStockOutList @MaterialMasterID=" + CommonLogic.QueryString("mmid") + ",@LocationID=" + CommonLogic.QueryString("locid") + ",@OutboundID=" + CommonLogic.QueryString("obdid") + ",@linenumber=" + CommonLogic.QueryString("LineNum") + ",@SOHeaderID=" + CommonLogic.QueryString("soid");
                    Build_gvPicketItem(Build_gvPicketItem());
                    if (gvPickedItemList.Rows.Count != 0)
                    {
                        Decimal StockoutSum = DB.GetSqlNDecimal("EXEC dbo.sp_INV_GetGoodsOutQuantitySum @MaterialMasterID=" + CommonLogic.QueryString("mmid") + ",@LocationID=" + CommonLogic.QueryString("locid") + ",@OutboundID=" + CommonLogic.QueryString("obdid") + ",@linenumber=" + CommonLogic.QueryString("LineNum") + ",@SOHeaderID=" + CommonLogic.QueryString("soid"));
                        ((Literal)gvPickedItemList.FooterRow.FindControl("ltQuantityCount")).Text = "Sum :" + StockoutSum;
                    }
                }
                else
                {
                    pnlgoodsOut.Visible = false;
                    resetError("No data found to this criteria", true);
                }
                rsPickitemDetails.Close();
            }
            catch (Exception ex)
            {
                resetError("Error while build data", true);
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
            }
        }

        protected void lnkPickItem_Click(object sender, EventArgs e)
        {
            //test case ID (TC_012)
            //Update Picked details
            cp = HttpContext.Current.User as CustomPrincipal;
            if (chkIsPrintRequired.Checked == true)
            {
                if (ddlNetworkPrinter.SelectedValue == "0")
                {
                    resetError("Please Select Printer", true);
                    return;
                }
                if (ddlLabelSize.SelectedValue == "0")
                {
                    resetError("Please Select Label Size ", true);
                    return;
                }
            }


           
            //String text= txtPickLocation.Text;
            Page.Validate("Pickitems");
            if (IsValid)
            {

                int gridviewcount = gvPOQuantityList.Rows.Count;
                foreach (GridViewRow gvrow in gvPOQuantityList.Rows)
                {
                    CheckBox rdbutton = ((CheckBox)gvrow.FindControl("SelectOne"));
                    if (rdbutton.Checked)
                    {
                        String LineNumber = CommonLogic.QueryString("LineNum");
                        String MMID = CommonLogic.QueryString("mmid");
                        String Outboundid = CommonLogic.QueryString("obdid");
                        String LocationID = CommonLogic.QueryString("locid");
                        String kitid = CommonLogic.QueryString("kitid");
                        String SOHeaderID = CommonLogic.QueryString("soid");


                        //Check whether is returned outbound are not

                        //If Normal Outbound 
                        if (((int)ViewState["DocumentTypeID"]) != 7)
                        {
                            String pickQuantity = txtPickQty.Text.Trim();
                            String location = ((Literal)gvrow.FindControl("ltLocation")).Text;
                            String IsDamaged = ((Literal)gvrow.FindControl("ltIsDamaged")).Text;
                            String IsNonConformity = ((Literal)gvrow.FindControl("ltIsNonConfirmity")).Text;
                            String AsIs = ((Literal)gvrow.FindControl("ltAsIs")).Text;
                            String IsPositiveRecall = ((Literal)gvrow.FindControl("ltIsPositiveRecall")).Text;
                            String HasDiscrepancy = ((Literal)gvrow.FindControl("ltHasDiscrepancy")).Text;
                            String AvialableQty = ((Literal)gvrow.FindControl("ltQuantity")).Text;
                            String CartonID = ((HiddenField)gvrow.FindControl("hifCartonID")).Value;

                            // Storage Location

                            String StorageLocation = ((Literal)gvrow.FindControl("ltStorageLocationID")).Text;

                            //
                            Decimal AvialableQuantityinDecimal = Convert.ToDecimal(AvialableQty);
                            String Quantity = (Convert.ToDecimal(pickQuantity) * Convert.ToDecimal(hifConversion.Value)).ToString();
                            StringBuilder sCmdUpdatePickItem = new StringBuilder();

                            //check status of outbound
                            int StatusID = DB.GetSqlN("SELECT DeliveryStatusID AS N FROM OBD_Outbound WHERE OutboundID=" + Outboundid);
                            if (StatusID >= 4 && StatusID != 5)
                            {
                                //test case ID (TC_023)
                                //PGI is updated, cannot pick

                                resetError("PGI is updated, cannot pick", true);
                                return;
                            }

                            //GEt MSPs list
                            IDataReader rsConfigureMSP = null;
                            try
                            {
                                rsConfigureMSP = DB.GetRS("[sp_ORD_GetMaterialStorageParameters] @MaterialMasterID=" + MMID + ",@TenantID=" + cp.TenantID);
                            }
                            catch (Exception ex)
                            {
                                resetError("Error while transation", true);
                                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
                                return;
                            }
                            String ControlID = "";
                            String value = "";
                            String MSPIDs = "";
                            String MSPValues = "";
                            while (rsConfigureMSP.Read())
                            {
                                String ControlType = DB.RSField(rsConfigureMSP, "ControlType");
                                if (ControlType == "DropDownList")
                                {
                                    if (DB.RSField(rsConfigureMSP, "ParameterName") == "Plant")
                                        ControlID = "M" + DB.RSField(rsConfigureMSP, "ParameterName") + "ID";
                                    else
                                        ControlID = DB.RSField(rsConfigureMSP, "ParameterName") + "ID";
                                }
                                else if (ControlType == "TextBox")
                                {
                                    ControlID = DB.RSField(rsConfigureMSP, "ParameterName");
                                    if (ControlID == "ExpDate" && ((int)ViewState["IsWriteOffs"]) == 0)
                                    {
                                        String stExpDate = ((Literal)gvrow.FindControl(ControlID)).Text;
                                        if (stExpDate != "")
                                        {
                                            DateTime expiredate = DateTime.ParseExact(stExpDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                            int result = DateTime.Compare(expiredate, DateTime.Now);
                                            if (result < 0)
                                            {
                                                //test case ID (TC_022)
                                                //Expired items cannot be picked

                                                resetError("Cannot pick the expired items", true);
                                                rsConfigureMSP.Close();
                                                return;
                                            }
                                        }

                                    }
                                }
                                value = ((Literal)gvrow.FindControl(ControlID)).Text;


                                if (value != "")
                                {
                                    MSPIDs += DB.RSFieldInt(rsConfigureMSP, "MaterialStorageParameterID") + ",";
                                    MSPValues += value + ",";
                                }
                            }
                            rsConfigureMSP.Close();


                            // sCmdUpdatePickItem.Append(",@MaterialStorageParameterIDs="+DB.SQuote(MSPIDs));
                            // sCmdUpdatePickItem.Append(",@MaterialStorageParameterValues="+DB.SQuote(MSPValues));
                            try
                            {
                                /* IDataReader rsobdReceiveDate = DB.GetRS("select OBDReceivedOn from OBD_OutboundTracking_Warehouse obdw join OBD_Outbound obd on obd.OutboundID=obdw.OutboundID and obdw.IsActive=1 and obdw.IsDeleted=0 where obd.IsDeleted=0 and obd.IsActive=1 and obd.OutboundID=" + Outboundid);
                                 if (!rsobdReceiveDate.Read())
                                 {
                                     resetError("Outbound not received yet,cannot pick ",true);
                                     return;
                                 }*/

                                try
                                {
                                    //check cycle count
                                    int CycleCountOn = DB.GetSqlTinyInt("select convert(int,max(qcc.IsOn)) as N from QCC_CycleCount qcc left join QCC_CycleCountDetails qccd on qccd.CycleCountID=qcc.CycleCountID  where qccd.MaterialMasterID=" + MMID);
                                    if (CycleCountOn == 1)
                                    {
                                        //test case ID (TC_025)
                                        //Cannot pick, as material is in cycle count

                                        resetError("Cannot pick, as this material is in 'Cycle Count'", true);
                                        return;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    resetError("Error while transaction", true);
                                    CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
                                    return;
                                }
                                Decimal pickedQuantity = 0;
                                try
                                {
                                    pickedQuantity = Convert.ToDecimal(txtPickQty.Text.Trim());
                                }
                                catch (Exception ex)
                                {
                                    resetError("'Pick Quantity' is not a valid number", true);
                                    CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
                                    return;
                                }

                                //Picked Quantity should be non zero
                                if (pickedQuantity == 0)
                                {
                                    //test case ID (TC_020)
                                    //Qty. cannot be zero

                                    resetError("Quantity should be greater than zero", true);
                                    return;

                                }

                                //available quantity is greater than equal to picked quantity
                                if (pickedQuantity > AvialableQuantityinDecimal)
                                {
                                    //test case ID (TC_015)
                                    //Pick quantity should be less than or equal to available quantity

                                    resetError("Pick quantity should be less than or equal to available quantity", true);
                                    return;
                                }

                                Decimal StockoutSum = 0;
                                try
                                {
                                    StockoutSum = DB.GetSqlNDecimal("EXEC dbo.sp_INV_GetGoodsOutQuantitySum @MaterialMasterID=" + MMID + ",@LocationID=" + LocationID + ",@OutboundID=" + Outboundid + ",@linenumber=" + LineNumber + ",@SOHeaderID=" + SOHeaderID);
                                }
                                catch (Exception ex)
                                {
                                    resetError("Error while transaction", true);
                                    CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
                                    return;
                                }

                                //check picked quantity should be less than or equal to pending quantity
                                Decimal totalQuantity = Convert.ToDecimal(ltDelvDocQty.Text);
                                if (StockoutSum + pickedQuantity > totalQuantity)
                                {
                                    //test case ID (TC_016)
                                    //Pick quantity should be less than or equal to delivery doc. qty.

                                    resetError("Pick quantity is greater than 'Delv Doc. Qty.'", true);
                                    return;
                                }


                                if (chkIsPrintRequired.Checked)
                                {
                                    if (ddlNetworkPrinter.SelectedValue == "0")
                                    {
                                        //test case ID (TC_013)
                                        //select Print option when check the print check box

                                        resetError("Please select printer", true);
                                        return;
                                    }
                                }


                                int Result = InventoryCommonClass.UpdateGoodsMovementDetails("0", "2", location, hifSuomQtyID.Value, pickQuantity, Quantity, CommonLogic.QueryString("obdid"), "0", LineNumber, CommonLogic.QueryString("mmid"), IsDamaged.ToString(), HasDiscrepancy, CommonLogic.QueryString("soid"), cp.UserID.ToString(), MSPIDs, MSPValues, ViewState["ConvertionFactor"].ToString(), "", cp.UserID.ToString(), "0", ltKitID.Text, IsPositiveRecall, IsNonConformity, AsIs, CartonID, StorageLocation);

                                ViewState["gvPOQuantityList"] = "EXEC dbo.sp_INV_GetStockInList @MaterialMasterID=" + MMID + ",@LocationID=" + LocationID + ",@KitPlannerID=" + kitid + ",@OutBoundID=" + Outboundid + ",@SOHeaderID=" + SOHeaderID + ",@LineNumber=" + LineNumber;
                                Build_gvPOQuantityilst(Build_gvPOQuantityilst());

                                gvPickedItemList.EditIndex = -1;
                                ViewState["gvPickedItems"] = "EXEC dbo.sp_INV_GetStockOutList @MaterialMasterID=" + MMID + ",@LocationID=" + LocationID + ",@OutboundID=" + Outboundid + ",@linenumber=" + LineNumber + ",@SOHeaderID=" + SOHeaderID;
                                Build_gvPicketItem(Build_gvPicketItem());

                                if (gvPickedItemList.Rows.Count != 0)
                                {
                                    StockoutSum = DB.GetSqlNDecimal("EXEC dbo.sp_INV_GetGoodsOutQuantitySum @MaterialMasterID=" + MMID + ",@LocationID=" + LocationID + ",@OutboundID=" + Outboundid + ",@linenumber=" + LineNumber + ",@SOHeaderID=" + SOHeaderID);
                                    ((Literal)gvPickedItemList.FooterRow.FindControl("ltQuantityCount")).Text = "Sum :" + StockoutSum;
                                }
                                // DB.ExecuteSQL(sCmdUpdatePickItem.ToString());
                                if (Result == 1)
                                {

                                    resetError("Successfully Picked", false);

                                    if (chkIsPrintRequired.Checked)
                                    {
                                        if (ddlNetworkPrinter.SelectedValue != "0")
                                            PrintLabel((IsPositiveRecall == "1" ? ltMaterialCode.Text + "-P" : ltMaterialCode.Text), CommonLogic.QueryString("mmid"), ltDescription.Text, CommonLogic.QueryString("obdid"), MSPIDs, MSPValues, pickQuantity, ddlNetworkPrinter.SelectedValue, "", "", "", "", "");
                                    }

                                }
                                else if (Result == -1)
                                {
                                    //test case ID (TC_026)
                                    //Require qty. is not available in store

                                    resetError("Require quantity not available in store", true);
                                }
                                else
                                {
                                    resetError("Error while updating", true);
                                }
                            }
                            catch (Exception ex)
                            {
                                resetError("Error while transaction", true);
                                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
                            }
                        }
                        else
                        {
                            String pickQuantity = txtPickQty.Text.Trim();
                            String GoodsMovementDetailsID = ((HiddenField)gvrow.FindControl("hifGoodsMovementDetailsID")).Value;
                            String TransactionDocID = CommonLogic.QueryString("obdid");
                            String SOPODetialsID = ViewState["SODetailsID"].ToString();
                            String CreatedBy = cp.UserID.ToString();
                            String ConvertionFactor = ViewState["ConvertionFactor"].ToString();
                            String Quantity = (Convert.ToDecimal(pickQuantity) * Convert.ToDecimal(hifConversion.Value)).ToString();
                            String MaterialMaster_iuom = hifSuomQtyID.Value;
                            String CartonID = ((HiddenField)gvrow.FindControl("hifCartonID")).Value;
                            try
                            {
                                int StatusID = 0;
                                try
                                {
                                    StatusID = DB.GetSqlN("SELECT DeliveryStatusID AS N FROM OBD_Outbound WHERE OutboundID=" + Outboundid);
                                }
                                catch (Exception ex)
                                {
                                    resetError("Error while transaction", true);
                                    CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
                                    return;
                                }

                                //check status of outbound
                                if (StatusID >= 4 && StatusID != 5)
                                {
                                    resetError("PGI is updated, cannot pick", true);
                                    return;
                                }

                                //check line item is in SO or not
                                int CycleCountOn = DB.GetSqlTinyInt("select qcc.IsOn as TI from QCC_CycleCount qcc left join QCC_CycleCountDetails qccd on qccd.CycleCountID=qcc.CycleCountID  where qccd.MaterialMasterID=" + MMID);
                                if (CycleCountOn == 1)
                                {
                                    resetError("This material is in 'Cycle Count', cannot pick ", true);
                                    return;
                                }
                                Decimal pickedQuantity = Convert.ToDecimal(txtPickQty.Text.Trim());

                                //picked quantity shold be greater than zero
                                if (pickedQuantity == 0)
                                {
                                    resetError("Quantity always greaterthan 0", true);
                                    return;

                                }
                                String AvialableQty = ((Literal)gvrow.FindControl("ltQuantity")).Text;
                                Decimal AvialableQuantityinDecimal = Convert.ToDecimal(AvialableQty);
                                if (pickedQuantity > AvialableQuantityinDecimal)
                                {
                                    resetError("Pick quantity is more than available quantity", true);
                                    return;
                                }

                                //Get Currently Picked total Quantity
                                Decimal StockoutSum = DB.GetSqlNDecimal("EXEC dbo.sp_INV_GetGoodsOutQuantitySum @MaterialMasterID=" + MMID + ",@LocationID=" + LocationID + ",@OutboundID=" + Outboundid + ",@linenumber=" + LineNumber + ",@SOHeaderID=" + SOHeaderID);

                                Decimal totalQuantity = Convert.ToDecimal(ltDelvDocQty.Text);
                                if (StockoutSum + pickedQuantity > totalQuantity)
                                {
                                    resetError("Cannot pick, as pick quantity is greater than SO quantity", true);
                                    return;
                                }
                                int Result = InventoryCommonClass.UpdateReturnGoodsMovementDetails(GoodsMovementDetailsID, Quantity, pickQuantity, TransactionDocID, SOPODetialsID, ConvertionFactor, MaterialMaster_iuom, CreatedBy, CartonID);

                                ViewState["gvPOQuantityList"] = "EXEC dbo.sp_INV_GetStockInList @MaterialMasterID=" + MMID + ",@LocationID=" + LocationID + ",@KitPlannerID=" + kitid + ",@OutBoundID=" + Outboundid + ",@SOHeaderID=" + SOHeaderID + ",@LineNumber=" + LineNumber;
                                Build_gvPOQuantityilst(Build_gvPOQuantityilst());

                                gvPickedItemList.EditIndex = -1;
                                ViewState["gvPickedItems"] = "EXEC dbo.sp_INV_GetStockOutList @MaterialMasterID=" + MMID + ",@LocationID=" + LocationID + ",@OutboundID=" + Outboundid + ",@linenumber=" + LineNumber + ",@SOHeaderID=" + SOHeaderID;
                                Build_gvPicketItem(Build_gvPicketItem());

                                if (gvPickedItemList.Rows.Count != 0)
                                {
                                    StockoutSum = DB.GetSqlNDecimal("EXEC dbo.sp_INV_GetGoodsOutQuantitySum @MaterialMasterID=" + MMID + ",@LocationID=" + LocationID + ",@OutboundID=" + Outboundid + ",@linenumber=" + LineNumber + ",@SOHeaderID=" + SOHeaderID);
                                    ((Literal)gvPickedItemList.FooterRow.FindControl("ltQuantityCount")).Text = "Sum :" + StockoutSum;
                                }
                                // DB.ExecuteSQL(sCmdUpdatePickItem.ToString());
                                if (Result == 1)
                                {
                                    resetError("Successfully Picked", false);

                                }
                                else if (Result == -1)
                                    resetError("Require quantity not available in Store", true);
                                else
                                    resetError("Not updated ", true);
                            }
                            catch (Exception ex)
                            {
                                resetError("Error while transaction", true);
                                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
                            }
                        }
                        return;
                    }


                }
                resetError("Please select location", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "test", "closeAsynchronus();", true);
            }
        }

        protected String GetQtyInIUoM(String TotalQty)
        {
            //String QtyinIUoM = Math.Round((Convert.ToDecimal(TotalQty) / Convert.ToDecimal(ltQtyperuom.Text)), 2) .ToString();
            String QtyinIUoM = (Math.Truncate(Convert.ToDecimal(TotalQty) / Convert.ToDecimal(hifConversion.Value) * 100) / 100).ToString("#.00");
            //String QtyinIUoM = (Math.Truncate(Convert.ToDecimal(TotalQty) / 1 * 100) / 100).ToString();
            //String QtyinIUoM = Convert.ToDecimal( TotalQty).ToString("#.00");
            return QtyinIUoM;
        }

        #region----Grid data Functions---

        public int Getint(String IsChecked)
        {
            if (IsChecked != "")
            {
                if (IsChecked == "Yes")
                    return 1;
                else
                    return 0;
            }
            else
            {
                return 0;
            }
        }

        public Boolean GetBool(String IsChecked)
        {
            if (IsChecked != "")
            {
                if (Convert.ToBoolean(Convert.ToInt32(IsChecked)))
                    return true;
                else
                    return false;
            }
            else
            {
                return false;
            }
        }

        public String Getimage(String IsChecked)
        {
            if (IsChecked != "")
            {
                if (IsChecked == "Yes")
                    return "../Images/blue_menu_icons/check_mark.png";
                else
                    return null;
            }
            else
            {
                return null;
            }
        }

        #endregion

        #region ----Dynamic Controls Adding---

        private void AddMspDropDowns(String OBDNumber, String MCode, String LineNumber, String Location)
        {
            cp = HttpContext.Current.User as CustomPrincipal;
            try
            {
                //IDataReader drMatrialMsp = DB.GetRS("select ParameterName,ControlType,DataSource from MMT_MaterialStorageParameter  where IsDeleted=0 AND IsActive=1");
                IDataReader drMatrialMsp = DB.GetRS("sp_INV_GetMaterialStorageParameters @MCode=" + DB.SQuote(MCode) + ",@TenantID=" + cp.TenantID);

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
                        htRow = new HtmlTableRow();
                        htCell = new HtmlTableCell();
                        htCell.Align = "left";
                        htCell.Width = "54.5%";
                        ltliteral = new Literal();
                        ltliteral.ID = "lt" + DB.RSField(drMatrialMsp, "ParameterName");
                        ltliteral.Text = DB.RSField(drMatrialMsp, "ParameterName") + ":<br />";

                        String ControlType = DB.RSField(drMatrialMsp, "ControlType");
                        RequiredFieldValidator validator = new RequiredFieldValidator();
                        validator.ValidationGroup = "receiveQuantity";
                        validator.ID = "rfv" + DB.RSField(drMatrialMsp, "ParameterName");
                        validator.Display = ValidatorDisplay.Dynamic;
                        validator.ErrorMessage = "*";

                        ddlDropDownList = new DropDownList();

                        ddlDropDownList.ID = "ddl" + DB.RSField(drMatrialMsp, "ParameterName");
                        ddlDropDownList.Width = 160;
                        // BuildDynamicDropDown(ddlDropDownList, DB.RSField(drMatrialMsp, "DataSource"), DB.RSField(drMatrialMsp, "ParameterName"));

                        validator.ControlToValidate = ddlDropDownList.ID;
                        htCell.Controls.Add(validator);
                        htCell.Controls.Add(ltliteral);
                        htCell.Controls.Add(ddlDropDownList);
                        htRow.Cells.Add(htCell);
                    }
                    else
                    {
                        //htRow = new HtmlTableRow();
                        htCell = new HtmlTableCell();
                        htCell.Align = "left";
                        htCell.Width = "100%";
                        // htCell.Style.Add("padding-left", "140px");
                        txttextbox = new TextBox();
                        ltliteral = new Literal();
                        ltliteral.ID = "lt" + DB.RSField(drMatrialMsp, "ParameterName");
                        ltliteral.Text = DB.RSField(drMatrialMsp, "ParameterName") + ":<br />";

                        String ControlType = DB.RSField(drMatrialMsp, "ControlType");

                        RequiredFieldValidator validator = new RequiredFieldValidator();
                        validator.ValidationGroup = "receiveQuantity";
                        validator.ID = "rfv" + DB.RSField(drMatrialMsp, "ParameterName");
                        validator.Display = ValidatorDisplay.Dynamic;
                        validator.ErrorMessage = "*";

                        ddlDropDownList = new DropDownList();
                        ddlDropDownList.ID = "ddl" + DB.RSField(drMatrialMsp, "ParameterName");
                        ddlDropDownList.Width = 160;
                        // BuildDynamicDropDown(ddlDropDownList, DB.RSField(drMatrialMsp, "DataSource"), DB.RSField(drMatrialMsp, "ParameterName"));
                        validator.ControlToValidate = ddlDropDownList.ID;
                        htCell.Controls.Add(validator);
                        htCell.Controls.Add(ltliteral);

                        htCell.Controls.Add(ddlDropDownList);

                        htRow.Cells.Add(htCell);
                        //tbMSPView.Rows.Add(htRow);
                        htRow = new HtmlTableRow();
                        htCell = new HtmlTableCell();
                        htCell.Controls.Add(new LiteralControl("&nbsp;"));
                        htRow.Cells.Add(htCell);
                        /// tbMSPView.Rows.Add(htRow);
                    }


                }
                if (mspCount % 2 != 0)
                {
                    // tbMSPView.Rows.Add(htRow);
                    //  int count=tbMSPView.Controls.Count;
                    // ((Object)tbMSPView.Controls[0])
                }
                drMatrialMsp.Close();
                //tbMaterialStorageparameter.Rows.Add(new HtmlTableRow());
            }
            catch (Exception ex)
            {
                resetError("Error while add Material Storage Parameter", true);
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
            }
        }

        private void AddDynamicColumnsForStordInList(String MMID)
        {
            cp = HttpContext.Current.User as CustomPrincipal;
            TemplateField field;
            try
            {
                IDataReader rsMSPList = DB.GetRS("[sp_ORD_GetMaterialStorageParameters] @MaterialMasterID=" + MMID + ",@TenantID=" + cp.TenantID);
                //DataControlField ss = gvPOQuantityList.Columns[4];

                //gvPOQuantityList.Columns.RemoveAt(4);
                while (rsMSPList.Read())
                {
                    field = new TemplateField();
                    field.HeaderText = DB.RSField(rsMSPList, "DisplayName");
                    field.HeaderTemplate = new DynamicallyTemplatedGridViewHandler(ListItemType.Header, DB.RSField(rsMSPList, "DisplayName"), "Literal");
                    field.HeaderStyle.Width = 150;
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
                    gvPOQuantityList.Columns.Add(field);
                    // gvPOQuantityList.Columns.Insert(columnCount - 1, field);
                    //columnCount++;
                }

                rsMSPList.Close();
                field = new TemplateField();
                field.HeaderText = "Select One";
                //field.HeaderTemplate = new DynamicallyTemplatedGridViewHandler(ListItemType.Header, "Select", "Literal");

                field.ItemTemplate = new DynamicallyTemplatedGridViewHandler(ListItemType.Item, "SelectOne", "RadioButton");


                gvPOQuantityList.Columns.Add(field);
                // gvPOQuantityList.Columns.Add(ss);


            }
            catch (Exception ex)
            {
                resetError("Error while add Material Storage Parameter", true);
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
            }
        }

        private void AddDynamicColumnsForPickedList(String MMID)
        {
            cp = HttpContext.Current.User as CustomPrincipal;
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
                    field.ItemTemplate = new DynamicallyTemplatedGridViewHandler(ListItemType.Item, DB.RSField(rsMSPList, "ParameterName"), "Literal");
                    Boolean requirevalidation = Convert.ToBoolean(DB.RSFieldTinyInt(rsMSPList, "IsRequired"));
                    /* switch (DB.RSField(rsMSPList, "ControlType"))
                     {
                         case "DropDownList":
                             field.EditItemTemplate = new DynamicallyTemplatedGridViewHandler(ListItemType.EditItem, DB.RSField(rsMSPList, "ParameterName"), "DropDownList", DB.RSField(rsMSPList, "DataSource"), "gridrequirequantity", requirevalidation);
                             break;

                         case "TextBox":
                             field.EditItemTemplate = new DynamicallyTemplatedGridViewHandler(ListItemType.EditItem, DB.RSField(rsMSPList, "ParameterName"), "TextBox", "gridrequirequantity", requirevalidation);
                             break;

                         case "DateTime":
                             field.EditItemTemplate = new DynamicallyTemplatedGridViewHandler(ListItemType.EditItem, DB.RSField(rsMSPList, "ParameterName"), "DateTime", "gridrequirequantity", requirevalidation);
                             break;
                     }*/
                    field.EditItemTemplate = new DynamicallyTemplatedGridViewHandler(ListItemType.EditItem, DB.RSField(rsMSPList, "ParameterName"), DB.RSField(rsMSPList, "ControlType"), DB.RSField(rsMSPList, "ParameterDataType"), DB.RSField(rsMSPList, "DataSource"), DB.RSField(rsMSPList, "MinTolerance"), DB.RSField(rsMSPList, "MaxTolerance"));

                    gvPickedItemList.Columns.Add(field);


                }

                rsMSPList.Close();

                field = new TemplateField();
                field.HeaderTemplate = new DynamicallyTemplatedGridViewHandler(ListItemType.Header, "Delete", "Literal");
                field.ItemTemplate = new DynamicallyTemplatedGridViewHandler(ListItemType.Item, "Delete", "CheckBox");
                field.EditItemTemplate = new DynamicallyTemplatedGridViewHandler(ListItemType.EditItem, "Delete", "Empty");
                button = new LinkButton();
                button.ID = "deleteButton";
                button.Text = "Delete";
                button.Font.Underline = false;
                button.ForeColor = System.Drawing.Color.Blue;
                button.OnClientClick = "return confirm('Are you sure want to delete?')";
                button.Click += new EventHandler(lnkDeleteItems_click);
                field.FooterTemplate = new DynamicallyTemplatedGridViewHandler(ListItemType.Footer, button);


                gvPickedItemList.Columns.Add(field);


                BoundField editfield;
                editfield = new BoundField();
                editfield.DataField = "EditName";
                editfield.ReadOnly = true;
                editfield.Visible = false;
                gvPickedItemList.Columns.Add(editfield);

                CommandField cmdfield = new CommandField();


                cmdfield.CausesValidation = true;
                cmdfield.ButtonType = ButtonType.Link;
                cmdfield.CancelText = "Cancel";
                cmdfield.ControlStyle.Font.Underline = false;
                cmdfield.EditText = "<nobr> Edit <img src='../Images/redarrowright.gif' border='0' /></nobr>";
                cmdfield.ShowEditButton = true;
                cmdfield.ValidationGroup = "gridrequirequantity";
                cmdfield.UpdateImageUrl = "icons/update.gif";
                cmdfield.UpdateText = "Update";


                // gvPickedItemList.Columns.Add(cmdfield);

                //CommandField cmdSelectfield = new CommandField();


                //cmdSelectfield.CausesValidation = false;
                //cmdSelectfield.ButtonType = ButtonType.Link;
                ////cmdSelectfield.CancelText = "Cancel";
                //cmdSelectfield.ControlStyle.Font.Underline = false;
                //cmdSelectfield.SelectText = "<nobr> Print <img src='../Images/redarrowright.gif' border='0' /></nobr>";
                //cmdSelectfield.ShowSelectButton = true;
                ////cmdSelectfield.ValidationGroup = "gridrequirequantity";
                ////cmdSelectfield.UpdateImageUrl = "icons/update.gif";
                ////cmdSelectfield.UpdateText = "Update";

                //gvPickedItemList.Columns.Add(cmdSelectfield);

                field = new TemplateField();
                field.HeaderTemplate = new DynamicallyTemplatedGridViewHandler(ListItemType.Header, "Print", "Literal");
                field.ItemTemplate = new DynamicallyTemplatedGridViewHandler(ListItemType.Item, "Print", "LinkButton");
                field.EditItemTemplate = new DynamicallyTemplatedGridViewHandler(ListItemType.EditItem, "", "Empty");
                gvPickedItemList.Columns.Add(field);


            }
            catch (Exception ex)
            {
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
                // resetError(ex.ToString(), true);
            }
        }

        #endregion

        #region ----Grid Data----

        protected void lnkDeleteItems_click(object sender, EventArgs e)
        {

            //test case ID (TC_027)
            //Select line item to delete
            cp = HttpContext.Current.User as CustomPrincipal;
            string gvchkIDs = "";

            foreach (GridViewRow gvRow in gvPickedItemList.Rows)
            {
                if (gvRow.RowState != DataControlRowState.Edit)
                {
                    CheckBox isDeletedchkbxItem = (CheckBox)gvRow.FindControl("Delete");

                    if (isDeletedchkbxItem.Checked)
                    {


                        gvchkIDs += ((Literal)gvRow.FindControl("ltGoodsMovementDetailsID")).Text.ToString() + ",";
                    }
                }

            }
            try
            {
                int StatusID = DB.GetSqlN("SELECT DeliveryStatusID AS N FROM OBD_Outbound WHERE OutboundID=" + CommonLogic.QueryString("obdid"));
                if (StatusID >= 4 && StatusID != 5)
                {
                    //test case ID (TC_029)
                    //PGI is updated never delete

                    resetError("PGI is updated, cannot delete", true);
                    return;
                }

                int CycleCountOn = DB.GetSqlTinyInt("select qcc.IsOn as TI from QCC_CycleCount qcc left join QCC_CycleCountDetails qccd on qccd.CycleCountID=qcc.CycleCountID  where qccd.MaterialMasterID=" + CommonLogic.QueryString("mmid"));
                if (CycleCountOn == 1)
                {
                    //test case ID (TC_028)
                    //Cannot delete material in cycle count

                    resetError("This material is in 'Cycle Count', cannot delete ", true);
                    return;
                }


                String sCmdDeleteRows = "EXEC dbo.sp_INV_DeleteGoodsMovementDetails @GoodsMovementDetailsIDs=" + DB.SQuote(gvchkIDs) + ",@GoodsMovementTypeID=" + 2+ ",@UpdatedBy="+cp.UserID.ToString();
                DB.ExecuteSQL(sCmdDeleteRows);
                gvPickedItemList.EditIndex = -1;
                ViewState["gvPOQuantityList"] = "EXEC dbo.sp_INV_GetStockInList @MaterialMasterID=" + CommonLogic.QueryString("mmid") + ",@LocationID=" + CommonLogic.QueryString("locid") + ",@KitPlannerID=" + CommonLogic.QueryString("kitid") + ",@OutBoundID=" + CommonLogic.QueryString("obdid") + ",@SOHeaderID=" + CommonLogic.QueryString("soid") + ",@LineNumber=" + CommonLogic.QueryString("LineNum");
                Build_gvPOQuantityilst(Build_gvPOQuantityilst());

                ViewState["gvPickedItems"] = "EXEC dbo.sp_INV_GetStockOutList @MaterialMasterID=" + CommonLogic.QueryString("mmid") + ",@LocationID=" + CommonLogic.QueryString("locid") + ",@OutboundID=" + CommonLogic.QueryString("obdid") + ",@linenumber=" + CommonLogic.QueryString("LineNum") + ",@SOHeaderID=" + CommonLogic.QueryString("soid");
                Build_gvPicketItem(Build_gvPicketItem());
                if (gvPickedItemList.Rows.Count != 0)
                {
                    Decimal StockoutSum = DB.GetSqlNDecimal("EXEC dbo.sp_INV_GetGoodsOutQuantitySum  @MaterialMasterID=" + CommonLogic.QueryString("mmid") + ",@LocationID=" + CommonLogic.QueryString("locid") + ",@OutboundID=" + CommonLogic.QueryString("obdid") + ",@linenumber=" + CommonLogic.QueryString("LineNum") + ",@SOHeaderID=" + CommonLogic.QueryString("soid"));
                    ((Literal)gvPickedItemList.FooterRow.FindControl("ltQuantityCount")).Text = "Sum :" + StockoutSum;
                }
                resetError("Successfully deleted the selected items", false);

            }
            catch (Exception ex)
            {
                resetError("Error while deleting items", true);
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
            }


        }

        protected void gvPickedItemList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row == null)
                return;


            LinkButton lnk = (LinkButton)e.Row.FindControl("lnkPrint");

            if (lnk == null)
                return;
            lnk.Text = "Print";

            lnk.CommandName = "Print";
            lnk.CommandArgument = e.Row.RowIndex.ToString();

            //lnk.Click += new EventHandler(gvPickedItemList_SelectedIndexChanging);

            lnk.OnClientClick = "openPrintDialog();";



        }

        protected void gvPickedItemList_RowCommand(object sender, GridViewCommandEventArgs e)
        {

            if (e.CommandName == "Print")
            {
                int index = Convert.ToInt32(e.CommandArgument);
                ViewState["index"] = index;

            }

            //    if (chkIsPrintRequired.Checked)
            //    {
            //        if (ddlNetworkPrinter.SelectedValue == "0")
            //        {
            //            resetError("Please select printer", true);
            //            return;
            //        }
            //        else
            //        {
            //            int index =Convert.ToInt32(e.CommandArgument);

            //            String MfgDate = "", ExpDate = "", SerNo = "", BatchNo = "";

            //            GridViewRow gvrEditRow = gvPickedItemList.Rows[index];

            //            Control cMfgDate = gvrEditRow.FindControl("MfgDate");
            //            if (cMfgDate != null)
            //            {
            //                MfgDate = ((Literal)cMfgDate).Text.Trim().ToString();
            //            }

            //            Control cExpDate = gvrEditRow.FindControl("ExpDate");
            //            if (cExpDate != null)
            //            {
            //                ExpDate = ((Literal)cExpDate).Text.Trim().ToString();
            //            }

            //            Control cSerNo = gvrEditRow.FindControl("SerialNo");
            //            if (cSerNo != null)
            //            {
            //                SerNo = ((Literal)cSerNo).Text.Trim().ToString();
            //            }

            //            Control cBatchNo = gvrEditRow.FindControl("BatchNo");
            //            if (cBatchNo != null)
            //            {
            //                BatchNo = ((Literal)cBatchNo).Text.Trim().ToString();
            //            }

            //            String IsPositiveRecall = ((Literal)gvrEditRow.FindControl("ltIsPositiveRecall")).Text.Trim().ToString();

            //            String pickQuantity = ((Literal)gvrEditRow.FindControl("ltQuantity")).Text.Trim().ToString();

            //            PrintLabel((IsPositiveRecall == "1" ? ltMaterialCode.Text + "-P" : ltMaterialCode.Text), CommonLogic.QueryString("mmid"), ltDescription.Text, CommonLogic.QueryString("obdid"), "", "", pickQuantity, ddlNetworkPrinter.SelectedValue, MfgDate, ExpDate, SerNo, BatchNo);
            //        }
            //    }
            //    else
            //    {
            //        resetError("Please check 'Is Print Required' checkbox", true);
            //        return;
            //    }


            //}


        }

        protected void gvPickedItemList_RowEditing(object sender, GridViewEditEventArgs e)
        {
            cp = HttpContext.Current.User as CustomPrincipal;
            gvPickedItemList.EditIndex = e.NewEditIndex;
            List<String> MSPlist = new List<String>();
            IDataReader rsMSPList = null;
            try
            {
                rsMSPList = DB.GetRS("sp_INV_GetMaterialStorageParameters @MCode=" + DB.SQuote(CommonLogic.QueryString("MCode")));
            }
            catch (Exception ex)
            {
                resetError("Error while loading", true);
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
                return;
            }
            while (rsMSPList.Read())
            {

                MSPlist.Add(((Literal)gvPickedItemList.Rows[e.NewEditIndex].FindControl(DB.RSField(rsMSPList, "ParameterName"))).Text);
            }
            rsMSPList.Close();

            gvPickedItemList.EditIndex = e.NewEditIndex;
            ViewState["gvPickedItems"] = "EXEC dbo.sp_INV_GetStockOutList @MCode=" + DB.SQuote(CommonLogic.QueryString("MCode")) + ",@Location=" + DB.SQuote(CommonLogic.QueryString("location"));
            Build_gvPicketItem(Build_gvPicketItem());

            GridViewRow gvrEditRow = gvPickedItemList.Rows[e.NewEditIndex];


            rsMSPList = DB.GetRS("sp_INV_GetMaterialStorageParameters @MCode=" + DB.SQuote(CommonLogic.QueryString("MCode")));

            String MSPid = "";
            int position = 0;
            while (rsMSPList.Read())
            {
                if (DB.RSField(rsMSPList, "ControlType") == "DropDownList")
                {
                    MSPid = "ddl" + DB.RSField(rsMSPList, "ParameterName");
                    DropDownList dd_dynamic = ((DropDownList)gvrEditRow.FindControl(MSPid));
                    //dd_dynamic.SelectedIndex = dd_dynamic.Items.IndexOf(dd_dynamic.Items.FindByText(list[position++]));
                    dd_dynamic.SelectedValue = MSPlist[position++];

                }
                else
                {

                    MSPid = "txt" + DB.RSField(rsMSPList, "ParameterName");
                    ((TextBox)gvrEditRow.FindControl(MSPid)).Text = MSPlist[position++];
                }

            }
            //ltPickStatus.Text = "";
        }

        protected void gvPickedItemList_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            cp = HttpContext.Current.User as CustomPrincipal;
            GridViewRow gvrEditRow = gvPickedItemList.Rows[e.RowIndex];
            if (gvrEditRow != null)
            {

                String GoodsMovementDetailsID = ((Literal)gvrEditRow.FindControl("ltGoodsMovementDetailsID_Edit")).Text.Trim();
                String Location = ((TextBox)gvrEditRow.FindControl("txtLocation")).Text.Trim();
                String Quantity = ((TextBox)gvrEditRow.FindControl("txtQuantity")).Text.Trim();
                String IsDamaged = Convert.ToInt16(((CheckBox)gvrEditRow.FindControl("chkDamaged")).Checked).ToString();
                String OBDnumber = CommonLogic.QueryString("OBDNum");
                String LineNumber = CommonLogic.QueryString("LineNum");
                String MCode = CommonLogic.QueryString("MCode");
                String POHeaderID = CommonLogic.QueryString("location");



                int Locationid = DB.GetSqlN("select LocationID as N from INV_Location where IsActive=1 and IsDeleted=0 and Location=" + DB.SQuote(Location));
                if (Locationid == 0)
                {
                    // resetError("Not vallid Putaway check once", true);
                    return;
                }
                try
                {
                    /* Decimal QuantitySum = DB.GetSqlNDecimal("sp_INV_GetGoodsMovementQuantitySum @MCode=" + DB.SQuote(txtin_MaterialCode.Text.Trim()) + ",@StoreRefNo=" + DB.SQuote(txtin_Storefno.Text.Trim()) + ",@LineNumber=" + txtin_poitemline.Text.Trim() + ",@POHeaderID=" + hifPONumber.Value);
                     Decimal TotalQuantity = QuantitySum + Convert.ToDecimal(Quantity);

                     Decimal invoiceQuantity;
                     if (ltInvqtyvalue.Text != "")
                     {
                         invoiceQuantity = Convert.ToDecimal(ltInvqtyvalue.Text);
                     }
                     else
                     {
                         invoiceQuantity = 0;
                     }
                     if (TotalQuantity > invoiceQuantity)
                     {
                         resetError("Quantity is more then the invoice quantity", true);
                     }
                     */

                    IDataReader drMatrialMsp = DB.GetRS("sp_INV_GetMaterialStorageParameters @MCode=" + DB.SQuote(CommonLogic.QueryString("MCode")));
                    String MSPIDs = "";
                    String MSPValues = "";
                    while (drMatrialMsp.Read())
                    {
                        String ControlType = DB.RSField(drMatrialMsp, "ControlType");
                        String ControlID = "";
                        String value = "";
                        if (ControlType == "DropDownList")
                        {
                            ControlID = "ddl" + DB.RSField(drMatrialMsp, "ParameterName");
                            DropDownList ddlDropdown = ((DropDownList)gvrEditRow.FindControl(ControlID));
                            if (ddlDropdown.Visible)
                                value = ddlDropdown.SelectedValue;

                        }
                        else
                        {
                            ControlID = "txt" + DB.RSField(drMatrialMsp, "ParameterName");
                            TextBox txtTextBox = ((TextBox)gvrEditRow.FindControl(ControlID));
                            if (txtTextBox.Visible)
                                value = txtTextBox.Text.Trim();
                        }

                        if (value != "")
                        {
                            MSPIDs += DB.RSFieldInt(drMatrialMsp, "MaterialStorageParameterID") + ",";
                            MSPValues += value + ",";
                        }
                    }
                    drMatrialMsp.Close();

                    //Decimal stockInSum = DB.GetSqlNDecimal("sp_INV_GetGoodsInQuantitySum @MCode=" + DB.SQuote(CommonLogic.QueryString("MCode")) + ",@StoreRefNo=" + DB.SQuote("") + ",@LineNumber=" + 0 + ",@POHeaderID=" + 0);
                    // Decimal StockoutSum = DB.GetSqlNDecimal("EXEC dbo.sp_INV_GetGoodsOutQuantitySum @MCode=" + DB.SQuote(CommonLogic.QueryString("MCode")) + ",@Location=" + DB.SQuote(CommonLogic.QueryString("location")) + ",@MaterialStorageParameterIDs=" + DB.SQuote(MSPIDs) + ",@MaterialStorageParameterValues=" + DB.SQuote(MSPValues));
                    // Decimal pickQuantity = Convert.ToDecimal(txtPickQty.Text.Trim());
                    // if (StockoutSum < pickQuantity)
                    // {
                    //     resetError("Pick quantity is more than available quantity", true);
                    //     return;
                    //  }
                    //InventoryCommonClass.UpdateGoodsMovementDetails(GoodsMovementDetailsID, "2", Location != "" ? Location : "NULL", Quantity != "" ? Quantity : "NULL", OBDnumber, LineNumber, MCode, IsDamaged.ToString(), "0", POHeaderID, "1", MSPIDs, MSPValues, "", "", cp.UserID.ToString(),"0",ltKitID.Text);


                    gvPickedItemList.EditIndex = -1;
                    ViewState["gvPickedItems"] = "EXEC dbo.sp_INV_GetStockOutList @MCode=" + DB.SQuote(CommonLogic.QueryString("MCode")) + ",@Location=" + DB.SQuote(CommonLogic.QueryString("location"));
                    Build_gvPicketItem(Build_gvPicketItem());

                    // QuantitySum = DB.GetSqlNDecimal("sp_INV_GetGoodsMovementQuantitySum @MCode=" + DB.SQuote(txtin_MaterialCode.Text.Trim()) + ",@StoreRefNo=" + DB.SQuote(txtin_Storefno.Text.Trim()) + ",@LineNumber=" + txtin_poitemline.Text.Trim() + ",@POHeaderID=" + hifPONumber.Value);

                    ((Literal)gvPOQuantityList.FooterRow.FindControl("ltQuantityCount")).Text = "  Sum : ";// +QuantitySum;

                    resetError("Successfully Updated", false);
                }
                catch (Exception ex)
                {
                    resetError("Error while updating", true);
                    CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
                }


            }


        }

        protected void gvPickedItemList_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            cp = HttpContext.Current.User as CustomPrincipal;
            try
            {
                gvPickedItemList.EditIndex = -1;
                ViewState["gvPickedItems"] = "EXEC dbo.sp_INV_GetStockOutList @MCode=" + DB.SQuote(CommonLogic.QueryString("MCode")) + ",@Location=" + DB.SQuote(CommonLogic.QueryString("location"));
                Build_gvPicketItem(Build_gvPicketItem());
                //ltPickStatus.Text = "";
            }
            catch (Exception ex)
            {
                resetError("Error try again", true);
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
            }
        }

        protected void gvPickedItemList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            cp = HttpContext.Current.User as CustomPrincipal;
            try
            {
                gvPickedItemList.PageIndex = e.NewPageIndex;
                gvPickedItemList.EditIndex = -1;
                ViewState["gvPickedItems"] = "EXEC dbo.sp_INV_GetStockOutList @MCode=" + DB.SQuote(CommonLogic.QueryString("MCode")) + ",@Location=" + DB.SQuote(CommonLogic.QueryString("location"));
                Build_gvPicketItem(Build_gvPicketItem());
                //ltPickStatus.Text = "";
            }
            catch (Exception ex)
            {
                resetError("Error while change pageindex", true);
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
            }
        }

        protected void gvPickedItemList_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
        {

            //resetError("Please check 'Is Print Required' checkbox", true);

            //if (chkIsPrintRequired.Checked)
            //{
            //    if (ddlNetworkPrinter.SelectedValue == "0")
            //    {
            //        resetError("Please select printer", true);
            //        return;
            //    }
            //    else
            //    {

            // int index = ((GridViewRow)((LinkButton)sender).Parent.Parent).RowIndex;

            //int index = e.NewSelectedIndex;

            // ViewState["index"] = index;

            //String MfgDate = "", ExpDate = "", SerNo = "", BatchNo = "";

            //GridViewRow gvrEditRow = gvPickedItemList.Rows[index];

            //Control cMfgDate = gvrEditRow.FindControl("MfgDate");
            //if (cMfgDate != null)
            //{
            //    MfgDate = ((Literal)cMfgDate).Text.Trim().ToString();
            //}

            //Control cExpDate = gvrEditRow.FindControl("ExpDate");
            //if (cExpDate != null)
            //{
            //    ExpDate = ((Literal)cExpDate).Text.Trim().ToString();
            //}

            //Control cSerNo = gvrEditRow.FindControl("SerialNo");
            //if (cSerNo != null)
            //{
            //    SerNo = ((Literal)cSerNo).Text.Trim().ToString();
            //}

            //Control cBatchNo = gvrEditRow.FindControl("BatchNo");
            //if (cBatchNo != null)
            //{
            //    BatchNo = ((Literal)cBatchNo).Text.Trim().ToString();
            //}

            //String IsPositiveRecall = ((Literal)gvrEditRow.FindControl("ltIsPositiveRecall")).Text.Trim().ToString();

            //String pickQuantity = ((Literal)gvrEditRow.FindControl("ltQuantity")).Text.Trim().ToString();

            //PrintLabel((IsPositiveRecall == "1" ? ltMaterialCode.Text + "-P" : ltMaterialCode.Text), CommonLogic.QueryString("mmid"), ltDescription.Text, CommonLogic.QueryString("obdid"), "", "", pickQuantity, ddlNetworkPrinter.SelectedValue, MfgDate, ExpDate, SerNo, BatchNo);
            //    }
            //}
            //else
            //{
            //    resetError("Please check 'Is Print Required' checkbox", true);
            //    return;
            //}

        }

        protected void gvPOQuantityList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row == null)
                return;

            if (e.Row.RowType == DataControlRowType.DataRow && !(e.Row.RowState == DataControlRowState.Edit))
            {
                String SameWithSODetails = ((DataRowView)e.Row.DataItem).Row["SameWithSODetails"].ToString();
                String RefLocation = ((DataRowView)e.Row.DataItem).Row["RefLocation"].ToString();
                if (SameWithSODetails == "0")
                {
                    ((RadioButton)e.Row.FindControl("SelectOne")).Enabled = false;
                    foreach (Control control in e.Row.Controls)
                    {
                        if (e.Row.Controls[0] != control)
                        {
                            DataControlFieldCell td = (DataControlFieldCell)control;
                            td.CssClass = "strikeout";
                        }
                    }
                }
                if (RefLocation == "0")
                {
                    ((RadioButton)e.Row.FindControl("SelectOne")).Enabled = false;
                    ((DataControlFieldCell)e.Row.Controls[0]).CssClass = "strikeout";
                }

            }
        }

        protected void gvPOQuantityList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvPOQuantityList.PageIndex = e.NewPageIndex;
            ViewState["gvPOQuantityList"] = "";
            Build_gvPOQuantityilst(Build_gvPOQuantityilst());
        }

        //build gridview
        private void Build_gvPOQuantityilst(DataSet dsPOQuantityList)
        {
            gvPOQuantityList.DataSource = dsPOQuantityList.Tables[0];
            gvPOQuantityList.DataBind();
            dsPOQuantityList.Dispose();
        }

        //get dataset for grid
        private DataSet Build_gvPOQuantityilst()
        {
            cp = HttpContext.Current.User as CustomPrincipal;
            DataSet dsPOQuantity = null;
            String sCmdPOQuantitylist = ViewState["gvPOQuantityList"].ToString();
            try
            {
                dsPOQuantity = DB.GetDS(sCmdPOQuantitylist, false);
            }
            catch (Exception ex)
            {
                resetError("Error while loading", true);
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
            }
            return dsPOQuantity;
        }

        private DataSet Build_gvPicketItem()
        {
            cp = HttpContext.Current.User as CustomPrincipal;
            DataSet dsPickedItems = null;
            String sCmdPickedItems = ViewState["gvPickedItems"].ToString();
            try
            {
                dsPickedItems = DB.GetDS(sCmdPickedItems, false);
            }
            catch (Exception ex)
            {
                resetError("Error while loading", true);
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
            }
            return dsPickedItems;

        }

        private void Build_gvPicketItem(DataSet dspickedItems)
        {
            gvPickedItemList.DataSource = dspickedItems.Tables[0];
            gvPickedItemList.DataBind();
            dspickedItems.Dispose();
        }

        #endregion

        private void resetError(string error, bool isError)
        {

            /*string str = "<font class=\"noticeMsg\">NOTICE:</font>&nbsp;&nbsp;&nbsp;";
            if (isError)
                str = "<font class=\"errorMsg\">ERROR:</font>&nbsp;&nbsp;&nbsp;";

            if (error.Length > 0)
                str += error + "";
            else
                str = "";


            ltPickStatus.Text = str;*/
            ScriptManager.RegisterStartupScript(this, this.GetType(), "test", "closeAsynchronus(); showStickyToast(" + (isError.ToString().ToLower() == "true" ? "false" : "true") + ",\"" + error + "\");", true);

        }

        #region ----Print labels----

        public void LoadStoresWithIPs(DropDownList ddlWH)
        {

            ddlWH.Items.Clear();

            ddlWH.Items.Add(new ListItem("Select Printer", "0"));
            ddlWH.Items.Add(new ListItem("Data Max - LP", "172.18.0.127"));
            ddlWH.Items.Add(new ListItem("Zebraa - LP", "172.18.0.53"));

        }

        public void PrintLabel(String MCode, String MMID, String Description, String OBDID, String MspIDs, String MspValues, String Qty, String PrinterIP, String MfgDate, String ExpDate, String SerNo, String BatchNo, String PrintQty)
        {
            cp = HttpContext.Current.User as CustomPrincipal;
            try
            {

                MRLWMSC21Common.TracklineMLabel thisMLabel = new MRLWMSC21Common.TracklineMLabel();

                thisMLabel.MCode = MCode;
                thisMLabel.OEMPartNo = DB.GetSqlS("select OEMPartNo AS S from MMT_MaterialMaster where IsActive=1 AND IsDeleted=0 AND MaterialMasterID=" + MMID);
                thisMLabel.Description = Description;
                thisMLabel.AltMCode = ltSUoMQty.Text;


                if (Qty != "")
                    thisMLabel.InvQty = Convert.ToInt32(Convert.ToDecimal(Qty));
                else
                    Qty = "0.00";

                if (Convert.ToInt32(Convert.ToDecimal(Qty)) < 1)
                    thisMLabel.InvQty = 1;


                //thisMLabel.InvQty = 1;

                thisMLabel.PrintQty = PrintQty;

                string ddl = ddlLabelSize.SelectedValue;
                string Length = "";
                string width = "";
                string LabelType = "";
                string query = "select * from TPL_Tenant_BarcodeType where IsActive=1 and IsDeleted=0 and TenantBarcodeTypeID=" + ddl;
                DataSet DS = DB.GetDS(query, false);
                if (DS.Tables[0].Rows.Count != 0)
                {
                    foreach (DataRow row in DS.Tables[0].Rows)
                    {
                        Length = row["Length"].ToString();
                        width = row["Width"].ToString();
                        LabelType = row["LabelType"].ToString();
                    }
                }
             


                string vltMfgDate = "";
                string vltExpiryDate = "";
                string vltBatchNo = "";
                string vltSerialNo = "";

                if (MfgDate != "" || ExpDate != "" || SerNo != "" || BatchNo != "")
                {
                    vltMfgDate = MfgDate;
                    vltExpiryDate = ExpDate;
                    vltSerialNo = SerNo;
                    vltBatchNo = BatchNo;
                }


                IDataReader drMsps = DB.GetRS("[dbo].[sp_INV_GetMspValues] @MaterialStorageParameterIDs='" + MspIDs + "',@MaterialStorageParameterValues='" + MspValues + "'");

                while (drMsps.Read())
                {

                    switch (DB.RSFieldInt(drMsps, "MSPID"))
                    {
                        case 1:
                            vltMfgDate = DB.RSField(drMsps, "Value");
                            break;

                        case 2:
                            vltExpiryDate = DB.RSField(drMsps, "Value");
                            break;

                        case 3:
                            vltSerialNo = DB.RSField(drMsps, "Value");
                            break;

                        case 4:
                            vltBatchNo = DB.RSField(drMsps, "Value");
                            break;
                    }
                }

                drMsps.Close();

                if (vltMfgDate != "")
                {
                    thisMLabel.MfgDate = vltMfgDate;
                }
                else
                {
                    thisMLabel.MfgDate = "";
                }
                if (vltExpiryDate != "")
                {
                    thisMLabel.ExpDate = vltExpiryDate;
                }
                else
                {
                    thisMLabel.ExpDate = "";
                }


                thisMLabel.BatchNo = vltBatchNo;
                thisMLabel.SerialNo = vltSerialNo;

                //thisMLabel.InvQty = Convert.ToDecimal(Qty);

                thisMLabel.PrinterType = "IP";
                thisMLabel.PrinterIP = ddlNetworkPrinter.SelectedValue.Trim();
                thisMLabel.IsBoxLabelReq = false;
                thisMLabel.ReqNo = "";

                thisMLabel.StrRefNo = "";
                thisMLabel.OBDNumber = DB.GetSqlS("select OBDNumber AS S from OBD_Outbound where IsActive=1 AND IsDeleted=0 AND OutboundID=" + OBDID);

                thisMLabel.LabelType = LabelType;

                thisMLabel.Length = Length;
                thisMLabel.Width = width;
                int dpi = 0;
                dpi = CommonLogic.GETDPI(thisMLabel.PrinterIP);
                thisMLabel.Dpi = dpi;


                thisMLabel.KitCode = CommonLogic.QueryString("kitcode");
                //For Printing Label Using ZPL Added by Prasanna on 21st august 2017
                //CommonLogic.PrintLabel(thisMLabel);
                MRLWMSC21Common.PrintCommon.CommonPrint print = new MRLWMSC21Common.PrintCommon.CommonPrint();
                print.PrintBarcodeLabel(thisMLabel);



                //Commented  by Prasanna on 21st august 2017
                // ParameterizedThreadStart tThermalLabelWorker = new ParameterizedThreadStart(this.ThermalLabelWorker);
                // Thread worker = new Thread(new ParameterizedThreadStart(this.ThermalLabelWorker));


                //Thread worker = new Thread(this.ThermalLabelWorker);
                //Thread worker = new Thread(new ParameterizedThreadStart(this.ThermalLabelWorker));
                //worker.SetApartmentState(ApartmentState.STA);
                //worker.Name = "ThermalLabelWorker";
                //worker.Start(thisMLabel);
                //worker.Join();

                ////CommonLogic.SendPrintJob(DB.RSField(rsLineItem, "MCode"), DB.RSField(rsLineItem, "AlternativeMCode"), DB.RSField(rsLineItem, "MDescription"), DB.RSField(rsLineItem, "BatchNo"),"",DB.RSFieldInt(rsLineItem, "KitPlannerID").ToString(), Convert.ToInt32(DB.RSFieldDecimal(rsLineItem, "InvoiceQuantity")) + 1, DateTime.MinValue, DB.RSFieldDateTime(rsLineItem, "ExpiryDate"), "IP", ddlNetworkPrinter.SelectedValue.Trim(), false, out result);
                //if (thisMLabel.Result != "Success")
                //{
                //    resetError("Error while printing. Please contact admin<br/>" + thisMLabel.Result, true);
                //    return;
                //}
                //else
                //{
                //    resetError("Successfully printed selected line items", false);
                //}



            }
            catch (Exception ex)
            {
                resetError("Error while printing", true);
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
                return;
            }
        }

        private void ThermalLabelWorker(object thisMLabel2)
        {
            cp = HttpContext.Current.User as CustomPrincipal;
            try
            {
                MRLWMSC21Common.TracklineMLabel thisMLabel = (MRLWMSC21Common.TracklineMLabel)thisMLabel2;
                String vresult = "";
               // CommonLogic.SendPrintJob_Big_7p6x5(thisMLabel.MCode, thisMLabel.AltMCode, thisMLabel.OEMPartNo, thisMLabel.Description, thisMLabel.BatchNo, thisMLabel.SerialNo, thisMLabel.KitPlannerID.ToString(), thisMLabel.KitChildrenCount, thisMLabel.ParentMCode, thisMLabel.InvQty, thisMLabel.MfgDate, thisMLabel.ExpDate, thisMLabel.PrinterType, thisMLabel.PrinterIP, thisMLabel.StrRefNo, thisMLabel.OBDNumber, thisMLabel.KitCode, thisMLabel.ReqNo, thisMLabel.IsBoxLabelReq, true, thisMLabel.PrintQty, out vresult);
                thisMLabel.Result = vresult;
            }
            catch (Exception ex)
            {
                resetError("Error while printing", true);
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
                return;
            }
        }

        protected void lnkPrintSubmit_Click(object sender, EventArgs e)
        {
            cp = HttpContext.Current.User as CustomPrincipal;
            try
            {
                if (chkIsPrintRequired.Checked)
                {
                    if (ddlNetworkPrinter.SelectedValue == "0")
                    {
                        resetError("Please select printer", true);
                        return;
                    }
                    else
                    {

                        String MfgDate = "", ExpDate = "", SerNo = "", BatchNo = "", PrintQty = "";

                        GridViewRow gvrEditRow = gvPickedItemList.Rows[Convert.ToInt32(ViewState["index"])];

                        Control cMfgDate = gvrEditRow.FindControl("MfgDate");
                        if (cMfgDate != null)
                        {
                            MfgDate = ((Literal)cMfgDate).Text.Trim().ToString();
                        }

                        Control cExpDate = gvrEditRow.FindControl("ExpDate");
                        if (cExpDate != null)
                        {
                            ExpDate = ((Literal)cExpDate).Text.Trim().ToString();
                        }

                        Control cSerNo = gvrEditRow.FindControl("SerialNo");
                        if (cSerNo != null)
                        {
                            SerNo = ((Literal)cSerNo).Text.Trim().ToString();
                        }

                        Control cBatchNo = gvrEditRow.FindControl("BatchNo");
                        if (cBatchNo != null)
                        {
                            BatchNo = ((Literal)cBatchNo).Text.Trim().ToString();
                        }
                      

                        Control cPrintQty = gvrEditRow.FindControl("ltQuantity");
                        if (cPrintQty != null)
                        {
                            PrintQty = ((Literal)cPrintQty).Text.Trim().ToString();
                        }



                        String IsPositiveRecall = ((Literal)gvrEditRow.FindControl("ltIsPositiveRecall")).Text.Trim().ToString();

                        String pickQuantity = Convert.ToString(Convert.ToInt32(txtPrintQty.Text));  //((Literal)gvrEditRow.FindControl("ltQuantity")).Text.Trim().ToString();

                        PrintLabel((IsPositiveRecall == "1" ? ltMaterialCode.Text + "-P" : ltMaterialCode.Text), CommonLogic.QueryString("mmid"), ltDescription.Text, CommonLogic.QueryString("obdid"), "", "", pickQuantity, ddlNetworkPrinter.SelectedValue, MfgDate, ExpDate, SerNo, BatchNo, PrintQty);
                    }
                }
                else
                {
                    resetError("Please check 'Is Print Required' checkbox", true);
                    return;
                }




                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "dialog", "closePrintDialog()", true);
            }
            catch (Exception ex)
            {
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);

            }
        }
        #endregion
    }
}