using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MRLWMSC21Core.Entities
{
    public class Colour
    {
        private string _ColourCode;

        private string _ColourName;

        private int _ColourID;
        
        public string ColourCode { get => _ColourCode; set => _ColourCode = value; }
        public string ColourName { get => _ColourName; set => _ColourName = value; }
        public int ColourID { get => _ColourID; set => _ColourID = value; }
    }
}
