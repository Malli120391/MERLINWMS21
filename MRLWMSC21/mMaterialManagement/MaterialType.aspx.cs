using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using MRLWMSC21Common;
using System.Text;

namespace MRLWMSC21.mMaterialManagement
{
   
    public partial class MaterialType : System.Web.UI.Page
    {
        public static CustomPrincipal cp = HttpContext.Current.User as CustomPrincipal;
        //public static string connection = ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString;
        public static string connection = System.Web.Configuration.WebConfigurationManager.AppSettings["DBConn"].ToString();

        //======================= Added By M.D.Prasad For View Only Condition ======================//
        public static CustomPrincipal cp1 = HttpContext.Current.User as CustomPrincipal;
        public static String[] UserRole;
        public static String UserRoledat = "";
        public static int UserTypeID;
        //======================= Added By M.D.Prasad For View Only Condition ======================//
        protected void Page_PreInit(object sender, EventArgs e)
        {
            cp1 = HttpContext.Current.User as CustomPrincipal;
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            cp1 = HttpContext.Current.User as CustomPrincipal;
            if (!IsPostBack)
            {
                DesignLogic.SetInnerPageSubHeading(this.Page, "Material Type");
                //======================= Added By M.D.Prasad For View Only Condition ======================//
                UserRole = cp1.Roles;
                UserRoledat = "";
                for (int i = 0; i < UserRole.Length; i++)
                {
                    UserRoledat = UserRoledat + UserRole[i].ToString() + ",";
                }
                UserTypeID = cp1.UserTypeID;
                //======================= Added By M.D.Prasad For View Only Condition ======================//
            }
        }
        [WebMethod]
        public static List<tenant> Getdata(string value)
        {
            List<tenant> list = new List<tenant>();

            try
            {
                SqlConnection con = new SqlConnection(connection);
                con.Open();
                SqlCommand cmd = new SqlCommand("select top(10) TenantName From TPL_Tenant where IsActive=1 and IsDeleted=0 and TenantName like '" + value + "%'", con);
                SqlDataAdapter adpt = new SqlDataAdapter(cmd);
                DataTable ds = new DataTable();
                adpt.Fill(ds);

                for (int i = 0; i < ds.Rows.Count; i++)
                {
                    tenant obj = new tenant(ds.Rows[i]["TenantName"].ToString());
                    list.Add(obj);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return list;
        }
        [WebMethod]
       public static string delete(parameters prsobj)
        {

            //SqlConnection con = new SqlConnection(connection);
            //con.Open();
            string date = System.DateTime.Now.ToString();
            int exist = DB.GetSqlN("select count(MTypeID) as N from MMT_MaterialMaster where isactive=1 and isdeleted=0 and MTypeID=" + prsobj.mtypeID);
            if (exist != 0)
            {
                return "Exist";
               
            }

            StringBuilder sqlqurey = new StringBuilder();
            sqlqurey.Append("update MMT_MType set IsActive=0 , IsDeleted=1  where MTypeID='" + prsobj.mtypeID + "'  ");
            //SqlCommand cmd = new SqlCommand("update MMT_MType set IsActive=0 , IsDeleted=1 where MTypeID='"+ prsobj .mtypeID+ "'  ", con);
            DB.ExecuteSQL(sqlqurey.ToString());
            
            return "1";
        }
        [WebMethod]
        public static string insert(parameters prmtrs)
        {
            if (prmtrs.mtypeID != 0)
            {
                int exist = DB.GetSqlN("select count(MTypeID) as N from MMT_MaterialMaster where isactive=1 and isdeleted=0 and  MTypeID=" + prmtrs.mtypeID);
                if(exist!=0)
                {
                    return "exist";
                }
                }
            string val;
            StringBuilder sqlInsert = new StringBuilder();
            //SqlConnection con = new SqlConnection(connection);
            //con.Open();
            //SqlCommand cmd = new SqlCommand("[dbo].[sp_MMT_UpsetMType]", con);
            //cmd.CommandType = CommandType.StoredProcedure;
            //cmd.Parameters.AddWithValue("@MTypeID", prmtrs.mtypeID);
            //cmd.Parameters.AddWithValue("@MType", prmtrs.Mtype);
            //cmd.Parameters.AddWithValue("@DESCRIPTION", prmtrs.Desc);
            //cmd.Parameters.AddWithValue("@TenantID", prmtrs.tenentId);
            //cmd.Parameters.AddWithValue("@CreatedBy", cp.UserID);
            //cmd.Parameters.AddWithValue("@IsActive", 1);
            //cmd.Parameters.AddWithValue("@IsDeleted", 0);
            sqlInsert.Append("EXEC [dbo].[sp_MMT_UpsetMType]");
            sqlInsert.Append(" @MTypeID=" + prmtrs.mtypeID);
            sqlInsert.Append(",@MType='"+ prmtrs.Mtype + "'" );
            sqlInsert.Append(",@DESCRIPTION='"+ prmtrs.Desc + "'"  );
            sqlInsert.Append(",@TenantID=" + prmtrs.tenentId);
            sqlInsert.Append(",@CreatedBy=" + cp.UserID);
            sqlInsert.Append(",@IsActive=1");
            sqlInsert.Append(",@IsDeleted=0");
            try
            {
                DB.ExecuteSQL(sqlInsert.ToString());
                return "1";
            }
            catch (Exception ex)
            {             
                val= ex.Message;
                return "0";
            }          
            
        }
        [WebMethod]
        public static List<parameters> Getlistdata()
        {
             CustomPrincipal cp1 = HttpContext.Current.User as CustomPrincipal;
            List<parameters> list = new List<parameters>();
            DataSet dt = DB.GetDS("EXEC [dbo].[USP_GetMtypedata] @AccountID_new=" + cp1.AccountID + ",@UserId="+cp1.UserID+", @tenantID_new=" + cp1.TenantID, false);
            foreach (DataRow dr in dt.Tables[0].Rows)
            {
                list.Add(new parameters
                {
                    Mtype = dr["Mtype"].ToString(),
                    Desc = dr["Description"].ToString(),
                    tntname = dr["TenantName"].ToString(),
                    tenentId = dr["TenantID"].ToString(),
                    mtypeID=Convert.ToInt32(dr["mtypeID"])
                });
            }
            return list;
        }

        public class tenant
        {
            public string tenantdata;


            public tenant(string tenantdatadb)
            {
                tenantdata = tenantdatadb;
            }

        }
        public class parameters
        {
            public int mtypeID { get; set; }
            public string tntname { get; set; }
            public string Mtype { get; set; }
            public string Desc { get; set; }
            public string tenentId { get; set; }

            public int createdBy { get; set; }
            public int Isactive { get; set; }
            public int Isdelete { get; set; }
        }
    }
}