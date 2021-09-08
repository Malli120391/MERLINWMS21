using System.Collections.Generic;
using MRLWMSC21Core.Entities;

namespace MRLWMSC21Core.DataAccess.Interfaces
{
    interface ILocationDAL
    {
        //Implemented
        List<Location> GetLocations(SearchCriteria Criteria);

        //Implemented
        Location GetLocationByID(int LocationID);

        //Implemented
        bool BlockLocationForCycleCount(Location oLocation);

        //Implemented
        bool ReleaseLocationForCycleCount(Location oLocation);

        //Implemented
        bool BlockLocationForCycleCount(int LocationID);

        //Implemented
        bool ReleaseLocationForCycleCount(int LocationID);
    }
}
