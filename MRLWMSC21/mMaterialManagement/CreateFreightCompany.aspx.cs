using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.Services;
using MRLWMSC21Common;
using System.Text;

namespace MRLWMSC21.mMaterialManagement
{
    public partial class CreateFreightCompany : System.Web.UI.Page
    {
        public static string connect = System.Web.Configuration.WebConfigurationManager.AppSettings["DBConn"].ToString();
        public static CustomPrincipal cp = HttpContext.Current.User as CustomPrincipal;


        protected void Page_PreInit(object sender, EventArgs e)
        {
            cp = HttpContext.Current.User as CustomPrincipal;

        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DesignLogic.SetInnerPageSubHeading(this.Page, "Create Freight Company");
            }

        }
        [WebMethod]
        public static string Insert(FreightCompanycs objcom,int AccountID)
        {
            cp = HttpContext.Current.User as CustomPrincipal;
            int freight = DB.GetSqlN("select count(FR.FreightCompanyID) as N from GEN_FreightCompany FR JOIN  YM_MST_Vehicles VH ON FR.FreightCompanyID = VH.FreightCompanyID and FR.AccountID = VH.AccountID where FR.FreightCompanyID = "+ objcom.FreightCompanyID);
            if(freight!=0)
            {
               return "Exist";
            }
            string val;
            StringBuilder sqlinsert = new StringBuilder();
            sqlinsert.Append("EXEC Sp_GEN_UpsetFreightCompany");
            sqlinsert.Append(" @FreightCompanyID=" + objcom.FreightCompanyID);
            sqlinsert.Append(",@FreightCompany= '"+ objcom.FreightCompany + "'");
            sqlinsert.Append(",@TenantID="+cp.TenantID);
            sqlinsert.Append(",@IsActive=1");
            sqlinsert.Append(",@IsDeleted=0");

            sqlinsert.Append(",@CreatedBy="+ cp.UserID);
            sqlinsert.Append(",@AccountID=" + AccountID);

            sqlinsert.Append(",@FreightCompanyCode= '" + objcom.FreightCompanyCode + "'");

         //   DB.ExecuteSQL(sqlinsert.ToString());
            try
            {
                DB.ExecuteSQL(sqlinsert.ToString());
                return "Success";
            }
            catch (Exception ex)
            {
                val = ex.Message;
                return val;
            }

            //cmd.CommandType = CommandType.StoredProcedure;
            //cmd.Parameters.AddWithValue("@FreightCompanyID", objcom.FreightCompanyID);
            //cmd.Parameters.AddWithValue("@FreightCompany", objcom.FreightCompany);
            //cmd.Parameters.AddWithValue("@TenantID", 1);
            //cmd.Parameters.AddWithValue("@IsActive", 1);
            //cmd.Parameters.AddWithValue("@IsDeleted", 0);
            //cmd.Parameters.AddWithValue("@CreatedBy", cp.UserID);
            //cmd.Parameters.AddWithValue("@FreightCompanyCode", objcom.FreightCompanyCode);
            //cmd.Parameters.AddWithValue("@AccountID ",cp.AccountID);

            //cmd.ExecuteNonQuery();
            return "Success";
        }
        [WebMethod]
        public static string delete(FreightCompanycs obj)
        {
            int freight = DB.GetSqlN("select count(FR.FreightCompanyID) as N from GEN_FreightCompany FR JOIN  YM_MST_Vehicles VH ON FR.FreightCompanyID = VH.FreightCompanyID and FR.AccountID = VH.AccountID where FR.FreightCompanyID = " + obj.FreightCompanyID);
            if (freight != 0)
            {
                return "Exist";
            }
            string date = System.DateTime.Now.ToString();
            StringBuilder sqldel = new StringBuilder();
            sqldel.Append("update GEN_FreightCompany set IsActive=0 ,IsDeleted=1,DeletedOn='"+ date + "' where FreightCompanyID=" + obj.FreightCompanyID + "");
            DB.ExecuteSQL(sqldel.ToString());
            //SqlCommand cmd = new SqlCommand("update GEN_FreightCompany set IsActive=0 ,IsDeleted=1 where FreightCompanyID="+obj.FreightCompanyID+"", con);
           // cmd.ExecuteNonQuery();

            return "";
        }
        [WebMethod]
        public static List<FreightCompanycs> getlist()
        {
            List<FreightCompanycs> list = new List<FreightCompanycs>();
            StringBuilder sqllist = new StringBuilder();
          // sqllist.Append("select FreightCompany,FreightCompanyCode,FreightCompanyID,AccountID from GEN_FreightCompany where IsActive=1 and IsDeleted=0 and AccountID="+cp.AccountID+ " order By FreightCompanyID DESC");
            sqllist.Append("select FreightCompany,FreightCompanyCode,FreightCompanyID,FC.AccountID,ACC.AccountCode,ACC.Account from GEN_FreightCompany FC join gen_Account ACC on ACC.AccountID=FC.AccountID and ACC.IsActive=1 and ACC.IsDeleted=0  WHERE FC.IsActive=1 AND FC.IsDeleted=0 AND FC.AccountID= case when 0 =" + cp.AccountID.ToString() + " then FC.AccountID else " + cp.AccountID.ToString() + " end order By FC.FreightCompanyID DESC");

            // SqlDataAdapter adpt = new SqlDataAdapter(cmd);
            DataSet ds = DB.GetDS(sqllist.ToString(), false);
            //adpt.Fill(ds);

            foreach(DataRow dr in ds.Tables[0].Rows)
            {
                list.Add(new FreightCompanycs
                {
                    FreightCompany = dr["FreightCompany"].ToString(),
                    FreightCompanyCode = dr["FreightCompanyCode"].ToString(),
                    FreightCompanyID = Convert.ToInt32(dr["FreightCompanyID"]),
                    AccountCode=dr["AccountCode"].ToString(),
                    AccountName = dr["Account"].ToString(),
                    AccountID = dr["AccountID"].ToString() == "" ? 0 : Convert.ToInt32(dr["AccountID"])


                });
                

            }
            return list;
        }
        public class FreightCompanycs
        {
            public int FreightCompanyID { get; set; }
             public string FreightCompany { get; set; }
            public string AccountCode { get; set; }
            public string AccountName { get; set; }
            public int TenantID { get; set; }

            public int IsActive { get; set; }
            public int IsDeleted { get; set;}
             public int CreatedBy { get; set; }
             public string FreightCompanyCode { get; set; }
            public int AccountID { get; set; }


          
        }
    }
}