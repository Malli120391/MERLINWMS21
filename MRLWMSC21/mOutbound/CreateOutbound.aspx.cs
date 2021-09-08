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
    public partial class CreateOutbound : System.Web.UI.Page
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
                DesignLogic.SetInnerPageSubHeading(this.Page, "Create Outbound");
            }
        }
        //------------------------ added by durga for getting tenant data on 15/11/2017----------------------//
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
        //------------------------ added by durga for getting WareHose data on 15/11/2017----------------------//
        [WebMethod]
        public static List<DropDownListDataFilter> getWareHouseData()
        {
            try
            {
                OutBoundBL objbl = new OutBoundBL();
                return objbl.getWareHouseDataData();
            }
            catch
            {
                return null;
            }

        }
        //------------------------ added by durga for getting OBD types data on 15/11/2017----------------------//
        [WebMethod]
        public static List<DropDownListDataFilter> getOBDTypesData()
        {
            try
            {
                OutBoundBL objbl = new OutBoundBL();
                return objbl.getOBDTypesData();
            }
            catch
            {
                return null;
            }

        }
       
        //------------------------ added by durga for getting SOlist with availbleQty based on  input Excel sonumbers on 15/11/2017----------------------//
        [WebMethod]
        public static string getSOListForCreateOutbound(OutboundSerachData obj)
        {
            try
            {
                OutBoundBL objbl = new OutBoundBL();
                return DataTableToJSONWithJSONNet(objbl.getSOListForCreateOutbound(obj));
            }
            catch
            {
                return "";
            }

        }

        [WebMethod]
        public static List<SODetails> GetSoInfo(OutboundSerachData obj)
        {
            List<SODetails> lst = new List<SODetails>();
            try
            {
                
                OutBoundBL OBDBL = new OutBoundBL();
                lst = OBDBL.GetSOInfo(obj);
            }
            catch
            {
                return null;
            }
            return lst;
        }


        [WebMethod]
        public static OBDResult CreateOBD(OutboundSerachData obj)
        {
            OutBoundBL BL = new OutBoundBL();
            return BL.CreateOutbound(obj);
        }
        //------------------------ added by durga for creating outbound based input selected sonumbers on 17/11/2017----------------------//
        [WebMethod]
        public static int CreateOutboundForSelectedSonumbers(string obj, int tenantid, int warehouseid, int deliverytypeid)
        {
            cp = HttpContext.Current.User as CustomPrincipal;
            try
            {
                  var table = JsonConvert.DeserializeObject<DataTable>(obj);
           
            DataTable dt=(DataTable)table;
            DataSet ds = new DataSet();
            ds.Tables.Add(dt);
          

            string result;
            using (StringWriter sw = new StringWriter())
            {
                ds.WriteXml(sw);
                result = sw.ToString();
            }
           
            OutBoundBL objbl = new OutBoundBL();
            return objbl.CreateOutboundForSelectedSonumbers(result,tenantid,warehouseid,deliverytypeid,cp.UserID);
            }
            catch(Exception e)
            {
                return -1;
            }
          
        }

        //------------------------ added by durga for converting datatable to JSON object on 17/11/2017----------------------//
        public static string DataTableToJSONWithJSONNet(DataTable table)
        {
           

            string JSONString = string.Empty;
            JSONString = JsonConvert.SerializeObject(table);
            return JSONString;
        }  

    }
}