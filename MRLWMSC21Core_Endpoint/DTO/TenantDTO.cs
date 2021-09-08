using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MRLWMSC21_Endpoint.DTO
{
    public class TenantDTO
    {
        private string _TenantID;

        private string _TenantName;

        private string _AccountId;

        private string _MCode;

        private string _MaterialMasterID;

        private string _MType;

        private string _MTypeID;

        private string _WarehouseID;

        private string _WHCode;

        private string _PartNumber;

        private string _Bin;

        private string _SLoc;

        private string _QuantOfHandHeld;

        public string TenantID { get => _TenantID; set => _TenantID = value; }
        public string TenantName { get => _TenantName; set => _TenantName = value; }
        public string AccountId { get => _AccountId; set => _AccountId = value; }
        public string MCode { get => _MCode; set => _MCode = value; }
        public string MaterialMasterID { get => _MaterialMasterID; set => _MaterialMasterID = value; }
        public string MType { get => _MType; set => _MType = value; }
        public string MTypeID { get => _MTypeID; set => _MTypeID = value; }
        public string WarehouseID { get => _WarehouseID; set => _WarehouseID = value; }
        public string WHCode { get => _WHCode; set => _WHCode = value; }
        public string PartNumber { get => _PartNumber; set => _PartNumber = value; }
        public string Bin { get => _Bin; set => _Bin = value; }
        public string SLoc { get => _SLoc; set => _SLoc = value; }
        public string QuantOfHandHeld { get => _QuantOfHandHeld; set => _QuantOfHandHeld = value; }
    }
    public class WarehouseDTO
    {
        private string _WarehouseID;

        private string _WHCode;

        public string WarehouseID { get => _WarehouseID; set => _WarehouseID = value; }
        public string WHCode { get => _WHCode; set => _WHCode = value; }
    }

    public class ShipmentDTO
    {
        private string _InboundStatus;

        private string _InboundStatusID;

        private string _DeliveryStatusID;

        private string _DeliveryStatus;

        private string _POStatusID;

        private string _StatusName;

        private string _SOStatusID;

        private string _Type;

        private byte[] _FileConent;

        private string _FileName;

        private string _UserID;

        //OutBoundID
        private string _OBDTrackingID;

        private string _WcfURL;

        private string _OBDNumber;

        private string _Result;

        public string InboundStatus { get => _InboundStatus; set => _InboundStatus = value; }
        public string InboundStatusID { get => _InboundStatusID; set => _InboundStatusID = value; }
        public string DeliveryStatusID { get => _DeliveryStatusID; set => _DeliveryStatusID = value; }
        public string DeliveryStatus { get => _DeliveryStatus; set => _DeliveryStatus = value; }
        public string POStatusID { get => _POStatusID; set => _POStatusID = value; }
        public string StatusName { get => _StatusName; set => _StatusName = value; }
        public string SOStatusID { get => _SOStatusID; set => _SOStatusID = value; }
        public string Type { get => _Type; set => _Type = value; }
        public byte[] FileConent { get => _FileConent; set => _FileConent = value; }
        public string FileName { get => _FileName; set => _FileName = value; }
        public string OBDTrackingID { get => _OBDTrackingID; set => _OBDTrackingID = value; }
        public string UserID { get => _UserID; set => _UserID = value; }
        public string WcfURL { get => _WcfURL; set => _WcfURL = value; }
        public string OBDNumber { get => _OBDNumber; set => _OBDNumber = value; }
        public string Result { get => _Result; set => _Result = value; }
    }
}