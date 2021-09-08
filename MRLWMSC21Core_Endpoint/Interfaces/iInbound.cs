using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using MRLWMSC21Core.Entities;
using MRLWMSC21_Endpoint.Security;


namespace MRLWMSC21_Endpoint.Interfaces
{
    interface iInbound
    {
        
        string GetStoreRefNos(WMSCoreMessage oRequest);
        string GetStorageLocations(WMSCoreMessage oRequest);
        string CheckContainer(WMSCoreMessage oRequest);
        string GetReceivedQty(WMSCoreMessage oRequest);
        string UpdateReceiveItemForHHT(WMSCoreMessage oRequest);
        string GetSkipReasonList(WMSCoreMessage oRequest);
        string GetConatinerLocation(WMSCoreMessage oRequest);
        string GetItemTOPutAway(WMSCoreMessage oRequest);
        string CheckPutAwayItemQty(WMSCoreMessage oRequest);
        string SkipItem(WMSCoreMessage oRequest);
        string UpsertPutAwayItem(WMSCoreMessage oRequest);

    }
}