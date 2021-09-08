using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MRLWMSC21_Endpoint.DTO
{
    public class VehicleDTO
    {
        private string _VehicleID;

        private List<string> _VehicleNumber;

        private decimal _DocumentQuantity;

        private decimal _ReceivedQuantity;
        private List<InboundDTO> _InboundList;
        private List<ColorDTO> _ColorCodes;
        private List<StorageLocationDTO> _SLOC;
   


        public string VehicleID { get => _VehicleID; set => _VehicleID = value; }
        public List<string> VehicleNumber { get => _VehicleNumber; set => _VehicleNumber = value; }
       public decimal DocumentQuantity { get => _DocumentQuantity; set => _DocumentQuantity = value; }
        public decimal ReceivedQuantity { get => _ReceivedQuantity; set => _ReceivedQuantity = value; }
        public List<InboundDTO> InboundList { get => _InboundList; set => _InboundList = value; }
        public List<ColorDTO> ColorCodes { get => _ColorCodes; set => _ColorCodes = value; }
        public List<StorageLocationDTO> SLOC { get => _SLOC; set => _SLOC = value; }


    }
}