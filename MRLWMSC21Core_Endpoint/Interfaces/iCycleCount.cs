using MRLWMSC21_Endpoint.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MRLWMSC21_Endpoint.Interfaces
{
    interface iCycleCount
    {
        string GetCCNames(WMSCoreMessage oRequest);
        string IsBlockedLocation(WMSCoreMessage oRequest);
        string BlockLocationForCycleCount(WMSCoreMessage oRequest);
        string CheckMaterialAvailablilty(WMSCoreMessage oRequest);
        string GetCycleCountInformation(WMSCoreMessage oRequest);
        string ReleaseCycleCountLocation(WMSCoreMessage oRequest);
        string UpsertCycleCount(WMSCoreMessage oRequest);
    }
}