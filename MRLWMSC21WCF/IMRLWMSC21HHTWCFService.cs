using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Data;
using MRLWMSC21WCF.BO;
using MRLWMSC21Common;

namespace MRLWMSC21WCF
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IFalconHHTWCFService" in both code and config file together.
    [ServiceContract, XmlSerializerFormat]
    public interface IMRLWMSC21HHTWCFService
    {
        [OperationContract]
        void DoWork();
        [OperationContract]
        FalconHHTSData.LoginUserData GetUserDetails(String UserName, String Password, String LoginIPAddress, BO.ValidateUserLogin validateUserLogin);
        [OperationContract]
        List<THHTWSData.KeyValueStruct> GetMaterialStorageParameters(String MaterialCode, int TenantID);
        [OperationContract]
        List<THHTWSData.KeyValueStruct> GetDropDownData(String SqlString, String DefaultValue);
        [OperationContract]
        List<THHTWSData.KeyValueStruct> GetPOItemProperties(int LineNumber, string MaterialCode, string ATCRef_IB_OBD, string PONumber, string InvoiceNumber);
        [OperationContract]
        UserDataTable GetMaterialReceived(string LineNumber, String MaterialCode, String StoreRefNo, String PONumber, string SupplierInvoieID);

        //  [OperationContract]
        //string UpdateReceiveItem(string GoodsMovementDetailsID, string GoodsMovementTypeID, string Location, string MaterialMaster_IUoMID, string DocQty, string Quantity, string Storerefnum, string LineNumber, string mcode, string IsDamaged, string hasDiscrepancy, string POSOHeaderID, string CreatedBy, string MaterialStorageParameterIDs, string MaterialStorageParameterValues, string Conversionfactor, string remarks, string LastModifiedBy, string IsActive, string KitplannerID, string InvoiceQuantity, string SupplierInvoiceID, string IsPositiveRecall, int IsPrintRequired, string printerIP, string serialNumber);
        [OperationContract]
        string DeleteGoodsMomentDetails(String GoodsMomentID, int goodsMomentTypeID);
        [OperationContract]
        List<THHTWSData.KeyValueStruct> GetSOItemProperties(String OBDNumber, String MaterialCode, String Location, int LineNumber, String SONumber);
        [OperationContract]
        UserDataTable GetAvailbleQtyList(int OBDID, int MaterialMasterID, int LocationID, int LineNumber, int KitPlannerID, int SoHeaderID, int containerId);
        [OperationContract]
        UserDataTable GetMaterialPicked(int MaterialMasterID, int LocationID, int OBDID, int SOHeaderID, int LineNumber);
        [OperationContract]
        string UpdateGoosOUT(string GoodsMovementDetailsID, string GoodsMovementTypeID, int LocationID, string Location, string MaterialMaster_IUoMID, string DocQty, string Quantity, string SOQty, int OBDID, string LineNumber, int MaterialMasterID, string mcode, string IsDamaged, string hasDiscrepancy, string POSOHeaderID, string CreatedBy, string MaterialStorageParameterNames, string MaterialStorageParameterValues, string Conversionfactor, string remarks, string LastModifiedBy, string IsActive, int KitplannerID, int TenantID, string SODetailsID, string ISNC, string asIS, int IsPrintRequired, string printerIP, string IsPositiveRecall, string cartonid);
        [OperationContract]
        UserDataTable GetStockListForInterTransfer(String MaterialCode, String Location);
        [OperationContract]
        string UpsertInternalTransfer(string vGMDIDs, string Location, int UserID, decimal vQuantity, string vMcode);
        [OperationContract]
        List<THHTWSData.KeyValueStruct> GetUOMData(String Mcode);
        [OperationContract]
        string CycleCountCapture(String Mcode, int CCID, String Location, int KitID, Decimal CountQty, String UOMData, String MspIdList, String MspValueList, int UserID, string IpAddress, int IsDamaged);
        [OperationContract]
        string GetReleaseVersion();
        [OperationContract]
        GoodsInData1234 GoodsInData(string MaterialCode, int TenantID, int LineNumber, string ATCRef_IB_OBD, string PONumber, string InvoiceNumber);
        [OperationContract]
        List<THHTWSData.KeyValueData123> Test(String MaterialCode, int TenantID);

        [OperationContract]
        string UpdateEPCCodes(string Codes);
        [OperationContract]
        int CartonValidatorAtPutaway(string StoreRefNo, string ContainerCode);
        [OperationContract]
        string GetCurrentLocation(string ContainerCode);
        [OperationContract]
        string CartonPutaway(string cartonCode, string locationCode);
        [OperationContract]
        UserDataTable GetStockListForInterTransferFromContainer(String MaterialCode, String CartonCode);
        [OperationContract]
        string GetConatinerLocation(string Cartoncode, string WarehouseID);

        [OperationContract]
        string GetConatinerLocationBin(string Cartoncode, string WarehouseID);
        [OperationContract]
        BO.PutAway GetCurrentLocationForPutaway(BO.PutAway putAway);
        [OperationContract]
        string UpsertInternalTransferContainerCode(string vGMDIDs, string Location, int UserID, decimal vQuantity, string vMcode, string toCartonCode);
        [OperationContract]
        int CheckContainerForInternalTransfer(string cartonCode);
        [OperationContract]
        string SKuTransfer(string cartonCode, string locationCode, int userid);
        [OperationContract]
        UserDataTable GetStoreRefNos(BO.Inbound inbound);
        [OperationContract]
        UserDataTable GetDockVehicles(int inbound);
        [OperationContract]
        UserDataTable GetPoInvoiceNos(BO.Inbound inbound);
        [OperationContract]
        string CheckContainer(string CartonNo,string InboundID);
        [OperationContract]
        string CheckContainerOBD(string CartonNo, string OutboundID);
        [OperationContract]
        string CheckContainerStock(string CartonNo, string OutboundID);
        [OperationContract]
        string ValidateCartonLiveStock(BO.LiveStock liveStock);
        [OperationContract]
        UserDataTable GetWarehouse(BO.LiveStock liveStock);
        [OperationContract]
        BO.Inbound UpdateReceiveItemForHHT(BO.Inbound inbound);
        [OperationContract]
        UserDataTable GetStorageLocations();
        [OperationContract]
        UserDataTable GetobdRefNos(BO.Outbound outbound);
        [OperationContract]
        UserDataTable GetSONos(BO.Outbound outbound);
        [OperationContract]
        BO.TransferBO GetAvailbleQtyListforHHT(BO.TransferBO transferBO);
        [OperationContract]
        BO.Outbound UpdatePickItem(BO.Outbound outbound);
        [OperationContract]
        UserDataTable GetPoNos(BO.Inbound inbound);
        [OperationContract]
        string GetVLPDID(BO.VLPD VLPD);
        [OperationContract]
        BO.VLPD GetItemToPick(BO.VLPD VLPD);
        [OperationContract]
        UserDataTable GetTenants(BO.LiveStock liveStock);
        [OperationContract]
        UserDataTable GetLiveStockData(BO.LiveStock liveStock);
        [OperationContract]
        BO.VLPD UpsertPickItem(BO.VLPD Pickdata);
        [OperationContract]
        string CheckLocationForLiveStock(BO.LiveStock stock);
        [OperationContract]
        UserDataTable GetCCNames(BO.CycleCount cycleCount);
        [OperationContract]
        BO.CycleCount IsBlockedLocation(BO.CycleCount cycleCount);
        [OperationContract]
        List<BO.CycleCount> GetCycleCountInformation(BO.CycleCount cycleCount);
        [OperationContract]
        BO.CycleCount UpsertCycleCount(BO.CycleCount cycleCount);
        [OperationContract]
        BO.CycleCount ReleaseCycleCountLocation(BO.CycleCount cycleCount);
        [OperationContract]
        BO.CycleCount BlockLocationForCycleCount(BO.CycleCount cycleCount);
        [OperationContract]
        UserDataTable GetOpenVLPDNos(BO.VLPD vLPD);
        [OperationContract]
        UserDataTable GetOpenLoadsheetList(String TenantID, String AccountID);
        [OperationContract]
        UserDataTable GetPendingOBDListForLoading(String TenantID, String AccountID);
        [OperationContract]
        string UpsertLoadCreated(BO.VLPD VLPD);
        [OperationContract]
        string UpsertLoad(BO.VLPD VLPD);
        [OperationContract]
        string LoadVerification(String LoadNumber, String UserId);
        [OperationContract]
        UserDataTable GetOpenVLPDNosForSorting(BO.VLPD vLPD);

        [OperationContract]
        UserDataTable VLPDSorting(BO.VLPD vLPD);
        [OperationContract]
        BO.Inbound CheckDockInbound(BO.Inbound inbound);

        [OperationContract]
        BO.CycleCount GetBinCount(BO.CycleCount cyclecountinfo);
        [OperationContract]
        UserDataTable GetTransferOrderNos(BO.TransferBO transferInfo);
        [OperationContract]
        string GetTransferID(BO.TransferBO transferBO);
        [OperationContract]
        BO.TransferBO CheckItemForTransfer(BO.TransferBO transferBO);
        [OperationContract]
        BO.PutAway GetItemTOPutAway(BO.PutAway putAway);
        [OperationContract]
        BO.PutAway UpsertPutAwayItem(BO.PutAway putAway);
        [OperationContract]
        BO.TransferBO CloseTransferOrder(BO.TransferBO transferBO);
        [OperationContract]
        BO.Inbound GetInboundNo(BO.Inbound inbound);
        [OperationContract]
        BO.PutAway CheckPutAwayItemQty(BO.PutAway PutAway);
        [OperationContract]
        BO.PutAway SkipItem(BO.PutAway putAway);
        [OperationContract]
        UserDataTable GetSkipReasonList(string type);
        [OperationContract]
        BO.VLPD VLPDSkipItem(BO.VLPD VLPD);
        [OperationContract]
        BO.Inbound GetReceivedQty(BO.Inbound inbound);
        [OperationContract]
        void Logout(BO.ValidateUserLogin validateUserLogin);
        [OperationContract]
        BO.Outbound GetOBDNo(BO.Outbound outbound);
        [OperationContract]
        BO.Outbound GetOBDItemToPick(BO.Outbound outbound);
        [OperationContract]
        int CheckValidateUser(BO.ValidateUserLogin objValidateUserLogin);
        [OperationContract]
        BO.CycleCount CheckMaterialAvailablilty(BO.CycleCount cycleCount);
        [OperationContract]
        BO.Outbound OBDSkipItem(BO.Outbound outbound);
        [OperationContract]
        string CheckTenatMaterial(string Mcode, int AccountID, string TenantName);
        [OperationContract]
        UserDataTable GetBinToBinStorageLocations();
        [OperationContract]
        BO.TransferBO UpsertBinToBinTransferItem(BO.TransferBO transferBO);
        [OperationContract]
        UserDataTable GetVLPDPickedList(BO.VLPD vLPD, out string Result);
        [OperationContract]
        BO.VLPD DeleteVLPDPickedItems(BO.VLPD vLPD);
        [OperationContract]
        UserDataTable GetOBDPickedList(BO.Outbound outbound, out string Result);
        [OperationContract]
        string CheckStrickyCompliance(BO.ValidateUserLogin validateUserLogin);
        [OperationContract]
        BO.PutAway GetTransferContainer(BO.PutAway putAway);
        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, UriTemplate = "SaveBook/{id}")]
        string SaveBook(string book, string id);
        [OperationContract]
        string UploadFile(byte[] f, string fileName);
        [OperationContract]
        List<PODBO> GetJobOrderList(int Limit);
        [OperationContract]
        PODBO GetUserDetail(String UserName, String Password);
        [OperationContract]
        List<PODBO> GetMenuLinks();
        [OperationContract]
        List<PODBO> GetInboundStatistics();
        [OperationContract]
        List<PODBO> GetOutboundStatistics();
        [OperationContract]
        List<PODBO> GetInOutActivities();
        [OperationContract]
        List<PODBO> GetInboundList(string WarehouseIDs, int ShipmentTypeID, int InboundStatusID, int SupplierID, int Limit, string InboundStatus);
        [OperationContract]
        List<PODBO> GetOutboundList(string WarehouseIDs, int DocumentTypeID, int OBDStatusID, int CustomerID, int Limit, string DeliveryStatus);
        [OperationContract]
        List<PODBO> GetJobOrderOutboundList(int Limit);
        [OperationContract]
        List<PODBO> GetInOutOperations();
        [OperationContract]
        List<PODBO> GetLiveStock(String MCode, String Location, String BatchNo, String PageSize);
        [OperationContract]
        List<PODBO> GetWarehouses();
        [OperationContract]
        List<PODBO> GetSuppliers();
        [OperationContract]
        List<PODBO> GetShipmentStatus(int Limit);
        [OperationContract]
        List<PODBO> GetDeliveryStatus();
        [OperationContract]
        List<PODBO> GetCustomers();
        [OperationContract]
        List<PODBO> GetDocumentType();
        [OperationContract]
        List<PODBO> GetPartNumberList(int TenantID);
        [OperationContract]
        List<PODBO> GetVersionDetails();
        [OperationContract]
        List<PODBO> MaterialStorageParameters(String MaterialCode, int TenantID);
        [OperationContract]
        List<PODBO> MaterialReceived(string LineNumber, String MaterialCode, String StoreRefNo, String PONumber, string SupplierInvoieID);
        [OperationContract]
        string UpdateReceiveItem(string GoodsMovementDetailsID, string GoodsMovementTypeID, string Location, string MaterialMaster_IUoMID, string DocQty, string Quantity, string Storerefnum, string LineNumber, string mcode, string IsDamaged, string hasDiscrepancy, string POSOHeaderID, string CreatedBy, string MaterialStorageParameterIDs, string MaterialStorageParameterValues, string Conversionfactor, string remarks, string LastModifiedBy, string IsActive, string KitplannerID, string InvoiceQuantity, string SupplierInvoiceID, string IsPositiveRecall, int IsPrintRequired, string printerIP, string serialNumber);

        [OperationContract]
        List<PODBO> GetPOList(string StatusName);
        [OperationContract]

        List<PODBO> GetSOList(string StatusName);
        [OperationContract]

        List<PODBO> GetMaterialType(int TenantID);
        [OperationContract]

        List<PODBO> GetWHForWHList(int TenantID);

        [OperationContract]

        List<PODBO> GetShipmentType();

        [OperationContract]

        PODBO PODDelivery(PODBO OBDTrack);

        [OperationContract]

        List<PODBO> GetPendingOutboundList(int UserID);
        [OperationContract]

        List<PODBO> GetCurrentStock(PODBO _requestPOBO);

        [OperationContract]
         UserDataTable GetMSPsReceiving(BO.PutAway putAway);

    }

    [ServiceContract, XmlSerializerFormat]
    [DataContract]
    public class GoodsInData1234
    {

        [DataMember]
        public List<THHTWSData.KeyValueStruct> MSPs
        {
            get;
            set;
        }

        [DataMember]
        public List<THHTWSData.KeyValueStruct> POItemProperties
        {
            get;
            set;
        }

        [DataMember]
        public DataTable GoodsInListData
        {
            get;
            set;
        }


    }
}
