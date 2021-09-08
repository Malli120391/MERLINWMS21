using MRLWMSC21Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MRLWMSC21.mMaterialManagement
{
    public partial class MGroupMaster : System.Web.UI.Page
    {
        public static CustomPrincipal cp;
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
            cp = (CustomPrincipal)HttpContext.Current.User;
            if (!IsPostBack)
            {
                //======================= Added By M.D.Prasad For View Only Condition ======================//
                DesignLogic.SetInnerPageSubHeading(this.Page, "Material Group");
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
        protected void btnSave_Click(object sender, EventArgs e)
        {
            //string insert_cmd = "insert into MMT_MGroup(MaterialGroup,TenantID,MaterialGroupDesc,CreatedBy) values(" + DB.SQuote(txtGroupCode.Text) + "," + hifTenant.Value + "," + DB.SQuote(txtDescription.Text) + ",1); select @@identity AS N";
            //int result = DB.GetSqlN(insert_cmd);
            //if (result > 0) { lblMsg.Text = "Data inserted Successfully"; }
            cp = HttpContext.Current.User as CustomPrincipal;
            if (Convert.ToInt16(hifTenant.Value) == 0 || hifTenant.Value == null)
            {

                resetError(" Please select Tenant", true);                               
                return;
            }
            if (txtGroupCode.Text==null || txtGroupCode.Text == "")
            {

                resetError("Please enter Group Code", true);
                return;
            }
            if (txtDescription.Text.Trim()==null || txtDescription.Text.Trim() == "")
            {
                resetError("Please enter Description", true);
                return;
            }
           
            if (Convert.ToInt32(hifid.Value) != 0)
            {
                int exist = DB.GetSqlN("select count(MGroupID) as N from MMT_MaterialMaster where isactive=1 and isdeleted=0 and MGroupID=" + hifid.Value);
                if (exist != 0)
                {
                    resetBlkLableError("Cannot Update as Material Group is mapped an Item", true);
                    return;
                }
            }
            try
            {

                string cmd = "EXEC sp_MMT_UpsetMGroup @MGroupID=" + hifid.Value + ",@MaterialGroup=" + DB.SQuote(txtGroupCode.Text) + ",@MaterialGroupDesc=" + DB.SQuote(txtDescription.Text) + ",@TenantID=" + hifTenant.Value + ",@CreatedBy="+cp.UserID+",@IsActive=1,@IsDeleted=0";
                DB.ExecuteSQL(cmd);
                
                resetBlkLableError("Material Group saved successfully", false);
            }
            catch(Exception ex)
            {
                string msg = ex.Message;
                string dup = "duplicate";
                if (msg.Contains(dup))
                {                   
                    resetBlkLableError("duplicate material group", true);
                }
                else
                {
                    lblMsg.ForeColor = Color.Red;
                    lblMsg.Text = ex.Message;
                } 
            }
           
            
            hifid.Value = "0";
    
            //SqlParameter[] parameters = new SqlParameter[7];
            //parameters[0] = new SqlParameter();
            //parameters[0].SqlDbType = SqlDbType.Int;
            //parameters[0].ParameterName = "@MGroupID";
            //parameters[0].Value = hifid.Value;
            //parameters[1] = new SqlParameter();
            //parameters[1].SqlDbType = SqlDbType.VarChar;
            //parameters[1].ParameterName = "@MaterialGroup";
            //parameters[1].Value = txtGroupCode.Text;
            //parameters[2] = new SqlParameter();
            //parameters[2].SqlDbType = SqlDbType.VarChar;
            //parameters[2].ParameterName = "@MaterialGroupDesc";
            //parameters[2].Value = txtDescription.Text;
            //parameters[3] = new SqlParameter();
            //parameters[3].SqlDbType = SqlDbType.Int;
            //parameters[3].ParameterName = "@TenantID";
            //parameters[3].Value = hifTenant.Value;
            //parameters[4] = new SqlParameter();
            //parameters[4].SqlDbType = SqlDbType.Int;
            //parameters[4].ParameterName = "@CreatedBy";
            //parameters[4].Value = 1;
            //parameters[5] = new SqlParameter();
            //parameters[5].SqlDbType = SqlDbType.TinyInt;
            //parameters[5].ParameterName = "@IsActive";
            //parameters[5].Value = 1;
            //parameters[6] = new SqlParameter();
            //parameters[6].SqlDbType = SqlDbType.TinyInt;
            //parameters[6].ParameterName = "@IsDeleted";
            //parameters[6].Value = 0;
            //DB.ExecuteStoredProcedure("sp_MMT_UpsetMGroup",parameters);

        }
         //static string col = "CreatedOn";
        [WebMethod]
        public static List<Materials> GetMaterials()
        {
            cp = HttpContext.Current.User as CustomPrincipal;
            //string cmd = "select m.MGroupID,m.MaterialGroup,m.TenantID,m.MaterialGroupDesc,t.TenantName from MMT_MGroup m join TPL_Tenant t on m.TenantID=t.TenantID where m.IsActive=1"+cp.t order by m."+col+" desc;";
            string cmd = "EXEC USP_GetMaterialGroupdata @AccountID_New="+cp.AccountID+ ",@UserID_New="+cp.UserID+",@TenantID_New=" + cp.TenantID;
            DataSet ds = DB.GetDS(cmd, false);
            DataTable dt = ds.Tables[0];
            List<Materials> materialsList = new List<Materials>();
            foreach (DataRow dr in dt.Rows)
            {
                Materials materials = new Materials();
                materials.MGroupId = dr["MGroupID"].ToString();
                materials.MaterialGroup = dr["MaterialGroup"].ToString();
                materials.TenantId = dr["TenantID"].ToString();
                materials.Description = dr["MaterialGroupDesc"].ToString();
                materials.TenantName = dr["TenantName"].ToString();

                materialsList.Add(materials);
            }
            return materialsList;

        }
        [WebMethod]
        public static List<Materials> EditMaterials(int Id)
        {
            string cmd = "select m.MGroupID,m.MaterialGroup,m.TenantID,m.MaterialGroupDesc,t.TenantName from MMT_MGroup m join TPL_Tenant t on m.TenantID=t.TenantID where m.MGroupID=" + Id;
            DataSet ds = DB.GetDS(cmd, false);
            List<Materials> materialsList = new List<Materials>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                Materials materials = new Materials();
                materials.MGroupId = dr["MGroupID"].ToString();
                materials.MaterialGroup = dr["MaterialGroup"].ToString();
                materials.TenantId = dr["TenantID"].ToString();
                materials.Description = dr["MaterialGroupDesc"].ToString();
                materials.TenantName = dr["TenantName"].ToString();

                materialsList.Add(materials);
            }
            return materialsList;
        }
        public class Materials
        {
            public string MGroupId { get; set; }
            public string MaterialGroup { get; set; }
            public string TenantId { get; set; }
            public string Description { get; set; }
            public string TenantName { get; set; }
        }

        [WebMethod]
        public static string DeleteMaterial(int Id)
        {
            string date = System.DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
            int exist = DB.GetSqlN("select count(MGroupID) as N from MMT_MaterialMaster where isactive=1 and isdeleted=0 and MGroupID=" + Id);
            if (exist != 0)
            {
                return "Exist";
            }
            string delete = "update MMT_MGroup set IsActive=0,IsDeleted=1,DeletedOn='" + date + "' where MGroupID=" + Id;
            DB.ExecuteSQL(delete);
            return "Data Record Deleted Successfully";
        }



        protected void resetError(string error, bool isError)
        {

            ScriptManager.RegisterStartupScript(this, this.GetType(), "test", "openModal();showStickyToast(" + (isError.ToString().ToLower() == "true" ? "false" : "true") + ",\"" + error + "\");", true);

        }


        protected void resetBlkLableError(string error, bool isError)
        {

            ScriptManager.RegisterStartupScript(this, this.GetType(), "test", "showStickyToast(" + (isError.ToString().ToLower() == "true" ? "false" : "true") + ",\"" + error + "\");", true);

        }
    }


}