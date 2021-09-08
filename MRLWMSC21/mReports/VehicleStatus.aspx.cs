using MRLWMSC21Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MRLWMSC21.mReports
{
    public partial class VehicleStatus : System.Web.UI.Page
    {
        public CustomPrincipal cp1 = HttpContext.Current.User as CustomPrincipal;
        public static CustomPrincipal cp;

        protected void Page_PreInit(object sender, EventArgs e)
        {
            cp = HttpContext.Current.User as CustomPrincipal;
            Page.Theme = "Outbound";
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DesignLogic.SetInnerPageSubHeading(this.Page, "");
            }
        }

        //=================================== Added By M.D.Prasad =========================================//
        [WebMethod]
        public static string GetVehicleYardAvailability_Status()
        {
            DataSet ds = DB.GetDS("EXEC [sp_RPT_VehicleYardAvailability_Status] @AccountID=" + cp.AccountID, false);
            return JsonConvert.SerializeObject(ds);
        }
        //=====================================  Added By M.D.Prasad ======================================//
    }
}