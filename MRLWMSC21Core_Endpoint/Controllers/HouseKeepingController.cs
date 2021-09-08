using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MRLWMSC21Core.Business;
using MRLWMSC21Core.DataAccess;
using MRLWMSC21Core.Entities;
using MRLWMSC21Core.Library;
using MRLWMSC21_Endpoint.Interfaces;
using MRLWMSC21_Endpoint.Security;
using MRLWMSC21_Endpoint.DTO;
using MRLWMSC21_Endpoint.Constants;
using Newtonsoft.Json;
using System.Data;


namespace MRLWMSC21_Endpoint.Controllers
{
    [RoutePrefix("HouseKeeping")]
    public class HouseKeepingController :  BaseController, iHouseKeeping
    {
        private string _ClassCode = string.Empty;

        public HouseKeepingController()
        {
            _ClassCode = ExceptionHandling.GetClassExceptionCode(ExceptionHandling.ExcpConstants_API_Enpoint.HouseKeepingController);
        }



     

        [HttpPost]
        [Route("CheckLocationForLiveStock")]
        public string CheckLocationForLiveStock(WMSCoreMessage oRequest)
        {
            try
            {
                LoadControllerDefaults(oRequest);
                string result = null;
                InventoryDTO _oResponse = new InventoryDTO();
                if (oRequest != null)
                {
                    List<InventoryDTO> lInventoryDTO = new List<InventoryDTO>();
                    InventoryDTO _oInventory = (InventoryDTO)WMSCoreEndpointSecurity.ValidateRequest(oRequest);


                    if (_oInventory != null)
                    {
                        
                        MRLWMSC21Service.LiveStock liveStock = new MRLWMSC21Service.LiveStock()
                        {
                            AccountId = ConversionUtility.ConvertToInt(_oInventory.AccountID),
                            Location = _oInventory.LocationCode,
                            WarehouseID = ConversionUtility.ConvertToInt(_oInventory.WarehouseId)
                        };

                   
                       
                        MRLWMSC21Service.MRLWMSC21HHTWCFServiceClient oFalcon = new MRLWMSC21Service.MRLWMSC21HHTWCFServiceClient();
                        result=oFalcon.CheckLocationForLiveStock(liveStock);
                        if(result!=null && result!="")
                        {
                            throw new WMSExceptionMessage() { WMSExceptionCode = result, WMSMessage = result, ShowAsError = true };
                        }
                        else
                        {
                            _oResponse.Result = "1";
                            lInventoryDTO.Add(_oResponse);
                        }

                        string json = JsonConvert.SerializeObject(WMSCoreEndpointSecurity.PrepareResponse(oRequest.AuthToken, Constants.EndpointConstants.DTO.Inventory, lInventoryDTO));
                        return json;


                    }
                }
                return null;
            }
            catch (WMSExceptionMessage ex)
            {
                List<WMSExceptionMessage> _lstwMSExceptionMessage = new List<WMSExceptionMessage>();
                _lstwMSExceptionMessage.Add(ex);
                string json = JsonConvert.SerializeObject(WMSCoreEndpointSecurity.PrepareResponse(oRequest.AuthToken, Constants.EndpointConstants.DTO.Exception, _lstwMSExceptionMessage));
                return json;
                //  return null;

            }
            catch (Exception excp)
            {
                ExceptionHandling.LogException(excp, _ClassCode + "003");
                return null;
            }

        }

        //[HttpPost]
        //[Route("GetActivestock")]
        //public string GetActivestock(WMSCoreMessage oRequest)
        //{
        //    try
        //    {
        //        LoadControllerDefaults(oRequest);
        //        bool status = false;
        //        InventoryDTO _oResponse = new InventoryDTO();
        //        List<InventoryDTO> _lstActivestock = new List<InventoryDTO>();
        //        if (oRequest != null)
        //        {
        //            List<Inventory> lInventory = new List<Inventory>();
        //            InventoryDTO _oInventory = (InventoryDTO)WMSCoreEndpointSecurity.ValidateRequest(oRequest);


        //            if (_oInventory != null)
        //            {
        //                HouseKeepingBL oHouseKeepingBL = new HouseKeepingBL(LoggedInUserID, ConnectionString);


        //                SearchCriteria oSearchCriteria = new SearchCriteria()
        //                {

        //                    MaterialRSN = _oInventory.RSN,
        //                    ContainerCode = _oInventory.ContainerCode,
        //                    LocationCode=_oInventory.LocationCode,
        //                    MaterialCode=_oInventory.MaterialCode
        //                };

        //                lInventory = oHouseKeepingBL.GetActivestock(oSearchCriteria);
        //                foreach(Inventory oActivestock in lInventory)
        //                {
        //                    InventoryDTO _oActivestockDTO = new InventoryDTO();
        //                    _oActivestockDTO.MaterialCode = oActivestock.MaterialCode;
        //                    _oActivestockDTO.MOP = oActivestock.MOP.ToString();
        //                    _oActivestockDTO.MRP = oActivestock.MRP.ToString();
        //                    _oActivestockDTO.MaterialShortDescription = oActivestock.MaterialShortDescription;
        //                    _oActivestockDTO.LocationCode = oActivestock.DisplayLocationCode;
        //                    _oActivestockDTO.RSN = oActivestock.RSN;
        //                    _oActivestockDTO.Color = oActivestock.Color;
        //                    _lstActivestock.Add(_oActivestockDTO);

        //                }

        //                string json = JsonConvert.SerializeObject(WMSCoreEndpointSecurity.PrepareResponse(oRequest.AuthToken, Constants.EndpointConstants.DTO.Inventory, _lstActivestock));
        //                return json;


        //            }
        //        }
        //        return null;
        //    }
        //    catch (WMSExceptionMessage ex)
        //    {
        //        List<WMSExceptionMessage> _lstwMSExceptionMessage = new List<WMSExceptionMessage>();
        //        _lstwMSExceptionMessage.Add(ex);
        //        string json = JsonConvert.SerializeObject(WMSCoreEndpointSecurity.PrepareResponse(oRequest.AuthToken, Constants.EndpointConstants.DTO.Exception, _lstwMSExceptionMessage));
        //        return json;
        //        //  return null;

        //    }
        //    catch (Exception excp)
        //    {
        //        ExceptionHandling.LogException(excp, _ClassCode + "004");
        //        return null;
        //    }

        //}
        [HttpPost]
        [Route("GetActivestock")]
        public string GetActivestock(WMSCoreMessage oRequest)
        {
            try
            {
                LoadControllerDefaults(oRequest);
                MRLWMSC21Service.UserDataTable userDataTable = new MRLWMSC21Service.UserDataTable();

                List<InventoryDTO> _lstinventory = new List<InventoryDTO>();
                MRLWMSC21Service.LiveStock oiLiveStock = new MRLWMSC21Service.LiveStock();


                if (oRequest != null)
                {
                    List<Inventory> lInventory = new List<Inventory>();
                    InventoryDTO _oInventory = (InventoryDTO)WMSCoreEndpointSecurity.ValidateRequest(oRequest);

                    if (_oInventory != null)
                    {

                        oiLiveStock.Mcode = _oInventory.MaterialCode;
                        oiLiveStock.CartonNo = _oInventory.ContainerCode;
                        oiLiveStock.Location = _oInventory.LocationCode;
                        oiLiveStock.TenantCode = _oInventory.TenantCode;
                        oiLiveStock.BatchNo = _oInventory.BatchNo;
                        oiLiveStock.SerialNo = _oInventory.SerialNo;
                        oiLiveStock.ProjectNo = _oInventory.ProjectNo;
                        oiLiveStock.MfgDate = _oInventory.MfgDate;
                        oiLiveStock.ExpDate = _oInventory.ExpDate;
                        oiLiveStock.MRP = _oInventory.MRP;
                        oiLiveStock.AccountId = Convert.ToInt32(_oInventory.AccountID);
                        oiLiveStock.TenantID = Convert.ToInt32(_oInventory.TenantID);
                        oiLiveStock.WarehouseID = Convert.ToInt32(_oInventory.WarehouseId);
                   

                        MRLWMSC21Service.MRLWMSC21HHTWCFServiceClient oFalcon = new MRLWMSC21Service.MRLWMSC21HHTWCFServiceClient();
                        userDataTable = oFalcon.GetLiveStockData(oiLiveStock);
                    }
                    foreach (DataRow row in userDataTable.Table.Rows)
                    {


                        InventoryDTO responseDTO = new InventoryDTO();                       
                        responseDTO.MaterialCode =  row["Part Number"].ToString();
                        responseDTO.LocationCode= row["Location"].ToString();
                        responseDTO.Quantity = row["AvailableQty"].ToString();
                        responseDTO.SLOC= row["SLOC"].ToString();
                        responseDTO.MfgDate = row["MfgDate"].ToString();
                        responseDTO.ExpDate = row["ExpDate"].ToString();
                        responseDTO.SerialNo = row["SerialNo"].ToString();
                        responseDTO.BatchNo = row["BatchNo"].ToString();
                        responseDTO.ProjectNo = row["ProjectRefNo"].ToString();
                        responseDTO.MRP = row["MRP"].ToString();
                        // responseDTO.HasDisp = row["HasDiscrepancy"].ToString();
                        _lstinventory.Add(responseDTO);
                    }

                    string json = JsonConvert.SerializeObject(WMSCoreEndpointSecurity.PrepareResponse(oRequest.AuthToken, Constants.EndpointConstants.DTO.Inventory, _lstinventory));
                    return json;


                }

                return null;
            }
            catch (WMSExceptionMessage ex)
            {
                List<WMSExceptionMessage> _lstwMSExceptionMessage = new List<WMSExceptionMessage>();
                _lstwMSExceptionMessage.Add(ex);
                string json = JsonConvert.SerializeObject(WMSCoreEndpointSecurity.PrepareResponse(oRequest.AuthToken, Constants.EndpointConstants.DTO.Exception, _lstwMSExceptionMessage));
                return json;
                //  return null;

            }
            catch (Exception excp)
            {
                ExceptionHandling.LogException(excp, _ClassCode + "004");
                return null;
            }

        }

        [HttpPost]
        [Route("GetTenants")]
        public string GetTenants(WMSCoreMessage oRequest)
        {
            try
            {

                LoadControllerDefaults(oRequest);
                MRLWMSC21Service.UserDataTable userDataTable = new MRLWMSC21Service.UserDataTable();

                List<HouseKeepingDTO> _lsthousekeeping= new List<HouseKeepingDTO>();

                if (oRequest != null)
                {

                    HouseKeepingDTO _oHousekeepingDTO = (HouseKeepingDTO)WMSCoreEndpointSecurity.ValidateRequest(oRequest);

                    if (_oHousekeepingDTO != null)
                    {

                        MRLWMSC21Service.LiveStock ooutbound = new MRLWMSC21Service.LiveStock()
                        {

                            AccountId = ConversionUtility.ConvertToInt(_oHousekeepingDTO.AccountID),
                            WarehouseID= ConversionUtility.ConvertToInt(_oHousekeepingDTO.WarehouseId)
                        };


                        MRLWMSC21Service.MRLWMSC21HHTWCFServiceClient oFalcon = new MRLWMSC21Service.MRLWMSC21HHTWCFServiceClient();
                        userDataTable = oFalcon.GetTenants(ooutbound);
                    }
                    foreach (DataRow row in userDataTable.Table.Rows)
                    {
                        HouseKeepingDTO responseDTO = new HouseKeepingDTO();
                        responseDTO.TenantID = row["TenantID"].ToString();
                        responseDTO.TenantName = row["TenantName"].ToString();
                        _lsthousekeeping.Add(responseDTO);
                    }

                    string json = JsonConvert.SerializeObject(WMSCoreEndpointSecurity.PrepareResponse(oRequest.AuthToken, Constants.EndpointConstants.DTO.HouseKeepingDTO, _lsthousekeeping));
                    return json;
                }
                else return null;
            }
            catch (WMSExceptionMessage ex)
            {
                List<WMSExceptionMessage> _lstwMSExceptionMessage = new List<WMSExceptionMessage>();
                _lstwMSExceptionMessage.Add(ex);
                string json = JsonConvert.SerializeObject(WMSCoreEndpointSecurity.PrepareResponse(oRequest.AuthToken, Constants.EndpointConstants.DTO.Exception, _lstwMSExceptionMessage));
                return json;


            }
            catch (Exception excp)
            {
                ExceptionHandling.LogException(excp, _ClassCode + "001");
                return null;
            }

        }



        [HttpPost]
        [Route("GetWarehouse")]
        public string GetWarehouse(WMSCoreMessage oRequest)
        {
            try
            {

                LoadControllerDefaults(oRequest);
                MRLWMSC21Service.UserDataTable userDataTable = new MRLWMSC21Service.UserDataTable();

                List<HouseKeepingDTO> _lsthousekeeping = new List<HouseKeepingDTO>();

                if (oRequest != null)
                {

                    HouseKeepingDTO _oHousekeepingDTO = (HouseKeepingDTO)WMSCoreEndpointSecurity.ValidateRequest(oRequest);

                    if (_oHousekeepingDTO != null)
                    {

                        MRLWMSC21Service.LiveStock ooutbound = new MRLWMSC21Service.LiveStock()
                        {

                            AccountId = ConversionUtility.ConvertToInt(_oHousekeepingDTO.AccountID),
                            TenantID= ConversionUtility.ConvertToInt(_oHousekeepingDTO.TenantID),
                            UserId = ConversionUtility.ConvertToInt(_oHousekeepingDTO.UserId)
                        };


                        MRLWMSC21Service.MRLWMSC21HHTWCFServiceClient oFalcon = new MRLWMSC21Service.MRLWMSC21HHTWCFServiceClient();
                        userDataTable = oFalcon.GetWarehouse(ooutbound);
                    }
                    foreach (DataRow row in userDataTable.Table.Rows)
                    {
                        HouseKeepingDTO responseDTO = new HouseKeepingDTO();
                        responseDTO.Warehouse = row["WHCode"].ToString();
                        responseDTO.WarehouseId = row["WarehouseID"].ToString();
                        _lsthousekeeping.Add(responseDTO);
                    }

                    string json = JsonConvert.SerializeObject(WMSCoreEndpointSecurity.PrepareResponse(oRequest.AuthToken, Constants.EndpointConstants.DTO.HouseKeepingDTO, _lsthousekeeping));
                    return json;
                }
                else return null;
            }
            catch (WMSExceptionMessage ex)
            {
                List<WMSExceptionMessage> _lstwMSExceptionMessage = new List<WMSExceptionMessage>();
                _lstwMSExceptionMessage.Add(ex);
                string json = JsonConvert.SerializeObject(WMSCoreEndpointSecurity.PrepareResponse(oRequest.AuthToken, Constants.EndpointConstants.DTO.Exception, _lstwMSExceptionMessage));
                return json;


            }
            catch (Exception excp)
            {
                ExceptionHandling.LogException(excp, _ClassCode + "001");
                return null;
            }

        }

        [HttpPost]
        [Route("GetActivestockWithOutRSN")]
        public string GetActivestockWithOutRSN(WMSCoreMessage oRequest)
        {
            try
            {
                LoadControllerDefaults(oRequest);
                bool status = false;
                InventoryDTO _oResponse = new InventoryDTO();
                List<InventoryDTO> _lstActivestock = new List<InventoryDTO>();
                if (oRequest != null)
                {
                    List<Inventory> lInventory = new List<Inventory>();
                    InventoryDTO _oInventory = (InventoryDTO)WMSCoreEndpointSecurity.ValidateRequest(oRequest);


                    if (_oInventory != null)
                    {
                        HouseKeepingBL oHouseKeepingBL = new HouseKeepingBL(LoggedInUserID, ConnectionString);
                        string batchNumeber = string.Empty;
                        if(_oInventory.ReferenceDocumentNumber!=null)
                            batchNumeber=oHouseKeepingBL.GetBatchNumberByJOBRefNumber(_oInventory.ReferenceDocumentNumber);
                        SearchCriteria oSearchCriteria = new SearchCriteria()
                        {

                            MaterialRSN = _oInventory.RSN,
                            ContainerCode = _oInventory.ContainerCode,
                            LocationCode = _oInventory.LocationCode,
                            MaterialCode = _oInventory.MaterialCode,
                            BatchNumber = batchNumeber
                            
                        };

                        lInventory = oHouseKeepingBL.GetActivestockWithOutRSN(oSearchCriteria);
                        foreach (Inventory oActivestock in lInventory)
                        {
                            InventoryDTO _oActivestockDTO = new InventoryDTO();
                            _oActivestockDTO.MaterialCode = oActivestock.MaterialCode;
                            _oActivestockDTO.MOP = oActivestock.MOP.ToString();
                            _oActivestockDTO.MRP = oActivestock.MRP.ToString();
                            _oActivestockDTO.MaterialShortDescription = oActivestock.MaterialShortDescription;
                            _oActivestockDTO.LocationCode = oActivestock.DisplayLocationCode;
                            _oActivestockDTO.Quantity = oActivestock.Quantity.ToString().Split('.')[0];
                            _oActivestockDTO.Color = oActivestock.Color;
                            _lstActivestock.Add(_oActivestockDTO);

                        }

                        string json = JsonConvert.SerializeObject(WMSCoreEndpointSecurity.PrepareResponse(oRequest.AuthToken, Constants.EndpointConstants.DTO.Inventory, _lstActivestock));
                        return json;


                    }
                }
                return null;
            }
            catch (WMSExceptionMessage ex)
            {
                List<WMSExceptionMessage> _lstwMSExceptionMessage = new List<WMSExceptionMessage>();
                _lstwMSExceptionMessage.Add(ex);
                string json = JsonConvert.SerializeObject(WMSCoreEndpointSecurity.PrepareResponse(oRequest.AuthToken, Constants.EndpointConstants.DTO.Exception, _lstwMSExceptionMessage));
                return json;
                //  return null;

            }
            catch (Exception excp)
            {
                ExceptionHandling.LogException(excp, _ClassCode + "004");
                return null;
            }

        }

        [HttpPost]
        [Route("CheckTenatMaterial")]
        public string CheckTenatMaterial(WMSCoreMessage oRequest)
        {
            try
            {
                LoadControllerDefaults(oRequest);
                string result = null;
                InventoryDTO _oResponse = new InventoryDTO();
                if (oRequest != null)
                {
                    List<InventoryDTO> lInventoryDTO = new List<InventoryDTO>();
                    InventoryDTO _oInventory = (InventoryDTO)WMSCoreEndpointSecurity.ValidateRequest(oRequest);


                    if (_oInventory != null)
                    {



                        MRLWMSC21Service.MRLWMSC21HHTWCFServiceClient oFalcon = new MRLWMSC21Service.MRLWMSC21HHTWCFServiceClient();
                        result = oFalcon.CheckTenatMaterial(_oInventory.MaterialCode, ConversionUtility.ConvertToInt(_oInventory.AccountID),_oInventory.TenantCode);
                        if (result != null && result != "")
                        {
                            throw new WMSExceptionMessage() { WMSExceptionCode = result, WMSMessage = result, ShowAsError = true };
                        }
                        else
                        {
                            _oResponse.Result = "1";
                            lInventoryDTO.Add(_oResponse);
                        }

                        string json = JsonConvert.SerializeObject(WMSCoreEndpointSecurity.PrepareResponse(oRequest.AuthToken, Constants.EndpointConstants.DTO.Inventory, lInventoryDTO));
                        return json;


                    }
                }
                return null;
            }
            catch (WMSExceptionMessage ex)
            {
                List<WMSExceptionMessage> _lstwMSExceptionMessage = new List<WMSExceptionMessage>();
                _lstwMSExceptionMessage.Add(ex);
                string json = JsonConvert.SerializeObject(WMSCoreEndpointSecurity.PrepareResponse(oRequest.AuthToken, Constants.EndpointConstants.DTO.Exception, _lstwMSExceptionMessage));
                return json;
                //  return null;

            }
            catch (Exception excp)
            {
                ExceptionHandling.LogException(excp, _ClassCode + "003");
                return null;
            }

        }


        [HttpPost]
        [Route("ValidateCartonForLiveStock")]
        public string ValidateCartonForLiveStock(WMSCoreMessage oRequest)
        {
            try
            {

                LoadControllerDefaults(oRequest);
                string Result ="";

                List<HouseKeepingDTO> _lsthousekeeping = new List<HouseKeepingDTO>();

                if (oRequest != null)
                {

                    HouseKeepingDTO _oHousekeepingDTO = (HouseKeepingDTO)WMSCoreEndpointSecurity.ValidateRequest(oRequest);

                    if (_oHousekeepingDTO != null)
                    {

                        MRLWMSC21Service.LiveStock liveStock = new MRLWMSC21Service.LiveStock()
                        {

                            WarehouseID = ConversionUtility.ConvertToInt(_oHousekeepingDTO.WarehouseId),
                            CartonNo = _oHousekeepingDTO.CartonNo
                        };


                        MRLWMSC21Service.MRLWMSC21HHTWCFServiceClient oFalcon = new MRLWMSC21Service.MRLWMSC21HHTWCFServiceClient();
                        Result = oFalcon.ValidateCartonLiveStock(liveStock);
                    }

                    if (Result == "1")
                    {
                        _oHousekeepingDTO.Result = Result;
                        _lsthousekeeping.Add(_oHousekeepingDTO);
                    }
                    else
                    {
                        throw new WMSExceptionMessage() { WMSExceptionCode = "", WMSMessage = Result, ShowAsError = true };
                    }
                    

                    string json = JsonConvert.SerializeObject(WMSCoreEndpointSecurity.PrepareResponse(oRequest.AuthToken, Constants.EndpointConstants.DTO.HouseKeepingDTO, _lsthousekeeping));
                    return json;
                }
                else return null;
            }
            catch (WMSExceptionMessage ex)
            {
                List<WMSExceptionMessage> _lstwMSExceptionMessage = new List<WMSExceptionMessage>();
                _lstwMSExceptionMessage.Add(ex);
                string json = JsonConvert.SerializeObject(WMSCoreEndpointSecurity.PrepareResponse(oRequest.AuthToken, Constants.EndpointConstants.DTO.Exception, _lstwMSExceptionMessage));
                return json;


            }
            catch (Exception excp)
            {
                ExceptionHandling.LogException(excp, _ClassCode + "001");
                return null;
            }

        }

    }
}