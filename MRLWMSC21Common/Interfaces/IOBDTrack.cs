using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRLWMSC21Common.Interfaces
{
   public interface IOBDTrack
    {

        void LoadFromDB(int OutBoundID, int StoreID);

         int InitiateOBDTracking();
         bool UpdateOBDTracking_Initiator();
         int UpdateOBDTracking_SentForPGI();
        bool UpdateOBDTracking_Delivered();
        bool UpdateOBDTracking_Transfered();
        int UpdateOBDTracking_PGIDone();
        int UpdateOBDTracking_Packing();
        int UpdateOBDTracking_Delivery();
        
    }
}
