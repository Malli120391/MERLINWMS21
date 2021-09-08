using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MRLWMSC21.mOutbound.OptimalPickNote
{
    public class LiveStockData
    {
        public int HUBox { get; set; }
        public int NoOfUnits { get; set; }
        public string LocationCode { get; set; }
        public string serialNo { get; set; }
        public string batchNo { get; set; }
        public decimal availQty { get; set; }
        public decimal ExpDate { get; set; }
        public bool isMatchedMSP { get; set; }
        public int GoodsMovementID { get; set; }
        public IDictionary<int, string> ItemMSPInfo { get; set; }

        public int HasDiscrepency { get; set; }
        public int IsDamaged { get; set; }
        public int KitPlannerID { get; set; }


        public string MSPValues { get; set; }
        public decimal PickedQty { get; set; }
        public int LocationID { get; set; }
        public int SODetailsID { get; set; }

        public int POSODetailsID { get; set; }
        public decimal Quantity { get; set; }
        public int TransactionDocID { get; set; }
        public int WarehouseID { get; set; }


    }
}