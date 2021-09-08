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

// Module Name : PendingForDelivery Under Outbound
// DevelopedBy : Naresh P
// CreatedOn   : 23/11/2013
// Modified On : 24/03/2015

namespace MRLWMSC21.mOutbound
{
    public partial class PendingforDelivery : System.Web.UI.Page
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
        String DCRPendingSQL = "EXEC [dbo].[sp_OBD_GetPODPendingList]  ";


        protected void Page_PreInit(object sender, EventArgs e)
        {
            Page.Theme = "Outbound";
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            DesignLogic.SetInnerPageSubHeading(this.Page, "Deliveries");


            string query = "EXEC [dbo].[sp_TPL_GetTenantDirectoryInfo] @TypeID=2";

            DataSet dsPath = DB.GetDS(query.ToString(), false);

            TenantRootDir = dsPath.Tables[0].Rows[4][0].ToString();
            OutboundPath = dsPath.Tables[0].Rows[0][0].ToString();
            OBD_DeliveryNotePath = dsPath.Tables[0].Rows[1][0].ToString();
            OBD_PickandCheckSheetPath = dsPath.Tables[0].Rows[2][0].ToString();
            OBD_PODPath = dsPath.Tables[0].Rows[3][0].ToString();

            if (!IsPostBack)
            {
                // Delivery Pendings
                ViewState["DeliveryPendingSQLString"] = this.DeliveryPendingSQL + "@OBDNumber=NULL,@WarehouseIDs="+DB.SQuote(String.Format(",",cp.Warehouses));
                ObdDeliveryPending_buildGridData(this.ObdDeliveryPending_buildGridData());


                //POD Pending
                ViewState["DCRPendingSQLString"] = this.DCRPendingSQL + "@OBDNumber=NULL,@WarehouseIDs=" + DB.SQuote(String.Format(",", cp.Warehouses));
                ObdDCRPending_buildGridData(this.ObdDCRPending_buildGridData());

            }
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

        protected void gvOBDDeliveryPending_OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                if ((e.Row.RowState == DataControlRowState.Normal) || (e.Row.RowState == DataControlRowState.Alternate))
                {
                    String vPriority = ((Literal)e.Row.FindControl("ltPriorityLevel")).Text;
                    String vltltOBDNumber_Pending = ((Literal)e.Row.FindControl("ltOBDNumber_Pending")).Text;



                    Literal ltDocTypeID = (Literal)e.Row.FindControl("ltDocTypeID");
                    Label lblOBDID = (Label)e.Row.FindControl("lblOBDID");
                    Label lblLineItemCount = (Label)e.Row.FindControl("lblLineItemCount");

                    HyperLink link = (HyperLink)e.Row.FindControl("lnkHyperLink");


                    if (ltDocTypeID.Text != "6")
                    {
                        link.NavigateUrl = "DeliveryPickNote.aspx?obdid=" + lblOBDID.Text + "&lineitemcount=" + lblLineItemCount.Text;
                    }
                    else
                    {
                        link.NavigateUrl = "~/mManufacturingProcess/DeliveryPickNote.aspx?obdid=" + lblOBDID.Text + "&lineitemcount=" + lblLineItemCount.Text;
                    }



                    
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
                    }
                    
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

            if (txtDelvPOBDNumber.Text != "")
            {
                ViewState["DeliveryPendingSQLString"] = this.DeliveryPendingSQL + "@OBDNumber="+DB.SQuote(txtDelvPOBDNumber.Text)+",@WarehouseIDs=" + DB.SQuote(String.Format(",", cp.Warehouses));
            }
            else {
                ViewState["DeliveryPendingSQLString"] = this.DeliveryPendingSQL + "@OBDNumber=NULL,@WarehouseIDs=" + DB.SQuote(String.Format(",", cp.Warehouses));
            }


            ObdDeliveryPending_buildGridData(this.ObdDeliveryPending_buildGridData());

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

            if (dsPODPending.Tables[0].Rows.Count==0)
            {
                lblPODRecCount.InnerText = "";
            }else{
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

            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                if ((e.Row.RowState == DataControlRowState.Normal) || (e.Row.RowState == DataControlRowState.Alternate))
                {

                    Literal ltDocTypeID = (Literal)e.Row.FindControl("ltDocTypeID");
                    Label lblOBDID = (Label)e.Row.FindControl("lblOBDID");
                    Label lblLineItemCount = (Label)e.Row.FindControl("lblLineItemCount");

                    HyperLink link = (HyperLink)e.Row.FindControl("lnkHyperLink");


                    if (ltDocTypeID.Text != "6")
                    {
                        link.NavigateUrl = "DeliveryPickNote.aspx?obdid=" + lblOBDID.Text + "&lineitemcount=" + lblLineItemCount.Text;
                    }
                    else
                    {
                        link.NavigateUrl = "~/mManufacturingProcess/DeliveryPickNote.aspx?obdid=" + lblOBDID.Text + "&lineitemcount=" + lblLineItemCount.Text;
                    }

                }
            }
           
        }

        protected void lnlPODSearch_Click(object sender, EventArgs e)
        {

            if (txtPODDelDocNo.Text != "")
            {
                ViewState["DCRPendingSQLString"] = this.DCRPendingSQL + "@OBDNumber="+DB.SQuote(txtPODDelDocNo.Text)+",@WarehouseIDs=" + DB.SQuote(String.Format(",", cp.Warehouses));
            }
            else {
                ViewState["DCRPendingSQLString"] = this.DCRPendingSQL + "@OBDNumber=NULL,@WarehouseIDs=" + DB.SQuote(String.Format(",", cp.Warehouses));
            }

                
            ObdDCRPending_buildGridData(this.ObdDCRPending_buildGridData());

        }




        #endregion -------------------------POD Pending -----------------------------

        protected void imgbtngvOBDReceived_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
            
            //CommonLogic.PrepareGridViewForExport(gvShipmentResults, HiddenColumns);
            //String ExcelFileName = "InboundSearch";
            //CommonLogic.ExportGridView(gvShipmentResults, ExcelFileName);
                string[] hiddencolumns={"",""};
                this.ObdDeliveryPending_buildGridData(this.ObdDeliveryPending_buildGridData());
                CommonLogic.ExporttoExcel(gvOBDReceived, "Deliveries Pending", hiddencolumns);
            }
            catch (Exception ex)
            {
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
                resetError("Error While Exporting Data", true);
            }
        }

        protected void imgbtngvDCRPending_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                string[] hiddencolumns = { "", "" };
                this.ObdDCRPending_buildGridData(this.ObdDCRPending_buildGridData());
                CommonLogic.ExporttoExcel(gvDCRPending, "POD Pending ", hiddencolumns);
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

    }
}