using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MRLWMSC21Core.Library;

namespace MRLWMSC21Core.Entities
{
    public class GoodsMovement
    {
        private int _GoodsMovementDetailsID;
        private int _DockGoodsMovementDetailsID;

        private int _TransactionDocumentID;
        private string _TransactionDocumentNo;

        private int _POSODetailsID;
        
        private int _MaterialMasterID;

        private string _MaterialCode;
        private string _RSNNumber;

        private int _MaterialTransactionID;

        private int _KitPlannerID;

        private int _PickedFromLocationID;
        private string _PickedFromLocationCode;
        private int _PickedFromSLocID;
        private string _PickedFromSLoc;
        private int _PickedContainerID;
        private string _PickedContainerCode;
     

        private bool _AutoPickFromCurrentLocation;
        private bool _IsLost;
        private bool _IsFound;
        private bool _IsPutaway;
        private bool _IsCycleCountMovement;

        private decimal _PickedMRP;
        private decimal _PutawayMRP;

        private int _PutawayAtLocationID;
        private string _PutawayAtLocationCode;
        private int _PutawayAtSLocID;
        private string _PutawayAtSLoc;
        private int _PutawayAtContainerID;
        private string _PutawayAtContainerCode;

        private string _SourceBatch;
        private string _DestinationBatch;
        
        private int _BUoMID;
        private decimal _TransferQuantity;

        private int _SuggestionID;

        private bool _IsDamaged;

        private int _CreatedBy;
        private Constants.MovementType _GoodsMovementType;
        private Constants.ShipmentType _ShipmentType;

        private int _MoveInventoryResult;
        private bool _IsMaterialOldMRP;

        private string _OriginalReceiptMaterialCode;
        private int _OriginalReceiptMaterialID;

        private string _MaterialDescription;

        private string _NewRSNForPartialPicking;
        private int _IsSkipPickSuggetion;

        public int GoodsMovementDetailsID { get => _GoodsMovementDetailsID; set => _GoodsMovementDetailsID = value; }
        public int DockGoodsMovementDetailsID { get => _DockGoodsMovementDetailsID; set => _DockGoodsMovementDetailsID = value; }
        public int TransactionDocumentID { get => _TransactionDocumentID; set => _TransactionDocumentID = value; }
        public int POSODetailsID { get => _POSODetailsID; set => _POSODetailsID = value; }
        public int MaterialMasterID { get => _MaterialMasterID; set => _MaterialMasterID = value; }
        public int MaterialTransactionID { get => _MaterialTransactionID; set => _MaterialTransactionID = value; }
        public int KitPlannerID { get => _KitPlannerID; set => _KitPlannerID = value; }
        public int PickedFromLocationID { get => _PickedFromLocationID; set => _PickedFromLocationID = value; }
        public string PickedFromLocationCode { get => _PickedFromLocationCode; set => _PickedFromLocationCode = value; }
        public int PickedFromSLocID { get => _PickedFromSLocID; set => _PickedFromSLocID = value; }
        public int PickedContainerID { get => _PickedContainerID; set => _PickedContainerID = value; }
        public string PickedContainerCode { get => _PickedContainerCode; set => _PickedContainerCode = value; }
        public bool AutoPickFromCurrentLocation { get => _AutoPickFromCurrentLocation; set => _AutoPickFromCurrentLocation = value; }
        public int PutawayAtLocationID { get => _PutawayAtLocationID; set => _PutawayAtLocationID = value; }
        public string PutawayAtLocationCode { get => _PutawayAtLocationCode; set => _PutawayAtLocationCode = value; }
        public int PutawayAtSLocID { get => _PutawayAtSLocID; set => _PutawayAtSLocID = value; }
        public int PutawayAtContainerID { get => _PutawayAtContainerID; set => _PutawayAtContainerID = value; }
        public string PutawayAtContainerCode { get => _PutawayAtContainerCode; set => _PutawayAtContainerCode = value; }
        public int BUoMID { get => _BUoMID; set => _BUoMID = value; }
        public Constants.MovementType GoodsMovementType { get => _GoodsMovementType; set => _GoodsMovementType = value; }
        public Constants.ShipmentType ShipmentType { get => _ShipmentType; set => _ShipmentType = value; }
        public string SourceBatch { get => _SourceBatch; set => _SourceBatch = value; }
        public string DestinationBatch { get => _DestinationBatch; set => _DestinationBatch = value; }
        public string MaterialCode { get => _MaterialCode; set => _MaterialCode = value; }
        public string RSNNumber { get => _RSNNumber; set => _RSNNumber = value; }
        public int CreatedBy { get => _CreatedBy; set => _CreatedBy = value; }
        public decimal TransferQuantity { get => _TransferQuantity; set => _TransferQuantity = value; }
        public int MoveInventoryResult { get => _MoveInventoryResult; set => _MoveInventoryResult = value; }
        public decimal PickedMRP { get => _PickedMRP; set => _PickedMRP = value; }
        public decimal PutawayMRP { get => _PutawayMRP; set => _PutawayMRP = value; }
        public string PutawayAtSLoc { get => _PutawayAtSLoc; set => _PutawayAtSLoc = value; }
        public string PickedFromSLoc { get => _PickedFromSLoc; set => _PickedFromSLoc = value; }
        public bool IsDamaged { get => _IsDamaged; set => _IsDamaged = value; }
        public string TransactionDocumentNo { get => _TransactionDocumentNo; set => _TransactionDocumentNo = value; }
        public string OriginalReceiptMaterialCode { get => _OriginalReceiptMaterialCode; set => _OriginalReceiptMaterialCode = value; }
        public int OriginalReceiptMaterialID { get => _OriginalReceiptMaterialID; set => _OriginalReceiptMaterialID = value; }
        public bool IsMaterialOldMRP { get => _IsMaterialOldMRP; set => _IsMaterialOldMRP = value; }
        public int SuggestionID { get => _SuggestionID; set => _SuggestionID = value; }
        public bool IsLost { get => _IsLost; set => _IsLost = value; }
        public bool IsFound { get => _IsFound; set => _IsFound = value; }

        public string MaterialDescription { get => _MaterialDescription; set => _MaterialDescription = value; }
        public int IsPicking { get => isPicking; set => isPicking = value; }
        public string NewRSNForPartialPicking { get => _NewRSNForPartialPicking; set => _NewRSNForPartialPicking = value; }
        public bool IsPutaway { get => _IsPutaway; set => _IsPutaway = value; }
        public bool IsCycleCountMovement { get => _IsCycleCountMovement; set => _IsCycleCountMovement = value; }
        public int IsSkipPickSuggetion { get => _IsSkipPickSuggetion; set => _IsSkipPickSuggetion = value; }

        private int isPicking;
    }



}
