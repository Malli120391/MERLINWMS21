using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using MRLWMSC21Common;
using System.Data;
using System.Threading;
using System.Globalization;
using System.Collections;
using Newtonsoft.Json;
using System.IO;
using MRLWMSC21WCF.BL;
using MRLWMSC21WCF.BO;

namespace MRLWMSC21WCF
{

    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "FalconHHTWCFService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select FalconHHTWCFService.svc or FalconHHTWCFService.svc.cs at the Solution Explorer and start debugging.
    public class MRLWMSC21HHTWCFService : IMRLWMSC21HHTWCFService
    {


        public void DoWork()
        {
        }

        #region -------------------------Login-----------------------------


        public FalconHHTSData.LoginUserData GetUserDetails(String UserName, String Password, String LoginIPAddress, BO.ValidateUserLogin objValidateUserLogin)
        {
            BL.LoginHandler loginHandler = new BL.LoginHandler();
            return loginHandler.GetUserDetails(UserName, Password, LoginIPAddress, objValidateUserLogin);

        }
        #endregion
        public string CheckTenatMaterial(string Mcode, int AccountID, string TenantName)
        {
            BL.CommonHandler commonHandler = new BL.CommonHandler();
            return commonHandler.CheckTenatMaterial(Mcode, AccountID, TenantName);
        }
        #region ---------------Logout ----------------------
        public void Logout(BO.ValidateUserLogin validateUserLogin)
        {
            string result = null;
            SessionValidator.SingleSignOnDBSinkClient ssvalidator = new SessionValidator.SingleSignOnDBSinkClient();
            string json = JsonConvert.SerializeObject(validateUserLogin);
            ssvalidator.EndUserSession(validateUserLogin.Apps_MST_User_ID.ToString(), validateUserLogin.SessionIdentifier);
            return;
        }
        #endregion
        #region --------------Loding Printers----------------

        public List<THHTWSData.KeyValueStruct> GetDropDownData(String SqlString, String DefaultValue)
        {
            List<THHTWSData.KeyValueStruct> sources = new List<THHTWSData.KeyValueStruct>();

            IDataReader rsGetData = DB.GetRS(SqlString);
            sources.Clear();

            sources.Add(new THHTWSData.KeyValueStruct("0", DefaultValue));
            while (rsGetData.Read())
            {
                sources.Add(new THHTWSData.KeyValueStruct(rsGetData[1].ToString(), rsGetData[0].ToString()));
            }
            rsGetData.Close();

            return sources;
        }



        #endregion
        #region ------------- INBOUND --------------

        // Added by Prasanna for Inbound 
        //========= Get store ref nos=====//
        public UserDataTable GetStoreRefNos(BO.Inbound inbound)
        {
            BL.InboundHandler inboundHandler = new BL.InboundHandler();

            return inboundHandler.GetStorerefnos(inbound);

        }
        public UserDataTable GetDockVehicles(int inbound)
        {
            BL.InboundHandler inboundHandler = new BL.InboundHandler();

            return inboundHandler.GetDockVehicles(inbound);

        }
        //========= Get Storage Locations=====//
        public UserDataTable GetStorageLocations()
        {
            BL.InboundHandler inboundHandler = new BL.InboundHandler();
            return inboundHandler.GetStorageLocations();
        }
        //========= Get InvoiceNos=====//
        public UserDataTable GetPoInvoiceNos(BO.Inbound inbound)
        {
            BL.InboundHandler inboundHandler = new BL.InboundHandler();
            return inboundHandler.GetPoInvoiceNos(inbound);
        }
        //========= Check Container is avail or not=====//
        public string CheckContainer(string CartonNo,string InboundID)
        {
            BL.CommonHandler commonHandler = new BL.CommonHandler();
            return commonHandler.CheckContainer(CartonNo,  InboundID);
        }
        public string CheckContainerOBD(string CartonNo, string OutboundID)
        {
            BL.OutboundHanlder outHandler = new BL.OutboundHanlder();
            return outHandler.CheckContainerOBD(CartonNo, OutboundID);
        }
        public string CheckContainerStock(string CartonNo, string OutboundID)
        {
            BL.LiveStockHandler stockHandler = new BL.LiveStockHandler();
            return stockHandler.CheckContainerStock(CartonNo, OutboundID);
        }
        //========= Get PONOs=====//
        public UserDataTable GetPoNos(BO.Inbound inbound)
        {
            BL.InboundHandler inboundHandler = new BL.InboundHandler();
            return inboundHandler.GetPoNos(inbound);
        }
        //================Get Inbound Id==========//
        public BO.Inbound GetInboundNo(BO.Inbound inbound)
        {
            BL.InboundHandler InboundHandler = new BL.InboundHandler();
            return InboundHandler.GetInboundNo(inbound);
        }
        //========= Receiving item=====//
        public BO.Inbound UpdateReceiveItemForHHT(BO.Inbound inbound)
        {

            BL.InboundHandler inbounddetails = new BL.InboundHandler();
            return inbounddetails.UpdateReceiveItem(inbound);


        }
        public BO.Inbound GetReceivedQty(BO.Inbound inbound)
        {
            BL.InboundHandler inbounddetails = new BL.InboundHandler();
            return inbounddetails.GetReceivedQty(inbound);
        }
        public BO.Inbound CheckDockInbound(BO.Inbound inbound)
        {
            BL.InboundHandler inbounddetails = new BL.InboundHandler();
            return inbounddetails.CheckDock(inbound);
        }
        public BO.PutAway GetCurrentLocationForPutaway(BO.PutAway putAway)
        {
            BL.PutAwayHandler putAwayHandler = new BL.PutAwayHandler();
            return putAwayHandler.GetCurrentLocationForPutaway(putAway);

        }
        public string GetConatinerLocation(string Cartoncode,string WarehouseID )
        {
            BL.PutAwayHandler putAwayHandler = new BL.PutAwayHandler();
            return putAwayHandler.ChekContainerLocation(Cartoncode, WarehouseID);
        }

        public string GetConatinerLocationBin(string Cartoncode, string WarehouseID)
        {
            BL.BinTransferHandler binTransferHandler = new BL.BinTransferHandler();
            return binTransferHandler.ChekContainerLocation(Cartoncode, WarehouseID);
        }

        #endregion

        #region ------------- OUTBOUND --------------
        // Added by Prasanna for Outbound 
        //========= Get OBD ref nos=====//
        public UserDataTable GetobdRefNos(BO.Outbound outbound)
        {
            BL.OutboundHanlder outboundHanlder = new BL.OutboundHanlder();
            return outboundHanlder.GetOBDnos(outbound);

        }
        //CheckingStricly compliance
        public string CheckStrickyCompliance(BO.ValidateUserLogin validateUserLogin)
        {
            BL.CommonHandler commonHandler = new BL.CommonHandler();
            return commonHandler.CheckStrictlyCompliance(validateUserLogin);

        }

        //========= Get SONOs=====//
        public UserDataTable GetSONos(BO.Outbound outbound)
        {
            BL.OutboundHanlder outboundHanlder = new BL.OutboundHanlder();
            return outboundHanlder.GetSONos(outbound);
        }
        //========= Get SONOs=====//
        public BO.Outbound GetOBDItemToPick(BO.Outbound outbound)
        {
            BL.OutboundHanlder outboundHanlder = new BL.OutboundHanlder();
            return outboundHanlder.GetOBDItemToPick(outbound);
        }


        //========= For picking the Item=====//
        public BO.Outbound UpdatePickItem(BO.Outbound outbound)
        {

            BL.OutboundHanlder outboundHanlder = new BL.OutboundHanlder();
            return outboundHanlder.UpdatePickItem(outbound);


        }
        //================Get OBD Id==========//
        public BO.Outbound GetOBDNo(BO.Outbound outbound)
        {
            BL.OutboundHanlder OuboundHandler = new BL.OutboundHanlder();
            return OuboundHandler.GetOBDNo(outbound);
        }
        //================= OBD SKIP ================//
        public BO.Outbound OBDSkipItem(BO.Outbound outbound)
        {
            BL.OutboundHanlder outboundHanlder = new BL.OutboundHanlder();
            outboundHanlder.OBDSkipItem(outbound);
            return outbound;
        }
        #endregion
        #region ------------- VLPD --------------
        //--------------------VLPD-----------------------------//
        //========= get VLPD Id=====//
        public string GetVLPDID(BO.VLPD VLPD)
        {
            try
            {
                BL.VLPDHandler VLPDHandler = new BL.VLPDHandler();
                return VLPDHandler.GetVLPDNo(VLPD);
            }
            catch (Exception ex)
            {
                return "0";
            }
        }
        //=========get picking the Item based on VLPD=====//
        public BO.VLPD GetItemToPick(BO.VLPD VLPD)
        {
            BL.VLPDHandler VLPDHandler = new BL.VLPDHandler();
            return VLPDHandler.GetItemToPick(VLPD);
        }
        //========= For picking the Item using VLPD=====//
        public BO.VLPD UpsertPickItem(BO.VLPD Pickdata)
        {
            BL.VLPDHandler VLPDHandler = new BL.VLPDHandler();
            return VLPDHandler.UpsertPickItem(Pickdata);
        }
        public UserDataTable GetOpenVLPDNos(BO.VLPD vLPD)
        {
            BL.VLPDHandler VLPDHandler = new BL.VLPDHandler();
            return VLPDHandler.GetOpenVLPDNos(vLPD);

        }
        public UserDataTable GetOpenLoadsheetList(String TenantID, string AccountID)
        {
            BL.VLPDHandler VLPDHandler = new BL.VLPDHandler();
            return VLPDHandler.GetOpenLoadsheetList(TenantID, AccountID);
        }

        public UserDataTable GetPendingOBDListForLoading(String TenantID, String AccountID)
        {
            BL.VLPDHandler VLPDHandler = new BL.VLPDHandler();
            return VLPDHandler.GetPendingOBDListForLoading(TenantID, AccountID);
        }
        public UserDataTable GetOpenVLPDNosForSorting(BO.VLPD vLPD)
        {
            BL.VLPDHandler VLPDHandler = new BL.VLPDHandler();
            return VLPDHandler.GetOpenVLPDNosForSorting(vLPD);

        }

        public string UpsertLoadCreated(BO.VLPD VLPD)
        {
            BL.VLPDHandler VLPDHandler = new BL.VLPDHandler();
            return VLPDHandler.UpsertLoadCreated(VLPD);
        }
        public string UpsertLoad(BO.VLPD VLPD)
        {
            BL.VLPDHandler VLPDHandler = new BL.VLPDHandler();
            return VLPDHandler.UpsertLoad(VLPD);
        }
        public string LoadVerification(String LoadNumber, String UserId)
        {

            BL.VLPDHandler VLPDHandler = new BL.VLPDHandler();
            return VLPDHandler.LoadVerification(LoadNumber, UserId);
        }
        public string ValidateCartonLiveStock(BO.LiveStock liveStock)
        {
            BL.LiveStockHandler liveStockHandler = new BL.LiveStockHandler();
            return liveStockHandler.ValidateCartonLiveStock(liveStock.CartonNo,liveStock.WarehouseID);

        }

        public UserDataTable VLPDSorting(BO.VLPD vLPD)
        {
            BL.VLPDHandler VLPDHandler = new BL.VLPDHandler();
            return VLPDHandler.VLPDSorting(vLPD);

        }
        public BO.VLPD VLPDSkipItem(BO.VLPD VLPD)
        {
            BL.VLPDHandler vLPDHandler = new BL.VLPDHandler();
            vLPDHandler.VLPDSkipItem(VLPD);
            return VLPD;
        }
        public UserDataTable GetVLPDPickedList(BO.VLPD vLPD, out string Result)
        {
            BL.VLPDHandler VLPDHandler = new BL.VLPDHandler();
            return VLPDHandler.GetVLPDPickedList(vLPD,out Result);

        }
        public BO.VLPD DeleteVLPDPickedItems(BO.VLPD vLPD)
        {
            BL.VLPDHandler VLPDHandler = new BL.VLPDHandler();
            return VLPDHandler.DeleteVLPDPickedItems(vLPD);
        }
        #endregion
        #region ------------- Loading --------------

        #endregion
        #region -------------------------Live Stock Search-----------------------------
        //=================Get active tenants===========//
        public UserDataTable GetTenants(BO.LiveStock liveStock)
        {
            BL.LiveStockHandler LiveStockHandler = new BL.LiveStockHandler();
            return LiveStockHandler.GetTenants(liveStock);

        }

        public UserDataTable GetWarehouse(BO.LiveStock liveStock)
        {
            BL.CommonHandler commonHandler = new CommonHandler();
            return commonHandler.GetWarehouse(liveStock);

        }

        //========= Get Activestock=====//
        public UserDataTable GetLiveStockData(BO.LiveStock liveStock)
        {
            BL.LiveStockHandler LiveStockHandler = new BL.LiveStockHandler();
            return LiveStockHandler.GetLiveStockData(liveStock);

        }
        //========= Check Location is avail or not=====//
        public string CheckLocationForLiveStock(BO.LiveStock stock)
        {
            BL.CommonHandler commonHandler = new BL.CommonHandler();
            return commonHandler.CheckLoction(stock);
        }

        #endregion
        #region -------------------------Cycle Count Fot HHT-----------------------------
        //==================Cycle Count names =======================//
        public UserDataTable GetCCNames(BO.CycleCount cycleCount)
        {
            BL.CycleCountHandler cycleCountHandler = new BL.CycleCountHandler();
            return cycleCountHandler.GetCCNames(cycleCount);
        }
        //==================Check Blocking Locations =======================//
        public BO.CycleCount IsBlockedLocation(BO.CycleCount cycleCount)
        {
            BL.CycleCountHandler cycleCountHandler = new BL.CycleCountHandler();
            return cycleCountHandler.IsBlockedLocation(cycleCount);
        }
        //==================blocked location=======================//
        public BO.CycleCount BlockLocationForCycleCount(BO.CycleCount cycleCount)
        {
            BL.CycleCountHandler cycleCountHandler = new BL.CycleCountHandler();
            return cycleCountHandler.BlockLocationForCycleCount(cycleCount);
        }


        //==================Cyclecountdata =======================//
        public List<BO.CycleCount> GetCycleCountInformation(BO.CycleCount cycleCount)
        {
            try
            {
                BL.CycleCountHandler cyclecounthandler = new BL.CycleCountHandler();
                return cyclecounthandler.GetCycleCountInformation(cycleCount);
            }
            catch (Exception ex)
            {
                return null;

            }


        }
        //==================upsert cyclecount data =======================//
        public BO.CycleCount UpsertCycleCount(BO.CycleCount cycleCount)
        {
            BL.CycleCountHandler cycleCountHandler = new BL.CycleCountHandler();
            return cycleCountHandler.UpsertCycleCount(cycleCount);
        }
        //==================Realease cyclecount bin =======================//
        public BO.CycleCount ReleaseCycleCountLocation(BO.CycleCount cycleCount)
        {
            BL.CycleCountHandler cycleCountHandler = new BL.CycleCountHandler();
            return cycleCountHandler.ReleaseCycleCountLocation(cycleCount);
        }

        //==================get cyclecount bin count =======================//
        public BO.CycleCount GetBinCount(BO.CycleCount cyclecountinfo)
        {
            BL.CycleCountHandler cycleCountHandler = new BL.CycleCountHandler();
            return cycleCountHandler.GetBinCount(cyclecountinfo);
        }

        //========================check material in cyclecount==============//
        public BO.CycleCount CheckMaterialAvailablilty(BO.CycleCount cycleCount)
        {
            BL.CycleCountHandler cycleCountHandler = new BL.CycleCountHandler();
            return cycleCountHandler.CheckMaterialAvailablilty(cycleCount);
        }
        #endregion
        #region -------------------------Internal Transfers-----------------------------
        //=================Get active Transferorders===========//
        public UserDataTable GetTransferOrderNos(BO.TransferBO transferInfo)
        {
            BL.TransferHandler TransferHandler = new BL.TransferHandler();
            return TransferHandler.GetTransferOrderNos(transferInfo);

        }
        //=================Get Transferid===========//
        public string GetTransferID(BO.TransferBO transferBO)
        {
            try
            {
                BL.TransferHandler TransferHandler = new BL.TransferHandler();
                return TransferHandler.GetTransferID(transferBO);
            }
            catch (Exception ex)
            {
                return "0";
            }
        }


        //==================Check item for transfers =======================//
        public BO.TransferBO CheckItemForTransfer(BO.TransferBO transferBO)
        {
            BL.TransferHandler transferHandler = new BL.TransferHandler();
            return transferHandler.CheckItemForPutAway(transferBO);
        }
        //==================To close InternalTransfre Order =======================//
        public BO.TransferBO CloseTransferOrder(BO.TransferBO transferBO)
        {
            BL.TransferHandler transferHandler = new BL.TransferHandler();
            return transferHandler.CloseTransferOrder(transferBO);
        }
        #endregion
        public BO.PutAway GetTransferContainer(BO.PutAway putAway)
        {
            BL.PutAwayHandler putAwayHandler = new BL.PutAwayHandler();
            return putAwayHandler.GetTransferContainer(putAway);
        }
        #region   ----------------- Bin to Bin Transfer -------------
        //========= Get Storage Locations=====//
        public UserDataTable GetBinToBinStorageLocations()
        {
            BL.BinTransferHandler binTransferHandler = new BL.BinTransferHandler();
            return binTransferHandler.GetStorageLocations();
        }
        //========= Get Available qty of the SKU=====//

        public BO.TransferBO GetAvailbleQtyListforHHT(BO.TransferBO transferBO)
        {
            BL.BinTransferHandler transferHandler = new BL.BinTransferHandler();
            return transferHandler.GetAvailbleQtyList(transferBO);
        }
        //========Upsert bin to Bin item========//

        public BO.TransferBO UpsertBinToBinTransferItem(BO.TransferBO transferBO)
        {
            BL.BinTransferHandler binTransferHandler = new BL.BinTransferHandler();
            return binTransferHandler.UpsertBinToBinTransfer(transferBO);
        }
        #endregion
        #region -------------------------PutAway-----------------------------
        //=============Check PutAway Item============//
        public BO.PutAway CheckPutAwayItemQty(BO.PutAway PutAway)
        {
            BL.PutAwayHandler putAwayHandler = new BL.PutAwayHandler();
            return putAwayHandler.CheckPutAwayItemQty(PutAway);
        }
        public string CartonPutaway(string cartonCode, string locationCode)
        {
            string drlStatement = "EXEC [dbo].[sp_INV_LocationPutaway] @CartonCode=" + DB.SQuote(cartonCode) + ",@LocationCode=" + DB.SQuote(locationCode);
            int result = DB.GetSqlN(drlStatement);
            switch (result)
            {
                case -1:
                    return "Invalid container scanned";
                case -2:
                    return "Invalid location scanned";
                case -3:
                    return "Same container already placed in another location";
                case 0:
                    return "Invalid transaction";
                default:
                    return "Transfer completed";
            }
        }
        //==================Get skip Reason =======================//
        public UserDataTable GetSkipReasonList(string type)
        {
            BL.CommonHandler commonHandler = new BL.CommonHandler();
            return commonHandler.GetSkipReasonList(type);
        }
        #region 
        //==================Get PutAway Item=======================//
        public BO.PutAway GetItemTOPutAway(BO.PutAway putAway)
        {
            BL.PutAwayHandler putAwayHandler = new BL.PutAwayHandler();
            return putAwayHandler.GetItemTOPutAway(putAway);
        }
        // Upser Skip Item
        public BO.PutAway SkipItem(BO.PutAway putAway)
        {
            BL.PutAwayHandler putAwayHandler = new BL.PutAwayHandler();
            putAwayHandler.SkipItem(putAway);
            return putAway;
        }
        //========= For PutAway the Item =====//
        public BO.PutAway UpsertPutAwayItem(BO.PutAway putAway)
        {
            BL.PutAwayHandler putAwayHandler = new BL.PutAwayHandler();
            return putAwayHandler.UpsertPutAwayItem(putAway);
        }
        #endregion

        #region -------------------------Checking User session-----------------------------
        public int CheckValidateUser(BO.ValidateUserLogin checklogin)
        {

            int result = 0;
            checklogin.LastRequestTimestamp = DateTime.Now.ToString();
            string json = JsonConvert.SerializeObject(checklogin);
            SessionValidator.SingleSignOnDBSinkClient obj = new SessionValidator.SingleSignOnDBSinkClient();
            int output = obj.ValidateUserSession(json);
            if (output <= 0)
            {
                result = 1;
            }
            return result;


        }
        #endregion
        // end Prasanna code
        #region -------------------------Goods IN-----------------------------

        //public string UpdateReceiveItem(string GoodsMovementDetailsID, string GoodsMovementTypeID, string Location, string MaterialMaster_IUoMID, string DocQty, string Quantity, string Storerefnum, string LineNumber, string mcode, string IsDamaged, string hasDiscrepancy, string POSOHeaderID, string CreatedBy, string MaterialStorageParameterIDs, string MaterialStorageParameterValues, string Conversionfactor, string remarks, string LastModifiedBy, string IsActive, string KitplannerID, string InvoiceQuantity, string SupplierInvoiceID, string IsPositiveRecall, int IsPrintRequired, string printerIP, string serialNumber)
        //{


        //    int vInboundID = DB.GetSqlN("select InboundID as N from INB_Inbound where StoreRefNo=" + DB.SQuote(Storerefnum));
        //    if (serialNumber.Length != 0)
        //    {
        //        string QueryforSerialNumberCheck = "select GMDMSP.GoodsMovementDetails_MaterialStorageParameterID as N from INV_GoodsMovementDetails GMD " +
        //                 "JOIN INV_GoodsMovementDetails_MMT_MaterialStorageParameter GMDMSP ON GMD.GoodsMovementDetailsID=GMDMSP.GoodsMovementDetailsID " +
        //                 "AND GMDMSP.MaterialStorageParameterID=3 AND GMDMSP.IsDeleted=0 WHERE GMD.GoodsMovementTypeID=1 AND GMD.IsDeleted=0 AND " +
        //                 "GMD.TransactionDocID=" + vInboundID + " AND CONVERT(NVARCHAR,GMDMSP.Value)='" + serialNumber + "'";
        //        if (DB.GetSqlN(QueryforSerialNumberCheck) != 0)
        //        {
        //            return "Serial no. already exists, cannot receive";

        //        } 
        //    }

        //    IDataReader reader = DB.GetRS("select InboundTracking_WarehouseID from INB_InboundTracking_Warehouse where InboundID=" + vInboundID);
        //    if (reader.Read())
        //    {
        //    }
        //    else
        //    {
        //        return "Shipment not received";
        //    }
        //    int vDeliveryStatus = DB.GetSqlN("SELECT InboundStatusID AS N FROM INB_Inbound WHERE InboundID=" + vInboundID);
        //    if (vDeliveryStatus >= 4)
        //    {
        //        return "Shipment verified, Cannot receive";
        //    }

        //    int CARTONid = DB.GetSqlN("select CartonID as N from INV_Carton where IsActive=1 and IsDeleted=0 AND CartonCode='" + Location + "'");

        //    if (CARTONid == 0)
        //    {
        //        return " Not a valid container";
        //    }
        //    int vMMID = 0;// DB.GetSqlN("select MaterialMasterID as N from MMT_MaterialMaster where MCode=" + DB.SQuote(mcode));
        //    string OEMPartNumber = String.Empty;
        //    string Description = String.Empty;

        //    //string sqlcommandforMMDetails = "select MaterialMasterID,isnull(OEMPartNo,'') as OEMPartNo,isnull(MDescription,'') as MDescription  from MMT_MaterialMaster where MCode=" + DB.SQuote(mcode);

        //    string sqlcommandforMMDetails = "select MM.MaterialMasterID,isnull(MM.OEMPartNo,'') as OEMPartNo,isnull(MM.MDescription,'') as MDescription  from MMT_MaterialMaster MM JOIN TPL_Tenant_MaterialMaster TPL_MM ON TPL_MM.MaterialMasterID=MM.MaterialMasterID AND TPL_MM.IsDeleted=0 AND TPL_MM.IsActive=1  JOIN INB_Inbound INB ON INB.TenantID=TPL_MM.TenantID AND INB.IsDeleted=0 AND INB.IsActive=1  where INB.StoreRefNo='" + Storerefnum + "' AND MM.MCode=" + DB.SQuote(mcode);

        //    IDataReader dataReader = DB.GetRS(sqlcommandforMMDetails);
        //    while (dataReader.Read())
        //    {
        //        vMMID = DB.RSFieldInt(dataReader, "MaterialMasterID");
        //        OEMPartNumber = DB.RSField(dataReader, "OEMPartNo");
        //        Description = DB.RSField(dataReader, "MDescription");

        //    }
        //    //select MaterialMasterID,OEMPartNo,MDescription as N from MMT_MaterialMaster where MCode='RT205001000027'


        //    decimal vGoodsINQuantity = DB.GetSqlNDecimal("EXEC  [sp_INV_GetGoodsInQuantitySum]    @MaterialMasterID=" + vMMID + ",@InboundID=" + vInboundID + ",@LineNumber=" + LineNumber + ",@POHeaderID=" + POSOHeaderID + ",@GoodsMovementDetailsID=0" + ",@SupplierInvoiceID=" + SupplierInvoiceID);
        //    decimal vEnteredQuantity = decimal.Parse(DocQty);
        //    decimal vInvoiceQuantity = decimal.Parse(InvoiceQuantity);
        //    //if (vEnteredQuantity + vGoodsINQuantity > vInvoiceQuantity)
        //    //{
        //    //    return "Goodsin quantity is greater than invoice quantity cannot receive";
        //    //}
        //    int CycleCountOn = DB.GetSqlN("select convert(int,qcc.IsOn) as N from QCC_CycleCount qcc left join QCC_CycleCountDetails qccd on qccd.CycleCountID=qcc.CycleCountID  where qccd.MaterialMasterID=" + vMMID);
        //    if (CycleCountOn == 1)
        //    {
        //        return "This material is in cycle count,cannot receive";
        //    }

        //    try
        //    {
        //        int result = InventoryCommonClass.UpdateGoodsMovementDetails(GoodsMovementDetailsID, GoodsMovementTypeID, "", MaterialMaster_IUoMID, DocQty, Quantity, vInboundID.ToString(), SupplierInvoiceID, LineNumber, vMMID.ToString(), IsDamaged, hasDiscrepancy, POSOHeaderID, CreatedBy, MaterialStorageParameterIDs, MaterialStorageParameterValues, Conversionfactor, remarks, LastModifiedBy, IsActive, KitplannerID, IsPositiveRecall, "0", "0", CARTONid.ToString());
        //        if (result == 1)
        //        {
        //            try
        //            {
        //                if (IsPrintRequired == 1)
        //                {
        //                    //For Printing Lables
        //                    MRLWMSC21Common.TracklineMLabel thisMLabel = new MRLWMSC21Common.TracklineMLabel();

        //                    thisMLabel.MCode = mcode;
        //                    thisMLabel.OEMPartNo = OEMPartNumber;
        //                    thisMLabel.Description = Description;
        //                    thisMLabel.AltMCode = String.Empty;

        //                    string batchNo = String.Empty;
        //                    string serialNo = String.Empty;
        //                    DateTime mfgDate = DateTime.MinValue;
        //                    DateTime ExpDate = DateTime.MinValue;
        //                    string[] msps = MaterialStorageParameterIDs.Split(',');
        //                    string[] mspvalues = MaterialStorageParameterValues.Split(',');
        //                    for (int index = 0; index < msps.Length; index++)
        //                    {
        //                        if (msps[index] == "1")
        //                        {
        //                            mfgDate = Convert.ToDateTime(mspvalues[index]);
        //                        }
        //                        else if (msps[index] == "2")
        //                        {
        //                            ExpDate = Convert.ToDateTime(mspvalues[index]);
        //                        }
        //                        else if (msps[index] == "3")
        //                        {
        //                            serialNo = mspvalues[index];
        //                        }
        //                        else if (msps[index] == "4")
        //                        {
        //                            batchNo = mspvalues[index];
        //                        }
        //                    }
        //                    thisMLabel.MfgDate = mfgDate;
        //                    thisMLabel.ExpDate = ExpDate;

        //                    thisMLabel.BatchNo = batchNo;
        //                    thisMLabel.SerialNo = serialNo;
        //                    thisMLabel.PrintQty = DocQty;


        //                    thisMLabel.PrinterType = "IP";
        //                    thisMLabel.PrinterIP = printerIP;
        //                    thisMLabel.IsBoxLabelReq = true;
        //                    thisMLabel.ReqNo = String.Empty;

        //                    thisMLabel.StrRefNo = Storerefnum;
        //                    thisMLabel.KitCode = String.Empty;
        //                    thisMLabel.OBDNumber = String.Empty;
        //                    thisMLabel.IsBoxLabelReq = false;
        //                    CommonLogic.PrintLabel(thisMLabel);
        //                    //Thread worker = new Thread(new ParameterizedThreadStart(this.ThermalLabelWorkerForGoodsIN));
        //                    //worker.SetApartmentState(ApartmentState.STA);
        //                    //worker.Name = "ThermalLabelWorker";
        //                    //worker.Start(thisMLabel);
        //                    //worker.Join();
        //                    //if (thisMLabel.Result != "Success")
        //                    //{
        //                    //    return "successwithnoprint";//, unable to print the label due to printer unavailability. Please check the printer.";

        //                    //}
        //                    //else
        //                    //{
        //                    //    return "Success";
        //                    //}

        //                }
        //            }
        //            catch
        //            {
        //                return "successwithnoprint";//, unable to print the label due to printer unavailability. Please check the printer.";
        //            }
        //        }
        //        else
        //        {
        //            return "Process failed, Please contact support team";
        //        }
        //        return "Success";
        //    }
        //    catch
        //    {
        //        return "Process failed,Please contact support team";
        //    }



        //}

        #endregion
        public GoodsInData1234 GoodsInData(string MaterialCode, int TenantID, int LineNumber, string ATCRef_IB_OBD, string PONumber, string InvoiceNumber)
        {

            GoodsInData1234 data = new GoodsInData1234();


            List<THHTWSData.KeyValueStruct> sources = new List<THHTWSData.KeyValueStruct>();
            IDataReader rsGetData = DB.GetRS("EXEC [sp_INV_GetMaterialStorageParameters]  @MCode='" + MaterialCode + "',@TenantID=1");
            sources.Clear();
            while (rsGetData.Read())
            {
                sources.Add(new THHTWSData.KeyValueStruct("ParameterName", DB.RSField(rsGetData, "ParameterName").ToString()));
                sources.Add(new THHTWSData.KeyValueStruct("ParameterDataType", DB.RSField(rsGetData, "ParameterDataType").ToString()));
                sources.Add(new THHTWSData.KeyValueStruct("ControlType", DB.RSField(rsGetData, "ControlType").ToString()));
                sources.Add(new THHTWSData.KeyValueStruct("DataSource", DB.RSField(rsGetData, "DataSource").ToString()));
                sources.Add(new THHTWSData.KeyValueStruct("IsRequired", DB.RSFieldTinyInt(rsGetData, "IsRequired").ToString()));
                sources.Add(new THHTWSData.KeyValueStruct("MaterialStorageParameterID", DB.RSFieldInt(rsGetData, "MaterialStorageParameterID").ToString()));
                sources.Add(new THHTWSData.KeyValueStruct("DisplayName", DB.RSField(rsGetData, "DisplayName").ToString()));

            }
            rsGetData.Close();
            data.MSPs = sources;

            //Getting POITEM Properties
            List<THHTWSData.KeyValueStruct> POItemsources = new List<THHTWSData.KeyValueStruct>();
            int MaterialMasterID = DB.GetSqlN("select MaterialMasterID as N from MMT_MaterialMaster where MCode=" + DB.SQuote(MaterialCode));
            int InboundID = DB.GetSqlN("select InboundID as N from INB_Inbound where StoreRefNo=" + DB.SQuote(ATCRef_IB_OBD));
            int PoHeaderID;
            if (PONumber != "0")
            {
                PoHeaderID = DB.GetSqlN("select POHeaderID as N from ORD_POHeader where PONumber=" + DB.SQuote(PONumber));
                if (PoHeaderID == 0)
                {
                    POItemsources.Add(new THHTWSData.KeyValueStruct("PoHeaderID", "0"));
                    data.POItemProperties = POItemsources;
                    return data;

                }
            }
            else
            {
                PoHeaderID = 0;
            }
            int SupplierInvoiceID;
            if (InvoiceNumber != "0")
            {
                SupplierInvoiceID = DB.GetSqlN("SELECT SupplierInvoiceID AS N  FROM ORD_SupplierInvoice where InvoiceNumber like '%" + InvoiceNumber + "%' AND POHeaderID=" + PoHeaderID + " AND IsActive=1 AND IsDeleted=0");
                if (SupplierInvoiceID == 0)
                {
                    POItemsources.Add(new THHTWSData.KeyValueStruct("SupplierInvoiceID", "0"));
                    data.POItemProperties = POItemsources;
                    return data;
                }
            }
            else
            {
                SupplierInvoiceID = 0;
            }
            string sCmdPOQuantity = "dbo.sp_INV_GetPOQuantityListForStoreRef @InboundID=" + InboundID + ",@MaterialMasterID=" + MaterialMasterID + ",@POLineNumber=" + LineNumber + ",@POHeaderID=" + PoHeaderID + ",@SupplierInvoiceID=" + SupplierInvoiceID;

            IDataReader rsPOQuantityList = DB.GetRS(sCmdPOQuantity);
            POItemsources.Clear();
            int count = rsPOQuantityList.FieldCount;
            int mspvalue = count - 21;
            while (rsPOQuantityList.Read())
            {


                POItemsources.Add(new THHTWSData.KeyValueStruct("PUoM", DB.RSField(rsPOQuantityList, "PUoM").ToString()));
                POItemsources.Add(new THHTWSData.KeyValueStruct("PUoMQty", DB.RSFieldDecimal(rsPOQuantityList, "PUoMQty").ToString()));
                POItemsources.Add(new THHTWSData.KeyValueStruct("InvUoM", DB.RSField(rsPOQuantityList, "InvUoM").ToString()));
                POItemsources.Add(new THHTWSData.KeyValueStruct("InvUoMQty", DB.RSFieldDecimal(rsPOQuantityList, "InvUoMQty").ToString()));
                POItemsources.Add(new THHTWSData.KeyValueStruct("BUoM", DB.RSField(rsPOQuantityList, "BUoM")));
                POItemsources.Add(new THHTWSData.KeyValueStruct("BUoMQty", DB.RSFieldDecimal(rsPOQuantityList, "BUoMQty").ToString()));
                POItemsources.Add(new THHTWSData.KeyValueStruct("MUoM", DB.RSField(rsPOQuantityList, "MUoM").ToString()));
                POItemsources.Add(new THHTWSData.KeyValueStruct("MUoMQty", DB.RSFieldDecimal(rsPOQuantityList, "MUoMQty").ToString()));
                POItemsources.Add(new THHTWSData.KeyValueStruct("InvQty", DB.RSFieldDecimal(rsPOQuantityList, "InvQty").ToString()));
                POItemsources.Add(new THHTWSData.KeyValueStruct("KitPlannerID", DB.RSFieldInt(rsPOQuantityList, "KitPlannerID").ToString()));
                POItemsources.Add(new THHTWSData.KeyValueStruct("MaterialMaster_InvUoMID", DB.RSFieldInt(rsPOQuantityList, "MaterialMaster_InvUoMID").ToString()));
                POItemsources.Add(new THHTWSData.KeyValueStruct("MDescription", DB.RSField(rsPOQuantityList, "MDescription").ToString()));
                POItemsources.Add(new THHTWSData.KeyValueStruct("IsReeturned", DB.RSFieldInt(rsPOQuantityList, "IsReeturned").ToString()));
                POItemsources.Add(new THHTWSData.KeyValueStruct("POHeaderID", DB.RSFieldInt(rsPOQuantityList, "POHeaderID").ToString()));
                POItemsources.Add(new THHTWSData.KeyValueStruct("SupplierInvoiceID", DB.RSFieldInt(rsPOQuantityList, "SupplierInvoiceID").ToString()));
                SupplierInvoiceID = DB.RSFieldInt(rsPOQuantityList, "SupplierInvoiceID");
                POItemsources.Add(new THHTWSData.KeyValueStruct("MeasurementTypeID", DB.RSFieldInt(rsPOQuantityList, "MeasurementTypeID").ToString()));
                POItemsources.Add(new THHTWSData.KeyValueStruct("CF", DB.RSFieldDecimal(rsPOQuantityList, "CF").ToString()));
                POItemsources.Add(new THHTWSData.KeyValueStruct("CFInMUoM", DB.RSFieldDecimal(rsPOQuantityList, "CFInMUoM").ToString()));
                string mspdata = String.Empty;
                if (mspvalue != 0)
                {
                    for (int i = 23; i < count; i++)
                    {
                        mspdata += rsPOQuantityList.GetValue(i).ToString() + ",";
                    }
                }
                if (mspdata.Length != 0)
                {
                    string mspcolvalues = mspdata.Remove(mspdata.Length - 1, 1);
                    POItemsources.Add(new THHTWSData.KeyValueStruct("MspValues", mspcolvalues));
                }
                else
                {
                    POItemsources.Add(new THHTWSData.KeyValueStruct("MspValues", String.Empty));
                }

            }
            data.POItemProperties = POItemsources;


            if (PONumber != "0")
            {
                PoHeaderID = DB.GetSqlN("select POHeaderID as N from ORD_POHeader where PONumber=" + DB.SQuote(PONumber));
            }
            else
            {
                PoHeaderID = 0;
            }
            int vMMID = DB.GetSqlN("select MaterialMasterID as N from MMT_MaterialMaster where MCode=" + DB.SQuote(MaterialCode));
            int vInboundID = DB.GetSqlN("select InboundID as N from INB_Inbound where StoreRefNo=" + DB.SQuote(ATCRef_IB_OBD));
            DataSet ds = DB.GetDS("EXEC  [sp_INV_GetGoodsMovementDetailsList]@MaterialMasterID=" + vMMID + ",@InboundID=" + vInboundID + ",@LineNumber=" + LineNumber + ",@POHeaderID=" + PoHeaderID + ",@SupplierInvoiceID=" + SupplierInvoiceID, false);
            DataTable dt = ds.Tables[0];
            data.GoodsInListData = dt;

            return data;
        }
        public List<THHTWSData.KeyValueData123> Test(String MaterialCode, int TenantID)
        {
            List<THHTWSData.KeyValueData123> sources = new List<THHTWSData.KeyValueData123>();
            return sources;
        }

        public List<THHTWSData.KeyValueStruct> GetMaterialStorageParameters(String MaterialCode, int TenantID)
        {
            List<THHTWSData.KeyValueStruct> sources = new List<THHTWSData.KeyValueStruct>();


            IDataReader rsGetData = DB.GetRS("EXEC [sp_INV_GetMaterialStorageParameters]  @MCode='" + MaterialCode + "',@TenantID=1");
            sources.Clear();
            while (rsGetData.Read())
            {
                sources.Add(new THHTWSData.KeyValueStruct("ParameterName", DB.RSField(rsGetData, "ParameterName").ToString()));
                sources.Add(new THHTWSData.KeyValueStruct("ParameterDataType", DB.RSField(rsGetData, "ParameterDataType").ToString()));
                sources.Add(new THHTWSData.KeyValueStruct("ControlType", DB.RSField(rsGetData, "ControlType").ToString()));
                sources.Add(new THHTWSData.KeyValueStruct("DataSource", DB.RSField(rsGetData, "DataSource").ToString()));
                sources.Add(new THHTWSData.KeyValueStruct("IsRequired", DB.RSFieldTinyInt(rsGetData, "IsRequired").ToString()));
                sources.Add(new THHTWSData.KeyValueStruct("MaterialStorageParameterID", DB.RSFieldInt(rsGetData, "MaterialStorageParameterID").ToString()));

            }
            rsGetData.Close();

            return sources;
        }

        public List<THHTWSData.KeyValueStruct> GetPOItemProperties(int LineNumber, string MaterialCode, string ATCRef_IB_OBD, string PONumber, string InvoiceNumber)
        {
            List<THHTWSData.KeyValueStruct> sources = new List<THHTWSData.KeyValueStruct>();
            int MaterialMasterID = DB.GetSqlN("select MaterialMasterID as N from MMT_MaterialMaster where MCode=" + DB.SQuote(MaterialCode));
            int InboundID = DB.GetSqlN("select InboundID as N from INB_Inbound where StoreRefNo=" + DB.SQuote(ATCRef_IB_OBD));
            int PoHeaderID;
            if (PONumber != "0")
            {
                PoHeaderID = DB.GetSqlN("select POHeaderID as N from ORD_POHeader where PONumber=" + DB.SQuote(PONumber));
                if (PoHeaderID == 0)
                {
                    sources.Add(new THHTWSData.KeyValueStruct("PoHeaderID", "0"));
                    return sources;
                }
            }
            else
            {
                PoHeaderID = 0;
            }
            int SupplierInvoiceID;
            if (InvoiceNumber != "0")
            {
                SupplierInvoiceID = DB.GetSqlN("SELECT SupplierInvoiceID AS N  FROM ORD_SupplierInvoice where InvoiceNumber like '%" + InvoiceNumber + "%' AND POHeaderID=" + PoHeaderID + " AND IsActive=1 AND IsDeleted=0");
                if (SupplierInvoiceID == 0)
                {
                    sources.Add(new THHTWSData.KeyValueStruct("SupplierInvoiceID", "0"));
                    return sources;
                }
            }
            else
            {
                SupplierInvoiceID = 0;
            }
            string sCmdPOQuantity = "dbo.sp_INV_GetPOQuantityListForStoreRef @InboundID=" + InboundID + ",@MaterialMasterID=" + MaterialMasterID + ",@POLineNumber=" + LineNumber + ",@POHeaderID=" + PoHeaderID + ",@SupplierInvoiceID=" + SupplierInvoiceID;

            IDataReader rsPOQuantityList = DB.GetRS(sCmdPOQuantity);
            sources.Clear();
            int count = rsPOQuantityList.FieldCount;
            int mspvalue = count - 21;
            while (rsPOQuantityList.Read())
            {


                sources.Add(new THHTWSData.KeyValueStruct("PUoM", DB.RSField(rsPOQuantityList, "PUoM").ToString()));
                sources.Add(new THHTWSData.KeyValueStruct("PUoMQty", DB.RSFieldDecimal(rsPOQuantityList, "PUoMQty").ToString()));
                sources.Add(new THHTWSData.KeyValueStruct("InvUoM", DB.RSField(rsPOQuantityList, "InvUoM").ToString()));
                sources.Add(new THHTWSData.KeyValueStruct("InvUoMQty", DB.RSFieldDecimal(rsPOQuantityList, "InvUoMQty").ToString()));
                sources.Add(new THHTWSData.KeyValueStruct("BUoM", DB.RSField(rsPOQuantityList, "BUoM")));
                sources.Add(new THHTWSData.KeyValueStruct("BUoMQty", DB.RSFieldDecimal(rsPOQuantityList, "BUoMQty").ToString()));
                sources.Add(new THHTWSData.KeyValueStruct("MUoM", DB.RSField(rsPOQuantityList, "MUoM").ToString()));
                sources.Add(new THHTWSData.KeyValueStruct("MUoMQty", DB.RSFieldDecimal(rsPOQuantityList, "MUoMQty").ToString()));
                sources.Add(new THHTWSData.KeyValueStruct("InvQty", DB.RSFieldDecimal(rsPOQuantityList, "InvQty").ToString()));
                sources.Add(new THHTWSData.KeyValueStruct("KitPlannerID", DB.RSFieldInt(rsPOQuantityList, "KitPlannerID").ToString()));
                sources.Add(new THHTWSData.KeyValueStruct("MaterialMaster_InvUoMID", DB.RSFieldInt(rsPOQuantityList, "MaterialMaster_InvUoMID").ToString()));
                sources.Add(new THHTWSData.KeyValueStruct("MDescription", DB.RSField(rsPOQuantityList, "MDescription").ToString()));
                sources.Add(new THHTWSData.KeyValueStruct("IsReeturned", DB.RSFieldInt(rsPOQuantityList, "IsReeturned").ToString()));
                sources.Add(new THHTWSData.KeyValueStruct("POHeaderID", DB.RSFieldInt(rsPOQuantityList, "POHeaderID").ToString()));
                sources.Add(new THHTWSData.KeyValueStruct("SupplierInvoiceID", DB.RSFieldInt(rsPOQuantityList, "SupplierInvoiceID").ToString()));

                sources.Add(new THHTWSData.KeyValueStruct("MeasurementTypeID", DB.RSFieldInt(rsPOQuantityList, "MeasurementTypeID").ToString()));
                sources.Add(new THHTWSData.KeyValueStruct("CF", DB.RSFieldDecimal(rsPOQuantityList, "CF").ToString()));
                sources.Add(new THHTWSData.KeyValueStruct("CFInMUoM", DB.RSFieldDecimal(rsPOQuantityList, "CFInMUoM").ToString()));
                string mspdata = String.Empty;
                if (mspvalue != 0)
                {
                    for (int i = 23; i < count; i++)
                    {
                        mspdata += rsPOQuantityList.GetValue(i).ToString() + ",";
                    }
                }
                if (mspdata.Length != 0)
                {
                    string mspcolvalues = mspdata.Remove(mspdata.Length - 1, 1);
                    sources.Add(new THHTWSData.KeyValueStruct("MspValues", mspcolvalues));
                }
                else
                {
                    sources.Add(new THHTWSData.KeyValueStruct("MspValues", String.Empty));
                }

            }
            return sources;
        }
        public UserDataTable GetMaterialReceived(string LineNumber, String MaterialCode, String StoreRefNo, String PONumber, string SupplierInvoieID)
        {
            UserDataTable userTable = new UserDataTable();
            int PoHeaderID;
            if (PONumber != "0")
            {
                PoHeaderID = DB.GetSqlN("select POHeaderID as N from ORD_POHeader where PONumber=" + DB.SQuote(PONumber));
            }
            else
            {
                PoHeaderID = 0;
            }
            int vMMID = DB.GetSqlN("select MaterialMasterID as N from MMT_MaterialMaster where MCode=" + DB.SQuote(MaterialCode));
            int vInboundID = DB.GetSqlN("select InboundID as N from INB_Inbound where StoreRefNo=" + DB.SQuote(StoreRefNo));
            DataSet ds = DB.GetDS("EXEC  [sp_INV_GetGoodsMovementDetailsList]@MaterialMasterID=" + vMMID + ",@InboundID=" + vInboundID + ",@LineNumber=" + LineNumber + ",@POHeaderID=" + PoHeaderID + ",@SupplierInvoiceID=" + SupplierInvoieID, false);
            DataTable dt = ds.Tables[0];
            userTable.Table = dt;
            return userTable;
        }


        private void ThermalLabelWorkerForGoodsIN(object thisMLabel2)
        {
            try
            {
                MRLWMSC21Common.TracklineMLabel thisMLabel = (MRLWMSC21Common.TracklineMLabel)thisMLabel2;
                String vresult = String.Empty;
                // CommonLogic.SendPrintJob_Big_7p6x5(thisMLabel.MCode, thisMLabel.AltMCode, thisMLabel.OEMPartNo, thisMLabel.Description, thisMLabel.BatchNo, thisMLabel.SerialNo, thisMLabel.KitPlannerID.ToString(), thisMLabel.KitChildrenCount, thisMLabel.ParentMCode,1, thisMLabel.MfgDate, thisMLabel.ExpDate, thisMLabel.PrinterType, thisMLabel.PrinterIP, thisMLabel.StrRefNo, thisMLabel.OBDNumber, thisMLabel.KitCode, thisMLabel.ReqNo, thisMLabel.IsBoxLabelReq,true,thisMLabel.PrintQty, out vresult);
                thisMLabel.Result = vresult;
            }
            catch (Exception ex)
            {

                return;
            }
        }
        private void ThermalLabelWorkerForGoodsOut(object thisMLabel2)
        {
            try
            {
                MRLWMSC21Common.TracklineMLabel thisMLabel = (MRLWMSC21Common.TracklineMLabel)thisMLabel2;
                String vresult = String.Empty;
                //CommonLogic.SendPrintJob_Big_7p6x5(thisMLabel.MCode, thisMLabel.AltMCode, thisMLabel.OEMPartNo, thisMLabel.Description, thisMLabel.BatchNo, thisMLabel.SerialNo, thisMLabel.KitPlannerID.ToString(), thisMLabel.KitChildrenCount, thisMLabel.ParentMCode, 1, thisMLabel.MfgDate, thisMLabel.ExpDate, thisMLabel.PrinterType, thisMLabel.PrinterIP, thisMLabel.StrRefNo, thisMLabel.OBDNumber, thisMLabel.KitCode, thisMLabel.ReqNo, thisMLabel.IsBoxLabelReq, true,thisMLabel.PrintQty, out vresult);
                thisMLabel.Result = vresult;
            }
            catch (Exception ex)
            {

                return;
            }
        }

        public string DeleteGoodsMomentDetails(String GoodsMomentID, int goodsMomentTypeID)
        {
            int isactive = DB.GetSqlTinyInt("SELECT ISNULL(IsActive,0) AS TI FROM INV_GoodsMovementDetails WHERE GoodsMovementDetailsID=" + GoodsMomentID + " AND IsDeleted=0");
            if (isactive == 1)
            {
                if (goodsMomentTypeID == 1)
                {
                    return "Shipment verified,Cannot Delete";
                }
                else
                {
                    return "Cannot delete, as PGI is updated ";
                }

            }
            try
            {
                DB.ExecuteSQL("EXEC [sp_INV_DeleteGoodsMovementDetails] @GoodsMovementDetailsIDs=" + DB.SQuote(GoodsMomentID) + ", @GoodsMovementTypeID=" + goodsMomentTypeID);
                return "Successfully Deleted";
            }
            catch
            {
                return "Failed to delete selected details";
            }
        }




        #endregion



        #region -------------------------Goods Out-----------------------------


        public List<THHTWSData.KeyValueStruct> GetSOItemProperties(String OBDNumber, String MaterialCode, String Location, int LineNumber, String SONumber)
        {
            List<THHTWSData.KeyValueStruct> sources = new List<THHTWSData.KeyValueStruct>();
            try
            {

                int vMMID = DB.GetSqlN("select MaterialMasterID as N from MMT_MaterialMaster where MCode=" + DB.SQuote(MaterialCode));
                int vDocumneTypeID = DB.GetSqlN("select DocumentTypeID as N from OBD_Outbound where OBDNumber=" + DB.SQuote(OBDNumber));

                int CycleCountOn = DB.GetSqlN("select convert(int,qcc.IsOn) as N from QCC_CycleCount qcc left join QCC_CycleCountDetails qccd on qccd.CycleCountID=qcc.CycleCountID  where qccd.MaterialMasterID=" + vMMID);


                int vObdID = DB.GetSqlN("select OutboundID as N from OBD_Outbound where OBDNumber=" + DB.SQuote(OBDNumber));
                if (SONumber != "0")
                {
                    int SoHeaderID = DB.GetSqlN("select SOHeaderID as N from ORD_SOHeader where SONumber=" + DB.SQuote(SONumber));
                    SONumber = SoHeaderID.ToString();

                }
                string vSqlForCheckingDuplicates = "exec [dbo].[sp_INV_GetSOQuantityListForOBDNumber_ForHHT] @MatrialMastrID=" + vMMID + ",@LineNumber=" + LineNumber + ",@ObdID=" + vObdID + ",@SOHeaderID=" + SONumber;
                DataSet ds = DB.GetDS(vSqlForCheckingDuplicates, false);
                int rowCount = ds.Tables[0].Rows.Count;
                if (rowCount > 1)
                {
                    return sources;
                }
                else if (rowCount == 1)
                {
                    int vSoid = (int)ds.Tables[0].Rows[0][0];
                    int vKitid = (int)ds.Tables[0].Rows[0][1];
                    int vLocationId = DB.GetSqlN("select LocationID as N from  INV_Location where Location=" + DB.SQuote(Location));
                    IDataReader rsSOQuantityList;
                    if (vKitid != 0)
                    {
                        rsSOQuantityList = DB.GetRS("EXEC [sp_INV_GetSOQuantityListForOBDNumber]@MaterialMasterID=" + vMMID + ",@OuboundID=" + vObdID + ",@LocationID=" + vLocationId + ",@LineNumber=" + LineNumber + ",@kitPlannerID=" + vKitid + ",@SOHeaderID=" + vSoid);
                    }
                    else
                    {
                        rsSOQuantityList = DB.GetRS("EXEC [sp_INV_GetSOQuantityListForOBDNumber]@MaterialMasterID=" + vMMID + ",@OuboundID=" + vObdID + ",@LocationID=" + vLocationId + ",@LineNumber=" + LineNumber + ",@kitPlannerID=NULL,@SOHeaderID=" + vSoid);

                    }
                    sources.Clear();
                    while (rsSOQuantityList.Read())
                    {
                        sources.Add(new THHTWSData.KeyValueStruct("SOHeaderID", DB.RSFieldInt(rsSOQuantityList, "SOHeaderID").ToString()));
                        sources.Add(new THHTWSData.KeyValueStruct("MaterialMasterID", DB.RSFieldInt(rsSOQuantityList, "MaterialMasterID").ToString()));
                        sources.Add(new THHTWSData.KeyValueStruct("KitPlannerID", DB.RSFieldInt(rsSOQuantityList, "KitPlannerID").ToString()));
                        sources.Add(new THHTWSData.KeyValueStruct("MCode", DB.RSField(rsSOQuantityList, "MCode").ToString()));
                        sources.Add(new THHTWSData.KeyValueStruct("MDescription", DB.RSField(rsSOQuantityList, "MDescription").ToString()));
                        sources.Add(new THHTWSData.KeyValueStruct("BUoM", DB.RSField(rsSOQuantityList, "BUoM").ToString()));
                        sources.Add(new THHTWSData.KeyValueStruct("BUoMQty", DB.RSFieldDecimal(rsSOQuantityList, "BUoMQty").ToString()));
                        sources.Add(new THHTWSData.KeyValueStruct("MaterialMaster_SUoMID", DB.RSFieldInt(rsSOQuantityList, "MaterialMaster_SUoMID").ToString()));
                        sources.Add(new THHTWSData.KeyValueStruct("MUoM", DB.RSField(rsSOQuantityList, "MUoM").ToString()));
                        sources.Add(new THHTWSData.KeyValueStruct("MUoMQty", DB.RSFieldDecimal(rsSOQuantityList, "MUoMQty").ToString()));
                        sources.Add(new THHTWSData.KeyValueStruct("SUoM", DB.RSField(rsSOQuantityList, "SUoM").ToString()));
                        sources.Add(new THHTWSData.KeyValueStruct("SUoMQty", DB.RSFieldDecimal(rsSOQuantityList, "SUoMQty").ToString()));
                        sources.Add(new THHTWSData.KeyValueStruct("CustPOUoM", DB.RSField(rsSOQuantityList, "CustPOUoM").ToString()));
                        sources.Add(new THHTWSData.KeyValueStruct("CustPOUoMQty", DB.RSFieldDecimal(rsSOQuantityList, "CustPOUoMQty").ToString()));
                        sources.Add(new THHTWSData.KeyValueStruct("MaterialGroup", DB.RSField(rsSOQuantityList, "MaterialGroup").ToString()));
                        sources.Add(new THHTWSData.KeyValueStruct("SOQuantity", DB.RSFieldDecimal(rsSOQuantityList, "SOQuantity").ToString()));
                        sources.Add(new THHTWSData.KeyValueStruct("LocationID", vLocationId.ToString()));
                        sources.Add(new THHTWSData.KeyValueStruct("ISCycleCountON", CycleCountOn.ToString()));
                        sources.Add(new THHTWSData.KeyValueStruct("OBDID", vObdID.ToString()));
                        sources.Add(new THHTWSData.KeyValueStruct("SODetailsID", DB.RSFieldInt(rsSOQuantityList, "SODetailsID").ToString()));
                        sources.Add(new THHTWSData.KeyValueStruct("DocumentType", vDocumneTypeID.ToString()));

                        sources.Add(new THHTWSData.KeyValueStruct("MeasurementTypeID", DB.RSFieldInt(rsSOQuantityList, "MeasurementTypeID").ToString()));
                        sources.Add(new THHTWSData.KeyValueStruct("CF", DB.RSFieldDecimal(rsSOQuantityList, "CF").ToString()));
                        sources.Add(new THHTWSData.KeyValueStruct("CFInMUoM", DB.RSFieldDecimal(rsSOQuantityList, "CFInMUoM").ToString()));

                    }

                    return sources;

                }
                else
                {
                    sources.Add(new THHTWSData.KeyValueStruct("Error", "No Data Found"));
                }
            }
            catch
            {
                sources.Add(new THHTWSData.KeyValueStruct("Error", "No Data Found"));
            }
            return sources;
        }
        public UserDataTable GetAvailbleQtyList(int OBDID, int MaterialMasterID, int LocationID, int LineNumber, int KitPlannerID, int SoHeaderID, int containerId)
        {


            UserDataTable userTable = new UserDataTable();
            DataSet dataset;
            dataset = DB.GetDS("EXEC [sp_INV_GetStockInList]  @MaterialMasterID=" + MaterialMasterID + ", @LocationID=" + LocationID + ",@KitPlannerID=" + (KitPlannerID != 0 ? KitPlannerID.ToString() : "NULL") + ",@OutBoundID=" + OBDID + ",@SOHeaderID=" + SoHeaderID + ",@LineNumber=" + LineNumber + ",@CartonId=" + containerId, false);

            DataTable table = dataset.Tables[0];
            userTable.Table = table;
            return userTable;

        }
        public UserDataTable GetMaterialPicked(int MaterialMasterID, int LocationID, int OBDID, int SOHeaderID, int LineNumber)
        {
            UserDataTable userTable = new UserDataTable();
            DataSet ds = DB.GetDS("EXEC [sp_INV_GetStockOutList]@MaterialMasterID=" + MaterialMasterID + ", @LocationID=" + LocationID + ", @OutboundID=" + OBDID + ", @linenumber=" + LineNumber + ",@SOHeaderID=" + SOHeaderID, false);
            DataTable dt = ds.Tables[0];
            userTable.Table = dt;

            return userTable;
        }
        public string UpdateGoosOUT(string GoodsMovementDetailsID, string GoodsMovementTypeID, int LocationID, string Location, string MaterialMaster_IUoMID, string DocQty, string Quantity, string SOQty, int OBDID, string LineNumber, int MaterialMasterID, string mcode, string IsDamaged, string hasDiscrepancy, string POSOHeaderID, string CreatedBy, string MaterialStorageParameterNames, string MaterialStorageParameterValues, string Conversionfactor, string remarks, string LastModifiedBy, string IsActive, int KitplannerID, int TenantID, string SODetailsID, string ISNC, string asIS, int IsPrintRequired, string printerIP, string IsPositiveRecall, string cartonid)
        {


            int vDeliveryStatus = DB.GetSqlN("SELECT DeliveryStatusID AS N FROM OBD_Outbound WHERE OutboundID=" + OBDID);
            if (vDeliveryStatus > 3)
            {
                return "Cannot pick the item,as PGI is done";
            }
            Decimal StockoutSum = DB.GetSqlNDecimal("EXEC dbo.sp_INV_GetGoodsOutQuantitySum @MaterialMasterID=" + MaterialMasterID + ",@LocationID=" + LocationID + ",@OutboundID=" + OBDID + ",@linenumber=" + LineNumber + ",@SOHeaderID=" + POSOHeaderID);
            Decimal totalQuantity = Convert.ToDecimal(SOQty);
            if (StockoutSum + Convert.ToDecimal(DocQty) > totalQuantity)
            {

                return "Pick quantity should not exceed delv. doc. quantity";
            }
            string MaterialStorageParameterIDs = String.Empty;
            if (MaterialStorageParameterNames.Length != 0)
            {
                try
                {
                    /*IDataReader rsGetData = DB.GetRS("EXEC [sp_INV_GetMaterialStorageParameters] @MCode='" + mcode + "',@TenantID="+TenantID);
                    while (rsGetData.Read())
                    {
                        MaterialStorageParameterIDs += DB.RSFieldInt(rsGetData, "MaterialStorageParameterID").ToString()+",";
                     
                    }*/
                    IDataReader rsGetData = DB.GetRS("select MaterialStorageParameterID from MMT_MaterialStorageParameter where ParameterName in (" + MaterialStorageParameterNames + ") order by MaterialStorageParameterID");
                    while (rsGetData.Read())
                    {
                        MaterialStorageParameterIDs += DB.RSFieldInt(rsGetData, "MaterialStorageParameterID").ToString() + ",";

                    }
                }
                catch
                {
                    return "Error in picking item, please contact support team";
                }
                MaterialStorageParameterIDs = MaterialStorageParameterIDs.Remove(MaterialStorageParameterIDs.Length - 1, 1);
            }
            try
            {
                if (GoodsMovementDetailsID == "0")
                {
                    int result = InventoryCommonClass.UpdateGoodsMovementDetails(GoodsMovementDetailsID, GoodsMovementTypeID, Location, MaterialMaster_IUoMID, DocQty, Quantity, OBDID.ToString(), "0", LineNumber, MaterialMasterID.ToString(), IsDamaged, hasDiscrepancy, POSOHeaderID, CreatedBy, MaterialStorageParameterIDs, MaterialStorageParameterValues, Conversionfactor, remarks, LastModifiedBy, IsActive, KitplannerID.ToString(), IsPositiveRecall, ISNC, asIS, cartonid);
                    if (result != 1)
                    {
                        return "Error in picking item,Please contact support team";
                    }
                    try
                    {
                        if (IsPrintRequired == 1)
                        {
                            MRLWMSC21Common.TracklineMLabel thisMLabel = new MRLWMSC21Common.TracklineMLabel();
                            int vMMID = 0;// DB.GetSqlN("select MaterialMasterID as N from MMT_MaterialMaster where MCode=" + DB.SQuote(mcode));
                            string OEMPartNumber = String.Empty;
                            string Description = String.Empty;
                            string DelDocNumber = String.Empty;
                            string sqlcommandforMMDetails = "select MaterialMasterID,isnull(OEMPartNo,'') as OEMPartNo,isnull(MDescription,'') as MDescription ,(select OBDNumber from OBD_Outbound where OutboundID=" + OBDID + ") as DeliveryDocNumber from MMT_MaterialMaster where MCode=" + DB.SQuote(mcode);
                            IDataReader dataReader = DB.GetRS(sqlcommandforMMDetails);
                            while (dataReader.Read())
                            {
                                vMMID = DB.RSFieldInt(dataReader, "MaterialMasterID");
                                OEMPartNumber = DB.RSField(dataReader, "OEMPartNo");
                                Description = DB.RSField(dataReader, "MDescription");
                                DelDocNumber = DB.RSField(dataReader, "DeliveryDocNumber");

                            }
                            if (IsPositiveRecall == "1")
                            {
                                thisMLabel.MCode = mcode + "-P";
                            }
                            else
                            {
                                thisMLabel.MCode = mcode;
                            }

                            thisMLabel.OEMPartNo = OEMPartNumber;
                            thisMLabel.Description = Description;
                            thisMLabel.AltMCode = String.Empty;


                            string kitcode = String.Empty;
                            IDataReader dr = DB.GetRS("[sp_MFG_ProductionDeliveryPickNoteDetails]  @OutboundID=" + OBDID);
                            while (dr.Read())
                            {
                                kitcode = DB.RSField(dr, "KitCode");
                            }

                            string batchNo = String.Empty;
                            string serialNo = String.Empty;
                            DateTime mfgDate = DateTime.MinValue;
                            DateTime ExpDate = DateTime.MinValue;
                            string[] msps = MaterialStorageParameterIDs.Split(',');
                            string[] mspvalues = MaterialStorageParameterValues.Split(',');
                            for (int index = 0; index < msps.Length; index++)
                            {
                                if (msps[index] == "1")
                                {
                                    mfgDate = DateTime.ParseExact(mspvalues[index], "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                    //mfgDate = Convert.ToDateTime(mspvalues[index]);
                                }
                                else if (msps[index] == "2")
                                {
                                    ExpDate = DateTime.ParseExact(mspvalues[index], "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                }
                                else if (msps[index] == "3")
                                {
                                    serialNo = mspvalues[index];
                                }
                                else if (msps[index] == "4")
                                {
                                    batchNo = mspvalues[index];
                                }
                            }

                            thisMLabel.MfgDate = mfgDate.ToString();
                            thisMLabel.ExpDate = ExpDate.ToString();

                            thisMLabel.BatchNo = batchNo;
                            thisMLabel.SerialNo = serialNo;

                            thisMLabel.InvQty = Convert.ToDecimal(DocQty);
                            thisMLabel.PrintQty = DocQty;


                            thisMLabel.PrinterType = "IP";
                            thisMLabel.PrinterIP = printerIP;
                            thisMLabel.IsBoxLabelReq = true;
                            thisMLabel.ReqNo = String.Empty;

                            thisMLabel.StrRefNo = String.Empty;
                            thisMLabel.OBDNumber = DelDocNumber;
                            thisMLabel.IsBoxLabelReq = false;

                            thisMLabel.KitCode = kitcode;
                            CommonLogic.PrintLabel(thisMLabel);
                            //Thread worker = new Thread(new ParameterizedThreadStart(this.ThermalLabelWorkerForGoodsOut));
                            //worker.SetApartmentState(ApartmentState.STA);
                            //worker.Name = "ThermalLabelWorker";
                            //worker.Start(thisMLabel);
                            //worker.Join();
                            //if (thisMLabel.Result != "Success")
                            //{
                            //    return "successwithnoprint"; //"Item picked successfully, unable to print the label due to printer unavailability. Please check the printer.";

                            //}
                            //else
                            //{
                            //    return "success";

                            //}

                        }

                    }
                    catch
                    {
                        return "successwithnoprint";//Item picked successfully, unable to print the label due to printer unavailability. Please check the printer.";
                    }


                    return "success";
                }
                else
                {
                    InventoryCommonClass.UpdateReturnGoodsMovementDetails(GoodsMovementDetailsID, Quantity, DocQty, OBDID.ToString(), SODetailsID, Conversionfactor, MaterialMaster_IUoMID, CreatedBy, cartonid);


                    return "success";
                }


            }
            catch
            {
                return "Error in picking item,Please contact support team";
            }
        }


        #endregion


        public UserDataTable GetOBDPickedList(BO.Outbound outbound,out string Result)
        {
           
            BL.OutboundHanlder outboundHanlder = new BL.OutboundHanlder();          
            return outboundHanlder.GetOBDPickedList(outbound,out Result);
        }
        #region -------------------------Internal Transfer-----------------------------


        public UserDataTable GetStockListForInterTransfer(String MaterialCode, String Location)
        {
            UserDataTable userTable = new UserDataTable();
            int vMMID = DB.GetSqlN("select MaterialMasterID as N from MMT_MaterialMaster where MCode=" + DB.SQuote(MaterialCode));
            int vLocationId = DB.GetSqlN("select isnull(LocationID,0) as N from  INV_Location where Location=" + DB.SQuote(Location));
            DataSet dataset = DB.GetDS(" exec sp_INV_GetAvailableStockForMaterila	@MaterialMasterID=" + vMMID + ",@locationid=" + vLocationId, false);
            DataTable datatable = dataset.Tables[0];
            userTable.Table = datatable;
            return userTable;
        }
        public string UpsertInternalTransfer(string vGMDIDs, string Location, int UserID, decimal vQuantity, string vMcode)
        {
            int vLocationId = DB.GetSqlN("select isnull(LocationID,0) as N from  INV_Location where Location=" + DB.SQuote(Location));

            if (DB.GetSqlN("sp_INV_IsInSupplierReturns @GoodsMovementDetailsIDs=" + DB.SQuote(vGMDIDs)) != 0)
            {
                return "Material in Supplier Returns, cannot transfer";

            }
            int CycleCountOn = DB.GetSqlN("select convert(int,qcc.IsOn) as N from QCC_CycleCount qcc left join QCC_CycleCountDetails qccd on qccd.CycleCountID=qcc.CycleCountID  where qccd.MaterialMasterID=(select MaterialMasterID from MMT_MaterialMaster where MCode=" + DB.SQuote(vMcode) + " and IsActive=1 and IsDeleted=0)");
            if (CycleCountOn == 1)
            {
                return "This material is in Cycle Count,Cannot receive";
            }

            if (vLocationId != 0)
            {
                try
                {
                    DB.ExecuteSQL("EXEC [dbo].[sp_INV_UpsertInternalTransfer] @GoodsMovementDetailsIDs=" + DB.SQuote(vGMDIDs) + ", @Location=" + DB.SQuote(Location) + ", @Quantity=" + vQuantity + ", @CreatedBy=" + UserID);
                    return "1";
                }
                catch
                {
                    return "Error in transfer,Please contact support team.";
                }
            }
            else
            {
                return "Please provide valid location";
            }


        }


        #endregion



        #region -------------------------Cycle Count-----------------------------


        public List<THHTWSData.KeyValueStruct> GetUOMData(String Mcode)
        {
            List<THHTWSData.KeyValueStruct> sources = new List<THHTWSData.KeyValueStruct>();
            int MID = DB.GetSqlN("select MaterialMasterID as N from MMT_MaterialMaster where MCode=" + DB.SQuote(Mcode));
            string sqlstring = "EXEC [dbo].[sp_CC_GetUOMData]@MaterialMasterID=" + MID;
            //"select convert(nvarchar,convert(nvarchar,GUOM.UoM)+'/'+convert(nvarchar,MUOM.UoMQty)) as UOM,convert(nvarchar,convert(nvarchar,MUOM.UoMTypeID)+'|'+convert(nvarchar,MUOM.UoMID)+'|'+convert(nvarchar,MUOM.MaterialMaster_UoMID)) as ID from MMT_MaterialMaster_GEN_UoM MUOM join GEN_UoM GUOM on MUOM.UoMID=GUOM.UoMID where MaterialMasterID=" + MID + " and MUOM.IsDeleted=0 order by UoMTypeID";
            IDataReader rsGetData = DB.GetRS(sqlstring);
            sources.Clear();
            sources.Add(new THHTWSData.KeyValueStruct("0", "Select"));
            while (rsGetData.Read())
            {
                sources.Add(new THHTWSData.KeyValueStruct(rsGetData[1].ToString(), rsGetData[0].ToString()));
            }
            rsGetData.Close();

            return sources;
        }
        public string CycleCountCapture(String Mcode, int CCID, String Location, int KitID, Decimal CountQty, String UOMData, String MspIdList, String MspValueList, int UserID, string IpAddress, int IsDamaged)
        {

            //Checking For Valid CycleCount
            String sql = "select isnull(CycleCountID,0) as N from QCC_CycleCount where CycleCountID=" + CCID + "and ison=1";
            try
            {
                IDataReader reader = DB.GetRS(sql);
                if (reader.Read())
                {
                    //Query For Getting MaterialMasterID
                    int MID = DB.GetSqlN("select MaterialMasterID as N from MMT_MaterialMaster where MCode=" + DB.SQuote(Mcode));

                    //Query For CycleCountDetailsID
                    int cyclecountdetailsid = DB.GetSqlN(" select CycleCountDetailsID as N from QCC_CycleCountDetails where CycleCountID=" + CCID + " and MaterialMasterID=" + MID);
                    if (cyclecountdetailsid == 0)
                    {
                        return "This material is not includen in this CycleCount";
                    }
                    //Query For Getting LocationID
                    int LocID = DB.GetSqlN("Select LocationID as N from INV_Location where Location=" + DB.SQuote(Location));
                    if (LocID == 0)
                    {
                        return "Please enter valid location";
                    }
                    //For Capturing PhysicalQty in DataBase
                    decimal vPhysicalQuantity = 0;
                    string queryForGetPhysicalQuantity = "declare @ddd decimal(18,2); " +
                             " EXEC [sp_INV_GetStockInListQty]  @ST_MaterialMasterID=" + MID + ", @ST_LocationID=" + LocID + ", " +
                             "@ST_MaterialStorageParameterIDs=" + DB.SQuote(MspIdList) + ",@ST_MaterialStorageParameterValues=" + DB.SQuote(MspValueList) + ", " +
                             "@ST_IsDamaged=" + IsDamaged;
                    try
                    {
                        vPhysicalQuantity = DB.GetSqlNDecimal(queryForGetPhysicalQuantity);
                    }
                    catch
                    {
                        return "Error in getting quantity";
                    }

                    String[] UOMDataSplit = UOMData.Split('|');
                    decimal vConvertionFactorValue = decimal.Parse(UOMDataSplit[0]);
                    string vConvertionFactor = UOMDataSplit[1];
                    decimal ConvertoBuomQty = vConvertionFactorValue * CountQty;
                    /*
                     IDataReader rs = DB.GetRS("select GUOM.UoMID,UoMQty,UOM.UoM from MMT_MaterialMaster_GEN_UoM GUOM join GEN_UoM UOM on GUOM.UoMID=UoM.UoMID  where MaterialMasterID=" + MID + " and GUOM.IsDeleted=0 and UoMTypeID in(1,2)");
                     String SelectedUOM = "";
                     Decimal SelectedUOMQty = 0;

                     IDataReader rsforSelUOMperQty = DB.GetRS("select UOM.UoM,UoMQty from MMT_MaterialMaster_GEN_UoM GUOM  join GEN_UoM UOM on GUOM.UoMID=UoM.UoMID where MaterialMaster_UoMID=" + SelectdMUOMID + " and GUOM.IsDeleted=0");
                     Decimal CountQtyInBUOM = 0;
                     while (rsforSelUOMperQty.Read())
                     {
                         SelectedUOM = DB.RSField(rsforSelUOMperQty, "UoM");
                         SelectedUOMQty = DB.RSFieldDecimal(rsforSelUOMperQty, "UoMQty");
                     }
                     int BUOMID = 0;
                     String BUOM;
                     Decimal BUOMQty = 0;
                     int MUOMID = 0;
                     Decimal MUOMQty = 0;
                     String MUOM = "";
                     while (rs.Read())
                     {
                         if (BUOMID == 0)
                         {
                             BUOMID = DB.RSFieldInt(rs, "UoMID");
                             BUOMQty = DB.RSFieldDecimal(rs, "UoMQty");
                             BUOM = DB.RSField(rs, "UoM");
                         }
                         else
                         {

                             MUOMID = DB.RSFieldInt(rs, "UoMID");
                             MUOMQty = DB.RSFieldDecimal(rs, "UoMQty");
                             MUOM = DB.RSField(rs, "UoM");
                         }
                     }
                     if (SelectedUOMID == BUOMID)
                     {
                         conversionfactor = "1";
                         //CountQtyInBUOM = CountQty; ;
                     }
                     else if (SelectedUOMID == MUOMID)
                     {
                         conversionfactor = MUOM + ":" + MUOMQty;

                         //CountQtyInBUOM = MUOMQty * CountQty;
                     }
                     else if (BUOMID == MUOMID)
                     {
                         conversionfactor = SelectedUOM + ":" + SelectedUOMQty;

                         //CountQtyInBUOM = SelectedUOMQty * CountQty;
                     }
                     else
                     {
                         conversionfactor = MUOM + ":" + MUOMQty + "*" + SelectedUOM + ":" + SelectedUOMQty;

                         //CountQtyInBUOM = (MUOMQty * SelectedUOMQty) * CountQty;
                     }
                     */

                    try
                    {
                        string SqlForInsertCCCapture = "declare @NewResult int Exec [dbo].[sp_CC_InsertCycleCountCapture]@CycleCountDetailsID=" + cyclecountdetailsid + ",@KitPlannerID=NULL,@LocationID=" + LocID + ",@CapturedQty=" + vPhysicalQuantity + ",@CountUoMID=NULL,@ConversionFactor=" + DB.SQuote(vConvertionFactor) + ",@CountQty=" + CountQty + ",@Quantity=" + ConvertoBuomQty + ",@CountedBy=" + UserID + ",@HandlerIP=" + DB.SQuote(IpAddress) + ",@Remarks=null,@MaterialStorageParameterIDs=" + DB.SQuote(MspIdList) + ",@Values=" + DB.SQuote(MspValueList) + ",@CreatedBy=" + UserID + ",@IsDamaged=" + IsDamaged + ",@Result=@NewResult OUTPUT SELECT @NewResult AS N";
                        DB.GetSqlN(SqlForInsertCCCapture);
                        return "1";
                    }
                    catch
                    {
                        return "Failed In Capturing Details";
                    }
                }
                else
                {
                    return "Please provide valid CycleCountID";
                }
            }
            catch
            {
                return "Error in Getting CCID";
            }

        }


        #endregion

        #region -------------------------Updates-----------------------------


        public string GetReleaseVersion()
        {
            String retResult = String.Empty;

            string sql = "select top 1 isnull(Version+'|'+convert(nvarchar,ReleaseDate,103)+'|'+OS,'') as S from HHTRelease order by version desc";
            retResult = DB.GetSqlS(sql);
            return retResult;


            //return retResult;
        }



        #endregion



        public string UpdateEPCCodes(string Codes)
        {
            string Query = "insert into Temp_RFDetails(ScanedRFID) " +
                "select value from dbo.udf_Split(" + DB.SQuote(Codes) + ",',') data " +
                "left join Temp_RFDetails ddd on ddd.ScanedRFID=data.Value " +
                "where Value <> '' and ddd.ScanedRFID is null";
            try
            {
                DB.ExecuteSQL(Query);
            }
            catch
            {
                return "0";
            }
            return "Success";
        }

        public int CartonValidatorAtPutaway(string StoreRefNo, string ContainerCode)
        {

            if (StoreRefNo != null && !StoreRefNo.Trim().Equals("") && ContainerCode != null && !ContainerCode.Trim().Equals(""))
            {
                StringBuilder drlStatement = new StringBuilder(" EXEC [dbo].[sp_Inv_CartonValidationAtPutaway] @StoreRefNo=" + DB.SQuote(StoreRefNo) + " , @CartonCode=" + DB.SQuote(ContainerCode));
                return DB.GetSqlN(drlStatement.ToString());
            }

            return 0;
        }
        public string GetCurrentLocation(string ContainerCode)
        {
            string location = DB.GetSqlS("EXEC [dbo].[sp_INV_GetCurrentLocation] @CartonCode=" + DB.SQuote(ContainerCode));

            return location;

        }


        public UserDataTable GetStockListForInterTransferFromContainer(String MaterialCode, String CartonCode)
        {
            UserDataTable userTable = new UserDataTable();
            int vMMID = DB.GetSqlN("select MaterialMasterID as N from MMT_MaterialMaster where MCode=" + DB.SQuote(MaterialCode));
            //int vLocationId = DB.GetSqlN("select isnull(LocationID,0) as N from  INV_Location where Location=" + DB.SQuote(Location));
            int vCartonID = DB.GetSqlN("select isnull(CartonID,0) as N from  INV_Carton where CartonCode=" + DB.SQuote(CartonCode));

            //DataSet dataset = DB.GetDS(" exec sp_INV_GetAvailableStockForMaterila	@MaterialMasterID=" + vMMID + ",@CartonID=" + vCartonID, false);
            DataSet dataset = DB.GetDS(" exec sp_INV_GetAvailableStockForMaterial_HHT	@MCode=" + DB.SQuote(MaterialCode) + ",@CartonID=" + vCartonID, false);


            DataTable datatable = dataset.Tables[0];
            userTable.Table = datatable;
            return userTable;
        }
        public string UpsertInternalTransferContainerCode(string vGMDIDs, string Location, int UserID, decimal vQuantity, string vMcode, string toCartonCode)
        {

            //String actualLocation = DB.GetSqlS("select  DISTINCT l.Location as S from INV_Carton as car join INV_CartonGoodsMovementDetails  cargood on car.CartonID=cargood.CartonID join INV_GoodsMovementDetails gmd on cargood.GoodsMovementDetailsID=gmd.GoodsMovementDetailsID join INV_Location l on gmd.LocationID=l.LocationID where car.CartonCode=" + DB.SQuote(Location));

            int vLocationId = DB.GetSqlN("select isnull(LocationID,0) as N from  INV_Location where Location=" + DB.SQuote(Location));

            if (DB.GetSqlN("sp_INV_IsInSupplierReturns @GoodsMovementDetailsIDs=" + DB.SQuote(vGMDIDs)) != 0)
            {
                return "Material in supplier returns, cannot transfer";

            }
            int CycleCountOn = DB.GetSqlN("select convert(int,qcc.IsOn) as N from QCC_CycleCount qcc left join QCC_CycleCountDetails qccd on qccd.CycleCountID=qcc.CycleCountID  where qccd.MaterialMasterID=(select top 1 MaterialMasterID from MMT_MaterialMaster where MCode=" + DB.SQuote(vMcode) + " and IsActive=1 and IsDeleted=0)");
            if (CycleCountOn == 1)
            {
                return "This material is in Cycle Count,Cannot receive";
            }

            if (vLocationId != 0)
            {
                try
                {
                    int result = DB.GetSqlN("EXEC [dbo].[sp_INV_UpsertInternalTransfer_TC] @GoodsMovementDetailsIDs=" + DB.SQuote(vGMDIDs) + ", @Location=" + DB.SQuote(Location) + ", @Quantity=" + vQuantity + ", @CreatedBy=" + UserID + ",@CartonCode=" + DB.SQuote(toCartonCode));
                    return (result == 1) ? "1" : "This container is already assigned to other tenant";
                }
                catch
                {
                    return "Error in transfer,Please contact support team.";
                }
            }
            else
            {
                return "Please provide valid location";
            }


        }


        public int CheckContainerForInternalTransfer(string cartonCode)
        {
            string drlStatement = "EXEC [dbo].[sp_CheckContainerLocation] @CartonCode=" + DB.SQuote(cartonCode);
            int result = DB.GetSqlN(drlStatement);
            return result;
        }
        public string SKuTransfer(string cartonCode, string locationCode, int userid)
        {
            string drlStatement = "EXEC [dbo].[SP_UPSERT_CONTAINERINTERNALTRANSFER] @ContainerCode=" + DB.SQuote(cartonCode) + ",@UserID=" + userid + ",@Location=" + DB.SQuote(locationCode);
            int result = DB.GetSqlN(drlStatement);
            switch (result)
            {
                case -1:
                    return "Invalid container scanned";
                case -2:
                    return "Having In Hold quantity in this Container";
                case -3:
                    return "Having out Hold quantity in this Container";
                case -4:
                    return "There is no available quantity in this Container";
                case 0:
                    return "Transfer completed";
                default:
                    return "Transfer completed";
            }
        }

        public string SaveBook(string book, string id)
        {
            return "Inventrax";
        }

        #region POD Services
        public string UploadFile(byte[] f, string fileName)
        {
            // the byte array argument contains the content of the file
            // the string argument contains the name and extension
            // of the file passed in the byte array
            try
            {
                // instance a memory stream and pass the
                // byte array to its constructor
                MemoryStream ms = new MemoryStream(f);

                // instance a filestream pointing to the
                // storage folder, use the original file name
                // to name the resulting file
                FileStream fs = new FileStream
                    (System.Web.Hosting.HostingEnvironment.MapPath
                    ("~/uploadedfile/") +
                    fileName, FileMode.Create);

                // write the memory stream containing the original
                // file as a byte array to the filestream
                ms.WriteTo(fs);

                // clean up
                ms.Close();
                fs.Close();
                fs.Dispose();

                // return OK if we made it this far
                return "OK";
            }
            catch (Exception ex)
            {
                // return the error message if the operation fails
                return ex.Message.ToString();
            }
        }

        public List<PODBO> GetJobOrderList(int Limit)
        {
            PODHandler pODHandler = new PODHandler();
            return pODHandler.GetJobOrderList(Limit);
        }
        public PODBO GetUserDetail(String UserName, String Password)
        {
            PODHandler pODHandler = new PODHandler();
            return pODHandler.GetUserDetails(UserName, Password);
        }

        public List<PODBO> GetMenuLinks()
        {
            PODHandler pODHandler = new PODHandler();
            return pODHandler.GetMenuLinks();
        }

        public List<PODBO> GetInboundStatistics()
        {
            PODHandler pODHandler = new PODHandler();
            return pODHandler.GetInboundStatistics();
        }
        public List<PODBO> GetOutboundStatistics()
        {
            PODHandler pODHandler = new PODHandler();
            return pODHandler.GetOutboundStatistics();
        }
        public List<PODBO> GetInOutActivities()
        {
            PODHandler pODHandler = new PODHandler();
            return pODHandler.GetInOutActivities();
        }
        public List<PODBO> GetInboundList(String WarehouseIDs, int ShipmentTypeID, int InboundStatusID, int SupplierID, int Limit, string InboundStatus)
        {
            PODHandler pODHandler = new PODHandler();
            return pODHandler.GetInboundList(WarehouseIDs, ShipmentTypeID, InboundStatusID, SupplierID, Limit, InboundStatus);
        }
        public List<PODBO> GetOutboundList(String WarehouseIDs, int DocumentTypeID, int OBDStatusID, int CustomerID, int Limit, string DeliveryStatus)
        {
            PODHandler pODHandler = new PODHandler();
            return pODHandler.GetOutboundList(WarehouseIDs, DocumentTypeID, OBDStatusID, CustomerID, Limit, DeliveryStatus);
        }
        public List<PODBO> GetJobOrderOutboundList(int Limit)
        {
            PODHandler pODHandler = new PODHandler();
            return pODHandler.GetJobOrderOutboundList(Limit);
        }
        public List<PODBO> GetInOutOperations()
        {
            PODHandler pODHandler = new PODHandler();
            return pODHandler.GetInOutOperations();
        }
        public List<PODBO> GetLiveStock(String MCode, String Location, String BatchNo, String PageSize)
        {
            PODHandler pODHandler = new PODHandler();
            return pODHandler.GetLiveStock(MCode, Location, BatchNo, PageSize);
        }
        public List<PODBO> GetWarehouses()
        {
            PODHandler pODHandler = new PODHandler();
            return pODHandler.GetWarehouses();
        }
        public List<PODBO> GetSuppliers()
        {
            PODHandler pODHandler = new PODHandler();
            return pODHandler.GetSuppliers();
        }
        public List<PODBO> GetShipmentStatus(int Limit)
        {
            PODHandler pODHandler = new PODHandler();
            return pODHandler.GetShipmentStatus(Limit);
        }
        public List<PODBO> GetDeliveryStatus()
        {
            PODHandler pODHandler = new PODHandler();
            return pODHandler.GetDeliveryStatus();
        }
        public List<PODBO> GetCustomers()
        {
            PODHandler pODHandler = new PODHandler();
            return pODHandler.GetCustomers();
        }
        public List<PODBO> GetDocumentType()
        {
            PODHandler pODHandler = new PODHandler();
            return pODHandler.GetDocumentType();
        }
        public List<PODBO> GetPartNumberList(int TenantID)
        {
            PODHandler pODHandler = new PODHandler();
            return pODHandler.GetPartNumberList(TenantID);
        }
        public List<PODBO> GetVersionDetails()
        {
            PODHandler pODHandler = new PODHandler();
            return pODHandler.GetVersionDetails();
        }
        //GetMaterialStorageParameters
        public List<PODBO> MaterialStorageParameters(String MaterialCode, int TenantID)
        {
            PODHandler pODHandler = new PODHandler();
            return pODHandler.GetMaterialStorageParameters(MaterialCode, TenantID);
        }
        //GetMaterialReceived
        public List<PODBO> MaterialReceived(string LineNumber, String MaterialCode, String StoreRefNo, String PONumber, string SupplierInvoieID)
        {
            PODHandler pODHandler = new PODHandler();
            return pODHandler.GetMaterialReceived(LineNumber, MaterialCode, StoreRefNo, PONumber, SupplierInvoieID);
        }
        public string UpdateReceiveItem(string GoodsMovementDetailsID, string GoodsMovementTypeID, string Location, string MaterialMaster_IUoMID, string DocQty, string Quantity, string Storerefnum, string LineNumber, string mcode, string IsDamaged, string hasDiscrepancy, string POSOHeaderID, string CreatedBy, string MaterialStorageParameterIDs, string MaterialStorageParameterValues, string Conversionfactor, string remarks, string LastModifiedBy, string IsActive, string KitplannerID, string InvoiceQuantity, string SupplierInvoiceID, string IsPositiveRecall, int IsPrintRequired, string printerIP, string serialNumber)
        {
            PODHandler pODHandler = new PODHandler();
            return pODHandler.UpdateReceiveItem(GoodsMovementDetailsID, GoodsMovementTypeID, Location, MaterialMaster_IUoMID, DocQty, Quantity, Storerefnum, LineNumber, mcode, IsDamaged, hasDiscrepancy, POSOHeaderID, CreatedBy, MaterialStorageParameterIDs, MaterialStorageParameterValues, Conversionfactor, remarks, LastModifiedBy, IsActive, KitplannerID, InvoiceQuantity, SupplierInvoiceID, IsPositiveRecall, IsPrintRequired, printerIP, serialNumber);
        }
        public List<PODBO> GetPOList(string StatusName)
        {
            PODHandler pODHandler = new PODHandler();
            return pODHandler.GetPOList(StatusName);
        }

        public List<PODBO> GetSOList(string StatusName)
        {
            PODHandler pODHandler = new PODHandler();
            return pODHandler.GetSOList(StatusName);
        }

        public List<PODBO> GetMaterialType(int TenantID)
        {
            PODHandler pODHandler = new PODHandler();
            return pODHandler.GetMaterialType(TenantID);
        }

        public List<PODBO> GetWHForWHList(int TenantID)
        {
            PODHandler pODHandler = new PODHandler();
            return pODHandler.GetWHForWHList(TenantID);
        }

        public List<PODBO> GetShipmentType()
        {
            PODHandler pODHandler = new PODHandler();
            return pODHandler.GetShipmentType();
        }

        public PODBO PODDelivery(PODBO OBDTrack)
        {
            PODHandler pODHandler = new PODHandler();
            return pODHandler.PODDelivery(OBDTrack);
        }

        public List<PODBO> GetPendingOutboundList(int UserID)
        {
            PODHandler pODHandler = new PODHandler();
            return pODHandler.GetPendingOutboundList(UserID);
        }

        public List<PODBO> GetCurrentStock(PODBO _requestPOBO)
        {
            PODHandler pODHandler = new PODHandler();
            return pODHandler.GetCurrentStock(_requestPOBO);
        }
        //CartonValidatorAtPutaway

        //DeleteGoodsMomentDetails

        //GetSOItemProperties

        //UpdateGoosOUT

        //GetPOItemProperties

        //GetMaterialPicked

        #endregion

       

        public UserDataTable GetMSPsReceiving(BO.PutAway putAway)
        {
            BL.PutAwayHandler putAwayHandler = new BL.PutAwayHandler();
            return putAwayHandler.GetMSPsReceiving(putAway);
        }

        //public BO.Outbound GetOBDSortingList(BO.Outbound outbound)
        //{

        //    //BL.OutboundHanlder inbounddetails = new BL.InboundHandler();
        //    //return inbounddetails.UpdateReceiveItem(inbound);

        //    BL.OutboundHanlder outboundHanlder = new BL.OutboundHanlder();
        //    //return outboundHanlder.


        //}

    }
}
