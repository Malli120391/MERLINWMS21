using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRLWMSC21_Library.WMS_ServiceObjects
{
    public class SalesOrder
    {
        public int SOSysId { get; set; }
        public int SOId { get; set; }
        public DateTime CreatedOn { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedUser { get; set; }
        public string CustomerPO { get; set; }
        public string SONumber { get; set; }
        public int WarehouseId { get; set; }
        public int OutboundId { get; set; }

        public IList<LineItem> SOLineItems { get; set; }
        public Customer CUST { get; set; }

    }

    public class Customer 
    {
        public int CustomerId { get; set; }
        public string CustomerCode { get; set; }
        public DateTime CreatedOn { get; set; }
        public IList<AddressRecord> AddressBook {get;set;}
                                
    }

    public class AddressRecord 
    {
        public string City { get; set; }
        public string Country { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string Street1 { get; set; }
        public string street2 { get; set; }
        public string ZipCode { get; set; }

    }

    /* not a standard public object*/
    public class AvailableStockData
    {
        public IDictionary<int, SalesOrder> orders { get; set; }
        public IDictionary<int, MSP> MSP { get; set; }
        public IList<SuggestedStockInfo> suggestedStockData { get; set; }
    }
}
