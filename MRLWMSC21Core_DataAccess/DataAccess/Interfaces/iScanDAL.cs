using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WMSCore_DataAccess.DataAccess.Interfaces
{
    interface iScanDAL
    {
        WMSCore_BusinessEntities.Entities.ScannedItem ValidateLocation(WMSCore_BusinessEntities.Entities.ScannedItem obj);
        WMSCore_BusinessEntities.Entities.ScannedItem ValidatePallet(WMSCore_BusinessEntities.Entities.ScannedItem obj);
        WMSCore_BusinessEntities.Entities.ScannedItem ValidateItem(WMSCore_BusinessEntities.Entities.ScannedItem obj);
        WMSCore_BusinessEntities.Entities.ScannedItem ValidateSO(WMSCore_BusinessEntities.Entities.ScannedItem obj);
        WMSCore_BusinessEntities.Entities.ScannedItem ValidateCarton(WMSCore_BusinessEntities.Entities.ScannedItem obj);

    }
}
