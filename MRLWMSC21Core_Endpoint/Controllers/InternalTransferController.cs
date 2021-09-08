using System.Linq;
using System.Web.Http;
using MRLWMSC21Core.Business;
using MRLWMSC21Core.Entities;
using MRLWMSC21Core.Library;
using MRLWMSC21_Endpoint.Interfaces;
using MRLWMSC21_Endpoint.Security;
using MRLWMSC21_Endpoint.DTO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;

namespace MRLWMSC21_Endpoint.Controllers
{
    [RoutePrefix("Transfers")]
    public class InternalTransferController : BaseController, iInternaltransfer
    {
        private string _ClassCode = string.Empty;

        public InternalTransferController()
        {
            _ClassCode = ExceptionHandling.GetClassExceptionCode(ExceptionHandling.ExcpConstants_API_Enpoint.InternalTransferController);
        }

        
        [HttpPost]
        [Route("UpdateInternalTransfer")]
        public string UpdateInternalTransfer(WMSCoreMessage oRequest)
        {
            try

            {
                List<InventoryDTO> _lInventoryDTO = new List<InventoryDTO>();

                InventoryDTO _oResponseDTO = new InventoryDTO();
                if (oRequest != null)
                {

                    LoadControllerDefaults(oRequest);
                    InventoryDTO _oInventoryDTO = (InventoryDTO)WMSCoreEndpointSecurity.ValidateRequest(oRequest);
                    GoodsMovement oGoodsMovement = new GoodsMovement();

                    if (_oInventoryDTO != null)
                    {
                        HouseKeepingBL oHouseKeepingBL = new HouseKeepingBL(LoggedInUserID, ConnectionString);
                        HouseKeepingJob houseKeepingJob = new HouseKeepingJob()
                        {
                            TransferRequestNumber = _oInventoryDTO.ReferenceDocumentNumber
                        };
                        Inventory inventory = new Inventory(_oInventoryDTO.RSN);
                        inventory.LocationCode = _oInventoryDTO.LocationCode;


                        oGoodsMovement = oHouseKeepingBL.TransferItemBinToBin(houseKeepingJob, inventory, inventory.LocationCode, _oInventoryDTO.ToLocationCode);
                        if (oGoodsMovement != null)
                        {
                            _oResponseDTO.MaterialShortDescription = oGoodsMovement.MaterialDescription;

                        }
                        _lInventoryDTO.Add(_oResponseDTO);


                    }
                    else return null;

                }
                string json = JsonConvert.SerializeObject(WMSCoreEndpointSecurity.PrepareResponse(oRequest.AuthToken, Constants.EndpointConstants.DTO.Inventory, _lInventoryDTO));
                return json;
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
        [Route("ValidateTransferLocationCode")]
        public string ValidateTransferLocationCode(WMSCoreMessage oRequest)
        {
            try
            {
                LoadControllerDefaults(oRequest);
                bool status = false;
                InventoryDTO _oResponse = new InventoryDTO();
                List<InventoryDTO> _lInventoryDTO = new List<InventoryDTO>();
                if (oRequest != null)
                {
                    List<Inventory> lInventory = new List<Inventory>();
                    InventoryDTO _oInventory = (InventoryDTO)WMSCoreEndpointSecurity.ValidateRequest(oRequest);


                    if (_oInventory != null)
                    {
                        HouseKeepingBL oHouseKeepingBL = new HouseKeepingBL(LoggedInUserID, ConnectionString);
                        InboundBL oInboundBL = new InboundBL(LoggedInUserID, ConnectionString);

                        Location oRequestLocation = new Location()
                        {

                            LocationCode = _oInventory.LocationCode
                        };

                        status = oInboundBL.ValidateLocationCode(oRequestLocation, MRLWMSC21Core.Library.Constants.LocationType.DockOrBin, false);
                        if (status)
                        {
                            _oResponse.Result = "1";
                        }
                        else
                        {
                            _oResponse.Result = "0";
                        }

                        _lInventoryDTO.Add(_oResponse);
                        string json = JsonConvert.SerializeObject(WMSCoreEndpointSecurity.PrepareResponse(oRequest.AuthToken, Constants.EndpointConstants.DTO.Inventory, _lInventoryDTO));
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
                ExceptionHandling.LogException(excp, _ClassCode + "002");
                return null;
            }

        }

        [HttpPost]
        [Route("GetTransferReqNos")]
        public string GetTransferReqNos(WMSCoreMessage oRequest)
        {
            try
            {

                LoadControllerDefaults(oRequest);


               MRLWMSC21Service.UserDataTable userDataTable = new MRLWMSC21Service.UserDataTable();

                List<InventoryDTO> _lstinventory = new List<InventoryDTO>();

                if (oRequest != null)
                {

                    InventoryDTO _oinventoryDTO = (InventoryDTO)WMSCoreEndpointSecurity.ValidateRequest(oRequest);

                    if (_oinventoryDTO != null)
                    {

                       MRLWMSC21Service.TransferBO oTransfer = new MRLWMSC21Service.TransferBO()
                        {

                            AccountId = ConversionUtility.ConvertToInt(_oinventoryDTO.AccountID)
                        };


                       MRLWMSC21Service.MRLWMSC21HHTWCFServiceClient oFalcon = new MRLWMSC21Service.MRLWMSC21HHTWCFServiceClient();
                        userDataTable = oFalcon.GetTransferOrderNos(oTransfer);
                    }
                    foreach (DataRow row in userDataTable.Table.Rows)
                    {
                        InventoryDTO responseDTO = new InventoryDTO();
                        responseDTO.TransferRefNo = row["TransferRequestNumber"].ToString();
                        responseDTO.TransferRefId = Convert.ToInt32(row["TransferRequestID"]);
                        _lstinventory.Add(responseDTO);
                    }

                    string json = JsonConvert.SerializeObject(WMSCoreEndpointSecurity.PrepareResponse(oRequest.AuthToken, Constants.EndpointConstants.DTO.Inventory, _lstinventory));
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
                ExceptionHandling.LogException(excp, _ClassCode + "003");
                return null;
            }

        }

        [HttpPost]
        [Route("GetAvailbleQtyList")]
        public string GetAvailbleQtyList(WMSCoreMessage oRequest)
        {
            try
            {

                LoadControllerDefaults(oRequest);
               MRLWMSC21Service.TransferBO oresponseTransfer = new MRLWMSC21Service.TransferBO();
                InventoryDTO oInventoryDTO = new InventoryDTO();
                List<InventoryDTO> _lstinventory = new List<InventoryDTO>();
                if (oRequest != null)
                {
                    InventoryDTO _oinventoryDTO = (InventoryDTO)WMSCoreEndpointSecurity.ValidateRequest(oRequest);
                    if (_oinventoryDTO != null)
                    {

                       MRLWMSC21Service.TransferBO oTransfer = new MRLWMSC21Service.TransferBO()
                        {

                            AccountId = ConversionUtility.ConvertToInt(_oinventoryDTO.AccountID),
                            MCode = _oinventoryDTO.MaterialCode,
                            FromSLoc = _oinventoryDTO.SLOC,
                            FromLocation = _oinventoryDTO.LocationCode,
                            FromCartonNo = _oinventoryDTO.ContainerCode,
                            MfgDate = _oinventoryDTO.MfgDate,
                            ExpDate = _oinventoryDTO.ExpDate,
                            SerialNo = _oinventoryDTO.SerialNo,
                            BatchNo = _oinventoryDTO.BatchNo,
                            ProjectNo = _oinventoryDTO.ProjectNo,
                            MRP = _oinventoryDTO.MRP,
                            TenantID = _oinventoryDTO.TenantID,
                            WarehouseID = _oinventoryDTO.WarehouseId
                        };


                       MRLWMSC21Service.MRLWMSC21HHTWCFServiceClient oFalcon = new MRLWMSC21Service.MRLWMSC21HHTWCFServiceClient();
                        oresponseTransfer = oFalcon.GetAvailbleQtyListforHHT(oTransfer);

                        if (oresponseTransfer != null)
                        {
                            if (oresponseTransfer.AvailQty == "0")
                            {
                                throw new WMSExceptionMessage() { WMSExceptionCode = null, WMSMessage = "No stock available in the Location", ShowAsError = true };
                            }

                            else  if (oresponseTransfer.AvailQty == "-1")
                            {
                                throw new WMSExceptionMessage() { WMSExceptionCode = null, WMSMessage = "Material does not belong to the Tenant", ShowAsError = true };
                            }
                            oInventoryDTO.Quantity = oresponseTransfer.AvailQty;
                           // oInventoryDTO.SLOC = oresponseTransfer.FromSLoc;
                        }

                        _lstinventory.Add(oInventoryDTO);

                    }


                    string json = JsonConvert.SerializeObject(WMSCoreEndpointSecurity.PrepareResponse(oRequest.AuthToken, Constants.EndpointConstants.DTO.Inventory, _lstinventory));
                    return json;
                }
                else
                    return null;
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
                ExceptionHandling.LogException(excp, _ClassCode + "004");
                return null;
            }

        }

        [HttpPost]
        [Route("ChekContainerLocation")]
        public string ChekContainerLocation(WMSCoreMessage oRequest)
        { 
            try
            {
                LoadControllerDefaults(oRequest);

                string LocationCode = "",LocationCheck="";
                InventoryDTO oinventoryDTO = new InventoryDTO();
               MRLWMSC21Service.TransferBO oresponseTransfer = new MRLWMSC21Service.TransferBO();
                List<InventoryDTO> _lstinventory = new List<InventoryDTO>();
                if (oRequest != null)
                {
                    InventoryDTO _oinventoryDTO = (InventoryDTO)WMSCoreEndpointSecurity.ValidateRequest(oRequest);
                   MRLWMSC21Service.MRLWMSC21HHTWCFServiceClient oFalcon = new MRLWMSC21Service.MRLWMSC21HHTWCFServiceClient();
                    if (_oinventoryDTO != null)
                    {
                        
                        LocationCheck = _oinventoryDTO.LocationCode;
                        LocationCode = oFalcon.GetConatinerLocationBin(_oinventoryDTO.ContainerCode,_oinventoryDTO.WarehouseId);
                    }
                    if (LocationCode != null || LocationCode != string.Empty)
                    {
                       if(LocationCheck == LocationCode)
                        {
                           
                           MRLWMSC21Service.TransferBO oTransfer = new MRLWMSC21Service.TransferBO()
                            {

                                AccountId = ConversionUtility.ConvertToInt(_oinventoryDTO.AccountID),
                                MCode = "",
                                FromSLoc ="",
                                FromLocation = _oinventoryDTO.LocationCode,
                                FromCartonNo = _oinventoryDTO.ContainerCode,
                                MfgDate ="",
                                ExpDate ="",
                                SerialNo ="",
                                BatchNo ="",
                                ProjectNo = "",
                                MRP = "",
                                TenantID = _oinventoryDTO.TenantID,
                                WarehouseID = _oinventoryDTO.WarehouseId
                            };
                            oresponseTransfer = oFalcon.GetAvailbleQtyListforHHT(oTransfer);
                            if(oresponseTransfer!=null)
                            oinventoryDTO.Result = "1";                           
                            oinventoryDTO.Quantity = oresponseTransfer.AvailQty;
                            //oinventoryDTO.SLOC = oresponseTransfer.FromSLoc;
                        }
                        else
                        {

                             if (LocationCode == "-1")
                            {
                                throw new WMSExceptionMessage() { WMSExceptionCode = null, WMSMessage = "Container does not exist in the Warehouse", ShowAsError = true };
                            }
                            else if (LocationCode != "" && LocationCode != "1") {
                                throw new WMSExceptionMessage() { WMSExceptionCode = null, WMSMessage = "Container does not exist in the Location", ShowAsError = true };
                            }
                           
                            else
                            {
                                oinventoryDTO.Result = "1";
                                oinventoryDTO.Quantity = oresponseTransfer.AvailQty;
                                oinventoryDTO.SLOC = oresponseTransfer.FromSLoc;
                            }
                            
                        }
                    }
                    _lstinventory.Add(oinventoryDTO);

                    string json = JsonConvert.SerializeObject(WMSCoreEndpointSecurity.PrepareResponse(oRequest.AuthToken, Constants.EndpointConstants.DTO.Inventory, _lstinventory));
                    return json;
                }
                else
                    return null;
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
                ExceptionHandling.LogException(excp, _ClassCode + "005");
                return null;
            }
        }

        [HttpPost]
        [Route("UpsertBinToBinTransfer")]
        public string UpsertBinToBinTransfer(WMSCoreMessage oRequest)
        {
            try
            {
                LoadControllerDefaults(oRequest);
               MRLWMSC21Service.TransferBO oresponseTransfer = new MRLWMSC21Service.TransferBO();
                InventoryDTO oInventoryDTO = new InventoryDTO();
                List<InventoryDTO> _lstinventory = new List<InventoryDTO>();
                if (oRequest != null)
                {
                    InventoryDTO _oinventoryDTO = (InventoryDTO)WMSCoreEndpointSecurity.ValidateRequest(oRequest);
                    if (_oinventoryDTO != null)
                    {

                       MRLWMSC21Service.TransferBO oTransfer = new MRLWMSC21Service.TransferBO()
                        {
                            FromLocation = _oinventoryDTO.LocationCode,
                            MfgDate = _oinventoryDTO.MfgDate,
                            ExpDate = _oinventoryDTO.ExpDate,
                            SerialNo = _oinventoryDTO.SerialNo,
                            BatchNo = _oinventoryDTO.BatchNo,
                            ProjectNo = _oinventoryDTO.ProjectNo,
                            UserId = _oinventoryDTO.UserId,
                            FromCartonNo = _oinventoryDTO.ContainerCode,
                            MCode = _oinventoryDTO.MaterialCode,
                            ToLocation = _oinventoryDTO.ToLocationCode,
                            TransferQty = _oinventoryDTO.Quantity,
                            FromSLoc = _oinventoryDTO.SLOC,
                            ToSLoc = _oinventoryDTO.SLOC,
                            ToCartonNo = _oinventoryDTO.ContainerCode

                        };


                       MRLWMSC21Service.MRLWMSC21HHTWCFServiceClient oFalcon = new MRLWMSC21Service.MRLWMSC21HHTWCFServiceClient();
                        oresponseTransfer = oFalcon.UpsertBinToBinTransferItem(oTransfer);

                        if (oresponseTransfer != null)
                        {
                            oInventoryDTO.Quantity = oresponseTransfer.AvailQty;
                            oInventoryDTO.SLOC = oresponseTransfer.FromSLoc;
                        }

                        _lstinventory.Add(oInventoryDTO);

                    }


                    string json = JsonConvert.SerializeObject(WMSCoreEndpointSecurity.PrepareResponse(oRequest.AuthToken, Constants.EndpointConstants.DTO.Inventory, _lstinventory));
                    return json;
                }
                else
                    return null;
            }
            catch (Exception excp)
            {
                ExceptionHandling.LogException(excp, _ClassCode + "006");
                return null;
            }
        }

        [HttpPost]
        [Route("UpsertBinToBinTransferItem")]
        public string UpsertBinToBinTransferItem(WMSCoreMessage oRequest)
        {
            try
            {
                LoadControllerDefaults(oRequest);
               MRLWMSC21Service.TransferBO oresponseTransfer = new MRLWMSC21Service.TransferBO();
                InventoryDTO oInventoryDTO = new InventoryDTO();
                List<InventoryDTO> _lstinventory = new List<InventoryDTO>();
                if (oRequest != null)
                {
                    InventoryDTO _oinventoryDTO = (InventoryDTO)WMSCoreEndpointSecurity.ValidateRequest(oRequest);
                    if (_oinventoryDTO != null)
                    {

                       MRLWMSC21Service.TransferBO oTransfer = new MRLWMSC21Service.TransferBO()
                        {
                            FromLocation = _oinventoryDTO.LocationCode,
                            MfgDate = _oinventoryDTO.MfgDate,
                            ExpDate = _oinventoryDTO.ExpDate,
                            SerialNo = _oinventoryDTO.SerialNo,
                            BatchNo = _oinventoryDTO.BatchNo,
                            ProjectNo = _oinventoryDTO.ProjectNo,                          
                            UserId = _oinventoryDTO.UserId,
                            FromCartonNo = _oinventoryDTO.ContainerCode,
                            MCode = _oinventoryDTO.MaterialCode,
                            ToLocation = _oinventoryDTO.ToLocationCode,
                            TransferQty = _oinventoryDTO.Quantity,
                            FromSLoc = _oinventoryDTO.SLOC,
                            ToSLoc = _oinventoryDTO.SLOC,
                            ToCartonNo = _oinventoryDTO.ToContainerCode,
                            WarehouseID = _oinventoryDTO.WarehouseId,
                            TenantID = _oinventoryDTO.TenantID,
                            MRP = _oinventoryDTO.MRP

                        };


                       MRLWMSC21Service.MRLWMSC21HHTWCFServiceClient oFalcon = new MRLWMSC21Service.MRLWMSC21HHTWCFServiceClient();
                        oresponseTransfer = oFalcon.UpsertBinToBinTransferItem(oTransfer);

                        if (oresponseTransfer != null)
                        {
                            oInventoryDTO.Result = oresponseTransfer.Result;
                        }

                        _lstinventory.Add(oInventoryDTO);

                    }


                    string json = JsonConvert.SerializeObject(WMSCoreEndpointSecurity.PrepareResponse(oRequest.AuthToken, Constants.EndpointConstants.DTO.Inventory, _lstinventory));
                    return json;
                }
                else
                    return null;
            }
            catch (Exception excp)
            {
                ExceptionHandling.LogException(excp, _ClassCode + "006");
                return null;
            }
        }

        [HttpPost]
        [Route("GetBinToBinStorageLocations")]
        public string GetBinToBinStorageLocations(WMSCoreMessage oRequest)
        {
            try
            {
                LoadControllerDefaults(oRequest);
               MRLWMSC21Service.TransferBO oresponseTransfer = new MRLWMSC21Service.TransferBO();
                List<InventoryDTO> _lstinventory = new List<InventoryDTO>();
                if (oRequest != null)
                {
                    InventoryDTO _oinventoryDTO = (InventoryDTO)WMSCoreEndpointSecurity.ValidateRequest(oRequest);
                    if (_oinventoryDTO != null)
                    {
                       MRLWMSC21Service.MRLWMSC21HHTWCFServiceClient oFalcon = new MRLWMSC21Service.MRLWMSC21HHTWCFServiceClient();
                        MRLWMSC21Service.UserDataTable _userDataTable = oFalcon.GetBinToBinStorageLocations();


                        if (_userDataTable.Table.Rows.Count > 0)
                        {
                            _lstinventory = (from DataRow dr in _userDataTable.Table.Rows
                                             select new InventoryDTO
                                             {
                                                 LocationCode = dr[0].ToString()
                                             }).ToList();
                        }

                    }

                    string json = JsonConvert.SerializeObject(WMSCoreEndpointSecurity.PrepareResponse(oRequest.AuthToken, Constants.EndpointConstants.DTO.Inventory, _lstinventory));
                    return json;
                }
                else
                    return null;
            }
            catch (Exception excp)
            {
                ExceptionHandling.LogException(excp, _ClassCode + "006");
                return null;
            }
        }








        [HttpPost]
        [Route("TransferPalletToLocation")]
        public string TransferPalletToLocation(WMSCoreMessage oRequest)
        {
            try
            {
                LoadControllerDefaults(oRequest);
                
                if (oRequest != null)
                {
                    InventoryDTO _oinventoryDTO = (InventoryDTO)WMSCoreEndpointSecurity.ValidateRequest(oRequest);
                    Inventory _oInventory = new Inventory();
                    if (_oinventoryDTO != null)
                    {
                        
                        _oInventory.ContainerCode = _oinventoryDTO.ContainerCode;
                        _oInventory.LocationCode = _oinventoryDTO.LocationCode;
                        _oInventory.WarehouseID = Convert.ToInt32(_oinventoryDTO.WarehouseId);
                        _oInventory.TenantID = Convert.ToInt32(_oinventoryDTO.TenantID);

                        MRLWMSC21Core.Business.InventoryBL oinventoryBL = new MRLWMSC21Core.Business.InventoryBL(LoggedInUserID, ConnectionString);
                        oinventoryBL.TransferPallettoLocation(_oInventory);

                    }

                    string json = JsonConvert.SerializeObject(WMSCoreEndpointSecurity.PrepareResponse(oRequest.AuthToken, Constants.EndpointConstants.DTO.Inventory, _oInventory));
                    return json;
                }
                else
                    return null;
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
                ExceptionHandling.LogException(excp, _ClassCode + "010");
                return null;
            }



        }



        [HttpPost]
        [Route("GetSLocWiseActiveStockInfo")]
        public string GetSLocWiseActiveStockInfo(WMSCoreMessage oRequest)
        {
            try
            {
                LoadControllerDefaults(oRequest);

                if (oRequest != null)
                {
                    InventoryDTO _oinventoryDTO = (InventoryDTO)WMSCoreEndpointSecurity.ValidateRequest(oRequest);
                    Inventory _oInventory = new Inventory();
                    List<InventoryDTO> _lstInventoryDTO = new List<InventoryDTO>();
                    if (_oinventoryDTO != null)
                    {

                        _oInventory.ContainerCode = _oinventoryDTO.ContainerCode;
                        _oInventory.LocationCode = _oinventoryDTO.LocationCode;
                        _oInventory.MaterialCode = _oinventoryDTO.MaterialCode;
                        _oInventory.WarehouseID = Convert.ToInt32(_oinventoryDTO.WarehouseId);
                        _oInventory.TenantID = Convert.ToInt32(_oinventoryDTO.TenantID);
                        _oInventory.BatchNumber = _oinventoryDTO.BatchNo;
                        _oInventory.SerialNo = _oinventoryDTO.SerialNo;
                        _oInventory.Mfg_Date = _oinventoryDTO.MfgDate;
                        _oInventory.ExpDate= _oinventoryDTO.ExpDate;
                        _oInventory.ProjectRefNo = _oinventoryDTO.ProjectNo;
                        _oInventory.MRP =ConversionUtility.ConvertToDecimal(_oinventoryDTO.MRP);

                        MRLWMSC21Core.Business.InventoryBL oinventoryBL = new MRLWMSC21Core.Business.InventoryBL(LoggedInUserID, ConnectionString);

                        List<Inventory> _listInvetory = new List<Inventory>();


                       _listInvetory= oinventoryBL.GetSlocWiseActiveStock(_oInventory);


                        foreach(Inventory inventory in _listInvetory)
                        {
                            InventoryDTO inventoryDTO = new InventoryDTO();

                            inventoryDTO.StorageLocation = inventory.StorageLocation.ToString();
                            inventoryDTO.Quantity = inventory.AvailableQuantity.ToString();
                            _lstInventoryDTO.Add(inventoryDTO);
                        }
                    }

                    string json = JsonConvert.SerializeObject(WMSCoreEndpointSecurity.PrepareResponse(oRequest.AuthToken, Constants.EndpointConstants.DTO.Inventory, _lstInventoryDTO));
                    return json;
                }
                else
                    return null;
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
                ExceptionHandling.LogException(excp, _ClassCode + "011");
                return null;
            }
        }



        [HttpPost]
        [Route("UpdateMaterialTransfer")]
        public string UpdateMaterialTransfer(WMSCoreMessage oRequest)
        {
            try
            {
                LoadControllerDefaults(oRequest);

                if (oRequest != null)
                {
                    InventoryDTO _oinventoryDTO = (InventoryDTO)WMSCoreEndpointSecurity.ValidateRequest(oRequest);
                    Inventory _oInventory = new Inventory();
                    List<InventoryDTO> _lstInventoryDTO = new List<InventoryDTO>();
                    if (_oinventoryDTO != null)
                    {

                        _oInventory.ContainerCode = _oinventoryDTO.ContainerCode;
                        _oInventory.LocationCode = _oinventoryDTO.LocationCode;
                        _oInventory.MaterialCode = _oinventoryDTO.MaterialCode;
                        _oInventory.WarehouseID = Convert.ToInt32(_oinventoryDTO.WarehouseId);
                        _oInventory.TenantID = Convert.ToInt32(_oinventoryDTO.TenantID);
                        _oInventory.BatchNumber = _oinventoryDTO.BatchNo;
                        _oInventory.Quantity = ConversionUtility.ConvertToDecimal(_oinventoryDTO.Quantity);
                        _oInventory.StorageLocation = _oinventoryDTO.StorageLocation;
                        _oInventory.ToStorageLocation = _oinventoryDTO.ToStorageLocation;
                        _oInventory.UserId = _oinventoryDTO.UserId;
                        

                        MRLWMSC21Core.Business.InventoryBL oinventoryBL = new MRLWMSC21Core.Business.InventoryBL(LoggedInUserID, ConnectionString);

                        List<Inventory> _listInvetory = new List<Inventory>();


                        _listInvetory = oinventoryBL.UpdateMaterialTrasnfer(_oInventory);


                        foreach (Inventory inventory in _listInvetory)
                        {
                            InventoryDTO inventoryDTO = new InventoryDTO();

                            inventoryDTO.Result = inventory.Result.ToString();
                            
                            _lstInventoryDTO.Add(inventoryDTO);
                        }
                    }

                    string json = JsonConvert.SerializeObject(WMSCoreEndpointSecurity.PrepareResponse(oRequest.AuthToken, Constants.EndpointConstants.DTO.Inventory, _lstInventoryDTO));
                    return json;
                }
                else
                    return null;
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
                ExceptionHandling.LogException(excp, _ClassCode + "012");
                return null;
            }
        }






    }
}