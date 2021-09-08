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
using System.Drawing;
using System.Web.Services;
using MRLWMSC21.mOutbound.BL;

// Module Name : DeliveryPickNote Under Outbound
// Description : 
// DevelopedBy : Naresh P
// CreatedOn   : 26/11/2013
// Lsst Modified Date : 

namespace MRLWMSC21.mOutbound
{
    public partial class DeliveryPickNote : System.Web.UI.Page
    {

        public static CustomPrincipal cp = HttpContext.Current.User as CustomPrincipal;

        protected string OBDLineItemsListSQL = "";
        protected int OutboundID;
        protected String OBDNumber;
        protected OBDTrack thisOOBD;

        protected void Page_PreInit(object sender, EventArgs e)
        {
            Page.Theme = "Outbound";
            cp = HttpContext.Current.User as CustomPrincipal;
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            DesignLogic.SetInnerPageSubHeading(this.Page, "Pick List");

            //string[] rolesAllowedInThisPage = CommonLogic.GetRolesAllowed("1,2,4,5,7,17");

            //if (!cp.IsInAnyRoles(rolesAllowedInThisPage))
            //{
            //    Response.Redirect("Login.aspx?eid=6");
            //}


            //if (Request.UrlReferrer != null)
            //{
            //    lnkbackToList.PostBackUrl = "OutboundSearch.aspx";
            //}
            //else
            //{
            //    lnkbackToList.PostBackUrl = "OutboundTracking.aspx";
            //}

            OutboundID = DB.GetSqlN("select top 1 OutboundID AS N from OBD_Outbound where IsActive=1 and IsDeleted=0 AND OutboundID=" + CommonLogic.QueryStringUSInt("obdid"));

            if (OutboundID != 0)
            {
                thisOOBD = new OBDTrack(OutboundID);
            }


            if (!IsPostBack)
            {
                if (CommonLogic.QueryString("obdid") != "")
                    ViewState["OutboundID"] = CommonLogic.QueryString("obdid");
                ViewState["TenantID"] = cp.TenantID;
                if (OutboundID != 0 && CommonLogic.QueryString("lineitemcount").ToString() != "0")
                {
                    ltstore.Text = "<span class='FormLabels'><b>Store </b>:" + CommonLogic.GetWareHouseCode(thisOOBD.ReferedStoreIDs.ToString()) + " <span>";

                    DataSet ds = DB.GetDS("EXEC [dbo].[sp_OBD_GetDeliveryNote_Header] @OutboundID=" + CommonLogic.QueryString("obdid"), false);

                    string customerName = "";
                    if (ds != null)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            customerName = ds.Tables[0].Rows[0]["CustomerName"].ToString() == "" || ds.Tables[0].Rows[0]["CustomerName"].ToString() == null ? "" : ds.Tables[0].Rows[0]["CustomerName"].ToString();
                        }
                        else
                        {
                            customerName = "";
                        }
                    }

                    string areaSalesManager = "";
                    if (ds != null)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            areaSalesManager = ds.Tables[0].Rows[0]["AreaSalesManager"].ToString() == "" || ds.Tables[0].Rows[0]["AreaSalesManager"].ToString() == null ? "" : ds.Tables[0].Rows[0]["AreaSalesManager"].ToString();
                        }
                        else
                        {
                            areaSalesManager = "";
                        }
                    }

                    string discount = "";
                    if (ds != null)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            discount = ds.Tables[0].Rows[0]["Discount"].ToString() == "" || ds.Tables[0].Rows[0]["Discount"].ToString() == null ? "" : ds.Tables[0].Rows[0]["Discount"].ToString();
                        }
                        else
                        {
                            discount = "";
                        }
                    }

                    string IndentNo = "";
                    if (ds != null)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            IndentNo = ds.Tables[0].Rows[0]["IndentNo"].ToString() == "" || ds.Tables[0].Rows[0]["IndentNo"].ToString() == null ? "" : ds.Tables[0].Rows[0]["IndentNo"].ToString();
                        }
                        else
                        {
                            IndentNo = "";
                        }
                    }


                    string city = "";
                    if (ds != null)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            city = ds.Tables[0].Rows[0]["City"].ToString() == "" || ds.Tables[0].Rows[0]["City"].ToString() == null ? "" : ds.Tables[0].Rows[0]["City"].ToString();
                        }
                        else
                        {
                            city = "";
                        }
                    }


                    ltDelvDocNo.Text = "<span class='FormLabels'><b>&emsp;Pick List No. : " + thisOOBD.OBDNumber + "</b><span><br/><br/><img width='300' src=\" ../mInbound/Code39Handler.ashx?code=" + thisOOBD.OBDNumber + "\" />";
                    ltDelvDocNo1.Text = thisOOBD.OBDNumber;
                    ltDelvDocDetails.Text = "<table  cellpadding='2' cellspacing='1' border='0' align='right'>";
                    ltDelvDocDetails.Text += "<tr><td class='FormLabels' align='left' style='vertical-align:top !important;'><b>Tenant</b></td><td class='FormLabels' align='left' style='vertical-align:top !important;'> <b>:</b>&nbsp;" + CommonLogic.QueryString("TN") + "</td></tr>";
                    ltDelvDocDetails.Text += "<tr><td class='FormLabels' align='left' style='vertical-align:top !important;'><b>Store</b></td><td class='FormLabels' align='left' style='vertical-align:top !important;'> <b>:</b>&nbsp;" + CommonLogic.GetWareHouseCode(thisOOBD.ReferedStoreIDs.ToString()) + "</td></tr>";
                    //ltDelvDocDetails.Text += "<tr><td class='FormLabels' align='left' style='vertical-align:top !important;'><b>Customer :</b></td><td class='FormLabels' align='left' style='vertical-align:top !important;'>" + CommonLogic.GetCustomerName(thisOOBD.CustomerID.ToString()) + "</td></tr>";
                    ltDelvDocDetails.Text += "<tr><td class='FormLabels' align='left' style='vertical-align:top !important;'><b>Customer</b></td><td class='FormLabels' align='left' style='vertical-align:top !important;'> <b>:</b>&nbsp;" + customerName + "</td></tr>";
                    ltDelvDocDetails.Text += "<tr><td class='FormLabels' align='left' style='vertical-align:top !important;'><b>Indent No.</b></td><td class='FormLabels' align='left' style='vertical-align:top !important;'> <b>:</b>&nbsp;" + IndentNo + "</td></tr>";
                    ltDelvDocDetails.Text += "<tr><td class='FormLabels' align='left' style='vertical-align:top !important;'><b>City</b></td><td class='FormLabels' align='left' style='vertical-align:top !important;'> <b>:</b>&nbsp;" + city + "</td></tr>";
                    //ltDelvDocDetails.Text += "<tr><td class='FormLabels' align='left' style='vertical-align:top !important;'><b>Requested By</b></td><td class='FormLabels' align='left' style='vertical-align:top !important;'> <b>:</b>&nbsp;" + CommonLogic.GetUserName(thisOOBD.RequestedByID.ToString()) + "</td></tr>";
                    ltDelvDocDetails.Text += "<tr><td class='FormLabels' align='left' style='vertical-align:top !important;'><b>Date</b></td><td class='FormLabels' align='left' style='vertical-align:top !important;'> <b>:</b>&nbsp;" + thisOOBD.OBDDate + "</td></tr>";
                    ltDelvDocDetails.Text += "<tr><td class='FormLabels' align='left' style='vertical-align:top !important;'><b>Area Sales Manager</b></td><td class='FormLabels' align='left' style='vertical-align:top !important;'> <b>:</b>&nbsp;" + areaSalesManager + "</td></tr>";
                    ltDelvDocDetails.Text += "<tr><td class='FormLabels' align='left' style='vertical-align:top !important;'><b>Discount(%)</b></td><td class='FormLabels' align='left' style='vertical-align:top !important;'> <b>:</b>&nbsp;" + discount + "</td></tr>";
                    ltDelvDocDetails.Text += "<tr><td class='FormLabels' align='left' style='vertical-align:top !important;'><b>Format No.</b></td><td class='FormLabels' align='left' style='vertical-align:top !important;'> <b>:</b>&nbsp;"+"RFS/FSMS/STR F04"+"</td></tr>";                    
                    ltDelvDocDetails.Text += "</table>";

                    OBDNumber = thisOOBD.OBDNumber;


                    txtMCode.Attributes["onclick"] = "javascript:this.focus();";
                    txtMCode.Attributes["onfocus"] = "javascript:this.select();";


                    //AddGridHeaderList(OutboundID);

                    //OBDLineItemsListSQL = "EXEC [sp_OBD_GetDeliveryPickNote]  @OutboundID=" + OutboundID.ToString() + ",@WarehouseID=NULL,@MCode=''";  // ,@OBDNumber=NULL,@MCode=NULL,@WarehouseID=" + DB.SQuote(String.Join(",", cp.Warehouses));
                    //OBDLineItemsListSQL = "EXEC [sp_OBD_GetDeliveryPickNote]  @OutboundID=" + OutboundID.ToString() + ",@WarehouseID=" + DB.SQuote(String.Join(",", cp.Warehouses)) +",@MCode=''";  // ,@OBDNumber=NULL,@MCode=NULL,@WarehouseID=" + DB.SQuote(String.Join(",", cp.Warehouses));
                    OBDLineItemsListSQL = "EXEC [dbo].[sp_OBD_DeliveryPickNote_NEW] @OutboundId=" + OutboundID.ToString() + ",@MCode=''" + "";

                    ViewState["OBDLineItemsListSQL"] = this.OBDLineItemsListSQL;

                    this.OBDLineItemsList_buildGridData(this.OBDLineItemsList_buildGridData());
                }


            }

            if (!IsPostBack)
            {
                String IconPath = "";
                if (cp !=null)
                {
                    cp = HttpContext.Current.User as CustomPrincipal;
                    //cp = Session["UserProfile"] as CustomPrincipal;
                    string fileName = DB.GetSqlS("SELECT DISTINCT LogoPath AS S FROM GEN_User USR JOIN GEN_Account ACC ON ACC.AccountID = USR.AccountID AND ACC.IsActive = 1 AND ACC.IsDeleted = 0 WHERE USR.IsActive = 1 AND USR.IsDeleted = 0 AND USR.UserID = " + cp.UserID);
                    if (fileName == "" || fileName == null || cp.AccountID == 0)
                    {
                        fileName = "inventrax.png";
                        IconPath = "Images/";
                    }
                    else
                    {
                        fileName = fileName;
                        IconPath = "TPL/AccountLogos/";
                    }

                    if (cp.UserID == 1)
                    {
                        fileName = "inventrax.png";
                        IconPath = "Images/";
                    }

                    imgLogo.ImageUrl = Page.ResolveUrl("~") + IconPath + fileName;
                    //imgLogo.Attributes.Add("width", "166");
                    //imgLogo.Attributes.Add("height", "50");
                }
            }
        }

        protected void lnkMCodeSearch_Click(object sender, EventArgs e)
        {

            if (txtMCode.Text == "Search Part # ...")
                txtMCode.Text = "";

            if (txtMCode.Text.Trim() != "")
                //OBDLineItemsListSQL = " EXEC [sp_OBD_GetDeliveryPickNote] @OutboundID=" + OutboundID.ToString() + ",@OBDNumber=NULL,@MCode=" + DB.SQuote(txtMCode.Text.Trim()) + ",@WarehouseID=" + DB.SQuote(String.Join(",", cp.Warehouses)) ; //+ DB.SQuote(String.Join(",", cp.Warehouses));
                OBDLineItemsListSQL = "EXEC [dbo].[sp_OBD_DeliveryPickNote_NEW] @OutboundId=" + OutboundID.ToString() + ",@MCode=" + DB.SQuote(txtMCode.Text.Trim()) + "";
            else
                //OBDLineItemsListSQL = "EXEC [sp_OBD_GetDeliveryPickNote]  @OutboundID=" + OutboundID.ToString() + ",@OBDNumber=NULL,@MCode='',@WarehouseID=" + DB.SQuote(String.Join(",", cp.Warehouses));// + DB.SQuote(String.Join(",", cp.Warehouses));
                OBDLineItemsListSQL = "EXEC [dbo].[sp_OBD_DeliveryPickNote_NEW] @OutboundId=" + OutboundID.ToString() + ",@MCode=''" + "";

            ViewState["OBDLineItemsListSQL"] = this.OBDLineItemsListSQL;

            this.OBDLineItemsList_buildGridData(this.OBDLineItemsList_buildGridData());
            txtMCode.Text = "Search Part # ...";

        }

        public String GetKitPlannetID(String GetKitPlannetID)
        {
            return CommonLogic.IIF(GetKitPlannetID != "", "<b>Kit ID : " + GetKitPlannetID.PadLeft(3, '0') + "</b>", "");
        }



        #region -------------------------Delivery Pick Note  -----------------------------




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

        protected DataSet OBDLineItemsList_buildGridData()
        {
            string sql = ViewState["OBDLineItemsListSQL"].ToString();
            DataSet ds = DB.GetDS(sql, false);
            int lineitemcount = DB.GetSqlN("select count(distinct SODetailsID) as N from OBD_Outbound_ORD_CustomerPO where IsDeleted=0 and IsActive=1 and OutboundID=" + CommonLogic.QueryString("obdid"));
            //ltPNCPRecordCount.Text = "SO Line Items - [" + ds.Tables[0].Rows.Count.ToString() + "]";
            ltPNCPRecordCount.Text = "No. of Line Items - [" + lineitemcount + "]";
            //ltPNCPRecordCount.Text = "Line Items - [" + ds.Tables[0].DefaultView.ToTable(true,"MCode").Rows.Count.ToString() + "]";
            //DefaultView.ToTable(true, "employeeid")

            return ds;
        }

        protected void OBDLineItemsList_buildGridData(DataSet ds)
        {
            if (ds.Tables[0].Rows.Count != 0)
            {
                gvPOLineQty.DataSource = ds.Tables[0];
                gvPOLineQty.DataBind();
                ds.Dispose();
            }
            else
            {
                resetError("No data found", true);
            }

        }

        protected void gvPOLineQty_RowCreated(object sender, GridViewRowEventArgs e)
        {


        }

        protected void gvPOLineQty_Sorting(object sender, GridViewSortEventArgs e)
        {


        }

        protected void gvPOLineQty_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                int isvlpdpicking = DB.GetSqlN("select IsVLPDPicking as N from obd_outbound where OutboundID=" + ViewState["OutboundID"]);
                if (isvlpdpicking == 1)
                {
                    var a = e.Row.FindControl("btnPick") as HyperLink;
                    a.Style.Add("display", "none");
                }
                //String vltSplitLocation = ((Literal)e.Row.Cells[6].FindControl("ltSplitLocation")).Text;
                //String MaterialMasterID = ((Literal)e.Row.Cells[2].FindControl("ltMMID")).Text;
                //String LineNumber = ((Literal)e.Row.Cells[0].FindControl("ltLineNumber")).Text;
                ////String vKitID = ((Literal)e.Row.Cells[0].FindControl("hidKitPlannerID")).Text;
                //String OutboundID = CommonLogic.QueryString("obdid");  
                //String vSOHID = ((Literal)e.Row.Cells[2].FindControl("ltSOHID")).Text;

                //Literal ltLocation = (Literal)e.Row.Cells[6].FindControl("ltLocation");
                //Literal ltLocationID = (Literal)e.Row.Cells[6].FindControl("ltLocationID");
                // ltLocation.Text = DisplayInTableNew(e.Row, LineNumber, OutboundID, MaterialMasterID, vKitID, vltSplitLocation, ltLocationID, vSOHID);

            }

        }

        protected void gvPOLineQty_RowCommand(object sender, GridViewCommandEventArgs e)
        {

            if (e.CommandName == "UpdateLineItemQty")
            {
                //Set the rowindex
                int rowIndex = Convert.ToInt32(e.CommandArgument);

                GridViewRow row = gvPOLineQty.Rows[rowIndex];

                String valMCode = ((Literal)row.FindControl("ltMCode")).Text;
                TextBox valQty = (TextBox)row.FindControl("txtPickedQty");


            }

        }

        protected void lnkUpdateGoodsOut_Click(object sender, EventArgs e)
        {
            StringBuilder sqlBulkUpdate = new StringBuilder();

            foreach (GridViewRow row in gvPOLineQty.Rows)
            {
                Boolean isUpdated = ((CheckBox)row.FindControl("chkIsUpdate")).Checked;
                String MMID = ((Literal)row.FindControl("ltMMID")).Text;

                if (isUpdated)
                {
                    if (DB.GetSqlN("Select Count(*) from  MMGoodsMovementDetail GMD  JOIN MMGoodsMovement GM ON GM.GoodsMovementID = GMD.GoodsMovementID WHERE GMD.MaterialMasterID=" + MMID + " AND GM.ATCRefNumber=" + OBDNumber) == 0)
                    {

                    }

                }

            }

        }

        protected void gvPOLineQty_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            ViewState["ShipmentPendingIsInsert"] = false;

            gvPOLineQty.PageIndex = e.NewPageIndex;
            gvPOLineQty.EditIndex = -1;
            this.OBDLineItemsList_buildGridData(this.OBDLineItemsList_buildGridData());
        }

        private void LoadDropDowns(DropDownList ddl, String csvString)
        {
            char[] seps = new char[] { ',' };
            String[] ArrValues = csvString.Split(seps);

            ddl.Items.Clear();
            foreach (string arrVal in ArrValues)
            {
                ddl.Items.Add(new ListItem(arrVal, arrVal));
            }

        }

        private void LoadUoM(DropDownList ddlUoM, String selVal)
        {

            ddlUoM.Items.Clear();

            ddlUoM.Items.Add(new ListItem("Select UoM", "0"));
            IDataReader rsCur = DB.GetRS("Select MMSKUID, MMSKU from MMSKU Where IsActive =1");

            while (rsCur.Read())
            {
                ddlUoM.Items.Add(new ListItem(rsCur["MMSKU"].ToString(), rsCur["MMSKUID"].ToString()));

            }

            rsCur.Close();
            ddlUoM.SelectedValue = selVal;

        }

        public string GetLineItemProps(String StockType, String StockTypeCode, String BatchNo, String SerialNo)
        {
            String resString = "";

            if (StockType != "")
                resString += "StockType: " + StockType + " (" + StockTypeCode.Trim() + ")";

            if (BatchNo != "")
                resString += ", Batch#: " + BatchNo;

            if (SerialNo != "")
                resString += ", Serial#: " + SerialNo;

            return resString;
        }

        private void AddGridHeaderList(int OBDID)
        {
            IDataReader dsDynamicMSp = DB.GetRS("[dbo].[sp_MFG_GetMaterialStorageParametersFORDPN]@OutBoundID=" + OBDID + ",@TenantID=" + cp.TenantID);

            DataSet dataset = DB.GetDS("[dbo].[sp_MFG_GetMaterialStorageParametersFORDPN]@OutBoundID=" + OBDID + ",@TenantID=" + cp.TenantID, false);

            /*
            HeaderText.Append("<table width='100%' border=\"1\" align=\"center\" cellpadding='1'  style=\"border-collapse:collapse;border: 1px solid black;\"><tr><td align=\"left\" width='100'>Location&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td><td align=\"left\" width='100'>KitID&nbsp</td>");

            while (dsDynamicMSp.Read())
            {
                String columnname = DB.RSField(dsDynamicMSp, "ParameterName");
                HeaderText.Append("<td align=\"left\" width='100'>" + columnname + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>");
            }
            HeaderText.Append("<td align=\"left\" width='100'>IsDam</td><td align=\"left\" width='100'>Available</td><td align=\"left\" width='100'>Picked</td><td align=\"left\" width='100'>PickLink</td></tr></table>");
             */

            DataTable table = dataset.Tables[0];
            int count = table.Rows.Count;
            count = count + 7;
            int tablewidth = count * 80;
            StringBuilder HeaderText = new StringBuilder();
            HeaderText.Append("<table border=\"1\" style=\"border-collapse:collapse;\"><tr><td align=\"left\" width='80'>Location</td><td align=\"left\" width='40'>KitID</td>");

            //HeaderText.Append("<td><table width='" + tablewidth + "' border=\"1\" style=\"border-collapse:collapse;\"><tr>");
            while (dsDynamicMSp.Read())
            {
                String columnname = DB.RSField(dsDynamicMSp, "DisplayName");
                HeaderText.Append("<td  align=\"left\" width='100'>" + columnname + "</td>");
            }

            HeaderText.Append("<td align=\"left\" width='40' style=\"border-left-style:none;border-top-style:none;border-bottom-style:none;\">Is Dam.</td><td align=\"left\" width='40'>Has Dis.</td><td align=\"left\" width='40'>Is NC</td><td align=\"left\" width='40'>As Is</td><td align=\"left\" width='40'>Pve Recall</td> <td align=\"left\" width='100'>Aval.</td><td align=\"left\" width='100'>Picked</td>");
            //HeaderText.Append("</tr></table></td>");
            if (cp.TenantID == 0)
            {
                HeaderText.Append("<td align=\"left\" width='50'>Pick Link</td></tr></table>");
            }
            else
            {
                HeaderText.Append("</tr></table>");
            }

            gvPOLineQty.Columns[9].HeaderText = HeaderText.ToString();
        }

        public string DisplayInTable(GridViewRow gvRow, String LineNumber, String OutboundID, String MaterialMasterID, String vKitID, string SplitData, Literal ltLocationID, String vSOHID)
        {
            if (SplitData != "")
            {
                StringBuilder sbResult = new StringBuilder();

                String ssss = "";
                char[] Locationseps = new char[] { ',' };
                String[] Locations = SplitData.Split(Locationseps);

                char[] StockTypeseps = new char[] { '|' };
                String[] StockValues;
                if (Locations.Length != 0)
                {
                    sbResult.Append("<table width='100%' border=\"1\"  style=\"border-collapse:collapse;border:0px solid black;\">");

                    foreach (String Loc in Locations)
                    {
                        StockValues = Loc.Split(StockTypeseps);
                        if (StockValues[2].ToString() == "-555.00")
                        {
                            sbResult.Append("<tr><td> MM Item is not configured to handle multiple UoM's for this Delv. Doc. Pls contact MMAdmin . </td></tr>");
                        }
                        else
                        {
                            String PickLink = "";
                            sbResult.Append("<tr>");
                            int locationId = 0;
                            for (var i = 0; i < StockValues.Length; i++)
                            {
                                if (i == 1)
                                {
                                    ltLocationID.Text = StockValues[i];
                                    locationId = int.Parse(StockValues[i]);
                                }
                                else if (i == 3)
                                {
                                    sbResult.Append("<td style=\"border-top-style:none;border-bottom-style:none;border-left-style:none;border-right-style:none;\" >");
                                    String[] locwiseMSP = StockValues[i].Split('@');

                                    String[] valuesforcount = locwiseMSP[0].Split('!');
                                    int count = valuesforcount.Length - 1;
                                    int tablewidth = (count + 1) * 80;
                                    sbResult.Append("<table width='" + (tablewidth) + "'border=\"1\" style=\"border-collapse:collapse;\">");

                                    for (var j = 0; j < locwiseMSP.Length; j++)
                                    {
                                        String[] values = locwiseMSP[j].Split('!');

                                        if (values[(values.Length - 3)] == "0")
                                            ssss = "style=\"text-decoration:line-through;\"";
                                        else
                                            ssss = "";

                                        sbResult.Append("<tr >");

                                        for (var k = 0; k < values.Length; k++)
                                        {
                                            // border-left-style:none;border-right-style:none;
                                            //style=\"border-top-style:none;border-bottom-style:none\";

                                            if (k != (values.Length - 3))
                                            {

                                                if (k == (values.Length - 5) || k == (values.Length - 6) || k == (values.Length - 4) || k == (values.Length - 7) || k == (values.Length - 8))
                                                {
                                                    if (values[k] != "0")
                                                    {
                                                        sbResult.Append("<td align='left' width='80'  " + ssss + " >");
                                                        sbResult.Append("<img  src=\"../Images/blue_menu_icons/check_mark.png\"  />");
                                                        sbResult.Append("</td>");
                                                    }
                                                    else
                                                    {
                                                        sbResult.Append("<td align='left' width='80' " + ssss + " >");
                                                        sbResult.Append("");
                                                        sbResult.Append("</td>");
                                                    }

                                                }
                                                else
                                                {
                                                    sbResult.Append("<td align='left' width='80' " + ssss + " >");
                                                    sbResult.Append(values[k]);
                                                    sbResult.Append("</td>");
                                                }
                                            }

                                        }

                                        if (ssss == "")
                                        {
                                            PickLink = "<nobr><a style='text-decoration:none;' href='" + String.Format("../mInventory/PickItem.aspx?mmid={0}&obdid={1}&LineNum={2}&locid={3}&kitid={4}&soid={5}'> Pick <img src='../Images/redarrowright.gif' border='0' /></nobr>", MaterialMasterID, OutboundID, LineNumber, locationId, CommonLogic.IIF(vKitID != "", vKitID, "NULL"), CommonLogic.IIF(vSOHID != "", vSOHID, "NULL"));
                                            sbResult.Append("<td align='left' width='50' " + ssss + " >");
                                            sbResult.Append(PickLink);
                                            sbResult.Append("</td>");
                                        }
                                        else
                                        {
                                            sbResult.Append("<td align='left' width='50' >");

                                            sbResult.Append("</td>");
                                        }

                                        sbResult.Append("</tr>");


                                    }

                                    sbResult.Append("</table>");
                                    sbResult.Append("</td>");

                                    PickLink = "<nobr><a style='text-decoration:none;' href='" + String.Format("../mInventory/PickItem.aspx?mmid={0}&obdid={1}&LineNum={2}&locid={3}&kitid={4}&soid={5}'> Pick <img src='../Images/redarrowright.gif' border='0' /></nobr>", MaterialMasterID, OutboundID, LineNumber, locationId, CommonLogic.IIF(vKitID != "", vKitID, "NULL"), CommonLogic.IIF(vSOHID != "", vSOHID, "NULL"));

                                    /*
                                    sbResult.Append("<td align='left' width='80'>");
                                    if (i == 4)
                                    {
                                        if (StockValues[i] == "0")
                                        {

                                        }
                                    }
                                    else
                                    {
                                       // String[] locwiseMSPs = StockValues[i].Split('@');

                                        sbResult.Append(PickLink);
                                    }


                                    sbResult.Append("</td>");*/
                                }
                                else
                                {
                                    if (i != 4)
                                    {
                                        sbResult.Append("<td align='left' width='95'>");
                                        sbResult.Append(StockValues[i]);
                                        sbResult.Append("</td>");
                                    }

                                }

                            }

                            sbResult.Append("</tr>");

                        }
                    }

                    sbResult.Append("</table>");
                }

                return sbResult.ToString();
            }
            else
            {
                return "No item in DB ";
            }
        }

        public string DisplayInTableNew(GridViewRow gvRow, String LineNumber, String OutboundID, String MaterialMasterID, String vKitID, string SplitData, Literal ltLocationID, String vSOHID)
        {
            if (SplitData != "")
            {
                StringBuilder sbResult = new StringBuilder();

                String Strickout = "";
                String[] Locations = SplitData.Split(',');
                String[] StockValues;
                StringBuilder outerRow = new StringBuilder();
                String[] Multilines;
                String[] values;
                bool outer = true;
                bool flag = false;
                int locationId;
                if (Locations.Length != 0)
                {
                    sbResult.Append("<table width='100%' border=\"1\"  style=\"border-collapse:collapse;border:0px solid black;\">");
                    foreach (String Loc in Locations)
                    {
                        outerRow.Clear();
                        outer = true;
                        StockValues = Loc.Split('|');
                        if (StockValues[2].ToString() == "-555.00")
                        {
                            sbResult.Append("<tr><td> MM Item is not configured to handle multiple UoM's for this Delv. Doc. Pls contact MMAdmin . </td></tr>");
                        }
                        else
                        {
                            Multilines = StockValues[3].Split('@');

                            // This is for first row strickout validation
                            values = Multilines[0].Split('!');

                            if (cp.TenantID == 0)
                            {
                                //if (values[values.Length - 3] == "0")
                                if (values[values.Length - 3] == "0" || StockValues[StockValues.Length - 1] == "0")
                                {
                                    //Strickout = "style=\"text-decoration:line-through;\"";
                                    Strickout = "class='StrikeOut'";
                                }
                                else
                                {
                                    Strickout = "";
                                }
                            }

                            if (cp.TenantID != 0)
                            {

                                Strickout = "style=\"text-decoration:line-through;\"";
                            }

                            sbResult.Append("<tr>");
                            sbResult.Append("<td " + ((StockValues[4] == "0" || values[values.Length - 3] == "0") ? Strickout : "") + " rowspan=\"" + Multilines.Length + "\" align=\"left\" width='80' >" + StockValues[0] + "</td>");
                            // This is for set the location Id
                            ltLocationID.Text = StockValues[1];
                            locationId = int.Parse(StockValues[1]);
                            sbResult.Append("<td  " + Strickout + "rowspan=\"" + Multilines.Length + "\" align=\"left\" width='40' >" + StockValues[2] + "</td>");

                            //This is for multiple lines
                            for (int i = 0; i < Multilines.Length; i++)
                            {

                                if (i == 0)
                                    sbResult.Append(BindTable(values, false));
                                else
                                {
                                    values = Multilines[i].Split('!');
                                    outerRow.Append(BindTable(values, true));
                                    if (outer)
                                    {
                                        if (values[values.Length - 3] != "0")
                                            outer = false;
                                    }
                                }
                            }


                            if (cp.TenantID == 0)
                            {
                                sbResult.Append("<td rowspan=\"" + Multilines.Length + "\" align=\"left\" width='50'>");

                                if (Strickout == "" || !outer)
                                {
                                    sbResult.Append("<nobr><a style='text-decoration:none;' href='" + String.Format("../mInventory/PickItem.aspx?mmid={0}&obdid={1}&LineNum={2}&locid={3}&kitid={4}&soid={5}'> Pick <img src='../Images/redarrowright.gif' border='0' /></nobr>", MaterialMasterID, OutboundID, LineNumber, locationId, CommonLogic.IIF(vKitID != "", vKitID, "NULL"), CommonLogic.IIF(vSOHID != "", vSOHID, "NULL")));
                                    flag = false;
                                }
                            }

                            sbResult.Append("</td></tr>");
                            sbResult.Append(outerRow);
                        }
                    }
                    sbResult.Append("</table>");

                }

                return sbResult.ToString();
            }
            else
            {
                return "No item in DB ";
            }
        }


        private String BindTable(String[] values, bool outer)
        {
            StringBuilder temp = new StringBuilder();
            String Strickout = "";
            if (values[values.Length - 3] == "0")
                //Strickout = "style=\"text-decoration:line-through;\"";
                Strickout = "class='StrikeOut'";
            else
                Strickout = "";
            if (outer)
                temp.Append("<tr " + Strickout + ">");
            int valueslength = values.Length;
            for (int j = 0; j < valueslength; j++)
            {
                if (j != valueslength - 3)
                {
                    if (values[j] == "Ÿ" || values[j] == "ž")
                    {
                        if (values[j] == "ž")
                            temp.Append("<td align=\"center\" width='40'> <img  src=\"../Images/blue_menu_icons/check_mark.png\"  /> </td>");
                        else
                            temp.Append("<td align=\"center\" width='40'></td>");
                    }
                    else
                        temp.Append("<td align=\"left\" width='100' style=\"min-width: 100px;\"" + Strickout + " >" + values[j] + "</td>");
                }
            }
            if (outer)
                temp.Append("</tr>");
            return temp.ToString();
        }

        protected void lnkPick_Click(object sender, EventArgs e)
        {
            //string MaterialMasterID = "";
            //string LineNumber = "";
            //string locationId = "";

            //string vSOHID = "";

            //string query = "EXEC [dbo].[sp_OBD_DeliveryPickNote] @OutboundId=" + OutboundID.ToString() + "";
            //DataSet ds = DB.GetDS(query, false);
            //if (ds.Tables[0].Rows.Count != 0)
            //{
            //    foreach (DataRow row in ds.Tables[0].Rows)
            //    {
            //        MaterialMasterID = row["MaterialMasterID"].ToString();
            //        LineNumber = row["LineNumber"].ToString();
            //        locationId = row["LocationID"].ToString();
            //        vSOHID = row["SOHeaderID"].ToString();
            //    }
            //    //String PickLink = "";
            //    Response.Redirect("../mInventory/PickItem.aspx?mmid=" + MaterialMasterID + "&obdid=" + OutboundID + "&LineNum=" + LineNumber + "&locid=" + locationId + "&kitid=null" + "&soid=" + vSOHID);
            //    //PickLink = "<nobr><a style='text-decoration:none;' href='" + String.Format("../mInventory/PickItem.aspx?mmid={0}&obdid={1}&LineNum={2}&locid={3}&kitid={4}&soid={5}'> Pick <img src='../Images/redarrowright.gif' border='0' /></nobr>", MaterialMasterID, OutboundID, LineNumber, locationId, null, vSOHID);
            //}
            //else
            //{
            //    resetError("No Item in DB", true);
            //    return;
            //}



        }

        [WebMethod]
        public static int InsertPickItem(int MMId, string OBDNo, string LineNo, string SOH, string LOc, string Mcode, decimal Qty, string Mfgdate, string Expdate, string BatchNo, string SerailNo, string Projrefno, string CartonCode, string ToCartonCode, string sodetailsid, string AssignId, string MRP,int HUNo,int HUSize)
        {
            cp = HttpContext.Current.User as CustomPrincipal;
            string status = "";
            try
            {
                StringBuilder dmlstatement = new StringBuilder();


                if (HUNo > 1)
                {
                    dmlstatement.Append("Exec [dbo].[SP_Upsert_HU_OBD_Items_Picking] ");
                    dmlstatement.Append("@OBDNumber=" + DB.SQuote(OBDNo));
                    dmlstatement.Append(",@LineNumber=" + LineNo);
                    dmlstatement.Append(",@SOHeaderID=" + SOH);
                    dmlstatement.Append(",@MCode=" + DB.SQuote(Mcode));
                    dmlstatement.Append(",@Location=" + DB.SQuote(LOc));
                    dmlstatement.Append(",@Quantity=" + Qty);
                    dmlstatement.Append(",@IsDamaged=0");
                    dmlstatement.Append(",@HasDiscrepancy=0");
                    dmlstatement.Append(",@CreatedBy=" + cp.UserID);
                    dmlstatement.Append(",@MfgDate=" + DB.DateQuote(Mfgdate));
                    dmlstatement.Append(",@ExpDate=" + DB.DateQuote(Expdate));
                    dmlstatement.Append(",@SerialNo=" + DB.SQuote(SerailNo));
                    dmlstatement.Append(",@BatchNo=" + DB.SQuote(BatchNo));
                    dmlstatement.Append(",@Projrefno=" + DB.SQuote(Projrefno));
                    dmlstatement.Append(",@CartonCode =" + DB.SQuote(CartonCode));
                    dmlstatement.Append(",@ToCartonCode =" + DB.SQuote(ToCartonCode));
                    dmlstatement.Append(",@AssignedId =" + AssignId);
                    dmlstatement.Append(",@SoDetailsIdnew=" + sodetailsid);
                    dmlstatement.Append(",@MRP=" + DB.SQuote(MRP));
                    dmlstatement.Append(",@AccountID=" + cp.UserID);
                    dmlstatement.Append(",@HUSize=" + HUSize);
                    dmlstatement.Append(",@HUNo=" + HUNo);
                }

                else
                {

                    //dmlstatement.Append("EXEC [dbo].[sp_INV_Upsert_GoodsMovement_DPN_NEW1]");
                    dmlstatement.Append("EXEC [dbo].[sp_INV_PickItemFromBin]");
                    dmlstatement.Append("@OBDNumber=" + DB.SQuote(OBDNo) + ",");
                    dmlstatement.Append("@SOHeaderID=" + DB.SQuote(SOH) + ",");
                    dmlstatement.Append("@AssignedId=" + DB.SQuote(AssignId) + ",");
                    dmlstatement.Append("@SoDetailsIdnew=" + sodetailsid + ",");
                    dmlstatement.Append("@AccountID=" + cp.AccountID + ",");
                    dmlstatement.Append("@LineNumber=" + DB.SQuote(LineNo) + ",");
                    dmlstatement.Append("@Location=" + DB.SQuote(LOc) + ",");
                    dmlstatement.Append("@MCode=" + DB.SQuote(Mcode) + ",");
                    dmlstatement.Append("@Quantity=" + Qty + ",");
                    dmlstatement.Append("@Mfgdate=" + DB.SQuote(Mfgdate) + ",");
                    dmlstatement.Append("@ExpDate=" + DB.SQuote(Expdate) + ",");
                    dmlstatement.Append("@BatchNo=" + DB.SQuote(BatchNo) + ",");
                    dmlstatement.Append("@SerialNo=" + DB.SQuote(SerailNo) + ",");
                    dmlstatement.Append("@CreatedBy=" + cp.UserID + ",");
                    dmlstatement.Append("@CartonCode=" + DB.SQuote(CartonCode) + ",");
                    dmlstatement.Append("@ToCartonCode=" + DB.SQuote(ToCartonCode) + ",");
                    //dmlstatement.Append("@ToCartonCode=" + DB.SQuote(CartonCode) + ",");
                    dmlstatement.Append("@Projrefno=" + DB.SQuote(Projrefno) + ",");
                    dmlstatement.Append("@MRP=" + DB.SQuote(MRP) + "");
                }


                return DB.GetSqlN(dmlstatement.ToString());/////-------------- added bu durga on 02/03/2018 for checking assign quantity with received quantity in database

            }
            catch (Exception e)
            {
                return -1;/////-------------- added bu durga on 02/03/2018 for checking assign quantity with received quantity in database
            }


        }

        [WebMethod]
        public static List<MRLWMSC21.mOutbound.BL.DeliveryPickNote> GetPickedItems(int MMId, string LOc, string CartonCode, string OBDNo, string BatchNo, string SerailNo, string Projrefno, string Expdate, string Mfgdate)
        {
            List<MRLWMSC21.mOutbound.BL.DeliveryPickNote> lst = new List<BL.DeliveryPickNote>();
            string Query = "EXEC [dbo].[GEN_OBD_ITEMSWISEPICKEDLIST] @MaterialMasterID = " + MMId + ", @CartonCode = " + DB.SQuote(CartonCode) + ",@AccountID=" + cp.AccountID + ", @OBD = " + DB.SQuote(OBDNo) + ", @Location = " + DB.SQuote(LOc) + ",   " +
                              " @BatchNo = " + DB.SQuote(BatchNo) + ", @SerialNo = " + DB.SQuote(SerailNo) + ", @ProjectRefNo = " + DB.SQuote(Projrefno) + ", @ExpDate = " + DB.SQuote(Expdate) + ", @MfgDate = " + DB.SQuote(Mfgdate) + "";

            DataSet DS = DB.GetDS(Query, false);
            if (DS.Tables[0].Rows.Count != 0)
            {
                foreach (DataRow row in DS.Tables[0].Rows)
                {
                    MRLWMSC21.mOutbound.BL.DeliveryPickNote DPN = new BL.DeliveryPickNote();
                    DPN.MCode = row["MCode"].ToString();
                    DPN.Location = row["Location"].ToString();
                    DPN.CartonCode = row["CartonCode"].ToString();
                    DPN.PickedQty = Convert.ToDecimal(row["PickedQty"].ToString());
                    DPN.AssignID = Convert.ToInt32(row["PickedID"].ToString());
                    DPN.StorageLocationID = Convert.ToInt32(row["DeliveryStatusID"]); // Here Delevery Status Id is displayed 

                    lst.Add(DPN);
                }
            }
            return lst;
        }





        #endregion -------------------------Delivery Pick Note  -----------------------------





        /*
         
          public string DisplayInTable(GridViewRow gvRow, String LineNumber, String OutboundID, String MaterialMasterID, String vKitID, string SplitData, Literal ltLocationID, String vSOHID)
        {
            if (SplitData != "")
            {
                StringBuilder sbResult = new StringBuilder();

                String ssss = "";
                char[] Locationseps = new char[] { ',' };
                String[] Locations = SplitData.Split(Locationseps);

                char[] StockTypeseps = new char[] { '|' };
                String[] StockValues;
                if (Locations.Length != 0)
                {
                    sbResult.Append("<table width='100%' border=\"1\"  style=\"border-collapse:collapse;border:0px solid black;\">");

                    foreach (String Loc in Locations)
                    {
                        StockValues = Loc.Split(StockTypeseps);
                        if (StockValues[2].ToString() == "-555.00")
                        {
                            sbResult.Append("<tr><td> MM Item is not configured to handle multiple UoM's for this Delv. Doc. Pls contact MMAdmin . </td></tr>");
                        }
                        else
                        {
                            String PickLink = "";
                            sbResult.Append("<tr>");
                            int locationId = 0;
                            for (var i = 0; i < StockValues.Length; i++)
                            {
                                if (i == 1)
                                {
                                    ltLocationID.Text = StockValues[i];
                                    locationId = int.Parse(StockValues[i]);
                                }
                                else if (i == 3)
                                {
                                    sbResult.Append("<td style=\"border-top-style:none;border-bottom-style:none;border-left-style:none;border-right-style:none;\" >");
                                    String[] locwiseMSP = StockValues[i].Split('@');

                                    String[] valuesforcount = locwiseMSP[0].Split('!');
                                    int count = valuesforcount.Length-1;
                                    int tablewidth = count * 80;
                                    sbResult.Append("<table width='" + tablewidth + "'border=\"1\" style=\"border-collapse:collapse;\">");

                                    for (var j = 0; j < locwiseMSP.Length; j++)
                                    {
                                        String[] values = locwiseMSP[j].Split('!');

                                        if (values[(values.Length - 3)] == "0")
                                            ssss = "style=\"text-decoration:line-through;\"";
                                        else
                                            ssss = "";

                                        sbResult.Append("<tr >");
                                        for (var k = 0; k < values.Length; k++)
                                        {
                                            // border-left-style:none;border-right-style:none;
                                            //style=\"border-top-style:none;border-bottom-style:none\";

                                            if (k != (values.Length - 3))
                                            {

                                                if (k == (values.Length - 5) || k == (values.Length - 6) || k == (values.Length - 4) || k == (values.Length - 7))
                                                {
                                                    if (values[k] != "0")
                                                    {
                                                        sbResult.Append("<td align='left' width='80'  "+ssss+" >");
                                                        sbResult.Append("<img  src=\"../Images/blue_menu_icons/check_mark.png\"  />");
                                                        sbResult.Append("</td>");
                                                    }
                                                    else
                                                    {
                                                        sbResult.Append("<td align='left' width='80' " + ssss + " >");
                                                        sbResult.Append("");
                                                        sbResult.Append("</td>");
                                                    }

                                                }
                                                else
                                                {
                                                    sbResult.Append("<td align='left' width='80' "+ssss+" >");
                                                    sbResult.Append(values[k]);
                                                    sbResult.Append("</td>");
                                                }
                                            }

                                        }


                                        sbResult.Append("</tr>");


                                    }

                                    sbResult.Append("</table>");
                                    sbResult.Append("</td>");
                                    PickLink = "<nobr><a style='text-decoration:none;' href='" + String.Format("../mInventory/PickItem.aspx?mmid={0}&obdid={1}&LineNum={2}&locid={3}&kitid={4}&soid={5}'> Pick <img src='../Images/redarrowright.gif' border='0' /></nobr>", MaterialMasterID, OutboundID, LineNumber, locationId, CommonLogic.IIF(vKitID != "", vKitID, "NULL"), CommonLogic.IIF(vSOHID != "", vSOHID, "NULL"));
                                    sbResult.Append("<td align='left' width='80'>");
                                    if (i == 4)
                                    {
                                        if (StockValues[i] == "0")
                                        {

                                        }
                                    }
                                    else
                                    {
                                        sbResult.Append(PickLink);
                                    }
                                    sbResult.Append("</td>");
                                }
                                else
                                {
                                    if (i != 4)
                                    {
                                        sbResult.Append("<td align='left' width='150'>");
                                        sbResult.Append(StockValues[i]);
                                        sbResult.Append("</td>");
                                    }

                                }

                            }

                            sbResult.Append("</tr>");

                        }
                    }

                    sbResult.Append("</table>");
                }

                return sbResult.ToString();
            }
            else
            {
                return "No item in DB ";
            }
        }
         
         
         
         
         
         
         
         */

    }
}