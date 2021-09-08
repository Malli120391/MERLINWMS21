using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MRLWMSC21_Endpoint.DTO
{
    public class ColorDTO
    {
        private string _ColourCode;

        private string _ColourName;

        private string _ColourID;

        public string ColourCode { get => _ColourCode; set => _ColourCode = value; }
        public string ColourName { get => _ColourName; set => _ColourName = value; }
        public string ColourID { get => _ColourID; set => _ColourID = value; }
    }
}