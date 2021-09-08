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

namespace MRLWMSC21.mOutbound
{
    public partial class DispatchList : System.Web.UI.Page
    {
        static List<int> lst = new List<int>();
        public CustomPrincipal cp = HttpContext.Current.User as CustomPrincipal;
        public static CustomPrincipal cp1 = HttpContext.Current.User as CustomPrincipal;
        protected int OutboundID;
        protected OBDTrack thisOOBD;
        public string customerName = "";
        public string TENANTNAME { get; set; }
        protected void Page_PreInit(object sender, EventArgs e)
        {
            //Page.Theme = "DispatchList";
            cp = HttpContext.Current.User as CustomPrincipal;
        }
        protected void Page_Load(object sender, EventArgs e)
        {
      
        }

        [WebMethod]
        public static string GetMaterialDetailsData(string obdid)
        {

            string dbconn = System.Web.Configuration.WebConfigurationManager.AppSettings["DBConn"].ToString();
            SqlConnection con = new SqlConnection(dbconn);
            con.Open();
            try
            {              
                string query = "EXEC [sp_OBD_GetDispatchList]  @OutboundID=" + obdid + "";
                DataSet ds = DB.GetDS(query, false);
                return JsonConvert.SerializeObject(ds);
            }

            catch (Exception ex)
            {
                return "";
            }                  
        }
        [WebMethod]
        public static string DispatchListAdd(dynamic _Obj)
        {

            string dbconn = System.Web.Configuration.WebConfigurationManager.AppSettings["DBConn"].ToString();
            string xml = JsonConvert.DeserializeXmlNode("{\'row\' :" + _Obj + "}", "dataset").InnerXml;
            //SqlConnection con = new SqlConnection(dbconn);
            //SqlCommand cmd = new SqlCommand("SP_OBD_OutBound_Creation_New", con);
            //con.Open();
            //cmd.CommandType = CommandType.StoredProcedure;
            //SqlDataAdapter da = new SqlDataAdapter(cmd);
            //cmd.Parameters.AddWithValue("@xml", xml);
            //DataSet ds = new DataSet();
            //da.Fill(ds);
            //cmd.ExecuteNonQuery();
            //con.Close();
            //return JsonConvert.SerializeObject(ds);
            string query = "EXEC [dbo].[SP_OBD_OutBound_Creation_New]  @xml='" + xml +"'";
            DataSet ds = DB.GetDS(query, false);
            return JsonConvert.SerializeObject(ds);

        }

        [WebMethod]
        public static string BulkRelease(string outboundid, string DockID)

        {
            string Result = "";
            string OutBoundId = outboundid;
            int wareHouseid = GetWarehouseIDByOutbound(OutBoundId);
            if (outboundid != null)
            {
               
                try
                {
                    // string query = "Exec[dbo].[Sp_OBD_ReserveStockForPicking] @OutboundID = " + outboundid + ", @UserID = " + cp1.UserID + ",@DockID=" + DockID;
                    string query = "Exec[dbo].[SP_UPSERT_PICKEDDATA_OUTBOUND]  @OUTBOUNID = " + outboundid + ", @USERID = " + cp1.UserID + ", @DOCKID=" + DockID;
                    DataSet ds = DB.GetDS(query, false);
                    Result = JsonConvert.SerializeObject(ds);
                    int AssgnResult = Convert.ToInt32(ds.Tables[0].Rows[0]["Status"].ToString());

                }
                catch (Exception e)
                {
                    Result = e.Message;
                }
                
            }
            else
            {
                Result = "This OBD is already in Queue";
            }

            return Result;
        }

        [WebMethod]
        public static string PGI(string OutboundID)
        {
            string Result = "";           
            if (OutboundID != null)
            {               
                try
                {
                    // string query = "Exec[dbo].[Sp_OBD_ReserveStockForPicking] @OutboundID = " + outboundid + ", @UserID = " + cp1.UserID + ",@DockID=" + DockID;
                    string query = "EXEC [dbo].[SP_UPSERT_STOCKOUT_PGI_OUTBOUND] @OutboundID = " + OutboundID + "";
                    DataSet ds = DB.GetDS(query, false);
                    Result = JsonConvert.SerializeObject(ds);                   

                }
                catch (Exception e)
                {
                    Result = e.Message;
                }
             
            }
            else
            {
                Result = "PGI IS ALREADY IN PROCESS OR COMPLETED";
            }

            return Result;
        }

        private static int GetWarehouseIDByOutbound(string outboundID)
        {
            try
            {
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append(" select REF_WH.WarehouseID AS N from OBD_Outbound OBD ");
                sb.Append(" JOIN OBD_RefWarehouse_Details REF_WH ON REF_WH.OutboundID = OBD.OutboundID ");
                sb.Append(" WHERE OBD.OutboundID =" + outboundID);
                return DB.GetSqlN(sb.ToString());

            }
            catch (Exception ex)
            {
                return 0;
            }


        }

        public class OBDData
        {
            public string OBDNumber { get; set; }
            public int OutboundID { get; set; }
            public string SONumber { get; set; }
        }

    }
}