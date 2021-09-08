using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MRLWMSC21_Endpoint.DTO
{
    public class PutAwayDTO
    {

        private string _SuggestedPutawayID;
        private string _MaterialMasterId;
        private string _MCode;
        private string _MDescription;
        private string _CartonCode;
        private string _CartonID;
        private string _Location;
        private string _LocationID;
        private string _MfgDate;
        private string _ExpDate;
        private string _SerialNo;
        private string _BatchNo;
        private string _ProjectRefNo;
        private string _AssignedQuantity;
        private string _SuggestedQty;
        private string _SuggestedReceivedQty;
        private string _SuggestedRemainingQty;
        private string _TransferRequestDetailsId;
        private string _PickedLocationID;
        private string _GMDRemainingQty;
        private string _PutAwayQty;
        private string _InboundId;
        private string _Result;
        private string _SkipQty;
        private string _UserID;
        private string _TotalQty;
        private string _SkipReason;
        private string _transferRequestId;
        private string _mRP;
        private string _Dock;
        private string _StorageCode;
        private string _scannedLocation;


        public string SuggestedPutawayID { get => _SuggestedPutawayID; set => _SuggestedPutawayID = value; }
        public string MaterialMasterId { get => _MaterialMasterId; set => _MaterialMasterId = value; }
        public string MCode { get => _MCode; set => _MCode = value; }
        public string MDescription { get => _MDescription; set => _MDescription = value; }
        public string CartonCode { get => _CartonCode; set => _CartonCode = value; }
        public string CartonID { get => _CartonID; set => _CartonID = value; }
        public string Location { get => _Location; set => _Location = value; }
        public string LocationID { get => _LocationID; set => _LocationID = value; }
        public string MfgDate { get => _MfgDate; set => _MfgDate = value; }
        public string ExpDate { get => _ExpDate; set => _ExpDate = value; }
        public string SerialNo { get => _SerialNo; set => _SerialNo = value; }
        public string BatchNo { get => _BatchNo; set => _BatchNo = value; }
        public string ProjectRefNo { get => _ProjectRefNo; set => _ProjectRefNo = value; }
        public string AssignedQuantity { get => _AssignedQuantity; set => _AssignedQuantity = value; }
        public string SuggestedQty { get => _SuggestedQty; set => _SuggestedQty = value; }
        public string SuggestedReceivedQty { get => _SuggestedReceivedQty; set => _SuggestedReceivedQty = value; }
        public string SuggestedRemainingQty { get => _SuggestedRemainingQty; set => _SuggestedRemainingQty = value; }
        public string TransferRequestDetailsId { get => _TransferRequestDetailsId; set => _TransferRequestDetailsId = value; }
        public string PickedLocationID { get => _PickedLocationID; set => _PickedLocationID = value; }
        public string GMDRemainingQty { get => _GMDRemainingQty; set => _GMDRemainingQty = value; }
        public string PutAwayQty { get => _PutAwayQty; set => _PutAwayQty = value; }
        public string InboundId { get => _InboundId; set => _InboundId = value; }
        public string Result { get => _Result; set => _Result = value; }
        public string SkipQty { get => _SkipQty; set => _SkipQty = value; }
        public string UserID { get => _UserID; set => _UserID = value; }
        public string TotalQty { get => _TotalQty; set => _TotalQty = value; }
        public string SkipReason { get => _SkipReason; set => _SkipReason = value; }
        public string TransferRequestId { get => _transferRequestId; set => _transferRequestId = value; }
        public string MRP { get => _mRP; set => _mRP = value; }
        public string Dock { get => _Dock; set => _Dock = value; }
        public string StorageCode { get => _StorageCode; set => _StorageCode = value; }
        public string ScannedLocation { get => _scannedLocation; set => _scannedLocation = value; }
    }
}