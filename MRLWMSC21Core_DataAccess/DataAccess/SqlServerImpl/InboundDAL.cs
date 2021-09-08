using System;
using System.Collections.Generic;
using System.Text;
using MRLWMSC21Core.Library;
using MRLWMSC21Core.Entities;
using System.Data.SqlClient;
using System.Data;

namespace MRLWMSC21Core.DataAccess
{
    public class InboundDAL : BaseDAL, Interfaces.iInboundDAL
    {

        private SqlConnection _oSQLConnection;

        private string _ClassCode = string.Empty;

        public InboundDAL(int LoginUser, string ConnectionString) : base(LoginUser, ConnectionString)
        {
            _ClassCode = ExceptionHandling.GetClassExceptionCode(ExceptionHandling.ExcpConstants_API_DataLayer.InboundDAL);
        }

        //~BaseDAL()
        //{
        //    // Desctructor / Finalizer to de-allocate the memory for the Data Members.

        //    _oSQLConnection.Close();

        //    _sConnectionString = null;
        //    _oSQLConnection = null;
        //}

        //Developed & Tested
        public Inbound FetchInventoryMap(Inbound oInbound)
        {

            try
            {

                // Method block to Fetch the Entity List from Database.

                List<Inbound> oInboundget = null;

                SearchCriteria oSearch = new SearchCriteria()
                {
                    InboundID = oInbound.InboundID
                };

                oInboundget = GetInboundListSearch(oSearch);

                return oInboundget[0];


            }
            catch (WMSExceptionMessage excp)
            {
                throw excp;
            }
            catch (Exception ex)
            {
                ExceptionHandling.LogException(ex, _ClassCode + "001");

                return null;
            }

        }

        //Developed & Tested
        public Inbound GetInboundByID(int InboundID)
        {

            try
            {

                List<Inbound> oInboundget = null;

                SearchCriteria oSearch = new SearchCriteria()
                {
                    InboundID = InboundID
                };

                oInboundget = GetInboundListSearch(oSearch);

                return oInboundget[0];

            }
            catch (WMSExceptionMessage excp)
            {
                throw excp;
            }
            catch (Exception ex)
            {
                ExceptionHandling.LogException(ex, _ClassCode + "002");

                return null;
            }
        }

        //Developed & Tested
        public Inbound GetInboundByIDInventory(int InboundID, bool FetchInventoryMap)
        {
            try
            {

                // Method block to Fetch the Entity List from Database.

                Inbound _oInbound = null;

                string _sSQL = "EXEC [sp_API_INB_GetInboundDetails] @InboundID='" + InboundID + "', @InvMapFlag ='" + FetchInventoryMap.ToString() + "'";

                DataSet _dsResults = base.FillDataSet(_sSQL);

                if (_dsResults == null)
                    throw new Exception("Get Inbound Details SP is Dataset is NULL");
                else if (_dsResults.Tables.Count < 3)
                    throw new Exception("Get Inbound Details SP Data Set returned less than 3 Data Tables.");
                else if (_dsResults.Tables[0].Rows.Count != 1)
                    throw new Exception("Get Inbound Details SP Table 0 does not have an Inbound record in table with Index 0.");

                _oInbound = BindInboundData(_dsResults.Tables[0].Rows[0], _dsResults.Tables[1], _dsResults.Tables[2]);

                return _oInbound;
            }
            catch (WMSExceptionMessage excp)
            {
                throw excp;
            }
            catch (Exception ex)
            {
                ExceptionHandling.LogException(ex, _ClassCode + "003");

                return null;
            }
        }

        //Developed & Tested
        public List<Inbound> GetInboundList(int AccountID)
        {
            try
            {

                List<Inbound> _lInbound = new List<Inbound>();

                SearchCriteria oSearch = new SearchCriteria()
                {
                    AccountID = AccountID
                };

                _lInbound = GetInboundListSearch(oSearch);

                return _lInbound;


            }
            catch (WMSExceptionMessage excp)
            {
                throw excp;
            }
            catch (Exception ex)
            {
                ExceptionHandling.LogException(ex, _ClassCode + "004");

                return null;
            }
        }

        //Developed & Tested
        public List<Inbound> GetInboundListSearch(SearchCriteria oCriteria)
        {

            try
            {

                // Method block to Fetch the Entity List from Database.

                List<Inbound> _lInbound = new List<Inbound>();

                // string _sSQL = "EXEC [dbo].[sp_API_InvoiceDetails] @InboundID ='" + oCriteria.InboundID.ToString() + ", @POHeaderID = '" + oCriteria.POHeaderID.ToString() + "', @PODetailsID = '" + oCriteria.PODetailsID.ToString() + "', @SupplierInvoiceID = '" + oCriteria.SupplierInvoiceID.ToString() + "', @TenantID = '" + oCriteria.TenantID.ToString() + "', @WarehouseID = '" + oCriteria.WarehouseID.ToString() + "', @SupplierID = '" + oCriteria.SupplierID.ToString() + "', @AccountID = '" + oCriteria.AccountID.ToString() + "', @StoreRefNo = '" + oCriteria.StoreRefNo.ToString() + "', @LoggedInUserID="+oCriteria.UserID.ToString()+ ", @FetchInboundsForUserID =1";

                StringBuilder _sbSQL = new StringBuilder();
                List<SqlParameter> lSQLParams = new List<SqlParameter>();

                _sbSQL.Append("EXEC [dbo].[sp_API_INB_GetInboundDetails]");

                if (oCriteria.InboundID > 0)
                {
                    // _sbSQL.Append("@InboundID ='" + oCriteria.InboundID.ToString());

                    SqlParameter oParam = new SqlParameter("@InboundID", SqlDbType.Int);
                    oParam.Value = oCriteria.InboundID;

                    lSQLParams.Add(oParam);
                }


                if (oCriteria.StoreRefNo != null)
                {
                    // _sbSQL.Append("@InboundID ='" + oCriteria.InboundID.ToString());

                    SqlParameter oParam = new SqlParameter("@StoreRefNo", SqlDbType.NVarChar, 50);
                    oParam.Value = oCriteria.StoreRefNo;

                    lSQLParams.Add(oParam);
                }


                if (0 > 0)
                {
                    // _sbSQL.Append("@InboundID ='" + oCriteria.InboundID.ToString());

                    SqlParameter oParam = new SqlParameter("@ShipmentTypeID", SqlDbType.Int);
                    oParam.Value = 1;

                    lSQLParams.Add(oParam);
                }

                if (oCriteria.SupplierID > 0)
                {
                    // _sbSQL.Append("@InboundID ='" + oCriteria.InboundID.ToString());

                    SqlParameter oParam = new SqlParameter("@SupplierID", SqlDbType.Int);
                    oParam.Value = oCriteria.SupplierID;

                    lSQLParams.Add(oParam);
                }


                if (oCriteria.POHeaderID > 0)
                {
                    SqlParameter oParam = new SqlParameter("@POHeaderID", SqlDbType.Int);
                    oParam.Value = oCriteria.POHeaderID;

                    lSQLParams.Add(oParam);
                }


                if (oCriteria.PODetailsID > 0)
                {
                    SqlParameter oParam = new SqlParameter("@PODetailsID", SqlDbType.Int);
                    oParam.Value = oCriteria.PODetailsID;

                    lSQLParams.Add(oParam);
                }

                if (oCriteria.SupplierInvoiceID > 0)
                {
                    SqlParameter oParam = new SqlParameter("@SupplierInvoiceID", SqlDbType.Int);
                    oParam.Value = oCriteria.SupplierInvoiceID;

                    lSQLParams.Add(oParam);
                }

                if (oCriteria.TenantID > 0)
                {
                    SqlParameter oParam = new SqlParameter("@TenantID", SqlDbType.Int);
                    oParam.Value = oCriteria.TenantID;

                    lSQLParams.Add(oParam);
                }

                if (oCriteria.WarehouseID > 0)
                {
                    SqlParameter oParam = new SqlParameter("@WarehouseID", SqlDbType.Int);
                    oParam.Value = oCriteria.WarehouseID;

                    lSQLParams.Add(oParam);
                }

                if (oCriteria.SupplierID > 0)
                {
                    SqlParameter oParam = new SqlParameter("@SupplierID", SqlDbType.Int);
                    oParam.Value = oCriteria.SupplierID;

                    lSQLParams.Add(oParam);
                }


                if (oCriteria.AccountID > 0)
                {
                    SqlParameter oParam = new SqlParameter("@AccountID", SqlDbType.Int);
                    oParam.Value = oCriteria.AccountID;

                    lSQLParams.Add(oParam);
                }


                if (oCriteria.VehicleNumber !=null)
                {
                    SqlParameter oParam = new SqlParameter("@VehicleNumber", SqlDbType.NVarChar, 50);
                    oParam.Value = oCriteria.VehicleNumber;

                    lSQLParams.Add(oParam);
                }

                if (oCriteria.LoggedInUserID > 0)
                {
                    SqlParameter oParam = new SqlParameter("@LoggedInUserID", SqlDbType.Int);
                    oParam.Value = oCriteria.LoggedInUserID;

                    lSQLParams.Add(oParam);

                    SqlParameter oParam1 = new SqlParameter("@FetchInboundsForUserID", SqlDbType.Int);
                    oParam1.Value = oCriteria.LoggedInUserID;

                    lSQLParams.Add(oParam1);
                }

                DataSet _dsResults = base.FillDataSet(_sbSQL.ToString(), lSQLParams);

                if (_dsResults != null)
                    if (_dsResults.Tables.Count > 0)
                    {

                        foreach (DataRow _drInbound in _dsResults.Tables[0].Rows)
                        {
                            Inbound _TempInbound;
                            _TempInbound = BindInboundData(_drInbound, null, null);

                            _lInbound.Add(_TempInbound);
                        }
                    }

                return _lInbound;
            }
            catch (WMSExceptionMessage excp)
            {
                throw excp;
            }
            catch (Exception ex)
            {
                ExceptionHandling.LogException(ex, _ClassCode + "005");

                return null;
            }
        }

        public List<Inbound> GetStoreRefNos(Inbound inbound)
        {
            try
            {
                string _sSQL = "EXEC  [dbo].[Get_StoreRefnoList_HHT] ";



                List<SqlParameter> lSQLParams = new List<SqlParameter>();
                
                List<Inbound> lInbound = new List<Inbound>();
                
                if (inbound.CreatedByUserID > 0)

                {

                    SqlParameter oSQLParam = new SqlParameter("@UserID", SqlDbType.Int);

                    oSQLParam.Value = inbound.CreatedByUserID;



                    lSQLParams.Add(oSQLParam);

                }


                if (inbound.AccountID > 0)

                {

                    SqlParameter oSQLParam = new SqlParameter("@AccountId", SqlDbType.Int);

                    oSQLParam.Value = inbound.AccountID;



                    lSQLParams.Add(oSQLParam);

                }

                DataSet _dsSuggestions = FillDataSet(_sSQL, lSQLParams);

                if (_dsSuggestions != null)
                {
                    if (_dsSuggestions.Tables.Count > 0)
                    {
                        foreach (DataRow _drPickList in _dsSuggestions.Tables[0].Rows)
                        {
                            Inbound _oInbound = new Inbound()
                            {
                                InboundID = ConversionUtility.ConvertToInt(_drPickList["InboundID"].ToString()),
                                StoreRefNo = _drPickList["Storerefno"].ToString(),
                                InvoiceQty = ConversionUtility.ConvertToDecimal(_drPickList["InvoiceQuantity"].ToString()),
                                ReceivedQty = ConversionUtility.ConvertToDecimal(_drPickList["ReceivedQty"].ToString()),
                                VehicleNumber = _drPickList["VehicleRegNo"].ToString(),
                                DockNumber = _drPickList["Dock"].ToString(),
                                DockID =ConversionUtility.ConvertToInt(_drPickList["DockID"].ToString())

                            };

                            lInbound.Add(_oInbound);
                        }
                    }
                }



                return lInbound;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public Location GeneratePalletGateSuggestions(SearchCriteria oCriteria)

        {

            try

            {

                if (oCriteria.ContainerCode == null)

                    throw new WMSExceptionMessage() { WMSExceptionCode = "EMC_IB_DAL_001", WMSMessage = ErrorMessages.EMC_IB_DAL_001, ShowAsError = true };





                string _sSQL = "EXEC  [dbo].[USP_API_GenerateGateSuggestions] ";



                List<SqlParameter> lSQLParams = new List<SqlParameter>();



                List<Suggestion> lSuggestions = new List<Suggestion>();



                if (oCriteria.ContainerCode != null)

                {

                    SqlParameter oSQLParam = new SqlParameter("@CartonCode", SqlDbType.NVarChar, 50);

                    oSQLParam.Value = oCriteria.ContainerCode;



                    lSQLParams.Add(oSQLParam);

                }





                if (oCriteria.ContainerID > 0)

                {

                    SqlParameter oSQLParam = new SqlParameter("@CartonID", SqlDbType.Int);

                    oSQLParam.Value = oCriteria.ContainerID;



                    lSQLParams.Add(oSQLParam);

                }





                DataSet _dsSuggestions = FillDataSet(_sSQL, lSQLParams);

                Location _oLocation = new Location();



                if (_dsSuggestions != null)

                {

                    if (_dsSuggestions.Tables.Count > 0)

                    {
                        if (_dsSuggestions.Tables[0].Rows.Count > 0)
                            _oLocation.AssociatedGateOfAccess = _dsSuggestions.Tables[0].Rows[0]["AssociatedGate"].ToString();

                        else
                            throw new WMSExceptionMessage() { WMSExceptionCode = "EMC_IB_DAL_005", WMSMessage = ErrorMessages.EMC_IB_DAL_005, ShowAsError = true };

                    }

                }

                else

                    throw new Exception(" Procedure " + _sSQL.Remove(0, 4) + " did not return any dataset.");



                return _oLocation;

            }

            catch (WMSExceptionMessage excp)

            {

                throw excp;

            }

            catch (Exception excp)

            {

                ExceptionData oExcpData = new ExceptionData();

                oExcpData.AddInputs("oCriteria", oCriteria);



                ExceptionHandling.LogException(excp, _ClassCode + "013", oExcpData);

                throw new WMSExceptionMessage() { WMSExceptionCode = ErrorMessages.WMSExceptionCode, WMSMessage = ErrorMessages.WMSExceptionMessage, ShowAsCriticalError = true };

            }

        }

        public List<Vehicle> GetVehiclesList(SearchCriteria oCriteria)
        {

            try
            {

                // Method block to Fetch the Entity List from Database.

                List<Inbound> _lInbound = new List<Inbound>();

                // string _sSQL = "EXEC [dbo].[sp_API_InvoiceDetails] @InboundID ='" + oCriteria.InboundID.ToString() + ", @POHeaderID = '" + oCriteria.POHeaderID.ToString() + "', @PODetailsID = '" + oCriteria.PODetailsID.ToString() + "', @SupplierInvoiceID = '" + oCriteria.SupplierInvoiceID.ToString() + "', @TenantID = '" + oCriteria.TenantID.ToString() + "', @WarehouseID = '" + oCriteria.WarehouseID.ToString() + "', @SupplierID = '" + oCriteria.SupplierID.ToString() + "', @AccountID = '" + oCriteria.AccountID.ToString() + "', @StoreRefNo = '" + oCriteria.StoreRefNo.ToString() + "', @LoggedInUserID="+oCriteria.UserID.ToString()+ ", @FetchInboundsForUserID =1";

                StringBuilder _sbSQL = new StringBuilder();
                List<SqlParameter> lSQLParams = new List<SqlParameter>();

                _sbSQL.Append("EXEC [dbo].[sp_API_INB_GetInboundDetails]");

                if (oCriteria.InboundID > 0)
                {
                    // _sbSQL.Append("@InboundID ='" + oCriteria.InboundID.ToString());

                    SqlParameter oParam = new SqlParameter("@InboundID", SqlDbType.Int);
                    oParam.Value = oCriteria.InboundID;

                    lSQLParams.Add(oParam);
                }


                if (oCriteria.StoreRefNo != null)
                {
                    // _sbSQL.Append("@InboundID ='" + oCriteria.InboundID.ToString());

                    SqlParameter oParam = new SqlParameter("@StoreRefNo", SqlDbType.NVarChar, 50);
                    oParam.Value = oCriteria.StoreRefNo;

                    lSQLParams.Add(oParam);
                }


                if (0 > 0)
                {
                    // _sbSQL.Append("@InboundID ='" + oCriteria.InboundID.ToString());

                    SqlParameter oParam = new SqlParameter("@ShipmentTypeID", SqlDbType.Int);
                    oParam.Value = 1;

                    lSQLParams.Add(oParam);
                }

                if (oCriteria.SupplierID > 0)
                {
                    // _sbSQL.Append("@InboundID ='" + oCriteria.InboundID.ToString());

                    SqlParameter oParam = new SqlParameter("@SupplierID", SqlDbType.Int);
                    oParam.Value = oCriteria.SupplierID;

                    lSQLParams.Add(oParam);
                }


                if (oCriteria.POHeaderID > 0)
                {
                    SqlParameter oParam = new SqlParameter("@POHeaderID", SqlDbType.Int);
                    oParam.Value = oCriteria.POHeaderID;

                    lSQLParams.Add(oParam);
                }


                if (oCriteria.PODetailsID > 0)
                {
                    SqlParameter oParam = new SqlParameter("@PODetailsID", SqlDbType.Int);
                    oParam.Value = oCriteria.PODetailsID;

                    lSQLParams.Add(oParam);
                }

                if (oCriteria.SupplierInvoiceID > 0)
                {
                    SqlParameter oParam = new SqlParameter("@SupplierInvoiceID", SqlDbType.Int);
                    oParam.Value = oCriteria.SupplierInvoiceID;

                    lSQLParams.Add(oParam);
                }

                if (oCriteria.TenantID > 0)
                {
                    SqlParameter oParam = new SqlParameter("@TenantID", SqlDbType.Int);
                    oParam.Value = oCriteria.TenantID;

                    lSQLParams.Add(oParam);
                }

                if (oCriteria.WarehouseID > 0)
                {
                    SqlParameter oParam = new SqlParameter("@WarehouseID", SqlDbType.Int);
                    oParam.Value = oCriteria.WarehouseID;

                    lSQLParams.Add(oParam);
                }

                if (oCriteria.SupplierID > 0)
                {
                    SqlParameter oParam = new SqlParameter("@SupplierID", SqlDbType.Int);
                    oParam.Value = oCriteria.SupplierID;

                    lSQLParams.Add(oParam);
                }


                if (oCriteria.AccountID > 0)
                {
                    SqlParameter oParam = new SqlParameter("@AccountID", SqlDbType.Int);
                    oParam.Value = oCriteria.AccountID;

                    lSQLParams.Add(oParam);
                }

                if (oCriteria.LoggedInUserID > 0)
                {
                    SqlParameter oParam = new SqlParameter("@LoggedInUserID", SqlDbType.Int);
                    oParam.Value = oCriteria.LoggedInUserID;

                    lSQLParams.Add(oParam);

                    SqlParameter oParam1 = new SqlParameter("@FetchInboundsForUserID", SqlDbType.Int);
                    oParam1.Value = oCriteria.LoggedInUserID;

                    lSQLParams.Add(oParam1);
                }


                if (oCriteria.VehicleNumber != null)
                {
                    SqlParameter oParam = new SqlParameter("@VehicleNumber", SqlDbType.NVarChar, 50);
                    oParam.Value = oCriteria.VehicleNumber;

                    lSQLParams.Add(oParam);
                }

                if (oCriteria.LoggedInUserID > 0)
                {
                    SqlParameter oParam = new SqlParameter("@InvMapFlag", SqlDbType.Bit);
                    oParam.Value = 0;

                    lSQLParams.Add(oParam);
                }


                DataSet _dsResults = base.FillDataSet(_sbSQL.ToString(), lSQLParams);

                List<Vehicle> lVehicle = new List<Vehicle>();

                if (_dsResults != null)
                    if (_dsResults.Tables.Count > 0)
                    {
                        foreach (DataRow _drVehicles in _dsResults.Tables[2].Rows)
                        {
                            lVehicle.Add(BindVehicleData(_drVehicles, _dsResults.Tables[0], _dsResults.Tables[1]));
                        }
                    }

                return lVehicle;
            }
            catch (WMSExceptionMessage excp)
            {
                throw excp;
            }
            catch (Exception ex)
            {
                ExceptionHandling.LogException(ex, _ClassCode + "008");

                return null;
            }
        }

        private Vehicle BindVehicleData(DataRow _drVehicle, DataTable dtInbound, DataTable dtInventory)
        {

            Vehicle oVehicle = new Vehicle()
            {
                VehicleID = ConversionUtility.ConvertToInt(_drVehicle["VehicleID"].ToString()),
                VehicleNumber = _drVehicle["VehicleNumber"].ToString(),
                DocumentQuantity = ConversionUtility.ConvertToDecimal(_drVehicle["NoofPackagesInDocument"].ToString()),
                ReceivedQuantity = ConversionUtility.ConvertToDecimal(_drVehicle["ReceivedQuantity"].ToString()),
                VehicleInventoryQuantity = ConversionUtility.ConvertToDecimal(_drVehicle["VehicleDocQty"].ToString()),
                VehicleReceivedQuantity = ConversionUtility.ConvertToDecimal(_drVehicle["VehicleReceivedQty"].ToString())
            };

            dtInbound.DefaultView.RowFilter = "InboundID=" + _drVehicle["InboundID"].ToString();

            List<Inbound> lInboundsForVehicle = new List<Inbound>();

            foreach (DataRow _drInbound in dtInbound.DefaultView.ToTable().Rows)
            {
                Inbound _TempInbound;
                _TempInbound = BindInboundData(_drInbound, null, null);

                lInboundsForVehicle.Add(_TempInbound);
            }

            oVehicle.InboundList = lInboundsForVehicle;
            return oVehicle;
        }

        private Inbound BindInboundData(DataRow _drInbound, DataTable _dtInventory, DataTable _dtPalletization)
        {

            Inbound _oInbound = new Inbound()
            {
                InboundID = ConversionUtility.ConvertToInt(_drInbound["InboundID"].ToString()),
                NoofPackagesInDocument = ConversionUtility.ConvertToInt(_drInbound["NoofPackagesInDocument"].ToString()),
                GrossWeight = ConversionUtility.ConvertToDecimal(_drInbound["GrossWeight"].ToString()),


                StoreRefNo = _drInbound["StoreRefNo"].ToString(),
                GRNNumber = _drInbound["GRNNumber"].ToString(),
                ShipmentType = _drInbound["ShipmentType"].ToString(),
                SupplierName = _drInbound["SupplierName"].ToString(),
                InboundStatusName = _drInbound["InboundStatus"].ToString(),


                DocReceivedDate = DateTimeUtilities.FormatDateTime(_drInbound["DocReceivedDate"].ToString()),
                ShipmentExpectedDate = DateTimeUtilities.FormatDateTime(_drInbound["ShipmentExpectedDate"].ToString()),

                ShipmentTypeID = ConversionUtility.ConvertToInt(_drInbound["ShipmentTypeID"].ToString()),
                InboundStatusID = ConversionUtility.ConvertToInt(_drInbound["InboundStatusID"].ToString()),
                TenantID = ConversionUtility.ConvertToInt(_drInbound["TenantID"].ToString()),
                VehicleNumber= _drInbound["RegistrationNumber"].ToString()


            };



            // _oInbound = BindPalletizationZoning(_oInbound, _dtPalletization);
            _oInbound = BindInventory(_oInbound, _dtInventory);
            
            return _oInbound;
        }

        private Inbound BindPalletizationZoning(Inbound _Ref, DataTable _dtData)
        {
            if (_dtData != null)
            {
                _Ref.PalletizationPreferences = new List<PalletizationZoning>();

                foreach (DataRow _drPref in _dtData.Rows)
                {
                    PalletizationZoning oPal = new PalletizationZoning()
                    {
                        InboundID = ConversionUtility.ConvertToInt(_drPref["InboundID"].ToString()),
                        HomeLevelID = ConversionUtility.ConvertToInt(_drPref["HomeLevelID"].ToString()),
                        HomeZoneHeaderID = ConversionUtility.ConvertToInt(_drPref["HomeZoneHeaderID"].ToString()),
                        MaterialMasterID = ConversionUtility.ConvertToInt(_drPref["MaterialMasterID"].ToString()),
                        MaterialRangeID = ConversionUtility.ConvertToInt(_drPref["MaterialRangeID"].ToString()),

                        MaterialCode = _drPref["mcode"].ToString(),

                        IsByLevel = ConversionUtility.ConvertToBool(_drPref["ByLevel"].ToString()),
                        IsByRange = ConversionUtility.ConvertToBool(_drPref["ByRange"].ToString()),
                        IsBySKU = ConversionUtility.ConvertToBool(_drPref["BySKU"].ToString()),
                        IsByZone = ConversionUtility.ConvertToBool(_drPref["ByZone"].ToString())
                    };

                    _Ref.PalletizationPreferences.Add(oPal);

                }
            }
            return _Ref;
        }

        private Inbound BindInventory(Inbound oInbound, DataTable _dtInventory)
        {
            if (_dtInventory != null)
            {
                if (_dtInventory.Rows.Count > 0)
                {
                    List<InboundInventoryMap> _lInventory = new List<InboundInventoryMap>();

                    foreach (DataRow _drInventory in _dtInventory.Rows)
                    {
                        InboundInventoryMap _oIBInvMap = new InboundInventoryMap()
                        {
                            InboundID = ConversionUtility.ConvertToInt(_drInventory["InboundID"].ToString()),
                            POHeaderID = ConversionUtility.ConvertToInt(_drInventory["POHeaderID"].ToString()),
                            PODetailsID = ConversionUtility.ConvertToInt(_drInventory["PODetailsID"].ToString()),
                            SupplierInvoiceID = ConversionUtility.ConvertToInt(_drInventory["SupplierInvoiceID"].ToString()),
                            SupplierInvoiceDetailsID = ConversionUtility.ConvertToInt(_drInventory["SupplierInvoiceDetailsID"].ToString()),
                            OrderLineNumber = ConversionUtility.ConvertToInt(_drInventory["OrderLineNumber"].ToString()),
                            VehicleID = ConversionUtility.ConvertToInt(_drInventory["VehicleID"].ToString()),
                            MaterialMasterID = ConversionUtility.ConvertToInt(_drInventory["MaterialMasterID"].ToString()),
                            IUoMID = ConversionUtility.ConvertToInt(_drInventory["IUoMID"].ToString()),

                            MOP = ConversionUtility.ConvertToInt(_drInventory["MOP"].ToString()),
                            ColorID = ConversionUtility.ConvertToInt(_drInventory["ColourID"].ToString()),

                            PONumber = _drInventory["PONumber"].ToString(),
                            IUoM = _drInventory["IUoM"].ToString(),
                            InvoiceNumber = _drInventory["InvoiceNumber"].ToString(),
                            VehicleNumber = _drInventory["VehicleNumber"].ToString(),
                            MaterialCode = _drInventory["MCode"].ToString(),

                            IUoMConversionQty = ConversionUtility.ConvertToDecimal(_drInventory["IUoMConversionQty"].ToString()),
                            ShipmentQuantity = ConversionUtility.ConvertToDecimal(_drInventory["InvoiceQuantity"].ToString()),
                            ReceivedQuantity = ConversionUtility.ConvertToDecimal(_drInventory["Rcvd_Quantity"].ToString()),

                            GenericSKUID = ConversionUtility.ConvertToInt(_drInventory["SKUGenericCodeID"].ToString()),

                            MRP = ConversionUtility.ConvertToDecimal(_drInventory["MRP"].ToString())
                        };

                        _lInventory.Add(_oIBInvMap);
                    }

                    oInbound.InboundInventoryMap = _lInventory;
                }
            }

            return oInbound;
        }


        public Inbound ValidateRSNAndFetchInbound(Inventory oInventory, out bool IsRSNItemAlreadyReceived, out bool IsPalletizationPermitted)
        {
            Inbound responce = null;
            WMSExceptionMessage exMessage = new WMSExceptionMessage();
            try
            {
                IsRSNItemAlreadyReceived = false;
                IsPalletizationPermitted = false;

                string _sSQL = "EXEC [dbo].[sp_ValidateRSNAndGetInboundDetails]";

                //  @SerialNumber ='" + criteria.MaterialRSN.ToString() + "' ,@InboundID = " + criteria.InboundID.ToString();

                List<SqlParameter> _lsqlParameter = new List<SqlParameter>();

                if (!string.IsNullOrWhiteSpace(oInventory.RSN))
                {
                    SqlParameter oParam1 = new SqlParameter("@SerialNumber", SqlDbType.NVarChar, 50);
                    oParam1.Value = oInventory.RSN;

                    _lsqlParameter.Add(oParam1);
                }

                if (!string.IsNullOrWhiteSpace(oInventory.ReferenceDocumentNumber))
                {
                    SqlParameter oParam1 = new SqlParameter("@StoreRefNumber", SqlDbType.NVarChar, 50);
                    oParam1.Value = oInventory.ReferenceDocumentNumber;

                    _lsqlParameter.Add(oParam1);
                }


                if (oInventory.ReferenceDocumentID > 0)
                {
                    SqlParameter oParam1 = new SqlParameter("@InboundID", SqlDbType.Int);
                    oParam1.Value = oInventory.ReferenceDocumentID;

                    _lsqlParameter.Add(oParam1);
                }


                if (oInventory.Quantity > 0)
                {
                    SqlParameter oParam1 = new SqlParameter("@Quantity", SqlDbType.Decimal);
                    oParam1.Value = oInventory.Quantity;

                    _lsqlParameter.Add(oParam1);
                }

                if (!string.IsNullOrWhiteSpace(oInventory.MaterialCode))
                {
                    SqlParameter oParam1 = new SqlParameter("@Mcode", SqlDbType.NVarChar, 50);
                    oParam1.Value = oInventory.MaterialCode;

                    _lsqlParameter.Add(oParam1);
                }

                if(oInventory.ContainerCode!=null)
                {
                    SqlParameter oPAram = new SqlParameter("@CartonCode", SqlDbType.NVarChar, 50);
                    oPAram.Value = oInventory.ContainerCode;

                    _lsqlParameter.Add(oPAram);
                }


                

                DataSet _dsResults = base.FillDataSet(_sSQL, _lsqlParameter);
                if (_dsResults != null && _dsResults.Tables.Count != 0)
                {
                    if (_dsResults.Tables[0].Rows.Count == 1 && _dsResults.Tables[0].Columns.Count == 1)
                    {
                        switch (_dsResults.Tables[0].Rows[0][0].ToString())
                        {
                            case "-1":
                                exMessage.WMSExceptionCode = "EMC_IB_DAL_0002";
                                exMessage.WMSMessage = ErrorMessages.EMC_IB_DAL_0002;
                                throw exMessage;

                            case "-2":
                                IsRSNItemAlreadyReceived = true;
                                break;

                            case "-3":
                                throw new WMSExceptionMessage() { WMSExceptionCode = "EMC_IB_DAL_0003", WMSMessage = ErrorMessages.EMC_IB_DAL_0003, ShowAsCriticalError = true };
                                break;
                        }
                    }

                    if (_dsResults.Tables.Count == 6)
                    {
                        if (_dsResults.Tables[1].Rows.Count == 0)
                            throw new WMSExceptionMessage() { WMSExceptionCode = "EMC_IB_DAL_006", WMSMessage = ErrorMessages.EMC_IB_DAL_006, ShowAsError = true };

                        responce = BindInboundData(_dsResults.Tables[1].Rows[0], _dsResults.Tables[2], _dsResults.Tables[4]);

                        if (_dsResults.Tables[5].Rows.Count == 1 && _dsResults.Tables[5].Columns.Count == 2)
                            IsPalletizationPermitted = false;
                        else
                            IsPalletizationPermitted = true;
                    }
                    else
                    {
                        exMessage.WMSExceptionCode = ErrorMessages.WMSExceptionCode;
                        exMessage.WMSMessage = ErrorMessages.WMSExceptionMessage;
                        exMessage.ShowAsCriticalError = true;

                        throw exMessage;
                    }
                }
                else
                {
                    exMessage.WMSExceptionCode = ErrorMessages.WMSExceptionCode;
                    exMessage.WMSMessage = ErrorMessages.WMSExceptionMessage;
                    exMessage.ShowAsCriticalError = true;

                    throw exMessage;
                }

                return responce;
                //Method block to Fetch the Entity List from Database.
                //throw new NotImplementedException();

            }
            catch (WMSExceptionMessage excp)
            {
                throw excp;
            }
            catch (Exception ex)
            {

                ExceptionData oExcpData = new ExceptionData();
                oExcpData.AddInputs("oInventory", oInventory);

                ExceptionHandling.LogException(ex, _ClassCode + "006", oExcpData);

                throw new WMSExceptionMessage() { WMSExceptionCode = ErrorMessages.WMSExceptionCode, WMSMessage = ErrorMessages.WMSExceptionMessage, ShowAsCriticalError = true };
            }
        }

        public bool UpdateInbound(Inbound oInbound)
        {
            try
            {
                //Method block to Fetch the Entity List from Database.
                throw new NotImplementedException();

            }
            catch (Exception ex)
            {
                ExceptionHandling.LogException(ex, _ClassCode + "007");
                return false;
            }
        }


        public List<Suggestion> GeneratePutawaySuggestions(SearchCriteria oCriteria)
        {
            try
            {
                string _sSQL = "EXEC[dbo].[USP_TRN_GeneratePutawaySuggestions]";

                List<SqlParameter> lSQLParams = new List<SqlParameter>();

                List<Suggestion> lSuggestions = new List<Suggestion>();

                if (oCriteria.InboundID > 0)
                {
                    SqlParameter oSQLParam = new SqlParameter("@InboundID", SqlDbType.Int);
                    oSQLParam.Value = oCriteria.InboundID;

                    lSQLParams.Add(oSQLParam);
                }


                if (oCriteria.StoreRefNo != null)
                {
                    SqlParameter oSQLParam = new SqlParameter("@StoreRefNo", SqlDbType.NVarChar, 60);
                    oSQLParam.Value = oCriteria.StoreRefNo;

                    lSQLParams.Add(oSQLParam);
                }



                if (oCriteria.TransferRequestID > 0)
                {
                    SqlParameter oSQLParam = new SqlParameter("@TransferRequestID", SqlDbType.Int);
                    oSQLParam.Value = oCriteria.TransferRequestID;

                    lSQLParams.Add(oSQLParam);
                }


                if (oCriteria.UserID > 0)
                {
                    SqlParameter oSQLParam = new SqlParameter("@UpdatedBy", SqlDbType.Int);
                    oSQLParam.Value = oCriteria.UserID;

                    lSQLParams.Add(oSQLParam);
                }


                if (oCriteria.SuggestionID > 0)
                {
                    SqlParameter oSQLParam = new SqlParameter("@SuggestionID", SqlDbType.Int);
                    oSQLParam.Value = oCriteria.SuggestionID;

                    lSQLParams.Add(oSQLParam);
                }

                if (oCriteria.SuggestionFullfilledQuantity > 0)
                {
                    SqlParameter oSQLParam = new SqlParameter("@SuggestionQtyFulfilled", SqlDbType.Int);
                    oSQLParam.Value = oCriteria.SuggestionFullfilledQuantity;

                    lSQLParams.Add(oSQLParam);
                }

                if (oCriteria.ReasonID > 0)
                {
                    SqlParameter oSQLParam = new SqlParameter("@ReasonID", SqlDbType.Int);
                    oSQLParam.Value = oCriteria.ReasonID;

                    lSQLParams.Add(oSQLParam);
                }

                if (oCriteria.ContainerID > 0)
                {
                    SqlParameter oSQLParam = new SqlParameter("@PalletID", SqlDbType.Int);
                    oSQLParam.Value = oCriteria.ContainerID;

                    lSQLParams.Add(oSQLParam);
                }


                if (oCriteria.ContainerCode != null)
                {
                    SqlParameter oSQLParam = new SqlParameter("@PalletCode", SqlDbType.NVarChar, 60);
                    oSQLParam.Value = oCriteria.ContainerCode;

                    lSQLParams.Add(oSQLParam);
                }

                DataSet _dsSuggestions = FillDataSet(_sSQL, lSQLParams);

                if (_dsSuggestions != null)
                {
                    if (_dsSuggestions.Tables.Count > 0)
                    {
                        foreach (DataRow _drSuggestion in _dsSuggestions.Tables[1].Rows)
                        {
                            Suggestion _oSuggestion = new Suggestion()
                            {
                                LocationCode = _drSuggestion["DisplayLocationCode"].ToString(),
                                LocationID = ConversionUtility.ConvertToInt(_drSuggestion["SuggestedLocationID"].ToString()),
                                MaterialCode = _drSuggestion["MCode"].ToString(),
                                MaterialMasterID = ConversionUtility.ConvertToInt(_drSuggestion["MaterialMasterID"].ToString()),
                                Quantity = ConversionUtility.ConvertToDecimal(_drSuggestion["SuggestedQty"].ToString()),
                                SuggestedPutawayID= ConversionUtility.ConvertToInt(_drSuggestion["SuggestedPutawayID"].ToString())
                            };

                            lSuggestions.Add(_oSuggestion);
                        }
                    }                    
                }

                return lSuggestions;
            }
            catch (WMSExceptionMessage excp)
            {
                throw excp;
            }
            catch (Exception excp)
            {
                ExceptionData oExcpData = new ExceptionData();
                oExcpData.AddInputs("oCriteria", oCriteria);

                ExceptionHandling.LogException(excp, _ClassCode + "008", oExcpData);
                throw new WMSExceptionMessage() { WMSExceptionCode = ErrorMessages.WMSExceptionCode, WMSMessage = ErrorMessages.WMSExceptionMessage, ShowAsCriticalError = true };
            }
        }


        public bool MarkBinFull(Location oLocation)
        {
            try
            {
                string _sSqlQuery = "UPDATE Loc SET Loc.IsSkipedAsFull = 1 FROM INV_Location AS Loc WHERE Loc.DisplayLocationCode = '" + (oLocation.LocationCode == null ? oLocation.SystemLocationCode : oLocation.LocationCode) + "' OR Loc.Location = '" + (oLocation.LocationCode == null ? oLocation.SystemLocationCode : oLocation.LocationCode) + "'";

                int _Result = ExecuteNonQuery(_sSqlQuery);

                if (_Result > 0)
                    return true;
                else
                    return false;
                
            }
            catch (Exception excp)
            {
                ExceptionData oExcpData = new ExceptionData();
                oExcpData.AddInputs("oLocation", oLocation);

                ExceptionHandling.LogException(excp, _ClassCode + "009", oExcpData);
                throw new WMSExceptionMessage() { WMSExceptionCode = ErrorMessages.WMSExceptionCode, WMSMessage = ErrorMessages.WMSExceptionMessage, ShowAsCriticalError = true };
            }
        }


        public bool ModifyPurchaseDocumentForExcessQuantity(Inventory oInventory)
        {
            try
            {
                if (oInventory.ReferenceDocumentNumber == null)
                    throw new WMSExceptionMessage() { WMSMessage = "Shipment Document number cannot be empty.", ShowAsError = true };
                else if (oInventory.MaterialCode == null)
                    throw new WMSExceptionMessage() { WMSMessage = "Material Information cannot be empty.", ShowAsError = true };
                else
                {
                    string _sSQL = "EXEC [dbo].[sp_API_CreateDummyPOandMaptoInbound] ";

                    List<SqlParameter> lSQLParams = new List<SqlParameter>();

                    SqlParameter oParamSRN = new SqlParameter("@StoeRefNumber", SqlDbType.VarChar, 100);
                    oParamSRN.Value = oInventory.ReferenceDocumentNumber;
                    lSQLParams.Add(oParamSRN);

                    SqlParameter oParamMCode = new SqlParameter("@Mcode", SqlDbType.VarChar, 100);
                    oParamMCode.Value = oInventory.MaterialCode;
                    lSQLParams.Add(oParamMCode);

                    SqlParameter oParamCreatedBy = new SqlParameter("@CreatedBy", SqlDbType.Int);
                    oParamCreatedBy.Value = base.LoggedInUserID;
                    lSQLParams.Add(oParamCreatedBy);

                    SqlParameter oParamQty = new SqlParameter("@Quantity", SqlDbType.Decimal);
                    oParamQty.Value =1;
                    lSQLParams.Add(oParamQty);

                    int _Result = ExecuteNonQuery(_sSQL, lSQLParams);

                    return (_Result > 0);
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

                ExceptionHandling.LogException(excp, _ClassCode + "010", oExcpData);
                throw new WMSExceptionMessage() { WMSExceptionCode = ErrorMessages.WMSExceptionCode, WMSMessage = ErrorMessages.WMSExceptionMessage, ShowAsCriticalError = true };
            }
        }


        public List<Inventory> FetchSKUInformationByDockGoodsMovement(Inventory oInventory)
        {
            try
            {

                if (oInventory.RSN == null)
                    throw new WMSExceptionMessage() { WMSExceptionCode="EMC0001", WMSMessage = ErrorMessages.EMC0001, ShowAsCriticalError = true };
                else
                {
                    string _sSQL = "SELECT MaterialMasterID, MaterialTransactionID, TransactionDocID, POSODetailsID, SerialNumber, (SELECT CASE WHEN POType = 'EXCPO' THEN 1 ELSE 0 END FROM ORD_POHeader AS POH INNER JOIN ORD_POType AS POT ON POT.POTypeID = POH.POTypeID INNER JOIN ORD_PODetails AS POD ON POH.POHeaderID = POD.POHeaderID INNER JOIN ORD_SupplierInvoiceDetails AS INVD ON INVD.PODetailsID = POD.PODetailsID WHERE INVD.SupplierInvoiceDetailsID = POSODetailsID ) AS IsExcessPO, SUM(Quantity) AS Quantity FROM INV_DockGoodsMovement WHERE SerialNumber = '" + oInventory.RSN + "' GROUP BY MaterialMasterID, MaterialTransactionID, TransactionDocID, POSODetailsID, SerialNumber";


                    List<Inventory> lExcessInventory = new List<Inventory>();

                    DataSet _dsDGMData = base.FillDataSet(_sSQL);

                    if(_dsDGMData!=null && _dsDGMData.Tables.Count > 0) 
                    {
                        if (_dsDGMData.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow _drExcess in _dsDGMData.Tables[0].Rows)
                            {
                                Inventory _oExcInv = oInventory.DuplicateItem();

                                _oExcInv.PosoDetailsID = ConversionUtility.ConvertToInt(_drExcess["POSODetailsID"].ToString());
                                _oExcInv.MaterialTransactionID = ConversionUtility.ConvertToInt(_drExcess["MaterialTransactionID"].ToString());
                                _oExcInv.MaterialMasterID = ConversionUtility.ConvertToInt(_drExcess["MaterialMasterID"].ToString());
                                _oExcInv.ReferenceDocumentID = ConversionUtility.ConvertToInt(_drExcess["TransactionDocID"].ToString());
                                _oExcInv.IsExcessInventory = ConversionUtility.ConvertToBool(_drExcess["IsExcessPO"].ToString());
                                _oExcInv.RSN = _drExcess["SerialNumber"].ToString();

                                _oExcInv.Quantity = ConversionUtility.ConvertToDecimal(_drExcess["Quantity"].ToString());

                                lExcessInventory.Add(_oExcInv);
                            }
                        }
                    }

                    return lExcessInventory;
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
        public string PalletRules(Inventory inboundDTO)
        {
            List<Inbound> _lInbound = new List<Inbound>();



            StringBuilder _sbSQL = new StringBuilder();
            List<SqlParameter> lSQLParams = new List<SqlParameter>();

            _sbSQL.Append("EXEC  [dbo].[sp_API_GeneratePalletGateSuggestions]");

            if (inboundDTO.ContainerCode != null)
            {
                // _sbSQL.Append("@InboundID ='" + oCriteria.InboundID.ToString());

                SqlParameter oParam = new SqlParameter("@CartonCode", SqlDbType.NVarChar, 50);
                oParam.Value = inboundDTO.ContainerCode;

                lSQLParams.Add(oParam);
            }


            if (inboundDTO.ReferenceDocumentNumber != null)
            {
                // _sbSQL.Append("@InboundID ='" + oCriteria.InboundID.ToString());

                SqlParameter oParam = new SqlParameter("@StoreRefNo", SqlDbType.NVarChar, 50);
                oParam.Value = inboundDTO.ReferenceDocumentNumber;

                lSQLParams.Add(oParam);
            }




            if (inboundDTO.MaterialCode != null)
            {
                // _sbSQL.Append("@InboundID ='" + oCriteria.InboundID.ToString());

                SqlParameter oParam = new SqlParameter("@MaterialCode", SqlDbType.NVarChar, 50);
                oParam.Value = inboundDTO.MaterialCode;

                lSQLParams.Add(oParam);
            }


            if (inboundDTO.RSN != null)
            {
                SqlParameter oParam = new SqlParameter("@RSN", SqlDbType.NVarChar, 50);
                oParam.Value = inboundDTO.RSN;

                lSQLParams.Add(oParam);
            }



            if (inboundDTO.Quantity != 0)
            {
                SqlParameter oParam = new SqlParameter("@Quantity", SqlDbType.Decimal);
                oParam.Value = inboundDTO.Quantity;

                lSQLParams.Add(oParam);


            }

            DataSet _dsResults = FillDataSet(_sbSQL.ToString(), lSQLParams);

            return null;
        }


        public bool ReGeneratePalletizationZoneSuggestions(Inventory oInventory)
        {
            try
            {

                if (oInventory.RSN == null)
                    throw new WMSExceptionMessage() { WMSExceptionCode = "EMC0001", WMSMessage = ErrorMessages.EMC0001, ShowAsCriticalError = true };
                else
                {
                    string _sSQL = "EXEC [dbo].[sp_API_GeneratePalletGateSuggestions] ";


                    /*
                      
                        Procedure Parameters
                     
                        @CartonCode	 NVARCHAR(50) = NULL,
                        @RSN	NVARCHAR(50) = NULL, 
                        @MaterialCode	NVARCHAR(50) = NULL,
                        @StoreRefNo NVARCHAR(50) = NULL, 
                        @Quantity	DECIMAL(18, 3) = 0
                      
                     
                     */

                    List<SqlParameter> _lSQLParams = new List<SqlParameter>();

                    if (oInventory.ContainerCode == null)
                        throw new WMSExceptionMessage() { WMSExceptionCode = "EMC_IB_BL_013", WMSMessage = ErrorMessages.EMC_IB_BL_013, ShowAsError = true };
                    else if (oInventory.RSN == null)
                        throw new WMSExceptionMessage() { WMSExceptionCode = "EMC_IB_BL_015", WMSMessage = ErrorMessages.EMC_IB_BL_015, ShowAsError = true };
                    else if (oInventory.MaterialCode == null)
                        throw new WMSExceptionMessage() { WMSExceptionCode = "EMC_IB_BL_014", WMSMessage = ErrorMessages.EMC_IB_BL_014, ShowAsError = true };
                    else if (oInventory.ReferenceDocumentNumber == null)
                        throw new WMSExceptionMessage() { WMSExceptionCode = "EMC_IB_BL_016", WMSMessage = ErrorMessages.EMC_IB_BL_016, ShowAsError = true };
                    else if (oInventory.Quantity == 0)
                        throw new WMSExceptionMessage() { WMSExceptionCode = "EMC_IB_BL_017", WMSMessage = ErrorMessages.EMC_IB_BL_017, ShowAsError = true };


                    SqlParameter oParam = new SqlParameter("@CartonCode", SqlDbType.NVarChar, 50);
                    oParam.Value = oInventory.ContainerCode;

                    _lSQLParams.Add(oParam);

                    oParam = new SqlParameter("@RSN", SqlDbType.NVarChar, 50);
                    oParam.Value = oInventory.RSN;

                    _lSQLParams.Add(oParam);

                    oParam = new SqlParameter("@MaterialCode", SqlDbType.NVarChar, 50);
                    oParam.Value = oInventory.MaterialCode;

                    _lSQLParams.Add(oParam);

                    oParam = new SqlParameter("@StoreRefNo", SqlDbType.NVarChar, 50);
                    oParam.Value = oInventory.ReferenceDocumentNumber;

                    _lSQLParams.Add(oParam);

                    oParam = new SqlParameter("@Quantity", SqlDbType.NVarChar, 50);
                    oParam.Value = oInventory.Quantity;

                    _lSQLParams.Add(oParam);

                    
                    DataSet _dsPalletization = base.FillDataSet(_sSQL, _lSQLParams);

                    bool _bPermitPalletization = false;

                    if (_dsPalletization == null)
                        throw new Exception("Prcedure : [dbo].[sp_API_GeneratePalletGateSuggestions] did not return any DataSet.");
                    else
                    {
                        if (_dsPalletization.Tables[0].Rows.Count == 1 && _dsPalletization.Tables[0].Columns.Count == 2)
                            _bPermitPalletization = false;
                        else
                            _bPermitPalletization = true;
                    }


                    return _bPermitPalletization;
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


        public List<Suggestion> FetchPutawaySuggestions(SearchCriteria oCriteria, bool FetchNextItem)
        {
            try
            {

                //if (oCriteria.MaterialRSN == null)
                //    throw new WMSExceptionMessage() { WMSExceptionCode = "EMC0001", WMSMessage = ErrorMessages.EMC0001, ShowAsCriticalError = true };
                //else
                //{
                    string _sSQL = "EXEC [USP_API_FetchPutawaySuggestions] ";


                    /*
                      
                        Procedure Parameters
                     
                        @CartonCode	 NVARCHAR(50) = NULL,
                        @RSN	NVARCHAR(50) = NULL, 
                        @MaterialCode	NVARCHAR(50) = NULL,
                        @StoreRefNo NVARCHAR(50) = NULL, 
                        @Quantity	DECIMAL(18, 3) = 0
                      
                     
                     */

                    List<SqlParameter> _lSQLParams = new List<SqlParameter>();

                    if (oCriteria.ContainerCode == null)
                        throw new WMSExceptionMessage() { WMSExceptionCode = "EMC_IB_BL_013", WMSMessage = ErrorMessages.EMC_IB_BL_013, ShowAsError = true };

                    SqlParameter oParam = new SqlParameter("@PalletCode", SqlDbType.NVarChar, 50);
                    oParam.Value = oCriteria.ContainerCode;

                    _lSQLParams.Add(oParam);


                    if (oCriteria.LocationCode != null)
                    {
                        oParam = new SqlParameter("@CurrentLocation", SqlDbType.NVarChar, 50);
                        oParam.Value = oCriteria.LocationCode;

                        _lSQLParams.Add(oParam);
                    }


                    if (oCriteria.TransferRequestNo != null)
                    {
                        oParam = new SqlParameter("@JobRefNo", SqlDbType.NVarChar, 50);
                        oParam.Value = oCriteria.TransferRequestNo;

                        _lSQLParams.Add(oParam);
                    }

                    oParam = new SqlParameter("@FetchNextSuggestion", SqlDbType.Bit);
                    oParam.Value = FetchNextItem;

                    _lSQLParams.Add(oParam);

                    DataSet _dsSuggestions = base.FillDataSet(_sSQL, _lSQLParams);
                    List<Suggestion> _lSuggestions = new List<Suggestion>();

                    if (_dsSuggestions == null)
                        throw new Exception("Prcedure : [USP_API_FetchPutawaySuggestions] did not return any DataSet.");
                    else
                    {
                        if (_dsSuggestions.Tables[0].Rows.Count == 0)
                            throw new WMSExceptionMessage() { };
                        else
                        {
                            foreach (DataRow _drSuggestion in _dsSuggestions.Tables[0].Rows)
                            {
                                Suggestion oSugg = new Suggestion()
                                {
                                    LocationCode = _drSuggestion["DisplayLocationCode"].ToString(),
                                    LocationID = ConversionUtility.ConvertToInt(_drSuggestion["SuggestedLocationID"].ToString()),
                                    MaterialCode = _drSuggestion["MCode"].ToString(),
                                    MaterialMasterID = ConversionUtility.ConvertToInt(_drSuggestion["MCode"].ToString()),
                                    Quantity = ConversionUtility.ConvertToDecimal(_drSuggestion["SuggestedQty"].ToString())
                                };

                                _lSuggestions.Add(oSugg);
                            }
                        }
                    }


                    return _lSuggestions;
               // }
            }
            catch (WMSExceptionMessage excp)
            {
                throw excp;
            }
            catch (Exception excp)
            {
                ExceptionData oExcpData = new ExceptionData();
                oExcpData.AddInputs("oCriteria", oCriteria);
                oExcpData.AddInputs("FetchNextItem", FetchNextItem);

                ExceptionHandling.LogException(excp, _ClassCode + "012", oExcpData);
                throw new WMSExceptionMessage() { WMSExceptionCode = ErrorMessages.WMSExceptionCode, WMSMessage = ErrorMessages.WMSExceptionMessage, ShowAsCriticalError = true };
            }
        }
        
    }
}
