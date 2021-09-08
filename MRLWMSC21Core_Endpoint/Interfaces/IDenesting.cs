using MRLWMSC21_Endpoint.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MRLWMSC21_Endpoint.Interfaces
{
    public interface IDenesting
    {
        string GetDenestingJobList(WMSCoreMessage oRequest);
        string ValidateDenestingLocationCode(WMSCoreMessage oRequest);
        string ValidateDenestingPalletCode(WMSCoreMessage oRequest);
        string UpdateDenestingItem(WMSCoreMessage oRequest);
        string FetchDenstingJobItemsList(WMSCoreMessage oRequest);

    }
}