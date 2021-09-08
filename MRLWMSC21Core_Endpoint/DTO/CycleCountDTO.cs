using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MRLWMSC21_Endpoint.DTO
{
    public class CycleCountDTO
    {
        private string _UserId;
        private List<CycleCountType> _CCType;
        private string _Location;
        private decimal _BoxQty;
        private bool _IsSatisfiedBoxQty;
        private string _SerialNo;
        private string _MRP;
        private string _MOP;
        private string _MaterialCode;
        private string _BatchNo;
        private List<ColorDTO> _ColorCodes;
        private List<StorageLocationDTO> _SLOC;
        private string _SelectedSLOC;
        private string _CCName;
        private string _SelectedColorCode;
        private string _Result;
        private string _TotalSystemLocationQuantity;
        private string _TotalSystemStockAtLocation;
        private string _TotalScannedSKUQuantity;
        private string _TotalInventoryQuantityScannedAtLocation;
        private string _TotalSystemStockAllLocations;
        private string _TotalNoOfLocationsToScan;
        private string _TotalNoOfLocationsScanned;
        private bool _UserConfirmReDo;
        private int _AccountID;
        private string _Count;
        private string _MfgDate;
        private string _ExpDate;
     
        private string _ProjectRefNo;
        private string _PalletNo;
        private string _CCQty;
        private string _WarehouseID;
        private string _TenantId;

        private string _Rack;
        private string _Column;
        private string _Level;
        private string _StorageLocation;
        private string _CycleCountSeqCode;

        public string UserId { get => _UserId; set => _UserId = value; }
        public List<CycleCountType> CCType { get => _CCType; set => _CCType = value; }
        public string Location { get => _Location; set => _Location = value; }
        public decimal BoxQty { get => _BoxQty; set => _BoxQty = value; }
        public bool IsSatisfiedBoxQty { get => _IsSatisfiedBoxQty; set => _IsSatisfiedBoxQty = value; }
        public string SerialNo { get => _SerialNo; set => _SerialNo = value; }
        public string MRP { get => _MRP; set => _MRP = value; }
        public string MOP { get => _MOP; set => _MOP = value; }
        public string MaterialCode { get => _MaterialCode; set => _MaterialCode = value; }
        public string BatchNo { get => _BatchNo; set => _BatchNo = value; }
        public List<ColorDTO> ColorCodes { get => _ColorCodes; set => _ColorCodes = value; }
        public List<StorageLocationDTO> SLOC { get => _SLOC; set => _SLOC = value; }
        public string CCName { get => _CCName; set => _CCName = value; }
        public string SelectedSLOC { get => _SelectedSLOC; set => _SelectedSLOC = value; }
        public string SelectedColorCode { get => _SelectedColorCode; set => _SelectedColorCode = value; }
        public string Result { get => _Result; set => _Result = value; }
        public string TotalSystemLocationQuantity { get => _TotalSystemLocationQuantity; set => _TotalSystemLocationQuantity = value; }
        public string TotalSystemStockAtLocation { get => _TotalSystemStockAtLocation; set => _TotalSystemStockAtLocation = value; }
        public string TotalScannedSKUQuantity { get => _TotalScannedSKUQuantity; set => _TotalScannedSKUQuantity = value; }
        public string TotalInventoryQuantityScannedAtLocation { get => _TotalInventoryQuantityScannedAtLocation; set => _TotalInventoryQuantityScannedAtLocation = value; }
    
        public string TotalSystemStockAllLocations { get => _TotalSystemStockAllLocations; set => _TotalSystemStockAllLocations = value; }
        public string TotalNoOfLocationsToScan { get => _TotalNoOfLocationsToScan; set => _TotalNoOfLocationsToScan = value; }
        public string TotalNoOfLocationsScanned { get => _TotalNoOfLocationsScanned; set => _TotalNoOfLocationsScanned = value; }
        public bool UserConfirmReDo { get => _UserConfirmReDo; set => _UserConfirmReDo = value; }
        public int AccountID { get => _AccountID; set => _AccountID = value; }
        public string Count { get => _Count; set => _Count = value; }
        public string MfgDate { get => _MfgDate; set => _MfgDate = value; }
        public string ExpDate { get => _ExpDate; set => _ExpDate = value; }
        public string ProjectRefNo { get => _ProjectRefNo; set => _ProjectRefNo = value; }
        public string PalletNo { get => _PalletNo; set => _PalletNo = value; }
        public string CCQty { get => _CCQty; set => _CCQty = value; }
        public string WarehouseID { get => _WarehouseID; set => _WarehouseID = value; }
        public string TenantId { get => _TenantId; set => _TenantId = value; }

        //===============Modified By M.D.Prasad On 05-May-2020 For Adding New Params =======================//
        public string Rack { get => _Rack; set => _Rack = value; }
        public string Column { get => _Column; set => _Column = value; }
        public string Level { get => _Level; set => _Level = value; }
        public string StorageLocation { get => _StorageLocation; set => _StorageLocation = value; }
        public string CycleCountSeqCode { get => _CycleCountSeqCode; set => _CycleCountSeqCode = value; }
    }

    public class CycleCountType
    {
        private string _CCName;
        private bool _IsCCForStockTake;

        public string CCName { get => _CCName; set => _CCName = value; }
        public bool IsCCForStockTake { get => _IsCCForStockTake; set => _IsCCForStockTake = value; }

    }
}