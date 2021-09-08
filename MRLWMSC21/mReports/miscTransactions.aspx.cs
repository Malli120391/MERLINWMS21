using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Reflection;
using MRLWMSC21Common;
using System.Data;
using System.Globalization;
using System.Web.Script.Services;
using System.Web.Services;
using Newtonsoft.Json;
using System.Text;

namespace MRLWMSC21.mReports
{
    public partial class miscTransactions : System.Web.UI.Page
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
                DesignLogic.SetInnerPageSubHeading(this.Page, "Current Stock");
            }
        }

    

        //=================================== Added By M.D.Prasad =========================================//
        [WebMethod]
        public static string getMiscTransactions(string Tenantid, string Mcode)
        {            
            Tenantid = Tenantid == "" ? "0" : Tenantid;
            Mcode = Mcode == "" ? "0" : Mcode;
            DataSet ds = DB.GetDS("EXEC [sp_RPT_Misc_Transactions] @TenantID=" + Tenantid + ",@FromDate=NULL,@Todate=NULL,@MaterialMasterID=" + Mcode, false);
            return JsonConvert.SerializeObject(ds);
        }

        //=================================== Added By M.D.Prasad =========================================//

   
    }
}