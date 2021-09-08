using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MRLWMSC21Core.Business;
using MRLWMSC21Core.Business.Interfaces;
using MRLWMSC21Core.Entities;
using MRLWMSC21Core.DataAccess;
using MRLWMSC21Core.Library;
using MRLWMSC21Core.DataAccess.Utilities;


namespace MRLWMSC21Core.Business
{
    public class OutboundBL : BaseBL, Interfaces.IOutboundBL
    {
        private string _ClassCode = string.Empty;

        private OutboundDAL _oOutboundDAL = null;

        public OutboundBL(int LoginUser, string ConnectionString) : base(LoginUser, ConnectionString)
        {
            _oOutboundDAL = new OutboundDAL(LoginUser, ConnectionString);
            _ClassCode = ExceptionHandling.GetClassExceptionCode(ExceptionHandling.ExcpConstants_API_BusinessLayer.OutboundBL);
        }

        ~OutboundBL()
        {
            _oOutboundDAL = null;
        }

        public Inventory ConfirmLoading(Inventory oInventory)
        {

            try
            {
                if (oInventory.ReferenceDocumentNumber == null)
                    throw new WMSExceptionMessage() { WMSExceptionCode = "EMC_OB_BL_018", WMSMessage = ErrorMessages.EMC_OB_BL_018, ShowAsError = true };
                
                List<Inventory> lPickListInventory = _oOutboundDAL.FetchPickListInventoryForLoad(oInventory);
                List<LoadItem> lLoadItem = _oOutboundDAL.FetchInventoryForLoadSheet(new LoadSheet() { LoadRefNo = oInventory.ReferenceDocumentNumber });
                PickingPreferences _oPreference = _oOutboundDAL.FetchPickingPreferences(new SearchCriteria());


                bool bItemPartOfPickList = false;
                bool bItemPartOfLoadsheet = false;

                if (lPickListInventory == null)
                    throw new WMSExceptionMessage() { };
                else
                {
                    foreach (LoadItem _Item in lLoadItem)
                    {
                        if (oInventory.MaterialCode.Equals(_Item.MCode))
                        {


                            //if (_Item.LoadedQuantity + oInventory.Quantity > _Item.LoadQuantity)
                            //    throw new WMSExceptionMessage() { WMSExceptionCode = "EMC_OB_BL_003", WMSMessage = ErrorMessages.EMC_OB_BL_003, ShowAsWarning = true };
                            //else
                            //{
                                foreach (Inventory _oInv in lPickListInventory)
                                {
                                    if (oInventory.MaterialCode.Equals(_oInv.MaterialCode))
                                    {
                                        bItemPartOfPickList = true;
                                        if (!_oPreference.AllowDispatchOfOLDMRP && !_Item.ActiveMRP.Equals(_oInv.MRP)) // Item is of OLD MRP, show message that nested item cannot be dispatched. 
                                            throw new WMSExceptionMessage() { WMSExceptionCode = "EMC_OB_BL_010", WMSMessage = ErrorMessages.EMC_OB_BL_010, ShowAsError = true };
                                        //else if (!_Item.ActiveMRP.Equals(_oInv.MRP)) // Item is of an OLD MRP, show Confirm dialogue.
                                        //    throw new WMSExceptionMessage() { WMSExceptionCode = "EMC_OB_BL_011", WMSMessage = ErrorMessages.EMC_OB_BL_011, ShowUserConfirmDialogue = true };
                                        else if (!_oPreference.AllowNestedInventoryDispatch && _Item.IsParent) // Item is Nested Item and Preferences do now allow dispatch of such items.
                                            throw new WMSExceptionMessage() { WMSExceptionCode = "EMC_OB_BL_012", WMSMessage = ErrorMessages.EMC_OB_BL_012, ShowAsError = true };
                                        

                                        oInventory.MRP = _oInv.MRP;
                                        oInventory.BatchNumber = _oInv.BatchNumber;
                                        oInventory.StorageLocation = _oInv.StorageLocation;
                                        oInventory.StorageLocationID = _oInv.StorageLocationID;
                                        oInventory.Color = _oInv.Color;
                                        oInventory.ColorID = _oInv.ColorID;
                                        oInventory.MaterialTransactionID = _oInv.MaterialTransactionID;
                                        oInventory.ReferenceDocumentID = _oInv.ReferenceDocumentID;

                                        oInventory.SuggestionID = _oInv.SuggestionID;

                                        // Logic to Receive Inventory.
                                        Inventory _Response = _oOutboundDAL.ConfirmLoading(oInventory);
                                        oInventory.IsDispatched = (_Response != null);
                                        List<LoadItem> lstLoadItem = new List<LoadItem>();
                                        lstLoadItem = _oOutboundDAL.FetchInventoryForLoadSheet(new LoadSheet() { LoadRefNo = oInventory.ReferenceDocumentNumber });
                                        oInventory.Quantity = lstLoadItem[0].LoadedQuantity;
                                        oInventory.MaterialShortDescription = lstLoadItem[0].MDescription;
                               



                                    break;
                                    }
                                    else
                                        bItemPartOfPickList = false;


                                }

                            //}

                            bItemPartOfLoadsheet = true;
                            break;
                        }
                    }
                }

                if (!bItemPartOfLoadsheet)
                    throw new WMSExceptionMessage() { WMSExceptionCode = "EMC_OB_BL_004", WMSMessage = ErrorMessages.EMC_OB_BL_004, ShowAsWarning = true };

                if(!bItemPartOfPickList)
                    throw new WMSExceptionMessage() { WMSExceptionCode = "EMC_OB_BL_005", WMSMessage = ErrorMessages.EMC_OB_BL_005, ShowAsWarning = true };

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

                ExceptionHandling.LogException(excp, _ClassCode + "001", oExcpData);
                throw new WMSExceptionMessage() { WMSExceptionCode = ErrorMessages.WMSExceptionCode, WMSMessage = ErrorMessages.WMSExceptionMessage, ShowAsCriticalError = true };
            }
        }

        public List<LoadItem> FetchInventoryForLoadSheet(LoadSheet oLoadSheet)
        {


            try
            {
                return _oOutboundDAL.FetchInventoryForLoadSheet(oLoadSheet);
            }
            catch (WMSExceptionMessage excp)
            {
                throw excp;
            }
            catch (Exception excp)
            {
                ExceptionData oExcpData = new ExceptionData();
                oExcpData.AddInputs("oLoadSheet", oLoadSheet);

                ExceptionHandling.LogException(excp, _ClassCode + "002", oExcpData);
                throw new WMSExceptionMessage() { WMSExceptionCode = ErrorMessages.WMSExceptionCode, WMSMessage = ErrorMessages.WMSExceptionMessage, ShowAsCriticalError = true };
            }
        }

        public List<LoadSheet> FetchLoadSheetList(SearchCriteria oCriteria)
        {


            try
            {
                //LoadSheet test = new LoadSheet();
                //test.LoadRefNo = "test";
                //List<LoadSheet> test1 = new List<LoadSheet>();
                //test1.Add(test);
                //return test1;
                return _oOutboundDAL.FetchLoadSheetList(oCriteria);
            }
            catch (WMSExceptionMessage excp)
            {
                throw excp;
            }
            catch (Exception excp)
            {
                ExceptionData oExcpData = new ExceptionData();
                oExcpData.AddInputs("oCriteria", oCriteria);

                ExceptionHandling.LogException(excp, _ClassCode + "003", oExcpData);
                throw new WMSExceptionMessage() { WMSExceptionCode = ErrorMessages.WMSExceptionCode, WMSMessage = ErrorMessages.WMSExceptionMessage, ShowAsCriticalError = true };
            }
        }

        public Inventory FetchNextInventoryItemForLoadSheet(LoadSheet oLoadSheet)
        {


            try
            {
                return new Inventory();
            }
            catch (WMSExceptionMessage excp)
            {
                throw excp;
            }
            catch (Exception excp)
            {
                ExceptionData oExcpData = new ExceptionData();
                oExcpData.AddInputs("oLoadSheet", oLoadSheet);

                ExceptionHandling.LogException(excp, _ClassCode + "004", oExcpData);
                throw new WMSExceptionMessage() { WMSExceptionCode = ErrorMessages.WMSExceptionCode, WMSMessage = ErrorMessages.WMSExceptionMessage, ShowAsCriticalError = true };
            }
        }

        public List<PickListInventory> FetchPicklistInventory(Picklist oPicklist, SearchCriteria oCriteria)
        {


            try
            {
                return _oOutboundDAL.FetchPicklistInventory(oPicklist, oCriteria);
            }
            catch (WMSExceptionMessage excp)
            {
                throw excp;
            }
            catch (Exception excp)
            {
                ExceptionData oExcpData = new ExceptionData();
                oExcpData.AddInputs("oPicklist", oPicklist);
                oExcpData.AddInputs("oCriteria", oCriteria);

                ExceptionHandling.LogException(excp, _ClassCode + "005", oExcpData);
                throw new WMSExceptionMessage() { WMSExceptionCode = ErrorMessages.WMSExceptionCode, WMSMessage = ErrorMessages.WMSExceptionMessage, ShowAsCriticalError = true };
            }
        }

        public PickListInventory GetNextPicklistItemForPicking(Picklist oPicklist, SearchCriteria oCriteria)
        {

            try
            {
                return _oOutboundDAL.GetNextPicklistItemForPicking(oPicklist, oCriteria);
            }
            catch (WMSExceptionMessage excp)
            {
                throw excp;
            }
            catch (Exception excp)
            {
                ExceptionData oExcpData = new ExceptionData();
                oExcpData.AddInputs("oPicklist", oPicklist);

                ExceptionHandling.LogException(excp, _ClassCode + "006", oExcpData);
                throw new WMSExceptionMessage() { WMSExceptionCode = ErrorMessages.WMSExceptionCode, WMSMessage = ErrorMessages.WMSExceptionMessage, ShowAsCriticalError = true };
            }
        }


        public List<Picklist> GetPicklistsForPicking(SearchCriteria oCriteria)
        {

            try
            {
                List<Picklist> lPickLists = _oOutboundDAL.GetPicklistsForPicking(oCriteria);
               return lPickLists;
            }
            catch (WMSExceptionMessage excp)
            {
                throw excp;
            }
            catch (Exception excp)
            {
                ExceptionData oExcpData = new ExceptionData();
                oExcpData.AddInputs("oCriteria", oCriteria);

                ExceptionHandling.LogException(excp, _ClassCode + "007", oExcpData);
                throw new WMSExceptionMessage() { WMSExceptionCode = ErrorMessages.WMSExceptionCode, WMSMessage = ErrorMessages.WMSExceptionMessage, ShowAsCriticalError = true };
            }
        }

        public PickListInventory MarkMaterialDamaged(Inventory oInventory,string SLoc)
        {

            try
            {
                InventoryDAL _oInventoryDAL = new InventoryDAL(LoggedInUserID, ConnectionString);
                InventoryUtility oInventoryUtility = new InventoryUtility();
              
                GoodsMovement _oDamaged = oInventoryUtility.GenerateInventoryMaterialDamaged(oInventory, 0, SLoc);

                GoodsMovement _oMovement = _oInventoryDAL.MoveInventory(_oDamaged);

                if (_oMovement == null)
                    throw new WMSExceptionMessage() { };
                else
                {
                    // *************************** TODO => INTEGRATE & CALL PICKING SUGGESTIONS *************************
                }
                // Fetch Active Stock @ Location
         
                // *************************** TODO => INTEGRATE & CALL PICKING SUGGESTIONS *************************
                return new PickListInventory();

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

        public PickListInventory MarkMaterialNotFound(PickListInventory oInventory)
        {
            try
            {
                CycleCountBL oCycleCountBL = new CycleCountBL(LoggedInUserID, ConnectionString);

                if (oCycleCountBL.FlagLocationForCycleCount(new Location() { LocationCode = oInventory.PickDisplayLocationCode }))
                {
                    
                    InventoryDAL _oInventoryDAL = new InventoryDAL(LoggedInUserID, ConnectionString);
                    InventoryUtility oInventoryUtility = new InventoryUtility();

                    // Fetch Active Stock @ Location
                    List<Inventory> _lInventory = _oInventoryDAL.GetActiveStock(new SearchCriteria() { MaterialCode = oInventory.MaterialCode, LocationCode = oInventory.PickDisplayLocationCode });

                    if (_lInventory == null)
                        throw new WMSExceptionMessage() { WMSMessage = ErrorMessages.WMSExceptionMessage, WMSExceptionCode = ErrorMessages.WMSExceptionCode, ShowAsCriticalError = true };
                    else if (_lInventory.Count == 0) 
                        throw  new WMSExceptionMessage() { WMSExceptionCode = "EMC_IN_DAL_001", WMSMessage = ErrorMessages.EMC_IN_DAL_001, ShowAsError = true }; 
                    else if (_lInventory.Count > 0)
                    {
                        List<GoodsMovement> _lGoodsMovement = new List<GoodsMovement>();

                        // Mark Stock Lost
                        foreach (Inventory _oActiveStock in _lInventory)
                        { 
                                _oActiveStock.SuggestionID = oInventory.SuggestionID;
                                _lGoodsMovement.Add(oInventoryUtility.GenerateInventoryLost(_oActiveStock, Constants.MovementType.Picking,1));
                          }

                        List<GoodsMovement> _lMovement = _oInventoryDAL.MoveInventory(_lGoodsMovement);

                        if (_lMovement == null || _lMovement.Count < _lInventory.Count)
                            throw new WMSExceptionMessage() { WMSMessage = ErrorMessages.WMSExceptionMessage, WMSExceptionCode = ErrorMessages.WMSExceptionCode, ShowAsCriticalError = true };
                        
                    }

                    // *************************** TODO => INTEGRATE & CALL PICKING SUGGESTIONS *************************
                    List<PickListInventory> _lPickListInventory = new List<PickListInventory>();

                    SearchCriteria oCriteriaToRegenrateSuggestion = new SearchCriteria()
                    {
                        MaterialCode = oInventory.MaterialCode, 
                        MaterialMasterID = oInventory.MaterialMasterID,
                        SuggestionID=oInventory.SuggestionID
                        
                      
                    };

                    _lPickListInventory = _oOutboundDAL.ReGeneratePickingSuggestionOnSkip(oCriteriaToRegenrateSuggestion);

                    return oInventory;
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
                oExcpData.AddInputs("oInventory", oInventory);

                

                ExceptionHandling.LogException(excp, _ClassCode + "009", oExcpData);
                throw new WMSExceptionMessage() { WMSExceptionCode = ErrorMessages.WMSExceptionCode, WMSMessage = ErrorMessages.WMSExceptionMessage, ShowAsCriticalError = true };
            }
        }

        public Inventory PickInventory(Inventory oInventory, out bool IsOldMRP, string NewRSNForPartialPicking = null)
        {

            try
            {
                IsOldMRP = false;
                InventoryDAL _oInventoryDAL = new InventoryDAL(LoggedInUserID, ConnectionString);
                InventoryUtility oInventoryUtility = new InventoryUtility();
                PickingPreferences oPreferences = this.FetchPickingPreferences(new SearchCriteria());

                List<PickListInventory> lPickListInventory = _oOutboundDAL.FetchPicklistInventory(new Picklist() { PickListRefNo = oInventory.ReferenceDocumentNumber }, new SearchCriteria() { LoggedInUserID = base.LoggedInUserID });
                List<Inventory> _lInventory = _oInventoryDAL.GetActiveStock(new SearchCriteria() { MaterialRSN = oInventory.RSN });
                List<Inventory> _lPickedInventory = _oOutboundDAL.FetchPickListInventoryForLoad(new Inventory() { RSN = oInventory.RSN, MaterialCode = oInventory.MaterialCode });
                bool _bMaterialIsInPickList = false, _bMaterialPickedExcess = true, _MaterialNotAsSuggested = false;
                decimal _dPciklistQuantityExceededBy = 0;

                if (_lInventory == null || _lPickedInventory == null)
                    throw new WMSExceptionMessage() { WMSMessage = ErrorMessages.WMSExceptionMessage, WMSExceptionCode = ErrorMessages.WMSExceptionCode, ShowAsCriticalError = true };
                else if (_lInventory.Count == 0)
                    throw new WMSExceptionMessage() { WMSMessage = ErrorMessages.EMC_OB_BL_002, WMSExceptionCode = "EMC_OB_BL_002", ShowAsError = true }; // Invalid RSN
                else if (_lPickedInventory.Count > 0)
                    throw new WMSExceptionMessage() { WMSExceptionCode = "EMC_OB_BL_014", WMSMessage = ErrorMessages.EMC_OB_BL_014.Replace("[RSN]", oInventory.RSN), ShowAsError = true };
                else if (_lInventory.Count > 0)
                {
                    foreach (PickListInventory _oPicklistItem in lPickListInventory)
                    {

                        //if (oInventory.SuggestionID == _oPicklistItem.SuggestionID)
                        //{
                        //    _MaterialNotAsSuggested = true;

                        foreach (Inventory _oActiveStock in _lInventory)
                        {
                            if (_oPicklistItem.MaterialCode.Equals(_oActiveStock.MaterialCode) && oInventory.SuggestionID == _oPicklistItem.SuggestionID)
                            {
                                _bMaterialIsInPickList = true;
                                _MaterialNotAsSuggested = false;
                                

                                if (_oPicklistItem.PickedQuantity + (oInventory.UserConfirmedExcessTransaction ? oInventory.Quantity : _oActiveStock.Quantity) <= _oPicklistItem.RequirementQuantity)
                                {
                                    // Added By Swamy TO Verify Newly Scanned RSN is Valid Or Not
                                    if (oInventory.UserConfirmedExcessTransaction && _oInventoryDAL.IsRSNExisted(NewRSNForPartialPicking))
                                    {
                                        throw new WMSExceptionMessage() { WMSExceptionCode = "EMC_OB_BL_020", WMSMessage = ErrorMessages.EMC_OB_BL_020, ShowAsError = true };
                                    }

                                    _bMaterialPickedExcess = false;
 
                                    if (_oActiveStock.MaterialCode.Equals(oInventory.MaterialCode))
                                    {
                                        oInventory.StorageLocation = _oActiveStock.StorageLocation;
                                        oInventory.Color = _oActiveStock.Color;
                                        oInventory.MaterialMasterID = _oActiveStock.MaterialMasterID;

                                        if (_oActiveStock.Quantity < oInventory.Quantity)
                                            throw new WMSExceptionMessage() { WMSExceptionCode = "EMC_OB_BL_016", WMSMessage = ErrorMessages.EMC_OB_BL_016.Replace("[QTY]", _oActiveStock.Quantity.ToString()), ShowAsError = true };
                                        else if (_oActiveStock.IsFinishedGoods && !oInventory.UserConfirmedExcessTransaction)
                                            oInventory.Quantity = _oActiveStock.Quantity;

                                        if (!(_oActiveStock.LocationCode.Equals(oInventory.LocationCode) || _oActiveStock.DisplayLocationCode.Equals(oInventory.LocationCode)) && _oActiveStock.IsFinishedGoods)
                                        {
                                            List<GoodsMovement> _lGoodsMovement = new List<GoodsMovement>();

                                            if (!_oActiveStock.IsLost)
                                                _lGoodsMovement.Add(oInventoryUtility.GenerateInventoryLost(_oActiveStock, Constants.MovementType.Picking));

                                            Inventory _oFound = _oActiveStock.DuplicateItem();
                                            _oFound.DisplayLocationCode = oInventory.LocationCode;
                                            _lGoodsMovement.Add(oInventoryUtility.GenerateInventoryFound(_oFound, Constants.MovementType.Picking));


                                            List<GoodsMovement> _lMovement = _oInventoryDAL.MoveInventory(_lGoodsMovement);

                                            if (_lMovement == null || _lMovement.Count != _lGoodsMovement.Count)
                                                throw new WMSExceptionMessage() { WMSMessage = ErrorMessages.WMSExceptionMessage, WMSExceptionCode = ErrorMessages.WMSExceptionCode, ShowAsCriticalError = true };
                                        }
                                    }
                                }
                                else
                                {
                                    _bMaterialPickedExcess = true;
                                    _dPciklistQuantityExceededBy = _oPicklistItem.RequirementQuantity - _oPicklistItem.PickedQuantity;
                                    // Added By Swamy TO Check Excess Qty ENtered By User
                                    if (_bMaterialPickedExcess && oInventory.UserConfirmedExcessTransaction)
                                        throw new WMSExceptionMessage() { WMSExceptionCode = "EMC_OB_BL_017", WMSMessage = ErrorMessages.EMC_OB_BL_017.Replace("[QTY]", (_dPciklistQuantityExceededBy.ToString()).ToString()), ShowUserConfirmDialogue = true };

                                }
                            }
                            else
                            {
                                if (_oPicklistItem.MaterialCode.Equals(_oActiveStock.MaterialCode))
                                    _bMaterialIsInPickList = true;

                                _MaterialNotAsSuggested = true;
                            }



                        }
                       //// }
                       // else if(!_MaterialNotAsSuggested)
                            //_MaterialNotAsSuggested = false;
                    }

                    if (_MaterialNotAsSuggested)
                        throw new WMSExceptionMessage() { WMSExceptionCode = "EMC_OB_BL_019", WMSMessage = ErrorMessages.EMC_OB_BL_019, ShowAsError = true };



                    if (_bMaterialIsInPickList)
                    {
                        if (_bMaterialPickedExcess && !oInventory.UserConfirmedExcessTransaction)
                            throw new WMSExceptionMessage() { WMSExceptionCode = "EMC_OB_BL_017", WMSMessage = ErrorMessages.EMC_OB_BL_017.Replace("[QTY]", (_dPciklistQuantityExceededBy.ToString()).ToString()), ShowUserConfirmDialogue = true };
                        // Verify the current location of the Item.

                        // List<Picklist> oPickListList = _oOutboundDAL.GetPicklistsForPicking(new SearchCriteria() { UserID = oInventory.CreatedBy, LoggedInUserID = oInventory.CreatedBy });

                        List<PickListInventory> lPLI = _oOutboundDAL.FetchPicklistInventory(new Picklist() { PickListRefNo = oInventory.ReferenceDocumentNumber }, new SearchCriteria() { UserID = LoggedInUserID });
                        _bMaterialIsInPickList = false;
                        string _DockLocation = string.Empty;
                        foreach (PickListInventory _Iterator in lPLI)
                        {
                            // if (_Iterator.PickListRefNo.Equals(oInventory.ReferenceDocumentNumber) && oInventory.SuggestionID = )
                            if (oInventory.SuggestionID == _Iterator.SuggestionID)
                            {
                                oInventory.ReferenceDocumentID = _Iterator.PickListHeaderID;
                                _DockLocation = _Iterator.DockLocation;
                                _bMaterialIsInPickList = true;
                                if (_Iterator.PickedQuantity >= _Iterator.PicklistQuantity && _Iterator.PicklistQuantity > 0)
                                    throw new WMSExceptionMessage() { WMSExceptionCode = "EMC_OB_BL_001", WMSMessage = ErrorMessages.EMC_OB_BL_001, ShowAsError = true };
                            }

                        }

                        
                        GoodsMovement _oPickMovement = oInventoryUtility.GenerateInventoryMovement(oInventory, oInventory.LocationCode, _DockLocation, null, oInventory.ContainerCode, oInventory.StorageLocation, oInventory.StorageLocation, oInventory.Color, oInventory.Color);
                        _oPickMovement.IsPicking = 1;

                        if (oInventory.UserConfirmedExcessTransaction)
                            _oPickMovement.NewRSNForPartialPicking = NewRSNForPartialPicking;

                        GoodsMovement _oResult = _oInventoryDAL.MoveInventory(_oPickMovement);

                        IsOldMRP = _oResult.IsMaterialOldMRP;
                        oInventory.MaterialShortDescription = _oResult.MaterialDescription;

                        if (_oResult == null)
                            throw new WMSExceptionMessage() { WMSExceptionCode = ErrorMessages.WMSExceptionCode, WMSMessage = ErrorMessages.WMSExceptionMessage, ShowAsCriticalError = true };
                        else
                            return oInventory;
                    }
                    else
                        throw new WMSExceptionMessage() { WMSExceptionCode = "EMC_OB_BL_015", WMSMessage = ErrorMessages.EMC_OB_BL_015, ShowAsError = true };
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
                oExcpData.AddInputs("oInventory", oInventory);

                ExceptionHandling.LogException(excp, _ClassCode + "010", oExcpData);
                throw new WMSExceptionMessage() { WMSExceptionCode = ErrorMessages.WMSExceptionCode, WMSMessage = ErrorMessages.WMSExceptionMessage, ShowAsCriticalError = true };
            }
        }

        public Inventory RevertLoading(Inventory oInventory)
        {

            try
            {
                if (oInventory.ReferenceDocumentNumber == null || oInventory.RSN == null)
                    throw new WMSExceptionMessage() { WMSMessage = ErrorMessages.EMC_OB_BL_006, WMSExceptionCode = "EMC_OB_BL_006", ShowAsError = true };

                InventoryDAL _oInvDAL = new InventoryDAL(LoggedInUserID, ConnectionString);

                List<Inventory> _lActiveStock = _oInvDAL.GetActiveStock(new SearchCriteria() { MaterialRSN = oInventory.RSN });

                if (_lActiveStock == null)
                    throw new WMSExceptionMessage() { WMSExceptionCode = ErrorMessages.WMSExceptionCode, WMSMessage = ErrorMessages.WMSExceptionMessage, ShowAsCriticalError = true };
                else if (_lActiveStock.Count != 1)
                    throw new WMSExceptionMessage() { WMSExceptionCode = "EMC_OB_BL_007", WMSMessage = ErrorMessages.EMC_OB_BL_007, ShowAsError = true };
                else
                {

                    if (oInventory.Quantity == 0 && !oInventory.UserConfirmReDo)
                    {
                        if (_lActiveStock[0].IsFinishedGoods)
                            throw new WMSExceptionMessage() { WMSExceptionCode = "EMC_OB_BL_008", WMSMessage = ErrorMessages.EMC_OB_BL_008.Replace("[RSN]", oInventory.RSN), ShowUserConfirmDialogue = true };
                        else
                            throw new WMSExceptionMessage() { WMSExceptionCode = "EMC_OB_BL_009", WMSMessage = ErrorMessages.EMC_OB_BL_009.Replace("[RSN]", oInventory.RSN), ShowUserConfirmDialogue = true };
                    }

                    return _oOutboundDAL.RevertLoading(oInventory);
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

                ExceptionHandling.LogException(excp, _ClassCode + "011", oExcpData);
                throw new WMSExceptionMessage() { WMSExceptionCode = ErrorMessages.WMSExceptionCode, WMSMessage = ErrorMessages.WMSExceptionMessage, ShowAsCriticalError = true };
            }
        }

        public bool ValidateLocationAtPicking(Location oLocation)
        {

            try
            {
                LocationDAL _oLocationDAL = new LocationDAL(LoggedInUserID, ConnectionString);

                List<Location> lLocations = _oLocationDAL.GetLocations(
                        new SearchCriteria()
                        {
                            LocationCode = oLocation.LocationCode == null ? oLocation.SystemLocationCode : oLocation.LocationCode,
                            LocationID = oLocation.LocationID
                        });

                if (lLocations == null)
                    throw new WMSExceptionMessage()
                    {
                        ShowAsCriticalError = true,
                        WMSMessage = ErrorMessages.WMSExceptionMessage,
                        WMSExceptionCode = ErrorMessages.WMSExceptionCode
                    };
                else if (lLocations.Count == 1)
                    return true;
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
                oExcpData.AddInputs("oLocation", oLocation);

                ExceptionHandling.LogException(excp, _ClassCode + "012", oExcpData);
                throw new WMSExceptionMessage() { WMSExceptionCode = ErrorMessages.WMSExceptionCode, WMSMessage = ErrorMessages.WMSExceptionMessage, ShowAsCriticalError = true };
            }
        }

        public bool ValidatePalletAtPicking(Pallet oPallet)
        {

            try
            {
                PalletDAL _oPalletDAL = new PalletDAL(LoggedInUserID, ConnectionString);

                List<Pallet> _lPallets = _oPalletDAL.GetPallets(new SearchCriteria() { ContainerCode = oPallet.PalletCode, ContainerID = oPallet.PalletID });

                if (_lPallets == null)
                    throw new WMSExceptionMessage() { WMSMessage = ErrorMessages.WMSExceptionMessage, WMSExceptionCode = ErrorMessages.WMSExceptionCode, ShowAsCriticalError = true };
                else if (_lPallets.Count > 0)
                    return true;
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

                ExceptionHandling.LogException(excp, _ClassCode + "013", oExcpData);
                throw new WMSExceptionMessage() { WMSExceptionCode = ErrorMessages.WMSExceptionCode, WMSMessage = ErrorMessages.WMSExceptionMessage, ShowAsCriticalError = true };
            }
        }

        public bool LoadingComplete(LoadSheet oLoadSheet)
        {


            try
            {
                throw new NotImplementedException();
            }
            catch (WMSExceptionMessage excp)
            {
                throw excp;
            }
            catch (Exception excp)
            {
                ExceptionData oExcpData = new ExceptionData();
                oExcpData.AddInputs("oLoadSheet", oLoadSheet);

                ExceptionHandling.LogException(excp, _ClassCode + "014", oExcpData);
                throw new WMSExceptionMessage() { WMSExceptionCode = ErrorMessages.WMSExceptionCode, WMSMessage = ErrorMessages.WMSExceptionMessage, ShowAsCriticalError = true };
            }
        }

        /// <summary>
        /// Fetches the Picking Preferences Applicable for the User.
        /// </summary>
        /// <param name="oCriteria">Search Criteria for Picking Preferences</param>
        /// <returns>Object of type Picking Preferences</returns>
        public PickingPreferences FetchPickingPreferences(SearchCriteria oCriteria)
        {
            try
            {

                PickingPreferences oPreferences = _oOutboundDAL.FetchPickingPreferences(oCriteria);


                return oPreferences;
            }
            catch (WMSExceptionMessage excp)
            {
                throw excp;
            }
            catch (Exception excp)
            {
                ExceptionData oExcpData = new ExceptionData();
                oExcpData.AddInputs("oCriteria", oCriteria);

                ExceptionHandling.LogException(excp, _ClassCode + "015", oExcpData);
                throw new WMSExceptionMessage() { WMSExceptionCode = ErrorMessages.WMSExceptionCode, WMSMessage = ErrorMessages.WMSExceptionMessage, ShowAsCriticalError = true };
            }
        }


        public List<Outbound> GetItemsUnderSO(string SONumber, int AccountID,int UserId)
        {
            try
            {

                List<Outbound> loutbounds = _oOutboundDAL.GetMaterialsUnderSOForPacking(SONumber, AccountID, UserId);//============== Added By M.D.Prasad ON 28 - 08 - 2020 For AccounID ===========
                return loutbounds;
                

            }
            catch (Exception)
            {

                throw;
            }
        }



        public List<Outbound> GetMSPsForPacking(string SodetailsID)
        {
            try
            {

                List<Outbound> loutbounds = _oOutboundDAL.GetMSPForPacking(SodetailsID);
                return loutbounds;


            }
            catch (Exception)
            {

                throw;
            }
        }



        public Outbound UpsertPackItem(Outbound outbound)
        {
            try
            {
                Outbound _outbound = _oOutboundDAL.UpsertPackItem(outbound);

                return _outbound;


            }
            catch (Exception)
            {

                throw;
            }
        }


        public Outbound PackComplete(Outbound outbound)
        {
            try
            {
                Outbound _outbound = _oOutboundDAL.PackComplete(outbound);

                return _outbound;


            }
            catch (Exception)
            {

                throw;
            }
        }


        public Outbound LoadsheetGeneration(Outbound outbound)
        {
            try
            {
                Outbound _outbound = _oOutboundDAL.LoadSheetGeneration(outbound);

                return _outbound;


            }
            catch (Exception)
            {

                throw;
            }
        }


        public Outbound GetLoadDetails(Outbound outbound)
        {
            try
            {
                Outbound _outbound = _oOutboundDAL.GetSOCountUnderLoadSheet(outbound);

                return _outbound;


            }
            catch (Exception)
            {

                throw;
            }
        }


        public Outbound UpsertLoadDetails(Outbound outbound)
        {
            try
            {
                Outbound _outbound = _oOutboundDAL.UpsertLoadDetails(outbound);

                return _outbound;


            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<Outbound> GetOBDNosUnderSO(Outbound outbound)
        {
            try
            {
                List<Outbound> outbounds = new List<Outbound>();
                outbounds = _oOutboundDAL.GetOBDNosUnderSO(outbound);

                return outbounds;


            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool CheckSO(Outbound outbound)
        {
            try
            {
                bool result = _oOutboundDAL.CheckSO(outbound);

                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }


        public bool CheckOBDSO(Outbound outbound)
        {
            try
            {
                bool result = _oOutboundDAL.CheckOBDSO(outbound);

                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public List<Outbound> GetPackingCartonInfo(Outbound outbound)
        {
            try
            {
                List<Outbound> outbounds = new List<Outbound>();
                outbounds = _oOutboundDAL.GetPackingCartonInfo(outbound);

                return outbounds;


            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool CheckCarton(Outbound outbound)
        {
            try
            {
                bool result = _oOutboundDAL.CheckCarton(outbound);

                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public List<Outbound> GetOutboundRefNo(Outbound outbound)
        {
            try
            {
                List<Outbound> _lstOutbound = new List<Outbound>();

                _lstOutbound = _oOutboundDAL.GetOBDRefNos(outbound);

                return _lstOutbound;

            }
            catch (Exception)
            {

                throw;
            }
        }


        public List<Outbound> GetRevertOBDLIst(Outbound outbound)
        {
            try
            {
                List<Outbound> _lstOutbound = new List<Outbound>();

                _lstOutbound = _oOutboundDAL.GetRevertOBDLIst(outbound);

                return _lstOutbound;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<Outbound> GetRevertSOLIst(Outbound outbound)
        {
            try
            {
                List<Outbound> _lstOutbound = new List<Outbound>();

                _lstOutbound = _oOutboundDAL.GetRevertSOLIst(outbound);

                return _lstOutbound;

            }
            catch (Exception)
            {

                throw;
            }
        }
        public List<Outbound> GetRevertSOOBDInfo(Outbound outbound)
        {
            try
            {
                List<Outbound> _lstOutbound = new List<Outbound>();

                _lstOutbound = _oOutboundDAL.GetRevertSOOBDInfo(outbound);

                return _lstOutbound;

            }
            catch (Exception)
            {

                throw;
            }
        }
        public List<Outbound> GetRevertCartonCheck(Outbound outbound)
        {
            try
            {
                List<Outbound> _lstOutbound = new List<Outbound>();

                _lstOutbound = _oOutboundDAL.GetRevertCartonCheck(outbound);

                return _lstOutbound;

            }
            catch (Exception)
            {

                throw;
            }
        }
        public List<Outbound> GetScanqtyvalidation(Outbound outbound)
        {
            try
            {
                List<Outbound> _lstOutbound = new List<Outbound>();

                _lstOutbound = _oOutboundDAL.GetScanqtyvalidation(outbound);

                return _lstOutbound;

            }
            catch (Exception)
            {

                throw;
            }
        }
        public List<Outbound> UpsertHHTOBDRevert(Outbound outbound)
        {
            try
            {
                List<Outbound> _lstOutbound = new List<Outbound>();

                _lstOutbound = _oOutboundDAL.UpsertHHTOBDRevert(outbound);

                return _lstOutbound;

            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
