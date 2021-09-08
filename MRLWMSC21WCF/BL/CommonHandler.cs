using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using MRLWMSC21Common;

namespace MRLWMSC21WCF.BL
{
    public class CommonHandler
    {
        public UserDataTable GetSkipReasonList(string type)
        {
            UserDataTable usertable = new UserDataTable();
            DataSet ds = DB.GetDS("EXEC [dbo].[Get_SkipReasonList_HHT] @TypeId="+Convert.ToInt32(type), false);
            DataTable dt = ds.Tables[0];
            usertable.Table = dt;
            return usertable;
        }
        public string CheckContainer(string CartonNo,string InboundID)
        {
            string resultvalue;
            string sql = "select CartonID AS N from INV_Carton where CartonCode=" + DB.SQuote(CartonNo) + " and IsActive=1 and WareHouseID=(select WarehouseID from INB_RefWarehouse_Details where InboundID=" + InboundID+")";
            int result = DB.GetSqlN(sql);
            if (result != 0)
            {
                resultvalue = "";
            }
            else
            {
                resultvalue = "Not a valid Container.";
            }
            return resultvalue;
        }
        public string CheckLoction(BO.LiveStock stock)
        {
            string resultvalue;
            int result = DB.GetSqlN("EXEC [dbo].[CheckLocation_HHT]   @Location=" + DB.SQuote(stock.Location) + ",@AccountId=" + stock.AccountId+ ",@WarehouseID="+stock.WarehouseID);
            if (result == 0)
            {
                resultvalue = "Location doesn't belong to this Warehouse ";
            }
            else
            {

                resultvalue = "";
            }

            return resultvalue;


            //string resultvalue;
            //int result = DB.GetSqlN("select LocationID   AS N from INV_Location where Location=" + DB.SQuote(Location) + " and IsActive=1");
            //if (result != 0)
            //{
            //    resultvalue = "";
            //}
            //else
            //{
            //    resultvalue = "Not a valid Location.";
            //}
            //return resultvalue;
        }
        public string CheckTenatMaterial(string Mcode,int AccountID,string TenantName)
        {
            string resultvalue;
            // int result = DB.GetSqlN("select MaterialMasterID AS N from MMT_MaterialMaster where TenantID="+0+"and Mcode="+DB.SQuote(Mcode)+" and IsActive=1 and IsDeleted=0");

            int result = DB.GetSqlN("EXEC [dbo].[CheckTenantMaterial_HHT]   @MCODE=" + DB.SQuote(Mcode) + ", @ACCOUNTID=" + AccountID + ", @TENANTNAME=" + DB.SQuote(TenantName));
            if (result != 0)
            {
                resultvalue = "";
            }
            else
            {
                resultvalue = "Material doesn't belong to this Tenant";
            }
            return resultvalue;
        }
        public string CheckStrictlyCompliance(BO.ValidateUserLogin validateUserLogin)
        {
            string resultvalue;
            int result = DB.GetSqlN("EXEC [dbo].[CheckAccount_StriclyCompliance_HHT]   @ACCOUNTID=" + validateUserLogin.AccountId + ", @UserId=" + validateUserLogin.Apps_MST_User_ID);
            if(result>0)
            {
                resultvalue = "1";
            }
            else
            {
                resultvalue = "0";
            }
            return resultvalue;
        }


        public UserDataTable GetWarehouse(BO.LiveStock liveStock)
        {
            UserDataTable usertable = new UserDataTable();
            DataSet ds = DB.GetDS("EXEC [dbo].[USP_MST_DropWH] @AccountID=" + liveStock.AccountId + ",@TenantID=" + liveStock.TenantID + ",@UserID=" + liveStock.UserId + ",@Flag=2", false);
            DataTable dt = ds.Tables[0];
            usertable.Table = dt;
            return usertable;

        }

    }
}