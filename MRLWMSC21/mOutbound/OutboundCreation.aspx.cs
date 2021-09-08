using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Reflection;
using MRLWMSC21Common;
using System.Data;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using MRLWMSC21.mOutbound.BL;
using Newtonsoft.Json;
using System.IO;

namespace MRLWMSC21.mOutbound
{
    public partial class OutboundCreation : System.Web.UI.Page
    {
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
                DesignLogic.SetInnerPageSubHeading(this.Page, "Create Outbound New");
            }
        }
        [WebMethod]
        public static List<SaleOrder> getSOListForCreateOutbound(int TenantID)
        {
            cp = HttpContext.Current.User as CustomPrincipal;
            List<SaleOrder> SOList = new List<SaleOrder>();
            try
            {
                string Query = "EXEC [dbo].[GET_OPEN_SOLIST_OUTBOUNDCREATION] @TenantID = " + TenantID + ",@UserID="+cp.UserID+"";
                DataSet DS = DB.GetDS(Query, false);
                if(DS.Tables[0].Rows.Count > 0)
                {
                    foreach(DataRow row in DS.Tables[0].Rows)
                    {
                        SaleOrder so = new SaleOrder();
                        so.SOHeaderID = Convert.ToInt32(row["SOHeaderID"].ToString());
                        so.SOnumber = row["SONumber"].ToString();
                        so.SODate = row["SODate"].ToString();
                        so.CustomerName = row["CustomerName"].ToString();
                        so.SOType = row["SOType"].ToString();
                        so.SOSatus = row["StatusName"].ToString();
                        so.IsSelected = false;
                        SOList.Add(so);
                    }
                }
                OutBoundBL objbl = new OutBoundBL();
                return SOList;
            }
            catch
            {
                return null;
            }

        }
        //------------------------ added by durga for getting WareHose data on 15/11/2017----------------------//
        [WebMethod]
        public static List<DropDownListDataFilter> getWareHouseData(string TenantId)
        {
            try
            {
                OutBoundBL objbl = new OutBoundBL();
                return objbl.getWareHouseDataData(TenantId);
            }
            catch
            {
                return null;
            }

        }


        public class SaleOrder
        {
            public int SOHeaderID { get; set; }
            public string SOnumber { get; set; }
            public string SODate { get; set; }
            public string SOType { get; set; }
            public string SOSatus { get; set; }

            public string CustomerName { get; set; }
            public Boolean IsSelected { get; set; }
        }

    }
}