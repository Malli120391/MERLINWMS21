using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MRLWMSC21Core.Library;
//using MRLWMSC21Core.Library;

namespace MRLWMSC21Core.Entities
{
    public class Location
    {
        private int _LocationID;

        private int _WarehouseID;

        private string _LocationCode;

        private decimal _Length;
        private decimal _Width;
        private decimal _Height;
        private decimal _MaxWeight;

        private int _RackID;
        private string _RackCode;

        private int _ColumnID;
        private string _ColumnCode;

        private int _LevelID;
        private string _LevelCode;

        private int _PhaseID;
        private string _PhaseCode;

        private int _ZoneID;
        private string _ZoneCode;

        private int _DockID;
        private string _DockCode;

        private string _SystemLocationCode;

        private string _AssociatedGateOfAccess;

        private bool _IsFastMoving;
        private bool _IsQuarantine;
        private bool _IsDockingArea;
        private bool _IsCrossDockingLocation;
        private bool _IsExcessPickedStaging;
        private bool _IsMixedMaterialAllowed;
        private bool _IsBlockedForCycleCount;
        private bool _IsFlaggedForCC;
        private bool _IsDeNestingLocation;
        private bool _IsBinLocation;
        private bool _IsCheckDigitScanned;

        private int _BlockedForCycleCountID;
        private string _BlockedForCycleCount;

        private int _LocationBlockedByUserID;

        private decimal _TotalSystemLocationQuantity;
        private decimal _TotalSystemStockAtLocation;
        private decimal _TotalScannedSKUQuantity;
        private decimal _TotalInventoryQuantityScannedAtLocation;
        private decimal _TotalSystemStockAllLocations;
        private decimal _TotalNoOfLocationsToScan;
        private decimal _TotalNoOfLocationsScanned;


        public int LocationID { get => _LocationID; set => _LocationID = value; }
        public int WarehouseID { get => _WarehouseID; set => _WarehouseID = value; }
        public string LocationCode { get => _LocationCode; set => _LocationCode = value; }
        public decimal Length { get => _Length; set => _Length = value; }
        public decimal Width { get => _Width; set => _Width = value; }
        public decimal Height { get => _Height; set => _Height = value; }
        public decimal MaxWeight { get => _MaxWeight; set => _MaxWeight = value; }
        public int RackID { get => _RackID; set => _RackID = value; }
        public string RackCode { get => _RackCode; set => _RackCode = value; }
        public int ColumnID { get => _ColumnID; set => _ColumnID = value; }
        public string ColumnCode { get => _ColumnCode; set => _ColumnCode = value; }
        public int LevelID { get => _LevelID; set => _LevelID = value; }
        public string LevelCode { get => _LevelCode; set => _LevelCode = value; }
        public int PhaseID { get => _PhaseID; set => _PhaseID = value; }
        public string PhaseCode { get => _PhaseCode; set => _PhaseCode = value; }
        public int ZoneID { get => _ZoneID; set => _ZoneID = value; }
        public string ZoneCode { get => _ZoneCode; set => _ZoneCode = value; }
        public string SystemLocationCode { get => _SystemLocationCode; set => _SystemLocationCode = value; }
        public bool IsFastMoving { get => _IsFastMoving; set => _IsFastMoving = value; }
        public bool IsQuarantine { get => _IsQuarantine; set => _IsQuarantine = value; }
        public bool IsDockingArea { get => _IsDockingArea; set => _IsDockingArea = value; }
        public bool IsCrossDockingLocation { get => _IsCrossDockingLocation; set => _IsCrossDockingLocation = value; }
        public bool IsExcessPickedStaging { get => _IsExcessPickedStaging; set => _IsExcessPickedStaging = value; }
        public bool IsMixedMaterialAllowed { get => _IsMixedMaterialAllowed; set => _IsMixedMaterialAllowed = value; }
        public bool IsBlockedForCycleCount { get => _IsBlockedForCycleCount; set => _IsBlockedForCycleCount = value; }
        public bool IsFlaggedForCC { get => _IsFlaggedForCC; set => _IsFlaggedForCC = value; }
        public int BlockedForCycleCountID { get => _BlockedForCycleCountID; set => _BlockedForCycleCountID = value; }
        public int DockID { get => _DockID; set => _DockID = value; }
        public string DockCode { get => _DockCode; set => _DockCode = value; }
        public bool IsCheckDigitScanned { get => _IsCheckDigitScanned; set => _IsCheckDigitScanned = value; }
        public string AssociatedGateOfAccess { get => _AssociatedGateOfAccess; set => _AssociatedGateOfAccess = value; }
        public string BlockedForCycleCount { get => _BlockedForCycleCount; set => _BlockedForCycleCount = value; }
        public bool IsDeNestingLocation { get => _IsDeNestingLocation; set => _IsDeNestingLocation = value; }
        public bool IsBinLocation { get => _IsBinLocation; set => _IsBinLocation = value; }
        public decimal TotalSystemLocationQuantity { get => _TotalSystemLocationQuantity; set => _TotalSystemLocationQuantity = value; }
        public decimal TotalSystemStockAtLocation { get => _TotalSystemStockAtLocation; set => _TotalSystemStockAtLocation = value; }
        public decimal TotalScannedSKUQuantity { get => _TotalScannedSKUQuantity; set => _TotalScannedSKUQuantity = value; }
        public decimal TotalInventoryQuantityScannedAtLocation { get => _TotalInventoryQuantityScannedAtLocation; set => _TotalInventoryQuantityScannedAtLocation = value; }
        public int LocationBlockedByUserID { get => _LocationBlockedByUserID; set => _LocationBlockedByUserID = value; }
        public decimal TotalSystemStockAllLocations { get => _TotalSystemStockAllLocations; set => _TotalSystemStockAllLocations = value; }
        public decimal TotalNoOfLocationsToScan { get => _TotalNoOfLocationsToScan; set => _TotalNoOfLocationsToScan = value; }
        public decimal TotalNoOfLocationsScanned { get => _TotalNoOfLocationsScanned; set => _TotalNoOfLocationsScanned = value; }

        public void GateOfAccess()
        {
            int _iColumnNumber;
            _iColumnNumber = ConversionUtility.ConvertToInt(ColumnCode);


            if (_iColumnNumber <= 10)
                AssociatedGateOfAccess = "A" + LevelCode;
            else
                AssociatedGateOfAccess = "B" + LevelCode;
        }
    }
}
