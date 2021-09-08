using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MRLWMSC21Core.Entities;

namespace MRLWMSC21Core.Business.Interfaces
{
    interface iLocationBL
    {
        
        List<Location> GetLoationListBind(SearchCriteria Criteria);

        
        Location GetLocationByID(int LocationID);

        
        bool BlockLocationForCycleCount(Location oLocation);

       
        bool ReleaseLocationForCycleCount(Location oLocation);

        
        bool BlockLocationForCycleCount(int LocationID);

       
        bool ReleaseLocationForCycleCount(int LocationID);
    }
}
