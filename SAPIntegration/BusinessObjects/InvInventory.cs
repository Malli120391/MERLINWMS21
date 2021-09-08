using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAPIntegration.BusinessObjects
{
    public class InvInventory
    {

    }

    public class Inbound 
    {
        public string shipmentType { get; set; }
        public IList<InvPurchaseOrderInfo> POList { get; set; }
        public InvVendor vendor { get; set; }
        public int InboundID { get; set; }
        public string InboundCode { get; set; }
        public IList<string> storeRefNo { get; set; }
    }

    public class Outbound 
    {
        public string outboundType { get; set; }
        public IList<InvSalesOrderInfo> SOList { get; set; }
        public int outboundID { get; set; }
        public string outboundCode { get; set; }
        public InvCustomer customer { get; set; }
        public IList<string> storeRefNo { get; set; }

    }
}
