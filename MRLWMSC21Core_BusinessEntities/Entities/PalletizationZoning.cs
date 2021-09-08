using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MRLWMSC21Core.Entities
{
    public class PalletizationZoning
    {
        private int _InboundID;

        private int _MaterialMasterID;

        private int _HomeZoneHeaderID;

        private int _HomeLevelID;

        private int _MaterialRangeID;

        private bool _IsByLevel;

        private bool _IsByZone;

        private bool _IsByRange;

        private bool _IsBySKU;

        private string _MaterialCode;
        public int InboundID { get => _InboundID; set => _InboundID = value; }
        public int MaterialMasterID { get => _MaterialMasterID; set => _MaterialMasterID = value; }
        public int HomeZoneHeaderID { get => _HomeZoneHeaderID; set => _HomeZoneHeaderID = value; }
        public int HomeLevelID { get => _HomeLevelID; set => _HomeLevelID = value; }
        public int MaterialRangeID { get => _MaterialRangeID; set => _MaterialRangeID = value; }
        public bool IsByLevel { get => _IsByLevel; set => _IsByLevel = value; }
        public bool IsByZone { get => _IsByZone; set => _IsByZone = value; }
        public bool IsByRange { get => _IsByRange; set => _IsByRange = value; }
        public bool IsBySKU { get => _IsBySKU; set => _IsBySKU = value; }
        public string MaterialCode { get => _MaterialCode; set => _MaterialCode = value; }
    }
}
