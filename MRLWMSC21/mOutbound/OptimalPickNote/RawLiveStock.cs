using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MRLWMSC21.mOutbound.OptimalPickNote
{
    public class RawLiveStock
    {
       
        public int HUBox { get; set; }
        public int NoOfUnits { get; set; }
        public string LocationCode { get; set; }
        public bool isMatchedMSP { get; set; }
        public int LocID { get; set; }
        public int LocGenID { get; set; }
        public string serialNo { get; set; }
        public string BatchNo { get; set; }
        public decimal AvailQty { get; set; }
        public decimal pickableQty { get; set; }
        public string Mcode { get; set; }
        public string MDesc { get; set; }
        public string ExpiryDate { get; set; }
        public int GoodsMovementID { get; set; }
        public int MaterialID { get; set; }

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