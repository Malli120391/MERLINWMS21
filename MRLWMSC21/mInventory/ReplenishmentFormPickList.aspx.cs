using MRLWMSC21Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MRLWMSC21.mInventory
{
    public partial class ReplenishmentFormPickList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        //public DataSet LoadLocations(string prefix)
        //{
        //    string dbconn = System.Web.Configuration.WebConfigurationManager.AppSettings["DBConn"].ToString();
        //    string query = "select LocationID,Location,DisplayLocationCode from INV_Location loc where IsActive=1 and IsDeleted=0 and loc.Location like '" + prefix + "%'";
        //    SqlConnection con = new SqlConnection(dbconn);
        //    con.Open();
        //    SqlDataAdapter da = new SqlDataAdapter(query, con);
        //    da.SelectCommand.ExecuteReader();
        //    DataSet ds = new DataSet();
        //    da.Fill(ds);
        //    return ds;
        //}
        protected void lnkConfirmMovement_Click(object sender, EventArgs e)
        {

        }

        [WebMethod]
        public static string ReplenishmentFormSave(dynamic _Obj)
        {

            string dbconn = System.Web.Configuration.WebConfigurationManager.AppSettings["DBConn"].ToString();
            string xml = JsonConvert.DeserializeXmlNode("{\'row\' :" + _Obj + "}", "dataset").InnerXml;
            SqlConnection con = new SqlConnection(dbconn);
            con.Open();
            // DataSet ds = DB.GetDS( );
            return "";
            
        }
    }
}