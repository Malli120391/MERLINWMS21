using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MRLWMSC21.mReports
{
    public class GetDataListModel
    {       
        public class Zones
        {
            public int LocationZoneID { get; set; }
            public string LocationZoneCode { get; set; }
        }
        public class DocumentType
        { 
            public int DocumentTypeID { get; set; }
            public string Documenttype { get; set; }
        
        }

        public class DeliveryStatus
        {
            public int DeliveryStatusID { get; set; }
            public string Deliverystatus { get; set; }

        }
        public class Warehouse
        {
            public int WarehouseID { get; set; }
            public string WHCode { get; set; }
        }

        public class Printers
        {
            public string PrinterId {get;set;}
            public string PrinterName { get; set; }
        }

        public class LabelSize
        {
            public int BarCodeId { get; set; }
            public string BarCode { get; set; }
        }
        public class Tenant
        {
            public int TenantId { get; set; }
            public string TenantName { get; set; }
        }

        public class Mcodes
        {
            public int McodeId { get; set; }
            public string Mcode { get; set; }
        }

        public class MType
        {
            public int MtypeId { get; set; }
            public string Mtype { get; set; }
        }

        public class Locations
        {
            public int LocId { get; set; }
            public string LocationName { get; set; }
        }

        public class KitPlanner
        {
            public int KitId { get; set; }
            public string KitCode{ get; set; }
        }
    }
}