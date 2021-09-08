using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MRLWMSC21Common;
using System.Data.SqlClient;
using System.Data;
using MRLWMSC21.TPL.Invoice;



namespace MRLWMSC21.TPL
{
    public partial class InvoiceGeneration : System.Web.UI.Page
    {
        public CustomPrincipal cp = HttpContext.Current.User as CustomPrincipal;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (CommonLogic.QueryString("msg")!="")
                resetError(CommonLogic.QueryString("msg"),false);
                BuidGridData(BuidGridData());
            }
        }

        private DataSet BuidGridData()
        {
            cp = HttpContext.Current.User as CustomPrincipal;
            try
            {
                DataSet ds = DB.GetDS("EXEC [sp_TPL_GetInvoiceQueueList] @AccountID_New=" + cp.AccountID + ",@TenantID_New=" + cp.TenantID, false);
                return ds;
            }
            catch (Exception ex)
            {
                return null;
            }
            //DataSet ds = DB.GetDS("EXEC [sp_TPL_GetInvoiceData] @AccountID_New=" + cp.AccountID + ",@TenantID_New=" + cp.TenantID, false);
           
        }

        private void BuidGridData(DataSet ds)
        {
            gvInvoice.DataSource = ds;
            gvInvoice.DataBind();
            ds.Dispose();
        }

        protected void gvInvoice_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvInvoice.PageIndex = e.NewPageIndex;
            BuidGridData(BuidGridData());

        }

        protected void gvInvoice_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "InvoiceGeneration")
            {
                string[] arguments = e.CommandArgument.ToString().Split('`');
                TPLBilling bill = new TPLBilling(Convert.ToInt16(arguments[0]));
                if (bill.getInvoice(Convert.ToInt16(arguments[0]), Convert.ToDateTime(arguments[1]), Convert.ToDateTime(arguments[2])))
                {
                    string filename = Convert.ToDateTime(arguments[1]).ToString("dd-MM-yyyy") + "_" + Convert.ToDateTime(arguments[2]).ToString("dd-MM-yyyy") + "_" + DateTime.Now.ToString("dd-MM-yyyy") + "_" + arguments[0] + ".pdf";
                    Session["bill"] = bill;
                    Response.Redirect("DisplayInvoice.aspx?fid=" + filename + "&isEnable=" + bill.IsAccurateInvoice);

                }
                else
                {

                    resetError("Error while generating invoice", true);
                }                    
            }
            if (e.CommandName == "ViewBill")
            {
                try
                {
                    string[] arguments = e.CommandArgument.ToString().Split('`');
                    //Response.Redirect("WeeklyBilling.aspx?TenantID=" + Convert.ToInt16(arguments[0]) + "&From=" + Convert.ToDateTime(arguments[1]).ToString("dd-MM-yyyy") + "&To=" + Convert.ToDateTime(arguments[2]).ToString("dd-MM-yyyy"));
                    string url = "WeeklyBilling.aspx?TenantID=" + Convert.ToInt16(arguments[0]) + "&From=" + Convert.ToDateTime(arguments[1]).ToString("dd-MM-yyyy") + "&To=" + Convert.ToDateTime(arguments[2]).ToString("dd-MM-yyyy");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "key", "window.open('" + url + "');", true);
                }
                catch
                {
                    resetError("Error while viewing bill", true);
                }
            }
        }

        protected void resetError(string error, bool isError)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "test", "showStickyToast(" + (isError.ToString().ToLower() == "true" ? "false" : "true") + ",\"" + error + "\");", true);
        }

    }
}