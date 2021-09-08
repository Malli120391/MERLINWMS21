using MRLWMSC21_Endpoint.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MRLWMSC21_Endpoint.Interfaces
{
    interface iIntegration
    {
        object CreateOutbound(WMSCoreMessage oRequest);
        object FetchWarehouseInventory(WMSCoreMessage oRequest);
        object FetchOutboundOrders(WMSCoreMessage oRequest);
    }
}