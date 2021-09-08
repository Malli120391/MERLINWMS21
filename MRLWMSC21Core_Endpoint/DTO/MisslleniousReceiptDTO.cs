using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MRLWMSC21_Endpoint.DTO
{
    public class MisslleniousReceiptDTO
    {
        private string _MaterialMasterID;
        private string _POQuantity;
        private string _WareHouseCode;
        private string _Line;
        private string _LocationCode;
        private string _ContainerCode;
        private string _StorageConditionCode;
        private string _Remarks;
        private string _IsDamage;
        private string _IsHasDiscipency;
        private string _MfgDate;
        private string _ExpDate;
        private string _SerialNo;
        private string _BatchNo;
        private string _ProjectRefNo;
        public string MaterialMasterID { get => _MaterialMasterID; set => _MaterialMasterID = value; }
        public string POQuantity { get => _POQuantity; set => _POQuantity = value; }
        public string WareHouseCode { get => _WareHouseCode; set => _WareHouseCode = value; }
        public string Line { get => _Line; set => _Line = value; }
        public string LocationCode { get => _LocationCode; set => _LocationCode = value; }
        public string ContainerCode { get => _ContainerCode; set => _ContainerCode = value; }
        public string StorageConditionCode { get => _StorageConditionCode; set => _StorageConditionCode = value; }
        public string Remarks { get => _Remarks; set => _Remarks = value; }
        public string IsDamage { get => _IsDamage; set => _IsDamage = value; }
        public string IsHasDiscipency { get => _IsHasDiscipency; set => _IsHasDiscipency = value; }
        public string MfgDate { get => _MfgDate; set => _MfgDate = value; }
        public string ExpDate { get => _ExpDate; set => _ExpDate = value; }
        public string SerialNo { get => _SerialNo; set => _SerialNo = value; }
        public string BatchNo { get => _BatchNo; set => _BatchNo = value; }
        public string ProjectRefNo { get => _ProjectRefNo; set => _ProjectRefNo = value; }

    }
}