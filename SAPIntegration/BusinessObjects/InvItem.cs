using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAPIntegration.BusinessObjects
{
    public class InvItem
    {
        
        
        public IList<InvItemDim> msps { get; set; }
        public IList<InvVendor> vendors { get; set; }
        public IList<BusinessUOM> UOMS { get; set; }
        public IList<Measure> measures { get; set; }
        public IList<InvItem> childItems { get; set; }
        public IList<ItemPrice> price { get; set; }
        public IList<ItemSetup> ItemSetups { get; set; }

        public IList<string> alternatePartNOList { get; set; }
        public IList<string> skus { get; set; }
        

        public string partNumber { get; set; }
        public string OEMCode { get; set; }
        public string StorageGroup { get; set; }
        public string TrackingGroup { get; set; }

        public string ItemID { get; set; }
        public int ItemTypeID { get; set; }
        public string ItemType { get; set; }
        public int ProductTypeID { get; set; }
        public string ProductGroup { get; set; }
        public int handlingTypeID { get; set; }
        public int requestedBy { get; set; }
        public int createdBy { get; set; }
        public string createdUser { get; set; }

        public string description { get; set; }

        public decimal qtyPerLayer { get; set; }
        public decimal binCapacity { get; set; }
        public int TenantID { get; set; }
        public decimal replenishmentQty { get; set; }
        public string lastUpdated { get; set; }


    }

    public class KitPlanner 
    {
        public int KitID { get; set; }
        public string kitCode { get; set; }
        public InvItem parentPart { get; set; }
        public IList<InvItem> childParts { get; set; }
    }

    public class BusinessUOM 
    {
        public int UOMID { get; set; }
        public string UOMCode { get; set; }
        public int UOMTypeID { get; set; }
        public string UOMTypeCode { get; set; }
        public decimal price { get; set; }
        public decimal priceQty { get; set; }
        public decimal priceUnit { get; set; }
        
    }

    public class Measure 
    {
        public int UOMID { get; set; }
        public string UOM { get; set; }
        public string measureCode { get; set; }
        public int measureID { get; set; }
        public decimal value { get; set; }
    }

    public class ItemPrice
    {
        public BusinessUOM uom { get; set; }
        public decimal price { get; set; }
        public InvCurrency currency { get; set; }

    }

    public class ItemSetup
    {
        
        public string SetupType  {get;set;}
        public decimal minQty {get;set;}
        public decimal maxQty {get;set;}
        public int LeadTime {get;set;}
        
    }
}
