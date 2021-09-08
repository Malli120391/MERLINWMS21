using System;
using System.Collections.Generic;
using System.Text;

namespace MRLWMSC21Core.Library
{
    public static class Constants
    {
        public enum MovementType { Receive, Putaway, InternalTransfer, Picking, Loading, InventoryLost, InventoryFound, KittingPicking, KittingPutaway, InventoryConsolidation, DeNesting };

        public enum ShipmentType { InBound, OutBound };

        public enum LocationType { Bin, Dock, DeNesting, Staging, CrossDock, ExcessPicked, Quarantine, DockOrBin }

        public enum ApplicationAuthentication { InventraxSSO, DBAuthhentication }


        public enum TransferJobs
        {
            //FastMovingBinReplenishment = 1,
            //ExpiredMaterialMovement = 2,
            //MoveToQuarantineLocation = 3,
            BinToBinTransfer = 4,
            SLocToSLoc = 5,
            //KittingTransfers = 6,
            MRPChange = 7,
            //MigoToAST = 8,
            //ColorToColor = 9,
            SKUtoSKU = 10
        }


        public static int ParseInt(this string value, int defaultIntValue = 0)
        {
            int parsedInt;
            if (int.TryParse(value, out parsedInt))
            {
                return parsedInt;
            }

            return defaultIntValue;
        }

        /*
         
        1	1	Fast Moving Loc Fulfilling
        2	2	Expiry Material Movement
        3	3	QC Movement
        4	4	Bin To Bin
        5	5	SL to SL
        6	6	Kitting Transfers
        7	7	MRP Change
        8	8	Migo to AST
        9	9	Colour to Colour
        10	10	SKU to SKU

         */
    }
}
