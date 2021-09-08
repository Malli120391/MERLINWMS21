using MRLWMSC21Core.DataAccess;
using MRLWMSC21Core.Entities;
using MRLWMSC21Core.Library;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace MRLWMSC21Core.Business
{
    public class ShipperIDIntergrationBL : BaseBL, Interfaces.iShipperIDIntegrationBL
    {
        private string _ClassCode = string.Empty;

        private ShipperIDIntegrationDAL _oIntegrationDAL = null;
        public ShipperIDIntergrationBL(int LoginUser, string ConnectionString) : base(LoginUser, ConnectionString)
        {
            _oIntegrationDAL = new ShipperIDIntegrationDAL(LoginUser, ConnectionString);
            _ClassCode = ExceptionHandling.GetClassExceptionCode(ExceptionHandling.ExcpConstants_API_BusinessLayer.IntegrationBL);
        }

        public bool CheckAUthToken(string AuthToken)
        {
            try
            {
                bool result = _oIntegrationDAL.CheckAuthToken(AuthToken);
              

                return result;
            }
            catch (WMSExceptionMessage excp)
            {
                throw excp;
            }
            catch (Exception excp)
            {
                ExceptionData oExcpData = new ExceptionData();
                oExcpData.AddInputs("AuthToken", AuthToken);

                ExceptionHandling.LogException(excp, _ClassCode + "001", oExcpData);
                throw new WMSExceptionMessage() { WMSExceptionCode = ErrorMessages.WMSExceptionCode, WMSMessage = ErrorMessages.WMSExceptionMessage, ShowAsCriticalError = true };
            }
        }

        public List<APIInventory> GetWarehouseInventoryData(APIInventory inventory,string authkey)
        {
            try
            {
                List<APIInventory> inventories = new List<APIInventory>();

                inventories = _oIntegrationDAL.FetchWarehousewiseCurrentstock(inventory, authkey);

                return inventories;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public string GetTenatData(string StartNum, string EndNum,string authkey)
        {
            try
            {
               

               string inventories = _oIntegrationDAL.GetTenantDataDB(StartNum, EndNum,authkey);
                return inventories;


            }
            catch (Exception)
            {

                throw;
            }
        }


        public string FetchOutBoundByOBD(string authkey,int OutBoundID)
        {
            try
            {


                string inventories = _oIntegrationDAL.FetchOutBoundByOBDDB(authkey, OutBoundID);
                return inventories;


            }
            catch (Exception)
            {

                throw;
            }
        }



        public string RevertParticularInbounds(string authkey, int InboundID)
        {
            try
            {


                string inventories = _oIntegrationDAL.RevertParticularInboundsDB(authkey, InboundID);
                return inventories;


            }
            catch (Exception)
            {

                throw;
            }
        }


        public string RevertOutbounds(string authkey, int OutBound)
        {
            try
            {


                string inventories = _oIntegrationDAL.RevertOutboundsDB(authkey, OutBound);
                return inventories;


            }
            catch (Exception)
            {

                throw;
            }
        }



        public string GetReleaseOBD(string StartDate, string EndDate, string WarehouseCode, string PaginationID, string PageSize,  string authkey)
        {
            try
            {


                string inventories = _oIntegrationDAL.GetReleaseOBDDB(StartDate, EndDate, WarehouseCode, PaginationID, PageSize, authkey);
                return inventories;


            }
            catch (Exception)
            {

                throw;
            }
        }


        public string GetSupplierList(string StartNum, string EndNum,string UpdatedDate, string authkey)
        {
            try
            {


                string inventories = _oIntegrationDAL.GetSupplierListDB(StartNum, EndNum, UpdatedDate, authkey);
                return inventories;


            }
            catch (Exception)
            {

                throw;
            }
        }

        public string GetSupplierSpecList(string StartNum, string EndNum, string authkey,int SupplierID)
        {
            try
            {


                string inventories = _oIntegrationDAL.GetSupplierSpecListDB(StartNum, EndNum, authkey, SupplierID);
                return inventories;


            }
            catch (Exception)
            {

                throw;
            }
        }


        public string GetMaterialSpecList(string StartNum, string EndNum, string authkey, int MaterialMasterID)
        {
            try
            {


                string inventories = _oIntegrationDAL.GetMaterialSpecListDB(StartNum, EndNum, authkey, MaterialMasterID);
                return inventories;


            }
            catch (Exception)
            {

                throw;
            }
        }

        public string GetInboundSpecList(string StartNum, string EndNum, string authkey, int InboundID)
        {
            try
            {


                string inventories = _oIntegrationDAL.GetInboundSpecListDB(StartNum, EndNum, authkey, InboundID);
                return inventories;


            }
            catch (Exception)
            {

                throw;
            }
        }

        public string GetSLocToSLocList(string StartDate, string EndDate,string WareHouseCode,string PaginationId,string Pagesize ,string authkey)
        {
            try
            {


                string inventories = _oIntegrationDAL.GetSLocToSLocDB(StartDate, EndDate, WareHouseCode, PaginationId, Pagesize, authkey);
                return inventories;


            }
            catch (Exception)
            {

                throw;
            }
        }


        public string GetBinToBinList(string StartDate, string EndDate, string WareHouseCode, string PaginationId, string Pagesize,string TenantID, string authkey)
        {
            try
            {


                string inventories = _oIntegrationDAL.GetBinToBinListDB(StartDate, EndDate, WareHouseCode, PaginationId, Pagesize, TenantID, authkey);
                return inventories;


            }
            catch (Exception)
            {

                throw;
            }
        }




        public string GetItemMasterList(string StartNum, string EndNum, string authkey)
        {
            try
            {


                string inventories = _oIntegrationDAL.GetItemMasterListDB(StartNum, EndNum, authkey);
                return inventories;


            }
            catch (Exception)
            {

                throw;
            }
        }


        public string GetInboundList(string StartNum, string EndNum, string UpdatedDate, string authkey )
        {
            try
            {


                string inventories = _oIntegrationDAL.GetInbounbdListDB(StartNum, EndNum, UpdatedDate, authkey);
                return inventories;


            }
            catch (Exception)
            {

                throw;
            }
        }



        public string GetInboundCustomerReturn(string StartNum, string EndNum, string authkey)
        {
            try
            {


                string inventories = _oIntegrationDAL.GetInboundCustomerReturnDB(StartNum, EndNum, authkey);
                return inventories;


            }
            catch (Exception)
            {

                throw;
            }
        }


        public string GetOutboundSupplierReturn(string StartDate, string EndDate, string PaginationID, string PageSize,string WareHouseCode, string authkey)
        {
            try
            {


                string inventories = _oIntegrationDAL.GetOutboundSupplierReturnDB(StartDate, EndDate, PaginationID, PageSize, WareHouseCode, authkey);
                return inventories;


            }
            catch (Exception)
            {

                throw;
            }
        }






        public string GetIventoryWH(string StartNum, string EndNum, string authkey,int WareHouseID)
        {
            try
            {


                string inventories = _oIntegrationDAL.GetIventoryWHDB(StartNum, EndNum, authkey, WareHouseID);
                return inventories;


            }
            catch (Exception)
            {

                throw;
            }
        }



        public string GetIventoryIMWH(string StartNum, string EndNum, string authkey, int MaterialMasterID)
        {
            try
            {


                string inventories = _oIntegrationDAL.GetIventoryIMDB(StartNum, EndNum, authkey, MaterialMasterID);
                return inventories;


            }
            catch (Exception)
            {

                throw;
            }
        }
        public string CancelSalesOrder(string aPISOCreation, string authkey)
        {
            try
            {

                string result = "";
                result = _oIntegrationDAL.CancelSalesOrder(aPISOCreation, authkey);

                return result;

            }
            catch (Exception)
            {

                throw;
            }
        }


        public string FetOutboundOrder(string StartDate, string EndDate,string PaginationID,string PageSize,string WareHouseCode,string UpdatedDate, string authkey)
        {
            try
            {


                string inventories = _oIntegrationDAL.FetOutboundOrderDB(StartDate, EndDate, PaginationID, PageSize, WareHouseCode, UpdatedDate, authkey);
                return inventories;


            }
            catch (Exception)
            {

                throw;
            }
        }




        //public List<OutboundOrders> GetOutboundOrders(OutboundOrders outboundOrder,string authkey)
        //{
        //    try
        //    {
        //        List<OutboundOrders> outboundOrders = new List<OutboundOrders>();

        //        outboundOrders = _oIntegrationDAL.GetOutboundOrders(outboundOrder, authkey);

        //        return outboundOrders;
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}


        public string CreateSalesOrder(string aPISOCreation, string authkey)
        {
            try
            {

                string result = "";
                result = _oIntegrationDAL.CreateSalesOrder(aPISOCreation, authkey);

                return result;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public string CreatePurchaseOrder(string aPISOCreation, string authtoken)
        {
            try
            {


                string result = "";
                result = _oIntegrationDAL.CreatePurchaseOrder(aPISOCreation, authtoken);

                return result;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public string UpsertBulkOrders(string aPISOCreation,string authkey)
        {
            try
            {
                //List<APISOCreation> aPISOs = new List<APISOCreation>();

                string result = "";
                result = _oIntegrationDAL.UpsertBulkOBD(aPISOCreation, authkey);
                //aPISOs = _oIntegrationDAL.UpsertBulkOBD(aPISOCreation, authkey);

                //return aPISOs;
                return result;

            }
            catch (Exception)
            {

                throw;
            }
        }
        public string MiscReceipt(string aPIMRCreation, string authkey)
        {
            try
            {
                //List<APISOCreation> aPISOs = new List<APISOCreation>();

                string result = "";
                result = _oIntegrationDAL.MiscReceipt(aPIMRCreation, authkey);
                //aPISOs = _oIntegrationDAL.UpsertBulkOBD(aPISOCreation, authkey);

                //return aPISOs;
                return result;

            }
            catch (Exception)
            {

                throw;
            }
        }
        public string MiscIssue(string aPIMICreation, string authkey)
        {
            try
            {
                //List<APISOCreation> aPISOs = new List<APISOCreation>();

                string result = "";
                result = _oIntegrationDAL.MiscIssue(aPIMICreation, authkey);
                //aPISOs = _oIntegrationDAL.UpsertBulkOBD(aPISOCreation, authkey);

                //return aPISOs;
                return result;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public string CreateCustomer(string aPISOCreation, string authkey)
        {
            try
            {

                string result = "";
                result = _oIntegrationDAL.CreateCustomer(aPISOCreation, authkey);

                return result;

            }
            catch (Exception)
            {

                throw;
            }
        }


        public string CreateSupplier(string aPISOCreation, string authkey)
        {
            try
            {

                string result = "";
                result = _oIntegrationDAL.CreateSupplier(aPISOCreation, authkey);

                return result;

            }
            catch (Exception)
            {

                throw;
            }
        }
        public string UpsertBulkInbound(string aPISOCreation,string authtoken)
        {
            try
            {
                //List<APISOCreation> aPISOs = new List<APISOCreation>();

                string result = "";
                result = _oIntegrationDAL.UpsertBulkInbounds(aPISOCreation, authtoken);
                //aPISOs = _oIntegrationDAL.UpsertBulkOBD(aPISOCreation, authkey);

                //return aPISOs;
                return result;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public string UpsertCustomerReturn(string aPISOCreation, string authtoken)
        {
            try
            {
                //List<APISOCreation> aPISOs = new List<APISOCreation>();

                string result = "";
                result = _oIntegrationDAL.UpsertCustomerReturns(aPISOCreation, authtoken);
                //aPISOs = _oIntegrationDAL.UpsertBulkOBD(aPISOCreation, authkey);

                //return aPISOs;
                return result;

            }
            catch (Exception)
            {

                throw;
            }
        }




        public string UpsertBulkSupplier(string aPISOCreation, string authtoken)
        {
            try
            {
                //List<APISOCreation> aPISOs = new List<APISOCreation>();

                string result = "";
                result = _oIntegrationDAL.UpsertBulkSuppliers(aPISOCreation, authtoken);
                //aPISOs = _oIntegrationDAL.UpsertBulkOBD(aPISOCreation, authkey);

                //return aPISOs;
                return result;

            }
            catch (Exception)
            {

                throw;
            }
        }


        public string UpsertBulkItemmaster(string aPISOCreation,string authtoken)
        {
            try
            {
                //List<APISOCreation> aPISOs = new List<APISOCreation>();

                string result = "";
                result = _oIntegrationDAL.UpsertBulkItemMasters(aPISOCreation, authtoken);
                //aPISOs = _oIntegrationDAL.UpsertBulkOBD(aPISOCreation, authkey);

                //return aPISOs;
                return result;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public string UpsertPutItemMasterData(string aPISOCreation, string authtoken)
        {
            try
            {
                //List<APISOCreation> aPISOs = new List<APISOCreation>();

                string result = "";
                result = _oIntegrationDAL.UpsertPutItemMasterData(aPISOCreation, authtoken);
                //aPISOs = _oIntegrationDAL.UpsertBulkOBD(aPISOCreation, authkey);

                //return aPISOs;
                return result;

            }
            catch (Exception)
            {

                throw;
            }
        }


        public string UpsertPutSupplierData(string aPISOCreation, string authtoken)
        {
            try
            {
                //List<APISOCreation> aPISOs = new List<APISOCreation>();

                string result = "";
                result = _oIntegrationDAL.UpsertPutSupplierDatas(aPISOCreation, authtoken);
                //aPISOs = _oIntegrationDAL.UpsertBulkOBD(aPISOCreation, authkey);

                //return aPISOs;
                return result;

            }
            catch (Exception)
            {

                throw;
            }
        }





        public string UpsertSupplierReturn(string aPISOCreation, string authtoken)
        {
            try
            {
                //List<APISOCreation> aPISOs = new List<APISOCreation>();

                string result = "";
                result = _oIntegrationDAL.UpsertSupplierReturns(aPISOCreation, authtoken);
                //aPISOs = _oIntegrationDAL.UpsertBulkOBD(aPISOCreation, authkey);

                //return aPISOs;
                return result;

            }
            catch (Exception)
            {

                throw;
            }
        }


    }
}
