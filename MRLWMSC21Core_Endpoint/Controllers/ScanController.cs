using MRLWMSC21Core.Library;
using MRLWMSC21_Endpoint.Interfaces;
using MRLWMSC21_Endpoint.Security;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MRLWMSC21_Endpoint.Controllers
{

    [RoutePrefix("Scan")]
    public class ScanController : BaseController, iScan
    {
        private string _ClassCode = string.Empty;

        public ScanController()
        {
            _ClassCode = ExceptionHandling.GetClassExceptionCode(ExceptionHandling.ExcpConstants_API_Enpoint.InboundController);
        }

        [Route("ValidateLocation")]
        [HttpPost]
        public string ValidateLocation(WMSCoreMessage oRequest)  //001
        {

            try
            {

                LoadControllerDefaults(oRequest);

                DTO.ScanDTO _oScanDTO = (DTO.ScanDTO)WMSCoreEndpointSecurity.ValidateRequest(oRequest);

                if (_oScanDTO != null)
                {

                    WMSCore.Business.ScanningBL oScannigBL = new WMSCore.Business.ScanningBL(LoggedInUserID, ConnectionString);
                    WMSCore_BusinessEntities.Entities.ScannedItem obj = new WMSCore_BusinessEntities.Entities.ScannedItem()
                    {
                        ScanInput = _oScanDTO.ScanInput,
                        WarehouseID = _oScanDTO.WarehouseID,
                        InboundID = _oScanDTO.InboundID,
                        ObdNumber = _oScanDTO.ObdNumber,
                        VlpdNumber = _oScanDTO.VlpdNumber,
                        IsCycleCount = _oScanDTO.IsCycleCount ? 1 : 0
                    };
                    WMSCore_BusinessEntities.Entities.ScannedItem scanItem = oScannigBL.ValidateLocation(obj);
                    _oScanDTO.ScanResult = scanItem.ScanResult;
                    _oScanDTO.Message = scanItem.Message;


                    string json = JsonConvert.SerializeObject(WMSCoreEndpointSecurity.PrepareResponse(oRequest.AuthToken, Constants.EndpointConstants.DTO.ScanDTO, _oScanDTO));
                    return json;

                }
                else
                {
                    List<WMSExceptionMessage> _lstwMSExceptionMessage = new List<WMSExceptionMessage>();
                    _lstwMSExceptionMessage.Add(new WMSExceptionMessage() { WMSMessage = "Invalid Request format" });
                    string json = JsonConvert.SerializeObject(WMSCoreEndpointSecurity.PrepareResponse(oRequest.AuthToken, Constants.EndpointConstants.DTO.Exception, _lstwMSExceptionMessage));
                    return json;
                }


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

        [Route("ValiDateMaterial")]
        [HttpPost]
        public string ValiDateMaterial(WMSCoreMessage oRequest)//002
        {
            try
            {

                LoadControllerDefaults(oRequest);

                DTO.ScanDTO _oScanDTO = (DTO.ScanDTO)WMSCoreEndpointSecurity.ValidateRequest(oRequest);

                if (_oScanDTO != null)
                {

                    WMSCore.Business.ScanningBL oScannigBL = new WMSCore.Business.ScanningBL(LoggedInUserID, ConnectionString);
                    WMSCore_BusinessEntities.Entities.ScannedItem obj = new WMSCore_BusinessEntities.Entities.ScannedItem()
                    {
                        ScanInput = _oScanDTO.ScanInput,
                        InboundID = _oScanDTO.InboundID,
                        TenantID = _oScanDTO.TenantID,
                        ObdNumber = _oScanDTO.ObdNumber,
                        VlpdNumber = _oScanDTO.VlpdNumber
                    };
                    WMSCore_BusinessEntities.Entities.ScannedItem scanItem = oScannigBL.ValidateSKU(obj);
                    _oScanDTO.ScanResult = scanItem.ScanResult;
                    _oScanDTO.SkuCode = scanItem.SkuCode;
                    _oScanDTO.Batch = scanItem.Batch;
                    _oScanDTO.SerialNumber = scanItem.SerialNumber;
                    _oScanDTO.MfgDate = scanItem.MfgDate;
                    _oScanDTO.ExpDate = scanItem.ExpDate;
                    _oScanDTO.Mrp = scanItem.Mrp;
                    _oScanDTO.LineNumber = scanItem.LineNumber;
                    _oScanDTO.KitID = scanItem.KitID;
                    _oScanDTO.PrjRef = scanItem.PrjRef;
                    _oScanDTO.SupplierInvoiceDetailsID = scanItem.SupplierInvoiceDetailsId;
                    _oScanDTO.HUNo = scanItem.HUNo.ToString();
                    _oScanDTO.HUSize = scanItem.HUSize.ToString();
                    _oScanDTO.Message = scanItem.Message;


                    string json = JsonConvert.SerializeObject(WMSCoreEndpointSecurity.PrepareResponse(oRequest.AuthToken, Constants.EndpointConstants.DTO.ScanDTO, _oScanDTO));
                    return json;

                }
                else
                {
                    List<WMSExceptionMessage> _lstwMSExceptionMessage = new List<WMSExceptionMessage>();
                    _lstwMSExceptionMessage.Add(new WMSExceptionMessage() { WMSMessage = "Invalid Request format" });
                    string json = JsonConvert.SerializeObject(WMSCoreEndpointSecurity.PrepareResponse(oRequest.AuthToken, Constants.EndpointConstants.DTO.Exception, _lstwMSExceptionMessage));
                    return json;
                }


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

        [Route("ValidatePallet")]
        [HttpPost]
        public string ValidatePallet(WMSCoreMessage oRequest) //003
        {
            try
            {

                LoadControllerDefaults(oRequest);

                DTO.ScanDTO _oScanDTO = (DTO.ScanDTO)WMSCoreEndpointSecurity.ValidateRequest(oRequest);

                if (_oScanDTO != null)
                {

                    WMSCore.Business.ScanningBL oScannigBL = new WMSCore.Business.ScanningBL(LoggedInUserID, ConnectionString);
                    WMSCore_BusinessEntities.Entities.ScannedItem obj = new WMSCore_BusinessEntities.Entities.ScannedItem()
                    {
                        ScanInput = _oScanDTO.ScanInput,
                        WarehouseID = _oScanDTO.WarehouseID,
                        InboundID = _oScanDTO.InboundID,
                        ObdNumber = _oScanDTO.ObdNumber,
                        VlpdNumber = _oScanDTO.VlpdNumber
                    };
                    WMSCore_BusinessEntities.Entities.ScannedItem scanItem = oScannigBL.ValidatePallet(obj);
                    _oScanDTO.ScanResult = scanItem.ScanResult;
                    _oScanDTO.Message = scanItem.Message;


                    string json = JsonConvert.SerializeObject(WMSCoreEndpointSecurity.PrepareResponse(oRequest.AuthToken, Constants.EndpointConstants.DTO.ScanDTO, _oScanDTO));
                    return json;

                }
                else
                {
                    List<WMSExceptionMessage> _lstwMSExceptionMessage = new List<WMSExceptionMessage>();
                    _lstwMSExceptionMessage.Add(new WMSExceptionMessage() { WMSMessage = "Invalid Request format" });
                    string json = JsonConvert.SerializeObject(WMSCoreEndpointSecurity.PrepareResponse(oRequest.AuthToken, Constants.EndpointConstants.DTO.Exception, _lstwMSExceptionMessage));
                    return json;
                }


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


        [Route("ValidateSO")]
        [HttpPost]
        public string ValidateSO(WMSCoreMessage oRequest) //003
        {
            try
            {

                LoadControllerDefaults(oRequest);

                DTO.ScanDTO _oScanDTO = (DTO.ScanDTO)WMSCoreEndpointSecurity.ValidateRequest(oRequest);

                if (_oScanDTO != null)
                {

                    WMSCore.Business.ScanningBL oScannigBL = new WMSCore.Business.ScanningBL(LoggedInUserID, ConnectionString);
                    WMSCore_BusinessEntities.Entities.ScannedItem obj = new WMSCore_BusinessEntities.Entities.ScannedItem()
                    {
                        ScanInput = _oScanDTO.ScanInput,
                        AccountID = _oScanDTO.AccountID,
                        UserID = Convert.ToInt32(_oScanDTO.UserID)

                    };
                    WMSCore_BusinessEntities.Entities.ScannedItem scanItem = oScannigBL.ValidateSO(obj);
                    _oScanDTO.ScanResult = scanItem.ScanResult;
                    _oScanDTO.Message = scanItem.Message;


                    string json = JsonConvert.SerializeObject(WMSCoreEndpointSecurity.PrepareResponse(oRequest.AuthToken, Constants.EndpointConstants.DTO.ScanDTO, _oScanDTO));
                    return json;

                }
                else
                {
                    List<WMSExceptionMessage> _lstwMSExceptionMessage = new List<WMSExceptionMessage>();
                    _lstwMSExceptionMessage.Add(new WMSExceptionMessage() { WMSMessage = "Invalid Request format" });
                    string json = JsonConvert.SerializeObject(WMSCoreEndpointSecurity.PrepareResponse(oRequest.AuthToken, Constants.EndpointConstants.DTO.Exception, _lstwMSExceptionMessage));
                    return json;
                }


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

        [Route("ValidateCarton")]
        [HttpPost]
        public string ValidateCarton(WMSCoreMessage oRequest)
        {
            try
            {

                LoadControllerDefaults(oRequest);

                DTO.ScanDTO _oScanDTO = (DTO.ScanDTO)WMSCoreEndpointSecurity.ValidateRequest(oRequest);

                if (_oScanDTO != null)
                {

                    WMSCore.Business.ScanningBL oScannigBL = new WMSCore.Business.ScanningBL(LoggedInUserID, ConnectionString);
                    WMSCore_BusinessEntities.Entities.ScannedItem obj = new WMSCore_BusinessEntities.Entities.ScannedItem()
                    {
                        ScanInput = _oScanDTO.ScanInput,
                        ObdNumber = _oScanDTO.ObdNumber,
                        AccountID = _oScanDTO.AccountID

                    };
                    WMSCore_BusinessEntities.Entities.ScannedItem scanItem = oScannigBL.ValidateCarton(obj);
                    _oScanDTO.ScanResult = scanItem.ScanResult;
                    _oScanDTO.Message = scanItem.Message;


                    string json = JsonConvert.SerializeObject(WMSCoreEndpointSecurity.PrepareResponse(oRequest.AuthToken, Constants.EndpointConstants.DTO.ScanDTO, _oScanDTO));
                    return json;

                }
                else
                {
                    List<WMSExceptionMessage> _lstwMSExceptionMessage = new List<WMSExceptionMessage>();
                    _lstwMSExceptionMessage.Add(new WMSExceptionMessage() { WMSMessage = "Invalid Request format" });
                    string json = JsonConvert.SerializeObject(WMSCoreEndpointSecurity.PrepareResponse(oRequest.AuthToken, Constants.EndpointConstants.DTO.Exception, _lstwMSExceptionMessage));
                    return json;
                }


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
    }
}
