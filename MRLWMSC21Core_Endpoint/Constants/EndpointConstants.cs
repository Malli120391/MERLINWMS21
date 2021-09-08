using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json.Converters;
namespace MRLWMSC21_Endpoint.Constants
{

    public static class EndpointConstants
    {


        [JsonConverter(typeof(StringEnumConverter))]
        public enum DTO { None, LoginUserDTO, ProfileDTO, Inbound, Inventory, Exception, CycleCount, Outbound, DenestingDTO, PutAwayDTO, HouseKeepingDTO, POD, OutboundList, InboundList, POList, SOList, Tenant, Warehouse, Shipment, ScanDTO, InventoryDTO, APIInventoryDTO, MastersDTO, ReleaseOutbound, OutboundOrdersDTO, BulkOrderDTO, SupplierImportDTO, ItemMasterDTO, InboundDataDTO, SLOCToSLOCDTO, BinToBinDTO, SupplierReturnOBDDTO, CustomerReturnDTO, SupplierReturnDTO, UpdateItemMasterDTO, UpdateSupplierDTO, MiscReceipt, MiscIssue, CustomerImportDTO, SupplierCreationDTO, CustomerCreationDTO };
        [JsonConverter(typeof(StringEnumConverter))]
        public enum ScanType { Unloading, Putaway, Picking, Loading, DeNesting, Assortment };
        [JsonConverter(typeof(StringEnumConverter))]
        public enum LocationType { Bin, Dock, DeNesting, Staging, CrossDock, ExcessPicked, Quarantine };
    }
}