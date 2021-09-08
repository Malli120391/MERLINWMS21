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

namespace MRLWMSC21.mInbound
{
    public partial class InboundList : System.Web.UI.Page
    {
        public static CustomPrincipal cp1;
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        [WebMethod]
        public static string getOutboundData(int InboundID, string StoreRefNo, int WareHouseID, int TenantID, string PaginationId, string PageSize)
        {
            cp1 = HttpContext.Current.User as CustomPrincipal;
            try
            {
                string query = "";
                DataSet ds = DB.GetDS(query, false);
                return JsonConvert.SerializeObject(ds);
            }
            catch (Exception ex)
            {
                return "";
            }
        }
    }
}