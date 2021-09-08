using MRLWMSC21Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MRLWMSC21.mMaterialManagement
{
    public partial class CreateClearanceCompany : System.Web.UI.Page
    {
       
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                DesignLogic.SetInnerPageSubHeading(this.Page, "Create Clearance Company ");
               
            }
        }
        [WebMethod]
        public static string SaveClearance(int ClearanceId,string ClearanceName,string ClearanceCode,int AccountID)
        {
             
            CustomPrincipal cp = (CustomPrincipal)HttpContext.Current.User;
            try
            {
                string cmd = "EXEC SP_Gen_UpsetClearanceCompany @ClearanceCompanyID=" + ClearanceId + ",@AccountID="+ AccountID + ",@ClearanceCompany=" + DB.SQuote(ClearanceName) + ",@TenantID="+cp.TenantID+",@Isactive=1,@IsDeleted=0,@CreatedBy=" + cp.UserID + ",@ClearanceCompanyCode=" +DB.SQuote(ClearanceCode);
                DB.ExecuteSQL(cmd);
                return "1";
                
            }
           catch(Exception ex)
            {
                string msg = ex.Message;
                string dup = "duplicate";
                //if (msg.Contains(dup))
                 return "0";
               
            }
        }
        
        [WebMethod]
        public static string DeleteClearance(int Id)
        {
            string date = System.DateTime.Now.ToString();
            string cmd = "update GEN_ClearanceCompany set IsActive=0,IsDeleted=1,DeletedOn='"+ date + "' where ClearanceCompanyID=" +Id;
            DB.ExecuteSQL(cmd);
            return "Deleted the data Successfully";

        }
        [WebMethod]
        public static List<ClearanceCompany> ClearanceCompany_Data()
        {
            CustomPrincipal cp = (CustomPrincipal)HttpContext.Current.User;
            // string cmd = "SELECT ClearanceCompanyID,ClearanceCompany,ClearanceCompanyCode,ACC.AccountCode FROM GEN_ClearanceCompany CLC join gen_Account ACC on ACC.AccountID=CLC.AccountID and ACC.IsActive=1 and ACC.IsDeleted=0  WHERE CLC.IsActive=1 AND CLC.IsDeleted=0 AND CLC.AccountID=" + cp.AccountID + " ORDER BY CLC.CreatedOn DESC";
            string cmd = "SELECT ClearanceCompanyID,ClearanceCompany,ClearanceCompanyCode,ACC.AccountCode,ACC.Account,CLC.AccountID FROM GEN_ClearanceCompany CLC join gen_Account ACC on ACC.AccountID=CLC.AccountID and ACC.IsActive=1 and ACC.IsDeleted=0  WHERE CLC.IsActive=1 AND CLC.IsDeleted=0 AND CLC.AccountID= case when 0 =" + cp.AccountID.ToString() + " then CLC.AccountID else " + cp.AccountID.ToString() + " end ORDER BY CLC.CreatedOn DESC";
            DataSet ds=DB.GetDS(cmd,false);
            List<ClearanceCompany> listcompany = new List<ClearanceCompany>();
            foreach(DataRow dr in ds.Tables[0].Rows)
            {
                ClearanceCompany ccp = new ClearanceCompany();
                ccp.CompanyId = dr["ClearanceCompanyID"].ToString();
                ccp.CompanyName = dr["ClearanceCompany"].ToString();
                ccp.CompanyCode = dr["ClearanceCompanyCode"].ToString();
                ccp.AccountCode = dr["AccountCode"].ToString();
                ccp.AccountID = Convert.ToInt32(dr["AccountID"].ToString());
                ccp.AccountName = dr["Account"].ToString();
                listcompany.Add(ccp);
            }
            return listcompany;
        }

        //[WebMethod]
        //public static List<ClearanceCompany> EditClearance(int Id)
        //{
        //    string cmd = "select  ClearanceCompany ClearanceCompanyCode from GEN_ClearanceCompany where ClearanceCompanyID=" + Id;
        //    DataSet ds = DB.GetDS(cmd, true);
        //    List<ClearanceCompany> listcompany = new List<ClearanceCompany>();
        //    foreach (DataRow dr in ds.Tables[0].Rows)
        //    {
        //        ClearanceCompany ccp = new ClearanceCompany();
        //        ccp.CompanyName = dr["ClearanceCompany"].ToString();
        //        //ccp.CompanyCode = dr["ClearanceCompanyCode"].ToString();
                
        //        listcompany.Add(ccp);
        //    }
        //    return listcompany;
        //}
        public class ClearanceCompany
        {
            public string CompanyId { set; get; }
            public string CompanyName { set; get; }
            public string CompanyCode { set; get; }
            public string AccountCode { set; get; }
            public string AccountName { set; get; }
            public int AccountID { get; set; }
        }
        protected void resetBlkLableError(string error, bool isError)
        {

            ScriptManager.RegisterStartupScript(this, this.GetType(), "test", "showStickyToast(" + (isError.ToString().ToLower() == "true" ? "false" : "true") + ",\"" + error + "\");", true);

        }
    }
}