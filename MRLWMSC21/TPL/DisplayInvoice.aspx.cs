using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MRLWMSC21Common;
using MRLWMSC21.TPL.Invoice;

namespace MRLWMSC21.TPL
{
    public partial class DisplayInvoice : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //string filename = "01-07-2015_01-07-2015_01-07-2015_1.pdf";
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "test", "showStickyToast(true,\" Successfully generated invoice \");window.open(\" + " \",   \"resizable,scrollbars,status\"  );", true);
                iframe.Src = "DisplayPDF.aspx?filename=" + CommonLogic.QueryString("fid");
                lnksaveInvoice.Enabled = (CommonLogic.QueryString("isEnable") == "True");
            }
        }

        protected void lnksaveInvoice_Click(object sender, EventArgs e)
        {
            try
            {
                TPLBilling bill = (TPLBilling)Session["bill"];
                if (bill.saveInvoice(bill.TenantID, bill._FromDate, bill._ToDate))
                {
                    Session["bill"] = null;
                    Response.Redirect("InvoiceGeneration.aspx?msg=Invoice successfully sent");
                }
                else
                {
                    resetError("Error while sending invoice", true);
                }
            }
            catch (Exception ex)
            {
                CommonLogic.createErrorNode("0", "Display Invoice", ex.Source,ex.Message,ex.StackTrace);
            }
        }

        protected void lnkCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("../TPL/InvoiceGeneration.aspx");
        }

        protected void resetError(string error, bool isError)
        {

            ScriptManager.RegisterStartupScript(this, this.GetType(), "test", "showStickyToast(" + (isError.ToString().ToLower() == "true" ? "false" : "true") + ",\"" + error + "\");", true);

            
        }
    }
}