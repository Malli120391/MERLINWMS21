using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRLWMSC21_Library.WMS_DBCommon
{
    public enum DMLExecuteCode
    {
        
        
        MATERIAL_CREATE,
        MATERIAL_UPDATE,
        MATERIAL_DELETE,
        MATERIAL_BLOCK,
        MATERIAL_ACTIVATE,

        CUSTOMER_CREATE,
        CUSTOMER_UPDATE,
        CUSTOMER_DELETE,
        CUSTOMER_BLOCK,
        CUSTOMER_ACTIVATE,

        TENANT_CREATE,
        TENANT_UPDATE,
        TENANT_DELETE,
        TENANT_BLOCK,
        TENANT_ACTIVATE,

        SUPPLIER_CREATE,
        SUPPLIER_UPDATE,
        SUPPLIER_DELETE,
        SUPPLIER_BLOCK,
        SUPPLIER_ACTIVATE,

        LOCATION_CREATE,
        LOCATION_UPDATE,
        LOCATION_DELETE,
        LOCATION_BLOCK,
        LOCATION_ACTIVATE,
        HOLD_SUGGESTED_LOCATION,
        
                
        INVOICE_CREATE,
        INVOICE_UPDATE,
        INVOICE_BLOCK,
        INVOICE_DELETE,
        INVOICE_ACTIVATE,


        PO_CREATE,
        PO_UPDATE,
        PO_DELETE,
        PO_BLOCK,
        PO_ACTIVATE,

        SO_CREATE,
        SO_UPDATE,
        SO_DELETE,
        SO_BLOCK,
        SO_ACTIVATE,

        GOODS_IN_CREATE,
        GOODS_IN_DELETE,
        GOODS_IN_ACTIVATE,
        GOODS_OUT_CREATE,
        GOODS_OUT_DELETE,
        GOODS_OUT_ACTIVATE,

        GOODS_TRANSFER_CREATE,
        GOODS_TRANSFER_DELETE,
        GOODS_TRANSFER_ACTIVATE,
        
        INBOUND_CREATE,
        INBOUND_UPDATE,
        INBOUND_DELETE,
        INBOUND_BLOCK,
            

        OUTBOUND_CREATE,
        OUTBOUND_UPDATE,
        OUTBOUND_DELETE,
        OUTBOUND_ACTIVATE,

        GRN_CREATE,
        GRN_UPDATE,
        GRN_LINE_UPDATE,
        GRN_SUPPLIER_INVOICE,
        GRN_DELETE

    }
}
