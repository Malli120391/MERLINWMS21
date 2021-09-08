using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using MRLWMSC21Common;
namespace MRLWMSC21WCF.BL
{
    public class BinTransferHandler
    {
        public UserDataTable GetStorageLocations()
        {
            UserDataTable usertable = new UserDataTable();
            DataSet ds = DB.GetDS("select Code from StorageLocation where ID  in (3,4,5)", false);
            DataTable dt = ds.Tables[0];
            usertable.Table = dt;
            return usertable;

        }
        public BO.TransferBO GetAvailbleQtyList(BO.TransferBO transferBO)
        {
            DataSet dataset;
            
            string query = "EXEC [dbo].[sp_INV_GetAvailableQty_HHT]  @Mcode=" + (transferBO.MCode!=""?DB.SQuote(transferBO.MCode):"''") + ", @SLOC=" + DB.SQuote(transferBO.FromSLoc) + ", @Location=" + DB.SQuote(transferBO.FromLocation) + ",@CartonCode=" + DB.SQuote(transferBO.FromCartonNo) + ",@MfgDate=" + ((transferBO.MfgDate != "") ? DB.SQuote(transferBO.MfgDate) : "''") + ",@ExpDate=" + ((transferBO.ExpDate != "") ? DB.SQuote(transferBO.ExpDate) : "''") + ",@SerialNo=" + ((transferBO.SerialNo != "") ? DB.SQuote(transferBO.SerialNo) : "''") + ",@BatchNo = " + ((transferBO.BatchNo != "") ? DB.SQuote(transferBO.BatchNo) : "''") + ",@ProjectRefNo = " + (((transferBO.ProjectNo != "") ? DB.SQuote(transferBO.ProjectNo) : "''"))+" ";
            query += ",@TenantID=" + transferBO.TenantID + ",@WarehouseID=" + transferBO.WarehouseID+ ",@MRP=" + ((transferBO.MRP != "") ? DB.SQuote(transferBO.MRP) : "''");
            dataset = DB.GetDS(query, false);
            if (dataset != null && dataset.Tables[0] != null && dataset.Tables[0].Rows.Count != 0)
            {
                transferBO.AvailQty = dataset.Tables[0].Rows[0]["AvailableQuantity"].ToString();
                //transferBO.FromSLoc = dataset.Tables[0].Rows[0]["SLOC"].ToString();

            }

            return transferBO;

        }
        public BO.TransferBO UpsertBinToBinTransfer(BO.TransferBO transferBO)
        {

            string drlStatement = "EXEC [dbo].[Upsert_BinToBinTransfer_HHT] @FromLocation=" + DB.SQuote(transferBO.FromLocation) + ",@MfgDate=" + (transferBO.MfgDate != "" ? DB.SQuote(transferBO.MfgDate) : "''") + ",@ExpDate=" + (transferBO.ExpDate != "" ? DB.SQuote(transferBO.ExpDate) : "''") + ",@SerialNo=" + (transferBO.SerialNo != "" ? DB.SQuote(transferBO.SerialNo) : "''") + ",@BatchNo = " + (transferBO.BatchNo != "" ? DB.SQuote(transferBO.BatchNo) : "''") + ",@ProjectRefNo = " + (transferBO.ProjectNo != "" ? DB.SQuote(transferBO.ProjectNo) : "''")  +",@MRP = " + (transferBO.MRP != "" ? DB.SQuote(transferBO.MRP) : "''") + ",@CreatedBy=" + transferBO.UserId + ",@FromCarton=" + DB.SQuote(transferBO.FromCartonNo) + ",@MCODE=" + DB.SQuote(transferBO.MCode) + ",@TLoc=" + DB.SQuote(transferBO.ToLocation) + ",@Quantity=" + Convert.ToDecimal(transferBO.TransferQty) + ",@FromSLOC=" + DB.SQuote(transferBO.FromSLoc) + ",@ToSLOC=" + DB.SQuote(transferBO.ToSLoc) + ",@ToCarton=" + DB.SQuote(transferBO.ToCartonNo)+ ",@TenantID="+transferBO.TenantID+ ",@WarehouseID="+transferBO.WarehouseID;
            int result = DB.GetSqlN(drlStatement);
            if (result == -2)
            {
                transferBO.Result = "Mis Matched container Configuration";
            }
            else if (result == -3)
            {
                transferBO.Result = "Container is Configured to different Loc.";
            }
            else if (result == -4)
            {
                transferBO.Result = "No stock available";
            }
            else if (result == -5)
            {
                transferBO.Result = "Quantity exceeded";
            }
            else
            {
                transferBO.Result = "1";
            }
            return transferBO;

        }


        public string ChekContainerLocation(string cartoncode, string WarehouseID)
        {

            string Location = DB.GetSqlS("EXEC [dbo].[SP_CHECK_CONTAINER_LOCATION_MAPPING]  @CONTAINER=" + DB.SQuote(cartoncode) + ",@WarehouseID=" + WarehouseID);
            return Location;
        }
    }
}