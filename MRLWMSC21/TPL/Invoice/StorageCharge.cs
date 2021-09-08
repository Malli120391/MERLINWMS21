using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MRLWMSC21.TPL.Invoice
{
    public class StorageCharge
    {
        
        public int materialID { get; set; }
        public string materialMasterCode { get; set; }
        public string materialName { get; set; }
        public int UomID { get; set; }
        public string UOM { get; set; }
        public decimal perBinQuantity { get; set; }
        public int storageType { get; set; }
        public decimal area { get; set; }
        public decimal volume { get; set; }
        public int spaceUtilizationID { get; set; }
        public decimal weight { get; set; }
        public decimal rentalConstantCharge { get; set; }
        public string unitMeasure { get; set; }
        public string rateUom { get; set; }

        public IList<MaterialStorageCharge> materialStorageCharges { get; set;}
        public IList<MaterialDetailedCharges> charges { get; set; }
        
    }

    public class MaterialStorageCharge 
    {

        public string FromDate { get; set; }
        public string ToDate { get; set; }

        public string materialAvailDate { get; set; }
        public decimal quantity { get; set; }
        public IList<Charge> charges { get; set; }

    }

    public class MaterialDetailedCharges 
    {
        public decimal price { get; set; }
        public string storageType { get; set; }
        public string chargeName { get; set; }
        public int rateID { get; set; }
        public int UomID { get; set; }
        public string UOM { get; set; }
        public decimal disc { get; set; }
        public decimal discPrice { get; set; }
    }

    public class Charge 
    {
        public string chargeName { get; set; }
        public int rateID { get; set; }
        public decimal price { get; set; }
        public decimal discount { get; set; }
        public decimal costAfterDisc { get; set; }
        public decimal perBinCapacity { get; set; }
        public bool isUnitCost { get; set; }
        public decimal unitCost { get; set; }
    }


    public class CalculatedStorageCharge 
    {
        public string chargeName { get; set; }
        public int rateID { get; set; }
        public decimal price { get; set; }
        public decimal discount { get; set; }
        public decimal costAfterDisc { get; set; }
        public string currencyCode { get; set; }
        public int currencyID { get; set; }
        public int activityID { get; set; }

    }

}