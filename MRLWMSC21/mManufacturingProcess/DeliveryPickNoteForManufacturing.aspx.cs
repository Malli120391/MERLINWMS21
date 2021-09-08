using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using MRLWMSC21Common;

namespace MRLWMSC21.mManufacturingProcess
{
    public partial class DeliveryPickNoteForManufacturing : System.Web.UI.Page
    {
        public CustomPrincipal cp = HttpContext.Current.User as CustomPrincipal;

        protected void Page_PreInit(object sender, EventArgs e)
        {
            Page.Theme = "Inbound";

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!cp.IsInAnyRoles(CommonLogic.GetRolesAllowed("1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20")))
            {
                Response.Redirect("Login.aspx?eid=6");
            }
            DesignLogic.SetInnerPageSubHeading(this.Page, "Delivery Pick Note");
        }

        protected void lnkProductionorder_Click(object sender, EventArgs e)
        {
            if (atcProductionOrderNo.Text.Trim() != "Production Order RefNo...")
            {
                ViewState["DeliveryPickNote"] = "EXEC dbo.sp_MFG_GetDeliverypickNoteForManufacturing @PRORefNo=" + DB.SQuote(atcProductionOrderNo.Text.Trim());
                Biuld_gvDeliveryPickNote(Biuld_gvDeliveryPickNote());
            }
        }

        protected void gvDeliveryPickNote_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvDeliveryPickNote.PageIndex = e.NewPageIndex;
            Biuld_gvDeliveryPickNote(Biuld_gvDeliveryPickNote());
        }

        private DataSet Biuld_gvDeliveryPickNote()
        {
            String cmdDeliveryPickNote = ViewState["DeliveryPickNote"].ToString();
            return DB.GetDS(cmdDeliveryPickNote,false);
        }

        private void Biuld_gvDeliveryPickNote(DataSet dsDelivceryPickNote)
        {
            gvDeliveryPickNote.DataSource = dsDelivceryPickNote;
            gvDeliveryPickNote.DataBind();
            dsDelivceryPickNote.Dispose();
        }
    }
}