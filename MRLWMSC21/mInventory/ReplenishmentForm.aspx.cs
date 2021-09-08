using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Data.SqlClient;
using MRLWMSC21Common;
using System.Data;
using System.Web.Security;
using System.Security.Principal;
using System.Globalization;
using System.Web.Services;
using Newtonsoft.Json;
using System.Configuration;
namespace MRLWMSC21.mInventory
{
    public partial class ReplenishmentForm : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        [WebMethod]
        public static string ReplenishmentFormAdd(dynamic _Obj) 
        {

            string dbconn = System.Web.Configuration.WebConfigurationManager.AppSettings["DBConn"].ToString();
            string xml = JsonConvert.DeserializeXmlNode("{\'row\' :" + _Obj + "}", "dataset").InnerXml;
            SqlConnection con = new SqlConnection(dbconn);
            con.Open();                 
            DataSet ds = DB.GetDS("EXEC SP_INV_ReplenishmentFormPickList @xml="+DB.SQuote(xml), false);
            return JsonConvert.SerializeObject(ds);
            //return ds;
        }
    }
}