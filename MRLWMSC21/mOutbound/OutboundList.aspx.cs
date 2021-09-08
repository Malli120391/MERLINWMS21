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

namespace MRLWMSC21.mOutbound
{
    public partial class OutboundList : System.Web.UI.Page
    {
        public static CustomPrincipal cp1;

        protected void Page_Load(object sender, EventArgs e)
        {
            cp1 = HttpContext.Current.User as CustomPrincipal;
            if (!IsPostBack)
            {
                DesignLogic.SetInnerPageSubHeading(this.Page, "Search OutboundList");
            }

        }
        [WebMethod]
        public static string getOutboundData( string StartDate, string EndDate, int OutboundID ,string CustomerPONumber ,int WareHouseID  ,int TenantID, string PaginationId, string PageSize)
        {
            cp1 = HttpContext.Current.User as CustomPrincipal;
            StartDate = StartDate == "" || StartDate == null ? "null" : DB.SQuote(StartDate);
            EndDate = EndDate == "" || EndDate == null ? "null" : DB.SQuote(EndDate);
            
            try
            {                
                string query = "EXEC [SP_GET_OUTBOUNDLIST_NEW]   @StartDate=" + StartDate + ",@EndDate=" + EndDate + ", @OutboundID=" + OutboundID + ",@CustomerPONumber='" + CustomerPONumber + "',@WareHouseID =" + WareHouseID + ",@TenantID =" + TenantID + ",@PageIndex = " + PaginationId + ",@PageSize = " + PageSize + "";
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