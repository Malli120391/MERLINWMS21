using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MRLWMSC21_Endpoint.Constants;

namespace MRLWMSC21_Endpoint.DTO
{
    public class InventoryDTO
    {
        private Constants.EndpointConstants.ScanType _ScanType;

        private string _MaterialCode;
        private string _RSN;
        private string _LocationCode;
        private string _ContainerCode;
        private string _toContainerCode;
        private string _ReferenceDocumentNumber;
        private string _VehicleNumber; 

        private string _OBDNumber;

        private int _OutboundID;
        private int _MaterialMasterID;
        private int _MaterialTransactionID;
        
        private int _VehicleID;
        private int _ReferenceDocumentID;
        private int _ContainerID;
        private int _LocationID;
        private string _MonthOfMfg;
        private string _YearOfMfg;
        private string _StorageLocation;

        private int _DockGoodsMovementID;

        private string _MOP;
        private int _ColorID;
        private int _PosoDetailsID;
        private int _StorageLocationID;
        private string _MRP;
        private string _Quantity;
        private int _CreatedBy;
        private int _ReceivedInUoM;

        private bool _IsDamaged;
        private bool _IsReceived;
        private bool _IsDispatched;
        private int _UserId;
        private List<ColorDTO> _ColorCodes;
        private bool _IsFinishedGoods;
        private bool _IsRawMaterial;
        private bool _IsConsumables;
        private string _SLOC;
        private string _Color;
        private string _Result;
        private string _DocumentProcessedQuantity;
        public string _DocumentQuantity;
        private string _MaterialShortDescription;
        private bool _UserConfirmedExcessTransaction;
        private bool _UserConfirmReDo;
        private string _SuggestionID;
        private string _ExpDate;
        private string _BatchNo;
        private string _ProjectNo;
        private string _SerialNo;
        private string _MfgDate;
        private string _OldMRP;
        private string _ToLocationCode;
        private string _NewRSN;
        private bool _IsMaterialParent;
        private string _HasDisp;
        private string _AccountID;
        private string _TenantCode;
        private string _TransferRefNo;
        private int _TransferRefId;
        private string _WarehouseId;
        private string _Warehouse;
        private string _TenantID;
        private string _ToStorageLocation;
        


        public EndpointConstants.ScanType ScanType { get => _ScanType; set => _ScanType = value; }
        public string MaterialCode { get => _MaterialCode; set => _MaterialCode = value; }
        public string RSN { get => _RSN; set => _RSN = value; }
        public string LocationCode { get => _LocationCode; set => _LocationCode = value; }
        public string ContainerCode { get => _ContainerCode; set => _ContainerCode = value; }
        public string ReferenceDocumentNumber { get => _ReferenceDocumentNumber; set => _ReferenceDocumentNumber = value; }
        public string VehicleNumber { get => _VehicleNumber; set => _VehicleNumber = value; }
        public string OBDNumber { get => _OBDNumber; set => _OBDNumber = value; }
        public int OutboundID { get => _OutboundID; set => _OutboundID = value; }
        public int MaterialMasterID { get => _MaterialMasterID; set => _MaterialMasterID = value; }
        public int VehicleID { get => _VehicleID; set => _VehicleID = value; }
        public int ReferenceDocumentID { get => _ReferenceDocumentID; set => _ReferenceDocumentID = value; }
        public int ContainerID { get => _ContainerID; set => _ContainerID = value; }
        public int LocationID { get => _LocationID; set => _LocationID = value; }
        public string MonthOfMfg { get => _MonthOfMfg; set => _MonthOfMfg = value; }
        public string YearOfMfg { get => _YearOfMfg; set => _YearOfMfg = value; }
        public int MaterialTransactionID { get => _MaterialTransactionID; set => _MaterialTransactionID = value; }

        public int DockGoodsMovementID { get => _DockGoodsMovementID; set => _DockGoodsMovementID = value; }
        public string MOP { get => _MOP; set => _MOP = value; }
        public int ColorID { get => _ColorID; set => _ColorID = value; }
        public int PosoDetailsID { get => _PosoDetailsID; set => _PosoDetailsID = value; }
        public int StorageLocationID { get => _StorageLocationID; set => _StorageLocationID = value; }
        public string MRP { get => _MRP; set => _MRP = value; }
        public string Quantity { get => _Quantity; set => _Quantity = value; }
        public int CreatedBy { get => _CreatedBy; set => _CreatedBy = value; }
        public int ReceivedInUoM { get => _ReceivedInUoM; set => _ReceivedInUoM = value; }
        public bool IsDamaged { get => _IsDamaged; set => _IsDamaged = value; }
        public bool IsReceived { get => _IsReceived; set => _IsReceived = value; }
        public bool IsDispatched { get => _IsDispatched; set => _IsDispatched = value; }
        public int UserId { get => _UserId; set => _UserId = value; }
        public List<ColorDTO> ColorCodes { get => _ColorCodes; set => _ColorCodes = value; }
        public bool IsFinishedGoods { get => _IsFinishedGoods; set => _IsFinishedGoods = value; }
        public bool IsRawMaterial { get => _IsRawMaterial; set => _IsRawMaterial = value; }
        public bool IsConsumables { get => _IsConsumables; set => _IsConsumables = value; }
        public string SLOC { get => _SLOC; set => _SLOC = value; }
        public string Color { get => _Color; set => _Color = value; }
        public string Result { get => _Result; set => _Result = value; }
        public string OldMRP { get => _OldMRP; set => _OldMRP = value; }
        public string DocumentProcessedQuantity { get => _DocumentProcessedQuantity; set => _DocumentProcessedQuantity = value; }
        public string DocumentQuantity { get => _DocumentQuantity; set => _DocumentQuantity = value; }
         public string MaterialShortDescription { get => _MaterialShortDescription; set => _MaterialShortDescription = value; }
        public bool UserConfirmedExcessTransaction { get => _UserConfirmedExcessTransaction; set => _UserConfirmedExcessTransaction = value; }
        public bool UserConfirmReDo { get => _UserConfirmReDo; set => _UserConfirmReDo = value; }
        public string SuggestionID { get => _SuggestionID; set => _SuggestionID = value; }
       
        public string ToLocationCode { get => _ToLocationCode; set => _ToLocationCode = value; }
        public string NewRSN { get => _NewRSN; set => _NewRSN = value; }
        public bool IsMaterialParent { get => _IsMaterialParent; set => _IsMaterialParent = value; }
        public string ExpDate { get => _ExpDate; set => _ExpDate = value; }
        public string BatchNo { get => _BatchNo; set => _BatchNo = value; }
        public string ProjectNo { get => _ProjectNo; set => _ProjectNo = value; }
        public string SerialNo { get => _SerialNo; set => _SerialNo = value; }
        public string MfgDate { get => _MfgDate; set => _MfgDate = value; }
        public string HasDisp { get => _HasDisp; set => _HasDisp = value; }
        public string AccountID { get => _AccountID; set => _AccountID = value; }
        public string TenantCode { get => _TenantCode; set => _TenantCode = value; }
        public string TransferRefNo { get => _TransferRefNo; set => _TransferRefNo = value; }
        public int TransferRefId { get => _TransferRefId; set => _TransferRefId = value; }
        public string ToContainerCode { get => _toContainerCode; set => _toContainerCode = value; }
        public string WarehouseId { get => _WarehouseId; set => _WarehouseId = value; }
        public string Warehouse { get => _Warehouse; set => _Warehouse = value; }
        public string TenantID { get => _TenantID; set => _TenantID = value; }
        public string StorageLocation { get => _StorageLocation; set => _StorageLocation = value; }
        public string ToStorageLocation { get => _ToStorageLocation; set => _ToStorageLocation = value; }
        
    }
}