using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MRLWMSC21Core.Library;
using MRLWMSC21Core.Entities;
using System.Data.SqlClient;
using System.Data;

namespace MRLWMSC21Core.DataAccess
{
    public class LocationDAL : BaseDAL, Interfaces.ILocationDAL
    {
        private string _ClassCode = "WMSCore_LDA_001_";

        public LocationDAL(int LoginUser, string ConnectionString) : base(LoginUser, ConnectionString)
        {
            _ClassCode = ExceptionHandling.GetClassExceptionCode(ExceptionHandling.ExcpConstants_API_DataLayer.LocationDAL);

        }

        //Developed & Tested
        public List<Location> GetLocations(SearchCriteria Criteria)
        {
            try
            {
                //Method block to Fetch the Entity List from Database.
                List<Location> _lLocation = new List<Location>();

                StringBuilder _sbSQL = new StringBuilder();

                List<SqlParameter> lSQLParams = new List<SqlParameter>();

                _sbSQL.Append("EXEC [dbo].[sp_API_INV_FetchLocationList]");

                if(Criteria.LocationID > 0)
                {
                    SqlParameter oParam = new SqlParameter("@LocationID", SqlDbType.Int);
                    oParam.Value = Criteria.LocationID;

                    lSQLParams.Add(oParam);
                }

                if (Criteria.LocationCode != null)
                {
                    SqlParameter oParam = new SqlParameter("@LocationCode", SqlDbType.NVarChar, 10);
                    oParam.Value = Criteria.LocationCode;

                    lSQLParams.Add(oParam);
                }

                if (Criteria.WarehouseID > 0)
                {
                    SqlParameter oParam = new SqlParameter("@WarehouseID", SqlDbType.Int);
                    oParam.Value = Criteria.WarehouseID;

                    lSQLParams.Add(oParam);
                }

                if (Criteria.ZoneID > 0)
                {
                    SqlParameter oParam = new SqlParameter("@ZoneID", SqlDbType.Int);
                    oParam.Value = Criteria.ZoneID;

                    lSQLParams.Add(oParam);
                }

                if (Criteria.CategoryID > 0)
                {
                    SqlParameter oParam = new SqlParameter("@CategoryID", SqlDbType.Int);
                    oParam.Value = Criteria.CategoryID;

                    lSQLParams.Add(oParam);
                }

                if (Criteria.DivisionID > 0)
                {
                    SqlParameter oParam = new SqlParameter("@DivisionID", SqlDbType.Int);
                    oParam.Value = Criteria.DivisionID;

                    lSQLParams.Add(oParam);
                }

                if (Criteria.PageFormatID > 0)
                {
                    SqlParameter oParam = new SqlParameter("@PageFormatID", SqlDbType.Int);
                    oParam.Value = Criteria.PageFormatID;

                    lSQLParams.Add(oParam);
                }

                if (Criteria.IsMixedMaterialAllowed != false)
                {
                    SqlParameter oParam = new SqlParameter("@IsMixedMaterialAllowed", SqlDbType.Bit);
                    oParam.Value = Criteria.IsMixedMaterialAllowed;

                    lSQLParams.Add(oParam);
                }

                if (Criteria.IsExcessPickedStagingLocation != false)
                {
                    SqlParameter oParam = new SqlParameter("@IsExcessInventoryStaging", SqlDbType.Bit);
                    oParam.Value = Criteria.IsExcessPickedStagingLocation;

                    lSQLParams.Add(oParam);
                }

                SqlParameter oParamIgnoreZoning = new SqlParameter("@IgnoreZoning", SqlDbType.Bit);
                oParamIgnoreZoning.Value = 1;
                lSQLParams.Add(oParamIgnoreZoning);

                DataSet _dsResults = base.FillDataSet(_sbSQL.ToString(), lSQLParams);

                if (_dsResults != null)

                    if (_dsResults.Tables.Count > 0)
                    {
                        foreach (DataRow _drLocation in _dsResults.Tables[0].Rows)
                        {
                            Location _TempLocation;
                            _TempLocation = BindLocationData(_drLocation);

                            _lLocation.Add(_TempLocation);

                        }
                    }

                return _lLocation;

            }
            catch (Exception ex)
            {
                ExceptionHandling.LogException(ex, _ClassCode + "001");

                return null;
            }
        }

        //Developed & Tested
        public Location GetLocationByID(int LocationID)
        {
            try
            {
                //Method block to Fetch the Entity List from Database.

                List<Location> oLocationget = null;

                SearchCriteria oSearch = new SearchCriteria()
                {
                    LocationID=LocationID
                };

                oLocationget = GetLocations(oSearch);

                return oLocationget[0];


            }
            catch (Exception ex)
            {
                ExceptionHandling.LogException(ex, _ClassCode + "002");

                return null;
            }
        }

        //Developed & Tested
        public bool BlockLocationForCycleCount(Location oLocation)
        {
            try
            {
                //Method block to Fetch the Entity List from Database.

                Location _lLocation = null;

                string _sSQL = "DECLARE @RETURNFLAG BIT EXEC [dbo].[sp_API_BlockLocationForCycelCount] @IsBlockedForCycleCount ='" + oLocation.IsBlockedForCycleCount.ToString() + "',	@UpdatedBy =1,@LocationID='" + oLocation.LocationID.ToString() + "', @Flag = @RETURNFLAG OUT SELECT @RETURNFLAG AS IsBlock";

                DataSet _dsResults = base.FillDataSet(_sSQL);

                _lLocation = BindBlockReleaseData(_dsResults.Tables[0].Rows[0]);

                return _lLocation.IsBlockedForCycleCount;



                // Location _lLocation =null;

                // StringBuilder _sbSQL = new StringBuilder();

                // List<SqlParameter> lSQLParams = new List<SqlParameter>();

                // _sbSQL.Append("DECLARE @RETURNFLAG BIT EXEC [dbo].[sp_API_BlockLocationForCycelCount] @Flag = @RETURNFLAG OUT SELECT @RETURNFLAG AS IsBlock");

                // if (oLocation.LocationID > 0)
                // {
                //     SqlParameter oParam = new SqlParameter("@LocationID", SqlDbType.Int);
                //     oParam.Value = oLocation.LocationID;

                //     lSQLParams.Add(oParam);
                // }

                // if (oLocation.IsBlockedForCycleCount != false)
                // {
                //     SqlParameter oParam = new SqlParameter("@IsBlockedForCycleCount", SqlDbType.Bit);
                //     oParam.Value = oLocation.LocationCode;

                //     lSQLParams.Add(oParam);

                //     SqlParameter oParam1 = new SqlParameter("@UpdatedBy ", SqlDbType.Bit);
                //     oParam1.Value = 1;

                //     lSQLParams.Add(oParam1);
                // }

                //DataSet _dsResults = base.FillDataSet(_sbSQL.ToString(), lSQLParams);

                // _lLocation = BindBlockReleaseData(_dsResults.Tables[0].Rows[0]);

                // return _lLocation.IsBlockedForCycleCount;

            }
            catch (Exception ex)
            {
                ExceptionHandling.LogException(ex, _ClassCode + "003");

                return false;
            }
        }
         
        //Developed & Tested
        public bool ReleaseLocationForCycleCount(Location oLocation)
        {
            try
            {
                //Method block to Fetch the Entity List from Database.
                
                bool oLocationget = BlockLocationForCycleCount(oLocation);

                return oLocationget;

            }
            catch (Exception ex)
            {
                ExceptionHandling.LogException(ex, _ClassCode + "004");

                return false;
            }
        }

        //Developed & Tested
        public bool BlockLocationForCycleCount(int LocationID)
        {
            try
            {
                //Method block to Fetch the Entity List from Database.

                Location oLocation = new Location()
                {
                    LocationID = LocationID
                };

                bool oLocationget = BlockLocationForCycleCount(oLocation);

                return oLocationget;

               

            }
            catch (Exception ex)
            {
                ExceptionHandling.LogException(ex, _ClassCode + "005");

                return false;
            }
        }

        //Developed & Tested
        public bool ReleaseLocationForCycleCount(int LocationID)
        {
            try
            {
                //Method block to Fetch the Entity List from Database.

                Location _lLocation = new Location()
                {
                    LocationID=LocationID
                };

                bool oLocationget = BlockLocationForCycleCount(_lLocation);

                return oLocationget;

            }
            catch (Exception ex)
            {
                ExceptionHandling.LogException(ex, _ClassCode + "006");

                return false;
            }
        }

        //Developed
        private Location BindLocationData(DataRow _drLocation)
        {
            
            Location _oLocation = new Location()
            {
                LocationID = ConversionUtility.ConvertToInt(_drLocation["LocationID"].ToString()),
                WarehouseID = ConversionUtility.ConvertToInt(_drLocation["WarehouseID"].ToString()),
                ZoneID = ConversionUtility.ConvertToInt(_drLocation["LocationZoneID"].ToString()),
                RackID = ConversionUtility.ConvertToInt(_drLocation["LocationRack"].ToString()),
                ColumnID = ConversionUtility.ConvertToInt(_drLocation["LocationColumnID"].ToString()),
                //LevelID = ConversionUtility.ConvertToInt(_drLocation["LocationLevelID"].ToString()),
                //PhaseID = ConversionUtility.ConvertToInt(_drLocation["PhaseID"].ToString()),

                LocationCode = _drLocation["Location"].ToString(),
                RackCode = _drLocation["LocationRack"].ToString(),
                ColumnCode = _drLocation["LocationColumn"].ToString(),
                LevelCode = _drLocation["LocationLevel"].ToString(),
                PhaseCode = _drLocation["PhaseCode"].ToString(),
                ZoneCode = _drLocation["LocationZoneCode"].ToString(),
                SystemLocationCode = _drLocation["DisplayLocationCode"].ToString(),
                
                Length = ConversionUtility.ConvertToDecimal(_drLocation["Length"].ToString()),
                Width = ConversionUtility.ConvertToDecimal(_drLocation["Width"].ToString()),
                Height = ConversionUtility.ConvertToDecimal(_drLocation["Height"].ToString()),
                MaxWeight = ConversionUtility.ConvertToDecimal(_drLocation["MaxWeight"].ToString()),


                //Bool Convertion Library
                IsFastMoving = ConversionUtility.ConvertToBool(_drLocation["IsFastMoving"].ToString()),
                IsMixedMaterialAllowed = ConversionUtility.ConvertToBool(_drLocation["IsMixedMaterialAllowed"].ToString()),
                IsQuarantine = ConversionUtility.ConvertToBool(_drLocation["IsQuarantine"].ToString()),
                IsDockingArea = ConversionUtility.ConvertToBool(_drLocation["IsDockingArea"].ToString()),
                IsBinLocation = !ConversionUtility.ConvertToBool(_drLocation["IsDockingArea"].ToString()),
                IsCrossDockingLocation = ConversionUtility.ConvertToBool(_drLocation["IsCrossDockingLocation"].ToString()),
                IsExcessPickedStaging = ConversionUtility.ConvertToBool(_drLocation["IsExcessPickedStaging"].ToString()),
                IsBlockedForCycleCount = ConversionUtility.ConvertToBool(_drLocation["IsBlockedForCycleCount"].ToString()),
                IsFlaggedForCC = ConversionUtility.ConvertToBool(_drLocation["IsSkippedAsFull"].ToString())

               

            };
               
            return _oLocation;
        }

        //Developed
        private Location BindBlockReleaseData(DataRow _drLocation)
        {

            Location _oLocation = new Location()
            {
                IsBlockedForCycleCount = ConversionUtility.ConvertToBool(_drLocation["IsBlock"].ToString()),
               
            };

            return _oLocation;
        }

    }
}
