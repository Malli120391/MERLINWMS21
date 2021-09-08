using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MRLWMSC21Core.DataAccess;
using MRLWMSC21Core.Entities;
using MRLWMSC21Core.Library;

namespace MRLWMSC21Core.Business
{
    public class InventoryBL : BaseBL
    {

        private string _ClassCode = "WMSCore_BL_006_";

        private InventoryDAL _oInventoryDAL = null;

        public InventoryBL(int LoginUser, string ConnectionString) : base(LoginUser, ConnectionString)
        {
            _oInventoryDAL = new InventoryDAL(LoginUser, ConnectionString);
            _ClassCode = ExceptionHandling.GetClassExceptionCode(ExceptionHandling.ExcpConstants_API_BusinessLayer.InventoryBL);
        }

        public Inventory TransferPallettoLocation(Inventory obj)
        {

            try
            {
                return _oInventoryDAL.TransferPallettoLocation(obj);
            }
            catch (WMSExceptionMessage excp)
            {
                throw excp;
            }
            catch (Exception excp)
            {
                ExceptionData oExcpData = new ExceptionData();
                oExcpData.AddInputs("obj", obj);

                ExceptionHandling.LogException(excp, _ClassCode + "001", oExcpData);
                throw new WMSExceptionMessage() { WMSExceptionCode = ErrorMessages.WMSExceptionCode, WMSMessage = ErrorMessages.WMSExceptionMessage, ShowAsCriticalError = true };
            }

        }

        public List<Inventory> GetSlocWiseActiveStock(Inventory inventory)
        {
            try
            {
                List<Inventory> inventories = new List<Inventory>();

                inventories = _oInventoryDAL.GetSlocWiseActiveStock(inventory);

                return inventories;
            }
            catch (WMSExceptionMessage excp)
            {
                throw excp;
            }
            catch (Exception excp)
            {
                ExceptionData oExcpData = new ExceptionData();
                oExcpData.AddInputs("obj", inventory);

                ExceptionHandling.LogException(excp, _ClassCode + "002", oExcpData);
                throw new WMSExceptionMessage() { WMSExceptionCode = ErrorMessages.WMSExceptionCode, WMSMessage = ErrorMessages.WMSExceptionMessage, ShowAsCriticalError = true };
            }
        }

        public List<Inventory> UpdateMaterialTrasnfer(Inventory inventory)
        {
            try
            {
                List<Inventory> inventories = new List<Inventory>();

                inventories = _oInventoryDAL.UpdateMaterialTrasnfer(inventory);

                return inventories;
            }
            catch (WMSExceptionMessage excp)
            {
                throw excp;
            }
            catch (Exception excp)
            {
                ExceptionData oExcpData = new ExceptionData();
                oExcpData.AddInputs("obj", inventory);

                ExceptionHandling.LogException(excp, _ClassCode + "003", oExcpData);
                throw new WMSExceptionMessage() { WMSExceptionCode = ErrorMessages.WMSExceptionCode, WMSMessage = ErrorMessages.WMSExceptionMessage, ShowAsCriticalError = true };
            }
        }



        ~InventoryBL()
        {
            _oInventoryDAL = null;
        }
    }
}
