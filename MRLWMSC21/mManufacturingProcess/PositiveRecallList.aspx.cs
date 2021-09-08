using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using MRLWMSC21Common;

namespace MRLWMSC21.mManufacturingProcess
{
    public partial class PositiveRecallList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ViewState["PositiveReCallList"] = "dbo.sp_MFG_GetPositiveReCallList @PositiveRecallID=0";
                Build_PositiveReCall(Build_PositiveReCall());
            }
        }

        private DataSet Build_PositiveReCall()
        {
            String cmdPositiveReCall = ViewState["PositiveReCallList"].ToString();
            return DB.GetDS(cmdPositiveReCall,false);
        }

        private void Build_PositiveReCall(DataSet dsPositiveReCall)
        {
            gvPositiveRecall.DataSource = dsPositiveReCall;
            gvPositiveRecall.DataBind();
            dsPositiveReCall.Dispose();
        }

        protected void gvPositiveRecall_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.DataItem == null)
                return;

            if ((!((e.Row.RowState & DataControlRowState.Edit) == DataControlRowState.Edit)) )
            {
               
                DataRow row = ((DataRowView)e.Row.DataItem).Row;
                String ReCallID = row["PositiveRecallID"].ToString();
                String ProRefNo = row["PRORefNo"].ToString();
                String WorkCenter = row["ONWorkCenter"].ToString();

                ImageButton lnkSplit = (ImageButton)e.Row.FindControl("lnkClose");

                lnkSplit.OnClientClick = " openPositiveReCallDialog();";

            }
        }

        protected void lnkCloseRecall_Click(object sender, EventArgs e)
        {
            String jj= lbProRefNo.Text;

            if (txtResonForClose.Text != "")
            {

                DB.ExecuteSQL("update  MFG_PositiveRecall set IsActive=0 where PositiveRecallID=" + hifReCallID.Value);
                Build_PositiveReCall(Build_PositiveReCall());
                 Response.Redirect("PositiveReCallList.aspx");
                //ClientScript.RegisterStartupScript(this.GetType(), "Close", "closePositiveReCallDialog();", true);
                //HttpContext context = HttpContext.Current;
                // context.Response.Write("<script type='text/javascript'>window.frameElement.closePositiveReCallDialog()</script>");
                // context.Response.Flush();
                // context.Response.End();
            }
            else
            {
                resetError("Enter reason for Close",true);
            }
        }

        protected void resetError(string error, bool isError)
        {

            string str = "<font class=\"noticeMsg\">NOTICE:</font>&nbsp;&nbsp;&nbsp;";
            if (isError)
                str = "<font class=\"errorMsg\">ERROR:</font>&nbsp;&nbsp;&nbsp;";

            if (error.Length > 0)
                str += error + "";
            else
                str = "";


            lbStatus.Text = str;


        }

        protected void gvPositiveRecall_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvPositiveRecall.PageIndex = e.NewPageIndex;
            Build_PositiveReCall(Build_PositiveReCall());
        }

        protected void gvPositiveRecall_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            String []Arguments = e.CommandArgument.ToString().Split(',');
            lbProRefNo.Text = Arguments[0];
            lbWorkCenter.Text = Arguments[1];
            hifReCallID.Value=Arguments[2];
        }
    }
}