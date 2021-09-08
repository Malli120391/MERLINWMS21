using MRLWMSC21_Endpoint.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MRLWMSC21_Endpoint.Interfaces
{
    public interface iScan
    {
            string ValidatePallet(WMSCoreMessage oRequest);
            string ValidateLocation(WMSCoreMessage oRequest);
            string ValiDateMaterial(WMSCoreMessage oRequest);

    }
}