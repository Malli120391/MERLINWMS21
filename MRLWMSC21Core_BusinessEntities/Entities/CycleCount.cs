using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MRLWMSC21Core.Entities
{
    public class CycleCount
    {
        private int _CCM_TRN_CycleCount_ID;
        private int _CCM_MST_CycleCount_ID;
        private int _CCM_CNF_AccountCycleCount_ID;
        private int _CCM_MST_CycleCountStatus_ID;
        private int _SeqNo;
        private int _CreatedBy;
        private int _UpdatedBy;

        private string _AccountCycleCountName;
        private string _CycleCountCode;
        private int _AccountID;


        private DateTime _PlannedStart;
        private DateTime _PlannedEnd;
        private DateTime _InitiatedTimestamp;
        private DateTime _CompletionTimestamp;
        private DateTime _CreatedOn;
        private DateTime _UpdatedOn;


        private string _InitiationRemarks;
        private string _CompletionRemarks;

        private bool _IsStandardCycleCount;
        private bool _IsABCCycleCount;
        private bool _IsCCForLocationsWithMovement;
        private bool _IsStockTakeCycleCount;

        private bool _IsInitiated;
        private bool _IsCompleted;
        private bool _IsActive;
        private bool _IsDeleted;

        private decimal _AvailableQuantity;
        private decimal _LostQuantity;
        private decimal _FoundQuantity;

        private decimal _TotalSystemLocationQuantity;
        private decimal _TotalSystemLocationSKUQuantity;
        private decimal _TotalScannedSKUQuantity;
        private decimal _TotalScannedLocationQuantity;

        public int CCM_TRN_CycleCount_ID { get => _CCM_TRN_CycleCount_ID; set => _CCM_TRN_CycleCount_ID = value; }
        public int CCM_MST_CycleCount_ID { get => _CCM_MST_CycleCount_ID; set => _CCM_MST_CycleCount_ID = value; }
        public int CCM_CNF_AccountCycleCount_ID { get => _CCM_CNF_AccountCycleCount_ID; set => _CCM_CNF_AccountCycleCount_ID = value; }
        public int CCM_MST_CycleCountStatus_ID { get => _CCM_MST_CycleCountStatus_ID; set => _CCM_MST_CycleCountStatus_ID = value; }
        public int SeqNo { get => _SeqNo; set => _SeqNo = value; }
        public int CreatedBy { get => _CreatedBy; set => _CreatedBy = value; }
        public int UpdatedBy { get => _UpdatedBy; set => _UpdatedBy = value; }
        public string AccountCycleCountName { get => _AccountCycleCountName; set => _AccountCycleCountName = value; }
        public string CycleCountCode { get => _CycleCountCode; set => _CycleCountCode = value; }
        public DateTime PlannedStart { get => _PlannedStart; set => _PlannedStart = value; }
        public DateTime PlannedEnd { get => _PlannedEnd; set => _PlannedEnd = value; }
        public DateTime InitiatedTimestamp { get => _InitiatedTimestamp; set => _InitiatedTimestamp = value; }
        public DateTime CompletionTimestamp { get => _CompletionTimestamp; set => _CompletionTimestamp = value; }
        public DateTime CreatedOn { get => _CreatedOn; set => _CreatedOn = value; }
        public DateTime UpdatedOn { get => _UpdatedOn; set => _UpdatedOn = value; }
        public string InitiationRemarks { get => _InitiationRemarks; set => _InitiationRemarks = value; }
        public string CompletionRemarks { get => _CompletionRemarks; set => _CompletionRemarks = value; }
        public bool IsInitiated { get => _IsInitiated; set => _IsInitiated = value; }
        public bool IsCompleted { get => _IsCompleted; set => _IsCompleted = value; }
        public bool IsActive { get => _IsActive; set => _IsActive = value; }
        public bool IsDeleted { get => _IsDeleted; set => _IsDeleted = value; }
        public decimal AvailableQuantity { get => _AvailableQuantity; set => _AvailableQuantity = value; }
        public decimal LostQuantity { get => _LostQuantity; set => _LostQuantity = value; }
        public decimal FoundQuantity { get => _FoundQuantity; set => _FoundQuantity = value; }
        public bool IsStandardCycleCount { get => _IsStandardCycleCount; set => _IsStandardCycleCount = value; }
        public bool IsABCCycleCount { get => _IsABCCycleCount; set => _IsABCCycleCount = value; }
        public bool IsCCForLocationsWithMovement { get => _IsCCForLocationsWithMovement; set => _IsCCForLocationsWithMovement = value; }
        public bool IsStockTakeCycleCount { get => _IsStockTakeCycleCount; set => _IsStockTakeCycleCount = value; }
        public decimal TotalSystemLocationQuantity { get => _TotalSystemLocationQuantity; set => _TotalSystemLocationQuantity = value; }
        public decimal TotalSystemLocationSKUQuantity { get => _TotalSystemLocationSKUQuantity; set => _TotalSystemLocationSKUQuantity = value; }
        public decimal TotalScannedSKUQuantity { get => _TotalScannedSKUQuantity; set => _TotalScannedSKUQuantity = value; }
        public decimal TotalScannedLocationQuantity { get => _TotalScannedLocationQuantity; set => _TotalScannedLocationQuantity = value; }
        public int AccountID { get => _AccountID; set => _AccountID = value; }
    }
}
