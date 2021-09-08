using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Text;
using MRLWMSC21Common;


// Module Name : InboundTracking Under Inbound
// DevelopedBy : Naresh P
// CreatedOn   : 27/11/2013
// Modified On : 24/03/2015


namespace MRLWMSC21.mInbound
{
    public partial class InboundTracking : System.Web.UI.Page
    {
        public CustomPrincipal cp = HttpContext.Current.User as CustomPrincipal;
        string TenantRootDir = "";
        string InboundPath = "";
        string InbShpDelNotePath = "";
        string InbShpVerifNotePath = "";
        string InbDescPath = "";
        DataSet dsGRNPending;
        DataSet dsShipmentExpected;
        String ShipmentPendingSQL = "EXEC  [dbo].[sp_INB_GetSITList]   ";
        String ShipmentInProcessSQL = "EXEC [dbo].[sp_INB_GetSIPList]  ";
        String selectGRNPendingSQL = "EXEC [dbo].[sp_INB_GetGRNPendingList]   ";
        String DiscrepancySQL = "EXEC [dbo].[sp_INB_GetDiscepancyShipmentsList]   ";
        String BillsPendingSQL = "EXEC [dbo].[sp_INB_GetBillsPendingList]  ";
        String ShipmentExpectedSQL = "EXEC [dbo].[sp_INB_GetShipmentExpectedList]   ";

        protected void Page_PreInit(object sender, EventArgs e)
        {

            Page.Theme = "Inbound";

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            cp = HttpContext.Current.User as CustomPrincipal;
            if (!cp.IsInAnyRoles(CommonLogic.GetRolesAllowed(CommonLogic.GetRolesAllowedForthisPage("Inbound Tracking"))))
            {
                Response.Redirect("../Login.aspx?eid=6");
            }

            DesignLogic.SetInnerPageSubHeading(this.Page, "Inbound Tracking");

            string query = "EXEC [dbo].[sp_TPL_GetTenantDirectoryInfo] @TypeID=1";

            DataSet dsPath = DB.GetDS(query.ToString(), false);

            TenantRootDir = dsPath.Tables[0].Rows[4][0].ToString();
            InboundPath = dsPath.Tables[0].Rows[0][0].ToString();
            InbShpDelNotePath = dsPath.Tables[0].Rows[2][0].ToString();
            InbShpVerifNotePath = dsPath.Tables[0].Rows[3][0].ToString();
            InbDescPath = dsPath.Tables[0].Rows[1][0].ToString();

            if (!IsPostBack)
            {

                txtSITTenant.Enabled = CommonLogic.CheckSuperAdmin(txtSITTenant, cp, hifSITTenantID);
                txtSIPTenant.Enabled = CommonLogic.CheckSuperAdmin(txtSIPTenant, cp, hifSIPTenant);
                txtSIETenant.Enabled = CommonLogic.CheckSuperAdmin(txtSIETenant, cp, hifSIETenant);
                txtGRNTenant.Enabled = CommonLogic.CheckSuperAdmin(txtGRNTenant, cp, hifGRNTenant);
                txtDiscTenant.Enabled = CommonLogic.CheckSuperAdmin(txtDiscTenant, cp, hifDiscTenant);

                if (cp.TenantID != 0)
                {
                    gvShipmentPending.Columns[3].Visible = false;
                    gvShipmentExpected.Columns[4].Visible = false;
                    gvShipInProcess.Columns[5].Visible = false;
                    gvGRNPending.Columns[5].Visible = false;
                    gvDiscrepancy.Columns[4].Visible = false;
                }

                //Shipments in Transit
                //ViewState["ShipmentPendingSQLString"] = this.ShipmentPendingSQL + " @WarehouseIDs= " + DB.SQuote(String.Join(",", cp.Warehouses)) + ",@StoreRefNo=NULL";

                ViewState["ShipmentPendingSQLString"] = this.ShipmentPendingSQL + " @WarehouseIDs= " + DB.SQuote(String.Join(",", cp.Warehouses)) + ",@StoreRefNo=NULL" + ",@Tenant=NULL ,@TenantID=" + cp.TenantID + ",@AccountID_New=" + cp.AccountID.ToString() ;
                this.ShipmentPending_buildGridData(this.ShipmentPending_buildGridData());

                //Shipments In Process
                //ViewState["ShipInProcessSQLString"] = this.ShipmentInProcessSQL + " @WarehouseIDs= " + DB.SQuote(String.Join(",", cp.Warehouses)) + ",@StoreRefNo=NULL";

                ViewState["ShipInProcessSQLString"] = this.ShipmentInProcessSQL + " @WarehouseIDs= " + DB.SQuote(String.Join(",", cp.Warehouses)) + ",@StoreRefNo=NULL" + ",@Tenant=NULL ,@TenantID=" + cp.TenantID + ",@AccountID_New=" + cp.AccountID.ToString();
                this.ShipInProcess_buildGridData(this.ShipInProcess_buildGridData());

                //GRN Pending
                //ViewState["GRNPendingSQLString"] = this.selectGRNPendingSQL + " @WarehouseIDs= " + DB.SQuote(String.Join(",", cp.Warehouses)) + ",@StoreRefNo=NULL";

                ViewState["GRNPendingSQLString"] = this.selectGRNPendingSQL + " @WarehouseIDs= " + DB.SQuote(String.Join(",", cp.Warehouses)) + ",@StoreRefNo=NULL" + ",@Tenant=NULL ,@TenantID=" + cp.TenantID + ",@AccountID_New=" + cp.AccountID.ToString();
                this.GRNPending_buildGridData(this.GRNPending_buildGridData());

                //Discrepancy
                //ViewState["DiscrepancySQLString"] = this.DiscrepancySQL + " @WarehouseIDs= " + DB.SQuote(String.Join(",", cp.Warehouses)) + ",@StoreRefNo=NULL";

                ViewState["DiscrepancySQLString"] = this.DiscrepancySQL + " @WarehouseIDs= " + DB.SQuote(String.Join(",", cp.Warehouses)) + ",@StoreRefNo=NULL" + ",@Tenant=NULL ,@TenantID=" + cp.TenantID + ",@AccountID_New =" + cp.AccountID.ToString();
                this.Discrepancy_buildGridData(this.Discrepancy_buildGridData());

                //ShipmentExpected
                //ViewState["ShipmentExpectedSQLString"] = this.ShipmentExpectedSQL + " @WarehouseIDs= " + DB.SQuote(String.Join(",", cp.Warehouses)) + ",@StoreRefNo=NULL";

                ViewState["ShipmentExpectedSQLString"] = this.ShipmentExpectedSQL + " @WarehouseIDs= " + DB.SQuote(String.Join(",", cp.Warehouses)) + ",@StoreRefNo=NULL" + ",@Tenant=NULL ,@TenantID=" + cp.TenantID + ",@AccountID_New=" + cp.AccountID.ToString();
                this.ShipmentExpected_buildGridData(this.ShipmentExpected_buildGridData());

                
            }
        }


        #region -------------------------Shipment Pending -----------------------------

        protected DataSet ShipmentPending_buildGridData()
        {
            string sql = ViewState["ShipmentPendingSQLString"].ToString();

            DataSet ds = DB.GetDS(sql, false);

            if (ds.Tables[0].Rows.Count == 0)
            {
                lblSITRecordCount.InnerText = "";
            }
            else
            {
                if (ds.Tables[0].Rows[0]["TodayCount"].ToString() != "0")
                {
                    lbltodaySITRecCount.InnerHtml = "&nbsp;" + ds.Tables[0].Rows[0]["TodayCount"].ToString() + "&nbsp;";


                }

                lblSITRecordCount.InnerHtml = " &nbsp;&nbsp;&nbsp;  [" + ds.Tables[0].Rows.Count.ToString() + "]";
            }
            return ds;

        }

        protected void ShipmentPending_buildGridData(DataSet ds)
        {
            gvShipmentPending.DataSource = ds;
            gvShipmentPending.DataBind();
            ds.Dispose();
        }

        protected void gvShipmentPending_Sorting(object sender, GridViewSortEventArgs e)
        {
            this.ShipmentPending_buildGridData(this.ShipmentPending_buildGridData());
        }

        protected void gvShipmentPending_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            //======================= Added By M.D.Prasad For View Only Condition ======================//
            cp = HttpContext.Current.User as CustomPrincipal;
            string[] hh = cp.Roles;
            for (int i = 0; i < hh.Length; i++)
            {
                if (cp.UserTypeID == 3 && Convert.ToInt32(hh[i]) == 4)
                {
                    imgbtngvShipmentPending.Visible = false;
                }
            }

            if ((e.Row.RowType == DataControlRowType.DataRow) && (e.Row.RowState == DataControlRowState.Normal) || (e.Row.RowState == DataControlRowState.Alternate))
            {
                for (int i = 0; i < hh.Length; i++)
                {
                    if (cp.UserTypeID == 3 && Convert.ToInt32(hh[i]) == 4)
                    {
                        gvShipmentPending.Columns[7].Visible = false;
                        gvShipmentPending.Columns[8].Visible = false;
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
                            //StrTimetickerJS.Append("$('#ShipmentPendingCD" + e.Row.RowIndex.ToString() + "').countdown({ until: austDay, compact: true, format: 'dHMS', description: ''});");

                            if (dtPriorityDateTime > DateTime.Now)
                            {
                                StrTimetickerJS.Append("$('#ShipmentPendingCD" + e.Row.RowIndex.ToString() + "').countdown({ until: austDay, compact: true, format: 'dHMS', description: ''});");
                            }
                            else
                            {
                                StrTimetickerJS.Append("$('#ShipmentPendingCD" + e.Row.RowIndex.ToString() + "').countdown({ since: austDay, compact: true, format: 'dHMS', description: ''});");
                            }

                            StrTimetickerJS.Append("});</script>");

                            //StrTimetickerJS.Append("<img src=\"images\\horse1.gif\" border=\"0\" width=\"25\"/> <div id=\"ShipmentPendingCD" + e.Row.RowIndex.ToString() + "\" class=\"timeAlertDiv\"></div>");
                            //e.Row.Cells[0].Text = StrTimetickerJS.ToString();

                            if (dtPriorityDateTime > DateTime.Now)
                            {
                                StrTimetickerJS.Append("<img src=\"..\\Images\\horse1.gif\" border=\"0\" width=\"25\"/><div id=\"ShipmentPendingCD" + e.Row.RowIndex.ToString() + "\" class=\"timeAlertDiv\"></div>");
                                e.Row.Cells[0].Text = StrTimetickerJS.ToString();
                            }
                            else
                            {
                                StrTimetickerJS.Append("<div id=\"ShipmentPendingCD" + e.Row.RowIndex.ToString() + "\" class=\"timeAlertDiv\"></div>");
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

            }*/



            if (e.Row.RowType == DataControlRowType.DataRow && ((e.Row.RowState == DataControlRowState.Normal) || (e.Row.RowState == DataControlRowState.Alternate)))
            {
                if (cp.TenantID != 0)
                {
                    gvShipmentPending.Columns[8].Visible = false;
                }
            }
        }

        protected void gvShipmentPending_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            ViewState["ShipmentPendingIsInsert"] = false;
            //this.resetErrorSIT("", false);
            gvShipmentPending.PageIndex = e.NewPageIndex;
            gvShipmentPending.EditIndex = -1;
            this.ShipmentPending_buildGridData(this.ShipmentPending_buildGridData());
        }

        protected void resetErrorSIT(string error, bool isError)
        {

            //string str = "<font class=\"noticeMsg\">NOTICE:</font>&nbsp;&nbsp;&nbsp;";
            //if (isError)
            //    str = "<font class=\"errorMsg\">ERROR:</font>&nbsp;&nbsp;&nbsp;";

            //if (error.Length > 0)
            //    str += error + "";
            //else
            //    str = "";
            ////Literal ss = (Literal)this.Form.FindControl("ltError");

            //lblSITStatus.Text = str;

            ScriptManager.RegisterStartupScript(this, this.GetType(), "test", "showStickyToast(" + (isError.ToString().ToLower() == "true" ? "false" : "true") + ",\"" + error + "\");", true);
        }

        public string GetStoreRefNoWithLink(String StoreRefNo, String TenantID)
        {
            //String sFileName = CommonLogic._GetAttatchmentFile(TenantRootDir + DB.GetSqlS("select UniqueID AS S from GEN_Tenant where TenantID=" + cp.TenantID) + InboundPath + InbShpDelNotePath, StoreRefNo);

            String sFileName = CommonLogic._GetAttatchmentFile(TenantRootDir +  TenantID + InboundPath + InbShpDelNotePath, StoreRefNo);

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

        protected void lnkSITSearch_Click(object sender, EventArgs e)
        {
            cp = HttpContext.Current.User as CustomPrincipal;
            string StoreRefNo = txtSITStoreRefNo.Text.Trim();
            string Tenant = txtSITTenant.Text.Split('-')[0];
            string TenantID = hifSITTenantID.Value.ToString();
            if (StoreRefNo == "Search Store Ref.# ...")
                StoreRefNo = "";

            if (Tenant == "Search Tenant...")
                Tenant = "";
        
            ViewState["ShipmentPendingSQLString"] = this.ShipmentPendingSQL + " @WarehouseIDs= " + DB.SQuote(String.Join(",", cp.Warehouses)) + ",@StoreRefNo=" + DB.SQuote(StoreRefNo) + ",@Tenant=" + DB.SQuote(Tenant)+",@TenantID= " + DB.SQuote(TenantID)+ " , @AccountID_New = "+cp.AccountID;

            this.ShipmentPending_buildGridData(this.ShipmentPending_buildGridData());

            txtSITStoreRefNo.Text = "Search Store Ref.# ...";

            

           //  inboundtracking.Scripts.Add("javascript:HideSITLoader()");

        }

        #endregion -------------------------Shipment Pending -----------------------------

        #region ------------------- Shipments In Process  -----------------------------

        protected DataSet ShipInProcess_buildGridData()
        {
            string sql = ViewState["ShipInProcessSQLString"].ToString();

            DataSet ds = DB.GetDS(sql, false);

            if (ds.Tables[0].Rows.Count == 0)
            {
                lblSIPRecordCount.InnerText = "";
            }
            else
            {
                if (ds.Tables[0].Rows[0]["TodayCount"].ToString() != "0")
                {
                    lbltodaySIPRecCount.InnerHtml = "&nbsp;" + ds.Tables[0].Rows[0]["TodayCount"].ToString() + "&nbsp;";


                }


                lblSIPRecordCount.InnerHtml = " &nbsp;&nbsp;&nbsp;  [" + ds.Tables[0].Rows.Count.ToString() + "]";
            }


            return ds;

        }

        protected void ShipInProcess_buildGridData(DataSet ds)
        {
            gvShipInProcess.DataSource = ds;
            gvShipInProcess.DataBind();
            ds.Dispose();
        }

        protected void gvShipInProcess_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //======================= Added By M.D.Prasad For View Only Condition ======================//
            cp = HttpContext.Current.User as CustomPrincipal;
            string[] hh = cp.Roles;
            for (int i = 0; i < hh.Length; i++)
            {
                if (cp.UserTypeID == 3 && Convert.ToInt32(hh[i]) == 4)
                {
                    btngvShipInProcess.Visible = false;
                }
            }
            
            if ((e.Row.RowType == DataControlRowType.DataRow) && (e.Row.RowState == DataControlRowState.Normal) || (e.Row.RowState == DataControlRowState.Alternate))
            {
                for (int i = 0; i < hh.Length; i++)
                {
                    if (cp.UserTypeID == 3 && Convert.ToInt32(hh[i]) == 4)
                    {
                        gvShipInProcess.Columns[9].Visible = false;
                        gvShipInProcess.Columns[10].Visible = false;
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

                    if (vPriority != "")
                    {
                        String vltPriorityTime = ((Literal)e.Row.Cells[0].FindControl("ltPriorityTime")).Text;

                        StringBuilder StrTimetickerJS = new StringBuilder();

                        if (vltPriorityTime != "")
                        {

                            DateTime dtPriorityDateTime = Convert.ToDateTime(vltPriorityTime);

                            StrTimetickerJS.Append("<script type=\"text/javascript\">");
                            StrTimetickerJS.Append("$(function() {");
                            StrTimetickerJS.Append("var austDay = new Date();");
                            StrTimetickerJS.Append("austDay = new Date(" + dtPriorityDateTime.Year.ToString() + "," + (dtPriorityDateTime.Month - 1).ToString() + "," + dtPriorityDateTime.Day.ToString() + "," + dtPriorityDateTime.Hour.ToString() + "," + dtPriorityDateTime.Minute.ToString() + "," + CommonLogic.RandomNumber(60).ToString() + ");");
                            //StrTimetickerJS.Append("austDay = new Date(" + vltPriorityTime + ");");
                            //StrTimetickerJS.Append("$('#ShipInProcessCD" + e.Row.RowIndex.ToString() + "').countdown({ until: austDay, compact: true, format: 'dHMS', description: ''});");

                            if (dtPriorityDateTime > DateTime.Now)
                            {
                                StrTimetickerJS.Append("$('#ShipInProcessCD" + e.Row.RowIndex.ToString() + "').countdown({ until: austDay, compact: true, format: 'dHMS', description: ''});");
                            }
                            else
                            {
                                StrTimetickerJS.Append("$('#ShipInProcessCD" + e.Row.RowIndex.ToString() + "').countdown({ since: austDay, compact: true, format: 'dHMS', description: ''});");
                            }

                            StrTimetickerJS.Append("});</script>");

                            // StrTimetickerJS.Append("<img src=\"images\\horse1.gif\" border=\"0\" width=\"25\"/> <div id=\"ShipInProcessCD" + e.Row.RowIndex.ToString() + "\" class=\"timeAlertDiv\"></div>");
                            //e.Row.Cells[0].Text = StrTimetickerJS.ToString();

                            if (dtPriorityDateTime > DateTime.Now)
                            {
                                StrTimetickerJS.Append("<img src=\"..\\Images\\horse1.gif\" border=\"0\" width=\"25\"/><div id=\"ShipInProcessCD" + e.Row.RowIndex.ToString() + "\" class=\"timeAlertDiv\"></div>");
                                e.Row.Cells[0].Text = StrTimetickerJS.ToString();
                            }
                            else
                            {
                                StrTimetickerJS.Append("<div id=\"ShipInProcessCD" + e.Row.RowIndex.ToString() + "\" class=\"timeAlertDiv\"></div>");
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

            }*/

            if (e.Row.RowType == DataControlRowType.DataRow && ((e.Row.RowState == DataControlRowState.Normal) || (e.Row.RowState == DataControlRowState.Alternate)))
            {
                if (cp.TenantID != 0)
                {
                    gvShipInProcess.Columns[10].Visible = false;
                }
            }
        }

        protected void gvShipInProcess_Sorting(object sender, GridViewSortEventArgs e)
        {
            this.ShipInProcess_buildGridData(this.ShipInProcess_buildGridData());
        }

        protected void gvShipInProcess_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            ViewState["ShipInProcessIsInsert"] = false;

            gvShipInProcess.PageIndex = e.NewPageIndex;
            gvShipInProcess.EditIndex = -1;
            this.ShipInProcess_buildGridData(this.ShipInProcess_buildGridData());
        }

        protected void gvShipInProcess_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }

        protected void lnkSIpSearch_Click(object sender, EventArgs e)
        {
            string StoreRefNo = txtSIPStoreRefNo.Text.Trim();
            string Tenant = txtSIPTenant.Text.Split('-')[0];
            string TenantID = hifSIPTenant.Value.ToString();
            if (StoreRefNo == "Search Store Ref.# ...")
                StoreRefNo = "";

            if (Tenant == "Search Tenant...")
                Tenant = "";

            ViewState["ShipInProcessSQLString"] = this.ShipmentInProcessSQL + " @WarehouseIDs= " + DB.SQuote(String.Join(",", cp.Warehouses)) + ",@StoreRefNo=" + DB.SQuote(StoreRefNo) + ",@Tenant=" + DB.SQuote(Tenant)+",@TenantID= "+ DB.SQuote(TenantID) + " , @AccountID_New = " + cp.AccountID;

            this.ShipInProcess_buildGridData(this.ShipInProcess_buildGridData());

            txtSIPStoreRefNo.Text = "Search Store Ref.# ...";
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

        #endregion ------------------- Shipments In Process  ---------------------

        #region ------------------- GRN Pending  ----------------------

        protected DataSet GRNPending_buildGridData()
        {
            string sql = ViewState["GRNPendingSQLString"].ToString();

            dsGRNPending = DB.GetDS(sql, false);

            if (dsGRNPending.Tables[0].Rows.Count == 0)
            {
                lblGRNRecCount.InnerText = "";
            }
            else
            {
                if (dsGRNPending.Tables[0].Rows[0]["TodayCount"].ToString() != "0")
                {
                    lbltodayGRNRecCount.InnerHtml = "&nbsp;" + dsGRNPending.Tables[0].Rows[0]["TodayCount"].ToString() + "&nbsp;";


                }

                lblGRNRecCount.InnerHtml = " &nbsp;&nbsp;&nbsp;  [" + dsGRNPending.Tables[0].Rows.Count.ToString() + "]";
            }

            return dsGRNPending;

        }

        protected void GRNPending_buildGridData(DataSet ds)
        {
            gvGRNPending.DataSource = dsGRNPending.Tables[0];
            gvGRNPending.DataBind();
            ds.Dispose();

        }

        protected void gvGRNPending_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            cp = HttpContext.Current.User as CustomPrincipal;

            /*
           
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                if ((e.Row.RowState == DataControlRowState.Normal) || (e.Row.RowState == DataControlRowState.Alternate))
                {
                    String vPriority = ((Literal)e.Row.Cells[0].FindControl("ltPriorityLevel")).Text;
                    String vltStoreRefNo = ((Literal)e.Row.Cells[1].FindControl("ltStoreRefNo")).Text;
                   // String vDueDays = ((Literal)e.Row.Cells[1].FindControl("lthidDueDays")).Text;

                    if (vPriority != "")
                    {
                        String vltPriorityTime = ((Literal)e.Row.Cells[0].FindControl("ltPriorityTime")).Text;

                        StringBuilder StrTimetickerJS = new StringBuilder();

                        if (vltPriorityTime != "")
                        {

                            DateTime dtPriorityDateTime = Convert.ToDateTime(vltPriorityTime);

                            StrTimetickerJS.Append("<script type=\"text/javascript\">");
                            StrTimetickerJS.Append("$(function() {");
                            StrTimetickerJS.Append("var austDay = new Date();");
                            StrTimetickerJS.Append("austDay = new Date(" + dtPriorityDateTime.Year.ToString() + "," + (dtPriorityDateTime.Month - 1).ToString() + "," + dtPriorityDateTime.Day.ToString() + "," + dtPriorityDateTime.Hour.ToString() + "," + dtPriorityDateTime.Minute.ToString() + "," + CommonLogic.RandomNumber(60).ToString() + ");");
                            //StrTimetickerJS.Append("austDay = new Date(" + vltPriorityTime + ");");
                            //StrTimetickerJS.Append("$('#GRNPendingCD" + e.Row.RowIndex.ToString() + "').countdown({ until: austDay, compact: true, format: 'dHMS', description: ''});");

                            if (dtPriorityDateTime > DateTime.Now)
                            {
                                StrTimetickerJS.Append("$('#GRNPendingCD" + e.Row.RowIndex.ToString() + "').countdown({ until: austDay, compact: true, format: 'dHMS', description: ''});");
                            }
                            else
                            {
                                StrTimetickerJS.Append("$('#GRNPendingCD" + e.Row.RowIndex.ToString() + "').countdown({ since: austDay, compact: true, format: 'dHMS', description: ''});");
                            }

                            StrTimetickerJS.Append("});</script>");

                            // StrTimetickerJS.Append("<img src=\"images\\horse1.gif\" border=\"0\" width=\"25\"/> <div id=\"GRNPendingCD" + e.Row.RowIndex.ToString() + "\" class=\"timeAlertDiv\"></div>");
                            //e.Row.Cells[0].Text = StrTimetickerJS.ToString();

                            if (dtPriorityDateTime > DateTime.Now)
                            {
                                StrTimetickerJS.Append("<img src=\"..\\images\\horse1.gif\" border=\"0\" width=\"25\"/><div id=\"GRNPendingCD" + e.Row.RowIndex.ToString() + "\" class=\"timeAlertDiv\"></div>");
                                e.Row.Cells[0].Text = StrTimetickerJS.ToString();
                            }
                            else
                            {
                                StrTimetickerJS.Append("<div id=\"GRNPendingCD" + e.Row.RowIndex.ToString() + "\" class=\"timeAlertDiv\"></div>");
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

            }*/

            if (e.Row.RowType == DataControlRowType.DataRow && ((e.Row.RowState == DataControlRowState.Normal) || (e.Row.RowState == DataControlRowState.Alternate)))
            {
                if (cp.TenantID != 0)
                {
                    gvGRNPending.Columns[10].Visible = false;
                }
            }
        }

        protected void gvGRNPending_Sorting(object sender, GridViewSortEventArgs e)
        {
            this.GRNPending_buildGridData(this.GRNPending_buildGridData());
        }

        protected void gvGRNPending_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            ViewState["GRNPendingIsInsert"] = false;

            gvGRNPending.PageIndex = e.NewPageIndex;
            gvGRNPending.EditIndex = -1;
            this.GRNPending_buildGridData(this.GRNPending_buildGridData());
        }

        protected void lnkGRNSearch_Click(object sender, EventArgs e)
        {
            string StoreRefNo = txtGRNStoreRefNumber.Text.Trim();
            string Tenant = txtGRNTenant.Text.Trim();

            if (StoreRefNo == "Search Store Ref.# ...")
                StoreRefNo = "";

            if (Tenant == "Search Tenant...")
                Tenant = "";

            ViewState["GRNPendingSQLString"] = this.selectGRNPendingSQL + " @WarehouseIDs= " + DB.SQuote(String.Join(",", cp.Warehouses)) + ",@StoreRefNo=" + DB.SQuote(StoreRefNo) + ",@Tenant=" + DB.SQuote(Tenant);
            
            this.GRNPending_buildGridData(this.GRNPending_buildGridData());

            txtGRNStoreRefNumber.Text = "Search Store Ref.# ...";
        }

        #endregion ------------------- GRN Pending  ----------------------

        #region ------------------- Discripency Shipments  ----------------------

        protected DataSet Discrepancy_buildGridData()
        {
            string sql = ViewState["DiscrepancySQLString"].ToString();

            DataSet ds = DB.GetDS(sql, false);

            if (ds.Tables[0].Rows.Count == 0)
            {
                lblDisRecCount.InnerText = "";
            }
            else
            {
                if (ds.Tables[0].Rows[0]["TodayCount"].ToString() != "0")
                {
                    lbltodayDISRecCount.InnerHtml = "&nbsp;" + ds.Tables[0].Rows[0]["TodayCount"].ToString() + "&nbsp;";


                }

                lblDisRecCount.InnerHtml = " &nbsp;&nbsp;&nbsp;  [" + ds.Tables[0].Rows.Count.ToString() + "]";
            }


            return ds;

        }

        protected void Discrepancy_buildGridData(DataSet ds)
        {
            gvDiscrepancy.DataSource = ds;
            gvDiscrepancy.DataBind();
            ds.Dispose();
        }

        protected void gvDiscrepancy_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            cp = HttpContext.Current.User as CustomPrincipal;
            if (e.Row.RowType == DataControlRowType.DataRow && ((e.Row.RowState == DataControlRowState.Normal) || (e.Row.RowState == DataControlRowState.Alternate)))
            {
                if (cp.TenantID != 0)
                {
                    gvDiscrepancy.Columns[9].Visible = false;
                }
            }
        }

        protected void gvDiscrepancy_Sorting(object sender, GridViewSortEventArgs e)
        {
            this.Discrepancy_buildGridData(this.Discrepancy_buildGridData());
        }

        protected void gvDiscrepancy_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            ViewState["DiscrepancyIsInsert"] = false;

            gvDiscrepancy.PageIndex = e.NewPageIndex;
            gvDiscrepancy.EditIndex = -1;
            this.Discrepancy_buildGridData(this.Discrepancy_buildGridData());
        }

        protected void gvDiscrepancy_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }

        protected void lnkDiscSearch_Click(object sender, EventArgs e)
        {
            cp = HttpContext.Current.User as CustomPrincipal;
            string StoreRefNo = txtDiscStoreRefNumber.Text.Trim();
            string Tenant = txtDiscTenant.Text.Trim();

            if (StoreRefNo == "Search Store Ref.# ...")
                StoreRefNo = "";

            if (Tenant == "Search Tenant...")
                Tenant = "";
            ViewState["DiscrepancySQLString"] = this.DiscrepancySQL + " @WarehouseIDs= " + DB.SQuote(String.Join(",", cp.Warehouses)) + ",@StoreRefNo=" + DB.SQuote(StoreRefNo) + ",@Tenant=" + DB.SQuote(Tenant);

            this.Discrepancy_buildGridData(this.Discrepancy_buildGridData());

            txtDiscStoreRefNumber.Text = "Search Store Ref.# ...";

        }


        #endregion ------------------- Discripency Shipments  ----------------------

        #region -------------------------Shipment Expected -----------------------------------

        protected DataSet ShipmentExpected_buildGridData()
        {
            string sql = ViewState["ShipmentExpectedSQLString"].ToString();
            dsShipmentExpected = DB.GetDS(sql, false);
            if (dsShipmentExpected.Tables[0].Rows.Count == 0)
            {
                lblSIERecordCount.InnerText = "";
            }
            else
            {
                if (dsShipmentExpected.Tables[0].Rows[0]["TodayCount"].ToString() != "0")
                {
                    lbltodaySHERecCount.InnerHtml = "&nbsp;" + dsShipmentExpected.Tables[0].Rows[0]["TodayCount"].ToString() + "&nbsp;";


                }

                lblSIERecordCount.InnerHtml = "&nbsp;&nbsp;&nbsp;   [" + dsShipmentExpected.Tables[0].Rows.Count.ToString() + "]";
            }
            return dsShipmentExpected;

        }

        protected void ShipmentExpected_buildGridData(DataSet ds)
        {
            gvShipmentExpected.DataSource = dsShipmentExpected.Tables[0];
            gvShipmentExpected.DataBind();
            dsShipmentExpected.Dispose();
        }

        protected void gvShipmentExpected_Sorting(object sender, GridViewSortEventArgs e)
        {
            this.ShipmentExpected_buildGridData(this.ShipmentExpected_buildGridData());
        }

        protected void gvShipmentExpected_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            ViewState["ShipmentExpectedIsInsert"] = false;

            gvShipmentExpected.PageIndex = e.NewPageIndex;
            gvShipmentExpected.EditIndex = -1;
            this.ShipmentExpected_buildGridData(this.ShipmentExpected_buildGridData());
        }

        protected void gvShipmentExpected_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            //======================= Added By M.D.Prasad For View Only Condition ======================//
            cp = HttpContext.Current.User as CustomPrincipal;
            string[] hh = cp.Roles;
            for (int i = 0; i < hh.Length; i++)
            {
                if (cp.UserTypeID == 3 && Convert.ToInt32(hh[i]) == 4)
                {
                    imgbtngvShipmentExpected.Visible = false;
                }
            }

            if ((e.Row.RowType == DataControlRowType.DataRow) && (e.Row.RowState == DataControlRowState.Normal) || (e.Row.RowState == DataControlRowState.Alternate))
            {
                for (int i = 0; i < hh.Length; i++)
                {
                    if (cp.UserTypeID == 3 && Convert.ToInt32(hh[i]) == 4)
                    {
                        gvShipmentExpected.Columns[12].Visible = false;
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
                    String vltStoreRefNo = ((Literal)e.Row.Cells[1].FindControl("ltStoreRefNo")).Text;

                    if (vPriority != "")
                    {
                        String vltPriorityTime = ((Literal)e.Row.Cells[0].FindControl("ltPriorityTime")).Text;


                        StringBuilder StrTimetickerJS = new StringBuilder();

                        if (vltPriorityTime != "")
                        {

                            DateTime dtPriorityDateTime = Convert.ToDateTime(vltPriorityTime);

                            StrTimetickerJS.Append("<script type=\"text/javascript\">");
                            StrTimetickerJS.Append("$(function() {");
                            StrTimetickerJS.Append("var austDay = new Date();");
                            StrTimetickerJS.Append("austDay = new Date(" + dtPriorityDateTime.Year.ToString() + "," + (dtPriorityDateTime.Month - 1).ToString() + "," + dtPriorityDateTime.Day.ToString() + "," + dtPriorityDateTime.Hour.ToString() + "," + dtPriorityDateTime.Minute.ToString() + "," + CommonLogic.RandomNumber(60).ToString() + ");");
                            //StrTimetickerJS.Append("austDay = new Date(" + vltPriorityTime + ");");
                            //StrTimetickerJS.Append("$('#ShipmentExpectedCD" + e.Row.RowIndex.ToString() + "').countdown({ until: austDay, compact: true, format: 'dHMS', description: ''});");

                            if (dtPriorityDateTime > DateTime.Now)
                            {
                                StrTimetickerJS.Append("$('#ShipmentExpectedCD" + e.Row.RowIndex.ToString() + "').countdown({ until: austDay, compact: true, format: 'dHMS', description: ''});");
                            }
                            else
                            {
                                StrTimetickerJS.Append("$('#ShipmentExpectedCD" + e.Row.RowIndex.ToString() + "').countdown({ since: austDay, compact: true, format: 'dHMS', description: ''});");
                            }

                            StrTimetickerJS.Append("});</script>");

                            //StrTimetickerJS.Append("<img src=\"images\\horse1.gif\" border=\"0\" width=\"25\"/><div id=\"ShipmentExpectedCD" + e.Row.RowIndex.ToString() + "\" class=\"timeAlertDiv\"></div>");
                            //e.Row.Cells[0].Text = StrTimetickerJS.ToString();

                            if (dtPriorityDateTime > DateTime.Now)
                            {
                                StrTimetickerJS.Append("<img src=\"..\\images\\horse1.gif\" border=\"0\" width=\"25\"/><div id=\"ShipmentExpectedCD" + e.Row.RowIndex.ToString() + "\" class=\"timeAlertDiv\"></div>");
                                e.Row.Cells[0].Text = StrTimetickerJS.ToString();
                            }
                            else
                            {
                                StrTimetickerJS.Append("<div id=\"ShipmentExpectedCD" + e.Row.RowIndex.ToString() + "\" class=\"timeAlertDiv\"></div>");
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

            }*/

            if (e.Row.RowType == DataControlRowType.DataRow && ((e.Row.RowState == DataControlRowState.Normal) || (e.Row.RowState == DataControlRowState.Alternate)))
            {
                if (cp.TenantID != 0)
                {
                    gvShipmentExpected.Columns[12].Visible = false;
                }
            }
        }

        protected void lnkSIESearch_Click(object sender, EventArgs e)
        {
            string StoreRefNo = txtSIEStoreRefNo.Text.Trim();
            string Tenant = txtSIETenant.Text.Split('-')[0];
            string TenantID = hifSIETenant.Value.ToString();
            if (StoreRefNo == "")
                StoreRefNo = "";

            if (Tenant == "")
                Tenant = "";

            if (txtWarehouse.Text == "")
                hdnWarehouse.Value = "";

            //ViewState["ShipmentExpectedSQLString"] = this.ShipmentExpectedSQL + " @WarehouseIDs= " + DB.SQuote(String.Join(",", cp.Warehouses)) + ",@StoreRefNo=" + DB.SQuote(StoreRefNo) + ",@Tenant=" + DB.SQuote(Tenant)+",@TenantID= "+ DB.SQuote(TenantID) + " , @AccountID_New = " + cp.AccountID;
            ViewState["ShipmentExpectedSQLString"] = this.ShipmentExpectedSQL + " @WarehouseIDs= " + DB.SQuote(hdnWarehouse.Value) + ",@StoreRefNo=" + DB.SQuote(StoreRefNo) + ",@Tenant=" + DB.SQuote(Tenant) + ",@TenantID= " + DB.SQuote(TenantID) + " , @AccountID_New = " + cp.AccountID;

            this.ShipmentExpected_buildGridData(this.ShipmentExpected_buildGridData());

            //txtSIEStoreRefNo.Text = "Search Store Ref.# ...";

        }

        //protected void btngvShipmentExpected_Click(object sender, ImageClickEventArgs e)
            protected void btngvShipmentExpected_Click(object sender, EventArgs e)
        {
            try
            {
                gvShipmentExpected.AllowPaging = false;
                string[] hiddencolumns = { "Change" };
                this.ShipmentExpected_buildGridData(this.ShipmentExpected_buildGridData());
                CommonLogic.ExporttoExcel1(gvShipmentExpected, "Shipment Expected", hiddencolumns);
            }
            catch (Exception ex)
            {
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
                resetError("Error While Exporting Data", true);
            }
        }

        #endregion -------------------------Shipment Expected -----------------------------

        #region ---------------------Export Data-----------

        //protected void imgbtngvShipmentPending_Click1(object sender, ImageClickEventArgs e)
            protected void imgbtngvShipmentPending_Click1(object sender, EventArgs e)
        {
            cp = HttpContext.Current.User as CustomPrincipal;
            try
            {
                gvShipmentPending.AllowPaging = false;
                string[] hiddencolumns = { "Change" };
                this.ShipmentPending_buildGridData(this.ShipmentPending_buildGridData());
                CommonLogic.ExporttoExcel(gvShipmentPending, "Shipments In Transit", hiddencolumns);
            }
            catch (Exception ex)
            {
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
                resetError("Error While Exporting Data", true);
            }
        }

        //protected void btngvShipInProcess_Click(object sender, ImageClickEventArgs e)
         protected void btngvShipInProcess_Click(object sender, EventArgs e)
        {
            cp = HttpContext.Current.User as CustomPrincipal;
            try
            {
                gvShipInProcess.AllowPaging = false;
                string[] hiddencolumns = { "Change" };
                this.ShipInProcess_buildGridData(this.ShipInProcess_buildGridData());
                CommonLogic.ExporttoExcel(gvShipInProcess, "Shipments In Process", hiddencolumns);
            }
            catch (Exception ex)
            {
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
                resetError("Error While Exporting Data", true);
            }

        }

        //protected void imgbtngvGRNPending_Click(object sender, ImageClickEventArgs e)
         protected void imgbtngvGRNPending_Click(object sender, EventArgs e)

        {
            cp = HttpContext.Current.User as CustomPrincipal;
            try
            {
                gvGRNPending.AllowPaging = false;
                string[] hiddencolumns = { "Change" };
                this.GRNPending_buildGridData(this.GRNPending_buildGridData());
                CommonLogic.ExporttoExcel(gvGRNPending, "GRN Pending", hiddencolumns);
            }
            catch (Exception ex)
            {
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
                resetError("Error While Exporting Data", true);
            }
        }

        //protected void imgbtngvDiscrepancy_Click(object sender, ImageClickEventArgs e)
            protected void imgbtngvDiscrepancy_Click(object sender, EventArgs e)
        {
            cp = HttpContext.Current.User as CustomPrincipal;
            try
            {
                gvDiscrepancy.AllowPaging = false;
                string[] hiddencolumns = { "Change" };
               // Int16[] hiddencolumns = { 9 };
                this.Discrepancy_buildGridData(this.Discrepancy_buildGridData());
                CommonLogic.ExporttoExcel(gvDiscrepancy, "Discrepancy Shipments", hiddencolumns);
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

        #endregion ---------------------Export Data-----------





    }
}