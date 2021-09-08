using System;
using System.Collections.Generic;
using MRLWMSC21Core.Entities;
using MRLWMSC21Core.Library;
using MRLWMSC21Core.DataAccess;
using MRLWMSC21Core.Business.Interfaces;
using System.Linq;
using System.Text;
using MRLWMSC21Core.DataAccess.Utilities;

namespace MRLWMSC21Core.Business
{
    public class InboundBL : BaseBL, Interfaces.IInboundBL
    {
        private string _ClassCode = "WMSCore_BL_002_";
        private int _RecurranceCallCounter;

        private InboundDAL _oInboundDAL = null;

        public InboundBL(int LoginUser, string ConnectionString) : base(LoginUser, ConnectionString)
        {
            _oInboundDAL = new InboundDAL(LoginUser, ConnectionString);
            _RecurranceCallCounter = 0;
            _ClassCode = ExceptionHandling.GetClassExceptionCode(ExceptionHandling.ExcpConstants_API_BusinessLayer.InboundBL);
        }



        public List<Inbound> GetStoreRefNos(Inbound inbound)
        {
            try
            {
                List<Inbound> inbounds = new List<Inbound>();

                inbounds = _oInboundDAL.GetStoreRefNos(inbound);

                return inbounds;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public Location GeneratePalletGateSuggestion(SearchCriteria oSearchCriteria)

        {



            try

            {

                Location _oLocation;


                _oLocation = _oInboundDAL.GeneratePalletGateSuggestions(oSearchCriteria);
                
                return _oLocation;

            }
            catch (WMSExceptionMessage excp)
            {
                throw excp;
            }
            catch (Exception excp)

            {

                ExceptionData oExcpData = new ExceptionData();

                oExcpData.AddInputs("oSearchCriteria", oSearchCriteria);



                ExceptionHandling.LogException(excp, _ClassCode + "001", oExcpData);
                throw new WMSExceptionMessage() { WMSExceptionCode = ErrorMessages.WMSExceptionCode, WMSMessage = ErrorMessages.WMSExceptionMessage, ShowAsCriticalError = true };
            }



        }



        public List<Suggestion> GeneratePutawaySuggestion(SearchCriteria oCriteria)
        {

            try
            {


                List<Suggestion> _lSuggestions;

                _lSuggestions = _oInboundDAL.GeneratePutawaySuggestions(oCriteria);
                return _lSuggestions;
            }
            catch (WMSExceptionMessage excp)
            {
                throw excp;
            }
            catch (Exception excp)
            {
                ExceptionData oExcpData = new ExceptionData();
                oExcpData.AddInputs("oCriteria", oCriteria);

                ExceptionHandling.LogException(excp, _ClassCode + "002", oExcpData);
                throw new WMSExceptionMessage() { WMSExceptionCode = ErrorMessages.WMSExceptionCode, WMSMessage = ErrorMessages.WMSExceptionMessage, ShowAsCriticalError = true };
            }
        }

        public Suggestion FetchNextSuggestion(SearchCriteria oCriteria)
        {

            try
            {
                InboundDAL oInboundDAL = new InboundDAL(LoggedInUserID, ConnectionString);

                List<Suggestion> lSuggestion = oInboundDAL.FetchPutawaySuggestions(oCriteria, true);

                return lSuggestion[0];
            }
            catch (WMSExceptionMessage excp)
            {
                throw excp;
            }
            catch (Exception excp)
            {
                ExceptionData oExcpData = new ExceptionData();
                oExcpData.AddInputs("oCriteria", oCriteria);

                ExceptionHandling.LogException(excp, _ClassCode + "021", oExcpData);
                throw new WMSExceptionMessage() { WMSExceptionCode = ErrorMessages.WMSExceptionCode, WMSMessage = ErrorMessages.WMSExceptionMessage, ShowAsCriticalError = true };
            }
        }

         public List<Suggestion> FetchAllPutawaySuggestion(SearchCriteria oCriteria)
        {

            try
            {
                InboundDAL oInboundDAL = new InboundDAL(LoggedInUserID, ConnectionString);

                List<Suggestion> lSuggestion = oInboundDAL.FetchPutawaySuggestions(oCriteria, false);

                return lSuggestion;
            }
            catch (WMSExceptionMessage excp)
            {
                throw excp;
            }
            catch (Exception excp)
            {
                ExceptionData oExcpData = new ExceptionData();
                oExcpData.AddInputs("oCriteria", oCriteria);

                ExceptionHandling.LogException(excp, _ClassCode + "022", oExcpData);
                throw new WMSExceptionMessage() { WMSExceptionCode = ErrorMessages.WMSExceptionCode, WMSMessage = ErrorMessages.WMSExceptionMessage, ShowAsCriticalError = true };
            }
        }

        public List<Inbound> GetInboundsForUnloading(SearchCriteria oSearchCriteria)
        {

            try
            {
                InboundDAL oInboundDAL = new InboundDAL(LoggedInUserID, ConnectionString);

                List<Inbound> lInbounds = oInboundDAL.GetInboundListSearch(oSearchCriteria);

                return lInbounds;
            }
            catch (WMSExceptionMessage excp)
            {
                throw excp;
            }
            catch (Exception excp)
            {
                ExceptionData oExcpData = new ExceptionData();
                oExcpData.AddInputs("oSearchCriteria", oSearchCriteria);

                ExceptionHandling.LogException(excp, _ClassCode + "003", oExcpData);
                throw new WMSExceptionMessage() { WMSExceptionCode = ErrorMessages.WMSExceptionCode, WMSMessage = ErrorMessages.WMSExceptionMessage, ShowAsCriticalError = true };
            }
        }



        private List<GoodsMovement> PutawayInventory(List<GoodsMovement> lMovement)
        {
            /*
                  MCode

                  From Bin
                  To Bin

                From Pallet
                To Pallet

                From SLoc 
                To SLOC

                FromBatch
                ToBatch

                RSN Number

             */

            try
            {
                InventoryDAL _oInvenoryDAL = new InventoryDAL(LoggedInUserID, ConnectionString);
                
                List<GoodsMovement> _lMovementResponse = _oInvenoryDAL.MoveInventory(lMovement);
                if (_lMovementResponse != null)
                    return _lMovementResponse;
                else
                    throw new WMSExceptionMessage() { WMSExceptionCode = "" };
            }
            catch (WMSExceptionMessage excp)
            {
                throw excp;
            }
            catch (Exception excp)
            {
                ExceptionData oExcpData = new ExceptionData();
                oExcpData.AddInputs("lMovement", lMovement);

                ExceptionHandling.LogException(excp, _ClassCode + "004", oExcpData);

                throw new WMSExceptionMessage() { WMSExceptionCode = ErrorMessages.WMSExceptionCode, WMSMessage = ErrorMessages.WMSExceptionMessage, ShowAsCriticalError = true };
            }
        }

        public List<GoodsMovement> PutawayInventory(GoodsMovement oInventory)
        {

            try
            {
                InventoryDAL _oInventoryDAL = new InventoryDAL(LoggedInUserID, ConnectionString);
               
                List<Inventory> _lInventory = _oInventoryDAL.GetActiveStock(new SearchCriteria() { MaterialRSN = oInventory.RSNNumber });

                if (_lInventory == null)
                    throw new WMSExceptionMessage() { WMSExceptionCode = ErrorMessages.WMSExceptionCode, WMSMessage = ErrorMessages.WMSExceptionMessage, ShowAsCriticalError = true };
                else if (_lInventory.Count == 0)
                    throw new WMSExceptionMessage() { WMSExceptionCode = "", WMSMessage = "The RSN number is not Available for Putaway", ShowAsWarning = true };
                else if (_lInventory[0].ContainerID == 0 && this.ValidateLocationCode(new Location() { LocationCode = _lInventory[0].LocationCode }, Constants.LocationType.Bin,true) && (_lInventory[0].LocationID == oInventory.PutawayAtLocationID || _lInventory[0].LocationCode == oInventory.PutawayAtLocationCode || _lInventory[0].DisplayLocationCode == oInventory.PutawayAtLocationCode))
                    throw new WMSExceptionMessage() { WMSExceptionCode = "", WMSMessage = "The material with this RSN is already putaway.", ShowAsWarning = true };
                else
                {
                    oInventory.PickedMRP = _lInventory[0].MRP;
                    oInventory.PutawayMRP = _lInventory[0].MRP;
                    oInventory.PutawayAtSLoc = _lInventory[0].StorageLocation;
                    oInventory.PickedFromSLoc = _lInventory[0].StorageLocation;

                    oInventory.TransferQuantity = _lInventory[0].Quantity;
                    oInventory.IsPutaway = true;
                    
                    List<GoodsMovement> lInv = new List<GoodsMovement>() { oInventory };

                    List<GoodsMovement> _lMovementResponse = PutawayInventory(lInv);
                    
                    return _lMovementResponse;
                }
               
            }
            catch (WMSExceptionMessage excp)
            {
                throw excp;
            }
            catch (Exception excp)
            {
                ExceptionData oExcpData = new ExceptionData();
                oExcpData.AddInputs("oInventory", oInventory);

                ExceptionHandling.LogException(excp, _ClassCode + "005", oExcpData);
                throw new WMSExceptionMessage() { WMSMessage = ErrorMessages.WMSExceptionMessage, WMSExceptionCode = ErrorMessages.WMSExceptionCode, ShowAsCriticalError = true };
            }
        }

        public bool ValidateLocationCode(Location oLocation, Constants.LocationType cLocationType, bool DepricateExceptionsAndReturnFlags = false)
        {

            try
            {
                return ValidateLocation(oLocation, cLocationType, DepricateExceptionsAndReturnFlags);
                
            }
            catch (WMSExceptionMessage excp)
            {
                throw excp;
            }
            catch (Exception excp)
            {
                ExceptionData oExcpData = new ExceptionData();
                oExcpData.AddInputs("oLocation", oLocation);

                ExceptionHandling.LogException(excp, _ClassCode + "006", oExcpData);
                return false;
            }
        }


        public List<Inventory> ValidateLocationAndReturnInventory(Location oLocation, Constants.LocationType cLocationType, bool DepricateExceptionsAndReturnFlags = false)
        {

            try
            {
                List<Inventory> lInventory = new List<Inventory>();

                if (ValidateLocation(oLocation, cLocationType))
                {
                    InventoryDAL oInventoryDAL = new InventoryDAL(LoggedInUserID, ConnectionString);

                    // Fetch Inventory
                    return oInventoryDAL.GetActiveStock(new SearchCriteria() { LocationCode = oLocation.LocationCode });
                }
                else
                {
                    // Return Error
                    throw new WMSExceptionMessage() { WMSMessage = "Location Code in Invalid.",ShowAsError=true };
                }

                //return lInventory;

            }
            catch(WMSExceptionMessage excp)
            {
                throw excp;
            }
            catch (Exception excp)
            {
                ExceptionData oExcpData = new ExceptionData();
                oExcpData.AddInputs("oLocation", oLocation);

                ExceptionHandling.LogException(excp, _ClassCode + "007", oExcpData);
                return null;
            }
        }

        private bool ValidateLocation(Location oLocation, Constants.LocationType cLocationType, bool DepricateExceptionsAndReturnFlags = false)
        {

            LocationDAL oLocationDAL = new LocationDAL(LoggedInUserID, ConnectionString);

            SearchCriteria oCriteria = new SearchCriteria()
            {
                LocationCode = oLocation.LocationCode
            };

            List<Location> oLocations = oLocationDAL.GetLocations(oCriteria);


            if (!DepricateExceptionsAndReturnFlags)
            {
                if (oLocations == null || oLocations.Count <= 0)
                    return false;
                else if (oLocations.Count == 1 && oLocations[0].IsDockingArea == false && cLocationType == Constants.LocationType.Dock)
                    throw new WMSExceptionMessage() { WMSExceptionCode = "EMC_IB_BL_006", WMSMessage = ErrorMessages.EMC_IB_BL_006, ShowAsError = true };

                else if (oLocations.Count == 1 && oLocations[0].IsBinLocation == false && cLocationType == Constants.LocationType.Bin)
                    throw new WMSExceptionMessage() { WMSExceptionCode = "EMC_IB_BL_007", WMSMessage = ErrorMessages.EMC_IB_BL_007, ShowAsError = true };

                else if (oLocations.Count == 1 && oLocations[0].IsCrossDockingLocation == false && cLocationType == Constants.LocationType.CrossDock)
                    throw new WMSExceptionMessage() { WMSExceptionCode = "EMC_IB_BL_008", WMSMessage = ErrorMessages.EMC_IB_BL_008, ShowAsError = true };

                else if (oLocations.Count == 1 && oLocations[0].IsCrossDockingLocation == false && oLocations[0].IsDockingArea == false && oLocations[0].IsBinLocation == false && cLocationType == Constants.LocationType.DockOrBin)
                    throw new WMSExceptionMessage() { WMSExceptionCode = "EMC_IB_BL_019", WMSMessage = ErrorMessages.EMC_IB_BL_019, ShowAsError = true };

                else if (oLocations.Count == 1 && oLocations[0].IsQuarantine == false && cLocationType == Constants.LocationType.Quarantine)
                    throw new WMSExceptionMessage() { WMSExceptionCode = "EMC_IB_BL_009", WMSMessage = ErrorMessages.EMC_IB_BL_009, ShowAsError = true };

                else if (oLocations.Count == 1 && oLocations[0].IsExcessPickedStaging == false && cLocationType == Constants.LocationType.ExcessPicked)
                    throw new WMSExceptionMessage() { WMSExceptionCode = "EMC_IB_BL_010", WMSMessage = ErrorMessages.EMC_IB_BL_010, ShowAsError = true };

                else
                    return true;
            }
            else
            {

                if (oLocations == null || oLocations.Count <= 0)
                    return false;
                else if (oLocations.Count == 1 && oLocations[0].IsDockingArea == false && cLocationType == Constants.LocationType.Dock)
                    return false;

                else if (oLocations.Count == 1 && oLocations[0].IsBinLocation == false && cLocationType == Constants.LocationType.Bin)
                    return false;

                else if (oLocations.Count == 1 && oLocations[0].IsCrossDockingLocation == false && cLocationType == Constants.LocationType.CrossDock)
                    return false;

                else if (oLocations.Count == 1 && oLocations[0].IsCrossDockingLocation == false && oLocations[0].IsDockingArea == false && oLocations[0].IsBinLocation == false && cLocationType == Constants.LocationType.DockOrBin)
                    return false;

                else if (oLocations.Count == 1 && oLocations[0].IsQuarantine == false && cLocationType == Constants.LocationType.Quarantine)
                    return false;

                else if (oLocations.Count == 1 && oLocations[0].IsExcessPickedStaging == false && cLocationType == Constants.LocationType.ExcessPicked)
                    return false;

                else
                    return true;
            }
        }

        public Inventory ValidateRSNAndReceive(Inventory oInventory, bool IsDeNesting = false)
        {

            try
            {
                #region Method Steps => Developer Reference
                /*
                    STEPS TO CARRY OUT IN THIS PROCEDURE: 

                   1. Fetch Required Data 

                2. Validate Palletization Rules

                3. Validate if the material is already received. 
                    If yes, Update, else Receive.

                
                4. Control will flow further for Receive. 

                5. 
                 
                 */
                #endregion


                #region Fetch Master Data & Update the inventory entity
                
                oInventory.FetchMaterialDetailsFromRSN();

                MasterDAL oMasterDAL = new MasterDAL(LoggedInUserID, ConnectionString);

                Material _oMaterial = oMasterDAL.FetchMaterial(new SearchCriteria() { MaterialCode = oInventory.MaterialCode });
                if (_oMaterial == null)
                {
                    throw new WMSExceptionMessage() { WMSExceptionCode = ErrorMessages.WMSExceptionCode, WMSMessage = ErrorMessages.WMSExceptionMessage, ShowAsCriticalError = true };
                }
                else
                {
                    oInventory.Color = _oMaterial.ColorCode;
                    oInventory.ColorID = _oMaterial.ColorID;
                    oInventory.GenericMaterialID = _oMaterial.GenericSKUID;
                    oInventory.IsFinishedGoods = _oMaterial.IsFinishedGoods;
                    oInventory.IsRawMaterial = _oMaterial.IsRawMaterial;
                    oInventory.IsConsumables = _oMaterial.IsConsumables;
                    oInventory.Quantity = oInventory.Quantity == 0 ? _oMaterial.MOP : oInventory.Quantity;
                    oInventory.MOP =  _oMaterial.MOP ;

                }

                #endregion


                #region Fetch Other Data (For Location Validation, Pallet Validation, Active Stock, etc)

                Inventory oInventoryRequest = oInventory.DuplicateItem();
                Inventory oInventorySplitReceiveExcess = null;
                Inventory oInventorySplitReceiveIntoShipment = null;


                InboundDAL oInboundDAL = new InboundDAL(LoggedInUserID, ConnectionString);
                InventoryDAL oInventoryDAL = new InventoryDAL(base.LoggedInUserID, base.ConnectionString);

                Inbound oInbound = oInboundDAL.ValidateRSNAndFetchInbound(oInventory, out bool IsItemAlreadyReceived, out bool IsPalletizationPermitted);
                
                //List<Inventory> oPalletInventory = oInventoryDAL.GetActiveStock(new SearchCriteria() { ContainerCode = oInventory.ContainerCode,LocationCode= oInventory.LocationCode,LocationID=oInventory.LocationID });

                LocationDAL oLocationDAL = new LocationDAL(LoggedInUserID, ConnectionString);
                List<Location> lLocation = oLocationDAL.GetLocations(new SearchCriteria() { LocationCode = oInventory.LocationCode });

                #endregion


                #region Inbound Business Logic

                if (lLocation == null || oInbound == null)
                    throw new WMSExceptionMessage() { WMSMessage = ErrorMessages.WMSExceptionMessage, ShowAsCriticalError = true, WMSExceptionCode = ErrorMessages.WMSExceptionCode };
                else if ((lLocation.Count == 1 && (!lLocation[0].IsDockingArea && !IsDeNesting)) || lLocation.Count != 1)
                    throw new WMSExceptionMessage() { WMSExceptionCode = "EMC_IB_BL_006", WMSMessage = ErrorMessages.EMC_IB_BL_006, ShowAsError = true };
                else
                {
                    #region Palletization Check for Inventory being added into the pallet.

                   
                        if (!IsPalletizationPermitted && !IsDeNesting)
                            throw new WMSExceptionMessage() { WMSExceptionCode = "EMC_IB_BL_018", WMSMessage = ErrorMessages.EMC_IB_BL_018, ShowAsError = true };

                        #region OLD Palletization Logic => Replaced with above new Logic

                        /*
                            // Add Palletization Rules at this stage.
                            if (oInbound.PalletizationPreferences == null)
                                throw new WMSExceptionMessage() { WMSExceptionCode = ErrorMessages.WMSExceptionCode, WMSMessage = ErrorMessages.WMSExceptionMessage, ShowAsCriticalError = true };
                            foreach (PalletizationZoning _PZone in oInbound.PalletizationPreferences)
                            {
                                if (_PZone.MaterialCode.Equals(oInventory.MaterialCode))
                                {
                                    if (oPalletInventory.Count > 0)
                                    {
                                        if (_PZone.IsBySKU && _PZone.MaterialMasterID != oPalletInventory[0].MaterialMasterID)
                                            throw new WMSExceptionMessage() { WMSExceptionCode = "EMC_IB_BL_001", WMSMessage = ErrorMessages.EMC_IB_BL_001, ShowAsError = true };
                                        else if (_PZone.IsByRange && _PZone.MaterialRangeID != oPalletInventory[0].MaterialRangeID)
                                            throw new WMSExceptionMessage() { WMSExceptionCode = "EMC_IB_BL_002", WMSMessage = ErrorMessages.EMC_IB_BL_002, ShowAsError = true };
                                        else if (_PZone.IsByZone && _PZone.HomeZoneHeaderID != oPalletInventory[0].HomeZoneHeaderID)
                                            throw new WMSExceptionMessage() { WMSExceptionCode = "EMC_IB_BL_003", WMSMessage = ErrorMessages.EMC_IB_BL_003, ShowAsError = true };
                                        else if (_PZone.IsByLevel && _PZone.HomeLevelID != oPalletInventory[0].HomeLevelID)
                                            throw new WMSExceptionMessage() { WMSExceptionCode = "EMC_IB_BL_004", WMSMessage = ErrorMessages.EMC_IB_BL_004, ShowAsError = true };
                                    }
                                }
                            }
                        */

                        #endregion

                    #endregion


                    #region If Material is already received, update the quantites.

                    // Palletization Rules Cleared, so now do receiving checks.
                    List<Inventory> lInventory = new List<Inventory>();

                    decimal dInitialReceiptQuantity = 0;

                    bool _IsItemPresent = false;
                    bool _IsExcessInventory = false;
                    IList<Inventory> lInventoryFromDGM;

                    if (IsItemAlreadyReceived)
                    {
                        if (!oInventory.UserConfirmReDo)
                            throw new WMSExceptionMessage() { WMSExceptionCode = "EMC_IB_BL_011", WMSMessage = ErrorMessages.EMC_IB_BL_011, ShowUserConfirmDialogue = true };

                        lInventoryFromDGM = _oInboundDAL.FetchSKUInformationByDockGoodsMovement(oInventory);

                        foreach (Inventory inv in lInventoryFromDGM)
                        {
                            dInitialReceiptQuantity += inv.Quantity;
                        }
                        bool IsAddition = (dInitialReceiptQuantity > oInventory.Quantity) ? false : true;

                        decimal _dDifferenceQuantity = IsAddition ? oInventory.Quantity - dInitialReceiptQuantity : dInitialReceiptQuantity - oInventory.Quantity;

                        var orderbyresult = from dgm in lInventoryFromDGM
                                            orderby dgm.IsExcessInventory descending, dgm.PosoDetailsID descending
                                            select dgm;
                        List<Inventory> lInventoryToUpdate = new List<Inventory>();

                        // var result = lInventoryFromDGM.OrderBy
                        foreach (Inventory _InvItemInDGM in orderbyresult)
                        {
                            if (_dDifferenceQuantity > 0)
                            {
                                if (!oInventory.UserConfirmedExcessTransaction && _InvItemInDGM.IsExcessInventory)
                                    throw new WMSExceptionMessage() { WMSExceptionCode = "EMC_IB_BL_012", WMSMessage = ErrorMessages.EMC_IB_BL_012, ShowUserConfirmDialogue = true };

                                if (_InvItemInDGM.IsExcessInventory && IsAddition)
                                {
                                    // Excess PO Item Quantity Addition
                                    _InvItemInDGM.Quantity += _dDifferenceQuantity;
                                    _dDifferenceQuantity = 0;

                                    Inventory oInv = _InvItemInDGM.DuplicateItem();

                                    lInventoryToUpdate.Add(oInv);
                                }
                                else if (_InvItemInDGM.IsExcessInventory && !IsAddition)
                                {
                                    // Excess PO item quantity reduction
                                    Decimal _OriginalItemQuantity = _InvItemInDGM.Quantity;
                                    _InvItemInDGM.Quantity = (_InvItemInDGM.Quantity - _dDifferenceQuantity) > 0 ? (_InvItemInDGM.Quantity - _dDifferenceQuantity) : 0;
                                    _dDifferenceQuantity = _dDifferenceQuantity + _InvItemInDGM.Quantity - _OriginalItemQuantity;

                                    Inventory oInv = _InvItemInDGM.DuplicateItem();

                                    lInventoryToUpdate.Add(oInv);
                                }
                                else if(!_InvItemInDGM.IsExcessInventory && !IsAddition)
                                {
                                    // Not Excess PO item, quantity remove

                                    Decimal _OriginalItemQuantity = _InvItemInDGM.Quantity;
                                    _InvItemInDGM.Quantity = (_InvItemInDGM.Quantity - _dDifferenceQuantity) > 0 ? (_InvItemInDGM.Quantity - _dDifferenceQuantity) : 0;
                                    _dDifferenceQuantity = _dDifferenceQuantity + _InvItemInDGM.Quantity - _OriginalItemQuantity;

                                    Inventory oInv = _InvItemInDGM.DuplicateItem();

                                    lInventoryToUpdate.Add(oInv);

                                }
                                else if(!_InvItemInDGM.IsExcessInventory && IsAddition)
                                {
                                    // Not Excess PO Item, but Addition.
                                    break;
                                }
                            }
                        }

                        lInventoryToUpdate = oInventoryDAL.UpdateInventory(lInventoryToUpdate);

                    }

                    #endregion


                    #region Receive Inventory based on the Inbound Inventory MAP (PO/Invoice/Inbound combination) for the SKU matching the Map.
                    
                    foreach (InboundInventoryMap Item in oInbound.InboundInventoryMap)
                    {
                        if (oInventory.MaterialCode.Equals(Item.MaterialCode))
                        {
                            _IsItemPresent = true;
                            if (oInventory.Quantity + Item.ReceivedQuantity - dInitialReceiptQuantity > Item.ShipmentQuantity)
                            {
                                if (!oInventory.UserConfirmedExcessTransaction)
                                    throw new WMSExceptionMessage() { WMSExceptionCode = "EMC_IB_BL_012", WMSMessage = ErrorMessages.EMC_IB_BL_012, ShowUserConfirmDialogue = true };

                                if (!oInventory.IsFinishedGoods)
                                {
                                    decimal dExcessQuantity = oInventory.Quantity + Item.ReceivedQuantity - dInitialReceiptQuantity - Item.ShipmentQuantity;
                                    decimal dActualReceivedQuantity = oInventory.Quantity;

                                    oInventorySplitReceiveExcess = oInventory.DuplicateItem();
                                    oInventorySplitReceiveIntoShipment = oInventory.DuplicateItem();

                                    oInventorySplitReceiveExcess.Quantity = dExcessQuantity;
                                    oInventorySplitReceiveIntoShipment.Quantity = dActualReceivedQuantity - dExcessQuantity;

                                    if (oInventorySplitReceiveIntoShipment.Quantity > 0)
                                    {
                                      

                                        oInventorySplitReceiveIntoShipment.MovementType = Constants.MovementType.Receive;
                                        oInventorySplitReceiveIntoShipment.PosoDetailsID = Item.SupplierInvoiceDetailsID;
                                        oInventorySplitReceiveIntoShipment.MaterialMasterID = Item.MaterialMasterID;

                                        oInventorySplitReceiveIntoShipment.ColorID = Item.ColorID;
                                        oInventorySplitReceiveIntoShipment.MRP = Item.MRP;
                                        oInventorySplitReceiveIntoShipment.MOP = Item.MOP;

                                        if (!IsItemAlreadyReceived) // Receive Case
                                            oInventorySplitReceiveIntoShipment = oInventoryDAL.ReceiveInventory(oInventorySplitReceiveIntoShipment);
                                        else if (oInventorySplitReceiveIntoShipment.UserConfirmReDo)// Update Case
                                        {
                                            List<Inventory> _lTempInventory = new List<Inventory>();
                                            _lTempInventory.Add(oInventorySplitReceiveIntoShipment);
                                            _lTempInventory = oInventoryDAL.UpdateInventory(_lTempInventory);
                                        }
                                        else
                                            throw new WMSExceptionMessage() { WMSExceptionCode = "EMC_IB_BL_011", WMSMessage = ErrorMessages.EMC_IB_BL_011, ShowUserConfirmDialogue = true };

                                        if (oInventorySplitReceiveIntoShipment == null)
                                        {
                                            throw new WMSExceptionMessage() { WMSExceptionCode = "EMC_IB_BL_005", WMSMessage = ErrorMessages.EMC_IB_BL_005, ShowAsCriticalError = true };
                                        }
                                        else
                                        {
                                            oInventorySplitReceiveIntoShipment.IsReceived = true;
                                            oInventorySplitReceiveIntoShipment.DockGoodsMovementID = oInventorySplitReceiveIntoShipment.DockGoodsMovementID;
                                        }

                                    }

                                    oInventory = oInventorySplitReceiveExcess;
                                }
                                _IsExcessInventory = true;
                            }
                            else
                            {

                                oInventory.MovementType = Constants.MovementType.Receive;
                                oInventory.PosoDetailsID = Item.SupplierInvoiceDetailsID;
                                oInventory.MaterialMasterID = Item.MaterialMasterID;

                                oInventory.ColorID = Item.ColorID;
                                oInventory.MRP = Item.MRP;
                                oInventory.MOP = Item.MOP;


                                if (oInventory.IsFinishedGoods)
                                    oInventory.Quantity = Item.MOP;

                                Inventory oRetInventory;

                                if (!IsItemAlreadyReceived)
                                    oRetInventory = oInventoryDAL.ReceiveInventory(oInventory);
                                else if (oInventory.UserConfirmReDo)
                                {
                                    List<Inventory> lInventoryToUpdate = new List<Inventory>();
                                    lInventoryToUpdate.Add(oInventory);
                                    lInventoryToUpdate = oInventoryDAL.UpdateInventory(lInventoryToUpdate);
                                    oRetInventory = lInventoryToUpdate[0];
                                }
                                else
                                    throw new WMSExceptionMessage() { WMSExceptionCode = "EMC_IB_BL_011", WMSMessage = ErrorMessages.EMC_IB_BL_011, ShowUserConfirmDialogue = true };


                                if (oRetInventory == null)
                                {
                                    throw new WMSExceptionMessage() { WMSMessage = ErrorMessages.EMC_IB_BL_005, WMSExceptionCode = "EMC_IB_BL_005", ShowAsCriticalError = true };
                                }
                                else
                                {
                                    oInventory.IsReceived = true;
                                    oInventory.DockGoodsMovementID = oRetInventory.DockGoodsMovementID;
                                }

                                _IsExcessInventory = false;

                                break;
                            }
                        }

                    }

                    #endregion


                    #region Receive Inventory based on the Generic SKU ID (Material Range)

                    // BEGIN : BLock to check for the Material Generic SKU Codes, and Receive the material. 
                    if (!_IsItemPresent || _IsExcessInventory)
                    {
                        foreach (InboundInventoryMap oMap in oInbound.InboundInventoryMap)
                        {
                            // Check if the Generic SKU Code of the material Match, to Match if the Same Range item is a part of the PO.
                            if (oMap.GenericSKUID == oInventory.GenericMaterialID && oMap.GenericSKUID>0 && oInventory.GenericMaterialID>0)
                            {
                                // Check if the Received Quantity does not exceed the Shipment Document Quantity
                                if (oInventory.Quantity + oMap.ReceivedQuantity > oMap.ShipmentQuantity)
                                {
                                    _IsExcessInventory = true;
                                }
                                else
                                {
                                    oInventory.OriginalReceiptMaterialCode = oInventory.MaterialCode;
                                    oInventory.OriginalReceiptMaterialID = oInventory.MaterialMasterID;

                                    oInventory.MaterialCode = oMap.MaterialCode;
                                    oInventory.MaterialMasterID = oMap.MaterialMasterID;

                                    oInventory.MovementType = Constants.MovementType.Receive;
                                    oInventory.PosoDetailsID = oMap.SupplierInvoiceDetailsID;
                                    oInventory.MaterialMasterID = oMap.MaterialMasterID;

                                    oInventory.ColorID = oMap.ColorID;
                                    oInventory.MRP = oMap.MRP;
                                    oInventory.MOP = oMap.MOP;

                                    if (oInventory.IsFinishedGoods)
                                        oInventory.Quantity = oMap.MOP;

                                    Inventory _oTempInventory = null;
                                    _oTempInventory =  oInventoryDAL.ReceiveInventory(oInventory);

                                    if (_oTempInventory != null)
                                    {
                                        oInventory.IsExcessInventory = false;
                                        oInventory.IsReceived = true;
                                        oInventory.DockGoodsMovementID = _oTempInventory.DockGoodsMovementID;

                                        // This case when the inventory is split into multiple parts and received into the shipment and as excess.
                                        if (oInventorySplitReceiveIntoShipment != null)
                                        {
                                            oInventory.Quantity += oInventorySplitReceiveIntoShipment.Quantity;
                                        }
                                        
                                        return oInventory;
                                    }
                                    else
                                    {
                                        throw new WMSExceptionMessage() { WMSExceptionCode = "EMC_IB_BL_005", WMSMessage = ErrorMessages.EMC_IB_BL_005, ShowAsCriticalError = true };
                                    }
                                }
                            }
                        }
                    }
                    // END : BLock to check for the Material Generic SKU Codes, and Receive the material. 

                    #endregion


                    #region Receive Excess Inventory Case

                    // BEGIN : BLock to receive Excess Material.
                    if (_IsExcessInventory || !_IsItemPresent)
                    {
                        if (_RecurranceCallCounter++ <= 1)
                        {
                            Inventory _oTempInventory = null;
                            if (oInboundDAL.ModifyPurchaseDocumentForExcessQuantity(oInventory))
                                _oTempInventory = ValidateRSNAndReceive(oInventory);

                            if (_oTempInventory != null)
                            {
                                oInventory.IsExcessInventory = true;
                                oInventory.IsReceived = true;
                                oInventory.DockGoodsMovementID = _oTempInventory.DockGoodsMovementID;
                            }
                            else
                            {
                                throw new WMSExceptionMessage() { WMSExceptionCode = "EMC_IB_BL_005", WMSMessage = ErrorMessages.EMC_IB_BL_005, ShowAsCriticalError = true };
                            }
                        }
                        // throw new WMSExceptionMessage() { WMSMessage = "This material has been received as Excess.", ShowAsWarning=true };
                    }
                    // END : BLock to receive Excess Material.

                    #endregion

                }

                #endregion

                return oInventory;
            }
            catch (WMSExceptionMessage excp)
            {
                throw excp;
            }
            catch (Exception excp)
            {
                ExceptionData oExcpData = new ExceptionData();
                oExcpData.AddInputs("oInventory", oInventory);
                
                ExceptionHandling.LogException(excp, _ClassCode + "008", oExcpData);
                throw new WMSExceptionMessage() { WMSExceptionCode = ErrorMessages.WMSExceptionCode, WMSMessage = ErrorMessages.WMSExceptionMessage, ShowAsCriticalError = true };
            }
        }


        public List<Vehicle> GetVehiclesForUnloading(SearchCriteria oSearchCriteria)
        {

            try
            {
                InboundDAL oInboundDAL = new InboundDAL(LoggedInUserID, ConnectionString);

                List<Vehicle> lVehicles = oInboundDAL.GetVehiclesList(oSearchCriteria);

                return lVehicles;
            }
            catch (WMSExceptionMessage excp)
            {
                throw excp;
            }
            catch (Exception excp)
            {
                ExceptionData oExcpData = new ExceptionData();
                oExcpData.AddInputs("oSearchCriteria", oSearchCriteria);

                ExceptionHandling.LogException(excp, _ClassCode + "009", oExcpData);
                throw new WMSExceptionMessage() { WMSExceptionCode = ErrorMessages.WMSExceptionCode, WMSMessage = ErrorMessages.WMSExceptionMessage, ShowAsCriticalError = true };
            }
        }

        public bool ValidatePalletCode(Pallet oPallet)
        {
            return ValidatePallet(oPallet);
        }

        public List<Inventory> ValidatePalletAndReturnInventory(Pallet oPallet)
        {

            if (ValidatePallet(oPallet))
            {
                InventoryDAL oInventoryDAL = new InventoryDAL(LoggedInUserID, ConnectionString);

                // Fetch Inventory
                return oInventoryDAL.GetActiveStock(new SearchCriteria() { ContainerCode = oPallet.PalletCode });
            }
            else
            {
                // Return Error
                throw new WMSExceptionMessage() { WMSMessage = "Invalid Pallet Code.",ShowAsError=true };
            }
        }
        
        private bool ValidatePallet(Pallet oPallet)
        {

            PalletDAL oPalletDAL = new PalletDAL(LoggedInUserID, ConnectionString);

            SearchCriteria oCriteria = new SearchCriteria()
            {
                ContainerCode = oPallet.PalletCode
            };

            List<Pallet> oPallets = oPalletDAL.GetPallets(oCriteria);


            if (oPallets == null)
                return false;
            else
            {
                if (oPallets.Count > 0)
                    return true;
                else
                    return false;
            }
        }
        
       public List<Suggestion> MarkBinFull(Location oLocation, Pallet oPallet)
        {

            try
            {
                /*
                 
                STEPS TO ACCOMPLISH:

                    1. If the inventory at the location is not putaway and the bin is marked full 
                        then Flag the BIN Location for a Cyclecount.
                    2. Once done, re-generate the suggestions for the inventory left over and return the new location to putaway. 

                 */
                CycleCountBL oCycleCountBL = new CycleCountBL(LoggedInUserID, ConnectionString);


                if (_oInboundDAL.MarkBinFull(oLocation) && oCycleCountBL.FlagLocationForCycleCount(new Location() { LocationCode = oLocation.LocationCode }))
                {
                    // Case when BIN is marked as FULL.

                    InventoryDAL oInventoryDAL = new InventoryDAL(LoggedInUserID, ConnectionString);

                    List<Inventory> lPalletInventory = oInventoryDAL.GetActiveStock(new SearchCriteria() { ContainerCode = oPallet.PalletCode });

                    if (lPalletInventory.Count > 0)
                        return _oInboundDAL.GeneratePutawaySuggestions(new SearchCriteria() { ContainerCode = oPallet.PalletCode, ReasonID = 1 });
                    else
                        return new List<Suggestion>();
                }
                else
                    throw new WMSExceptionMessage() { WMSExceptionCode = ErrorMessages.WMSExceptionCode, WMSMessage = ErrorMessages.WMSExceptionMessage, ShowAsCriticalError = true };
                
            }
            catch (WMSExceptionMessage excp)
            {
                throw excp;
            }
            catch (Exception excp)
            {
                ExceptionData oExcpData = new ExceptionData();
                oExcpData.AddInputs("oLocation", oLocation);
                oExcpData.AddInputs("oPallet", oPallet);

                ExceptionHandling.LogException(excp, _ClassCode + "010", oExcpData);
                throw new WMSExceptionMessage() { WMSExceptionCode = ErrorMessages.WMSExceptionCode, WMSMessage = ErrorMessages.WMSExceptionMessage, ShowAsCriticalError = true };
            }
        }

        public bool PalletPutawayComplete(Pallet oPallet)
        {

            try
            {
                List<Inventory> lPalletInventory = null;
                if (oPallet != null)
                {
                    InventoryDAL oInventoryDAL = new InventoryDAL(LoggedInUserID, ConnectionString);
                    InventoryUtility oInventoryUtility = new InventoryUtility();
                    // Fetch Inventory
                    lPalletInventory = oInventoryDAL.GetActiveStock(new SearchCriteria() { ContainerCode = oPallet.PalletCode });

                    if (lPalletInventory.Count == 0) // Check if there is no more stock in the pallet, then return true.
                        return true;
                    else
                    {
                        // If there is stock left and the user has clicked on Pallet Complete, mark the inventory in the pallet as Lost. 
                        List<GoodsMovement> _lGoodsMovement = new List<GoodsMovement>();

                        //foreach (Inventory _oPalletInventory in lPalletInventory)
                        //{
                        //    GoodsMovement _oItemLost = new GoodsMovement();

                        //    _oItemLost.GoodsMovementType = Constants.MovementType.InventoryLost;
                        //    _oItemLost.MaterialMasterID = _oPalletInventory.MaterialMasterID;
                        //    _oItemLost.MaterialTransactionID = _oPalletInventory.MaterialTransactionID;
                        //    _oItemLost.RSNNumber = _oPalletInventory.RSN;

                        //    _oItemLost.PickedContainerCode = oPallet.PalletCode;
                        //    _oItemLost.PickedContainerID = oPallet.PalletID;
                        //    _oItemLost.AutoPickFromCurrentLocation = true;

                        //    _lGoodsMovement.Add(_oItemLost);



                        //}
                        foreach (Inventory _oPalletInventory in lPalletInventory)
                            if (!_oPalletInventory.IsLost)
                                _lGoodsMovement.Add(oInventoryUtility.GenerateInventoryLost(_oPalletInventory,Constants.MovementType.Putaway));

                        InventoryDAL _oInventoryDAL = new InventoryDAL(LoggedInUserID, ConnectionString);

                        List<GoodsMovement> _lMovementResponse = _oInventoryDAL.MoveInventory(_lGoodsMovement);

                        if (_lMovementResponse.Count == _lGoodsMovement.Count)
                            return true;
                        else
                            throw new WMSExceptionMessage() { WMSExceptionCode = ErrorMessages.WMSExceptionCode, WMSMessage = ErrorMessages.WMSExceptionMessage };

                    }
                }
                else
                    return false;
            }
            catch (WMSExceptionMessage excp)
            {
                throw excp;
            }
            catch (Exception excp)
            {
                ExceptionData oExcpData = new ExceptionData();
                oExcpData.AddInputs("oPallet", oPallet);

                ExceptionHandling.LogException(excp, _ClassCode + "011", oExcpData);
                throw new WMSExceptionMessage() { WMSExceptionCode = ErrorMessages.WMSExceptionCode, WMSMessage = ErrorMessages.WMSExceptionMessage, ShowAsCriticalError = true };
            }
        }

        public Inventory UpdateInventory(Inventory oInventory)
        {

            try
            {
                InventoryDAL _oInventoryDAL = new InventoryDAL(LoggedInUserID, ConnectionString);
                List<Inventory> _lInvenotryToUpdate = new List<Inventory>();
                _lInvenotryToUpdate.Add(oInventory);
                _lInvenotryToUpdate = _oInventoryDAL.UpdateInventory(_lInvenotryToUpdate);
                oInventory = _lInvenotryToUpdate[0];
                return oInventory;
            }
            catch (WMSExceptionMessage excp)
            {
                throw excp;
            }
            catch (Exception excp)
            {
                ExceptionData oExcpData = new ExceptionData();
                oExcpData.AddInputs("oInventory", oInventory);

                ExceptionHandling.LogException(excp, _ClassCode + "012", oExcpData);
                throw new WMSExceptionMessage() { WMSExceptionCode = ErrorMessages.WMSExceptionCode, WMSMessage = ErrorMessages.WMSExceptionMessage, ShowAsCriticalError = true };
            }
            //throw new NotImplementedException();
        }
        public string Palletrules(Inventory inboundDTO)
        {

            try
            {
                _oInboundDAL.PalletRules(inboundDTO);
                return null;
            }
            catch (WMSExceptionMessage excp)
            {
                throw excp;
            }
            catch (Exception ex)
            {
                ExceptionData exceptionData = new ExceptionData();
                exceptionData.AddInputs("inboundDTO", inboundDTO);

                ExceptionHandling.LogException(ex, _ClassCode + "005", exceptionData);

                throw new WMSExceptionMessage() { WMSExceptionCode = ErrorMessages.WMSExceptionCode, WMSMessage = ErrorMessages.WMSExceptionMessage, ShowAsCriticalError = true };
            }
        }

    }
}
