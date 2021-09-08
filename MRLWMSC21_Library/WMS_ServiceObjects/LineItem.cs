using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRLWMSC21_Library.WMS_ServiceObjects
{
    public class LineItem
    {

        public IList<SuggestedLocation> SuggestedLocations { get; set; }
        public MaterialDimension dimensions { get; set; }
        public IList<MaterialStockInfo> AvailStock { get; set; }
        public IDictionary<int,MSP> MSPS { get; set; }
        
        public string LineNo { get; set; }
        public decimal QTY { get; set; }
        public int InvoiceSysLineId { get; set; }

        public decimal QTY_Base_UOM { get; set; }

        public decimal INV_QTY { get; set; }
        public string INV_UOM { get; set; }
        public int INV_UOMID { get; set; }
        public decimal INV_UOM_QTY { get; set; }

        public string MaterialCode { get; set; }
        public int MaterialID { get; set; }
        public int MTypeId { get; set; }
        public string MDesc { get; set; }

        public string Base_UOM { get; set; }
        public int Base_UOMID { get; set; }
        public decimal Base_UOM_QTY { get; set; }

        public decimal PO_QTY { get; set; }
        public string PO_UOM { get; set; }
        public decimal PO_UOM_QTY { get; set; }
        public int PO_UOMID { get; set; }

        public decimal SO_QTY { get; set; }
        public string SO_UOM { get; set; }
        public decimal SO_UOM_QTY { get; set; }
        public int SO_UOMID { get; set; }

        public bool PGI_STATUS { get; set; }
        public bool GRN_STATUS { get; set; }

        public int PODetailsID { get; set; }
        public int SODetailsID { get; set; }

        public int KitPlannerId { get; set; }
        public int LineSeqId { get; set; }
        public decimal processedDocQty { get; set; }
        public decimal processedQty { get; set; }
                
        public decimal ConversionFactor { get; set; }
        public bool IsPalletized { get; set; }
        public bool IsMSPRequest { get; set; }
        public int WarehouseId { get; set; }


    }

    public class SuggestedLocation
    {
    
        public int LocationID { get; set; }
        public string LocationCode { get; set; }
        public decimal QTY { get; set; }
        public IList<MSP> MSPS { get; set; }

        public decimal InitiatedQTY { get; set; }
        public decimal SuggestedQTY { get; set; }
        public bool IsMatchedMSP { get; set; }
        public int LocationSysGenId { get; set; }

        public bool IsDamaged { get; set; }
        public bool HasDiscrepancy { get; set; }
        public int KitPlannerId { get; set; }

        public int InvoiceLineId { get; set; }
        public decimal SuggestedVolume { get; set; }
        public decimal SuggestedWeight { get; set; }
        
    }
    
    public class MaterialStockInfo 
    {
        public int MaterialId { get; set; }
        public string MaterialCode { get; set; }
        public string LocationCode { get; set; }
        public int LocationSysId { get; set; }
        public int LocationGenId { get; set; }
        public int SysTranId { get; set; }
        public DateTime SysTranDate { get; set; }
        public decimal QTY { get; set; }
        public IList<MSP> MSPS { get; set; }
        public int InvoiceSysLineId { get; set; }
        public string BatchNo { get; set; }
        
        public bool IsDamaged { get; set; }
        public bool HasDiscrepancy { get; set; }
        public int KitPlannerId { get; set; }

        public decimal InitiatedQTY { get; set; }
        public int UOMId { get; set; }

        //FEFO purpose
        public DateTime dummyExpiry { get; set; }
    }

    public class SuggestedStockInfo
    {
        public string SONumber { get; set; }
        public int SOId { get; set; }
        public int SODetailsId { get; set; }
        public int SOLineSysId { get; set; }

        public int LineNo { get; set; }

        public int MaterialId { get; set; }
        public string MaterialCode { get; set; }
        public string MaterialDesc { get; set; }

        public string SOUOM { get; set; }
        public decimal SOUOMQTY { get; set; }
        public decimal SOQTY { get; set; }
        public int SOUOMId { get; set; }

        public DateTime CreatedOn { get; set; }
        public int LocId { get; set; }
        public string LocCode { get; set; }

        public decimal InitiatedQTY { get; set; }
        public decimal SuggestedQTY { get; set; }
        public decimal QTY { get; set; }

        public int OutboundId { get; set; }
        public int LocationSysGenId { get; set; }
        public IList<MSP> MSPS { get; set; }
        public bool IsMatchedMSP { get; set; }
        public bool IsDamaged { get; set; }
        public bool HasDiscrepancy { get; set; }
        public int KitPlannerId { get; set; }


    }

}
