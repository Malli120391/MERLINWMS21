using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MRLWMSC21_Endpoint.DTO
{
    public class ItemMasterDTO
    {
       
        private string _SupplierCode;
        private string _PartNumber;
        private string _OEMPartNumber;
        private string _MaterialType;
        private string _ProdcutCategory;
        private string _StorageCondition;
        private string _ItemDescriptionShort;
        private string _ItemDescriptionLong;
        private string _MLength;
        private string _MWidth;
        private string _MHeight;
        private string _MWeight;
        private string _GroupCode;
        private string _CapacityPerBin;
        private string _MCodeAlternative1;
        private string _MCodeAlternative2;
        private string _BUoM;
        private string _MUoM;
        private string _BUoM_Quantity;
        private string _MUoM_Quantity;

        
        public string SupplierCode { get => _SupplierCode; set => _SupplierCode = value; }
        public string PartNumber { get => _PartNumber; set => _PartNumber = value; }
        public string OEMPartNumber { get => _OEMPartNumber; set => _OEMPartNumber = value; }
        public string MaterialType { get => _MaterialType; set => _MaterialType = value; }
        public string ProdcutCategory { get => _ProdcutCategory; set => _ProdcutCategory = value; }
        public string StorageCondition { get => _StorageCondition; set => _StorageCondition = value; }
        public string ItemDescriptionShort { get => _ItemDescriptionShort; set => _ItemDescriptionShort = value; }
        public string ItemDescriptionLong { get => _ItemDescriptionLong; set => _ItemDescriptionLong = value; }
        public string MLength { get => _MLength; set => _MLength = value; }
        public string MWidth { get => _MWidth; set => _MWidth = value; }
        public string MHeight { get => _MHeight; set => _MHeight = value; }
        public string MWeight { get => _MWeight; set => _MWeight = value; }
        public string GroupCode { get => _GroupCode; set => _GroupCode = value; }
        public string CapacityPerBin { get => _CapacityPerBin; set => _CapacityPerBin = value; }
        public string MCodeAlternative1 { get => _MCodeAlternative1; set => _MCodeAlternative1 = value; }
        public string MCodeAlternative2 { get => _MCodeAlternative2; set => _MCodeAlternative2 = value; }
        public string BUoM { get => _BUoM; set => _BUoM = value; }
        public string MUoM { get => _MUoM; set => _MUoM = value; }
        public string BUoM_Quantity { get => _BUoM_Quantity; set => _BUoM_Quantity = value; }
        public string MUoM_Quantity { get => _MUoM_Quantity; set => _MUoM_Quantity = value; }
    }
}