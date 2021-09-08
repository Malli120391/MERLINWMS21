using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using MRLWMSC21Common;
namespace MRLWMSC21WCF.BL
{
    public class LiveStockHandler
    {
        public UserDataTable GetTenants(BO.LiveStock liveStock)
        {
            UserDataTable usertable = new UserDataTable();
            DataSet ds = DB.GetDS("EXEC [dbo].[Get_Tenants_HHT] @AccountID=" + liveStock.AccountId + ",@WarehouseID=" + liveStock.WarehouseID, false);
            DataTable dt = ds.Tables[0];
            usertable.Table = dt;
            return usertable;

        }
        public UserDataTable GetLiveStockData(BO.LiveStock liveStock)
        {
            UserDataTable usertable = new UserDataTable();
            string dataSource = "dbo.sp_INV_GetLiveStock_HHT @MaterialMaster=" + (liveStock.Mcode != "0" ? DB.SQuote(liveStock.Mcode) : "null") + ",@Location=" + (liveStock.Location != "0" ? DB.SQuote(liveStock.Location) : "null") + ",@BatchNo=" + (liveStock.BatchNo!= "0" ? DB.SQuote(liveStock.BatchNo) : "null") + ",@CartonCode=" + (liveStock.CartonNo != "" ? DB.SQuote(liveStock.CartonNo) : "null" )+ ",@TenantID=" + liveStock.TenantID+ ",@AccountID="+liveStock.AccountId+ ",@WarehouseID="+liveStock.WarehouseID;
            dataSource += "  ,@ProjectRef = " + (liveStock.ProjectNo != "" ? DB.SQuote(liveStock.ProjectNo) : "''") + " ,@Serial = " + (liveStock.SerialNo != "" ? DB.SQuote(liveStock.SerialNo) : "''") + " ,@MFGDate = " + (liveStock.MfgDate != "" ? DB.SQuote(liveStock.MfgDate) : "''") + " ,@EXPDate = " + (liveStock.ExpDate != "" ? DB.SQuote(liveStock.ExpDate) : "''") + " ,@MRP = " + (liveStock.MRP != "" ? DB.SQuote(liveStock.MRP) : "''");
            DataSet ds = DB.GetDS(dataSource, false);
            DataTable dt = ds.Tables[0];
            usertable.Table = dt;
            return usertable;

        }

        public string CheckContainerStock(string CartonNo, string InboundID)
        {
            string resultvalue;
            string sql = "select CartonID AS N from INV_Carton where CartonCode=" + DB.SQuote(CartonNo) + " and IsActive=1 and WareHouseID=(select WarehouseID from INB_RefWarehouse_Details where InboundID=" + InboundID + ")";
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

        // To validate carton in live stock HHT
        public string ValidateCartonLiveStock(string CartonNo, int WarehouseId)
        {
            string resultvalue;
            string sql = "Exec ValidateCarton_HHT @WarehouseId =" + WarehouseId + ",@CartonCode=" +DB.SQuote(CartonNo);
            int result = DB.GetSqlN(sql);
            if (result != 0)
            {
                resultvalue = "1";
            }
            else
            {
                resultvalue = "Not a valid Container.";
            }
            return resultvalue;
        }

    }
}