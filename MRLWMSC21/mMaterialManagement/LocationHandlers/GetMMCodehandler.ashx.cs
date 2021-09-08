using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Script;
using System.Web.Script.Services;
using System.Data;
using System.Text;
using MRLWMSC21Common;
using System.Web.Script.Serialization;

namespace MRLWMSC21.mMaterialManagement
{
    /// <summary>
    /// Summary description for GetMMCodehandler
    /// </summary>
    public class GetMMCodehandler : IHttpHandler
    {
        public CustomPrincipal cp = HttpContext.Current.User as CustomPrincipal;
        public void ProcessRequest(HttpContext context)
        {

            context.Response.ContentType = "application/json";
            string prefix = context.Request.QueryString["prefix"];
            string choice = context.Request.QueryString["choice"];
            string Tenant = context.Request.QueryString["Tenant"];
            string WHID = context.Request.QueryString["WarehouseID"];

            string json = "";
           
            if (choice == "mcode")
            {
                json = getMMcodes(prefix,Tenant);
            }
            else if (choice == "tenentdata")
            {
                json = getTenantData(prefix,Tenant,WHID);
            }

            else if (choice == "SupplierData")
            {
                json = getSupplierData("", Tenant);
            }

            context.Response.Write(json);
        }

        public string getMMcodes(string prefix, string Tenant)
        {
            List<string> mmt = new List<string>();
            
            //string sCmd = "select distinct TOP 10 MCode +   isnull( ' ` '+ OEMPartNo,'')  AS MCode ,TNT_M.MaterialMasterID from TPL_Tenant_MaterialMaster TNT_M JOIN MMT_MaterialMaster MM ON MM.MaterialMasterID=TNT_M.MaterialMasterID AND MM.IsDeleted=0 AND MM.IsActive=1 where TNT_M.IsActive=1 and TNT_M.IsDeleted=0 AND (0=" + Tenant + " OR TNT_M.TenantID=" + Tenant + ") AND  ( MCode like '" + prefix + "%') ";
            //sCmd = sCmd + " AND TNT_M.TenantID = case when 0 = " + cp.TenantID.ToString() + " then TNT_M.TenantID else " + cp.TenantID.ToString() + " end order by MCode";

            int AccountID = 0, TenantID = 0;
            //1 - Inventrax. it should show all Part numbers
            if (cp.UserTypeID == 2 || cp.UserTypeID == 4)
            {
                AccountID = cp.AccountID;
                TenantID = Tenant == "" ? 0 : Convert.ToInt32(Tenant);
            }
            else if (cp.UserTypeID == 3)
            {
                if (cp.AccountID == null)
                {
                    AccountID = 0;
                }
                else
                {
                    AccountID = cp.AccountID;
                }
                TenantID = cp.TenantID;
            }
            string sCmd = "EXEC  [dbo].[SP_Get_MCodesUnderAccount] @TenantID=" + TenantID + ", @prefix='" + prefix + "', @AccountID=" + AccountID;

            
            IDataReader dataReader = DB.GetRS(sCmd);

            while (dataReader.Read())
            {
                mmt.Add(string.Format("{0}~{1}", dataReader["MCode"], dataReader["MaterialMasterID"]));
            }
            JavaScriptSerializer jsr = new JavaScriptSerializer();
            string json = jsr.Serialize(mmt);
            dataReader.Close();
            return json;
        }
        public string getTenantData(String prefix, String TenantID,string WarehouseID)
        {
            List<string> mmt = new List<string>();
            //string sCmd = "select top 20  TenantID,TenantName CompanyName from TPL_tenant where IsActive=1 AND TenantID<>0  AND  TenantName like '" + prefix + "%'";

            //string sCmd = "select top 20  TenantID,TenantName CompanyName from TPL_tenant where IsActive=1 AND (0=" + TenantID + " OR TenantID=" + TenantID + ") AND TenantID<>0 AND IsDeleted=0 AND TenantName like '" + prefix + "%'";

            //sCmd = sCmd + "AND AccountID = case when 0 ="+ cp.AccountID.ToString() +"then AccountID else "+ cp.AccountID.ToString() +" end";
            string sCmd = "   SELECT TCT.TenantID,TNT.TenantName CompanyName FROM TPL_Tenant_Contract TCT JOIN TPL_Tenant TNT ON TNT.TenantID=TCT.TenantID AND TCT.IsActive=1 AND TCT.IsDeleted=0 AND TNT.IsActive=1 AND TNT.IsDeleted=0 AND FORMAT(GETDATE(),'yyyy-MM-dd') BETWEEN FORMAT(TCT.EffectiveFrom,'yyyy-MM-dd') AND  FORMAT(TCT.EffectiveTo,'yyyy-MM-dd') where WarehouseID='" + WarehouseID + "' AND TenantName like '" + prefix + "%'";

           
            IDataReader dataReader = DB.GetRS(sCmd);

            while (dataReader.Read())
            {
                mmt.Add(string.Format("{0},{1}", dataReader["CompanyName"], dataReader["TenantID"]));
            }
            JavaScriptSerializer jsr = new JavaScriptSerializer();
            string json = jsr.Serialize(mmt);
            dataReader.Close();
            return json;
        }

        public string getSupplierData(String prefix, String Tenant)
        {
            List<string> mmt = new List<string>();
            //string sCmd = "SELECT DISTINCT TOP 10 SUP.SupplierID,SUP.SupplierName FROM TPL_Tenant_Supplier TNT_S JOIN MMT_Supplier SUP ON SUP.SupplierID=TNT_S.SupplierID AND TNT_S.IsDeleted=0 WHERE (0=" + TenantID + " OR TNT_S.TenantID=" + TenantID + ") AND SUP.IsActive=1 AND SUP.IsDeleted=0 AND  SupplierName like '" + prefix + "%'";

            string sCmd = "SELECT DISTINCT SUP.SupplierID,SUP.SupplierName FROM TPL_Tenant_Supplier TNT_S JOIN MMT_Supplier SUP ON SUP.SupplierID=TNT_S.SupplierID AND TNT_S.IsDeleted=0 JOIN TPL_Tenant TNT ON TNT.TenantID=TNT_S.TenantID AND TNT.IsDeleted=0 AND TNT.IsActive=1 WHERE TNT.TenantName ='" + Tenant + "' AND TNT.AccountID=" + cp.AccountID + " AND SUP.IsActive=1 AND SUP.IsDeleted=0 AND SUP.SupplierID>0 AND  SupplierName like '" + prefix + "%'";

            IDataReader dataReader = DB.GetRS(sCmd);

            while (dataReader.Read())
            {
                mmt.Add(string.Format("{0},{1}", dataReader["SupplierName"], dataReader["SupplierID"]));
            }
            JavaScriptSerializer jsr = new JavaScriptSerializer();
            string json = jsr.Serialize(mmt);
            dataReader.Close();
            return json;
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}