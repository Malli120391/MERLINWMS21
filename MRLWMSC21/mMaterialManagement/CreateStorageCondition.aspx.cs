using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using MRLWMSC21Common;
using System.Web.Services;
using System.Text;
namespace MRLWMSC21.mMaterialManagement
{
    public partial class CreateStorageCondition : System.Web.UI.Page
    {  //public static string connect= ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString;
        public static string connect = System.Web.Configuration.WebConfigurationManager.AppSettings["DBConn"].ToString();
        public static CustomPrincipal cp = HttpContext.Current.User as CustomPrincipal;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

               

                DesignLogic.SetInnerPageSubHeading(this.Page, "Storage Condition");
            }
            }
        [WebMethod]
        public static string  insert(storagecs strgobj)
        {
           cp=HttpContext.Current.User as CustomPrincipal;
            if (strgobj.StorageConditionID != 0)
            {
                //List<storagecs> list = new List<storagecs>();
                int exist = DB.GetSqlN("select count(StorageConditionID) as N from MMT_MaterialMaster where isactive=1 and isdeleted=0 and StorageConditionID=" + strgobj.StorageConditionID);
                if (exist != 0)
                {
                    return "exist";
                }
            }

            StringBuilder sqlquery = new StringBuilder();
            sqlquery.Append("EXEC [dbo].[Sp_MMT_UpSet_StorageCondition]");
            sqlquery.Append(" @StorageConditionID=" + strgobj.StorageConditionID);
            sqlquery.Append(",@StorageCondition='" + strgobj.StorageCondition +"'");
            sqlquery.Append(",@TenantID=" + strgobj.TenantID);
            sqlquery.Append(",@IsActive=1");
            sqlquery.Append(",@IsDeleted=0");
            sqlquery.Append(",@CreatedBy=" + cp.UserID);
            sqlquery.Append(",@StorageConditionCode='" + strgobj.StorageConditionCode + "'");
            DB.ExecuteSQL(sqlquery.ToString());

            //cmd.CommandType = CommandType.StoredProcedure;
            //cmd.Parameters.AddWithValue("StorageConditionID", strgobj.StorageConditionID);
            //cmd.Parameters.AddWithValue("@StorageCondition", strgobj.StorageCondition);
            //cmd.Parameters.AddWithValue("@TenantID", strgobj.TenantID);
            //cmd.Parameters.AddWithValue("@IsActive", 1);
            //cmd.Parameters.AddWithValue("@IsDeleted", 0);
            //cmd.Parameters.AddWithValue("@CreatedBy", cp.UserID);
            //cmd.Parameters.AddWithValue("@StorageConditionCode", strgobj.StorageConditionCode);
            //cmd.ExecuteNonQuery();
            return "Success";

        }
        [WebMethod]
        public static List<storagecs> Getlistdata()
        {
            CustomPrincipal cp1 = HttpContext.Current.User as CustomPrincipal;
            List<storagecs> list = new List<storagecs>();          

            string sqlQuery = "EXEC [dbo].[USP_GetStorageconditiondata] @AccountID_New=" + cp1.AccountID + ",@UserID_New="+cp1.UserID+" ,@TenantID_New=" + cp1.TenantID;
            DataSet dt = DB.GetDS(sqlQuery, false);
            //adpt.Fill(dt);
            foreach(DataRow dr in dt.Tables[0].Rows)
            {
                list.Add(new storagecs
                {   StorageConditionID=Convert.ToInt32(dr["StorageConditionID"]),
                    TenantName = dr["TenantName"].ToString(),
                    StorageConditionCode = dr["StorageConditionCode"].ToString(),
                    StorageCondition = dr["StorageCondition"].ToString(),
                    TenantID=Convert.ToInt32(dr["TenantID"])
                });

                
            }
            return list;
        }
        [WebMethod]
        public static string Delete(storagecs obj)
        {
            if (obj.StorageConditionID != 0)
            {
                int exist = DB.GetSqlN("select count(StorageConditionID) as N from MMT_MaterialMaster where isactive=1 and isdeleted=0 and  StorageConditionID = " + obj.StorageConditionID);
                if (exist != 0)
                {
                    return "exist";
                }
            }
            string date = System.DateTime.Now.ToString();
            StringBuilder sqlDel = new StringBuilder();
            sqlDel.Append("Update MMT_StorageCondition set IsActive=0 ,IsDeleted=1,DeletedOn='" + date + "' where StorageConditionID=" + obj.StorageConditionID);
            DB.ExecuteSQL(sqlDel.ToString());
            //SqlCommand cmd = new SqlCommand("Update MMT_StorageCondition set IsActive=0 ,IsDeleted=1 where StorageConditionID="+obj.StorageConditionID+" ",con);
            //cmd.ExecuteNonQuery();
            return "";
        }
        public class storagecs
        {
            public int StorageConditionID { get; set; }
            public string StorageCondition { get; set; }
            public int TenantID { get; set; }

            public int IsActive { get; set; }


            public int IsDeleted { get; set; }

            public int CreatedBy { get; set; }

            public string StorageConditionCode { get; set; }

            public string TenantName { get; set; }



        }
    }
}