using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MRLWMSC21_Endpoint.DTO
{
    public class LoadDTO
    {
        private string _LoadSheetNumber;
        private string _VehicleNumber;
        private string _DockNumber;
        private decimal _Volume;
        private decimal _Weight;
        private decimal _BoxQty;
        private string _LoadSheetQuantity;
        private string _LoadedQuantity;

        private string _CustomerCode;
        public decimal BoxQty { get => _BoxQty; set => _BoxQty = value; }
        public decimal Volume { get => _Volume; set => _Volume = value; }
        public decimal Weight { get => _Weight; set => _Weight = value; }
        public string DockNumber { get => _DockNumber; set => _DockNumber = value; }
        public string VehicleNumber { get => _VehicleNumber; set => _VehicleNumber = value; }
        public string LoadSheetNumber { get => _LoadSheetNumber; set => _LoadSheetNumber = value; }
        public string LoadSheetQuantity { get => _LoadSheetQuantity; set => _LoadSheetQuantity = value; }
        public string LoadedQuantity { get => _LoadedQuantity; set => _LoadedQuantity = value; }
        public string CustomerCode { get => _CustomerCode; set => _CustomerCode = value; }
    }
}