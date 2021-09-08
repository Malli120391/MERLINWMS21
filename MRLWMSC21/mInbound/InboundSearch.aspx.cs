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
using System.Globalization;
using System.IO;
using System.Drawing;


// Module Name : InboundSearch Under Inbound
// Usecase Ref.: Search Inbound_UC_012
// DevelopedBy : Naresh P
// CreatedOn   : 25/11/2013
// Modified On : 24/03/2015

namespace MRLWMSC21.mInbound
{

   
   
    public partial class InboundSearch : System.Web.UI.Page
    {
        public static CustomPrincipal cp = HttpContext.Current.User as CustomPrincipal;

        int _TotalRowCount;

        protected string SQLSearch = "EXEC [sp_INB_SearchInbound_New]  @StartDate=NULL,@EndDate=NULL,@StoreID =0,@ShipmentTypeID=0,@ClearenceCompanyID=0,@ShipmentStatusID=0,@WarehouseID=NULL,@SearchText=NULL,@SearchField=NULL,@Qty=0,@GrossWeight=0";
        string TenantRootDir = "";
        string InboundPath = "";
        string InbShpDelNotePath = "";
        string InbShpVerifNotePath = "";
        string InbDescPath = "";

        protected void Page_PreInit(object sender, EventArgs e)
        {
            Page.Theme = "Inbound";
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!cp.IsInAnyRoles(CommonLogic.GetRolesAllowed(CommonLogic.GetRolesAllowedForthisPage("Search Inbound"))))
            {
                Response.Redirect("../Login.aspx?eid=6");
            }
            CustomPrincipal cp2 = HttpContext.Current.User as CustomPrincipal;

            string query = "EXEC [dbo].[sp_TPL_GetTenantDirectoryInfo] @TypeID=1";

            DataSet dsPath = DB.GetDS(query.ToString(), false);

            TenantRootDir = dsPath.Tables[0].Rows[4][0].ToString();
            InboundPath = dsPath.Tables[0].Rows[0][0].ToString();
            InbShpDelNotePath = dsPath.Tables[0].Rows[2][0].ToString();
            InbShpVerifNotePath = dsPath.Tables[0].Rows[3][0].ToString();
            InbDescPath = dsPath.Tables[0].Rows[1][0].ToString();

            if (!IsPostBack)
            {
                DesignLogic.SetInnerPageSubHeading(this.Page, "Search Inbound");
                LoadStores(ddlStore);
                LoadShipmentType(ddlShipmentType);
                LoadClearanceCompany(ddlClearanceCompany);
                LoadDateTypes();
                LoadCategories();
                
                if (cp2.TenantID != 0)
                {
                    gvShipmentResults.Columns[15].Visible = false;
                    gvShipmentResults.Columns[5].Visible = false;
                }

                else
                {
                    gvShipmentResults.Columns[15].Visible = false;
                }

                hifTenant.Value = cp2.TenantID.ToString();
                txtTenant.Enabled = CommonLogic.CheckSuperAdmin(txtTenant, cp2, hifTenant);


               // ViewState["ShipmentResultsSQLString"] = this.SQLSearch + ",@TenantID=" + hifTenant.Value + ",@AccountID_New=" + cp2.AccountID.ToString() + ",@USERID ="+cp2.UserID+",@TenantID_New=" + cp2.TenantID.ToString();
                ViewState["SortingOrder"] = "ASC";
                ViewState["SortString"] = "StoreRefNo";
                ViewState["IsSorting"] = "false";
                hfpageId.Value = "1";

                
            //    this.ShipmentResults_buildGridData(this.ShipmentResults_buildGridData(hfpageId.Value, drppagesize.SelectedValue), hfpageId.Value, drppagesize.SelectedValue);
            }
        }



        private void LoadShipmentType(DropDownList ddlShipType)
        {

            ddlShipType.Items.Clear();
            //ddlShipType.Items.Add(new ListItem("Select Shipment Type", "0"));
            ddlShipType.Items.Add(new ListItem("Select ShipmentType", "0"));
            IDataReader rsShipType = DB.GetRS("Select ShipmentTypeID, ShipmentType from GEN_ShipmentType Where IsActive =1 and IsDeleted=0 ");

            while (rsShipType.Read())
            {
                ddlShipType.Items.Add(new ListItem(rsShipType["ShipmentType"].ToString(), rsShipType["ShipmentTypeID"].ToString()));

            }
            rsShipType.Close();

        }

        private void LoadClearanceCompany(DropDownList ddlShipType)
        {

            ddlClearanceCompany.Items.Clear();
            ddlClearanceCompany.Items.Add(new ListItem("All", "0"));
            IDataReader rsCC = DB.GetRS("Select ClearanceCompanyID,ClearanceCompany from GEN_ClearanceCompany Where IsActive=1 and IsDeleted=0 ");

            while (rsCC.Read())
            {
                ddlClearanceCompany.Items.Add(new ListItem(rsCC["ClearanceCompany"].ToString(), rsCC["ClearanceCompanyID"].ToString()));

            }
            rsCC.Close();

        }

        private void LoadDateTypes()
        {
            ddlShipmentStatus.Items.Clear();
            //ddlShipmentStatus.Items.Add(new ListItem("Select Shipment Status", "0"));
            ddlShipmentStatus.Items.Add(new ListItem("Select Inbound Status", "0"));
            ddlShipmentStatus.Items.Add(new ListItem("Shipment Initiated", "1"));
            ddlShipmentStatus.Items.Add(new ListItem("Shipment Expected", "2"));
            ddlShipmentStatus.Items.Add(new ListItem("Shipment Received", "3"));
            ddlShipmentStatus.Items.Add(new ListItem("GRN Pending", "5"));
            //ddlShipmentStatus.Items.Add(new ListItem("Discrepancy", "5"));
            ddlShipmentStatus.Items.Add(new ListItem("GRN Updated", "6"));
            ddlShipmentStatus.Items.Add(new ListItem("Shipment Closed", "11"));


        }

        private void LoadCategories()
        {
            ddlCategory.Items.Clear();
            ddlCategory.Items.Add(new ListItem("Select Category", "0"));
            ddlCategory.Items.Add(new ListItem("Store Ref. Number", "1"));
            //   ddlCategory.Items.Add(new ListItem("PO Number", "2"));
            ddlCategory.Items.Add(new ListItem("Invoice Number", "3"));
            // ddlCategory.Items.Add(new ListItem("AWB #", "4"));
            //  ddlCategory.Items.Add(new ListItem("BL #", "5"));
            // ddlCategory.Items.Add(new ListItem("Clearance Invoice#", "6"));
            // ddlCategory.Items.Add(new ListItem("Freight Invoice #", "7"));
            ddlCategory.Items.Add(new ListItem("GRN #", "8"));
            // ddlCategory.Items.Add(new ListItem("Weight", "9"));
            // ddlCategory.Items.Add(new ListItem("No of. Packages", "10"));
            ddlCategory.Items.Add(new ListItem("Supplier", "11"));
            ddlCategory.Items.Add(new ListItem("PO Number", "13"));
        }

        protected void lnkSearchtext_Click(object sender, EventArgs e)
        {
            cp = HttpContext.Current.User as CustomPrincipal;
            if (ddlStore.SelectedValue == "" || ddlStore.SelectedValue == "0")
            {
                resetError("Please select Warehouse", true);
                return;
            }

            if (lblIbSearchStatus.Text == "Search...")
                lblIbSearchStatus.Text = "";

            this.SQLSearch = "EXEC [sp_INB_SearchInbound_New] "; 





            if (txtFromDate.Text.Trim() != "")
                // this.SQLSearch += "@StartDate=" + DB.SQuote(Convert.ToDateTime(CommonLogic.GetConfigValue(cp.TenantID, "DateFormat") == "dd/MM/yyyy" ? DateTime.ParseExact(txtFromDate.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy") : txtFromDate.Text.Trim()).ToString());

                //this.SQLSearch += "@StartDate=" + DB.SQuote(Convert.ToDateTime(CommonLogic.GetConfigValue(Convert.ToInt16(hifTenant.Value), "DateFormat") == "dd-MMM-yyyy" ? DateTime.ParseExact(txtFromDate.Text.Trim(), "dd-MMM-yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy") : txtFromDate.Text.Trim()).ToString());
                this.SQLSearch += "@StartDate=" + DB.DateQuote(txtFromDate.Text);
            else
                this.SQLSearch += "@StartDate=NULL";

            if (txtToDate.Text.Trim() != "")
                //this.SQLSearch += ",@EndDate=" + DB.SQuote(Convert.ToDateTime(CommonLogic.GetConfigValue(cp.TenantID, "DateFormat") == "dd/MM/yyyy" ? DateTime.ParseExact(txtToDate.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy") : txtToDate.Text.Trim()).ToString());

                //this.SQLSearch += ",@EndDate=" + DB.SQuote(Convert.ToDateTime(CommonLogic.GetConfigValue(Convert.ToInt16(hifTenant.Value), "DateFormat") == "dd-MMM-yyyy" ? DateTime.ParseExact(txtToDate.Text.Trim(), "dd-MMM-yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy") : txtToDate.Text.Trim()).ToString());
                this.SQLSearch += ",@EndDate=" + DB.DateQuote(txtToDate.Text);
            else if (txtFromDate.Text != "" && txtToDate.Text == "")
            {
                this.SQLSearch += ",@EndDate=" + DB.DateQuote(DateTime.Now);
            }
            else
                this.SQLSearch += ",@EndDate=NULL";

            if (ddlStore.SelectedValue != "0")
                this.SQLSearch += ",@StoreID=" + ddlStore.SelectedValue;
            else
                this.SQLSearch += ",@StoreID=0";

            if (ddlShipmentType.SelectedValue != "0")
                this.SQLSearch += ",@ShipmentTypeID=" + ddlShipmentType.SelectedValue;
            else
                this.SQLSearch += ",@ShipmentTypeID=0";

            if (ddlClearanceCompany.SelectedValue != "0")
                this.SQLSearch += ",@ClearenceCompanyID=" + ddlClearanceCompany.SelectedValue;
            else
                this.SQLSearch += ",@ClearenceCompanyID=0";


            if (ddlShipmentStatus.SelectedValue != "0")
                this.SQLSearch += ",@ShipmentStatusID=" + ddlShipmentStatus.SelectedValue;
            else
                this.SQLSearch += ",@ShipmentStatusID=0";

            if (ddlStore.SelectedValue == "0")
            {
                this.SQLSearch += ",@WarehouseID=NULL"; 
            }
            else
            {
                //this.SQLSearch += ",@WarehouseID=" + DB.SQuote(String.Join(",", cp.Warehouses));
                this.SQLSearch += ",@WarehouseID=" + ddlStore.SelectedValue;
            }
            
            

            //this.SQLSearch += ",@AccountID_New=case when 0 =" + cp.AccountID.ToString() + " then @AccountID_New else " + cp.AccountID.ToString() + " end";

            //this.SQLSearch += ",@TenantID_New=case when 0 =" + cp.TenantID.ToString() + " then @TenantID_New else " + cp.TenantID.ToString() + " end";
            this.SQLSearch += ",@AccountID_New=" + cp.AccountID.ToString() + " ";

            this.SQLSearch += ",@TenantID_New=" + cp.TenantID.ToString() + "";


            if (ddlCategory.SelectedValue != "0")
            {
                this.SQLSearch += ",@SearchField=" + ddlCategory.SelectedValue;
            }
            else
            {
                this.SQLSearch += ",@SearchField=0";
            }


            if (txtsearchText.Text.Trim() != "")
            {
                if (ddlCategory.SelectedValue == "9") //Gross Weight
                    this.SQLSearch += ",@SearchText=dummy,@Qty=0,@GrossWeight=" + txtsearchText.Text.Trim();
                else if (ddlCategory.SelectedValue == "10") //No .of Packages
                    this.SQLSearch += ",@SearchText=dummy,@GrossWeight=0,@Qty=" + txtsearchText.Text.Trim();
                else
                    this.SQLSearch += ",@SearchText=" + DB.SQuote(txtsearchText.Text.Trim()) + ",@GrossWeight=0,@Qty=0";
            }
            else
            {
                this.SQLSearch += ",@SearchText=NULL,@GrossWeight=0,@Qty=0";
            }

            if (hifTenant.Value != "0")
            {
                this.SQLSearch += ",@TenantID=" + hifTenant.Value;
            }

            this.SQLSearch += ", @USERID = " + cp.UserID;

            ViewState["ShipmentResultsSQLString"] = this.SQLSearch;
            ViewState["IsSorting"] = "false";
            this.ShipmentResults_buildGridData(this.ShipmentResults_buildGridData(hfpageId.Value, drppagesize.SelectedValue), hfpageId.Value, drppagesize.SelectedValue);


        }

        public void LoadStores(DropDownList ddlWH)
        {
      


            try
            {
                CustomPrincipal cp1 = HttpContext.Current.User as CustomPrincipal;
                ddlWH.Items.Clear();

                ddlWH.Items.Add(new ListItem("Select Warehouse", "0"));
                //ddlWH.Items.Add(new ListItem("Select All", "0"));

                // IDataReader rsWH = DB.GetRS("select Location,WHCode,WarehouseID from GEN_Warehouse where IsActive=1 and IsDeleted=0 AND AccountID = case when 0 =" + cp.AccountID.ToString() + " then AccountID else " + cp.AccountID.ToString() + " end");
                 IDataReader rsWH = DB.GetRS("SELECT  WH.Location,WH.WarehouseID,WH.WHCode FROM GEN_Warehouse WH JOIN GEN_User_Warehouse UW ON UW.WarehouseID = WH.WarehouseID AND UW.IsActive = 1 AND UW.IsDeleted = 0 AND WH.IsActive = 1 AND UW.USERID =" + cp1.UserID+" AND WH.IsDeleted = 0 AND AccountID = case when 0 =" + cp1.AccountID.ToString() + " then AccountID else " + cp1.AccountID.ToString() + " end");

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

                    ddlWH.Items.Add(new ListItem(StoreVal, rsWH["WareHouseID"].ToString()));
                }
                rsWH.Close();
            }
            catch (Exception ex)
            {
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
                resetError("Error while loading stores", true);

            }



        }

        public String GetPriorityLeve(String Level)
        {

            switch (Level)
            {
                case "0":
                    return "Normal";
                case "1":
                    return "Medium";
                case "2":
                    return "High";
                case "3":
                    return "Highest";
                default:
                    return "";
            }

        }

        public string GetStoreRefNoWithLink(String StoreRefNo, String TenantID)
        {
           // String sFileName = CommonLogic._GetAttatchmentFile(TenantRootDir + DB.GetSqlS("select UniqueID AS S from GEN_Tenant where TenantID=" + cp.TenantID) + InboundPath + InbShpDelNotePath, StoreRefNo);

            String sFileName = CommonLogic._GetAttatchmentFile(TenantRootDir + TenantID + InboundPath + InbShpDelNotePath, StoreRefNo);

            String ResValue = "";
            if (sFileName != "")
            {
                String Path = "../ViewImage.aspx?path=" + sFileName;

                ResValue += "<a style=\"text-decoration:none;\" href=\"#\" onclick=\" OpenImage(' " + Path + " ')  \" > " + StoreRefNo + " </a>";
                // ResValue += "<img src=\"../Images/redarrowright.gif\"   />";
            }
            else
            {
                ResValue = StoreRefNo;
            }


            return ResValue;
        }

        #region ---------  Start Shipment Results Grid   ---------------------

        protected DataTable ShipmentResults_buildGridData(String PageNumber, String PageSize)
        {

            StringBuilder sql = new StringBuilder();
            sql.Append(ViewState["ShipmentResultsSQLString"].ToString());

            if (ViewState["IsSorting"].ToString().Equals("true"))
                sql.Append(",@PageIndex=0 ");
            else
                sql.Append(",@PageIndex=" + PageNumber);

            sql.Append(",@PageSize =" + PageSize);

            DataSet ds = DB.GetDS(sql.ToString(), false);
            _TotalRowCount = Convert.ToInt16(ds.Tables[0].Rows[0][0]);
            if (_TotalRowCount == 0)
            {
                lblRecordCount.Text = "<span style='Color:Red;'>No records found for the given criteria</span>";
                btninboundsearchlist.Visible = false;
                lblstringperpage.Visible = false;
                drppagesize.Visible = false;
            }
            else
            {
                lblRecordCount.Text = "Total Items [" + _TotalRowCount + "]";
                lblstringperpage.Visible = true;
                btninboundsearchlist.Visible = true;
                drppagesize.Visible = true;
            }
            //if (ds.Tables[0].Rows.Count == 0)
            //{
            //    lblRecordCount.Text = "<span style='Color:Red;'>No records found for the given criteria</span>";
            //}
            //else
            //{
            //    lblRecordCount.Text = "Total Items [" + ds.Tables[0].Rows.Count.ToString() + "]";
            //}
            ds.Tables[0].Dispose();
            generatePager(_TotalRowCount, Convert.ToInt16(drppagesize.SelectedValue), Convert.ToInt16(hfpageId.Value));
            return ds.Tables[1];

        }
        protected void ShipmentResults_buildGridData(DataTable dt, String SPageNumber, String SPageSize)
        {
            int PageNumber = Convert.ToInt16(SPageNumber);
            int PageSize = Convert.ToInt16(SPageSize);
            if (ViewState["IsSorting"].ToString().Equals("true"))
            {
                dt.DefaultView.Sort = ViewState["SortString"].ToString() + " " + ViewState["SortingOrder"].ToString();
                if (PageSize * (PageNumber - 1) > _TotalRowCount)
                    PageNumber = (int)Math.Ceiling((decimal)_TotalRowCount / PageSize);
                gvShipmentResults.DataSource = dt.DefaultView.ToTable().Rows.Cast<System.Data.DataRow>().Skip((PageNumber - 1) * PageSize).Take(PageSize).CopyToDataTable();
            }
            else
                gvShipmentResults.DataSource = dt;
            gvShipmentResults.DataBind();
            dt.Dispose();
        }
        protected void gvShipmentResults_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            cp = HttpContext.Current.User as CustomPrincipal;
            //======================= Added By M.D.Prasad For View Only Condition ======================//
            string[] hh = cp.Roles;
            for (int i = 0; i < hh.Length; i++)
            {
                if (cp.UserTypeID == 3 && Convert.ToInt32(hh[i]) == 4)
                {
                    btninboundsearchlist.Visible = false;
                }
            }
            if ((e.Row.RowType == DataControlRowType.DataRow) && (e.Row.RowState == DataControlRowState.Normal) || (e.Row.RowState == DataControlRowState.Alternate))
            {         
                for (int i = 0; i < hh.Length; i++)
                {
                    if (cp.UserTypeID == 3 && Convert.ToInt32(hh[i]) == 4)
                    {
                        gvShipmentResults.Columns[13].Visible = false;
                        gvShipmentResults.Columns[16].Visible = false;
                    }
                }                
            }
            //======================= Added By M.D.Prasad For View Only Condition ======================//







            /*
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                if ((e.Row.RowState == DataControlRowState.Normal) || (e.Row.RowState == DataControlRowState.Alternate))
                {
                    String vPriority = ((Literal)e.Row.Cells[0].FindControl("ltPriorityLevel")).Text;
                    String vltPriorityTime = ((Literal)e.Row.Cells[0].FindControl("ltPriorityTime")).Text;

                    StringBuilder StrTimetickerJS = new StringBuilder();

                    //if (vltPriorityTime != "" && Convert.ToDateTime(vltPriorityTime) > DateTime.Now)
                    if (vltPriorityTime != "")
                    {

                        DateTime dtPriorityDateTime = Convert.ToDateTime(vltPriorityTime);

                        StrTimetickerJS.Append("<script type=\"text/javascript\">");
                        StrTimetickerJS.Append("$(function() {");
                        StrTimetickerJS.Append("var austDay = new Date();");
                        StrTimetickerJS.Append("austDay = new Date(" + dtPriorityDateTime.Year.ToString() + "," + (dtPriorityDateTime.Month - 1).ToString() + "," + dtPriorityDateTime.Day.ToString() + "," + dtPriorityDateTime.Hour.ToString() + "," + dtPriorityDateTime.Minute.ToString() + "," + CommonLogic.RandomNumber(60).ToString() + ");");
                        //StrTimetickerJS.Append("austDay = new Date(" + vltPriorityTime + ");");
                        //StrTimetickerJS.Append("$('#ShipmentResultsCD" + e.Row.RowIndex.ToString() + "').countdown({ until: austDay, compact: true, format: 'dHMS', description: ''});");

                        if (dtPriorityDateTime > DateTime.Now)
                        {
                            StrTimetickerJS.Append("$('#ShipmentResultsCD" + e.Row.RowIndex.ToString() + "').countdown({ until: austDay, compact: true, format: 'dHMS', description: ''});");
                        }
                        else
                        {
                            StrTimetickerJS.Append("$('#ShipmentResultsCD" + e.Row.RowIndex.ToString() + "').countdown({ since: austDay, compact: true, format: 'dHMS', description: ''});");
                        }

                        StrTimetickerJS.Append("});</script>");

                        //StrTimetickerJS.Append("<img src=\"images\\horse1.gif\" border=\"0\" width=\"25\"/><div id=\"ShipmentResultsCD" + e.Row.RowIndex.ToString() + "\" class=\"timeAlertDiv\"></div>");
                        //e.Row.Cells[0].Text = StrTimetickerJS.ToString();
                        if (dtPriorityDateTime > DateTime.Now)
                        {
                            StrTimetickerJS.Append("<img src=\"..\\Images\\horse1.gif\" border=\"0\" width=\"25\"/><div id=\"ShipmentResultsCD" + e.Row.RowIndex.ToString() + "\" class=\"timeAlertDiv\"></div>");
                            e.Row.Cells[0].Text = StrTimetickerJS.ToString();
                        }
                        else
                        {
                            StrTimetickerJS.Append("<div id=\"ShipmentResultsCD" + e.Row.RowIndex.ToString() + "\" class=\"timeAlertDiv\"></div>");
                            e.Row.Cells[0].Text = " <span class='clsTimeOut'>" + StrTimetickerJS.ToString() + "</span>";
                        }

                    }

                    switch (vPriority)
                    {
                        case "3":
                            e.Row.Cells[0].CssClass = "Highest";
                            e.Row.CssClass = "Highest_row";

                            break;
                        case "2":
                            e.Row.Cells[0].CssClass = "High";
                            e.Row.CssClass = "High_row";
                            break;
                        case "1":
                            e.Row.Cells[0].CssClass = "Medium";
                            e.Row.CssClass = "Medium_row";
                            break;
                        case "0":
                            e.Row.Cells[0].CssClass = "Normal";
                            break;
                        default:
                            e.Row.Cells[0].CssClass = "Normal";
                            break;


                    }

                }

            }*/

        }

        protected void gvShipmentResults_Sorting(object sender, GridViewSortEventArgs e)
        {
            gvShipmentResults.EditIndex = -1;
            ViewState["SortString"] = e.SortExpression.ToString();
            ViewState["SortingOrder"] = (ViewState["SortingOrder"].ToString() == "ASC" ? "DESC" : "ASC");
            ViewState["IsSorting"] = "true";
            hfpageId.Value = "1";
            this.ShipmentResults_buildGridData(this.ShipmentResults_buildGridData(hfpageId.Value, drppagesize.SelectedValue), hfpageId.Value, drppagesize.SelectedValue);

        }

        protected void gvShipmentResults_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            ViewState["ShipmentResultsIsInsert"] = false;

            gvShipmentResults.PageIndex = e.NewPageIndex;
            gvShipmentResults.EditIndex = -1;
            this.ShipmentResults_buildGridData(this.ShipmentResults_buildGridData(hfpageId.Value, drppagesize.SelectedValue), hfpageId.Value, drppagesize.SelectedValue);
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

        #endregion ---------  END Shipment Results Grid   ---------------------

        //protected void btninboundsearchlist_Click(object sender, ImageClickEventArgs e)
        protected void btninboundsearchlist_Click(object sender, EventArgs e)
        {
            if (ddlStore.SelectedValue == "" || ddlStore.SelectedValue == "0")
            {
                resetError("Please select Warehouse", true);
                return;
            }
            try
            {

                //CommonLogic.PrepareGridViewForExport(gvShipmentResults, HiddenColumns);
                //String ExcelFileName = "InboundSearch";
                //CommonLogic.ExportGridView(gvShipmentResults, ExcelFileName);
                gvShipmentResults.AllowPaging = false;
                string[] hiddencolumns = { "Modify" };
                this.ShipmentResults_buildGridData(this.ShipmentResults_buildGridData("0", drppagesize.SelectedValue), hfpageId.Value, drppagesize.SelectedValue);
                CommonLogic.ExporttoExcel1(gvShipmentResults, "InboundSearch", hiddencolumns);
            }
            catch (Exception ex)
            {
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
                resetError("Error While Exporting Data", true);
            }
        }

        public override void VerifyRenderingInServerForm(Control control)
        {
            /* Verifies that the control is rendered */
        }

        protected void drppagesize_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.ShipmentResults_buildGridData(this.ShipmentResults_buildGridData(hfpageId.Value, drppagesize.SelectedValue), hfpageId.Value, drppagesize.SelectedValue);
        }

        protected void dlPagerupper_ItemCommand(object source, DataListCommandEventArgs e)
        {
            if (e.CommandName == "PageNo")
            {
                hfpageId.Value = e.CommandArgument.ToString();
                this.ShipmentResults_buildGridData(this.ShipmentResults_buildGridData(hfpageId.Value, drppagesize.SelectedValue), hfpageId.Value, drppagesize.SelectedValue);
            }
        }

        public void generatePager(int totalRowCount, int pageSize, int currentPage)
        {
            //This is for when 
            if (pageSize * (currentPage - 1) > totalRowCount)
                currentPage = (int)Math.Ceiling((decimal)totalRowCount / pageSize);


            int totalLinkInPage = 10;
            int totalPageCount = (int)Math.Ceiling((decimal)totalRowCount / pageSize);

            int startPageLink = Math.Max(currentPage - (int)Math.Floor((decimal)totalLinkInPage / 2), 1);
            int lastPageLink = Math.Min(startPageLink + totalLinkInPage - 1, totalPageCount);

            if ((startPageLink + totalLinkInPage - 1) > totalPageCount)
            {
                lastPageLink = Math.Min(currentPage + (int)Math.Floor((decimal)totalLinkInPage / 2), totalPageCount);
                startPageLink = Math.Max(lastPageLink - totalLinkInPage + 1, 1);
            }

            List<ListItem> pageLinkContainer = new List<ListItem>();

            if (startPageLink != 1)
                pageLinkContainer.Add(new ListItem("&lt;&lt;", "01", currentPage != 1));
            for (int i = startPageLink; i <= lastPageLink; i++)
            {
                pageLinkContainer.Add(new ListItem(i.ToString("00"), i.ToString(), currentPage != i));
            }
            if (lastPageLink != totalPageCount)
                pageLinkContainer.Add(new ListItem("&gt;&gt;", totalPageCount.ToString("00"), currentPage != totalPageCount));

            dlPagerupper.DataSource = pageLinkContainer;
            dlPagerupper.DataBind();
            dlPager.DataSource = pageLinkContainer;
            dlPager.DataBind();
        }

    }
}