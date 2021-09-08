using MRLWMSC21Common;
using MRLWMSC21_Library.WMS_ServiceObjects;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MRLWMSC21_Library.WMS_DBCommon.MasterDataDAO
{
    
    internal class LocationDAO : DBServiceController
    {
        delegate object LocationDataAccess(object input, int MSG_TYPE);
        
        public object processDBCall(string DB_CALL_CODE, object input, int IN_MSG_TYPE)
        {
            try
            {
                
                if (DBServicePool.DBServiceImplementRef.ContainsKey(DB_CALL_CODE.Trim()) || DBServicePool.DBServiceImplementRef[DB_CALL_CODE].Trim() != "")
                {
                    MethodInfo methodInfo = typeof(LocationDAO).GetMethod(DBServicePool.DBServiceImplementRef[DB_CALL_CODE]);
                    LocationDataAccess dataAccess = (LocationDataAccess)Delegate.CreateDelegate(typeof(LocationDataAccess), null, methodInfo);
                    return dataAccess(input, IN_MSG_TYPE);
                }
                else
                {
                    throw new NotImplementedException("the requested DB execution call not implemented under InvoiceDAO. [" + DB_CALL_CODE + "]");
                }
            }
            catch (Exception ex)
            {
                throw new NotImplementedException("the requested DB Execution Call not implemented yet. [" + DB_CALL_CODE + "] " + ex.Message);
            }
        }



        public object GetLocationOccupancyById(object input, int MSG_TYPE)
        {
            try
            {
                if (input != null && MSG_TYPE == 2)
                {
                    return fetchLocationOccupancyById((IDictionary<string, object>)input);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw new NotImplementedException("Exception while fetching data from DB" + ex.Message);
            }
        }


        private LocationOccupancy fetchLocationOccupancyById(IDictionary<string, object> input)
        {
            if (input != null && input.Count > 0)
            {

                DataSet locationOccupancy = DB.GetDS("[dbo].[SP_SUGGESTED_PUTAWAY_LOCATIONS_INFO] @LOCATION_ZONE = '" + input["LOCATION_ZONE"] + "',@LOCATIONID=" + input["LOCATIONID"] + ",@MATERIALID =0,@TENANTID =0,@SUPPLIERID =0,@NEARBY_LOC =1,@MATERIAL_BIN_RESULT =0", false);
                if (locationOccupancy != null && locationOccupancy.Tables != null && locationOccupancy.Tables.Count > 0)
                {
                    return getLocationOccupancyByDataRow(locationOccupancy.Tables[0].Rows[0]);
                }

            }
            return null;
        }

        private LocationOccupancy getLocationOccupancyByDataRow(DataRow row)
        {
            LocationOccupancy locationOcc = null;
            if (row != null)
            {
                locationOcc = new LocationOccupancy();
                locationOcc.LocationCode = (row["Location"]).ToString();
                locationOcc.LocationID = Convert.ToInt32(row["LocationID"].ToString());
                locationOcc.MaxWeight = Convert.ToDecimal(row["MAX_WEIGHT"]);
                locationOcc.Volume = Convert.ToDecimal(row["VOLUME"]);
                locationOcc.VolumeOccupancy = Convert.ToDecimal(row["TOTAL_VOL"]);
                locationOcc.WeightOccupancy = Convert.ToDecimal(row["TOTAL_W"]);
                locationOcc.VolOccupiedPercentage = Convert.ToDecimal(row["VOL_PERCENT"]);
                locationOcc.WeightHoldPercentage = Convert.ToDecimal(row["WEIGHT_PERCENT"]);

            }
            return locationOcc;
        }


        private IList<LocationOccupancy> fetchNearByLocationsByCode(IDictionary<string, object> input)
        {
            if (input != null && input.Count > 0)
            {
                DataSet locations = DB.GetDS("[dbo].[SP_SUGGESTED_PUTAWAY_LOCATIONS_INFO] @LOCATION_ZONE = '" + input["LOCATION_ZONE"] + "',@LOCATIONID=0,@MATERIALID =0,@TENANTID =" + input["TENANTID"] + ",@SUPPLIERID =" + input["SUPPLIERID"] + ",@NEARBY_LOC =" + input["NEAR_BY_LOC"] + ",@MATERIAL_BIN_RESULT =0", false);
                if (locations != null && locations.Tables != null && locations.Tables.Count > 0)
                {
                    IList<LocationOccupancy> locationList = new List<LocationOccupancy>();
                    foreach (DataRow row in locations.Tables[0].Rows)
                    {
                        LocationOccupancy lo = getLocationOccupancyByDataRow(row);
                        if (lo != null)
                            locationList.Add(lo);
                    }
                    return locationList;
                }

            }
            return null;
        }


        public IList<LocationOccupancy> GetZoneWiseBinsByLocationCode(object input, int MSG_TYPE)
        {
            try
            {
                if (input != null && MSG_TYPE == 2)
                {
                    return fetchNearByLocationsByCode((IDictionary<string, object>)input);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw new NotImplementedException("Exception while fetching data from DB" + ex.Message);
            }
        }

        public IList<WarehouseZone> GetZoneInfoByWarehouseCode(object input, int MSG_TYPE)
        {
            IList<WarehouseZone> warehouseZones = null;
            if (input != null && MSG_TYPE == 1)
            {
                //getting data through query call
                string WarehouseCode = input.ToString();
                string drlStatement = "SELECT distinct LZ.LocationZoneCode,LZ.LocationZoneID,LZ.WarehouseID,W.WHCode,W.WarehouseGroupID,LZ.Description as ZoneDesc FROM INV_LocationZone LZ JOIN GEN_Warehouse W ON LZ.WarehouseID = W.WarehouseID JOIN GEN_WarehouseGroup WG on WG.WarehouseGroupID = W.WarehouseGroupID WHERE WG.WarehouseGroupCode LIKE '" + WarehouseCode.Trim() + "%' AND W.IsDeleted=0 AND LZ.IsDeleted=0 and WG.IsDeleted=0";
                //string drlStatement = "SELECT LZ.LocationZoneCode,LZ.LocationZoneID,LZ.WarehouseID,W.WHCode,W.WarehouseGroupID,LZ.Description as ZoneDesc FROM INV_LocationZone LZ JOIN GEN_Warehouse W ON LZ.WarehouseID = W.WarehouseID WHERE W.WHCode LIKE '" + WarehouseCode + "%' AND W.IsDeleted=0 AND LZ.IsDeleted=0 ";
                DataSet zoneSet = DB.GetDS(drlStatement,false);
                if(zoneSet != null && zoneSet.Tables.Count > 0)
                {
                    warehouseZones = new List<WarehouseZone>();
                    foreach(DataRow row in zoneSet.Tables[0].Rows)
                    {
                        WarehouseZone zone = new WarehouseZone();
                        zone.WarehouseCode = row["WHCode"].ToString();
                        zone.WarehouseGroupId = Convert.ToInt32(row["WarehouseGroupID"].ToString());
                        zone.WarehouseId = Convert.ToInt32(row["WarehouseID"].ToString());
                        zone.ZoneCode = (row["LocationZoneCode"].ToString());
                        zone.ZoneDescription = row["ZoneDesc"].ToString();
                        zone.ZoneId = Convert.ToInt16(row["LocationZoneID"].ToString());
                        warehouseZones.Add(zone);
                    }
                }
            }
            return warehouseZones;
        }

        public IList<LocationOccupancy> GetSuggetedLocationOccupancy(object input, int MSG_TYPE) 
        {
            if(input != null)
            {
                
                IDictionary<string,object> criteria = (IDictionary<string,object>)input;
                string drlStatement = "[dbo].[SP_SUGGESTED_LOCATIONS_OCCUPANCY_INFO] @INBOUNDID=" + criteria["INBOUNDID"] + ",@LOGICAL_OCCUPANCY=" + criteria["LOGICAL_OCCUPANCY"].ToString() + ",@TENANTID=" + criteria["TENANTID"].ToString() + ",@SUPPLIERID=" + criteria["SUPPLIERID"].ToString();
                                
                DataSet occupancySet = DB.GetDS(drlStatement,false);
                IList<LocationOccupancy> los = null;
                if (occupancySet != null)
                {
                    los = new List<LocationOccupancy>();
                    foreach(DataRow row in occupancySet.Tables[0].Rows)
                    {
                        LocationOccupancy lo = new LocationOccupancy();
                        lo.LocationID = Convert.ToInt16(row["LocationID"]);
                        lo.WeightOccupancy = Convert.ToDecimal(row["WeightOccupancy"]);
                        lo.VolumeOccupancy = Convert.ToDecimal(row["VolumeOccupancy"]);
                        los.Add(lo);
                    }
                }
                
              return los;
            }
            else{
                return null;
            }
        }
    }
}
