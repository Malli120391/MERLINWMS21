using MRLWMSC21_Endpoint.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MRLWMSC21_Endpoint.Interfaces
{
    interface iOutbound
    {
        string GetobdRefNos(WMSCoreMessage oRequest);
        string OBDSkipItem(WMSCoreMessage oRequest);
        string GetOBDItemToPick(WMSCoreMessage oRequest);
        string CheckStrickyCompliance(WMSCoreMessage oRequest);

        string UpdatePickItem(WMSCoreMessage oRequest);

        string GetOpenVLPDNos(WMSCoreMessage oRequest);

        string GetItemToPick(WMSCoreMessage oRequest);

        string VLPDSkipItem(WMSCoreMessage oRequest);

        string UpsertPickItem(WMSCoreMessage oRequest);

        string GetVLPDPickedList(WMSCoreMessage oRequest);

        string GetOBDPickedList(WMSCoreMessage oRequest);

        string DeleteVLPDPickedItems(WMSCoreMessage oRequest);
    }
}