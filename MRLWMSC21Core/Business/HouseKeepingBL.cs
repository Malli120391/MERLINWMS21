using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MRLWMSC21Core.Entities;
using MRLWMSC21Core.Library;
using MRLWMSC21Core.DataAccess;
using MRLWMSC21Core.DataAccess.Utilities;

namespace MRLWMSC21Core.Business
{
    public class HouseKeepingBL : BaseBL
    {
        private string _ClassCode = string.Empty;//"WMSCore_BL_011_";

        private InventoryDAL _goInventoryDAL = null;

        public HouseKeepingBL(int LoggedInUser, string ConnectionString) : base(LoggedInUser, ConnectionString)
        {
            _goInventoryDAL = new InventoryDAL(LoggedInUser, ConnectionString);
            _ClassCode = ExceptionHandling.GetClassExceptionCode(ExceptionHandling.ExcpConstants_API_BusinessLayer.HouseKeepingBL);
        }

        /// <summary>
        /// Update the item MRP at MRP Change Job.
        /// </summary>
        /// <param name="oInventory"></param>
        /// <param name="OldMRP"></param>
        /// <param name="NewMRP"></param>
        /// <returns></returns>
        public Inventory ChangeItemMRP(Inventory oInventory, HouseKeepingJob oJob=null, decimal OldMRP=1, decimal NewMRP=1)
        {
            try
            {
                HouseKeepingDAL houseKeepingDAL = new HouseKeepingDAL(LoggedInUserID, ConnectionString);
                if (oInventory == null)
                    throw new WMSExceptionMessage() { WMSExceptionCode = "WMC_HK_BL_0003", WMSMessage = ErrorMessages.WMC_HK_BL_0003, ShowAsError = true };
                //else if (OldMRP == 0)
                //    throw new WMSExceptionMessage() { WMSExceptionCode = "WMC_HK_BL_0001", WMSMessage = ErrorMessages.WMC_HK_BL_0001, ShowAsError = true };
                //else if (NewMRP == 0)
                //    throw new WMSExceptionMessage() { WMSExceptionCode = "WMC_HK_BL_0002", WMSMessage = ErrorMessages.WMC_HK_BL_0002, ShowAsError = true };
                //else if (oJob != null && (oJob.TransferRequestNumber.Equals(string.Empty) || oJob.TransferRequestNumber == null) && oInventory.ReferenceDocumentNumber != null)
                //    throw new WMSExceptionMessage() { WMSExceptionCode = "WMC_HK_BL_0004", WMSMessage = ErrorMessages.WMC_HK_BL_0004, ShowAsError = true };
                else
                    NewMRP = houseKeepingDAL.GetMRPValuesForSKU(oInventory, out OldMRP);
                if (NewMRP == OldMRP)
                    throw new WMSExceptionMessage() { WMSExceptionCode = "WMC_HK_BL_0006", WMSMessage = ErrorMessages.WMC_HK_BL_0006, ShowAsError = true };
               InventoryUtility oInvUtil = new InventoryUtility();

                GoodsMovement oMRPChangeMovement = oInvUtil.GenerateInventoryMovementMRPChange(oInventory, OldMRP, NewMRP);

                // GoodsMovement _MovementOutput = _goInventoryDAL.MoveInventory(oMRPChangeMovement);
                GoodsMovement _MovementOutput = _goInventoryDAL.ChangeMRP(oMRPChangeMovement);

                if (_MovementOutput == null)
                    throw new WMSExceptionMessage() { WMSExceptionCode = ErrorMessages.WMSExceptionCode, WMSMessage = ErrorMessages.WMSExceptionMessage, ShowAsCriticalError = true };
                else
                {
                    oInventory.IsReceived = true;
                    oInventory.OldMRP = OldMRP;
                    oInventory.MRP = NewMRP;
                }

                return oInventory;
            }
            catch (WMSExceptionMessage excp)
            {
                throw excp;
            }
            catch (Exception excp)
            {
                ExceptionData oExcpData = new ExceptionData();
                oExcpData.AddInputs("oJob", oJob);
                oExcpData.AddInputs("oInventory", oInventory);
                oExcpData.AddInputs("OldMRP", OldMRP);
                oExcpData.AddInputs("NewMRP", NewMRP);

                ExceptionHandling.LogException(excp, _ClassCode + "001", oExcpData);
                return null;
            }
        }


        public bool ValidateLocation(Location oLocation)
        {
            try
            {

                InboundBL oInboundBL = new InboundBL(LoggedInUserID, ConnectionString);

                return oInboundBL.ValidateLocationCode(oLocation, Constants.LocationType.Bin, false);

            }
            catch (WMSExceptionMessage excp)
            {
                throw excp;
            }
            catch (Exception excp)
            {
                ExceptionData oExcpData = new ExceptionData();
                oExcpData.AddInputs("oLocation", oLocation);

                ExceptionHandling.LogException(excp, _ClassCode + "002", oExcpData);
                throw new WMSExceptionMessage() { WMSExceptionCode = ErrorMessages.WMSExceptionCode, WMSMessage = ErrorMessages.WMSExceptionMessage, ShowAsCriticalError = true };
            }
        }
        
        public GoodsMovement TransferItemBinToBin(HouseKeepingJob oJob, Inventory oInventory, string FromLocation, string ToLocation)
        {
            try
            {
                if(oJob!=null)
                {
                    oInventory.ReferenceDocumentNumber = oJob.TransferRequestNumber;
                }

                InventoryUtility _oInventoryUtil = new InventoryUtility();
                InventoryDAL _oInventoryDAL = new InventoryDAL(LoggedInUserID, ConnectionString);

                if (oInventory == null)
                    throw new WMSExceptionMessage() { WMSExceptionCode = "WMC_HK_BL_008", WMSMessage = ErrorMessages.WMC_HK_BL_008, ShowAsError = true };
                else if (oInventory.RSN == null)
                    throw new WMSExceptionMessage() { WMSExceptionCode = "WMC_HK_BL_009", WMSMessage = ErrorMessages.WMC_HK_BL_009, ShowAsError = true };
                else if (string.IsNullOrEmpty(FromLocation))
                    throw new WMSExceptionMessage() { WMSExceptionCode = "WMC_HK_BL_010", WMSMessage = ErrorMessages.WMC_HK_BL_010, ShowAsError = true };
                //else if (string.IsNullOrEmpty(ToLocation))
                //    throw new WMSExceptionMessage() { WMSExceptionCode = "WMC_HK_BL_011", WMSMessage = ErrorMessages.WMC_HK_BL_011, ShowAsError = true };


                List<Inventory> _lActiveStock = _oInventoryDAL.GetActiveStock(new SearchCriteria() { MaterialRSN = oInventory.RSN });

                if (_lActiveStock == null)
                    throw new Exception("An error has occoured in fetching the Active Stock.");
                else if (_lActiveStock.Count != 1)
                    throw new WMSExceptionMessage() { WMSExceptionCode = "WMC_HK_BL_012", WMSMessage = ErrorMessages.WMC_HK_BL_012, ShowAsError = true };
                else
                {
                    foreach (Inventory _StockItem in _lActiveStock)
                    {
                        if (_StockItem.RSN.Equals(oInventory.RSN))
                        {
                            oInventory.Quantity = _StockItem.Quantity;


                            if (!(oInventory.LocationCode.Equals(_StockItem.LocationCode) || oInventory.LocationCode.Equals(_StockItem.DisplayLocationCode)))
                            {
                                List<GoodsMovement> _lLostFoundMovements = new List<GoodsMovement>();


                                if (!_StockItem.IsLost)
                                {
                                    GoodsMovement _MoveLost = _oInventoryUtil.GenerateInventoryLost(_StockItem, Constants.MovementType.InternalTransfer);
                                    _lLostFoundMovements.Add(_MoveLost);
                                }


                                GoodsMovement _MoveFound = _oInventoryUtil.GenerateInventoryFound(oInventory, Constants.MovementType.InternalTransfer);
                                _MoveFound.PutawayAtLocationCode = FromLocation;
                                _lLostFoundMovements.Add(_MoveFound);

                                List<GoodsMovement> _lLostFoundMovementsResponse = _oInventoryDAL.MoveInventory(_lLostFoundMovements);

                                if (_lLostFoundMovements.Count != _lLostFoundMovementsResponse.Count)
                                    throw new Exception("An error has occoured when posting Lost & Found in Bin to Bin Transfers.");
                            }
                        }
                    }

                    GoodsMovement _BinToBinGoodsMovement = _oInventoryUtil.GenerateInventoryMovementBinToBin(oInventory, FromLocation, ToLocation);
                    if (string.IsNullOrEmpty(ToLocation))
                        _BinToBinGoodsMovement.PutawayAtLocationCode = FromLocation;

                         _BinToBinGoodsMovement = _oInventoryDAL.MoveInventory(_BinToBinGoodsMovement);

                    return _BinToBinGoodsMovement;

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
                oExcpData.AddInputs("FromLocation", FromLocation);
                oExcpData.AddInputs("ToLocation", ToLocation);

                ExceptionHandling.LogException(excp, _ClassCode + "003", oExcpData);
                throw new WMSExceptionMessage() { WMSExceptionCode = ErrorMessages.WMSExceptionCode, WMSMessage = ErrorMessages.WMSExceptionMessage, ShowAsCriticalError = true };
            }
        }
        
        private GoodsMovement TransferItemSLocToSloc(HouseKeepingJob oJob, Inventory oInventory, string FromSLoc, string ToSLoc)
        {
            try
            {
                if (oJob != null)
                {
                    oInventory.ReferenceDocumentNumber = oJob.TransferRequestNumber;
                }

                InventoryUtility _oInventoryUtil = new InventoryUtility();
                InventoryDAL _oInventoryDAL = new InventoryDAL(LoggedInUserID, ConnectionString);

                if (oInventory == null)
                    throw new WMSExceptionMessage() { WMSExceptionCode = "WMC_HK_BL_008", WMSMessage = ErrorMessages.WMC_HK_BL_008, ShowAsError = true };
                else if (oInventory.RSN == null)
                    throw new WMSExceptionMessage() { WMSExceptionCode = "WMC_HK_BL_009", WMSMessage = ErrorMessages.WMC_HK_BL_009, ShowAsError = true };
                else if (string.IsNullOrEmpty(FromSLoc))
                    throw new WMSExceptionMessage() { WMSExceptionCode = "WMC_HK_BL_014", WMSMessage = ErrorMessages.WMC_HK_BL_014, ShowAsError = true };
                else if (string.IsNullOrEmpty(ToSLoc))
                    throw new WMSExceptionMessage() { WMSExceptionCode = "WMC_HK_BL_015", WMSMessage = ErrorMessages.WMC_HK_BL_015, ShowAsError = true };


                List<Inventory> _lActiveStock = _oInventoryDAL.GetActiveStock(new SearchCriteria() { MaterialRSN = oInventory.RSN });

                if (_lActiveStock == null)
                    throw new Exception("An error has occoured in fetching the Active Stock.");
                else if (_lActiveStock.Count != 1)
                    throw new WMSExceptionMessage() { WMSExceptionCode = "WMC_HK_BL_012", WMSMessage = ErrorMessages.WMC_HK_BL_012, ShowAsError = true };
                else
                {
                    foreach (Inventory _StockItem in _lActiveStock)
                    {
                        if (_StockItem.RSN.Equals(oInventory.RSN))
                        {
                            List<GoodsMovement> _lLostFoundMovements = new List<GoodsMovement>();

                            GoodsMovement _MoveLost = _oInventoryUtil.GenerateInventoryLost(_StockItem, Constants.MovementType.InternalTransfer);

                            if (!_StockItem.IsLost)
                                _lLostFoundMovements.Add(_MoveLost);

                            GoodsMovement _MoveFound = _oInventoryUtil.GenerateInventoryFound(oInventory, Constants.MovementType.InternalTransfer);
                            _lLostFoundMovements.Add(_MoveFound);

                            _lLostFoundMovements = _oInventoryDAL.MoveInventory(_lLostFoundMovements);

                            if (_lLostFoundMovements.Count != 2)
                                throw new Exception("An error has occoured when posting Lost & Found in Bin to Bin Transfers.");

                        }
                    }

                    GoodsMovement _BinToBinGoodsMovement = _oInventoryUtil.GenerateInventoryMovementSLocToSLoc(oInventory, FromSLoc, ToSLoc);

                    _BinToBinGoodsMovement = _oInventoryDAL.MoveInventory(_BinToBinGoodsMovement);

                    return _BinToBinGoodsMovement;

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
                oExcpData.AddInputs("FromSLoc", FromSLoc);
                oExcpData.AddInputs("ToSLoc", ToSLoc);

                ExceptionHandling.LogException(excp, _ClassCode + "004", oExcpData);
                throw new WMSExceptionMessage() { WMSExceptionCode = ErrorMessages.WMSExceptionCode, WMSMessage = ErrorMessages.WMSExceptionMessage, ShowAsCriticalError = true };
            }
        }


        public List<Inventory> GetActivestock(SearchCriteria oSearchCriteria)
        {
            try
            {
  
                //if (oSearchCriteria.MaterialRSN == string.Empty)
                //    throw new WMSExceptionMessage() { WMSExceptionCode = "WMC_BE_DEF_INV_0002", ShowAsError = true, WMSMessage = ErrorMessages.WMC_BE_DEF_INV_0002 };
                InventoryDAL oInventoryDAL = new InventoryDAL(base.LoggedInUserID, base.ConnectionString);

                List<Inventory> lstActiveStock = oInventoryDAL.GetActiveStock(new SearchCriteria() { MaterialRSN = oSearchCriteria.MaterialRSN , ContainerCode=oSearchCriteria.ContainerCode,LocationCode=oSearchCriteria.LocationCode,MaterialCode=oSearchCriteria.MaterialCode });

                if (lstActiveStock.Count == 0)
                    throw new WMSExceptionMessage() { WMSExceptionCode = "WMC_HK_BL_016", WMSMessage = ErrorMessages.WMC_HK_BL_016, ShowAsError = true };

                return lstActiveStock;
            }
            catch (WMSExceptionMessage excp)
            {
                throw excp;
            }
            catch (Exception excp)
            {
                ExceptionData oExcpData = new ExceptionData();
                oExcpData.AddInputs("oSearchCriteria", oSearchCriteria);

                ExceptionHandling.LogException(excp, _ClassCode + "002", oExcpData);
                throw new WMSExceptionMessage() { WMSExceptionCode = ErrorMessages.WMSExceptionCode, WMSMessage = ErrorMessages.WMSExceptionMessage, ShowAsCriticalError = true };
            }
        }



        public List<Inventory> GetActivestockWithOutRSN(SearchCriteria oSearchCriteria)
        {
            try
            {
                //if (oSearchCriteria.MaterialRSN == string.Empty)
                //    throw new WMSExceptionMessage() { WMSExceptionCode = "WMC_BE_DEF_INV_0002", ShowAsError = true, WMSMessage = ErrorMessages.WMC_BE_DEF_INV_0002 };
                InventoryDAL oInventoryDAL = new InventoryDAL(base.LoggedInUserID, base.ConnectionString);

                List<Inventory> lstActiveStock = oInventoryDAL.GetActiveStockWithOutRSN(new SearchCriteria() { MaterialRSN = oSearchCriteria.MaterialRSN, ContainerCode = oSearchCriteria.ContainerCode, LocationCode = oSearchCriteria.LocationCode, MaterialCode = oSearchCriteria.MaterialCode,BatchNumber=oSearchCriteria.BatchNumber });

                if (lstActiveStock.Count == 0)
                    throw new WMSExceptionMessage() { WMSExceptionCode = "WMC_HK_BL_016", WMSMessage = ErrorMessages.WMC_HK_BL_016, ShowAsError = true };

                return lstActiveStock;
            }
            catch (WMSExceptionMessage excp)
            {
                throw excp;
            }
            catch (Exception excp)
            {
                ExceptionData oExcpData = new ExceptionData();
                oExcpData.AddInputs("oSearchCriteria", oSearchCriteria);

                ExceptionHandling.LogException(excp, _ClassCode + "005", oExcpData);
                throw new WMSExceptionMessage() { WMSExceptionCode = ErrorMessages.WMSExceptionCode, WMSMessage = ErrorMessages.WMSExceptionMessage, ShowAsCriticalError = true };
            }
        }


        public string GetBatchNumberByJOBRefNumber(string DenestingJONumber)
        {

            try
            {
                HouseKeepingDAL houseKeepingDAL = new HouseKeepingDAL(LoggedInUserID, ConnectionString);
                return houseKeepingDAL.GetBatchNumberByDenestingJO(DenestingJONumber);
            }
            catch (WMSExceptionMessage excp)
            {
                throw excp;
            }
            catch (Exception excp)
            {
                ExceptionData oExcpData = new ExceptionData();
                ExceptionHandling.LogException(excp, _ClassCode + "001", oExcpData);
                return "";
            }
        }
    }
}
