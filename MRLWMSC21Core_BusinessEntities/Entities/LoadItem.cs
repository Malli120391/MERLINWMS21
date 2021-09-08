using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MRLWMSC21Core.Entities
{
    public class LoadItem
    {
        private int _LoadHeaderID;
        private int _LoadDetailsID;
        private int _PickListHeaderID;
        private int _PickListDetailsID;
        private int _MaterialMasterID;
        private int _SuggestionID;

        private decimal _LoadQuantity;
        private decimal _LoadedQuantity;

        private string _LoadRefNo;
        private string _MCode;

        private string _MDescription;
        private decimal _ActiveMRP;

        private bool _IsParent;
        private decimal _TotalLoadedQuantity;
        private decimal _TotalLoadQuantity;

        private decimal _ItemVolume;
        private decimal _ItemWeight;

        private decimal _TotalLoadedVolume;
        private decimal _TotalLoadedWeight;


     

        public int LoadHeaderID { get => _LoadHeaderID; set => _LoadHeaderID = value; }
        public int LoadDetailsID { get => _LoadDetailsID; set => _LoadDetailsID = value; }
        public int PickListHeaderID { get => _PickListHeaderID; set => _PickListHeaderID = value; }
        public int PickListDetailsID { get => _PickListDetailsID; set => _PickListDetailsID = value; }
        public int MaterialMasterID { get => _MaterialMasterID; set => _MaterialMasterID = value; }
        public int SuggestionID { get => _SuggestionID; set => _SuggestionID = value; }
        public decimal LoadQuantity { get => _LoadQuantity; set => _LoadQuantity = value; }
        public decimal LoadedQuantity { get => _LoadedQuantity; set => _LoadedQuantity = value; }
        public string LoadRefNo { get => _LoadRefNo; set => _LoadRefNo = value; }
        public string MCode { get => _MCode; set => _MCode = value; }
        public decimal ActiveMRP { get => _ActiveMRP; set => _ActiveMRP = value; }
        public bool IsParent { get => _IsParent; set => _IsParent = value; }
        public decimal TotalLoadedQuantity { get => _TotalLoadedQuantity; set => _TotalLoadedQuantity = value; }
        public decimal TotalLoadQuantity { get => _TotalLoadQuantity; set => _TotalLoadQuantity = value; }
        public decimal ItemVolume { get => _ItemVolume; set => _ItemVolume = value; }
        public decimal ItemWeight { get => _ItemWeight; set => _ItemWeight = value; }
        public decimal TotalLoadedVolume { get => _TotalLoadedVolume; set => _TotalLoadedVolume = value; }
        public decimal TotalLoadedWeight { get => _TotalLoadedWeight; set => _TotalLoadedWeight = value; }
        public string MDescription { get => _MDescription; set => _MDescription = value; }
    }
}
