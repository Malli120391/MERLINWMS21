using MRLWMSC21Core.Entities;
using MRLWMSC21Core.Library;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace MRLWMSC21Core.DataAccess
{
    public class ShipperIDIntegrationDAL: BaseDAL
    {
        private string _ClassCode = "WMSCore_DAL_0013_";
        public ShipperIDIntegrationDAL(int LoginUser, string ConnectionString) : base(LoginUser, ConnectionString)
        {

            _ClassCode = ExceptionHandling.GetClassExceptionCode(ExceptionHandling.ExcpConstants_API_DataLayer.IntegrationDAL);
        }

        public bool CheckAuthToken(string AuthKey)
        {
            try
            {
                bool result;
                string _sSQL = "EXEC [dbo].[USP_CheckTenantWiseAuthKey] ";

                List<SqlParameter> lParam = new List<SqlParameter>();

                SqlParameter oParamUser = new SqlParameter("@AuthKey", SqlDbType.NVarChar,100);
                oParamUser.Value = AuthKey;

                lParam.Add(oParamUser);


                DataSet _dsResults = base.FillDataSet(_sSQL, lParam);

                
                if (_dsResults != null && _dsResults.Tables.Count != 0 && _dsResults.Tables[0].Rows.Count != 0)
                {
                    
                    result = true;
                }
                else
                {
                    result = false;
                    
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
                oExcpData.AddInputs("AuthKey", AuthKey);

                ExceptionHandling.LogException(excp, _ClassCode + "001", oExcpData);
                throw new WMSExceptionMessage() { WMSExceptionCode = ErrorMessages.WMSExceptionCode, WMSMessage = ErrorMessages.WMSExceptionMessage, ShowAsCriticalError = true };
            }
        }
        public List<APIInventory> FetchWarehousewiseCurrentstock(APIInventory inventory,string authkey)
        {
            try
            {
                List<APIInventory> inventories = new List<APIInventory>();

                string _sSQL = "EXEC [dbo].[USP_API_GetWarehouseStockInformation] ";

                List<SqlParameter> lParam = new List<SqlParameter>();


                if (inventory.StartDate != null)
                {
                    SqlParameter oParam = new SqlParameter("@ToDate", SqlDbType.NVarChar, 50);
                    oParam.Value = inventory.StartDate;

                    lParam.Add(oParam);
                }

                if (inventory.WareHouseCode != null)
                {
                    SqlParameter oParam = new SqlParameter("@WHCode", SqlDbType.NVarChar, 50);
                    oParam.Value = inventory.WareHouseCode;

                    lParam.Add(oParam);
                }

                if (authkey != null)
                {
                    SqlParameter oParam = new SqlParameter("@AuthKey", SqlDbType.NVarChar, 50);
                    oParam.Value = authkey;

                    lParam.Add(oParam);
                }
                if(inventory.PaginationID != null)
                {
                    SqlParameter oParam = new SqlParameter("@RowNumber", SqlDbType.NVarChar, 50);
                    oParam.Value = inventory.PaginationID;

                    lParam.Add(oParam);
                }

                if (inventory.PageSize != null)
                {
                    SqlParameter oParam = new SqlParameter("@NofRecordsPerPage", SqlDbType.NVarChar, 50);
                    oParam.Value = inventory.PageSize;

                    lParam.Add(oParam);
                }

                if (inventory.UpdatedDate != null)
                {
                    SqlParameter oParam = new SqlParameter("@UpdatedDate", SqlDbType.NVarChar, 50);
                    oParam.Value = inventory.UpdatedDate;

                    lParam.Add(oParam);
                }


                DataSet _dsPickLists = base.FillDataSet(_sSQL, lParam);

                if (_dsPickLists != null)
                {
                    if (_dsPickLists.Tables.Count > 0)
                    {
                        foreach (DataRow _drPickList in _dsPickLists.Tables[0].Rows)
                        {
                            APIInventory _oInventory = new APIInventory()
                            {
                                WareHouseCode = _drPickList["WHCode"].ToString(),
                                TenantCode = _drPickList["TenantCode"].ToString(),
                                MaterialCode = _drPickList["MCode"].ToString(),
                                MaterialDescription = _drPickList["MDescription"].ToString(),
                                UoMQty = _drPickList["UoMQty"].ToString(),
                                MfgDate = _drPickList["MfgDate"].ToString(),
                                ExpDate = _drPickList["ExpDate"].ToString(),
                                BatchNo = _drPickList["BatchNo"].ToString(),
                                SerialNo = _drPickList["SerialNo"].ToString(),
                                ProjectRefNo = _drPickList["ProjectRefNo"].ToString(),
                                MRP =   _drPickList["MRP"].ToString(),
                                BatchNumber = _drPickList["BatchNo"].ToString(),
                                AvailableQuantity = ConversionUtility.ConvertToDecimal(_drPickList["AvailableQty"].ToString()),
                                OnHandQty = ConversionUtility.ConvertToDecimal(_drPickList["OnhandQty"].ToString()),
                                DamagedQty = ConversionUtility.ConvertToDecimal(_drPickList["DamagedQty"].ToString()),
                                AllocatedQty = ConversionUtility.ConvertToDecimal(_drPickList["AllocatedQty"].ToString()),
                                TotalRecords = _drPickList["TotalRecords"].ToString()
                                



                            };
                            inventories.Add(_oInventory);
                        }
                    }
                }


                return inventories;
            }
            catch (WMSExceptionMessage excp)
            {
                throw excp;
            }
            catch (Exception excp)
            {
                ExceptionData oExcpData = new ExceptionData();
                oExcpData.AddInputs("inventory", inventory);

                ExceptionHandling.LogException(excp, _ClassCode + "002", oExcpData);
                throw new WMSExceptionMessage() { WMSExceptionCode = ErrorMessages.WMSExceptionCode, WMSMessage = ErrorMessages.WMSExceptionMessage, ShowAsCriticalError = true };
            }
        }

        //public List<OutboundOrders> GetOutboundOrders(OutboundOrders outboundOrder,string authkey)
        //{
        //    try
        //    {
        //        List<OutboundOrders> outboundOrders = new List<OutboundOrders>();

        //        string _sSQL = "EXEC [dbo].[USP_API_GetOutboundOrdersData] ";

        //        List<SqlParameter> lParam = new List<SqlParameter>();

        //        if (outboundOrder.StartDate != null)
        //        {
        //            SqlParameter oParam = new SqlParameter("@StartDate", SqlDbType.NVarChar, 50);
        //            oParam.Value = outboundOrder.StartDate;

        //            lParam.Add(oParam);
        //        }

        //        if (outboundOrder.EndDate != null)
        //        {
        //            SqlParameter oParam = new SqlParameter("@EndDate", SqlDbType.NVarChar, 50);
        //            oParam.Value = outboundOrder.EndDate;

        //            lParam.Add(oParam);
        //        }
        //        if (authkey != null)
        //        {
        //            SqlParameter oParam = new SqlParameter("@AuthKey", SqlDbType.NVarChar, 50);
        //            oParam.Value = authkey;

        //            lParam.Add(oParam);
        //        }
        //        if (outboundOrder.PaginationID != null)
        //        {
        //            SqlParameter oParam = new SqlParameter("@RowNumber", SqlDbType.NVarChar, 50);
        //            oParam.Value = outboundOrder.PaginationID;

        //            lParam.Add(oParam);
        //        }

        //        if (outboundOrder.PageSize != null)
        //        {
        //            SqlParameter oParam = new SqlParameter("@NofRecordsPerPage", SqlDbType.NVarChar, 50);
        //            oParam.Value = outboundOrder.PageSize;

        //            lParam.Add(oParam);
        //        }

        //        if (outboundOrder.WareHouseCode != null)
        //        {
        //            SqlParameter oParam = new SqlParameter("@WHCode", SqlDbType.NVarChar, 50);
        //            oParam.Value = outboundOrder.WareHouseCode;

        //            lParam.Add(oParam);
        //        }


        //        DataSet _dsPickLists = base.FillDataSet(_sSQL, lParam);

        //        if (_dsPickLists != null)
        //        {
        //            if (_dsPickLists.Tables.Count > 0)
        //            {
        //                foreach (DataRow _drPickList in _dsPickLists.Tables[0].Rows)
        //                {
        //                    OutboundOrders _oOrders = new OutboundOrders()
        //                    {
        //                        WareHouseCode = _drPickList["WHCode"].ToString(),
        //                        TenantCode = _drPickList["TenantCode"].ToString(),
        //                        DeliveryDocNumber = _drPickList["OBDNumber"].ToString(),
        //                        InvoiceNo = _drPickList["InvoiceNo"].ToString(),
        //                        SONumber = _drPickList["SONumber"].ToString(),
        //                        AWBNo = _drPickList["AWBNo"].ToString(),
        //                        Courier = _drPickList["Courier"].ToString(),
        //                        DeliveryDocType=_drPickList["DocumentType"].ToString(),
        //                        DeliveryDocDate = _drPickList["DelvDocDate"].ToString(),
        //                        PickingDate = _drPickList["PickingDate"].ToString(),
        //                        PackingDate = _drPickList["PackingDate"].ToString(),
        //                        LoadGeneratedDate = _drPickList["LoadGeneateDate"].ToString(),
        //                        PGIDate = _drPickList["PGIDate"].ToString(),
        //                        DeliveryDate = _drPickList["DeliveryDate"].ToString(),
        //                        DeliveryStatus = _drPickList["DeliveryStatus"].ToString(),
        //                        TotalRecords = _drPickList["TotalRecords"].ToString()


        //                    };
        //                    outboundOrders.Add(_oOrders);
        //                }
        //            }
        //        }

        //        return outboundOrders;
        //    }
        //    catch (WMSExceptionMessage excp)
        //    {
        //        throw excp;
        //    }
        //    catch (Exception excp)
        //    {
        //        ExceptionData oExcpData = new ExceptionData();
        //        oExcpData.AddInputs("OutboundOrders", outboundOrder);

        //        ExceptionHandling.LogException(excp, _ClassCode + "003", oExcpData);
        //        throw new WMSExceptionMessage() { WMSExceptionCode = ErrorMessages.WMSExceptionCode, WMSMessage = ErrorMessages.WMSExceptionMessage, ShowAsCriticalError = true };
        //    }
        //}


  




        public string UpsertBulkOBD(string aPISOCreation, string authkey)
        {
            try
            {
                List<APISOCreation> aPISOs = new List<APISOCreation>();

                List<SqlParameter> lParam = new List<SqlParameter>();

                string _sSQL = "EXEC [dbo].[USP_OBD_CreateBulkOutboundFromAPI] ";

                _sSQL += "@XML=" + base.SQuote(aPISOCreation);
                _sSQL += ",@AuthToken=" + base.SQuote(authkey);

                DataSet _dsResult = base.FillDataSet(_sSQL, new List<SqlParameter>());

                if (_dsResult != null)
                {
                    if (_dsResult.Tables.Count > 0)
                    {
                        //foreach (DataRow _drPickList in _dsResult.Tables[0].Rows)
                        //{
                        //    //APISOCreation aPISO = new APISOCreation()
                        //    //{

                        //    //};
                        //    aPISOs[0].OrderReuest[0].Result = _drPickList["Result"].ToString();

                        //}
                      
                    }
                }
                return JsonConvert.SerializeObject(_dsResult);


            }
            catch (WMSExceptionMessage excp)
            {
                throw excp;
            }
            catch (Exception excp)
            {
                ExceptionData oExcpData = new ExceptionData();
                oExcpData.AddInputs("APISOCreation", aPISOCreation);

                ExceptionHandling.LogException(excp, _ClassCode + "003", oExcpData);
                throw new WMSExceptionMessage() { WMSExceptionCode = ErrorMessages.WMSExceptionCode, WMSMessage = ErrorMessages.WMSExceptionMessage, ShowAsCriticalError = true };
            }
        }
        public string MiscReceipt(string aPIMRCreation, string authkey)
        {
            try
            {
                List<APISOCreation> aPISOs = new List<APISOCreation>();

                List<SqlParameter> lParam = new List<SqlParameter>();

                string _sSQL = "EXEC [dbo].[INV_POST_MisslleniousReceipt_API] ";

                _sSQL += "@XML=" + base.SQuote(aPIMRCreation);
                _sSQL += ",@AuthKey=" + base.SQuote(authkey);

                DataSet _dsResult = base.FillDataSet(_sSQL, new List<SqlParameter>());

                //if (_dsResult != null)
                //{
                //    if (_dsResult.Tables.Count > 0)
                //    {
                //        //foreach (DataRow _drPickList in _dsResult.Tables[0].Rows)
                //        //{
                //        //    //APISOCreation aPISO = new APISOCreation()
                //        //    //{

                //        //    //};
                //        //    aPISOs[0].OrderReuest[0].Result = _drPickList["Result"].ToString();

                //        //}

                //    }
                //}
                return JsonConvert.SerializeObject(_dsResult);


            }
            catch (WMSExceptionMessage excp)
            {
                throw excp;
            }
            catch (Exception excp)
            {
                ExceptionData oExcpData = new ExceptionData();
                oExcpData.AddInputs("APISOCreation", aPIMRCreation);

                ExceptionHandling.LogException(excp, _ClassCode + "003", oExcpData);
                throw new WMSExceptionMessage() { WMSExceptionCode = ErrorMessages.WMSExceptionCode, WMSMessage = ErrorMessages.WMSExceptionMessage, ShowAsCriticalError = true };
            }
        }

        public string CreateCustomer(string aPISOCreation, string authkey)
        {
            try
            {
                List<APISOCreation> aPISOs = new List<APISOCreation>();

                List<SqlParameter> lParam = new List<SqlParameter>();

                string _sSQL = "EXEC [dbo].[USP_MST_CreateBulkCustomerFromAPI] ";

                _sSQL += "@XML=" + base.SQuote(aPISOCreation);
                _sSQL += ",@AuthKey=" + base.SQuote(authkey);

                DataSet _dsResult = base.FillDataSet(_sSQL, new List<SqlParameter>());

                if (_dsResult != null)
                {
                    if (_dsResult.Tables.Count > 0)
                    {
                        //foreach (DataRow _drPickList in _dsResult.Tables[0].Rows)
                        //{
                        //    //APISOCreation aPISO = new APISOCreation()
                        //    //{

                        //    //};
                        //    aPISOs[0].OrderReuest[0].Result = _drPickList["Result"].ToString();

                        //}

                    }
                }
                return JsonConvert.SerializeObject(_dsResult);


            }
            catch (WMSExceptionMessage excp)
            {
                throw excp;
            }
            catch (Exception excp)
            {
                ExceptionData oExcpData = new ExceptionData();
                oExcpData.AddInputs("APISOCreation", aPISOCreation);

                ExceptionHandling.LogException(excp, _ClassCode + "003", oExcpData);
                throw new WMSExceptionMessage() { WMSExceptionCode = ErrorMessages.WMSExceptionCode, WMSMessage = ErrorMessages.WMSExceptionMessage, ShowAsCriticalError = true };
            }
        }


        public string CreateSupplier(string aPISOCreation, string authkey)
        {
            try
            {
                List<APISOCreation> aPISOs = new List<APISOCreation>();

                List<SqlParameter> lParam = new List<SqlParameter>();

                string _sSQL = "EXEC [dbo].[USP_MST_CreateBulkSupplierFromAPI] ";

                _sSQL += "@XML=" + base.SQuote(aPISOCreation);
                _sSQL += ",@AuthKey=" + base.SQuote(authkey);

                DataSet _dsResult = base.FillDataSet(_sSQL, new List<SqlParameter>());

                if (_dsResult != null)
                {
                    if (_dsResult.Tables.Count > 0)
                    {
                        //foreach (DataRow _drPickList in _dsResult.Tables[0].Rows)
                        //{
                        //    //APISOCreation aPISO = new APISOCreation()
                        //    //{

                        //    //};
                        //    aPISOs[0].OrderReuest[0].Result = _drPickList["Result"].ToString();

                        //}

                    }
                }
                return JsonConvert.SerializeObject(_dsResult);


            }
            catch (WMSExceptionMessage excp)
            {
                throw excp;
            }
            catch (Exception excp)
            {
                ExceptionData oExcpData = new ExceptionData();
                oExcpData.AddInputs("APISOCreation", aPISOCreation);

                ExceptionHandling.LogException(excp, _ClassCode + "003", oExcpData);
                throw new WMSExceptionMessage() { WMSExceptionCode = ErrorMessages.WMSExceptionCode, WMSMessage = ErrorMessages.WMSExceptionMessage, ShowAsCriticalError = true };
            }
        }

        public string MiscIssue(string aPIMICreation, string authkey)
        {
            try
            {
                List<APISOCreation> aPISOs = new List<APISOCreation>();

                List<SqlParameter> lParam = new List<SqlParameter>();

                string _sSQL = "EXEC [dbo].[SP_POST_Missllenious_Issue_API] ";

                _sSQL += "@XML=" + base.SQuote(aPIMICreation);
                _sSQL += ",@AuthKey=" + base.SQuote(authkey);

                DataSet _dsResult = base.FillDataSet(_sSQL, new List<SqlParameter>());

              
                return JsonConvert.SerializeObject(_dsResult);


            }
            catch (WMSExceptionMessage excp)
            {
                throw excp;
            }
            catch (Exception excp)
            {
                ExceptionData oExcpData = new ExceptionData();
                oExcpData.AddInputs("APISOCreation", aPIMICreation);

                ExceptionHandling.LogException(excp, _ClassCode + "003", oExcpData);
                throw new WMSExceptionMessage() { WMSExceptionCode = ErrorMessages.WMSExceptionCode, WMSMessage = ErrorMessages.WMSExceptionMessage, ShowAsCriticalError = true };
            }
        }

        public string UpsertCustomerReturns(string aPISOCreation, string authkey)
        {
            try
            {
                List<APISOCreation> aPISOs = new List<APISOCreation>();

                List<SqlParameter> lParam = new List<SqlParameter>();

                string _sSQL = "EXEC [dbo].[sp_Post_INV_InboundForCustomerReturns_API] ";

                _sSQL += "@XML=" + base.SQuote(aPISOCreation);
                _sSQL += ",@Authkey=" + base.SQuote(authkey);

                DataSet _dsResult = base.FillDataSet(_sSQL, new List<SqlParameter>());

                if (_dsResult != null)
                {
                    if (_dsResult.Tables.Count > 0)
                    {
                        //foreach (DataRow _drPickList in _dsResult.Tables[0].Rows)
                        //{
                        //    //APISOCreation aPISO = new APISOCreation()
                        //    //{

                        //    //};
                        //    aPISOs[0].OrderReuest[0].Result = _drPickList["Result"].ToString();

                        //}

                    }
                }
                return JsonConvert.SerializeObject(_dsResult);


            }
            catch (WMSExceptionMessage excp)
            {
                throw excp;
            }
            catch (Exception excp)
            {
                ExceptionData oExcpData = new ExceptionData();
                oExcpData.AddInputs("APISOCreation", aPISOCreation);

                ExceptionHandling.LogException(excp, _ClassCode + "003", oExcpData);
                throw new WMSExceptionMessage() { WMSExceptionCode = ErrorMessages.WMSExceptionCode, WMSMessage = ErrorMessages.WMSExceptionMessage, ShowAsCriticalError = true };
            }
        }






        public string UpsertBulkInbounds(string aPISOCreation,string authtoken)
        {
            try
            {
                List<APISOCreation> aPISOs = new List<APISOCreation>();

                List<SqlParameter> lParam = new List<SqlParameter>();

                string _sSQL = "EXEC [dbo].[sp_Post_INB_Excel_InwardImport_API] ";

                _sSQL += "@XMLDATA=" + base.SQuote(aPISOCreation);
                _sSQL += ",@Authkey=" + base.SQuote(authtoken);

                DataSet _dsResult = base.FillDataSet(_sSQL, new List<SqlParameter>());

                if (_dsResult != null)
                {
                    if (_dsResult.Tables.Count > 0)
                    {
                        //foreach (DataRow _drPickList in _dsResult.Tables[0].Rows)
                        //{
                        //    //APISOCreation aPISO = new APISOCreation()
                        //    //{

                        //    //};
                        //    aPISOs[0].OrderReuest[0].Result = _drPickList["Result"].ToString();

                        //}

                    }
                }
                return JsonConvert.SerializeObject(_dsResult);


            }
            catch (WMSExceptionMessage excp)
            {
                throw excp;
            }
            catch (Exception excp)
            {
                ExceptionData oExcpData = new ExceptionData();
                oExcpData.AddInputs("APISOCreation", aPISOCreation);

                ExceptionHandling.LogException(excp, _ClassCode + "003", oExcpData);
                throw new WMSExceptionMessage() { WMSExceptionCode = ErrorMessages.WMSExceptionCode, WMSMessage = ErrorMessages.WMSExceptionMessage, ShowAsCriticalError = true };
            }
        }

        public string UpsertBulkSuppliers(string aPISOCreation,string authtoken)
        {
            try
            {
                List<APISOCreation> aPISOs = new List<APISOCreation>();

                List<SqlParameter> lParam = new List<SqlParameter>();

                string _sSQL = "EXEC [dbo].[sp_Post_importSupplier_API] ";

                _sSQL += "@XMLDATA=" + base.SQuote(aPISOCreation);
                 _sSQL += ",@Authkey=" + base.SQuote(authtoken);

                DataSet _dsResult = base.FillDataSet(_sSQL, new List<SqlParameter>());

                if (_dsResult != null)
                {
                    if (_dsResult.Tables.Count > 0)
                    {
                        //foreach (DataRow _drPickList in _dsResult.Tables[0].Rows)
                        //{
                        //    //APISOCreation aPISO = new APISOCreation()
                        //    //{

                        //    //};
                        //    aPISOs[0].OrderReuest[0].Result = _drPickList["Result"].ToString();

                        //}

                    }
                }
                return JsonConvert.SerializeObject(_dsResult);


            }
            catch (WMSExceptionMessage excp)
            {
                throw excp;
            }
            catch (Exception excp)
            {
                ExceptionData oExcpData = new ExceptionData();
                oExcpData.AddInputs("APISOCreation", aPISOCreation);

                ExceptionHandling.LogException(excp, _ClassCode + "003", oExcpData);
                throw new WMSExceptionMessage() { WMSExceptionCode = ErrorMessages.WMSExceptionCode, WMSMessage = ErrorMessages.WMSExceptionMessage, ShowAsCriticalError = true };
            }
        }

        public string UpsertBulkItemMasters(string aPISOCreation,string authtoken)
        {
            try
            {
                List<APISOCreation> aPISOs = new List<APISOCreation>();

                List<SqlParameter> lParam = new List<SqlParameter>();

                string _sSQL = "EXEC [dbo].[sp_Post_importItemMaster_API] ";

                _sSQL += "@XMLDATA=" + base.SQuote(aPISOCreation);
                 _sSQL += ",@Authkey=" + base.SQuote(authtoken);

                DataSet _dsResult = base.FillDataSet(_sSQL, new List<SqlParameter>());

                if (_dsResult != null)
                {
                    if (_dsResult.Tables.Count > 0)
                    {
                        //foreach (DataRow _drPickList in _dsResult.Tables[0].Rows)
                        //{
                        //    //APISOCreation aPISO = new APISOCreation()
                        //    //{

                        //    //};
                        //    aPISOs[0].OrderReuest[0].Result = _drPickList["Result"].ToString();

                        //}

                    }
                }
                return JsonConvert.SerializeObject(_dsResult);


            }
            catch (WMSExceptionMessage excp)
            {
                throw excp;
            }
            catch (Exception excp)
            {
                ExceptionData oExcpData = new ExceptionData();
                oExcpData.AddInputs("APISOCreation", aPISOCreation);

                ExceptionHandling.LogException(excp, _ClassCode + "003", oExcpData);
                throw new WMSExceptionMessage() { WMSExceptionCode = ErrorMessages.WMSExceptionCode, WMSMessage = ErrorMessages.WMSExceptionMessage, ShowAsCriticalError = true };
            }
        }


        public string UpsertPutItemMasterData(string aPISOCreation, string authtoken)
        {
            try
            {
                List<APISOCreation> aPISOs = new List<APISOCreation>();

                List<SqlParameter> lParam = new List<SqlParameter>();

                string _sSQL = "EXEC [dbo].[SP_Upsert_ItemMaster_API] ";

                _sSQL += "@XMLDATA=" + base.SQuote(aPISOCreation);
                _sSQL += ",@Authkey=" + base.SQuote(authtoken);

                DataSet _dsResult = base.FillDataSet(_sSQL, new List<SqlParameter>());

                if (_dsResult != null)
                {
                    if (_dsResult.Tables.Count > 0)
                    {
                        //foreach (DataRow _drPickList in _dsResult.Tables[0].Rows)
                        //{
                        //    //APISOCreation aPISO = new APISOCreation()
                        //    //{

                        //    //};
                        //    aPISOs[0].OrderReuest[0].Result = _drPickList["Result"].ToString();

                        //}

                    }
                }
                return JsonConvert.SerializeObject(_dsResult);


            }
            catch (WMSExceptionMessage excp)
            {
                throw excp;
            }
            catch (Exception excp)
            {
                ExceptionData oExcpData = new ExceptionData();
                oExcpData.AddInputs("APISOCreation", aPISOCreation);

                ExceptionHandling.LogException(excp, _ClassCode + "003", oExcpData);
                throw new WMSExceptionMessage() { WMSExceptionCode = ErrorMessages.WMSExceptionCode, WMSMessage = ErrorMessages.WMSExceptionMessage, ShowAsCriticalError = true };
            }
        }


        public string UpsertPutSupplierDatas(string aPISOCreation, string authtoken)
        {
            try
            {
                List<APISOCreation> aPISOs = new List<APISOCreation>();

                List<SqlParameter> lParam = new List<SqlParameter>();

                string _sSQL = "EXEC [dbo].[SP_Upsert_Supplier_API] ";

                _sSQL += "@XMLDATA=" + base.SQuote(aPISOCreation);
                _sSQL += ",@Authkey=" + base.SQuote(authtoken);

                DataSet _dsResult = base.FillDataSet(_sSQL, new List<SqlParameter>());

                if (_dsResult != null)
                {
                    if (_dsResult.Tables.Count > 0)
                    {
                        //foreach (DataRow _drPickList in _dsResult.Tables[0].Rows)
                        //{
                        //    //APISOCreation aPISO = new APISOCreation()
                        //    //{

                        //    //};
                        //    aPISOs[0].OrderReuest[0].Result = _drPickList["Result"].ToString();

                        //}

                    }
                }
                return JsonConvert.SerializeObject(_dsResult);


            }
            catch (WMSExceptionMessage excp)
            {
                throw excp;
            }
            catch (Exception excp)
            {
                ExceptionData oExcpData = new ExceptionData();
                oExcpData.AddInputs("APISOCreation", aPISOCreation);

                ExceptionHandling.LogException(excp, _ClassCode + "003", oExcpData);
                throw new WMSExceptionMessage() { WMSExceptionCode = ErrorMessages.WMSExceptionCode, WMSMessage = ErrorMessages.WMSExceptionMessage, ShowAsCriticalError = true };
            }
        }




        public string UpsertSupplierReturns(string aPISOCreation, string authtoken)
        {
            try
            {
                List<APISOCreation> aPISOs = new List<APISOCreation>();

                List<SqlParameter> lParam = new List<SqlParameter>();

                string _sSQL = "EXEC [dbo].[DummyOutbound_SupplierReturns_API] ";

                _sSQL += "@XML=" + base.SQuote(aPISOCreation);
                _sSQL += ",@Authkey=" + base.SQuote(authtoken);

                DataSet _dsResult = base.FillDataSet(_sSQL, new List<SqlParameter>());

                if (_dsResult != null)
                {
                    if (_dsResult.Tables.Count > 0)
                    {
                        //foreach (DataRow _drPickList in _dsResult.Tables[0].Rows)
                        //{
                        //    //APISOCreation aPISO = new APISOCreation()
                        //    //{

                        //    //};
                        //    aPISOs[0].OrderReuest[0].Result = _drPickList["Result"].ToString();

                        //}

                    }
                }
                return JsonConvert.SerializeObject(_dsResult);


            }
            catch (WMSExceptionMessage excp)
            {
                throw excp;
            }
            catch (Exception excp)
            {
                ExceptionData oExcpData = new ExceptionData();
                oExcpData.AddInputs("APISOCreation", aPISOCreation);

                ExceptionHandling.LogException(excp, _ClassCode + "003", oExcpData);
                throw new WMSExceptionMessage() { WMSExceptionCode = ErrorMessages.WMSExceptionCode, WMSMessage = ErrorMessages.WMSExceptionMessage, ShowAsCriticalError = true };
            }
        }





        public string GetTenantDataDB(string StartNum, string EndNum,string authkey)
        {
            try
            {
                

                List<SqlParameter> lParam = new List<SqlParameter>();

                string _sSQL = "EXEC [dbo].[SP_GET_TenatData_API] ";

                // _sSQL += "@XML=" + base.SQuote(aPISOCreation);
                _sSQL += "@StartNum=" + base.SQuote(StartNum);
                _sSQL += ",@EndNum=" + base.SQuote(EndNum);
                _sSQL += ",@Authkey=" + base.SQuote(authkey);

                DataSet _dsResult = base.FillDataSet(_sSQL, new List<SqlParameter>());

                
                return JsonConvert.SerializeObject(_dsResult);


            }
            catch (WMSExceptionMessage excp)
            {
                throw excp;
            }
            catch (Exception excp)
            {
                ExceptionData oExcpData = new ExceptionData();
                //oExcpData.AddInputs("APISOCreation", aPISOCreation);

                ExceptionHandling.LogException(excp, _ClassCode + "003", oExcpData);
                throw new WMSExceptionMessage() { WMSExceptionCode = ErrorMessages.WMSExceptionCode, WMSMessage = ErrorMessages.WMSExceptionMessage, ShowAsCriticalError = true };
            }
        }



        public string FetchOutBoundByOBDDB( string authkey,int OutBoundID)
        {
            try
            {


                List<SqlParameter> lParam = new List<SqlParameter>();

                string _sSQL = "EXEC [dbo].[USP_API_GetOutboundOrdersData_For_OBD] ";

                 _sSQL += "@OutBoundID=" + OutBoundID;
              
                _sSQL += ",@Authkey=" + base.SQuote(authkey);

                DataSet _dsResult = base.FillDataSet(_sSQL, new List<SqlParameter>());


                return JsonConvert.SerializeObject(_dsResult);


            }
            catch (WMSExceptionMessage excp)
            {
                throw excp;
            }
            catch (Exception excp)
            {
                ExceptionData oExcpData = new ExceptionData();
                //oExcpData.AddInputs("APISOCreation", aPISOCreation);

                ExceptionHandling.LogException(excp, _ClassCode + "003", oExcpData);
                throw new WMSExceptionMessage() { WMSExceptionCode = ErrorMessages.WMSExceptionCode, WMSMessage = ErrorMessages.WMSExceptionMessage, ShowAsCriticalError = true };
            }
        }


        public string RevertParticularInboundsDB(string authkey, int InboundID)
        {
            try
            {


                List<SqlParameter> lParam = new List<SqlParameter>();

                string _sSQL = "EXEC [dbo].[SP_RevertInbound] ";

                _sSQL += "@InboundID=" + InboundID;

                _sSQL += ",@Authkey=" + base.SQuote(authkey);

                DataSet _dsResult = base.FillDataSet(_sSQL, new List<SqlParameter>());


                return JsonConvert.SerializeObject(_dsResult);


            }
            catch (WMSExceptionMessage excp)
            {
                throw excp;
            }
            catch (Exception excp)
            {
                ExceptionData oExcpData = new ExceptionData();
                //oExcpData.AddInputs("APISOCreation", aPISOCreation);

                ExceptionHandling.LogException(excp, _ClassCode + "003", oExcpData);
                throw new WMSExceptionMessage() { WMSExceptionCode = ErrorMessages.WMSExceptionCode, WMSMessage = ErrorMessages.WMSExceptionMessage, ShowAsCriticalError = true };
            }
        }


        public string RevertOutboundsDB(string authkey, int OutBound)
        {
            try
            {


                List<SqlParameter> lParam = new List<SqlParameter>();

                string _sSQL = "EXEC [dbo].[SP_RevertOutbound] ";

                _sSQL += "@OUTBOUNDID=" + OutBound;

                _sSQL += ",@Authkey=" + base.SQuote(authkey);

                DataSet _dsResult = base.FillDataSet(_sSQL, new List<SqlParameter>());


                return JsonConvert.SerializeObject(_dsResult);


            }
            catch (WMSExceptionMessage excp)
            {
                throw excp;
            }
            catch (Exception excp)
            {
                ExceptionData oExcpData = new ExceptionData();
                //oExcpData.AddInputs("APISOCreation", aPISOCreation);

                ExceptionHandling.LogException(excp, _ClassCode + "003", oExcpData);
                throw new WMSExceptionMessage() { WMSExceptionCode = ErrorMessages.WMSExceptionCode, WMSMessage = ErrorMessages.WMSExceptionMessage, ShowAsCriticalError = true };
            }
        }



        public string GetReleaseOBDDB(string StartDate, string EndDate, string WarehouseCode, string PaginationID, string PageSize, string authkey)
        {
            try
            {


                List<SqlParameter> lParam = new List<SqlParameter>();

                string _sSQL = "EXEC [dbo].[SP_GET_ReleseOBDStatus_API] ";

                // _sSQL += "@XML=" + base.SQuote(aPISOCreation);
                _sSQL += "@StartDate=" + base.SQuote(StartDate);
                _sSQL += ",@ENDdate=" + base.SQuote(EndDate);
                _sSQL += ",@WHCODE =" + base.SQuote(WarehouseCode);
                _sSQL += ",@PaginationId=" + PaginationID;
                _sSQL += ",@Pagesize=" + PageSize;
                _sSQL += ",@Authkey=" + base.SQuote(authkey);

                DataSet _dsResult = base.FillDataSet(_sSQL, new List<SqlParameter>());


                return JsonConvert.SerializeObject(_dsResult);


            }
            catch (WMSExceptionMessage excp)
            {
                throw excp;
            }
            catch (Exception excp)
            {
                ExceptionData oExcpData = new ExceptionData();
                //oExcpData.AddInputs("APISOCreation", aPISOCreation);

                ExceptionHandling.LogException(excp, _ClassCode + "003", oExcpData);
                throw new WMSExceptionMessage() { WMSExceptionCode = ErrorMessages.WMSExceptionCode, WMSMessage = ErrorMessages.WMSExceptionMessage, ShowAsCriticalError = true };
            }
        }



        public string GetSupplierListDB(string StartNum, string EndNum,string UpdatedDate, string authkey)
        {
            try
            {


                List<SqlParameter> lParam = new List<SqlParameter>();

                string _sSQL = "EXEC [dbo].[SP_GET_Suppliers_API] ";

                // _sSQL += "@XML=" + base.SQuote(aPISOCreation);
                _sSQL += "@StartNum=" + base.SQuote(StartNum);
                _sSQL += ",@EndNum=" + base.SQuote(EndNum);
                _sSQL += ",@UpdatedDate=" + base.SQuote(UpdatedDate);
                _sSQL += ",@Authkey=" + base.SQuote(authkey);

                DataSet _dsResult = base.FillDataSet(_sSQL, new List<SqlParameter>());


                return JsonConvert.SerializeObject(_dsResult);


            }
            catch (WMSExceptionMessage excp)
            {
                throw excp;
            }
            catch (Exception excp)
            {
                ExceptionData oExcpData = new ExceptionData();
                //oExcpData.AddInputs("APISOCreation", aPISOCreation);

                ExceptionHandling.LogException(excp, _ClassCode + "003", oExcpData);
                throw new WMSExceptionMessage() { WMSExceptionCode = ErrorMessages.WMSExceptionCode, WMSMessage = ErrorMessages.WMSExceptionMessage, ShowAsCriticalError = true };
            }
        }


        public string GetSupplierSpecListDB(string StartNum, string EndNum, string authkey,int SupplierID)
        {
            try
            {


                List<SqlParameter> lParam = new List<SqlParameter>();

                string _sSQL = "EXEC [dbo].[SP_GET_Spec_suppliers_API] ";

                // _sSQL += "@XML=" + base.SQuote(aPISOCreation);
                _sSQL += "@StartNum=" + base.SQuote(StartNum);
                _sSQL += ",@EndNum=" + base.SQuote(EndNum);
                _sSQL += ",@Authkey=" + base.SQuote(authkey);
                _sSQL += ",@SupplierID=" + SupplierID;

                DataSet _dsResult = base.FillDataSet(_sSQL, new List<SqlParameter>());


                return JsonConvert.SerializeObject(_dsResult);


            }
            catch (WMSExceptionMessage excp)
            {
                throw excp;
            }
            catch (Exception excp)
            {
                ExceptionData oExcpData = new ExceptionData();
                //oExcpData.AddInputs("APISOCreation", aPISOCreation);

                ExceptionHandling.LogException(excp, _ClassCode + "003", oExcpData);
                throw new WMSExceptionMessage() { WMSExceptionCode = ErrorMessages.WMSExceptionCode, WMSMessage = ErrorMessages.WMSExceptionMessage, ShowAsCriticalError = true };
            }
        }





        public string GetMaterialSpecListDB(string StartNum, string EndNum, string authkey, int MaterialMasterID)
        {
            try
            {


                List<SqlParameter> lParam = new List<SqlParameter>();

                string _sSQL = "EXEC [dbo].[SP_GET_Spec_ItemMaster_API] ";

                // _sSQL += "@XML=" + base.SQuote(aPISOCreation);
                _sSQL += "@StartNum=" + base.SQuote(StartNum);
                _sSQL += ",@EndNum=" + base.SQuote(EndNum);
                _sSQL += ",@Authkey=" + base.SQuote(authkey);
                _sSQL += ",@MaterialMasterID=" +MaterialMasterID;

                DataSet _dsResult = base.FillDataSet(_sSQL, new List<SqlParameter>());


                return JsonConvert.SerializeObject(_dsResult);


            }
            catch (WMSExceptionMessage excp)
            {
                throw excp;
            }
            catch (Exception excp)
            {
                ExceptionData oExcpData = new ExceptionData();
                //oExcpData.AddInputs("APISOCreation", aPISOCreation);

                ExceptionHandling.LogException(excp, _ClassCode + "003", oExcpData);
                throw new WMSExceptionMessage() { WMSExceptionCode = ErrorMessages.WMSExceptionCode, WMSMessage = ErrorMessages.WMSExceptionMessage, ShowAsCriticalError = true };
            }
        }


        public string GetInboundSpecListDB(string StartNum, string EndNum, string authkey, int InboundID)
        {
            try
            {


                List<SqlParameter> lParam = new List<SqlParameter>();

                string _sSQL = "EXEC [dbo].[SP_GET_Spec_INBOUND_API] ";

                // _sSQL += "@XML=" + base.SQuote(aPISOCreation);
                _sSQL += "@StartNum=" + base.SQuote(StartNum);
                _sSQL += ",@EndNum=" + base.SQuote(EndNum);
                _sSQL += ",@Authkey=" + base.SQuote(authkey);
                _sSQL += ",@InboundID=" + InboundID;

                DataSet _dsResult = base.FillDataSet(_sSQL, new List<SqlParameter>());


                return JsonConvert.SerializeObject(_dsResult);


            }
            catch (WMSExceptionMessage excp)
            {
                throw excp;
            }
            catch (Exception excp)
            {
                ExceptionData oExcpData = new ExceptionData();
                //oExcpData.AddInputs("APISOCreation", aPISOCreation);

                ExceptionHandling.LogException(excp, _ClassCode + "003", oExcpData);
                throw new WMSExceptionMessage() { WMSExceptionCode = ErrorMessages.WMSExceptionCode, WMSMessage = ErrorMessages.WMSExceptionMessage, ShowAsCriticalError = true };
            }
        }


        public string GetSLocToSLocDB(string StartDate, string EndDate, string WareHouseCode, string PaginationId, string Pagesize, string authkey)
        {
            try
            {


                List<SqlParameter> lParam = new List<SqlParameter>();

                string _sSQL = "EXEC [dbo].[GET_TRANSFERREQUEST_HEADER_INFO_API] ";

                // _sSQL += "@XML=" + base.SQuote(aPISOCreation);
                _sSQL += "@StartDate=" + base.SQuote(StartDate);
                _sSQL += ",@EndDate=" + base.SQuote(EndDate);
                _sSQL += ",@WareHouseCode=" + base.SQuote(WareHouseCode);
                _sSQL += ",@Authkey=" + base.SQuote(authkey);
                _sSQL += ",@PaginationId=" + PaginationId;
                _sSQL += ",@Pagesize=" + Pagesize;

                DataSet _dsResult = base.FillDataSet(_sSQL, new List<SqlParameter>());


                return JsonConvert.SerializeObject(_dsResult);


            }
            catch (WMSExceptionMessage excp)
            {
                throw excp;
            }
            catch (Exception excp)
            {
                ExceptionData oExcpData = new ExceptionData();
                //oExcpData.AddInputs("APISOCreation", aPISOCreation);

                ExceptionHandling.LogException(excp, _ClassCode + "003", oExcpData);
                throw new WMSExceptionMessage() { WMSExceptionCode = ErrorMessages.WMSExceptionCode, WMSMessage = ErrorMessages.WMSExceptionMessage, ShowAsCriticalError = true };
            }
        }


        public string GetBinToBinListDB(string StartDate, string EndDate, string WareHouseCode, string PaginationId, string Pagesize,string TenantID, string authkey)
        {
            try
            {


                List<SqlParameter> lParam = new List<SqlParameter>();

                string _sSQL = "EXEC [dbo].[SP_Bin_to_Bin_API] ";

                // _sSQL += "@XML=" + base.SQuote(aPISOCreation);
                _sSQL += "@StartDate=" + base.SQuote(StartDate);
                _sSQL += ",@EndDate=" + base.SQuote(EndDate);
                _sSQL += ",@WareHouseCode=" + base.SQuote(WareHouseCode);
                _sSQL += ",@Authkey=" + base.SQuote(authkey);
                _sSQL += ",@PaginationId=" + base.SQuote(PaginationId);
                _sSQL += ",@Pagesize=" + base.SQuote(Pagesize);
              

                DataSet _dsResult = base.FillDataSet(_sSQL, new List<SqlParameter>());


                return JsonConvert.SerializeObject(_dsResult);


            }
            catch (WMSExceptionMessage excp)
            {
                throw excp;
            }
            catch (Exception excp)
            {
                ExceptionData oExcpData = new ExceptionData();
                //oExcpData.AddInputs("APISOCreation", aPISOCreation);

                ExceptionHandling.LogException(excp, _ClassCode + "003", oExcpData);
                throw new WMSExceptionMessage() { WMSExceptionCode = ErrorMessages.WMSExceptionCode, WMSMessage = ErrorMessages.WMSExceptionMessage, ShowAsCriticalError = true };
            }
        }






        public string GetItemMasterListDB(string StartNum, string EndNum, string authkey)
        {
            try
            {


                List<SqlParameter> lParam = new List<SqlParameter>();

                string _sSQL = "EXEC [dbo].[SP_GET_ItemMaster_API] ";

                // _sSQL += "@XML=" + base.SQuote(aPISOCreation);
                _sSQL += "@StartNum=" + base.SQuote(StartNum);
                _sSQL += ",@EndNum=" + base.SQuote(EndNum);
                _sSQL += ",@Authkey=" + base.SQuote(authkey);

                DataSet _dsResult = base.FillDataSet(_sSQL, new List<SqlParameter>());




                return JsonConvert.SerializeObject(_dsResult);

               


            }
            catch (WMSExceptionMessage excp)
            {
                throw excp;
            }
            catch (Exception excp)
            {
                ExceptionData oExcpData = new ExceptionData();
                //oExcpData.AddInputs("APISOCreation", aPISOCreation);

                ExceptionHandling.LogException(excp, _ClassCode + "003", oExcpData);
                throw new WMSExceptionMessage() { WMSExceptionCode = ErrorMessages.WMSExceptionCode, WMSMessage = ErrorMessages.WMSExceptionMessage, ShowAsCriticalError = true };
            }
        }

        public string GetInbounbdListDB(string StartNum, string EndNum,string UpdatedDate, string authkey)
        {
            try
            {


                List<SqlParameter> lParam = new List<SqlParameter>();

                string _sSQL = "EXEC [dbo].[SP_GET_INBOUND_API] ";

                // _sSQL += "@XML=" + base.SQuote(aPISOCreation);
                _sSQL += "@StartNum=" + base.SQuote(StartNum);
                _sSQL += ",@EndNum=" + base.SQuote(EndNum);
                _sSQL += ",@UpdatedDate=" + base.SQuote(UpdatedDate);
                _sSQL += ",@Authkey=" + base.SQuote(authkey);

                DataSet _dsResult = base.FillDataSet(_sSQL, new List<SqlParameter>());


                return JsonConvert.SerializeObject(_dsResult);


            }
            catch (WMSExceptionMessage excp)
            {
                throw excp;
            }
            catch (Exception excp)
            {
                ExceptionData oExcpData = new ExceptionData();
                //oExcpData.AddInputs("APISOCreation", aPISOCreation);

                ExceptionHandling.LogException(excp, _ClassCode + "003", oExcpData);
                throw new WMSExceptionMessage() { WMSExceptionCode = ErrorMessages.WMSExceptionCode, WMSMessage = ErrorMessages.WMSExceptionMessage, ShowAsCriticalError = true };
            }
        }


        public string GetInboundCustomerReturnDB(string StartNum, string EndNum, string authkey)
        {
            try
            {


                List<SqlParameter> lParam = new List<SqlParameter>();

                string _sSQL = "EXEC [dbo].[SP_GET_CustomerReturn_INBOUND_API] ";

                // _sSQL += "@XML=" + base.SQuote(aPISOCreation);
                _sSQL += "@StartNum=" + base.SQuote(StartNum);
                _sSQL += ",@EndNum=" + base.SQuote(EndNum);
                _sSQL += ",@Authkey=" + base.SQuote(authkey);

                DataSet _dsResult = base.FillDataSet(_sSQL, new List<SqlParameter>());


                return JsonConvert.SerializeObject(_dsResult);


            }
            catch (WMSExceptionMessage excp)
            {
                throw excp;
            }
            catch (Exception excp)
            {
                ExceptionData oExcpData = new ExceptionData();
                //oExcpData.AddInputs("APISOCreation", aPISOCreation);

                ExceptionHandling.LogException(excp, _ClassCode + "003", oExcpData);
                throw new WMSExceptionMessage() { WMSExceptionCode = ErrorMessages.WMSExceptionCode, WMSMessage = ErrorMessages.WMSExceptionMessage, ShowAsCriticalError = true };
            }
        }



        public string GetOutboundSupplierReturnDB(string StartDate, string EndDate, string PaginationID, string PageSize, string WareHouseCode, string authkey)
        {
            try
            {


                List<SqlParameter> lParam = new List<SqlParameter>();

                string _sSQL = "EXEC [dbo].[USP_API_GetOutboundSupplierReturn_API] ";

                // _sSQL += "@XML=" + base.SQuote(aPISOCreation);
                _sSQL += "@StartDate=" + base.SQuote(StartDate);
                _sSQL += ",@EndDate=" + base.SQuote(EndDate);
                _sSQL += ",@WHCode=" + base.SQuote(WareHouseCode);
                _sSQL += ",@RowNumber="  +base.SQuote(PaginationID);
                _sSQL += ",@NofRecordsPerPage="+base.SQuote(PageSize);
                _sSQL += ",@Authkey=" + base.SQuote(authkey);

                DataSet _dsResult = base.FillDataSet(_sSQL, new List<SqlParameter>());


                return JsonConvert.SerializeObject(_dsResult);


            }
            catch (WMSExceptionMessage excp)
            {
                throw excp;
            }
            catch (Exception excp)
            {
                ExceptionData oExcpData = new ExceptionData();
                //oExcpData.AddInputs("APISOCreation", aPISOCreation);

                ExceptionHandling.LogException(excp, _ClassCode + "003", oExcpData);
                throw new WMSExceptionMessage() { WMSExceptionCode = ErrorMessages.WMSExceptionCode, WMSMessage = ErrorMessages.WMSExceptionMessage, ShowAsCriticalError = true };
            }
        }


        public string GetIventoryWHDB(string StartNum, string EndNum, string authkey,int WareHouseID)
        {
            try
            {


                List<SqlParameter> lParam = new List<SqlParameter>();

                string _sSQL = "EXEC [dbo].[SP_GET_SpecificWHSTOCKREPORT_API] ";

                // _sSQL += "@XML=" + base.SQuote(aPISOCreation);
                _sSQL += "@StartNum=" + base.SQuote(StartNum);
                _sSQL += ",@EndNum=" + base.SQuote(EndNum);
                _sSQL += ",@WHID=" + (WareHouseID);
                _sSQL += ",@Authkey=" + base.SQuote(authkey);

                DataSet _dsResult = base.FillDataSet(_sSQL, new List<SqlParameter>());


                return JsonConvert.SerializeObject(_dsResult);


            }
            catch (WMSExceptionMessage excp)
            {
                throw excp;
            }
            catch (Exception excp)
            {
                ExceptionData oExcpData = new ExceptionData();
                //oExcpData.AddInputs("APISOCreation", aPISOCreation);

                ExceptionHandling.LogException(excp, _ClassCode + "003", oExcpData);
                throw new WMSExceptionMessage() { WMSExceptionCode = ErrorMessages.WMSExceptionCode, WMSMessage = ErrorMessages.WMSExceptionMessage, ShowAsCriticalError = true };
            }
        }


        public string GetIventoryIMDB(string StartNum, string EndNum, string authkey, int MaterialMasterID)
        {
            try
            {


                List<SqlParameter> lParam = new List<SqlParameter>();

                string _sSQL = "EXEC [dbo].[SP_GET_SpecificItemMaterWHStockReport_API] ";

                // _sSQL += "@XML=" + base.SQuote(aPISOCreation);
                _sSQL += "@StartNum=" + base.SQuote(StartNum);
                _sSQL += ",@EndNum=" + base.SQuote(EndNum);
                _sSQL += ",@MaterialMasterID=" + (MaterialMasterID);
                _sSQL += ",@Authkey=" + base.SQuote(authkey);

                DataSet _dsResult = base.FillDataSet(_sSQL, new List<SqlParameter>());


                return JsonConvert.SerializeObject(_dsResult);


            }
            catch (WMSExceptionMessage excp)
            {
                throw excp;
            }
            catch (Exception excp)
            {
                ExceptionData oExcpData = new ExceptionData();
                //oExcpData.AddInputs("APISOCreation", aPISOCreation);

                ExceptionHandling.LogException(excp, _ClassCode + "003", oExcpData);
                throw new WMSExceptionMessage() { WMSExceptionCode = ErrorMessages.WMSExceptionCode, WMSMessage = ErrorMessages.WMSExceptionMessage, ShowAsCriticalError = true };
            }
        }

        public string CancelSalesOrder(string aPISOCreation, string authkey)
        {
            try
            {
                List<APISOCreation> aPISOs = new List<APISOCreation>();

                List<SqlParameter> lParam = new List<SqlParameter>();

                string _sSQL = "EXEC [dbo].[USP_ORD_UpdateSalesOrder] ";

                _sSQL += "@XML=" + base.SQuote(aPISOCreation);
                _sSQL += ",@AuthToken=" + base.SQuote(authkey);

                DataSet _dsResult = base.FillDataSet(_sSQL, new List<SqlParameter>());

                if (_dsResult != null)
                {
                    if (_dsResult.Tables.Count > 0)
                    {
                        //foreach (DataRow _drPickList in _dsResult.Tables[0].Rows)
                        //{
                        //    //APISOCreation aPISO = new APISOCreation()
                        //    //{

                        //    //};
                        //    aPISOs[0].OrderReuest[0].Result = _drPickList["Result"].ToString();

                        //}

                    }
                }
                return JsonConvert.SerializeObject(_dsResult);


            }
            catch (WMSExceptionMessage excp)
            {
                throw excp;
            }
            catch (Exception excp)
            {
                ExceptionData oExcpData = new ExceptionData();
                oExcpData.AddInputs("APISOCreation", aPISOCreation);

                ExceptionHandling.LogException(excp, _ClassCode + "003", oExcpData);
                throw new WMSExceptionMessage() { WMSExceptionCode = ErrorMessages.WMSExceptionCode, WMSMessage = ErrorMessages.WMSExceptionMessage, ShowAsCriticalError = true };
            }
        }

        public string CreateSalesOrder(string aPISOCreation, string authkey)
        {
            try
            {
                List<APISOCreation> aPISOs = new List<APISOCreation>();

                List<SqlParameter> lParam = new List<SqlParameter>();

                string _sSQL = "EXEC [dbo].[USP_API_CreateSalesOrdersFromAPI] ";

                _sSQL += "@XML=" + base.SQuote(aPISOCreation);
                _sSQL += ",@AuthToken=" + base.SQuote(authkey);

                DataSet _dsResult = base.FillDataSet(_sSQL, new List<SqlParameter>());

                if (_dsResult != null)
                {
                    if (_dsResult.Tables.Count > 0)
                    {
                        //foreach (DataRow _drPickList in _dsResult.Tables[0].Rows)
                        //{
                        //    //APISOCreation aPISO = new APISOCreation()
                        //    //{

                        //    //};
                        //    aPISOs[0].OrderReuest[0].Result = _drPickList["Result"].ToString();

                        //}

                    }
                }
                return JsonConvert.SerializeObject(_dsResult);


            }
            catch (WMSExceptionMessage excp)
            {
                throw excp;
            }
            catch (Exception excp)
            {
                ExceptionData oExcpData = new ExceptionData();
                oExcpData.AddInputs("APISOCreation", aPISOCreation);

                ExceptionHandling.LogException(excp, _ClassCode + "003", oExcpData);
                throw new WMSExceptionMessage() { WMSExceptionCode = ErrorMessages.WMSExceptionCode, WMSMessage = ErrorMessages.WMSExceptionMessage, ShowAsCriticalError = true };
            }
        }

        public string CreatePurchaseOrder(string aPISOCreation, string authtoken)
        {
            try
            {
                List<APISOCreation> aPISOs = new List<APISOCreation>();

                List<SqlParameter> lParam = new List<SqlParameter>();

                string _sSQL = "EXEC [dbo].[USP_API_CreatePurchaseOrdersFromAPI] ";

                _sSQL += "@XMLDATA=" + base.SQuote(aPISOCreation);
                _sSQL += ",@Authkey=" + base.SQuote(authtoken);

                DataSet _dsResult = base.FillDataSet(_sSQL, new List<SqlParameter>());

                if (_dsResult != null)
                {
                    if (_dsResult.Tables.Count > 0)
                    {
                        //foreach (DataRow _drPickList in _dsResult.Tables[0].Rows)
                        //{
                        //    //APISOCreation aPISO = new APISOCreation()
                        //    //{

                        //    //};
                        //    aPISOs[0].OrderReuest[0].Result = _drPickList["Result"].ToString();

                        //}

                    }
                }
                return JsonConvert.SerializeObject(_dsResult);


            }
            catch (WMSExceptionMessage excp)
            {
                throw excp;
            }
            catch (Exception excp)
            {
                ExceptionData oExcpData = new ExceptionData();
                oExcpData.AddInputs("APISOCreation", aPISOCreation);

                ExceptionHandling.LogException(excp, _ClassCode + "003", oExcpData);
                throw new WMSExceptionMessage() { WMSExceptionCode = ErrorMessages.WMSExceptionCode, WMSMessage = ErrorMessages.WMSExceptionMessage, ShowAsCriticalError = true };
            }
        }
        public string FetOutboundOrderDB(string StartDate, string EndDate, string PaginationID, string PageSize, string WareHouseCode,string UpdatedDate, string authkey)
        {
            try
            {


                List<SqlParameter> lParam = new List<SqlParameter>();

                string _sSQL = "EXEC [dbo].[USP_API_GetOutboundOrdersData] ";

                // _sSQL += "@XML=" + base.SQuote(aPISOCreation);
                _sSQL += "@StartDate=" + base.SQuote(StartDate);
                _sSQL += ",@EndDate=" + base.SQuote(EndDate);
                _sSQL += ",@WHCode=" + base.SQuote(WareHouseCode);
                _sSQL += ",@RowNumber=" + base.SQuote(PaginationID);
                _sSQL += ",@NofRecordsPerPage=" + base.SQuote(PageSize);
                _sSQL += ",@UpdatedDate=" + base.SQuote(UpdatedDate);
                _sSQL += ",@Authkey=" + base.SQuote(authkey);

                DataSet _dsResult = base.FillDataSet(_sSQL, new List<SqlParameter>());


                return JsonConvert.SerializeObject(_dsResult);


            }
            catch (WMSExceptionMessage excp)
            {
                throw excp;
            }
            catch (Exception excp)
            {
                ExceptionData oExcpData = new ExceptionData();
                //oExcpData.AddInputs("APISOCreation", aPISOCreation);

                ExceptionHandling.LogException(excp, _ClassCode + "003", oExcpData);
                throw new WMSExceptionMessage() { WMSExceptionCode = ErrorMessages.WMSExceptionCode, WMSMessage = ErrorMessages.WMSExceptionMessage, ShowAsCriticalError = true };
            }
        }




    }
}
