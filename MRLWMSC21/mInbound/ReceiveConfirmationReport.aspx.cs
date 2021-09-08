using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MRLWMSC21Common;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Security.Principal;
using System.Threading;
using System.Drawing;
using System.Collections;
using System.Globalization;

namespace MRLWMSC21.mInbound
{
    public partial class ReceiveConfirmationReport : System.Web.UI.Page
    {
        public CustomPrincipal cp = HttpContext.Current.User as CustomPrincipal;
        protected String POQuantityListSQL = "";
        InboundTrack IBTrack;
        int prmInboundTrackingID = 0;
        private GridViewHelper helper;


        protected void PagePre_Init(object sender, EventArgs e)
        {

            Page.Theme = "Inbound";

        }

        protected void Page_Load(object sender, EventArgs e)
        {


            prmInboundTrackingID = DB.GetSqlN("Select top 1 InboundID AS N from INB_Inbound where IsActive=1 and IsDeleted=0 AND InboundID=" + CommonLogic.QueryStringUSInt("ibdid"));

            if (!IsPostBack)
            {
                //AddDynamicMsp(Convert.ToUInt16(CommonLogic.QueryString("ibdid")));

                DesignLogic.SetInnerPageSubHeading(this.Page, "Receipt Confirmation Report");



                if (prmInboundTrackingID != 0)
                {
                    ViewState["InboundID"] = prmInboundTrackingID;

                    CommonLogic.LoadPrinters(ddlNetworkPrinter);

                    //if (cp.TenantID != 0)
                    //{
                    //    gvPOLineQty.Columns[9].Visible = false;
                    //}

                    IBTrack = new InboundTrack(prmInboundTrackingID);

                    if (IBTrack.ShipmentTypeID == 4)
                        lblHeader.Text = "Returns Receiving Tally Report";

                    ltbarStoreRefNo.Text = IBTrack.StoreRefNumber;
                    if (IBTrack.ReferedStoreIDs != "" && IBTrack.ReferedStoreIDs != null)
                    {
                        ltbarWarehouse.Text = CommonLogic.GetStoreNames(IBTrack.ReferedStoreIDs, ",");
                    }
                    else
                    {
                        ltbarWarehouse.Text = "";
                    }
                    ltSupplier.Text = IBTrack.SupplierName;
                    ltAWBBLNo.Text = IBTrack.ConsignmentNoteTypeValue;
                    ltNoofPackages.Text = IBTrack.NoofPackagesInDocument.ToString();
                    //  ltWeight.Text = IBTrack.GrossWeight;
                    ltDocDate.Text = IBTrack.DocReceivedDate;
                    ltTenant.Text = CommonLogic.QueryString("TN");
                    // ltStoreIncharge.Text = cp.FirstName + " " + cp.LastName;

                    txtMcode.Attributes["onclick"] = "javascript:this.focus();";
                    txtMcode.Attributes["onfocus"] = "javascript:this.select();";

                    ViewState["IsMSPAvailable"] = false;
                    LoadHeaders();
                }
            }

            if (cp.TenantID != 0)
            {
                gvPOLineQty.Columns[9].Visible = false;
            }

            string _sStatuId = DB.GetDS("SELECT InboundStatusID FROM INB_Inbound WHERE InboundID =" + CommonLogic.QueryString("ibdid"), false).Tables[0].Rows[0][0].ToString();
            hdnStatus.Value = _sStatuId;

            POQuantityListSQL = "EXEC [sp_INB_ReceivingConfirmationReport]  @InboundID=" + prmInboundTrackingID + ",@MCode=null";
            ViewState["POQuantityListSQL"] = POQuantityListSQL;
            AddDynamicMsp(Convert.ToUInt16(CommonLogic.QueryString("ibdid")));
            this.POQuantityList_buildGridData(this.POQuantityList_buildGridData());
            //helper = new GridViewHelper(this.gvPOLineQty, false);
            //ConfigSample();

        }

        private void LoadHeaders()
        {
            DataSet ds = DB.GetDS("EXEC [sp_INB_GetReceiptConfirmationReport_Header] @InboundID=" + CommonLogic.QueryString("ibdid"), false);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                //if (ds.Tables[0].Rows[0]["ShipmentVerifiedOn"].ToString() != "")
                //    LtReceiptDateRCR.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["ShipmentVerifiedOn"].ToString()).ToString("dd/MM/yyyy hh:mm tt");
                LtCustomerName.Text =  ds.Tables[0].Rows[0]["TenantName"].ToString();
                //LtTruckOrTrailerIDRCR.Text = ds.Tables[0].Rows[0]["EquipmentName"].ToString();

                LtAddress.Text = ds.Tables[0].Rows[0]["Address1"].ToString() + ", " + ds.Tables[0].Rows[0]["Address2"].ToString() + ", <br>&nbsp;&nbsp;&nbsp;" +
                    ds.Tables[0].Rows[0]["City"].ToString() + "," + ds.Tables[0].Rows[0]["State"].ToString() + ", " + ds.Tables[0].Rows[0]["Zip"].ToString() + ".";
                LtSupplierRCR.Text = ds.Tables[0].Rows[0]["SupplierCode"].ToString() + " - " + ds.Tables[0].Rows[0]["SupplierName"].ToString();
                LtShipmentType.Text = ds.Tables[0].Rows[0]["ShipmentType"].ToString();
                LtTruckOrTrailerIDRCR.Text = ds.Tables[0].Rows[0]["VehicleRegistrationNo"].ToString();

            }
        }
        private void ConfigSample()
        {
            //string[] sortColumns = { "KitPlannerID", "ParentMcode" };
            //helper.RegisterGroup(sortColumns, true, false);
            //helper.GroupHeader += new GroupEvent(helper_GroupHeader);
            /////helper.RegisterGroup("LineNumber", true, false);
            //helper.ApplyGroupSort();
        }

        private void helper_GroupHeader(string groupName, object[] values, GridViewRow row)
        {
            row.BackColor = Color.FromArgb(236, 236, 246);
            string seprator = "&nbsp; &nbsp; &nbsp; | &nbsp; &nbsp; &nbsp;";
            row.Cells[0].Text = "<span class='BarCodetext' >" + CommonLogic.IIF(values[0].ToString() != "", "Kit ID : " + values[0].ToString(), "") + CommonLogic.IIF(values[1].ToString() != "", seprator + "Parent Material : " + values[1].ToString(), "");


        }

        public String GetKitPlannetID(String GetKitPlannetID)
        {
            return CommonLogic.IIF(GetKitPlannetID != "", "<b>Kit ID : " + GetKitPlannetID.PadLeft(3, '0') + "</b>", "");
        }

        public String DisplayLocation(String Locations, String sep)
        {
            return Locations.Replace(",", sep);
        }

        public void LoadStoresWithIPs(DropDownList ddlWH, String StoreIDValues)
        {

            ddlWH.Items.Clear();


            IDataReader rsWH = DB.GetRS("Select WHCode,Location from GEN_Warehouse Where IsDeleted=0 and IsActive =1 AND InoutID <> 2  AND WarehouseID IN (Select value from udf_Split(''+" + DB.SQuote(StoreIDValues) + "+ '',','))");

            String StoreVal = "";
            while (rsWH.Read())
            {
                if (rsWH["Location"].ToString() != "")
                {
                    StoreVal = rsWH["WHCode"].ToString() + "[" + rsWH["Location"].ToString() + "]";
                }
                else
                {
                    StoreVal = rsWH["WHCode"].ToString();
                }


                ddlWH.Items.Add(new ListItem(StoreVal, "172.18.0.127"));

            }
            rsWH.Close();

        }

        protected void lnkSearchMaterial_Click(object sender, EventArgs e)
        {

            if (txtMcode.Text == "Search Part# ...")
                txtMcode.Text = "";

            //if (txtMcode.Text.Trim() != "")
            //{
            POQuantityListSQL = "EXEC [sp_INB_ReceivingConfirmationReport]  @InboundID=" + prmInboundTrackingID + ",@MCode=" + (txtMcode.Text.Trim().Equals("") ? "null" : DB.SQuote(txtMcode.Text.Trim()));
            ViewState["POQuantityListSQL"] = POQuantityListSQL;
            this.POQuantityList_buildGridData(this.POQuantityList_buildGridData());

            //}
            //else
            //{
            //    POQuantityListSQL = "EXEC [sp_INB_ReceivingTallyReport]  @InboundID=" + prmInboundTrackingID + ",@MCode=''";
            //    ViewState["POQuantityListSQL"] = POQuantityListSQL;
            //    AddDynamicMsp(Convert.ToUInt16(CommonLogic.QueryString("ibdid")));
            //    this.POQuantityList_buildGridData(this.POQuantityList_buildGridData());

            //}
            //helper = new GridViewHelper(this.gvPOLineQty, false);
            //ConfigSample();

        }

        public static MRLWMSC21Common.TracklineMLabel thisMLabel = new MRLWMSC21Common.TracklineMLabel();

        protected void lnkPrintBarCodeLabel_Click(object sender, EventArgs e)
        {

            if (ddlNetworkPrinter.SelectedValue == "0")
            {
                resetError("Please select a printer", true);
                return;
            }

            String vMCode = "";
            //Print Barcode  the selected Linte Items
            try
            {

                //string[] gvMCode_POs ;

                ArrayList gvMCode_POs = new ArrayList();

                if (CommonLogic.QueryString("ibdid") != "")
                {
                    IBTrack = new InboundTrack(CommonLogic.QueryStringUSInt("ibdid"));
                }

                bool chkBox = false;
                //'Navigate through each row in the GridView for checkbox items
                foreach (GridViewRow gv in gvPOLineQty.Rows)
                {
                    CheckBox isPrint = (CheckBox)gv.FindControl("chkIsPrint");
                    if (isPrint.Checked)
                    {
                        chkBox = true;
                        //  Concatenate GridView items with comma for SQL Delete
                        //gvMCode_POs.Add(((Literal)gv.FindControl("ltLineNumber")).Text.ToString() + "|" + ((Label)gv.FindControl("lblMCode")).Text.ToString() + "|" + ((Literal)gv.FindControl("ltPONumber")).Text.ToString());

                        //String vltLineNumber = ((Literal)gv.FindControl("ltLineNumber")).Text.ToString();
                        vMCode = ((Label)gv.FindControl("lblMCode")).Text.ToString();
                        String vltKitID = ((Literal)gv.FindControl("ltKitID")).Text.ToString();

                        String vltAlternativeCode = ((Literal)gv.FindControl("ltAlternativeCode")).Text.ToString();
                        String vOEMPartNo = ((Literal)gv.FindControl("ltOEMPartNo")).Text.ToString();

                        String vltBatchNo = "";// ((Literal)gv.FindControl("ltBatchNo")).Text.ToString();
                        String vltMfgDate = "";// ((Literal)gv.FindControl("ltMfgDate")).Text.ToString();
                        String vltExpiryDate = "";// ((Literal)gv.FindControl("ltExpiryDate")).Text.ToString();

                        String vltInvQuantity = ((Literal)gv.FindControl("ltInvQuantity")).Text.ToString();

                        String vltItemDesc = ((Literal)gv.FindControl("ltItemDesc")).Text.ToString();

                        String vltSerialNo = "";// ((Literal)gv.FindControl("ltSerialNo")).Text.ToString();
                        String vltKitChildCount = ((Literal)gv.FindControl("ltKitChildCount")).Text.ToString();
                        // String vltParentMcode = ((Literal)gv.FindControl("ltParentMcode")).Text.ToString();



                        thisMLabel.MCode = vMCode;
                        thisMLabel.OEMPartNo = vOEMPartNo;
                        thisMLabel.AltMCode = vltAlternativeCode;
                        thisMLabel.Description = vltItemDesc;
                        thisMLabel.BatchNo = vltBatchNo;
                        thisMLabel.SerialNo = vltSerialNo;
                        thisMLabel.KitPlannerID = Convert.ToInt32(CommonLogic.IIF(vltKitID == "", "0", vltKitID));
                        thisMLabel.KitChildrenCount = Convert.ToInt32(vltKitChildCount);

                        // thisMLabel.ParentMCode = vltParentMcode;

                        DateTime ExpDate;

                        string expdate = "";
                        if (vltExpiryDate != "")
                        {
                            try
                            {
                                expdate = DateTime.ParseExact(vltExpiryDate, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");
                            }
                            catch (Exception ex)
                            {

                                expdate = DateTime.MinValue.ToString("MM/dd/yyyy");
                                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
                            }
                        }
                        else
                        {
                            expdate = DateTime.MinValue.ToString("MM/dd/yyyy");
                        }


                        ExpDate = Convert.ToDateTime(expdate);

                        // Following to Check the Printer Network/USB
                        int IsNetworkDevice = DB.GetSqlTinyInt("Select ISNULL(IsNetworkDevice,0) as TI from GEN_ClientResource Where DeviceIP=" + DB.SQuote(ddlNetworkPrinter.SelectedValue));

                        if (IsNetworkDevice == 1)
                        {
                            thisMLabel.PrinterType = "IP";
                        }
                        else
                        {
                            thisMLabel.PrinterType = "USB";
                        }

                        thisMLabel.PrinterIP = ddlNetworkPrinter.SelectedValue.Trim();
                        thisMLabel.IsBoxLabelReq = false;
                        thisMLabel.ReqNo = "";
                        thisMLabel.OBDNumber = "";
                        thisMLabel.KitCode = "";

                        thisMLabel.StrRefNo = IBTrack.StoreRefNumber;




                        ZPL zplString = new ZPL();

                        int FirstText_Y = 62;
                        int rowHeight = 22;


                        //
                        zplString.addBarcode(10, 6, vMCode, 50, BarcodeType.CODE128, false, 2);
                        zplString.drawText(5, FirstText_Y, 26, 32, vMCode);
                        FirstText_Y = FirstText_Y + 34;
                        zplString.drawText(5, FirstText_Y, 18, 22, vltItemDesc);

                        FirstText_Y = FirstText_Y + rowHeight + 5;

                        //Exp Date
                        if (ExpDate != DateTime.MinValue)
                        {
                            zplString.drawText(5, FirstText_Y, 18, 22, "Exp.Dt: " + ExpDate.ToShortDateString());
                            FirstText_Y = FirstText_Y + rowHeight;
                        }
                        // Batch No
                        if (vltBatchNo != "")
                        {
                            zplString.drawText(5, 104, 18, 22, "BatchNo: " + vltBatchNo);
                            FirstText_Y = FirstText_Y + rowHeight;
                        }
                        // StrRefNo 
                        zplString.drawText(5, FirstText_Y, 18, 22, "Str.Ref. #:" + IBTrack.StoreRefNumber);


                        // zplString.drawText(170, 290, 50, 50, humanReadableText);

                        //Logging ZPL Scripts
                        //System.IO.File.WriteAllText(@"C:\Users\swamyp\Desktop\Prints\" + humanReadableText +  ".txt", zplString.getZPLString());  
                        //string data = zplString.getZPLString();


                        zplString.PrintUsingUSB("ZDesigner", Convert.ToInt32(vltInvQuantity), 0);



                    }
                }
            }

            catch (Exception ex)
            {
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
                resetError("Error while printing", true);
            }


            this.POQuantityList_buildGridData(this.POQuantityList_buildGridData());
            //helper = new GridViewHelper(this.gvPOLineQty, false);
            //ConfigSample();
        }


        protected void lnkPrintBarCodeLabel_Click_Old(object sender, EventArgs e)
        {

            if (ddlNetworkPrinter.SelectedValue == "0")
            {
                resetError("Please select a printer", true);
                return;
            }

            String vMCode = "";
            //Print Barcode  the selected Linte Items
            try
            {

                //string[] gvMCode_POs ;

                ArrayList gvMCode_POs = new ArrayList();

                if (CommonLogic.QueryString("ibdid") != "")
                {
                    IBTrack = new InboundTrack(CommonLogic.QueryStringUSInt("ibdid"));
                }

                bool chkBox = false;
                //'Navigate through each row in the GridView for checkbox items
                foreach (GridViewRow gv in gvPOLineQty.Rows)
                {
                    CheckBox isPrint = (CheckBox)gv.FindControl("chkIsPrint");
                    if (isPrint.Checked)
                    {
                        chkBox = true;
                        //  Concatenate GridView items with comma for SQL Delete
                        //gvMCode_POs.Add(((Literal)gv.FindControl("ltLineNumber")).Text.ToString() + "|" + ((Label)gv.FindControl("lblMCode")).Text.ToString() + "|" + ((Literal)gv.FindControl("ltPONumber")).Text.ToString());

                        String vltLineNumber = ((Literal)gv.FindControl("ltLineNumber")).Text.ToString();
                        vMCode = ((Label)gv.FindControl("lblMCode")).Text.ToString();
                        String vltKitID = ((Literal)gv.FindControl("ltKitID")).Text.ToString();

                        String vltAlternativeCode = ((Literal)gv.FindControl("ltAlternativeCode")).Text.ToString();
                        String vOEMPartNo = ((Literal)gv.FindControl("ltOEMPartNo")).Text.ToString();

                        String vltBatchNo = "";// ((Literal)gv.FindControl("ltBatchNo")).Text.ToString();
                        String vltMfgDate = "";// ((Literal)gv.FindControl("ltMfgDate")).Text.ToString();
                        String vltExpiryDate = "";// ((Literal)gv.FindControl("ltExpiryDate")).Text.ToString();

                        String vltInvQuantity = ((Literal)gv.FindControl("ltInvQuantity")).Text.ToString();

                        String vltItemDesc = ((Literal)gv.FindControl("ltItemDesc")).Text.ToString();

                        String vltSerialNo = "";// ((Literal)gv.FindControl("ltSerialNo")).Text.ToString();
                        String vltKitChildCount = ((Literal)gv.FindControl("ltKitChildCount")).Text.ToString();
                        // String vltParentMcode = ((Literal)gv.FindControl("ltParentMcode")).Text.ToString();



                        thisMLabel.MCode = vMCode;
                        thisMLabel.OEMPartNo = vOEMPartNo;
                        thisMLabel.AltMCode = vltAlternativeCode;
                        thisMLabel.Description = vltItemDesc;
                        thisMLabel.BatchNo = vltBatchNo;
                        thisMLabel.SerialNo = vltSerialNo;
                        thisMLabel.KitPlannerID = Convert.ToInt32(CommonLogic.IIF(vltKitID == "", "0", vltKitID));
                        thisMLabel.KitChildrenCount = Convert.ToInt32(vltKitChildCount);

                        // thisMLabel.ParentMCode = vltParentMcode;

                        if (vltInvQuantity != "")
                            thisMLabel.InvQty = 1;
                        else
                            vltInvQuantity = "0.00";

                        string mfgdate = "";
                        if (vltMfgDate != "")
                        {
                            try
                            {
                                mfgdate = DateTime.ParseExact(vltMfgDate, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");
                            }
                            catch (Exception ex)
                            {
                                mfgdate = DateTime.MinValue.ToString("MM/dd/yyyy");
                                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
                            }
                        }
                        else
                        {
                            mfgdate = DateTime.MinValue.ToString("MM/dd/yyyy");
                        }


                        string expdate = "";
                        if (vltExpiryDate != "")
                        {
                            try
                            {
                                expdate = DateTime.ParseExact(vltExpiryDate, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");
                            }
                            catch (Exception ex)
                            {

                                expdate = DateTime.MinValue.ToString("MM/dd/yyyy");
                                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
                            }
                        }
                        else
                        {
                            expdate = DateTime.MinValue.ToString("MM/dd/yyyy");
                        }

                        //thisMLabel.MfgDate = Convert.ToDateTime(mfgdate);
                        //thisMLabel.ExpDate = Convert.ToDateTime(expdate);

                        thisMLabel.MfgDate = mfgdate;
                        thisMLabel.ExpDate = expdate;

                        // Following to Check the Printer Network/USB
                        int IsNetworkDevice = DB.GetSqlTinyInt("Select ISNULL(IsNetworkDevice,0) as TI from GEN_ClientResource Where DeviceIP=" + DB.SQuote(ddlNetworkPrinter.SelectedValue));

                        if (IsNetworkDevice == 1)
                        {
                            thisMLabel.PrinterType = "IP";
                        }
                        else
                        {
                            thisMLabel.PrinterType = "USB";
                        }

                        thisMLabel.PrinterIP = ddlNetworkPrinter.SelectedValue.Trim();
                        thisMLabel.IsBoxLabelReq = false;
                        thisMLabel.ReqNo = "";
                        thisMLabel.OBDNumber = "";
                        thisMLabel.KitCode = "";

                        thisMLabel.StrRefNo = IBTrack.StoreRefNumber;



                        // ParameterizedThreadStart tThermalLabelWorker = new ParameterizedThreadStart(this.ThermalLabelWorker);
                        // Thread worker = new Thread(new ParameterizedThreadStart(this.ThermalLabelWorker));


                        //Thread worker = new Thread(this.ThermalLabelWorker);
                        Thread worker = new Thread(new ParameterizedThreadStart(this.ThermalLabelWorker));
                        worker.SetApartmentState(ApartmentState.STA);
                        worker.Name = "ThermalLabelWorker";
                        worker.Start(thisMLabel);
                        worker.Join();

                        //CommonLogic.SendPrintJob(DB.RSField(rsLineItem, "MCode"), DB.RSField(rsLineItem, "AlternativeMCode"), DB.RSField(rsLineItem, "MDescription"), DB.RSField(rsLineItem, "BatchNo"),"",DB.RSFieldInt(rsLineItem, "KitPlannerID").ToString(), Convert.ToInt32(DB.RSFieldDecimal(rsLineItem, "InvoiceQuantity")) + 1, DateTime.MinValue, DB.RSFieldDateTime(rsLineItem, "ExpiryDate"), "IP", ddlNetworkPrinter.SelectedValue.Trim(), false, out result);
                        if (thisMLabel.Result != "Success")
                        {
                            resetError("Error while printing. Please contact admin<br/>" + thisMLabel.Result, true);
                        }
                        else
                        {
                            resetError("Successfully printed selected line items", false);
                        }
                        /*
                                }
                                else
                                {
                                    resetError("Error printing  line item  for Material Code: " + vMCode + " . Please check the Line numbers are added in the PO Line Items", false);
                                    return;
                                }
                        */
                    }
                }
            }

            catch (Exception ex)
            {
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
                resetError("Error while printing", true);
            }


            this.POQuantityList_buildGridData(this.POQuantityList_buildGridData());
            //helper = new GridViewHelper(this.gvPOLineQty, false);
            //ConfigSample();
        }

        private void ThermalLabelWorker(object thisMLabel2)
        {
            MRLWMSC21Common.TracklineMLabel thisMLabel = (MRLWMSC21Common.TracklineMLabel)thisMLabel2;

            String vresult = "";
            // if (rdlLabelSize.SelectedValue.ToLower() == "big")
            //CommonLogic.SendPrintJob_Big_7p6x5(thisMLabel.MCode, thisMLabel.AltMCode, thisMLabel.OEMPartNo, thisMLabel.Description, thisMLabel.BatchNo, thisMLabel.SerialNo, thisMLabel.KitPlannerID.ToString(), thisMLabel.KitChildrenCount, thisMLabel.ParentMCode, thisMLabel.InvQty, thisMLabel.MfgDate, thisMLabel.ExpDate, thisMLabel.PrinterType, thisMLabel.PrinterIP, thisMLabel.StrRefNo, thisMLabel.OBDNumber, thisMLabel.KitCode, thisMLabel.ReqNo, thisMLabel.IsBoxLabelReq, false,thisMLabel.PrintQty, out vresult);
            //  CommonLogic.SendPrintJob_Mat_2x1(thisMLabel.MCode, thisMLabel.AltMCode, thisMLabel.OEMPartNo, thisMLabel.Description, thisMLabel.BatchNo, thisMLabel.SerialNo, thisMLabel.KitPlannerID.ToString(), thisMLabel.KitChildrenCount, thisMLabel.ParentMCode, thisMLabel.InvQty, thisMLabel.MfgDate, thisMLabel.ExpDate, thisMLabel.PrinterType, thisMLabel.PrinterIP, thisMLabel.StrRefNo, thisMLabel.OBDNumber, thisMLabel.KitCode, thisMLabel.ReqNo, thisMLabel.IsBoxLabelReq, false, thisMLabel.PrintQty, out vresult);
            //  CommonLogic.SendPrintJob_Location_2x1p5("sss",
            //else
            //  CommonLogic.SendPrintJob_Small_5x2p5(thisMLabel.MCode, thisMLabel.AltMCode, thisMLabel.Description, thisMLabel.BatchNo, thisMLabel.SerialNo, thisMLabel.KitPlannerID.ToString(), thisMLabel.KitChildrenCount, thisMLabel.ParentMCode, thisMLabel.InvQty, thisMLabel.MfgDate, thisMLabel.ExpDate, thisMLabel.PrinterType, thisMLabel.PrinterIP, thisMLabel.StrRefNo, thisMLabel.ReqNo, thisMLabel.IsBoxLabelReq, out vresult);

            thisMLabel.Result = vresult;
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


        #region ----   RTR  Grid ------

        protected DataSet POQuantityList_buildGridData()
        {
            //AddDynamicMsp(Convert.ToUInt16(CommonLogic.QueryString("ibdid")));
            string sql = ViewState["POQuantityListSQL"].ToString();

            DataSet ds = DB.GetDS(sql, false);


            return ds;

        }

        protected void POQuantityList_buildGridData(DataSet ds)
        {
            gvPOLineQty.DataSource = ds.Tables[0];
            gvPOLineQty.DataBind();
            ds.Dispose();

            if (ds.Tables[0].Rows.Count == 0)
            {
                return;
            }

            gvPOLineQty.UseAccessibleHeader = true;
            gvPOLineQty.HeaderRow.TableSection = TableRowSection.TableHeader;
            //gvPOLineQty.FooterRow.TableSection = TableRowSection.TableFooter;
            gvPOLineQty.Attributes["style"] = "border-collapse:separate";
            foreach (GridViewRow row in gvPOLineQty.Rows)
            {
                if (row.RowIndex % 18 == 0 && row.RowIndex != 0)
                {
                    row.Attributes["style"] = "page-break-after:always;";
                }
            }

        }

        protected void gvPOLineQty_Sorting(object sender, GridViewSortEventArgs e)
        {

        }

        protected void gvPOLineQty_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {


                IBTrack = new InboundTrack(prmInboundTrackingID);


                Literal ltDynamicDisplayData = (Literal)e.Row.Cells[6].FindControl("ltDynamicDisplayData");
                //ltDynamicDisplayData.Text = "<td>test</td>";
                DataRow row = ((DataRowView)e.Row.DataItem).Row;
                String InvoiceNos = row["MaterialMasterValueList"].ToString();
                string[] DynamicMspsvalues = InvoiceNos.Split('`');
                StringBuilder DspData = new StringBuilder();

                if ((bool)ViewState["IsMSPAvailable"] == true)
                {
                    foreach (string dynamicmsp in DynamicMspsvalues)
                    {
                        DspData.Append("<td>" + dynamicmsp + "</td>");
                    }
                    ltDynamicDisplayData.Text += DspData.ToString();
                }

                Literal ltInvoiceQty = (Literal)e.Row.FindControl("ltInvQuantity");
                Literal ltReceivedQuantity = (Literal)e.Row.FindControl("ltReceivedQuantity");

                HyperLink lnkReceive = (HyperLink)e.Row.FindControl("lnkReceiveItem");


                if (IBTrack.InBoundStatusID < 3)
                {
                    //lnkReceive.Visible = false;
                }

                if (ltInvoiceQty != null && ltReceivedQuantity != null)
                {
                    if (ltInvoiceQty.Text.Equals(ltReceivedQuantity.Text))
                    {
                        //lnkReceive.Visible = false;
                    }
                }

            }
        }

        protected void gvPOLineQty_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            ViewState["ShipmentPendingIsInsert"] = false;

            gvPOLineQty.PageIndex = e.NewPageIndex;
            gvPOLineQty.EditIndex = -1;
            this.POQuantityList_buildGridData(this.POQuantityList_buildGridData());
        }

        protected void AddDynamicMsp(int ibdid)
        {
            IDataReader dsDynamicMSp = DB.GetRS("[dbo].[sp_MFG_GetMaterialStorageParametersFORDPN] @InboundID=" + ibdid + ",@TenantID=" + cp.TenantID);
            StringBuilder dsp = new StringBuilder();
            StringBuilder Footer = new StringBuilder();
            while (dsDynamicMSp.Read())
            {
                dsp.Append("<th align=\"center\"> " + DB.RSField(dsDynamicMSp, "DisplayName") + " </th>");
                Footer.Append("<td>&nbsp;</td>");

                ViewState["IsMSPAvailable"] = true;
            }
            gvPOLineQty.Columns[8].HeaderText = gvPOLineQty.Columns[8].HeaderText + dsp.ToString();
            gvPOLineQty.Columns[8].FooterText = gvPOLineQty.Columns[8].FooterText + Footer.ToString();

        }

        protected void lnkPrintBarCodeLabel_Click1(object sender, EventArgs e)
        {

        }


        #endregion ----   RTR  Grid ------
    }
}