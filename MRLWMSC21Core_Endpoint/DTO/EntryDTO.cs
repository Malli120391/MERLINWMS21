using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MRLWMSC21_Endpoint.DTO
{
    public class EntryDTO
    {
        private string _DockNumber;
        private string _VehicleNumber;
        private string _DockID;

        public string DockNumber { get => _DockNumber; set => _DockNumber = value; }
        public string VehicleNumber { get => _VehicleNumber; set => _VehicleNumber = value; }
        public string DockID { get => _DockID; set => _DockID = value; }
    }
}