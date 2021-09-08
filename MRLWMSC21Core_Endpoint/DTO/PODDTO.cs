using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MRLWMSC21_Endpoint.DTO
{
    public class PODDTO
    {
        private Guid _UserGUID;

        private string _UserID;

        private string _FirstName;

        private string _LastName;

        private string _Email;

        private int _TenantID;

        private string _Password;

        private string _Roles;

        private string _Warehouses;

        private string _Zones;

        private string _Departments;

        private string _MenuText;

        private string _Date;

        private DateTime _ShipmentExpDate;

        private int _Expected;

        private int _DocsReceived;

        private int _GRNDoneCount;

        private int _ReceivedCount;
        private int _VerifiedCount;

        private DateTime _OrderDate;

        private int _OrderCount;

        private int _PGIDoneCount;

        private string _WHCode;

        private int _Inbound;

        private int _Outbound;

        private int _RowNum;

        private string _InboundID;

        private string _StoreRefNo;

        private string _ShipmentType;

        private string _SupplierName;

        private DateTime _ShipmentReceivedOn;

        private string _DocReceivedDate;

        private string _ReferedStores;

        private string _InboundStatus;

        private int _LineCount;

        private int _OutboundID;

        private string _OBDNumber;

        private string _OBDDate;

        private string _CustomerName;

        private string _DocumentType;

        private int _TodayCount;

        private string _DeliveryStatus;

        private string _JobOrderRefNo;

        private string _RoutingDocumentType;

        private string _WONumber;

        private string _IORefNo;

        private string _KitCode;

        private int _TodayInbounds;

        private int _TodayOBDs;

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

        private int _WarehouseGroupID;

        private string _WarehouseGroupCode;

        private int _WarehouseTypeID;

        private string _WarehouseType;

        private int _SupplierID;

        private int _ShipmentTypeID;

        private int _InboundStatusID;

        private int _DeliveryStatusID;

        private int _CustomerID;

        private int _DocumentTypeID;

        private string _MCode;

        private decimal _Version;

        private string _ReleaseDate;

        private string _ParameterName;

        private string _ControlType;

        private string _ParameterDataType;

        private string _DataSource;

        private int _IsRequired;

        private int _MaterialStorageParameterID;

        private string _DisplayName;

        private int _GoodsMovementDetailsID;

        private string _CartonCode;

        private int _DocQty;

        private string _POSODetailsID;

        private int _CapturingQc;

        private int _Result;

        private string _UserName;

        private string _Limit;

        private string _StatusName;

        private string _WarehouseIDs;

        private int __AccountId;

        private string _TenantName;

        public Guid UserGUID { get => _UserGUID; set => _UserGUID = value; }
        public string UserID { get => _UserID; set => _UserID = value; }
        public string FirstName { get => _FirstName; set => _FirstName = value; }
        public string LastName { get => _LastName; set => _LastName = value; }
        public string Email { get => _Email; set => _Email = value; }
        public int TenantID { get => _TenantID; set => _TenantID = value; }
        public string Password { get => _Password; set => _Password = value; }
        public string Roles { get => _Roles; set => _Roles = value; }
        public string Warehouses { get => _Warehouses; set => _Warehouses = value; }
        public string Zones { get => _Zones; set => _Zones = value; }
        public string Departments { get => _Departments; set => _Departments = value; }
        public string MenuText { get => _MenuText; set => _MenuText = value; }
        public string Date { get => _Date; set => _Date = value; }
        public DateTime ShipmentExpDate { get => _ShipmentExpDate; set => _ShipmentExpDate = value; }
        public int Expected { get => _Expected; set => _Expected = value; }
        public int DocsReceived { get => _DocsReceived; set => _DocsReceived = value; }
        public int GRNDoneCount { get => _GRNDoneCount; set => _GRNDoneCount = value; }
        public int ReceivedCount { get => _ReceivedCount; set => _ReceivedCount = value; }
        public int VerifiedCount { get => _VerifiedCount; set => _VerifiedCount = value; }
        public DateTime OrderDate { get => _OrderDate; set => _OrderDate = value; }
        public int OrderCount { get => _OrderCount; set => _OrderCount = value; }
        public int PGIDoneCount { get => _PGIDoneCount; set => _PGIDoneCount = value; }
        public string WHCode { get => _WHCode; set => _WHCode = value; }
        public int Inbound { get => _Inbound; set => _Inbound = value; }
        public int Outbound { get => _Outbound; set => _Outbound = value; }
        public int RowNum { get => _RowNum; set => _RowNum = value; }
        public string InboundID { get => _InboundID; set => _InboundID = value; }
        public string StoreRefNo { get => _StoreRefNo; set => _StoreRefNo = value; }
        public string ShipmentType { get => _ShipmentType; set => _ShipmentType = value; }
        public string SupplierName { get => _SupplierName; set => _SupplierName = value; }
        public DateTime ShipmentReceivedOn { get => _ShipmentReceivedOn; set => _ShipmentReceivedOn = value; }
        public string DocReceivedDate { get => _DocReceivedDate; set => _DocReceivedDate = value; }
        public string ReferedStores { get => _ReferedStores; set => _ReferedStores = value; }
        public string InboundStatus { get => _InboundStatus; set => _InboundStatus = value; }
        public int LineCount { get => _LineCount; set => _LineCount = value; }
        public int OutboundID { get => _OutboundID; set => _OutboundID = value; }
        public string OBDNumber { get => _OBDNumber; set => _OBDNumber = value; }
        public string OBDDate { get => _OBDDate; set => _OBDDate = value; }
        public string CustomerName { get => _CustomerName; set => _CustomerName = value; }
        public string DocumentType { get => _DocumentType; set => _DocumentType = value; }
        public int TodayCount { get => _TodayCount; set => _TodayCount = value; }
        public string DeliveryStatus { get => _DeliveryStatus; set => _DeliveryStatus = value; }
        public string JobOrderRefNo { get => _JobOrderRefNo; set => _JobOrderRefNo = value; }
        public string RoutingDocumentType { get => _RoutingDocumentType; set => _RoutingDocumentType = value; }
        public string WONumber { get => _WONumber; set => _WONumber = value; }
        public string IORefNo { get => _IORefNo; set => _IORefNo = value; }
        public string KitCode { get => _KitCode; set => _KitCode = value; }
        public int TodayInbounds { get => _TodayInbounds; set => _TodayInbounds = value; }
        public int TodayOBDs { get => _TodayOBDs; set => _TodayOBDs = value; }
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
        public int WarehouseGroupID { get => _WarehouseGroupID; set => _WarehouseGroupID = value; }
        public string WarehouseGroupCode { get => _WarehouseGroupCode; set => _WarehouseGroupCode = value; }
        public int WarehouseTypeID { get => _WarehouseTypeID; set => _WarehouseTypeID = value; }
        public string WarehouseType { get => _WarehouseType; set => _WarehouseType = value; }
        public int SupplierID { get => _SupplierID; set => _SupplierID = value; }
        public int ShipmentTypeID { get => _ShipmentTypeID; set => _ShipmentTypeID = value; }
        public int InboundStatusID { get => _InboundStatusID; set => _InboundStatusID = value; }
        public int DeliveryStatusID { get => _DeliveryStatusID; set => _DeliveryStatusID = value; }
        public int CustomerID { get => _CustomerID; set => _CustomerID = value; }
        public int DocumentTypeID { get => _DocumentTypeID; set => _DocumentTypeID = value; }
        public string MCode { get => _MCode; set => _MCode = value; }
        public decimal Version { get => _Version; set => _Version = value; }
        public string ReleaseDate { get => _ReleaseDate; set => _ReleaseDate = value; }
        public string ParameterName { get => _ParameterName; set => _ParameterName = value; }
        public string ControlType { get => _ControlType; set => _ControlType = value; }
        public string ParameterDataType { get => _ParameterDataType; set => _ParameterDataType = value; }
        public string DataSource { get => _DataSource; set => _DataSource = value; }
        public int IsRequired { get => _IsRequired; set => _IsRequired = value; }
        public int MaterialStorageParameterID { get => _MaterialStorageParameterID; set => _MaterialStorageParameterID = value; }
        public string DisplayName { get => _DisplayName; set => _DisplayName = value; }
        public int GoodsMovementDetailsID { get => _GoodsMovementDetailsID; set => _GoodsMovementDetailsID = value; }
        public string CartonCode { get => _CartonCode; set => _CartonCode = value; }
        public int DocQty { get => _DocQty; set => _DocQty = value; }
        public string POSODetailsID { get => _POSODetailsID; set => _POSODetailsID = value; }
        public int CapturingQc { get => _CapturingQc; set => _CapturingQc = value; }
        public int Result { get => _Result; set => _Result = value; }
        public string UserName { get => _UserName; set => _UserName = value; }
        public string Limit { get => _Limit; set => _Limit = value; }
        public string StatusName { get => _StatusName; set => _StatusName = value; }
        public string WarehouseIDs { get => _WarehouseIDs; set => _WarehouseIDs = value; }
        public int AccountId { get => __AccountId; set => __AccountId = value; }
        public string TenantName { get => _TenantName; set => _TenantName = value; }
    }
}