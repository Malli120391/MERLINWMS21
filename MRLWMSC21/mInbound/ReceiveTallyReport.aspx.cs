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


// Module Name : ReceivingTallyReport Under Inbound
// DevelopedBy : Naresh P
// Created On  : 9/11/2013
// Modified On : 24/03/2015

namespace MRLWMSC21.mInbound
{
    public partial class ReceiveTallyReport : System.Web.UI.Page
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

                DesignLogic.SetInnerPageSubHeading(this.Page, "Receiving Tally Report");

                

                if (prmInboundTrackingID != 0)
                {
                    ViewState["InboundID"] = prmInboundTrackingID;

                    CommonLogic.LoadPrinters(ddlNetworkPrinter);
                    CommonLogic.LoadLabelSizes(ddlLabelSize);

                    //if (cp.TenantID != 0)


                    //{
                    //    gvPOLineQty.Columns[9].Visible = false;
                    //}

                    IBTrack = new InboundTrack(prmInboundTrackingID);

                    if (IBTrack.ShipmentTypeID == 4)
                        lblHeader.Text = "Returns Receiving Tally Report";

                    ltbarStoreRefNo.Text = IBTrack.StoreRefNumber + "<br/><br/><img border=0 src=\"Code39Handler.ashx?code=" + IBTrack.StoreRefNumber + "\" />";
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

                    ltDocDate.Text = IBTrack.DocReceivedDate;// DateTime.ParseExact(IBTrack.DocReceivedDate, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("dd-MMM-yyyy");
                    ltTenant.Text = CommonLogic.QueryString("TN");
                    // ltStoreIncharge.Text = cp.FirstName + " " + cp.LastName;

                    txtMcode.Attributes["onclick"] = "javascript:this.focus();";
                    txtMcode.Attributes["onfocus"] = "javascript:this.select();";

                    ViewState["IsMSPAvailable"] = false;


                }

            }

            if (cp.TenantID != 0)
            {
                gvPOLineQty.Columns[9].Visible = false;
            }

            POQuantityListSQL = "EXEC [sp_INB_ReceivingTallyReport]  @InboundID=" + prmInboundTrackingID + ",@MCode=null";
            ViewState["POQuantityListSQL"] = POQuantityListSQL;
           // AddDynamicMsp(Convert.ToUInt16(CommonLogic.QueryString("ibdid")));
            this.POQuantityList_buildGridData(this.POQuantityList_buildGridData());
            //helper = new GridViewHelper(this.gvPOLineQty, false);
            //ConfigSample();

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
            POQuantityListSQL = "EXEC [sp_INB_ReceivingTallyReport]  @InboundID=" + prmInboundTrackingID + ",@MCode=" + (txtMcode.Text.Trim().Equals("") ? "null" : DB.SQuote(txtMcode.Text.Trim()));
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
                resetError("Please select printer", true);
                return;
            }

            if (ddlLabelSize.SelectedValue == "0")
            {
                resetError("Please Select Label", true);
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

                int count = 0;
                int isPrintchkcount = 0;
                GridViewRow abcd = null;
                for (int i = 0; i < gvPOLineQty.Rows.Count; i++)
                {
                    abcd = gvPOLineQty.Rows[i];
                    bool isChecked = ((CheckBox)abcd.FindControl("chkIsPrint")).Checked;
                    if (isChecked)
                    {
                        isPrintchkcount = isPrintchkcount + 1;
                        count++;
                    }
                }

                if (isPrintchkcount != 0)
                {
                    string ZPL = "";
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
                            vMCode = ((Literal)gv.FindControl("lblMCode")).Text.ToString();
                            String vltKitID = ((Literal)gv.FindControl("ltKitID")).Text.ToString();

                            ////String vltAlternativeCode = ((Literal)gv.FindControl("ltAlternativeCode")).Text.ToString();
                            ////String vOEMPartNo = ((Literal)gv.FindControl("ltOEMPartNo")).Text.ToString();

                            String vltBatchNo = ((Literal)gv.FindControl("ltBatchNo")).Text.ToString();
                            String vltMfgDate = ((Literal)gv.FindControl("ltmfgdate")).Text.ToString();
                            String vltExpiryDate = ((Literal)gv.FindControl("ltexpdate")).Text.ToString();

                            String vltInvQuantity = ((Literal)gv.FindControl("ltInvQuantity")).Text.ToString();

                            String vltItemDesc = ((Literal)gv.FindControl("ltItemDesc")).Text.ToString();

                            String vltSerialNo = ((Literal)gv.FindControl("ltSerialNo")).Text.ToString();
                            String vltProjectNo = ((Literal)gv.FindControl("ltprojectRefNo")).Text.ToString();
                            String vltKitChildCount = ((Literal)gv.FindControl("ltKitChildCount")).Text.ToString();


                            string ddl = ddlLabelSize.SelectedValue;
                            string length = "";
                            string width = "";
                            string LabelType = "";
                            string query = "select * from TPL_Tenant_BarcodeType where IsActive=1 and IsDeleted=0 and TenantBarcodeTypeID=" + ddl;
                            DataSet DS = DB.GetDS(query, false);
                            foreach (DataRow row in DS.Tables[0].Rows)
                            {
                                length = row["Length"].ToString();
                                width = row["Width"].ToString();
                                LabelType = row["LabelType"].ToString();
                            }
                            thisMLabel.Lineno = vltLineNumber;
                            thisMLabel.LabelType = LabelType;
                            thisMLabel.MCode = vMCode;

                            thisMLabel.Description = vltItemDesc;
                            thisMLabel.BatchNo = vltBatchNo;
                            thisMLabel.SerialNo = vltSerialNo;
                            thisMLabel.KitPlannerID = Convert.ToInt32(CommonLogic.IIF(vltKitID == "", "0", vltKitID));
                            thisMLabel.KitChildrenCount = Convert.ToInt32(vltKitChildCount);
                            thisMLabel.ProjectNo = vltProjectNo;

                            // thisMLabel.ParentMCode = vltParentMcode;

                            if (vltInvQuantity != "")
                                thisMLabel.InvQty = 1;
                            else
                                vltInvQuantity = "0.00";

                            //string mfgdate = "";
                            //if (vltMfgDate != "")
                            //{
                            //    try
                            //    {
                            //        mfgdate = DateTime.ParseExact(vltMfgDate, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");
                            //    }
                            //    catch (Exception ex)
                            //    {
                            //        mfgdate = DateTime.MinValue.ToString("MM/dd/yyyy");
                            //        CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
                            //    }
                            //}
                            //else
                            //{
                            //    mfgdate = DateTime.MinValue.ToString("MM/dd/yyyy");
                            //}


                            //string expdate = "";
                            //if (vltExpiryDate != "")
                            //{
                            //    try
                            //    {
                            //        expdate = DateTime.ParseExact(vltExpiryDate, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");
                            //    }
                            //    catch (Exception ex)
                            //    {

                            //        expdate = DateTime.MinValue.ToString("MM/dd/yyyy");
                            //        CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
                            //    }
                            //}
                            //else
                            //{
                            //    expdate = DateTime.MinValue.ToString("MM/dd/yyyy");
                            //}
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

                            thisMLabel.PrinterType = "IP";
                            thisMLabel.PrinterIP = ddlNetworkPrinter.SelectedValue.Trim();
                            thisMLabel.IsBoxLabelReq = false;
                            thisMLabel.ReqNo = "";
                            thisMLabel.OBDNumber = "";
                            thisMLabel.KitCode = "";

                            thisMLabel.Length = length;
                            thisMLabel.Width = width;
                            int dpi = 0;
                            dpi = CommonLogic.GETDPI(thisMLabel.PrinterIP);
                            //Commented ON 01-JAN-2019 BY Prasad // thisMLabel.Dpi = 203;//dpi;
                            thisMLabel.Dpi = dpi;

                            thisMLabel.StrRefNo = IBTrack.StoreRefNumber;
                            //For Printing Label Using ZPL Added by Prasanna on 21st august 2017
                            MRLWMSC21Common.PrintCommon.CommonPrint print = new MRLWMSC21Common.PrintCommon.CommonPrint();

                            print.PrintBarcodeLabel(thisMLabel);
                            resetError("Successfully printed selected line items", false);

                            // Commented ON 01-JAN-2019 BY Prasad //
                            /*
                             * ZPL +=  print.PrintBarcodeLabel(thisMLabel);
                             bool z = ZPL.Contains("\r\n");
                             if (z == true)
                             {
                                 ZPL = ZPL.Replace("\r\n", "XYZ786XYZ");
                             }*/








                            //Commented  by Prasanna on 21st august 2017
                            // ParameterizedThreadStart tThermalLabelWorker = new ParameterizedThreadStart(this.ThermalLabelWorker);
                            // Thread worker = new Thread(new ParameterizedThreadStart(this.ThermalLabelWorker));


                            //Thread worker = new Thread(this.ThermalLabelWorker);
                            //  Thread worker = new Thread(new ParameterizedThreadStart(this.ThermalLabelWorker));
                            //worker.SetApartmentState(ApartmentState.STA);
                            //worker.Name = "ThermalLabelWorker";
                            //worker.Start(thisMLabel);
                            //worker.Join();

                            ////CommonLogic.SendPrintJob(DB.RSField(rsLineItem, "MCode"), DB.RSField(rsLineItem, "AlternativeMCode"), DB.RSField(rsLineItem, "MDescription"), DB.RSField(rsLineItem, "BatchNo"),"",DB.RSFieldInt(rsLineItem, "KitPlannerID").ToString(), Convert.ToInt32(DB.RSFieldDecimal(rsLineItem, "InvoiceQuantity")) + 1, DateTime.MinValue, DB.RSFieldDateTime(rsLineItem, "ExpiryDate"), "IP", ddlNetworkPrinter.SelectedValue.Trim(), false, out result);
                            //if (thisMLabel.Result != "Success")
                            //{
                            //    resetError("Error while printing. Please contact admin<br/>" + thisMLabel.Result, true);
                            //}
                            //else
                            //{
                            //    resetError("Successfully printed selected line items", false);
                            //}
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

                    // Commented ON 01-JAN-2019 BY Prasad// ScriptManager.RegisterStartupScript(this, this.GetType(), "test", "PrintRTR_ZPL('" + ZPL.ToString().Trim() + "');", true);
                }
                else
                {
                    resetError("Please Select Items to Print", true);
                    return;
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
           // CommonLogic.SendPrintJob_Big_7p6x5(thisMLabel.MCode, thisMLabel.AltMCode, thisMLabel.OEMPartNo, thisMLabel.Description, thisMLabel.BatchNo, thisMLabel.SerialNo, thisMLabel.KitPlannerID.ToString(), thisMLabel.KitChildrenCount, thisMLabel.ParentMCode, thisMLabel.InvQty, thisMLabel.MfgDate.ToString(), thisMLabel.ExpDate, thisMLabel.PrinterType, thisMLabel.PrinterIP, thisMLabel.StrRefNo, thisMLabel.OBDNumber, thisMLabel.KitCode, thisMLabel.ReqNo, thisMLabel.IsBoxLabelReq, false,thisMLabel.PrintQty, out vresult);
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
                resetError("No data found", true);
                return;
            }

            gvPOLineQty.UseAccessibleHeader = true;
            gvPOLineQty.HeaderRow.TableSection = TableRowSection.TableHeader;
            gvPOLineQty.FooterRow.TableSection = TableRowSection.TableFooter;
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
                    lnkReceive.Visible = false;
                }
                

                /*
                if (ltInvoiceQty != null && ltReceivedQuantity != null)
                {
                    //if (ltInvoiceQty.Text.Equals(ltReceivedQuantity.Text))
                    //{
                    //    lnkReceive.Visible = false;
                    //}
                }
                */

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
            gvPOLineQty.Columns[7].HeaderText = gvPOLineQty.Columns[7].HeaderText + dsp.ToString();
            gvPOLineQty.Columns[7].FooterText = gvPOLineQty.Columns[7].FooterText + Footer;

        }

        protected void lnkPrintBarCodeLabel_Click1(object sender, EventArgs e)
        {

        }


        #endregion ----   RTR  Grid ------




    }

}