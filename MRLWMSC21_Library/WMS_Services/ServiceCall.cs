using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRLWMSC21_Library.WMS_Services
{
    public enum ServiceCall
    {
        INB_CREATE,
        INB_UPDATE,
        INB_DELETE,
        INB_SELECT,
        INB_LIST,
        INB_INVOICE_GRN,
        INB_INVOICE_LINE_GRN,
        INB_RECEIVE,
        INB_PUTAWAY,
        INB_RTR,
        INB_SUGGESTED_PUTAWAY,
        INV_STOCK,
        INV_LIVE_STOCK,
        INV_ONHOLD_STOCK,
        INV_STOCK_BY_MM,
        INV_STOCK_BY_TENANT,
        INV_STOCK_TRANSFER,
        INV_STOCK_ADJUST,
        INV_STOCK_BY_MSP,
        OBD_CREATE,
        OBD_SELECT,
        OBD_UPDATE,
        OBD_DELETE,
        OBD_LIST,
        OBD_DPN,
        OBD_SUGGESTED_PICKNOTE,
        OBD_RESTRICTED_PICKNOTE,
        OBD_PGI,
        INV_STOCK_RESERVATION
    }
}
