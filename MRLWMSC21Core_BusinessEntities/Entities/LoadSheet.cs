using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MRLWMSC21Core.Entities
{
    public class LoadSheet
    {
        private int _LoadHeaderID;
        private int _LoadStatusID;
        private int _PickListHeaderID;
        private int _YM_MST_VehicleType_ID;
        private int _YM_TRN_VehicleYardAvailability_ID;

        private string _LoadRefNo;
        private string _Remarks;

        private int _VehicleID;
        private int _DockLocationID;

        private decimal _LoadSheetQuantity;
        private decimal _LoadedQuantity;


        private string _CustomerCode;
        private string _VehicleType;
        private string _VehicleNumber;
        private string _DockSystemLocationCode;
        private string _DockDisplayLocationCode;
        
        private decimal _VehicleMaxVolumeCFT;
        private decimal _VehicleMaxWeightKG;

        private List<LoadItem> _LoadItemsList;

        public int LoadHeaderID { get => _LoadHeaderID; set => _LoadHeaderID = value; }
        public int LoadStatusID { get => _LoadStatusID; set => _LoadStatusID = value; }
        public int PickListHeaderID { get => _PickListHeaderID; set => _PickListHeaderID = value; }
        public int YM_MST_VehicleType_ID { get => _YM_MST_VehicleType_ID; set => _YM_MST_VehicleType_ID = value; }
        public int YM_TRN_VehicleYardAvailability_ID { get => _YM_TRN_VehicleYardAvailability_ID; set => _YM_TRN_VehicleYardAvailability_ID = value; }
        public string LoadRefNo { get => _LoadRefNo; set => _LoadRefNo = value; }
        public string Remarks { get => _Remarks; set => _Remarks = value; }
        public int VehicleID { get => _VehicleID; set => _VehicleID = value; }
        public int DockLocationID { get => _DockLocationID; set => _DockLocationID = value; }
        public string VehicleType { get => _VehicleType; set => _VehicleType = value; }
        public string VehicleNumber { get => _VehicleNumber; set => _VehicleNumber = value; }
        public string DockSystemLocationCode { get => _DockSystemLocationCode; set => _DockSystemLocationCode = value; }
        public string DockDisplayLocationCode { get => _DockDisplayLocationCode; set => _DockDisplayLocationCode = value; }
        public decimal VehicleMaxVolumeCFT { get => _VehicleMaxVolumeCFT; set => _VehicleMaxVolumeCFT = value; }
        public decimal VehicleMaxWeightKG { get => _VehicleMaxWeightKG; set => _VehicleMaxWeightKG = value; }
        public List<LoadItem> LoadItemsList { get => _LoadItemsList; set => _LoadItemsList = value; }
        public decimal LoadSheetQuantity { get => _LoadSheetQuantity; set => _LoadSheetQuantity = value; }
        public decimal LoadedQuantity { get => _LoadedQuantity; set => _LoadedQuantity = value; }
        public string CustomerCode { get => _CustomerCode; set => _CustomerCode = value; }
    }
}
