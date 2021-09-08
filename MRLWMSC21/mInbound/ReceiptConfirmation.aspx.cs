using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using MRLWMSC21Common;
using System.Data.SqlClient;
using System.Data;

// Module Name : ReceiptConfirmation Under Inbound
// Description : 
// DevelopedBy : Naresh P
// CreatedOn   : 25/11/2013
// Lsst Modified Date : 

namespace MRLWMSC21.mInbound
{
    public partial class ReceiptConfirmation : System.Web.UI.Page
    {

        public CustomPrincipal cp = HttpContext.Current.User as CustomPrincipal;
        string TenantRootDir = "";
        string InboundPath = "";
        string InbShpDelNotePath = "";
        string InbShpVerifNotePath = "";
        string InbDescPath = "";
        DataSet dsShipmentExpected;
        DataSet dsVIProcess;

        String ShipmentExpectedSQL = "EXEC [dbo].[sp_INB_GetShipmentExpectedList]   ";
        String VerificationInProcessSQL = "EXEC [dbo].[sp_INB_GetShipmentVerificationProcessList]  ";
        String DiscrepancySQL = "EXEC [dbo].[sp_INB_GetDiscepancyShipmentsList]   ";

        protected void Page_PreInit(object sender, EventArgs e)
        {

            Page.Theme = "Inbound";

        }   

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!cp.IsInAnyRoles(CommonLogic.GetRolesAllowed(CommonLogic.GetRolesAllowedForthisPage("Inward QC Pending List"))))
            {
                Response.Redirect("../Login.aspx?eid=6");
            }


            DesignLogic.SetInnerPageSubHeading(this.Page, "QC In Process");

             //Get Tenant Root Directory
            TenantRootDir = DB.GetSqlS("select DefaultValue AS S from SYS_SystemConfiguration SYS_C left join SYS_SysConfigKey SYS_K on SYS_K.SysConfigKeyID=SYS_C.SysConfigKeyID where TenantID=" + cp.TenantID + " and SysConfigKey='TenantContentPath'");

            // Get Inbound Path
            InboundPath = DB.GetSqlS(" select DefaultValue AS S from SYS_SystemConfiguration SYS_C left join SYS_SysConfigKey SYS_K on SYS_K.SysConfigKeyID=SYS_C.SysConfigKeyID where TenantID=" + cp.TenantID + " and SysConfigKey='InboundPath'");

            // Get inbound delivery note path
            InbShpDelNotePath = DB.GetSqlS(" select DefaultValue AS S from SYS_SystemConfiguration SYS_C left join SYS_SysConfigKey SYS_K on SYS_K.SysConfigKeyID=SYS_C.SysConfigKeyID where TenantID=" + cp.TenantID + " and SysConfigKey='InboundShipmentDeliveryNote'");

            // Get inbound shipverification path
            InbShpVerifNotePath = DB.GetSqlS(" select DefaultValue AS S from SYS_SystemConfiguration SYS_C left join SYS_SysConfigKey SYS_K on SYS_K.SysConfigKeyID=SYS_C.SysConfigKeyID where TenantID=" + cp.TenantID + " and SysConfigKey='InboundShipmentVerificationNote'");

            // Get inbound Discrepancy path
            InbDescPath = DB.GetSqlS(" select DefaultValue AS S from SYS_SystemConfiguration SYS_C left join SYS_SysConfigKey SYS_K on SYS_K.SysConfigKeyID=SYS_C.SysConfigKeyID where TenantID=" + cp.TenantID + " and SysConfigKey='InboundDiscrepancyDoc'");

            if (!IsPostBack)
            {

               
                //QC In Process
                ViewState["VIProcessSQLString"] = this.VerificationInProcessSQL + " @WarehouseIDs= " + DB.SQuote(String.Join(",", cp.Warehouses)) + ",@StoreRefNo=NULL";
                this.VIProcess_buildGridData(this.VIProcess_buildGridData());

               
            }

        }

        public string GetStoreRefNoWithLink(String StoreRefNo)
        {
            String sFileName = CommonLogic._GetAttatchmentFile(TenantRootDir + DB.GetSqlS("select UniqueID AS S from GEN_Tenant where TenantID=" + cp.TenantID) + InboundPath + InbShpDelNotePath, StoreRefNo);

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


        #region -------------------------QC In Process -----------------------------

        protected DataSet VIProcess_buildGridData()
        {
            string sql = ViewState["VIProcessSQLString"].ToString();
            
            dsVIProcess = DB.GetDS(sql, false);

            if (dsVIProcess.Tables[0].Rows.Count == 0)
            {
                lblVerificationRecCout.InnerText = "";
            }
            else
            {
                lblVerificationRecCout.InnerHtml = "&nbsp;&nbsp;&nbsp;  [" + dsVIProcess.Tables[0].Rows.Count.ToString() + "]";
            }

            
            return dsVIProcess;

        }

        protected void VIProcess_buildGridData(DataSet ds)
        {
            gvVIProcess.DataSource = dsVIProcess.Tables[0];
            gvVIProcess.DataBind();
            dsVIProcess.Dispose();
        }

        protected void gvVIProcess_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                /*
                if ((e.Row.RowState == DataControlRowState.Normal) || (e.Row.RowState == DataControlRowState.Alternate))
                {
                    String vPriority = ((Literal)e.Row.Cells[0].FindControl("ltPriorityLevel")).Text;
                    String vltStoreRefNo = ((Literal)e.Row.Cells[0].FindControl("ltStoreRefNo")).Text;
                    
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
                            //StrTimetickerJS.Append("$('#VIProcessCD" + e.Row.RowIndex.ToString() + "').countdown({ until: austDay, compact: true, format: 'dHMS', description: ''});");

                            if (dtPriorityDateTime > DateTime.Now)
                            {
                                StrTimetickerJS.Append("$('#VIProcessCD" + e.Row.RowIndex.ToString() + "').countdown({ until: austDay, compact: true, format: 'dHMS', description: ''});");
                            }
                            else
                            {
                                StrTimetickerJS.Append("$('#VIProcessCD" + e.Row.RowIndex.ToString() + "').countdown({ since: austDay, compact: true, format: 'dHMS', description: ''});");
                            }


                            StrTimetickerJS.Append("});</script>");

                            //StrTimetickerJS.Append("<img src=\"images\\horse1.gif\" border=\"0\" width=\"25\"/><div id=\"VIProcessCD" + e.Row.RowIndex.ToString() + "\" class=\"timeAlertDiv\"></div>");
                            //e.Row.Cells[0].Text = StrTimetickerJS.ToString();

                            if (dtPriorityDateTime > DateTime.Now)
                            {
                                StrTimetickerJS.Append("<img src=\"..\\images\\horse1.gif\" border=\"0\" width=\"25\"/><div id=\"VIProcessCD" + e.Row.RowIndex.ToString() + "\" class=\"timeAlertDiv\"></div>");
                                e.Row.Cells[0].Text = StrTimetickerJS.ToString();
                            }
                            else
                            {
                                StrTimetickerJS.Append("<div id=\"VIProcessCD" + e.Row.RowIndex.ToString() + "\" class=\"timeAlertDiv\"></div>");
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

        protected void gvVIProcess_Sorting(object sender, GridViewSortEventArgs e)
        {
            this.VIProcess_buildGridData(this.VIProcess_buildGridData());
        }

        protected void gvVIProcess_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            ViewState["VIProcessIsInsert"] = false;
           
            gvVIProcess.PageIndex = e.NewPageIndex;
            gvVIProcess.EditIndex = -1;
            this.VIProcess_buildGridData(this.VIProcess_buildGridData());
        }

       

        protected void lnkVerifSearch_Click(object sender, EventArgs e)
        {

            if (txtVerStoreRefNo.Text == "Search Store Ref.# ...")
            {
                txtVerStoreRefNo.Text = "";
            }

            if (txtVerStoreRefNo.Text != "")
            {
                ViewState["VIProcessSQLString"] = this.VerificationInProcessSQL + " @WarehouseIDs= " + DB.SQuote(String.Join(",", cp.Warehouses)) + ",@StoreRefNo="+DB.SQuote(txtVerStoreRefNo.Text);
            }
            else {
                ViewState["VIProcessSQLString"] = this.VerificationInProcessSQL + " @WarehouseIDs= " + DB.SQuote(String.Join(",", cp.Warehouses)) + ",@StoreRefNo=NULL";
            }

            this.VIProcess_buildGridData(this.VIProcess_buildGridData());

            txtVerStoreRefNo.Text = "Search Store Ref.# ...";
        }

         #endregion ------------------------- QC In Process -----------------------------

        


        protected void imgbtngvVIProcess_Click(object sender, ImageClickEventArgs e)
        {
            try
            {

                gvVIProcess.AllowPaging = false;
                string[] hiddencolumns = { "", "RTR", "" };
                this.VIProcess_buildGridData(this.VIProcess_buildGridData());
                CommonLogic.ExporttoExcel(gvVIProcess, "QC In Process", hiddencolumns);
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


    }
}