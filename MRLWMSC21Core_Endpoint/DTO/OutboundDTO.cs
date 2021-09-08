using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MRLWMSC21_Endpoint.DTO
{
    public class OutboundDTO
    {
        private string _OutboundID;
         private string _SKU;
        private string _SerialNo;
        private string _MfgDate;
        private string _Location;
        private string _PalletNo;
        private string _CustomerName;
        private string _BusinessType;
        private string _PSNID;
        private string _CartonSerialNo;
        private string _PSNDetailsID;
        private string _PackType;
        private string _SOHeaderID;
        private string _PackComplete;
        private List<string> _lstSalesOrders;
        private string _LoadRefNo;
        private string _TotalSOCount;
        private string _ScannedSOCount;
        private string _CustomerCode;
        private string _CustomerAddress;
        private string _WareHouseID;


        private string _UserId;
       
        private string _RequiredQty;
        private string _PickedQty;
        private List<string> _PackingMcodes;


        private string _SuggestionID;
        private string _RevertQty;
        private int _TransferRequestDetailsId;
        private int _TransferRequestId;
        private string _SOQty;
        private string _PackedQty;
        private string _PackingList;


        private string _MaterialMasterId;
        private string _MaterialDescription;
        private string _AccountID;
        private string _OBDNo;
        private string _Flag;
        private string _SkipReason;
        private string _SkipQty;
        private string _AssignedID;
        private string _CartonID;
        private string _LocationId;
        private string _ExpDate;
        private string _BatchNo;
        private string _ProjectNo;
        private string _AssignedQuantity;
        private string _SODetailsID;
        private string _SLocId;
        private string _SLoc;
        private string _GoodsmomentDeatilsId;
        private string _Lineno;
        private string _MaterialMaster_IUoMID;
        private string _CF;
        private string _POSOHeaderId;
        private string _PendingQty;
        private string _Result;
        private string _KitId;
        private string _ToCartonNO;
        private string _CartonNO;
        private string _VLPDId;
        private string _VLPDNumber;
        private string _IsDam;
        private string _HasDis;
        private string _PickedId;
        private string _MCode;

        private string _MRP;
        private string _TenatID;
        private string _Vehicle;
        private string _OBDNumber;
        private string _DriverName;
        private string _DriverNo;
        private string _LRnumber;
        private string _SONumber;
        private string _Status;
        private string _HUSize;
        private string _HUNo;

        public string Status { get => _Status; set => _Status = value; }
        public string Vehicle { get => _Vehicle; set => _Vehicle = value; }
        public string OBDNumber { get => _OBDNumber; set => _OBDNumber = value; }
        public string DriverName { get => _DriverName; set => _DriverName = value; }
        public string DriverNo { get => _DriverNo; set => _DriverNo = value; }
        public string LRnumber { get => _LRnumber; set => _LRnumber = value; }
        public string TenatID { get => _TenatID; set => _TenatID = value; }
        public string SKU { get => _SKU; set => _SKU = value; }
        public string SerialNo { get => _SerialNo; set => _SerialNo = value; }
        public string MfgDate { get => _MfgDate; set => _MfgDate = value; }
        public string Location { get => _Location; set => _Location = value; }
        public string PalletNo { get => _PalletNo; set => _PalletNo = value; }
   
        public string UserId { get => _UserId; set => _UserId = value; }
   
        public string RequiredQty { get => _RequiredQty; set => _RequiredQty = value; }
        public string PickedQty { get => _PickedQty; set => _PickedQty = value; }
      
        public string SuggestionID { get => _SuggestionID; set => _SuggestionID = value; }
        public string RevertQty { get => _RequiredQty; set => _RequiredQty = value; }
        
        public string MaterialMasterId { get => _MaterialMasterId; set => _MaterialMasterId = value; }
        public string MaterialDescription { get => _MaterialDescription; set => _MaterialDescription = value; }
        public string AccountID { get => _AccountID; set => _AccountID = value; }
        public string OBDNo { get => _OBDNo; set => _OBDNo = value; }
        public string OutboundID { get => _OutboundID; set => _OutboundID = value; }
        public string Flag { get => _Flag; set => _Flag = value; }
        public string SkipReason { get => _SkipReason; set => _SkipReason = value; }
        public string SkipQty { get => _SkipQty; set => _SkipQty = value; }
        public string AssignedID { get => _AssignedID; set => _AssignedID = value; }
        public string CartonID { get => _CartonID; set => _CartonID = value; }
        public string LocationId { get => _LocationId; set => _LocationId = value; }
        public string ExpDate { get => _ExpDate; set => _ExpDate = value; }
        public string BatchNo { get => _BatchNo; set => _BatchNo = value; }
        public string ProjectNo { get => _ProjectNo; set => _ProjectNo = value; }
        public string AssignedQuantity { get => _AssignedQuantity; set => _AssignedQuantity = value; }
        public string SODetailsID { get => _SODetailsID; set => _SODetailsID = value; }
        public string SLocId { get => _SLocId; set => _SLocId = value; }
        public string SLoc { get => _SLoc; set => _SLoc = value; }
        public string GoodsmomentDeatilsId { get => _GoodsmomentDeatilsId; set => _GoodsmomentDeatilsId = value; }
        public string Lineno { get => _Lineno; set => _Lineno = value; }
        public string MaterialMaster_IUoMID { get => _MaterialMaster_IUoMID; set => _MaterialMaster_IUoMID = value; }
        public string CF { get => _CF; set => _CF = value; }
        public string POSOHeaderId { get => _POSOHeaderId; set => _POSOHeaderId = value; }
        public string PendingQty { get => _PendingQty; set => _PendingQty = value; }
        public string Result { get => _Result; set => _Result = value; }
        public string KitId { get => _KitId; set => _KitId = value; }
        public string ToCartonNO { get => _ToCartonNO; set => _ToCartonNO = value; }
        public string VLPDId { get => _VLPDId; set => _VLPDId = value; }
        public string VLPDNumber { get => _VLPDNumber; set => _VLPDNumber = value; }
        public string IsDam { get => _IsDam; set => _IsDam = value; }
        public string HasDis { get => _HasDis; set => _HasDis = value; }
        public string PickedId { get => _PickedId; set => _PickedId = value; }
        public int TransferRequestDetailsId { get => _TransferRequestDetailsId; set => _TransferRequestDetailsId = value; }
        public int TransferRequestId { get => _TransferRequestId; set => _TransferRequestId = value; }

        public string MCode { get => _MCode; set => _MCode = value; }

        public string MRP { get => _MRP; set => _MRP = value; }
        public string SONumber { get => _SONumber; set => _SONumber = value; }
        public List<string> PackingMcodes { get => _PackingMcodes; set => _PackingMcodes = value; }
        public string SOQty { get => _SOQty; set => _SOQty = value; }
        public string PackedQty { get => _PackedQty; set => _PackedQty = value; }
        public string PackingList { get => _PackingList; set => _PackingList = value; }
        public string CustomerName { get => _CustomerName; set => _CustomerName = value; }
        public string BusinessType { get => _BusinessType; set => _BusinessType = value; }
        public string PSNID { get => _PSNID; set => _PSNID = value; }
        public string CartonSerialNo { get => _CartonSerialNo; set => _CartonSerialNo = value; }
        public string PSNDetailsID { get => _PSNDetailsID; set => _PSNDetailsID = value; }
        public string PackType { get => _PackType; set => _PackType = value; }
        public string SOHeaderID { get => _SOHeaderID; set => _SOHeaderID = value; }
        public string PackComplete { get => _PackComplete; set => _PackComplete = value; }
        public List<string> LstSalesOrders { get => _lstSalesOrders; set => _lstSalesOrders = value; }
        public string LoadRefNo { get => _LoadRefNo; set => _LoadRefNo = value; }
        public string TotalSOCount { get => _TotalSOCount; set => _TotalSOCount = value; }
        public string ScannedSOCount { get => _ScannedSOCount; set => _ScannedSOCount = value; }
        public string CartonNO { get => _CartonNO; set => _CartonNO = value; }
        public string CustomerCode { get => _CustomerCode; set => _CustomerCode = value; }
        public string CustomerAddress { get => _CustomerAddress; set => _CustomerAddress = value; }
        public string WareHouseID { get => _WareHouseID; set => _WareHouseID = value; }
        public string HUSize { get => _HUSize; set => _HUSize = value; }
        public string HUNo { get => _HUNo; set => _HUNo = value; }
    }
}