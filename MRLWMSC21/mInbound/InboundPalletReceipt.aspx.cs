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
using System.Xml.Serialization;
using System.Xml;
using System.IO;
using System.Reflection;

namespace MRLWMSC21.mInbound
{
    public partial class InboundPalletReceipt : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        [WebMethod]
        public static string GeneratePutawaySuggestionList(dynamic _Obj)
        {

            string dbconn = System.Web.Configuration.WebConfigurationManager.AppSettings["DBConn"].ToString();
            string xml = JsonConvert.DeserializeXmlNode("{\'row\' :" + _Obj + "}", "dataset").InnerXml;
            SqlConnection con = new SqlConnection(dbconn);
            SqlCommand cmd = new SqlCommand("sp_INB_BulkInsert_InwardGSImport", con);
            con.Open();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@xml", xml);
            SqlParameter outparameter = new SqlParameter();
            outparameter.ParameterName = "@RefNumber";
            outparameter.SqlDbType = System.Data.SqlDbType.NVarChar;
            outparameter.Direction = System.Data.ParameterDirection.Output;
            outparameter.Size = 100;
            cmd.Parameters.Add(outparameter);
            cmd.ExecuteNonQuery();
            string refNumber = Convert.ToString(cmd.Parameters["@RefNumber"].Value);
            return refNumber;
        }

    }
}