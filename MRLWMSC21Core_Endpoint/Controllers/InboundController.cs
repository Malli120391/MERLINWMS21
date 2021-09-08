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
using System.Text;
using System.Data;

namespace MRLWMSC21_Endpoint.Controllers
{
    [RoutePrefix("Inbound")]
    public class InboundController : BaseController, iInbound
    {
        private string _ClassCode = string.Empty;

        public InboundController()
        {
            _ClassCode = ExceptionHandling.GetClassExceptionCode(ExceptionHandling.ExcpConstants_API_Enpoint.InboundController);
        }


        [HttpPost]
        [Route("GetStoreRefNos")]
        public string GetStoreRefNos(WMSCoreMessage oRequest)
        {
            try
            {

                LoadControllerDefaults(oRequest);
                MRLWMSC21Service.UserDataTable userDataTable = new MRLWMSC21Service.UserDataTable();

                InboundBL inboundBL = new InboundBL(LoggedInUserID, ConnectionString);

                List<InboundDTO> _lstinbound = new List<InboundDTO>();

                List<Inbound> lInbound = new List<Inbound>();

                if (oRequest != null)
                {

                    InboundDTO _oInboundDTO = (InboundDTO)WMSCoreEndpointSecurity.ValidateRequest(oRequest);

                    if (_oInboundDTO != null)
                    {


                        Inbound inbound = new Inbound()
                        {
                            AccountID = ConversionUtility.ConvertToInt(_oInboundDTO.AccountID),
                            CreatedByUserID = ConversionUtility.ConvertToInt(_oInboundDTO.UserId)
                        };

                        lInbound = inboundBL.GetStoreRefNos(inbound);


                        foreach (Inbound inboundItem in lInbound)
                        {
                            InboundDTO responseDTO = new InboundDTO();
                            responseDTO.StoreRefNo = inboundItem.StoreRefNo.ToString();
                            responseDTO.InboundID = inboundItem.InboundID.ToString();
                            responseDTO.InvoiceQty = inboundItem.InvoiceQty.ToString();
                            responseDTO.ReceivedQty = inboundItem.ReceivedQty.ToString();
                            List<EntryDTO> entryList = new List<EntryDTO>();

                           
                            EntryDTO entry = new EntryDTO()
                            {
                                VehicleNumber = inboundItem.VehicleNumber.ToString(),
                                DockNumber = inboundItem.DockNumber.ToString(),
                                DockID = inboundItem.DockID.ToString()
                            };
                            entryList.Add(entry);

                            responseDTO.Entry = entryList;
                            _lstinbound.Add(responseDTO);
                        }


                    }

                    string json = JsonConvert.SerializeObject(WMSCoreEndpointSecurity.PrepareResponse(oRequest.AuthToken, Constants.EndpointConstants.DTO.Inbound, _lstinbound));
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

        public List<EntryDTO> GetDockVehicles(int InboundID)
        {
            MRLWMSC21Service.UserDataTable userDataTable = new MRLWMSC21Service.UserDataTable();
            MRLWMSC21Service.MRLWMSC21HHTWCFServiceClient oFalcon = new MRLWMSC21Service.MRLWMSC21HHTWCFServiceClient();
            userDataTable = oFalcon.GetDockVehicles(InboundID);
            List<EntryDTO> entryList = new List<EntryDTO>();
              
            if(userDataTable != null)
            { 
                foreach(DataRow dr in userDataTable.Table.Rows)
                {
                    EntryDTO entry = new EntryDTO()
                    {
                        VehicleNumber = dr["VehicleRegNo"].ToString(),
                        DockNumber = dr["Dock"].ToString(),
                        DockID= dr["DockID"].ToString()
                    };
                    entryList.Add(entry);
                }
            }
            else
            {
                EntryDTO entry = new EntryDTO()
                {
                    VehicleNumber = "",
                    DockNumber = ""
                };
                entryList.Add(entry);
            }

            return entryList;
        }

        [HttpPost]
        [Route("GetStorageLocations")]
        public string GetStorageLocations(WMSCoreMessage oRequest)
        {
            try
            {

                LoadControllerDefaults(oRequest);
                MRLWMSC21Service.UserDataTable userDataTable = new MRLWMSC21Service.UserDataTable();
                InboundDTO responseDTO = new InboundDTO();
                List<InboundDTO> _lstinbound = new List<InboundDTO>();

                if (oRequest != null)
                {

                    InboundDTO _oInboundDTO = (InboundDTO)WMSCoreEndpointSecurity.ValidateRequest(oRequest);

                    if (_oInboundDTO != null)
                    {

                        MRLWMSC21Service.MRLWMSC21HHTWCFServiceClient oFalcon = new MRLWMSC21Service.MRLWMSC21HHTWCFServiceClient();
                        userDataTable = oFalcon.GetStorageLocations();
                    }
                    foreach (DataRow row in userDataTable.Table.Rows)
                    {
                        InboundDTO _responseInboundDTO = new InboundDTO();
                        _responseInboundDTO.StorageLocation = row["Code"].ToString();

                        _lstinbound.Add(_responseInboundDTO);
                    }

                    string json = JsonConvert.SerializeObject(WMSCoreEndpointSecurity.PrepareResponse(oRequest.AuthToken, Constants.EndpointConstants.DTO.Inbound, _lstinbound));
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
        [Route("CheckContainer")]
        public string CheckContainer(WMSCoreMessage oRequest)
        {
            try
            {

                LoadControllerDefaults(oRequest);
                MRLWMSC21Service.UserDataTable userDataTable = new MRLWMSC21Service.UserDataTable();
                InboundDTO responseDTO = new InboundDTO();
                List<InboundDTO> _lstinbound = new List<InboundDTO>();
                String result = null;
                if (oRequest != null)
                {

                    InboundDTO _oInboundDTO = (InboundDTO)WMSCoreEndpointSecurity.ValidateRequest(oRequest);

                    if (_oInboundDTO != null)
                    {

                        MRLWMSC21Service.MRLWMSC21HHTWCFServiceClient oFalcon = new MRLWMSC21Service.MRLWMSC21HHTWCFServiceClient();
                        result = oFalcon.CheckContainer(_oInboundDTO.PalletNo, _oInboundDTO.InboundID);
                        if (result != "")
                        {
                            throw new WMSExceptionMessage() { WMSExceptionCode = result, WMSMessage = result, ShowAsError = true };
                        }
                        else
                        {
                            responseDTO.Result = "1";
                        }
                        _lstinbound.Add(responseDTO);
                    }


                    string json = JsonConvert.SerializeObject(WMSCoreEndpointSecurity.PrepareResponse(oRequest.AuthToken, Constants.EndpointConstants.DTO.Inbound, _lstinbound));
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
        [Route("GetReceivedQty")]
        public string GetReceivedQty(WMSCoreMessage oRequest)
        {
            try
            {

                LoadControllerDefaults(oRequest);
                MRLWMSC21Service.Inbound oresponseinbound = new MRLWMSC21Service.Inbound();
                InboundDTO responseDTO = new InboundDTO();
                List<InboundDTO> _lstinbound = new List<InboundDTO>();
                String result = null;
                if (oRequest != null)
                {

                    InboundDTO _oInboundDTO = (InboundDTO)WMSCoreEndpointSecurity.ValidateRequest(oRequest);

                    if (_oInboundDTO != null)
                    {
                        MRLWMSC21Service.Inbound oinbound = new MRLWMSC21Service.Inbound()
                        {

                            AccountId = ConversionUtility.ConvertToInt(_oInboundDTO.AccountID),
                            InboundId = _oInboundDTO.InboundID,
                            MCode = _oInboundDTO.Mcode,
                            Storerefno = _oInboundDTO.StoreRefNo,
                            Lineno = _oInboundDTO.LineNo,
                            MfgDate = _oInboundDTO.MfgDate,
                            ExpDate = _oInboundDTO.ExpDate,
                            SerialNo = _oInboundDTO.SerialNo,
                            ProjectNo = _oInboundDTO.ProjectRefno,
                            MRP = _oInboundDTO.MRP,
                            SupplierInvoiceDetailsId = _oInboundDTO.SupplierInvoiceDetailsID,
                            HUNo = _oInboundDTO.HUNo,
                            HUSize = _oInboundDTO.HUSize,
                            BatchNo =_oInboundDTO.BatchNo
                        };
                        MRLWMSC21Service.MRLWMSC21HHTWCFServiceClient oFalcon = new MRLWMSC21Service.MRLWMSC21HHTWCFServiceClient();
                        oresponseinbound = oFalcon.GetReceivedQty(oinbound);
                        if (oresponseinbound != null)
                        {
                            if(oresponseinbound.Result== "invalid")
                            {
                                throw new WMSExceptionMessage() { WMSExceptionCode = null, WMSMessage =" Please scan valid SKU", ShowAsError = true };
                            }
                            responseDTO.ItemPendingQty = oresponseinbound.ItemPendingQty.ToString();
                            responseDTO.ReceivedQty = oresponseinbound.ReceivedQty.ToString();
                        }


                        _lstinbound.Add(responseDTO);
                    }


                    string json = JsonConvert.SerializeObject(WMSCoreEndpointSecurity.PrepareResponse(oRequest.AuthToken, Constants.EndpointConstants.DTO.Inbound, _lstinbound));
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
        [Route("UpdateReceiveItemForHHT")]
        public string UpdateReceiveItemForHHT(WMSCoreMessage oRequest)
        {
            try
            {

                LoadControllerDefaults(oRequest);
                MRLWMSC21Service.Inbound oresponseinbound = new MRLWMSC21Service.Inbound();
                InboundDTO responseDTO = new InboundDTO();
                List<InboundDTO> _lstinbound = new List<InboundDTO>();

                if (oRequest != null)
                {

                    InboundDTO _oInboundDTO = (InboundDTO)WMSCoreEndpointSecurity.ValidateRequest(oRequest);

                    if (_oInboundDTO != null)
                    {
                        MRLWMSC21Service.Inbound oinbound = new MRLWMSC21Service.Inbound()
                        {

                            AccountId = ConversionUtility.ConvertToInt(_oInboundDTO.AccountID),
                            InboundId = _oInboundDTO.InboundID,
                            MCode = _oInboundDTO.Mcode,
                            Qty = _oInboundDTO.Qty,
                            IsDam = _oInboundDTO.IsDam,
                            HasDisc = _oInboundDTO.HasDisc,
                            Lineno = _oInboundDTO.LineNo,
                            CreatedBy = _oInboundDTO.UserId,
                            CartonNo = _oInboundDTO.CartonNo,
                            SLoc = _oInboundDTO.StorageLocation,
                            BatchNo = _oInboundDTO.BatchNo,
                            SerialNo = _oInboundDTO.SerialNo,
                            ExpDate = _oInboundDTO.ExpDate,
                            MfgDate = _oInboundDTO.MfgDate,
                            ProjectNo = _oInboundDTO.ProjectRefno,
                            Storerefno = _oInboundDTO.StoreRefNo,
                            MRP = _oInboundDTO.MRP,
                            Dock=_oInboundDTO.Dock,
                            SupplierInvoiceDetailsId = _oInboundDTO.SupplierInvoiceDetailsID,
                            HUNo = _oInboundDTO.HUNo.ToString(),
                            HUSize = _oInboundDTO.HUSize.ToString(),
                            VehicleNo =_oInboundDTO.VehicleNo
                            

                        };
                        MRLWMSC21Service.MRLWMSC21HHTWCFServiceClient oFalcon = new MRLWMSC21Service.MRLWMSC21HHTWCFServiceClient();
                        oresponseinbound = oFalcon.UpdateReceiveItemForHHT(oinbound);

                        if (oresponseinbound.Result == "Quantity exceed")
                        {
                            throw new WMSExceptionMessage() { WMSExceptionCode = oresponseinbound.Result, WMSMessage = oresponseinbound.Result, ShowAsError = true };
                        }
                        if (oresponseinbound.Result == "Please enter correct recieved Qty")
                        {
                            throw new WMSExceptionMessage() { WMSExceptionCode = oresponseinbound.Result, WMSMessage = oresponseinbound.Result, ShowAsError = true };
                        }




                        if (oresponseinbound != null)
                        {
                            responseDTO.ItemPendingQty = oresponseinbound.ItemPendingQty.ToString();
                            responseDTO.ReceivedQty = oresponseinbound.ReceivedQty.ToString();
                            responseDTO.Result = oresponseinbound.Result;
                        }
                        _lstinbound.Add(responseDTO);
                    }


                    string json = JsonConvert.SerializeObject(WMSCoreEndpointSecurity.PrepareResponse(oRequest.AuthToken, Constants.EndpointConstants.DTO.Inbound, _lstinbound));
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
        [Route("GetSkipReasonList")]
        public string GetSkipReasonList(WMSCoreMessage oRequest)
        {
            try
            {

                LoadControllerDefaults(oRequest);
                MRLWMSC21Service.UserDataTable userDataTable = new MRLWMSC21Service.UserDataTable();
                InboundDTO responseDTO = new InboundDTO();
                List<InboundDTO> _lstinbound = new List<InboundDTO>();

                if (oRequest != null)
                {

                    InboundDTO _oInboundDTO = (InboundDTO)WMSCoreEndpointSecurity.ValidateRequest(oRequest);

                    if (_oInboundDTO != null)
                    {


                        MRLWMSC21Service.MRLWMSC21HHTWCFServiceClient oFalcon = new MRLWMSC21Service.MRLWMSC21HHTWCFServiceClient();
                        userDataTable = oFalcon.GetSkipReasonList(_oInboundDTO.SkipType);
                    }
                    foreach (DataRow row in userDataTable.Table.Rows)
                    {
                        string sss = row["Reason"].ToString();
                        InboundDTO responseDTO1 = new InboundDTO();
                        responseDTO1.SkipReason = row["Reason"].ToString();

                        _lstinbound.Add(responseDTO1);
                    }

                    string json = JsonConvert.SerializeObject(WMSCoreEndpointSecurity.PrepareResponse(oRequest.AuthToken, Constants.EndpointConstants.DTO.Inbound, _lstinbound));
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
        [Route("GetConatinerLocation")]
        public string GetConatinerLocation(WMSCoreMessage oRequest)
        {
            try
            {

                LoadControllerDefaults(oRequest);
                MRLWMSC21Service.UserDataTable userDataTable = new MRLWMSC21Service.UserDataTable();
                InboundDTO responseDTO = new InboundDTO();
                List<InboundDTO> _lstinbound = new List<InboundDTO>();
                String result = null;
                if (oRequest != null)
                {

                    InboundDTO _oInboundDTO = (InboundDTO)WMSCoreEndpointSecurity.ValidateRequest(oRequest);

                    if (_oInboundDTO != null)
                    {

                        MRLWMSC21Service.MRLWMSC21HHTWCFServiceClient oFalcon = new MRLWMSC21Service.MRLWMSC21HHTWCFServiceClient();
                       // result = oFalcon.GetConatinerLocation(_oInboundDTO.PalletNo);

                        responseDTO.Result = result;

                        _lstinbound.Add(responseDTO);
                    }


                    string json = JsonConvert.SerializeObject(WMSCoreEndpointSecurity.PrepareResponse(oRequest.AuthToken, Constants.EndpointConstants.DTO.Inbound, _lstinbound));
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
        [Route("GetItemTOPutAway")]
        public string GetItemTOPutAway(WMSCoreMessage oRequest)
        {
            try
            {

                LoadControllerDefaults(oRequest);
                MRLWMSC21Service.PutAway oPutawayresponse = new MRLWMSC21Service.PutAway();
                PutAwayDTO responseDTO = new PutAwayDTO();
                List<PutAwayDTO> _lstinbound = new List<PutAwayDTO>();

                if (oRequest != null)
                {

                    PutAwayDTO _oPutawayDTO = (PutAwayDTO)WMSCoreEndpointSecurity.ValidateRequest(oRequest);

                    if (_oPutawayDTO != null)
                    {
                        MRLWMSC21Service.PutAway oPutaway = new MRLWMSC21Service.PutAway()
                        {

                            InboundId = _oPutawayDTO.InboundId,
                            TransferRequestId = Convert.ToInt32(_oPutawayDTO.TransferRequestId)
                        };
                        MRLWMSC21Service.MRLWMSC21HHTWCFServiceClient oFalcon = new MRLWMSC21Service.MRLWMSC21HHTWCFServiceClient();
                        oPutawayresponse = oFalcon.GetItemTOPutAway(oPutaway);
                        if (oPutawayresponse != null && oPutawayresponse.Result == "3")
                        {
                            responseDTO.SuggestedPutawayID = oPutawayresponse.SuggestedPutawayID.ToString();
                            responseDTO.MaterialMasterId = oPutawayresponse.MaterialMasterId.ToString();
                            responseDTO.MCode = oPutawayresponse.MCode.ToString();

                            responseDTO.CartonCode = oPutawayresponse.CartonCode.ToString();
                            responseDTO.CartonID = oPutawayresponse.CartonID.ToString();
                            responseDTO.Location = oPutawayresponse.Location.ToString();
                            responseDTO.LocationID = oPutawayresponse.LocationID.ToString();
                            responseDTO.MfgDate = oPutawayresponse.MfgDate.ToString();
                            responseDTO.ExpDate = oPutawayresponse.ExpDate.ToString();
                            responseDTO.SerialNo = oPutawayresponse.SerialNo.ToString();
                            responseDTO.BatchNo = oPutawayresponse.BatchNo.ToString();
                            responseDTO.ProjectRefNo = oPutawayresponse.ProjectRefNo.ToString();
                            responseDTO.MRP = oPutawayresponse.MRP.ToString();
                            responseDTO.SuggestedQty = oPutawayresponse.SuggestedQty.ToString();
                            responseDTO.SuggestedReceivedQty = oPutawayresponse.SuggestedReceivedQty.ToString();
                            responseDTO.SuggestedRemainingQty = oPutawayresponse.SuggestedRemainingQty.ToString();
                            responseDTO.TransferRequestDetailsId = oPutawayresponse.TransferRequestDetailsId.ToString();
                            responseDTO.PickedLocationID = oPutawayresponse.PickedLocationID.ToString();
                            responseDTO.GMDRemainingQty = oPutawayresponse.GMDRemainingQty.ToString();
                            responseDTO.PutAwayQty = "1";
                            responseDTO.Result = oPutawayresponse.Result;
                            responseDTO.Dock = oPutawayresponse.Dock;
                            responseDTO.StorageCode = oPutawayresponse.StorageLocation;
                        }
                        else
                        {
                            responseDTO.Result = oPutawayresponse.Result;
                        }
                        _lstinbound.Add(responseDTO);
                    }


                    string json = JsonConvert.SerializeObject(WMSCoreEndpointSecurity.PrepareResponse(oRequest.AuthToken, Constants.EndpointConstants.DTO.PutAwayDTO, _lstinbound));
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
        [Route("CheckPutAwayItemQty")]
        public string CheckPutAwayItemQty(WMSCoreMessage oRequest)
        {
            try
            {

                LoadControllerDefaults(oRequest);
                MRLWMSC21Service.PutAway oresponseputaway = new MRLWMSC21Service.PutAway();
                PutAwayDTO responseDTO = new PutAwayDTO();
                List<PutAwayDTO> _lstputaway = new List<PutAwayDTO>();
                String result = null;
                if (oRequest != null)
                {

                    PutAwayDTO _oPutawayDTO = (PutAwayDTO)WMSCoreEndpointSecurity.ValidateRequest(oRequest);

                    if (_oPutawayDTO != null)
                    {
                        MRLWMSC21Service.PutAway oinbound = new MRLWMSC21Service.PutAway()
                        {
                            TransferRequestId = Convert.ToInt32(_oPutawayDTO.TransferRequestId),
                            InboundId = _oPutawayDTO.InboundId,
                            MaterialMasterId = ConversionUtility.ConvertToInt(_oPutawayDTO.MaterialMasterId),
                            PutAwayQty = "0",
                            SuggestedPutawayID= ConversionUtility.ConvertToInt(_oPutawayDTO.SuggestedPutawayID)
                        };
                        MRLWMSC21Service.MRLWMSC21HHTWCFServiceClient oFalcon = new MRLWMSC21Service.MRLWMSC21HHTWCFServiceClient();
                        oresponseputaway = oFalcon.CheckPutAwayItemQty(oinbound);
                        if (oresponseputaway != null)
                        {
                            if (oresponseputaway.Result != "")
                            {
                                result = oresponseputaway.Result;
                                throw new WMSExceptionMessage() { WMSExceptionCode = result, WMSMessage = result, ShowAsError = true };
                            }
                            else
                            {
                                responseDTO.Result = "1";
                            }
                        }
                        _lstputaway.Add(responseDTO);
                    }


                    string json = JsonConvert.SerializeObject(WMSCoreEndpointSecurity.PrepareResponse(oRequest.AuthToken, Constants.EndpointConstants.DTO.PutAwayDTO, _lstputaway));
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
        [Route("SkipItem")]
        public string SkipItem(WMSCoreMessage oRequest)
        {
            try
            {

                LoadControllerDefaults(oRequest);
                MRLWMSC21Service.PutAway oPutawayresponse = new MRLWMSC21Service.PutAway();
                PutAwayDTO responseDTO = new PutAwayDTO();
                List<PutAwayDTO> _lstinbound = new List<PutAwayDTO>();

                if (oRequest != null)
                {

                    PutAwayDTO _oputawayDTO = (PutAwayDTO)WMSCoreEndpointSecurity.ValidateRequest(oRequest);

                    if (_oputawayDTO != null)
                    {
                        MRLWMSC21Service.PutAway oPutaway = new MRLWMSC21Service.PutAway()
                        {
                            TransferRequestId = Convert.ToInt32(_oputawayDTO.TransferRequestId),
                            InboundId = _oputawayDTO.InboundId,
                            SuggestedPutawayID = Convert.ToInt32(_oputawayDTO.SuggestedPutawayID),
                            MCode = _oputawayDTO.MCode,
                            MfgDate = _oputawayDTO.MfgDate,
                            ExpDate = _oputawayDTO.ExpDate,
                            BatchNo = _oputawayDTO.BatchNo,
                            SerialNo = _oputawayDTO.SerialNo,
                            ProjectRefNo = _oputawayDTO.ProjectRefNo,
                            MRP = _oputawayDTO.MRP,
                            SkipQty = Convert.ToDecimal(_oputawayDTO.SkipQty),
                            SuggestedReceivedQty = Convert.ToDecimal(_oputawayDTO.SuggestedReceivedQty),
                            UserId = Convert.ToInt32(_oputawayDTO.UserID),
                            StorageLocation = "Available",
                            CartonCode = _oputawayDTO.CartonCode,
                            Flag = 1,
                            Location = _oputawayDTO.Location,
                            Skipreason = _oputawayDTO.SkipReason
                        };
                        MRLWMSC21Service.MRLWMSC21HHTWCFServiceClient oFalcon = new MRLWMSC21Service.MRLWMSC21HHTWCFServiceClient();
                        oPutawayresponse = oFalcon.SkipItem(oPutaway);
                        if (oPutawayresponse != null)
                        {
                            responseDTO.Result = "1";

                            return GetItemTOPutAway(oRequest);
                        }
                        _lstinbound.Add(responseDTO);
                    }


                    string json = JsonConvert.SerializeObject(WMSCoreEndpointSecurity.PrepareResponse(oRequest.AuthToken, Constants.EndpointConstants.DTO.Inbound, _lstinbound));
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
        [Route("UpsertPutAwayItem")]
        public string UpsertPutAwayItem(WMSCoreMessage oRequest)
        {
            try
            {

                LoadControllerDefaults(oRequest);
                MRLWMSC21Service.PutAway oPutawayresponse = new MRLWMSC21Service.PutAway();
                PutAwayDTO responseDTO = new PutAwayDTO();
                List<PutAwayDTO> _lstPutaway = new List<PutAwayDTO>();

                if (oRequest != null)
                {

                    PutAwayDTO _oPutawayDTO = (PutAwayDTO)WMSCoreEndpointSecurity.ValidateRequest(oRequest);

                    if (_oPutawayDTO != null)
                    {
                        MRLWMSC21Service.PutAway oPutaway = new MRLWMSC21Service.PutAway()
                        {

                            TransferRequestId = Convert.ToInt32(_oPutawayDTO.TransferRequestId),

                            InboundId = _oPutawayDTO.InboundId,
                            SuggestedPutawayID = ConversionUtility.ConvertToInt(_oPutawayDTO.SuggestedPutawayID),
                            MCode = _oPutawayDTO.MCode,
                            MfgDate = _oPutawayDTO.MfgDate,
                            ExpDate = _oPutawayDTO.ExpDate,
                            BatchNo = _oPutawayDTO.BatchNo,
                            ProjectRefNo = _oPutawayDTO.ProjectRefNo,
                            SerialNo = _oPutawayDTO.SerialNo,
                            MRP = _oPutawayDTO.MRP,
                            PutAwayQty = _oPutawayDTO.PutAwayQty,
                            TotalQuantity = ConversionUtility.ConvertToDecimal(_oPutawayDTO.TotalQty),
                            UserId = Convert.ToInt32(_oPutawayDTO.UserID),
                            CartonCode = _oPutawayDTO.CartonCode,
                            Location = _oPutawayDTO.Location,
                            StorageLocation = _oPutawayDTO.StorageCode,
                            ScnnedLocation= _oPutawayDTO.ScannedLocation
                        };
                        MRLWMSC21Service.MRLWMSC21HHTWCFServiceClient oFalcon = new MRLWMSC21Service.MRLWMSC21HHTWCFServiceClient();
                        oPutawayresponse = oFalcon.UpsertPutAwayItem(oPutaway);
                        if (oPutawayresponse != null)
                        {
                            if (oPutawayresponse.Result == "3")
                            {
                                responseDTO.SuggestedPutawayID = oPutawayresponse.SuggestedPutawayID.ToString();
                                responseDTO.MaterialMasterId = oPutawayresponse.MaterialMasterId.ToString();
                                responseDTO.MCode = oPutawayresponse.MCode.ToString();

                                responseDTO.CartonCode = oPutawayresponse.CartonCode.ToString();
                                responseDTO.CartonID = oPutawayresponse.CartonID.ToString();
                                responseDTO.Location = oPutawayresponse.Location.ToString();
                                responseDTO.LocationID = oPutawayresponse.LocationID.ToString();
                                responseDTO.MfgDate = oPutawayresponse.MfgDate.ToString();
                                responseDTO.ExpDate = oPutawayresponse.ExpDate.ToString();
                                responseDTO.SerialNo = oPutawayresponse.SerialNo.ToString();
                                responseDTO.BatchNo = oPutawayresponse.BatchNo.ToString();
                                responseDTO.ProjectRefNo = oPutawayresponse.ProjectRefNo.ToString();
                                responseDTO.MRP = oPutawayresponse.MRP.ToString();
                                responseDTO.SuggestedQty = oPutawayresponse.SuggestedQty.ToString();
                                responseDTO.SuggestedReceivedQty = oPutawayresponse.SuggestedReceivedQty.ToString();
                                responseDTO.SuggestedRemainingQty = oPutawayresponse.SuggestedRemainingQty.ToString();
                                responseDTO.TransferRequestDetailsId = oPutawayresponse.TransferRequestDetailsId.ToString();
                                responseDTO.PickedLocationID = oPutawayresponse.PickedLocationID.ToString();
                                responseDTO.GMDRemainingQty = oPutawayresponse.GMDRemainingQty.ToString();
                                responseDTO.PutAwayQty = "1";
                                responseDTO.Result = "3";
                            }
                            else if (oPutawayresponse.Result == "1" || oPutawayresponse.Result == "2")
                            {
                                responseDTO.Result = oPutawayresponse.Result == "1" ? "1" : "2";
                                responseDTO.SuggestedReceivedQty = oPutawayresponse.SuggestedReceivedQty.ToString();
                                responseDTO.SuggestedQty = oPutawayresponse.SuggestedQty.ToString();
                                responseDTO.PutAwayQty = oPutawayresponse.PutAwayQty.ToString();
                                responseDTO.CartonCode = "";
                                responseDTO.MCode = "";
                                responseDTO.MfgDate = oPutawayresponse.MfgDate.ToString();
                                responseDTO.ExpDate = oPutawayresponse.ExpDate.ToString();
                                responseDTO.SerialNo = oPutawayresponse.SerialNo.ToString();
                                responseDTO.BatchNo = oPutawayresponse.BatchNo.ToString();
                                responseDTO.ProjectRefNo = oPutawayresponse.ProjectRefNo.ToString();
                                responseDTO.MRP = oPutawayresponse.MRP.ToString();
                                responseDTO.Location = oPutawayresponse.Location.ToString();
                            }
                            else
                            {
                                throw new WMSExceptionMessage() { WMSExceptionCode = oPutawayresponse.Result, WMSMessage = oPutawayresponse.Result, ShowAsError = true };
                            }
                        }
                        _lstPutaway.Add(responseDTO);
                    }


                    string json = JsonConvert.SerializeObject(WMSCoreEndpointSecurity.PrepareResponse(oRequest.AuthToken, Constants.EndpointConstants.DTO.PutAwayDTO, _lstPutaway));
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

        //new requirement for MSP's list
        [HttpPost]
        [Route("GetMSPsReceiving")]
        public string GetMSPsReceiving(WMSCoreMessage oRequest)
        {
            try
            {

                LoadControllerDefaults(oRequest);
                MRLWMSC21Service.UserDataTable userDataTable = new MRLWMSC21Service.UserDataTable();
                // PutAwayDTO responseDTO = new PutAwayDTO();
                List<PutAwayDTO> _lstPutaway = new List<PutAwayDTO>();

                if (oRequest != null)
                {

                    PutAwayDTO _oPutawayDTO = (PutAwayDTO)WMSCoreEndpointSecurity.ValidateRequest(oRequest);

                    if (_oPutawayDTO != null)
                    {
                        MRLWMSC21Service.PutAway oPutaway = new MRLWMSC21Service.PutAway()
                        {
                            InboundId = _oPutawayDTO.InboundId,
                            MCode = _oPutawayDTO.MCode,

                        };
                        MRLWMSC21Service.MRLWMSC21HHTWCFServiceClient oFalcon = new MRLWMSC21Service.MRLWMSC21HHTWCFServiceClient();
                        userDataTable = oFalcon.GetMSPsReceiving(oPutaway);
                        if (userDataTable != null)
                        {
                            foreach (DataRow dr in userDataTable.Table.Rows)
                            {
                                PutAwayDTO putAwayDTO = new PutAwayDTO();
                                putAwayDTO.BatchNo = dr["BatchNo"].ToString();
                                putAwayDTO.SerialNo = dr["SerialNo"].ToString();
                                putAwayDTO.ProjectRefNo = dr["ProjectRefNo"].ToString();
                                putAwayDTO.MfgDate = dr["MfgDate"].ToString();
                                putAwayDTO.PutAwayQty = dr["InvoiceQuantity"].ToString();
                                _lstPutaway.Add(putAwayDTO);
                            }

                        }



                    }
                    string json = JsonConvert.SerializeObject(WMSCoreEndpointSecurity.PrepareResponse(oRequest.AuthToken, Constants.EndpointConstants.DTO.PutAwayDTO, _lstPutaway));
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
        [Route("CheckDockGoodsIn")]
        public string CheckDockGoodsIn(WMSCoreMessage oRequest)
        {
            try
            {

                LoadControllerDefaults(oRequest);              
                InboundDTO responseDTO = new InboundDTO();
                List<InboundDTO> _lstinbound = new List<InboundDTO>();          
                if (oRequest != null)
                {

                    InboundDTO _oInboundDTO = (InboundDTO)WMSCoreEndpointSecurity.ValidateRequest(oRequest);

                    if (_oInboundDTO != null)
                    {
             
                        MRLWMSC21Service.Inbound oInbound = new MRLWMSC21Service.Inbound()
                        {

                            InboundId = _oInboundDTO.InboundID,
                            Dock=_oInboundDTO.Dock
                        };
                        MRLWMSC21Service.MRLWMSC21HHTWCFServiceClient oFalcon = new MRLWMSC21Service.MRLWMSC21HHTWCFServiceClient();
                        MRLWMSC21Service.Inbound resInbound = new MRLWMSC21Service.Inbound();
                        resInbound=oFalcon.CheckDockInbound(oInbound);
                        if (resInbound.Result == "0")
                        {
                            responseDTO.Result = "Please scan valid Dock";
                            throw new WMSExceptionMessage() { WMSExceptionCode = responseDTO.Result, WMSMessage = responseDTO.Result, ShowAsError = true };
                        }
                        else
                        {
                            responseDTO.Result = "1";
                        }
                       

                        _lstinbound.Add(responseDTO);
                    }


                    string json = JsonConvert.SerializeObject(WMSCoreEndpointSecurity.PrepareResponse(oRequest.AuthToken, Constants.EndpointConstants.DTO.Inbound, _lstinbound));
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

