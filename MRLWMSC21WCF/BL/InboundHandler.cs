using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using MRLWMSC21Common;

namespace MRLWMSC21WCF.BL
{
    public class InboundHandler
    {
      public UserDataTable GetStorerefnos(BO.Inbound inbound)
        {
            UserDataTable usertable = new UserDataTable();
            DataSet ds = DB.GetDS("EXEC [dbo].[Get_StoreRefnoList_HHT] @AccountId=" + inbound.AccountId + ",@UserID=" + inbound.UserId, false);
            DataTable dt = ds.Tables[0];
            usertable.Table = dt;
            return usertable;

        }

        public UserDataTable GetDockVehicles(int inbound)
        {
            UserDataTable usertable = new UserDataTable();
            DataSet ds = DB.GetDS("EXEC INB_GetDockVehicles @InboundID="+ inbound, false);
            DataTable dt = ds.Tables[0];
            usertable.Table = dt;
            return usertable;

        }

        public UserDataTable GetStorageLocations()
        {
            UserDataTable usertable = new UserDataTable();
            DataSet ds = DB.GetDS("select * from StorageLocation where ID  in (1,3,4,5)", false);
            DataTable dt = ds.Tables[0];
            usertable.Table = dt;
            return usertable;

        }
        public UserDataTable GetPoInvoiceNos(BO.Inbound inbound)
        {
            UserDataTable usertable = new UserDataTable();
            DataSet ds = DB.GetDS("EXEC [dbo].[sp_INB_POInvoiceNos_HHT] @StoreRefNo="+DB.SQuote(inbound.Storerefno) + ",@PONO=" + DB.SQuote(inbound.PONumber), false);
            DataTable dt = null;
            if (ds != null)
                dt = ds.Tables[0];
                usertable.Table = dt;
                return usertable;
          
        }
        public UserDataTable GetPoNos(BO.Inbound inbound)
        {
            UserDataTable usertable = new UserDataTable();
            DataSet ds = DB.GetDS("EXEC [dbo].[sp_INB_GET_PONos_HHT] @StoreRefNo=" + DB.SQuote(inbound.Storerefno) + ",@Mcode=" + DB.SQuote(inbound.MCode) + ",@LineNo=" + DB.SQuote(inbound.Lineno), false);
            DataTable dt = null;
            if (ds != null)
                dt = ds.Tables[0];
            usertable.Table = dt;
            return usertable;

        }
        public BO.Inbound GetInboundNo(BO.Inbound inbound)
        {
            try
            {
                inbound =GetInboundID(inbound);
                inbound = GetInvoiceQty(inbound);
                return inbound;
            }
            catch (Exception ex)
            {
                return inbound;
            }
        }
        public static BO.Inbound GetInboundID(BO.Inbound inbound)
        {
            int id = 0;
            id =  DB.GetSqlN("select InboundID AS N from INB_Inbound where StoreRefNo=" + DB.SQuote(inbound.Storerefno));
            inbound.InboundId = id.ToString() ;
            return inbound;
        }

        public BO.Inbound GetInvoiceQty(BO.Inbound inbound)
        {
             DataSet ds = DB.GetDS("EXEC [dbo].[Get_InboundInvoiceQty_HHT] @Storerefno=" + DB.SQuote(inbound.Storerefno),false);
            if (ds.Tables[0].Rows.Count != 0)
            {
                inbound.InvoiceQty= Convert.ToDecimal(ds.Tables[0].Rows[0]["InvoiceQuantity"].ToString());
            }
                return inbound;
        }

        public BO.Inbound UpdateReceiveItem(BO.Inbound inbound)
        {

            //int response = CheckQuantity(inbound.AccountId, inbound.InboundId, inbound.MCode); commented by lalitha on 04/03/2019
            //inbound = CheckQuantity(inbound);
            inbound.Result = "0";
            if (inbound.Result == "0")
            {
                //if (inbound.ItemPendingQty >= Convert.ToDecimal(inbound.Qty))
                //  {

                MRLWMSC21Common.GoodsInDTO goodsInDTO = new GoodsInDTO();
                goodsInDTO.Location = inbound.Dock;
                goodsInDTO.DocumentQuantity = inbound.Qty;
                goodsInDTO.ConvertedQuantity = inbound.Qty.ToString();
                goodsInDTO.InboundID = inbound.InboundId;
                goodsInDTO.LineNumber = inbound.Lineno;
                goodsInDTO.MCode = inbound.MCode;
                goodsInDTO.LoggedinUserID = Convert.ToInt32(inbound.CreatedBy);
                goodsInDTO.CartonCode = inbound.CartonNo;
                goodsInDTO.StorageLocation = inbound.SLoc;
                goodsInDTO.MfgDate = inbound.MfgDate;
                goodsInDTO.ExpDate = inbound.ExpDate;
                goodsInDTO.MRP = inbound.MRP;
                goodsInDTO.SerialNumber = inbound.SerialNo;
                goodsInDTO.BatchNumber = inbound.BatchNo;
                goodsInDTO.ProjectRefNo = inbound.ProjectNo;
                goodsInDTO.SupplierInvoiceId = inbound.SupplierInvoiceID;
                goodsInDTO.PoHeaderID = inbound.POSOHeaderId;
                goodsInDTO.IsRequestFromPC = 0;
                goodsInDTO.HUNo = inbound.HUNo;
                goodsInDTO.HUsize = inbound.HUSize;
                goodsInDTO.Storerefno = inbound.Storerefno;
                string Result = MRLWMSC21Common.InventoryCommonClass.ReceiveItem(goodsInDTO);
                //   int result = InventoryCommonClass.UpdateGoodsMovementDetails_new(inbound.Dock, inbound.Qty, inbound.Qty, inbound.InboundId, inbound.Lineno, inbound.MCode, inbound.IsDam, inbound.HasDisc, inbound.CreatedBy, inbound.CartonNo, inbound.SLoc, inbound.CreatedBy, inbound.MfgDate, inbound.ExpDate, inbound.SerialNo, inbound.BatchNo, inbound.ProjectNo, "0", "0", "", "0","0",inbound.MRP,inbound.VehicleNo, inbound.SupplierInvoiceDetailsId);
                if (Result == "1")
                {

                    inbound.Result = "Success";
                    CheckItem(inbound);
                    GetReceivedQty(inbound);
                    return inbound;
                }
                else
                {
                    //inbound.Result = "Process failed,Please Contact Support team";
                    inbound.Result = Result;
                    return inbound;
                }

            }
            else if (inbound.Result == "1")
            {
                inbound.Result = "Quantity exceed";
            }
            return inbound;

        }

        public void CheckItem(BO.Inbound inbound)
        {
           DB.GetSqlN("EXEC [dbo].[SP_ChangeGateEntryStatus] @ShipmentId =" + inbound.InboundId + ",@IsForInbound =1");
        }


        public BO.Inbound CheckDock(BO.Inbound inbound)
        {

            inbound.Result= DB.GetSqlN("EXEC [dbo].INB_CheckDock_HHT @Location =" + inbound.Dock + ",@InboundID="+inbound.InboundId).ToString();
            return inbound;
        }
        public BO.Inbound GetReceivedQty(BO.Inbound inbound)
        {
            if (CheckValidItem(inbound))
            {
                string strQuery = "EXEC [dbo].[Get_ReceivedQty_HHT] @INBOUNDID=" + inbound.InboundId + ",@Mcode=" + DB.SQuote(inbound.MCode) + ",@SupplierInvoiceDetailsId = "+inbound.SupplierInvoiceDetailsId+", @HUNO = "+inbound.HUNo+",@LineNo = " + DB.SQuote(inbound.Lineno);
                DataSet ds = DB.GetDS(strQuery, false);
                if (ds.Tables[0].Rows.Count != 0)
                {
                    inbound.ReceivedQty = Convert.ToDecimal(ds.Tables[0].Rows[0]["ReceivedQty"].ToString());
                }
                inbound = GetPendingLineItemQty(inbound);
                return inbound;
            }
            else
            {
                inbound.Result = "invalid";
                return inbound;
            }
        }

        public bool CheckValidItem(BO.Inbound inbound)
        {
            int count;
            string sql = "EXEC INB_ValidSKU_GoodsIn  @INBOUNDID=" + inbound.InboundId + ",@Mcode=" + DB.SQuote(inbound.MCode) + ", @LineNo = " + DB.SQuote(inbound.Lineno) + ", @MfgDate = " + (inbound.MfgDate != "" ? DB.SQuote(inbound.MfgDate) : "''") + ", @ExpDate = " + (inbound.ExpDate != "" ? DB.SQuote(inbound.ExpDate) : "''") + ", @SerialNo = " + (inbound.SerialNo != "" ? DB.SQuote(inbound.SerialNo) : "''") + ", @BatchNo = " + (inbound.BatchNo != "" ? DB.SQuote(inbound.BatchNo) : "''") + ", @ProjectRefNo = " + (inbound.ProjectNo != "" ? DB.SQuote(inbound.ProjectNo) : "''") + ", @MRP = " + (inbound.MRP != "" ? DB.SQuote(inbound.MRP) : "''")+"";
            count = DB.GetSqlN(sql);
            return (count != 0) ? true : false;
        }
        public BO.Inbound GetPendingLineItemQty(BO.Inbound inbound)
        {
            DataSet ds = DB.GetDS("EXEC [dbo].[Get_InboundMaterialPendingQty_HHT] @Storerefno=" + inbound.InboundId + ",@Mcode=" + DB.SQuote(inbound.MCode) + ",@LineNo=" + DB.SQuote(inbound.Lineno), false);
            if (ds.Tables[0].Rows.Count != 0)
            {
                inbound.ItemPendingQty = Convert.ToDecimal(ds.Tables[0].Rows[0]["LineItemInvoiceQty"].ToString());
            }

            return inbound;
        }
        public Boolean CheckMaterialStorageParameters(BO.Inbound inbound)
        {
            List<string> MaterialStorageParameterList = new List<string>();
            DataSet dataSet = DB.GetDS("EXEC [dbo].[sp_Check_MaterialStorageParameters_HHT] @MCode=" + DB.SQuote(inbound.MCode), false);
            if (dataSet.Tables[0].Rows.Count != 0)
            {                
                foreach (DataRow dr in dataSet.Tables[0].Rows)
                {
                    MaterialStorageParameterList.Add(dr[0].ToString());
                }
            }

            var Mspvalues = inbound.Msps.Split(',');
            
            foreach ( var Msps in  MaterialStorageParameterList)
            {
                if (Mspvalues.Contains(Msps))
                {
                    return true;
                }
             
            }
            return false;
        }


        // added by lalitha on 01/03/2019 

        public  BO.Inbound CheckQuantity(BO.Inbound inbound)
        {
            int TenantId = DB.GetSqlN("select TenantID as N from INB_Inbound where InboundID="+ inbound.InboundId + " and IsActive=1 and IsDeleted=0");

            //int MMId = DB.GetSqlN("select MaterialMasterID AS N from MMT_MaterialMaster where MCode="+DB.SQuote(Mcode) +" and TenantID="+ TenantId + " and IsActive=1 and IsDeleted=0");

            // string  RecieveQty = DB.GetSqlS("select  isnull( CONVERT(Nvarchar(50), sum(Quantity)),0) AS S from INV_GoodsMovementDetails where MaterialMasterID= (select MaterialMasterID  from MMT_MaterialMaster where MCode=" + DB.SQuote(inbound.MCode) + " and TenantID=" + TenantId + " and IsActive=1 and IsDeleted=0) and IsActive=0 and IsDeleted=0 and TransactionDocID="+inbound.InboundId+"");

            string RecieveQty="";
            DataSet ds = DB.GetDS("EXEC [dbo].[Get_ReceivedQty_HHT] @INBOUNDID=" + inbound.InboundId + ",@Mcode=" + DB.SQuote(inbound.MCode) + ", @LineNo = " + DB.SQuote(inbound.Lineno) + ", @MfgDate = " + (inbound.MfgDate != "" ? DB.SQuote(inbound.MfgDate) : "''") + ", @ExpDate = " + (inbound.ExpDate != "" ? DB.SQuote(inbound.ExpDate) : "''") + ", @SerialNo = " + (inbound.SerialNo != "" ? DB.SQuote(inbound.SerialNo) : "''") + ", @BatchNo = " + (inbound.BatchNo != "" ? DB.SQuote(inbound.BatchNo) : "''") + ", @ProjectRefNo = " + (inbound.ProjectNo != "" ? DB.SQuote(inbound.ProjectNo) : "''")+ ",@MRP="+(inbound.MRP != "" ? DB.SQuote(inbound.MRP) : "''"), false);
            if (ds.Tables[0].Rows.Count != 0)
            {
                RecieveQty = ds.Tables[0].Rows[0]["ReceivedQty"].ToString();
            }

            // string TotalQty = DB.GetSqlS ("select isnull( CONVERT(Nvarchar, sum(InvoiceQuantity)),0) AS S from ORD_SupplierInvoiceDetails where PODetailsID = (select PODetailsID from ord_PODetails where POHeaderId = (select distinct POHeaderId from INB_Inbound_ORD_SupplierInvoice  where inboundId = "+inbound.InboundId+") and MaterialMasterID = (select MaterialMasterID from MMT_MaterialMaster where MCode = "+DB.SQuote(inbound.MCode)+" and TenantID = "+TenantId+ " and IsActive = 1 and IsDeleted = 0) and LineNumber="+inbound.Lineno+" )");
            string TotalQty = DB.GetSqlS("Exec SP_GetTotalInvoiceQty @InboundID=" + inbound.InboundId + ",@MCode=" + DB.SQuote(inbound.MCode) + ",@TenantID=" + TenantId + ",@LineNo='" + inbound.Lineno + "', @MfgDate = " + (inbound.MfgDate != "" ? DB.SQuote(inbound.MfgDate) : "''") + ", @ExpDate = " + (inbound.ExpDate != "" ? DB.SQuote(inbound.ExpDate) : "''") + ", @SerialNo = " + (inbound.SerialNo != "" ? DB.SQuote(inbound.SerialNo) : "''") + ", @BatchNo = " + (inbound.BatchNo != "" ? DB.SQuote(inbound.BatchNo) : "''") + ", @ProjectRefNo = " + (inbound.ProjectNo != "" ? DB.SQuote(inbound.ProjectNo) : "''") + ",@MRP=" + (inbound.MRP != "" ? DB.SQuote(inbound.MRP) : "''"));
            //string TotalQty =  DB.GetSqlS("select convert(nvarchar(50), poQuantity) as S from ord_PODetails where POHeaderId=(select POHeaderId from INB_Inbound_ORD_SupplierInvoice where  inboundId=" + inbound.InboundId+ ") and  MaterialMasterID=(select MaterialMasterID  from MMT_MaterialMaster where MCode=" + DB.SQuote(inbound.MCode) + " and TenantID=" + TenantId + " and IsActive=1 and IsDeleted=0)");

            if (Convert.ToDecimal(RecieveQty)>= Convert.ToDecimal(TotalQty))
            {
                inbound.Result = "1";                
            }
            else
            {
                inbound.Result = "0";
            }
            Decimal PendingQty = Convert.ToDecimal(TotalQty) - Convert.ToDecimal(RecieveQty);
            inbound.ItemPendingQty = PendingQty;

            return inbound;
        }
    }
}