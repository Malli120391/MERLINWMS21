using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MRLWMSC21.mOutbound.OptimalPickNote
{
    public class SOLineItemData
    {
        public string MCode { get; set; }
        public string MDesc { get; set; }
        public int MaterialID { get; set; }
        public string SO_UOM { get; set; }
        public decimal SO_QTY { get; set; }
        public decimal SO_UOM_QTY { get; set; }

        public string Base_UOM { get; set; }
        public decimal Base_UOM_QTY { get; set; }
        

        public int LineNo { get; set; }
        public int SOID { get; set; }
        public decimal ConvesionValue { get; set; }
        public int KitPlannerID { get; set; }
        public int SODetailsID { get; set; }
                
        public IDictionary<int, string> RequestedSOMSP { get; set; }
        public IList<LiveStockData> StockData { get; set; }
        
    }
}