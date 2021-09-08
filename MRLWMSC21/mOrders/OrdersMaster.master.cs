using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MRLWMSC21Common;

namespace MRLWMSC21.mOrders
{
    public partial class OrdersMaster : System.Web.UI.MasterPage
    {
        CustomPrincipal cp = HttpContext.Current.User as CustomPrincipal;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (HttpContext.Current.User.Identity.IsAuthenticated)
                {
                    cp = HttpContext.Current.User as CustomPrincipal;
                    //ltMenuInbound.Text = DesignLogic.LoadQuickLinks(String.Join(",", cp.Roles), 74);

                    ltMenuInbound.Text = DesignLogic.LoadQuickLinks(String.Join(",", cp.Roles), 74,cp.TenantID);
                }
            }

        }
    }
}