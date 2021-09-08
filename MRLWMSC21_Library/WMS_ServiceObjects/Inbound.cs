using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRLWMSC21_Library.WMS_ServiceObjects
{
    public class Inbound
    {
        public string InboundId { get; set; }
        public int InboundSysId { get; set; }
        public string InboundStoreRef { get; set; }
        public string Description { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public string Tenant { get; set; }
        public int TenantId { get; set; }
        public int SupplierSysId { get; set; }
        public IDictionary<int, SupplierInvoice> invoices { get; set; }
    }
}
