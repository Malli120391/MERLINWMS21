using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRLWMSC21_Library.WMS_ServiceObjects
{
    public class MaterialMaster
    {
        public string MCode { get; set; }
        public int MaterialId { get; set; }

        public string Description { get; set; }
        public int MaterialGroupID { get; set; }
        public int MaterialTypeID { get; set; }
        public int MaterialSysId { get; set; }    
    
        public decimal Length { get; set; }
        public decimal Width { get; set; }
        public decimal Height { get; set; }
        public string Dim_UOM { get; set; }
        public int Dim_UOMID { get; set; }

        public decimal Weight { get; set; }
        public string Weight_UOM { get; set; }
        public int Weight_UOMID { get; set; }
        public decimal UnitPrice { get; set; }
        public bool IsPalletized { get; set; }
        
        public bool IsFastMovingGood { get; set; }
        public decimal SaleRate { get; set; }

        public IList<UOM> MaterialUOM { get; set; }
        public DateTime ConsumptionFrom { get; set; }
        public DateTime ConsumptionTo { get; set; }

        public IList<MaterialInwardOutward> inwardOutward { get; set; }
        public IList<MaterialConsumption> consumptionRate { get; set; }
        public MaterialStatistics materialStat { get; set; }

    }

    public class UOM 
    {
        public string UOMCode { get; set; }
        public int UOMID { get; set; }
        public string BusinessUomTypeId { get; set; }

    }

    public class MaterialStatistics 
    {   
        public decimal  ConsumptionRate { get; set; }
        public DateTime ConsumptionFrom { get; set; }
        public DateTime ConsumptionTo { get; set; }
        public decimal AvgConsumption { get; set; }
        public int NoOfOrders {get;set;}
    }

    public class MaterialConsumption 
    {
    
        public int SupplierSysInvoiceId {get;set;}
        public decimal InwardQty { get; set; }
        public decimal OutwardQty { get; set; }
        public DateTime TranDate { get; set; }
        public decimal DerivedQty { get; set; }
        public decimal Consumption { get; set; }

    }

    public class MaterialBinInfo
    {
        public string MCode { get; set; }
        public int MaterialID { get; set; }
        public decimal QTY { get; set; }
        public string QTY_UOM { get; set; }
        public int QTY_UOMID { get; set; }
        public decimal Base_QTY { get; set; }

        public int LocationID { get; set; }
        public string LocationCode { get; set; }
        public DateTime TranDate { get; set; }
        public int SuppierLineId { get; set; }
    }

    public class MaterialDimension 
    {
        public decimal MLength { get; set; }
        public decimal MWidth { get; set; }
        public decimal MHeight { get; set; }
        public string UOM { get; set; }
        public int UOMID { get; set; }
        public string WeightUOM { get; set; }
        public int WeightUOMID { get; set; }

        public decimal MVOLUME { get; set; }
        public decimal MWEIGHT { get; set; }

    }


    public class MaterialInwardOutward 
    {
        public decimal InwardQty { get; set; }
        public decimal OutwardQty { get; set; }
        public int SupplierSysInvId { get; set; }
        public DateTime InwardTranDate { get; set; }
        public DateTime OutwardTranDate { get; set; }
        public int SupplierInvId { get; set; }
        public int SupplierId { get; set; }
        public int DeliveryDocId { get; set; }
        
        public int SOSysId { get; set; }
        public int POSysId { get; set; }

        public int MaterialId { get; set; }
        public string MCode { get; set; }

        public int SOdetailId { get; set; }
        public decimal SO_QTY { get; set; }
        public decimal SO_DOC_QTY { get; set; }
        public int SOLineId { get; set; }
        public int TransactionDocId { get; set; }
    }

    public class MSP 
    {
        public int MSPSysId { get; set; }
        public string DisplayName { get; set; }
        public string SearchText { get; set; }
        public string Code { get; set; }
        public object Value { get; set; }
        public string NativeValue { get; set; }
        
    }
}
