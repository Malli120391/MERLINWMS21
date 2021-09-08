using MRLWMSC21Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MRLWMSC21Core.DataAccess.Interfaces
{
    interface IPalletDAL
    {
        
        List<Pallet> GetPallets(SearchCriteria Criteria);

        
        Location GetPalletByID(int PalletID);
    }
}
