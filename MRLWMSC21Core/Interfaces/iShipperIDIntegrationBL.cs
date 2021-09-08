using MRLWMSC21Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MRLWMSC21Core.Business.Interfaces
{
    interface iShipperIDIntegrationBL
    {
        bool CheckAUthToken(string AuthToken);
        List<APIInventory> GetWarehouseInventoryData(APIInventory inventory, string authkey);
        //List<OutboundOrders> GetOutboundOrders(OutboundOrders outboundOrders, string authkey);
        string UpsertBulkOrders(string aPISOCreation, string authkey);
    }
}
