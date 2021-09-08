using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MRLWMSC21Core.Entities
{
    public class Pallet
    {
        private int _PalletID;
        private string _PalletCode;
        private string _PalletType;

        public int PalletID { get => _PalletID; set => _PalletID = value; }
        public string PalletCode { get => _PalletCode; set => _PalletCode = value; }
        public string PalletType { get => _PalletType; set => _PalletType = value; }
    }
}
