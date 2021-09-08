using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WMSCore.Interfaces
{
    public interface iScanningBL
    {

         WMSCore_BusinessEntities.Entities.ScannedItem ValidateLocation(WMSCore_BusinessEntities.Entities.ScannedItem obj);
        WMSCore_BusinessEntities.Entities.ScannedItem ValidatePallet(WMSCore_BusinessEntities.Entities.ScannedItem obj);
        WMSCore_BusinessEntities.Entities.ScannedItem ValidateSKU(WMSCore_BusinessEntities.Entities.ScannedItem obj);
    }
}
