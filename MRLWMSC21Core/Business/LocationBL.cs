using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MRLWMSC21Core.Entities;
using MRLWMSC21Core.SSOService;
using MRLWMSC21Core.Library;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using MRLWMSC21Core.DataAccess;
using WMSCore_DataAccess.DataAccess.SqlServerImpl;

namespace MRLWMSC21Core.Business
{
    public class LocationBL : BaseBL, Interfaces.iLocationBL
    {
        private string _ClassCode = "WMSCore_BL_001_";
        private LocationDAL oLocationDAL = null;
        private SingleSignOnDBSinkClient _objServiceClient = null;

        public LocationBL(int LoginUser, string ConnectionString) : base(LoginUser, ConnectionString)
        {
            oLocationDAL = new LocationDAL(LoginUser, ConnectionString);
            _ClassCode = ExceptionHandling.GetClassExceptionCode(ExceptionHandling.ExcpConstants_API_BusinessLayer.LocationBL);
        }

        public List<Location> GetLoationListBind(SearchCriteria oCriteria)
        {
            try
            {
               
                List<Location> oInbound = oLocationDAL.GetLocations(oCriteria);
                return oInbound;

            }
            catch (Exception ex)
            {
                ExceptionHandling.LogException(ex, _ClassCode + "001");

                return null;
            }
        }

        public Location GetLocationByID(int LocationID)
        {

            try
            {
                Location oLocation = new Location();
                
                oLocation = oLocationDAL.GetLocationByID(LocationID);
                return oLocation;

            }
            catch (Exception ex)
            {
                ExceptionHandling.LogException(ex, _ClassCode + "002");

                return null;
            }

        }
        public bool BlockLocationForCycleCount(Location oLocation)
        {
            try
            {
                //Location oLocationget = new Location();
                
                bool oLocationget = oLocationDAL.BlockLocationForCycleCount(oLocation);
                return oLocationget;

            }
            catch (Exception ex)
            {
                ExceptionHandling.LogException(ex, _ClassCode + "003");

                return false;
            }
        }
        public bool ReleaseLocationForCycleCount(Location oLocation)
        {
            try
            {
                bool oLocationget = oLocationDAL.ReleaseLocationForCycleCount(oLocation);
                return oLocationget;

            }
            catch (Exception ex)
            {
                ExceptionHandling.LogException(ex, _ClassCode + "004");

                return false;
            }
        }
        public bool BlockLocationForCycleCount(int LocationID)
        {
            try
            {
                bool oLocationget = oLocationDAL.BlockLocationForCycleCount(LocationID);
                return oLocationget;

            }
            catch (Exception ex)
            {
                ExceptionHandling.LogException(ex, _ClassCode + "005");

                return false;
            }
        }
        public bool ReleaseLocationForCycleCount(int LocationID)
        {
            try
            {
                bool oLocationget = oLocationDAL.ReleaseLocationForCycleCount(LocationID);
                return oLocationget;

            }
            catch (Exception ex)
            {
                ExceptionHandling.LogException(ex, _ClassCode + "006");

                return false;
            }
        }
    }
}
