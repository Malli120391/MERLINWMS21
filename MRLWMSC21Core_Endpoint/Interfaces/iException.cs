using MRLWMSC21_Endpoint.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MRLWMSC21_Endpoint.Interfaces
{
   interface iException
    {
        string LogException(WMSCoreMessage oRequest);
    }
}