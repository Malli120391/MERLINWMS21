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
using DocumentFormat.OpenXml.Drawing.Charts;

// Module Name : OutboundTracking Under Outbound
// DevelopedBy : Naresh P
// Created On  : 17/11/2013
// Modified On : 24/03/2015

namespace MRLWMSC21.mOutbound
{
    public partial class OutboundTracking : System.Web.UI.Page
    {
        public CustomPrincipal cp = HttpContext.Current.User as CustomPrincipal;
        string TenantRootDir = "";
        string OutboundPath = "";
        string OBD_DeliveryNotePath = "";
        string OBD_PickandCheckSheetPath = "";
        string OBD_PODPath = "";

        DataSet dsDeliveryPending;
        DataSet dsPODPending;

        String DeliveryPendingSQL = "EXEC [dbo].[sp_OBD_GetDeliveriesPendingList]  ";
        String VLPDPendingSQL = "EXEC [dbo].[sp_OBD_GetVLPDPendingOBDList]  ";
        String DCRPendingSQL = "EXEC [dbo].[sp_OBD_GetPODPendingList]  ";

        String PGIPendingSQL = "EXEC [dbo].[sp_OBD_GetPGIPendingList]  ";
        String DeliveriesInProcessSQL = "EXEC [dbo].[sp_OBD_GetDIPList]   ";

        String PlantDeliveryNoteSQL = "EXEC [dbo].[sp_OBD_GetPlantDeliveryNoteList]";


        DataSet ds_PGIPending;
        DataSet ds_PDNOBDLinkPending;


        protected void Page_PreInit(object sender, EventArgs e)
        {
            Page.Theme = "Outbound";
        }




        protected void Page_Load(object sender, EventArgs e)
        {

            cp = HttpContext.Current.User as CustomPrincipal;
            if (!cp.IsInAnyRoles(CommonLogic.GetRolesAllowed(CommonLogic.GetRolesAllowedForthisPage("Outbound Tracking"))))
            {
                Response.Redirect("../Login.aspx?eid=6");
            }

            DesignLogic.SetInnerPageSubHeading(this.Page, "Outbound Tracking");


            string query = "EXEC [dbo].[sp_TPL_GetTenantDirectoryInfo] @TypeID=2";

            DataSet dsPath = DB.GetDS(query.ToString(), false);

            TenantRootDir = dsPath.Tables[0].Rows[4][0].ToString();
            OutboundPath = dsPath.Tables[0].Rows[0][0].ToString();
            OBD_DeliveryNotePath = dsPath.Tables[0].Rows[1][0].ToString();
            OBD_PickandCheckSheetPath = dsPath.Tables[0].Rows[2][0].ToString();
            OBD_PODPath = dsPath.Tables[0].Rows[3][0].ToString();

            if (!IsPostBack) {

                //txtdptenant.text = cp.firstname;
                //txtdptenant.enabled = false;
                //hifdptenantid.value = cp.tenantid.tostring();

                //txtpptenant.text = cp.firstname;
                //txtpptenant.enabled = false;
                //hifpptenantid.value = cp.tenantid.tostring();

                //txtpgitenant.text = cp.firstname;
                //txtpgitenant.enabled = false;
                //hifpgitenantid.value = cp.tenantid.tostring();

                //txtpncptenant.text = cp.firstname;
                //txtpncptenant.enabled = false;
                //hifpncptenantid.value = cp.tenantid.tostring();

                if (cp.TenantID != 0)
                {
                    gvOBDInProcess.Columns[4].Visible = false;
                    gvOBDPGIPending.Columns[4].Visible = false;
                    gvOBDReceived.Columns[3].Visible = false;
                    gvDCRPending.Columns[3].Visible = false;
                }


                txtDPTenant.Enabled = CommonLogic.CheckSuperAdmin(txtDPTenant, cp, hifDPTenantID);
                txtPPTenant.Enabled = CommonLogic.CheckSuperAdmin(txtPPTenant, cp, hifPPTenantID);
                txtPGITenant.Enabled = CommonLogic.CheckSuperAdmin(txtPGITenant, cp, hifPGITenantID);
                txtPNCpTenant.Enabled = CommonLogic.CheckSuperAdmin(txtPNCpTenant, cp, hifPNCpTenantID);

                // PGI Pending
                this.PGIPendingSQL += "@OBDNumber=NULL,@WarehouseIDs=" + DB.SQuote(String.Join(",", cp.Warehouses)) + ",@Tenant=NULL,@TenantID=" + cp.TenantID;
           

                ViewState["PGIPendingSQLString"] = this.PGIPendingSQL + ", @AccountID_New = " + cp.AccountID.ToString() + ",@UserID_New=" + cp.UserID + ",@UserTypeID_New = " + cp.UserTypeID.ToString() + ",@TenantID_New = " + cp.TenantID.ToString();
                this.ObdPGIPending_buildGridData(this.ObdPGIPending_buildGridData());

                // DeliveriesInProcess
                this.DeliveriesInProcessSQL += "@OBDNumber=NULL,@WarehouseIDs=" + DB.SQuote(String.Join(",", cp.Warehouses)) + ",@Tenant=NULL,@TenantID=" + cp.TenantID;

                ViewState["DeliveriesInProcessSQLString"] = this.DeliveriesInProcessSQL + ", @AccountID_New = " + cp.AccountID.ToString() + ",@UserID_New=" + cp.UserID + ",@UserTypeID_New = " + cp.UserTypeID.ToString() + ",@TenantID_New = " + cp.TenantID.ToString();
                this.ObdPending_buildGridData(this.OBDPending_buildGridData());


                //  ViewState["PlantDeliverySQLString"] = this.PlantDeliveryNoteSQL + "@OBDNumber=NULL,@WarehouseIDs=" + DB.SQuote(String.Join(",", cp.Warehouses));
                //  this.OBDPlant_buildGridData(this.OBDPlant_buildGridData());


                // Delivery Pendings
                this.DeliveryPendingSQL += "@OBDNumber=NULL,@WarehouseIDs=" + DB.SQuote(String.Join(",", cp.Warehouses)) + ",@Tenant=NULL,@TenantID=" + cp.TenantID;

                //  this.DeliveryPendingSQL = this.DeliveryPendingSQL + ", @AccountID_New = " + cp.AccountID.ToString() + ",@UserTypeID_New = " + cp.UserTypeID.ToString() + ",@TenantID_New = " + cp.TenantID.ToString();

                ViewState["DeliveryPendingSQLString"] = this.DeliveryPendingSQL + ", @AccountID_New = " + cp.AccountID.ToString() + ",@UserID_New="+cp.UserID+",@UserTypeID_New = " + cp.UserTypeID.ToString() + ",@TenantID_New = " + cp.TenantID.ToString();
                ObdDeliveryPending_buildGridData(this.ObdDeliveryPending_buildGridData());



                // Delivery Pendings
                this.VLPDPendingSQL += "@OBDNumber=NULL,@WarehouseIDs=" + DB.SQuote(String.Join(",", cp.Warehouses)) + ",@Tenant=NULL,@TenantID=" + cp.TenantID;

                ViewState["VLPDPendingSQLString"] = this.VLPDPendingSQL + ", @AccountID_New = " + cp.AccountID.ToString() + ",@UserID_New=" + cp.UserID + ",@UserTypeID_New = " + cp.UserTypeID.ToString() + ",@TenantID_New = " + cp.TenantID.ToString();
                ObdVLPDPending_buildGridData(this.ObdVLPDPending_buildGridData());


                //POD Pending
                this.DCRPendingSQL += "@OBDNumber=NULL,@WarehouseIDs=" + DB.SQuote(String.Join(",", cp.Warehouses)) + ",@Tenant=NULL,@TenantID=" + cp.TenantID;

                // this.DCRPendingSQL = this.DCRPendingSQL + ", @AccountID_New = " + cp.AccountID.ToString() + ",@UserTypeID_New = " + cp.UserTypeID.ToString() + ",@TenantID_New = " + cp.TenantID.ToString();
                ViewState["DCRPendingSQLString"] = this.DCRPendingSQL + ", @AccountID_New = " + cp.AccountID.ToString() + ",@UserID_New=" + cp.UserID + ",@UserTypeID_New = " + cp.UserTypeID.ToString() + ",@TenantID_New = " + cp.TenantID.ToString();
                ObdDCRPending_buildGridData(this.ObdDCRPending_buildGridData());
                
            }


        }






        #region -------------------------PGI Pending -----------------------------

        protected DataSet ObdPGIPending_buildGridData()
        {
            string sql = ViewState["PGIPendingSQLString"].ToString();
           
            ds_PGIPending = DB.GetDS(sql, false);

            if (ds_PGIPending.Tables[0].Rows.Count == 0)
            {
                lblPGIRecordCount.InnerText = "";
            }
            else
            {

                if (ds_PGIPending.Tables[0].Rows[0]["TodayCount"].ToString() != "0")
                {
                    lbltodayPGIRecCount.InnerHtml = "&nbsp;" + ds_PGIPending.Tables[0].Rows[0]["TodayCount"].ToString() + "&nbsp;";
                }

                lblPGIRecordCount.InnerHtml = "&nbsp;&nbsp;&nbsp;  [" + ds_PGIPending.Tables[0].Rows.Count.ToString() + "]";
            }
            return ds_PGIPending;

        }

        protected void ObdPGIPending_buildGridData(DataSet ds)
        {
            gvOBDPGIPending.DataSource = ds;
            gvOBDPGIPending.DataBind();
            ds.Dispose();
        }

        protected void gvOBDPGIPending_Sorting(object sender, GridViewSortEventArgs e)
        {
            this.ObdPGIPending_buildGridData(this.ObdPGIPending_buildGridData());
        }

        protected void gvOBDPGIPending_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            ViewState["PendingIsInsert"] = false;
         
            gvOBDPGIPending.PageIndex = e.NewPageIndex;
            gvOBDPGIPending.EditIndex = -1;
            this.ObdPGIPending_buildGridData(this.ObdPGIPending_buildGridData());
        }

        protected void gvOBDPGIPending_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //======================= Added By M.D.Prasad For View Only Condition ======================//
            cp = HttpContext.Current.User as CustomPrincipal;
            string[] hh = cp.Roles;
            for (int i = 0; i < hh.Length; i++)
            {
                if (cp.UserTypeID == 3 && Convert.ToInt32(hh[i]) == 4)
                {
                    imgbtngvOBDPGIPending.Visible = false;
                }
            }
            //======================= Added By M.D.Prasad For View Only Condition ======================//
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                if ((e.Row.RowState == DataControlRowState.Normal) || (e.Row.RowState == DataControlRowState.Alternate))
                {
                    if (cp.TenantID != 0)
                    {
                        gvOBDPGIPending.Columns[10].Visible = false;
                    }

                    DataRow row = ((DataRowView)e.Row.DataItem).Row;

                    String ltGTSDate = ((Literal)e.Row.FindControl("ltGTSDate")).Text;
                    String ltOBDNumber_Pending = ((Literal)e.Row.FindControl("ltOBDNumber_Pending")).Text;



                    Literal ltDocTypeID = (Literal)e.Row.FindControl("ltDocTypeID");
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
                        link.NavigateUrl = "~/mManufacturingProcess/DeliveryPickNote.aspx?obdid=" + lblOBDID.Text + "&lineitemcount=" + lblLineItemCount.Text + "&TN=" + row["TenantName"]; ;
                        //linkDel.NavigateUrl = "~/mManufacturingProcess/DeliveryPickNote.aspx?obdid=" + lblOBDID.Text + "&lineitemcount=" + lblLineItemCount.Text + "&TN=" + row["TenantName"]; ;
                    }

                    //======================= Added By M.D.Prasad For View Only Condition ======================//
                    for (int i = 0; i < hh.Length; i++)
                    {
                        if (cp.UserTypeID == 3 && Convert.ToInt32(hh[i]) == 4)
                        {
                            gvOBDPGIPending.Columns[9].Visible = false;
                            gvOBDPGIPending.Columns[10].Visible = false;
                        }
                    }
                    //======================= Added By M.D.Prasad For View Only Condition ======================//


                    /*

                    String vPriority = ((Literal)e.Row.FindControl("ltPriorityLevel")).Text;

                    if (vPriority != "")
                    {
                        String vltPriorityTime = ((Literal)e.Row.FindControl("ltPriorityTime")).Text;

                        StringBuilder StrTimetickerJS = new StringBuilder();

                        if (vltPriorityTime != "")
                        {

                            DateTime dtPriorityDateTime = Convert.ToDateTime(vltPriorityTime);

                            StrTimetickerJS.Append("<script type=\"text/javascript\">");
                            StrTimetickerJS.Append("$(function() {");
                            StrTimetickerJS.Append("var austDay = new Date();");
                            StrTimetickerJS.Append("austDay = new Date(" + dtPriorityDateTime.Year.ToString() + "," + (dtPriorityDateTime.Month - 1).ToString() + "," + dtPriorityDateTime.Day.ToString() + "," + dtPriorityDateTime.Hour.ToString() + "," + dtPriorityDateTime.Minute.ToString() + "," + CommonLogic.RandomNumber(60).ToString() + ");");
                            //StrTimetickerJS.Append("austDay = new Date(" + vltPriorityTime + ");");
                            //StrTimetickerJS.Append("$('#OBDPGIPendingCD" + e.Row.RowIndex.ToString() + "').countdown({ until: austDay, compact: true, format: 'dHMS', description: ''});");

                            if (dtPriorityDateTime > DateTime.Now)
                            {
                                StrTimetickerJS.Append("$('#OBDPGIPendingCD" + e.Row.RowIndex.ToString() + "').countdown({ until: austDay, compact: true, format: 'dHMS', description: ''});");
                            }
                            else
                            {
                                StrTimetickerJS.Append("$('#OBDPGIPendingCD" + e.Row.RowIndex.ToString() + "').countdown({ since: austDay, compact: true, format: 'dHMS', description: ''});");
                            }

                            StrTimetickerJS.Append("});</script>");

                            if (dtPriorityDateTime > DateTime.Now)
                            {
                                StrTimetickerJS.Append("<img src=\"..\\images\\horse1.gif\" border=\"0\" width=\"25\"/><div id=\"OBDPGIPendingCD" + e.Row.RowIndex.ToString() + "\" class=\"timeAlertDiv\"></div>");
                                e.Row.Cells[0].Text = StrTimetickerJS.ToString();
                            }
                            else
                            {
                                StrTimetickerJS.Append("<div id=\"OBDPGIPendingCD" + e.Row.RowIndex.ToString() + "\" class=\"timeAlertDiv\"></div>");
                                e.Row.Cells[0].Text = " <span class='clsTimeOut'>" + StrTimetickerJS.ToString() + "</span>";
                            }

                            // StrTimetickerJS.Append("<img src=\"images\\horse1.gif\" border=\"0\" width=\"25\"/><div id=\"OBDPGIPendingCD" + e.Row.RowIndex.ToString() + "\" class=\"timeAlertDiv\"></div>");
                            //e.Row.Cells[0].Text = StrTimetickerJS.ToString();

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
                    }*/
                }

            }
        }

        protected void lnklPGISearch_Click(object sender, EventArgs e)
        {
            cp = HttpContext.Current.User as CustomPrincipal;
            string OBDNumber = txtPGIOBDNumber.Text.Trim();
            string Tenant = txtPGITenant.Text.Split('-')[0];

            if (OBDNumber == "Search Delv. Doc.#...")
                OBDNumber = "";

            if (Tenant == "Search Tenant...")
            {
                Tenant = "";
                hifPGITenantID.Value = "0";
            }
            this.PGIPendingSQL += "@OBDNumber=" + DB.SQuote(OBDNumber) + ",@WarehouseIDs=" + DB.SQuote(String.Join(",", cp.Warehouses)) + ",@Tenant=" + DB.SQuote(Tenant);

            ViewState["PGIPendingSQLString"] = this.PGIPendingSQL + ", @AccountID_New = " + cp.AccountID.ToString() + ",@UserTypeID_New = " + cp.UserTypeID.ToString() + ",@TenantID="+ hifPGITenantID.Value + ",@TenantID_New = " + cp.TenantID.ToString();

       //     ViewState["PGIPendingSQLString"] = this.PGIPendingSQL + "@OBDNumber=" + DB.SQuote(OBDNumber) + ",@WarehouseIDs=" + DB.SQuote(String.Join(",", cp.Warehouses)) + ",@Tenant=" + DB.SQuote(Tenant);

            this.ObdPGIPending_buildGridData(this.ObdPGIPending_buildGridData());
            txtPGIOBDNumber.Text = "Search Delv. Doc.#...";
            txtPGITenant.Text = "Search Tenant...";
            hifPGITenantID.Value = "0";

        }






        #endregion -------------------------PGI Pending -----------------------------

        #region -------------------------VLPD Pending -----------------------------

        #endregion  -------------------------VLPD Pending -----------------------------

        #region -------------------------Delivery's In Process -----------------------------

        protected DataSet OBDPending_buildGridData()
        {
            cp = HttpContext.Current.User as CustomPrincipal;
            string sql = ViewState["DeliveriesInProcessSQLString"].ToString();
            
            DataSet ds = DB.GetDS(sql, false);

              

            if (ds.Tables[0].Rows.Count == 0)
            {
                lblDeliveryRecCount.InnerText = "";
            }
            else {

                if (ds.Tables[0].Rows[0]["TodayCount"].ToString() != "0")
                {
                    lbltodayRecCount.InnerHtml = "&nbsp;" + ds.Tables[0].Rows[0]["TodayCount"].ToString() + "&nbsp;";
                    
                    
                }

                lblDeliveryRecCount.InnerHtml = "&nbsp;&nbsp;&nbsp;  [" + ds.Tables[0].Rows.Count.ToString() + "]";
            }
           
            return ds;

        }

        protected void ObdPending_buildGridData(DataSet ds)
        {
            gvOBDInProcess.DataSource = ds;
            gvOBDInProcess.DataBind();
            ds.Dispose();
        }

        protected void gvOBDInProcess_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            this.ObdPending_buildGridData(this.OBDPending_buildGridData());
        }

        protected void gvOBDInProcess_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //======================= Added By M.D.Prasad For View Only Condition ======================//
            cp = HttpContext.Current.User as CustomPrincipal;
            string[] hh = cp.Roles;
            for (int i = 0; i < hh.Length; i++)
            {
                if (cp.UserTypeID == 3 && Convert.ToInt32(hh[i]) == 4)
                {
                    imgbtngvOBDInProcess.Visible = false;
                }
            }
            //======================= Added By M.D.Prasad For View Only Condition ======================//
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (cp.TenantID != 0)
                {
                    gvOBDInProcess.Columns[9].Visible = false;
                }

                if ((e.Row.RowState == DataControlRowState.Normal) || (e.Row.RowState == DataControlRowState.Alternate))
                {

                    DataRow row = ((DataRowView)e.Row.DataItem).Row;

                    String vPriority = ((Literal)e.Row.FindControl("ltPriorityLevel")).Text;
                    String vltPriorityTime = ((Literal)e.Row.FindControl("ltPriorityTime")).Text;


                    Literal ltDocTypeID = (Literal)e.Row.FindControl("ltDocTypeID");
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
                    }

                    //======================= Added By M.D.Prasad For View Only Condition ======================//
                    for (int i = 0; i < hh.Length; i++)
                    {
                        if (cp.UserTypeID == 3 && Convert.ToInt32(hh[i]) == 4)
                        {
                            gvOBDInProcess.Columns[8].Visible = false;
                            gvOBDInProcess.Columns[9].Visible = false;
                        }
                    }
                    //======================= Added By M.D.Prasad For View Only Condition ======================//



                    /*
                    if (vPriority != "")
                    {
                        StringBuilder StrTimetickerJS = new StringBuilder();

                        if (vltPriorityTime != "")
                        {

                            DateTime dtPriorityDateTime = Convert.ToDateTime(vltPriorityTime);

                            StrTimetickerJS.Append("<script type=\"text/javascript\">");
                            StrTimetickerJS.Append("$(function() {");
                            StrTimetickerJS.Append("var austDay = new Date();");
                            StrTimetickerJS.Append("austDay = new Date(" + dtPriorityDateTime.Year.ToString() + "," + (dtPriorityDateTime.Month - 1).ToString() + "," + dtPriorityDateTime.Day.ToString() + "," + dtPriorityDateTime.Hour.ToString() + "," + dtPriorityDateTime.Minute.ToString() + "," + CommonLogic.RandomNumber(60).ToString() + ");");
                            //StrTimetickerJS.Append("austDay = new Date(" + vltPriorityTime + ");");
                            //StrTimetickerJS.Append("$('#CDOBDInProcess" + e.Row.RowIndex.ToString() + "').countdown({ until: austDay, compact: true, format: 'dHMS', description: ''});");

                            if (dtPriorityDateTime > DateTime.Now)
                            {
                                StrTimetickerJS.Append("$('#CDOBDInProcess" + e.Row.RowIndex.ToString() + "').countdown({ until: austDay, compact: true, format: 'dHMS', description: ''});");
                            }
                            else
                            {
                                StrTimetickerJS.Append("$('#CDOBDInProcess" + e.Row.RowIndex.ToString() + "').countdown({ since: austDay, compact: true, format: 'dHMS', description: ''});");
                            }

                            StrTimetickerJS.Append("});</script>");

                            //StrTimetickerJS.Append("<img src=\"images\\horse1.gif\" border=\"0\" width=\"25\"/> <div id=\"CDOBDInProcess" + e.Row.RowIndex.ToString() + "\" class=\"timeAlertDiv\"></div>");
                            //e.Row.Cells[0].Text = StrTimetickerJS.ToString();

                            if (dtPriorityDateTime > DateTime.Now)
                            {
                                StrTimetickerJS.Append("<img src=\"..\\images\\horse1.gif\" border=\"0\" width=\"25\"/><div id=\"CDOBDInProcess" + e.Row.RowIndex.ToString() + "\" class=\"timeAlertDiv\"></div>");
                                e.Row.Cells[0].Text = StrTimetickerJS.ToString();
                            }
                            else
                            {
                                StrTimetickerJS.Append("<div id=\"CDOBDInProcess" + e.Row.RowIndex.ToString() + "\" class=\"timeAlertDiv\"></div>");
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
                    }*/



                }

            }
        }

        protected void gvOBDInProcess_Sorting(object sender, GridViewSortEventArgs e)
        {
            this.ObdPending_buildGridData(this.OBDPending_buildGridData());
        }

        protected void gvOBDInProcess_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            ViewState["PendingIsInsert"] = false;
          
            gvOBDInProcess.PageIndex = e.NewPageIndex;
            gvOBDInProcess.EditIndex = -1;
            this.ObdPending_buildGridData(this.OBDPending_buildGridData());
        }

        protected void gvOBDInProcess_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }

        protected void lnkDelSearch_Click(object sender, EventArgs e)
        {
            cp = HttpContext.Current.User as CustomPrincipal;
            string OBDNumber = txtDelDocNumber.Text.Trim();
            string Tenant = txtPNCpTenant.Text.Split('-')[0];

            if (OBDNumber == "")
                OBDNumber = "";

            if (Tenant == "")
            {
                Tenant = "";
                hifPNCpTenantID.Value = "0";
            }
            if (txtWarehouse.Text == "")
            {
                hdnWarehouse.Value = "";
            }

           // this.DeliveriesInProcessSQL += "@OBDNumber=" + DB.SQuote(OBDNumber) + ",@WarehouseIDs=" + DB.SQuote(String.Join(",", cp.Warehouses)) + ",@Tenant=" + DB.SQuote(Tenant); 
            this.DeliveriesInProcessSQL += "@OBDNumber=" + DB.SQuote(OBDNumber) + ",@WarehouseIDs=" + DB.SQuote(hdnWarehouse.Value) + ",@Tenant=" + DB.SQuote(Tenant);

            ViewState["DeliveriesInProcessSQLString"] = this.DeliveriesInProcessSQL + ", @AccountID_New = " + cp.AccountID.ToString() + ",@UserTypeID_New = " + cp.UserTypeID.ToString() + ",@TenantID="+ hifPNCpTenantID.Value + ",@TenantID_New = " + cp.TenantID.ToString();


          //  ViewState["DeliveriesInProcessSQLString"] = this.DeliveriesInProcessSQL + "@OBDNumber=" + DB.SQuote(OBDNumber) + ",@WarehouseIDs=" + DB.SQuote(String.Join(",", cp.Warehouses)) + ",@Tenant=" + DB.SQuote(Tenant);

            this.ObdPending_buildGridData(this.OBDPending_buildGridData());
            //txtDelDocNumber.Text = "Search Delv. Doc.#...";
            //txtPNCpTenant.Text = "Search Tenant...";


        }


        protected void lnnkVLPDPendingSearch_Click(object sender, EventArgs e)
        {
            cp = HttpContext.Current.User as CustomPrincipal;
            string OBDNumber = txtVLPDDocNumber.Text.Trim();
            string Tenant = hifVLPDTenantID.Value.ToString();

            if (OBDNumber == "Search Delv. Doc.#...")
                OBDNumber = "";

            if (Tenant == "Search Tenant...")
                Tenant = "";

            this.VLPDPendingSQL += "@OBDNumber=" + DB.SQuote(OBDNumber) + ",@WarehouseIDs=" + DB.SQuote(String.Join(",", cp.Warehouses)) + ",@Tenant=" + DB.SQuote(Tenant);

            ViewState["VLPDPendingSQLString"] = this.VLPDPendingSQL + ", @AccountID_New = " + cp.AccountID.ToString() + ",@UserTypeID_New = " + cp.UserTypeID.ToString() + ",@TenantID_New = " + cp.TenantID.ToString();
            ObdVLPDPending_buildGridData(this.ObdVLPDPending_buildGridData());
            txtDelDocNumber.Text = "Search Delv. Doc.#...";
            txtPNCpTenant.Text = "Search Tenant...";


        }




        #endregion -------------------------Delivery's In Process -----------------------------

        protected DataSet ObdVLPDPending_buildGridData()
        {
            string sql = ViewState["VLPDPendingSQLString"].ToString();
            dsDeliveryPending = DB.GetDS(sql, false);

            if (dsDeliveryPending.Tables[0].Rows.Count == 0)
            {
                spnvlpdcount.InnerText = "";
            }
            else
            {

                if (dsDeliveryPending.Tables[0].Rows[0]["TodayCount"].ToString() != "0")
                {
                    spnvlpdcount.InnerHtml = "&nbsp;" + dsDeliveryPending.Tables[0].Rows[0]["TodayCount"].ToString() + "&nbsp;";
                }

                spnvlpdcount.InnerHtml = "&nbsp;&nbsp;&nbsp;  [" + dsDeliveryPending.Tables[0].Rows.Count.ToString() + "]";
            }
            return dsDeliveryPending;

        }


        #region -------------------------Delivery Pending -----------------------------

        protected DataSet ObdDeliveryPending_buildGridData()
        {
            string sql = ViewState["DeliveryPendingSQLString"].ToString();
            dsDeliveryPending = DB.GetDS(sql, false);

            if (dsDeliveryPending.Tables[0].Rows.Count == 0)
            {
                lblDelvPRecCount.InnerText = "";
            }
            else
            {

                if (dsDeliveryPending.Tables[0].Rows[0]["TodayCount"].ToString() != "0")
                {
                    lbltodayDPRecCount.InnerHtml = "&nbsp;" + dsDeliveryPending.Tables[0].Rows[0]["TodayCount"].ToString() + "&nbsp;";
                }

                lblDelvPRecCount.InnerHtml = "&nbsp;&nbsp;&nbsp;  [" + dsDeliveryPending.Tables[0].Rows.Count.ToString() + "]";
            }
            return dsDeliveryPending;

        }

        protected void ObdDeliveryPending_buildGridData(DataSet ds)
        {
            gvOBDReceived.DataSource = ds.Tables[0];
            gvOBDReceived.DataBind();
            dsDeliveryPending.Dispose();
        }
        protected void ObdVLPDPending_buildGridData(DataSet ds)
        {
            gvVLPDPending.DataSource = ds.Tables[0];
            gvVLPDPending.DataBind();
            gvVLPDPending.Dispose();
        }

        protected void gvOBDDeliveryPending_OnRowDataBound(object sender, GridViewRowEventArgs e)
        {

            //======================= Added By M.D.Prasad For View Only Condition ======================//
            string[] hh = cp.Roles;
            for (int i = 0; i < hh.Length; i++)
            {
                if (cp.UserTypeID == 3 && Convert.ToInt32(hh[i]) == 4)
                {
                    imgbtngvOBDReceived.Visible = false;
                }
            }
            //======================= Added By M.D.Prasad For View Only Condition ======================//
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (cp.TenantID != 0)
                {
                    gvOBDReceived.Columns[9].Visible = false;
                }
                if ((e.Row.RowState == DataControlRowState.Normal) || (e.Row.RowState == DataControlRowState.Alternate))
                {
                    DataRow row = ((DataRowView)e.Row.DataItem).Row;

                    //String vPriority = ((Literal)e.Row.FindControl("ltPriorityLevel")).Text;
                    String vltltOBDNumber_Pending = ((Literal)e.Row.FindControl("ltOBDNumber_Pending")).Text;



                    Literal ltDocTypeID = (Literal)e.Row.FindControl("ltDocTypeID");
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
                    }

                    //======================= Added By M.D.Prasad For View Only Condition ======================//
                    for (int i = 0; i < hh.Length; i++)
                    {
                        if (cp.UserTypeID == 3 && Convert.ToInt32(hh[i]) == 4)
                        {
                            gvOBDReceived.Columns[8].Visible = false;
                            gvOBDReceived.Columns[9].Visible = false;
                        }
                    }
                    //======================= Added By M.D.Prasad For View Only Condition ======================//



                    /*
                    if (vPriority != "")
                    {
                        String vltPriorityTime = ((Literal)e.Row.FindControl("ltPriorityTime")).Text;

                        StringBuilder StrTimetickerJS = new StringBuilder();

                        if (vltPriorityTime != "")
                        {

                            DateTime dtPriorityDateTime = Convert.ToDateTime(vltPriorityTime);

                            StrTimetickerJS.Append("<script type=\"text/javascript\">");
                            StrTimetickerJS.Append("$(function() {");
                            StrTimetickerJS.Append("var austDay = new Date();");
                            StrTimetickerJS.Append("austDay = new Date(" + dtPriorityDateTime.Year.ToString() + "," + (dtPriorityDateTime.Month - 1).ToString() + "," + dtPriorityDateTime.Day.ToString() + "," + dtPriorityDateTime.Hour.ToString() + "," + dtPriorityDateTime.Minute.ToString() + "," + CommonLogic.RandomNumber(60).ToString() + ");");
                            //StrTimetickerJS.Append("austDay = new Date(" + vltPriorityTime + ");");


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
                                StrTimetickerJS.Append("<img src=\".\\Images\\horse1.gif\" border=\"0\" width=\"25\"/><div id=\"defaultCountdown" + e.Row.RowIndex.ToString() + "\" class=\"timeAlertDiv\"></div>");
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
                    }*/

                }


            }
        }

        protected void gvOBDDeliveryPending_Sorting(object sender, GridViewSortEventArgs e)
        {
            this.ObdDeliveryPending_buildGridData(this.ObdDeliveryPending_buildGridData());
        }

        protected void gvOBDDeliveryPending_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            ViewState["DeliveryPendingIsInsert"] = false;

            gvOBDReceived.PageIndex = e.NewPageIndex;
            gvOBDReceived.EditIndex = -1;
            this.ObdDeliveryPending_buildGridData(this.ObdDeliveryPending_buildGridData());
        }

        protected void lnkDelvpSearch_Click(object sender, EventArgs e)
        {
            string OBDNumber = txtDelvPOBDNumber.Text.Trim();
            string Tenant = txtDPTenant.Text.Split('-')[0];

            if (OBDNumber == "Search Delv. Doc.#...")
                OBDNumber = "";

            if (Tenant == "Search Tenant...")
            {
                Tenant = "";
                hifDPTenantID.Value = "0";
            }

         this.DeliveryPendingSQL += "@OBDNumber=" + DB.SQuote(OBDNumber) + ",@WarehouseIDs=" + DB.SQuote(String.Join(",", cp.Warehouses)) + " ,@Tenant=" + DB.SQuote(Tenant);
         
          
            ViewState["DeliveryPendingSQLString"] = this.DeliveryPendingSQL + ", @AccountID_New = " + cp.AccountID.ToString() + ",@UserTypeID_New = " + cp.UserTypeID.ToString() + ",@TenantID= " + hifDPTenantID.Value + ",@TenantID_New = " + cp.TenantID.ToString();

        //    ViewState["DeliveryPendingSQLString"] = this.DeliveryPendingSQL + "@OBDNumber=" + DB.SQuote(OBDNumber) + ",@WarehouseIDs=" + DB.SQuote(String.Join(",", cp.Warehouses)) + " ,@Tenant=" + DB.SQuote(Tenant);

            ObdDeliveryPending_buildGridData(this.ObdDeliveryPending_buildGridData());

            txtDelvPOBDNumber.Text = "Search Delv. Doc.#...";
            txtDPTenant.Text = "Search Tenant...";

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

        #endregion -------------------------Delivery Pending -----------------------------




        #region -------------------------POD Pending -----------------------------

        protected DataSet ObdDCRPending_buildGridData()
        {
            string sql = ViewState["DCRPendingSQLString"].ToString();
            dsPODPending = DB.GetDS(sql, false);

            if (dsPODPending.Tables[0].Rows.Count == 0)
            {
                lblPODRecCount.InnerText = "";
            }
            else
            {

                if (dsPODPending.Tables[0].Rows[0]["TodayCount"].ToString() != "0")
                {
                    lbltodayPODRecCount.InnerHtml = "&nbsp;" + dsPODPending.Tables[0].Rows[0]["TodayCount"].ToString() + "&nbsp;";
                }

                lblPODRecCount.InnerHtml = "&nbsp;&nbsp;&nbsp;  [" + dsPODPending.Tables[0].Rows.Count.ToString() + "]";
            }


            return dsPODPending;

        }

        protected void ObdDCRPending_buildGridData(DataSet ds)
        {
            gvDCRPending.DataSource = ds;
            gvDCRPending.DataBind();
            dsPODPending.Dispose();

        }


        protected void gvDCRPending_Sorting(object sender, GridViewSortEventArgs e)
        {
            this.ObdDCRPending_buildGridData(this.ObdDCRPending_buildGridData());
        }


        protected void gvDCRPending_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            ViewState["DCRPendingIsInsert"] = false;

            gvDCRPending.PageIndex = e.NewPageIndex;
            gvDCRPending.EditIndex = -1;
            this.ObdDCRPending_buildGridData(this.ObdDCRPending_buildGridData());
        }


        protected void gvDCRPending_OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            //======================= Added By M.D.Prasad For View Only Condition ======================//
            string[] hh = cp.Roles;
            for (int i = 0; i < hh.Length; i++)
            {
                if (cp.UserTypeID == 3 && Convert.ToInt32(hh[i]) == 4)
                {
                    imgbtngvDCRPending.Visible = false;
                }
            }
            //======================= Added By M.D.Prasad For View Only Condition ======================//
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (cp.TenantID != 0)
                {
                    gvDCRPending.Columns[10].Visible = false;
                }

                if ((e.Row.RowState == DataControlRowState.Normal) || (e.Row.RowState == DataControlRowState.Alternate))
                {
                    DataRow row = ((DataRowView)e.Row.DataItem).Row;

                    Literal ltDocTypeID = (Literal)e.Row.FindControl("ltDocTypeID");
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
                    }

                    //======================= Added By M.D.Prasad For View Only Condition ======================//
                    for (int i = 0; i < hh.Length; i++)
                    {
                        if (cp.UserTypeID == 3 && Convert.ToInt32(hh[i]) == 4)
                        {
                            gvDCRPending.Columns[9].Visible = false;
                            gvDCRPending.Columns[10].Visible = false;
                        }
                    }
                    //======================= Added By M.D.Prasad For View Only Condition ======================//

                }
            }

        }

        protected void lnlPODSearch_Click(object sender, EventArgs e)
        {
            string OBDNumber = txtPODDelDocNo.Text.Trim();
            string Tenant = txtPPTenant.Text.Split('-')[0];

            if (OBDNumber == "Search Delv. Doc.#...")
                OBDNumber = "";

            if (Tenant == "Search Tenant...")
            {
                Tenant = "";
                hifPPTenantID.Value = "0";
            }
           this.DCRPendingSQL+= "@OBDNumber=" + DB.SQuote(OBDNumber) + ",@WarehouseIDs=" + DB.SQuote(String.Join(",", cp.Warehouses)) + ",@Tenant=" + DB.SQuote(Tenant);

            ViewState["DCRPendingSQLString"] = this.DCRPendingSQL + ", @AccountID_New = " + cp.AccountID.ToString() + ",@UserTypeID_New = " + cp.UserTypeID.ToString() + ",@TenantID="+ hifPPTenantID.Value + ",@TenantID_New = " + cp.TenantID.ToString();

            //ViewState["DCRPendingSQLString"] = this.DCRPendingSQL + "@OBDNumber=" + DB.SQuote(OBDNumber) + ",@WarehouseIDs=" + DB.SQuote(String.Join(",", cp.Warehouses)) + ",@Tenant=" + DB.SQuote(Tenant);

            ObdDCRPending_buildGridData(this.ObdDCRPending_buildGridData());

            txtPODDelDocNo.Text = "Search Delv. Doc.#...";
            txtPPTenant.Text = "Search Tenant...";
            hifPPTenantID.Value = "0";
        }




        #endregion -------------------------POD Pending -----------------------------


        //protected void imgbtngvOBDReceived_Click(object sender, ImageClickEventArgs e)
            protected void imgbtngvOBDReceived_Click(object sender, EventArgs e)
        {
            cp = HttpContext.Current.User as CustomPrincipal;
            try
            {

                //CommonLogic.PrepareGridViewForExport(gvShipmentResults, HiddenColumns);
                //String ExcelFileName = "InboundSearch";
                //CommonLogic.ExportGridView(gvShipmentResults, ExcelFileName);
                string[] hiddencolumns = { "P.Note", "Change" };
                gvOBDReceived.AllowPaging = false;
                this.ObdDeliveryPending_buildGridData(this.ObdDeliveryPending_buildGridData());
                CommonLogic.ExporttoExcel1(gvOBDReceived, "Deliveries Pending", hiddencolumns);
            }
            catch (Exception ex)
            {
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
                resetError("Error While Exporting Data", true);
            }
        }


        public override void VerifyRenderingInServerForm(Control control)
        {

        }

        //protected void imgbtngvDCRPending_Click(object sender, ImageClickEventArgs e)
            protected void imgbtngvDCRPending_Click(object sender, EventArgs e)
        {
            cp = HttpContext.Current.User as CustomPrincipal;
            try
            {
                string[] hiddencolumns = { "Change" };
                gvDCRPending.AllowPaging = false;
                this.ObdDCRPending_buildGridData(this.ObdDCRPending_buildGridData());
                CommonLogic.ExporttoExcel1(gvDCRPending, "POD Pending ", hiddencolumns);
            }
            catch (Exception ex)
            {
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
                resetError("Error While Exporting Data", true);
            }
        }


        #region begin ---------------Export to Excel ---------------

        //protected void imgbtngvOBDPGIPending_Click(object sender, ImageClickEventArgs e)
            protected void imgbtngvOBDPGIPending_Click(object sender, EventArgs e)
        {
            cp = HttpContext.Current.User as CustomPrincipal;
            try
            {
                String[] hiddencolumns = { "P.Note", "Change" };
                gvOBDPGIPending.AllowPaging = false;
                this.ObdPGIPending_buildGridData(this.ObdPGIPending_buildGridData());
                CommonLogic.ExporttoExcel1(gvOBDPGIPending, "PGI Pending", hiddencolumns);
            }
            catch (Exception ex)
            {
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
                resetError("Error While Exporting Data", true);
            }
        }

        //protected void imgbtngvOBDInProcess_Click(object sender, ImageClickEventArgs e)
        protected void imgbtngvOBDInProcess_Click(object sender, EventArgs e)
        {
            cp = HttpContext.Current.User as CustomPrincipal;
            try
            {
                String[] hiddencolumns = { "P.Note", "Change" };
                gvOBDInProcess.AllowPaging = false;
                ObdPending_buildGridData(OBDPending_buildGridData());
                CommonLogic.ExporttoExcel1(gvOBDInProcess, "Deliveries In Process", hiddencolumns);
            }
            catch (Exception ex)
            {
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
                resetError("Error While Exporting Data", true);
            }

        }


        protected void imgbtngvOBDInVLPD_Click(object sender, EventArgs e)
        {
            cp = HttpContext.Current.User as CustomPrincipal;
            try
            {
                String[] hiddencolumns = { "P.Note","Change" };
                gvVLPDPending.AllowPaging = false;
                gvVLPDPending.Columns.RemoveAt(0);
                ObdVLPDPending_buildGridData(this.ObdVLPDPending_buildGridData());
                CommonLogic.ExporttoExcel1(gvVLPDPending, "VLPD Pending", hiddencolumns);
            }
            catch (Exception ex)
            {
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
                resetError("Error While Exporting Data", true);
            }

        }


        protected void imgPlantDeliveryNote_Click(object sender, ImageClickEventArgs e)
        {
            cp = HttpContext.Current.User as CustomPrincipal;
            try
            {
                String[] hiddencolumns = { "", "" };
                gvOBDInProcess.AllowPaging = false;
                ObdPending_buildGridData(OBDPending_buildGridData());
                CommonLogic.ExporttoExcel1(gvOBDInProcess, "Deliveries In Process", hiddencolumns);
            }
            catch (Exception ex)
            {
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
                resetError("Error While Exporting Data", true);
            }
        }















        #endregion End ---------------Export to Excel ---------------

        #region -------------------------Delivery's In Process for Plant -----------------------------
        /*
       protected DataSet OBDPlant_buildGridData()
       {
           string sql = ViewState["PlantDeliverySQLString"].ToString();

           DataSet ds = DB.GetDS(sql, false);

           if (ds.Tables[0].Rows.Count == 0)
           {
               plntDeliveryNoteListCount.InnerText = "";
           }
           else
           {
               plntDeliveryNoteListCount.InnerHtml = "&nbsp;&nbsp;&nbsp;  [" + ds.Tables[0].Rows.Count.ToString() + "]";
           }

           return ds;

       }

       protected void OBDPlant_buildGridData(DataSet ds)
       {
           gvPlantDeliveryNote.DataSource = ds;
           gvPlantDeliveryNote.DataBind();
           ds.Dispose();
       }

       protected void OBDPlant_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
       {
           this.OBDPlant_buildGridData(this.OBDPlant_buildGridData());
       }

       protected void OBDPlant_RowDataBound(object sender, GridViewRowEventArgs e)
       {
           if (e.Row.RowType == DataControlRowType.DataRow)
           {

               if ((e.Row.RowState == DataControlRowState.Normal) || (e.Row.RowState == DataControlRowState.Alternate))
               {
                   String vPriority = ((Literal)e.Row.FindControl("ltPriorityLevel")).Text;
                   String vltPriorityTime = ((Literal)e.Row.FindControl("ltPriorityTime")).Text;


                   Literal ltDocTypeID = (Literal)e.Row.FindControl("ltDocTypeID");
                   Label lblOBDID = (Label)e.Row.FindControl("lblOBDID");
                   Label lblLineItemCount = (Label)e.Row.FindControl("lblLineItemCount");

                   HyperLink link = (HyperLink)e.Row.FindControl("lnkHyperLink");


                   if (ltDocTypeID.Text != "6" && ltDocTypeID.Text != "9")
                   {
                       link.NavigateUrl = "DeliveryPickNote.aspx?obdid=" + lblOBDID.Text + "&lineitemcount=" + lblLineItemCount.Text;
                   }
                   else
                   {
                       link.NavigateUrl = "~/mManufacturingProcess/DeliveryPickNote.aspx?obdid=" + lblOBDID.Text + "&lineitemcount=" + lblLineItemCount.Text;
                   }

                   /*
                   if (vPriority != "")
                   {
                       StringBuilder StrTimetickerJS = new StringBuilder();

                       if (vltPriorityTime != "")
                       {

                           DateTime dtPriorityDateTime = Convert.ToDateTime(vltPriorityTime);

                           StrTimetickerJS.Append("<script type=\"text/javascript\">");
                           StrTimetickerJS.Append("$(function() {");
                           StrTimetickerJS.Append("var austDay = new Date();");
                           StrTimetickerJS.Append("austDay = new Date(" + dtPriorityDateTime.Year.ToString() + "," + (dtPriorityDateTime.Month - 1).ToString() + "," + dtPriorityDateTime.Day.ToString() + "," + dtPriorityDateTime.Hour.ToString() + "," + dtPriorityDateTime.Minute.ToString() + "," + CommonLogic.RandomNumber(60).ToString() + ");");
                           //StrTimetickerJS.Append("austDay = new Date(" + vltPriorityTime + ");");
                           //StrTimetickerJS.Append("$('#CDOBDInProcess" + e.Row.RowIndex.ToString() + "').countdown({ until: austDay, compact: true, format: 'dHMS', description: ''});");

                           if (dtPriorityDateTime > DateTime.Now)
                           {
                               StrTimetickerJS.Append("$('#CDOBDInProcess" + e.Row.RowIndex.ToString() + "').countdown({ until: austDay, compact: true, format: 'dHMS', description: ''});");
                           }
                           else
                           {
                               StrTimetickerJS.Append("$('#CDOBDInProcess" + e.Row.RowIndex.ToString() + "').countdown({ since: austDay, compact: true, format: 'dHMS', description: ''});");
                           }

                           StrTimetickerJS.Append("});</script>");

                           //StrTimetickerJS.Append("<img src=\"images\\horse1.gif\" border=\"0\" width=\"25\"/> <div id=\"CDOBDInProcess" + e.Row.RowIndex.ToString() + "\" class=\"timeAlertDiv\"></div>");
                           //e.Row.Cells[0].Text = StrTimetickerJS.ToString();

                           if (dtPriorityDateTime > DateTime.Now)
                           {
                               StrTimetickerJS.Append("<img src=\"..\\images\\horse1.gif\" border=\"0\" width=\"25\"/><div id=\"CDOBDInProcess" + e.Row.RowIndex.ToString() + "\" class=\"timeAlertDiv\"></div>");
                               e.Row.Cells[0].Text = StrTimetickerJS.ToString();
                           }
                           else
                           {
                               StrTimetickerJS.Append("<div id=\"CDOBDInProcess" + e.Row.RowIndex.ToString() + "\" class=\"timeAlertDiv\"></div>");
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



               }

           }
       }

       protected void OBDPlant_Sorting(object sender, GridViewSortEventArgs e)
       {
           this.OBDPlant_buildGridData(this.OBDPlant_buildGridData());
       }

       protected void OBDPlant_PageIndexChanging(object sender, GridViewPageEventArgs e)
       {
           ViewState["PendingIsInsert"] = false;

           gvPlantDeliveryNote.PageIndex = e.NewPageIndex;
           gvPlantDeliveryNote.EditIndex = -1;
           this.OBDPlant_buildGridData(this.OBDPlant_buildGridData());
       }

       protected void OBDPlant_RowCommand(object sender, GridViewCommandEventArgs e)
       {

       }

       protected void lnkPlantDeliveryNote_Click(object sender, EventArgs e)
       {

           if (txtPlantDeliveryNote.Text == "Search Delv. Doc.#...")
               txtPlantDeliveryNote.Text = "";

           if (txtPlantDeliveryNote.Text != "")
           {
               ViewState["PlantDeliverySQLString"] = this.PlantDeliveryNoteSQL + "@OBDNumber=" + DB.SQuote(txtPlantDeliveryNote.Text) + ",@WarehouseIDs=" + DB.SQuote(String.Join(",", cp.Warehouses));
           }
           else
           {
               ViewState["PlantDeliverySQLString"] = this.PlantDeliveryNoteSQL + "@OBDNumber=NULL,@WarehouseIDs=" + DB.SQuote(String.Join(",", cp.Warehouses));
           }

           this.OBDPlant_buildGridData(this.OBDPlant_buildGridData());
           txtPlantDeliveryNote.Text = "Search Delv. Doc.#...";

       }

 */


        #endregion -------------------------Delivery's In Process for Plant-----------------------------

        protected void gvVLPDPending_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //======================= Added By M.D.Prasad For View Only Condition ======================//
            cp = HttpContext.Current.User as CustomPrincipal;
            string[] hh = cp.Roles;
            for (int i = 0; i < hh.Length; i++)
            {
                if (cp.UserTypeID == 3 && Convert.ToInt32(hh[i]) == 4)
                {
                    LinkButton2.Visible = false;
                }
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRow row = ((DataRowView)e.Row.DataItem).Row;

                Literal ltDocTypeID = (Literal)e.Row.FindControl("ltDocTypeID");
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
                }

                if ((e.Row.RowState == DataControlRowState.Normal) || (e.Row.RowState == DataControlRowState.Alternate))
                {                   
                    for (int i = 0; i < hh.Length; i++)
                    {
                        if (cp.UserTypeID == 3 && Convert.ToInt32(hh[i]) == 4)
                        {
                            gvVLPDPending.Columns[8].Visible = false;
                            gvVLPDPending.Columns[9].Visible = false;                            
                        }
                    }                    
                }
            }
            //======================= Added By M.D.Prasad For View Only Condition ======================//
        }
    }
}