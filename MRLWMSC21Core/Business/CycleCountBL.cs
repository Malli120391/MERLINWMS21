using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MRLWMSC21Core.Entities;
using MRLWMSC21Core.DataAccess;
using MRLWMSC21Core.Library;
using MRLWMSC21Core.Business.Interfaces;

namespace MRLWMSC21Core.Business
{
    public class CycleCountBL : BaseBL, ICycleCountBL
    {
        private string _ClassCode = string.Empty;//"WMSCore_BL_005_";

        private CycleCountDAL oCCDAL = null;

        public CycleCountBL(int LoginUser, string ConnectionString) : base(LoginUser, ConnectionString)
        {
            oCCDAL = new CycleCountDAL(LoginUser, ConnectionString);
            _ClassCode = ExceptionHandling.GetClassExceptionCode(ExceptionHandling.ExcpConstants_API_BusinessLayer.CycleCountBL);
        }

        ~CycleCountBL()
        {
            oCCDAL = null;
        }


        public Location BlockLocationForCycleCount(Location oLocation)
        {

            try
            {
                Location _oLocationResponse = oCCDAL.BlockLocationForCycleCount(oLocation);

                if (_oLocationResponse == null)
                    throw new Exception("An error has occoured while populating Location Response for Block Location.");
                else if (!_oLocationResponse.IsBinLocation)
                    throw new WMSExceptionMessage() { WMSExceptionCode = "WMC_CC_BL_0004", WMSMessage = ErrorMessages.WMC_CC_BL_0004, ShowAsError = true };
                else if (_oLocationResponse.IsBlockedForCycleCount && _oLocationResponse.LocationBlockedByUserID != LoggedInUserID)
                    throw new WMSExceptionMessage() { WMSExceptionCode = "WMC_CC_BL_0005", WMSMessage = ErrorMessages.WMC_CC_BL_0005, ShowAsError = true };

                return _oLocationResponse;
               
            }
            catch (WMSExceptionMessage excp)
            {
                throw excp;
            }
            catch (Exception excp)
            {
                ExceptionData oExcpData = new ExceptionData();
                oExcpData.AddInputs("oLocation", oLocation);

                ExceptionHandling.LogException(excp, _ClassCode + "001", oExcpData);
                throw new WMSExceptionMessage() { WMSExceptionCode = ErrorMessages.WMSExceptionCode, WMSMessage = ErrorMessages.WMSExceptionMessage, ShowAsCriticalError = true };
            }
        }

        public Inventory CaptureCycleCount(CycleCount oCycleCount, Inventory oInventory, int UserID)
        {

            try
            {
                bool _bIsStockTakeMode = false;
                if (oCycleCount == null || oCycleCount.CycleCountCode == null)
                    throw new WMSExceptionMessage() { WMSExceptionCode = "WMC_HK_BL_007", WMSMessage = ErrorMessages.WMC_HK_BL_007, ShowAsError = true };

                List<CycleCount> lCycleCounts = oCCDAL.FetchCycleCounts(new SearchCriteria());
                LocationDAL _oLocationDAL = new LocationDAL(LoggedInUserID, ConnectionString);
                List<Location> _lLocations = _oLocationDAL.GetLocations(new SearchCriteria() { LocationCode = oInventory.LocationCode });


                foreach (CycleCount oCC in lCycleCounts)
                {
                    if ((oCC.CycleCountCode.Equals(oInventory.ReferenceDocumentNumber) || oCC.CycleCountCode.Equals(oCycleCount.CycleCountCode)) && oCC.IsStockTakeCycleCount)
                        _bIsStockTakeMode = true;
                }

                if (_lLocations == null)
                    throw new Exception("Get Locations returned NULL.");
                else if (_lLocations.Count != 1)
                    throw new WMSExceptionMessage() { WMSExceptionCode = "WMC_CC_BL_0004", WMSMessage = ErrorMessages.WMC_CC_BL_0004, ShowAsError = true };

                List<Inventory> lInventoryCaptured = oCCDAL.FetchInventoryCapturedAtCycleCount(oInventory);
                
                if (lInventoryCaptured == null)
                    throw new Exception("The Material Information List returned NULL.");
                else if (lInventoryCaptured.Count > 0)
                {
                    if (!_bIsStockTakeMode)
                        throw new WMSExceptionMessage() { WMSExceptionCode = "WMC_CC_BL_0007", WMSMessage = ErrorMessages.WMC_CC_BL_0007, ShowAsError = true };
                    else if (!oInventory.UserConfirmReDo)
                        throw new WMSExceptionMessage() { WMSExceptionCode = "WMC_CC_BL_0009", WMSMessage = ErrorMessages.WMC_CC_BL_0009, ShowUserConfirmDialogue = true };
                }

                List<Inventory> oCycleCountInventory = oCCDAL.CaptureCycleCount(oCycleCount, oInventory, UserID);

                if (oCycleCountInventory == null || oCycleCountInventory.Count == 0)
                    throw new WMSExceptionMessage() { WMSExceptionCode = ErrorMessages.WMSExceptionCode, WMSMessage = ErrorMessages.WMSExceptionMessage, ShowAsCriticalError = true };
                else
                {
                    return oCycleCountInventory[0];
                }
            }
            catch (WMSExceptionMessage excp)
            {
                throw excp;
            }
            catch (Exception excp)
            {
                ExceptionData oExcpData = new ExceptionData();
                oExcpData.AddInputs("oCycleCount", oCycleCount);
                oExcpData.AddInputs("oInventory", oInventory);
                oExcpData.AddInputs("UserID", UserID);

                ExceptionHandling.LogException(excp, _ClassCode + "002", oExcpData);
                throw new WMSExceptionMessage() { WMSExceptionCode = ErrorMessages.WMSExceptionCode, WMSMessage = ErrorMessages.WMSExceptionMessage, ShowAsCriticalError = true };
            }
        }

        public List<CycleCount> FetchActiveCycleCountList(SearchCriteria oCriteria)
        {

            try
            {
                List<CycleCount> lCycleCounts = new List<CycleCount>();

                lCycleCounts = oCCDAL.FetchCycleCounts(oCriteria);

                return lCycleCounts;
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

        public Location FetchNextLocationForCycleCount(CycleCount oCycleCount)
        {


            try
            {

                return oCCDAL.FetchNextLocationForCycleCount(oCycleCount);
            }
            catch (WMSExceptionMessage excp)
            {
                throw excp;
            }
            catch (Exception excp)
            {
                ExceptionData oExcpData = new ExceptionData();
                oExcpData.AddInputs("oCycleCount", oCycleCount);

                ExceptionHandling.LogException(excp, _ClassCode + "004", oExcpData);
                throw new WMSExceptionMessage() { WMSExceptionCode = ErrorMessages.WMSExceptionCode, WMSMessage = ErrorMessages.WMSExceptionMessage, ShowAsCriticalError = true };
            }
        }

        public bool MarkBinComplete(Location oLocation)
        {

            try
            {
                return oCCDAL.ClearLocationBlock(oLocation);
            }
            catch (WMSExceptionMessage excp)
            {
                throw excp;
            }
            catch (Exception excp)
            {
                ExceptionData oExcpData = new ExceptionData();
                oExcpData.AddInputs("oLocation", oLocation);

                ExceptionHandling.LogException(excp, _ClassCode + "005", oExcpData);
                throw new WMSExceptionMessage() { WMSExceptionCode = ErrorMessages.WMSExceptionCode, WMSMessage = ErrorMessages.WMSExceptionMessage, ShowAsCriticalError = true };
            }
        }

        public bool ValidateLocationBoxQuantity(Location oLocation, Inventory oInventory)
        {

            try
            {
                decimal _Result = oCCDAL.ValidateLocationBoxQuantity(oLocation, oInventory);


                if (_Result == oInventory.Quantity)
                {
                    // Release Location Block as Box quantity has matched. 

                    bool _ResultRelease = oCCDAL.ClearLocationBlock(oLocation);
                    return true;
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
                oExcpData.AddInputs("oLocation", oLocation);
                oExcpData.AddInputs("oInventory", oInventory);

                ExceptionHandling.LogException(excp, _ClassCode + "006", oExcpData);
                throw new WMSExceptionMessage() { WMSExceptionCode = ErrorMessages.WMSExceptionCode, WMSMessage = ErrorMessages.WMSExceptionMessage, ShowAsCriticalError = true };
            }
        }


        public bool FlagLocationForCycleCount(Location oLocation)
        {

            try
            {
                bool _Result = oCCDAL.FlagLocationForCycleCount(oLocation);

                return _Result;
            }
            catch (WMSExceptionMessage excp)
            {
                throw excp;
            }
            catch (Exception excp)
            {
                ExceptionData oExcpData = new ExceptionData();
                oExcpData.AddInputs("oLocation", oLocation);

                ExceptionHandling.LogException(excp, _ClassCode + "007", oExcpData);
                throw new WMSExceptionMessage() { WMSExceptionCode = ErrorMessages.WMSExceptionCode, WMSMessage = ErrorMessages.WMSExceptionMessage, ShowAsCriticalError = true };
            }
        }

        public bool MarkBinComplete(Location oLocation, CycleCount oCyclecount, Inventory oInventory)
        {

            try
            {
                return oCCDAL.ClearLocationBlock(oLocation, oCyclecount, oInventory, true);
            }
            catch (WMSExceptionMessage excp)
            {
                throw excp;
            }
            catch (Exception excp)
            {
                ExceptionData oExcpData = new ExceptionData();
                oExcpData.AddInputs("oLocation", oLocation);

                ExceptionHandling.LogException(excp, _ClassCode + "005", oExcpData);
                throw new WMSExceptionMessage() { WMSExceptionCode = ErrorMessages.WMSExceptionCode, WMSMessage = ErrorMessages.WMSExceptionMessage, ShowAsCriticalError = true };
            }
        }

    }
}
