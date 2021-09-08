using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRLWMSC21_Library.WMS_ServiceObjects
{
    
    public class Outbound
    {
        public int OutboundId { get; set; }
        public string Description { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public string Tenant { get; set; }
        public int TenantId { get; set; }
        public int SupplierSysId { get; set; }
        public int DeliveryDocId { get; set; }
    }

}
