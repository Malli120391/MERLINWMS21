using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MRLWMSC21Core.Entities;
using MRLWMSC21Core.Library;
using MRLWMSC21Core.DataAccess;

namespace MRLWMSC21Core.Business
{
    public class MasterBL : BaseBL
    {
        private string _ClassCode = "WMSCore_BL_003_";
        private MasterDAL _oMasterDAL = null;

        public MasterBL(int LoginUser, string ConnectionString) : base(LoginUser, ConnectionString)
        {
            _oMasterDAL = new MasterDAL(LoginUser, ConnectionString);
            _ClassCode = ExceptionHandling.GetClassExceptionCode(ExceptionHandling.ExcpConstants_API_BusinessLayer.MasterBL);
        }

        public List<Colour> FetchColourMaster()
        {


            try
            {
                List<Colour> oColourMaster = null;
                
                oColourMaster = _oMasterDAL.FetchColoursMaster(new SearchCriteria());

                return oColourMaster;
            }
            catch (Exception excp)
            {
                ExceptionData oExcpData = null;
                ExceptionHandling.LogException(excp, _ClassCode + "001", oExcpData);
                throw new WMSExceptionMessage() { WMSExceptionCode = ErrorMessages.WMSExceptionCode, WMSMessage = ErrorMessages.WMSExceptionMessage };
            }
        }

        public List<Inventory> FetchColoursForInventory(List<Inventory> oInventory)
        {

            try
            {
                oInventory = _oMasterDAL.FetchColoursForInventory(oInventory);

                return oInventory;
            }
            catch (Exception excp)
            {
                ExceptionData oExcpData = new ExceptionData();

                oExcpData.AddInputs("oInventory", oInventory);

                ExceptionHandling.LogException(excp, _ClassCode + "002", oExcpData);
                throw new WMSExceptionMessage() { WMSExceptionCode = ErrorMessages.WMSExceptionCode, WMSMessage = ErrorMessages.WMSExceptionMessage };
            }
        }

        /// <summary>
        /// Returns a List of Storage Locations based on the Criteria.
        /// </summary>
        /// <param name="oCriteria">WarehouseID of the warehouse.</param>
        /// <returns>List of Storage Locations</returns>
        public List<StorageLocations> FetchStorageLocations()
        {
            try
            {
                List<StorageLocations> oSLOC = null;

                oSLOC = _oMasterDAL.FetchStorageLocations();

                return oSLOC;

            }
            catch (Exception excp)
            {
                

                ExceptionHandling.LogException(excp, _ClassCode + "003", null);
                throw new WMSExceptionMessage() { WMSExceptionCode = ErrorMessages.WMSExceptionCode, WMSMessage = ErrorMessages.WMSExceptionMessage };
            }
            
        }


        public PutawayPreferences FetchPutawaySuggestionsForAccount(SearchCriteria oCriteria)
        {
            try
            {
                return _oMasterDAL.FetchPutawaySuggestionsForAccount(oCriteria);
            }
            catch(Exception ex)
            {
                return null;
            }
        }



    }
}
