using MRLWMSC21Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MRLWMSC21.TPL
{
    public partial class TenantInvoiceList : System.Web.UI.Page
    {
        public CustomPrincipal cp = HttpContext.Current.User as CustomPrincipal;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                
                GETINVOICELIST(GETINVOICELIST());
            }
        }

        protected DataSet GETINVOICELIST()
        {
            DataSet DS = DB.GetDS("EXEC [dbo].[GET_INVOICELIST] @AccountID_New=" + cp.AccountID + ",@TenantID_New=" + cp.TenantID, false);
            return DS;
        }
        protected void GETINVOICELIST(DataSet DS)
        {
            gvInvoice.DataSource = DS;
            gvInvoice.DataBind();
            DS.Dispose();
        }

        protected void gvInvoice_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvInvoice.PageIndex = e.NewPageIndex;
            gvInvoice.EditIndex = -1;
            GETINVOICELIST(GETINVOICELIST());
        }
    }
}