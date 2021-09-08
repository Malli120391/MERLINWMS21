using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MRLWMSC21Core.Entities
{
    public class PickListInventory
    {


        private int _MaterialMasterID;
        private int _MaterialTransactionID;

        private decimal _RequirementQuantity;
        private decimal _PicklistQuantity;
        private decimal _PickedQuantity;

        private int _MaterialPriorityID;
        private string _MaterialPriority;
        private bool _IsBatchPicklist;

        private int _PickLocationID;
        private string _PickDisplayLocationCode;
        private string _PickSystemLocationCode;

        private string _DockLocation;
        private int _DockID;
        private string _MaterialCode;

        private int _PickListHeaderID;
        private int _PickListDetailsID;
        private string _PickListRefNo;
        private string _PalletCode;
        private int _SuggestionID;
        private string _CustomerCode;
        private string _MaterialDescription;

        public int MaterialMasterID { get => _MaterialMasterID; set => _MaterialMasterID = value; }
        public int MaterialTransactionID { get => _MaterialTransactionID; set => _MaterialTransactionID = value; }
        public decimal RequirementQuantity { get => _RequirementQuantity; set => _RequirementQuantity = value; }
        public decimal PicklistQuantity { get => _PicklistQuantity; set => _PicklistQuantity = value; }
        public decimal PickedQuantity { get => _PickedQuantity; set => _PickedQuantity = value; }
        public int MaterialPriorityID { get => _MaterialPriorityID; set => _MaterialPriorityID = value; }
        public string MaterialPriority { get => _MaterialPriority; set => _MaterialPriority = value; }
        public bool IsBatchPicklist { get => _IsBatchPicklist; set => _IsBatchPicklist = value; }
        public int PickLocationID { get => _PickLocationID; set => _PickLocationID = value; }
        public string PickDisplayLocationCode { get => _PickDisplayLocationCode; set => _PickDisplayLocationCode = value; }
        public string PickSystemLocationCode { get => _PickSystemLocationCode; set => _PickSystemLocationCode = value; }
        public string DockLocation { get => _DockLocation; set => _DockLocation = value; }
        public int DockID { get => _DockID; set => _DockID = value; }
        public string MaterialCode { get => _MaterialCode; set => _MaterialCode = value; }
        public int PickListHeaderID { get => _PickListHeaderID; set => _PickListHeaderID = value; }
        public int PickListDetailsID { get => _PickListDetailsID; set => _PickListDetailsID = value; }
        public string PickListRefNo { get => _PickListRefNo; set => _PickListRefNo = value; }
        public string PalletCode { get => _PalletCode; set => _PalletCode = value; }
        public int SuggestionID { get => _SuggestionID; set => _SuggestionID = value; }
        public string CustomerCode { get => _CustomerCode; set => _CustomerCode = value; }
        public string MaterialDescription { get => _MaterialDescription; set => _MaterialDescription = value; }
    }
}
