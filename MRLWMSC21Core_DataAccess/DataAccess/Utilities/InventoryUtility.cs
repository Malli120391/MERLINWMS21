using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MRLWMSC21Core.Entities;
using MRLWMSC21Core.Library;

namespace MRLWMSC21Core.DataAccess.Utilities
{
    public class InventoryUtility
    {
        
        public GoodsMovement GenerateInventoryLost(Inventory oInventory, Constants.MovementType movementType,int IsSkipPickSuggetion=0)
        {
            GoodsMovement _oMovementLost = new GoodsMovement()
            {
                GoodsMovementType = Constants.MovementType.InventoryLost,

                TransactionDocumentNo = oInventory.ReferenceDocumentNumber,

                OriginalReceiptMaterialCode = oInventory.OriginalReceiptMaterialCode,
                OriginalReceiptMaterialID = oInventory.OriginalReceiptMaterialID,

                MaterialCode = oInventory.MaterialCode,
                MaterialMasterID = oInventory.MaterialMasterID,
                MaterialTransactionID = oInventory.MaterialTransactionID,
                RSNNumber = oInventory.RSN,

                PickedFromLocationCode = oInventory.DisplayLocationCode,
                PickedFromLocationID = oInventory.LocationID,
                PickedContainerCode = oInventory.ContainerCode,
                PickedContainerID = oInventory.ContainerID,
                PickedFromSLoc = oInventory.StorageLocation,
                PickedFromSLocID = oInventory.StorageLocationID,

                PutawayAtSLocID = oInventory.StorageLocationID,

                SourceBatch = oInventory.BatchNumber,
                DestinationBatch = oInventory.BatchNumber,
                IsLost = true,
                IsFound = false,
                TransferQuantity = oInventory.Quantity,
                IsPicking = (movementType.Equals(Constants.MovementType.Picking) ? 1 : 0)   ,
                SuggestionID= oInventory.SuggestionID,
                IsSkipPickSuggetion= IsSkipPickSuggetion
            };

            return _oMovementLost;
        }

        public GoodsMovement GenerateInventoryFound(Inventory oInventory, Constants.MovementType movementType)
        {

            GoodsMovement _oMovementFound = new GoodsMovement()
            {
                GoodsMovementType = Constants.MovementType.InventoryFound,

                TransactionDocumentNo = oInventory.ReferenceDocumentNumber,

                OriginalReceiptMaterialCode = oInventory.OriginalReceiptMaterialCode,
                OriginalReceiptMaterialID = oInventory.OriginalReceiptMaterialID,

                MaterialCode = oInventory.MaterialCode,
                MaterialMasterID = oInventory.MaterialMasterID,
                MaterialTransactionID = oInventory.MaterialTransactionID,
                RSNNumber = oInventory.RSN,

                PutawayAtLocationCode = oInventory.DisplayLocationCode,
                PutawayAtLocationID = oInventory.LocationID,
                PutawayAtContainerID = 0,
                PutawayAtSLoc = oInventory.StorageLocation,
                PutawayAtSLocID = oInventory.StorageLocationID,

                SourceBatch = oInventory.BatchNumber,
                DestinationBatch = oInventory.BatchNumber,
                IsLost = false,
                IsFound = true,
                TransferQuantity = oInventory.Quantity,
                IsPicking = (movementType.Equals(Constants.MovementType.Picking) ? 1 : 0),
                SuggestionID = oInventory.SuggestionID
            };

            return _oMovementFound;
        }

        public GoodsMovement GenerateInventoryMovementBinToBin(Inventory oInventory, int FromLocationID, int ToLocationID)
        {
            return GenerateInventoryMovement(oInventory, FromLocationID, ToLocationID, oInventory.ContainerID, oInventory.ContainerID, oInventory.StorageLocationID, oInventory.StorageLocationID, oInventory.ColorID, oInventory.ColorID);
        }

        public GoodsMovement GenerateInventoryMovementBinToBin(Inventory oInventory, string FromLocation, string ToLocation)
        {
            return GenerateInventoryMovement(oInventory, FromLocation, ToLocation, oInventory.ContainerCode, oInventory.ContainerCode, oInventory.StorageLocation, oInventory.StorageLocation, oInventory.Color, oInventory.Color);
        }
        
        public GoodsMovement GenerateInventoryMovement(Inventory oInventory, int FromLocationID, int ToLocationID, int FromContainerID, int ToContainerID)
        {
            return GenerateInventoryMovement(oInventory, FromLocationID, ToLocationID, FromContainerID, ToContainerID, oInventory.StorageLocationID, oInventory.StorageLocationID, oInventory.ColorID, oInventory.ColorID);
        }
        
        public GoodsMovement GenerateInventoryMovement(Inventory oInventory, string FromLocation, string ToLocation, string FromContainer, string ToContainer)
        {
            return GenerateInventoryMovement(oInventory, FromLocation, ToLocation, FromContainer, ToContainer, oInventory.StorageLocation, oInventory.StorageLocation, oInventory.Color, oInventory.Color);
        }

        public GoodsMovement GenerateInventoryMovement(Inventory oInventory, int FromLocationID, int ToLocationID, int FromContainerID, int ToContainerID, int FromSLocID, int ToSLocID, int FromColorID, int ToColorID)
        {
            GoodsMovement oMovement = new GoodsMovement()
            {
                GoodsMovementType = Constants.MovementType.InternalTransfer,


                OriginalReceiptMaterialCode = oInventory.OriginalReceiptMaterialCode,
                OriginalReceiptMaterialID = oInventory.OriginalReceiptMaterialID,

                TransactionDocumentNo = oInventory.ReferenceDocumentNumber,

                MaterialCode = oInventory.MaterialCode,
                MaterialMasterID = oInventory.MaterialMasterID,
                MaterialTransactionID = oInventory.MaterialTransactionID,
                RSNNumber = oInventory.RSN,

                PickedFromLocationID = FromLocationID,
                PickedContainerID = FromContainerID,
                PickedFromSLocID = FromSLocID,
                SourceBatch = oInventory.BatchNumber,

                PickedMRP = oInventory.MRP,
                PutawayMRP = oInventory.MRP,

                PutawayAtLocationID = ToLocationID,
                PutawayAtContainerID = ToContainerID,
                PutawayAtSLocID = ToSLocID,
                DestinationBatch = oInventory.BatchNumber,

                TransferQuantity = oInventory.Quantity
            };

            return oMovement;
        }
        
        public GoodsMovement GenerateInventoryMovement(Inventory oInventory, string FromLocation, string ToLocation, string FromContainer, string ToContainer, string FromSLoc, string ToSLoc, string FromColor, string ToColor)
        {
            GoodsMovement oMovement = new GoodsMovement()
            {
                GoodsMovementType = Constants.MovementType.InternalTransfer,

                OriginalReceiptMaterialCode = oInventory.OriginalReceiptMaterialCode,
                OriginalReceiptMaterialID = oInventory.OriginalReceiptMaterialID,

                TransactionDocumentNo = oInventory.ReferenceDocumentNumber,

                MaterialCode = oInventory.MaterialCode,
                MaterialMasterID = oInventory.MaterialMasterID,
                MaterialTransactionID = oInventory.MaterialTransactionID,
                RSNNumber = oInventory.RSN,

                PickedFromLocationCode = FromLocation,
                PickedContainerCode = FromContainer,
                PickedFromSLoc = FromSLoc,
                SourceBatch = oInventory.BatchNumber,

                PickedMRP = oInventory.MRP,
                PutawayMRP = oInventory.MRP,

                PutawayAtLocationCode = ToLocation,
                PutawayAtContainerCode = ToContainer,
                PutawayAtSLoc = ToSLoc,
                DestinationBatch = oInventory.BatchNumber,

                TransferQuantity = oInventory.Quantity,

                SuggestionID = oInventory.SuggestionID,

                IsLost = false,
                IsFound = false,
                IsDamaged = oInventory.IsDamaged

            };

            return oMovement;

        }
        
        public GoodsMovement GenerateInventoryMovement(Inventory oInventory, Constants.MovementType MovementType, bool AutoPickInventory,int ToLocationID, int ToContainerID, int ToSLocID)
        {

            GoodsMovement oMovement = new GoodsMovement()
            {
                GoodsMovementType = MovementType,

                OriginalReceiptMaterialCode = oInventory.OriginalReceiptMaterialCode,
                OriginalReceiptMaterialID = oInventory.OriginalReceiptMaterialID,

                TransactionDocumentNo = oInventory.ReferenceDocumentNumber,

                MaterialCode = oInventory.MaterialCode,
                MaterialMasterID = oInventory.MaterialMasterID,
                MaterialTransactionID = oInventory.MaterialTransactionID,
                RSNNumber = oInventory.RSN,

                AutoPickFromCurrentLocation = AutoPickInventory,

                PickedMRP = oInventory.MRP,
                PutawayMRP = oInventory.MRP,

                PutawayAtLocationID = ToLocationID,
                PutawayAtContainerID = ToContainerID,
                PutawayAtSLocID = ToSLocID,
                DestinationBatch = oInventory.BatchNumber,

                TransferQuantity = oInventory.Quantity
            };

            return oMovement;
        }
        
        public GoodsMovement GenerateMaterialDamaged(Inventory oInventory, string FromLocation, string ToLocation)
        {
            throw new NotImplementedException();
        }

        public GoodsMovement GenerateInventoryMovement(Inventory oInventory, Constants.MovementType MovementType, bool AutoPickInventory, string ToLocation, string ToContainer, string ToSLoc)
        {

            GoodsMovement oMovement = new GoodsMovement()
            {
                GoodsMovementType = MovementType,

                OriginalReceiptMaterialCode = oInventory.OriginalReceiptMaterialCode,
                OriginalReceiptMaterialID = oInventory.OriginalReceiptMaterialID,

                TransactionDocumentNo = oInventory.ReferenceDocumentNumber,

                MaterialCode = oInventory.MaterialCode,
                MaterialMasterID = oInventory.MaterialMasterID,
                MaterialTransactionID = oInventory.MaterialTransactionID,
                RSNNumber = oInventory.RSN,

                AutoPickFromCurrentLocation = AutoPickInventory,

                PickedMRP = oInventory.MRP,
                PutawayMRP = oInventory.MRP,

                PutawayAtLocationCode = ToLocation,
                PutawayAtContainerCode = ToContainer,
                PutawayAtSLoc = ToSLoc,
                DestinationBatch = oInventory.BatchNumber,

                TransferQuantity = oInventory.Quantity
            };

            return oMovement; 
        }

        public GoodsMovement GenerateInventoryMovementBatchToBatch(Inventory oInventory, string FromBatch, string ToBatch)
        {

            GoodsMovement oMovement = new GoodsMovement()
            {
                GoodsMovementType = Constants.MovementType.InternalTransfer,

                OriginalReceiptMaterialCode = oInventory.OriginalReceiptMaterialCode,
                OriginalReceiptMaterialID = oInventory.OriginalReceiptMaterialID,

                TransactionDocumentNo = oInventory.ReferenceDocumentNumber,

                MaterialCode = oInventory.MaterialCode,
                MaterialMasterID = oInventory.MaterialMasterID,
                MaterialTransactionID = oInventory.MaterialTransactionID,
                RSNNumber = oInventory.RSN,

                PickedFromLocationCode = oInventory.LocationCode,
                PickedContainerCode = oInventory.ContainerCode,
                PickedFromSLoc = oInventory.StorageLocation,
                SourceBatch = FromBatch,

                PickedMRP = oInventory.MRP,
                PutawayMRP = oInventory.MRP,
                
                PutawayAtLocationCode = oInventory.LocationCode,
                PutawayAtContainerCode = oInventory.ContainerCode,
                PutawayAtSLoc = oInventory.StorageLocation,
                DestinationBatch = ToBatch,

                TransferQuantity = oInventory.Quantity
            };

            return oMovement;
        }
        
        public GoodsMovement GenerateInventoryMovementSLocToSLoc(Inventory oInventory, int FromSLocID, int ToSLocID)
        {

            GoodsMovement oMovement = new GoodsMovement()
            {
                GoodsMovementType = Constants.MovementType.InternalTransfer,

                TransactionDocumentNo = oInventory.ReferenceDocumentNumber,

                OriginalReceiptMaterialCode = oInventory.OriginalReceiptMaterialCode,
                OriginalReceiptMaterialID = oInventory.OriginalReceiptMaterialID,

                MaterialCode = oInventory.MaterialCode,
                MaterialMasterID = oInventory.MaterialMasterID,
                MaterialTransactionID = oInventory.MaterialTransactionID,
                RSNNumber = oInventory.RSN,
                
                PickedFromLocationCode = oInventory.LocationCode,
                PickedContainerCode = oInventory.ContainerCode,
                PickedFromSLocID = FromSLocID,
                SourceBatch = oInventory.BatchNumber,

                PickedMRP = oInventory.MRP,
                PutawayMRP = oInventory.MRP,

                PutawayAtLocationCode = oInventory.LocationCode,
                PutawayAtContainerCode = oInventory.ContainerCode,
                PutawayAtSLocID = ToSLocID,
                DestinationBatch = oInventory.BatchNumber,

                TransferQuantity = oInventory.Quantity
            };

            return oMovement;
        }
        
        public GoodsMovement GenerateInventoryMovementSLocToSLoc(Inventory oInventory, string FromSLoc, string ToSLoc)
        {

            GoodsMovement oMovement = new GoodsMovement()
            {
                GoodsMovementType = Constants.MovementType.InternalTransfer,

                TransactionDocumentNo = oInventory.ReferenceDocumentNumber,

                OriginalReceiptMaterialCode = oInventory.OriginalReceiptMaterialCode,
                OriginalReceiptMaterialID = oInventory.OriginalReceiptMaterialID,

                MaterialCode = oInventory.MaterialCode,
                MaterialMasterID = oInventory.MaterialMasterID,
                MaterialTransactionID = oInventory.MaterialTransactionID,
                RSNNumber = oInventory.RSN,

                PickedFromLocationCode = oInventory.LocationCode,
                PickedContainerCode = oInventory.ContainerCode,
                PickedFromSLoc = FromSLoc,
                SourceBatch = oInventory.BatchNumber,

                PickedMRP = oInventory.MRP,
                PutawayMRP = oInventory.MRP,

                PutawayAtLocationCode = oInventory.LocationCode,
                PutawayAtContainerCode = oInventory.ContainerCode,
                PutawayAtSLoc = ToSLoc,
                DestinationBatch = oInventory.BatchNumber,

                TransferQuantity = oInventory.Quantity
            };

            return oMovement;
        }
        
        public GoodsMovement GenerateInventoryMovementMRPChange(Inventory oInventory, decimal OldMRP, decimal NewMRP)
        {

            GoodsMovement oMovement = new GoodsMovement()
            {
                GoodsMovementType = Constants.MovementType.InternalTransfer,

                TransactionDocumentNo = oInventory.ReferenceDocumentNumber,

                OriginalReceiptMaterialCode = oInventory.OriginalReceiptMaterialCode,
                OriginalReceiptMaterialID = oInventory.OriginalReceiptMaterialID,

                MaterialCode = oInventory.MaterialCode,
                MaterialMasterID = oInventory.MaterialMasterID,
                MaterialTransactionID = oInventory.MaterialTransactionID,
                RSNNumber = oInventory.RSN,

                PickedFromLocationCode = oInventory.LocationCode,
                PickedContainerCode = oInventory.ContainerCode,
                PickedFromSLoc = oInventory.StorageLocation,
                SourceBatch = oInventory.BatchNumber,

                PickedMRP = OldMRP,
                PutawayMRP = NewMRP,

                PutawayAtLocationCode = oInventory.LocationCode,
                PutawayAtContainerCode = oInventory.ContainerCode,
                PutawayAtSLoc = oInventory.StorageLocation,
                DestinationBatch = oInventory.BatchNumber,

                TransferQuantity = oInventory.Quantity
            };

            return oMovement;
        }

        public GoodsMovement GenerateInventoryMaterialDamaged(Inventory oInventory, int ToSLocID, string ToSLoc)
        {

            GoodsMovement oMovement = new GoodsMovement()
            {
                GoodsMovementType = Constants.MovementType.InternalTransfer,
                
                OriginalReceiptMaterialCode = oInventory.OriginalReceiptMaterialCode,
                OriginalReceiptMaterialID = oInventory.OriginalReceiptMaterialID,

                TransactionDocumentNo = oInventory.ReferenceDocumentNumber,

                MaterialCode = oInventory.MaterialCode,
                MaterialMasterID = oInventory.MaterialMasterID,
                MaterialTransactionID = oInventory.MaterialTransactionID,
                RSNNumber = oInventory.RSN,

                PickedFromLocationCode = oInventory.LocationCode,
                PickedContainerCode = oInventory.ContainerCode,
                PickedFromSLoc = oInventory.StorageLocation,
                SourceBatch = oInventory.BatchNumber,

                PickedMRP = oInventory.MRP,
                PutawayMRP = oInventory.MRP,

                PutawayAtLocationCode = oInventory.LocationCode,
                PutawayAtContainerCode = oInventory.ContainerCode,
                PutawayAtSLoc = ToSLoc,
                PutawayAtSLocID = ToSLocID,
                DestinationBatch = oInventory.BatchNumber,

                TransferQuantity = oInventory.Quantity
            };

            return oMovement;
        }


    }
}
