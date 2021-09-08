using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MRLWMSC21Common;
using Newtonsoft.Json;
using System.Web.Script.Services;
using System.Web.Services;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using System.Xml;
using System.Reflection;
using ClosedXML.Excel;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace MRLWMSC21.mMaterialManagement
{
    public partial class attributesDetails : System.Web.UI.Page
    {
        public CustomPrincipal cp = HttpContext.Current.User as CustomPrincipal;
        protected void Page_PreInit(object sender, EventArgs e)
        {
            Page.Theme = "Orders";//_sit";
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DesignLogic.SetInnerPageSubHeading(this.Page, "Customer");         
            }
        }


        [WebMethod]
        public static string GetList(int Id)
        {
            CustomPrincipal cp = HttpContext.Current.User as CustomPrincipal;
            DataSet ds = DB.GetDS("EXEC [dbo].[USP_GET_Attributes] @AccountID=" + cp.AccountID + ",@AttributeID=0", false);
            return JsonConvert.SerializeObject(ds);
            //SET SP ewms.USP_SET_CCM_CNF_AccountCycleCounts
        }

        [WebMethod]
        public static string GetEditData(int AttributeID)
        {
            CustomPrincipal cp = HttpContext.Current.User as CustomPrincipal;
            DataSet ds = DB.GetDS("EXEC [dbo].[USP_GET_Attributes_Edit] @AccountID=" + cp.AccountID + ",@AttributeID=" + AttributeID, false);
            return JsonConvert.SerializeObject(ds);

        }

      
        [WebMethod]
        public static string MasterDetailsSet(string Sp_Set, string JSON)
        {
            Sp_Set = "[dbo].[USP_SET_Attributes]";
            Dictionary<dynamic, dynamic> values = JsonConvert.DeserializeObject<Dictionary<dynamic, dynamic>>(JsonConvert.DeserializeObject(JSON).ToString());

            StringBuilder sb = new StringBuilder();
            int count = 0;
            foreach (KeyValuePair<dynamic, dynamic> pair in values)
            {
                if (count != 0)
                    sb.Append(",");
                sb.Append(pair.Key + "=" + DB.SQuote(pair.Value));
                count++;
            }
            DB.ExecuteSQL("EXEC " + Sp_Set + " " + sb.ToString());
            return "";
        }

        [WebMethod]
        public static string MasterDetailsSetIndustry(string Sp_Set, string JSON)
        {
            Sp_Set = "[dbo].[USP_SET_CNF_Industry_Attributes]";
            Dictionary<dynamic, dynamic> values = JsonConvert.DeserializeObject<Dictionary<dynamic, dynamic>>(JsonConvert.DeserializeObject(JSON).ToString());

            StringBuilder sb = new StringBuilder();
            int count = 0;
            foreach (KeyValuePair<dynamic, dynamic> pair in values)
            {
                if (count != 0)
                    sb.Append(",");
                sb.Append(pair.Key + "=" + DB.SQuote(pair.Value));
                count++;
            }
            DB.ExecuteSQL("EXEC " + Sp_Set + " " + sb.ToString());
            return "";
        }

        [WebMethod]
        public static string MasterDetailsSetLookup(string Sp_Set, string JSON)
        {
            Sp_Set = "[dbo].[USP_SET_Attributes_Lookup]";
            Dictionary<dynamic, dynamic> values = JsonConvert.DeserializeObject<Dictionary<dynamic, dynamic>>(JsonConvert.DeserializeObject(JSON).ToString());

            StringBuilder sb = new StringBuilder();
            int count = 0;
            foreach (KeyValuePair<dynamic, dynamic> pair in values)
            {
                if (count != 0)
                    sb.Append(",");
                sb.Append(pair.Key + "=" + DB.SQuote(pair.Value));
                count++;
            }
            DB.ExecuteSQL("EXEC " + Sp_Set + " " + sb.ToString());
            return "";
        }

        [WebMethod]
        public static string DeleteItemsByIdIndustry(string SP_Del, string ID)
        {
            SP_Del = "[dbo].[USP_Delete_IndustryAttributeDetails]";
            CustomPrincipal cp1 = HttpContext.Current.User as CustomPrincipal;
            StringBuilder sb = new StringBuilder();

            string date = System.DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");

            sb.Append(" @PK=" + ID + ", @LoggedInUserID=" + cp1.UserID + ", @UTCTimestamp=" + DB.SQuote(date));
            //DB.ExecuteSQL("EXEC " + SP_Del + " " + sb.ToString());
            //return 0;
            DataSet ds = DB.GetDS("EXEC " + SP_Del + " " + sb.ToString(), false);
            return JsonConvert.SerializeObject(ds);

        }

        [WebMethod]
        public static string DeleteItemsByIdLookup(string SP_Del, string ID)
        {
            SP_Del = "[dbo].[USP_Delete_LookupAttributeDetails]";
            CustomPrincipal cp1 = HttpContext.Current.User as CustomPrincipal;
            StringBuilder sb = new StringBuilder();

            string date = System.DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");

            sb.Append(" @PK=" + ID + ", @LoggedInUserID=" + cp1.UserID + ", @UTCTimestamp=" + DB.SQuote(date));
            //DB.ExecuteSQL("EXEC " + SP_Del + " " + sb.ToString());
            //return 0;
            DataSet ds = DB.GetDS("EXEC " + SP_Del + " " + sb.ToString(), false);
            return JsonConvert.SerializeObject(ds);

        }
    }
}