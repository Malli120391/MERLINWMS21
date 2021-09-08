using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MRLWMSC21Core.Entities;

namespace MRLWMSC21Core.Business.Interfaces
{
    interface ICycleCountBL
    {

        List<CycleCount> FetchActiveCycleCountList(SearchCriteria oCriteria);

        Location FetchNextLocationForCycleCount(CycleCount oCycleCount);

        Location BlockLocationForCycleCount(Location oLocation);
        
        bool ValidateLocationBoxQuantity(Location oLocation, Inventory oInventory);

        Inventory CaptureCycleCount(CycleCount oCycleCount, Inventory oInventory, int UserID);

        bool MarkBinComplete(Location oLocation);

        bool FlagLocationForCycleCount(Location oLocation);
        
    }
}
