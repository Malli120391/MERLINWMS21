using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MRLWMSC21Core.Entities;
using MRLWMSC21Core.Library;
using System.Data;
using System.Data.SqlClient;

namespace MRLWMSC21Core.DataAccess
{
    public class CycleCountDAL : BaseDAL
    {
        private string _ClassCode = "WMSCore_DAL_005_";

        public CycleCountDAL(int LoginUser, string ConnectionString) : base(LoginUser, ConnectionString)
        {

            _ClassCode = ExceptionHandling.GetClassExceptionCode(ExceptionHandling.ExcpConstants_API_DataLayer.CycleCountDAL);
        }

        public List<CycleCount> FetchCycleCounts(SearchCriteria oCriteria)
        {

            try
            {
                List<CycleCount> _lActiveCycleCounts = new List<CycleCount>();

                string _sSQL = "SELECT * FROM CCM_TRN_CycleCounts AS CTC WHERE CTC.PlannedStart < GETDATE() AND ISNULL(IsCompleted,0) = 0 AND IsInitiated = 1 AND ISActive=1 and Isdeleted=0";

                DataSet _dsResults = FillDataSet(_sSQL);

                if (_dsResults == null)
                    throw new Exception("Could not Fetch Cycle Count List. CycleCountDAL has encountered an Error.");
                else
                {
                    if (_dsResults.Tables.Count > 0)
                    {
                        foreach (DataRow _drCC in _dsResults.Tables[0].Rows)
                        {
                            CycleCount _oCycleCount = new CycleCount()
                            {
                                AccountCycleCountName = _drCC["CycleCountCode"].ToString(),

                                CCM_TRN_CycleCount_ID = ConversionUtility.ConvertToInt(_drCC["CCM_TRN_CycleCount_ID"].ToString()),
                                CCM_MST_CycleCountStatus_ID = ConversionUtility.ConvertToInt(_drCC["CCM_MST_CycleCountStatus_ID"].ToString()),
                                CCM_MST_CycleCount_ID = ConversionUtility.ConvertToInt(_drCC["CCM_MST_CycleCount_ID"].ToString()),
                                CCM_CNF_AccountCycleCount_ID = ConversionUtility.ConvertToInt(_drCC["CCM_CNF_AccountCycleCount_ID"].ToString()),
                                CreatedBy = ConversionUtility.ConvertToInt(_drCC["CreatedBy"].ToString()),
                                UpdatedBy = ConversionUtility.ConvertToInt(_drCC["UpdatedBy"].ToString()),
                                SeqNo = ConversionUtility.ConvertToInt(_drCC["SeqNo"].ToString()),

                                AvailableQuantity = ConversionUtility.ConvertToDecimal(_drCC["AvailableQuantity"].ToString()),
                                FoundQuantity = ConversionUtility.ConvertToDecimal(_drCC["FoundQuantity"].ToString()),
                                LostQuantity = ConversionUtility.ConvertToDecimal(_drCC["LostQuantity"].ToString()),

                                PlannedEnd = ConversionUtility.ConvertToDateTime(_drCC["PlannedEnd"].ToString()),
                                PlannedStart = ConversionUtility.ConvertToDateTime(_drCC["PlannedStart"].ToString()),
                                CreatedOn = ConversionUtility.ConvertToDateTime(_drCC["CreatedOn"].ToString()),
                                UpdatedOn = ConversionUtility.ConvertToDateTime(_drCC["UpdatedOn"].ToString()),
                                CompletionTimestamp = ConversionUtility.ConvertToDateTime(_drCC["CompletionTimestamp"].ToString()),
                                InitiatedTimestamp = ConversionUtility.ConvertToDateTime(_drCC["InitiatedTimestamp"].ToString()),

                                IsCompleted = ConversionUtility.ConvertToBool(_drCC["IsCompleted"].ToString()),
                                IsInitiated = ConversionUtility.ConvertToBool(_drCC["IsInitiated"].ToString()),

                                IsABCCycleCount = (ConversionUtility.ConvertToInt(_drCC["CCM_MST_CycleCount_ID"].ToString()) == 1 ? true : false),
                                IsStockTakeCycleCount = (ConversionUtility.ConvertToInt(_drCC["CCM_MST_CycleCount_ID"].ToString())==5 ? true : false),
                                IsCCForLocationsWithMovement = (ConversionUtility.ConvertToInt(_drCC["CCM_MST_CycleCount_ID"].ToString()) == 4 ? true : false),
                                IsStandardCycleCount = (ConversionUtility.ConvertToInt(_drCC["CCM_MST_CycleCount_ID"].ToString()) == 3 ? true : false),

                                InitiationRemarks = _drCC["InitiationRemarks"].ToString(),
                                CompletionRemarks = _drCC["CompletionRemarks"].ToString(),
                                CycleCountCode = _drCC["CycleCountCode"].ToString()

                            };


                            _lActiveCycleCounts.Add(_oCycleCount);

                        }
                    }
                }

                return _lActiveCycleCounts;
            }
            catch (WMSExceptionMessage excp)
            {
                throw excp;
            }
            catch (Exception excp)
            {
                ExceptionData oExcpData = new ExceptionData();
                oExcpData.AddInputs("oCriteria", oCriteria);

                ExceptionHandling.LogException(excp, _ClassCode + "001", oExcpData);
                throw new WMSExceptionMessage() { WMSExceptionCode = ErrorMessages.WMSExceptionCode, WMSMessage = ErrorMessages.WMSExceptionMessage, ShowAsCriticalError = true };
            }

        }


        public Location FetchNextLocationForCycleCount(CycleCount oCycleCount)
        {

            try
            {
                Location oLocation = new Location();

                string _sSQL = "EXEC USP_GET_CCM_TRN_LocationFilterQuery";

                List<SqlParameter> lSQLParams = new List<SqlParameter>();

                if (oCycleCount.CCM_TRN_CycleCount_ID > 0)
                {
                    SqlParameter _oParam = new SqlParameter("@CCM_TRN_CycleCount_ID", SqlDbType.Int);
                    _oParam.Value = oCycleCount.CCM_TRN_CycleCount_ID;

                    lSQLParams.Add(_oParam);
                }
                else
                {
                    SqlParameter _oParam = new SqlParameter("@CCM_TRN_CycleCount_ID", SqlDbType.Int);
                    _oParam.Value = 0;

                    lSQLParams.Add(_oParam);
                }
                if (oCycleCount.AccountCycleCountName != "")
                {
                    SqlParameter _oParam = new SqlParameter("@CycleCountCode", SqlDbType.NVarChar);
                    _oParam.Value = oCycleCount.AccountCycleCountName;

                    lSQLParams.Add(_oParam);
                }

                if (oCycleCount.CCM_CNF_AccountCycleCount_ID > 0)
                {
                    SqlParameter _oParam = new SqlParameter("@CCM_CNF_AccountCycleCount_ID", SqlDbType.Int);
                    _oParam.Value = oCycleCount.CCM_CNF_AccountCycleCount_ID;

                    lSQLParams.Add(_oParam);
                }
                if (LoggedInUserID != 0)
                {
                    SqlParameter _oParam = new SqlParameter("@LoggedInUserID", SqlDbType.Int);
                    _oParam.Value = LoggedInUserID;

                    lSQLParams.Add(_oParam);
                }

                lSQLParams.Add(new SqlParameter() { ParameterName = "@IsHH", Value = 1 });

                DataSet _dsResults = FillDataSet(_sSQL, lSQLParams);

                if (_dsResults == null)
                    throw new Exception("Fetch Next Location for Cycle Count has encountered an Error. Data Set Returned NULL.");
                else
                {
                    if (_dsResults.Tables.Count > 0 && _dsResults.Tables[0].Rows.Count == 1)
                    {

                        Location _oLocation = new Location()
                        {
                            LocationCode = _dsResults.Tables[0].Rows[0]["DisplayLocationCode"].ToString()
                        };
                        oLocation = _oLocation;
                    }
                    else
                        throw new WMSExceptionMessage() { WMSExceptionCode = "WMC_CC_BL_0001", WMSMessage = ErrorMessages.WMC_CC_BL_0001, ShowAsSuccess = true };

                }

                return oLocation;
            }
            catch (WMSExceptionMessage excp)
            {
                throw excp;
            }
            catch (Exception excp)
            {
                ExceptionData oExcpData = new ExceptionData();
                oExcpData.AddInputs("oCycleCount", oCycleCount);

                ExceptionHandling.LogException(excp, _ClassCode + "002", oExcpData);
                throw new WMSExceptionMessage() { WMSExceptionCode = ErrorMessages.WMSExceptionCode, WMSMessage = ErrorMessages.WMSExceptionMessage, ShowAsCriticalError = true };
            }
        }

        public bool BlockLocationForCycleCount1(Location oLocation)
        {

            try
            {
                if (oLocation == null || oLocation.BlockedForCycleCount == null)
                    throw new WMSExceptionMessage() { };


                string _sSQL = "UPDATE LM SET LM.IsBlockedForCycleCount = 1, LM.CCM_TRN_CycleCount_ID = ( SELECT CCM_TRN_CycleCount_ID FROM CCM_TRN_CycleCounts WHERE CycleCountCode = '" + oLocation.BlockedForCycleCount + "') FROM INV_Location AS LM WHERE LM.LocationID = " + oLocation.LocationID.ToString() + " OR LM.DisplayLocationCode LIKE '%" + (oLocation.SystemLocationCode == null ? oLocation.LocationCode : oLocation.SystemLocationCode) + "%' OR LM.Location LIKE '%" + (oLocation.SystemLocationCode == null ? oLocation.LocationCode : oLocation.SystemLocationCode) + "%'";


                int _Result = ExecuteNonQuery(_sSQL);

                if (_Result > 0)
                    return true;
                else
                    throw new WMSExceptionMessage()
                    {
                        WMSExceptionCode = ErrorMessages.WMSExceptionCode,
                        WMSMessage = ErrorMessages.WMSExceptionMessage,
                        ShowAsCriticalError = true
                    };

            }
            catch (WMSExceptionMessage excp)
            {
                throw excp;
            }
            catch (Exception excp)
            {
                ExceptionData oExcpData = new ExceptionData();
                oExcpData.AddInputs("oLocation", oLocation);

                ExceptionHandling.LogException(excp, _ClassCode + "003", oExcpData);
                throw new WMSExceptionMessage() { WMSExceptionCode = ErrorMessages.WMSExceptionCode, WMSMessage = ErrorMessages.WMSExceptionMessage, ShowAsCriticalError = true };
            }
        }

        

        public Location BlockLocationForCycleCount(Location oLocation)
        {

            try
            {
                if (oLocation == null || oLocation.LocationCode == null)
                    throw new WMSExceptionMessage() { WMSExceptionCode = "WMC_CC_BL_0002", WMSMessage = ErrorMessages.WMC_CC_BL_0002, ShowAsError = true };
                else if (oLocation.BlockedForCycleCount == null)
                    throw new WMSExceptionMessage() { WMSExceptionCode = "WMC_CC_BL_0003", WMSMessage = ErrorMessages.WMC_CC_BL_0003, ShowAsError = true };
                

                    string _sSQL = " EXEC [dbo].[USP_API_CCM_ValidateLocationAndBlockForCC] ";

                List<SqlParameter> lParams = new List<SqlParameter>();

                SqlParameter oLocatio = new SqlParameter("@LocationCode", SqlDbType.NVarChar, 50);
                oLocatio.Value = oLocation.LocationCode;

                lParams.Add(oLocatio);


                SqlParameter oCC = new SqlParameter("@CycleCountCode", SqlDbType.NVarChar, 50);
                oCC.Value = oLocation.BlockedForCycleCount;

                lParams.Add(oCC);


                SqlParameter oLoggedInUser = new SqlParameter("@UserID", SqlDbType.Int);
                oLoggedInUser.Value = LoggedInUserID;

                lParams.Add(oLoggedInUser);

                DataSet _dsResult = FillDataSet(_sSQL, lParams);

                Location oCCResponseLocation = null;

                if (_dsResult == null)
                    throw new Exception("Location Block DataSet is returned as NULL");
                else if (_dsResult.Tables.Count == 0 || _dsResult.Tables[0].Rows.Count == 0)
                    throw new WMSExceptionMessage() { WMSExceptionCode = "WMC_CC_BL_0008", WMSMessage = ErrorMessages.WMC_CC_BL_0008, ShowAsError = true };
                else
                {
                    oCCResponseLocation = new Location()
                    {

                        TotalSystemStockAtLocation = ConversionUtility.ConvertToDecimal(_dsResult.Tables[0].Rows[0]["TotalSystemStockAtLocation"].ToString()),
                        TotalSystemStockAllLocations = ConversionUtility.ConvertToDecimal(_dsResult.Tables[0].Rows[0]["TotalSystemStockAllLocations"].ToString()),
                        TotalInventoryQuantityScannedAtLocation = ConversionUtility.ConvertToDecimal(_dsResult.Tables[0].Rows[0]["TotalInventoryQuantityScannedAtLocation"].ToString()),
                        TotalNoOfLocationsToScan= ConversionUtility.ConvertToDecimal(_dsResult.Tables[0].Rows[0]["TotalNoOfLocationsToScan"].ToString()),
                        TotalScannedSKUQuantity=ConversionUtility.ConvertToDecimal(_dsResult.Tables[0].Rows[0]["TotalInventoryQuantityScannedAtLocation"].ToString()),
                        
                        LocationBlockedByUserID= ConversionUtility.ConvertToInt(_dsResult.Tables[0].Rows[0]["InitiatedByUser"].ToString()),
                        LocationCode = _dsResult.Tables[0].Rows[0]["DisplayLocationCode"].ToString(), 
                        SystemLocationCode=_dsResult.Tables[0].Rows[0]["SystemLocationCode"].ToString(),

                        LocationID = ConversionUtility.ConvertToInt(_dsResult.Tables[0].Rows[0]["LocationID"].ToString()),

                        IsBlockedForCycleCount = ConversionUtility.ConvertToBool(_dsResult.Tables[0].Rows[0]["IsBlockedForCC"].ToString()),
                        IsBinLocation = ConversionUtility.ConvertToBool(_dsResult.Tables[0].Rows[0]["IsBINLocation"].ToString())

                    };
                }

                return oCCResponseLocation;
            }
            catch (WMSExceptionMessage excp)
            {
                throw excp;
            }
            catch (Exception excp)
            {
                ExceptionData oExcpData = new ExceptionData();
                oExcpData.AddInputs("oLocation", oLocation);

                ExceptionHandling.LogException(excp, _ClassCode + "003", oExcpData);
                throw new WMSExceptionMessage() { WMSExceptionCode = ErrorMessages.WMSExceptionCode, WMSMessage = ErrorMessages.WMSExceptionMessage, ShowAsCriticalError = true };
            }
        }


        public decimal ValidateLocationBoxQuantity(Location oLocation, Inventory oInventory)
        {

            try
            {
                if (oLocation == null)
                    throw new WMSExceptionMessage() { };


                string _sSQL = "SELECT ISNULL(SUM(ASD.AvailableQty +  ISNULL(IBOHQuantity,0) +  ISNULL(OBOHQuantity,0)),0) AS Quantity FROM INV_ActiveStockDetails AS ASD WHERE LocationID = (SELECT LocationID FROM INV_Location Loc WHERE Loc.LocationID = " + oLocation.LocationID.ToString() + " OR Loc.DisplayLocationCode LIKE '%" + (oLocation.SystemLocationCode == null ? oLocation.LocationCode : oLocation.SystemLocationCode) + "%' OR Loc.Location  LIKE '%" + (oLocation.SystemLocationCode == null ? oLocation.LocationCode : oLocation.SystemLocationCode) + "%')";

                //List<SqlParameter> lSQLParams = new List<SqlParameter>();

                //if (oLocation.BlockedForCycleCountID > 0)
                //{
                //    SqlParameter _oParam = new SqlParameter("@CCM_TRN_CycleCount_ID", SqlDbType.Int);
                //    _oParam.Value = oLocation.BlockedForCycleCountID;

                //    lSQLParams.Add(_oParam);
                //}
                //else
                //{
                //    SqlParameter _oParam = new SqlParameter("@CCM_TRN_CycleCount_ID", SqlDbType.Int);
                //    _oParam.Value = null;

                //    lSQLParams.Add(_oParam);
                //}

                //if (oLocation.SystemLocationCode.Length > 0)
                //{
                //    SqlParameter _oParam = new SqlParameter("@LocationCode", SqlDbType.NVarChar, 25);
                //    _oParam.Value = oLocation.SystemLocationCode;

                //    lSQLParams.Add(_oParam);
                //}
                //else if (oLocation.LocationCode.Length > 0)
                //{
                //    SqlParameter _oParam = new SqlParameter("@LocationCode", SqlDbType.NVarChar, 25);
                //    _oParam.Value = oLocation.LocationCode;

                //    lSQLParams.Add(_oParam);
                //}
                //else
                //{
                //    SqlParameter _oParam = new SqlParameter("@LocationCode", SqlDbType.NVarChar, 25);
                //    _oParam.Value = null;

                //    lSQLParams.Add(_oParam);
                //}

                //if (oLocation.LocationID > 0)
                //{
                //    SqlParameter _oParam = new SqlParameter("@LocationID", SqlDbType.Int);
                //    _oParam.Value = oLocation.LocationID;

                //    lSQLParams.Add(_oParam);
                //}
                //else
                //{
                //    SqlParameter _oParam = new SqlParameter("@LocationID", SqlDbType.Int);
                //    _oParam.Value = null;

                //    lSQLParams.Add(_oParam);
                //}

                decimal _Result = ConversionUtility.ConvertToDecimal(ExecuteScalar(_sSQL));

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
                oExcpData.AddInputs("oInventory", oInventory);

                ExceptionHandling.LogException(excp, _ClassCode + "004", oExcpData);

                throw new WMSExceptionMessage()
                {
                    WMSExceptionCode = ErrorMessages.WMSExceptionCode,
                    WMSMessage = ErrorMessages.WMSExceptionMessage,
                    ShowAsCriticalError = true
                };
            }
        }


        public bool ClearLocationBlock(Location oLocation)
        {

            try
            {
                if (oLocation == null)
                    throw new WMSExceptionMessage() { };

                string _sSQL = "UPDATE LM SET LM.IsBlockedForCycleCount = 0 FROM INV_Location AS LM WHERE LM.LocationID = " + oLocation.LocationID.ToString() + " OR LM.DisplayLocationCode LIKE '%" + (oLocation.SystemLocationCode == null ? oLocation.LocationCode : oLocation.SystemLocationCode) + "%' OR LM.Location LIKE '%" + (oLocation.SystemLocationCode == null ? oLocation.LocationCode : oLocation.SystemLocationCode) + "%'";



                int _Result = ExecuteNonQuery(_sSQL);

                if (_Result > 0)
                    return true;
                else
                    throw new Exception("Failed to Clear cycle count block on location");

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

                throw new WMSExceptionMessage()
                {
                    WMSExceptionCode = ErrorMessages.WMSExceptionCode,
                    WMSMessage = ErrorMessages.WMSExceptionMessage,
                    ShowAsCriticalError = true
                };
            }
        }


        public List<Inventory> CaptureCycleCount(CycleCount oCycleCount, Inventory oInventory, int UserID)
        {

            try
            {
                if (oCycleCount == null || oInventory == null)
                    throw new WMSExceptionMessage() { };

                string _sSQL = "EXEC [dbo].[USP_API_CaptureCycleCountInventory]";

                List<SqlParameter> lSQLParams = new List<SqlParameter>();

                List<Inventory> lInventory = new List<Inventory>();

                if (oCycleCount.CycleCountCode != null)
                {
                    SqlParameter _oParam = new SqlParameter("@CycleCountCode", SqlDbType.NVarChar, 30);
                    _oParam.Value = oCycleCount.CycleCountCode;

                    lSQLParams.Add(_oParam);
                }

                if (UserID > 0)
                {
                    SqlParameter _oParam = new SqlParameter("@UserID", SqlDbType.Int);
                    _oParam.Value = UserID;

                    lSQLParams.Add(_oParam);
                }

                if (oInventory.LocationCode != null)
                {
                    SqlParameter _oParam = new SqlParameter("@LocationCode", SqlDbType.NVarChar, 20);
                    _oParam.Value = oInventory.LocationCode;

                    lSQLParams.Add(_oParam);
                }


                if (oInventory.ContainerCode != null)
                {
                    SqlParameter _oParam = new SqlParameter("@PalletCode", SqlDbType.NVarChar, 20);
                    _oParam.Value = oInventory.ContainerCode;

                    lSQLParams.Add(_oParam);
                }

                if (oInventory.RSN != null)
                {
                    SqlParameter _oParam = new SqlParameter("@RSNNumber", SqlDbType.NVarChar, 20);
                    _oParam.Value = oInventory.RSN;

                    lSQLParams.Add(_oParam);
                }

                if (oInventory.MaterialCode != null)
                {
                    SqlParameter _oParam = new SqlParameter("@MaterialCode", SqlDbType.NVarChar, 20);
                    _oParam.Value = oInventory.MaterialCode;

                    lSQLParams.Add(_oParam);
                }

                if (oInventory.Quantity > 0)
                {
                    SqlParameter _oParam = new SqlParameter("@Quantity", SqlDbType.Decimal);
                    _oParam.Value = oInventory.Quantity;

                    lSQLParams.Add(_oParam);
                }

                if (oInventory.BatchNumber != null && !string.IsNullOrEmpty(oInventory.BatchNumber))
                {
                    SqlParameter _oParam = new SqlParameter("@BatchNumber", SqlDbType.NVarChar, 30);
                    _oParam.Value = oInventory.BatchNumber;

                    lSQLParams.Add(_oParam);
                }


                if (oInventory.MRP > 0)
                {
                    SqlParameter _oParam = new SqlParameter("@MRP", SqlDbType.Decimal);
                    _oParam.Value = oInventory.MRP;

                    lSQLParams.Add(_oParam);
                }

                if (oInventory.MOP > 0)
                {
                    SqlParameter _oParam = new SqlParameter("@MOP", SqlDbType.Decimal);
                    _oParam.Value = oInventory.MOP;

                    lSQLParams.Add(_oParam);
                }


                if (oInventory.Color != null)
                {
                    SqlParameter _oParam = new SqlParameter("@ColorCode", SqlDbType.NVarChar, 50);
                    _oParam.Value = oInventory.Color;

                    lSQLParams.Add(_oParam);
                }


                if (oInventory.StorageLocation != null)
                {
                    SqlParameter _oParam = new SqlParameter("@StorageLocation", SqlDbType.NVarChar, 50);
                    _oParam.Value = oInventory.StorageLocation;

                    lSQLParams.Add(_oParam);
                }

                DataSet _dsCycleCount = FillDataSet(_sSQL, lSQLParams);

                if (_dsCycleCount == null)
                    throw new Exception("Method : Capture Cycle Count has returned a NULL Dataset.");
                else
                {
                    if (_dsCycleCount.Tables.Count > 0)
                    {
                        foreach (DataRow _drInventory in _dsCycleCount.Tables[0].Rows)
                        {
                            Inventory _oInventory = new Inventory()
                            {
                                BatchNumber = _drInventory["BatchNo"].ToString(),
                                RSN = _drInventory["SerialNo"].ToString(),

                                MonthOfMfg = ConversionUtility.ConvertToInt(_drInventory["MfgDateMonth"].ToString()),
                                YearOfMfg = ConversionUtility.ConvertToInt(_drInventory["MfgDateYear"].ToString()),
                                MaterialTransactionID = ConversionUtility.ConvertToInt(_drInventory["MaterialTransactionID"].ToString()),
                                MaterialMasterID = ConversionUtility.ConvertToInt(_drInventory["MaterialMasterID"].ToString()),

                                ContainerID = ConversionUtility.ConvertToInt(_drInventory["ContainerID"].ToString()),
                                ContainerCode = _drInventory["ContainerCode"].ToString(),

                                IsFinishedGoods = _drInventory["SerialNo"].ToString().Length > 0 ? true : false,
                                IsRawMaterial = false,
                                IsConsumables = false,

                                MRP = ConversionUtility.ConvertToDecimal(_drInventory["MRP"].ToString()),
                                MOP = ConversionUtility.ConvertToInt(_drInventory["MOP"].ToString()),

                                MaterialCode = _drInventory["MCode"].ToString(),

                                LocationCode = _drInventory["DisplayLocationCode"].ToString(),
                                Color = _drInventory["ColorName"].ToString(),
                                ColorID = ConversionUtility.ConvertToInt(_drInventory["ColorAttributeID"].ToString()),

                                DockGoodsMovementID = ConversionUtility.ConvertToInt(_drInventory["DockGoodsMovementID"].ToString()),
                                LocationID = ConversionUtility.ConvertToInt(_drInventory["LocationID"].ToString()),
                                StorageLocation = _drInventory["StorageLocation"].ToString()

                            };

                            lInventory.Add(_oInventory);

                        }
                    }
                }

                return lInventory;

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

                ExceptionHandling.LogException(excp, _ClassCode + "006", oExcpData);

                throw new WMSExceptionMessage()
                {
                    WMSExceptionCode = ErrorMessages.WMSExceptionCode,
                    WMSMessage = ErrorMessages.WMSExceptionMessage,
                    ShowAsCriticalError = true
                };
            }
        }


        public bool FlagLocationForCycleCount(Location oLocation)
        {

            try
            {
                if (oLocation == null)
                    throw new WMSExceptionMessage() { };


                string _sSQL = "UPDATE LM SET LM.IsFlaggedForCC = 1 FROM INV_Location AS LM WHERE LM.LocationID = " + oLocation.LocationID.ToString() + " OR LM.DisplayLocationCode LIKE '%" + (oLocation.SystemLocationCode == null ? oLocation.LocationCode : oLocation.SystemLocationCode) + "%' OR LM.Location LIKE '%" + (oLocation.SystemLocationCode == null ? oLocation.LocationCode : oLocation.SystemLocationCode) + "%'";


                int _Result = ExecuteNonQuery(_sSQL);

                if (_Result > 0)
                    return true;
                else
                    throw new WMSExceptionMessage()
                    {
                        WMSExceptionCode = ErrorMessages.WMSExceptionCode,
                        WMSMessage = ErrorMessages.WMSExceptionMessage,
                        ShowAsCriticalError = true
                    };

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


        public List<Inventory> FetchInventoryCapturedAtCycleCount(Inventory oInventory)
        {

            try
            {
                if (oInventory == null)
                    throw new WMSExceptionMessage() { WMSExceptionCode = "WMC_CC_BL_0006", WMSMessage = ErrorMessages.WMC_CC_BL_0006, ShowAsError = true };
                else if (oInventory.ReferenceDocumentNumber == null)
                    throw new WMSExceptionMessage() { WMSExceptionCode = "WMC_HK_BL_007", WMSMessage = ErrorMessages.WMC_HK_BL_007, ShowAsError = true };

                string _sSQL = "EXEC [dbo].[USP_API_CCM_GetCycleCountCapturedInventory]";

                List<SqlParameter> lParams = new List<SqlParameter>();

                SqlParameter oParam = new SqlParameter("@CycleCountCode", SqlDbType.NVarChar, 50);
                oParam.Value = oInventory.ReferenceDocumentNumber;

                lParams.Add(oParam);

                if (oInventory.RSN != null)
                {
                    SqlParameter oParamRSN = new SqlParameter("@RSNNumber", SqlDbType.NVarChar, 50);
                    oParamRSN.Value = oInventory.RSN;

                    lParams.Add(oParamRSN);
                }

                DataSet _dsResult = FillDataSet(_sSQL, lParams);
                List<Inventory> lInventory = new List<Inventory>();

                if (_dsResult == null)
                    throw new Exception("Fetch Inventory Captured at Cycle Count returned NULL Data set.");
                else if (_dsResult.Tables.Count > 0)
                {
                    foreach (DataRow _drInventory in _dsResult.Tables[0].Rows)
                    {
                        Inventory oInv = new Inventory()
                        {
                            ReferenceDocumentNumber = _drInventory["CycleCountCode"].ToString(),
                            ReferenceDocumentID = ConversionUtility.ConvertToInt(_drInventory["CCM_TRN_CycleCount_ID"].ToString()),
                            MaterialMasterID = ConversionUtility.ConvertToInt(_drInventory["MaterialMasterID"].ToString()),
                            DisplayLocationCode = _drInventory["DisplayLocationCode"].ToString(),
                            LocationID = ConversionUtility.ConvertToInt(_drInventory["LocationID"].ToString()),
                            AvailableQuantity = ConversionUtility.ConvertToDecimal(_drInventory["MaterialMasterID"].ToString()),
                            Quantity = ConversionUtility.ConvertToDecimal(_drInventory["MaterialMasterID"].ToString())
                        };

                        lInventory.Add(oInv);
                    }
                }

                return lInventory;

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

        public bool ClearLocationBlock(Location oLocation, CycleCount oCycleCount, Inventory oInventory, bool isConsolidationRequired)
        {
            try
            {
                if (oLocation == null)
                    throw new WMSExceptionMessage() { };

                string _sSQL = "UPDATE LM SET LM.IsBlockedForCycleCount = 0 FROM INV_Location AS LM WHERE LM.LocationID = " + oLocation.LocationID.ToString() + " OR LM.DisplayLocationCode LIKE '%" + (oLocation.SystemLocationCode == null ? oLocation.LocationCode : oLocation.SystemLocationCode) + "%' OR LM.Location LIKE '%" + (oLocation.SystemLocationCode == null ? oLocation.LocationCode : oLocation.SystemLocationCode) + "%'";

                string _sQuery = "UPDATE CCM_TRN_CycleCountInventoryPlan SET IsDeleted=1,IsScanned=1 WHERE CCM_CNF_AccountCycleCount_ID=(SELECT CCM_CNF_AccountCycleCount_ID FROM CCM_CNF_AccountCycleCounts WHERE IsActive=1 AND IsDeleted=0 AND AM_MST_Account_ID=" + oCycleCount.AccountID + " AND ISNULL(dbo.UDF_ParseAndReturnLocaleString(AccountCycleCountName, 'en'),'')='" + oCycleCount.AccountCycleCountName + "') AND LocationID = (SELECT LocationID FROM INV_Location WHERE IsActive=1 AND IsDeleted=0 AND DisplayLocationCode='" + oLocation.LocationCode + "')";


                int _Result = ExecuteNonQuery(_sSQL);
                int _ResultPaln = ExecuteNonQuery(_sQuery);
                if (isConsolidationRequired)
                {
                    ConsloidateCycleCountInventory(oCycleCount, oInventory);

                }
                //else
                //{
                //    StringBuilder _sSqlBuilder = new StringBuilder();
                //    _sSqlBuilder.Append("EXEC [dbo].[USP_API_CCM_UpdateBinCompleteStatus]");
                //    _sSqlBuilder.Append("@CycleCountCode=" + base.SQuote(oCycleCount.CycleCountCode));
                //    _sSqlBuilder.Append(",@LocationCode=" + base.SQuote(oLocation.LocationCode));
                //    _sSqlBuilder.Append(",@ActivityByUser=" + base.LoggedInUserID);
                //    ExecuteNonQuery(_sSqlBuilder.ToString());
                //    oInventory.LocationCode = oLocation.LocationCode;

                //}


                if (_Result > 0)
                    return true;
                else
                    throw new Exception("Failed to Clear cycle count block on location");

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

                throw new WMSExceptionMessage()
                {
                    WMSExceptionCode = ErrorMessages.WMSExceptionCode,
                    WMSMessage = ErrorMessages.WMSExceptionMessage,
                    ShowAsCriticalError = true
                };
            }
        }
        public void ConsloidateCycleCountInventory(CycleCount oCycleCount, Inventory oInventory)
        {
            try
            {
                if (oCycleCount == null || oInventory == null)
                    throw new WMSExceptionMessage() { };

                string _sSQL = "EXEC [dbo].[USP_API_CCM_ConsolidateInventoryAfterCycleCount]";

                List<SqlParameter> lSQLParams = new List<SqlParameter>();

                List<Inventory> lInventory = new List<Inventory>();

                if (oCycleCount.CycleCountCode != null)
                {
                    SqlParameter _oParam = new SqlParameter("@CycleCountCode", SqlDbType.NVarChar, 30);
                    _oParam.Value = oCycleCount.CycleCountCode;

                    lSQLParams.Add(_oParam);
                }
                if (oInventory.LocationCode != null)
                {
                    SqlParameter _oParam = new SqlParameter("@LocationCode", SqlDbType.NVarChar, 20);
                    _oParam.Value = oInventory.LocationCode;

                    lSQLParams.Add(_oParam);
                }
                if (oCycleCount.AccountID != 0)
                {
                    SqlParameter _oParam = new SqlParameter("@AccountID", SqlDbType.NVarChar, 20);
                    _oParam.Value = oCycleCount.AccountID;

                    lSQLParams.Add(_oParam);
                }
                if (base.LoggedInUserID != 0)
                {

                    SqlParameter _oParam = new SqlParameter("@ActivityByUser", SqlDbType.Int);
                    _oParam.Value = base.LoggedInUserID;
                    lSQLParams.Add(_oParam);
                }
                DataSet _dsCycleCount = FillDataSet(_sSQL, lSQLParams);
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

                ExceptionHandling.LogException(excp, _ClassCode + "010", oExcpData);

                throw new WMSExceptionMessage()
                {
                    WMSExceptionCode = ErrorMessages.WMSExceptionCode,
                    WMSMessage = ErrorMessages.WMSExceptionMessage,
                    ShowAsCriticalError = true
                };
            }
        }

    }
}
