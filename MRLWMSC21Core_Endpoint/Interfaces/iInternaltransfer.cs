using MRLWMSC21_Endpoint.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MRLWMSC21_Endpoint.Interfaces
{
    interface iInternaltransfer
    {
        string UpdateInternalTransfer(WMSCoreMessage oRequest);
        string ValidateTransferLocationCode(WMSCoreMessage oRequest);
        string GetTransferReqNos(WMSCoreMessage oRequest);
        string GetAvailbleQtyList(WMSCoreMessage oRequest);
        string ChekContainerLocation(WMSCoreMessage oRequest);
        string UpsertBinToBinTransfer(WMSCoreMessage oRequest);
        string UpsertBinToBinTransferItem(WMSCoreMessage oRequest);
        string GetBinToBinStorageLocations(WMSCoreMessage oRequest);


    }
}