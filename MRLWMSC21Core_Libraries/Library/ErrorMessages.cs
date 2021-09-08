using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MRLWMSC21Core.Library
{
    public static class ErrorMessages
    {
        public static string WMSExceptionMessage = "An Internal Error has Occoured";
        public static string WMSExceptionCode = "EXCP01";

        // Below is used when the Item (or) SKU Information is being updated at receipt, and the RSN Number is not available.
        public static string EMC0001 = "An Internal Error has occoured. Kindly contact Inventrax Support team for assistance.";

        public static string EMC_User_DAL_0001 = "Please enter valid credentails.";

        public static string EMC_IB_DAL_0002 = "The Inbound you have selected is not Valid.";

        public static string EMC_IB_DAL_0003 = "You cannot receive this material as the RSN is received as part of another shipment.";

        public static string EMC_IB_BL_001 = "Multiple SKUs are not allowed in the Pallet.";

        public static string EMC_IB_BL_002 = "SKUs of the same Range need to be Palletized.";

        public static string EMC_IB_BL_003 = "SKUs of the same Zone need to be Palletized.";

        public static string EMC_IB_BL_004 = "SKUs of the same Level need to be Palletized.";

        public static string EMC_IB_BL_005 = "An error has occoured in Receiving the Inventory.";

        public static string EMC_IB_BL_006 = "Please select a valid dock location for Receiving Inventory.";

        public static string EMC_IB_BL_007 = "Please select a valid Bin location.";

        public static string EMC_IB_BL_008 = "Please select a valid cross dock location.";

        public static string EMC_IB_BL_009 = "Please select a valid quarantine location.";

        public static string EMC_IB_BL_010 = "Please select a valid excess picked staging location.";

        public static string EMC_IB_BL_011 = "The Material with this RSN is already received. Please confirm if you want to update.";

        public static string EMC_IB_BL_012 = "This SKU will be received as Excess. Please confirm to receive.";

        // Palletization Method Messages
        public static string EMC_IB_BL_013 = "Passing Container Code is mandatory.";

        public static string EMC_IB_BL_014 = "Passing Material Code is mandatory.";

        public static string EMC_IB_BL_015 = "Passing Material RSN is mandatory.";

        public static string EMC_IB_BL_016 = "Passing Store Reference Number is mandatory.";

        public static string EMC_IB_BL_017 = "Passing RSN Quantity is mandatory.";

        public static string EMC_IB_BL_018 = "This material cannot be palletized in this Pallet. Kindly use a new Pallet.";

        public static string EMC_IB_BL_019 = "Please select a valid Bin (or) Dock Location.";


        public static string EMC_IB_DAL_001 = "Passing the container code is Mandatory.";

        public static string EMC_IB_DAL_002 = "Passing Material RSN is Mandatory.";

        public static string EMC_IB_DAL_003 = "Passing Material Code is Mandatory.";

        public static string EMC_IB_DAL_004 = "Passing RSN Number is Mandatory.";

        public static string EMC_IB_DAL_005 = "Gate Suggestions could not be generated.";

        public static string EMC_IB_DAL_006 = "The Inbound reference number you have passed is Invalid.";

        public static string EMC_IN_DAL_001 = "No Stock available.";


        public static string EMC_OB_BL_001 = "The inventory in the picklist has been picked.";

        public static string EMC_OB_BL_002 = "Material with this RSN does not exists.";

        public static string EMC_OB_BL_003 = "The SKU cannot be loaded as the load quantity for this SKU has exceed.";

        public static string EMC_OB_BL_004 = "The SKU you are trying to load is not a part of this Loadsheet.";

        public static string EMC_OB_BL_005 = "The SKU you are trying to load is not part of the Picklist.";

        public static string EMC_OB_BL_006 = "The RSN number is Mandatory.";

        public static string EMC_OB_BL_007 = "Please scan a valid RSN Number.";

        public static string EMC_OB_BL_008 = "You are about to Revert loading for the FG with Serial Number [RSN].";

        public static string EMC_OB_BL_009 = "You are about to Revert loading for the Raw Material / Consumables with Serial Number [RSN]. Please confirm the quantity remaining in the Vehicle.";


        public static string EMC_OB_BL_010 = "You are not permitted to dispatch Materials with an old MRP. Perform MRP change operation first.";

        public static string EMC_OB_BL_011 = "You are about to dispatch a Material with an OLD MRP. Please confirm if you wish to proceed.";

        public static string EMC_OB_BL_012 = "You are not permitted to dispatch Nested Materials.";

        public static string EMC_OB_BL_013 = "The material has been marked as Damaged and hence cannot be dispatched.";

        public static string EMC_OB_BL_014 = "The item with Serial Number - [RSN] is already picked.";

        public static string EMC_OB_BL_015 = "This Material is not a part of the Picklist.";

        public static string EMC_OB_BL_016 = "You cannot pick more than [QTY] quantity as this RSN has only [QTY] quantity in stock.";

        public static string EMC_OB_BL_017 = "You cannot pick more than [QTY] Nos. you are exceeding the Picklist Quantity.";

        public static string EMC_OB_BL_018 = "Loading reference number is not passed.";

        public static string EMC_OB_BL_019 = "You need to pick the SKU as Suggested.";

        public static string EMC_OB_BL_020 = "Please scan valid RSN";

        public static string EMC_OB_DAL_001 = "Document Reference number is not being passed to Generate the Suggestions.";

        public static string EMC_OB_DAL_002 = "The RSN number you have scanned is not a part of this Load.";

        public static string EMC_OB_DAL_003 = "This RSN  is already loaded.";

        public static string EMC_OB_DAL_004 = "Invalid RSN";

        public static string EMC_OB_DAL_005 = "No Pick List Configured";

        public static string EMC_OB_DAL_006 = "invalid Load Ref#";

        public static string EMC_OB_DAL_007 = "Loaded Successfully.";

        public static string EMC_OB_DAL_008 = "Cannot load more than Required Qty.";

        public static string EMC_OB_DAL_009 = "The Material is in Lost & Found Location.";

        public static string EMC_OB_DAL_010 = "Please pick all Items without Carton, since first item is picked without carton";

        public static string EMC_OB_DAL_011= "Please pick all Items with Carton, earlier picked with carton";


        public static string EMC_OB_DAL_PICKSugg_EC01 = "There are no materials in the pick list.";

        public static string EMC_OB_DAL_PICKSugg_EC02 = "Inventory is unavailable for this material.";

        public static string EMC_OB_DAL_PICKSugg_EC03 = "Partial fulfilment of Inventory due to partial availability.";

        public static string EMC_OB_DAL_PICKSugg_EC04 = "Partial / No allocation for the Kit Selected.";


        public static string WMC_GEN_CTRLR_001 = "The request authentication has failed. If this issue persists very freequently, Contact Inventrax Support Team.";

        public static string WMC_DENEST_BL_0001 = "The DeNesting Job order you have selected is Invalid.";

        public static string WMC_DENEST_BL_0002 = "Location is not being passed as part of the Inventory Object.";

        public static string WMC_DENEST_BL_0003 = "The Locaion you have scanned is Invalid.";

        public static string WMC_DENEST_BL_0004 = "The Material you have scanned is Invalid.";

        public static string WMC_DENEST_BL_0005 = "The material could not be consumed for De-Nesting.";

        public static string WMC_DENEST_BL_0006 = "Material with this RSN is already received.";

        public static string WMC_DENEST_BL_0007 = "You cannot receive any more Inventory as the entire DeNested stock is received into WMS.";

        public static string WMC_DENEST_BL_0008 = "You cannot consume this item as you are exceeding the Job Order Quantity for DeNesting.";

        public static string WMC_DENEST_BL_0009 = "The material could not be received.";

        public static string WMC_DENEST_BL_0010 = "The mateiral you are trying to DeNest is not a part of this DeNesting Job.";

        public static string WMC_DENEST_BL_0013 = "You cannot receive this Material as the corresponding Nested SKU is not scanned.";

        public static string WMC_DENEST_BL_0014 = "You cannot receive this Material as it is not a part of this DeNesting Job.";

        // Do not change the [Batch] & [SLOC] Template in the below strings as they are used in the application to pass the valid BAtch No & SLOC for Picking. 
        // These can however be removed.
        public static string WMC_DENEST_BL_0011 = "You cannot pick a Material from [Batch] Batch only.";

        public static string WMC_DENEST_BL_0012 = "You need to pick a Material from [SLOC] Storage Location only.";

        public static string WMC_DENEST_BL_0015 = "This material is is already received.";

        public static string WMC_DENEST_BL_0016 = "An OLD MRP Material has been used in DeNesting. Please reconfirm if the MRP is changed.";

        public static string WMC_DENEST_DAL_0001 = "There is no stock available for this SKU.";

        public static string WMC_DENEST_CTRLR_0001 = "INFO: OutboundID needs to be passed as part of the Job Order Information.";

        public static string WMC_DENEST_CTRLR_0002 = "INFO: SODetailsID needs to be passed as part of the Job Order Information.";

        public static string WMC_DENEST_CTRLR_0003 = "INFO: LocationID needs to be passed as part of the Inventory Information.";

        public static string WMC_DENEST_CTRLR_0004 = "INFO: MaterialMaster_UOMId needs to be passed as part of the Job Order Information.";

        public static string WMC_DENEST_CTRLR_0005 = "INFO: MaterialMasterID needs to be passed as part of the Job Order Information.";

        public static string WMC_DENEST_CTRLR_0006 = "INFO: RSN Number needs to be passed as part of the Inventory Information.";

        public static string WMC_DENEST_CTRLR_0007 = "INFO: Storage Location ID needs to be passed as part of the Inventory Information.";

        public static string WMC_DENEST_CTRLR_0008 = "INFO: Batch Number needs to be passed as part of the Inventory Information.";




        public static string WMC_HK_BL_0001 = "Passing OLD MRP of the Item is Mandatory.";

        public static string WMC_HK_BL_0002 = "Passing New MRP of the Item is Mandatory.";

        public static string WMC_HK_BL_0003 = "Passing Inventory object is Mandatory.";

        public static string WMC_HK_BL_0004 = "Job Order Number is Mandatory for MRP Changed based on Job Order.";


        public static string WMC_HK_BL_0005 = EMC_OB_BL_002;

        public static string WMC_HK_BL_0006 = "The MRP of the item is upto date.";

        public static string WMC_HK_BL_007 = "The Cycle Count reference is Invalid.";

        public static string WMC_HK_BL_008 = "Passing Inventory entity is Mandatory.";

        public static string WMC_HK_BL_009 = WMC_BE_DEF_INV_0002;

        public static string WMC_HK_BL_010 = "Passing From Location for BIN to BIN Transfer is mandatory.";

        public static string WMC_HK_BL_011 = "Passing To Location for BIN to BIN Transfer is mandatory.";

        public static string WMC_HK_BL_012 = EMC_OB_DAL_004;
        
        public static string WMC_HK_BL_0013 = EMC_OB_DAL_004;

        public static string WMC_HK_BL_014 = "Passing From Storage Location for BIN to BIN Transfer is mandatory.";

        public static string WMC_HK_BL_015 = "Passing To Storage Location for BIN to BIN Transfer is mandatory.";

        public static string WMC_HK_BL_016 = "No Stock available.";

        public static string WMC_CC_BL_0001 = "There are no more locations to carry out cycle count.";

        public static string WMC_CC_BL_0002 = "Please pass a Location.";

        public static string WMC_CC_BL_0003 = "Please pass Cycle Count reference to Block the location.";

        public static string WMC_CC_BL_0004 = "The location you are trying to scan is not a Valid Bin Location.";

        public static string WMC_CC_BL_0005 = "The Cycle Count for this location has already been carried out by another User.";

        public static string WMC_CC_BL_0006 = "Inventory entity is not initialized.";

        public static string WMC_CC_BL_0007 = "Material with this RSN is already scanned.";

        public static string WMC_CC_DAL_0001 = "The Location you have scaaned is not a part of this Cycle count.";

        public static string WMC_CC_BL_0008 = "This location is not mapped for the cycle count.";

        public static string WMC_CC_BL_0009 = "This material has already been received. Please confirm if you want to Update Information.";



        public static string WMC_BE_DEF_INV_0001 = "The Format of the RSN Number is Invalid.";

        public static string WMC_BE_DEF_INV_0002 = "RSN Number cannot be empty.";

        public static string WMC_CNTRL_LOGIN_0001 = "Not a valid credentails";


        public static string WMC_DAL_SCAN_0001 = "Invalid Carton scanned";
        public static string WMC_DAL_SCAN_0002 = "Invalid Location scanned";
        public static string WMC_DAL_SCAN_0003 = "Invalid Material scanned";
        public static string WMC_DAL_SCAN_0004 = "Scanned Location is Blocked For Cycle Count";

        public static string WMC_DAL_INV_0001 = "Invalid Carton scanned";
        public static string WMC_DAL_INV_0002 = "Invalid Location scanned";
        public static string WMC_DAL_INV_0003 = "Cannot transfer to same location";
        public static string WMC_DAL_INV_0004 = "No stock available in source pallet";
        public static string WMC_DAL_INV_0005 = "Exception while transfer stock";
        public static string WMC_DAL_INV_0006 = "Stock reserved cannot transfer";
        public static string WMC_DAL_INV_0007 = "Unexpected result";

    }
}
