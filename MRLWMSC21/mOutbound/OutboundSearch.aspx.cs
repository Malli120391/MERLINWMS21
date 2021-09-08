using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MRLWMSC21Common;
using System.Data.SqlClient;
using System.Data;
using System.Text;
using System.Globalization;
using MRLWMSC21.mOutbound.BL;
using System.Collections;




// Module Name : OutboundSearch Under Outbound
// DevelopedBy : Naresh P
// CreatedOn   : 10/11/2013
// Modified On : 24/03/2015

namespace MRLWMSC21.mOutbound
{
    public partial class OutboundSearch : System.Web.UI.Page
    {
        public CustomPrincipal cp = HttpContext.Current.User as CustomPrincipal;
        int _TotalRowCount;
        protected string SQLSearch = "EXEC [sp_OBD_SearchOutbound_New]   @StartDate=NULL,@EndDate=NULL,@StoreID=0,@DocumentTypeID=0,@DeliveryStatusID=0,@SearchText=NULL,@SearchField=0,@DivisionIDs=NULL,";


        protected void Page_PreInit(object sender, EventArgs e)
        {
            Page.Theme = "Outbound";
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            cp = HttpContext.Current.User as CustomPrincipal;
            if (!cp.IsInAnyRoles(CommonLogic.GetRolesAllowed(CommonLogic.GetRolesAllowedForthisPage("Search Outbound "))))
            {
                Response.Redirect("../Login.aspx?eid=6");
            }

            DesignLogic.SetInnerPageSubHeading(this.Page, "Search Outbound");

            ViewState["TenantID"] = cp.TenantID.ToString();

            if (!IsPostBack)
            {

                if (cp.TenantID != 0)
                {
                    //deliverydata.Columns[15].Visible = false;
                    deliverydata.Columns[3].Visible = false;
                }

                else
                {
                    //deliverydata.Columns[15].Visible = false;
                }

                hifTenant.Value = cp.TenantID.ToString();
                txtTenant.Enabled = CommonLogic.CheckSuperAdmin(txtTenant, cp, hifTenant);

                // LoadStores(ddlStore);
                LoadDocType(ddlDocumentType);
                LoadDeliveryStatus(ddlDeliveryStatus);
                LoadCategories();

                //this.SQLSearch += "@WarehouseID=" + DB.SQuote(String.Join(",", cp.Warehouses)) + ",@TenantID=" + hifTenant.Value;
                //this.SQLSearch = this.SQLSearch + ", @AccountID_New = " + cp.AccountID.ToString() + ",@UserTypeID_New = " + cp.UserTypeID.ToString() + ",@TenantID_New = " + cp.TenantID.ToString();
                //ViewState["DeliveryResultsSQLString"] = this.SQLSearch;
                // this.DeliveryResults_buildGridData(this.DeliveryResults_buildGridData(hfpageId.Value, drppagesize.SelectedValue), hfpageId.Value, drppagesize.SelectedValue);

                hfpageId.Value = "1";
                ViewState["IsSorting"] = "false";
                //this.DeliveryResults_buildGridData(this.DeliveryResults_buildGridData());
             }
        }

        public void LoadStores(DropDownList ddlWH)
        {
            cp = HttpContext.Current.User as CustomPrincipal;
            ddlWH.Items.Clear();

            ddlWH.Items.Add(new ListItem("Select Warehouse", "0"));
            //IDataReader rsWH = DB.GetRS("select Location,WHCode,WarehouseID from GEN_Warehouse where IsActive=1 and IsDeleted=0 and AccountID="+cp.AccountID+"");
            IDataReader rsWH = DB.GetRS("Exec [dbo].[USP_MST_DropWH] @Accountid = " + cp.AccountID + ",@TenantID=" + cp.TenantID);
            //IDataReader rsWH = DB.GetRS("select Location,WHCode,WarehouseID from GEN_Warehouse where IsActive=1 and IsDeleted=0 and AccountID="+cp.AccountID+" ");

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

        private void LoadDocType(DropDownList ddlDocType)
        {

            ddlDocType.Items.Clear();
            ddlDocType.Items.Add(new ListItem("All", "0"));

            //  IDataReader rsDocType = DB.GetRS("Select DocumentTypeID, DocumentType from GEN_DocumentType Where IsActive =1");
            IDataReader rsDocType = DB.GetRS("[dbo].[sp_Android_GetDocumentType]");

            while (rsDocType.Read())
            {
                ddlDocType.Items.Add(new ListItem(rsDocType["DocumentType"].ToString(), rsDocType["DocumentTypeID"].ToString()));

            }
            rsDocType.Close();

        }

        private void LoadDeliveryStatus(DropDownList ddlDStatus)
        {

            ddlDStatus.Items.Clear();
            ddlDStatus.Items.Add(new ListItem("All", "0"));
            ddlDStatus.Items.Add(new ListItem("Sent to Store", "1"));
            ddlDStatus.Items.Add(new ListItem("On Hold", "12"));
            ddlDStatus.Items.Add(new ListItem("Sent for PGI", "2"));
            ddlDStatus.Items.Add(new ListItem("Sent to Packing", "6"));
            ddlDStatus.Items.Add(new ListItem("PGI Done/Sent to Delivery", "3"));
            ddlDStatus.Items.Add(new ListItem("Delivered", "4"));
            //ddlDStatus.Items.Add(new ListItem("Canceled", "5"));
            //ddlDStatus.Items.Add(new ListItem("Closed", "6"));
            //ddlDStatus.Items.Add(new ListItem("POD Pending", "7"));
            //ddlDStatus.Items.Add(new ListItem("Customer Return", "11"));
        }

        private void LoadCategories()
        {
            ddlCategory.Items.Clear();
            ddlCategory.Items.Add(new ListItem("Select Category", "0"));
            ddlCategory.Items.Add(new ListItem("Delv. Doc. Number", "1"));
            ddlCategory.Items.Add(new ListItem("Customer Name", "2"));
            // ddlCategory.Items.Add(new ListItem("Document Number", "3"));
            // ddlCategory.Items.Add(new ListItem("Delv. Vehicle No.", "4"));
            ddlCategory.Items.Add(new ListItem("Customer PONumber", "5"));
            ddlCategory.Items.Add(new ListItem("Invoice Number", "6"));
            ddlCategory.Items.Add(new ListItem("SO Number", "7"));
            //ddlCategory.Items.Add(new ListItem("Kit Code", "8"));
        }

        protected void lnkSearchtext_Click(object sender, EventArgs e)
        {
            cp = HttpContext.Current.User as CustomPrincipal;
            if (ddlStore.Text == "")
            {
                resetError("Please select Warehouse", true);
                return;
            }
            this.SQLSearch = "EXEC [sp_OBD_SearchOutbound_New] ";

            if (txtFromDate.Text.Trim() != "")

                this.SQLSearch += " @StartDate=" + DB.SQuote(Convert.ToDateTime(txtFromDate.Text.Trim()).ToString("MM/dd/yyyy"));
            //this.SQLSearch += "@StartDate=" + DB.SQuote(Convert.ToDateTime(CommonLogic.GetConfigValue(cp.TenantID, "DateFormat") == "dd/MM/yyyy" ? DateTime.ParseExact(txtFromDate.Text.Trim(), "dd-MMM-yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy") : txtFromDate.Text.Trim()).ToString());


            else
                this.SQLSearch += "@StartDate=NULL";

            if (txtToDate.Text.Trim() != "")
                this.SQLSearch += ",@EndDate=" + DB.SQuote(Convert.ToDateTime(txtToDate.Text.Trim()).ToString("MM/dd/yyyy"));
            //this.SQLSearch += ",@EndDate=" + DB.SQuote(Convert.ToDateTime(CommonLogic.GetConfigValue(cp.TenantID, "DateFormat") == "dd/MM/yyyy" ? DateTime.ParseExact(txtToDate.Text.Trim(), "dd-MMM-yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy") : txtToDate.Text.Trim()).ToString());
            else
                this.SQLSearch += ",@EndDate=NULL";

            if (ddlStoreID.Value != "0" && ddlStoreID.Value != "" && ddlStoreID.Value != null)
                this.SQLSearch += ",@StoreID=" + ddlStoreID.Value;
            else
                this.SQLSearch += ",@StoreID=0";

            if (ddlDocumentType.SelectedValue != "0")
                this.SQLSearch += ",@DocumentTypeID=" + ddlDocumentType.SelectedValue;
            else
                this.SQLSearch += ",@DocumentTypeID=0";

            if (ddlDeliveryStatus.SelectedValue != "0")
                this.SQLSearch += ",@DeliveryStatusID=" + ddlDeliveryStatus.SelectedValue;
            else
                this.SQLSearch += ",@DeliveryStatusID=0";


            this.SQLSearch += ",@WarehouseID=" + DB.SQuote(String.Join(",", cp.Warehouses));

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
                this.SQLSearch += ",@SearchText=" + DB.SQuote(txtsearchText.Text.Trim());
            }
            else
            {
                this.SQLSearch += ",@SearchText=NULL";
            }

            if (ddlCategory.SelectedValue == "8")
            {
                this.SQLSearch += ",@KitCode=" + (txtsearchText.Text.Trim() != "" ? DB.SQuote(txtsearchText.Text.Trim()) : "NULL");
            }


            String DivisionIDs = "";

            if (atcDepartment.Text != "")
            {

                if (Request.Form[hifDepartmentID.UniqueID] != null)
                {
                    if (atcDivision.Text != "")
                        DivisionIDs = Request.Form[hifDivisionID.UniqueID].ToString();

                }

                if (Request.Form[hifDepartmentID.UniqueID].ToString() != "0" && DivisionIDs == "0")
                {
                    DivisionIDs = DB.GetSqlS("Select STUFF(( Select ', ' +  CONVERT(nvarchar(3),DivisionID) from GEN_Division Where IsActive=1 and IsDeleted=0 and DepartmentID =" + Request.Form[hifDepartmentID.UniqueID].ToString() + "FOR XML PATH('')),1, 2, '') AS S");
                }
            }

            if (DivisionIDs == "0")
                DivisionIDs = "";

            this.SQLSearch += ",@DivisionIDs=" + (DivisionIDs == "" ? "NULL" : DB.SQuote(DivisionIDs));


            this.SQLSearch += ",@TenantID=" + hifTenant.Value;


            if (txtAWBNo.Text.Trim() != "")
            {
                this.SQLSearch += ",@AWBNo=" + DB.SQuote(txtAWBNo.Text.Trim());
            }
            else
            {
                this.SQLSearch += ",@AWBNo=NULL";
            }


            if (txtDueDate.Text.Trim() != "")
                this.SQLSearch += ",@DueDate=" + DB.SQuote(Convert.ToDateTime(txtDueDate.Text.Trim()).ToString("MM/dd/yyyy"));
            //this.SQLSearch += ",@DueDate=" + DB.SQuote(Convert.ToDateTime(CommonLogic.GetConfigValue(cp.TenantID, "DateFormat") == "dd/MM/yyyy" ? DateTime.ParseExact(txtDueDate.Text.Trim(), "dd-MMM-yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy") : txtDueDate.Text.Trim()).ToString());
            else
                this.SQLSearch += ",@DueDate=NULL";



            this.SQLSearch = this.SQLSearch + ", @AccountID_New = " + cp.AccountID.ToString() + ",@UserTypeID_New = " + cp.UserTypeID.ToString() + ",@TenantID_New = " + cp.TenantID.ToString();
            ViewState["DeliveryResultsSQLString"] = this.SQLSearch;

            //this.DeliveryResults_buildGridData(this.DeliveryResults_buildGridData());
            this.DeliveryResults_buildGridData(this.DeliveryResults_buildGridData(hfpageId.Value, drppagesize.SelectedValue), hfpageId.Value, drppagesize.SelectedValue);

        }


        #region -------------------------Delivery Results -----------------------------

        protected DataTable DeliveryResults_buildGridData(String PageNumber, String PageSize)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append(ViewState["DeliveryResultsSQLString"].ToString());
            // sql += " order by " + ViewState["DeliveryResultsSort"].ToString() + " " + ViewState["DeliveryResultsSortOrder"].ToString();
            // sql += ",@OrderByString=" + DB.SQuote(ViewState["DeliveryResultsSort"].ToString()) + ",@SortOrderString=" + DB.SQuote(ViewState["DeliveryResultsSortOrder"].ToString());
            var pagenumber = ViewState["IsSorting"].ToString().Equals("true") ? "0" : PageNumber;

            sql.Append(",@PageIndex=" + pagenumber + ", @PageSize=" + PageSize);

            DataSet ds = DB.GetDS(sql.ToString(), false);
            // _TotalRowCount = ds.Tables[1].Rows.Count;
            _TotalRowCount = Convert.ToInt16(ds.Tables[0].Rows[0][0]);
            if (_TotalRowCount == 0)
            {
                gvStatusMsg.Text = "<span style='Color:Red;'>  No Deliveries found</span>";
                imgbtnoutboundsearch.Visible = false;
                lblstringperpage.Visible = false;
                drppagesize.Visible = false;
            }
            else
            {
                gvStatusMsg.Text = "Total Items [" + _TotalRowCount + "]";
                lblstringperpage.Visible = true;
                imgbtnoutboundsearch.Visible = true;
                drppagesize.Visible = true;
            }
            ds.Tables[0].Dispose();
            generatePager(_TotalRowCount, Convert.ToInt16(drppagesize.SelectedValue), Convert.ToInt16(hfpageId.Value));
            return ds.Tables[1];

        }

        protected void DeliveryResults_buildGridData(DataTable dt, String SPageNumber, String SPageSize)
        {
            int PageNumber = Convert.ToInt16(SPageNumber);
            int PageSize = Convert.ToInt16(SPageSize);
            if (ViewState["IsSorting"].ToString().Equals("true"))
            {
                dt.DefaultView.Sort = ViewState["SortString"].ToString() + " " + ViewState["SortingOrder"].ToString();
                if (PageSize * (PageNumber - 1) > _TotalRowCount)
                    PageNumber = (int)Math.Ceiling((decimal)_TotalRowCount / PageSize);
                deliverydata.DataSource = dt.DefaultView.ToTable().Rows.Cast<System.Data.DataRow>().Skip((PageNumber - 1) * PageSize).Take(PageSize).CopyToDataTable();
            }
            else
                deliverydata.DataSource = dt;
            deliverydata.DataBind();
            dt.Dispose();
        }




        protected void gvDeliveryResults_Sorting(object sender, GridViewSortEventArgs e)
        {
            ViewState["ShimpentPendingIsInsert"] = false;
            deliverydata.EditIndex = -1;
            //this.DeliveryResults_buildGridData(this.DeliveryResults_buildGridData());
            this.DeliveryResults_buildGridData(this.DeliveryResults_buildGridData(hfpageId.Value, drppagesize.SelectedValue), hfpageId.Value, drppagesize.SelectedValue);
        }



        protected void gvDeliveryResults_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            ViewState["DeliveryResultsIsInsert"] = false;

            deliverydata.PageIndex = e.NewPageIndex;
            deliverydata.EditIndex = -1;
            //this.DeliveryResults_buildGridData(this.DeliveryResults_buildGridData());
            this.DeliveryResults_buildGridData(this.DeliveryResults_buildGridData(hfpageId.Value, drppagesize.SelectedValue), hfpageId.Value, drppagesize.SelectedValue);
        }



        protected void gvDeliveryResults_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //======================= Added By M.D.Prasad For View Only Condition ======================//
            string[] hh = cp.Roles;
            for (int i = 0; i < hh.Length; i++)
            {
                if (cp.UserTypeID == 3 && Convert.ToInt32(hh[i]) == 4)
                {
                    imgbtnoutboundsearch.Visible = false;
                }
            }
            //======================= Added By M.D.Prasad For View Only Condition ======================//

            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                if ((e.Row.RowState == DataControlRowState.Normal) || (e.Row.RowState == DataControlRowState.Alternate))
                {
                    DataRow row = ((DataRowView)(e.Row.DataItem)).Row;
                    String vPriority = ((Literal)e.Row.FindControl("ltPriorityLevel")).Text;

                    Literal ltDelvType = (Literal)e.Row.FindControl("ltDelvType");
                    Literal ltDocTypeID = (Literal)e.Row.FindControl("ltDocTypeID");
                    Literal ltIsReservationCheck = (Literal)e.Row.FindControl("ltIsReservationCheck");

                    Label lblOBDID = (Label)e.Row.FindControl("lblOBDID");
                    Label lblLineItemCount = (Label)e.Row.FindControl("lblLineItemCount");

                    HyperLink link = (HyperLink)e.Row.FindControl("lnkHyperLink");
                    HyperLink linkDel = (HyperLink)e.Row.FindControl("lnkHyperLinkDel");


                    if (ltDocTypeID.Text != "6" && ltDocTypeID.Text != "9")
                    {
                        link.NavigateUrl = "DeliveryPickNote.aspx?obdid=" + lblOBDID.Text + "&lineitemcount=" + lblLineItemCount.Text + "&TN=" + row["TenantName"];
                        linkDel.NavigateUrl = "DeliveryPickNoteData.aspx?obdid=" + lblOBDID.Text + "&lineitemcount=" + lblLineItemCount.Text + "&TN=" + row["TenantName"];
                    }
                    else
                    {
                        link.NavigateUrl = "~/mManufacturingProcess/DeliveryPickNote.aspx?obdid=" + lblOBDID.Text + "&lineitemcount=" + lblLineItemCount.Text + "&TN=" + row["TenantName"];
                        linkDel.NavigateUrl = "~/mManufacturingProcess/DeliveryPickNote.aspx?obdid=" + lblOBDID.Text + "&lineitemcount=" + lblLineItemCount.Text + "&TN=" + row["TenantName"];
                    }


                    //======================= Added By M.D.Prasad For View Only Condition ======================//
                    for (int i = 0; i < hh.Length; i++)
                    {
                        if (cp.UserTypeID == 3 && Convert.ToInt32(hh[i]) == 4)
                        {
                            deliverydata.Columns[14].Visible = false;
                            deliverydata.Columns[16].Visible = false;
                        }
                    }
                    //======================= Added By M.D.Prasad For View Only Condition ======================//

                    /*
                    //If Delivery Reservation Mnetion that with the folowing code
                    if (ltDocTypeID.Text == "4")
                    {
                        if (ltIsReservationCheck.Text == "1")
                            ltDelvType.Text = "Delivery Reservation";

                    }

                    if (vPriority != "")
                    {
                        String vltPriorityTime = ((Literal)e.Row.FindControl("ltPriorityTime")).Text;
                        String vltDeliverystatusID = ((Literal)e.Row.FindControl("ltHidDeliveryStatusID")).Text;

                        StringBuilder StrTimetickerJS = new StringBuilder();

                        if (!(vltDeliverystatusID == "3" || vltDeliverystatusID == "10" || vltDeliverystatusID == "10"))
                        {
                            if (vltPriorityTime != "")
                            {

                                DateTime dtPriorityDateTime = Convert.ToDateTime(vltPriorityTime);



                                StrTimetickerJS.Append("<script type=\"text/javascript\">");
                                StrTimetickerJS.Append("$(function() {");
                                StrTimetickerJS.Append("var austDay = new Date();");
                                StrTimetickerJS.Append("austDay = new Date(" + dtPriorityDateTime.Year.ToString() + "," + (dtPriorityDateTime.Month - 1).ToString() + "," + dtPriorityDateTime.Day.ToString() + "," + dtPriorityDateTime.Hour.ToString() + "," + dtPriorityDateTime.Minute.ToString() + "," + CommonLogic.RandomNumber(60).ToString() + ");");
                                //StrTimetickerJS.Append("austDay = new Date(" + vltPriorityTime + ");");
                                //StrTimetickerJS.Append("$('#defaultCountdown" + e.Row.RowIndex.ToString() + "').countdown({ until: austDay, compact: true, format: 'dHMS', description: ''});");


                                if (dtPriorityDateTime > DateTime.Now)
                                {
                                    StrTimetickerJS.Append("$('#defaultCountdown" + e.Row.RowIndex.ToString() + "').countdown({ until: austDay, compact: true, format: 'dHMS', description: ''});");
                                }
                                else
                                {
                                    StrTimetickerJS.Append("$('#defaultCountdown" + e.Row.RowIndex.ToString() + "').countdown({ since: austDay, compact: true, format: 'dHMS', description: ''});");
                                }

                                StrTimetickerJS.Append("});</script>");

                                if (dtPriorityDateTime > DateTime.Now)
                                {
                                    StrTimetickerJS.Append("<img src=\"..\\Images\\horse1.gif\" border=\"0\" width=\"25\"/><div id=\"defaultCountdown" + e.Row.RowIndex.ToString() + "\" class=\"timeAlertDiv\"></div>");
                                    e.Row.Cells[0].Text = StrTimetickerJS.ToString();
                                }
                                else
                                {
                                    StrTimetickerJS.Append("<div id=\"defaultCountdown" + e.Row.RowIndex.ToString() + "\" class=\"timeAlertDiv\"></div>");
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

            }
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



        #endregion

        #region Export To Excel Beg 


        //protected void imgbtnoutboundsearch_Click(object sender, ImageClickEventArgs e)
        protected void imgbtnoutboundsearch_Click(object sender, EventArgs e)
        {
            try
            {

                String[] hiddencolumns = { "", "Modify" };
                deliverydata.AllowPaging = false;
                //deliverydata.Columns[15].Visible = false;
                deliverydata.Columns[14].Visible = false;
                //this.DeliveryResults_buildGridData(this.DeliveryResults_buildGridData());
                this.DeliveryResults_buildGridData(this.DeliveryResults_buildGridData("1", drppagesize.SelectedValue), hfpageId.Value, drppagesize.SelectedValue);
                CommonLogic.ExporttoExcel(deliverydata, "OutBoundSearch", hiddencolumns);
                //deliverydata.Columns[15].Visible = true;
                deliverydata.Columns[14].Visible = true;
            }
            catch (Exception ex)
            {
                //deliverydata.Columns[15].Visible = true;
                deliverydata.Columns[14].Visible = true;
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
                resetError("Error While Exporting Data", true);
            }
        }
        public override void VerifyRenderingInServerForm(Control control)
        {
            /* Verifies that the control is rendered */
        }

        #endregion End Export To Excel Beg

        protected void drppagesize_SelectedIndexChanged(object sender, EventArgs e)
        {
            cp = HttpContext.Current.User as CustomPrincipal;
            this.DeliveryResults_buildGridData(this.DeliveryResults_buildGridData(hfpageId.Value, drppagesize.SelectedValue), hfpageId.Value, drppagesize.SelectedValue);
        }

        protected void dlPagerupper_ItemCommand(object source, DataListCommandEventArgs e)
        {
            if (e.CommandName == "PageNo")
            {
                hfpageId.Value = e.CommandArgument.ToString();
                this.DeliveryResults_buildGridData(this.DeliveryResults_buildGridData(hfpageId.Value, drppagesize.SelectedValue), hfpageId.Value, drppagesize.SelectedValue);
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
                pageLinkContainer.Add(new ListItem("&lt;&lt;FirstPage", "01", currentPage != 1));
            for (int i = startPageLink; i <= lastPageLink; i++)
            {
                pageLinkContainer.Add(new ListItem(i.ToString("00"), i.ToString(), currentPage != i));
            }
            if (lastPageLink != totalPageCount)
                pageLinkContainer.Add(new ListItem("LastPage&gt;&gt;", totalPageCount.ToString("00"), currentPage != totalPageCount));

            dlPagerupper.DataSource = pageLinkContainer;
            dlPagerupper.DataBind();
            dlPager.DataSource = pageLinkContainer;
            dlPager.DataBind();
        }

        //==================== Added By M.D.Prasad ON 01-Jun-2020 for Generate Packing Slip PDF ======================//       
        public class PackingPDFData
        {
            public string AWBNo { get; set; }
            public string Courier { get; set; }
            public string Priority { get; set; }
            public string DueDate { get; set; }
            public string Notes { get; set; }
            public string Address { get; set; }
            public int OutboundID { get; set; }
            public string TenantCode { get; set; }
        }

        protected void lnkTest_Click(object sender, EventArgs e)
        {
            OutBoundBL obj = new OutBoundBL();
            // obj.generatePDFData();
        }

        protected void lnkGeneratePckingSlipPDF_Click(object sender, EventArgs e)
        {
            cp = HttpContext.Current.User as CustomPrincipal;
            try
            {
                //string[] gvMCode_POs ;

                ArrayList gvMCode_POs = new ArrayList();

                bool chkBox = false;

                int count = 0;
                int isPrintchkcount = 0;
                GridViewRow abcd = null;
                for (int i = 0; i < deliverydata.Rows.Count; i++)
                {
                    abcd = deliverydata.Rows[i];
                    bool isChecked = ((CheckBox)abcd.FindControl("chkIsPrint")).Checked;
                    if (isChecked)
                    {
                        isPrintchkcount = isPrintchkcount + 1;
                        count++;
                    }
                }

                if (isPrintchkcount != 0)
                {
                    OutBoundBL obj = new OutBoundBL();
                    List<PackingPDFData> _pdfData = new List<PackingPDFData>();
                    //'Navigate through each row in the GridView for checkbox items
                    foreach (GridViewRow gv in deliverydata.Rows)
                    {
                        CheckBox isPrint = (CheckBox)gv.FindControl("chkIsPrint");
                        if (isPrint.Checked)
                        {
                            chkBox = true;
                            String ltOutboundNo = ((Literal)gv.FindControl("ltOBDNumber")).Text.ToString();
                            String ltTenantName = ((Literal)gv.FindControl("ltTenantName")).Text.ToString();
                            string query = "select OutboundID AS N from OBD_Outbound where IsActive=1 and IsDeleted=0 and OBDNumber='" + ltOutboundNo + "'";
                            int OutboundID = DB.GetSqlN(query);
                            PackingPDFData PDFData = new PackingPDFData
                            {
                                OutboundID = OutboundID,
                                TenantCode = ltTenantName,
                            };
                            _pdfData.Add(PDFData);
                        }
                    }
                    obj.generatePDFDataForServer(_pdfData);
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

        }

        //=================== END ====================//
    }
}