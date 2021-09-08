using MRLWMSC21Core.Library;
using MRLWMSC21_Endpoint.DTO;
using MRLWMSC21_Endpoint.Interfaces;
using MRLWMSC21_Endpoint.Security;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MRLWMSC21_Endpoint.Controllers
{
    [RoutePrefix("POD")]
    public class PODController : BaseController, iPOD
    {
        private string _ClassCode = string.Empty;

        public PODController()
        {
            _ClassCode = ExceptionHandling.GetClassExceptionCode(ExceptionHandling.ExcpConstants_API_Enpoint.PODController);
        }

        [HttpPost]
        [Route("GetUserDetail")]
        public string GetUserDetail(WMSCoreMessage oRequest)
        {
            try
            {

                LoadControllerDefaults(oRequest);
                MRLWMSC21Service.PODBO _oPODBO = new MRLWMSC21Service.PODBO();

                List<PODDTO> _lstcyclecount = new List<PODDTO>();

                if (oRequest != null)
                {

                    PODDTO _oPODDTO = (PODDTO)WMSCoreEndpointSecurity.ValidateRequest(oRequest);

                    if (_oPODDTO != null)
                    {

                        MRLWMSC21Service.MRLWMSC21HHTWCFServiceClient oFalcon = new MRLWMSC21Service.MRLWMSC21HHTWCFServiceClient();
                        _oPODBO = oFalcon.GetUserDetail(_oPODDTO.UserName, _oPODDTO.Password);
                    }
                    PODDTO _responsepODDTO = ConvertJson<PODDTO>(_oPODBO);
                    string json = JsonConvert.SerializeObject(WMSCoreEndpointSecurity.PrepareResponse(oRequest.AuthToken, Constants.EndpointConstants.DTO.POD, _responsepODDTO));
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
        [Route("GetJobOrderList")]
        public string GetJobOrderList(WMSCoreMessage oRequest)
        {
            try
            {

                LoadControllerDefaults(oRequest);
                MRLWMSC21Service.PODBO[] _lstPODBO = null;// = new MRLWMSC21Service.PODBO[];

                List<PODDTO> _lstcyclecount = new List<PODDTO>();

                if (oRequest != null)
                {

                    PODDTO _oPODDTO = (PODDTO)WMSCoreEndpointSecurity.ValidateRequest(oRequest);

                    if (_oPODDTO != null)
                    {

                        MRLWMSC21Service.MRLWMSC21HHTWCFServiceClient oFalcon = new MRLWMSC21Service.MRLWMSC21HHTWCFServiceClient();
                        _lstPODBO = oFalcon.GetJobOrderList(ConversionUtility.ConvertToInt(_oPODDTO.Limit));


                    }
                    List<PODDTO> _responsepODDTO = ConvertListJson<PODDTO>(_lstPODBO);
                    string json = JsonConvert.SerializeObject(WMSCoreEndpointSecurity.PrepareResponse(oRequest.AuthToken, Constants.EndpointConstants.DTO.POD, _responsepODDTO));
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
        [Route("GetPOList")]
        public string GetPOList(WMSCoreMessage oRequest)
        {
            try
            {

                LoadControllerDefaults(oRequest);
                MRLWMSC21Service.PODBO[] _lstPODBO = null;// = new MRLWMSC21Service.PODBO[];

                List<POListDTO> _lstcyclecount = new List<POListDTO>();

                if (oRequest != null)
                {

                    POListDTO _oPODDTO = (POListDTO)WMSCoreEndpointSecurity.ValidateRequest(oRequest);

                    if (_oPODDTO != null)
                    {

                        MRLWMSC21Service.MRLWMSC21HHTWCFServiceClient oFalcon = new MRLWMSC21Service.MRLWMSC21HHTWCFServiceClient();
                        _lstPODBO = oFalcon.GetPOList(_oPODDTO.StatusName);
                    }
                    List<POListDTO> _responsepODDTO = ConvertListJson<POListDTO>(_lstPODBO);
                    string json = JsonConvert.SerializeObject(WMSCoreEndpointSecurity.PrepareResponse(oRequest.AuthToken, Constants.EndpointConstants.DTO.POList, _responsepODDTO));
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
        [Route("GetInboundList")]
        public string GetInboundList(WMSCoreMessage oRequest)
        {
            try
            {

                LoadControllerDefaults(oRequest);
                MRLWMSC21Service.PODBO[] _lstPODBO = null;// = new MRLWMSC21Service.PODBO[];

                List<PODDTO> _lstcyclecount = new List<PODDTO>();

                if (oRequest != null)
                {

                    InboundListDTO _oInboundListDTO = (InboundListDTO)WMSCoreEndpointSecurity.ValidateRequest(oRequest);

                    if (_oInboundListDTO != null)
                    {

                        MRLWMSC21Service.MRLWMSC21HHTWCFServiceClient oFalcon = new MRLWMSC21Service.MRLWMSC21HHTWCFServiceClient();
                        _lstPODBO = oFalcon.GetInboundList(_oInboundListDTO.WarehouseIDs, ConversionUtility.ConvertToInt(_oInboundListDTO.ShipmentTypeID), ConversionUtility.ConvertToInt(_oInboundListDTO.InboundStatusID), ConversionUtility.ConvertToInt(_oInboundListDTO.SupplierID), ConversionUtility.ConvertToInt(_oInboundListDTO.Limit), _oInboundListDTO.StatusName);
                    }
                    List<InboundListDTO> _responsepODDTO = ConvertListJson<InboundListDTO>(_lstPODBO);
                    string json = JsonConvert.SerializeObject(WMSCoreEndpointSecurity.PrepareResponse(oRequest.AuthToken, Constants.EndpointConstants.DTO.InboundList, _responsepODDTO));
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
        [Route("GetOutboundList")]
        public string GetOutboundList(WMSCoreMessage oRequest)
        {
            try
            {

                LoadControllerDefaults(oRequest);
                MRLWMSC21Service.PODBO[] _lstPODBO = null;// = new MRLWMSC21Service.PODBO[];

                List<OutboundListDTO> _lstcyclecount = new List<OutboundListDTO>();

                if (oRequest != null)
                {

                    OutboundListDTO _oPODDTO = (OutboundListDTO)WMSCoreEndpointSecurity.ValidateRequest(oRequest);

                    if (_oPODDTO != null)
                    {

                        MRLWMSC21Service.MRLWMSC21HHTWCFServiceClient oFalcon = new MRLWMSC21Service.MRLWMSC21HHTWCFServiceClient();
                        _lstPODBO = oFalcon.GetOutboundList(_oPODDTO.WarehouseIDs, ConversionUtility.ConvertToInt(_oPODDTO.ShipmentTypeID), ConversionUtility.ConvertToInt(_oPODDTO.InboundStatusID), ConversionUtility.ConvertToInt(_oPODDTO.SupplierID), ConversionUtility.ConvertToInt(_oPODDTO.Limit), _oPODDTO.StatusName);
                    }
                    List<OutboundListDTO> _responsepODDTO = ConvertListJson<OutboundListDTO>(_lstPODBO);
                    string json = JsonConvert.SerializeObject(WMSCoreEndpointSecurity.PrepareResponse(oRequest.AuthToken, Constants.EndpointConstants.DTO.OutboundList, _responsepODDTO));
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
        [Route("GetTenants")]
        public string GetTenants(WMSCoreMessage oRequest)
        {
            try
            {

                LoadControllerDefaults(oRequest);
                MRLWMSC21Service.UserDataTable _responseUserDataTable = new MRLWMSC21Service.UserDataTable();// = new MRLWMSC21Service.PODBO[];

                List<TenantDTO> _lstTenantDTO = new List<TenantDTO>();

                if (oRequest != null)
                {

                    TenantDTO _oPODDTO = (TenantDTO)WMSCoreEndpointSecurity.ValidateRequest(oRequest);

                    if (_oPODDTO != null)
                    {
                        MRLWMSC21Service.LiveStock _requestliveStock = new MRLWMSC21Service.LiveStock
                        {
                            AccountId = ConversionUtility.ConvertToInt(_oPODDTO.AccountId),
                            WarehouseID = ConversionUtility.ConvertToInt(_oPODDTO.WarehouseID)
                        };

                        MRLWMSC21Service.MRLWMSC21HHTWCFServiceClient oFalcon = new MRLWMSC21Service.MRLWMSC21HHTWCFServiceClient();
                        _responseUserDataTable = oFalcon.GetTenants(_requestliveStock);
                        foreach (DataRow item in _responseUserDataTable.Table.Rows)
                        {
                            _lstTenantDTO.Add
                            (
                                new TenantDTO { TenantID = item["TenantID"].ToString(), TenantName = item["TenantName"].ToString() }
                            );
                        }
                    }
                    List<TenantDTO> _responsepODDTO = ConvertListJson<TenantDTO>(_lstTenantDTO);
                    string json = JsonConvert.SerializeObject(WMSCoreEndpointSecurity.PrepareResponse(oRequest.AuthToken, Constants.EndpointConstants.DTO.Tenant, _responsepODDTO));
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
        [Route("GetSOList")]
        public string GetSOList(WMSCoreMessage oRequest)
        {
            try
            {

                LoadControllerDefaults(oRequest);
                MRLWMSC21Service.PODBO[] _lstPODBO = null;// = new MRLWMSC21Service.PODBO[];

                List<POListDTO> _lstcyclecount = new List<POListDTO>();

                if (oRequest != null)
                {

                    SOListDTO _oPOListDTO = (SOListDTO)WMSCoreEndpointSecurity.ValidateRequest(oRequest);

                    if (_oPOListDTO != null)
                    {

                        MRLWMSC21Service.MRLWMSC21HHTWCFServiceClient oFalcon = new MRLWMSC21Service.MRLWMSC21HHTWCFServiceClient();
                        _lstPODBO = oFalcon.GetSOList(_oPOListDTO.StatusName);
                    }
                    List<SOListDTO> _responsepODDTO = ConvertListJson<SOListDTO>(_lstPODBO);
                    string json = JsonConvert.SerializeObject(WMSCoreEndpointSecurity.PrepareResponse(oRequest.AuthToken, Constants.EndpointConstants.DTO.POList, _responsepODDTO));
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
        [Route("GetWarehouses")]
        public string GetWarehouses(WMSCoreMessage oRequest)
        {
            try
            {

                LoadControllerDefaults(oRequest);
                MRLWMSC21Service.PODBO[] _lstPODBO = null;// = new MRLWMSC21Service.PODBO[];

                List<WarehouseDTO> _lstcyclecount = new List<WarehouseDTO>();

                if (oRequest != null)
                {

                    PODDTO _oPODDTO = (PODDTO)WMSCoreEndpointSecurity.ValidateRequest(oRequest);

                    if (_oPODDTO != null)
                    {

                        MRLWMSC21Service.MRLWMSC21HHTWCFServiceClient oFalcon = new MRLWMSC21Service.MRLWMSC21HHTWCFServiceClient();
                        _lstPODBO = oFalcon.GetWarehouses();
                    }
                    List<WarehouseDTO> _responseWarehouseDTO = ConvertListJson<WarehouseDTO>(_lstPODBO);
                    string json = JsonConvert.SerializeObject(WMSCoreEndpointSecurity.PrepareResponse(oRequest.AuthToken, Constants.EndpointConstants.DTO.Warehouse, _responseWarehouseDTO));
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
        [Route("GetPartNumberList")]
        public string GetPartNumberList(WMSCoreMessage oRequest)
        {
            try
            {

                LoadControllerDefaults(oRequest);
                MRLWMSC21Service.PODBO[] _lstPODBO = null;// = new MRLWMSC21Service.PODBO[];

                List<TenantDTO> _lstcyclecount = new List<TenantDTO>();

                if (oRequest != null)
                {

                    TenantDTO _oPODDTO = (TenantDTO)WMSCoreEndpointSecurity.ValidateRequest(oRequest);

                    if (_oPODDTO != null)
                    {

                        MRLWMSC21Service.MRLWMSC21HHTWCFServiceClient oFalcon = new MRLWMSC21Service.MRLWMSC21HHTWCFServiceClient();
                        _lstPODBO = oFalcon.GetPartNumberList(ConversionUtility.ConvertToInt(_oPODDTO.TenantID));
                    }
                    List<TenantDTO> _responseWarehouseDTO = ConvertListJson<TenantDTO>(_lstPODBO);
                    string json = JsonConvert.SerializeObject(WMSCoreEndpointSecurity.PrepareResponse(oRequest.AuthToken, Constants.EndpointConstants.DTO.Tenant, _responseWarehouseDTO));
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
        [Route("GetMaterialType")]
        public string GetMaterialType(WMSCoreMessage oRequest)
        {
            try
            {

                LoadControllerDefaults(oRequest);
                MRLWMSC21Service.PODBO[] _lstPODBO = null;// = new MRLWMSC21Service.PODBO[];

                List<TenantDTO> _lstcyclecount = new List<TenantDTO>();

                if (oRequest != null)
                {

                    TenantDTO _oPODDTO = (TenantDTO)WMSCoreEndpointSecurity.ValidateRequest(oRequest);

                    if (_oPODDTO != null)
                    {

                        MRLWMSC21Service.MRLWMSC21HHTWCFServiceClient oFalcon = new MRLWMSC21Service.MRLWMSC21HHTWCFServiceClient();
                        _lstPODBO = oFalcon.GetMaterialType(ConversionUtility.ConvertToInt(_oPODDTO.TenantID));
                    }
                    List<TenantDTO> _responseWarehouseDTO = ConvertListJson<TenantDTO>(_lstPODBO);
                    string json = JsonConvert.SerializeObject(WMSCoreEndpointSecurity.PrepareResponse(oRequest.AuthToken, Constants.EndpointConstants.DTO.Tenant, _responseWarehouseDTO));
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
        [Route("GetWHForWHList")]
        public string GetWHForWHList(WMSCoreMessage oRequest)
        {
            try
            {

                LoadControllerDefaults(oRequest);
                MRLWMSC21Service.PODBO[] _lstPODBO = null;// = new MRLWMSC21Service.PODBO[];

                List<WarehouseDTO> _lstcyclecount = new List<WarehouseDTO>();

                if (oRequest != null)
                {

                    TenantDTO _oPODDTO = (TenantDTO)WMSCoreEndpointSecurity.ValidateRequest(oRequest);

                    if (_oPODDTO != null)
                    {
                        MRLWMSC21Service.MRLWMSC21HHTWCFServiceClient oFalcon = new MRLWMSC21Service.MRLWMSC21HHTWCFServiceClient();
                        _lstPODBO = oFalcon.GetWHForWHList(ConversionUtility.ConvertToInt(_oPODDTO.TenantID));
                    }
                    List<WarehouseDTO> _responseWarehouseDTO = ConvertListJson<WarehouseDTO>(_lstPODBO);
                    string json = JsonConvert.SerializeObject(WMSCoreEndpointSecurity.PrepareResponse(oRequest.AuthToken, Constants.EndpointConstants.DTO.Warehouse, _responseWarehouseDTO));
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
        [Route("GetShipmentStatus")]
        public string GetShipmentStatus(WMSCoreMessage oRequest)
        {
            try
            {

                LoadControllerDefaults(oRequest);
                MRLWMSC21Service.PODBO[] _lstPODBO = null;// = new MRLWMSC21Service.PODBO[];

                List<ShipmentDTO> _lstcyclecount = new List<ShipmentDTO>();

                if (oRequest != null)
                {

                    ShipmentDTO _oPODDTO = (ShipmentDTO)WMSCoreEndpointSecurity.ValidateRequest(oRequest);

                    if (_oPODDTO != null)
                    {
                        MRLWMSC21Service.MRLWMSC21HHTWCFServiceClient oFalcon = new MRLWMSC21Service.MRLWMSC21HHTWCFServiceClient();
                        _lstPODBO = oFalcon.GetShipmentStatus(ConversionUtility.ConvertToInt(_oPODDTO.Type));
                    }
                    List<ShipmentDTO> _responseShipmentDTO = ConvertListJson<ShipmentDTO>(_lstPODBO);
                    string json = JsonConvert.SerializeObject(WMSCoreEndpointSecurity.PrepareResponse(oRequest.AuthToken, Constants.EndpointConstants.DTO.Shipment, _responseShipmentDTO));
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
        [Route("PODDelivery")]
        public string PODDelivery(WMSCoreMessage oRequest)
        {
            try
            {

                LoadControllerDefaults(oRequest);
                MRLWMSC21Service.PODBO _responsePODBO = null;// = new MRLWMSC21Service.PODBO[];

                List<ShipmentDTO> _lstShipmentDTO = new List<ShipmentDTO>();

                if (oRequest != null)
                {

                    ShipmentDTO _oPODDTO = (ShipmentDTO)WMSCoreEndpointSecurity.ValidateRequest(oRequest);

                    MRLWMSC21Service.PODBO _PODBO = new MRLWMSC21Service.PODBO()
                    {
                        FileName = _oPODDTO.FileName,
                        FileConent = _oPODDTO.FileConent,
                        OBDTrackingID = _oPODDTO.OBDTrackingID,
                        UserID = _oPODDTO.UserID,
                        OBDNumber = _oPODDTO.OBDNumber
                        //WebURL = System.Configuration.ConfigurationManager.AppSettings["WebURL"]


                    };

                    if (_oPODDTO != null)
                    {
                        //PODImsgeService.PODUploadSoapClient cl = new PODImsgeService.PODUploadSoapClient();
                        //Byte[] vs = null;
                        //cl.UploadFile("", vs, "");


                        MRLWMSC21Service.MRLWMSC21HHTWCFServiceClient oFalcon = new MRLWMSC21Service.MRLWMSC21HHTWCFServiceClient();
                        _responsePODBO = oFalcon.PODDelivery(_PODBO);
                    }
                    ShipmentDTO _responseShipmentDTO = ConvertJson<ShipmentDTO>(_responsePODBO);
                    _lstShipmentDTO.Add(_responseShipmentDTO);
                    string json = JsonConvert.SerializeObject(WMSCoreEndpointSecurity.PrepareResponse(oRequest.AuthToken, Constants.EndpointConstants.DTO.Shipment, _lstShipmentDTO));
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
        [Route("GetPendingOutboundList")]
        public string GetPendingOutboundList(WMSCoreMessage oRequest)
        {
            try
            {

                LoadControllerDefaults(oRequest);
                MRLWMSC21Service.PODBO[] _lstPODBO = null;// = new MRLWMSC21Service.PODBO[];

                List<OutboundListDTO> _lstOutboundListDTO = new List<OutboundListDTO>();

                if (oRequest != null)
                {

                    OutboundListDTO _oOutboundListDTO = (OutboundListDTO)WMSCoreEndpointSecurity.ValidateRequest(oRequest);

                    if (_oOutboundListDTO != null)
                    {
                        MRLWMSC21Service.MRLWMSC21HHTWCFServiceClient oFalcon = new MRLWMSC21Service.MRLWMSC21HHTWCFServiceClient();
                        _lstPODBO = oFalcon.GetPendingOutboundList(ConversionUtility.ConvertToInt(_oOutboundListDTO.UserID));
                    }
                    List<OutboundListDTO> _responseOutboundListDTO = ConvertListJson<OutboundListDTO>(_lstPODBO);
                    string json = JsonConvert.SerializeObject(WMSCoreEndpointSecurity.PrepareResponse(oRequest.AuthToken, Constants.EndpointConstants.DTO.OutboundList, _responseOutboundListDTO));
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
        [Route("GetCurrentStock")]
        public string GetCurrentStock(WMSCoreMessage oRequest)
        {
            try
            {

                LoadControllerDefaults(oRequest);
                MRLWMSC21Service.PODBO[] _lstPODBO = null;// = new MRLWMSC21Service.PODBO[];

                List<TenantDTO> _lstTenantDTO = new List<TenantDTO>();

                if (oRequest != null)
                {

                    TenantDTO _oTenantDTO = (TenantDTO)WMSCoreEndpointSecurity.ValidateRequest(oRequest);

                    MRLWMSC21Service.PODBO _requestPODBO = new MRLWMSC21Service.PODBO()
                    {
                        TenantID = ConversionUtility.ConvertToInt(_oTenantDTO.TenantID),
                        MTypeID = _oTenantDTO.MTypeID,
                        MaterialMasterID = _oTenantDTO.MaterialMasterID,
                        WarehouseID = _oTenantDTO.WarehouseID
                    };

                    if (_oTenantDTO != null)
                    {
                        MRLWMSC21Service.MRLWMSC21HHTWCFServiceClient oFalcon = new MRLWMSC21Service.MRLWMSC21HHTWCFServiceClient();
                        _lstPODBO = oFalcon.GetCurrentStock(_requestPODBO);
                    }
                    List<TenantDTO> _responseTenantDTO = ConvertListJson<TenantDTO>(_lstPODBO);
                    string json = JsonConvert.SerializeObject(WMSCoreEndpointSecurity.PrepareResponse(oRequest.AuthToken, Constants.EndpointConstants.DTO.Tenant, _responseTenantDTO));
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
