using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MRLWMSC21Core.Entities;
using System.Data;
using System.Data.SqlClient;
using MRLWMSC21Core.Library;

namespace MRLWMSC21Core.DataAccess
{
    public class MasterDAL : BaseDAL
    {   

        private string _ClassCode = "WMSCore_DA_008_";
     
        public MasterDAL(int LoginUser, string ConnectionString) : base(LoginUser, ConnectionString)
        {

            _ClassCode = ExceptionHandling.GetClassExceptionCode(ExceptionHandling.ExcpConstants_API_DataLayer.MasterDAL);
        }

        public List<Colour> FetchColoursMaster(SearchCriteria oCriteria)
        {

            try
            {

                // Method block to Fetch the Entity List from Database.

                List<Colour> oColours = new List<Colour>();


                StringBuilder _sbSQL = new StringBuilder();
                List<SqlParameter> lSQLParams = new List<SqlParameter>();

                _sbSQL.Append("EXEC [dbo].[SP_API_GetSKUColors_BySearchParams] ");

                if (oCriteria.MaterialCode != null)
                {
                    // _sbSQL.Append("@InboundID ='" + oCriteria.InboundID.ToString());

                    SqlParameter oParam = new SqlParameter("@MaterialID", SqlDbType.NVarChar, 100)
                    {
                        Value = oCriteria.MaterialCode
                    };

                    lSQLParams.Add(oParam);
                }

                DataSet _dsResults = base.FillDataSet(_sbSQL.ToString(), lSQLParams);

                if (_dsResults != null)
                    if (_dsResults.Tables.Count > 0)
                    {

                        foreach (DataRow _drColor in _dsResults.Tables[0].Rows)
                        {
                            Colour _oColour = new Colour()
                            {
                                ColourCode = _drColor["Colorcode"].ToString(),
                                ColourID = ConversionUtility.ConvertToInt(_drColor["ColorID"].ToString()),
                                ColourName = _drColor["Colorcode"].ToString()
                            };

                            oColours.Add(_oColour);
                        }
                    }

                return oColours;

            }
            catch (Exception ex)
            {
                ExceptionHandling.LogException(ex, _ClassCode + "001");

                return null;
            }

        }

        public List<Inventory> FetchColoursForInventory(List<Inventory> oInventory)
        {

            try
            {

                // Method block to Fetch the Entity List from Database.

                List<Colour> oColours = new List<Colour>();

                StringBuilder _sbMaterialList = new StringBuilder();

                foreach (Inventory _oInv in oInventory)
                {
                    _sbMaterialList.Append("," + _oInv.MaterialMasterID);
                }

                StringBuilder _sbSQL = new StringBuilder();
                List<SqlParameter> lSQLParams = new List<SqlParameter>();

                _sbSQL.Append("EXEC [dbo].[SP_API_GetSKUColors_BySearchParams] ");

                if (_sbMaterialList.ToString().Length > 0)
                {
                    // _sbSQL.Append("@InboundID ='" + oCriteria.InboundID.ToString());

                    SqlParameter oParam = new SqlParameter("@MaterialID", SqlDbType.NVarChar, 100)
                    {
                        Value = _sbMaterialList.ToString()
                    };

                    lSQLParams.Add(oParam);
                }

                DataSet _dsResults = base.FillDataSet(_sbSQL.ToString(), lSQLParams);
                
                if (_dsResults != null)
                    if (_dsResults.Tables.Count > 0)
                    {
                        foreach (Inventory _oInv in oInventory)
                        {
                            _oInv.ColourMasterList = new List<Colour>();

                            _dsResults.Tables[0].DefaultView.RowFilter = "MaterialMasterID=" + _oInv.MaterialMasterID.ToString();

                            foreach (DataRow _drColor in _dsResults.Tables[0].DefaultView.ToTable().Rows)
                            {
                                Colour _oColour = new Colour()
                                {
                                    ColourCode = _drColor["ColorCode"].ToString(),
                                    ColourID = ConversionUtility.ConvertToInt(_drColor["ColorID"].ToString()),
                                    ColourName = _drColor["ColorCode"].ToString()
                                };

                                _oInv.ColourMasterList.Add(_oColour);
                            }
                        }
                    }

                return oInventory;

            }
            catch (Exception ex)
            {
                ExceptionHandling.LogException(ex, _ClassCode + "002");

                return null;
            }
        }



        public List<StorageLocations> FetchStorageLocations()
        {

            try
            {

                // Method block to Fetch the Entity List from Database.
                List<StorageLocations> oStorageLocations = null;

                StringBuilder _sbSQL = new StringBuilder();
              
                _sbSQL.Append("SELECT * FROM StorageLocation WHERE ISActive = 1 and ISDeleted = 0");

                DataSet _dsResults = base.FillDataSet(_sbSQL.ToString(), null);
                
                if (_dsResults != null)
                    if (_dsResults.Tables.Count > 0)
                    {
                        oStorageLocations = new List<StorageLocations>();

                        foreach (DataRow _drSLoc in _dsResults.Tables[0].Rows)
                        {
                            StorageLocations _SLOC = new StorageLocations()
                            {
                                SLocName = _drSLoc["Description"].ToString(),
                                SLocCode = _drSLoc["Code"].ToString(),
                                SLocID = ConversionUtility.ConvertToInt(_drSLoc["Id"].ToString()),
                                IsDefault = _drSLoc["Code"].ToString().ToUpper().Equals("STSR") ? true : false
                            };


                            if (_drSLoc["Description"].ToString().Contains("Lost"))
                                _SLOC.IsLostAndFound = true;
                            else _SLOC.IsLostAndFound = false;
                            oStorageLocations.Add(_SLOC);
                        }
                       
                    }

                return oStorageLocations;

            }
            catch (Exception ex)
            {
                ExceptionHandling.LogException(ex, _ClassCode + "003");

                return null;
            }
        }


        public List<Material> FetchMaterialList()
        {
            try
            {

                List<Material> lMaterial = new List<Material>();

                DataSet _dsMaterial = FetchMaterialDataSet(new SearchCriteria());
                if (_dsMaterial != null && _dsMaterial.Tables.Count > 0)
                    foreach (DataRow _dr in _dsMaterial.Tables[0].Rows)
                    {
                        lMaterial.Add(BindMaterialEntity(_dsMaterial.Tables[0].Rows[0]));
                    }

                return lMaterial;
            }

            catch (Exception ex)
            {
                ExceptionHandling.LogException(ex, _ClassCode + "004");

                return null;
            }
        }



        public Material FetchMaterial(SearchCriteria oCriteria)
        {

            try
            {
                if (!(oCriteria.MaterialMasterID > 0 || oCriteria.MaterialCode != null))
                    throw new WMSExceptionMessage() { };

                Material oMaterial = null;

                DataSet _dsMaterial = FetchMaterialDataSet(oCriteria);
                if (_dsMaterial != null && _dsMaterial.Tables.Count > 0)
                    if (_dsMaterial.Tables[0].Rows.Count > 0)
                        oMaterial = (BindMaterialEntity(_dsMaterial.Tables[0].Rows[0]));


                return oMaterial;


            }

            catch (Exception ex)
            {
                ExceptionHandling.LogException(ex, _ClassCode + "005");

                return null;
            }
        }
        
        private Material BindMaterialEntity(DataRow _drMaterial)
        {
                Material oMaterial = new Material()
                {
                    MaterialMasterID=ConversionUtility.ConvertToInt(_drMaterial["MaterialMasterID"].ToString()), 
                    GenericSKUID = ConversionUtility.ConvertToInt(_drMaterial["SKUGenericCodeID"].ToString()),

                    CategoryID = ConversionUtility.ConvertToInt(_drMaterial["ProductCategoryID"].ToString()),
                    ColorID = ConversionUtility.ConvertToInt(_drMaterial["ColourID"].ToString()),
                    DivisionID = ConversionUtility.ConvertToInt(_drMaterial["DivisionID"].ToString()),
                    PageFormatID = ConversionUtility.ConvertToInt(_drMaterial["PageFormatID"].ToString()),
                    MOP = ConversionUtility.ConvertToInt(_drMaterial["MOP"].ToString()),

                    Category = _drMaterial["ProductCategory"].ToString(), 
                    ColorCode = _drMaterial["Colour"].ToString(),
                    Division = _drMaterial["Division"].ToString(),
                    MCode = _drMaterial["MCode"].ToString(),
                    GenericSKUCode = _drMaterial["SKUGenericCode"].ToString(),
                    PageFormat = _drMaterial["PageFormat"].ToString(),
                    MaterialShortDescription = _drMaterial["MDescription"].ToString(),

                    MRP = ConversionUtility.ConvertToDecimal(_drMaterial["MRP"].ToString()), 
                    IsParent = ConversionUtility.ConvertToBool(_drMaterial["IsParent"].ToString()),
                    IsConsumables= ConversionUtility.ConvertToBool(_drMaterial["IsConsummables"].ToString()),
                    IsFinishedGoods= ConversionUtility.ConvertToBool(_drMaterial["IsFG"].ToString()),
                    IsRawMaterial= ConversionUtility.ConvertToBool(_drMaterial["IsRM"].ToString())


                };

                return oMaterial;
            
        }

        private DataSet FetchMaterialDataSet(SearchCriteria oCriteria)
        {
            DataSet _dsResults = null;

            string _sSQL = "EXEC [dbo].[sp_API_FetchMaterialMasterInformation] ";

            List<SqlParameter> lParams = new List<SqlParameter>();

            if (oCriteria.MaterialCode != null)
            {
                SqlParameter oMCode = new SqlParameter("@MCode", SqlDbType.NVarChar, 50);
                oMCode.Value = oCriteria.MaterialCode;

                lParams.Add(oMCode);
            }

            if (oCriteria.MaterialMasterID > 0)
            {
                SqlParameter oMCode = new SqlParameter("@MaterialMasterID", SqlDbType.Int);
                oMCode.Value = oCriteria.MaterialMasterID;

                lParams.Add(oMCode);
            }

            _dsResults = FillDataSet(_sSQL, lParams);

            return _dsResults;
        }

        public PutawayPreferences FetchPutawaySuggestionsForAccount(SearchCriteria oCriteria)
        {
            try
            {
                PutawayPreferences oPref = new PutawayPreferences();

                string _sSQL = "SELECT ISNULL((SELECT Value FROM GEN_TRN_Preferences WHERE GEN_MST_PreferenceOption_ID = 5), 0)";

                oPref.StrictComplianceToSuggestions = ConversionUtility.ConvertToBool(ExecuteScalar(_sSQL));

                return oPref;
            }
            catch(WMSExceptionMessage excp)
            { throw excp; }
            catch(Exception excp)
            {
                ExceptionData oExcpData = new ExceptionData();
                oExcpData.AddInputs("oCriteria", oCriteria);

                ExceptionHandling.LogException(excp, _ClassCode + "009", oExcpData);
                throw new WMSExceptionMessage() { WMSExceptionCode = ErrorMessages.WMSExceptionCode, WMSMessage = ErrorMessages.WMSExceptionMessage, ShowAsCriticalError = true };
            }

        }

    }
}
