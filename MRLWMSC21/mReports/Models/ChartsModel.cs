using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

//  ====================== Created by M.D.Prasad =========================//

namespace MRLWMSC21.mReports.Models
{
    public class ChartsModel
    {

        public class TotalWHData
        {
            public TotalBins objTotalBins { get; set; }
            public TotalInwardReceipts objTotalInwardReceipts { get; set; }
            public TotalInwardPutaways objTotalInwardPutaways { get; set; }
            public TotalInwardPicking objTotalInwardPicking { get; set; }

            public TotalInboundData objTotalInboundData { get; set; }
            public TotalOutBoundData objTotalOutBoundData { get; set; }

            public TotalBays objTotalBays { get; set; }

            public InventoryAgeing objInventoryAgeing { get; set; }
            public List<ItemLinesAgeing> objItemLinesAgeing { get; set; }
        }
        public class TotalBins
        {
            public string TotalBinLocations { get; set; }
            public string NoOfOccupiedLocations { get; set; }
            public string NoOfEmptyLocations { get; set; }
            public string TotalWarehouseBinVolume { get; set; }
            public string InventoryVolumeOccupied { get; set; }
            public string EmptyVolume { get; set; }
            public string UtilByVolumePercent { get; set; }
            public string TotalWHCapacityByWeight { get; set; }
            public string WightOfAvailableStock { get; set; }
            public string EmptyCapacityByWeight { get; set; }
            public string UtilByWeight { get; set; }      
        }
        public class TotalInwardReceipts
        {
            public string TotalInward { get; set; }
            public string WorkItemsCompleted { get; set; }
            public string ReceiptsToDo { get; set; }
        }

        public class TotalInwardPutaways
        {
            public string TotalInwardWorkLines { get; set; }
            public string WorkItemsCompleted { get; set; }
            public string WorkItemsPanding { get; set; }
        }

        public class TotalInwardPicking
        {
            public string PickingLines { get; set; }
            public string CompletedLines { get; set; }
            public string PendingLines { get; set; }
        }

        public class TotalInboundData
        {
            public string TimeTakenForCompletion { get; set; }
            public string QuantityReceived { get; set; }
            public string NormalizedTime { get; set; }
            public string SOQtyNormalizationFactor { get; set; }
        }

        public class TotalOutBoundData
        {
            public string TimeTakenForCompletion { get; set; }
            public string QuantitySold { get; set; }
            public string NormalizedTime { get; set; }
            public string SOQtyNormalizationFactor { get; set; }
        }

        public class TotalBays
        {
            public string TotalLoadingBays { get; set; }
            public string BaysCurrentlyUsed { get; set; }
        }

        public class InventoryAgeing
        {
            public string VolumeAgeGT6Months { get; set; }
            public string VolumeAgeGT12Months { get; set; }
            public string VolumeAgeGT24Months { get; set; }
            public string NoSKUsGT6Months { get; set; }
            public string NoSKUsGT12Months { get; set; }
            public string NoSKUsGT24Months { get; set; }
        }

        public class ItemLinesAgeing
        {
            public string DimCode { get; set; }
            public string DimName { get; set; }
            public string MonthID { get; set; }
            public string MonthName { get; set; }
            public string YearID { get; set; }
            public string MovementType { get; set; }
            public string GoodsMovementType { get; set; }
            public string Quantity { get; set; }
            public string Lines { get; set; }
        }

        //===================================== New Chats ===============================//

        public class TotalOperatorEfficiency
        {
            public List<TotalUser> objTotalUsers { get; set; }
            public List<TotalInbound> objTotalInbds { get; set; }
            public List<TotalLineNo> objTotalLineno { get; set; }

            public List<TotalUserOBD> objTotalUsersObd { get; set; }
            public List<TotalOutbound> objTotalObbds { get; set; }
            public List<TotalLineNoOBD> objTotalLinenoObd { get; set; }

            public List<TotalInbQty> objTotalInbQty { get; set; }
            public List<TotalObdQty> objTotalObdQty { get; set; }

        }

        public class TotalUser
        {
            public string Totalusers { get; set; }
            public string Month { get; set; }
            public string Year { get; set; }
        }

        public class TotalInbound
        {
            public string TotalInbounds { get; set; }
            public string Month { get; set; }
            public string Year { get; set; }
        }


        public class TotalLineNo
        {
            public string TotalLineNO { get; set; }
            public string Month { get; set; }
            public string Year { get; set; }
        }

        public class TotalInbQty
        {
            public string Qty { get; set; }
            public string Month { get; set; }
            public string Year { get; set; }
        }

        public class TotalUserOBD
        {
            public string Totalusers { get; set; }
            public string Month { get; set; }
            public string Year { get; set; }
        }

        public class TotalOutbound
        {
            public string TotalOutbounds { get; set; }
            public string Month { get; set; }
            public string Year { get; set; }
        }

        public class TotalLineNoOBD
        {
            public string TotalLineNO { get; set; }
            public string Month { get; set; }
            public string Year { get; set; }
        }

        public class TotalObdQty
        {
            public string Qty { get; set; }
            public string Month { get; set; }
            public string Year { get; set; } 
        }

        //===================================== New Chats ===============================//
    }
}