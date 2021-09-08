using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MRLWMSC21Core.Library;

namespace MRLWMSC21Core.Entities
{
    public class Inventory
    {
        private Constants.MovementType _MovementType;

        private string _MaterialCode;
        private string _RSN;
        private string _LocationCode;
        private string _DisplayLocationCode;
        private string _ContainerCode;
        private string _ReferenceDocumentNumber;
        private string _VehicleNumber;
        private string _BatchNumber;
        private string _Color;
        private string _StorageLocation;
        private string _MaterialShortDescription;

        private string _OriginalReceiptMaterialCode;
        private int _OriginalReceiptMaterialID;

        private int _MaterialMasterID;
        private int _MaterialTransactionID;

        private int _VehicleID;
        private int _ReferenceDocumentID;
        private int _ContainerID;
        private int _LocationID;
        private int _MonthOfMfg;
        private int _YearOfMfg;
        private int _DockGoodsMovementID;
        private int _GenericMaterialID;
        private int _HomeZoneHeaderID;
        private int _HomeLevelID;
        private int _MOP;
        private int _ColorID;
        private int _PosoDetailsID;
        private int _StorageLocationID;
        private decimal _MRP;
        private decimal _Quantity;

        private bool _IsMaterialParent;

        private decimal _DocumentQuantity;
        private decimal _DocumentProcessedQuantity;
        private decimal _OldMRP;
        private int _CreatedBy;
        private int _ReceivedInUoM;
        private int _MaterialRangeID;
        private bool _IsDamaged;
        private bool _IsReceived;

        private bool _IsAlreadyPerformed;
        private bool _UserConfirmReDo;
        private bool _UserConfirmedExcessTransaction;

        private bool _IsDispatched;
        private decimal _IBOHQuantity;
        private decimal _OBOHQuantity;
        private decimal _AvailableQuantity;

        private bool _Revert;
        private bool _IsFinishedGoods;
        private bool _IsRawMaterial;
        private bool _IsConsumables;
        private bool _IsLost;

        private bool _IsOldMRP;

        private int _SuggestionID;

        private decimal _ItemVolume;
        private decimal _ItemWeight;

        private DateTime _MfgDate;

        private string _mfgDate;

        private string _ExpDate;
        private string _SerialNo;
        private string _ProjectRefNo;
        private string _ToStorageLocation;
        private int _UserId;
        private int _Result;

        private List<Colour> _ColourMasterList;

        private bool _IsExcessInventory;

        private int _WarehouseID;
        private int _TenantID;


        public string MaterialCode { get => _MaterialCode; set => _MaterialCode = value; }
        public string RSN { get => _RSN; set => _RSN = value; }
        public string LocationCode { get => _LocationCode; set => _LocationCode = value; }
        public string ContainerCode { get => _ContainerCode; set => _ContainerCode = value; }
        public string ReferenceDocumentNumber { get => _ReferenceDocumentNumber; set => _ReferenceDocumentNumber = value; }
        public string VehicleNumber { get => _VehicleNumber; set => _VehicleNumber = value; }

        public int MaterialMasterID { get => _MaterialMasterID; set => _MaterialMasterID = value; }
        public int VehicleID { get => _VehicleID; set => _VehicleID = value; }
        public int ReferenceDocumentID { get => _ReferenceDocumentID; set => _ReferenceDocumentID = value; }
        public int ContainerID { get => _ContainerID; set => _ContainerID = value; }
        public int LocationID { get => _LocationID; set => _LocationID = value; }
        public int MonthOfMfg { get => _MonthOfMfg; set => _MonthOfMfg = value; }
        public int YearOfMfg { get => _YearOfMfg; set => _YearOfMfg = value; }
        public int MaterialTransactionID { get => _MaterialTransactionID; set => _MaterialTransactionID = value; }
        public int MOP { get => _MOP; set => _MOP = value; }
        public int ColorID { get => _ColorID; set => _ColorID = value; }
        public decimal MRP { get => _MRP; set => _MRP = value; }
        public int ReceivedInUoM { get => _ReceivedInUoM; set => _ReceivedInUoM = value; }
        public bool IsDamaged { get => _IsDamaged; set => _IsDamaged = value; }
        public Constants.MovementType MovementType { get => _MovementType; set => _MovementType = value; }
        public int PosoDetailsID { get => _PosoDetailsID; set => _PosoDetailsID = value; }
        public int StorageLocationID { get => _StorageLocationID; set => _StorageLocationID = value; }
        public decimal Quantity { get => _Quantity; set => _Quantity = value; }
        public int CreatedBy { get => _CreatedBy; set => _CreatedBy = value; }
        public bool IsReceived { get => _IsReceived; set => _IsReceived = value; }
        public bool IsDispatched { get => _IsDispatched; set => _IsDispatched = value; }
        public int DockGoodsMovementID { get => _DockGoodsMovementID; set => _DockGoodsMovementID = value; }
        public decimal IBOHQuantity { get => _IBOHQuantity; set => _IBOHQuantity = value; }
        public decimal OBOHQuantity { get => _OBOHQuantity; set => _OBOHQuantity = value; }
        public List<Colour> ColourMasterList { get => _ColourMasterList; set => _ColourMasterList = value; }
        public bool Revert { get => _Revert; set => _Revert = value; }
        public bool IsFinishedGoods { get => _IsFinishedGoods; set => _IsFinishedGoods = value; }
        public bool IsRawMaterial { get => _IsRawMaterial; set => _IsRawMaterial = value; }
        public bool IsConsumables { get => _IsConsumables; set => _IsConsumables = value; }
        public string BatchNumber { get => _BatchNumber; set => _BatchNumber = value; }
        public string Color { get => _Color; set => _Color = value; }
        public string StorageLocation { get => _StorageLocation; set => _StorageLocation = value; }
        public decimal ItemVolume { get => _ItemVolume; set => _ItemVolume = value; }
        public decimal ItemWeight { get => _ItemWeight; set => _ItemWeight = value; }
        public string DisplayLocationCode { get => _DisplayLocationCode; set => _DisplayLocationCode = value; }
        public int GenericMaterialID { get => _GenericMaterialID; set => _GenericMaterialID = value; }
        public int HomeZoneHeaderID { get => _HomeZoneHeaderID; set => _HomeZoneHeaderID = value; }
        public int HomeLevelID { get => _HomeLevelID; set => _HomeLevelID = value; }
        public DateTime MfgDate { get => _MfgDate; set => _MfgDate = value; }
        public decimal DocumentQuantity { get => _DocumentQuantity; set => _DocumentQuantity = value; }
        public decimal DocumentProcessedQuantity { get => _DocumentProcessedQuantity; set => _DocumentProcessedQuantity = value; }
        public bool IsExcessInventory { get => _IsExcessInventory; set => _IsExcessInventory = value; }
        public int MaterialRangeID { get => _MaterialRangeID; set => _MaterialRangeID = value; }
        public string OriginalReceiptMaterialCode { get => _OriginalReceiptMaterialCode; set => _OriginalReceiptMaterialCode = value; }
        public int OriginalReceiptMaterialID { get => _OriginalReceiptMaterialID; set => _OriginalReceiptMaterialID = value; }
        public bool IsLost { get => _IsLost; set => _IsLost = value; }
        public bool IsAlreadyPerformed { get => _IsAlreadyPerformed; set => _IsAlreadyPerformed = value; }
        public bool UserConfirmReDo { get => _UserConfirmReDo; set => _UserConfirmReDo = value; }
        public bool UserConfirmedExcessTransaction { get => _UserConfirmedExcessTransaction; set => _UserConfirmedExcessTransaction = value; }
        public string MaterialShortDescription { get => _MaterialShortDescription; set => _MaterialShortDescription = value; }
        public bool IsMaterialParent { get => _IsMaterialParent; set => _IsMaterialParent = value; }
        public int SuggestionID { get => _SuggestionID; set => _SuggestionID = value; }
        public decimal OldMRP { get => _OldMRP; set => _OldMRP = value; }
        public bool IsOldMRP { get => _IsOldMRP; set => _IsOldMRP = value; }
        public decimal AvailableQuantity { get => _AvailableQuantity; set => _AvailableQuantity = value; }
        public int WarehouseID { get => _WarehouseID; set => _WarehouseID = value; }
        public int TenantID { get => _TenantID; set => _TenantID = value; }
        
        public string SerialNo { get => _SerialNo; set => _SerialNo = value; }
        public string ProjectRefNo { get => _ProjectRefNo; set => _ProjectRefNo = value; }
        public string ToStorageLocation { get => _ToStorageLocation; set => _ToStorageLocation = value; }
        public int UserId { get => _UserId; set => _UserId = value; }
        public int Result { get => _Result; set => _Result = value; }
        public string ExpDate { get => _ExpDate; set => _ExpDate = value; }
        public string Mfg_Date { get => _mfgDate; set => _mfgDate = value; }

        public Inventory(string MaterialRSN)
        {
            this.RSN = MaterialRSN;

            FetchMaterialDetailsFromRSN();
        }

         public Inventory()
        {       
            
        }

        public Inventory(Inventory oInventory)
        {

        }


        public Inventory DuplicateItem()
        {
            Inventory oInventoryDuplicate = new Inventory(this.RSN)
            {
                MaterialShortDescription = this.MaterialShortDescription,
                BatchNumber = this.BatchNumber,
                Color = this.Color,
                StorageLocation = this.StorageLocation,
                StorageLocationID = this.StorageLocationID,
                RSN = this.RSN,
                ColorID = this.ColorID,
                ColourMasterList = this.ColourMasterList,
                ContainerCode = this.ContainerCode,
                ContainerID = this.ContainerID,
                CreatedBy = this.CreatedBy,
                DisplayLocationCode = this.DisplayLocationCode,
                DockGoodsMovementID = this.DockGoodsMovementID,
                DocumentProcessedQuantity = this.DocumentProcessedQuantity,
                DocumentQuantity = this.DocumentQuantity,
                GenericMaterialID = this.GenericMaterialID,
                HomeLevelID = this.HomeLevelID,
                HomeZoneHeaderID = this.HomeZoneHeaderID,
                IBOHQuantity = this.IBOHQuantity,
                IsConsumables = this.IsConsumables,
                IsDamaged = this.IsDamaged,
                IsDispatched = this.IsDispatched,
                IsExcessInventory = this.IsExcessInventory,
                IsFinishedGoods = this.IsFinishedGoods,
                IsRawMaterial = this.IsRawMaterial,

                IsReceived = this.IsReceived,
                ItemVolume = this.ItemVolume,
                ItemWeight = this.ItemWeight,
                LocationCode = this.LocationCode,
                LocationID = this.LocationID,
                MaterialCode = this.MaterialCode,
                MaterialMasterID = this.MaterialMasterID,
                MaterialRangeID = this.MaterialRangeID,
                MaterialTransactionID = this.MaterialTransactionID,

                MfgDate = this.MfgDate,
                MonthOfMfg = this.MonthOfMfg,
                MOP = this.MOP,
                MovementType = this.MovementType,
                MRP = this.MRP,
                OBOHQuantity = this.OBOHQuantity,
                OriginalReceiptMaterialCode = this.OriginalReceiptMaterialCode,
                OriginalReceiptMaterialID = this.OriginalReceiptMaterialID,
                PosoDetailsID = this.PosoDetailsID,
                Quantity = this.Quantity,
                ReceivedInUoM = this.ReceivedInUoM,
                ReferenceDocumentID = this.ReferenceDocumentID,
                ReferenceDocumentNumber = this.ReferenceDocumentNumber,
                Revert = this.Revert,
                VehicleID = this.VehicleID,
                VehicleNumber = this.VehicleNumber,
                YearOfMfg = this.YearOfMfg,

                IsMaterialParent=this.IsMaterialParent,
                SuggestionID=this.SuggestionID,
                IsAlreadyPerformed = this.IsAlreadyPerformed,
                IsLost = this.IsLost,
                UserConfirmedExcessTransaction = this.UserConfirmedExcessTransaction,
                UserConfirmReDo = this.UserConfirmReDo

            };



            return oInventoryDuplicate;
        }

        public void FetchMaterialDetailsFromRSN()
        {
            /*
             VIP FRD Ref #: 3.2.1

            [SKU Code]-[Supplier Code][Country of Origin][Month of Manufacturing]
            [Year of Manufacturing][Unique Running Serial Number for Factory]   

             */

            if (this.RSN == null || string.IsNullOrEmpty(this.RSN))
                throw new WMSExceptionMessage() { WMSExceptionCode = "WMC_BE_DEF_INV_0002", WMSMessage = ErrorMessages.WMC_BE_DEF_INV_0002, ShowAsError = true };

            string _SKUCode = string.Empty;
            string _SupplierCode = string.Empty;
            string _COO = string.Empty;
            string _MMfg = string.Empty;
            string _YMfg = string.Empty;
            string _ColorCode = string.Empty;

            this.OriginalReceiptMaterialCode = this.MaterialCode;
            this.OriginalReceiptMaterialID = this.MaterialMasterID;

            string[] _Parts = RSN.Split('-');

            if (_Parts.Length == 2)
            {
                int _CharIndex = 1;
                foreach (char _Char in _Parts[1])
                {

                    if (_CharIndex <= 2 && !CheckAlphaNumeric((int)_Char))
                        ThrowRSNFormatException();
                    else if (_CharIndex > 2 && _CharIndex <= 4 && !(CheckAlphabetsUpperCase((int)_Char) || CheckAlphabetsLowerCase((int)_Char)))
                        ThrowRSNFormatException();
                    else if (_CharIndex > 4 && !CheckNumeric((int)_Char))
                        ThrowRSNFormatException();


                    _CharIndex++;
                }

                _SKUCode = _Parts[0];                       // VIP FRD : 3.2.1.1
                _SupplierCode = _Parts[1].Substring(0, 2);  // VIP FRD : 3.2.1.2
                _COO = _Parts[1].Substring(2, 1);           // VIP FRD : 3.2.1.3
                _MMfg = _Parts[1].Substring(3, 1);          // VIP FRD : 3.2.1.4
                _YMfg = "20" + _Parts[1].Substring(4, 2);   // VIP FRD : 3.2.1.5      

                _ColorCode = _SKUCode.Substring(_SKUCode.Length - 3, 3).Trim();

            }
            else ThrowRSNFormatException();

            this.MaterialCode = _SKUCode;
            this.MonthOfMfg = MonthOfManufacturing(_MMfg);
            this.YearOfMfg = Int32.Parse(_YMfg);
            this.MfgDate = new DateTime(Int32.Parse(_YMfg), MonthOfManufacturing(_MMfg), 01);
            this.Color = _ColorCode;
        }

        private void ThrowRSNFormatException()
        {
            throw new WMSExceptionMessage() { WMSExceptionCode = "WMC_BE_DEF_INV_0001", WMSMessage = ErrorMessages.WMC_BE_DEF_INV_0001, ShowAsError = true };
        }

        private bool CheckAlphaNumeric(int ASCIIValue)
        {
            return (CheckAlphabetsLowerCase(ASCIIValue) || CheckAlphabetsUpperCase(ASCIIValue) || CheckNumeric(ASCIIValue));
        }


        private bool CheckAlphabetsLowerCase(int ASCIIValue)
        {
            if (ASCIIValue >= 97 && ASCIIValue <= 122)
                return true;
            else
                return false;
        }

        private bool CheckAlphabetsUpperCase(int ASCIIValue)
        {
            // ASCII A: 65 - Z: 90
            if (ASCIIValue >= 65 && ASCIIValue <= 90)
                return true;
            else
                return false;
                          
                         
        }


        private bool CheckNumeric(int ASCIIValue)
        {

            // ASCII: 0 => 48 to 9 => 57
            if (ASCIIValue >= 48 && ASCIIValue <= 57)
                return true;
            else
                return false;

        }

        private int MonthOfManufacturing(string MonthString)
        {
            switch(MonthString.ToUpper())
            {
                case "A":
                    return 1;
                case "B":
                    return 2;
                case "C":
                    return 3;
                case "D":
                    return 4;
                case "E":
                    return 5;
                case "F":
                    return 6;
                case "G":
                    return 7;
                case "H":
                    return 8;
                case "I":
                    return 9;
                case "J":
                    return 10;
                case "K":
                    return 11;
                case "L":
                    return 12;
                default:
                    {
                        throw new WMSExceptionMessage() { WMSExceptionCode = "WMC_BE_DEF_INV_0001", WMSMessage = ErrorMessages.WMC_BE_DEF_INV_0001, ShowAsError = true };
                    }

            }
        }
    }
}
