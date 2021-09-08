using MRLWMSC21Common;
using MRLWMSC21.mManufacturingProcess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MRLWMSC21.mMaterialManagement
{
    public partial class BOM : System.Web.UI.Page
    {
        public CustomPrincipal cp = HttpContext.Current.User as CustomPrincipal;
        public static CustomPrincipal cp1 = HttpContext.Current.User as CustomPrincipal;
        protected void Page_PreInit(object sender, EventArgs e)
        {
            cp1 = HttpContext.Current.User as CustomPrincipal;
            cp = HttpContext.Current.User as CustomPrincipal;
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            DesignLogic.SetInnerPageSubHeading(this.Page, "BOM Creation");
            cp1 = HttpContext.Current.User as CustomPrincipal;
        }
        [WebMethod]
        public static List<DropDownData> GetAccounts()
        {
            List<DropDownData> olst = new List<DropDownData>();
            DataSet ds = DB.GetDS("select accountid,account from GEN_Account where 0=" + cp1.AccountID + " or accountid=" + cp1.AccountID,false);
            try
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    olst.Add(new DropDownData()
                    {
                        ID = Convert.ToInt32(row["accountid"]),
                        Name = (row["account"]).ToString()

                    });
                }
            }
            catch (Exception e)
            {
            }
            return olst;
        }
        [WebMethod]
        public static int  GetCurrentAccount()
        {
            return cp1.AccountID;
        }

        [WebMethod]
        public static int UpsertBOM(BOMData obj)
        {
            try
            {
                int res = DB.GetSqlN("exec [SP_UpsertBOMHeader] @BOMHeaderId=" + obj.BOMID + ",@TenantId=" + obj.TenantId + ",@MaterialMasterId=" + obj.MMID + ",@MaterialMaster_UOMId=" + obj.UOMID + ",@Remarks=" +DB.SQuote(obj.Remarks) + ",@userid=" + cp1.UserID);
                return res;
            }
            catch(Exception e)
            {
                return 0;
            }
           
        }
        [WebMethod]
        public static int CheckBOMWithJOBHeader(string bomid)
        {
            try
            {
                int res = DB.GetSqlN("select count(*) as N from MMT_KitJobOrderHeader where BOMRefID="+bomid);
                return res;
            }
            catch (Exception e)
            {
                return 0;
            }

        }
        [WebMethod]
        public static BOMData GetBOMHeaderData(int bomid)
        {
            try
            {
                DataSet ds = DB.GetDS("[SP_GetBOMHeaderData] @BOMHeaderId=" + bomid, false);
                DataTable dtPrimaryInfo = ds.Tables[0];
                BOMData obj = new BOMData();
                if (dtPrimaryInfo.Rows.Count > 0)
                {
                    obj.BOMRefNo = dtPrimaryInfo.Rows[0]["BOMRefNo"].ToString();
                    obj.TenantId = dtPrimaryInfo.Rows[0]["TenantID"].ToString();
                    obj.Tenant = dtPrimaryInfo.Rows[0]["TenantName"].ToString();
                    obj.MCode = dtPrimaryInfo.Rows[0]["mcode"].ToString();
                   obj.MMID = dtPrimaryInfo.Rows[0]["MaterialMasterID"].ToString();
                   obj.UOM = dtPrimaryInfo.Rows[0]["UOM"].ToString();
                    obj.UOMID = dtPrimaryInfo.Rows[0]["MaterialMaster_UOMId"].ToString();
                    obj.Remarks = dtPrimaryInfo.Rows[0]["remarks"].ToString();
                    obj.BOMID = dtPrimaryInfo.Rows[0]["BOMHeaderId"].ToString();
                    obj.AccountId = dtPrimaryInfo.Rows[0]["AccountID"].ToString();
                    obj.Account = dtPrimaryInfo.Rows[0]["AccountCode"].ToString();
                }
                return obj;
            }
            catch (Exception e)
            {
                return null;
            }
            
        }
        [WebMethod]
        public static List<BOMDetailsData> GetBOMDetails(int objId)
        {
            List<BOMDetailsData> olst = new List<BOMDetailsData>();
            try
            {
                DataSet ds = DB.GetDS("exec [dbo].[SP_GetBOMdetailsData] @BOMHeaderId=" + objId, false);
                
               
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        olst.Add(new BOMDetailsData()
                        {
                            BOMID = (row["bomheaderid"]).ToString(),
                            BOMDetailsID= (row["BOMDetailsId"]).ToString(),
                            MMID = (row["MaterialMasterId"]).ToString(),
                            MCode = (row["MCode"]).ToString(),
                            UOMID = (row["MaterialMaster_UoMID"]).ToString(),
                            UOM = (row["UOM"]).ToString(),
                            Quantity= (row["bomquantity"]).ToString(),
                            UPdatedQuantity= (row["bomquantity"]).ToString(),
                        });
                    }
               
            }
            catch (Exception e)
            {
                return null;
            }
            return olst;

        }
        [WebMethod]
        public static int UpsertBOMDetails(BOMDetailsData obj)
        {
            try
            {
                if(DB.GetSqlN("select count(*) as N from MMT_KitJobOrderHeader where BOMRefID=" + obj.BOMID)>0)
                {
                    return -222;
                }
                
                int createdid = DB.GetSqlN("exec [dbo].[sp_MFG_UpsertBoMDetailsForPhantomKit] @BOMDetailsID="+obj.BOMDetailsID+",@BOMHeaderID=" + obj.BOMID + ",@BOMMaterialMasterID=" + obj.MMID + ",@MaterialMaster_BOMUoMID=" + obj.UOMID + ",@BOMQuantity=" + obj.Quantity + ",@CreatedBy=" + cp1.UserID);
                return createdid;
            }
            catch (Exception e)
            {
                return 0;
            }
           
        }
        [WebMethod]
        public static int DeleteBOMDetails(int bomdetailsid,string bomid)
        {
            try
            {
                if (DB.GetSqlN("select count(*) as N from MMT_KitJobOrderHeader where BOMRefID=" + bomid) > 0)
                {
                    return -222;
                }
                DB.ExecuteSQL("update BOMDetails set Isdeleted=1 where BOMDetailsId=" + bomdetailsid);
                return 1;
            }
            catch (Exception e)
            {
                return 0;
            }
            
        }
        public class DropDownData
        {
            public int ID { get; set; }
            public String Name { get; set; }

        }
        public class BOMData
        {
            public int SNO { get; set; }
            public string BOMID { get; set;   }
            public string BOMRefNo  {get;set;  }  
            public string TenantId  {get;set;  } 
            public string Tenant  {get;set;  }  
            public string AccountId  {get;set;  }  
            public string Account  {get;set;  }  
            public string MMID  {get;set;  }  
            public string MCode  {get;set;  } 
            public string UOMID  {get;set;  }  
            public string UOM  {get;set;  }  
            public string Remarks  {get;set;  }
            public string CreatedDate { get; set; }
            public string CreatedUser { get; set; }
        }
        public class BOMDetailsData
        {
             public string BOMID {get;set;}
            public string BOMDetailsID { get; set; }
            public string MMID {get;set;}
           public string MCode {get;set;} 
           public string UOMID {get;set;} 
           public string UOM {get;set;} 
           public string Quantity {get;set;}
            public string UPdatedQuantity { get; set; }
        }
     }

}