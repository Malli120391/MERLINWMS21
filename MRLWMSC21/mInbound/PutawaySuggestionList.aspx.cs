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
using Newtonsoft.Json;
using System.Data.SqlClient;

namespace MRLWMSC21.mInbound
{
    public partial class PutawaySuggestionList : System.Web.UI.Page
    {
        public static CustomPrincipal cp = HttpContext.Current.User as CustomPrincipal;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //DesignLogic.SetInnerPageSubHeading(this.Page, "Putaway Suggestion List");
            }
        }
        [ScriptMethod]
        [WebMethod]
        public static string getPutawaySuggestionList(int InbID)
        {
            try
            {
                DataSet ds = DB.GetDS("EXEC [dbo].[sp_INV_GetSuggestedPutwayList]  @InboundID=" + InbID , false);
                return JsonConvert.SerializeObject(ds);
            }
            catch (Exception ex)
            {
                return "Error : " + ex;
            }
        }
        [WebMethod]
        public static int SavePutawaySuggestions(dynamic _Obj)
        {
            string dbconn = System.Web.Configuration.WebConfigurationManager.AppSettings["DBConn"].ToString();
            string xml = JsonConvert.DeserializeXmlNode("{\'row\' :" + _Obj + "}", "dataset").InnerXml;
            SqlConnection con = new SqlConnection(dbconn);
            SqlCommand cmd = new SqlCommand("SP_RECEIVING_GOODSCONFIRMATION_INBOUND", con);
            con.Open();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@xml", xml);
            int M = cmd.ExecuteNonQuery();
            return M;
        }
    }
}