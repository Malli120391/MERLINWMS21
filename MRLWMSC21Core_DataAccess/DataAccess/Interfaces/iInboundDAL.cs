using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MRLWMSC21Core.Entities;

namespace MRLWMSC21Core.DataAccess.Interfaces
{
    interface iInboundDAL
    {
        //Implemented
        List<Inbound> GetInboundList(int AccountID);

        //Implemented
        List<Inbound> GetInboundListSearch(SearchCriteria oCriteria);

        //Implemented
        Inbound GetInboundByID(int InboundID);

        //Implemented
        Inbound GetInboundByIDInventory(int InboundID, bool FetchInventoryMap);

        //Implemented
        Inbound FetchInventoryMap(Inbound oInbound);
        
        Inbound ValidateRSNAndFetchInbound(Inventory criteria, out bool IsRSNItemAlreadyReceived, out bool IsPalletizationPermitted);

        bool UpdateInbound(Inbound oInbound);

        bool ModifyPurchaseDocumentForExcessQuantity(Inventory oInventory);

        List<Inventory> FetchSKUInformationByDockGoodsMovement(Inventory oInventory);
    }
}
