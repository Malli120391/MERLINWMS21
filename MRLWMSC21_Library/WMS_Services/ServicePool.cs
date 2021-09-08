using MRLWMSC21_Library.WMS_DBCommon;
using MRLWMSC21_Library.WMS_ServiceControl;
using MRLWMSC21_Library.WMS_ServiceImpl;
using MRLWMSC21_Library.WMS_ServiceObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRLWMSC21_Library.WMS_Services
{
    internal class ServicePool
    {

        private static WMSService inboundServiceImpl = new InboundServiceImpl();
        private static WMSService gRNServiceImpl = new GRNServiceImpl();
        private static WMSService putawayServiceImpl = new PutawayServiceImpl();
        private static WMSService suggestedPutawayImpl = new SuggestedPutawayImpl();
        private static WMSService outboundServiceImpl = new OutboundServiceImpl();
        private static WMSService suggestedPicknoteImpl = new SuggestedPicknoteImpl();
        private static WMSService InventoryServiceImpl = new InventoryServiceImpl();
        private static WMSService invoiceServiceImpl = new InvoiceServiceImpl();
        private static WMSService materialServiceImpl = new MaterialServiceImpl();
        
        public static IDictionary<string, WMSService> AvailServiceNames = new Dictionary<string, WMSService>() 
        { 
        
            {"MATERIAL_SALERATE_PERMONTH",materialServiceImpl},
            {"INB_CREATE",inboundServiceImpl},
            {"INB_UPDATE",inboundServiceImpl},
            {"INB_DELETE",inboundServiceImpl},
            {"INB_SELECT",inboundServiceImpl},
            {"INB_LIST",inboundServiceImpl},

            {"INB_INVOICE_CREATE",invoiceServiceImpl},
            {"INB_INVOICE_UPDATE",invoiceServiceImpl},
            {"INB_INVOICE_LIST",invoiceServiceImpl},

            
            {"INB_GRN",gRNServiceImpl},
            {"INB_RECEIVE",putawayServiceImpl},
            {"INB_PUTAWAY",putawayServiceImpl},
            {"INB_RTR",inboundServiceImpl},
            {"INB_SUGGESTED_PUTAWAY",suggestedPutawayImpl},

            {"OBD_CREATE",outboundServiceImpl},
            {"OBD_SELECT",outboundServiceImpl},
            {"OBD_UPDATE",outboundServiceImpl},
            {"OBD_DELETE",outboundServiceImpl},
            {"OBD_LIST",outboundServiceImpl},
            {"OBD_DPN",outboundServiceImpl},
            {"OBD_PGI",outboundServiceImpl},
            {"OBD_SUGGESTED_PICKNOTE",suggestedPicknoteImpl},
            {"OBD_RESTRICTED_PICKNOTE",suggestedPicknoteImpl},
            
            {"INV_STOCK",InventoryServiceImpl},
            {"INV_LIVE_STOCK",InventoryServiceImpl},
            {"INV_ONHOLD_STOCK",InventoryServiceImpl},
            {"INV_STOCK_BY_MM",InventoryServiceImpl},
            {"INV_STOCK_BY_TENANT",InventoryServiceImpl},
            {"INV_STOCK_TRANSFER",InventoryServiceImpl},
            {"INV_STOCK_ADJUST",InventoryServiceImpl},
            {"INV_STOCK_BY_MSP",InventoryServiceImpl}

        };


        public static IDictionary<string, string> ServiceImplRef = new Dictionary<string, string>() 
        { 
        
            {"MATERIAL_SALERATE_PERMONTH","GetMaterialSalesOrderRate"},
            {"INB_CREATE",""},
            {"INB_UPDATE",""},
            {"INB_DELETE",""},
            {"INB_SELECT",""},
            {"INB_LIST",""},
            {"INB_GRN",""},
            {"INB_RECEIVE",""},
            {"INB_PUTAWAY",""},
            {"INB_RTR",""},
            {"INB_SUGGESTED_PUTAWAY","GetSuggestedPutawayInfo"},
            {"INB_SUGGESTED_PUTAWAY_RECORD","HoldSuggestedPutawayInfo"},
            {"INB_INVOICE_CREATE",""},
            {"INB_INVOICE_UPDATE",""},
            {"INB_INVOICE_LIST",""},
            {"OBD_CREATE",""},
            {"OBD_SELECT",""},
            {"OBD_UPDATE",""},
            {"OBD_DELETE",""},
            {"OBD_LIST",""},
            {"OBD_DPN",""},
            {"OBD_SUGGESTED_PICKNOTE","GetSuggestedPicknote"},
            {"OBD_RESTRICTED_PICKNOTE","GetRestrictedPicknote"},
            
            {"OBD_PGI",""},
            {"INV_STOCK",""},
            {"INV_LIVE_STOCK",""},
            {"INV_ONHOLD_STOCK",""},
            {"INV_STOCK_BY_MM",""},
            {"INV_STOCK_BY_TENANT",""},
            {"INV_STOCK_TRANSFER",""},
            {"INV_STOCK_ADJUST",""},
            {"INV_STOCK_BY_MSP",""}

        };

        private static ServiceController iNBServiceControl = new INBServiceController();
        private static ServiceController oBDServiceControl = new OBDServiceController();
        private static ServiceController iNVServiceControl = new INVServiceController();
        
        public static IDictionary<string, ServiceController> ServiceGroups = new Dictionary<string, ServiceController>() 
        { 
                      
            {"INB_CREATE",iNBServiceControl},
            {"INB_UPDATE",iNBServiceControl},
            {"INB_DELETE",iNBServiceControl},
            {"INB_SELECT",iNBServiceControl},
            {"INB_LIST",iNBServiceControl},
            {"INB_GRN",iNBServiceControl},
            {"INB_RECEIVE",iNBServiceControl},
            {"INB_PUTAWAY",iNBServiceControl},
            {"INB_RTR",iNBServiceControl},
            {"INB_SUGGESTED_PUTAWAY",iNBServiceControl},
            {"INB_INVOICE",iNBServiceControl},

            {"OBD_CREATE",oBDServiceControl},
            {"OBD_SELECT",oBDServiceControl},
            {"OBD_UPDATE",oBDServiceControl},
            {"OBD_DELETE",oBDServiceControl},
            {"OBD_LIST",oBDServiceControl},
            {"OBD_DPN",oBDServiceControl},
            {"OBD_SUGGESTED_PICKNOTE",oBDServiceControl},
            {"OBD_PGI",oBDServiceControl},
            {"OBD_RESTRICTED_PICKNOTE",oBDServiceControl},
            {"INV_STOCK",iNVServiceControl},
            {"INV_LIVE_STOCK",iNVServiceControl},
            {"INV_ONHOLD_STOCK",iNVServiceControl},
            {"INV_STOCK_BY_MM",iNVServiceControl},
            {"INV_STOCK_BY_TENANT",iNVServiceControl},
            {"INV_STOCK_TRANSFER",iNVServiceControl},
            {"INV_STOCK_ADJUST",iNVServiceControl},
            {"INV_STOCK_BY_MSP",iNVServiceControl}
            

        };


        //WMS configuration information
        public static WMSConfig wms_config = InitConfigurationInformation();

        public static WMSConfig InitConfigurationInformation() 
        {
            Object DBResult = DAOController.GetData(DRLExecuteCode.WMS_CONFIG, null);
            if(DBResult != null)
            {
                wms_config = (WMSConfig)DBResult;
                return wms_config;
            }
            return null;
        }

    }
}
