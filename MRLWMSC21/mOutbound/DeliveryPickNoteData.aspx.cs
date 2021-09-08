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
using System.IO;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using System.Web.UI.HtmlControls;

// Module Name : DeliveryPickNote Under Outbound
// Description : 
// DevelopedBy : Naresh P
// CreatedOn   : 26/11/2013
// Lsst Modified Date : 

namespace MRLWMSC21.mOutbound
{
    public partial class DeliveryPickNoteData : System.Web.UI.Page
    {

        public CustomPrincipal cp = HttpContext.Current.User as CustomPrincipal;

        protected string OBDLineItemsListSQL = "";
        protected string OBDLineItemsListSQL_Customer = "";
        protected int OutboundID;
        protected String OBDNumber;
        protected OBDTrack thisOOBD;

        protected void Page_PreInit(object sender, EventArgs e)
        {
            Page.Theme = "Outbound";
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            DesignLogic.SetInnerPageSubHeading(this.Page, "Delivery Note");

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
                string _sDelvStatusId = DB.GetDS("SELECT DeliveryStatusID FROM OBD_Outbound WHERE OutboundID=" + OutboundID, false).Tables[0].Rows[0][0].ToString();
                hdnDelvStatusId.Value = _sDelvStatusId;
            }


            if (!IsPostBack)
            {
                if (CommonLogic.QueryString("obdid") != "")
                    ViewState["OutboundID"] = CommonLogic.QueryString("obdid");
                ViewState["TenantID"] = cp.TenantID;
                if (OutboundID != 0 && CommonLogic.QueryString("lineitemcount").ToString() != "0")
                {
                    DataSet ds = DB.GetDS("EXEC [sp_OBD_GetDeliveryNote_Header] @OutboundID=" + ViewState["OutboundID"], false);
                    string dlvrDate = ""; string dlvrTime = "";
                    if (ds != null)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            if (ds.Tables[0].Rows[0]["DeliveryDate"].ToString() != "")
                            {
                                dlvrDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["DeliveryDate"].ToString()).ToString("dd/MM/yyyy");
                                dlvrTime = Convert.ToDateTime(ds.Tables[0].Rows[0]["DeliveryDate"].ToString()).ToString("hh:mm tt");
                            }

                            ltCustomerName.Text = ds.Tables[0].Rows[0]["CustomerName"].ToString();
                            //ltShipmentID.Text = ds.Tables[0].Rows[0]["OBDNumber"].ToString();
                            ltShipmentID.Text = ds.Tables[0].Rows[0]["OutboundNumber"].ToString();
                            ltShipmentID1.Text = ds.Tables[0].Rows[0]["OutboundNumber"].ToString();
                            ltSONumber.Text = ds.Tables[0].Rows[0]["SONumber"].ToString();
                            ltInvNo.Text = ds.Tables[0].Rows[0]["InvoiceNo"].ToString();
                            ltAddress1.Text = ds.Tables[0].Rows[0]["Address1"].ToString();
                            //ltRefNo.Text = ds.Tables[0].Rows[0][""].ToString();
                            ltAddress2.Text = ds.Tables[0].Rows[0]["Address2"].ToString();
                            ltVehicleNo.Text = ds.Tables[0].Rows[0]["EquipmentName"].ToString();
                            ltTelephone.Text = ds.Tables[0].Rows[0]["Phone1"].ToString();
                            ltDeliveryDate.Text = dlvrDate;
                            ltFax.Text = ds.Tables[0].Rows[0]["Fax"].ToString();
                            ltDeliveryTime.Text = dlvrTime;
                            ltContactPerson.Text = ds.Tables[0].Rows[0]["PCP"].ToString();
                        }
                        string file = DB.GetSqlS("SELECT LogoPath AS S FROM GEN_Account WHERE AccountID=" + cp.AccountID);
                        if (file != "")
                        {
                            if (File.Exists(Server.MapPath("../TPL/AccountLogos/" + file)))
                            {
                                Img1.Attributes["src"] = ResolveUrl("~/TPL/AccountLogos/" + file);
                                Img1.Visible = true;
                            }
                        }
                    }

                    //string dlvrDate = ds.Tables[0].Rows[0]["DeliveryDate"].ToString() == "" ? "" : Convert.ToDateTime(ds.Tables[0].Rows[0]["DeliveryDate"].ToString()).ToString("dd/MM/yyyy hh:mm tt");
                    //ltDelvDocNo.Text = "<b>Delivery Doc. No. :" + thisOOBD.OBDNumber+"</b><br>Delivery Date :"+ dlvrDate;

                    //ltDelvDocDetails.Text = "<table style='display:none;' cellpadding='2' cellspacing='1' border='0' >";
                    //ltDelvDocDetails.Text += "<tr><td  align='left' >Tenant Name :</td><td  align='left'>" + CommonLogic.QueryString("TN") + "</td></tr>";
                    //ltDelvDocDetails.Text += "<tr><td  align='left' >Customer :</td><td  align='left'>" + CommonLogic.GetCustomerName(thisOOBD.CustomerID.ToString()) + "</td></tr>";
                    //ltDelvDocDetails.Text += "<tr><td  align='left'>Requested By  :</td><td  align='left'>" + CommonLogic.GetUserName(thisOOBD.RequestedByID.ToString()) + "</td></tr>";
                    //ltDelvDocDetails.Text += "<tr><td  align='left'>Delv. Doc. Date :</td><td  align='left'>" + thisOOBD.OBDDate+ "</td></tr>";
                    //ltDelvDocDetails.Text += "</table>";



                    //ltDelvDocDetails.Text += "<table style='float:right;' class='tblDelNoteDetails' cellpadding='2' cellspacing='1' border='0' >";
                    //ltDelvDocDetails.Text += "<tr><td  align='left' >Delivery To </td><td  align='left'>: " + ds.Tables[0].Rows[0]["Customername"]+" </td></tr>";
                    //ltDelvDocDetails.Text += "<tr><td valign='top' align='left' >Address </td><td  align='left'>: " + ds.Tables[0].Rows[0]["Address1"] + ", " + ds.Tables[0].Rows[0]["Address2"] + "," + ds.Tables[0].Rows[0]["City"] + ",<br> &nbsp;&nbsp;&nbsp;" + ds.Tables[0].Rows[0]["State"] + "," + ds.Tables[0].Rows[0]["Zip"] + ". </td></tr>";
                    //ltDelvDocDetails.Text += "<tr><td  align='left'>Shipment ID  </td><td  align='left'>: </td></tr>";
                    //ltDelvDocDetails.Text += "<tr><td  align='left'>Reference </td><td  align='left'>: </td></tr>";
                    //ltDelvDocDetails.Text += "<tr><td  align='left'>Vehicle No. </td><td  align='left'>: </td></tr>";
                    //ltDelvDocDetails.Text += "<tr><td  align='left'>Delivery Type </td><td  align='left'>: " + ds.Tables[0].Rows[0]["DocumentType"] + "</td></tr>";
                    //ltDelvDocDetails.Text += "</table>";

                    OBDNumber = thisOOBD.OBDNumber;


                    txtMCode.Attributes["onclick"] = "javascript:this.focus();";
                    txtMCode.Attributes["onfocus"] = "javascript:this.select();";


                    AddGridHeaderList(OutboundID);
                    AddGridHeaderList_Customer(OutboundID);

                    //OBDLineItemsListSQL = "EXEC [sp_OBD_GetDeliveryPickNote]  @OutboundID=" + OutboundID.ToString() + ",@WarehouseID=NULL,@MCode=''";  // ,@OBDNumber=NULL,@MCode=NULL,@WarehouseID=" + DB.SQuote(String.Join(",", cp.Warehouses));
                    OBDLineItemsListSQL = "EXEC [sp_OBD_GetDeliveryPickNote_TC]  @OutboundID=" + OutboundID.ToString() + ",@WarehouseID=" + DB.SQuote(String.Join(",", cp.Warehouses)) + ",@MCode=''";  // ,@OBDNumber=NULL,@MCode=NULL,@WarehouseID=" + DB.SQuote(String.Join(",", cp.Warehouses));
                    OBDLineItemsListSQL_Customer = "EXEC [sp_OBD_GetDeliveryPickNote_TC_Customer]  @OutboundID=" + OutboundID.ToString() + ",@WarehouseID=" + DB.SQuote(String.Join(",", cp.Warehouses)) + ",@MCode=''";  // ,@OBDNumber=NULL,@MCode=NULL,@WarehouseID=" + DB.SQuote(String.Join(",", cp.Warehouses));
                    ViewState["OBDLineItemsListSQL"] = this.OBDLineItemsListSQL;
                    ViewState["OBDLineItemsListSQL_Customer"] = this.OBDLineItemsListSQL_Customer;

                    this.OBDLineItemsList_buildGridData(this.OBDLineItemsList_buildGridData());
                    this.OBDLineItemsList_buildGridData_Customer(this.OBDLineItemsList_buildGridData_Customer());
                }


            }
        }

        protected void lnkMCodeSearch_Click(object sender, EventArgs e)
        {

            if (txtMCode.Text == "Search Part # ...")
                txtMCode.Text = "";

            if (txtMCode.Text.Trim() != "")
                OBDLineItemsListSQL = " EXEC [sp_OBD_GetDeliveryPickNote_TC] @OutboundID=" + OutboundID.ToString() + ",@OBDNumber=NULL,@MCode=" + DB.SQuote(txtMCode.Text.Trim()) + ",@WarehouseID=" + DB.SQuote(String.Join(",", cp.Warehouses)); //+ DB.SQuote(String.Join(",", cp.Warehouses));
            else
                OBDLineItemsListSQL = "EXEC [sp_OBD_GetDeliveryPickNote_TC]  @OutboundID=" + OutboundID.ToString() + ",@OBDNumber=NULL,@MCode='',@WarehouseID=" + DB.SQuote(String.Join(",", cp.Warehouses));// + DB.SQuote(String.Join(",", cp.Warehouses));

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
            ltPNCPRecordCount.Text = "Line Items - [" + ds.Tables[0].Rows.Count.ToString() + "]";
            //ltPNCPRecordCount.Text = "Line Items - [" + ds.Tables[0].DefaultView.ToTable(true,"MCode").Rows.Count.ToString() + "]";
            //DefaultView.ToTable(true, "employeeid")

            return ds;
        }

        protected void OBDLineItemsList_buildGridData(DataSet ds)
        {
            //DataTable dt = new DataTable();
            ds.Tables[0].Columns.Add("Image");
            foreach (DataTable table in ds.Tables)
            {
                foreach (DataRow dr in table.Rows)
                {
                    dr["Image"] = LoadMMItemData(Convert.ToInt32(dr["MaterialMasterID"]));
                    //// Iterates through the rows of the GridView control
                    //foreach (GridViewRow row in gvPOLineQty.Rows)
                    //{
                    //    // Selects the text from the TextBox
                    //    // which is inside the GridView control
                    //    //string ltPicHolder = ((Literal)row.FindControl("ltPicHolder")).Text;
                    //    ((Literal)row.FindControl("ltPicHolder")).Text=LoadMMItemData(Convert.ToInt32(dr["MaterialMasterID"]));
                    //}
                }

            }

            gvPOLineQty.DataSource = ds.Tables[0];
            gvPOLineQty.DataBind();
            ds.Dispose();
        }
        public string LoadMMItemData(int mid)
        {
            string TenantRootDir = "";
            string MMTPath = "";
            string TenantIDLS = "0";
            string _MCode = "";
            //Get Attatchment file
            //String sFileName = CommonLogic._GetAttatchmentFile(TenantRootDir + DB.GetSqlS("select UniqueID AS S from GEN_Tenant where TenantID=" + TenantID) + MMTPath, _MCode);
            TenantRootDir = DB.GetSqlS("select DefaultValue AS S from SYS_SystemConfiguration SYS_C left join SYS_SysConfigKey SYS_K on SYS_K.SysConfigKeyID=SYS_C.SysConfigKeyID where SysConfigKey='TenantContentPath'");

            // Get MaterialMaster Path
            MMTPath = DB.GetSqlS(" select DefaultValue AS S from SYS_SystemConfiguration SYS_C left join SYS_SysConfigKey SYS_K on SYS_K.SysConfigKeyID=SYS_C.SysConfigKeyID where SysConfigKey='MaterialManagementPath'");


            DataSet mt = DB.GetDS("select MCode, TenantID from MMT_MaterialMaster where isactive=1 and isdeleted=0 and MaterialMasterID=" + mid, false);
            if (mt.Tables[0].Rows.Count > 0)
            {
                _MCode = mt.Tables[0].Rows[0]["MCode"].ToString();
                TenantIDLS = mt.Tables[0].Rows[0]["TenantID"].ToString();
            }
            int TenantIDL = Convert.ToInt32(TenantIDLS);

            string sFileName = CommonLogic._GetAttatchmentFile(TenantRootDir + TenantIDL + MMTPath, _MCode);

            return sFileName;

            //if (sFileName != "")
            //{
            //    ltPicHolder.Text = "<img border='0' height='140' width='180' src='../" + sFileName + "'/>";
            //}
            //else
            //{
            //    ltPicHolder.Text = "";
            //}

        }

        protected DataSet OBDLineItemsList_buildGridData_Customer()
        {
            string sql = ViewState["OBDLineItemsListSQL_Customer"].ToString();
            DataSet ds = DB.GetDS(sql, false);
            ltPNCPRecordCount.Text = "Line Items - [" + ds.Tables[0].Rows.Count.ToString() + "]";
            //ltPNCPRecordCount.Text = "Line Items - [" + ds.Tables[0].DefaultView.ToTable(true,"MCode").Rows.Count.ToString() + "]";
            //DefaultView.ToTable(true, "employeeid")

            return ds;
        }


        protected void OBDLineItemsList_buildGridData_Customer(DataSet ds)
        {
            ds.Tables[0].Columns.Add("Image");
            foreach (DataTable table in ds.Tables)
            {
                foreach (DataRow dr in table.Rows)
                {
                    dr["Image"] = LoadMMItemData(Convert.ToInt32(dr["MaterialMasterID"]));
                }
            }
            gvPOLineQty1.DataSource = ds.Tables[0];
            gvPOLineQty1.DataBind();
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
                ddl.Items.Add(new System.Web.UI.WebControls.ListItem(arrVal, arrVal));
            }

        }

        private void LoadUoM(DropDownList ddlUoM, String selVal)
        {

            ddlUoM.Items.Clear();

            ddlUoM.Items.Add(new System.Web.UI.WebControls.ListItem("Select UoM", "0"));
            IDataReader rsCur = DB.GetRS("Select MMSKUID, MMSKU from MMSKU Where IsActive =1");

            while (rsCur.Read())
            {
                ddlUoM.Items.Add(new System.Web.UI.WebControls.ListItem(rsCur["MMSKU"].ToString(), rsCur["MMSKUID"].ToString()));

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
            HeaderText.Append("<table class=\"tblGridInnerTable\" style=\"border-collapse:collapse;width:100%;\"><tr><td align=\"center\" width='100' style='display:none' class='HideInPickNote'>Location</td><td align=\"center\" width='150'  class='HideInPickNote' style='display:none'>Container Code</td>"); //<td align=\"left\" width='40'>KitID</td>

            //HeaderText.Append("<td><table width='" + tablewidth + "' border=\"1\" style=\"border-collapse:collapse;\"><tr>");
            while (dsDynamicMSp.Read())
            {
                String columnname = DB.RSField(dsDynamicMSp, "DisplayName");
                if (columnname != "Exp. Date" && columnname != "Batch No.") // excluding Exp. Date , Batch No.
                    HeaderText.Append("<td  align=\"center\" width='100'>" + columnname + "</td>");
            }

            HeaderText.Append("<td align=\"center\" class='tdIsDmg' width='40' style=\"border-left-style:none;border-top-style:none;border-bottom-style:none;display:none;\">Is Dmg.</td> <td align=\"center\" width='100' style='display:none;'>Aval.</td><td align=\"right\" style='text-align:right !important;font-weight:700 !important;'>Good Qty.</td><td align=\"right\" class=\"tdDamagedQty\" style='text-align:right !important;font-weight:700 !important;'>Damage Qty.</td>");
            //HeaderText.Append("</tr></table></td>");
            if (cp.TenantID == 0)
            {
                //HeaderText.Append("<td align=\"center\" class=\"tdPickLink\" width='50'>Pick Link</td></tr></table>");
                HeaderText.Append("</tr></table>");
            }
            else
            {
                HeaderText.Append("</tr></table>");
            }

            gvPOLineQty.Columns[9].HeaderText = HeaderText.ToString();
        }
        private void AddGridHeaderList_Customer(int OBDID)
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
            HeaderText.Append("<table class=\"tblGridInnerTable\" style=\"border-collapse:collapse;width:100%;\"><tr><td align=\"center\" width='100' style='display:none' class='HideInPickNote'>Location</td><td align=\"center\" width='150'  class='HideInPickNote' style='display:none'>Container Code</td>"); //<td align=\"left\" width='40'>KitID</td>

            //HeaderText.Append("<td><table width='" + tablewidth + "' border=\"1\" style=\"border-collapse:collapse;\"><tr>");
            while (dsDynamicMSp.Read())
            {
                String columnname = DB.RSField(dsDynamicMSp, "DisplayName");
                if (columnname != "Exp. Date" && columnname != "Batch No.") // excluding Exp. Date , Batch No.
                    HeaderText.Append("<td  align=\"center\" width='100'>" + columnname + "</td>");
            }

            HeaderText.Append("<td align=\"center\" class='tdIsDmg' width='40' style=\"border-left-style:none;border-top-style:none;border-bottom-style:none;display:none;\">Is Dmg.</td> <td align=\"center\" width='100' style='display:none;'>Aval.</td><td align=\"right\"  style='text-align:right !important;font-size:12px !important;font-weight:bold !important;' class=\"tdStyle\">Good Qty.</td><td align=\"right\" class=\"tdDamagedQty\" style='text-align:right !important;font-size:12px !important;font-weight:bold !important;'>Damage Qty.</td>");
            //HeaderText.Append("</tr></table></td>");
            if (cp.TenantID == 0)
            {
                HeaderText.Append("<td align=\"center\" class=\"tdPickLink\" width='50' hidden>Pick Link</td></tr></table>");
            }
            else
            {
                HeaderText.Append("</tr></table>");
            }

            gvPOLineQty1.Columns[9].HeaderText = HeaderText.ToString();
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
                                                        sbResult.Append("&emsp;");
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
                                            PickLink = "<nobr><a style='text-decoration:none;' href='" + String.Format("../mInventory/PickItem.aspx?mmid={0}&obdid={1}&LineNum={2}&locid={3}&kitid={4}&soid={5}'> Pick </nobr>", MaterialMasterID, OutboundID, LineNumber, locationId, CommonLogic.IIF(vKitID != "", vKitID, "NULL"), CommonLogic.IIF(vSOHID != "", vSOHID, "NULL")); //<img src='../Images/redarrowright.gif' border='0' />
                                            sbResult.Append("<td align='left' width='50' " + ssss + " >");
                                            sbResult.Append(PickLink);
                                            sbResult.Append("</td>");
                                        }
                                        else
                                        {
                                            sbResult.Append("<td align='left' width='50' >");
                                            sbResult.Append("&emsp;");
                                            sbResult.Append("</td>");
                                        }

                                        sbResult.Append("</tr>");


                                    }

                                    sbResult.Append("</table>");
                                    sbResult.Append("</td>");

                                    PickLink = "<nobr><a style='text-decoration:none;' href='" + String.Format("../mInventory/PickItem.aspx?mmid={0}&obdid={1}&LineNum={2}&locid={3}&kitid={4}&soid={5}'> Pick </nobr>", MaterialMasterID, OutboundID, LineNumber, locationId, CommonLogic.IIF(vKitID != "", vKitID, "NULL"), CommonLogic.IIF(vSOHID != "", vSOHID, "NULL"));//<img src='../Images/redarrowright.gif' border='0' />

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
                return " ";
            }
        }
        #region for print page by kashyap
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
                int cartonId;
                if (Locations.Length != 0)
                {
                    sbResult.Append("<table class=\"tblGridInnerTable\" style=\"border-collapse:collapse;width:100%;\">");
                    foreach (String Loc in Locations)
                    {
                        outerRow.Clear();
                        outer = true;

                        //TCA1RAM0110|1750||!!!Ÿ!Ÿ!1!1.00!1.00|1|2016TCA10000001|4005

                        //!!!Ÿ!Ÿ!1!1.00!1.00

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
                                if (values[values.Length - 3] == "0" || StockValues[StockValues.Length - 2] == "0")
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
                            sbResult.Append("<td " + ((StockValues[4] == "0" || values[values.Length - 3] == "0") ? Strickout : "") + " rowspan=\"" + Multilines.Length + "\" align=\"left\" width='100'  class='HideInPickNote' style='display:none' >" + StockValues[0] + "</td>");
                            // This is for set the location Id
                            ltLocationID.Text = StockValues[1];
                            locationId = int.Parse(StockValues[1]);
                            cartonId = int.Parse(StockValues[StockValues.Length - 1]);
                            sbResult.Append("<td  " + Strickout + "rowspan=\"" + Multilines.Length + "\" align=\"center\" width='150'  class='HideInPickNote' style='display:none' >" + StockValues[5] + "</td>");
                            //string _sKitID = StockValues[2] == "" ? "&emsp;&emsp;&emsp;" : StockValues[2];

                            //sbResult.Append("<td  " + Strickout + "rowspan=\"" + Multilines.Length + "\" align=\"left\" width='40' >" + _sKitID + "</td>");

                            //This is for multiple lines
                            for (int i = 0; i < Multilines.Length; i++)
                            {

                                if (i == 0)
                                    //sbResult.Append(BindTable(values, false));
                                    BindTable(values, false);
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


                            //if (cp.TenantID == 0)
                            //{
                            //    sbResult.Append("<td rowspan=\"" + Multilines.Length + "\" align=\"center\" width='50' class=\"tdPickLink\">");

                            //    if (Strickout == "" || !outer)
                            //    {
                            //        sbResult.Append("<nobr><a style='text-decoration:none;' href='" + String.Format("../mInventory/PickItem.aspx?mmid={0}&obdid={1}&LineNum={2}&locid={3}&kitid={4}&soid={5}&cid={6}'> Pick </nobr>", MaterialMasterID, OutboundID, LineNumber, locationId, CommonLogic.IIF(vKitID != "", vKitID, "NULL"), CommonLogic.IIF(vSOHID != "", vSOHID, "NULL"), cartonId));//<img src='../Images/redarrowright.gif' border='0' />
                            //        flag = false;
                            //    }
                            //}

                            sbResult.Append("</td></tr>");

                            //sbResult.Append(outerRow);

                        }
                    }
                    sbResult.Append("<tr>");
                    sbResult.Append("<td align=\"center\" width='100' class=\"tdDamagedQty\" style=\"min-width: 100px;\">" + totalGoodsQty + "</td><td align=\"center\" width='100' style=\"min-width: 100px;text-align:right;\">" + totalDamageQty + "</td>");
                    sbResult.Append("</tr>");
                    sbResult.Append("</table>");
                    totalGoodsQty = 0;
                    totalDamageQty = 0;
                }

                return sbResult.ToString();
            }
            else
            {
                return "";
            }
        }

        #endregion

        decimal totalGoodsQty = 0;
        decimal totalDamageQty = 0;
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

                if (j != valueslength - 4) //3
                {
                    //if (j == 4) // its to skip the "plant", "Has Desc." columns
                    //{
                    //    continue;
                    //}
                    if (values[j] == "Ÿ" || values[j] == "ž")
                    {
                        // Commented by rajunaidu to exclude "Is Dmg." (we dont require any yes no columns so commented whole loop)
                        //if (values[j] == "ž")
                        //    temp.Append("<td align=\"center\" width='40'> <img  src=\"../Images/blue_menu_icons/check_mark.png\"  /> </td>");
                        //else
                        //    temp.Append("<td align=\"center\"  width='40'>&emsp;&emsp;</td>");
                    }
                    else
                    {

                        if (j == values.Length - 1) // i.e, Picked Quantity. so iam adding border
                        {
                            totalDamageQty = totalDamageQty + Convert.ToDecimal(values[j]);
                            temp.Append("<td align=\"center\" width='100' style=\"min-width: 100px;text-align:center;\" " + Strickout + " ><center><div class=\"lblPickedQty\" style=\"margin:5px;width:88px;height:30px;border:0px solid black;text-align:center;padding-top:5px;\">" + values[j] + "</div></center></td>");
                        }
                        else
                        {
                            if (j == values.Length - 2) // i.e, Damaged Qty. So adding classname to hide in pickList Case in JQuery
                            {
                                totalGoodsQty = totalGoodsQty + Convert.ToDecimal(values[j]);
                                temp.Append("<td align=\"center\" width='100' class=\"tdDamagedQty\" style=\"min-width: 100px;\" " + Strickout + " >" + values[j] + "</td>");
                            }
                            else
                            {
                                if (j == values.Length - 3) // i.e, Avail Qty. So adding classname to hide in pickList Case in JQuery
                                {
                                    temp.Append("<td align=\"center\" width='100' class=\"tdAvailQty\" style=\"min-width: 100px;display:none;\" " + Strickout + " >" + values[j] + "</td>");
                                }
                                else
                                {
                                    temp.Append("<td align=\"center\" width='100' style=\"min-width: 100px;\" " + Strickout + " >" + values[j] + "</td>");
                                }
                            }
                        }
                    }
                }
            }
            if (outer)
                temp.Append("</tr>");

            return temp.ToString();
        }

        #endregion -------------------------Delivery Pick Note  -----------------------------


        protected void Print(object sender, EventArgs e)
        {
            gvPOLineQty1.UseAccessibleHeader = true;
            gvPOLineQty1.HeaderRow.TableSection = TableRowSection.TableHeader;
            gvPOLineQty1.FooterRow.TableSection = TableRowSection.TableFooter;
            gvPOLineQty1.Attributes["style"] = "border-collapse:separate";
            foreach (GridViewRow row in gvPOLineQty1.Rows)
            {
                if (row.RowIndex % 10 == 0 && row.RowIndex != 0)
                {
                    row.Attributes["style"] = "page-break-after:always;";
                }
            }
            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);
            gvPOLineQty1.RenderControl(hw);
            string gridHTML = sw.ToString().Replace("\"", "'").Replace(System.Environment.NewLine, "");
            StringBuilder sb = new StringBuilder();
            sb.Append("<script type = 'text/javascript'>");
            sb.Append("window.onload = new function(){");
            sb.Append("var printWin = window.open('', '', 'left=0");
            sb.Append(",top=0,width=1000,height=600,status=0');");
            sb.Append("printWin.document.write(\"");
            string style = "<style type = 'text/css'>thead {display:table-header-group;} tfoot{display:table-footer-group;}</style>";
            sb.Append(style + gridHTML);
            sb.Append("\");");
            sb.Append("printWin.document.close();");
            sb.Append("printWin.focus();");
            sb.Append("printWin.print();");
            sb.Append("printWin.close();");
            sb.Append("};");
            sb.Append("</script>");
            ClientScript.RegisterStartupScript(this.GetType(), "GridPrint", sb.ToString());
            gvPOLineQty1.DataBind();
        }

        public override void VerifyRenderingInServerForm(Control control)
        {
            /* Verifies that the control is rendered */
        }

        protected void gvPOLineQty1_RowCreated(object sender, GridViewRowEventArgs e)
        {

        }

        protected void gvPOLineQty1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "UpdateLineItemQty")
            {
                //Set the rowindex
                int rowIndex = Convert.ToInt32(e.CommandArgument);

                GridViewRow row = gvPOLineQty1.Rows[rowIndex];

                String valMCode = ((Literal)row.FindControl("ltMCode")).Text;
                TextBox valQty = (TextBox)row.FindControl("txtPickedQty");


            }
        }

        protected void gvPOLineQty1_Sorting(object sender, GridViewSortEventArgs e)
        {

        }

        protected void gvPOLineQty1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            ViewState["ShipmentPendingIsInsert"] = false;

            gvPOLineQty1.PageIndex = e.NewPageIndex;
            gvPOLineQty1.EditIndex = -1;
            this.OBDLineItemsList_buildGridData_Customer(this.OBDLineItemsList_buildGridData_Customer());
        }

        protected void gvPOLineQty1_RowDataBound(object sender, GridViewRowEventArgs e)
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