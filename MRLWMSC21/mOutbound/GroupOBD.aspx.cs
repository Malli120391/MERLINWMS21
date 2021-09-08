using MRLWMSC21Common;
using MRLWMSC21.mOutbound.BL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MRLWMSC21.mOutbound
{
    public partial class GroupOBD : System.Web.UI.Page
    {
        public static CustomPrincipal cp;
        protected void Page_PreInit(object sender, EventArgs e)
        {
            cp = HttpContext.Current.User as CustomPrincipal;
           // Page.Theme = "Inventory";
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DesignLogic.SetInnerPageSubHeading(this.Page, "Group OBD");
            }
        }
        //------------------------ added by durga for getting customers on 15/11/2017----------------------//
        [WebMethod]
        public static List<DropDownListDataFilter> getCustomerData(string prefix, int tenantid)
        {
            try
            {
                OutBoundBL objbl = new OutBoundBL();
                return objbl.getCustomerData(prefix, tenantid);
            }
            catch
            {
                return null;
            }

        }
   
        [WebMethod]
        public static List<DropDownListDataFilter> getVehicleTypeData()
        {
            try
            {
                OutBoundBL objbl = new OutBoundBL();
                return objbl.getVehicleTypeData();
            }
            catch
            {
                return null;
            }

        }
        [WebMethod]
        public static List<DropDownListDataFilter> getDocks(string WareHouseID)
        {
            try
            {
                OutBoundBL objbl = new OutBoundBL();
                return objbl.getDocks(WareHouseID);
            }
            catch
            {
                return null;
            }

        }
        //------------------------ added by durga for getting customers on 15/11/2017----------------------//
        [WebMethod]
        public static List<Outbound> getoUTBOUNDDataForGroupOBD(GroupOutboundSearchData obj)
        {
            try
            {
               
                OutBoundBL objbl = new OutBoundBL();
                return objbl.getoUTBOUNDDataForGroupOBD(obj);
            }
            catch
            {
                return null;
            }

        }
        //------------------------ added by durga for getting customers on 15/11/2017----------------------//
        [WebMethod]
        public static string CreateGroupOBD(string outboundids,GroupOutboundSearchData obj,int VehicletypeId,string VehicleNo,string Driver, string Mobile,int DockID)
        {
            try
            {
                OutBoundBL objbl = new OutBoundBL();
                return objbl.createGroupobd(obj, outboundids, cp.UserID, VehicletypeId, VehicleNo, Driver, Mobile, DockID);                
            }
            catch
            {
                return null;
            }

        }
        [WebMethod]
        public static List<DropDownListData> getTenantData()
        {
            try
            {
                OutBoundBL objbl = new OutBoundBL();
                return objbl.getTenantData();
            }
            catch
            {
                return null;
            }

        }
        [WebMethod]
        public static List<DropDownListDataFilter> getWareHouseData(int TenantID)
        {
            try
            {
                OutBoundBL objbl = new OutBoundBL();
                return objbl.getWareHouseDataData1(TenantID);
            }
            catch
            {
                return null;
            }

        }
    }
}