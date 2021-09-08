using MRLWMSC21_Library.WMS_DBCommon.MasterDataDAO;
using MRLWMSC21_Library.WMS_DBCommon.MasterDataDTO;
using MRLWMSC21_Library.WMS_DBCommon.TransactionalDAO;
using MRLWMSC21_Library.WMS_DBCommon.TransactionalDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRLWMSC21_Library.WMS_DBCommon
{
    public class DBServicePool
    {
        private static DBServiceController materialDTO = new MaterialDTO();
        private static DBServiceController materialDAO = new MaterialDAO();

        private static DBServiceController masterConfigDAO = new MasterConfigDAO();

        private static DBServiceController customerDTO = new CustomerDTO();
        private static DBServiceController customerDAO = new CustomerDAO();

        private static DBServiceController tenantDTO = new TenantDTO();
        private static DBServiceController tenantDAO = new TenantDAO();

        public static DBServiceController supplierDTO = new SupplierDTO();
        public static DBServiceController supplierDAO = new SupplierDAO();

        public static DBServiceController goodsMovementDAO = new GoodsMovementDAO();
        public static DBServiceController goodsMovementDTO = new GoodsMovementDTO();

        public static DBServiceController inboundDAO = new InboundDAO();
        public static DBServiceController inboundDTO = new InboundDTO();

        public static DBServiceController invoiceDAO = new InvoiceDAO();
        public static DBServiceController invoiceDTO = new InvoiceDTO();

        public static DBServiceController outboundDAO = new OutboundDAO();
        public static DBServiceController outboundDTO = new OutboundDTO();

        public static DBServiceController purchaseOrderDAO = new PurchaseOrderDAO();
        public static DBServiceController purchaseOrderDTO = new PurchaseOrderDTO();

        public static DBServiceController salesOrderDTO = new SalesOrderDTO();
        public static DBServiceController salesOrderDAO = new SalesOrderDAO();

        public static DBServiceController locationDAO = new LocationDAO();
        public static DBServiceController locationDTO = new LocationDTO();


        public static IDictionary<string, DBServiceController> DBServicePoolList = new Dictionary<string, DBServiceController>() 
        { 
                    {"MATERIAL_CREATE",materialDTO},
                    {"MATERIAL_UPDATE",materialDTO},
                    {"MATERIAL_DELETE",materialDTO},
                    {"MATERIAL_BLOCK",materialDTO},
                    {"MATERIAL_ACTIVATE",materialDTO},

                    {"CUSTOMER_CREATE",customerDTO},
                    {"CUSTOMER_UPDATE",customerDTO},
                    {"CUSTOMER_DELETE",customerDTO},
                    {"CUSTOMER_BLOCK",customerDTO},
                    {"CUSTOMER_ACTIVATE",customerDTO},

                    {"TENANT_CREATE",tenantDTO},
                    {"TENANT_UPDATE",tenantDTO},
                    {"TENANT_DELETE",tenantDTO},
                    {"TENANT_BLOCK",tenantDTO},
                    {"TENANT_ACTIVATE",tenantDTO},
                    
                    {"SUPPLIER_CREATE",supplierDTO},
                    {"SUPPLIER_UPDATE",supplierDTO},
                    {"SUPPLIER_DELETE",supplierDTO},
                    {"SUPPLIER_BLOCK",supplierDTO},
                    {"SUPPLIER_ACTIVATE",supplierDTO},

                    {"LOCATION_CREATE",locationDTO},
                    {"LOCATION_UPDATE",locationDTO},
                    {"LOCATION_DELETE",locationDTO},
                    {"LOCATION_BLOCK",locationDTO},
                    {"LOCATION_ACTIVATE",locationDTO},
                    {"HOLD_SUGGESTED_LOCATION",locationDTO},

                    {"INVOICE_CREATE",invoiceDTO},
                    {"INVOICE_UPDATE",invoiceDTO},
                    {"INVOICE_BLOCK",invoiceDTO},
                    {"INVOICE_DELETE",invoiceDTO},
                    {"INVOICE_ACTIVATE",invoiceDTO},

                    {"PO_CREATE",purchaseOrderDTO},
                    {"PO_UPDATE",purchaseOrderDTO},
                    {"PO_DELETE",purchaseOrderDTO},
                    {"PO_BLOCK",purchaseOrderDTO},
                    {"PO_ACTIVATE",purchaseOrderDTO},

                    {"SO_CREATE",salesOrderDTO},
                    {"SO_UPDATE",salesOrderDTO},
                    {"SO_DELETE",salesOrderDTO},
                    {"SO_BLOCK",salesOrderDTO},
                    {"SO_ACTIVATE",salesOrderDTO},

                    {"GOODS_IN_CREATE",goodsMovementDTO},
                    {"GOODS_IN_DELETE",goodsMovementDTO},
                    {"GOODS_IN_ACTIVATE",goodsMovementDTO},
                    {"GOODS_OUT_CREATE",goodsMovementDTO},
                    {"GOODS_OUT_DELETE",goodsMovementDTO},
                    {"GOODS_OUT_ACTIVATE",goodsMovementDTO},
                    {"GOODS_TRANSFER_CREATE",goodsMovementDTO},
                    {"GOODS_TRANSFER_DELETE",goodsMovementDTO},
                    {"GOODS_TRANSFER_ACTIVATE",goodsMovementDTO},

                    {"INBOUND_CREATE",inboundDTO},
                    {"INBOUND_UPDATE",inboundDTO},
                    {"INBOUND_DELETE",inboundDTO},
                    {"INBOUND_BLOCK",inboundDTO},
                    {"INBOUND_ACTIVATE",inboundDTO},


                    {"OUTBOUND_CREATE",outboundDTO},
                    {"OUTBOUND_UPDATE",outboundDTO},
                    {"OUTBOUND_DELETE",outboundDTO},
                    {"OUTBOUND_ACTIVATE",outboundDTO},
                    {"OUTBOUND_BLOCK",outboundDTO},

                    {"GRN_CREATE",inboundDTO},
                    {"GRN_UPDATE",inboundDTO},
                    {"GRN_LINE_UPDATE",inboundDTO},
                    {"GRN_SUPPLIER_INVOICE",inboundDTO},
                    {"GRN_DELETE",inboundDTO},

                    {"WMS_CONFIG",masterConfigDAO},
                    {"MATERIAL_INWARD_OUTWARD_BY_ID",materialDAO},
                    {"MATERIAL_LIST",materialDAO},
                    {"MATERIAL_BY_ID",materialDAO},
                    {"MATERIAL_BY_MCODE",materialDAO},
                    {"MATERIAL_LIST_BY_SUPPLIER",materialDAO},
                    {"MATERIAL_INFO_BY_PO",materialDAO},
                    {"MATERIAL_INFO_BY_INVOICE",materialDAO},
                    {"MATERIAL_INFO_BY_SO",materialDAO},
                    {"MATERIAL_TENANT",materialDAO},
                    {"MATERIAL_BY_TYPE",materialDAO},
                    {"MATERIAL_BY_GROUP",materialDAO},
                    {"MATERIAL_BIN_INFO_BY_MATERIALID",materialDAO},

                    {"INVOICE_LIST_TENANT",invoiceDAO},
                    {"INVOICE_LIST_BY_SUPPLIER",invoiceDAO},
                    {"INVOICE_BY_ID",invoiceDAO},
                    {"INVOICE_LIST_BY_PO",invoiceDAO},
                    {"INVOICE_LIST_BY_INBOUND",invoiceDAO},


                    {"PO_LIST",purchaseOrderDAO},
                    {"PO_LIST_BY_SUPPLIER",purchaseOrderDAO},
                    {"PO_LIST_BY_STATUS",purchaseOrderDAO},

                    {"CUSTOMER_LIST",customerDAO},
                    {"CUSTOMER_LIST_BY_TENANT",customerDAO},
                    {"CUSTOMER_BY_ID",customerDAO},

                    {"SUPPLIER_LIST",supplierDAO},
                    {"SUPPLIER_LIST_BY_TENANT",supplierDAO},
                    {"SUPPLIER_BY_ID",supplierDAO},
                    {"SUPPLIER_LIST_BY_MATERIAL",supplierDAO},

                    {"LOCATION_LIST_BY_WAREHOUSE",locationDAO},
                    {"LOCATION_LIST_BY_ZONE",locationDAO},
                    {"LOCATION_LIST_BY_RACK",locationDAO},
                    {"LOCATION_LIST_BY_TYPE",locationDAO},
                    {"LOCATION_LIST_BY_TENANT",locationDAO},
                    {"LOCATION_LIST_BY_SUPPLIER",locationDAO},
                    {"LOCATION_OCCUPANCY_BY_ID",locationDAO},
                    {"LOCATION_LIST_BY_ZONECODE",locationDAO},
                    {"SUGGESTED_LOCATION_OCCUPANCY",locationDAO},
                    {"ZONE_LIST_BY_WAREHOUSE",locationDAO},
                    {"DELIVERY_PICK_NOTE",outboundDAO}

        };

        public static IDictionary<string, string> DBServiceImplementRef = new Dictionary<string, string>() 
        {
                    {"WMS_CONFIG","InitConfigurationInformation"},
                    {"MATERIAL_CREATE",""},
                    {"MATERIAL_UPDATE",""},
                    {"MATERIAL_DELETE",""},
                    {"MATERIAL_BLOCK",""},
                    {"MATERIAL_ACTIVATE",""},
                    
                    {"CUSTOMER_CREATE",""},
                    {"CUSTOMER_UPDATE",""},
                    {"CUSTOMER_DELETE",""},
                    {"CUSTOMER_BLOCK",""},
                    {"CUSTOMER_ACTIVATE",""},
                    
                    {"TENANT_CREATE",""},
                    {"TENANT_UPDATE",""},
                    {"TENANT_DELETE",""},
                    {"TENANT_BLOCK",""},
                    {"TENANT_ACTIVATE",""},
                    
                    {"SUPPLIER_CREATE",""},
                    {"SUPPLIER_UPDATE",""},
                    {"SUPPLIER_DELETE",""},
                    {"SUPPLIER_BLOCK",""},
                    {"SUPPLIER_ACTIVATE",""},
                    
                    {"LOCATION_CREATE",""},
                    {"LOCATION_UPDATE",""},
                    {"LOCATION_DELETE",""},
                    {"LOCATION_BLOCK",""},
                    {"LOCATION_ACTIVATE",""},
                    {"HOLD_SUGGESTED_LOCATION","HoldSuggestedLocation"},
                    
                    {"INVOICE_CREATE",""},
                    {"INVOICE_UPDATE",""},
                    {"INVOICE_BLOCK",""},
                    {"INVOICE_DELETE",""},
                    {"INVOICE_ACTIVATE",""},
                    
                    {"PO_CREATE",""},
                    {"PO_UPDATE",""},
                    {"PO_DELETE",""},
                    {"PO_BLOCK",""},
                    {"PO_ACTIVATE",""},
                    
                    {"SO_CREATE",""},
                    {"SO_UPDATE",""},
                    {"SO_DELETE",""},
                    {"SO_BLOCK",""},
                    {"SO_ACTIVATE",""},
                    
                    {"GOODS_IN_CREATE",""},
                    {"GOODS_IN_DELETE",""},
                    {"GOODS_IN_ACTIVATE",""},
                    {"GOODS_OUT_CREATE",""},
                    {"GOODS_OUT_DELETE",""},
                    {"GOODS_OUT_ACTIVATE",""},
                    {"GOODS_TRANSFER_CREATE",""},
                    {"GOODS_TRANSFER_DELETE",""},
                    {"GOODS_TRANSFER_ACTIVATE",""},
                    
                    {"INBOUND_CREATE",""},
                    {"INBOUND_UPDATE",""},
                    {"INBOUND_DELETE",""},
                    {"INBOUND_BLOCK",""},
                    
                    {"OUTBOUND_CREATE",""},
                    {"OUTBOUND_UPDATE",""},
                    {"OUTBOUND_DELETE",""},
                    {"OUTBOUND_ACTIVATE",""},
                    
                    {"GRN_CREATE",""},
                    {"GRN_UPDATE",""},
                    {"GRN_LINE_UPDATE",""},
                    {"GRN_SUPPLIER_INVOICE",""},
                    {"GRN_DELETE",""},
                    
                    {"MATERIAL_LIST",""},
                    {"MATERIAL_INWARD_OUTWARD_BY_ID","GetMaterialConsumptionById"},
                    {"MATERIAL_OUTWARD_BY_ID","GetMaterialOutwardInfoById"},
                    {"MATERIAL_BY_ID","GetMaterialInfoById"},
                    {"MATERIAL_BIN_INFO_BY_MATERIALID","GetMaterialBinInfo"},
                    {"MATERIAL_BY_MCODE",""},
                    {"MATERIAL_LIST_BY_SUPPLIER",""},
                    {"MATERIAL_INFO_BY_PO",""},
                    {"MATERIAL_INFO_BY_INVOICE",""},
                    {"MATERIAL_INFO_BY_SO",""},
                    {"MATERIAL_TENANT",""},
                    {"MATERIAL_BY_TYPE",""},
                    {"MATERIAL_BY_GROUP",""},
                    
                    
                    //PROVIDED IMPLEMENTATIONS 
                    {"INVOICE_LIST_BY_TENANT","GetInvoiceListByTenant"},
                    {"INVOICE_LIST_BY_SUPPLIER","GetInvoiceListBySupplier"},
                    {"INVOICE_BY_ID","GetInvoiceById"},
                    {"INVOICE_LIST_BY_PO","GetInvoiceListByPO"},
                    {"INVOICE_LIST_BY_INBOUND","GetInvoiceListByInboud"},
                    
                    
                    {"PO_LIST",""},
                    {"PO_LIST_BY_SUPPLIER",""},
                    {"PO_LIST_BY_STATUS",""},
                    
                    {"CUSTOMER_LIST","GetCustomerList"},
                    {"CUSTOMER_LIST_BY_TENANT","GetCustomerListByTenant"},
                    {"CUSTOMER_BY_ID","GetCustomerDataById"},

                    {"SUPPLIER_LIST",""},
                    {"SUPPLIER_LIST_BY_TENANT",""},
                    {"SUPPLIER_BY_ID",""},
                    {"SUPPLIER_LIST_BY_MATERIAL",""},

                    {"LOCATION_LIST_BY_WAREHOUSE",""},
                    {"LOCATION_LIST_BY_ZONE",""},
                    {"LOCATION_LIST_BY_RACK",""},
                    {"LOCATION_LIST_BY_TYPE",""},
                    {"LOCATION_LIST_BY_TENANT",""},
                    {"LOCATION_LIST_BY_SUPPLIER",""},
                    
                    {"SUGGESTED_LOCATION_OCCUPANCY","GetSuggetedLocationOccupancy"},
                    {"LOCATION_OCCUPANCY_BY_ID","GetLocationOccupancyById"},
                    {"LOCATION_LIST_BY_ZONECODE","GetZoneWiseBinsByLocationCode"},
                    {"ZONE_LIST_BY_WAREHOUSE","GetZoneInfoByWarehouseCode"},
                    {"DELIVERY_PICK_NOTE","GetDeliveryPickInfoByOutboundId"}
        };

    }

}
