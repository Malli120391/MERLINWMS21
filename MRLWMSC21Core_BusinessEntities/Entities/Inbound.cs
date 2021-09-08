using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MRLWMSC21Core.Entities
{
    public class Inbound
    {
        private int _InboundID;
        private string _StoreRefNo;


        private DateTime _DocReceivedDate;

        private int _ShipmentTypeID;
        private int _OutboundID;
        private int _SupplierID;
        private int _ConsignmentNoteTypeID;
        private int _FreightCompanyID;
        private int _ClearanceCompanyID;
        private int _InboundStatusID;
        private int _InitiatedByUserID;
        private int _TenantID;
        private int _PriorityLevel;
        private int _CreatedByUserID;
        private int _UpdatedBy;
        private int _GRNDoneBy;
        private int _AccountID;
        private decimal _InvoiceQty;
        private decimal _ReceivedQty;
        private string _DockNumber;
        private int _DockID;

        private int _NoofPackagesReceived;
        private int _NoofPackagesInDocument;

        private string _ShipmentLocation;
        private decimal _ConsignmentNoteTypeValue;

        private decimal _GrossWeight;
        private decimal _CBM;
        private decimal _FreightAmount;
        private decimal _ClearanceAmount;

        private string _FreightInvoiceNo;
        private string _ClearanceInvoiceNo;
        private string _GRNNumber;
        private string _SupplierName;
        private string _InboundStatusName;
        private string _ShipmentType;

        
        private bool _IsDeleted;
        private bool _IsActive;
        private bool _IsRTRPublished;
        private bool _IsChargesRequired;
        private bool _IsChargesUpdated;
        private bool _IsSentForVerification;

        private string _RemarksBy_Ini;
        private string _RemarksBy_StoreIncharge;
        private string _Remarks_Billing;
        private string _Remarks_InDiscrepancy;

        private DateTime _ShipmentExpectedDate;
        private DateTime _ReceiptConfirmDate;
        private DateTime _FreightInvoiceDate;
        private DateTime _ClearanceInvoiceDate;
        private DateTime _PriorityDateTime;
        private DateTime _CreatedOn;
        private DateTime _UpdatedOn;
        private DateTime _GRNTimestamp;
        private DateTime _GRNDate;
        private DateTime _ChargesUpdatedDate;
        private DateTime _SentForVerificationDate;
        private DateTime _ConsignmentNoteTypeDate;

        private string _VehicleNumber;
        private List<GRN> _ShipmentGRN;

        private List<InboundInventoryMap> _InboundInventoryMap;

        private List<PalletizationZoning> _PalletizationPreferences;

        public int InboundID { get => _InboundID; set => _InboundID = value; }
        public string StoreRefNo { get => _StoreRefNo; set => _StoreRefNo = value; }
        public DateTime DocReceivedDate { get => _DocReceivedDate; set => _DocReceivedDate = value; }
        public int ShipmentTypeID { get => _ShipmentTypeID; set => _ShipmentTypeID = value; }
        public int OutboundID { get => _OutboundID; set => _OutboundID = value; }
        public int SupplierID { get => _SupplierID; set => _SupplierID = value; }
        public int ConsignmentNoteTypeID { get => _ConsignmentNoteTypeID; set => _ConsignmentNoteTypeID = value; }
        public int FreightCompanyID { get => _FreightCompanyID; set => _FreightCompanyID = value; }
        public int ClearanceCompanyID { get => _ClearanceCompanyID; set => _ClearanceCompanyID = value; }
        public int InboundStatusID { get => _InboundStatusID; set => _InboundStatusID = value; }
        public int InitiatedByUserID { get => _InitiatedByUserID; set => _InitiatedByUserID = value; }
        public int TenantID { get => _TenantID; set => _TenantID = value; }
        public int PriorityLevel { get => _PriorityLevel; set => _PriorityLevel = value; }
        public int CreatedByUserID { get => _CreatedByUserID; set => _CreatedByUserID = value; }
        public int UpdatedBy { get => _UpdatedBy; set => _UpdatedBy = value; }
        public int GRNDoneBy { get => _GRNDoneBy; set => _GRNDoneBy = value; }
        public int NoofPackagesReceived { get => _NoofPackagesReceived; set => _NoofPackagesReceived = value; }
        public int NoofPackagesInDocument { get => _NoofPackagesInDocument; set => _NoofPackagesInDocument = value; }
        public string ShipmentLocation { get => _ShipmentLocation; set => _ShipmentLocation = value; }
        public decimal ConsignmentNoteTypeValue { get => _ConsignmentNoteTypeValue; set => _ConsignmentNoteTypeValue = value; }
        public decimal GrossWeight { get => _GrossWeight; set => _GrossWeight = value; }
        public decimal CBM { get => _CBM; set => _CBM = value; }
        public decimal FreightAmount { get => _FreightAmount; set => _FreightAmount = value; }
        public decimal ClearanceAmount { get => _ClearanceAmount; set => _ClearanceAmount = value; }
        public string FreightInvoiceNo { get => _FreightInvoiceNo; set => _FreightInvoiceNo = value; }
        public string ClearanceInvoiceNo { get => _ClearanceInvoiceNo; set => _ClearanceInvoiceNo = value; }
        public string GRNNumber { get => _GRNNumber; set => _GRNNumber = value; }
        public bool IsDeleted { get => _IsDeleted; set => _IsDeleted = value; }
        public bool IsActive { get => _IsActive; set => _IsActive = value; }
        public bool IsRTRPublished { get => _IsRTRPublished; set => _IsRTRPublished = value; }
        public bool IsChargesRequired { get => _IsChargesRequired; set => _IsChargesRequired = value; }
        public bool IsChargesUpdated { get => _IsChargesUpdated; set => _IsChargesUpdated = value; }
        public bool IsSentForVerification { get => _IsSentForVerification; set => _IsSentForVerification = value; }
        public string RemarksBy_Ini { get => _RemarksBy_Ini; set => _RemarksBy_Ini = value; }
        public string RemarksBy_StoreIncharge { get => _RemarksBy_StoreIncharge; set => _RemarksBy_StoreIncharge = value; }
        public string Remarks_Billing { get => _Remarks_Billing; set => _Remarks_Billing = value; }
        public string Remarks_InDiscrepancy { get => _Remarks_InDiscrepancy; set => _Remarks_InDiscrepancy = value; }
        public DateTime ShipmentExpectedDate { get => _ShipmentExpectedDate; set => _ShipmentExpectedDate = value; }
        public DateTime ReceiptConfirmDate { get => _ReceiptConfirmDate; set => _ReceiptConfirmDate = value; }
        public DateTime FreightInvoiceDate { get => _FreightInvoiceDate; set => _FreightInvoiceDate = value; }
        public DateTime ClearanceInvoiceDate { get => _ClearanceInvoiceDate; set => _ClearanceInvoiceDate = value; }
        public DateTime PriorityDateTime { get => _PriorityDateTime; set => _PriorityDateTime = value; }
        public DateTime CreatedOn { get => _CreatedOn; set => _CreatedOn = value; }
        public DateTime UpdatedOn { get => _UpdatedOn; set => _UpdatedOn = value; }
        public DateTime GRNTimestamp { get => _GRNTimestamp; set => _GRNTimestamp = value; }
        public DateTime GRNDate { get => _GRNDate; set => _GRNDate = value; }
        public DateTime ChargesUpdatedDate { get => _ChargesUpdatedDate; set => _ChargesUpdatedDate = value; }
        public DateTime SentForVerificationDate { get => _SentForVerificationDate; set => _SentForVerificationDate = value; }
        public DateTime ConsignmentNoteTypeDate { get => _ConsignmentNoteTypeDate; set => _ConsignmentNoteTypeDate = value; }

        public string SupplierName { get => _SupplierName; set => _SupplierName = value; }
        public string InboundStatusName { get => _InboundStatusName; set => _InboundStatusName = value; }
        public string ShipmentType { get => _ShipmentType; set => _ShipmentType = value; }

        public string VehicleNumber { get => _VehicleNumber; set => _VehicleNumber = value; }
        public List<InboundInventoryMap> InboundInventoryMap { get => _InboundInventoryMap; set => _InboundInventoryMap = value; }
        public List<GRN> ShipmentGRN { get => _ShipmentGRN; set => _ShipmentGRN = value; }
        public List<PalletizationZoning> PalletizationPreferences { get => _PalletizationPreferences; set => _PalletizationPreferences = value; }
        public int AccountID { get => _AccountID; set => _AccountID = value; }
        public decimal InvoiceQty { get => _InvoiceQty; set => _InvoiceQty = value; }
        public decimal ReceivedQty { get => _ReceivedQty; set => _ReceivedQty = value; }
        public string DockNumber { get => _DockNumber; set => _DockNumber = value; }
        public int DockID { get => _DockID; set => _DockID = value; }
    }

    public class InboundByID
    {
        private string _InboundID;

        public string InboundID { get => _InboundID; set => _InboundID = value; }
    }
}
