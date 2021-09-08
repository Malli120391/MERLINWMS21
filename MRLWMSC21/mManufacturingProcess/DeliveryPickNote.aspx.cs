using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MRLWMSC21Common;
using System.Data;
using System.Text;
namespace MRLWMSC21.mManufacturingProcess
{
    public partial class DeliveryPickNote : System.Web.UI.Page
    {
        CustomPrincipal cp = HttpContext.Current.User as CustomPrincipal;
        protected string OBDLineItemsListSQL = "";
        protected int OutboundID;
        protected OBDTrack thisOOBD;
        protected String OBDNumber;
        protected void Page_PreInit(object sender, EventArgs e)
        {
            Page.Theme = "Inbound";
        }

        
        protected void Page_Load(object sender, EventArgs e)
        {
            //DesignLogic.SetInnerPageSubHeading(this.Page, "Delivery Pick Note");

            string[] rolesAllowedInThisPage = CommonLogic.GetRolesAllowed("1,2,4,5,7,17");

            if (!cp.IsInAnyRoles(rolesAllowedInThisPage))
            {
                Response.Redirect("Login.aspx?eid=6");
            }
            //if (Request.UrlReferrer != null)
            //{
            //    lnkbackToList.PostBackUrl = Request.UrlReferrer.ToString();
            //}
            //else
            //{
            //    lnkbackToList.PostBackUrl = "PickingPending.aspx";
            //}
            OutboundID = DB.GetSqlN("select top 1 OutboundID AS N from OBD_Outbound where IsActive=1 and IsDeleted=0 AND OutboundID=" + CommonLogic.QueryStringUSInt("obdid"));

            if (OutboundID != 0)
            {
                thisOOBD = new OBDTrack(OutboundID);
            }

            if (!IsPostBack)
            {
                if (CommonLogic.QueryString("obdid")!="")
                ViewState["OutboundID"] = CommonLogic.QueryString("obdid");

                ViewState["TenantID"] = cp.TenantID;

                DesignLogic.SetInnerPageSubHeading(this.Page, "Delivery Pick Note");

                if (OutboundID != 0)
                {


                    ltDelvDocNo.Text = "<span class='FormLabels'> <b>  &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;  Delivery Doc. No. :" + thisOOBD.OBDNumber + "</b><span><br/><br/><img src=\" ../mInbound/Code39Handler.ashx?code=" + thisOOBD.OBDNumber + "\" />";
                    ltDelvDocDetails.Text = "<table cellpadding='2' cellspacing='1' border='0' >";
                    ltDelvDocDetails.Text += "<tr><td class='FormLabels' align='left' >Customer :</td><td class='FormLabels' align='left'>" + CommonLogic.GetCustomerName(thisOOBD.CustomerID.ToString()) + "</td></tr>";
                    ltDelvDocDetails.Text += "<tr><td class='FormLabels' align='left'>Requested By  :</td><td class='FormLabels' align='left'>" + CommonLogic.GetUserName(thisOOBD.RequestedByID.ToString()) + "</td></tr>";
                    ltDelvDocDetails.Text += "<tr><td class='FormLabels' align='left'>Delv. Doc. Date :</td><td class='FormLabels' align='left'>" + thisOOBD.OBDDate + "</td></tr>";


                    String SqlDelvryPick = "EXEC [sp_MFG_ProductionDeliveryPickNoteDetails]  @OutboundID=" + CommonLogic.QueryStringUSInt("obdid");

                    IDataReader dr = DB.GetRS(SqlDelvryPick);

                    while (dr.Read())
                    {

                        ltDelvDocDetails.Text += "<tr><td class='FormLabels' align='left'>SO Ref. Number :</td><td class='FormLabels' align='left'>" + DB.RSField(dr, "WONumber") + "</td></tr>";
                        ltDelvDocDetails.Text += "<tr><td class='FormLabels' align='left'>Job Order Ref.# :</td><td class='FormLabels' align='left'>" + DB.RSField(dr, "ProRefNo") + "</td></tr>";
                        if (!DB.RSField(dr, "JobOrderRefNo").Equals(""))
                            ltDelvDocDetails.Text += "<tr><td class='FormLabels' align='left'>Job Order# :</td><td class='FormLabels' align='left'>" + DB.RSField(dr, "JobOrderRefNo") + "</td></tr>";

                        if (DB.RSFieldInt(dr, "IsFromProductionOrder") == 0)
                        {
                            ltDelvDocDetails.Text += "<tr><td class='FormLabels' align='left'>NC Ref. Number :</td><td class='FormLabels' align='left'>" + DB.RSField(dr, "IORefNo") + "</td></tr>";
                        }

                        ltDelvDocDetails.Text += "<tr><td class='FormLabels' align='left'>Routing Type :</td><td class='FormLabels' align='left'>" + DB.RSField(dr, "RoutingDocumentType") + "</td></tr>";
                        ltDelvDocDetails.Text += "<tr><td class='FormLabels' align='left'>Kit Code :</td><td class='FormLabels' align='left'>" + DB.RSField(dr, "KitCode") + "</td></tr>";
                        ViewState["KitCode"] = DB.RSField(dr, "KitCode");


                    }

                    dr.Close();


                    ltDelvDocDetails.Text += "</table>";

                    OBDNumber = thisOOBD.OBDNumber;
                }else
                {
                    resetError("Invalid request",true);
                    return;
                }


                txtMCode.Attributes["onclick"] = "javascript:this.focus();";
                txtMCode.Attributes["onfocus"] = "javascript:this.select();";
                AddGridHeaderList(OutboundID);

                OBDLineItemsListSQL = "EXEC [sp_MFG_GetDeliverypickNoteForManufacturing]  @OutboundID=" + OutboundID.ToString() + ",@MCode=''";  // ,@OBDNumber=NULL,@MCode=NULL,@WarehouseID=" + DB.SQuote(String.Join(",", cp.Warehouses));

                ViewState["OBDLineItemsListSQL"] = this.OBDLineItemsListSQL;

                this.OBDLineItemsList_buildGridData(this.OBDLineItemsList_buildGridData());

            }
        }

        protected void lnkMCodeSearch_Click(object sender, EventArgs e)
        {
            if (txtMCode.Text == "Search Part # ...")
                txtMCode.Text = "";

            if (txtMCode.Text.Trim() != "")
                OBDLineItemsListSQL = " EXEC [sp_MFG_GetDeliverypickNoteForManufacturing] @OutboundID=" + OutboundID.ToString() + ",@MCode=" + DB.SQuote(txtMCode.Text.Trim());//+ ",@OBDNumber=NULL,@WarehouseID=NULL"; //+ DB.SQuote(String.Join(",", cp.Warehouses));
            else
                OBDLineItemsListSQL = "EXEC [sp_MFG_GetDeliverypickNoteForManufacturing]  @OutboundID=" + OutboundID.ToString() + ",@MCode=''";//,@OBDNumber=NULL,@WarehouseID=NULL";// + DB.SQuote(String.Join(",", cp.Warehouses));

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
            //ltPNCPRecordCount.Text = "Line Items - [" + ds.Tables[0].Rows.Count.ToString() + "]";
            ltPNCPRecordCount.Text = "Line Items - [" + ds.Tables[0].DefaultView.ToTable(true, "SODetailsID").Rows.Count.ToString() + "]";
            return ds;
        }

        protected void OBDLineItemsList_buildGridData(DataSet ds)
        {
            gvPOLineQty.DataSource = ds.Tables[0];
            gvPOLineQty.DataBind();
            ds.Dispose();
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

                String vltSplitLocation = ((Literal)e.Row.Cells[6].FindControl("ltSplitLocation")).Text;
                String MaterialMasterID = ((Literal)e.Row.Cells[2].FindControl("ltMMID")).Text;
                String LineNumber = ((Literal)e.Row.Cells[0].FindControl("ltLineNumber")).Text;
                String vKitID = ((Literal)e.Row.Cells[0].FindControl("hidKitPlannerID")).Text;
                String OutboundID = CommonLogic.QueryString("obdid");
                String vSOHID = ((Literal)e.Row.Cells[2].FindControl("ltSOHID")).Text;

                Literal ltLocation = (Literal)e.Row.Cells[6].FindControl("ltLocation");
                Literal ltLocationID = (Literal)e.Row.Cells[6].FindControl("ltLocationID");
                ltLocation.Text = DisplayInTableNew(e.Row, LineNumber, OutboundID, MaterialMasterID, vKitID, vltSplitLocation, ltLocationID, vSOHID);

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

            //DataTable table = dataset.Tables[0];
            //int count = table.Rows.Count;
            //count = count +7;
            //int tablewidth = count * 80;
            //StringBuilder HeaderText = new StringBuilder();
            //HeaderText.Append("<table width='100%' border=\"1\" style=\"border-collapse:collapse;\"><tr><td align=\"left\" width='100'>Location&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp</td><td align=\"left\" width='100'>KitID&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp</td>");

            //HeaderText.Append("<td><table width='" + tablewidth + "' border=\"1\" style=\"border-collapse:collapse;\"><tr>");
            //while (dsDynamicMSp.Read())
            //{
            //    String columnname = DB.RSField(dsDynamicMSp, "DisplayName");
            //    HeaderText.Append("<td  align=\"left\" width='80'>" + columnname + "</td>");
            //}

            //HeaderText.Append("<td align=\"left\" width='80'>Is Dam.</td><td align=\"left\" width='80'>Has Dis.</td><td align=\"left\" width='80'>Is NC</td><td align=\"left\" width='80'>As Is</td><td align=\"left\" width='80'>Positive Recall</td> <td align=\"left\" width='80'>Available</td><td align=\"left\" width='80'>Picked</td>");
            //HeaderText.Append("</tr></table></td>");
            //HeaderText.Append("<td align=\"left\" width='50'>PickLink</td></tr></table>");
            //gvPOLineQty.Columns[7].HeaderText = HeaderText.ToString();



            DataTable table = dataset.Tables[0];
            int count = table.Rows.Count;
            count = count + 7;
            int tablewidth = count * 80;
            StringBuilder HeaderText = new StringBuilder();
            HeaderText.Append("<table border=\"1\" width='100%' style=\"border-collapse:collapse;\"><tr><td align=\"left\" width='80'>Location</td><td align=\"left\" width=40>KitID</td>");
            

            //HeaderText.Append("<td><table width='" + tablewidth + "' border=\"1\" style=\"border-collapse:collapse;\"><tr>");
            while (dsDynamicMSp.Read())
            {
                String columnname = DB.RSField(dsDynamicMSp, "DisplayName");
                HeaderText.Append("<td  align=\"left\" width='100'>" + columnname + "</td>");
            }

            HeaderText.Append("<td align=\"left\" width='40' style=\"border-left-style:none;border-top-style:none;border-bottom-style:none;\">Is Dam.</td><td align=\"left\" width='40'>Has Dis.</td><td align=\"left\" width='40'>Is NC</td><td align=\"left\" width='40'>As Is</td><td align=\"left\" width='40'>Pve Recall</td> <td align=\"left\" width='100'>Aval.</td><td align=\"left\" width='100'>Picked</td>");
            //HeaderText.Append("</tr></table></td>");
            HeaderText.Append("<td class=\"Picked\" align=\"left\" width='50'>Pick Link</td></tr></table>");
            gvPOLineQty.Columns[7].HeaderText = HeaderText.ToString();
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
                                    int count = valuesforcount.Length;
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
                                            PickLink = "<nobr><a style='text-decoration:none;' href='" + String.Format("../mInventory/PickItem.aspx?mmid={0}&obdid={1}&LineNum={2}&locid={3}&kitid={4}&soid={5}&kitcode={6}'> Pick <img src='../Images/redarrowright.gif' border='0' /></nobr>", MaterialMasterID, OutboundID, LineNumber, locationId, CommonLogic.IIF(vKitID != "", vKitID, "NULL"), CommonLogic.IIF(vSOHID != "", vSOHID, "NULL"), ViewState["KitCode"]);
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
                                    PickLink = "<nobr><a style='text-decoration:none;' href='" + String.Format("../mInventory/PickItem.aspx?mmid={0}&obdid={1}&LineNum={2}&locid={3}&kitid={4}&soid={5}&kitcode={6}'> Pick <img src='../Images/redarrowright.gif' border='0' /></nobr>", MaterialMasterID, OutboundID, LineNumber, locationId, CommonLogic.IIF(vKitID != "", vKitID, "NULL"), CommonLogic.IIF(vSOHID != "", vSOHID, "NULL"), ViewState["KitCode"]);
                                    
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
                            if (values[values.Length - 3] == "0")
                            {
                                //Strickout = "style=\"text-decoration:line-through;\"";
                                Strickout = "class='StrikeOut'";
                            }
                            else
                            {
                                Strickout = "";
                            }
                            sbResult.Append("<tr>");
                            //sbResult.Append("<td " + (StockValues[4] == "0" ? Strickout : "") + " rowspan=\"" + Multilines.Length + "\" align=\"left\" width='80' >" + StockValues[0] + "</td>");
                            sbResult.Append("<td rowspan=\"" + Multilines.Length + "\" align=\"left\" width='80' >" + StockValues[0] + "</td>");
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
                            sbResult.Append("<td class=\"Picked\" rowspan=\"" + Multilines.Length + "\" align=\"left\" width='50'>");
                            if (Strickout == "" || !outer)
                            {
                                sbResult.Append("<nobr><a style='text-decoration:none;' href='" + String.Format("../mInventory/PickItem.aspx?mmid={0}&obdid={1}&LineNum={2}&locid={3}&kitid={4}&soid={5}&kitcode={6}'> Pick <img src='../Images/redarrowright.gif' border='0' /></nobr>", MaterialMasterID, OutboundID, LineNumber, locationId, CommonLogic.IIF(vKitID != "", vKitID, "NULL"), CommonLogic.IIF(vSOHID != "", vSOHID, "NULL"), ViewState["KitCode"]));
                                flag = false;
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
                        temp.Append("<td align=\"left\" width='100' " + Strickout + " >" + values[j] + "</td>");
                }
            }
            if (outer)
                temp.Append("</tr>");
            return temp.ToString();
        }


        #endregion -------------------------Delivery Pick Note  -----------------------------






        /*
        protected DataSet OBDLineItemsList_buildGridData()
        {
            string sql = ViewState["OBDLineItemsListSQL"].ToString();


            DataSet ds = DB.GetDS(sql, false);
           
            ltPNCPRecordCount.Text = "Line Items - [" + ds.Tables[0].Rows.Count.ToString() + "]";
           
            return ds;

        }
        private void AddGridHeaderList(int OBDID)
        {
            IDataReader dsDynamicMSp = DB.GetRS("[dbo].[sp_MFG_GetMaterialStorageParametersFORDPN]@OutBoundID="+OBDID+ ",@TenantID=" + cp.TenantID);
            DataSet dataset = DB.GetDS("[dbo].[sp_MFG_GetMaterialStorageParametersFORDPN]@OutBoundID=" + OBDID + ",@TenantID=" + cp.TenantID,false);
            DataTable table = dataset.Tables[0];
            int count = table.Rows.Count;
            count = count + 2;
            int tablewidth = count * 80;
            StringBuilder HeaderText = new StringBuilder();
            HeaderText.Append("<table width='100%' border=\"1\" style=\"border-collapse:collapse;\"><tr><td>Location&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp</td><td align=\"left\" width='100'>KitID&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp</td>");

            HeaderText.Append("<td><table width='" + tablewidth + "' border=\"1\" style=\"border-collapse:collapse;\"><tr>");
            while (dsDynamicMSp.Read())
            {
                String columnname = DB.RSField(dsDynamicMSp, "ParameterName");
                HeaderText.Append("<td  align=\"left\" width='80'>" + columnname + "</td>");
            }
           
            HeaderText.Append("<td align=\"left\" width='80'>IsDam</td><td align=\"left\" width='80'>Available</td><td align=\"left\" width='80'>Picked</td>");
            HeaderText.Append("</tr></table></td>");
            HeaderText.Append("<td align=\"left\" width='50'>PickLink</td></tr></table>");
            gvPOLineQty.Columns[5].HeaderText = HeaderText.ToString();
        }
        protected void OBDLineItemsList_buildGridData(DataSet ds)
        {
            gvPOLineQty.DataSource = ds.Tables[0];
            gvPOLineQty.DataBind();
            ds.Dispose();
        }

        
        protected void gvPOLineQty_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }

        protected void gvPOLineQty_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                String vltSplitLocation = ((Literal)e.Row.Cells[5].FindControl("ltSplitLocation")).Text;
               String MaterialMasterID= ((Literal)e.Row.Cells[2].FindControl("ltMMID")).Text;
                 String LineNumber= ((Literal)e.Row.Cells[0].FindControl("ltLineNumber")).Text;
                String vKitID= ((Literal)e.Row.Cells[0].FindControl("hidKitPlannerID")).Text;
                String OutboundID= ((Literal)e.Row.Cells[2].FindControl("ltOBDTrackingID")).Text;
                String vSOHID = ((Literal)e.Row.Cells[2].FindControl("ltSOHID")).Text;
                
                Literal ltLocation = (Literal)e.Row.Cells[5].FindControl("ltLocation");
                Literal ltLocationID = (Literal)e.Row.Cells[5].FindControl("ltLocationID");
                ltLocation.Text = DisplayInTable(e.Row, LineNumber, OutboundID, MaterialMasterID, vKitID, vltSplitLocation, ltLocationID, vSOHID);
            }

        }
        public string DisplayInTable(GridViewRow gvRow, String LineNumber, String OutboundID, String MaterialMasterID, String vKitID, string SplitData, Literal ltLocationID, String vSOHID)
        {
            if (SplitData != "")
            {
                StringBuilder sbResult = new StringBuilder();

                
                char[] Locationseps = new char[] { ',' };
                String[] Locations = SplitData.Split(Locationseps);

                char[] StockTypeseps = new char[] { '|' };
                String[] StockValues;
                if (Locations.Length != 0)
                {
                    sbResult.Append("<table width='100%' border=\"1\" align=\"center\" cellpadding='1'  style=\"border-collapse:collapse;border:0px solid black;\">");

                    foreach (String Loc in Locations)
                    {
                        StockValues = Loc.Split(StockTypeseps);
                        if (StockValues[2].ToString() == "-555.00")
                        {
                            sbResult.Append("<tr><td colspan='4'> MM Item is not configured to handle multiple UoM's for this Delv. Doc. Pls contact MMAdmin . </td></tr>");
                        }
                        else
                        {
                            String PickLink = "";
                            sbResult.Append("<tr>");
                            int locationId=0;
                            for (var i = 0; i < StockValues.Length; i++)
                            {
                                if (i== 1)
                                {
                                    ltLocationID.Text = StockValues[i];
                                    locationId=int.Parse( StockValues[i]);
                                }
                                else if (i==3)
                                {
                                    sbResult.Append("<td style=\"border-top-style:none;border-bottom-style:none;border-left-style:none;border-right-style:none;\" >");
                                    String[] locwiseMSP=StockValues[i].Split('@');
                                    String[] valuesforcount = locwiseMSP[0].Split('!');
                                    int count = valuesforcount.Length;
                                    int tablewidth = count *80;
                                    sbResult.Append("<table width='"+tablewidth+"'border=\"1\" style=\"border-collapse:collapse;\">");

                                    for (var j = 0; j < locwiseMSP.Length; j++)
                                    {
                                        String[] values = locwiseMSP[j].Split('!');
                                        sbResult.Append("<tr>");
                                        for (var k = 0; k < values.Length; k++)
                                        {
                                            sbResult.Append("<td align='left' width='80'>");
                                            sbResult.Append(values[k]);
                                            sbResult.Append("</td>");
                                        }
                                       
                               
                                        sbResult.Append("</tr>");
                                       

                                    }
                                    
                                    sbResult.Append("</table>");
                                    sbResult.Append("</td>");
                                    PickLink = "<nobr><a style='text-decoration:none;' href='" + String.Format("../mInventory/PickItem.aspx?mmid={0}&obdid={1}&LineNum={2}&locid={3}&kitid={4}&soid={5}'> Pick <img src='../Images/redarrowright.gif' border='0' /></nobr>", MaterialMasterID, OutboundID, LineNumber, locationId, CommonLogic.IIF(vKitID != "", vKitID, "NULL"), CommonLogic.IIF(vSOHID != "", vSOHID, "NULL"));
                                    sbResult.Append("<td align='left' width='50'>");
                                    sbResult.Append(PickLink);
                                    sbResult.Append("</td>");
                                }
                                else
                                {
                                    sbResult.Append("<td align='left' width='80'>");
                                    sbResult.Append(StockValues[i]);
                                    sbResult.Append("</td>");
                                   
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
        }*/



    }
}