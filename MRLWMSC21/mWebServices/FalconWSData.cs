using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MRLWMSC21.mWebServices
{
    public class FalconWSData
    {
        [Serializable]
        public class MaterialReceivedStuct
        {
            private int _GoodsMovementDetailsID;
            private int _DocQty;
            private string _IsDamaged;
            private DateTime _POSODetailsID;
            private DateTime _Location;
            private string _HasDiscrepancy;
            private string _SerialNo;
        
            //Constructor
            private MaterialReceivedStuct() { }
        }
    }
}