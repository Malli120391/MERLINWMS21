using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MRLWMSC21_Endpoint.DTO
{
    public class SupplierReturnDTO
    {

        private string _MaterialMasterID;
                       
        private string _MaterialMasterIDUomUID;
        private string _POHeaderID;
                       
        private string _DockID;
        private string _VehicleTypeID;
        private string _VehicleNo;
                       
        private string _DriverName;
        private string _Line;
                       
        private string _WareHouseCode;
        private string _MobileNo;
                       
        private string _BatchNo;
                       
        private string _MfgDate;
        private string _ExpDate;
        private string _SerialNo;
        private string _KitplannerID;
        private string _ProjectRefNo;
        private string _PickedQty;
        private string _PendingReturnQty;
        private string _ReturnQty;
        private string _Isselected;

        public string MaterialMasterID { get => _MaterialMasterID; set => _MaterialMasterID = value; }
        public string MaterialMasterIDUomUID { get => _MaterialMasterIDUomUID; set => _MaterialMasterIDUomUID = value; }
        public string POHeaderID { get => _POHeaderID; set => _POHeaderID = value; }
        public string DockID { get => _DockID; set => _DockID = value; }
        public string VehicleTypeID { get => _VehicleTypeID; set => _VehicleTypeID = value; }
        public string VehicleNo { get => _VehicleNo; set => _VehicleNo = value; }
        public string DriverName { get => _DriverName; set => _DriverName = value; }
        public string Line { get => _Line; set => _Line = value; }
        public string WareHouseCode { get => _WareHouseCode; set => _WareHouseCode = value; }
        public string MobileNo { get => _MobileNo; set => _MobileNo = value; }
        public string BatchNo { get => _BatchNo; set => _BatchNo = value; }
        public string MfgDate { get => _MfgDate; set => _MfgDate = value; }
        public string ExpDate { get => _ExpDate; set => _ExpDate = value; }
        public string SerialNo { get => _SerialNo; set => _SerialNo = value; }
        public string KitplannerID { get => _KitplannerID; set => _KitplannerID = value; }
        public string ProjectRefNo { get => _ProjectRefNo; set => _ProjectRefNo = value; }
        public string PickedQty { get => _PickedQty; set => _PickedQty = value; }
        public string PendingReturnQty { get => _PendingReturnQty; set => _PendingReturnQty = value; }
        public string ReturnQty { get => _ReturnQty; set => _ReturnQty = value; }
        public string Isselected { get => _Isselected; set => _Isselected = value; }
    }
}