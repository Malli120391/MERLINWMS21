using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace MRLWMSC21Core.Entities
{
    public class PickingPreferences
    {
        private bool _AllowNestedInventoryDispatch;

        private bool _AllowDispatchOfOLDMRP;

        private bool _AllowCrossDocking;

        private bool _StrictComplianceToPicking;

        private bool _AutoReconsileInventoryOnSkip;

        public bool AllowNestedInventoryDispatch { get => _AllowNestedInventoryDispatch; set => _AllowNestedInventoryDispatch = value; }
        public bool AllowDispatchOfOLDMRP { get => _AllowDispatchOfOLDMRP; set => _AllowDispatchOfOLDMRP = value; }
        public bool AllowCrossDocking { get => _AllowCrossDocking; set => _AllowCrossDocking = value; }
        public bool StrictComplianceToPicking { get => _StrictComplianceToPicking; set => _StrictComplianceToPicking = value; }
        public bool AutoReconsileInventoryOnSkip { get => _AutoReconsileInventoryOnSkip; set => _AutoReconsileInventoryOnSkip = value; }
    }
}
