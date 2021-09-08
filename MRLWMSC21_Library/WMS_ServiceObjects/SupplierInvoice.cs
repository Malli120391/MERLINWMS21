using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRLWMSC21_Library.WMS_ServiceObjects
{
    public class SupplierInvoice
    {
        public string SupplierCode { get; set; }
        public int SupplierSysId { get; set; }
        public decimal NetAmount { get; set; }
        public decimal GrossAmount { get; set; }
        public string VatCode { get; set; }
        public DateTime ValueDate { get; set; }
        public DateTime ProcessDate { get; set; }
        public decimal VatPercent { get; set; }
        public int supplierInvID { get; set; }
        public string supplierInvCode { get; set; }
        public int TenantID { get; set; }
        public int POID { get; set; }
        public string POCode { get; set; }
        public string StoreRefId { get; set; }
        public int InboundId { get; set; }
        public IList<LineItem> InvoiceLineItems { get; set; }

    }

    

    
    
}
