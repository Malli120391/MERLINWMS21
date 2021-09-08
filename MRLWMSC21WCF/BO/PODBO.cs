using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MRLWMSC21WCF.BO
{
    public class PODBO
    {
        private Guid _UserGUID;

        private string _UserID;

        private string _FirstName;

        private string _LastName;

        private string _Email;

        private Int64 _TenantID;

        private string _Password;

        private string _Roles;

        private string _Warehouses;

        private string _Zones;

        private string _Departments;

        private string _MenuText;

        private string _Date;

        private DateTime _ShipmentExpDate;

        private Int64 _Expected;

        private Int64 _DocsReceived;

        private Int64 _GRNDoneCount;

        private Int64 _ReceivedCount;
        private Int64 _VerifiedCount;

        private DateTime _OrderDate;

        private Int64 _OrderCount;

        private Int64 _PGIDoneCount;

        private string _WHCode;

        private Int64 _Inbound;

        private Int64 _Outbound;

        private Int64 _RowNum;

        private Int64 _InboundID;

        private string _StoreRefNo;

        private string _ShipmentType;

        private string _SupplierName;

        private DateTime _ShipmentReceivedOn;

        private string _DocReceivedDate;

        private string _ReferedStores;

        private string _InboundStatus;

        private Int64 _LineCount;

        private Int64 _OutboundID;

        private string _OBDNumber;

        private string _OBDDate;

        private string _CustomerName;

        private string _DocumentType;

        private Int64 _TodayCount;

        private string _DeliveryStatus;

        private string _JobOrderRefNo;

        private string _RoutingDocumentType;

        private string _WONumber;

        private string _IORefNo;

        private string _KitCode;

        private Int64 _TodayInbounds;

        private Int64 _TodayOBDs;

        private string _PartNumber;

        private string _Location;

        private string _OEMPartNo;

        private bool _AvailableQty;

        private bool _IsDamaged;

        private bool _HasDiscrepancy;

        private bool _IsNonConfirmity;

        private bool _AsIs;

        private bool _IsPositiveRecall;

        private string _WarehouseID;

        private Int64 _WarehouseGroupID;

        private string _WarehouseGroupCode;

        private Int64 _WarehouseTypeID;

        private string _WarehouseType;

        private Int64 _SupplierID;

        private Int64 _ShipmentTypeID;

        private string _InboundStatusID;

        private string _DeliveryStatusID;

        private Int64 _CustomerID;

        private Int64 _DocumentTypeID;

        private string _MCode;

        private decimal _Version;

        private string _ReleaseDate;

        private string _ParameterName;

        private string _ControlType;

        private string _ParameterDataType;

        private string _DataSource;

        private Int64 _IsRequired;

        private Int64 _MaterialStorageParameterID;

        private string _DisplayName;

        private Int64 _GoodsMovementDetailsID;

        private string _CartonCode;

        private Int64 _DocQty;

        private string _POSODetailsID;

        private Int64 _CapturingQc;

        private string _Result;

        private string _SONumber;

        private string _StatusName;

        private string _SOType;

        private string _POType;

        private string _PONumber;

        private string _MaterialMasterID;

        private string _MType;

        private string _MTypeID;


        private string _POStatusID;

        private string _SOStatusID;

        private string _Type;

        //_DeliveryStatusID;

        private string _OBDTrackingID;

        private string _InstructionModeID;

        private string _Requester;

        private string _DocumentNumber;

        private string _DocumentReceivedDate;

        private int _DeliveredBy;

        private string _DriverName;

        private string _ReceivedBy;

        private string _RemBy_DeliveryIncharge;

        private int _RefWHID;

        private int _TransferedtoStoreID;

        private int _IsDCRReceived;

        private string _DeliveryDate;

        private byte[] _FileConent;
        private string _FileName;
        private string _WebURL;

        private string _TenantName;

        public string _Bin;

        public string Bin { get => _Bin; set => _Bin = value; }

        public string _SLoc;

        public string SLoc { get => _SLoc; set => _SLoc = value; }

        public string _QuantOfHandHeld;

        public string QuantOfHandHeld { get => _QuantOfHandHeld; set => _QuantOfHandHeld = value; }
        //DeliveryStatusID;





        public Guid UserGUID { get => _UserGUID; set => _UserGUID = value; }
        public string UserID { get => _UserID; set => _UserID = value; }
        public string FirstName { get => _FirstName; set => _FirstName = value; }
        public string LastName { get => _LastName; set => _LastName = value; }
        public string Email { get => _Email; set => _Email = value; }
        public Int64 TenantID { get => _TenantID; set => _TenantID = value; }
        public string Password { get => _Password; set => _Password = value; }
        public string Roles { get => _Roles; set => _Roles = value; }
        public string Warehouses { get => _Warehouses; set => _Warehouses = value; }
        public string Zones { get => _Zones; set => _Zones = value; }
        public string Departments { get => _Departments; set => _Departments = value; }
        public string MenuText { get => _MenuText; set => _MenuText = value; }
        public string Date { get => _Date; set => _Date = value; }
        public DateTime ShipmentExpDate { get => _ShipmentExpDate; set => _ShipmentExpDate = value; }
        public Int64 Expected { get => _Expected; set => _Expected = value; }
        public Int64 DocsReceived { get => _DocsReceived; set => _DocsReceived = value; }
        public Int64 GRNDoneCount { get => _GRNDoneCount; set => _GRNDoneCount = value; }
        public Int64 ReceivedCount { get => _ReceivedCount; set => _ReceivedCount = value; }
        public Int64 VerifiedCount { get => _VerifiedCount; set => _VerifiedCount = value; }
        public DateTime OrderDate { get => _OrderDate; set => _OrderDate = value; }
        public Int64 OrderCount { get => _OrderCount; set => _OrderCount = value; }
        public Int64 PGIDoneCount { get => _PGIDoneCount; set => _PGIDoneCount = value; }
        public string WHCode { get => _WHCode; set => _WHCode = value; }
        public Int64 Inbound { get => _Inbound; set => _Inbound = value; }
        public Int64 Outbound { get => _Outbound; set => _Outbound = value; }
        public Int64 RowNum { get => _RowNum; set => _RowNum = value; }
        public Int64 InboundID { get => _InboundID; set => _InboundID = value; }
        public string StoreRefNo { get => _StoreRefNo; set => _StoreRefNo = value; }
        public string ShipmentType { get => _ShipmentType; set => _ShipmentType = value; }
        public string SupplierName { get => _SupplierName; set => _SupplierName = value; }
        public DateTime ShipmentReceivedOn { get => _ShipmentReceivedOn; set => _ShipmentReceivedOn = value; }
        public string DocReceivedDate { get => _DocReceivedDate; set => _DocReceivedDate = value; }
        public string ReferedStores { get => _ReferedStores; set => _ReferedStores = value; }
        public string InboundStatus { get => _InboundStatus; set => _InboundStatus = value; }
        public Int64 LineCount { get => _LineCount; set => _LineCount = value; }
        public Int64 OutboundID { get => _OutboundID; set => _OutboundID = value; }
        public string OBDNumber { get => _OBDNumber; set => _OBDNumber = value; }
        public string OBDDate { get => _OBDDate; set => _OBDDate = value; }
        public string CustomerName { get => _CustomerName; set => _CustomerName = value; }
        public string DocumentType { get => _DocumentType; set => _DocumentType = value; }
        public Int64 TodayCount { get => _TodayCount; set => _TodayCount = value; }
        public string DeliveryStatus { get => _DeliveryStatus; set => _DeliveryStatus = value; }
        public string JobOrderRefNo { get => _JobOrderRefNo; set => _JobOrderRefNo = value; }
        public string RoutingDocumentType { get => _RoutingDocumentType; set => _RoutingDocumentType = value; }
        public string WONumber { get => _WONumber; set => _WONumber = value; }
        public string IORefNo { get => _IORefNo; set => _IORefNo = value; }
        public string KitCode { get => _KitCode; set => _KitCode = value; }
        public Int64 TodayInbounds { get => _TodayInbounds; set => _TodayInbounds = value; }
        public Int64 TodayOBDs { get => _TodayOBDs; set => _TodayOBDs = value; }
        public string PartNumber { get => _PartNumber; set => _PartNumber = value; }
        public string Location { get => _Location; set => _Location = value; }
        public string OEMPartNo { get => _OEMPartNo; set => _OEMPartNo = value; }
        public bool AvailableQty { get => _AvailableQty; set => _AvailableQty = value; }
        public bool IsDamaged { get => _IsDamaged; set => _IsDamaged = value; }
        public bool HasDiscrepancy { get => _HasDiscrepancy; set => _HasDiscrepancy = value; }
        public bool IsNonConfirmity { get => _IsNonConfirmity; set => _IsNonConfirmity = value; }
        public bool AsIs { get => _AsIs; set => _AsIs = value; }
        public bool IsPositiveRecall { get => _IsPositiveRecall; set => _IsPositiveRecall = value; }
        public string WarehouseID { get => _WarehouseID; set => _WarehouseID = value; }
        public Int64 WarehouseGroupID { get => _WarehouseGroupID; set => _WarehouseGroupID = value; }
        public string WarehouseGroupCode { get => _WarehouseGroupCode; set => _WarehouseGroupCode = value; }
        public Int64 WarehouseTypeID { get => _WarehouseTypeID; set => _WarehouseTypeID = value; }
        public string WarehouseType { get => _WarehouseType; set => _WarehouseType = value; }
        public Int64 SupplierID { get => _SupplierID; set => _SupplierID = value; }
        public Int64 ShipmentTypeID { get => _ShipmentTypeID; set => _ShipmentTypeID = value; }
        public string InboundStatusID { get => _InboundStatusID; set => _InboundStatusID = value; }
        public string DeliveryStatusID { get => _DeliveryStatusID; set => _DeliveryStatusID = value; }
        public Int64 CustomerID { get => _CustomerID; set => _CustomerID = value; }
        public Int64 DocumentTypeID { get => _DocumentTypeID; set => _DocumentTypeID = value; }
        public string MCode { get => _MCode; set => _MCode = value; }
        public decimal Version { get => _Version; set => _Version = value; }
        public string ReleaseDate { get => _ReleaseDate; set => _ReleaseDate = value; }
        public string ParameterName { get => _ParameterName; set => _ParameterName = value; }
        public string ControlType { get => _ControlType; set => _ControlType = value; }
        public string ParameterDataType { get => _ParameterDataType; set => _ParameterDataType = value; }
        public string DataSource { get => _DataSource; set => _DataSource = value; }
        public Int64 IsRequired { get => _IsRequired; set => _IsRequired = value; }
        public Int64 MaterialStorageParameterID { get => _MaterialStorageParameterID; set => _MaterialStorageParameterID = value; }
        public string DisplayName { get => _DisplayName; set => _DisplayName = value; }
        public Int64 GoodsMovementDetailsID { get => _GoodsMovementDetailsID; set => _GoodsMovementDetailsID = value; }
        public string CartonCode { get => _CartonCode; set => _CartonCode = value; }
        public Int64 DocQty { get => _DocQty; set => _DocQty = value; }
        public string POSODetailsID { get => _POSODetailsID; set => _POSODetailsID = value; }
        public Int64 CapturingQc { get => _CapturingQc; set => _CapturingQc = value; }
        public string Result { get => _Result; set => _Result = value; }
        public string SONumber { get => _SONumber; set => _SONumber = value; }
        public string StatusName { get => _StatusName; set => _StatusName = value; }
        public string SOType { get => _SOType; set => _SOType = value; }
        public string POType { get => _POType; set => _POType = value; }
        public string PONumber { get => _PONumber; set => _PONumber = value; }
        public string MaterialMasterID { get => _MaterialMasterID; set => _MaterialMasterID = value; }
        public string MType { get => _MType; set => _MType = value; }
        public string MTypeID { get => _MTypeID; set => _MTypeID = value; }
        public string POStatusID { get => _POStatusID; set => _POStatusID = value; }
        public string SOStatusID { get => _SOStatusID; set => _SOStatusID = value; }
        public string Type { get => _Type; set => _Type = value; }
        public string OBDTrackingID { get => _OBDTrackingID; set => _OBDTrackingID = value; }
        public string InstructionModeID { get => _InstructionModeID; set => _InstructionModeID = value; }
        public string Requester { get => _Requester; set => _Requester = value; }
        public string DocumentNumber { get => _DocumentNumber; set => _DocumentNumber = value; }
        public string DocumentReceivedDate { get => _DocumentReceivedDate; set => _DocumentReceivedDate = value; }
        public int DeliveredBy { get => _DeliveredBy; set => _DeliveredBy = value; }
        public string DriverName { get => _DriverName; set => _DriverName = value; }
        public string ReceivedBy { get => _ReceivedBy; set => _ReceivedBy = value; }
        public string RemBy_DeliveryIncharge { get => _RemBy_DeliveryIncharge; set => _RemBy_DeliveryIncharge = value; }
        public int RefWHID { get => _RefWHID; set => _RefWHID = value; }
        public int TransferedtoStoreID { get => _TransferedtoStoreID; set => _TransferedtoStoreID = value; }
        public int IsDCRReceived { get => _IsDCRReceived; set => _IsDCRReceived = value; }
        public string DeliveryDate { get => _DeliveryDate; set => _DeliveryDate = value; }
        public byte[] FileConent { get => _FileConent; set => _FileConent = value; }
        public string FileName { get => _FileName; set => _FileName = value; }
        public string WebURL { get => _WebURL; set => _WebURL = value; }
        public string TenantName { get => _TenantName; set => _TenantName = value; }
    }
}