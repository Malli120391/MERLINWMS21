using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using MRLWMSC21Common;
using System.Text;

namespace MRLWMSC21WCF.BL
{
    public class TransferHandler
    {
        public UserDataTable GetTransferOrderNos(BO.TransferBO Transferinfo)
        {
            UserDataTable usertable = new UserDataTable();
            DataSet ds = DB.GetDS("EXEC [dbo].[Get_TransfernoList_HHT] @AccountId="+Transferinfo.AccountId, false);
            DataTable dt = ds.Tables[0];
            usertable.Table = dt;
            return usertable;

        }
        public string GetTransferID(BO.TransferBO Transferinfo)
        {
            try
            {
                return GetTransferIdID(Transferinfo).ToString();
            }
            catch (Exception ex)
            {
                return "0";
            }
        }
        public static int GetTransferIdID(BO.TransferBO Transferinfo)
        {
            return DB.GetSqlN("select TransferRequestID as N from INV_TransferRequest where TransferRequestNumber=" + DB.SQuote(Transferinfo.TransferOrderNo) + " AND IsDeleted=0 ");
        }
        public BO.TransferBO CheckItemForPutAway(BO.TransferBO transferBO)
        {
            int result= DB.GetSqlN("EXEC [dbo].[CheckPickedDataForInternalTransfer_HHT]  @TransferRequestID=" + transferBO.TransferOrderId);
            if(result==-1)
            {
                transferBO.Result = "No Item Picked for this Order";

            }
            else
            {
                transferBO.Result = "0";
            }
          
            return transferBO;
        }
        public BO.TransferBO CloseTransferOrder(BO.TransferBO transferinfo)

        {

            StringBuilder sbSqlString = new StringBuilder();
            sbSqlString.Append(" EXEC [dbo].[CloseTransferOrder_HHT]");
            sbSqlString.Append(" @TransferRequestId=" + transferinfo.TransferOrderId);

            int result = MRLWMSC21Common.DB.GetSqlN(sbSqlString.ToString());
            if (result == 0)
            {
                transferinfo.Result = "Failed in Transaction";
            }
            else
            {
                transferinfo.Result = "Transfer Completed successfully";
            }
            return transferinfo;


        }
    }
}