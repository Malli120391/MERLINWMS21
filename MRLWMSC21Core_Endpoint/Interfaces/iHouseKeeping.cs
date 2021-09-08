using MRLWMSC21_Endpoint.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MRLWMSC21_Endpoint.Interfaces
{
    interface iHouseKeeping
    {
       

       

        string CheckLocationForLiveStock(WMSCoreMessage oRequest);

        string GetActivestock(WMSCoreMessage oRequest);

        string GetActivestockWithOutRSN(WMSCoreMessage oRequest);

        string GetTenants(WMSCoreMessage oRequest);

    }
}