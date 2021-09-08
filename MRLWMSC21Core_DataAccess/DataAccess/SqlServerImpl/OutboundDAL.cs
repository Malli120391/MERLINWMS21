using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MRLWMSC21Core.Entities;
using MRLWMSC21Core.Library;
using System.Data;
using System.Data.SqlClient;
using WMSCore_BusinessEntities.Entities;

namespace MRLWMSC21Core.DataAccess
{
    public class OutboundDAL : BaseDAL
    {
        private string _ClassCode = "WMSCore_DAL_009_";

        public OutboundDAL(int LoginUser, string ConnectionString) : base(LoginUser, ConnectionString)
        {

            _ClassCode = ExceptionHandling.GetClassExceptionCode(ExceptionHandling.ExcpConstants_API_DataLayer.OutboundDAL);
        }

        public List<Picklist> GetPicklistsForPicking(SearchCriteria oCriteria)
        {


            try
            {
                List<Picklist> lPickList = new List<Picklist>();

                string _sSQL = "EXEC [dbo].[USP_API_FetchPickListsForPicking] ";

                List<SqlParameter> lParam = new List<SqlParameter>();

                SqlParameter oParamUser = new SqlParameter("@UserID", SqlDbType.Int);
                oParamUser.Value = oCriteria.LoggedInUserID;

                lParam.Add(oParamUser);

                if (oCriteria.PickListRefNo != null)
                {
                    SqlParameter oParam = new SqlParameter("@PickListRefNo", SqlDbType.NVarChar, 50);
                    oParam.Value = oCriteria.PickListRefNo;

                    lParam.Add(oParam);
                }

                DataSet _dsPickLists = base.FillDataSet(_sSQL, lParam);

                if(_dsPickLists!=null)
                {
                    if(_dsPickLists.Tables.Count>0)
                    {
                        foreach(DataRow _drPickList in _dsPickLists.Tables[0].Rows)
                        {
                            Picklist _oPickList = new Picklist()
                            {
                                PicklistHeaderID = ConversionUtility.ConvertToInt(_drPickList["PicklistHeaderID"].ToString()),
                                PickListRefNo = _drPickList["PickListRefNo"].ToString(),
                                DockLocationCode = _drPickList["LocationCode"].ToString(),
                                Dock_LocationID = ConversionUtility.ConvertToInt(_drPickList["Dock_LocationID"].ToString()),
                                TotalPicklistQuantity = ConversionUtility.ConvertToDecimal(_drPickList["PickListQuantity"].ToString()),
                                TotalPickedQuantity = ConversionUtility.ConvertToDecimal(_drPickList["PickedQuantity"].ToString()),

                                CustomerCode = _drPickList["CustomerCode"].ToString()
                            };

                            lPickList.Add(_oPickList);                           
                        }
                    }
                }

                return lPickList;
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

        public PickListInventory GetNextPicklistItemForPicking(Picklist oPicklist, SearchCriteria oCriteria)
        {

            try
            {
                PickListInventory oPickListInventory = new PickListInventory();

                DataSet _dsInventory = FetchPickListInformation(oPicklist, oCriteria.UserID, true);

                if (_dsInventory == null)
                {

                }
               
                else if (_dsInventory.Tables.Count > 0 && _dsInventory.Tables[0].Rows.Count>0)
                {
               
                  oPickListInventory = BindAndReturnInventory(_dsInventory.Tables[0].Rows[0]);
                    
                }
                else if(_dsInventory.Tables.Count==3)
                {
                    foreach (DataRow _drListInventory in _dsInventory.Tables[2].Rows)
                    {

                        if (_drListInventory["ErrorCode"].ToString() == "2")
                            throw new WMSExceptionMessage() { WMSExceptionCode = "EMC_OB_DAL_009", WMSMessage = ErrorMessages.EMC_OB_DAL_009, ShowAsError = true };

                    }

                }
                return oPickListInventory;
            }
            catch (WMSExceptionMessage excp)
            {
                throw excp;
            }
            catch (Exception excp)
            {
                ExceptionData oExcpData = new ExceptionData();
                oExcpData.AddInputs("oPicklist", oPicklist);

                ExceptionHandling.LogException(excp, _ClassCode + "003", oExcpData);
                throw new WMSExceptionMessage() { WMSExceptionCode = ErrorMessages.WMSExceptionCode, WMSMessage = ErrorMessages.WMSExceptionMessage, ShowAsCriticalError = true };
            }
        }

        public List<PickListInventory> FetchPicklistInventory(Picklist oPicklist, SearchCriteria oCriteria)
        {

            try
            {
                if (oPicklist.PickListRefNo == null)
                    throw new WMSExceptionMessage() { WMSExceptionCode = "EMC_OB_DAL_001", WMSMessage = ErrorMessages.EMC_OB_DAL_001, ShowAsError = true };

                List<PickListInventory> lPickListInventory = new List<PickListInventory>();

                DataSet _dsInventory = FetchPickListInformation(oPicklist, base.LoggedInUserID, false);

                if (_dsInventory == null)
                {
                    throw new Exception("Fetch Picklist procedure returned a NULL Dataset.");
                }
                else if (_dsInventory.Tables.Count > 0)
                {
                    foreach (DataRow _drInventory in _dsInventory.Tables[0].Rows)
                    {
                        PickListInventory oInventory = null;

                        oInventory = BindAndReturnInventory(_drInventory);

                        lPickListInventory.Add(oInventory);
                    }
                }

                return lPickListInventory;
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

                ExceptionHandling.LogException(excp, _ClassCode + "004", oExcpData);
                throw new WMSExceptionMessage() { WMSExceptionCode = ErrorMessages.WMSExceptionCode, WMSMessage = ErrorMessages.WMSExceptionMessage, ShowAsCriticalError = true };
            }
        }

        private DataSet FetchPickListInformation(Picklist oPickList, int iUserID, bool bPickNextItem)
        {
            try
            {
                string _sQuery = "EXEC [dbo].[USP_API_FetchPickListsInventoryForPicking]";

                List<SqlParameter> lSQLParams = new List<SqlParameter>();

                if (oPickList.PicklistHeaderID > 0)
                {
                    SqlParameter oParam1 = new SqlParameter("@PickListHeaderID", SqlDbType.Int);
                    oParam1.Value = oPickList.PicklistHeaderID;

                    lSQLParams.Add(oParam1);
                }

                if (oPickList.PickListRefNo != null)
                {

                    SqlParameter oParam2 = new SqlParameter("@PickListRefNo", SqlDbType.NVarChar, 50);
                    oParam2.Value = oPickList.PickListRefNo;

                    lSQLParams.Add(oParam2);
                }


                SqlParameter oParam = new SqlParameter("@FetchNextItem", SqlDbType.Bit);
                oParam.Value = 1;

                lSQLParams.Add(oParam);

                SqlParameter oUser = new SqlParameter("@UserID", SqlDbType.Int);
                oUser.Value = iUserID;
                lSQLParams.Add(oUser);



                DataSet _dsResults = FillDataSet(_sQuery, lSQLParams);

                return _dsResults;
            }
            catch (WMSExceptionMessage excp)
            {
                throw excp;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private PickListInventory BindAndReturnInventory(DataRow drInventoryItem)
        {
            try
            {
                PickListInventory oInventory = BindPickListInventoryItem(drInventoryItem);

                return oInventory;
            }
            catch (WMSExceptionMessage excp)
            {
                throw excp;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        public List<LoadSheet> FetchLoadSheetList(SearchCriteria oCriteria)
        {

            try
            {

                List<LoadSheet> _lLoadSheetList = new List<LoadSheet>();

                string _sSQL = "EXEC [dbo].[USP_API_FetchLoadSheets]";

                List<SqlParameter> _lParams = new List<SqlParameter>();

                SqlParameter oParamUser = new SqlParameter("@UserID", SqlDbType.Int);
                oParamUser.Value = LoggedInUserID;

                _lParams.Add(oParamUser);

                DataSet dsLoadList = base.FillDataSet(_sSQL, _lParams);

                if (dsLoadList == null)
                    throw new WMSExceptionMessage() { WMSExceptionCode = ErrorMessages.WMSExceptionCode, WMSMessage = ErrorMessages.WMSExceptionMessage, ShowAsCriticalError = true };
                else
                {
                    foreach (DataRow _drLoadSheet in dsLoadList.Tables[0].Rows)
                    {
                        LoadSheet _oLoadsheet = new LoadSheet()
                        {
                            LoadHeaderID = ConversionUtility.ConvertToInt(_drLoadSheet["LoadHeaderID"].ToString()),
                            PickListHeaderID = ConversionUtility.ConvertToInt(_drLoadSheet["PickListHeaderID"].ToString()),
                            YM_MST_VehicleType_ID = ConversionUtility.ConvertToInt(_drLoadSheet["YM_MST_VehicleType_ID"].ToString()),
                             YM_TRN_VehicleYardAvailability_ID = ConversionUtility.ConvertToInt(_drLoadSheet["YM_TRN_VehicleYardAvailability_ID"].ToString()),
                            LoadStatusID = ConversionUtility.ConvertToInt(_drLoadSheet["LoadStatusID"].ToString()),

                            LoadRefNo = _drLoadSheet["LoadRefNo"].ToString(),
                           // Remarks = _drLoadSheet["Remarks"].ToString(), 

                            DockDisplayLocationCode = _drLoadSheet["LocationCode"].ToString(), 
                            DockLocationID = ConversionUtility.ConvertToInt(_drLoadSheet["Dock_LocationID"].ToString()), 
                            DockSystemLocationCode = _drLoadSheet["LocationCode"].ToString(), 
                            VehicleID = ConversionUtility.ConvertToInt(_drLoadSheet["VehicleID"].ToString()), 
                            VehicleType = _drLoadSheet["VEhicleType"].ToString(),
                            CustomerCode=_drLoadSheet["CustomerCode"].ToString(),

                            VehicleMaxVolumeCFT = ConversionUtility.ConvertToDecimal(_drLoadSheet["StorageVolume"].ToString()),
                            VehicleMaxWeightKG = ConversionUtility.ConvertToDecimal(_drLoadSheet["MaxStorageWeight"].ToString()),
                            VehicleNumber = _drLoadSheet["RegistrationNo"].ToString(), 

                            LoadSheetQuantity = ConversionUtility.ConvertToDecimal(_drLoadSheet["LoadSheetQuantity"].ToString()), 
                            LoadedQuantity = ConversionUtility.ConvertToDecimal(_drLoadSheet["LoadSheetQuantity"].ToString())

                        };

                        _lLoadSheetList.Add(_oLoadsheet);
                    }
                }

                return _lLoadSheetList;
            }
            catch (WMSExceptionMessage excp)
            {
                throw excp;
            }
            catch (Exception excp)
            {
                ExceptionData oExcpData = new ExceptionData();
                oExcpData.AddInputs("oCriteria", oCriteria);

                ExceptionHandling.LogException(excp, _ClassCode + "005", oExcpData);
                throw new WMSExceptionMessage() { WMSExceptionCode = ErrorMessages.WMSExceptionCode, WMSMessage = ErrorMessages.WMSExceptionMessage, ShowAsCriticalError = true };
            }
        }

        public List<PickListInventory> ReGeneratePickingSuggestionOnSkip(SearchCriteria oCriteria)
        {
            /*
             
                ALTER PROCEDURE [dbo].[USP_TRN_AllocateAndReserveInventoryForOBDandVLPD]
                
                
                @SuggestionID					INT = NULL, 
                @SuggestionFulfilledQuantity	DECIMAL(18, 2) = 0,
                @ReasonID						INT = NULL,
                @IsVLPDPicking					INT = 0, 

             */

            try
            {
                if (oCriteria.SuggestionID == 0 && oCriteria.PickListHeaderID == 0 && oCriteria.TransferRequestID == 0 && oCriteria.TransferRequestDetailsID == 0 && oCriteria.PickListDetailsID == 0)
                    throw new WMSExceptionMessage() { WMSExceptionCode = "EMC_OB_DAL_001", WMSMessage = ErrorMessages.EMC_OB_DAL_001, ShowAsError = true };
                
                List<PickListInventory> _lPickListInventory = new List<PickListInventory>();
                List<SqlParameter> lSQLParams = new List<SqlParameter>();

                string _sSQL = "EXEC [dbo].[USP_TRN_AllocateAndReserveInventoryForOBDandVLPD] ";

                if(oCriteria.TransferRequestID > 0)
                {
                    SqlParameter oParam = new SqlParameter("@TransferRequestID", SqlDbType.Int);
                    oParam.Value = oCriteria.TransferRequestID;

                    lSQLParams.Add(oParam);
                }


                if (oCriteria.TransferRequestDetailsID > 0)
                {
                    SqlParameter oParam = new SqlParameter("@TransferRaquestDetailsID", SqlDbType.Int);
                    oParam.Value = oCriteria.TransferRequestDetailsID;

                    lSQLParams.Add(oParam);
                }


                if (oCriteria.PickListHeaderID > 0)
                {
                    SqlParameter oParam = new SqlParameter("@PickListHeaderID", SqlDbType.Int);
                    oParam.Value = oCriteria.PickListHeaderID;

                    lSQLParams.Add(oParam);
                }


                if (oCriteria.PickListDetailsID > 0)
                {
                    SqlParameter oParam = new SqlParameter("@PickListDetailsID", SqlDbType.Int);
                    oParam.Value = oCriteria.PickListDetailsID;

                    lSQLParams.Add(oParam);
                }


                if (oCriteria.PickListDetailsID > 0)
                {
                    SqlParameter oParam = new SqlParameter("@PickListDetailsID", SqlDbType.Int);
                    oParam.Value = oCriteria.PickListDetailsID;

                    lSQLParams.Add(oParam);
                }


                if (oCriteria.LoggedInUserID > 0)
                {
                    SqlParameter oParam = new SqlParameter("@User", SqlDbType.Int);
                    oParam.Value = oCriteria.LoggedInUserID;

                    lSQLParams.Add(oParam);
                }

                SqlParameter oParamFetch = new SqlParameter("@FetchPickList", SqlDbType.Bit);
                oParamFetch.Value = 0;

                lSQLParams.Add(oParamFetch);


                if (oCriteria.SuggestionID > 0)
                {
                    SqlParameter oParam = new SqlParameter("@SuggestionID", SqlDbType.Int);
                    oParam.Value = oCriteria.SuggestionID;

                    lSQLParams.Add(oParam);
                }


                if (oCriteria.SuggestionID > 0)
                {
                    SqlParameter oParam = new SqlParameter("@SuggestionFulfilledQuantity", SqlDbType.Decimal);
                    oParam.Value = oCriteria.SuggestionID;

                    lSQLParams.Add(oParam);
                }



                DataSet dsSuggestions = base.FillDataSet(_sSQL, lSQLParams);

                if (dsSuggestions == null)
                    throw new WMSExceptionMessage() { WMSExceptionCode = ErrorMessages.WMSExceptionCode, WMSMessage = ErrorMessages.WMSExceptionMessage, ShowAsCriticalError = true };
               
                else
                {
                    foreach (DataRow _drPickSuggestionItem in dsSuggestions.Tables[0].Rows)
                    {

                        if(_drPickSuggestionItem["ErrorCode"].ToString()=="1")
                            throw new WMSExceptionMessage() { WMSExceptionCode = "EMC_OB_DAL_PICKSugg_EC01", WMSMessage = ErrorMessages.EMC_OB_DAL_PICKSugg_EC01, ShowAsError = true };
                        if (_drPickSuggestionItem["ErrorCode"].ToString() == "2")
                            throw new WMSExceptionMessage() { WMSExceptionCode = "EMC_OB_DAL_PICKSugg_EC02", WMSMessage = ErrorMessages.EMC_OB_DAL_PICKSugg_EC02, ShowAsError = true };
                        if (_drPickSuggestionItem["ErrorCode"].ToString() == "3")
                            throw new WMSExceptionMessage() { WMSExceptionCode = "EMC_OB_DAL_PICKSugg_EC03", WMSMessage = ErrorMessages.EMC_OB_DAL_PICKSugg_EC03, ShowAsError = true };
                        if (_drPickSuggestionItem["ErrorCode"].ToString() == "4")
                            throw new WMSExceptionMessage() { WMSExceptionCode = "EMC_OB_DAL_PICKSugg_EC04", WMSMessage = ErrorMessages.EMC_OB_DAL_PICKSugg_EC04, ShowAsError = true };
                        PickListInventory _SuggestionItem = null; //BindPickListInventoryItem(_drPickSuggestionItem);

                        _lPickListInventory.Add(_SuggestionItem);
                    }
                }

                return _lPickListInventory;
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


        public PickingPreferences FetchPickingPreferences(SearchCriteria oCriteria)
        {
          

            try
            {
                PickingPreferences oPreferences = null;

                List<SqlParameter> lSQLParams = new List<SqlParameter>();

                string _sSQL = "EXEC [dbo].[USP_API_FetchPickingPreferences]";

                if (oCriteria.LoggedInUserID > 0)
                {
                    SqlParameter oParam = new SqlParameter("@UserID", SqlDbType.Int);
                    oParam.Value = oCriteria.LoggedInUserID;

                    lSQLParams.Add(oParam);
                }
                

                DataSet dsSuggestions = base.FillDataSet(_sSQL, lSQLParams);

                if (dsSuggestions == null)
                    throw new WMSExceptionMessage() { WMSExceptionCode = ErrorMessages.WMSExceptionCode, WMSMessage = ErrorMessages.WMSExceptionMessage, ShowAsCriticalError = true };
                else
                {
                    oPreferences = new PickingPreferences()
                    {
                        AllowCrossDocking = ConversionUtility.ConvertToBool(dsSuggestions.Tables[0].Rows[0]["AllowCrossdocking"].ToString()),
                        AllowDispatchOfOLDMRP = ConversionUtility.ConvertToBool(dsSuggestions.Tables[0].Rows[0]["AllowDispatchOLDMRP"].ToString()),
                        AllowNestedInventoryDispatch = ConversionUtility.ConvertToBool(dsSuggestions.Tables[0].Rows[0]["AllowDispatchNested"].ToString()),
                        AutoReconsileInventoryOnSkip = ConversionUtility.ConvertToBool(dsSuggestions.Tables[0].Rows[0]["AutoReconcileInventory"].ToString()),
                        StrictComplianceToPicking = ConversionUtility.ConvertToBool(dsSuggestions.Tables[0].Rows[0]["StrictPickingCompliance"].ToString())
                    };
                }
                
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

                ExceptionHandling.LogException(excp, _ClassCode + "006", oExcpData);
                throw new WMSExceptionMessage() { WMSExceptionCode = ErrorMessages.WMSExceptionCode, WMSMessage = ErrorMessages.WMSExceptionMessage, ShowAsCriticalError = true };
            }
        }

        private PickListInventory BindPickListInventoryItem(DataRow _drSuggestionItem)
        {
            PickListInventory _oPLI = new PickListInventory()
            {
                PickListHeaderID = ConversionUtility.ConvertToInt(_drSuggestionItem["PickListHeaderID"].ToString()),
                PickListDetailsID = ConversionUtility.ConvertToInt(_drSuggestionItem["PickListDetailsID"].ToString()),
                MaterialMasterID = ConversionUtility.ConvertToInt(_drSuggestionItem["MaterialMasterID"].ToString()),
                //MaterialTransactionID = ConversionUtility.ConvertToInt(_drSuggestionItem["MaterialTransactionID"].ToString()),
              //  DockID = ConversionUtility.ConvertToInt(_drSuggestionItem["MaterialTransactionID"].ToString()),
                MaterialPriorityID = ConversionUtility.ConvertToInt(_drSuggestionItem["MaterialPriorityID"].ToString()),
                PickLocationID = ConversionUtility.ConvertToInt(_drSuggestionItem["PickLocationID"].ToString()),

                SuggestionID=ConversionUtility.ConvertToInt(_drSuggestionItem["SuggestionID"].ToString()),

                IsBatchPicklist = ConversionUtility.ConvertToBool(_drSuggestionItem["IsBatchPicklist"].ToString()),

                PickDisplayLocationCode = _drSuggestionItem["PickDisplayLocationCode"].ToString(),
                PickSystemLocationCode = _drSuggestionItem["PickSystemLocationCode"].ToString(),
                MaterialPriority = _drSuggestionItem["MaterialPriority"].ToString(),

                MaterialCode = _drSuggestionItem["MCode"].ToString(),
                DockLocation = _drSuggestionItem["DockDisplayCode"].ToString(),

                PickListRefNo = _drSuggestionItem["PickListRefNo"].ToString(),

                PicklistQuantity = ConversionUtility.ConvertToDecimal(_drSuggestionItem["PicklistQuantity"].ToString()), 
                PickedQuantity = ConversionUtility.ConvertToDecimal(_drSuggestionItem["PickedQuantity"].ToString()),
                RequirementQuantity = ConversionUtility.ConvertToDecimal(_drSuggestionItem["SuggestionQuantity"].ToString()),
                CustomerCode= _drSuggestionItem["CustomerCode"].ToString(),
                MaterialDescription= _drSuggestionItem["MDescription"].ToString()

            };

            return _oPLI;
        }


        public List<LoadItem> FetchInventoryForLoadSheet(LoadSheet oLoadSheet)
        {

            try
            {
                string _sSQL = "EXEC sp_API_INV_FetchLoadInventory ";

                List<SqlParameter> lSQLParam = new List<SqlParameter>();

                // @LoadRefNo, @Mcode

                if(oLoadSheet.LoadRefNo !=null)
                {
                    SqlParameter oParam = new SqlParameter("@LoadRefNo", SqlDbType.NVarChar, 50);
                    oParam.Value = oLoadSheet.LoadRefNo;

                    lSQLParam.Add(oParam);
                }

                DataSet _dsLoadInventory = FillDataSet(_sSQL, lSQLParam);
                List<LoadItem> lItems = new List<LoadItem>();

                if (_dsLoadInventory == null)
                {
                    throw new WMSExceptionMessage() { WMSExceptionCode = ErrorMessages.WMSExceptionCode, WMSMessage = ErrorMessages.WMSExceptionMessage, ShowAsCriticalError = true };
                }
                else
                {
                    lItems = BindLoadItems(_dsLoadInventory.Tables[0]);
                }

                return lItems;
            }
            catch (WMSExceptionMessage excp)
            {
                throw excp;
            }
            catch (Exception excp)
            {
                ExceptionData oExcpData = new ExceptionData();
                oExcpData.AddInputs("oLoadSheet", oLoadSheet);

                ExceptionHandling.LogException(excp, _ClassCode + "008", oExcpData);
                throw new WMSExceptionMessage() { WMSExceptionCode = ErrorMessages.WMSExceptionCode, WMSMessage = ErrorMessages.WMSExceptionMessage, ShowAsCriticalError = true };
            }
        }

        private List<LoadItem> BindLoadItems(DataTable _dtInventory)
        {
            List<LoadItem> lItems = new List<LoadItem>();

            foreach (DataRow _Item in _dtInventory.Rows)
            {
                LoadItem oItem = new LoadItem()
                {
                    LoadDetailsID = ConversionUtility.ConvertToInt(_Item["LoadDetailsID"].ToString()),
                    LoadHeaderID = ConversionUtility.ConvertToInt(_Item["LoadHeaderID"].ToString()),
                    PickListDetailsID = ConversionUtility.ConvertToInt(_Item["PickListDetailsID"].ToString()),
                    PickListHeaderID = ConversionUtility.ConvertToInt(_Item["PickListHeaderID"].ToString()),
                    MaterialMasterID = ConversionUtility.ConvertToInt(_Item["MaterialMasterID"].ToString()),
                    //  SuggestionID = ConversionUtility.ConvertToInt(_Item["SuggestionID"].ToString()),

                    // LoadRefNo = _Item["LoadRefNo"].ToString(),
                    MCode = _Item["MCode"].ToString(),
                    MDescription= _Item["Mdescription"].ToString(),

                    LoadQuantity = ConversionUtility.ConvertToDecimal(_Item["LoadQuantity"].ToString()),
                    LoadedQuantity = ConversionUtility.ConvertToDecimal(_Item["LoadedQuantity"].ToString()),
                    TotalLoadedQuantity = ConversionUtility.ConvertToDecimal(_Item["TotalLoadedQuantity"].ToString()),
                    TotalLoadQuantity = ConversionUtility.ConvertToDecimal(_Item["TotalLoadQuantity"].ToString()),

                    TotalLoadedWeight = ConversionUtility.ConvertToDecimal(_Item["TotalLoadedWeight"].ToString()),
                    TotalLoadedVolume = ConversionUtility.ConvertToDecimal(_Item["TotalLoadedWeight"].ToString()),
                    
                   

                    ActiveMRP = ConversionUtility.ConvertToDecimal(_Item["ActiveMRP"].ToString()),

                    IsParent = ConversionUtility.ConvertToBool(_Item["IsParent"].ToString())
                };

                lItems.Add(oItem);
            }

            return lItems;
        }

        public List<Inventory> FetchPickListInventoryForLoad(Inventory oInventory)
        {

            try
            {
                string _sSQL = "EXEC [dbo].[sp_API_INV_FetchPickListInventoryForLoad] ";

                List<SqlParameter> lSQLParam = new List<SqlParameter>();

                // @LoadRefNo, @Mcode

                if (oInventory.ReferenceDocumentNumber != null)
                {
                    SqlParameter oParam = new SqlParameter("@LoadRefNo", SqlDbType.NVarChar, 50);
                    oParam.Value = oInventory.ReferenceDocumentNumber;

                    lSQLParam.Add(oParam);
                }
             

                if (oInventory.MaterialCode != null)
                {
                    SqlParameter oParam = new SqlParameter("@MCode", SqlDbType.NVarChar, 50);
                    oParam.Value = oInventory.MaterialCode;

                    lSQLParam.Add(oParam);
                }

                if (oInventory.RSN != null)
                {
                    SqlParameter oParam = new SqlParameter("@RSNNumber", SqlDbType.NVarChar, 50);
                    oParam.Value = oInventory.RSN;

                    lSQLParam.Add(oParam);
                }
                DataSet _dsLoadInventory = FillDataSet(_sSQL, lSQLParam);
                List<Inventory> lItems = new List<Inventory>();

                if (_dsLoadInventory == null)
                {
                    throw new WMSExceptionMessage() { WMSExceptionCode = ErrorMessages.WMSExceptionCode, WMSMessage = ErrorMessages.WMSExceptionMessage, ShowAsCriticalError = true };
                }
                else if (_dsLoadInventory.Tables.Count > 0)
                {
                    lItems = BindInventoryForLoad(_dsLoadInventory.Tables[0]);
                }

                return lItems;
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

        private List<Inventory> BindInventoryForLoad(DataTable _dtInventory)
        {
            List<Inventory> lInventory = new List<Inventory>();

            foreach(DataRow _drInventory in _dtInventory.Rows)
            {
                Inventory oInv = new Inventory()
                {
                    ReferenceDocumentNumber = _drInventory["PickListRefNo"].ToString(),

                    ReferenceDocumentID = ConversionUtility.ConvertToInt(_drInventory["PickListHeaderID"].ToString()),

                    MaterialTransactionID = ConversionUtility.ConvertToInt(_drInventory["MaterialTransactionID"].ToString()),
                    MaterialMasterID = ConversionUtility.ConvertToInt(_drInventory["MaterialMasterID"].ToString()),
                    
                    RSN = _drInventory["SerialNo"].ToString(),
                    BatchNumber = _drInventory["BatchNo"].ToString(),

                    StorageLocationID = ConversionUtility.ConvertToInt(_drInventory["StorageLocationID"].ToString()),
                    MRP = ConversionUtility.ConvertToDecimal(_drInventory["MRP"].ToString()),

                    ColorID = ConversionUtility.ConvertToInt(_drInventory["ColourID"].ToString()),
                    Color = _drInventory["Colour"].ToString(),

                    StorageLocation = _drInventory["StorageLocation"].ToString(), 
                    MaterialCode=_drInventory["MCode"].ToString(), 
                    IsDamaged = ConversionUtility.ConvertToBool(_drInventory["IsDamaged"].ToString())               
                    
                };

                lInventory.Add(oInv);
            }

            return lInventory;
        }

        public Inventory ConfirmLoading(Inventory oInventory)
        {

            try
            {
                string _sSQL = "EXEC [dbo].[sp_API_LoadItem]  ";

                List<SqlParameter> lSQLParam = new List<SqlParameter>();

                // @LoadRefNo, @Mcode

                if (oInventory.ReferenceDocumentNumber != null)
                {
                    SqlParameter oParam = new SqlParameter("@LoadingRefNumber", SqlDbType.NVarChar, 50);
                    oParam.Value = oInventory.ReferenceDocumentNumber;

                    lSQLParam.Add(oParam);
                }

                if (oInventory.Quantity > 0)
                {
                    SqlParameter oParam = new SqlParameter("@QUANTITY", SqlDbType.Decimal);
                    oParam.Value = oInventory.Quantity;

                    lSQLParam.Add(oParam);
                }

                if (oInventory.RSN != null)
                {
                    SqlParameter oParam = new SqlParameter("@SerialNumber", SqlDbType.NVarChar, 50);
                    oParam.Value = oInventory.RSN;

                    lSQLParam.Add(oParam);
                }


                if (base.LoggedInUserID > 0)
                {
                    SqlParameter oParam = new SqlParameter("@CreatedBy", SqlDbType.Int);
                    oParam.Value = base.LoggedInUserID;

                    lSQLParam.Add(oParam);
                }
                DataSet _dsLoadInventory = FillDataSet(_sSQL, lSQLParam);
                List<Inventory> lItems = new List<Inventory>();

                if (_dsLoadInventory == null)
                {
                    throw new WMSExceptionMessage() { WMSExceptionCode = ErrorMessages.WMSExceptionCode, WMSMessage = ErrorMessages.WMSExceptionMessage, ShowAsCriticalError = true };
                }
                else if(_dsLoadInventory.Tables.Count==1 && _dsLoadInventory.Tables[0].Rows.Count==1 && _dsLoadInventory.Tables[0].Columns.Count==1)
                {
                    switch(ConversionUtility.ConvertToInt(_dsLoadInventory.Tables[0].Rows[0][0].ToString()))
                    { 
                    
                        case -4:
                            throw new WMSExceptionMessage() { WMSExceptionCode = "EMC_OB_DAL_003", WMSMessage = ErrorMessages.EMC_OB_DAL_003, ShowAsError = true };
                         
                        case -3:
                            throw new WMSExceptionMessage() { WMSExceptionCode = "EMC_OB_DAL_004", WMSMessage = ErrorMessages.EMC_OB_DAL_004, ShowAsError = true };
                          
                        case -2:
                            throw new WMSExceptionMessage() { WMSExceptionCode = "EMC_OB_DAL_005", WMSMessage = ErrorMessages.EMC_OB_DAL_005, ShowAsError = true };
                            
                        case -1:
                            throw new WMSExceptionMessage() { WMSExceptionCode = "EMC_OB_DAL_006", WMSMessage = ErrorMessages.EMC_OB_DAL_006, ShowAsError = true };
                          
                        case -5:
                            throw new WMSExceptionMessage() { WMSExceptionCode = "EMC_OB_DAL_008", WMSMessage = ErrorMessages.EMC_OB_DAL_008, ShowAsError = true };
                           

                    }
                }
                else if (_dsLoadInventory.Tables.Count > 0)
                {

                    lItems = BindInventoryForLoad(_dsLoadInventory.Tables[0]);
                }

                return oInventory;
            }
            catch(WMSExceptionMessage exp)
            {
                throw exp;
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
                string _sSQL = "EXEC  [dbo].[USP_API_RevertLoading] ";

                List<SqlParameter> lSQLParam = new List<SqlParameter>();

               
                if (oInventory.ReferenceDocumentNumber != null)
                {
                    SqlParameter oParam = new SqlParameter("@LoadRefNo", SqlDbType.NVarChar, 50);
                    oParam.Value = oInventory.ReferenceDocumentNumber;

                    lSQLParam.Add(oParam);
                }


                SqlParameter oParamQty = new SqlParameter("@LoadedQuantity", SqlDbType.Decimal);
                oParamQty.Value = oInventory.Quantity;

                lSQLParam.Add(oParamQty);
               

                if (oInventory.RSN != null)
                {
                    SqlParameter oParam = new SqlParameter("@RSNNumber", SqlDbType.NVarChar, 50);
                    oParam.Value = oInventory.RSN;

                    lSQLParam.Add(oParam);
                }
                
                SqlParameter oParamUser = new SqlParameter("@UserID", SqlDbType.Int);
                oParamUser.Value = LoggedInUserID;

                lSQLParam.Add(oParamUser);

                int _Result = ExecuteNonQuery(_sSQL, lSQLParam);

                if (_Result == 0)
                    throw new WMSExceptionMessage() { WMSExceptionCode = ErrorMessages.WMSExceptionCode, WMSMessage = ErrorMessages.WMSExceptionMessage, ShowAsCriticalError = true };
                else if (_Result > 0)
                    oInventory.IsDispatched = true;
                //else
                //    throw new WMSExceptionMessage() { WMSExceptionCode = "EMC_OB_DAL_002", WMSMessage = ErrorMessages.EMC_OB_DAL_002, ShowAsError = true };
                else
                    oInventory.Quantity = ConversionUtility.ConvertToDecimal(_Result.ToString());

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

                ExceptionHandling.LogException(excp, _ClassCode + "011", oExcpData);
                throw new WMSExceptionMessage() { WMSExceptionCode = ErrorMessages.WMSExceptionCode, WMSMessage = ErrorMessages.WMSExceptionMessage, ShowAsCriticalError = true };
            }
        }

        public List<Outbound> GetMaterialsUnderSOForPacking(string SONumber, int AccountID, int UserId)
        {
            try
            {
                List<Outbound> lOutbound = new List<Outbound>();

                string _sSQL = "EXEC [dbo].[USP_GetMCodesUnderSO] ";

                List<SqlParameter> lParam = new List<SqlParameter>();

               

                if (SONumber != null)
                {
                    SqlParameter oParam = new SqlParameter("@SONumber", SqlDbType.NVarChar, 100);
                    oParam.Value = SONumber;

                    lParam.Add(oParam);
                }
                if (AccountID != 0) //============== Added By M.D.Prasad ON 28 - 08 - 2020 For AccounID ===========
                {
                    SqlParameter oParam1 = new SqlParameter("@AccountID", SqlDbType.NVarChar, 100);
                    oParam1.Value = AccountID;

                    lParam.Add(oParam1);
                }
                if (UserId != 0)
                {
                    SqlParameter oParam1 = new SqlParameter("@UserId", SqlDbType.Int);
                    oParam1.Value = UserId;

                    lParam.Add(oParam1);
                }
                DataSet _dsPickLists = base.FillDataSet(_sSQL, lParam);

                if (_dsPickLists != null)
                {
                    if (_dsPickLists.Tables.Count > 0)
                    {
                        foreach (DataRow _drPickList in _dsPickLists.Tables[0].Rows)
                        {
                            Outbound _oOutbound = new Outbound()
                            {
                                //MaterialMasterID = ConversionUtility.ConvertToInt(_drPickList["MaterialMasterID"].ToString()),
                                Mcode = _drPickList["MCode"].ToString(),
                                PickedQty = ConversionUtility.ConvertToDecimal(_drPickList["PickedQty"].ToString()),
                                PackedQty = ConversionUtility.ConvertToDecimal(_drPickList["PackedQty"].ToString()),
                               // OutboundID = ConversionUtility.ConvertToInt(_drPickList["OutboundID"].ToString()),
                                CustomerName = _drPickList["CustomerName"].ToString(),
                                SOHeaderID = ConversionUtility.ConvertToInt(_drPickList["SOHeaderID"].ToString()),
                                BusinessType = _drPickList["BusinessType"].ToString(),
                                //OBDNumber = _drPickList["OBDNumber"].ToString(),
                                MFGDate = _drPickList["MfgDate"].ToString(),
                                EXPDate = _drPickList["ExpDate"].ToString(),
                                SerialNo = _drPickList["SerialNo"].ToString(),
                                ProjectRefNo = _drPickList["ProjectRefNo"].ToString(),
                                BatchNo = _drPickList["BatchNo"].ToString(),
                                MRP = _drPickList["MRP"].ToString(),
                                HUNo = _drPickList["HUNo"].ToString(),
                                HUSize = _drPickList["HUSize"].ToString()

                            };

                            lOutbound.Add(_oOutbound);
                        }
                    }
                }

                return lOutbound;
            }
            catch (WMSExceptionMessage excp)
            {
                throw excp;
            }
            catch (Exception excp)
            {
                ExceptionData oExcpData = new ExceptionData();
                oExcpData.AddInputs("oCriteria", SONumber);

                ExceptionHandling.LogException(excp, _ClassCode + "001", oExcpData);
                throw new WMSExceptionMessage() { WMSExceptionCode = ErrorMessages.WMSExceptionCode, WMSMessage = ErrorMessages.WMSExceptionMessage, ShowAsCriticalError = true };
            }

        }


        public List<Outbound> GetMSPForPacking(string SodetailsID)
        {
            try
            {
                List<Outbound> lOutbound = new List<Outbound>();

                string _sSQL = "EXEC [dbo].[USP_API_GetMSPsForPacking] ";

                List<SqlParameter> lParam = new List<SqlParameter>();



                if (SodetailsID != null)
                {
                    SqlParameter oParam = new SqlParameter("@SoDetailsID", SqlDbType.NVarChar, 100);
                    oParam.Value = SodetailsID;

                    lParam.Add(oParam);
                }

                DataSet _dsPickLists = base.FillDataSet(_sSQL, lParam);

                if (_dsPickLists != null)
                {
                    if (_dsPickLists.Tables.Count > 0)
                    {
                        foreach (DataRow _drPickList in _dsPickLists.Tables[0].Rows)
                        {
                            Outbound _oOutbound = new Outbound()
                            {
                                MFGDate = _drPickList["MfgDate"].ToString(),
                                EXPDate = _drPickList["ExpDate"].ToString(),
                                SerialNo = _drPickList["SerialNo"].ToString(),
                                ProjectRefNo = _drPickList["ProjectRefNo"].ToString(),
                                BatchNo = _drPickList["BatchNo"].ToString(),
                                MRP = _drPickList["MRP"].ToString()
                            };

                            lOutbound.Add(_oOutbound);
                        }
                    }
                }

                return lOutbound;
            }
            catch (WMSExceptionMessage excp)
            {
                throw excp;
            }
            catch (Exception excp)
            {
                ExceptionData oExcpData = new ExceptionData();
                oExcpData.AddInputs("oCriteria", SodetailsID);

                ExceptionHandling.LogException(excp, _ClassCode + "001", oExcpData);
                throw new WMSExceptionMessage() { WMSExceptionCode = ErrorMessages.WMSExceptionCode, WMSMessage = ErrorMessages.WMSExceptionMessage, ShowAsCriticalError = true };
            }

        }


        public Outbound UpsertPackItem (Outbound outbound)
        {
            try
            {
                string _sSQL = "EXEC [dbo].[USP_API_UpsertPackItem ]  ";


                List<SqlParameter> lSQLParam = new List<SqlParameter>();

                if (base.LoggedInUserID > 0)
                {
                    SqlParameter oParam = new SqlParameter("@CreatedBy", SqlDbType.Int);
                    oParam.Value = base.LoggedInUserID;

                    lSQLParam.Add(oParam);
                }

                if (outbound.OutboundID != 0)
                {
                    SqlParameter oParam = new SqlParameter("@OutBoundId", SqlDbType.Int);
                    oParam.Value = outbound.OutboundID;

                    lSQLParam.Add(oParam);
                }

               
                    SqlParameter oPSN = new SqlParameter("@PSNHeaderID", SqlDbType.Int);
                    oPSN.Value = outbound.PSNID;

                    lSQLParam.Add(oPSN);


                SqlParameter oPSND = new SqlParameter("@PSNDetailsID", SqlDbType.Int);
                oPSND.Value = outbound.PSNDetailsID;

                lSQLParam.Add(oPSND);

                if (outbound.Mcode != string.Empty)
                {
                    SqlParameter oParam = new SqlParameter("@Material", SqlDbType.NVarChar,100);
                    oParam.Value = outbound.Mcode;

                    lSQLParam.Add(oParam);
                }

                if (outbound.PickedQty != 0)
                {
                    SqlParameter oParam = new SqlParameter("@PickedQty", SqlDbType.Int);
                    oParam.Value = outbound.PickedQty;

                    lSQLParam.Add(oParam);
                }

                if (outbound.PackedQty != 0)
                {
                    SqlParameter oParam = new SqlParameter("@PackedQty", SqlDbType.Int);
                    oParam.Value = outbound.PackedQty;

                    lSQLParam.Add(oParam);
                }

                if (outbound.CartonSerialNo != string.Empty)
                {
                    SqlParameter oParam = new SqlParameter("@CartonNumber", SqlDbType.NVarChar, 100);
                    oParam.Value = outbound.CartonSerialNo;

                    lSQLParam.Add(oParam);
                }

                if (outbound.MFGDate != string.Empty || outbound.MFGDate != null) 
                {
                    SqlParameter oParam = new SqlParameter("@MfgDate", SqlDbType.NVarChar, 100);
                    oParam.Value = outbound.MFGDate;

                    lSQLParam.Add(oParam);
                }

                if (outbound.EXPDate != string.Empty || outbound.EXPDate != null) 
                {
                    SqlParameter oParam = new SqlParameter("@ExpDate", SqlDbType.NVarChar, 100);
                    oParam.Value = outbound.EXPDate;

                    lSQLParam.Add(oParam);
                }
                if (outbound.BatchNo != string.Empty || outbound.BatchNo != null) 
                {
                    SqlParameter oParam = new SqlParameter("@BatchNo", SqlDbType.NVarChar, 100);
                    oParam.Value = outbound.BatchNo;

                    lSQLParam.Add(oParam);
                }
                if (outbound.SerialNo != string.Empty || outbound.SerialNo != null) 
                {
                    SqlParameter oParam = new SqlParameter("@SerialNo", SqlDbType.NVarChar, 100);
                    oParam.Value = outbound.SerialNo;

                    lSQLParam.Add(oParam);
                }
                if (outbound.ProjectRefNo != string.Empty || outbound.ProjectRefNo != null) 
                {
                    SqlParameter oParam = new SqlParameter("@ProjectRefNo", SqlDbType.NVarChar, 100);
                    oParam.Value = outbound.ProjectRefNo;

                    lSQLParam.Add(oParam);
                }
                if (outbound.MRP != string.Empty || outbound.MRP != null) 
                {
                    SqlParameter oParam = new SqlParameter("@MRP", SqlDbType.NVarChar, 100);
                    oParam.Value = outbound.MRP;

                    lSQLParam.Add(oParam);
                }

                if (outbound.PackType != string.Empty || outbound.PackType != null)
                {
                    SqlParameter oParam = new SqlParameter("@PackType", SqlDbType.NVarChar, 100);
                    oParam.Value = outbound.PackType;

                    lSQLParam.Add(oParam);
                }

                if (outbound.SONumber != string.Empty || outbound.SONumber != null)
                {
                    SqlParameter oParam = new SqlParameter("@SONumber", SqlDbType.NVarChar, 100);
                    oParam.Value = outbound.SONumber;

                    lSQLParam.Add(oParam);
                }


                if (outbound.SoDetailsID != 0)
                {
                    SqlParameter oParam = new SqlParameter("@SODetailsID", SqlDbType.Int);
                    oParam.Value = outbound.SoDetailsID;

                    lSQLParam.Add(oParam);
                }


                if (outbound.SOHeaderID != 0)
                {
                    SqlParameter oParam = new SqlParameter("@SOHeaderID", SqlDbType.Int);
                    oParam.Value = outbound.SOHeaderID;

                    lSQLParam.Add(oParam);
                }

                if (outbound.AccountID != 0)
                {
                    SqlParameter oParam = new SqlParameter("@AccountID", SqlDbType.Int);
                    oParam.Value = outbound.AccountID;

                    lSQLParam.Add(oParam);
                }

                if (outbound.HUSize != "0")
                {
                    SqlParameter oParam = new SqlParameter("@HUSize", SqlDbType.NVarChar,100);
                    oParam.Value = outbound.HUSize;

                    lSQLParam.Add(oParam);
                }

                if (outbound.HUNo != "0")
                {
                    SqlParameter oParam = new SqlParameter("@HUNo", SqlDbType.NVarChar, 100);
                    oParam.Value = outbound.HUNo;

                    lSQLParam.Add(oParam);
                }

                DataSet _dsLoadInventory = FillDataSet(_sSQL, lSQLParam);


                if (_dsLoadInventory == null)
                {
                    throw new WMSExceptionMessage() { WMSExceptionCode = ErrorMessages.WMSExceptionCode, WMSMessage = ErrorMessages.WMSExceptionMessage, ShowAsCriticalError = true };
                }
                else
                {
                    int Columnscount = _dsLoadInventory.Tables[0].Columns.Count;
                    //if (Columnscount == 1)
                    //{

                    //    foreach (DataRow _dtPack in _dsLoadInventory.Tables[0].Rows)

                    //    {
                    //        outbound.PSNID = ConversionUtility.ConvertToInt(_dtPack["PSNID"].ToString());
                    //        outbound.PSNDetailsID = ConversionUtility.ConvertToInt(_dtPack["PSNDetailsID"].ToString());
                    //    }
                    //}
                    //else
                    //{
                    //    foreach (DataRow _dtPack in _dsLoadInventory.Tables[0].Rows)
                    //    {
                    //        string errormessage = "";
                    //        errormessage = _dtPack["ErrorMessage"].ToString();
                    //        throw new WMSExceptionMessage() { WMSExceptionCode = ErrorMessages.WMSExceptionCode, WMSMessage = errormessage, ShowAsWarning = true };
                    //    }
                    //}
                    if(Columnscount == 1)
                    {
                        foreach (DataRow _dtPack in _dsLoadInventory.Tables[0].Rows)
                        {
                            string errormessage = "";
                            errormessage = _dtPack["ErrorMessage"].ToString();
                            throw new WMSExceptionMessage() { WMSExceptionCode = ErrorMessages.WMSExceptionCode, WMSMessage = errormessage, ShowAsWarning = true };
                        }
                    }
                    else
                    {
                        foreach (DataRow _dtPack in _dsLoadInventory.Tables[0].Rows)

                        {
                            outbound.PSNID = ConversionUtility.ConvertToInt(_dtPack["PSNID"].ToString());
                            outbound.PSNDetailsID = ConversionUtility.ConvertToInt(_dtPack["PSNDetailsID"].ToString());
                        }
                    }
                }

                return outbound;
            }
            catch (WMSExceptionMessage exp)
            {
                throw exp;
            }
            catch (Exception excp)
            {
                ExceptionData oExcpData = new ExceptionData();
                oExcpData.AddInputs("outbound", outbound);

                ExceptionHandling.LogException(excp, _ClassCode + "010", oExcpData);
                throw new WMSExceptionMessage() { WMSExceptionCode = ErrorMessages.WMSExceptionCode, WMSMessage = ErrorMessages.WMSExceptionMessage, ShowAsCriticalError = true };
            }
        }
        
        public Outbound PackComplete (Outbound outbound)
        {
            try
            {
                string _sSQL = "EXEC [dbo].[USP_API_GetPackCompleteData ]  ";


                List<SqlParameter> lSQLParam = new List<SqlParameter>();

                if (outbound.SONumber != null)
                {
                    SqlParameter oParam = new SqlParameter("@SONO", SqlDbType.NVarChar, 100);
                    oParam.Value = outbound.SONumber;

                    lSQLParam.Add(oParam);
                }

                if (outbound.AccountID != 0)
                {
                    SqlParameter oParam = new SqlParameter("@AccountID", SqlDbType.NVarChar, 100);
                    oParam.Value = outbound.AccountID;

                    lSQLParam.Add(oParam);
                }

                DataSet _dsPickLists = base.FillDataSet(_sSQL, lSQLParam);

                if (_dsPickLists == null)
                {
                    throw new WMSExceptionMessage() { WMSExceptionCode = ErrorMessages.WMSExceptionCode, WMSMessage = ErrorMessages.WMSExceptionMessage, ShowAsCriticalError = true };
                }
                else
                {
                    string packcmp = "";
                    foreach (DataRow _dtPack in _dsPickLists.Tables[0].Rows)

                    {
                        packcmp = _dtPack["PCKComplete"].ToString();
                    }

                    if (packcmp == "0.00")
                    {
                        outbound.PackComplete = "true";
                    }
                    else
                    {
                        outbound.PackComplete = "false";
                    }
                }


                return outbound;

            }
            catch (WMSExceptionMessage excp)
            {
                throw excp;
            }
            catch (Exception excp)
            {
                ExceptionData oExcpData = new ExceptionData();
                oExcpData.AddInputs("oCriteria", outbound);

                ExceptionHandling.LogException(excp, _ClassCode + "001", oExcpData);
                throw new WMSExceptionMessage() { WMSExceptionCode = ErrorMessages.WMSExceptionCode, WMSMessage = ErrorMessages.WMSExceptionMessage, ShowAsCriticalError = true };
            }

        }


        public Outbound LoadSheetGeneration(Outbound outbound)
        {
            try
            {
                string _sSQL = "EXEC [dbo].[USP_API_GenerateLoadSheet ]  ";


                List<SqlParameter> lSQLParam = new List<SqlParameter>();

                if (outbound.TenantId != null)
                {
                    SqlParameter oParam = new SqlParameter("@TenantId", SqlDbType.NVarChar, 100);
                    oParam.Value = outbound.TenantId;

                    lSQLParam.Add(oParam);
                }
                
                if (outbound.Vehicle != null)
                {
                    SqlParameter oParam = new SqlParameter("@VEHICLENO", SqlDbType.NVarChar, 100);
                    oParam.Value = outbound.Vehicle;

                    lSQLParam.Add(oParam);
                }
                if (outbound.SONumber != null)
                {
                    SqlParameter oParam = new SqlParameter("@SOSNUMBER", SqlDbType.NVarChar, 100);
                    oParam.Value = outbound.SONumber;

                    lSQLParam.Add(oParam);
                }
                if (outbound.DriverNo != null)
                {
                    SqlParameter oParam = new SqlParameter("@DRIVERNO", SqlDbType.NVarChar, 100);
                    oParam.Value = outbound.DriverNo;

                    lSQLParam.Add(oParam);
                }
                if (outbound.DriverName != null)
                {
                    SqlParameter oParam = new SqlParameter("@DRIVERNAME", SqlDbType.NVarChar, 100);
                    oParam.Value = outbound.DriverName;

                    lSQLParam.Add(oParam);
                }
                
                if (outbound.LRnumber != null)
                {
                    SqlParameter oParam = new SqlParameter("@LRNumber", SqlDbType.NVarChar, 100);
                    oParam.Value = outbound.LRnumber;

                    lSQLParam.Add(oParam);
                }
                 if (base.LoggedInUserID > 0)
                {
                    SqlParameter oParam = new SqlParameter("@USERID", SqlDbType.Int);
                    oParam.Value = base.LoggedInUserID;

                    lSQLParam.Add(oParam);
                }

                if (outbound.AccountID != 0)
                {
                    SqlParameter oParam = new SqlParameter("@AccountID", SqlDbType.NVarChar, 100); //============== Added By M.D.Prasad ON 28 - 08 - 2020 For AccounID ===========
                    oParam.Value = outbound.AccountID;

                    lSQLParam.Add(oParam);
                }

                DataSet _dsPickLists = base.FillDataSet(_sSQL, lSQLParam);

                if (_dsPickLists == null)
                {
                    throw new WMSExceptionMessage() { WMSExceptionCode = ErrorMessages.WMSExceptionCode, WMSMessage = ErrorMessages.WMSExceptionMessage, ShowAsCriticalError = true };
                }
                else
                {
                    //string packcmp = "";
                    //foreach (DataRow _dtPack in _dsPickLists.Tables[0].Rows)

                    //{
                    //    outbound.LoadRefNo = _dtPack["S"].ToString();

                    //}


                    int Columnscount = _dsPickLists.Tables[0].Columns.Count;

                    if (Columnscount == 1)
                    {
                        foreach(DataRow _dtPack in _dsPickLists.Tables[0].Rows)
                        {
                            outbound.LoadRefNo = _dtPack["S"].ToString();
                        }
                        
                    }
                    else
                    {

                        foreach (DataRow _dtPack in _dsPickLists.Tables[0].Rows)
                        {
                            string errormessage = "";
                             errormessage= _dtPack["ErrorMessage"].ToString();
                            throw new WMSExceptionMessage() { WMSExceptionCode = ErrorMessages.WMSExceptionCode, WMSMessage = errormessage, ShowAsWarning = true };
                        }

                        
                    }
                    
                }


                return outbound;

            }
            catch (WMSExceptionMessage excp)
            {
                throw excp;
            }
            catch (Exception excp)
            {
                ExceptionData oExcpData = new ExceptionData();
                oExcpData.AddInputs("oCriteria", outbound);

                ExceptionHandling.LogException(excp, _ClassCode + "001", oExcpData);
                throw new WMSExceptionMessage() { WMSExceptionCode = ErrorMessages.WMSExceptionCode, WMSMessage = ErrorMessages.WMSExceptionMessage, ShowAsCriticalError = true };
            }

        }

        public Outbound GetSOCountUnderLoadSheet(Outbound outbound)
        {
            try
            {
                string _sSQL = "EXEC [dbo].[USP_API_GET_LoadSheetCount ]  ";


                List<SqlParameter> lSQLParam = new List<SqlParameter>();

                if(outbound.LoadRefNo != null)
                {
                    SqlParameter oParam = new SqlParameter("@LoadRefNO", SqlDbType.NVarChar, 100);
                    oParam.Value = outbound.LoadRefNo;

                    lSQLParam.Add(oParam);
                }
                DataSet _dsPickLists = base.FillDataSet(_sSQL, lSQLParam);

                if (_dsPickLists == null)
                {
                    throw new WMSExceptionMessage() { WMSExceptionCode = ErrorMessages.WMSExceptionCode, WMSMessage = ErrorMessages.WMSExceptionMessage, ShowAsCriticalError = true };
                }
                else
                {
                    foreach (DataRow _dtPack in _dsPickLists.Tables[0].Rows)
                    {
                        outbound.BusinessType = _dtPack["BussinessType"].ToString();
                        outbound.TotalSOCount = ConversionUtility.ConvertToInt(_dtPack["TotalSOs"].ToString());
                        outbound.ScannedSOCount = ConversionUtility.ConvertToInt(_dtPack["ScannedSOs"].ToString());
                    }

                }

                return outbound;

            }
            catch (WMSExceptionMessage excp)
            {
                throw excp;
            }
            catch (Exception excp)
            {
                ExceptionData oExcpData = new ExceptionData();
                oExcpData.AddInputs("oCriteria", outbound);

                ExceptionHandling.LogException(excp, _ClassCode + "001", oExcpData);
                throw new WMSExceptionMessage() { WMSExceptionCode = ErrorMessages.WMSExceptionCode, WMSMessage = ErrorMessages.WMSExceptionMessage, ShowAsCriticalError = true };
            }

        }

        public Outbound UpsertLoadDetails(Outbound outbound)
        {
            try
            {


                string _sSQL = "EXEC [dbo].[USP_API_UpsertSOUnderLoadSheet ]  ";


                List<SqlParameter> lSQLParam = new List<SqlParameter>();

                if (outbound.LoadRefNo != null)
                {
                    SqlParameter oParam = new SqlParameter("@LoadRefNo", SqlDbType.NVarChar, 100);
                    oParam.Value = outbound.LoadRefNo;

                    lSQLParam.Add(oParam);
                }
                if (outbound.SONumber != null || outbound.SONumber!=string.Empty)
                {
                    SqlParameter oParam = new SqlParameter("@SONumber", SqlDbType.NVarChar, 100);
                    oParam.Value = outbound.SONumber;

                    lSQLParam.Add(oParam);
                }
                if (outbound.CartonSerialNo != null || outbound.CartonSerialNo !=string.Empty)
                {
                    SqlParameter oParam = new SqlParameter("@CartonRefNO", SqlDbType.NVarChar, 100);
                    oParam.Value = outbound.CartonSerialNo;

                    lSQLParam.Add(oParam);
                }
                if (outbound.AccountID != 0)
                {
                    SqlParameter oParam = new SqlParameter("@AccountID", SqlDbType.NVarChar, 100); //============== Added By M.D.Prasad ON 28 - 08 - 2020 For AccounID ===========
                    oParam.Value = outbound.AccountID;

                    lSQLParam.Add(oParam);
                }
                DataSet _dsPickLists = base.FillDataSet(_sSQL, lSQLParam);

                if (_dsPickLists == null)
                {
                    throw new WMSExceptionMessage() { WMSExceptionCode = ErrorMessages.WMSExceptionCode, WMSMessage = ErrorMessages.WMSExceptionMessage, ShowAsCriticalError = true };
                }

                else if (_dsPickLists.Tables.Count == 2)
                {
                    foreach (DataRow _dtPack in _dsPickLists.Tables[1].Rows)
                    {
                        string errormessage = "";
                        errormessage = _dtPack["ErrorMessage"].ToString();
                        throw new WMSExceptionMessage() { WMSExceptionCode = ErrorMessages.WMSExceptionCode, WMSMessage = errormessage, ShowAsWarning = true };
                    }
                }
                else
                {
                    foreach (DataRow _dtPack in _dsPickLists.Tables[0].Rows)
                    {
                        outbound.CustomerCode = _dtPack["CustomerCode"].ToString();
                        outbound.CustomerName = _dtPack["CustomerName"].ToString();
                        outbound.CustomerAddress = _dtPack["Address1"].ToString();
                    }

                }

                return outbound;
            }
            catch (WMSExceptionMessage excp)
            {
                throw excp;
            }
            catch (Exception excp)
            {
                ExceptionData oExcpData = new ExceptionData();
                oExcpData.AddInputs("oCriteria", outbound);

                ExceptionHandling.LogException(excp, _ClassCode + "001", oExcpData);
                throw new WMSExceptionMessage() { WMSExceptionCode = ErrorMessages.WMSExceptionCode, WMSMessage = ErrorMessages.WMSExceptionMessage, ShowAsCriticalError = true };
            }

        }


        public List<Outbound> GetOBDNosUnderSO(Outbound outbound)
        {
            try
            {
                List<Outbound> _lstOutbounds = new List<Outbound>();

                string _sSQL = "EXEC [dbo].[USP_Get_OBD_OutboundDetailsUnderSO]  ";


                List<SqlParameter> lSQLParam = new List<SqlParameter>();

                
                if (outbound.SONumber != null || outbound.SONumber != string.Empty)
                {
                    SqlParameter oParam = new SqlParameter("@SONO", SqlDbType.NVarChar, 100);
                    oParam.Value = outbound.SONumber;

                    lSQLParam.Add(oParam);
                }

                if (outbound.AccountID !=0)
                {
                    SqlParameter oParam = new SqlParameter("@AccountID", SqlDbType.NVarChar, 100);
                    oParam.Value = outbound.AccountID;

                    lSQLParam.Add(oParam);
                }

                if (outbound.UserId != 0)
                {
                    SqlParameter oParam = new SqlParameter("@UserId", SqlDbType.Int);
                    oParam.Value = outbound.UserId;

                    lSQLParam.Add(oParam);
                }

                DataSet _dsPickLists = base.FillDataSet(_sSQL, lSQLParam);

                foreach (DataRow _dtPack in _dsPickLists.Tables[0].Rows)
                {
                    Outbound _oOutbound = new Outbound()
                    {
                        OBDNumber = _dtPack["OBDNumber"].ToString(),
                        OutboundID = ConversionUtility.ConvertToInt(_dtPack["OutboundID"].ToString())

                    };
                    _lstOutbounds.Add(_oOutbound);


                }

                return _lstOutbounds;
            }
            catch (WMSExceptionMessage excp)
            {
                throw excp;
            }
            catch (Exception excp)
            {
                ExceptionData oExcpData = new ExceptionData();
                oExcpData.AddInputs("oCriteria", outbound);

                ExceptionHandling.LogException(excp, _ClassCode + "001", oExcpData);
                throw new WMSExceptionMessage() { WMSExceptionCode = ErrorMessages.WMSExceptionCode, WMSMessage = ErrorMessages.WMSExceptionMessage, ShowAsCriticalError = true };
            }

        }

        public bool CheckSO(Outbound outbound)
        {
            try
            {
                bool result = true;

                string _sSQL = "EXEC [dbo].[USP_Get_OBD_SODetails]  ";


                List<SqlParameter> lSQLParam = new List<SqlParameter>();


                if (outbound.SONumber != null || outbound.SONumber != string.Empty)
                {
                    SqlParameter oParam = new SqlParameter("@SONO", SqlDbType.NVarChar, 100);
                    oParam.Value = outbound.SONumber;

                    lSQLParam.Add(oParam);
                }
                if (outbound.AccountID != 0)
                {
                    SqlParameter oParam1 = new SqlParameter("@AccountID", SqlDbType.NVarChar, 100);//============== Added By M.D.Prasad ON 28 - 08 - 2020 For AccounID ===========
                    oParam1.Value = outbound.AccountID;

                    lSQLParam.Add(oParam1);
                }
                if (outbound.UserId != 0)
                {
                    SqlParameter oParam = new SqlParameter("@UserID", SqlDbType.NVarChar, 100);
                    oParam.Value = outbound.UserId;

                    lSQLParam.Add(oParam);
                }

                DataSet _dsResults = base.FillDataSet(_sSQL, lSQLParam);

                if (_dsResults != null && _dsResults.Tables.Count != 0 && _dsResults.Tables[0].Rows.Count != 0)
                {

                    result = true;
                }
                else
                {
                    throw new WMSExceptionMessage() { WMSExceptionCode = ErrorMessages.WMSExceptionCode, WMSMessage = "Invalid SO Number", ShowAsWarning = true };

                }

                return result;
            }
            catch (WMSExceptionMessage excp)
            {
                throw excp;
            }
            catch (Exception excp)
            {
                ExceptionData oExcpData = new ExceptionData();
                oExcpData.AddInputs("oCriteria", outbound);

                ExceptionHandling.LogException(excp, _ClassCode + "001", oExcpData);
                throw new WMSExceptionMessage() { WMSExceptionCode = ErrorMessages.WMSExceptionCode, WMSMessage = ErrorMessages.WMSExceptionMessage, ShowAsCriticalError = true };
            }
        }


        public bool CheckOBDSO(Outbound outbound)
        {
            try
            {
                bool result = true;

                string _sSQL = "EXEC [dbo].[USP_GET_OBD_SODetails_Picking]  ";


                List<SqlParameter> lSQLParam = new List<SqlParameter>();


                if (outbound.SONumber != null || outbound.SONumber != string.Empty)
                {
                    SqlParameter oParam = new SqlParameter("@SONO", SqlDbType.NVarChar, 100);
                    oParam.Value = outbound.SONumber;

                    lSQLParam.Add(oParam);
                }
                if (outbound.AccountID != 0)
                {
                    SqlParameter oParam1 = new SqlParameter("@AccountID", SqlDbType.NVarChar, 100);//============== Added By M.D.Prasad ON 28 - 08 - 2020 For AccounID ===========
                    oParam1.Value = outbound.AccountID;

                    lSQLParam.Add(oParam1);
                }
                if (outbound.UserId != 0)
                {
                    SqlParameter oParam = new SqlParameter("@UserID", SqlDbType.NVarChar, 100);
                    oParam.Value = outbound.UserId;

                    lSQLParam.Add(oParam);
                }

                DataSet _dsResults = base.FillDataSet(_sSQL, lSQLParam);

                if (_dsResults != null && _dsResults.Tables.Count != 0 && _dsResults.Tables[0].Rows.Count != 0)
                {

                    result = true;
                }
                else
                {
                    throw new WMSExceptionMessage() { WMSExceptionCode = ErrorMessages.WMSExceptionCode, WMSMessage = "Invalid SO Number", ShowAsWarning = true };

                }

                return result;
            }
            catch (WMSExceptionMessage excp)
            {
                throw excp;
            }
            catch (Exception excp)
            {
                ExceptionData oExcpData = new ExceptionData();
                oExcpData.AddInputs("oCriteria", outbound);

                ExceptionHandling.LogException(excp, _ClassCode + "001", oExcpData);
                throw new WMSExceptionMessage() { WMSExceptionCode = ErrorMessages.WMSExceptionCode, WMSMessage = ErrorMessages.WMSExceptionMessage, ShowAsCriticalError = true };
            }
        }
        public bool CheckCarton(Outbound outbound)
        {
            try
            {
                bool result = true;

                string _sSQL = "EXEC [dbo].[USP_Get_OBD_CartonDetails]  ";


                List<SqlParameter> lSQLParam = new List<SqlParameter>();


                if (outbound.CartonSerialNo != null) 
                {
                    SqlParameter oParam = new SqlParameter("@CartonNumber", SqlDbType.NVarChar, 100);
                    oParam.Value = outbound.CartonSerialNo;

                    lSQLParam.Add(oParam);


                    SqlParameter WareParam = new SqlParameter("@WHID", SqlDbType.NVarChar, 100);
                    WareParam.Value = outbound.WareHouseID;

                    lSQLParam.Add(WareParam);

                }



                DataSet _dsResults = base.FillDataSet(_sSQL, lSQLParam);

                if (_dsResults != null && _dsResults.Tables.Count != 0 && _dsResults.Tables[0].Rows.Count != 0)
                {

                    result = true;
                }
                else
                {
                    throw new WMSExceptionMessage() { WMSExceptionCode = ErrorMessages.WMSExceptionCode, WMSMessage = "Invalid Carton Serial Number", ShowAsWarning = true };

                }

                return result;
            }
            catch (WMSExceptionMessage excp)
            {
                throw excp;
            }
            catch (Exception excp)
            {
                ExceptionData oExcpData = new ExceptionData();
                oExcpData.AddInputs("oCriteria", outbound);

                ExceptionHandling.LogException(excp, _ClassCode + "001", oExcpData);
                throw new WMSExceptionMessage() { WMSExceptionCode = ErrorMessages.WMSExceptionCode, WMSMessage = ErrorMessages.WMSExceptionMessage, ShowAsCriticalError = true };
            }
        }
        public List<Outbound> GetPackingCartonInfo(Outbound outbound)
        {
            try
            {
                List<Outbound> _lstOutbounds = new List<Outbound>();

                string _sSQL = "EXEC [dbo].[OBD_SOWISECARTONDETAIL]  ";


                List<SqlParameter> lSQLParam = new List<SqlParameter>();


                if (outbound.SONumber != null || outbound.SONumber != string.Empty)
                {
                    SqlParameter oParam = new SqlParameter("@SONUMBER", SqlDbType.NVarChar, 100);
                    oParam.Value = outbound.SONumber;

                    lSQLParam.Add(oParam);
                }
                if (outbound.CartonSerialNo != null || outbound.CartonSerialNo != string.Empty)
                {
                    SqlParameter oParam = new SqlParameter("@CARTONCODE", SqlDbType.NVarChar, 100);
                    oParam.Value = outbound.CartonSerialNo;

                    lSQLParam.Add(oParam);
                }
                if (outbound.TenantId != null || outbound.TenantId != string.Empty)
                {

                    SqlParameter oParam = new SqlParameter("@TenantID", SqlDbType.NVarChar, 100);
                    oParam.Value = outbound.TenantId;

                    lSQLParam.Add(oParam);
                }

                if (outbound.AccountID != 0)
                {

                    SqlParameter oParam = new SqlParameter("@AccountID", SqlDbType.NVarChar, 100);
                    oParam.Value = outbound.AccountID;

                    lSQLParam.Add(oParam);
                }

                if (outbound.WareHouseID != null || outbound.WareHouseID != string.Empty)
                {

                    SqlParameter oParam = new SqlParameter("@WHID", SqlDbType.NVarChar, 100);
                    oParam.Value = outbound.WareHouseID;

                    lSQLParam.Add(oParam);
                }

                DataSet _dsPickLists = base.FillDataSet(_sSQL, lSQLParam);

                if (_dsPickLists != null && _dsPickLists.Tables.Count != 0 && _dsPickLists.Tables[0].Rows.Count != 0)
                {


                    foreach (DataRow _dtPack in _dsPickLists.Tables[0].Rows)
                    {
                        Outbound _oOutbound = new Outbound()
                        {
                            CartonSerialNo = _dtPack["CartonSerialNO"].ToString(),
                            Mcode = _dtPack["MCode"].ToString(),
                            PickedQty = ConversionUtility.ConvertToDecimal(_dtPack["PickedQuantity"].ToString()),
                            SONumber = _dtPack["SONumber"].ToString()

                        };
                        _lstOutbounds.Add(_oOutbound);


                    }
                }

                else
                {
                    throw new WMSExceptionMessage() { WMSExceptionCode = ErrorMessages.WMSExceptionCode, WMSMessage = "No Data Found For Given Search Criteria.", ShowAsWarning = true };
                }
                return _lstOutbounds;
            }
            catch (WMSExceptionMessage excp)
            {
                throw excp;
            }
            catch (Exception excp)
            {
                ExceptionData oExcpData = new ExceptionData();
                oExcpData.AddInputs("oCriteria", outbound);

                ExceptionHandling.LogException(excp, _ClassCode + "001", oExcpData);
                throw new WMSExceptionMessage() { WMSExceptionCode = ErrorMessages.WMSExceptionCode, WMSMessage = ErrorMessages.WMSExceptionMessage, ShowAsCriticalError = true };
            }

        }
        public List<Outbound> GetOBDRefNos(Outbound outbound)
        {
            try
            {
                List<Outbound> _lstOutbounds = new List<Outbound>();

                string _sSQL = "EXEC [dbo].[Get_OBDNoList]  ";


                List<SqlParameter> lSQLParam = new List<SqlParameter>();


                if (outbound.UserId != 0) 
                {
                    SqlParameter oParam = new SqlParameter("@UserID", SqlDbType.NVarChar, 100);
                    oParam.Value = outbound.UserId;

                    lSQLParam.Add(oParam);
                }
                if (outbound.AccountID != 0)
                {
                    SqlParameter oParam = new SqlParameter("@AccountId", SqlDbType.NVarChar, 100);
                    oParam.Value = outbound.AccountID;

                    lSQLParam.Add(oParam);
                }


                DataSet _dsPickLists = base.FillDataSet(_sSQL, lSQLParam);

                if (_dsPickLists != null && _dsPickLists.Tables.Count != 0 && _dsPickLists.Tables[0].Rows.Count != 0)
                {


                    foreach (DataRow _dtPack in _dsPickLists.Tables[0].Rows)
                    {
                        Outbound _oOutbound = new Outbound()
                        {
                            OutboundID = ConversionUtility.ConvertToInt(_dtPack["OutboundID"].ToString()),
                            OBDNumber = _dtPack["OBDNumber"].ToString()
                            

                        };
                        _lstOutbounds.Add(_oOutbound);


                    }
                }

                else
                {
                    throw new WMSExceptionMessage() { WMSExceptionCode = ErrorMessages.WMSExceptionCode, WMSMessage = "No Data Found For Given Search Criteria.", ShowAsWarning = true };
                }
                return _lstOutbounds;
            }
            catch (WMSExceptionMessage excp)
            {
                throw excp;
            }
            catch (Exception excp)
            {
                ExceptionData oExcpData = new ExceptionData();
                oExcpData.AddInputs("oCriteria", outbound);

                ExceptionHandling.LogException(excp, _ClassCode + "001", oExcpData);
                throw new WMSExceptionMessage() { WMSExceptionCode = ErrorMessages.WMSExceptionCode, WMSMessage = ErrorMessages.WMSExceptionMessage, ShowAsCriticalError = true };
            }

        }

        public List<Outbound> GetRevertOBDLIst(Outbound outbound)
        {
            try
            {
                List<Outbound> _lstOutbounds = new List<Outbound>();

                string _sSQL = "EXEC [dbo].[SP_GET_OBD_RevertOBDList]  ";


                List<SqlParameter> lSQLParam = new List<SqlParameter>();


                if (outbound.UserId != 0)
                {
                    SqlParameter oParam = new SqlParameter("@UserID", SqlDbType.NVarChar, 100);
                    oParam.Value = outbound.UserId;

                    lSQLParam.Add(oParam);
                }

                DataSet _dsPickLists = base.FillDataSet(_sSQL, lSQLParam);

                if (_dsPickLists != null && _dsPickLists.Tables.Count != 0 && _dsPickLists.Tables[0].Rows.Count != 0)
                {


                    foreach (DataRow _dtPack in _dsPickLists.Tables[0].Rows)
                    {
                        Outbound _oOutbound = new Outbound()
                        {
                            OutboundID = ConversionUtility.ConvertToInt(_dtPack["OutboundId"].ToString()),
                            OBDNumber = _dtPack["OBDNumber"].ToString()


                        };
                        _lstOutbounds.Add(_oOutbound);


                    }
                }

                else
                {
                    throw new WMSExceptionMessage() { WMSExceptionCode = ErrorMessages.WMSExceptionCode, WMSMessage = "No Data Found For Given Search Criteria.", ShowAsWarning = true };
                }
                return _lstOutbounds;
            }
            catch (WMSExceptionMessage excp)
            {
                throw excp;
            }
            catch (Exception excp)
            {
                ExceptionData oExcpData = new ExceptionData();
                oExcpData.AddInputs("oCriteria", outbound);

                ExceptionHandling.LogException(excp, _ClassCode + "001", oExcpData);
                throw new WMSExceptionMessage() { WMSExceptionCode = ErrorMessages.WMSExceptionCode, WMSMessage = ErrorMessages.WMSExceptionMessage, ShowAsCriticalError = true };
            }

        }
        public List<Outbound> GetRevertSOLIst(Outbound outbound)
        {
            try
            {
                List<Outbound> _lstOutbounds = new List<Outbound>();

                string _sSQL = "EXEC [dbo].[SP_GET_OBD_Revert_SOList]  ";


                List<SqlParameter> lSQLParam = new List<SqlParameter>();


                if (outbound.UserId != 0)
                {
                    SqlParameter oParam = new SqlParameter("@UserID", SqlDbType.NVarChar, 100);
                    oParam.Value = outbound.UserId;

                    lSQLParam.Add(oParam);
                }
                if (outbound.OutboundID != 0)
                {
                    SqlParameter oParam = new SqlParameter("@OutboundID", SqlDbType.NVarChar, 100);
                    oParam.Value = outbound.OutboundID;

                    lSQLParam.Add(oParam);
                }
                DataSet _dsPickLists = base.FillDataSet(_sSQL, lSQLParam);

                if (_dsPickLists != null && _dsPickLists.Tables.Count != 0 && _dsPickLists.Tables[0].Rows.Count != 0)
                {


                    foreach (DataRow _dtPack in _dsPickLists.Tables[0].Rows)
                    {
                        Outbound _oOutbound = new Outbound()
                        {
                            SOHeaderID = ConversionUtility.ConvertToInt(_dtPack["SOHeaderID"].ToString()),
                            SONumber = _dtPack["SONumber"].ToString()
                        };
                        _lstOutbounds.Add(_oOutbound);


                    }
                }

                else
                {
                    throw new WMSExceptionMessage() { WMSExceptionCode = ErrorMessages.WMSExceptionCode, WMSMessage = "No Data Found For Given Search Criteria.", ShowAsWarning = true };
                }
                return _lstOutbounds;
            }
            catch (WMSExceptionMessage excp)
            {
                throw excp;
            }
            catch (Exception excp)
            {
                ExceptionData oExcpData = new ExceptionData();
                oExcpData.AddInputs("oCriteria", outbound);

                ExceptionHandling.LogException(excp, _ClassCode + "001", oExcpData);
                throw new WMSExceptionMessage() { WMSExceptionCode = ErrorMessages.WMSExceptionCode, WMSMessage = ErrorMessages.WMSExceptionMessage, ShowAsCriticalError = true };
            }

        }
        public List<Outbound> GetRevertSOOBDInfo(Outbound outbound)
        {
            try
            {
                List<Outbound> _lstOutbounds = new List<Outbound>();

                string _sSQL = "EXEC [dbo].[SP_GET_OBD_SO_Info]  ";


                List<SqlParameter> lSQLParam = new List<SqlParameter>();


                if (outbound.OutboundID != 0)
                {
                    SqlParameter oParam = new SqlParameter("@OutboundID", SqlDbType.NVarChar, 100);
                    oParam.Value = outbound.OutboundID;

                    lSQLParam.Add(oParam);
                }
                if (outbound.SONumber != "")
                {
                    SqlParameter oParam = new SqlParameter("@SoNumber", SqlDbType.NVarChar, 100);
                    oParam.Value = outbound.SONumber;

                    lSQLParam.Add(oParam);
                }

                if (outbound.AccountID != 0)
                {
                    SqlParameter oParam = new SqlParameter("@AcccountID", SqlDbType.NVarChar, 100);
                    oParam.Value = outbound.AccountID;

                    lSQLParam.Add(oParam);
                }
                DataSet _dsPickLists = base.FillDataSet(_sSQL, lSQLParam);

                if (_dsPickLists != null && _dsPickLists.Tables.Count != 0 && _dsPickLists.Tables[0].Rows.Count != 0)
                {


                    foreach (DataRow _dtPack in _dsPickLists.Tables[0].Rows)
                    {
                        Outbound _oOutbound = new Outbound()
                        {
                            SOHeaderID = ConversionUtility.ConvertToInt(_dtPack["SOHeaderID"].ToString()),
                            SONumber = _dtPack["SONumber"].ToString(),
                            OBDNumber = _dtPack["OBDNumber"].ToString(),
                            Status = _dtPack["OutboundStatus"].ToString(),
                            BusinessType = _dtPack["BusinessType"].ToString(),
                            OutboundID = ConversionUtility.ConvertToInt(_dtPack["OutboundID"].ToString())
                        };
                        _lstOutbounds.Add(_oOutbound);
                    }
                }

                else
                {
                    throw new WMSExceptionMessage() { WMSExceptionCode = ErrorMessages.WMSExceptionCode, WMSMessage = "No Data Found For Given Search Criteria.", ShowAsWarning = true };
                }
                return _lstOutbounds;
            }
            catch (WMSExceptionMessage excp)
            {
                throw excp;
            }
            catch (Exception excp)
            {
                ExceptionData oExcpData = new ExceptionData();
                oExcpData.AddInputs("oCriteria", outbound);

                ExceptionHandling.LogException(excp, _ClassCode + "001", oExcpData);
                throw new WMSExceptionMessage() { WMSExceptionCode = ErrorMessages.WMSExceptionCode, WMSMessage = ErrorMessages.WMSExceptionMessage, ShowAsCriticalError = true };
            }

        }
        public List<Outbound> GetRevertCartonCheck(Outbound outbound)
        {
            try
            {
                List<Outbound> _lstOutbounds = new List<Outbound>();

                string _sSQL = "EXEC [dbo].[SP_GET_OBD_CartonCheck]  ";


                List<SqlParameter> lSQLParam = new List<SqlParameter>();


                if (outbound.OutboundID != 0)
                {
                    SqlParameter oParam = new SqlParameter("@OutboundID", SqlDbType.NVarChar, 100);
                    oParam.Value = outbound.OutboundID;

                    lSQLParam.Add(oParam);
                }
                if (outbound.SOHeaderID != 0)
                {
                    SqlParameter oParam = new SqlParameter("@SOHeaderID", SqlDbType.NVarChar, 100);
                    oParam.Value = outbound.SOHeaderID;

                    lSQLParam.Add(oParam);
                }
                if (outbound.CartonSerialNo != "")
                {
                    SqlParameter oParam = new SqlParameter("@CartonSerailNo", SqlDbType.NVarChar, 100);
                    oParam.Value = outbound.CartonSerialNo;

                    lSQLParam.Add(oParam);
                }
                DataSet _dsPickLists = base.FillDataSet(_sSQL, lSQLParam);

                if (_dsPickLists != null && _dsPickLists.Tables.Count != 0 && _dsPickLists.Tables[0].Rows.Count != 0)
                {


                    foreach (DataRow _dtPack in _dsPickLists.Tables[0].Rows)
                    {
                        Outbound _oOutbound = new Outbound()
                        {
                            Status = _dtPack["Result"].ToString()
                        };
                        _lstOutbounds.Add(_oOutbound);
                    }
                }

                else
                {
                    throw new WMSExceptionMessage() { WMSExceptionCode = ErrorMessages.WMSExceptionCode, WMSMessage = "No Data Found For Given Search Criteria.", ShowAsWarning = true };
                }
                return _lstOutbounds;
            }
            catch (WMSExceptionMessage excp)
            {
                throw excp;
            }
            catch (Exception excp)
            {
                ExceptionData oExcpData = new ExceptionData();
                oExcpData.AddInputs("oCriteria", outbound);

                ExceptionHandling.LogException(excp, _ClassCode + "001", oExcpData);
                throw new WMSExceptionMessage() { WMSExceptionCode = ErrorMessages.WMSExceptionCode, WMSMessage = ErrorMessages.WMSExceptionMessage, ShowAsCriticalError = true };
            }

        }
        public List<Outbound> GetScanqtyvalidation(Outbound outbound)
        {
            try
            {
                List<Outbound> _lstOutbounds = new List<Outbound>();

                string _sSQL = "EXEC [dbo].[SP_GET_OBD_MMT_Revet_ScanValidation]  ";


                List<SqlParameter> lSQLParam = new List<SqlParameter>();


                if (outbound.OutboundID != 0)
                {
                    SqlParameter oParam = new SqlParameter("@OutboundID", SqlDbType.NVarChar, 100);
                    oParam.Value = outbound.OutboundID;

                    lSQLParam.Add(oParam);
                }
                if (outbound.SOHeaderID != 0)
                {
                    SqlParameter oParam = new SqlParameter("@SOHeaderID", SqlDbType.NVarChar, 100);
                    oParam.Value = outbound.SOHeaderID;

                    lSQLParam.Add(oParam);
                }
                if (outbound.CartonSerialNo != "" && outbound.CartonSerialNo != null)
                {
                    SqlParameter oParam = new SqlParameter("@CartonSerailNo", SqlDbType.NVarChar, 100);
                    oParam.Value = outbound.CartonSerialNo;

                    lSQLParam.Add(oParam);
                }
                if (outbound.Mcode != "")
                {
                    SqlParameter oParam = new SqlParameter("@SKU", SqlDbType.NVarChar, 100);
                    oParam.Value = outbound.Mcode;

                    lSQLParam.Add(oParam);
                }
                if (outbound.BatchNo != "" && outbound.BatchNo != null)
                {
                    SqlParameter oParam = new SqlParameter("@BatchNo", SqlDbType.NVarChar, 100);
                    oParam.Value = outbound.BatchNo;

                    lSQLParam.Add(oParam);
                }
                if (outbound.MFGDate != "" && outbound.MFGDate != null)
                {
                    SqlParameter oParam = new SqlParameter("@MfgDate", SqlDbType.NVarChar, 100);
                    oParam.Value = outbound.MFGDate;

                    lSQLParam.Add(oParam);
                }
                if (outbound.EXPDate != "" && outbound.EXPDate != null)
                {
                    SqlParameter oParam = new SqlParameter("@ExpDate", SqlDbType.NVarChar, 100);
                    oParam.Value = outbound.EXPDate;

                    lSQLParam.Add(oParam);
                }
                if (outbound.SerialNo != "" && outbound.SerialNo != null)
                {
                    SqlParameter oParam = new SqlParameter("@SerialNo", SqlDbType.NVarChar, 100);
                    oParam.Value = outbound.SerialNo;

                    lSQLParam.Add(oParam);
                }
                if (outbound.ProjectRefNo != "" && outbound.ProjectRefNo != null)
                {
                    SqlParameter oParam = new SqlParameter("@ProjectRefNo", SqlDbType.NVarChar, 100);
                    oParam.Value = outbound.ProjectRefNo;

                    lSQLParam.Add(oParam);
                }
                if (outbound.MRP != "" && outbound.MRP != null)
                {
                    SqlParameter oParam = new SqlParameter("@MRP", SqlDbType.NVarChar, 100);
                    oParam.Value = outbound.MRP;

                    lSQLParam.Add(oParam);
                }
                DataSet _dsPickLists = base.FillDataSet(_sSQL, lSQLParam);

                if (_dsPickLists != null && _dsPickLists.Tables.Count != 0 && _dsPickLists.Tables[0].Rows.Count != 0)
                {


                    foreach (DataRow _dtPack in _dsPickLists.Tables[0].Rows)
                    {
                        Outbound _oOutbound = new Outbound()
                        {
                            Status = _dtPack["Result"].ToString(),
                            SOQty = ConversionUtility.ConvertToDecimal(_dtPack["Qty"].ToString())
                        };
                        _lstOutbounds.Add(_oOutbound);
                    }
                }

                else
                {
                    throw new WMSExceptionMessage() { WMSExceptionCode = ErrorMessages.WMSExceptionCode, WMSMessage = "No Data Found For Given Search Criteria.", ShowAsWarning = true };
                }
                return _lstOutbounds;
            }
            catch (WMSExceptionMessage excp)
            {
                throw excp;
            }
            catch (Exception excp)
            {
                ExceptionData oExcpData = new ExceptionData();
                oExcpData.AddInputs("oCriteria", outbound);

                ExceptionHandling.LogException(excp, _ClassCode + "001", oExcpData);
                throw new WMSExceptionMessage() { WMSExceptionCode = ErrorMessages.WMSExceptionCode, WMSMessage = ErrorMessages.WMSExceptionMessage, ShowAsCriticalError = true };
            }

        }
        public List<Outbound> UpsertHHTOBDRevert(Outbound outbound)
        {
            try
            {
                List<Outbound> _lstOutbounds = new List<Outbound>();

                string _sSQL = "EXEC [dbo].[SP_SET_HHT_Load_Pack_Revert]  ";


                List<SqlParameter> lSQLParam = new List<SqlParameter>();


                if (outbound.OutboundID != 0)
                {
                    SqlParameter oParam = new SqlParameter("@OutboundID", SqlDbType.NVarChar, 100);
                    oParam.Value = outbound.OutboundID;

                    lSQLParam.Add(oParam);
                }
                if (outbound.SOHeaderID != 0)
                {
                    SqlParameter oParam = new SqlParameter("@SOHeaderID", SqlDbType.NVarChar, 100);
                    oParam.Value = outbound.SOHeaderID;

                    lSQLParam.Add(oParam);
                }
                if (outbound.CartonSerialNo != "" && outbound.CartonSerialNo != null)
                {
                    SqlParameter oParam = new SqlParameter("@CartonSerailNo", SqlDbType.NVarChar, 100);
                    oParam.Value = outbound.CartonSerialNo;

                    lSQLParam.Add(oParam);
                }
                if (outbound.Mcode != "" && outbound.Mcode != null)
                {
                    SqlParameter oParam = new SqlParameter("@SKU", SqlDbType.NVarChar, 100);
                    oParam.Value = outbound.Mcode;

                    lSQLParam.Add(oParam);
                }
                if (outbound.BatchNo != "" && outbound.BatchNo != null)
                {
                    SqlParameter oParam = new SqlParameter("@BatchNo", SqlDbType.NVarChar, 100);
                    oParam.Value = outbound.BatchNo;

                    lSQLParam.Add(oParam);
                }
                if (outbound.MFGDate != "" && outbound.MFGDate != null)
                {
                    SqlParameter oParam = new SqlParameter("@MfgDate", SqlDbType.NVarChar, 100);
                    oParam.Value = outbound.MFGDate;

                    lSQLParam.Add(oParam);
                }
                if (outbound.EXPDate != "" && outbound.EXPDate != null)
                {
                    SqlParameter oParam = new SqlParameter("@ExpDate", SqlDbType.NVarChar, 100);
                    oParam.Value = outbound.EXPDate;

                    lSQLParam.Add(oParam);
                }
                if (outbound.SerialNo != "" && outbound.SerialNo != null)
                {
                    SqlParameter oParam = new SqlParameter("@SerialNo", SqlDbType.NVarChar, 100);
                    oParam.Value = outbound.SerialNo;

                    lSQLParam.Add(oParam);
                }
                if (outbound.ProjectRefNo != "" && outbound.ProjectRefNo != null)
                {
                    SqlParameter oParam = new SqlParameter("@ProjectRefNo", SqlDbType.NVarChar, 100);
                    oParam.Value = outbound.ProjectRefNo;

                    lSQLParam.Add(oParam);
                }
                if (outbound.MRP != "" && outbound.MRP != null)
                {
                    SqlParameter oParam = new SqlParameter("@MRP", SqlDbType.NVarChar, 100);
                    oParam.Value = outbound.MRP;

                    lSQLParam.Add(oParam);
                }
                if (outbound.PackedQty != 0)
                {
                    SqlParameter oParam = new SqlParameter("@RevertQty", SqlDbType.NVarChar, 100);
                    oParam.Value = outbound.PackedQty;

                    lSQLParam.Add(oParam);
                }
                if (outbound.UserId != 0)
                {
                    SqlParameter oParam = new SqlParameter("@CreatedBy", SqlDbType.NVarChar, 100);
                    oParam.Value = outbound.UserId;

                    lSQLParam.Add(oParam);
                }
                DataSet _dsPickLists = base.FillDataSet(_sSQL, lSQLParam);

                if (_dsPickLists != null && _dsPickLists.Tables.Count != 0 && _dsPickLists.Tables[0].Rows.Count != 0)
                {


                    foreach (DataRow _dtPack in _dsPickLists.Tables[0].Rows)
                    {
                        Outbound _oOutbound = new Outbound()
                        {
                            Status = _dtPack["S"].ToString()
                        };
                        _lstOutbounds.Add(_oOutbound);
                    }
                }

                else
                {
                    throw new WMSExceptionMessage() { WMSExceptionCode = ErrorMessages.WMSExceptionCode, WMSMessage = "No Data Found For Given Search Criteria.", ShowAsWarning = true };
                }
                return _lstOutbounds;
            }
            catch (WMSExceptionMessage excp)
            {
                throw excp;
            }
            catch (Exception excp)
            {
                ExceptionData oExcpData = new ExceptionData();
                oExcpData.AddInputs("oCriteria", outbound);

                ExceptionHandling.LogException(excp, _ClassCode + "001", oExcpData);
                throw new WMSExceptionMessage() { WMSExceptionCode = ErrorMessages.WMSExceptionCode, WMSMessage = ErrorMessages.WMSExceptionMessage, ShowAsCriticalError = true };
            }

        }

    }
}
