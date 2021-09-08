using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MRLWMSC21Core.Entities
{
    public class SearchCriteria
    {
        private int _InboundID;

        private int _AccountID;

        private int _TenantID;

        private int _WarehouseID;

        private int _SupplierID;

        private int _POHeaderID;

        private int _PODetailsID;

        private int _SupplierInvoiceID;

        private int _SupplierInvoiceDetailsID;

        private int _UserID;

        private string _StoreRefNo;

        private string _MaterialRSN;

        private string _MaterialCode;

        private int _MaterialMasterID;

        private int _VehicleID;

        private string _VehicleNumber;

        private int _LoggedInUserID;

        private int _TransferRequestID;

        private int _TransferRequestDetailsID;

        private int _SuggestionID;

        private decimal _SuggestionFullfilledQuantity;

        private int _ReasonID;

        private string _TransferRequestNo;

        private string _BatchNumber;

        // BEGIN: For Location Filtering

        private string _LocationCode;
        private bool _IsFastMoving;
        private bool _IsQuarantine;
        private bool _IsDockingLocation;
        private bool _IsCrossDockingLocation;
        private bool _IsExcessPickedStagingLocation;
        private bool _IsMixedMaterialAllowed;


        private int _LocationID;
        private int _ZoneID;
        private int _DivisionID;
        private int _PageFormatID;
        private int _CategoryID;

        private int _ContainerID;
        private string _ContainerCode;
        // END: For Location Filtering



        private int _PickListHeaderID;
        private int _PickListDetailsID;
        private string _PickListRefNo;


        // BEGIN: DeNesting Section
        private string _DeNestingJobRef;
        // END: DeNesting Section

        public int InboundID { get => _InboundID; set => _InboundID = value; }
        public int AccountID { get => _AccountID; set => _AccountID = value; }
        public int TenantID { get => _TenantID; set => _TenantID = value; }
        public int WarehouseID { get => _WarehouseID; set => _WarehouseID = value; }
        public int SupplierID { get => _SupplierID; set => _SupplierID = value; }
        public int POHeaderID { get => _POHeaderID; set => _POHeaderID = value; }
        public int PODetailsID { get => _PODetailsID; set => _PODetailsID = value; }
        public int SupplierInvoiceID { get => _SupplierInvoiceID; set => _SupplierInvoiceID = value; }
        public int SupplierInvoiceDetailsID { get => _SupplierInvoiceDetailsID; set => _SupplierInvoiceDetailsID = value; }
        public int UserID { get => _UserID; set => _UserID = value; }
        public string StoreRefNo { get => _StoreRefNo; set => _StoreRefNo = value; }
        public string MaterialRSN { get => _MaterialRSN; set => _MaterialRSN = value; }
        public bool IsFastMoving { get => _IsFastMoving; set => _IsFastMoving = value; }
        public bool IsQuarantine { get => _IsQuarantine; set => _IsQuarantine = value; }
        public bool IsDockingLocation { get => _IsDockingLocation; set => _IsDockingLocation = value; }
        public bool IsCrossDockingLocation { get => _IsCrossDockingLocation; set => _IsCrossDockingLocation = value; }
        public bool IsExcessPickedStagingLocation { get => _IsExcessPickedStagingLocation; set => _IsExcessPickedStagingLocation = value; }
        public bool IsMixedMaterialAllowed { get => _IsMixedMaterialAllowed; set => _IsMixedMaterialAllowed = value; }
        public int ZoneID { get => _ZoneID; set => _ZoneID = value; }
        public int DivisionID { get => _DivisionID; set => _DivisionID = value; }
        public int PageFormatID { get => _PageFormatID; set => _PageFormatID = value; }
        public int CategoryID { get => _CategoryID; set => _CategoryID = value; }
        public int LocationID { get => _LocationID; set => _LocationID = value; }
        public string LocationCode { get => _LocationCode; set => _LocationCode = value; }
        public string MaterialCode { get => _MaterialCode; set => _MaterialCode = value; }
        public int VehicleID { get => _VehicleID; set => _VehicleID = value; }
        public string VehicleNumber { get => _VehicleNumber; set => _VehicleNumber = value; }
        public int LoggedInUserID { get => _LoggedInUserID; set => _LoggedInUserID = value; }
        public int ContainerID { get => _ContainerID; set => _ContainerID = value; }
        public string ContainerCode { get => _ContainerCode; set => _ContainerCode = value; }
        public int TransferRequestID { get => _TransferRequestID; set => _TransferRequestID = value; }
        public int TransferRequestDetailsID { get => _TransferRequestDetailsID; set => _TransferRequestDetailsID = value; }
        public int SuggestionID { get => _SuggestionID; set => _SuggestionID = value; }
        public decimal SuggestionFullfilledQuantity { get => _SuggestionFullfilledQuantity; set => _SuggestionFullfilledQuantity = value; }
        public int ReasonID { get => _ReasonID; set => _ReasonID = value; }
        public int MaterialMasterID { get => _MaterialMasterID; set => _MaterialMasterID = value; }
        public int PickListHeaderID { get => _PickListHeaderID; set => _PickListHeaderID = value; }
        public int PickListDetailsID { get => _PickListDetailsID; set => _PickListDetailsID = value; }
        public string PickListRefNo { get => _PickListRefNo; set => _PickListRefNo = value; }
        public string DeNestingJobRef { get => _DeNestingJobRef; set => _DeNestingJobRef = value; }
        public string TransferRequestNo { get => _TransferRequestNo; set => _TransferRequestNo = value; }
        public string BatchNumber { get => _BatchNumber; set => _BatchNumber = value; }
    }
}
